using Log;
using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Server.Shared.Resources.Config
{
    /// <summary>
    /// 캐릭터 레벨 성장 정보
    /// </summary>
    public class CharacterConfig : FileResource.IRecordSingle, FileResource.Loader.IDataProcessing
    {
        [JsonInclude] public LevelInfo LevelInfos { get; set; } = null;

        [JsonIgnore] public FrozenDictionary<int, LevelInfo> _levelInfoDic = null;

        public class LevelInfo
        {
            [JsonInclude] public int Level { get; set; }

            [JsonIgnore] public int Exp { get; set; }
        }

        public void PostProcess()
        {
        }

        public void PreProcess()
        {
            DEV.CHECK(LevelInfos is not null, $"CharacterConfig - LevelInfos is null.");
        }
    }
}
