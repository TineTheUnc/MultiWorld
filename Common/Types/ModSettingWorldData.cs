using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiWorld.Common.Types
{
	[JsonObject(MemberSerialization.Fields)]
	public class ModSettingWorldData
	{
		public List<string> Global = [];
		public Dictionary<string, List<string>> Group = [];
	}
}
