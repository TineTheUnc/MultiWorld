using MultiWorld.Common.Systems;
using MultiWorld.Common.Types;
using MultiWorld.Common.UI.Elements;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace MultiWorld.Common.UI.States
{
    public class UIWorldCreate : UIState
    {
        UIAutoScaleTextTextPanel<LocalizedText> backbutton;
        UINumberBox multiworldRadius;
        UITextPanel<string> waitUpdate;
        UIList specialPanel;
        UIScrollbar scrollbar;
        public override void OnInitialize()
        {
            WorldManageSystem worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
            UIPanel panel = new();
            panel.Width.Set(500, 0);
            panel.Height.Set(450, 0);
            panel.HAlign = 0.5f;
            panel.VAlign = 0.45f;
            multiworldRadius = new UINumberBox(0, 100, worldManageSystem.Radius, Language.GetText("Mods.MultiWorld.UI.RadiusNumberBox").Value)
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
            specialPanel = new()
            {
                Width = new StyleDimension(-20, 1),
                Top = { Pixels = 60 },
                Height = { Pixels = 360 },
                Left = { Pixels = 5 },
                ListPadding = 5f
            };
            scrollbar = new();
            scrollbar.Height.Set(-70, 1f);
            scrollbar.Top.Set(70, 0);
            scrollbar.Left.Set(-20, 1f);
            scrollbar.SetView(360f, 1000f);
            specialPanel.SetScrollbar(scrollbar);
            UINumberSliderPanel biomeSlider = new(Language.GetText("Mods.MultiWorld.UI.Special.Headers.Biome").Value,
                [
                new(Language.GetText("Mods.MultiWorld.UI.Special.Biome.Forest").Value, 1,10,1,worldManageSystem.BiomesChance["Forest"],OnValueChanged: SetBiomeChance("Forest")),
                new(Language.GetText("Mods.MultiWorld.UI.Special.Biome.Desert").Value, 1,10,1,worldManageSystem.BiomesChance["Desert"],OnValueChanged: SetBiomeChance("Desert")),
                new(Language.GetText("Mods.MultiWorld.UI.Special.Biome.Jungle").Value, 1,10,1,worldManageSystem.BiomesChance["Jungle"],OnValueChanged: SetBiomeChance("Jungle")),
                new(Language.GetText("Mods.MultiWorld.UI.Special.Biome.Snow").Value, 1,10,1,worldManageSystem.BiomesChance["Snow"], OnValueChanged : SetBiomeChance("Snow")),
                new(Language.GetText("Mods.MultiWorld.UI.Special.Biome.Ocean").Value, 1,10,1,worldManageSystem.BiomesChance["Ocean"], OnValueChanged : SetBiomeChance("Ocean")),
                ]
            )
            {
                Width = new StyleDimension(-20, 1),
            };
            specialPanel.Add(biomeSlider);
            UINumberSliderPanel structureSlider = new(Language.GetText("Mods.MultiWorld.UI.Special.Headers.Structure").Value,
                [
                new(Language.GetText("Mods.MultiWorld.UI.Special.Structure.Dungeon").Value, 1,10,1,worldManageSystem.StructureChance["Dungeon"], OnValueChanged : SetStructureChance("Dungeon")),
                new(Language.GetText("Mods.MultiWorld.UI.Special.Structure.Temple").Value, 1,10,1,worldManageSystem.StructureChance["Temple"], OnValueChanged : SetStructureChance("Temple")),
                new(Language.GetText("Mods.MultiWorld.UI.Special.Structure.Shimmer").Value, 1,10,1,worldManageSystem.StructureChance["Shimmer"], OnValueChanged : SetStructureChance("Shimmer"))
                ]
            )
            {
                Width = new StyleDimension(-20, 1),
            };
            specialPanel.Add(structureSlider);
            UINumberSliderPanel hardModeSlider = new(Language.GetText("Mods.MultiWorld.UI.Special.Headers.HardMode").Value,
                [
                new(Language.GetText("Mods.MultiWorld.UI.Special.HardMode.Evil").Value, 1,10,1,worldManageSystem.HardmodeChance["Evil"], OnValueChanged : SetHardModeChance("Evil")),
                new(Language.GetText("Mods.MultiWorld.UI.Special.HardMode.Hallow").Value, 1,10,1,worldManageSystem.HardmodeChance["Hallow"], OnValueChanged : SetHardModeChance("Hallow")),
                new(Language.GetText("Mods.MultiWorld.UI.Special.HardMode.NoneGen").Value, 1,10,1,worldManageSystem.HardmodeChance["NoneGen"], OnValueChanged : SetHardModeChance("NoneGen"))
                ]
            )
            {
                Width = new StyleDimension(-20, 1),
            };
            specialPanel.Add(hardModeSlider);
            var worldtypebutton = new UIAutoScaleTextTextPanel<LocalizedText>(Language.GetText($"Mods.MultiWorld.UI.WorldtypeButton.{worldManageSystem.genMode}"))
            {
                Width = new StyleDimension(-10, 1),
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
            Addchildren(panel);
        }

        private static Action<int> SetBiomeChance(string biome)
        {
            return (x) =>
            {
                WorldManageSystem worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
                worldManageSystem.BiomesChance[biome] = x;
            };
        }

        private static Action<int> SetHardModeChance(string biome)
        {
            return (x) =>
            {
                WorldManageSystem worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
                worldManageSystem.HardmodeChance[biome] = x;
            };
        }

        private static Action<int> SetStructureChance(string structure)
        {
            return (x) =>
            {
                WorldManageSystem worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
                worldManageSystem.StructureChance[structure] = x;
            };
        }


        private void Addchildren(UIElement parent)
        {
            WorldManageSystem worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
            parent.RemoveChild(multiworldRadius);
            parent.RemoveChild(waitUpdate);
            parent.RemoveChild(specialPanel);
            parent.RemoveChild(scrollbar);
            if (worldManageSystem.genMode == GenMode.Normal)
            {
                parent.Append(multiworldRadius);
            }
            else if (worldManageSystem.genMode == GenMode.Special)
            {
                parent.Append(scrollbar);
                parent.Append(specialPanel);
            }
            else if (worldManageSystem.genMode != GenMode.Off)
            {
                parent.Append(waitUpdate);
            }
            parent.Recalculate();
        }

        private static void InputUpdateNumber(int number)
        {
            WorldManageSystem worldManageSystem = ModContent.GetInstance<WorldManageSystem>();
            worldManageSystem.WorldRadius = number;
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
            Addchildren(button.Parent);
        }

        private void Backbutton_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            ModContent.GetInstance<UIManageSystem>().dragging = null;
            SoundEngine.PlaySound(SoundID.MenuClose);
            Main.MenuUI.GoBack();
        }

        public override void OnDeactivate()
        {
            ModContent.GetInstance<UIManageSystem>().dragging = null;
        }
    }
}
