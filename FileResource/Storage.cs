namespace FileResource
{
    public static class Storage
    {
        private class IntegerKeyTableHolder<T>
            where T : class, IRecordWithIntegerKey
        {
            internal static Table<T> Table { get; } = new();
        }

        private class  SingleKeyTableHolder<T>
            where T : class, IRecordSingle
        {
            internal static T Table { get; set; } = null;
        }

        public static T Get<T>()
            where T : class, IRecordSingle
        {
            if (SingleKeyTableHolder<T>.Table == null)
                return null; // No record exists

            return SingleKeyTableHolder<T>.Table;
        }

        public static T Get<T>(int key)
            where T : class, IRecordWithIntegerKey
        {
            if (!IntegerKeyTableHolder<T>.Table.TryGetValue(key, out var record))
                return null;

            return record;
        }

        public static bool SaveRecord<T>(int key, T record)
            where T : class, IRecordWithIntegerKey, new()
        {
            if (IntegerKeyTableHolder<T>.Table.ContainsKey(key))
                return false; // Key already exists

            if (!IntegerKeyTableHolder<T>.Table.Add(key, record))
                return false;

            return true;
        }

        public static bool SaveRecord<T>(T record)
            where T : class, IRecordSingle, new()
        {
            if (SingleKeyTableHolder<T>.Table != null)
                return false; // Single record already exists

            SingleKeyTableHolder<T>.Table = record;
            return true;
        }

    }
}
