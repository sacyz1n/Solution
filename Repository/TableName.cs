using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Repository
{
    internal class NameHolder<T> where T : class
    {
        public string Name { get; private set; } = string.Empty;
        public NameHolder(string schemaName = "")
        {
            if (string.IsNullOrEmpty(schemaName))
                Name = typeof(T).GetCustomAttribute<TableAttribute>()?.Name;
            else
                Name = $"{schemaName}.{typeof(T).GetCustomAttribute<TableAttribute>()?.Name}";
        }
    }


    public class TableName<T> where T : class
    {
        private static NameHolder<T> s_NameHolder = new NameHolder<T>();
        public static string Get()
            => s_NameHolder.Name;
    }

    public static class GlobalDB<T> where T : class
    {
        private static NameHolder<T> s_globalTableName = new(Repository.Contexts.Constants.GlobalDB);

        public static string Get() => s_globalTableName.Name;
    }

    public static class GameDB<T> where T : class
    {
        private static NameHolder<T> s_gameTableName = new(Repository.Contexts.Constants.GameDB);

        public static string Get() => s_gameTableName.Name;
    }

}
