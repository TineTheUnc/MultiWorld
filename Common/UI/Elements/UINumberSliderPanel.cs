using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace MultiWorld.Common.UI.Elements
{
    public class UINumberSliderPanel : UIPanel
    {
        private const int RowHeight = 40;
        public record SliderInfo(
            string Text,
            int Min,
            int Max,
            int Increment,
            int Start,
            Utils.ColorLerpMethod ColorMethod = null,
            Color? SliderColor = null,
            Action<int> OnValueChanged = null
        );

        public UINumberSliderPanel(string header, IEnumerable<SliderInfo> sliders)
        {
            var list = sliders.ToList();
            Height.Set(list.Count * RowHeight + 40 + PaddingTop + PaddingBottom, 0);
            var head = new UIText(header, 0.5f, true)
            {
                HAlign = 0.5f
            };
            head.Top.Set(10, 0);
            Append(head);
            int y = 40;
            foreach (var x in list)
            {
                var text = new UIText($"{x.Text} {x.Min}")
                {
                    HAlign = 0f,
                    Top = { Pixels = y }
                };
                text.Width.Set(0, 0.5f);

                var slider = new UINumberSlider(x.Min, x.Max, x.Increment,x.Start)
                {
                    DrawTicks = true,
                    HAlign = 1f,
                    Top = { Pixels = y }
                };

                slider.Left.Set(-10, 0);

                if (x.ColorMethod != null)
                    slider.ColorMethod = x.ColorMethod;

                if (x.SliderColor != null)
                    slider.SliderColor = x.SliderColor.Value;
                if (x.OnValueChanged != null)
                    slider.OnValueChanged += x.OnValueChanged;

                slider.OnValueChanged += v => text.SetText($"{x.Text} {v}");

                Append(text);
                Append(slider);

                y += RowHeight;
            }
        }
    }
}
