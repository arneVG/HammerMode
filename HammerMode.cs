using Terraria.ModLoader;

namespace HammerMode
{
    enum ModeID
    {
        // 1, 2, 3, 4 are slope IDs
        // 0, 5, 5 are custom values
        Disabled = -1,
        FullBlock = 0,
        Slope1 = 1,
        Slope2 = 2,
        Slope3 = 3,
        Slope4 = 4,
        HalfBlock = 5,
        Wall = 6
    }

    public class HammerMode : Mod
	{
        internal static ModKeybind OpenMenuKeybind;
        public override void Load()
        {
            OpenMenuKeybind = KeybindLoader.RegisterKeybind(this, "Cycle hammer mode", "Mouse2");
        }

        public override void Unload()
        {
            OpenMenuKeybind = null;
        }
    }
}