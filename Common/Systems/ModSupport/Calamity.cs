using CalamityMod;
using CalamityMod.Systems;
using CalamityMod.Tiles.AstralSnow;
using CalamityMod.Tiles.Ores;
using CalamityMod.World;
using CalamityMod.World.Planets;
using MultiWorld.Common.Types;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Terraria;
using Terraria.ModLoader;

namespace MultiWorld.Common.Systems.ModSupport
{
	[ExtendsFromMod("CalamityMod")]
	public class CalamitySupport : ModSystem
	{
		public bool updateDifficulty = false;
		public Dictionary<int, dynamic> WorldUpdateEvent = [];
		public int mode = 0;

		public override void PostSetupContent()
		{
			var MultiSystem = ModContent.GetInstance<WorldManageSystem>();
			MultiSystem.OnMultiWorldLoad += MultiWorldLoad;
			MultiSystem.OnMultiWorldSave += MultiWorldSave;
		}

		public override void Unload()
		{
			var MultiSystem = ModContent.GetInstance<WorldManageSystem>();
			MultiSystem.OnMultiWorldLoad -= MultiWorldLoad;
			MultiSystem.OnMultiWorldSave -= MultiWorldSave;
		}	
		public void MultiWorldLoad(MetaData data)
		{
			if (data.GetExtensionData<int>("CalamityDifficulty") != mode)
			{
				mode = data.GetExtensionData<int>("CalamityDifficulty");
				WorldUpdateEvent.Add(0, null);
			}
			DownedBossSystem.downedEoCAcidRain = data.GetExtensionData<bool>("downedEoCAcidRain");
			DownedBossSystem.downedDesertScourge = data.GetExtensionData<bool>("downedDesertScourge");
			DownedBossSystem.downedCLAM = data.GetExtensionData<bool>("downedCLAM");
			bool downedHiveMind = data.GetExtensionData<bool>("downedHiveMind");
			bool downedPerforator = data.GetExtensionData<bool>("downedPerforator");
			if ((!DownedBossSystem.downedHiveMind && !DownedBossSystem.downedPerforator) && (downedHiveMind || downedPerforator))
			{
				WorldUpdateEvent.Add(1, null);
			}
			DownedBossSystem.downedHiveMind = downedHiveMind;
			DownedBossSystem.downedPerforator = downedPerforator;
			DownedBossSystem.downedSlimeGod = data.GetExtensionData<bool>("downedSlimeGod");
			DownedBossSystem.downedCLAMHardMode = data.GetExtensionData<bool>("downedCLAMHardMode");
			DownedBossSystem.downedAquaticScourgeAcidRain = data.GetExtensionData<bool>("downedAquaticScourgeAcidRain");
			bool downedCryogen = data.GetExtensionData<bool>("downedCryogen");
			if (!DownedBossSystem.downedCryogen && downedCryogen)
			{
				WorldUpdateEvent.Add(2, null);
			}
			DownedBossSystem.downedCryogen = downedCryogen;
			DownedBossSystem.downedAquaticScourge = data.GetExtensionData<bool>("downedAquaticScourge");
			DownedBossSystem.downedCragmawMire = data.GetExtensionData<bool>("downedCragmawMire");
			DownedBossSystem.downedBrimstoneElemental = data.GetExtensionData<bool>("downedBrimstoneElemental");
			DownedBossSystem.downedCalamitasClone = data.GetExtensionData<bool>("downedCalamitasClone");
			DownedBossSystem.downedGSS = data.GetExtensionData<bool>("downedGSS");
			DownedBossSystem.downedLeviathan = data.GetExtensionData<bool>("downedLeviathan");
			DownedBossSystem.downedAstrumAureus = data.GetExtensionData<bool>("downedAstrumAureus");
			DownedBossSystem.downedPlaguebringer = data.GetExtensionData<bool>("downedPlaguebringer");
			DownedBossSystem.downedRavager = data.GetExtensionData<bool>("downedRavager");
			DownedBossSystem.downedAstrumDeus = data.GetExtensionData<bool>("downedAstrumDeus");
			DownedBossSystem.downedGuardians = data.GetExtensionData<bool>("downedGuardians");
			DownedBossSystem.downedDragonfolly = data.GetExtensionData<bool>("downedDragonfolly");
			bool downedProvidence = data.GetExtensionData<bool>("downedProvidence");
			if (!DownedBossSystem.downedProvidence && downedProvidence)
			{
				WorldUpdateEvent.Add(3, null);
			}
			DownedBossSystem.downedProvidence = downedProvidence;
			DownedBossSystem.downedPolterghast = data.GetExtensionData<bool>("downedPolterghast");
			DownedBossSystem.downedNuclearTerror = data.GetExtensionData<bool>("downedNuclearTerror");
			DownedBossSystem.downedMauler = data.GetExtensionData<bool>("downedMauler");
			DownedBossSystem.downedBoomerDuke = data.GetExtensionData<bool>("downedBoomerDuke");
			DownedBossSystem.downedCeaselessVoid = data.GetExtensionData<bool>("downedCeaselessVoid");
			DownedBossSystem.downedStormWeaver = data.GetExtensionData<bool>("downedStormWeaver");
			DownedBossSystem.downedSignus = data.GetExtensionData<bool>("downedSignus");
			DownedBossSystem.downedDoG = data.GetExtensionData<bool>("downedDoG");
			bool downedYharon = data.GetExtensionData<bool>("downedYharon");
			if (!DownedBossSystem.downedYharon && downedYharon)
			{
				WorldUpdateEvent.Add(4, null);
			}
			DownedBossSystem.downedYharon = data.GetExtensionData<bool>("downedYharon");
			DownedBossSystem.downedExoMechs = data.GetExtensionData<bool>("downedExoMechs");
			DownedBossSystem.downedArtemisAndApollo = data.GetExtensionData<bool>("downedArtemisAndApollo");
			DownedBossSystem.downedCalamitas = data.GetExtensionData<bool>("downedCalamitas");
			DownedBossSystem.downedPrimordialWyrm = data.GetExtensionData<bool>("downedPrimordialWyrm");
			DownedBossSystem.downedBossRush = data.GetExtensionData<bool>("downedBossRush");
			DownedBossSystem.downedThanatos = data.GetExtensionData<bool>("downedThanatos");
			DownedBossSystem.downedAres = data.GetExtensionData<bool>("downedAres");
			DownedBossSystem.downedBetsy = data.GetExtensionData<bool>("downedBetsy");
			DownedBossSystem.downedCrabulon = data.GetExtensionData<bool>("downedCrabulon");
			DownedBossSystem.downedDreadnautilus = data.GetExtensionData<bool>("downedDreadnautilus");
			DownedBossSystem.downedSecondSentinels = data.GetExtensionData<bool>("downedSecondSentinels");
			DownedBossSystem.startedBossRushAtLeastOnce = data.GetExtensionData<bool>("startedBossRushAtLeastOnce");
			if (!NPC.downedPlantBoss && data.NPC_downedPlantBoss)
			{
				WorldUpdateEvent.Add(5, null);
			}
			if (!NPC.downedMoonlord && data.NPC_downedMoonlord)
			{
				WorldUpdateEvent.Add(6, null);
			}
			if ((!NPC.downedMechBoss1 && data.NPC_downedMechBoss1)) {
				if (!NPC.downedMechBossAny)
				{
					WorldUpdateEvent.Add(7, null);
				}
				else if ((!NPC.downedMechBoss1 && !NPC.downedMechBoss2) || (!NPC.downedMechBoss2 && !NPC.downedMechBoss3) || (!NPC.downedMechBoss3 && !NPC.downedMechBoss1))
				{
					WorldUpdateEvent.Add(8, null);
				}
				else {
					WorldUpdateEvent.Add(9, null);
				} 
			}
			if ((!NPC.downedMechBoss2 && data.NPC_downedMechBoss2))
			{
				if (!NPC.downedMechBossAny)
				{
					WorldUpdateEvent.Add(7, null);
				}
				else if ((!NPC.downedMechBoss1 && !NPC.downedMechBoss2) || (!NPC.downedMechBoss2 && !NPC.downedMechBoss3) || (!NPC.downedMechBoss3 && !NPC.downedMechBoss1))
				{
					WorldUpdateEvent.Add(8, null);
				}
				else
				{
					WorldUpdateEvent.Add(9, null);
				}
			}
			if ((!NPC.downedMechBoss3 && data.NPC_downedMechBoss3))
			{
				if (!NPC.downedMechBossAny)
				{
					WorldUpdateEvent.Add(7, null);
				}
				else if ((!NPC.downedMechBoss1 && !NPC.downedMechBoss2) || (!NPC.downedMechBoss2 && !NPC.downedMechBoss3) || (!NPC.downedMechBoss3 && !NPC.downedMechBoss1))
				{
					WorldUpdateEvent.Add(8, null);
				}
				else
				{
					WorldUpdateEvent.Add(9, null);
				}
			}
		}
		public MetaData MultiWorldSave(MetaData data)
		{
			var m = 0;
			if (DifficultyModeSystem.GetCurrentDifficulty.GetType() == typeof(NoDifficulty)) {
				m = 0;
			}
			else if (DifficultyModeSystem.GetCurrentDifficulty.GetType() == typeof(RevengeanceDifficulty))
			{
				m = 1;
			}
			else if (DifficultyModeSystem.GetCurrentDifficulty.GetType() == typeof(DeathDifficulty))
			{
				m = 2;
			}
			data.SetExtensionData<int>("CalamityDifficulty", m);
			data.SetExtensionData<bool>("downedEoCAcidRain", DownedBossSystem.downedEoCAcidRain);
			data.SetExtensionData<bool>("downedDesertScourge", DownedBossSystem.downedDesertScourge);
			data.SetExtensionData<bool>("downedCLAM", DownedBossSystem.downedCLAM);
			data.SetExtensionData<bool>("downedHiveMind", DownedBossSystem.downedHiveMind);
			data.SetExtensionData<bool>("downedPerforator", DownedBossSystem.downedPerforator);
			data.SetExtensionData<bool>("downedSlimeGod", DownedBossSystem.downedSlimeGod);
			data.SetExtensionData<bool>("downedCLAMHardMode", DownedBossSystem.downedCLAMHardMode);
			data.SetExtensionData<bool>("downedAquaticScourgeAcidRain", DownedBossSystem.downedAquaticScourgeAcidRain);
			data.SetExtensionData<bool>("downedCryogen", DownedBossSystem.downedCryogen);
			data.SetExtensionData<bool>("downedAquaticScourge", DownedBossSystem.downedAquaticScourge);
			data.SetExtensionData<bool>("downedCragmawMire", DownedBossSystem.downedCragmawMire);
			data.SetExtensionData<bool>("downedBrimstoneElemental", DownedBossSystem.downedBrimstoneElemental);
			data.SetExtensionData<bool>("downedCalamitasClone", DownedBossSystem.downedCalamitasClone);
			data.SetExtensionData<bool>("downedCalamitas", DownedBossSystem.downedCalamitas);
			data.SetExtensionData<bool>("downedGSS", DownedBossSystem.downedGSS);
			data.SetExtensionData<bool>("downedLeviathan", DownedBossSystem.downedLeviathan);
			data.SetExtensionData<bool>("downedAstrumAureus", DownedBossSystem.downedAstrumAureus);
			data.SetExtensionData<bool>("downedPlaguebringer", DownedBossSystem.downedPlaguebringer);
			data.SetExtensionData<bool>("downedRavager", DownedBossSystem.downedRavager);
			data.SetExtensionData<bool>("downedAstrumDeus", DownedBossSystem.downedAstrumDeus);
			data.SetExtensionData<bool>("downedGuardians", DownedBossSystem.downedGuardians);
			data.SetExtensionData<bool>("downedDragonfolly", DownedBossSystem.downedDragonfolly);
			data.SetExtensionData<bool>("downedProvidence", DownedBossSystem.downedProvidence);
			data.SetExtensionData<bool>("downedPolterghast", DownedBossSystem.downedPolterghast);
			data.SetExtensionData<bool>("downedNuclearTerror", DownedBossSystem.downedNuclearTerror);
			data.SetExtensionData<bool>("downedMauler", DownedBossSystem.downedMauler);
			data.SetExtensionData<bool>("downedBoomerDuke", DownedBossSystem.downedBoomerDuke);
			data.SetExtensionData<bool>("downedCeaselessVoid", DownedBossSystem.downedCeaselessVoid);
			data.SetExtensionData<bool>("downedStormWeaver", DownedBossSystem.downedStormWeaver);
			data.SetExtensionData<bool>("downedSignus", DownedBossSystem.downedSignus);
			data.SetExtensionData<bool>("downedDoG", DownedBossSystem.downedDoG);
			data.SetExtensionData<bool>("downedYharon", DownedBossSystem.downedYharon);
			data.SetExtensionData<bool>("downedExoMechs", DownedBossSystem.downedExoMechs);
			data.SetExtensionData<bool>("downedArtemisAndApollo", DownedBossSystem.downedArtemisAndApollo);
			data.SetExtensionData<bool>("downedCalamitas", DownedBossSystem.downedCalamitas);
			data.SetExtensionData<bool>("downedPrimordialWyrm", DownedBossSystem.downedPrimordialWyrm);
			data.SetExtensionData<bool>("downedBossRush", DownedBossSystem.downedBossRush);
			data.SetExtensionData<bool>("downedThanatos", DownedBossSystem.downedThanatos);
			data.SetExtensionData<bool>("downedAres", DownedBossSystem.downedAres);
			data.SetExtensionData<bool>("downedBetsy", DownedBossSystem.downedBetsy);
			data.SetExtensionData<bool>("downedCrabulon", DownedBossSystem.downedCrabulon);
			data.SetExtensionData<bool>("downedDreadnautilus", DownedBossSystem.downedDreadnautilus);
			data.SetExtensionData<bool>("downedSecondSentinels", DownedBossSystem.downedSecondSentinels);
			data.SetExtensionData<bool>("startedBossRushAtLeastOnce", DownedBossSystem.startedBossRushAtLeastOnce);
			return data;
		}

		public static void SwitchToDifficulty(DifficultyMode mode) {
			if (mode == DifficultyModeSystem.GetCurrentDifficulty)
			{
				return;
			}
			var _difficultyTierInfo = typeof(DifficultyMode).GetField("_difficultyTier",BindingFlags.NonPublic | BindingFlags.Instance);
			for (int i = 0; i < DifficultyModeSystem.Difficulties.Count; i++)
			{
				 
				if ((int)_difficultyTierInfo.GetValue(DifficultyModeSystem.Difficulties[i])  >= (int)_difficultyTierInfo.GetValue(mode) && DifficultyModeSystem.Difficulties[i] != mode && DifficultyModeSystem.Difficulties[i].Enabled)
				{
					DifficultyModeSystem.Difficulties[i].Enabled = false;
				}
			}

			for (int j = 0; j < (int)_difficultyTierInfo.GetValue(mode); j++)
			{
				if (DifficultyModeSystem.DifficultyTiers[j].Length == 1)
				{
					DifficultyModeSystem.DifficultyTiers[j][0].Enabled = true;
					continue;
				}

				for (int k = 0; k < DifficultyModeSystem.DifficultyTiers[j].Length; k++)
				{
					DifficultyModeSystem.DifficultyTiers[j][k].Enabled = false;
				}

				DifficultyModeSystem.DifficultyTiers[j][mode.FavoredDifficultyAtTier(j)].Enabled = true;
			}
			mode.Enabled = true;
			CalamityNetcode.SyncCalamityWorldDifficulties(Main.myPlayer);
		}

		public override void PostUpdateWorld()
		{
			if (MultiWorldFileData.IsMultiWorld(Main.ActiveWorldFileData.Path))
			{
				if (WorldUpdateEvent.Keys.Count > 0)
				{
					if (WorldUpdateEvent.ContainsKey(0)) {
						if (mode == 0)
						{
							SwitchToDifficulty(new NoDifficulty());
						}
						else if (mode == 1)
						{
							SwitchToDifficulty(new RevengeanceDifficulty());
						}
						else if (mode == 2)
						{
							SwitchToDifficulty(new DeathDifficulty());
						}
						WorldUpdateEvent.Remove(0);
					}
					if (WorldUpdateEvent.ContainsKey(1))
					{
						AerialiteOreGen.Enchant();
						WorldUpdateEvent.Remove(1);
					}
					if (WorldUpdateEvent.ContainsKey(2))
					{
						int num = ModContent.TileType<CryonicOre>();
						double num2 = 0.00015;
						float num3 = 0.45f;
						float num4 = 0.7f;
						int num5 = 3;
						int num6 = 8;
						int[] array = [147, 161, 163, 200, 164, 0, 0];
						array[5] = ModContent.TileType<AstralSnow>();
						array[6] = ModContent.TileType<AstralIce>();
						CalamityUtils.SpawnOre(num, num2, num3, num4, num5, num6, array);
						WorldUpdateEvent.Remove(2);
					}
					if (WorldUpdateEvent.ContainsKey(3))
					{
						CalamityUtils.SpawnOre(ModContent.TileType<UelibloomOre>(), 0.00017, 0.55f, 0.9f, 8, 14, [59]);
						WorldUpdateEvent.Remove(3);
					}
					if (WorldUpdateEvent.ContainsKey(4)) {
						CalamityUtils.SpawnOre(ModContent.TileType<AuricOre>(), 2E-05, 0.75f, 0.9f, 10, 20, []);
						WorldUpdateEvent.Remove(4);
					} 
					if (WorldUpdateEvent.ContainsKey(5))
					{
						CalamityUtils.SpawnOre(ModContent.TileType<PerennialOre>(), 0.00012, 0.65f, 0.85f, 5, 10, [0, 1]);
						WorldUpdateEvent.Remove(5);
					}
					if (WorldUpdateEvent.ContainsKey(6))
					{
						if (!CalamityWorld.HasGeneratedLuminitePlanetoids)
						{
							ThreadPool.QueueUserWorkItem(delegate (object _)
							{
								LuminitePlanet.GenerateLuminitePlanetoids();
							});
							CalamityWorld.HasGeneratedLuminitePlanetoids = true;
						}
						WorldUpdateEvent.Remove(6);
					}
					if (WorldUpdateEvent.ContainsKey(7))
					{
						CalamityUtils.SpawnOre(108, 0.00012, 0.55f, 0.8f, 3, 8, []);
						CalamityUtils.SpawnOre(222, 0.00012, 0.55f, 0.8f, 3, 8, []);
						WorldUpdateEvent.Remove(7);
					}
					if (WorldUpdateEvent.ContainsKey(8))
					{
						CalamityUtils.SpawnOre(111, 0.00012, 0.65f, 0.9f, 3, 8, []);
						CalamityUtils.SpawnOre(223, 0.00012, 0.65f, 0.9f, 3, 8, []);
						WorldUpdateEvent.Remove(8);
					}
					if (WorldUpdateEvent.ContainsKey(9))
					{
						CalamityUtils.SpawnOre(ModContent.TileType<HallowedOre>(), 0.00017, 0.55f, 0.9f, 8, 14, [117, 402, 403, 164]);
						WorldUpdateEvent.Remove(9);
					}
				}
			}
		}

	}
}
