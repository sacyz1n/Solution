using Client.Shared;
using GameService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly ILogger<MailController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IGameDbService _gameDbService;
        private readonly IGlobalDbService _globalDbService;

        public MailController(
            ILogger<MailController> logger,
            IConfiguration configuration,
            IGameDbService gameDbService,
            IGlobalDbService globalDbService)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._gameDbService = gameDbService;
            this._globalDbService = globalDbService;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("/MailList")]
        public async Task<MailListResponse> MailList([FromBody] MailListRequest input)
        {
            var output = new MailListResponse();
            _logger.LogInformation("MailList request received");

            //var accountInfo = await _globalDbService.GetAccountInfo(input.MemberId);

            //// 새 계정 생성
            //if (accountInfo == null)
            //{
            //    var newAccount = new Repository.GlobalDB.AccountInfo()
            //    {
            //        MemberId = input.MemberId,
            //        PlatformType = (byte)input.PlatformType,
            //        CreateTime = DateTime.UtcNow,
            //        LoginTime = DateTime.UtcNow
            //    };

            //    var insertResult = await _globalDbService.InsertAccountInfo(newAccount);
            //    if (insertResult == 0)
            //    {
            //        _logger.LogError($"Failed to create new account for MemberId: {input.MemberId}");
            //        return output.SetErrorCode(ErrorCodes.LOGIN_ERROR);
            //    }

            //    _logger.LogInformation($"New account created for MemberId: {input.MemberId}, AccountNo: {insertResult}");
            //    output.AccountNo = insertResult;
            //}
            //else
            //{
            //    output.AccountNo = accountInfo.AccountNo;
            //}


            //output.MemberId = input.MemberId;
            return output;
        }
    }
}
