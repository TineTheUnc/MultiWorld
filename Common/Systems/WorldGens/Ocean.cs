using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Initializers;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace MultiWorld.Common.Systems.WorldGens
{
	public class Ocean
	{

		public static List<string> removeList = [
			"Dunes",
			"Generate Ice Biome",
			"Jungle",
			"Mud Caves To Grass",
			"Full Desert",
			"Corruption",
			"Dungeon",
			"Pyramids",
			"Wet Jungle",
			"Jungle Temple",
			"Hives",
			"Jungle Chests",
			"Oasis",
			"Ice",
			"Jungle Chests Placement",
			"Temple",
			"Jungle Trees",
			"Glowing Mushrooms and Jungle Plants",
			"Jungle Plants",
			"Gems In Ice Biome",
			"Muds Walls In Jungle",
			"Larva",
			"Lihzahrd Altars",
			"Buried Chests",
			"Shimmer",
			"Living Trees",
			"Wood Tree Walls",
			"Planting Trees",
			"Sunflowers",
			"Grass Wall",
			"Ocean Sand",
			"Beaches",
			"Remove Water From Sand",
			"Tunnels",
			"Surface Caves",
			"Surface Ore and Stone",
			"Surface Chests",
			"Piles"
		];

		public static List<GenPass> Gens(List<GenPass> tasks, ref double totalWeight)
		{
			OneBiome.Shimmer = false;
			OneBiome.Evil = 0;
			OneBiome.Biome = "Ocean";
			foreach (string item in removeList)
			{
				int index = tasks.FindIndex(genpass => genpass.Name.Equals(item));
				if (index != -1)
				{
					double loadWeight = tasks[index].Weight;
					tasks.Remove(tasks[index]);
					if (item == "Buried Chests")
					{
						tasks.Insert(index, new OneBiome.BuriedChestPass(false, loadWeight));
					}
					if (item == "Ocean Sand")
					{
						tasks.Insert(index, new OceanSandPass(loadWeight));
					}
					if (item == "Beaches")
					{
						tasks.Insert(index, new BeachesPass(loadWeight));
					}
				}
			}
			return tasks;
		}

		public class OceanSandPass(double loadWeight) : GenPass("Ocean Sand", loadWeight)
		{

			protected override void ApplyPass(GenerationProgress progress, GameConfiguration passConfig)
			{
				progress.Message = Language.GetTextValue("WorldGeneration.OceanSand");
				int num1075 = WorldGen.genRand.Next(50, 100);
				for (int num1076 = 0; num1076 < Main.maxTilesX; num1076++)
				{
					progress.Set((double)num1076 / Main.maxTilesX);

					for (int num1077 = 0; (double)num1077 < (Main.worldSurface + Main.rockLayer) / 2.0; num1077++)
					{
						if (Main.tile[num1076, num1077].HasTile)
						{
							int num1078 = num1075;
							num1078 += WorldGen.genRand.Next(5);
							for (int num1079 = num1077; num1079 < num1077 + num1078; num1079++)
							{
								Main.tile[num1076, num1079].TileType = 53;
							}

							break;
						}
					}
				}
			}
		}

		public class BeachesPass(double loadWeight) : GenPass("Beaches", loadWeight)
		{
			protected override void ApplyPass(GenerationProgress progress, GameConfiguration passConfig)
			{
				var TuneOceanDepthInfo = typeof(WorldGen).GetMethod("TuneOceanDepth", BindingFlags.NonPublic | BindingFlags.Static);
				progress.Message = Lang.gen[22].Value;
				bool floridaStyle = false;
				bool floridaStyle2 = false;
				if (WorldGen.genRand.NextBool(4))
				{
					if (WorldGen.genRand.NextBool(2))
						floridaStyle = true;
					else
						floridaStyle2 = true;
				}
				int num738;
				for (num738 = 0; !Main.tile[(Main.maxTilesX / 2) - 1, num738].HasTile; num738++)
				{
				}
				for (int num732 = 0; num732 < 2; num732++)
				{
					int num735 = WorldGen.genRand.Next(50, 100);
					int num733;
					int num734;
					if (num732 == 0)
					{
						num733 = 0;
						num734 = (Main.maxTilesX / 2) - num735;

						int num736 = 0;
						double num737 = 1.0;
						GenVars.shellStartXLeft = 0;
						GenVars.shellStartYLeft = num738;
						num738 += WorldGen.genRand.Next(1, 5);
						for (int num739 = num734 - 1; num739 >= num733; num739--)
						{
							if (num739 > 30)
							{
								num736++;
								num737 = (double)TuneOceanDepthInfo.Invoke(null, [num736, num737, floridaStyle]);
							}
							else
							{
								num737 += 1.0;
							}

							int num740 = WorldGen.genRand.Next(15, 20);
							for (int num741 = (int)Main.worldSurface / 2; (double)num741 < (double)num738 + num737 + (double)num740; num741++)
							{
								if ((double)num741 < (double)num738 + num737 * 0.75 - 3.0)
								{
									Main.tile[num739, num741].Get<TileWallWireStateData>().HasTile = false;
									if (num741 > num738)
									{
										Main.tile[num739, num741].LiquidAmount = byte.MaxValue;
										if (Main.tile[num739, num741].LiquidType == 1)
										{
											Main.tile[num739, num741].Get<LiquidData>().LiquidType = 0;
										}
									}
									else if (num741 == num738)
									{
										Main.tile[num739, num741].LiquidAmount = 127;
										if (GenVars.shellStartXLeft == 0)
											GenVars.shellStartXLeft = num739;
									}
								}
								else if (num741 > num738)
								{
									Main.tile[num739, num741].TileType = 53;
									Main.tile[num739, num741].Get<TileWallWireStateData>().HasTile = true;
								}

								Main.tile[num739, num741].WallType = 0;
							}
						}
					}
					else
					{
						num733 = (Main.maxTilesX / 2) + num735;
						num734 = Main.maxTilesX;

						double num743 = 1.0;
						int num744 = 0;
						int num745 = num738;

						GenVars.shellStartXRight = 0;
						GenVars.shellStartYRight = num745;
						num745 += WorldGen.genRand.Next(1, 5);
						for (int num746 = num733; num746 < num734; num746++)
						{
							if (num746 < num734 - 30)
							{
								num744++;
								num743 = (double)TuneOceanDepthInfo.Invoke(null, [num744, num743, floridaStyle2]);
							}
							else
							{
								num743 += 1.0;
							}

							int num747 = WorldGen.genRand.Next(15, 20);
							for (int num748 = (int)Main.worldSurface / 2; (double)num748 < (double)num745 + num743 + (double)num747; num748++)
							{
								if ((double)num748 < (double)num745 + num743 * 0.75 - 3.0)
								{
									Main.tile[num746, num748].Get<TileWallWireStateData>().HasTile = false;
									if (num748 > num745)
									{
										Main.tile[num746, num748].LiquidAmount = byte.MaxValue;
										if (Main.tile[num746, num748].LiquidType == 1)
										{
											Main.tile[num746, num748].Get<LiquidData>().LiquidType = 0;
										}
									}
									else if (num748 == num745)
									{
										Main.tile[num746, num748].LiquidAmount = 127;
										if (GenVars.shellStartXRight == 0)
											GenVars.shellStartXRight = num746;
									}
								}
								else if (num748 > num745)
								{
									Main.tile[num746, num748].TileType = 53;
									Main.tile[num746, num748].Get<TileWallWireStateData>().HasTile = true;
								}

								Main.tile[num746, num748].WallType = 0;
							}
						}
					}
				}
			}
		}
	}
}
