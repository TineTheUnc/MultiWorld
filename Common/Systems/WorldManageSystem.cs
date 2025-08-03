using Microsoft.Xna.Framework;
using MultiWorld.Common.Config;
using MultiWorld.Common.Systems.WorldGens;
using MultiWorld.Common.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Events;
using Terraria.GameContent.Generation;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Social;
using Terraria.WorldBuilding;
namespace MultiWorld.Common.Systems
{
	public class WorldManageSystem : ModSystem
	{

		public int NextWorldIndex = 0;
		public int CurrentWorldIndex = 0;
		public bool do_worldGen = false;
		public bool do_chaneWorld = false;
		public bool do_respawn = false;
		public bool do_recall = false;
		public bool do_phone = false;
		public int playerY = 0;
		public bool CreateMultiWorld = false;
		public int Radius = 0;
		public bool load;
		public Dictionary<int, dynamic> WorldUpdateEvent = [];
		public delegate void OnMultiWorldLoadHandler(MetaData data);
		public event OnMultiWorldLoadHandler OnMultiWorldLoad;
		public delegate MetaData OnMultiWorldSaveHandler(MetaData data);
		public event OnMultiWorldSaveHandler OnMultiWorldSave;
		public override void Load()
		{
			WorldGen.ModifyPass((PassLegacy)WorldGen.VanillaGenPasses["Final Cleanup"], OneBiome.ModifyFinalCleanup);
			WorldGen.ModifyPass((PassLegacy)WorldGen.VanillaGenPasses["Wall Variety"], OneBiome.ModifyWallVariety);
			WorldGen.ModifyPass((PassLegacy)WorldGen.VanillaGenPasses["Spreading Grass"], OneBiome.ModifySpreadingGrass);
			WorldGen.ModifyPass((PassLegacy)WorldGen.VanillaGenPasses["Grass Wall"], OneBiome.ModifyGrassWall);
		}

		public override void LoadWorldData(TagCompound tag)
		{
			if (MultiWorldFileData.IsMultiWorld(Main.ActiveWorldFileData.Path)) if (tag.ContainsKey("Biome")) OneBiome.Biome = tag.GetString("Biome");
		}

		public override void SaveWorldData(TagCompound tag)
		{
			if (MultiWorldFileData.IsMultiWorld(Main.ActiveWorldFileData.Path)) tag["Biome"] = OneBiome.Biome;
		}

		public override void PreSaveAndQuit()
		{
			MultiWorldSave();
		}

		public void MultiWorldSave()
		{
			if (MultiWorldFileData.IsMultiWorld(Main.ActiveWorldFileData.Path))
			{
				var _hasRoom = typeof(TownRoomManager).GetField("_hasRoom", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(WorldGen.TownManager) as bool[];
				var _killCountsByNpcId = typeof(NPCKillsTracker).GetField("_killCountsByNpcId", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Main.BestiaryTracker.Kills) as Dictionary<string, int>;
				var _wasSeenNearPlayerByNetId = typeof(NPCWasNearPlayerTracker).GetField("_wasSeenNearPlayerByNetId", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Main.BestiaryTracker.Sights) as List<int>;
				var _chattedWithPlayer = typeof(NPCWasChatWithTracker).GetField("_chattedWithPlayer", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Main.BestiaryTracker.Chats) as HashSet<string>;
				var data = MultiWorldFileData.LoadMeta(Path.Combine(Path.GetDirectoryName(Main.ActiveWorldFileData.Path), "meta.world"));
				data.NPC_downedBoss1 = NPC.downedBoss1;
				data.NPC_downedBoss2 = NPC.downedBoss2;
				data.NPC_downedBoss3 = NPC.downedBoss3;
				data.NPC_downedQueenBee = NPC.downedQueenBee;
				data.NPC_downedMechBoss1 = NPC.downedMechBoss1;
				data.NPC_downedMechBoss2 = NPC.downedMechBoss2;
				data.NPC_downedMechBoss3 = NPC.downedMechBoss3;
				data.NPC_downedMechBossAny = NPC.downedMechBossAny;
				data.NPC_downedPlantBoss = NPC.downedPlantBoss;
				data.NPC_downedGolemBoss = NPC.downedGolemBoss;
				data.NPC_downedSlimeKing = NPC.downedSlimeKing;
				data.NPC_savedGoblin = NPC.savedGoblin;
				data.NPC_savedWizard = NPC.savedWizard;
				data.NPC_savedMech = NPC.savedMech;
				data.NPC_downedGoblins = NPC.downedGoblins;
				data.NPC_downedClown = NPC.downedClown;
				data.NPC_downedFrost = NPC.downedFrost;
				data.NPC_downedPirates = NPC.downedPirates;
				data.NPC_downedFishron = NPC.downedFishron;
				data.NPC_downedMartians = NPC.downedMartians;
				data.NPC_downedAncientCultist = NPC.downedAncientCultist;
				data.NPC_downedMoonlord = NPC.downedMoonlord;
				data.NPC_downedHalloweenKing = NPC.downedHalloweenKing;
				data.NPC_downedHalloweenTree = NPC.downedHalloweenTree;
				data.NPC_downedChristmasIceQueen = NPC.downedChristmasIceQueen;
				data.NPC_downedChristmasSantank = NPC.downedChristmasSantank;
				data.NPC_downedChristmasTree = NPC.downedChristmasTree;
				data.NPC_downedTowerSolar = NPC.downedTowerSolar;
				data.NPC_downedTowerVortex = NPC.downedTowerVortex;
				data.NPC_downedTowerNebula = NPC.downedTowerNebula;
				data.NPC_downedTowerStardust = NPC.downedTowerStardust;
				data.NPC_downedEmpressOfLight = NPC.downedEmpressOfLight;
				data.NPC_downedQueenSlime = NPC.downedQueenSlime;
				data.NPC_downedDeerclops = NPC.downedDeerclops;
				data.NPC_boughtCat = NPC.boughtCat;
				data.NPC_boughtDog = NPC.boughtDog;
				data.NPC_boughtBunny = NPC.boughtBunny;
				data.NPC_unlockedSlimeBlueSpawn = NPC.unlockedSlimeBlueSpawn;
				data.NPC_unlockedMerchantSpawn = NPC.unlockedMerchantSpawn;
				data.NPC_unlockedDemolitionistSpawn = NPC.unlockedDemolitionistSpawn;
				data.NPC_unlockedPartyGirlSpawn = NPC.unlockedPartyGirlSpawn;
				data.NPC_unlockedDyeTraderSpawn = NPC.unlockedDyeTraderSpawn;
				data.NPC_unlockedTruffleSpawn = NPC.unlockedTruffleSpawn;
				data.NPC_unlockedArmsDealerSpawn = NPC.unlockedArmsDealerSpawn;
				data.NPC_unlockedNurseSpawn = NPC.unlockedNurseSpawn;
				data.NPC_unlockedPrincessSpawn = NPC.unlockedPrincessSpawn;
				data.NPC_unlockedSlimeGreenSpawn = NPC.unlockedSlimeGreenSpawn;
				data.NPC_unlockedSlimeOldSpawn = NPC.unlockedSlimeOldSpawn;
				data.NPC_unlockedSlimePurpleSpawn = NPC.unlockedSlimePurpleSpawn;
				data.NPC_unlockedSlimeRainbowSpawn = NPC.unlockedSlimeRainbowSpawn;
				data.NPC_unlockedSlimeRedSpawn = NPC.unlockedSlimeRedSpawn;
				data.NPC_unlockedSlimeYellowSpawn = NPC.unlockedSlimeYellowSpawn;
				data.NPC_unlockedSlimeCopperSpawn = NPC.unlockedSlimeCopperSpawn;
				data.NPC_savedAngler = NPC.savedAngler;
				data.NPC_savedStylist = NPC.savedStylist;
				data.NPC_savedTaxCollector = NPC.savedTaxCollector;
				data.NPC_savedGolfer = NPC.savedGolfer;
				data.NPC_combatBookWasUsed = NPC.combatBookWasUsed;
				data.NPC_combatBookVolumeTwoWasUsed = NPC.combatBookVolumeTwoWasUsed;
				data.NPC_peddlersSatchelWasUsed = NPC.peddlersSatchelWasUsed;
				data.NPC_savedBartender = NPC.savedBartender;
				data.NPC_ShimmeredTownNPCs = NPC.ShimmeredTownNPCs;
				data.NPC_hasRoom = _hasRoom;
				data.NPC_killCount = NPC.killCount;
				data.WorldGen_shadowOrbSmashed = WorldGen.shadowOrbSmashed;
				data.WorldGen_spawnMeteor = WorldGen.spawnMeteor;
				data.WorldGen_shadowOrbCount = WorldGen.shadowOrbCount;
				data.WorldGen_altarCount = WorldGen.altarCount;
				data.WorldGen_treeBG1 = WorldGen.treeBG1;
				data.WorldGen_treeBG2 = WorldGen.treeBG2;
				data.WorldGen_treeBG3 = WorldGen.treeBG3;
				data.WorldGen_treeBG4 = WorldGen.treeBG4;
				data.WorldGen_corruptBG = WorldGen.corruptBG;
				data.WorldGen_jungleBG = WorldGen.jungleBG;
				data.WorldGen_snowBG = WorldGen.snowBG;
				data.WorldGen_hallowBG = WorldGen.hallowBG;
				data.WorldGen_crimsonBG = WorldGen.crimsonBG;
				data.WorldGen_desertBG = WorldGen.desertBG;
				data.WorldGen_oceanBG = WorldGen.oceanBG;
				data.WorldGen_mushroomBG = WorldGen.mushroomBG;
				data.WorldGen_underworldBG = WorldGen.underworldBG;
				data.Main_hardMode = Main.hardMode;
				data.Main_afterPartyOfDoom = Main.afterPartyOfDoom;
				data.Main_invasionDelay = Main.invasionDelay;
				data.Main_invasionSize = Main.invasionSize;
				data.Main_invasionType = Main.invasionType;
				data.Main_invasionX = Main.invasionX;
				data.Main_slimeRainTime = Main.slimeRainTime;
				data.Main_sundialCooldown = Main.sundialCooldown;
				data.Main_raining = Main.raining;
				data.Main_rainTime = Main.rainTime;
				data.Main_maxRain = Main.maxRain;
				data.Main_cloudBGActive = Main.cloudBGActive;
				data.Main_cloudBGAlpha = Main.cloudBGAlpha;
				data.Main_numClouds = Main.numClouds;
				data.Main_windSpeedTarget = Main.windSpeedTarget;
				data.Main_windSpeedCurrent = Main.windSpeedCurrent;
				data.Main_anglerWhoFinishedToday = Main.anglerWhoFinishedToday;
				data.Main_anglerQuest = Main.anglerQuest;
				data.Main_invasionSizeStart = Main.invasionSizeStart;
				data.Main_fastForwardTimeToDawn = Main.fastForwardTimeToDawn;
				data.Main_treeBGSet1 = Main.treeBGSet1;
				data.Main_treeBGSet2 = Main.treeBGSet2;
				data.Main_treeBGSet3 = Main.treeBGSet3;
				data.Main_treeBGSet4 = Main.treeBGSet4;
				data.Main_treeMntBGSet1 = Main.treeMntBGSet1;
				data.Main_treeMntBGSet2 = Main.treeMntBGSet2;
				data.Main_treeMntBGSet3 = Main.treeMntBGSet3;
				data.Main_treeMntBGSet4 = Main.treeMntBGSet4;
				data.Main_corruptBG = Main.corruptBG;
				data.Main_jungleBG = Main.jungleBG;
				data.Main_snowBG = Main.snowBG;
				data.Main_snowMntBG = Main.snowMntBG;
				data.Main_hallowBG = Main.hallowBG;
				data.Main_crimsonBG = Main.crimsonBG;
				data.Main_desertBG = Main.desertBG;
				data.Main_mushroomBG = Main.mushroomBG;
				data.Main_underworldBG = Main.underworldBG;
				data.Main_forceHalloweenForToday = Main.forceHalloweenForToday;
				data.Main_forceXMasForToday = Main.forceXMasForToday;
				data.Main_fastForwardTimeToDusk = Main.fastForwardTimeToDusk;
				data.Main_moondialCooldown = Main.moondialCooldown;
				data.Main_moonType = Main.moonType;
				data.Main_eclipse = Main.eclipse;
				data.Main_time = Main.time;
				data.Main_dayTime = Main.dayTime;
				data.Main_moonPhase = Main.moonPhase;
				data.Main_bloodMoon = Main.bloodMoon;
				data.CultistRitual_delay = CultistRitual.delay;
				data.BirthdayParty_ManualParty = BirthdayParty.ManualParty;
				data.BirthdayParty_GenuineParty = BirthdayParty.GenuineParty;
				data.BirthdayParty_PartyDaysOnCooldown = BirthdayParty.PartyDaysOnCooldown;
				data.BirthdayParty_CelebratingNPCs = BirthdayParty.CelebratingNPCs;
				data.Sandstorm_Happening = Sandstorm.Happening;
				data.Sandstorm_TimeLeft = Sandstorm.TimeLeft;
				data.Sandstorm_Severity = Sandstorm.Severity;
				data.Sandstorm_IntendedSeverity = Sandstorm.IntendedSeverity;
				data.LanternNight_LanternNightsOnCooldown = LanternNight.LanternNightsOnCooldown;
				data.LanternNight_GenuineLanterns = LanternNight.GenuineLanterns;
				data.LanternNight_ManualLanterns = LanternNight.ManualLanterns;
				data.LanternNight_NextNightIsLanternNight = LanternNight.NextNightIsLanternNight;
				data.DD2Event_DownedInvasionT1 = DD2Event.DownedInvasionT1;
				data.DD2Event_DownedInvasionT2 = DD2Event.DownedInvasionT2;
				data.DD2Event_DownedInvasionT3 = DD2Event.DownedInvasionT3;
				data.Bestiary_Kills = _killCountsByNpcId;
				data.Bestiary_wasSeenNearPlayerByNetId = _wasSeenNearPlayerByNetId;
				data.Bestiary_chattedWithPlayer = _chattedWithPlayer;
				data.SaveFrom = Path.GetFileNameWithoutExtension(Main.ActiveWorldFileData.Path);
				data.HaveDungeon = OneBiome.HaveDungeonGen;
				data.HaveShimmer = OneBiome.HaveShimmerGen;
				data.HaveTemple = OneBiome.HaveTempleGen;
				if (OnMultiWorldSave != null)
				{
					foreach (OnMultiWorldSaveHandler handler in OnMultiWorldSave.GetInvocationList().Cast<OnMultiWorldSaveHandler>())
					{
						data = handler(data);
					}
				}
				MultiWorldFileData.SaveMeta(Path.Combine(Path.GetDirectoryName(Main.ActiveWorldFileData.Path), "meta.world"), data);
			}
		}

		public override void OnWorldLoad()
		{
			load = true;
		}

		public void MultiWorldLoad()
		{
			if (MultiWorldFileData.IsMultiWorld(Main.ActiveWorldFileData.Path))
			{
				var data = MultiWorldFileData.LoadMeta(Path.Combine(Path.GetDirectoryName(Main.ActiveWorldFileData.Path), "meta.world"));
				if (data.SaveFrom != Path.GetFileNameWithoutExtension(Main.ActiveWorldFileData.Path) && data.SaveFrom != null)
				{
					OnMultiWorldLoad?.Invoke(data);
					NPC.downedBoss1 = data.NPC_downedBoss1;
					NPC.downedBoss2 = data.NPC_downedBoss2;
					NPC.downedBoss3 = data.NPC_downedBoss3;
					NPC.downedQueenBee = data.NPC_downedQueenBee;
					if ((data.NPC_downedMechBoss1 && data.NPC_downedMechBoss2 && data.NPC_downedMechBoss3) && !(NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3))
					{
						WorldUpdateEvent.Add(3, null);
					}
					NPC.downedMechBoss1 = data.NPC_downedMechBoss1;
					NPC.downedMechBoss2 = data.NPC_downedMechBoss2;
					NPC.downedMechBoss3 = data.NPC_downedMechBoss3;
					NPC.downedMechBossAny = data.NPC_downedMechBossAny;
					NPC.downedPlantBoss = data.NPC_downedPlantBoss;
					NPC.downedGolemBoss = data.NPC_downedGolemBoss;
					NPC.downedSlimeKing = data.NPC_downedSlimeKing;
					NPC.savedGoblin = data.NPC_savedGoblin;
					NPC.savedWizard = data.NPC_savedWizard;
					NPC.savedMech = data.NPC_savedMech;
					NPC.downedGoblins = data.NPC_downedGoblins;
					NPC.downedClown = data.NPC_downedClown;
					NPC.downedFrost = data.NPC_downedFrost;
					NPC.downedPirates = data.NPC_downedPirates;
					NPC.downedFishron = data.NPC_downedFishron;
					NPC.downedMartians = data.NPC_downedMartians;
					NPC.downedAncientCultist = data.NPC_downedAncientCultist;
					NPC.downedMoonlord = data.NPC_downedMoonlord;
					NPC.downedHalloweenKing = data.NPC_downedHalloweenKing;
					NPC.downedHalloweenTree = data.NPC_downedHalloweenTree;
					NPC.downedChristmasIceQueen = data.NPC_downedChristmasIceQueen;
					NPC.downedChristmasSantank = data.NPC_downedChristmasSantank;
					NPC.downedChristmasTree = data.NPC_downedChristmasTree;
					NPC.downedTowerSolar = data.NPC_downedTowerSolar;
					NPC.downedTowerVortex = data.NPC_downedTowerVortex;
					NPC.downedTowerNebula = data.NPC_downedTowerNebula;
					NPC.downedTowerStardust = data.NPC_downedTowerStardust;
					NPC.downedEmpressOfLight = data.NPC_downedEmpressOfLight;
					NPC.downedQueenSlime = data.NPC_downedQueenSlime;
					NPC.downedDeerclops = data.NPC_downedDeerclops;
					NPC.boughtCat = data.NPC_boughtCat;
					NPC.boughtDog = data.NPC_boughtDog;
					NPC.boughtBunny = data.NPC_boughtBunny;
					NPC.unlockedSlimeBlueSpawn = data.NPC_unlockedSlimeBlueSpawn;
					NPC.unlockedMerchantSpawn = data.NPC_unlockedMerchantSpawn;
					NPC.unlockedDemolitionistSpawn = data.NPC_unlockedDemolitionistSpawn;
					NPC.unlockedPartyGirlSpawn = data.NPC_unlockedPartyGirlSpawn;
					NPC.unlockedDyeTraderSpawn = data.NPC_unlockedDyeTraderSpawn;
					NPC.unlockedTruffleSpawn = data.NPC_unlockedTruffleSpawn;
					NPC.unlockedArmsDealerSpawn = data.NPC_unlockedArmsDealerSpawn;
					NPC.unlockedNurseSpawn = data.NPC_unlockedNurseSpawn;
					NPC.unlockedPrincessSpawn = data.NPC_unlockedPrincessSpawn;
					NPC.unlockedSlimeGreenSpawn = data.NPC_unlockedSlimeGreenSpawn;
					NPC.unlockedSlimeOldSpawn = data.NPC_unlockedSlimeOldSpawn;
					NPC.unlockedSlimePurpleSpawn = data.NPC_unlockedSlimePurpleSpawn;
					NPC.unlockedSlimeRainbowSpawn = data.NPC_unlockedSlimeRainbowSpawn;
					NPC.unlockedSlimeRedSpawn = data.NPC_unlockedSlimeRedSpawn;
					NPC.unlockedSlimeYellowSpawn = data.NPC_unlockedSlimeYellowSpawn;
					NPC.unlockedSlimeCopperSpawn = data.NPC_unlockedSlimeCopperSpawn;
					NPC.savedAngler = data.NPC_savedAngler;
					NPC.savedStylist = data.NPC_savedStylist;
					NPC.savedTaxCollector = data.NPC_savedTaxCollector;
					NPC.savedGolfer = data.NPC_savedGolfer;
					NPC.combatBookWasUsed = data.NPC_combatBookWasUsed;
					NPC.combatBookVolumeTwoWasUsed = data.NPC_combatBookVolumeTwoWasUsed;
					NPC.peddlersSatchelWasUsed = data.NPC_peddlersSatchelWasUsed;
					NPC.savedBartender = data.NPC_savedBartender;
					if (data.NPC_ShimmeredTownNPCs != null) NPC.ShimmeredTownNPCs = data.NPC_ShimmeredTownNPCs;
					if (data.NPC_hasRoom != null) typeof(TownRoomManager).GetField("_hasRoom", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(WorldGen.TownManager, data.NPC_hasRoom);
					if (data.NPC_killCount != null) NPC.killCount = data.NPC_killCount;
					WorldGen.shadowOrbSmashed = data.WorldGen_shadowOrbSmashed;
					WorldGen.spawnMeteor = data.WorldGen_spawnMeteor;
					if (data.WorldGen_shadowOrbCount != null) WorldGen.shadowOrbCount = (int)data.WorldGen_shadowOrbCount;
					if (data.WorldGen_treeBG1 != null) WorldGen.treeBG1 = (int)data.WorldGen_treeBG1;
					if (data.WorldGen_treeBG2 != null) WorldGen.treeBG2 = (int)data.WorldGen_treeBG2;
					if (data.WorldGen_treeBG3 != null) WorldGen.treeBG3 = (int)data.WorldGen_treeBG3;
					if (data.WorldGen_treeBG4 != null) WorldGen.treeBG4 = (int)data.WorldGen_treeBG4;
					if (data.WorldGen_corruptBG != null) WorldGen.corruptBG = (int)data.WorldGen_corruptBG;
					if (data.WorldGen_jungleBG != null) WorldGen.jungleBG = (int)data.WorldGen_jungleBG;
					if (data.WorldGen_snowBG != null) WorldGen.snowBG = (int)data.WorldGen_snowBG;
					if (data.WorldGen_hallowBG != null) WorldGen.hallowBG = (int)data.WorldGen_hallowBG;
					if (data.WorldGen_crimsonBG != null) WorldGen.crimsonBG = (int)data.WorldGen_crimsonBG;
					if (data.WorldGen_desertBG != null) WorldGen.desertBG = (int)data.WorldGen_desertBG;
					if (data.WorldGen_oceanBG != null) WorldGen.oceanBG = (int)data.WorldGen_oceanBG;
					if (data.WorldGen_mushroomBG != null) WorldGen.mushroomBG = (int)data.WorldGen_mushroomBG;
					if (data.WorldGen_underworldBG != null) WorldGen.underworldBG = (int)data.WorldGen_underworldBG;
					if (!Main.hardMode && data.Main_hardMode)
					{
						WorldUpdateEvent.Add(1, null);
					}
					if (data.WorldGen_altarCount > WorldGen.altarCount)
					{
						WorldUpdateEvent.Add(2, data.WorldGen_altarCount - WorldGen.altarCount);
					}
					Main.afterPartyOfDoom = data.Main_afterPartyOfDoom;
					if (data.Main_invasionDelay != null) Main.invasionDelay = (int)data.Main_invasionDelay;
					if (data.Main_invasionSize != null) Main.invasionSize = (int)data.Main_invasionSize;
					if (data.Main_invasionType != null) Main.invasionType = (int)data.Main_invasionType;
					if (data.Main_invasionX != null) Main.invasionX = (double)data.Main_invasionX;
					if (data.Main_slimeRainTime != null) Main.slimeRainTime = (double)data.Main_slimeRainTime;
					if (data.Main_sundialCooldown != null) Main.sundialCooldown = (int)data.Main_sundialCooldown;
					Main.raining = data.Main_raining;
					if (data.Main_rainTime != null) Main.rainTime = (double)data.Main_rainTime;
					if (data.Main_maxRain != null) Main.maxRain = (int)data.Main_maxRain;
					if (data.Main_cloudBGActive != null) Main.cloudBGActive = (float)data.Main_cloudBGActive;
					if (data.Main_cloudBGAlpha != null) Main.cloudBGAlpha = (float)data.Main_cloudBGAlpha;
					if (data.Main_numClouds != null) Main.numClouds = (int)data.Main_numClouds;
					if (data.Main_windSpeedTarget != null) Main.windSpeedTarget = (float)data.Main_windSpeedTarget;
					if (data.Main_windSpeedCurrent != null) Main.windSpeedCurrent = (float)data.Main_windSpeedCurrent;
					if (data.Main_anglerWhoFinishedToday != null) Main.anglerWhoFinishedToday = data.Main_anglerWhoFinishedToday;
					if (data.Main_anglerQuest != null) Main.anglerQuest = (int)data.Main_anglerQuest;
					if (data.Main_invasionSizeStart != null) Main.invasionSizeStart = (int)data.Main_invasionSizeStart;
					Main.fastForwardTimeToDawn = data.Main_fastForwardTimeToDawn;
					if (data.Main_treeBGSet1 != null) Main.treeBGSet1 = data.Main_treeBGSet1;
					if (data.Main_treeBGSet2 != null) Main.treeBGSet2 = data.Main_treeBGSet2;
					if (data.Main_treeBGSet3 != null) Main.treeBGSet3 = data.Main_treeBGSet3;
					if (data.Main_treeBGSet4 != null) Main.treeBGSet4 = data.Main_treeBGSet4;
					if (data.Main_treeMntBGSet1 != null)Main.treeMntBGSet1 = data.Main_treeMntBGSet1;
					if (data.Main_treeMntBGSet2 != null)Main.treeMntBGSet2 = data.Main_treeMntBGSet2;
					if (data.Main_treeMntBGSet3 != null)Main.treeMntBGSet3 = data.Main_treeMntBGSet3;
					if (data.Main_treeMntBGSet4 != null)Main.treeMntBGSet4 = data.Main_treeMntBGSet4;
					if (data.Main_corruptBG != null)Main.corruptBG = data.Main_corruptBG;
					if (data.Main_jungleBG != null)Main.jungleBG = data.Main_jungleBG;
					if (data.Main_snowBG != null)Main.snowBG = data.Main_snowBG;
					if (data.Main_snowMntBG != null)Main.snowMntBG = data.Main_snowMntBG;
					if (data.Main_hallowBG != null)Main.hallowBG = data.Main_hallowBG;
					if (data.Main_crimsonBG != null)Main.crimsonBG = data.Main_crimsonBG;
					if (data.Main_desertBG != null)Main.desertBG = data.Main_desertBG;
					if (data.Main_mushroomBG != null)Main.mushroomBG = data.Main_mushroomBG;
					if (data.Main_underworldBG != null) Main.underworldBG = data.Main_underworldBG;
					Main.forceHalloweenForToday = data.Main_forceHalloweenForToday;
					Main.forceXMasForToday = data.Main_forceXMasForToday;
					Main.fastForwardTimeToDusk = data.Main_fastForwardTimeToDusk;
					if (data.Main_moondialCooldown != null) Main.moondialCooldown = (int)data.Main_moondialCooldown;
					if (data.Main_moonType != null) Main.moonType = (int)data.Main_moonType;
					Main.eclipse = data.Main_eclipse;
					if (data.Main_time != null) Main.time = (double)data.Main_time;
					Main.dayTime = data.Main_dayTime;
					if (data.Main_moonPhase != null) Main.moonPhase = (int)data.Main_moonPhase;
					Main.bloodMoon = data.Main_bloodMoon;
					if (data.CultistRitual_delay != null) CultistRitual.delay = (double)data.CultistRitual_delay;
					BirthdayParty.ManualParty = data.BirthdayParty_ManualParty;
					BirthdayParty.GenuineParty = data.BirthdayParty_GenuineParty;
					if (data.BirthdayParty_PartyDaysOnCooldown != null) BirthdayParty.PartyDaysOnCooldown = (int)data.BirthdayParty_PartyDaysOnCooldown;
					if (data.BirthdayParty_CelebratingNPCs != null) BirthdayParty.CelebratingNPCs = data.BirthdayParty_CelebratingNPCs;
					Sandstorm.Happening = data.Sandstorm_Happening;
					if (data.Sandstorm_TimeLeft != null) Sandstorm.TimeLeft = (double)data.Sandstorm_TimeLeft;
					if (data.Sandstorm_Severity != null) Sandstorm.Severity = (float)data.Sandstorm_Severity;
					if (data.Sandstorm_IntendedSeverity != null) Sandstorm.IntendedSeverity = (float)data.Sandstorm_IntendedSeverity;
					if (data.LanternNight_LanternNightsOnCooldown != null) LanternNight.LanternNightsOnCooldown = (int)data.LanternNight_LanternNightsOnCooldown;
					LanternNight.GenuineLanterns = data.LanternNight_GenuineLanterns;
					LanternNight.ManualLanterns = data.LanternNight_ManualLanterns;
					LanternNight.NextNightIsLanternNight = data.LanternNight_NextNightIsLanternNight;
					DD2Event.DownedInvasionT1 = data.DD2Event_DownedInvasionT1;
					DD2Event.DownedInvasionT2 = data.DD2Event_DownedInvasionT2;
					DD2Event.DownedInvasionT3 = data.DD2Event_DownedInvasionT3;
					OneBiome.HaveDungeon = data.HaveDungeon;
					OneBiome.HaveShimmer = data.HaveShimmer;
					OneBiome.HaveTemple = data.HaveTemple;
					if (data.Bestiary_Kills != null) typeof(NPCKillsTracker).GetField("_killCountsByNpcId", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Main.BestiaryTracker.Kills, data.Bestiary_Kills);
					if (data.Bestiary_wasSeenNearPlayerByNetId != null) typeof(NPCWasNearPlayerTracker).GetField("_wasSeenNearPlayerByNetId", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Main.BestiaryTracker.Sights, data.Bestiary_wasSeenNearPlayerByNetId);
					if (data.Bestiary_chattedWithPlayer != null) typeof(NPCWasChatWithTracker).GetField("_chattedWithPlayer", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Main.BestiaryTracker.Chats, data.Bestiary_chattedWithPlayer);
				}
			}
		}

		public override void PostUpdateWorld()
		{
			if (MultiWorldFileData.IsMultiWorld(Main.ActiveWorldFileData.Path)) {
				if (load)
				{
					MultiWorldLoad();
					load = false;
				}
				if (WorldUpdateEvent.Keys.Count > 0)
				{
					if (WorldUpdateEvent.ContainsKey(1))
					{
						WorldGen.StartHardmode();
						WorldUpdateEvent.Remove(1);
					}
					if (WorldUpdateEvent.TryGetValue(2, out object value) && Main.hardMode)
					{
						var num = (int)value;
						for (var i = 0; i < num; i++)
						{
							SmashAltar();
						}
						WorldUpdateEvent.Remove(2);
					}
					if (WorldUpdateEvent.ContainsKey(3) && Main.hardMode)
					{
						WorldGen.GeneratePlanteraBulbOnAllMechsDefeated();
						WorldUpdateEvent.Remove(3);
					}
				}
			}
		}

		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
		{
			
			if (OneBiome.Biome != string.Empty)
			{
				OneBiome.Reset();
				tasks = OneBiome.GenWorld(tasks, ref totalWeight);
			}
		}

		public override void ModifyHardmodeTasks(List<GenPass> tasks)
		{
			if (OneBiome.Biome != string.Empty) {
				OneBiome.GenHardMode(tasks);
			}
		}

		public void ChangeWorld()
		{
			do_chaneWorld = true;
			var directory = Path.GetDirectoryName(Main.ActiveWorldFileData.Path);
			var data = MultiWorldFileData.LoadMeta(Path.Combine(directory, "meta.world"));
			var config = ModContent.GetInstance<Beta>();
			if (!config.SepecialWorld) {
				Radius = data.WorldRadius;
				if (Radius > 0)
				{
					if (Math.Abs(NextWorldIndex) > Radius)
					{
						if (NextWorldIndex > 0)
						{
							NextWorldIndex = -Radius;
						}
						else if (NextWorldIndex < 0)
						{
							NextWorldIndex = Radius;
						}
					}
				}
			}
			var nextWorld = Main.ActiveWorldFileData.Path.Replace(CurrentWorldIndex.ToString(), NextWorldIndex.ToString());
			if (!File.Exists(nextWorld))
			{
				do_worldGen = true;
			}
			MultiWorldSave();
			WorldGen.SaveAndQuit(GenWolrd);
		}

		public void GenWolrd()
		{
			var nextWorld = Main.ActiveWorldFileData.Path.Replace(CurrentWorldIndex.ToString(), NextWorldIndex.ToString());
			if (do_worldGen)
			{
				var directory = Path.GetDirectoryName(Main.ActiveWorldFileData.Path);
				var data = MultiWorldFileData.LoadMeta(Path.Combine(directory, "meta.world"));
				data = ProcessSeed(out var processedSeed, data);
				processedSeed = (processedSeed + nextWorld).Trim();
				switch (data.optionSize)
				{
					case WorldSizeId.Small:
						Main.maxTilesX = 4200;
						Main.maxTilesY = 1200;
						break;
					case WorldSizeId.Medium:
						Main.maxTilesX = 6400;
						Main.maxTilesY = 1800;
						break;
					case WorldSizeId.Large:
						Main.maxTilesX = 8400;
						Main.maxTilesY = 2400;
						break;
				}

				WorldGen.setWorldSize();
				switch (data.optionDifficulty)
				{
					case WorldDifficultyId.Creative:
						Main.GameMode = 3;
						break;
					case WorldDifficultyId.Normal:
						Main.GameMode = 0;
						break;
					case WorldDifficultyId.Expert:
						Main.GameMode = 1;
						break;
					case WorldDifficultyId.Master:
						Main.GameMode = 2;
						break;
				}

				switch (data.optionEvil)
				{
					case WorldEvilId.Random:
						WorldGen.WorldGenParam_Evil = Main.rand.Next(2);
						break;
					case WorldEvilId.Corruption:
						WorldGen.WorldGenParam_Evil = 0;
						break;
					case WorldEvilId.Crimson:
						WorldGen.WorldGenParam_Evil = 1;
						break;
				}
				Main.ActiveWorldFileData = CreateMetadata(data.optionwWorldName, nextWorld, SocialAPI.Cloud != null && SocialAPI.Cloud.EnabledByDefault, Main.GameMode);
				if (processedSeed.Length == 0)
					Main.ActiveWorldFileData.SetSeedToRandom();
				else
					Main.ActiveWorldFileData.SetSeed(processedSeed);
				WorldGen.CreateNewWorld();
			}
			else {
				Main.ActiveWorldFileData = WorldFile.GetAllMetadata(nextWorld, SocialAPI.Cloud != null && SocialAPI.Cloud.EnabledByDefault);
				WorldGen.playWorld();
			}
		}

		private static MetaData ProcessSeed(out string processedSeed,MetaData data)
		{
			processedSeed = data.optionSeed;
			UIWorldCreation.ProcessSpecialWorldSeeds(processedSeed);
			string[] array = data.optionSeed.Split('.', StringSplitOptions.None);
			if (array.Length != 4)
			{
				return data;
			}
			if (int.TryParse(array[0], out int result))
			{
				switch (result)
				{
					case 1:
						data.optionSize = WorldSizeId.Small;
						break;
					case 2:
						data.optionSize = WorldSizeId.Medium;
						break;
					case 3:
						data.optionSize = WorldSizeId.Large;
						break;
				}
			}
			if (int.TryParse(array[1], out result))
			{
				switch (result)
				{
					case 1:
						data.optionDifficulty = WorldDifficultyId.Normal;
						break;
					case 2:
						data.optionDifficulty = WorldDifficultyId.Expert;
						break;
					case 3:
						data.optionDifficulty = WorldDifficultyId.Master;
						break;
					case 4:
						data.optionDifficulty = WorldDifficultyId.Creative;
						break;
				}
			}
			if (int.TryParse(array[2], out result))
			{
				if (result != 1)
				{
					if (result == 2)
					{
						data.optionEvil = WorldEvilId.Crimson;
					}
				}
				else
				{
					data.optionEvil = WorldEvilId.Corruption;
				}
			}
			processedSeed = array[3];
			return data;
		}

		private static WorldFileData CreateMetadata(string name ,string path, bool cloudSave, int GameMode)
		{
			WorldFileData worldFileData = new(path, cloudSave);
			if (Main.autoGenFileLocation != null && Main.autoGenFileLocation != "")
			{
				worldFileData = new WorldFileData(Main.autoGenFileLocation, cloudSave);
				Main.autoGenFileLocation = null;
			}
			Main.worldName = name;
			worldFileData.Name = name;
			worldFileData.GameMode = GameMode;
			worldFileData.CreationTime = DateTime.Now;
			worldFileData.Metadata = FileMetadata.FromCurrentSettings(FileType.World);
			worldFileData.SetFavorite(favorite: false);
			worldFileData.WorldGeneratorVersion = 1198295875585uL;
			worldFileData.UniqueId = Guid.NewGuid();
			worldFileData.SetWorldSize(Main.maxTilesX , Main.maxTilesY );
			if (Main.DefaultSeed == "")
				worldFileData.SetSeedToRandom();
			else
				worldFileData.SetSeed(Main.DefaultSeed);
			return worldFileData;
		}

		public static void SmashAltar()
		{
			if (Main.netMode == NetmodeID.MultiplayerClient || !Main.hardMode || WorldGen.noTileActions || WorldGen.gen)
			{
				return;
			}
			int num = WorldGen.altarCount % 3;
			int num2 = WorldGen.altarCount / 3 + 1;
			double num3 = (double)Main.maxTilesX / 4200.0;
			int num4 = 1 - num;
			num3 = num3 * 310.0 - (double)(85 * num);
			num3 *= 0.85;
			num3 /= (double)num2;
			bool flag = false;
			if (Main.drunkWorld)
			{
				if (WorldGen.SavedOreTiers.Adamantite == 111)
				{
					WorldGen.SavedOreTiers.Adamantite = 223;
				}
				else if (WorldGen.SavedOreTiers.Adamantite == 223)
				{
					WorldGen.SavedOreTiers.Adamantite = 111;
				}
			}
			if (num != 0)
			{
				if (num != 1)
				{
					if (Main.drunkWorld)
					{
						if (WorldGen.SavedOreTiers.Cobalt == 107)
						{
							WorldGen.SavedOreTiers.Cobalt = 221;
						}
						else if (WorldGen.SavedOreTiers.Cobalt == 221)
						{
							WorldGen.SavedOreTiers.Cobalt = 107;
						}
					}
					if (WorldGen.SavedOreTiers.Adamantite == -1)
					{
						flag = true;
						WorldGen.SavedOreTiers.Adamantite = 111;
						if (WorldGen.genRand.NextBool(2))
						{
							WorldGen.SavedOreTiers.Adamantite = 223;
						}
					}
					int num5 = 14;
					if (WorldGen.SavedOreTiers.Adamantite == 223)
					{
						num5 += 9;
						num3 *= 0.8999999761581421;
					}
					if (Main.netMode == NetmodeID.SinglePlayer)
					{
						Main.NewText(Lang.misc[num5].Value, 50, byte.MaxValue, 130);
					}
					else if (Main.netMode == NetmodeID.Server)
					{
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[num5].Key, []), new Color(50, 255, 130), -1);
					}
					num = WorldGen.SavedOreTiers.Adamantite;
				}
				else
				{
					if (Main.drunkWorld)
					{
						if (WorldGen.SavedOreTiers.Mythril == 108)
						{
							WorldGen.SavedOreTiers.Mythril = 222;
						}
						else if (WorldGen.SavedOreTiers.Mythril == 222)
						{
							WorldGen.SavedOreTiers.Mythril = 108;
						}
					}
					if (WorldGen.SavedOreTiers.Mythril == -1)
					{
						flag = true;
						WorldGen.SavedOreTiers.Mythril = 108;
						if (WorldGen.genRand.NextBool(2))
						{
							WorldGen.SavedOreTiers.Mythril = 222;
						}
					}
					int num6 = 13;
					if (WorldGen.SavedOreTiers.Mythril == 222)
					{
						num6 += 9;
						num3 *= 0.8999999761581421;
					}
					if (Main.netMode == NetmodeID.SinglePlayer)
					{
						Main.NewText(Lang.misc[num6].Value, 50, byte.MaxValue, 130);
					}
					else if (Main.netMode == NetmodeID.Server)
					{
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[num6].Key, []), new Color(50, 255, 130), -1);
					}
					num = WorldGen.SavedOreTiers.Mythril;
				}
			}
			else
			{
				if (WorldGen.SavedOreTiers.Cobalt == -1)
				{
					flag = true;
					WorldGen.SavedOreTiers.Cobalt = 107;
					if (WorldGen.genRand.NextBool(2))
					{
						WorldGen.SavedOreTiers.Cobalt = 221;
					}
				}
				int num7 = 12;
				if (WorldGen.SavedOreTiers.Cobalt == 221)
				{
					num7 += 9;
					num3 *= 0.8999999761581421;
				}
				if (Main.netMode == NetmodeID.SinglePlayer)
				{
					Main.NewText(Lang.misc[num7].Value, 50, byte.MaxValue, 130);
				}
				else if (Main.netMode == NetmodeID.Server)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[num7].Key, []), new Color(50, 255, 130), -1);
				}
				num = WorldGen.SavedOreTiers.Cobalt;
				num3 *= 1.0499999523162842;
			}
			if (flag)
			{
				NetMessage.SendData(MessageID.WorldData, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
			}
			int k = 0;
			while ((double)k < num3)
			{
				int i2 = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
				double num8 = Main.worldSurface;
				if (num == 108 || num == 222)
				{
					num8 = Main.rockLayer;
				}
				if (num == 111 || num == 223)
				{
					num8 = (Main.rockLayer + Main.rockLayer + (double)Main.maxTilesY) / 3.0;
				}
				int j2 = WorldGen.genRand.Next((int)num8, Main.maxTilesY - 150);
				if (Main.remixWorld)
				{
					double num9 = (double)(Main.maxTilesX - 350);
					if (num == 108 || num == 222)
					{
						num9 = (Main.rockLayer + Main.rockLayer + (double)Main.maxTilesY - 350.0) / 3.0;
					}
					if (num == 111 || num == 223)
					{
						num9 = Main.rockLayer - 25.0;
					}
					j2 = WorldGen.genRand.Next((int)Main.worldSurface + 15, (int)num9);
				}
				if (Main.tenthAnniversaryWorld)
				{
					WorldGen.OreRunner(i2, j2, (double)WorldGen.genRand.Next(5, 11 + num4), WorldGen.genRand.Next(5, 11 + num4), (ushort)num);
				}
				else
				{
					WorldGen.OreRunner(i2, j2, (double)WorldGen.genRand.Next(5, 9 + num4), WorldGen.genRand.Next(5, 9 + num4), (ushort)num);
				}
				k++;
			}
			WorldGen.altarCount ++;
		}
	}
}
