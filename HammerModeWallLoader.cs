using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace HammerMode
{
    internal class HammerModeGlobalWall : GlobalWall
    {
        public override void KillWall(int i, int j, int type, ref bool fail)
        {
            HammerModePlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<HammerModePlayer>();
            if (modPlayer.CurrentMode != ModeID.Disabled && modPlayer.CurrentMode != ModeID.Wall)
            {
                fail = true;
            }
        }
    }
}
