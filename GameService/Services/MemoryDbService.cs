namespace GameService.Services
{
    public interface IMemoryDbService
    {
        // Define methods for memory database operations here
    }

    public class MemoryDbService : IMemoryDbService
    {
        private readonly ILogger<MemoryDbService> _logger;

        private CloudStructures.RedisConnection _redisConnection;

        public MemoryDbService(ILogger<MemoryDbService> logger) 
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");

        }

        public void Initialize(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Redis connection string is not set in the configuration.");

            this._redisConnection = new CloudStructures.RedisConnection(
                new CloudStructures.RedisConfig(null, connectionString)
                );

            _logger.LogInformation("MemoryDbService initialized successfully.");
        }
    }
}
