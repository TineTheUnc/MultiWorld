using Terraria.ModLoader.Config;

namespace MultiWorld.Common.Config
{
	public class Beta: ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		public bool SepecialWorld { get; set; } = false;

	}
}
