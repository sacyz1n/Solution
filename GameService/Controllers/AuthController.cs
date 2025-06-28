using Client.Shared;
using GameService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameService.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISessionService _sessionService;
        private readonly IGameDbService _gameDbService;

        public AuthController(
            ILogger<AuthController> logger, 
            IConfiguration configuration, 
            ISessionService sessionService,
            IGameDbService gameDbService)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._sessionService = sessionService;
            this._gameDbService = gameDbService;
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
        [Route("/login")]
        public async Task<LoginResponse> Login([FromBody] LoginRequest input)
        {
            var output = new LoginResponse();
            _logger.LogInformation("Login request received");

            await _gameDbService.GetAccountInfo(input.TestValue01);

            return output;
        }
    }
}
