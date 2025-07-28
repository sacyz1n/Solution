namespace Repository.Contexts
{
    public enum E_DBShardType : byte
    {
        GameDB = 1,
        GameLogDB = 2,
    }

    public static class Constants
    {
        public const string GameDB = "gamedb_{0}";

        public const string GameLogDB = "gamelogdb_{0}";

        public const string GlobalDB = "globaldb";

        public const string GlobalLogDB = "globallogdb";

        public static string GetGameDBShard(int shardIndex)
            => string.Format(GameDB, shardIndex);

        public static string GetGameLogDBShard(int shardIndex)
            => string.Format(GameLogDB, shardIndex);


    }
}
