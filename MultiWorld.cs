using Microsoft.Xna.Framework;
using MultiWorld.Common.Config;
using MultiWorld.Common.Systems;
using MultiWorld.Common.Types;
using MultiWorld.Common.UI.Elements;
using MultiWorld.Common.UI.State;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace MultiWorld
{
	public partial class MultiWorld : Mod
	{
        private MetaData MetaData;
		public static readonly string MainDir = Path.Combine(Main.SavePath, nameof(MultiWorld));
		public static readonly string Asset = Path.Combine(MainDir, "Asset");
		public static readonly string WorldSetting = Path.Combine(MainDir, "Setting");

		public override void Load()
		{
			Directory.CreateDirectory(WorldSetting);
			LoadHook();
			LoadIL();
		}

		public override void Unload()
		{
			UnloadHook();
			UnloadIL();
		}

		private static int WorldListSortMethod(WorldFileData data1, WorldFileData data2)
		{
			if (data1 == null && data2 == null)
				return 0;

			if (data1 == null || data1.Name == null)
				return 1;

			if (data2 == null || data2.Name == null)
				return -1;

			return data1.Name.CompareTo(data2.Name);
		}
		private static void AddMultiWorldMenu(UIElement Container, float accumualtedHeight)
		{
            WorldManageSystem worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
			worldManageSystem.genMode = 0;
            Container.Parent.Parent.Height = StyleDimension.FromPixels((float)(338 + 18));
			var multiworldButton = new UITextPanel<LocalizedText>(Language.GetText("Mods.MultiWorld.UI.MultiWorldButton"))
			{
				Width = StyleDimension.FromPixelsAndPercent(0f, 3f),
			}.WithFadedMouseOver(); ;
			multiworldButton.Top.Set(accumualtedHeight, 0f);
			multiworldButton.OnLeftClick += (UIMouseEvent _, UIElement listeningElement) => {

				Main.MenuUI.SetState(new UIWorldCreate());
                Main.menuMode = 888;
            };
			multiworldButton.OnMouseOver += (UIMouseEvent _, UIElement listeningElement) =>
			{
				var UIWorldCreation_descriptionTextInfo = typeof(UIWorldCreation).GetField("_descriptionText", BindingFlags.Instance | BindingFlags.NonPublic);
				var UIWorldCreation_descriptionText = (UIText)UIWorldCreation_descriptionTextInfo.GetValue(Main.MenuUI.CurrentState);
				UIWorldCreation_descriptionText.SetText(Language.GetText("Mods.MultiWorld.UI.Description.MultiWorldButton"));
			};
			multiworldButton.OnMouseOut += ClearOptionDescription;
			multiworldButton.OnUpdate += MultiworldButtonOnUpdate;
            Container.Append(multiworldButton);
		}

		private static void MultiworldButtonOnUpdate(UIElement listeningElement)
		{
            WorldManageSystem worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
			var multiworldButton = listeningElement as UITextPanel<LocalizedText>;
			if (worldManageSystem.genMode>0) multiworldButton.BackgroundColor = Color.DarkGreen;

        }

        private static void ClearOptionDescription(UIMouseEvent evt, UIElement listeningElement)
		{
			if (Main.MenuUI.CurrentState is UIWorldCreation)
			{
				var UIWorldCreation_descriptionTextInfo = typeof(UIWorldCreation).GetField("_descriptionText", BindingFlags.Instance | BindingFlags.NonPublic);
				var UIWorldCreation_descriptionText = (UIText)UIWorldCreation_descriptionTextInfo.GetValue(Main.MenuUI.CurrentState);
				UIWorldCreation_descriptionText.SetText(Language.GetText("UI.WorldDescriptionDefault"));
			}
		}
		public static bool CanSpawn(int x, int y)
		{
			List<bool> X = [];
			List<bool> Y = [];
			for (int i = x - 1; i < x + 1; i++)
			{
				var I = i;
				if (I < 0)
				{
					I = 10;
				}
				else if (I > Main.maxTilesX - 1)
				{
					I = Main.maxTilesX - 10;
				}
				if (IsAir(i, y))
				{
					X.Add(true);
				}
				else
				{
					X.Add(false);
				}
			}
			for (int i = y - 2; i < y + 2; i++)
			{
				var I = i;
				if (I < 0)
				{
					I = 10;
				}
				else if (I > Main.maxTilesY - 1)
				{
					I = Main.maxTilesY - 10;
				}
				if (IsAir(x, i))
				{
					Y.Add(true);
				}
				else
				{
					Y.Add(false);
				}
			}
			return !(X.Contains(false) || Y.Contains(false));
		}


		public static bool IsAir(int x, int y)
		{
			return !(Main.tile[x, y].HasUnactuatedTile || Main.tile[x, y].LiquidAmount > 0);
		}
	}
}
