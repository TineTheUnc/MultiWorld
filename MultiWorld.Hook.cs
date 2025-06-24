using MultiWorld.Common.Systems;
using MultiWorld.Common.Type;
using System;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.UI;

namespace MultiWorld
{
	public partial class MultiWorld : Mod
	{
		private void LoadHook()
		{
			On_Main.GetWorldPathFromName += On_GetWorldPathFromName;
			On_WorldGen.do_worldGenCallBack += On_do_worldGenCallBack;
			On_Main.LoadWorlds += On_LoadWorlds;
			On_UIWorldCreation.FinishCreatingWorld += On_FinishCreatingWorld;
			On_UIWorldListItem.DeleteButtonClick += On_DeleteButtonClick;
			On_WorldFileData.OnWorldRenameSuccess += On_OnWorldRenameSuccess;
			On_Player.ChangeSpawn += On_Player_ChangeSpawn;
			On_Player.RemoveSpawn += On_Player_RemoveSpawn;
		}

		private void UnloadHook()
		{
			On_Main.GetWorldPathFromName -= On_GetWorldPathFromName;
			On_WorldGen.do_worldGenCallBack -= On_do_worldGenCallBack;
			On_Main.LoadWorlds -= On_LoadWorlds;
			On_UIWorldCreation.FinishCreatingWorld -= On_FinishCreatingWorld;
			On_UIWorldListItem.DeleteButtonClick -= On_DeleteButtonClick;
			On_WorldFileData.OnWorldRenameSuccess -= On_OnWorldRenameSuccess;
			On_Player.ChangeSpawn -= On_Player_ChangeSpawn;
			On_Player.RemoveSpawn -= On_Player_RemoveSpawn;
		}


		private void On_Player_RemoveSpawn(On_Player.orig_RemoveSpawn orig, Player self)
		{
			orig(self);
			if (MultiWorldFileData.IsMultiWorld(Main.ActiveWorldFileData.Path))
			{
				var entityIdInfo = self.GetType().GetField("entityId", BindingFlags.Instance | BindingFlags.NonPublic);
				var entityId = (long)entityIdInfo.GetValue(self);
				var data = MultiWorldFileData.LoadMeta(Path.Combine(Path.GetDirectoryName(Main.ActiveWorldFileData.Path), "meta.world"));
				data ??= MultiWorldFileData.CreateMetaData();
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
				data ??= MultiWorldFileData.CreateMetaData();
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

		private void On_FinishCreatingWorld(On_UIWorldCreation.orig_FinishCreatingWorld orig, UIWorldCreation self)
		{
			orig(self);
			var worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
			if (worldManageSystem.CreateMultiWorld)
			{
				var optionSeedInfo = self.GetType().GetField("_optionSeed", BindingFlags.Instance | BindingFlags.NonPublic);
				var optionSeed = (string)optionSeedInfo.GetValue(self);
				var ProcessSeedInfo = self.GetType().GetMethod("ProcessSeed", BindingFlags.Instance | BindingFlags.NonPublic);
				object[] processedSeed = [null];
				ProcessSeedInfo.Invoke(self, processedSeed);
				var optionSizeInfo = self.GetType().GetField("_optionSize", BindingFlags.Instance | BindingFlags.NonPublic);
				var optionSize = optionSizeInfo.GetValue(self);
				var optionDifficultyInfo = self.GetType().GetField("_optionDifficulty", BindingFlags.Instance | BindingFlags.NonPublic);
				var optionDifficulty = optionDifficultyInfo.GetValue(self);
				var optionEvilInfo = self.GetType().GetField("_optionEvil", BindingFlags.Instance | BindingFlags.NonPublic);
				var optionEvil = optionEvilInfo.GetValue(self);
				var optionwWorldNameInfo = self.GetType().GetField("_optionwWorldName", BindingFlags.Instance | BindingFlags.NonPublic);
				var optionwWorldName = (string)optionwWorldNameInfo.GetValue(self);
				MetaData = MultiWorldFileData.CreateMetaData();
				MetaData.optionSeed = optionSeed;
				MetaData.optionSize = (WorldSizeId)Enum.Parse(typeof(WorldSizeId), optionSize.ToString()); ;
				MetaData.optionDifficulty = (WorldDifficultyId)Enum.Parse(typeof(WorldDifficultyId), optionDifficulty.ToString()); ;
				MetaData.optionEvil = (WorldEvilId)Enum.Parse(typeof(WorldEvilId), optionEvil.ToString()); ;
				MetaData.optionwWorldName = optionwWorldName;
				MetaData.WorldRadius = WorldRadius;
				WorldRadius = 0;
			}

		}

		private void On_do_worldGenCallBack(On_WorldGen.orig_do_worldGenCallBack orig, object threadContext)
		{
			orig(threadContext);
			if (MultiWorldFileData.IsMultiWorld(Main.ActiveWorldFileData.Path))
			{
				var directory = Path.GetDirectoryName(Main.ActiveWorldFileData.Path);
				if (!File.Exists(Path.Combine(directory, "meta.world")))
				{
					MultiWorldFileData.SaveMeta(Path.Combine(directory, "meta.world"), MetaData);
				}
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
