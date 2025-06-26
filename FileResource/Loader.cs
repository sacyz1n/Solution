using Log;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata.Ecma335;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using ZLogger;

namespace FileResource
{
    public static class Loader
    {
        private class ResourcesProvider
        {
            private string _baseFilePath = string.Empty;

            public ResourcesProvider(string baseFilePath)
            {
                if (string.IsNullOrWhiteSpace(baseFilePath))
                    throw new ArgumentException($"Base file path cannot be null or empty. BaseFilePath:{baseFilePath}");

                _baseFilePath = baseFilePath;
            }

            internal string GetResourcePath(string resourcePath)
                => Path.Combine(_baseFilePath, resourcePath);
        }


        /// <summary>
        /// 테이블 로드 이후 유효성 검증 및 후처리 작업을 위한 인터페이스입니다.
        /// </summary>
        public interface IDataProcessing
        {
            void PreProcess();
            void PostProcess();
        }

        private static ILogger _logger;

        private static ResourcesProvider _resourcesProvider = null;

        private static Action<Exception> _exceptionHandler = (ex) =>
            {
                DEV.CHECK(false, $"[FileResource.Loader] Exception occurred during data processing. Message:{ex?.Message}");
            };

        private static List<IDataProcessing> _dataProcessingList = new();

        private static JsonSerializerOptions _jsonSerializerOptions = new()
        {
            IgnoreReadOnlyFields = false,
            IgnoreReadOnlyProperties = false,
            AllowTrailingCommas = true,
            IncludeFields = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            //TypeInfoResolver = AutoPolymorphicTypeResolver
        };

        public static void Initialize(string baseFilePath, ILogger logger)
        {
            _resourcesProvider = new(baseFilePath);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private static void handleException(Exception exception)
            => _exceptionHandler?.Invoke(exception);

        public static void ProcessAll()
        {
            foreach (var data in _dataProcessingList)
                data.PreProcess();

            foreach (var data in _dataProcessingList)
                data.PostProcess();
        }

        /// <summary>
        /// 단일 레코드를 로드하는 메서드입니다.
        /// </summary>
        public static bool LoadSingleTable<T>(string filePath) where T : class, IDataProcessing, IRecordSingle, new()
        {
            var fullPath = _resourcesProvider?.GetResourcePath(filePath);
            if (string.IsNullOrWhiteSpace(fullPath))
                throw new ArgumentException($"[LoadSingleTable] File path cannot be null or empty. FilePath:{fullPath}");

            try
            {
                using (var reader = new StreamReader(fullPath))
                {
                    var obj = JsonSerializer.Deserialize<T>(reader.BaseStream, _jsonSerializerOptions);
                    if (obj == null)
                    {
                        handleException(new Exception("[LoadSingleTable] JsonSerializer.Deserialize is null"));
                        return false;
                    }

                    Storage.SaveRecord(obj);
                    _dataProcessingList.Add(obj);
                    _logger.LogInformation($"Load ServerData - {typeof(T).Name}");
                    return true;
                }
            }
            catch (Exception e)
            {
                handleException(e);
                return false;
            }
        }

        public static bool LoadArrayTable<T>(string filePath) where T : class, IDataProcessing, IRecordSingle, new()
        {
            var fullPath = _resourcesProvider?.GetResourcePath(filePath);
            if (string.IsNullOrWhiteSpace(fullPath))
                throw new ArgumentException($"[LoadArrayTable] File path cannot be null or empty. FilePath:{fullPath}");
            try
            {
                using (var reader = new StreamReader(fullPath))
                {
                    var obj = JsonSerializer.Deserialize<List<T>>(reader.BaseStream, _jsonSerializerOptions);
                    //if (obj == null)
                    //{
                    //    LastException = new Exception("[LoadFromOneJson] JsonSerializer.Deserialize is null");
                    //    errorHandler?.Invoke(LastException);
                    //    return null;
                    //}
                    //Storage.SaveToTables(obj, name);
                    return true;
                }
            }
            catch (Exception e)
            {
                //LastException = e;
                //errorHandler?.Invoke(e);
                return false;
            }
        }

        public static bool LoadListTable<TList, TData>(string listFilePath, string dataFilePath) where TData : class, IDataProcessing, IRecordWithIntegerKey, new()
        {
            var listFullPath = _resourcesProvider?.GetResourcePath(listFilePath);
            if (string.IsNullOrWhiteSpace(listFullPath))
                throw new ArgumentException($"[LoadListTable] File path cannot be null or empty. FilePath:{listFullPath}");
            try
            {
                using (var reader = new StreamReader(listFullPath))
                {
                    //var obj = JsonSerializer.Deserialize<List<T>>(reader.BaseStream, _jsonSerializerOptions);
                    //if (obj == null)
                    //{
                    //    LastException = new Exception("[LoadListTable] JsonSerializer.Deserialize is null");
                    //    errorHandler?.Invoke(LastException);
                    //    return null;
                    //}
                    //Storage.SaveToTables(obj, name);
                    return true;
                }
            }
            catch (Exception e)
            {
                //LastException = e;
                //errorHandler?.Invoke(e);
                return false;
            }
        }
    }
}
