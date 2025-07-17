using Microsoft.Xna.Framework;
using MonoMod.Cil;
using MonoMod.Core.Platforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Biomes;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static MultiWorld.Common.Systems.WorldManageSystem;

namespace MultiWorld.Common.Systems.WorldGens
{
	public class Forest
	{
		public static List<string> removeList = [
			"Dunes",
			"Ocean Sand",
			"Generate Ice Biome",
			"Jungle",
			"Mud Caves To Grass",
			"Full Desert",
			"Corruption",
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
		];

		public static List<GenPass> Gens(List<GenPass> tasks, bool first, ref double totalWeight) {
			OneBiome.Shimmer = false;
			OneBiome.Evil = 0;
			OneBiome.Biome = "Forest";
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
				}
			}
			if (first)
			{
				int dungeon = tasks.FindIndex(genpass => genpass.Name.Equals("Dungeon"));
				tasks.Remove(tasks[dungeon]);
				int shimmer = tasks.FindIndex(genpass => genpass.Name.Equals("Shimmer"));
				tasks.Remove(tasks[shimmer]);
			}
			else {
				int dungeon = tasks.FindIndex(genpass => genpass.Name.Equals("Dungeon"));
				if (OneBiome.HaveDungeon)
				{
					tasks.Remove(tasks[dungeon]);
				}
				else
				{
					if (WorldGen.genRand.NextBool(9, 10))
					{
						tasks.Remove(tasks[dungeon]);
					}
				}
				int shimmer = tasks.FindIndex(genpass => genpass.Name.Equals("Shimmer"));
				if (OneBiome.HaveShimmer)
				{
					tasks.Remove(tasks[shimmer]);
				}
				else
				{
					if (WorldGen.genRand.NextBool(9, 10))
					{
						tasks.Remove(tasks[shimmer]);
					}
				}
			}
			return tasks;
		}
		
	}
}
