using Microsoft.Xna.Framework;
using MultiWorld.Common.Config;
using MultiWorld.Common.Systems;
using MultiWorld.Common.Types;
using MultiWorld.Common.UI;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace MultiWorld
{
	public partial class MultiWorld : Mod
	{

		private MetaData MetaData;
		private static int WorldRadius = 0;

		public override void Load()
		{
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
			Container.Parent.Parent.Height = StyleDimension.FromPixels((float)(338 + 18));
			var multiworldCreate = new UIPanel()
			{
				Width = StyleDimension.FromPixelsAndPercent((0f) * 2f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(42f, 0f),
			};
			multiworldCreate.Top.Set(accumualtedHeight, 0f);
			var multiworldButton = new UITextPanel<string>("Multi World: off")
			{
				Width = StyleDimension.FromPixelsAndPercent(0f, 0.1f),
			};
			var worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
			worldManageSystem.CreateMultiWorld = false;
			multiworldButton.Top.Set(-11, 0f);
			multiworldButton.Left.Set(0, -0.025f);
			multiworldButton.OnLeftClick += (UIMouseEvent _, UIElement listeningElement) => {
				var Button = (UITextPanel<string>)listeningElement;
				var worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
				if (worldManageSystem.CreateMultiWorld)
				{
					worldManageSystem.CreateMultiWorld = false;
					Button.SetText("Multi World: off");
				}
				else
				{
					worldManageSystem.CreateMultiWorld = true;
					Button.SetText("Multi World: on");
				}
			};
			multiworldButton.OnMouseOver += ButtonOver;
			multiworldButton.OnMouseOut += ButtonOut;
			multiworldButton.OnMouseOver += (UIMouseEvent _, UIElement listeningElement) =>
			{
				var UIWorldCreation_descriptionTextInfo = typeof(UIWorldCreation).GetField("_descriptionText", BindingFlags.Instance | BindingFlags.NonPublic);
				var UIWorldCreation_descriptionText = (UIText)UIWorldCreation_descriptionTextInfo.GetValue(Main.MenuUI.CurrentState);
				UIWorldCreation_descriptionText.SetText(Language.GetText("Mods.MultiWorld.UI.Description.MultiWorldToggle"));
			};
			multiworldButton.OnMouseOut += ClearOptionDescription;
			multiworldCreate.Append(multiworldButton);
			WorldRadius = 0;
			var config = ModContent.GetInstance<Beta>();
			var multiworldRadius = new UINumberBox(0, 100, 0, Language.GetText("Mods.MultiWorld.UI.RadiusNumberBox").Value);
			multiworldRadius.Top.Set(-11, 0f);
			multiworldRadius.Left.Set(0, 0.32f);
			if (!config.SepecialWorld)
			{
				multiworldRadius.OnAdd += InputUpdateNumber;
				multiworldRadius.OnReduce += InputUpdateNumber;
				multiworldRadius.OnMouseOver += ButtonOver;
				multiworldRadius.OnMouseOut += ButtonOut;
			}
			multiworldRadius.OnMouseOver += (UIMouseEvent _, UIElement listeningElement) =>
			{
				var config = ModContent.GetInstance<Beta>();
				var UIWorldCreation_descriptionTextInfo = typeof(UIWorldCreation).GetField("_descriptionText", BindingFlags.Instance | BindingFlags.NonPublic);
				var UIWorldCreation_descriptionText = (UIText)UIWorldCreation_descriptionTextInfo.GetValue(Main.MenuUI.CurrentState);
				if (config.SepecialWorld)
				{
					UIWorldCreation_descriptionText.SetText(Language.GetText("Mods.MultiWorld.UI.Description.SpecialWorldRadius"));
				}
				else
				{
					UIWorldCreation_descriptionText.SetText(Language.GetText("Mods.MultiWorld.UI.Description.MultiWorldRadius"));
				}

			};
			multiworldRadius.OnMouseOut += ClearOptionDescription;
			multiworldCreate.Append(multiworldRadius);
			Container.Append(multiworldCreate);
		}

		private static void ClearOptionDescription(UIMouseEvent evt, UIElement listeningElement)
		{
			var UIWorldCreation_descriptionTextInfo = typeof(UIWorldCreation).GetField("_descriptionText", BindingFlags.Instance | BindingFlags.NonPublic);
			var UIWorldCreation_descriptionText = (UIText)UIWorldCreation_descriptionTextInfo.GetValue(Main.MenuUI.CurrentState);
			UIWorldCreation_descriptionText.SetText(Language.GetText("UI.WorldDescriptionDefault"));
		}

		private static void InputUpdateNumber(UINumberBox self)
		{
			WorldRadius = self.Number;
		}

		private static void ButtonOver(UIMouseEvent _, UIElement listeningElement)
		{
			var Button = (UITextPanel<string>)listeningElement;
			Button.BorderColor = Colors.RarityYellow;
		}

		private static void ButtonOut(UIMouseEvent _, UIElement listeningElement)
		{
			var Button = (UITextPanel<string>)listeningElement;
			Button.BorderColor = Color.Black;
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
