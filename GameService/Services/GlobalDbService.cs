namespace GameService.Services
{
    public interface IGlobalDbService
    {
    }

    public class GlobalDbService<T> 
    {
        private ILogger<T> _logger;

        public GlobalDbService(ILogger<T> logger, IConfiguration configuration, IDbAccessService dbAccessService, ISessionService sessionService, IGameDbService gameDbService)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            _logger = logger;
        }
    }
}
