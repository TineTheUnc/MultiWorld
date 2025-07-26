using MonoMod.Core.Platforms;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace MultiWorld.Common.Systems.WorldGens
{
	public class Corruption
	{
		public static List<string> removeList = [
				"Dunes",
				"Ocean Sand",
				"Generate Ice Biome",
				"Jungle",
				"Mud Caves To Grass",
				"Full Desert",
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
				"Corruption",
				"Flowers",
				"Living Trees",
				"Wood Tree Walls",
		];

		public static List<GenPass> Gens(List<GenPass> tasks, ref double totalWeight)
		{
			OneBiome.Biome = "Evil";
			OneBiome.Shimmer = false;
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
					else if(item == "Corruption")
					{
						tasks.Insert(index, new CorruptionPass(loadWeight));
					}
				}
			}
			return tasks;
		}

		public class CorruptionPass(double loadWeight) : GenPass("Corruption", loadWeight)
		{

			protected override void ApplyPass(GenerationProgress progress, GameConfiguration passConfig)
			{
				int num778 = Main.maxTilesX;
				int num779 = 0;
				int num780 = Main.maxTilesX;
				int num781 = 0;
				int num784 = 10;
				num778 -= num784;
				num779 += num784;
				num780 -= num784;
				num781 += num784;
				int num785 = 500;
				int num786 = 100;
				bool flag49 = WorldGen.crimson;
				double num787 = (double)Main.maxTilesX * 0.001;
				if (WorldGen.tenthAnniversaryWorldGen)
				{
					num785 *= 2;
					num786 *= 2;
				}
				if (WorldGen.drunkWorldGen)
				{
					flag49 = true;
				}
				if (flag49)
				{
					OneBiome.Evil = 2;
					progress.Message = Lang.gen[72].Value;
					int num788 = 0;
				
					while ((double)num788 < num787)
					{
						double value15 = (double)num788 / num787;
						progress.Set(value15);
						int num794 = 0;
						int num795 = Main.maxTilesX;
						var num793 = WorldGen.genRand.Next(100, Main.maxTilesX-100);
						WorldGen.CrimStart(num793, (int)GenVars.worldSurfaceLow - 10);
						for (int num798 = num794; num798 < num795; num798++)
						{
							int num799 = (int)GenVars.worldSurfaceLow;
							while ((double)num799 < Main.worldSurface - 1.0)
							{
								if (Main.tile[num798, num799].HasTile)
								{
									int num800 = num799 + WorldGen.genRand.Next(10, 14);
									for (int num801 = num799; num801 < num800; num801++)
									{
										if (Main.tile[num798, num801].TileType == 60 && num798 >= num794 + WorldGen.genRand.Next(5) && num798 < num795 - WorldGen.genRand.Next(5))
										{
											Main.tile[num798, num801].TileType = 662;
										}
									}
									break;
								}
								num799++;
							}
						}
						double num802 = Main.worldSurface + 40.0;
						for (int num803 = num794; num803 < num795; num803++)
						{
							num802 += (double)WorldGen.genRand.Next(-2, 3);
							if (num802 < Main.worldSurface + 30.0)
							{
								num802 = Main.worldSurface + 30.0;
							}
							if (num802 > Main.worldSurface + 50.0)
							{
								num802 = Main.worldSurface + 50.0;
							}
							bool flag51 = false;
							int num804 = (int)GenVars.worldSurfaceLow;
							while ((double)num804 < num802)
							{
								if (Main.tile[num803, num804].HasTile)
								{
									if (Main.tile[num803, num804].TileType == 53 && num803 >= num794 + WorldGen.genRand.Next(5) && num803 <= num795 - WorldGen.genRand.Next(5))
									{
										Main.tile[num803, num804].TileType = 234;
									}
									if ((double)num804 < Main.worldSurface - 1.0 && !flag51)
									{
										if (Main.tile[num803, num804].TileType == 0)
										{
											WorldGen.grassSpread = 0;
											WorldGen.SpreadGrass(num803, num804, 0, 199, true, default);
										}
										else if (Main.tile[num803, num804].TileType == 59)
										{
											WorldGen.grassSpread = 0;
											WorldGen.SpreadGrass(num803, num804, 59, 662, true, default);
										}
									}
									flag51 = true;
									if (Main.tile[num803, num804].WallType == 216)
									{
										Main.tile[num803, num804].WallType = 218;
									}
									else if (Main.tile[num803, num804].WallType == 187)
									{
										Main.tile[num803, num804].WallType = 221;
									}
									if (Main.tile[num803, num804].TileType == 1)
									{
										if (num803 >= num794 + WorldGen.genRand.Next(5) && num803 <= num795 - WorldGen.genRand.Next(5))
										{
											Main.tile[num803, num804].TileType = 203;
										}
									}
									else if (Main.tile[num803, num804].TileType == 2)
									{
										Main.tile[num803, num804].TileType = 199;
									}
									else if (Main.tile[num803, num804].TileType == 60)
									{
										Main.tile[num803, num804].TileType = 662;
									}
									else if (Main.tile[num803, num804].TileType == 161)
									{
										Main.tile[num803, num804].TileType = 200;
									}
									else if (Main.tile[num803, num804].TileType == 396)
									{
										Main.tile[num803, num804].TileType = 401;
									}
									else if (Main.tile[num803, num804].TileType == 397)
									{
										Main.tile[num803, num804].TileType = 399;
									}
									else if (Main.tile[num803, num804].TileType == 398)
									{
										Main.tile[num803, num804].TileType = 400;
									}
								}
								num804++;
							}
						}
						int num805 = WorldGen.genRand.Next(10, 15);
						for (int num806 = 0; num806 < num805; num806++)
						{
							int num807 = 0;
							bool flag52 = false;
							int num808 = 0;
							while (!flag52)
							{
								num807++;
								int num809 = WorldGen.genRand.Next(num794, (num795 - 500) + num808);
								int num810 = WorldGen.genRand.Next((int)(Main.worldSurface - (double)(num808 / 2)), (int)(Main.worldSurface + 100.0 + (double)num808));
								if (num807 > 100)
								{
									num808++;
									num807 = 0;
								}
								if (!Main.tile[num809, num810].HasTile)
								{
									while (!Main.tile[num809, num810].HasTile)
									{
										num810++;
									}
									num810--;
								}
								else
								{
									while (Main.tile[num809, num810].HasTile && (double)num810 > Main.worldSurface)
									{
										num810--;
									}
								}
								if ((num808 > 10 || (Main.tile[num809, num810 + 1].HasTile && Main.tile[num809, num810 + 1].TileType == 203)) && !WorldGen.IsTileNearby(num809, num810, 26, 3))
								{
									WorldGen.Place3x2(num809, num810, 26, 1);
									if (Main.tile[num809, num810].TileType == 26)
									{
										flag52 = true;
									}
								}
								if (num808 > 100)
								{
									flag52 = true;
								}
							}
						}
						num788++;
					}
					WorldGen.CrimPlaceHearts();
				}
				if (!flag49)
				{
					OneBiome.Evil = 1;
					progress.Message = Lang.gen[20].Value;
					int num811 = 0;
					while ((double)num811 < num787)
					{
						int num812 = num780;
						int num813 = num781;
						int num814 = num778;
						int num815 = num779;
						double value16 = (double)num811 / num787;
						progress.Set(value16);
						int num816 = ((!WorldGen.drunkWorldGen) ? WorldGen.genRand.Next(100, Main.maxTilesX - 100) : (GenVars.crimsonLeft ? WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.5), Main.maxTilesX - 100) : WorldGen.genRand.Next(100, (int)((double)Main.maxTilesX * 0.5))));
						int num817 = 0;
						int num821 = 0;
						for (int num822 = num817; num822 < Main.maxTilesX; num822++)
						{
							if (num821 > 0)
							{
								num821--;
							}
							if (num822 == num816 || num821 == 0)
							{
								int num823 = (int)GenVars.worldSurfaceLow;
								while ((double)num823 < Main.worldSurface - 1.0)
								{
									if (Main.tile[num822, num823].HasTile || Main.tile[num822, num823].WallType > 0)
									{
										if (num822 == num816)
										{
											num821 = 200;

											WorldGen.ChasmRunner(Utils.Clamp<int>(num822, 100, Main.maxTilesX-100), num823, WorldGen.genRand.Next(150) + 150, true);
											break;
										}
										if (WorldGen.genRand.NextBool(35)&& num821 == 0)
										{
											num821 = 300;
											bool makeOrb = true;
											WorldGen.ChasmRunner(Utils.Clamp<int>(num822, 100, Main.maxTilesX - 100), num823, WorldGen.genRand.Next(50) + 50, makeOrb);
											break;
										}
										break;
									}
									else
									{
										num823++;
									}
								}
							}
							int num824 = (int)GenVars.worldSurfaceLow;
							while ((double)num824 < Main.worldSurface - 1.0)
							{
								if (Main.tile[num822, num824].HasTile)
								{
									int num825 = num824 + WorldGen.genRand.Next(10, 14);
									for (int num826 = num824; num826 < num825; num826++)
									{
										if (Main.tile[num822, num826].TileType == 60 && num822 >= num817 + WorldGen.genRand.Next(5) && num822 < Main.maxTilesX - WorldGen.genRand.Next(5))
										{
											Main.tile[num822, num826].TileType = 661;
										}
									}
									break;
								}
								num824++;
							}
						}
						double num827 = Main.worldSurface + 40.0;
						for (int num828 = num817; num828 < Main.maxTilesX; num828++)
						{
							num827 += (double)WorldGen.genRand.Next(-2, 3);
							if (num827 < Main.worldSurface + 30.0)
							{
								num827 = Main.worldSurface + 30.0;
							}
							if (num827 > Main.worldSurface + 50.0)
							{
								num827 = Main.worldSurface + 50.0;
							}
							bool flag54 = false;
							int num829 = (int)GenVars.worldSurfaceLow;
							while ((double)num829 < num827)
							{
								if (Main.tile[num828, num829].HasTile)
								{
									if (Main.tile[num828, num829].TileType == 53 && num828 >= num817 + WorldGen.genRand.Next(5) && num828 <= Main.maxTilesX - WorldGen.genRand.Next(5))
									{
										Main.tile[num828, num829].TileType = 112;
									}
									if ((double)num829 < Main.worldSurface - 1.0 && !flag54)
									{
										if (Main.tile[num828, num829].TileType == 0)
										{
											WorldGen.grassSpread = 0;
											WorldGen.SpreadGrass(num828, num829, 0, 23, true, default);
										}
										else if (Main.tile[num828, num829].TileType == 59)
										{
											WorldGen.grassSpread = 0;
											WorldGen.SpreadGrass(num828, num829, 59, 661, true, default);
										}
									}
									flag54 = true;
									if (Main.tile[num828, num829].WallType == 216)
									{
										Main.tile[num828, num829].WallType = 217;
									}
									else if (Main.tile[num828, num829].WallType == 187)
									{
										Main.tile[num828, num829].WallType = 220;
									}
									if (Main.tile[num828, num829].TileType == 1)
									{
										if (num828 >= num817 + WorldGen.genRand.Next(5) && num828 <= Main.maxTilesX - WorldGen.genRand.Next(5))
										{
											Main.tile[num828, num829].TileType = 25;
										}
									}
									else if (Main.tile[num828, num829].TileType == 2)
									{
										Main.tile[num828, num829].TileType = 23;
									}
									else if (Main.tile[num828, num829].TileType == 60)
									{
										Main.tile[num828, num829].TileType = 661;
									}
									else if (Main.tile[num828, num829].TileType == 161)
									{
										Main.tile[num828, num829].TileType = 163;
									}
									else if (Main.tile[num828, num829].TileType == 396)
									{
										Main.tile[num828, num829].TileType = 400;
									}
									else if (Main.tile[num828, num829].TileType == 397)
									{
										Main.tile[num828, num829].TileType = 398;
									}
								}
								num829++;
							}
						}
						for (int num830 = num817; num830 < Main.maxTilesX; num830++)
						{
							for (int num831 = 0; num831 < Main.maxTilesY - 50; num831++)
							{
								if (Main.tile[num830, num831].HasTile && Main.tile[num830, num831].TileType == 31)
								{
									int num837 = num830 - 13;
									int num832 = num830 + 13;
									int num833 = num831 - 13;
									int num834 = num831 + 13;
									for (int num835 = num837; num835 < num832; num835++)
									{
										if (num835 > 10 && num835 < Main.maxTilesX - 10)
										{
											for (int num836 = num833; num836 < num834; num836++)
											{
												if (Math.Abs(num835 - num830) + Math.Abs(num836 - num831) < 9 + WorldGen.genRand.Next(11) && !WorldGen.genRand.NextBool(3)&& Main.tile[num835, num836].TileType != 31)
												{
													Main.tile[num835, num836].Get<TileWallWireStateData>().HasTile = true;
													Main.tile[num835, num836].TileType = 25;
													if (Math.Abs(num835 - num830) <= 1 && Math.Abs(num836 - num831) <= 1)
													{
														Main.tile[num835, num836].Get<TileWallWireStateData>().HasTile = false;
													}
												}
												if (Main.tile[num835, num836].TileType != 31 && Math.Abs(num835 - num830) <= 2 + WorldGen.genRand.Next(3) && Math.Abs(num836 - num831) <= 2 + WorldGen.genRand.Next(3))
												{
													Main.tile[num835, num836].Get<TileWallWireStateData>().HasTile  = false;
												}
											}
										}
									}
								}
							}
						}
						num811++;
					}
				}
			}
		}
	}
}
