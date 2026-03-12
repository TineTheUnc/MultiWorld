using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;

namespace MultiWorld.Common.Config
{
    public class Beta : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("Action")]
        [SeparatePage]
        public OpenWorldSettingButton OpenWorldSettingButton { get; set; } = new();
    }


    public class OpenWorldSettingButton()
    {
        [CustomModConfigItem(typeof(OpenWorldSettingProcess))]
        public int Button;
    }
    public class OpenWorldSettingProcess : FloatElement
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            ProcessStartInfo startInfo = new()
            {
                Arguments = MultiWorld.WorldSetting,
                FileName = "explorer.exe"
            };
            SoundEngine.PlaySound(SoundID.MenuOpen);
            Process.Start(startInfo);
            Main.MenuUI.GoBack();
        }
    }
}
