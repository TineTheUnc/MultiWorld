using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace MultiWorld.Common.Systems.WorldGens
{
	public class GoodHardMode
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
					tasks[index].Disable();
					if (item == "Hardmode Good")
					{
						tasks.Insert(index, new GoodPass(loadWeight));
					}
				}
			}
			return tasks;
		}

		public class GoodPass(double loadWeight) : GenPass("Hardmode Good", loadWeight)
		{
			protected override void ApplyPass(GenerationProgress progress, GameConfiguration passConfig)
			{
				var index = int.Parse(Path.GetFileNameWithoutExtension(Main.ActiveWorldFileData.Path));
				if (index > 0)
				{
					for (int m = 0; m < Main.maxTilesX; m++)
					{
						for (int n = 0; n < Main.maxTilesY - 5; n++)
						{
							bool convertWall = WallLoader.Convert(m, n, 2);
							bool convertTile = TileLoader.Convert(m, n, 2);
							if (convertWall)
							{

								if (Main.tile[m, n].WallType == 63 || Main.tile[m, n].WallType == 65 || Main.tile[m, n].WallType == 66 || Main.tile[m, n].WallType == 68 || Main.tile[m, n].WallType == 69 || Main.tile[m, n].WallType == 81)
									Main.tile[m, n].WallType = 70;
								else if (Main.tile[m, n].WallType == 216)
									Main.tile[m, n].WallType = 219;
								else if (Main.tile[m, n].WallType == 187)
									Main.tile[m, n].WallType = 222;
								else if (Main.tile[m, n].WallType == 3 || Main.tile[m, n].WallType == 83)
									Main.tile[m, n].WallType = 28;

							}
							if (convertTile)
							{

								if (Main.tile[m, n].TileType == 225)
								{
									Main.tile[m, n].TileType = 117;
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == 230)
								{
									Main.tile[m, n].TileType = 402;
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == 2)
								{
									Main.tile[m, n].TileType = 109;
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == 1 || Main.tile[m, n].TileType == 25 || Main.tile[m, n].TileType == 203)
								{
									Main.tile[m, n].TileType = 117;
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == 53 || Main.tile[m, n].TileType == 123 || Main.tile[m, n].TileType == 112 || Main.tile[m, n].TileType == 234)
								{
									Main.tile[m, n].TileType = 116;
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == 661 || Main.tile[m, n].TileType == 662)
								{
									Main.tile[m, n].TileType = 60;
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == 23 || Main.tile[m, n].TileType == 199)
								{
									Main.tile[m, n].TileType = 109;
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == 161 || Main.tile[m, n].TileType == 163 || Main.tile[m, n].TileType == 200)
								{
									Main.tile[m, n].TileType = 164;
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == 396)
								{
									Main.tile[m, n].TileType = 403;
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == 397)
								{
									Main.tile[m, n].TileType = 402;
									WorldGen.SquareTileFrame(m, n);
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
							bool convertWall = WallLoader.Convert(m, n, 2);
							bool convertTile = TileLoader.Convert(m, n, 2);
							if (convertWall)
							{

								if (Main.tile[m, n].WallType == 63 || Main.tile[m, n].WallType == 65 || Main.tile[m, n].WallType == 66 || Main.tile[m, n].WallType == 68 || Main.tile[m, n].WallType == 69 || Main.tile[m, n].WallType == 81)
									Main.tile[m, n].WallType = 70;
								else if (Main.tile[m, n].WallType == 216)
									Main.tile[m, n].WallType = 219;
								else if (Main.tile[m, n].WallType == 187)
									Main.tile[m, n].WallType = 222;
								else if (Main.tile[m, n].WallType == 3 || Main.tile[m, n].WallType == 83)
									Main.tile[m, n].WallType = 28;

							}
							if (convertTile)
							{

								if (Main.tile[m, n].TileType == 225)
								{
									Main.tile[m, n].TileType = 117;
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == 230)
								{
									Main.tile[m, n].TileType = 402;
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == 2)
								{
									Main.tile[m, n].TileType = 109;
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == 1 || Main.tile[m, n].TileType == 25 || Main.tile[m, n].TileType == 203)
								{
									Main.tile[m, n].TileType = 117;
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == 53 || Main.tile[m, n].TileType == 123 || Main.tile[m, n].TileType == 112 || Main.tile[m, n].TileType == 234)
								{
									Main.tile[m, n].TileType = 116;
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == 661 || Main.tile[m, n].TileType == 662)
								{
									Main.tile[m, n].TileType = 60;
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == 23 || Main.tile[m, n].TileType == 199)
								{
									Main.tile[m, n].TileType = 109;
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == 161 || Main.tile[m, n].TileType == 163 || Main.tile[m, n].TileType == 200)
								{
									Main.tile[m, n].TileType = 164;
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == 396)
								{
									Main.tile[m, n].TileType = 403;
									WorldGen.SquareTileFrame(m, n);
								}
								else if (Main.tile[m, n].TileType == 397)
								{
									Main.tile[m, n].TileType = 402;
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
