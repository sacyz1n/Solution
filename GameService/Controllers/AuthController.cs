using Client.Shared;
using GameService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Shared.Models.Auth;

namespace GameService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMemoryDbService _memoryDbService;
        private readonly IGameDbService _gameDbService;
        private readonly IGlobalDbService _globalDbService;

        public AuthController(
            ILogger<AuthController> logger,
            IConfiguration configuration,
            IMemoryDbService memoryDbService,
            IGameDbService gameDbService,
            IGlobalDbService globalDbService)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._memoryDbService = memoryDbService;
            this._gameDbService = gameDbService;
            this._globalDbService = globalDbService;
        }

        /// <summary>
        /// health check용 API
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("/")]
        public IActionResult Index()
            => Ok(new { Info = $"GameService is running. {DateTime.Now}" });


        [AllowAnonymous]
        [HttpPost]
        [Route("/Login")]
        public async Task<LoginResponse> Login([FromBody] LoginRequest input)
        {
            var output = new LoginResponse();
            _logger.LogInformation("Login request received");

            if (string.IsNullOrEmpty(input.Token) == true)
            {
                return output.SetErrorCode(ErrorCodes.INVALID_PARAM);
            }

            if (string.IsNullOrEmpty(input.MemberId) == true)
            {
                return output.SetErrorCode(ErrorCodes.INVALID_PARAM);
            }

            if (Enum.IsDefined(input.PlatformType) == false)
            {
                return output.SetErrorCode(ErrorCodes.INVALID_PARAM);
            }

            long accountNo = 0;
            var accountInfo = await _globalDbService.GetAccountInfo(input.MemberId);
            // 생성된 계정이 없다면
            if (accountInfo == null)
            {
                // 토큰 검증
                var verifyResult = await verifyToken(input.PlatformType, input.Token, input.MemberId);
                if (verifyResult != ErrorCodes.SUCCESS)
                {
                    _logger.LogError($"Token verification failed for MemberId: {input.MemberId}, ErrorCode: {verifyResult}");
                    return output.SetErrorCode(verifyResult);
                }

                var newAccount = new Repository.GlobalDB.AccountInfo()
                {
                    MemberId = input.MemberId,
                    PlatformType = (byte)input.PlatformType,

                    // TODO: 특정 규칙을 적용해서 db 샤딩 필요
                    GameDBIndex = 1,
                    GameLogDBIndex = 1,
                    CreateTime = DateTime.UtcNow,
                    LoginTime = DateTime.UtcNow
                };

                var insertResult = await _globalDbService.InsertAccountInfo(newAccount);
                if (insertResult == 0)
                {
                    _logger.LogError($"Failed to create new account for MemberId: {input.MemberId}");
                    return output.SetErrorCode(ErrorCodes.LOGIN_ERROR);
                }

                _logger.LogInformation($"New account created for MemberId: {input.MemberId}, AccountNo: {insertResult}");
                accountNo = insertResult;
            }
            else
            {
                var (isAuthorized, authInfo) = await _memoryDbService.IsAuthorizedUser(accountNo);

                // 이미 로그인 처리되어 있는 경우
                if (isAuthorized == ErrorCodes.SUCCESS)
                {
                    return output.SetErrorCode(ErrorCodes.LOGIN_ALREADY);
                }

                // 토큰 검증
                var verifyResult = await verifyToken((E_PlatformType)accountInfo.PlatformType, input.Token, accountInfo.MemberId);
                if (verifyResult != ErrorCodes.SUCCESS)
                {
                    _logger.LogError($"Token verification failed for MemberId: {input.MemberId}, ErrorCode: {verifyResult}");
                    return output.SetErrorCode(verifyResult);
                }

                accountNo = accountInfo.AccountNo;
            }

            // 인증된 유저로 레디스에 등록한다.
            var token = Utility.Security.CreateAuthToken();
            var registResult = await _memoryDbService.RegistAuthorizedUser(accountNo,
                new AuthorizedUser()
                {
                    AccountNo = accountNo,
                    AuthToken = token
                });

            if (registResult != ErrorCodes.SUCCESS)
            {
                _logger.LogError($"Failed to register authorized user for AccountNo: {accountNo}, ErrorCode: {registResult}");
                return output.SetErrorCode(registResult);
            }

            output.AccountNo = accountNo;
            output.MemberId = input.MemberId;
            output.AuthToken = token;
            return output;
        }

        private static async ValueTask<int> verifyToken(E_PlatformType platformType, string token, string memberId)
        {
            switch (platformType)
            {
                case E_PlatformType.GOOGLE:
                case E_PlatformType.APPLE:
                    return await Logic.Login.FirebaseFacade.VerifyToken(token, memberId);

                case E_PlatformType.DEV:
                    return ErrorCodes.SUCCESS;

                default:
                    return ErrorCodes.INVALID_PLATFORM_TYPE;
            }
        }
    }
}
