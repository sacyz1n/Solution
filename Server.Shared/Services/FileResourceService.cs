using Microsoft.Extensions.Logging;
using Server.Shared.Resources.Config;
using Server.Shared.Resources.Item;

namespace Server.Shared.Services
{
    public interface IFileResourceService
    {
        ValueTask LoadTableData(string baseFilePath);
    }

    public class FileResourceService : IFileResourceService
    {
        private readonly ILogger<FileResourceService> _logger;
        public FileResourceService(ILogger<FileResourceService> logger)
        {
            this._logger = logger;
        }

        public ValueTask LoadTableData(string baseFilePath)
        {
            FileResource.Loader.Initialize(baseFilePath, _logger);

            //FileResource.Loader.LoadSingleTable<GameConfig>("./Resources/Config/GameConfig.json");
            FileResource.Loader.LoadSingleTable<CharacterConfig>($"Config/CharacterConfig.json");
            //FileResource.Loader.LoadListTable<ItemFlyweight.ItemList, ItemFlyweight>("./Resources/Item/ItemList.json", "./Resources/Item/Item_{0}.json");
            _logger.LogInformation("Load ServerData - Completed.");

            FileResource.Loader.ProcessAll();
            _logger.LogInformation("Load ServerData - Process Completed.");

            return ValueTask.CompletedTask;
        }
    }
}
