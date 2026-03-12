using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
namespace MultiWorld.Common.UI.Elements
{
    public class UINumberBox : UITextPanel<string>
    {
        public int Number { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public string Message { get;}
        public event Action<int> OnAdd;
        public event Action<int> OnReduce;

        public UINumberBox(int min, int max, int start, string message): base($"{message}{start}")
        {
            Number = Utils.Clamp(start, min, max);
            Min = min;
            Max = max;
            Message = message;
            this.OnLeftClick += (evt, element) =>
            {

                if (Number < Max)
                {
                    Number++;
                    OnAdd?.Invoke(Number);
                    SetText($"{Message}{Number}");
                }

            };
            this.OnRightClick += (evt, element) =>
            {
                if (Number > Min)
                {
                    Number--;
                    OnReduce?.Invoke(Number);
                    SetText($"{Message}{Number}");
                }
            };
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (IsMouseHovering)
            {
                Main.instance.MouseText("Left: +1  Right: -1");
            }
        }
    }
}
