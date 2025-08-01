using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repository;
using Repository.Contexts;
using Repository.GlobalDB;
using SqlKata.Execution;

namespace GameService.Services
{
    public partial interface IGameDbService
    {
    }

    public partial class GameDbService : DbServiceBase, IGameDbService
    {
        private Dictionary<int /* Shard Index */, DbContextInfo<GameDbContext>> _queryFactoryDic = new();

        public int CurrentShardIndex { get; set; }

        public QueryFactory QueryFactory(int shardIndex)
        {
            if (_queryFactoryDic.TryGetValue(shardIndex, out var dbContextInfo))
            {
                return dbContextInfo.QueryFactory;
            }

            dbContextInfo = base.CreateDbContext<GameDbContext>(Constants.GetGameDBShard(shardIndex));
            if (dbContextInfo == null)
                return null!;

            _queryFactoryDic[shardIndex] = dbContextInfo;
            return dbContextInfo.QueryFactory;
        }

        public GameDbService(ILogger<GameDbService> logger) : base(Log.LogManager.LoggerFactory)
        {
        }
    }
}
