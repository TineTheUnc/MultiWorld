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
		public List<string> removeList = [
			"Dunes",
			"Ocean Sand",
			"Generate Ice Biome",
			"Jungle",
			"Mud Caves To Grass",
			"Full Desert",
			"Corruption",
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
			"Shimmer"
		];

		public List<GenPass> Gens(List<GenPass> tasks) {
			var system = ModContent.GetInstance<WorldManageSystem>();
			system.Shimmer = false;
			system.Evil = 0;
			foreach (string item in removeList)
			{
				int index = tasks.FindIndex(genpass => genpass.Name.Equals(item));
				if (index != -1)
				{
					tasks.Remove(tasks[index]);
					if (item == "Buried Chests")
					{
						tasks.Insert(index, new CommonGen.BuriedChestPass(false));
					}
				}
			}
			return tasks;
		}
		
	}
}
