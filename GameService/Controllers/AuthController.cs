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

        public AuthController(ILogger<AuthController> logger, IConfiguration configuration, ISessionService sessionService)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._sessionService = sessionService;
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

    }
}
