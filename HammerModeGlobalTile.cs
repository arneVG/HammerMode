using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace HammerMode
{
    internal class HammerModeGlobalTile : GlobalTile
    {
        private static bool CanSlope(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            return (tile != null && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType]) || tile.TileType == TileID.Platforms;
        }

        private static bool CreateHalfblock(int x, int y)
        {
            WorldGen.SlopeTile(x, y, 0);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 14, x, y, (int)Main.tile[x, y].Slope, 0, 0, 0);
            }
            WorldGen.PoundTile(x, y);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 7, x, y, 1f, 0, 0, 0);
            }
            return false;
        }

        private static void CreateSlope(HammerModePlayer modPlayer, int x, int y)
        {
            WorldGen.SlopeTile(x, y, (int)modPlayer.CurrentMode);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                int num = (int)Main.tile[x, y].Slope;
                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 14, x, y, num, 0, 0, 0);
            }
        }

        public override bool Slope(int x, int y, int type)
        {
            HammerModePlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<HammerModePlayer>();
            
            if (modPlayer.CurrentMode == ModeID.Disabled || !(CanSlope(x, y)))
            {
                return base.Slope(x, y, type);
            }
            
            if (modPlayer.CurrentMode == ModeID.Wall)
            {
                return false;
            }

            if (modPlayer.CurrentMode == ModeID.HalfBlock && WorldGen.CanPoundTile(x, y))
            {
                return CreateHalfblock(x, y);
            }

            CreateSlope(modPlayer, x, y);
            return false;
        }
    }
}
