using Microsoft.Xna.Framework.Graphics;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;

namespace MultiWorld.Common.Config
{
	public class Beta: ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[OptionStrings(["Normal", "Sepecial", "Random Mod"])]
		[DefaultValue("Normal")]
		public string GenMode { get; set; }

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

		[Header("HardModeChance")]
		[DefaultValue(1)]
		[Range(1, 10)]
		public int EvilHardModeChance { get; set; }
		[DefaultValue(1)]
		[Range(1, 10)]
		public int HallowHardModeChance { get; set; }
		[DefaultValue(9)]
		[Range(1, 10)]
		public int NoneGenChance { get; set; }


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

		[Header("Action")]
		[SeparatePage]
		public OpenWorldSettingButton OpenWorldSettingButton { get; set; } = new();
	}


	public class OpenWorldSettingButton()
	{
		[CustomModConfigItem(typeof(OpenWorldSettingProcess))]
		public int Button;
	}
	public class OpenWorldSettingProcess : FloatElement
	{
		public override void Draw(SpriteBatch spriteBatch)
		{
			ProcessStartInfo startInfo = new()
			{
				Arguments = MultiWorld.WorldSetting,
				FileName = "explorer.exe"
			};
			SoundEngine.PlaySound(SoundID.MenuOpen);
			Process.Start(startInfo);
			Main.MenuUI.GoBack();
		}
	}
}
