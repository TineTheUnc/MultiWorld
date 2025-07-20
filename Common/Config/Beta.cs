using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace MultiWorld.Common.Config
{
	public class Beta: ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		public bool SepecialWorld { get; set; } = false;

		[Header("BiomeChance")]
		[DefaultValue(1)]
		[Range(1, 10)]
		public int ForestChance { get; set; }
		[DefaultValue(1)]
		[Range(1, 10)]
		public int DesertChance { get; set; }
		[DefaultValue(1)]
		[Range(1, 10)]
		public int JungleChance { get; set; }
		[DefaultValue(1)]
		[Range(1, 10)]
		public int SnowChance { get; set; }
		[DefaultValue(1)]
		[Range(1, 10)]
		public int OceanChance { get; set; }
		[DefaultValue(1)]
		[Range(1, 10)]
		public int EvilChance { get; set; }

		[Header("StructureChance")]
		[DefaultValue(9)]
		[Range(1, 10)]
		public int DungeonChance { get; set; }
		[DefaultValue(9)]
		[Range(1, 10)]
		public int TempleChance { get; set; }
		[DefaultValue(9)]
		[Range(1, 10)]
		public int ShimmerChance { get; set; }
	}
}
