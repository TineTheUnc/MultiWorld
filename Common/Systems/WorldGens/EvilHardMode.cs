using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace MultiWorld.Common.Systems.WorldGens
{
	public class EvilHardMode
	{
		public static List<string> removeList = [
			"Hardmode Good Remix",
			"Hardmode Good",
			"Hardmode Evil"
		];

		public static List<GenPass> Gens(List<GenPass> tasks)
		{
			foreach (string item in removeList)
			{
				int index = tasks.FindIndex(genpass => genpass.Name.Equals(item));
				if (index != -1)
				{
					double loadWeight = tasks[index].Weight;
					tasks.Remove(tasks[index]);
					if (item == "Hardmode Evil")
					{
						tasks.Insert(index, new EvilPass(loadWeight));
					}
				}
			}
			return tasks;
		}

		public class EvilPass(double loadWeight) : GenPass("Hardmode Evil", loadWeight)
		{
			protected override void ApplyPass(GenerationProgress progress, GameConfiguration passConfig)
			{
				var crimson = WorldGen.crimson;
				var index = int.Parse(Path.GetFileNameWithoutExtension(Main.ActiveWorldFileData.Path));
				if (index > 0)
				{
					for (int m = 0; m < Main.maxTilesX; m++)
					{
						for (int n = 0; n < Main.maxTilesY - 5; n++)
						{
							bool convertWall = WallLoader.Convert(m, n, crimson ? 4 : 1);
							bool convertTile = TileLoader.Convert(m, n, crimson ? 4 : 1);
							if (crimson)
							{
								if (convertWall)
								{
									if (Main.tile[m, n].WallType == 63 || Main.tile[m, n].WallType == 65 || Main.tile[m, n].WallType == 66 || Main.tile[m, n].WallType == 68)
										Main.tile[m, n].WallType = 81;
									else if (Main.tile[m, n].WallType == 216)
										Main.tile[m, n].WallType = 218;
									else if (Main.tile[m, n].WallType == 187)
										Main.tile[m, n].WallType = 221;
								}
								if (convertTile)
								{

									if (Main.tile[m, n].TileType == 225)
									{
										Main.tile[m, n].TileType = 203;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 230)
									{
										Main.tile[m, n].TileType = 399;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 60 || Main.tile[m, n].TileType == 661)
									{
										Main.tile[m, n].TileType = 662;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 2 || Main.tile[m, n].TileType == 109)
									{
										Main.tile[m, n].TileType = 199;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 1 || Main.tile[m, n].TileType == 117)
									{
										Main.tile[m, n].TileType = 203;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 53 || Main.tile[m, n].TileType == 123 || Main.tile[m, n].TileType == 116)
									{
										Main.tile[m, n].TileType = 234;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 161 || Main.tile[m, n].TileType == 164)
									{
										Main.tile[m, n].TileType = 200;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 396)
									{
										Main.tile[m, n].TileType = 401;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 397)
									{
										Main.tile[m, n].TileType = 399;
										WorldGen.SquareTileFrame(m, n);
									}
								}
							}
							else
							{
								if (convertWall)
								{
									if (Main.tile[m, n].WallType == 63 || Main.tile[m, n].WallType == 65 || Main.tile[m, n].WallType == 66 || Main.tile[m, n].WallType == 68)
										Main.tile[m, n].WallType = 69;
									else if (Main.tile[m, n].WallType == 216)
										Main.tile[m, n].WallType = 217;
									else if (Main.tile[m, n].WallType == 187)
										Main.tile[m, n].WallType = 220;
								}
								if (convertTile)
								{

									if (Main.tile[m, n].TileType == 225)
									{
										Main.tile[m, n].TileType = 25;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 230)
									{
										Main.tile[m, n].TileType = 398;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 60 || Main.tile[m, n].TileType == 662)
									{
										Main.tile[m, n].TileType = 661;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 2 || Main.tile[m, n].TileType == 109)
									{
										Main.tile[m, n].TileType = 23;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 1 || Main.tile[m, n].TileType == 117)
									{
										Main.tile[m, n].TileType = 25;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 53 || Main.tile[m, n].TileType == 123 || Main.tile[m, n].TileType == 116)
									{
										Main.tile[m, n].TileType = 112;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 161 || Main.tile[m, n].TileType == 164)
									{
										Main.tile[m, n].TileType = 163;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 396)
									{
										Main.tile[m, n].TileType = 400;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 397)
									{
										Main.tile[m, n].TileType = 398;
										WorldGen.SquareTileFrame(m, n);
									}
								}
							}
						}
					}
				}
				else
				{
					for (int m = Main.maxTilesX; m > 0; m--)
					{
						for (int n = 0; n < Main.maxTilesY - 5; n++)
						{
							bool convertWall = WallLoader.Convert(m, n, crimson ? 4 : 1);
							bool convertTile = TileLoader.Convert(m, n, crimson ? 4 : 1);
							if (crimson)
							{
								if (convertWall)
								{
									if (Main.tile[m, n].WallType == 63 || Main.tile[m, n].WallType == 65 || Main.tile[m, n].WallType == 66 || Main.tile[m, n].WallType == 68)
										Main.tile[m, n].WallType = 81;
									else if (Main.tile[m, n].WallType == 216)
										Main.tile[m, n].WallType = 218;
									else if (Main.tile[m, n].WallType == 187)
										Main.tile[m, n].WallType = 221;
								}
								if (convertTile)
								{

									if (Main.tile[m, n].TileType == 225)
									{
										Main.tile[m, n].TileType = 203;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 230)
									{
										Main.tile[m, n].TileType = 399;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 60 || Main.tile[m, n].TileType == 661)
									{
										Main.tile[m, n].TileType = 662;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 2 || Main.tile[m, n].TileType == 109)
									{
										Main.tile[m, n].TileType = 199;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 1 || Main.tile[m, n].TileType == 117)
									{
										Main.tile[m, n].TileType = 203;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 53 || Main.tile[m, n].TileType == 123 || Main.tile[m, n].TileType == 116)
									{
										Main.tile[m, n].TileType = 234;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 161 || Main.tile[m, n].TileType == 164)
									{
										Main.tile[m, n].TileType = 200;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 396)
									{
										Main.tile[m, n].TileType = 401;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 397)
									{
										Main.tile[m, n].TileType = 399;
										WorldGen.SquareTileFrame(m, n);
									}
								}
							}
							else
							{
								if (convertWall)
								{
									if (Main.tile[m, n].WallType == 63 || Main.tile[m, n].WallType == 65 || Main.tile[m, n].WallType == 66 || Main.tile[m, n].WallType == 68)
										Main.tile[m, n].WallType = 69;
									else if (Main.tile[m, n].WallType == 216)
										Main.tile[m, n].WallType = 217;
									else if (Main.tile[m, n].WallType == 187)
										Main.tile[m, n].WallType = 220;
								}
								if (convertTile)
								{

									if (Main.tile[m, n].TileType == 225)
									{
										Main.tile[m, n].TileType = 25;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 230)
									{
										Main.tile[m, n].TileType = 398;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 60 || Main.tile[m, n].TileType == 662)
									{
										Main.tile[m, n].TileType = 661;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 2 || Main.tile[m, n].TileType == 109)
									{
										Main.tile[m, n].TileType = 23;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 1 || Main.tile[m, n].TileType == 117)
									{
										Main.tile[m, n].TileType = 25;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 53 || Main.tile[m, n].TileType == 123 || Main.tile[m, n].TileType == 116)
									{
										Main.tile[m, n].TileType = 112;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 161 || Main.tile[m, n].TileType == 164)
									{
										Main.tile[m, n].TileType = 163;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 396)
									{
										Main.tile[m, n].TileType = 400;
										WorldGen.SquareTileFrame(m, n);
									}
									else if (Main.tile[m, n].TileType == 397)
									{
										Main.tile[m, n].TileType = 398;
										WorldGen.SquareTileFrame(m, n);
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
