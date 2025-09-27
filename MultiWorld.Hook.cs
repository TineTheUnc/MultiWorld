using MonoMod.RuntimeDetour;
using MultiWorld.Common.Config;
using MultiWorld.Common.Systems;
using MultiWorld.Common.Systems.WorldGens;
using MultiWorld.Common.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.WorldBuilding;

namespace MultiWorld
{
	public partial class MultiWorld : Mod
	{
		private List<Hook> On_ModLoader = [];
		private delegate Mod ModLoader_GetMod_orig(string name);
		private delegate bool ModLoader_TryGetMod_orig(string name, out Mod mod);
		private delegate bool ModLoader_HasMod_orig(string name);

		private void LoadHook()
		{
			On_Main.GetWorldPathFromName += On_GetWorldPathFromName;
			On_Main.LoadWorlds += On_LoadWorlds;
			On_UIWorldListItem.DeleteButtonClick += On_DeleteButtonClick;
			On_WorldFileData.OnWorldRenameSuccess += On_OnWorldRenameSuccess;
			On_Player.ChangeSpawn += On_Player_ChangeSpawn;
			On_Player.RemoveSpawn += On_Player_RemoveSpawn;
			On_WorldGen.oceanDepths += On_WorldGen_oceanDepths;
			On_WorldGen.CreateNewWorld += On_WorldGen_CreateNewWorld;
			On_ModLoader.Add(new Hook(
				typeof(ModLoader).GetMethod("GetMod", BindingFlags.Public | BindingFlags.Static),
				On_ModLoader_GetMod
			));
			On_ModLoader.Add(new Hook(
				typeof(ModLoader).GetMethod("TryGetMod", BindingFlags.Public | BindingFlags.Static),
				On_ModLoader_TryGetMod
			));
			On_ModLoader.Add(new Hook(
				typeof(ModLoader).GetMethod("HasMod", BindingFlags.Public | BindingFlags.Static),
				On_ModLoader_HasMod
			));
		}

		private void UnloadHook()
		{
			On_Main.GetWorldPathFromName -= On_GetWorldPathFromName;
			On_Main.LoadWorlds -= On_LoadWorlds;
			On_UIWorldListItem.DeleteButtonClick -= On_DeleteButtonClick;
			On_WorldFileData.OnWorldRenameSuccess -= On_OnWorldRenameSuccess;
			On_Player.ChangeSpawn -= On_Player_ChangeSpawn;
			On_Player.RemoveSpawn -= On_Player_RemoveSpawn;
			On_WorldGen.oceanDepths -= On_WorldGen_oceanDepths;
			On_WorldGen.CreateNewWorld -= On_WorldGen_CreateNewWorld;
			foreach (var hook in On_ModLoader)hook.Dispose();
			On_ModLoader.Clear();
		}

		private Mod On_ModLoader_GetMod(ModLoader_GetMod_orig orig, string name)
		{
			var worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
			if (worldManageSystem.control_ModsByName != null)
			{

				return worldManageSystem.control_ModsByName[name];
			}
			return orig(name);
		}

		private bool On_ModLoader_TryGetMod(ModLoader_TryGetMod_orig orig, string name, out Mod mod)
		{
			var worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
			if (worldManageSystem.control_ModsByName != null)
			{

				return worldManageSystem.control_ModsByName.TryGetValue(name, out mod);
			}
			return orig(name,out mod);
		}

		private bool On_ModLoader_HasMod(ModLoader_HasMod_orig orig, string name)
		{
			var worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
			if (worldManageSystem.control_ModsByName != null)
			{
				return worldManageSystem.control_ModsByName.ContainsKey(name);
			}
			return orig(name);
		}

		private Task On_WorldGen_CreateNewWorld(On_WorldGen.orig_CreateNewWorld orig, GenerationProgress progress = null)
		{
			var directory = Path.GetDirectoryName(Main.ActiveWorldFileData.Path);
			var data = MultiWorldFileData.LoadMeta(Path.Combine(directory, "meta.world"));
			if (data != null)
			{
				if (data.GenMode == "Random Mod")
				{
					var worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
					int index = WorldGen.genRand.Next(worldManageSystem.ModHookList.Count);
					worldManageSystem.RandomMod = worldManageSystem.ModHookList.ToArray()[index];
				}
			}
			return orig(progress);
		}

		private bool On_WorldGen_oceanDepths(On_WorldGen.orig_oceanDepths orig, int x, int y)
		{
			var orig_data =  orig( x,  y);
			if (OneBiome.Biome != string.Empty)
			{
				if (OneBiome.Biome == "Ocean")
				{
					return y <= WorldGen.oceanLevel;
				}
				else {
					return false;
				}
			}
			return orig_data;
		}

		private void On_Player_RemoveSpawn(On_Player.orig_RemoveSpawn orig, Player self)
		{
			orig(self);
			if (MultiWorldFileData.IsMultiWorld(Main.ActiveWorldFileData.Path))
			{
				var entityIdInfo = self.GetType().GetField("entityId", BindingFlags.Instance | BindingFlags.NonPublic);
				var entityId = (long)entityIdInfo.GetValue(self);
				var data = MultiWorldFileData.LoadMeta(Path.Combine(Path.GetDirectoryName(Main.ActiveWorldFileData.Path), "meta.world"));
				data.spawnPoint?.Remove(entityId);
				MultiWorldFileData.SaveMeta(Path.Combine(Path.GetDirectoryName(Main.ActiveWorldFileData.Path), "meta.world"), data);
			}
		}

		private void On_Player_ChangeSpawn(On_Player.orig_ChangeSpawn orig, Player self, int x, int y)
		{
			orig(self, x, y);
			if (MultiWorldFileData.IsMultiWorld(Main.ActiveWorldFileData.Path))
			{
				var entityIdInfo = self.GetType().GetField("entityId", BindingFlags.Instance | BindingFlags.NonPublic);
				var entityId = (long)entityIdInfo.GetValue(self);
				var data = MultiWorldFileData.LoadMeta(Path.Combine(Path.GetDirectoryName(Main.ActiveWorldFileData.Path), "meta.world"));
				data.spawnPoint?.Add(entityId, int.Parse(Path.GetFileNameWithoutExtension(Main.ActiveWorldFileData.Path)));
				MultiWorldFileData.SaveMeta(Path.Combine(Path.GetDirectoryName(Main.ActiveWorldFileData.Path), "meta.world"), data);
			}

		}

		private void On_OnWorldRenameSuccess(On_WorldFileData.orig_OnWorldRenameSuccess orig, WorldFileData self, string newName)
		{
			orig(self, newName);
			if (MultiWorldFileData.IsMultiWorld(self.Path))
			{
				var directory = Path.GetDirectoryName(self.Path);
				Directory.Move(directory, directory.Replace(MultiWorldFileData.GetFileName(self.Path), newName.Trim()));
			}
		}

		private void On_DeleteButtonClick(On_UIWorldListItem.orig_DeleteButtonClick orig, UIWorldListItem self, UIMouseEvent evt, UIElement listeningElement)
		{
			if (MultiWorldFileData.IsMultiWorld(self.Data.Path))
			{
				Main.WorldList.Remove(self.Data);
				Directory.Delete(Path.GetDirectoryName(self.Data.Path), true);
			}
			else
			{
				orig(self, evt, listeningElement);
			}
		}

		private string On_GetWorldPathFromName(On_Main.orig_GetWorldPathFromName orig, string worldName, bool cloudSave)
		{
			var worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
			if (worldManageSystem.CreateMultiWorld)
			{
				return orig(worldName, cloudSave).Replace(".wld", ".world" + Path.DirectorySeparatorChar + "0" + ".wld");
			}
			return orig(worldName, cloudSave);
		}

		private void On_LoadWorlds(On_Main.orig_LoadWorlds orig)
		{
			orig();
			if (!Utils.TryCreatingDirectory(Main.WorldPath))
				return;

			string[] files = Directory.GetDirectories(Main.WorldPath, "*.world");
			int num = Math.Min(files.Length, (int)typeof(Main).GetField("maxLoadWorld", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null));
			if (Main.dedServ)
			{
				for (int i = 0; i < num; i++)
				{
					try
					{
						WorldFileData allMetadata = WorldFile.GetAllMetadata(MultiWorldFileData.GetFilePath(files[i]), cloudSave: false);
						if (allMetadata != null)
							Main.WorldList.Add(allMetadata);
						else
							Main.WorldList.Add(WorldFileData.FromInvalidWorld(MultiWorldFileData.GetFilePath(files[i]), cloudSave: false));
					}
					catch
					{
						continue;
					}
				}
			}
			else
			{
				for (int j = 0; j < num; j++)
				{
					try
					{
						WorldFileData allMetadata2 = WorldFile.GetAllMetadata(MultiWorldFileData.GetFilePath(files[j]), cloudSave: false);
						if (allMetadata2 != null)
							Main.WorldList.Add(allMetadata2);
						else
							Main.WorldList.Add(WorldFileData.FromInvalidWorld(MultiWorldFileData.GetFilePath(files[j]), cloudSave: false));
					}
					catch
					{
						continue;
					}
				}
			}

			Main.WorldList.Sort(WorldListSortMethod);
		}

	}
}
