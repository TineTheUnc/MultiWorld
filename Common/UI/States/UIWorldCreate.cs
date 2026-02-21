using Microsoft.Xna.Framework.Graphics;
using MultiWorld.Common.Config;
using MultiWorld.Common.Systems;
using MultiWorld.Common.Types;
using MultiWorld.Common.UI.Elements;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;
using tModPorter;

namespace MultiWorld.Common.UI.State
{
    public class UIWorldCreate: UIState
    {
        UIAutoScaleTextTextPanel<LocalizedText> backbutton;
        UINumberBox multiworldRadius;
        UITextPanel<string> waitUpdate;
        public override void OnInitialize()
        {
            WorldManageSystem worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
            //var accumualtedHeight = 0;
            multiworldRadius = new UINumberBox(0, 100, 0, Language.GetText("Mods.MultiWorld.UI.RadiusNumberBox").Value)
            {
                Width = new StyleDimension(-10, 1),
                Height = { Pixels = 40 },
                Top = { Pixels = 60 },
                Left = { Pixels = 5 }
            }.WithFadedMouseOver();
            multiworldRadius.OnAdd += InputUpdateNumber;
            multiworldRadius.OnReduce += InputUpdateNumber;
            waitUpdate = new UITextPanel<string>("Wait for update. Use other way to create.")
            {
                Width = new StyleDimension(-10, 1),
                Height = { Pixels = 40 },
                Top = { Pixels = 60 }
            }.WithFadedMouseOver();
            UIPanel panel = new();
            panel.Width.Set(500, 0);
            panel.Height.Set(450, 0);
            panel.HAlign = 0.5f;
            panel.VAlign = 0.45f;
            if (worldManageSystem.genMode == GenMode.Normal)
            {
                panel.Append(multiworldRadius);
            }
            else if (worldManageSystem.genMode != GenMode.Off)
            {
                panel.Append(waitUpdate);
            }
            var worldtypebutton = new UIAutoScaleTextTextPanel<LocalizedText>(Language.GetText($"Mods.MultiWorld.UI.WorldtypeButton.{worldManageSystem.genMode}"))
            {
                Width = new StyleDimension(-10,1),
                Height = { Pixels = 40 },
                HAlign = 0.5f,
                Top = { Pixels = 10 }
            }.WithFadedMouseOver();
            worldtypebutton.OnLeftClick += Worldtypebutton_OnClick;
            panel.Append(worldtypebutton);
            backbutton = new UIAutoScaleTextTextPanel<LocalizedText>(Language.GetText("UI.Back"))
            {
                Width = new StyleDimension(-40f, 1f / 3f),
                Height = { Pixels = 40 },
                HAlign = 0.5f,
                VAlign = 0.8f,
                Top = { Pixels = -20 }
            }.WithFadedMouseOver();
            backbutton.OnLeftClick += Backbutton_OnClick;
            Append(panel);
            Append(backbutton);
        }

        private static void InputUpdateNumber(UINumberBox self)
        {
            WorldManageSystem worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
            worldManageSystem.WorldRadius = self.Number;
        }

        private void Worldtypebutton_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            WorldManageSystem worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
            if (worldManageSystem.genMode != GenMode.RandomMod)
            {
                worldManageSystem.genMode += 1;
            }
            else
            {
                worldManageSystem.genMode = 0;
            }
            var button = listeningElement as UIAutoScaleTextTextPanel<LocalizedText>;
            button.SetText(Language.GetText($"Mods.MultiWorld.UI.WorldtypeButton.{worldManageSystem.genMode}"));
            if (button.Parent.HasChild(multiworldRadius))
            {
                button.Parent.RemoveChild(multiworldRadius);
            }
            if (button.Parent.HasChild(waitUpdate))
            {
                button.Parent.RemoveChild(waitUpdate);
            }
            if (worldManageSystem.genMode == GenMode.Normal)
            {
                button.Parent.Append(multiworldRadius);
            }
            else if (worldManageSystem.genMode != GenMode.Off)
            {
                button.Parent.Append(waitUpdate);
            }
        }

        private void Backbutton_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuClose);
            Main.MenuUI.GoBack();
        }
    }
}
