using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiWorld.Common.Systems;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace MultiWorld.Common.UI.Elements
{
    public class UINumberSlider : UIElement
    {
        public Color SliderColor { get; set; } = Color.White;

        public Utils.ColorLerpMethod ColorMethod { get; set; }

        public bool DrawTicks { get; set; }

        private Rectangle rectangle = default;

        public int Min { get; set; }

        public int Max { get; set; }

        public int Increment { get; set; }

        public int NumberTicks => Increment <= 0 ? 0 : (Max - Min) / Increment + 1;

        public float TickIncrement => Increment <= 0 ? 0 : (float)Increment / (Max - Min);

        protected float Proportion
        {
            get
            {
                int range = Max - Min;
                if (range == 0) return 0f;

                return (float)(Value - Min) / range;
            }
            set
            {
                Value = Utils.Clamp((int)Math.Round((value * (float)(Max - Min) + (float)Min) * (1f / (float)Increment)) * Increment, Min, Max);
            }
        }

        private float rawProportion;

        public event Action<int> OnValueChanged;

        private int _value;

        public int Value
        {
            get => _value;
            set
            {
                value = Utils.Clamp(value, Min, Max);
                if (_value != value)
                {
                    _value = value;
                    OnValueChanged?.Invoke(value);
                }
            }
        }

        private const int BarPixels = 167;

        public UINumberSlider(int min, int max, int increment, int start)
        {
            Min = min;
            Value = start;
            Max = max;
            Increment = increment;
            ColorMethod = percent => Color.Lerp(Color.Black, SliderColor, percent);
            Texture2D tex = TextureAssets.ColorBar.Value;

            Width.Set(tex.Width, 0f);
            Height.Set(tex.Height, 0f);
            IgnoresMouseInteraction = false;
        }

        public void DrawValueBar(SpriteBatch sb, float scale, float perc, Vector2 Position, Utils.ColorLerpMethod colorMethod = null)
        {
            perc = Utils.Clamp(perc, -0.05f, 1.05f);
            colorMethod ??= Utils.ColorLerp_BlackToWhite;
            Texture2D colorBarTexture = TextureAssets.ColorBar.Value;
            Vector2 vector = new Vector2((float)colorBarTexture.Width, (float)colorBarTexture.Height) * scale;
            rectangle = new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                (int)vector.X,
                (int)vector.Y
            );
            Rectangle destinationRectangle = rectangle;
            float num2 = (float)rectangle.X + 5f * scale;
            float num3 = (float)rectangle.Y + 4f * scale;
            if (DrawTicks)
            {
                int numTicks = NumberTicks;
                if (numTicks > 1)
                {
                    for (int tick = 0; tick < numTicks; tick++)
                    {
                        float percent = (float)tick * TickIncrement;
                        if (percent <= 1f)
                        {
                            sb.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)(num2 + (float)BarPixels * percent * scale), rectangle.Y - 2, 2, rectangle.Height + 4), Color.White);
                        }
                    }
                }
            }
            sb.Draw(colorBarTexture, rectangle, Color.White);
            for (float num4 = 0f; num4 < (float)BarPixels; num4 += 1f)
            {
                float percent2 = num4 / (float)BarPixels;
                sb.Draw(TextureAssets.ColorBlip.Value, new Vector2(num2 + num4 * scale, num3), (Rectangle?)null, colorMethod(percent2), 0f, Vector2.Zero, scale, (SpriteEffects)0, 0f);
            }
            rectangle.Inflate((int)(-5f * scale), 2);
            var UIManage = ModContent.GetInstance<UIManageSystem>();
            if ((IsMouseHovering && UIManage.dragging == null) || UIManage.dragging == this)
            {
                sb.Draw(TextureAssets.ColorHighlight.Value, destinationRectangle, Main.OurFavoriteColor);
            }
            Texture2D colorSlider = TextureAssets.ColorSlider.Value;
            sb.Draw(colorSlider, new Vector2(num2 + BarPixels * scale * perc, num3 + 4f * scale), (Rectangle?)null, Color.White, 0f, colorSlider.Size() * 0.5f, scale, (SpriteEffects)0, 0f);
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var UIManage = ModContent.GetInstance<UIManageSystem>();
            var rect = GetDimensions().ToRectangle();
            if (UIManage.dragging == null && Main.mouseLeft && rect.Contains(Main.MouseScreen.ToPoint()))
            {
                UIManage.dragging = this;
                rawProportion = MathHelper.Clamp(
                    (Main.mouseX - rect.X) / (float)rect.Width,
                    0f, 1f
                );

                Proportion = rawProportion;
            }

            if (!Main.mouseLeft)
            {
                UIManage.dragging = null;
            }

            if (UIManage.dragging == this)
            {
                Main.LocalPlayer.mouseInterface = true;

                rawProportion = MathHelper.Clamp(
                    (Main.mouseX - rect.X) / (float)rect.Width,
                    0f, 1f
                );

                Proportion = rawProportion;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            CalculatedStyle dimensions = GetDimensions();
            Vector2 vector2 = dimensions.Position();
            DrawValueBar(spriteBatch, 1f, rawProportion, vector2, ColorMethod);
        }
    }
}
