using System.Collections.Generic;
using Terraria.WorldBuilding;
namespace MultiWorld.Common.Systems.WorldGens
{
	public class NoneGaneHardmode
	{
		public static List<string> removeList = [
			"Hardmode Good Remix",
			"Hardmode Good",
			"Hardmode Evil",
			"Hardmode Walls",
		];

		public static List<GenPass> Gens(List<GenPass> tasks)
		{
			foreach (string item in removeList)
			{
				int index = tasks.FindIndex(genpass => genpass.Name.Equals(item));
				if (index != -1)
				{
					tasks.Remove(tasks[index]);
				}
			}
			return tasks;
		}
	}
}
