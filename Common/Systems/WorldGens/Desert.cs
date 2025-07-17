using Microsoft.Xna.Framework;
using MonoMod.Core.Platforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Biomes;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace MultiWorld.Common.Systems.WorldGens
{
	public class Desert
	{
		public static List<string> removeList = [
			"Ocean Sand",
			"Generate Ice Biome",
			"Jungle",
			"Mud Caves To Grass",
			"Corruption",
			"Dungeon",
			"Beaches",
			"Create Ocean Caves",
			"Wet Jungle",
			"Jungle Temple",
			"Hives",
			"Jungle Chests",
			"Shell Piles",
			"Ice",
			"Jungle Chests Placement",
			"Temple",
			"Jungle Trees",
			"Glowing Mushrooms and Jungle Plants",
			"Jungle Plants",
			"Moss",
			"Gems In Ice Biome",
			"Muds Walls In Jungle",
			"Larva",
			"Lihzahrd Altars",
			"Buried Chests",
			"Shimmer",
			"Dunes",
			"Sand Patches",
			"Clay",
			"Mushroom Patches",
			"Marble",
			"Granite",
			"Silt",
			"Full Desert"
		];

		public static List<GenPass> Gens(List<GenPass> tasks, ref double totalWeight)
		{
			OneBiome.Shimmer = false;
			OneBiome.Evil = 0;
			OneBiome.Biome = "Desert";
			foreach (string item in removeList)
			{
				int index = tasks.FindIndex(genpass => genpass.Name.Equals(item));
				if (index != -1)
				{
					double loadWeight = tasks[index].Weight;
					tasks.Remove(tasks[index]);
					if (item == "Buried Chests")
					{
						tasks.Insert(index, new OneBiome.BuriedChestPass(true, loadWeight));
					}
					else if (item == "Dunes")
					{
						tasks.Insert(index, new DunesPass(loadWeight));
					} else if ( item == "Full Desert" )
					{
						tasks.Insert(index, new DesertPass(loadWeight/2));
						tasks.Insert(index, new MoreSand(loadWeight/2));
					}
				}
			}
			return tasks;
		}

		public class DesertPass(double loadWeight) : GenPass("Full Desert", loadWeight)
		{
			protected override void ApplyPass(GenerationProgress progress, GameConfiguration passConfig)
			{
				progress.Message = Lang.gen[78].Value;
				Main.tileSolid[484] = false;
				int count = WorldGen.genRand.Next(1, 5);
				int[] x16 = new int[count];
				for (int c = 0; c < count; c++)
				{
					int num958 = 0;
					int num960 = WorldGen.genRand.Next(300, Main.maxTilesX-300);
					int num961 = WorldGen.genRand.Next(num960) / 8;
					num961 += num960 / 8;
					x16[c] = num960 + num961;
					int num962 = 0;
					DesertBiome desertBiome = GenVars.configuration.CreateBiome<DesertBiome>();
					while (!desertBiome.Place(new Point(x16[c], (int)GenVars.worldSurfaceHigh + 25), GenVars.structures))
					{
						bool ok = false;
						while (ok) {
							num961 = WorldGen.genRand.Next(num960) / 2;
							num961 += num960 / 8;
							num961 += WorldGen.genRand.Next(num962 / 12);
							foreach (var x in x16) {
								if (Math.Abs(x - (num960 + num961)) > 100) {
									ok = true;
								}
							}
							if (++num962 > Main.maxTilesX-500)
							{
								num962 = 0;
								num958++;
								if (num958 >= 2)
								{
									GenVars.skipDesertTileCheck = true;
								}
							}
						}
						x16[c] = num960 + num961;
					}
					if (WorldGen.remixWorldGen)
					{
						for (int num963 = 50; num963 < Main.maxTilesX - 50; num963++)
						{
							for (int num964 = (int)Main.rockLayer + WorldGen.genRand.Next(-1, 2); num964 < Main.maxTilesY - 50; num964++)
							{
								if ((Main.tile[num963, num964].TileType == 396 || Main.tile[num963, num964].TileType == 397 || Main.tile[num963, num964].TileType == 53) && !WorldGen.SolidTile(num963, num964 - 1, false))
								{
									int num965 = num964;
									while (num965 < num964 + WorldGen.genRand.Next(4, 7) && Main.tile[num963, num965 + 1].HasTile && (Main.tile[num963, num965].TileType == 396 || Main.tile[num963, num965].TileType == 397))
									{
										Main.tile[num963, num965].TileType = 53;
										num965++;
									}
								}
							}
						}
					}
				}
			}
		}

		public class DunesPass(double loadWeight) : GenPass("Dunes", loadWeight)
		{
			protected override void ApplyPass(GenerationProgress progress, GameConfiguration passConfig)
			{
				progress.Message = Lang.gen[1].Value;
				int random9 = WorldGen.genRand.Next(10, 20);
				double num1080 = passConfig.Get<double>("ChanceOfPyramid");
				if (WorldGen.drunkWorldGen)
				{
					num1080 = 1.0;
				}

				GenVars.PyrX = new int[random9 + 3];
				GenVars.PyrY = new int[random9 + 3];
				DunesBiome dunesBiome = GenVars.configuration.CreateBiome<DunesBiome>();
				for (int num1082 = 0; num1082 < random9; num1082++)
				{
					progress.Set((double)num1082 / (double)random9);
					Point origin5 = WorldGen.RandomWorldPoint(500, 0, 0, 500);
					dunesBiome.Place(origin5, GenVars.structures);
					if (WorldGen.genRand.NextDouble() <= num1080)
					{
						int num1084 = WorldGen.genRand.Next(origin5.X - 200, origin5.X + 200);
						for (int num1085 = 0; num1085 < Main.maxTilesY; num1085++)
						{
							if (Main.tile[num1084, num1085].HasTile)
							{
								GenVars.PyrX[GenVars.numPyr] = num1084;
								GenVars.PyrY[GenVars.numPyr] = num1085 + 20;
								GenVars.numPyr++;
								break;
							}
						}
					}
				}
			}
		}

		public class MoreSand(double loadWeight) : GenPass("More Sand", loadWeight)
		{
			protected override void ApplyPass(GenerationProgress progress, GameConfiguration passConfig)
			{
				for (int i = 0; i < Main.maxTilesX - 1; i++)
				{
					var num1 = WorldGen.genRand.Next(1, 10);
					for (int j = GenVars.lavaLine - num1; j > 1; j--)
					{
						if (Main.tile[i, j].HasTile)
						{
							WorldGen.PlaceTile(i, j, 53, mute: true, forced: true);
						}
					}
				}
			}
		}
	}
}
