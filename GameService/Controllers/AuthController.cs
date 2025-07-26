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

            var accountInfo = await _globalDbService.GetAccountInfo(input.MemberId);

            // 새 계정 생성
            long accountNo = 0;
            if (accountInfo == null)
            {
                var newAccount = new Repository.GlobalDB.AccountInfo()
                {
                    MemberId = input.MemberId,
                    PlatformType = (byte)input.PlatformType,
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
    }
}
