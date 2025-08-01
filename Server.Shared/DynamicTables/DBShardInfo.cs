using Microsoft.Extensions.Logging;
using Repository.Contexts;
using ShareLib;
using System.Collections.Frozen;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Server.Shared.DynamicTables
{
    public class DBShardInfo : FileResource.IRecordSingle
    {
        [JsonInclude] public Repository.GlobalDB.DBShardInfo[] DBShardInfos { get; set; } = Array.Empty<Repository.GlobalDB.DBShardInfo>();

        [JsonIgnore] private Dictionary<E_DBShardType, List<ProbItem>> _probItemByShardType = new();

        public static string GetTableKey()
            => $"{nameof(DBShardInfo)}";

        public string Serialize()
            => JsonSerializer.Serialize(this, FileResource.Loader.DefaultOptions);

        public static DBShardInfo Load(string value)
        {
            var record = FileResource.Storage.Get<DBShardInfo>();
            if (record != null)
                return record;

            record = JsonSerializer.Deserialize<DBShardInfo>(value, FileResource.Loader.DefaultOptions);
            if (record == null)
                return null;

            if (record.process() == false)
            {
                Log.LogManager.Logger.LogError("Failed to process DBShardInfo.");
                return null;
            }
            return record;
        }

        public int GenerateDBIndex(E_DBShardType shardDBType)
        {
            if (_probItemByShardType.TryGetValue(shardDBType, out var probList) == false)
            {
                return -1;
            }

            var selectedIndex = probList.GetSelectedDataIndex();
            return probList[selectedIndex].Index;
        }

        private bool process()
        {
            return true;
        }
    }
}
