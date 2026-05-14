using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MultiWorld.Common.Types
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

    [Flags]
    public enum NPCFlags : ulong
    {
        none = 0,
        downedBoss1 = 1UL << 0,
        downedBoss2 = 1UL << 1,
        downedBoss3 = 1UL << 2,
        downedQueenBee = 1UL << 3,
        downedMechBoss1 = 1UL << 4,
        downedMechBoss2 = 1UL << 5,
        downedMechBoss3 = 1UL << 6,
        downedMechBossAny = 1UL << 7,
        downedPlantBoss = 1UL << 8,
        downedGolemBoss = 1UL << 9,
        downedSlimeKing = 1UL << 10,
        savedGoblin = 1UL << 11,
        savedWizard = 1UL << 12,
        savedMech = 1UL << 13,
        downedGoblins = 1UL << 14,
        downedClown = 1UL << 15,
        downedFrost = 1UL << 16,
        downedPirates  = 1UL << 17,
        downedFishron = 1UL << 18,
        downedMartians = 1UL << 19,
        downedAncientCultist = 1UL << 20,
        downedMoonlord = 1UL << 21,
        downedHalloweenKing = 1UL << 22,
        downedHalloweenTree = 1UL << 23,
        downedChristmasIceQueen = 1UL << 24,
        downedChristmasSantank = 1UL << 25,
        downedChristmasTree = 1UL << 26,
        downedTowerSolar = 1UL << 27,
        downedTowerVortex = 1UL << 28,
        downedTowerNebula = 1UL << 29,
        downedTowerStardust = 1UL << 30,
        downedEmpressOfLight = 1UL << 31,
        downedQueenSlime = 1UL << 32,
        downedDeerclops = 1UL << 33,
        boughtCat = 1UL << 34,
        boughtDog = 1UL << 35,
        boughtBunny = 1UL << 36,
        unlockedSlimeBlueSpawn = 1UL << 37,
        unlockedMerchantSpawn = 1UL << 38,
        unlockedDemolitionistSpawn = 1UL << 39,
        unlockedPartyGirlSpawn = 1UL << 40,
        unlockedDyeTraderSpawn  = 1UL << 41,
        unlockedTruffleSpawn  = 1UL << 42,
        unlockedArmsDealerSpawn = 1UL << 43,
        unlockedNurseSpawn = 1UL << 44,
        unlockedPrincessSpawn = 1UL << 45,
        unlockedSlimeGreenSpawn = 1UL << 46,
        unlockedSlimeOldSpawn = 1UL << 47,
        unlockedSlimePurpleSpawn = 1UL << 48,
        unlockedSlimeRainbowSpawn = 1UL << 49,
        unlockedSlimeRedSpawn = 1UL << 50,
        unlockedSlimeYellowSpawn = 1UL << 51,
        unlockedSlimeCopperSpawn = 1UL << 52,
        savedAngler = 1UL << 53,
        savedStylist = 1UL << 54,
        savedTaxCollector = 1UL << 55,
        savedGolfer = 1UL << 56,
        combatBookWasUsed = 1UL << 57,
        combatBookVolumeTwoWasUsed = 1UL << 58,
        peddlersSatchelWasUsed = 1UL << 59,
        savedBartender = 1UL << 60,

    }

    public class MetaData
    {
        public string optionSeed = string.Empty;
        public WorldSizeId optionSize;
        public WorldDifficultyId optionDifficulty;
        public WorldEvilId optionEvil;
        public string optionwWorldName = string.Empty;
        public GenMode GenMode;
        public Dictionary<long, int> spawnPoint = new() {
            { 0,0 }
        };
        public int WorldRadius = 0;
        public NPCFlags npcFlags;
        [JsonIgnore]
        public bool NPC_downedBoss1
        {
            get => GetFlag(NPCFlags.downedBoss1);
            set => SetFlag(NPCFlags.downedBoss1, value);
        }
        [JsonIgnore]
        public bool NPC_downedBoss2
        {
            get => GetFlag(NPCFlags.downedBoss2);
            set => SetFlag(NPCFlags.downedBoss2, value);
        }
        [JsonIgnore]
        public bool NPC_downedBoss3
        {
            get => GetFlag(NPCFlags.downedBoss3);
            set => SetFlag(NPCFlags.downedBoss3, value);
        }
        [JsonIgnore]
        public bool NPC_downedQueenBee
        {
            get => GetFlag(NPCFlags.downedQueenBee);
            set => SetFlag(NPCFlags.downedQueenBee, value);
        }
        [JsonIgnore]
        public bool NPC_downedMechBoss1
        {
            get => GetFlag(NPCFlags.downedMechBoss1);
            set => SetFlag(NPCFlags.downedMechBoss1, value);
        }
        [JsonIgnore]
        public bool NPC_downedMechBoss2
        {
            get => GetFlag(NPCFlags.downedMechBoss2);
            set => SetFlag(NPCFlags.downedMechBoss2, value);
        }
        [JsonIgnore]
        public bool NPC_downedMechBoss3
        {
            get => GetFlag(NPCFlags.downedMechBoss3);
            set => SetFlag(NPCFlags.downedMechBoss3, value);
        }
        [JsonIgnore]
        public bool NPC_downedMechBossAny
        {
            get => GetFlag(NPCFlags.downedMechBossAny);
            set => SetFlag(NPCFlags.downedMechBossAny, value);
        }
        [JsonIgnore]
        public bool NPC_downedPlantBoss
        {
            get => GetFlag(NPCFlags.downedPlantBoss);
            set => SetFlag(NPCFlags.downedPlantBoss, value);
        }
        [JsonIgnore]
        public bool NPC_downedGolemBoss
        {
            get => GetFlag(NPCFlags.downedGolemBoss);
            set => SetFlag(NPCFlags.downedGolemBoss, value);
        }
        [JsonIgnore]
        public bool NPC_downedSlimeKing
        {
            get => GetFlag(NPCFlags.downedSlimeKing);
            set => SetFlag(NPCFlags.downedSlimeKing, value);
        }
        [JsonIgnore]
        public bool NPC_savedGoblin
        {
            get => GetFlag(NPCFlags.savedGoblin);
            set => SetFlag(NPCFlags.savedGoblin, value);
        }
        [JsonIgnore]
        public bool NPC_savedWizard
        {
            get => GetFlag(NPCFlags.savedWizard);
            set => SetFlag(NPCFlags.savedWizard, value);
        }
        [JsonIgnore]
        public bool NPC_savedMech
        {
            get => GetFlag(NPCFlags.savedMech);
            set => SetFlag(NPCFlags.savedMech, value);
        }
        [JsonIgnore]
        public bool NPC_downedGoblins
        {
            get => GetFlag(NPCFlags.downedGoblins);
            set => SetFlag(NPCFlags.downedGoblins, value);
        }
        [JsonIgnore]
        public bool NPC_downedClown
        {
            get => GetFlag(NPCFlags.downedClown);
            set => SetFlag(NPCFlags.downedClown, value);
        }
        [JsonIgnore]
        public bool NPC_downedFrost
        {
            get => GetFlag(NPCFlags.downedFrost);
            set => SetFlag(NPCFlags.downedFrost, value);
        }
        [JsonIgnore]
        public bool NPC_downedPirates 
        { 
            get => GetFlag(NPCFlags.downedPirates);
            set => SetFlag(NPCFlags.downedPirates, value);
        }
        [JsonIgnore]
        public bool NPC_downedFishron
        {
            get => GetFlag(NPCFlags.downedFishron);
            set => SetFlag(NPCFlags.downedFishron, value);
        }
        [JsonIgnore]
        public bool NPC_downedMartians
        {
            get => GetFlag(NPCFlags.downedMartians);
            set => SetFlag(NPCFlags.downedMartians, value);
        }
        [JsonIgnore]
        public bool NPC_downedAncientCultist
        {
            get => GetFlag(NPCFlags.downedAncientCultist);
            set => SetFlag(NPCFlags.downedAncientCultist, value);
        }
        [JsonIgnore]
        public bool NPC_downedMoonlord
        {
            get => GetFlag(NPCFlags.downedMoonlord);
            set => SetFlag(NPCFlags.downedMoonlord, value);
        }
        [JsonIgnore]
        public bool NPC_downedHalloweenKing
        {
            get => GetFlag(NPCFlags.downedHalloweenKing);
            set => SetFlag(NPCFlags.downedHalloweenKing, value);
        }
        [JsonIgnore]
        public bool NPC_downedHalloweenTree
        {
            get => GetFlag(NPCFlags.downedHalloweenTree);
            set => SetFlag(NPCFlags.downedHalloweenTree, value);
        }
        [JsonIgnore]
        public bool NPC_downedChristmasIceQueen
        {
            get => GetFlag(NPCFlags.downedChristmasIceQueen);
            set => SetFlag(NPCFlags.downedChristmasIceQueen, value);
        }
        [JsonIgnore]
        public bool NPC_downedChristmasSantank
        {
            get => GetFlag(NPCFlags.downedChristmasSantank);
            set => SetFlag(NPCFlags.downedChristmasSantank, value);
        }
        [JsonIgnore]
        public bool NPC_downedChristmasTree
        {
            get => GetFlag(NPCFlags.downedChristmasTree);
            set => SetFlag(NPCFlags.downedChristmasTree, value);
        }
        [JsonIgnore]
        public bool NPC_downedTowerSolar
        {
            get => GetFlag(NPCFlags.downedTowerSolar);
            set => SetFlag(NPCFlags.downedTowerSolar, value);
        }
        [JsonIgnore]
        public bool NPC_downedTowerVortex
        {
            get => GetFlag(NPCFlags.downedTowerVortex);
            set => SetFlag(NPCFlags.downedTowerVortex, value);
        }
        [JsonIgnore]
        public bool NPC_downedTowerNebula
        {
            get => GetFlag(NPCFlags.downedTowerNebula);
            set => SetFlag(NPCFlags.downedTowerNebula, value);
        }
        [JsonIgnore]
        public bool NPC_downedTowerStardust
        {
            get => GetFlag(NPCFlags.downedTowerStardust);
            set => SetFlag(NPCFlags.downedTowerStardust, value);
        }
        [JsonIgnore]
        public bool NPC_downedEmpressOfLight
        {
            get => GetFlag(NPCFlags.downedEmpressOfLight);
            set => SetFlag(NPCFlags.downedEmpressOfLight, value);
        }
        [JsonIgnore]
        public bool NPC_downedQueenSlime
        {
            get => GetFlag(NPCFlags.downedQueenSlime);
            set => SetFlag(NPCFlags.downedQueenSlime, value);
        }
        [JsonIgnore]
        public bool NPC_downedDeerclops
        {
            get => GetFlag(NPCFlags.downedDeerclops);
            set => SetFlag(NPCFlags.downedDeerclops, value);
        }
        [JsonIgnore]
        public bool NPC_boughtCat
        {
            get => GetFlag(NPCFlags.boughtCat);
            set => SetFlag(NPCFlags.boughtCat, value);
        }
        [JsonIgnore]
        public bool NPC_boughtDog
        {
            get => GetFlag(NPCFlags.boughtDog);
            set => SetFlag(NPCFlags.boughtDog, value);
        }
        [JsonIgnore]
        public bool NPC_boughtBunny
        {
            get => GetFlag(NPCFlags.boughtBunny);
            set => SetFlag(NPCFlags.boughtBunny, value);
        }
        [JsonIgnore]
        public bool NPC_unlockedSlimeBlueSpawn
        {
            get => GetFlag(NPCFlags.unlockedSlimeBlueSpawn);
            set => SetFlag(NPCFlags.unlockedSlimeBlueSpawn, value);
        }
        [JsonIgnore]
        public bool NPC_unlockedMerchantSpawn
        {
            get => GetFlag(NPCFlags.unlockedMerchantSpawn);
            set => SetFlag(NPCFlags.unlockedMerchantSpawn, value);
        }
        [JsonIgnore]
        public bool NPC_unlockedDemolitionistSpawn
        {
            get => GetFlag(NPCFlags.unlockedDemolitionistSpawn);
            set => SetFlag(NPCFlags.unlockedDemolitionistSpawn, value);
        }
        [JsonIgnore]
        public bool NPC_unlockedPartyGirlSpawn
        {
            get => GetFlag(NPCFlags.unlockedPartyGirlSpawn);
            set => SetFlag(NPCFlags.unlockedPartyGirlSpawn, value);
        }
        [JsonIgnore]
        public bool NPC_unlockedDyeTraderSpawn
        {
            get => GetFlag(NPCFlags.unlockedDyeTraderSpawn);
            set => SetFlag(NPCFlags.unlockedDyeTraderSpawn, value);
        }
        [JsonIgnore]
        public bool NPC_unlockedTruffleSpawn
        {
            get => GetFlag(NPCFlags.unlockedTruffleSpawn);
            set => SetFlag(NPCFlags.unlockedTruffleSpawn, value);
        }
        [JsonIgnore]
        public bool NPC_unlockedArmsDealerSpawn
        {
            get => GetFlag(NPCFlags.unlockedArmsDealerSpawn);
            set => SetFlag(NPCFlags.unlockedArmsDealerSpawn, value);
        }
        [JsonIgnore]
        public bool NPC_unlockedNurseSpawn
        {
            get => GetFlag(NPCFlags.unlockedNurseSpawn);
            set => SetFlag(NPCFlags.unlockedNurseSpawn, value);
        }
        [JsonIgnore]
        public bool NPC_unlockedPrincessSpawn
        {
            get => GetFlag(NPCFlags.unlockedPrincessSpawn);
            set => SetFlag(NPCFlags.unlockedPrincessSpawn, value);
        }
        [JsonIgnore]
        public bool NPC_unlockedSlimeGreenSpawn
        {
            get => GetFlag(NPCFlags.unlockedSlimeGreenSpawn);
            set => SetFlag(NPCFlags.unlockedSlimeGreenSpawn, value);
        }
        [JsonIgnore]
        public bool NPC_unlockedSlimeOldSpawn
        {
            get => GetFlag(NPCFlags.unlockedSlimeOldSpawn);
            set => SetFlag(NPCFlags.unlockedSlimeOldSpawn, value);
        }
        [JsonIgnore]
        public bool NPC_unlockedSlimePurpleSpawn
        {
            get => GetFlag(NPCFlags.unlockedSlimePurpleSpawn);
            set => SetFlag(NPCFlags.unlockedSlimePurpleSpawn, value);
        }
        [JsonIgnore]
        public bool NPC_unlockedSlimeRainbowSpawn
        {
            get => GetFlag(NPCFlags.unlockedSlimeRainbowSpawn);
            set => SetFlag(NPCFlags.unlockedSlimeRainbowSpawn, value);
        }
        [JsonIgnore]
        public bool NPC_unlockedSlimeRedSpawn
        {
            get => GetFlag(NPCFlags.unlockedSlimeRedSpawn);
            set => SetFlag(NPCFlags.unlockedSlimeRedSpawn, value);
        }
        [JsonIgnore]
        public bool NPC_unlockedSlimeYellowSpawn
        {
            get => GetFlag(NPCFlags.unlockedSlimeYellowSpawn);
            set => SetFlag(NPCFlags.unlockedSlimeYellowSpawn, value);
        }
        [JsonIgnore]
        public bool NPC_unlockedSlimeCopperSpawn
        {
            get => GetFlag(NPCFlags.unlockedSlimeCopperSpawn);
            set => SetFlag(NPCFlags.unlockedSlimeCopperSpawn, value);
        }
        [JsonIgnore]
        public bool NPC_savedAngler
        {
            get => GetFlag(NPCFlags.savedAngler);
            set => SetFlag(NPCFlags.savedAngler, value);
        }
        [JsonIgnore]
        public bool NPC_savedStylist
        {
            get => GetFlag(NPCFlags.savedStylist);
            set => SetFlag(NPCFlags.savedStylist, value);
        }
        [JsonIgnore]
        public bool NPC_savedTaxCollector
        {
            get => GetFlag(NPCFlags.savedTaxCollector);
            set => SetFlag(NPCFlags.savedTaxCollector, value);
        }
        [JsonIgnore]
        public bool NPC_savedGolfer
        {
            get => GetFlag(NPCFlags.savedGolfer);
            set => SetFlag(NPCFlags.savedGolfer, value);
        }
        [JsonIgnore]
        public bool NPC_combatBookWasUsed
        {
            get => GetFlag(NPCFlags.combatBookWasUsed);
            set => SetFlag(NPCFlags.combatBookWasUsed, value);
        }
        [JsonIgnore]
        public bool NPC_combatBookVolumeTwoWasUsed
        {
            get => GetFlag(NPCFlags.combatBookVolumeTwoWasUsed);
            set => SetFlag(NPCFlags.combatBookVolumeTwoWasUsed, value);
        }
        [JsonIgnore]
        public bool NPC_peddlersSatchelWasUsed
        {
            get => GetFlag(NPCFlags.peddlersSatchelWasUsed);
            set => SetFlag(NPCFlags.peddlersSatchelWasUsed, value);
        }
        [JsonIgnore]
        public bool NPC_savedBartender
        {
            get => GetFlag(NPCFlags.savedBartender);
            set => SetFlag(NPCFlags.savedBartender, value);
        }
        public int[] NPC_killCount = [];
        public bool[] NPC_hasRoom = [];
        public bool[] NPC_ShimmeredTownNPCs = [];
        public bool WorldGen_shadowOrbSmashed;
        public bool WorldGen_spawnMeteor;
        public int? WorldGen_shadowOrbCount;
        public int? WorldGen_altarCount;
        public int? WorldGen_treeBG1;
        public int? WorldGen_treeBG2;
        public int? WorldGen_treeBG3;
        public int? WorldGen_treeBG4;
        public int? WorldGen_corruptBG;
        public int? WorldGen_jungleBG;
        public int? WorldGen_snowBG;
        public int? WorldGen_hallowBG;
        public int? WorldGen_crimsonBG;
        public int? WorldGen_desertBG;
        public int? WorldGen_oceanBG;
        public int? WorldGen_mushroomBG;
        public int? WorldGen_underworldBG;
        public bool Main_hardMode;
        public bool Main_afterPartyOfDoom;
        public int? Main_invasionDelay;
        public int? Main_invasionSize;
        public int? Main_invasionType;
        public double? Main_invasionX;
        public double? Main_slimeRainTime;
        public int? Main_sundialCooldown;
        public bool Main_raining;
        public double? Main_rainTime;
        public int? Main_maxRain;
        public float? Main_cloudBGActive;
        public float? Main_cloudBGAlpha;
        public int? Main_numClouds;
        public float? Main_windSpeedTarget;
        public float? Main_windSpeedCurrent;
        public List<string> Main_anglerWhoFinishedToday = [];
        public int? Main_anglerQuest;
        public int? Main_invasionSizeStart;
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
        public int? Main_moondialCooldown;
        public int? Main_moonType;
        public bool Main_eclipse;
        public double? Main_time;
        public bool Main_dayTime;
        public int? Main_moonPhase;
        public bool Main_bloodMoon;
        public double? CultistRitual_delay;
        public bool BirthdayParty_ManualParty;
        public bool BirthdayParty_GenuineParty;
        public int? BirthdayParty_PartyDaysOnCooldown;
        public List<int> BirthdayParty_CelebratingNPCs = [];
        public bool Sandstorm_Happening;
        public double? Sandstorm_TimeLeft;
        public float? Sandstorm_Severity;
        public float? Sandstorm_IntendedSeverity;
        public int? LanternNight_LanternNightsOnCooldown;
        public bool LanternNight_GenuineLanterns;
        public bool LanternNight_ManualLanterns;
        public bool LanternNight_NextNightIsLanternNight;
        public bool DD2Event_DownedInvasionT1;
        public bool DD2Event_DownedInvasionT2;
        public bool DD2Event_DownedInvasionT3;
        public Dictionary<string, int> Bestiary_Kills = [];
        public List<int> Bestiary_wasSeenNearPlayerByNetId = [];
        public HashSet<string> Bestiary_chattedWithPlayer = [];
        public string SaveFrom = string.Empty;
        public bool HaveDungeon = false;
        public bool HaveTemple = false;
        public bool HaveShimmer = false;
        private static readonly Dictionary<string, int> DefaultBiomesChance = new()
        {
            { "Forest",1 }, { "Desert",1 }, { "Jungle",1 },
            { "Corruption",1 }, { "Snow",1 }, { "Ocean",1 },
        };

        private static readonly Dictionary<string, int> DefaultHardmodeChance = new()
        {
            { "Hallow",1 }, { "Evil",1 }, { "NoneGen",1 },
        };

        private static readonly Dictionary<string, int> DefaultStructureChance = new()
        {
            { "Dungeon",1 }, { "Temple",1 }, { "Shimmer",1 }
        };
        public Dictionary<string, int> BiomesChance = new(DefaultBiomesChance);
        public Dictionary<string, int> HardmodeChance = new(DefaultHardmodeChance);
        public Dictionary<string, int> StructureChance = new(DefaultStructureChance);
        [JsonExtensionData]
        private Dictionary<string, JToken> _extensionData = [];
        [JsonIgnore]
        private Dictionary<string, object> _extensionCache = [];

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            NPC_killCount ??= [];
            NPC_hasRoom ??= [];
            NPC_ShimmeredTownNPCs ??= [];

            Main_anglerWhoFinishedToday ??= [];
            BirthdayParty_CelebratingNPCs ??= [];

            Bestiary_Kills ??= [];
            Bestiary_wasSeenNearPlayerByNetId ??= [];
            Bestiary_chattedWithPlayer ??= [];

            BiomesChance ??= new(DefaultBiomesChance);
            HardmodeChance ??= new(DefaultHardmodeChance);
            StructureChance ??= new(DefaultStructureChance);

            _extensionData ??= [];
            _extensionCache ??= [];
        }

        private bool GetFlag(NPCFlags flag) => (npcFlags & flag) != 0;

        private void SetFlag(NPCFlags flag, bool value)
        {
            if (value)
                npcFlags |= flag;
            else
                npcFlags &= ~flag;
        }

        public void SetExtensionData<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            if (value == null)
            {
                _extensionData.Remove(key);
                _extensionCache.Remove(key);
                return;
            }

            _extensionData[key] = value is JToken jt ? jt : JToken.FromObject(value);
            _extensionCache.Remove(key);
        }
        public T GetExtensionData<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }


            if (_extensionCache.TryGetValue(key, out var cached))
            {
                if (cached is T t)
                {
                    return t;
                }
            }

            if (_extensionData.TryGetValue(key, out var token))
            {
                try
                {
                    var value = token.ToObject<T>();
                    _extensionCache[key] = value!;
                    return value;
                }
                catch
                {

                }
            }

            return default;
        }

        public bool TryGetExtensionData<T>(string key, out T value)
        {
            if (string.IsNullOrEmpty(key))
            {
                value = default;
                return false;
            }


            if (_extensionCache.TryGetValue(key, out var cached))
            {
                if (cached is T t)
                {
                    value = t;
                    return true;
                }
            }

            if (_extensionData.TryGetValue(key, out var token))
            {
                try
                {
                    value = token.ToObject<T>();
                    _extensionCache[key] = value!;
                    return true;
                }
                catch
                {
                  
                }
            }

            value = default;
            return false;
        }

        public JToken GetRawExtensionData(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            return _extensionData.TryGetValue(key, out var token) ? token : null;
        }

        public string GetModeName()
        {
            return GenMode switch
            {
                GenMode.Off => "",
                GenMode.Normal => "Normal",
                GenMode.Special => "Special",
                GenMode.RandomMod => "Random Mod",
                _ => "Unknown"
            };

        }
    }
}
