namespace GameService.Services
{
    public interface IDbAccessService
    {
        // Define methods for database access here
    }

    public class DbAccessService : IDbAccessService
    {
        private readonly ILogger<DbAccessService> _logger;
        private readonly IConfiguration _configuration;
        public DbAccessService(ILogger<DbAccessService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
    }
}
