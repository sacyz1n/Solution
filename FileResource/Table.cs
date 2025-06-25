namespace FileResource
{
    public class Table<T> where T : class, IRecordWithIntegerKey
    {
        private Dictionary<int, T> _records = new();

        public bool Add(int key, T record)
        {
            if (_records.ContainsKey(key))
            {
                return false; // Key already exists
            }
            _records[key] = record;
            return true;
        }

        public bool Remove(int key)
            => _records.Remove(key);

        public bool TryGetValue(int key, out T record)
        {
            if (_records.TryGetValue(key, out record))
                return true; 

            record = null; 
            return false;
        }

        public bool ContainsKey(int key)
            => _records.ContainsKey(key);

        public IEnumerable<T> GetAllRecords()
            => _records.Values;
    }
}
