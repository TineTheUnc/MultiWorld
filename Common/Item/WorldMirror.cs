using System.IO;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using MultiWorld.Common.Systems;
using MultiWorld.Common.Types;

namespace MultiWorld.Common.Item
{
	public class WorldMirror: ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.MagicMirror);
		}

		public override void UseAnimation(Player player)
		{
			base.UseAnimation(player);
			if (MultiWorldFileData.IsMultiWorld(Main.ActiveWorldFileData.Path))
			{
				WorldManageSystem worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
				var x = (int)player.position.X / 16;
				worldManageSystem.playerY = (int)player.position.Y / 16;
				var index = int.Parse(Path.GetFileNameWithoutExtension(Main.ActiveWorldFileData.Path));
				worldManageSystem.CurrentWorldIndex = index;
				if (x <= 60)
				{
					worldManageSystem.NextWorldIndex = index - 1;
					worldManageSystem.ChangeWorld();
				}
				else if ((Main.ActiveWorldFileData.WorldSizeX - x) <= 60)
				{
					worldManageSystem.NextWorldIndex = index + 1;
					
					worldManageSystem.ChangeWorld();
				}
				else {
					ChatHelper.SendChatMessageToClient(NetworkText.FromLiteral("Use at left and right side of world."), Colors.RarityNormal, Main.LocalPlayer.whoAmI);
				}
			}
			else {
				ChatHelper.SendChatMessageToClient(NetworkText.FromLiteral("Use only in Multi World"), Colors.RarityNormal, Main.LocalPlayer.whoAmI);
			}
		}

		public override void AddRecipes()
		{
			base.AddRecipes();
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MagicMirror);
			recipe.AddIngredient(ItemID.PlatinumBar, 5);
			recipe.AddIngredient(ItemID.Diamond, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
