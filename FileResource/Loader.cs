using Log;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace FileResource
{
    public static class Loader
    {
        /// <summary>
        /// 테이블 로드 이후 유효성 검증 및 후처리 작업을 위한 인터페이스입니다.
        /// </summary>
        public interface IDataProcessing
        {
            void PreProcess();
            void PostProcess();
        }

        private static Action<Exception> _exceptionHandler = (ex) =>
            {
                DEV.CHECK(false, $"[FileResource.Loader] Exception occurred during data processing. Message:{ex?.Message}");
            };

        private static List<IDataProcessing> _dataProcessingList = new();

        private static JsonSerializerOptions _jsonSerializerOptions = new()
        {
            
        };

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
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException($"[LoadSingleTable] File path cannot be null or empty. FilePath:{filePath}");

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    var obj = JsonSerializer.Deserialize<T>(reader.BaseStream, _jsonSerializerOptions);
                    if (obj == null)
                    {
                        handleException(new Exception("[LoadSingleTable] JsonSerializer.Deserialize is null"));
                        return false;
                    }

                    Storage.SaveRecord(obj);
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
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException($"[LoadArrayTable] File path cannot be null or empty. FilePath:{filePath}");
            try
            {
                using (var reader = new StreamReader(filePath))
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
            if (string.IsNullOrWhiteSpace(listFilePath))
                throw new ArgumentException($"[LoadListTable] File path cannot be null or empty. FilePath:{listFilePath}");
            try
            {
                using (var reader = new StreamReader(listFilePath))
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
