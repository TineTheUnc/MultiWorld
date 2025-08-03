using Microsoft.Xna.Framework;
using MonoMod.Cil;
using MultiWorld.Common.Config;
using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent.Biomes;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace MultiWorld.Common.Systems.WorldGens
{
	public class OneBiome
	{

		public static bool Shimmer = true;
		public static short Evil = 0; 
		public static string Biome = string.Empty;
		public static bool HaveDungeon = false;
		public static bool HaveTemple = false;
		public static bool HaveShimmer = false;
		public static bool HaveDungeonGen = false;
		public static bool HaveTempleGen = false;
		public static bool HaveShimmerGen = false;
		public delegate List<GenPass> GenFunc(List<GenPass> tasks, ref double totalWeight);
		public delegate List<GenPass> HardmodeFunc(List<GenPass> tasks);
		public delegate Dictionary<string, int> OnRandomGenHandler(Dictionary<string, int> BiomesChance,bool HardMode = false);
		public static event OnRandomGenHandler OnRandomGen;
		public static Dictionary<string, GenFunc> WorldBiomes = new()
		{
			{ "Forest" , Forest.Gens},
			{ "Desert", Desert.Gens},
			{ "Jungle", Jungle.Gens},
			{ "Corruption", Corruption.Gens},
			{ "Snow", Snow.Gens},
			{ "Ocean", Ocean.Gens},
		};
		public static Dictionary<string, HardmodeFunc> HardModeBiomes = new()
		{
			{ "Good" , GoodHardMode.Gens},
			{ "Evil", EvilHardMode.Gens},
			{ "None", NoneGaneHardmode.Gens},
		};
		public static void Reset()
		{
			Shimmer = true;
			Evil = 0;
			HaveDungeonGen = HaveDungeon;
			HaveTemple = HaveTempleGen;
			HaveShimmer = HaveShimmerGen;
		}

		public static string RandomGen(Dictionary<string, int> BiomesChance)
		{
			int totalWeight = BiomesChance.Values.Sum();
			int roll = WorldGen.genRand.Next(0, totalWeight);
			int cumulative = 0;
			var shuffled = BiomesChance.OrderBy(x => WorldGen.genRand.Next());
			foreach (var kvp in shuffled)
			{
				cumulative += kvp.Value;
				if (roll<cumulative)
					return kvp.Key;
			}

			throw new InvalidOperationException("Random roll failed");
		}

		public static List<GenPass> GenWorld(List<GenPass> tasks, ref double totalWeight) {
			var index = Path.GetFileNameWithoutExtension(Main.ActiveWorldFileData.Path);
			if (index == "0")
			{
				Forest.Gens(tasks,ref totalWeight);
			}
			else
			{
				int i = tasks.FindIndex(genpass => genpass.Name.Equals("Spawn Point"));
				tasks.Remove(tasks[i]);
				int j = tasks.FindIndex(genpass => genpass.Name.Equals("Guide"));
				tasks.Remove(tasks[j]);
				Biome = string.Empty;
				var config = ModContent.GetInstance<Beta>();
				Dictionary<string, int> BiomesChance = new(){
					{ "Forest",config.ForestChance },
					{ "Desert",config.DesertChance },
					{ "Jungle",config.JungleChance },
					{ "Corruption",config.EvilChance },
					{ "Snow",config.SnowChance },
					{ "Ocean",config.OceanChance }
				};
				if (OnRandomGen != null)
				{
					foreach (OnRandomGenHandler handler in OnRandomGen.GetInvocationList().Cast<OnRandomGenHandler>())
					{
						BiomesChance = handler(BiomesChance);
					}
				}
				var bio = RandomGen(BiomesChance);
				tasks = WorldBiomes[bio](tasks, ref totalWeight);
			}
			return tasks;
		}

		public static List<GenPass> GenHardMode(List<GenPass> tasks)
		{
			var index = Path.GetFileNameWithoutExtension(Main.ActiveWorldFileData.Path);
			if (index == "0" || OneBiome.Biome == "Jungle")
			{
				tasks = NoneGaneHardmode.Gens(tasks);
			} 
			else
			{
				if (OneBiome.Biome == "Evil")
				{
					tasks = EvilHardMode.Gens(tasks);
				}
				else
				{
					var config = ModContent.GetInstance<Beta>();
					Dictionary<string, int> HardmodeChance = new(){
						{ "Good" , config.HallowHardModeChance},
						{ "Evil", config.EvilChance},
						{ "None", config.NoneGenChance},
					};
					if (OnRandomGen != null)
					{
						foreach (OnRandomGenHandler handler in OnRandomGen.GetInvocationList().Cast<OnRandomGenHandler>())
						{
							HardmodeChance = handler(HardmodeChance, true);
						}
					}
					var bio = RandomGen(HardmodeChance);
					tasks = HardModeBiomes[bio](tasks);
				}
			}
			return tasks;
		}

		public class BuriedChestPass(bool Desert, double loadWeight) : GenPass("Buried Chests", loadWeight)
		{
			protected override void ApplyPass(GenerationProgress progress, GameConfiguration passConfig)
			{
				progress.Message = Lang.gen[30].Value;
				Main.tileSolid[226] = true;
				Main.tileSolid[162] = true;
				Main.tileSolid[225] = true;
				CaveHouseBiome caveHouseBiome = GenVars.configuration.CreateBiome<CaveHouseBiome>();
				int random6 = passConfig.Get<WorldGenRange>("CaveHouseCount").GetRandom(WorldGen.genRand);
				int random7 = passConfig.Get<WorldGenRange>("UnderworldChestCount").GetRandom(WorldGen.genRand);
				int num546 = passConfig.Get<WorldGenRange>("CaveChestCount").GetRandom(WorldGen.genRand);
				int random8 = 0;
				if (Desert)
				{
					random8 = passConfig.Get<WorldGenRange>("AdditionalDesertHouseCount").GetRandom(WorldGen.genRand);
				}
				if (Main.starGame)
				{
					num546 = (int)((double)num546 * Main.starGameMath(0.2));
				}
				int num547 = random6 + random7 + num546 + random8;
				int num548 = 10000;
				int num549 = 0;
				while (num549 < num546 && num548 > 0)
				{
					progress.Set((double)num549 / (double)num547);
					int num550 = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
					int num551 = WorldGen.genRand.Next((int)((GenVars.worldSurfaceHigh + 20.0 + Main.rockLayer) / 2.0), Main.maxTilesY - 230);
					if (WorldGen.remixWorldGen)
					{
						num551 = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY - 400);
					}
					ushort wall = Main.tile[num550, num551].WallType;
					if (Main.wallDungeon[(int)wall] || wall == 87 || WorldGen.oceanDepths(num550, num551) || !WorldGen.AddBuriedChest(num550, num551, 0, false, -1, false, 0))
					{
						num548--;
						num549--;
					}
					num549++;
				}
				num548 = 10000;
				int num552 = 0;
				while (num552 < random7 && num548 > 0)
				{
					progress.Set((double)(num552 + num546) / (double)num547);
					int num553 = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
					int num554 = WorldGen.genRand.Next(Main.UnderworldLayer, Main.maxTilesY - 50);
					if (Main.wallDungeon[(int)(Main.tile[num553, num554].WallType)] || !WorldGen.AddBuriedChest(num553, num554, 0, false, -1, false, 0))
					{
						num548--;
						num552--;
					}
					num552++;
				}
				num548 = 10000;
				int num555 = 0;
				while (num555 < random6 && num548 > 0)
				{
					progress.Set((double)(num555 + num546 + random7) / (double)num547);
					int x11 = WorldGen.genRand.Next(80, Main.maxTilesX - 80);
					int y9 = WorldGen.genRand.Next((int)(GenVars.worldSurfaceHigh + 20.0), Main.maxTilesY - 230);
					if (WorldGen.remixWorldGen)
					{
						y9 = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY - 400);
					}
					if (WorldGen.oceanDepths(x11, y9) || !caveHouseBiome.Place(new Point(x11, y9), GenVars.structures))
					{
						num548--;
						num555--;
					}
					num555++;
				}
				if (Desert)
				{
					num548 = 10000;
					Rectangle undergroundDesertHiveLocation = GenVars.UndergroundDesertHiveLocation;
					if ((double)undergroundDesertHiveLocation.Y < Main.worldSurface + 26.0)
					{
						int num556 = (int)Main.worldSurface + 26 - undergroundDesertHiveLocation.Y;
						undergroundDesertHiveLocation.Y += num556;
						undergroundDesertHiveLocation.Height -= num556;
					}
					int num557 = 0;
					while (num557 < random8 && num548 > 0)
					{
						progress.Set((double)(num557 + num546 + random7 + random6) / (double)num547);
						if (!caveHouseBiome.Place(WorldGen.RandomRectanglePoint(undergroundDesertHiveLocation), GenVars.structures))
						{
							num548--;
							num557--;
						}
						num557++;
					}
				}
				Main.tileSolid[226] = false;
				Main.tileSolid[162] = false;
				Main.tileSolid[225] = false;
			}
		}

		public static void ModifySpreadingGrass(ILContext il)
		{
			try
			{
				var ilCursor = new ILCursor(il);
				ilCursor.GotoNext(i => i.MatchCall(typeof(WorldGen).GetMethod("SpreadGrass", BindingFlags.Static | BindingFlags.Public)));
				ilCursor.GotoPrev(i => i.MatchLdcI4(2));
				ilCursor.Remove();
				ilCursor.EmitDelegate<Func<int>>(() =>
				{
					if (OneBiome.Biome == string.Empty)
					{
						return 2;
					}
					if (OneBiome.Evil == 1)
					{
						return 23;
					}
					else if (OneBiome.Evil == 2)
					{
						return 199;
					}
					else
					{
						return 2;
					}
				});
				ilCursor.GotoNext(i => i.MatchLdcI4(2));
				ilCursor.Remove();
				ilCursor.EmitDelegate<Func<int>>(() =>
				{
					if (OneBiome.Biome == string.Empty)
					{
						return 2;
					}
					if (OneBiome.Evil == 1)
					{
						return 23;
					}
					else if (OneBiome.Evil == 2)
					{
						return 199;
					}
					else
					{
						return 2;
					}
				});
			}
			catch (Exception e)
			{
				MonoModHooks.DumpIL(ModContent.GetInstance<MultiWorld>(), il);
				throw new ILPatchFailureException(ModContent.GetInstance<MultiWorld>(), il, e);
			}
		}

		public static void ModifyFinalCleanup(ILContext il)
		{
			try
			{
				var ilCursor = new ILCursor(il);
				ilCursor.GotoNext(i => i.MatchCall(typeof(WorldGen).GetMethod("ShimmerCleanUp", BindingFlags.Static | BindingFlags.NonPublic)));
				ilCursor.Remove();
				ilCursor.EmitDelegate<Action>(() =>
				{
					var ShimmerCleanUpInfo = typeof(WorldGen).GetMethod("ShimmerCleanUp", BindingFlags.Static | BindingFlags.NonPublic);
					if (OneBiome.Shimmer  || OneBiome.Biome == string.Empty)
					{
						ShimmerCleanUpInfo.Invoke(null, null);
					}

				});
			}
			catch (Exception e)
			{
				MonoModHooks.DumpIL(ModContent.GetInstance<MultiWorld>(), il);
				throw new ILPatchFailureException(ModContent.GetInstance<MultiWorld>(), il, e);
			}
		}

		public static void ModifyWallVariety(ILContext il)
		{
			try
			{
				var ilCursor = new ILCursor(il);
				ilCursor.GotoNext(i => i.MatchLdsfld(typeof(GenVars).GetField("worldSurface", BindingFlags.Public | BindingFlags.Static)));
				ilCursor.GotoNext(i => i.MatchLdsfld(typeof(GenVars).GetField("worldSurface", BindingFlags.Public | BindingFlags.Static)));
				ilCursor.Index--;
				ilCursor.RemoveRange(20);
				ilCursor.EmitLdloc(4);
				ilCursor.EmitDelegate<Func<Point, Point>>((point2) =>
				{
					Console.WriteLine(0);
					if (OneBiome.Shimmer ||  OneBiome.Biome == string.Empty)
					{
						while (Vector2D.Distance(new Vector2D(point2.X, point2.Y), GenVars.shimmerPosition) < (double)WorldGen.shimmerSafetyDistance)
						{
							point2 = WorldGen.RandomWorldPoint((int)GenVars.worldSurface, 2, 190, 2);
						}
					}
					return point2;
				});
				ilCursor.EmitStloc(4);
			}
			catch (Exception e)
			{
				MonoModHooks.DumpIL(ModContent.GetInstance<MultiWorld>(), il);
				throw new ILPatchFailureException(ModContent.GetInstance<MultiWorld>(), il, e);
			}
		}

		public static void ModifyGrassWall(ILContext il)
		{
			try
			{
				var ilCursor = new ILCursor(il);
				var label = ilCursor.DefineLabel();
				ilCursor.GotoNext(i => i.MatchLdloc(10));
				ilCursor.Index++;
				ilCursor.EmitDelegate<Func<ushort, ushort>>((wallType) =>
				{
					if (OneBiome.Biome == string.Empty)
					{
						return wallType;
					}
					if (OneBiome.Evil == 1)
					{
						return 69;
					}
					else if (OneBiome.Evil == 2)
					{
						return 81;
					}
					else
					{
						return wallType;
					}
				});
				ilCursor.GotoNext(i => i.MatchLdcI4(5));
				ilCursor.EmitDelegate<Func<bool>>(() => {
					return OneBiome.Biome != string.Empty;
				});
				ilCursor.EmitBrfalse(label);

				ilCursor.EmitDelegate<Action>(() =>
				{
					for (int num306 = 5; num306 < Main.maxTilesX - 5; num306++)
					{
						int num307 = 10;
						while ((double)num307 < Main.worldSurface - 1.0)
						{
							if (Main.tile[num306, num307].HasTile && Main.tile[num306, num307].TileType == 0)
							{
								bool flag9 = false;
								bool flag10 = false;
								for (int num308 = num306 - 1; num308 <= num306 + 1; num308++)
								{
									for (int num309 = num307 - 1; num309 <= num307 + 1; num309++)
									{
										if (Main.tile[num308, num309].WallType == 81)
										{
											flag9 = true;
											break;
										}
										else if (Main.tile[num308, num309].WallType == 69)
										{
											flag10 = true;
											break;
										}
									}
								}
								if (flag9)
								{
									WorldGen.SpreadGrass(num306, num307, 0, 199, true, default);
								}
								else if (flag10)
								{
									WorldGen.SpreadGrass(num306, num307, 0, 23, true, default);
								}
							}
							num307++;
						}
					}
				});
				ilCursor.GotoNext(i => i.MatchLdcI4(5));
				ilCursor.MarkLabel(label);
				MonoModHooks.DumpIL(ModContent.GetInstance<MultiWorld>(), il);
			}
			catch (Exception e)
			{
				MonoModHooks.DumpIL(ModContent.GetInstance<MultiWorld>(), il);
				throw new ILPatchFailureException(ModContent.GetInstance<MultiWorld>(), il, e);
			}
		}
	}
}
