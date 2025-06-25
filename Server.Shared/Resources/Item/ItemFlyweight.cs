using System.Text.Json.Serialization;

namespace Server.Shared.Resources.Item
{
    public partial class ItemFlyweight : FileResource.IRecordWithIntegerKey, FileResource.Loader.IDataProcessing
    {
        public class ItemList : FileResource.IRecordWithIntegerKey
        {
            public int GetKey() => ItemIndex;

            [JsonInclude] public int ItemIndex { get; set; } = 0;
        }

        public int GetKey() => ItemIndex;

        [JsonInclude] public int ItemIndex { get; set; } = 0;
        [JsonInclude] public string ItemName { get; set; } = string.Empty;

        [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
        public abstract class ItemDataRecord
        {
            //public virtual E_ItemType 
        }

        public void PostProcess()
        {
            // Post-processing logic if needed
        }
        public void PreProcess()
        {
            // Pre-processing logic if needed
        }
    }
}
