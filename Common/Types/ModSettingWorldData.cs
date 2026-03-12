using Newtonsoft.Json;
using System.Collections.Generic;

namespace MultiWorld.Common.Types
{
    [JsonObject(MemberSerialization.Fields)]
    public class ModSettingWorldData
    {
        public List<string> Global = [];
        public Dictionary<string, List<string>> Group = [];
    }
}
