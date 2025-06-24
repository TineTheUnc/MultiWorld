using Newtonsoft.Json;
using System.Collections.Generic;

namespace MultiWorld.Common.Type
{
	public enum WorldSizeId
	{
		Small,
		Medium,
		Large
	}

	public enum WorldDifficultyId
	{
		Normal,
		Expert,
		Master,
		Creative
	}

	public enum WorldEvilId
	{
		Random,
		Corruption,
		Crimson
	}

	[JsonObject(MemberSerialization.Fields)]
	public class MetaData 
	{
		public string optionSeed;
		public WorldSizeId optionSize;
		public WorldDifficultyId optionDifficulty;
		public WorldEvilId optionEvil;
		public string optionwWorldName;
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public Dictionary<long, int> spawnPoint = [];
		public int WorldRadius;
		public bool NPC_downedBoss1;
		public bool NPC_downedBoss2;
		public bool NPC_downedBoss3;
		public bool NPC_downedQueenBee;
		public bool NPC_downedMechBoss1;
		public bool NPC_downedMechBoss2;
		public bool NPC_downedMechBoss3;
		public bool NPC_downedMechBossAny;
		public bool NPC_downedPlantBoss;
		public bool NPC_downedGolemBoss;
		public bool NPC_downedSlimeKing;
		public bool NPC_savedGoblin;
		public bool NPC_savedWizard;
		public bool NPC_savedMech;
		public bool NPC_downedGoblins;
		public bool NPC_downedClown;
		public bool NPC_downedFrost;
		public bool NPC_downedPirates;
		public bool NPC_downedFishron;
		public bool NPC_downedMartians;
		public bool NPC_downedAncientCultist;
		public bool NPC_downedMoonlord;
		public bool NPC_downedHalloweenKing;
		public bool NPC_downedHalloweenTree;
		public bool NPC_downedChristmasIceQueen;
		public bool NPC_downedChristmasSantank;
		public bool NPC_downedChristmasTree;
		public bool NPC_downedTowerSolar;
		public bool NPC_downedTowerVortex;
		public bool NPC_downedTowerNebula;
		public bool NPC_downedTowerStardust;
		public bool NPC_downedEmpressOfLight;
		public bool NPC_downedQueenSlime;
		public bool NPC_downedDeerclops;
		public bool NPC_boughtCat;
		public bool NPC_boughtDog;
		public bool NPC_boughtBunny;
		public bool NPC_unlockedSlimeBlueSpawn;
		public bool NPC_unlockedMerchantSpawn;
		public bool NPC_unlockedDemolitionistSpawn;
		public bool NPC_unlockedPartyGirlSpawn;
		public bool NPC_unlockedDyeTraderSpawn;
		public bool NPC_unlockedTruffleSpawn;
		public bool NPC_unlockedArmsDealerSpawn;
		public bool NPC_unlockedNurseSpawn;
		public bool NPC_unlockedPrincessSpawn;
		public bool NPC_unlockedSlimeGreenSpawn;
		public bool NPC_unlockedSlimeOldSpawn;
		public bool NPC_unlockedSlimePurpleSpawn;
		public bool NPC_unlockedSlimeRainbowSpawn;
		public bool NPC_unlockedSlimeRedSpawn;
		public bool NPC_unlockedSlimeYellowSpawn;
		public bool NPC_unlockedSlimeCopperSpawn;
		public bool NPC_savedAngler;
		public bool NPC_savedStylist;
		public bool NPC_savedTaxCollector;
		public bool NPC_savedGolfer;
		public bool NPC_combatBookWasUsed;
		public bool NPC_combatBookVolumeTwoWasUsed;
		public bool NPC_peddlersSatchelWasUsed;
		public bool NPC_savedBartender;
		public int[] NPC_killCount = [];
		public bool[] NPC_hasRoom = [];
		public bool[] NPC_ShimmeredTownNPCs = [];
		public bool WorldGen_shadowOrbSmashed;
		public bool WorldGen_spawnMeteor;
		public int WorldGen_shadowOrbCount;
		public int WorldGen_altarCount;
		public int WorldGen_treeBG1;
		public int WorldGen_treeBG2;
		public int WorldGen_treeBG3;
		public int WorldGen_treeBG4;
		public int WorldGen_corruptBG;
		public int WorldGen_jungleBG;
		public int WorldGen_snowBG;
		public int WorldGen_hallowBG;
		public int WorldGen_crimsonBG;
		public int WorldGen_desertBG;
		public int WorldGen_oceanBG;
		public int WorldGen_mushroomBG;
		public int WorldGen_underworldBG;
		public bool Main_hardMode;
		public bool Main_afterPartyOfDoom;
		public int Main_invasionDelay;
		public int Main_invasionSize;
		public int Main_invasionType;
		public double Main_invasionX;
		public double Main_slimeRainTime;
		public int Main_sundialCooldown;
		public bool Main_raining;
		public double Main_rainTime;
		public int Main_maxRain;
		public float Main_cloudBGActive;
		public float Main_cloudBGAlpha;
		public int Main_numClouds;
		public float Main_windSpeedTarget;
		public float Main_windSpeedCurrent;
		public List<string> Main_anglerWhoFinishedToday = [];
		public int Main_anglerQuest;
		public int Main_invasionSizeStart;
		public bool Main_fastForwardTimeToDawn;
		public int[] Main_treeBGSet1 = [];
		public int[] Main_treeBGSet2 = [];
		public int[] Main_treeBGSet3 = [];
		public int[] Main_treeBGSet4 = [];
		public int[] Main_treeMntBGSet1 = [];
		public int[] Main_treeMntBGSet2 = [];
		public int[] Main_treeMntBGSet3 = [];
		public int[] Main_treeMntBGSet4 = [];
		public int[] Main_corruptBG = [];
		public int[] Main_jungleBG = [];
		public int[] Main_snowBG = [];
		public int[] Main_snowMntBG = [];
		public int[] Main_hallowBG = [];
		public int[] Main_crimsonBG = [];
		public int[] Main_desertBG = [];
		public int[] Main_mushroomBG = [];
		public int[] Main_underworldBG = [];
		public bool Main_forceHalloweenForToday;
		public bool Main_forceXMasForToday;
		public bool Main_fastForwardTimeToDusk;
		public int Main_moondialCooldown;
		public int Main_moonType;
		public bool Main_eclipse;
		public double Main_time;
		public bool Main_dayTime;
		public int Main_moonPhase;
		public bool Main_bloodMoon;
		public double CultistRitual_delay;
		public bool BirthdayParty_ManualParty;
		public bool BirthdayParty_GenuineParty;
		public int BirthdayParty_PartyDaysOnCooldown;
		public List<int> BirthdayParty_CelebratingNPCs = [];
		public bool Sandstorm_Happening;
		public double Sandstorm_TimeLeft;
		public float Sandstorm_Severity;
		public float Sandstorm_IntendedSeverity;
		public int LanternNight_LanternNightsOnCooldown;
		public bool LanternNight_GenuineLanterns;
		public bool LanternNight_ManualLanterns;
		public bool LanternNight_NextNightIsLanternNight;
		public bool DD2Event_DownedInvasionT1;
		public bool DD2Event_DownedInvasionT2;
		public bool DD2Event_DownedInvasionT3;
		public Dictionary<string, int> Bestiary_Kills = [];
		public List<int> Bestiary_wasSeenNearPlayerByNetId = [];
		public HashSet<string> Bestiary_chattedWithPlayer = [];
		public string SaveFrom;
	}
}
