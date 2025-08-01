using CloudStructures.Structures;
using Microsoft.Extensions.Logging;
using Repository.NoSql;

namespace Server.Shared.Services
{
    public interface IDynamicTableService
    {
        ValueTask LoadAllTables();
    }

    public class DynamicTableService : IDynamicTableService
    {
        private readonly ILogger<DynamicTableService> _logger;

        private readonly IConnector _connector;

        public DynamicTableService(ILogger<DynamicTableService> logger, IConnector connector)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
            _connector = connector ?? throw new ArgumentNullException(nameof(connector), "Connector cannot be null.");
        }

        public async ValueTask LoadAllTables()
        {
            var connection  = _connector.Connection;
            if (connection == null)
            {
                _logger.LogError("DynamicTableService: Connection is null.");
                throw new InvalidOperationException("Connection cannot be null.");
            }

            {
                var key = DynamicTables.DBShardInfo.GetTableKey();
                var handle = new RedisString<string>(_connector.Connection, key, null);
                var value = await handle.GetAsync();
                var table = (value.HasValue == true ? DynamicTables.DBShardInfo.Load(value.Value) : null) ?? new();
                FileResource.Storage.SaveRecord<DynamicTables.DBShardInfo>(table);
            }
        }
    }
}
