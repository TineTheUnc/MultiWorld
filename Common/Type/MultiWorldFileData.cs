using System.Collections.Generic;
using Terraria.IO;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using System.Reflection;
using Terraria;

namespace MultiWorld.Common.Type
{
	public class MultiWorldFileData: WorldFileData
	{
		public List<WorldFileData> Maps;
		public bool CloudSave;
		private static readonly byte[] Key = Encoding.UTF8.GetBytes("1248624862486248"); // 16 bytes for AES-128
		private static readonly byte[] IV = Encoding.UTF8.GetBytes("8421521521521521");
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
			if (!Directory.Exists(f.DirectoryName)) {
				Directory.CreateDirectory(f.DirectoryName);
			}
			using var aes = Aes.Create();
			aes.Key = Key;
			aes.IV = IV;

			var encryptor = aes.CreateEncryptor();
			using var fs = new FileStream(path, FileMode.Create);
			using var cs = new CryptoStream(fs, encryptor, CryptoStreamMode.Write);
			using var sw = new StreamWriter(cs);
			var jsonData = JsonConvert.SerializeObject(data);
			sw.Write(jsonData);
		}

		public static MetaData LoadMeta(string path)
		{
			using var aes = Aes.Create();
			aes.Key = Key;
			aes.IV = IV;

			var decryptor = aes.CreateDecryptor();
			using var fs = new FileStream(path, FileMode.Open);
			using var cs = new CryptoStream(fs, decryptor, CryptoStreamMode.Read);
			using var sr = new StreamReader(cs);
			return JsonConvert.DeserializeObject<MetaData>(sr.ReadToEnd());
		}



		public static MetaData CreateMetaData() {
			return new MetaData();
		}

		public static bool IsMultiWorld(string path)
		{
			var directory = System.IO.Path.GetDirectoryName(path).Split(System.IO.Path.DirectorySeparatorChar);
			if (directory.Last() == "Worlds") {
				return false;
			}
			if (directory.Last().Contains(".world")) {
				return true;
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
