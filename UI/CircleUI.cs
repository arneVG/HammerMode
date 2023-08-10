using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;

namespace HammerMode.UI
{
    public class CircleUI : UIState
    {
		internal static int? selected;

		internal const int mainDiameter = 44;
		internal const int outerRadius = mainDiameter + 12;
		
		internal static Vector2 spawnPosition = default;
		internal static bool updatePosition = false;

		internal const int CircleAmount = 7;
		internal const double angleSteps = 2.0d / CircleAmount;

		private static Vector2 TopLeftCorner { get { return spawnPosition - new Vector2(mainDiameter / 2, mainDiameter / 2); } }
			
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
			Main.LocalPlayer.mouseInterface = true;
			Main.LocalPlayer.cursorItemIconID = 0;
			Main.LocalPlayer.cursorItemIconText = "";
			selected = null;

			// Main.MouseScreen returns a different value when run outside of DrawSelf so it was moved to here
			if (updatePosition)
            {
				spawnPosition = Main.MouseScreen;
				updatePosition = false;
            }
			
			// Draw mode circles
            for (int i = 0; i < CircleAmount; i++)
            {
                double x = outerRadius * Math.Sin(angleSteps * i * Math.PI);
                double y = outerRadius * -Math.Cos(angleSteps * i * Math.PI);

                Rectangle bgRect = new((int)(TopLeftCorner.X + x), (int)(TopLeftCorner.Y + y), mainDiameter, mainDiameter);
                bool isMouseWithin = CheckMouseWithinCircle(Main.MouseScreen, bgRect);

				Color drawColor = Color.Gray;
				if (isMouseWithin) drawColor = Color.LightGray;
				if (i == (int)Main.LocalPlayer.GetModPlayer<HammerModePlayer>().CurrentMode) drawColor = Color.White;

                Main.spriteBatch.Draw(TextureAssets.WireUi[isMouseWithin ? 1 : 0].Value, bgRect, drawColor);

                Texture2D optionTexture = (Texture2D)UISystem.textures[i];
                Vector2 optionPos = new((int)(spawnPosition.X + x) - (optionTexture.Width / 2), (int)(spawnPosition.Y + y) - (optionTexture.Height / 2));

				if (isMouseWithin) selected = i;

                Main.spriteBatch.Draw(optionTexture, optionPos, drawColor);
            }
			
			// Draw hammer in centre
			Asset<Texture2D> asset = TextureAssets.Item[UISystem.CircleUITriggerType];
			Vector2 assetPos = new((int)spawnPosition.X - (asset.Width() / 2), (int)spawnPosition.Y - (asset.Height() / 2));
			Main.spriteBatch.Draw(asset.Value, assetPos, Color.White);
		}

		internal static bool CheckMouseWithinCircle(Vector2 mousePos, Rectangle rect)
        {
			Vector2 center = new(rect.X + (rect.Width / 2), rect.Y + (rect.Height / 2));
			int radius = rect.Width / 2;

			double dx = Math.Abs(mousePos.X - center.X);
			if (dx > radius) return false;
			double dy = Math.Abs(mousePos.Y - center.Y);
			if (dy > radius) return false;
			if (dx + dy <= radius) return true;
			return (dx * dx + dy * dy <= radius * radius);
		}

		public static void UpdatePosition()
        {
			updatePosition = true;
		}

		public static int? GetSelected() { return selected; }
    }
}
