using System.Collections.Generic;
using Terraria;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace MultiWorld.Common.Systems.WorldGens
{
	public class Snow
	{
		public static List<string> removeList = [
				"Dunes",
				"Ocean Sand",
				"Sand Patches",
				"Jungle",
				"Mud Caves To Grass",
				"Full Desert",
				"Corruption",
				"Living Trees",
				"Wood Tree Walls",
				"Dungeon",
				"Beaches",
				"Create Ocean Caves",
				"Pyramids",
				"Wet Jungle",
				"Jungle Temple",
				"Hives",
				"Jungle Chests",
				"Oasis",
				"Shell Piles",
				"Jungle Chests Placement",
				"Temple",
				"Jungle Trees",
				"Glowing Mushrooms and Jungle Plants",
				"Jungle Plants",
				"Muds Walls In Jungle",
				"Larva",
				"Moss",
				"Cactus, Palm Trees, & Coral",
				"Lihzahrd Altars",
				"Shimmer",
				"Slush",
				"Gems In Ice Biome",
				"Generate Ice Biome",
				"Buried Chests"
			];
		public static List<GenPass> Gens(List<GenPass> tasks, ref double totalWeight)
		{
			OneBiome.Shimmer = false;
			OneBiome.Evil = 0;
			OneBiome.Biome = "Snow";
			foreach (string item in removeList)
			{
				int index = tasks.FindIndex(genpass => genpass.Name.Equals(item));
				if (index != -1)
				{
					double loadWeight = tasks[index].Weight;
					totalWeight += loadWeight;
					tasks[index].Disable();
					if (item == "Buried Chests")
					{
						tasks.Insert(index, new OneBiome.BuriedChestPass(false, loadWeight));
					} 
					else if (item == "Generate Ice Biome")
					{
						tasks.Insert(index, new IcePass(loadWeight));
					}
					else if (item == "Slush")
					{
						tasks.Insert(index, new SlushPass(loadWeight));
					}
					else if (item == "Gems In Ice Biome")
					{
						tasks.Insert(index, new GemsIcePass(loadWeight));
					}
				}
			}
			return tasks;
		}
		public class GemsIcePass(double loadWeight) : GenPass("Gems In Ice Biome", loadWeight)
		{
			protected override void ApplyPass(GenerationProgress progress, GameConfiguration passConfig)
			{
				progress.Set(1.0);
				int num198 = 0;
				while ((double)num198 < (double)Main.maxTilesX * 0.25)
				{
					int num199 = (!WorldGen.remixWorldGen) ? WorldGen.genRand.Next((int)(Main.worldSurface + Main.rockLayer) / 2, GenVars.lavaLine) : WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY - 300);
					int num200 = WorldGen.genRand.Next(GenVars.snowMinX[num199], GenVars.snowMaxX[num199]);
					if (Main.tile[num200, num199].HasTile && (Main.tile[num200, num199].TileType == 147 || Main.tile[num200, num199].TileType == 161 || Main.tile[num200, num199].TileType == 162 || Main.tile[num200, num199].TileType == 224))
					{
						int num203 = WorldGen.genRand.Next(1, 4);
						int num204 = WorldGen.genRand.Next(1, 4);
						int num205 = WorldGen.genRand.Next(12);
						int num206 = (num205 >= 3) ? ((num205 < 6) ? 1 : ((num205 < 8) ? 2 : ((num205 < 10) ? 3 : ((num205 >= 11) ? 5 : 4)))) : 0;
						for (int num207 = num200; num207 < num200; num207++)
						{
							for (int num208 = num199 - num203; num208 < num199 + num204; num208++)
							{
								if (!Main.tile[num207, num208].HasTile)
								{
									WorldGen.PlaceTile(num207, num208, 178, true, false, -1, num206);
								}
							}
						}
					}
					num198++;
				}
			}
		}
		public class SlushPass(double loadWeight) : GenPass("Slush", loadWeight) {

			protected override void ApplyPass(GenerationProgress progress, GameConfiguration passConfig) {
				for (int num750 = GenVars.snowTop; num750 < GenVars.snowBottom; num750++)
				{
					progress.Set(num750 / GenVars.snowBottom);
					for (int num751 = GenVars.snowMinX[num750]; num751 < GenVars.snowMaxX[num750]; num751++)
					{
						ushort num755 = Main.tile[num751, num750].TileType;
						if (num755 != 1)
						{
							if (num755 != 59)
							{
								if (num755 == 123)
								{
									Main.tile[num751, num750].TileType = 224;
								}
							}
							else
							{
								bool flag46 = true;
								int num752 = 0;
								for (int num753 = num751 - num752; num753 <= num751 + num752; num753++)
								{
									for (int num754 = num750 - num752; num754 <= num750 + num752; num754++)
									{
										if (Main.tile[num753, num754].TileType == 60 || Main.tile[num753, num754].TileType == 70 || Main.tile[num753, num754].TileType == 71 || Main.tile[num753, num754].TileType == 72)
										{
											flag46 = false;
											break;
										}
									}
								}
								if (flag46)
								{
									Main.tile[num751, num750].TileType = 224;
								}
							}
						}
						else
						{
							Main.tile[num751, num750].TileType = 161;
						}
					}
				}
			}

		}
		public class IcePass(double loadWeight) : GenPass("Generate Ice Biome", loadWeight)
		{
			protected override void ApplyPass(GenerationProgress progress, GameConfiguration passConfig)
			{
				progress.Message = Lang.gen[56].Value;
				GenVars.snowTop = (int)Main.worldSurface;
				int num975 = GenVars.lavaLine - WorldGen.genRand.Next(160, 200);
				int num976 = GenVars.lavaLine;
				if (WorldGen.remixWorldGen)
				{
					num976 = Main.maxTilesY - 250;
					num975 = num976 - WorldGen.genRand.Next(160, 200);
				}
				int num977 = 0;
				int num978 = Main.maxTilesX;
				int num979 = 10;
				for (int num980 = 0; num980 <= num976 - 140; num980++)
				{
					progress.Set((double)num980 / (double)(num976 - 140));
					GenVars.snowMinX[num980] = num977;
					GenVars.snowMaxX[num980] = num978;
					for (int num981 = num977; num981 < num978; num981++)
					{
						if (num980 < num975)
						{
							if (Main.tile[num981, num980].WallType == 2)
							{
								Main.tile[num981, num980].WallType = 40;
							}
							ushort num983 = Main.tile[num981, num980].TileType;
							if (num983 <= 23)
							{
								switch (num983)
								{
									case 0:
									case 2:
										break;
									case 1:
										Main.tile[num981, num980].TileType = 161;
										goto IL_33F;
									default:
										if (num983 != 23)
										{
											goto IL_33F;
										}
										break;
								}
							}
							else if (num983 != 40 && num983 != 53)
							{
								goto IL_33F;
							}
							Main.tile[num981, num980].TileType = 147;
						}
						else
						{
							num979 += WorldGen.genRand.Next(-3, 4);
							if (WorldGen.genRand.NextBool(3))
							{
								num979 += WorldGen.genRand.Next(-4, 5);
								if (WorldGen.genRand.NextBool(3))
								{
									num979 += WorldGen.genRand.Next(-6, 7);
								}
							}
							if (num979 < 0)
							{
								num979 = WorldGen.genRand.Next(3);
							}
							else if (num979 > 50)
							{
								num979 = 50 - WorldGen.genRand.Next(3);
							}
							int num982 = num980;
							while (num982 < num980 + num979)
							{
								if (Main.tile[num981, num982].WallType == 2)
								{
									Main.tile[num981, num982].WallType = 40;
								}
								ushort num983 = Main.tile[num981, num982].TileType;
								if (num983 <= 23)
								{
									switch (num983)
									{
										case 0:
										case 2:
											goto IL_2F1;
										case 1:
											Main.tile[num981, num982].TileType = 161;
											break;
										default:
											if (num983 == 23)
											{
												goto IL_2F1;
											}
											break;
									}
								}
								else if (num983 == 40 || num983 == 53)
								{
									goto IL_2F1;
								}
							IL_32D:
								num982++;
								continue;
							IL_2F1:
								Main.tile[num981, num982].TileType = 147;
								goto IL_32D;
							}
						}
					IL_33F:;
					}
					if (GenVars.snowBottom < num980)
					{
						GenVars.snowBottom = num980;
					}
				}
			}
		}
	}
}
