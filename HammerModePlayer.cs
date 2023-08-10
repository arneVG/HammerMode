using System.Collections.Generic;
using HammerMode.UI;
using Terraria;
using Terraria.GameContent.ObjectInteractions;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace HammerMode
{
    internal class HammerModePlayer : ModPlayer
    {
        public ModeID CurrentMode = ModeID.Disabled;
        public bool HotkeyPressed = false;

        public static Item GetHeldItem { get { return Main.LocalPlayer.HeldItem; } }

        /*public override void Initialize()
        {
            base.Initialize();
        }*/

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (HammerMode.OpenMenuKeybind.JustPressed)
            {
                if (UISystem.ShowCircleUI)
                {
                    UISystem.ShowCircleUI = false;
                    return;
                }
                
                if (GetHeldItem.hammer > 0 && UISystem.AllowedToOpenUI)
                {
                    if (PlayerInput.LockGamepadTileUseButton || Player.noThrow != 0)
                    {
                        return;
                    }
                    UISystem.ShowCircleUI = true;
                    UISystem.CircleUITriggerType = GetHeldItem.type;
                    CircleUI.UpdatePosition();
                }
            }
            else if (PlayerInput.Triggers.JustPressed.MouseLeft)
            {
                if (UISystem.ShowCircleUI)
                {
                    ModeID? selectedMode = (ModeID)CircleUI.GetSelected();
                    if (selectedMode.HasValue)
                    {
                        CurrentMode = selectedMode == CurrentMode ? ModeID.Disabled : selectedMode.Value;
                    }
                    UISystem.ShowCircleUI = false;
                }
            }
            else if (PlayerInput.Triggers.JustPressed.MouseRight && UISystem.ShowCircleUI)
            {
                UISystem.ShowCircleUI = false;
            }
        }

        public override void PostUpdate()
        {
            if (UISystem.ShowCircleUI && UISystem.CircleUITriggerType != GetHeldItem.type) UISystem.ShowCircleUI = false;
            if (CurrentMode != ModeID.Disabled && GetHeldItem.hammer > 0 && !UISystem.ShowCircleUI) UISystem.ShowTooltip = true; else UISystem.ShowTooltip = false;
        }
    }
}
