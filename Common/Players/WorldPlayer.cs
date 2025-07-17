using MultiWorld.Common.Systems;
using MultiWorld.Common.Types;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace MultiWorld.Common.Players
{
	public class WorldPlayer: ModPlayer
	{
		public override void OnRespawn()
		{
			base.OnRespawn();
			if (MultiWorldFileData.IsMultiWorld(Main.ActiveWorldFileData.Path))
			{
				var System = ModContent.GetInstance<WorldManageSystem>();
				var data = MultiWorldFileData.LoadMeta(Path.Combine(Path.GetDirectoryName(Main.ActiveWorldFileData.Path), "meta.world"));
				if (data == null)
				{
					data = MultiWorldFileData.CreateMetaData();
					System.NextWorldIndex = 0;
				}
				else
				{
					var entityIdInfo = Player.GetType().GetField("entityId", BindingFlags.Instance | BindingFlags.NonPublic);
					var entityId = (long)entityIdInfo.GetValue(Player);
					System.NextWorldIndex = 0;
					if (data.spawnPoint != null) {
						if (data.spawnPoint.TryGetValue(entityId, out int value))
						{
							System.NextWorldIndex = value;
						}
					}
				}
				if (System.NextWorldIndex.ToString() == Path.GetFileNameWithoutExtension(Main.ActiveWorldFileData.Path)) {
					return;
				}
				System.CurrentWorldIndex = int.Parse(Path.GetFileNameWithoutExtension(Main.ActiveWorldFileData.Path));
				System.do_respawn = true;
				System.ChangeWorld();
			}
		}

		public override void OnEnterWorld()
		{
			base.OnEnterWorld();
			if (MultiWorldFileData.IsMultiWorld(Main.ActiveWorldFileData.Path)) {
				var System = ModContent.GetInstance<WorldManageSystem>();
				System.do_chaneWorld = false;
				System.do_worldGen = false;
				System.do_respawn = false;
				System.CurrentWorldIndex = int.Parse(Path.GetFileNameWithoutExtension(Main.ActiveWorldFileData.Path));
			}
		}
	}
}
