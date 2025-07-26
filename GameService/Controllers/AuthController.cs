using Client.Shared;
using GameService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IGameDbService _gameDbService;
        private readonly IGlobalDbService _globalDbService;

        public AuthController(
            ILogger<AuthController> logger, 
            IConfiguration configuration, 
            IGameDbService gameDbService,
            IGlobalDbService globalDbService)
        {
            this._logger = logger;
            this._configuration = configuration;
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
                output.AccountNo = insertResult;
            }
            else
            {
                output.AccountNo = accountInfo.AccountNo;
            }


            output.MemberId = input.MemberId;
            return output;
        }
    }
}
