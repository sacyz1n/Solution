using Microsoft.Extensions.Logging;
using Server.Shared.Resources.Config;
using Server.Shared.Resources.Item;

namespace Server.Shared.Services
{
    public interface IFileResourceService
    {
        ValueTask LoadTableData();
    }

    public class FileResourceService : IFileResourceService
    {
        private readonly ILogger<FileResourceService> _logger;
        public FileResourceService(ILogger<FileResourceService> logger)
        {
            this._logger = logger;
        }

        public ValueTask LoadTableData()
        {
            //FileResource.Loader.LoadSingleTable<GameConfig>("./Resources/Config/GameConfig.json");
            FileResource.Loader.LoadSingleTable<CharacterConfig>("./Resources/Config/CharacterConfig.json");
            //FileResource.Loader.LoadListTable<ItemFlyweight.ItemList, ItemFlyweight>("./Resources/Item/ItemList.json", "./Resources/Item/Item_{0}.json");
            _logger.LogInformation("FileResourceService - LoadTableData completed.");

            FileResource.Loader.ProcessAll();
            _logger.LogInformation("FileResourceService - ProcessAll completed.");

            return ValueTask.CompletedTask;
        }
    }
}
