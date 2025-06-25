using System.Text.Json.Serialization;

namespace Server.Shared.Resources.Config
{
    public class GameConfig : FileResource.IRecordSingle, FileResource.Loader.IDataProcessing
    {
        //[JsonInclude] public int 

        public void PostProcess()
        {
        }

        public void PreProcess()
        {
        }
    }
}
