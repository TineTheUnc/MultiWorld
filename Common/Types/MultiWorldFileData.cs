using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace MultiWorld.Common.Types
{
	public class MultiWorldFileData: WorldFileData
	{
		public List<WorldFileData> Maps;
		public bool CloudSave;
		public MultiWorldFileData(string path, bool cloudSave) : base(path, cloudSave)
		{
			foreach (var fileName in Directory.GetFiles(Path))
			{
				var file = new FileInfo(fileName);
				if (file.Exists) {
					if (file.Extension == "wld") {
						if (file.Name != "meta.wld") {
							Maps.Add(new(file.FullName, cloudSave));
						}
					}
				}
			}
		}

		public static void SaveMeta(string path, MetaData data)
		{
			var f = new FileInfo(path);
			if (!Directory.Exists(f.DirectoryName))
				Directory.CreateDirectory(f.DirectoryName);

			var jsonData = JsonConvert.SerializeObject(data);
			using var fs = new FileStream(path, FileMode.Create);
			using var gz = new GZipStream(fs, CompressionLevel.Optimal);
			gz.Write(Encoding.ASCII.GetBytes(jsonData));
			gz.Flush();
		}

		public static MetaData LoadMeta(string path)
		{
			try {
				using var fs = new FileStream(path, FileMode.Open);
				using var ms = new MemoryStream();
				fs.CopyTo(ms);
				ms.Position = 0;
				using var gz = new GZipStream(ms, CompressionMode.Decompress);
				using var sr = new StreamReader(gz);
				string json = sr.ReadToEnd();
				if (string.IsNullOrWhiteSpace(json)) throw new Exception("Decompressed JSON is empty.");
				return JsonConvert.DeserializeObject<MetaData>(json) ?? throw new Exception("Failed to deserialize MetaData.");
			} catch (Exception ex) {
				ModContent.GetInstance<MultiWorld>().Logger.Error($"Failed to load meta data from {path}: {ex.Message}");
				var data = CreateMetaData();
				var worldFileData = new WorldFileData(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), "0.wld"), false);
				data.optionSeed = worldFileData.SeedText;
				data.optionwWorldName = worldFileData.Name;
				if (worldFileData.WorldSizeX == 4200 && worldFileData.WorldSizeY == 1200)
				{
					data.optionSize = WorldSizeId.Small;
				}
				else if (worldFileData.WorldSizeX == 6400 && worldFileData.WorldSizeY == 1800)
				{
					data.optionSize = WorldSizeId.Medium;
				}
				else if (worldFileData.WorldSizeX == 8400 && worldFileData.WorldSizeY == 2400)
				{
					data.optionSize = WorldSizeId.Large;
				}
				else
				{
					Array values = Enum.GetValues(typeof(WorldSizeId));
					Random random = new();
					data.optionSize = (WorldSizeId)values.GetValue(random.Next(values.Length));
				}
				if (worldFileData.GameMode == 3)
				{
					data.optionDifficulty = WorldDifficultyId.Creative;
				}
				else if (worldFileData.GameMode == 0)
				{
					data.optionDifficulty = WorldDifficultyId.Normal;
				}
				else if (worldFileData.GameMode == 1)
				{
					data.optionDifficulty = WorldDifficultyId.Expert;
				}
				else if (worldFileData.GameMode == 2)
				{
					data.optionDifficulty = WorldDifficultyId.Master;
				}
				else
				{
					data.optionDifficulty = WorldDifficultyId.Normal;
				}
				if (worldFileData.HasCrimson)
				{
					data.optionEvil = WorldEvilId.Crimson;
				}
				else {
					data.optionEvil = WorldEvilId.Corruption;
				}
				data.Main_hardMode = worldFileData.IsHardMode;
				return data;
			} 
		}



		public static MetaData CreateMetaData() {
			return new MetaData();
		}

		public static bool IsMultiWorld(string path)
		{
			if (path != null)
			{
				var directory = System.IO.Path.GetDirectoryName(path).Split(System.IO.Path.DirectorySeparatorChar);
				if (directory.Last() == "Worlds")
				{
					return false;
				}
				if (directory.Last().Contains(".world"))
				{
					return true;
				}
			}
			return false;
		}

		public static string GetFileName(string path)
		{
			var directory = System.IO.Path.GetDirectoryName(path).Split(System.IO.Path.DirectorySeparatorChar);
			if (directory.Last() == "Worlds")
			{
				return null;
			}
			if (directory.Last().Contains(".world"))
			{
				return directory.Last().Replace(".world", "");
			}
			return null;
		}

		public static string GetFilePath(string path) {
			var data = LoadMeta(System.IO.Path.Combine(path, "meta.world"));
			data ??= MultiWorldFileData.CreateMetaData();
			var playerInfo = Main.ActivePlayerFileData.GetType().GetField("_player", BindingFlags.Instance | BindingFlags.NonPublic);
			var player = (Player)playerInfo.GetValue(Main.ActivePlayerFileData);
			var entityIdInfo = player.GetType().GetField("entityId", BindingFlags.Instance | BindingFlags.NonPublic);
			var entityId = (long)entityIdInfo.GetValue(player);
			string file = System.IO.Path.Combine(path, "0.wld");
			if (data.spawnPoint != null)
			{
				if (data.spawnPoint.TryGetValue(entityId, out int value))
				{
					file = System.IO.Path.Combine(path, value.ToString() + ".wld");
				}
			}
			if (File.Exists(file))
			{
				return file;
			}
			else
			{
				return null;
			}
		}

		public string GetFilePath()
		{
			return GetFilePath(Path);
		}
	}
}
