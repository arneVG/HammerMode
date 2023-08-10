using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;

namespace HammerMode.UI
{
    internal class TooltipUI : UIState
    {
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            int currentMode = (int)Main.LocalPlayer.GetModPlayer<HammerModePlayer>().CurrentMode;

            Texture2D texture = (Texture2D)UISystem.textures[currentMode];
            Vector2 pos = new(Main.mouseX - 25, Main.mouseY + 20);
            Main.spriteBatch.Draw(texture, pos, Color.White);
        }
    }
}
