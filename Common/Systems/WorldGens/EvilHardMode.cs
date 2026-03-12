using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace MultiWorld.Common.Systems.WorldGens
{
    public class EvilHardMode
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
                    if (item == "Hardmode Evil")
                    {
                        tasks.Insert(index, new EvilPass(loadWeight));
                    }
                }
            }
            return tasks;
        }

        public class EvilPass(double loadWeight) : GenPass("Hardmode Evil", loadWeight)
        {
            protected override void ApplyPass(GenerationProgress progress, GameConfiguration passConfig)
            {
                var crimson = WorldGen.crimson;
                var index = int.Parse(Path.GetFileNameWithoutExtension(Main.ActiveWorldFileData.Path));
                if (index > 0)
                {
                    for (int m = 0; m < Main.maxTilesX; m++)
                    {
                        for (int n = 0; n < Main.maxTilesY - 5; n++)
                        {
                            bool convertWall = WallLoader.Convert(m, n, crimson ? 4 : 1);
                            bool convertTile = TileLoader.Convert(m, n, crimson ? 4 : 1);
                            if (crimson)
                            {
                                if (convertWall)
                                {
                                    if (Main.tile[m, n].WallType == WallID.GrassUnsafe || Main.tile[m, n].WallType == WallID.FlowerUnsafe || Main.tile[m, n].WallType == WallID.Grass || Main.tile[m, n].WallType == WallID.Flower)
                                        Main.tile[m, n].WallType = WallID.CrimsonGrassUnsafe;
                                    else if (Main.tile[m, n].WallType == WallID.HardenedSand)
                                        Main.tile[m, n].WallType = WallID.CrimsonHardenedSand;
                                    else if (Main.tile[m, n].WallType == WallID.Sandstone)
                                        Main.tile[m, n].WallType = WallID.CrimsonSandstone;
                                }
                                if (convertTile)
                                {

                                    if (Main.tile[m, n].TileType == TileID.Hive)
                                    {
                                        Main.tile[m, n].TileType = TileID.Crimstone;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.CrispyHoneyBlock)
                                    {
                                        Main.tile[m, n].TileType = TileID.CrimsonHardenedSand;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.JungleGrass || Main.tile[m, n].TileType == TileID.CorruptJungleGrass)
                                    {
                                        Main.tile[m, n].TileType = TileID.CrimsonJungleGrass;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.Grass || Main.tile[m, n].TileType == TileID.HallowedGrass)
                                    {
                                        Main.tile[m, n].TileType = TileID.CrimsonGrass;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.Stone || Main.tile[m, n].TileType == TileID.Pearlstone)
                                    {
                                        Main.tile[m, n].TileType = TileID.Crimstone;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.Sand || Main.tile[m, n].TileType == TileID.Silt || Main.tile[m, n].TileType == TileID.Pearlsand)
                                    {
                                        Main.tile[m, n].TileType = TileID.Crimsand;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.IceBlock || Main.tile[m, n].TileType == TileID.HallowedIce)
                                    {
                                        Main.tile[m, n].TileType = TileID.FleshIce;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.Sandstone)
                                    {
                                        Main.tile[m, n].TileType = TileID.CrimsonSandstone;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.HardenedSand)
                                    {
                                        Main.tile[m, n].TileType = TileID.CrimsonHardenedSand;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                }
                            }
                            else
                            {
                                if (convertWall)
                                {
                                    if (Main.tile[m, n].WallType == WallID.GrassUnsafe || Main.tile[m, n].WallType == WallID.FlowerUnsafe || Main.tile[m, n].WallType == WallID.Grass || Main.tile[m, n].WallType == WallID.Flower)
                                        Main.tile[m, n].WallType = WallID.CorruptGrassUnsafe;
                                    else if (Main.tile[m, n].WallType == WallID.HardenedSand)
                                        Main.tile[m, n].WallType = WallID.CorruptHardenedSand;
                                    else if (Main.tile[m, n].WallType == WallID.Sandstone)
                                        Main.tile[m, n].WallType = WallID.CorruptSandstone;
                                }
                                if (convertTile)
                                {

                                    if (Main.tile[m, n].TileType == TileID.Hive)
                                    {
                                        Main.tile[m, n].TileType = TileID.Ebonstone;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.CrispyHoneyBlock)
                                    {
                                        Main.tile[m, n].TileType = TileID.CorruptHardenedSand;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.JungleGrass || Main.tile[m, n].TileType == TileID.CrimsonJungleGrass)
                                    {
                                        Main.tile[m, n].TileType = TileID.CorruptJungleGrass;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.Grass || Main.tile[m, n].TileType == TileID.HallowedGrass)
                                    {
                                        Main.tile[m, n].TileType = TileID.CorruptGrass;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.Stone || Main.tile[m, n].TileType == TileID.Pearlstone)
                                    {
                                        Main.tile[m, n].TileType = TileID.Ebonstone;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.Sand || Main.tile[m, n].TileType == TileID.Silt || Main.tile[m, n].TileType == TileID.Pearlsand)
                                    {
                                        Main.tile[m, n].TileType = TileID.Ebonsand;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.IceBlock || Main.tile[m, n].TileType == TileID.HallowedIce)
                                    {
                                        Main.tile[m, n].TileType = TileID.CorruptIce;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.Sandstone)
                                    {
                                        Main.tile[m, n].TileType = TileID.CorruptSandstone;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.HardenedSand)
                                    {
                                        Main.tile[m, n].TileType = TileID.CorruptHardenedSand;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
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
                            bool convertWall = WallLoader.Convert(m, n, crimson ? 4 : 1);
                            bool convertTile = TileLoader.Convert(m, n, crimson ? 4 : 1);
                            if (crimson)
                            {
                                if (convertWall)
                                {
                                    if (Main.tile[m, n].WallType == WallID.GrassUnsafe || Main.tile[m, n].WallType == WallID.FlowerUnsafe || Main.tile[m, n].WallType == WallID.Grass || Main.tile[m, n].WallType == WallID.Flower)
                                        Main.tile[m, n].WallType = WallID.CrimsonGrassUnsafe;
                                    else if (Main.tile[m, n].WallType == WallID.HardenedSand)
                                        Main.tile[m, n].WallType = WallID.CrimsonHardenedSand;
                                    else if (Main.tile[m, n].WallType == WallID.Sandstone)
                                        Main.tile[m, n].WallType = WallID.CrimsonSandstone;
                                }
                                if (convertTile)
                                {

                                    if (Main.tile[m, n].TileType == TileID.Hive)
                                    {
                                        Main.tile[m, n].TileType = TileID.Crimstone;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.CrispyHoneyBlock)
                                    {
                                        Main.tile[m, n].TileType = TileID.CrimsonHardenedSand;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.JungleGrass || Main.tile[m, n].TileType == TileID.CorruptJungleGrass)
                                    {
                                        Main.tile[m, n].TileType = TileID.CrimsonJungleGrass;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.Grass || Main.tile[m, n].TileType == TileID.HallowedGrass)
                                    {
                                        Main.tile[m, n].TileType = TileID.CrimsonGrass;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.Stone || Main.tile[m, n].TileType == TileID.Pearlstone)
                                    {
                                        Main.tile[m, n].TileType = TileID.Crimstone;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.Sand || Main.tile[m, n].TileType == TileID.Silt || Main.tile[m, n].TileType == TileID.Pearlsand)
                                    {
                                        Main.tile[m, n].TileType = TileID.Crimsand;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.IceBlock || Main.tile[m, n].TileType == TileID.HallowedIce)
                                    {
                                        Main.tile[m, n].TileType = TileID.FleshIce;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.Sandstone)
                                    {
                                        Main.tile[m, n].TileType = TileID.CrimsonSandstone;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.HardenedSand)
                                    {
                                        Main.tile[m, n].TileType = TileID.CrimsonHardenedSand;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                }
                            }
                            else
                            {
                                if (convertWall)
                                {
                                    if (Main.tile[m, n].WallType == WallID.GrassUnsafe || Main.tile[m, n].WallType == WallID.FlowerUnsafe || Main.tile[m, n].WallType == WallID.Grass || Main.tile[m, n].WallType == WallID.Flower)
                                        Main.tile[m, n].WallType = WallID.CorruptGrassUnsafe;
                                    else if (Main.tile[m, n].WallType == WallID.HardenedSand)
                                        Main.tile[m, n].WallType = WallID.CorruptHardenedSand;
                                    else if (Main.tile[m, n].WallType == WallID.Sandstone)
                                        Main.tile[m, n].WallType = WallID.CorruptSandstone;
                                }
                                if (convertTile)
                                {

                                    if (Main.tile[m, n].TileType == TileID.Hive)
                                    {
                                        Main.tile[m, n].TileType = TileID.Ebonstone;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.CrispyHoneyBlock)
                                    {
                                        Main.tile[m, n].TileType = TileID.CorruptHardenedSand;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.JungleGrass || Main.tile[m, n].TileType == TileID.CrimsonJungleGrass)
                                    {
                                        Main.tile[m, n].TileType = TileID.CorruptJungleGrass;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.Grass || Main.tile[m, n].TileType == TileID.HallowedGrass)
                                    {
                                        Main.tile[m, n].TileType = TileID.CorruptGrass;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.Stone || Main.tile[m, n].TileType == TileID.Pearlstone)
                                    {
                                        Main.tile[m, n].TileType = TileID.Ebonstone;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.Sand || Main.tile[m, n].TileType == TileID.Silt || Main.tile[m, n].TileType == TileID.Pearlsand)
                                    {
                                        Main.tile[m, n].TileType = TileID.Ebonsand;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.IceBlock || Main.tile[m, n].TileType == TileID.HallowedIce)
                                    {
                                        Main.tile[m, n].TileType = TileID.CorruptIce;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.Sandstone)
                                    {
                                        Main.tile[m, n].TileType = TileID.CorruptSandstone;
                                        WorldGen.SquareTileFrame(m, n);
                                    }
                                    else if (Main.tile[m, n].TileType == TileID.HardenedSand)
                                    {
                                        Main.tile[m, n].TileType = TileID.CorruptHardenedSand;
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
}
