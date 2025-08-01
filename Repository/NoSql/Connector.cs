using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.NoSql
{
    public interface IConnector
    {
        public CloudStructures.RedisConnection Connection { get; }

        public void Initialize(string connectionString);
    }

    public class Connector : IConnector
    {
        private readonly ILogger<Connector> _logger;
        public Connector(ILogger<Connector> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
        }

        private CloudStructures.RedisConnection _redisConnection;

        public CloudStructures.RedisConnection Connection
        {
            get
            {
                if (_redisConnection == null)
                    throw new InvalidOperationException("Redis connection is not initialized. Call Initialize() first.");

                return _redisConnection;
            }
        }

        public void Initialize(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Redis connection string is not set in the configuration.");

            this._redisConnection = new CloudStructures.RedisConnection(
                new CloudStructures.RedisConfig(null, connectionString)
                );

            //var connection = this._redisConnection.GetConnection();
            //connection.ConnectionRestored += onConnectionRestored;
            _logger.LogInformation("Connector initialized successfully.");
        }

        private void onConnectionRestored(object sender, EventArgs e)
        {
            _logger.LogInformation("Redis connection restored successfully.");
        }
    }
}
