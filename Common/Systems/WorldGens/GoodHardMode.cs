using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
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

                                if (Main.tile[m, n].WallType == WallID.GrassUnsafe || Main.tile[m, n].WallType == WallID.FlowerUnsafe || Main.tile[m, n].WallType == WallID.Grass || Main.tile[m, n].WallType == WallID.Flower || Main.tile[m, n].WallType == WallID.CorruptGrassUnsafe || Main.tile[m, n].WallType == WallID.CrimsonGrassUnsafe)
                                    Main.tile[m, n].WallType = WallID.HallowedGrassUnsafe;
                                else if (Main.tile[m, n].WallType == WallID.HardenedSand)
                                    Main.tile[m, n].WallType = WallID.HallowHardenedSand;
                                else if (Main.tile[m, n].WallType == WallID.Sandstone)
                                    Main.tile[m, n].WallType = WallID.HallowSandstone;
                                else if (Main.tile[m, n].WallType == WallID.EbonstoneUnsafe || Main.tile[m, n].WallType == WallID.CrimstoneUnsafe)
                                    Main.tile[m, n].WallType = WallID.PearlstoneBrickUnsafe;

                            }
                            if (convertTile)
                            {

                                if (Main.tile[m, n].TileType == TileID.Hive)
                                {
                                    Main.tile[m, n].TileType = TileID.Pearlstone;
                                    WorldGen.SquareTileFrame(m, n);
                                }
                                else if (Main.tile[m, n].TileType == TileID.CrispyHoneyBlock)
                                {
                                    Main.tile[m, n].TileType = TileID.HallowHardenedSand;
                                    WorldGen.SquareTileFrame(m, n);
                                }
                                else if (Main.tile[m, n].TileType == TileID.Grass)
                                {
                                    Main.tile[m, n].TileType = TileID.HallowedGrass;
                                    WorldGen.SquareTileFrame(m, n);
                                }
                                else if (Main.tile[m, n].TileType == TileID.Stone || Main.tile[m, n].TileType == TileID.Ebonstone || Main.tile[m, n].TileType == TileID.Crimstone)
                                {
                                    Main.tile[m, n].TileType = TileID.Pearlstone;
                                    WorldGen.SquareTileFrame(m, n);
                                }
                                else if (Main.tile[m, n].TileType == TileID.Sand || Main.tile[m, n].TileType == TileID.Silt || Main.tile[m, n].TileType == TileID.Ebonsand || Main.tile[m, n].TileType == TileID.Crimsand)
                                {
                                    Main.tile[m, n].TileType = TileID.Pearlsand;
                                    WorldGen.SquareTileFrame(m, n);
                                }
                                else if (Main.tile[m, n].TileType == TileID.CorruptJungleGrass || Main.tile[m, n].TileType == TileID.CrimsonJungleGrass)
                                {
                                    Main.tile[m, n].TileType = TileID.JungleGrass;
                                    WorldGen.SquareTileFrame(m, n);
                                }
                                else if (Main.tile[m, n].TileType == TileID.CorruptGrass || Main.tile[m, n].TileType == TileID.CrimsonGrass)
                                {
                                    Main.tile[m, n].TileType = TileID.HallowedGrass;
                                    WorldGen.SquareTileFrame(m, n);
                                }
                                else if (Main.tile[m, n].TileType == TileID.IceBlock || Main.tile[m, n].TileType == TileID.CorruptIce || Main.tile[m, n].TileType == TileID.FleshIce)
                                {
                                    Main.tile[m, n].TileType = TileID.HallowedIce;
                                    WorldGen.SquareTileFrame(m, n);
                                }
                                else if (Main.tile[m, n].TileType == TileID.Sandstone)
                                {
                                    Main.tile[m, n].TileType = TileID.HallowSandstone;
                                    WorldGen.SquareTileFrame(m, n);
                                }
                                else if (Main.tile[m, n].TileType == TileID.HardenedSand)
                                {
                                    Main.tile[m, n].TileType = TileID.HallowHardenedSand;
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

                                if (Main.tile[m, n].WallType == WallID.GrassUnsafe || Main.tile[m, n].WallType == WallID.FlowerUnsafe || Main.tile[m, n].WallType == WallID.Grass || Main.tile[m, n].WallType == WallID.Flower || Main.tile[m, n].WallType == WallID.CorruptGrassUnsafe || Main.tile[m, n].WallType == WallID.CrimsonGrassUnsafe)
                                    Main.tile[m, n].WallType = WallID.HallowedGrassUnsafe;
                                else if (Main.tile[m, n].WallType == WallID.HardenedSand)
                                    Main.tile[m, n].WallType = WallID.HallowHardenedSand;
                                else if (Main.tile[m, n].WallType == WallID.Sandstone)
                                    Main.tile[m, n].WallType = WallID.HallowSandstone;
                                else if (Main.tile[m, n].WallType == WallID.EbonstoneUnsafe || Main.tile[m, n].WallType == WallID.CrimstoneUnsafe)
                                    Main.tile[m, n].WallType = WallID.PearlstoneBrickUnsafe;

                            }
                            if (convertTile)
                            {

                                if (Main.tile[m, n].TileType == TileID.Hive)
                                {
                                    Main.tile[m, n].TileType = TileID.Pearlstone;
                                    WorldGen.SquareTileFrame(m, n);
                                }
                                else if (Main.tile[m, n].TileType == TileID.CrispyHoneyBlock)
                                {
                                    Main.tile[m, n].TileType = TileID.HallowHardenedSand;
                                    WorldGen.SquareTileFrame(m, n);
                                }
                                else if (Main.tile[m, n].TileType == TileID.Grass)
                                {
                                    Main.tile[m, n].TileType = TileID.HallowedGrass;
                                    WorldGen.SquareTileFrame(m, n);
                                }
                                else if (Main.tile[m, n].TileType == TileID.Stone || Main.tile[m, n].TileType == TileID.Ebonstone || Main.tile[m, n].TileType == TileID.Crimstone)
                                {
                                    Main.tile[m, n].TileType = TileID.Pearlstone;
                                    WorldGen.SquareTileFrame(m, n);
                                }
                                else if (Main.tile[m, n].TileType == TileID.Sand || Main.tile[m, n].TileType == TileID.Silt || Main.tile[m, n].TileType == TileID.Ebonsand || Main.tile[m, n].TileType == TileID.Crimsand)
                                {
                                    Main.tile[m, n].TileType = TileID.Pearlsand;
                                    WorldGen.SquareTileFrame(m, n);
                                }
                                else if (Main.tile[m, n].TileType == TileID.CorruptJungleGrass || Main.tile[m, n].TileType == TileID.CrimsonJungleGrass)
                                {
                                    Main.tile[m, n].TileType = TileID.JungleGrass;
                                    WorldGen.SquareTileFrame(m, n);
                                }
                                else if (Main.tile[m, n].TileType == TileID.CorruptGrass || Main.tile[m, n].TileType == TileID.CrimsonGrass)
                                {
                                    Main.tile[m, n].TileType = TileID.HallowedGrass;
                                    WorldGen.SquareTileFrame(m, n);
                                }
                                else if (Main.tile[m, n].TileType == TileID.IceBlock || Main.tile[m, n].TileType == TileID.CorruptIce || Main.tile[m, n].TileType == TileID.FleshIce)
                                {
                                    Main.tile[m, n].TileType = TileID.HallowedIce;
                                    WorldGen.SquareTileFrame(m, n);
                                }
                                else if (Main.tile[m, n].TileType == TileID.Sandstone)
                                {
                                    Main.tile[m, n].TileType = TileID.HallowSandstone;
                                    WorldGen.SquareTileFrame(m, n);
                                }
                                else if (Main.tile[m, n].TileType == TileID.HardenedSand)
                                {
                                    Main.tile[m, n].TileType = TileID.HallowHardenedSand;
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
