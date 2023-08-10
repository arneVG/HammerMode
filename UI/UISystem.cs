using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace HammerMode.UI
{
    internal class UISystem : ModSystem
    {
        public static bool ShowCircleUI = false;
        public static int CircleUITriggerType = -1;
        public static bool ShowTooltip = false;
        
        internal static UserInterface CircleUIInterface;
        internal static CircleUI CircleUI;

        internal static UserInterface TooltipUIInterface;
        internal static TooltipUI TooltipUI;

        private const string _assetPath = "HammerMode/UI/Textures";
        public static readonly Dictionary<int, Asset<Texture2D>> textures = new()
        {
            { 1, ModContent.Request<Texture2D>($"{_assetPath}/slope_1") },
            { 2, ModContent.Request<Texture2D>($"{_assetPath}/slope_2") },
            { 3, ModContent.Request<Texture2D>($"{_assetPath}/slope_3") },
            { 4, ModContent.Request<Texture2D>($"{_assetPath}/slope_4") },
            { 0, ModContent.Request<Texture2D>($"{_assetPath}/full_block") },
            { 5, ModContent.Request<Texture2D>($"{_assetPath}/half_block") },
            { 6, ModContent.Request<Texture2D>($"{_assetPath}/wall") }
        };

        public static bool AllowedToOpenUI { 
            get {
                return Main.hasFocus &&
                !Main.gamePaused &&
                !Main.LocalPlayer.dead &&
                !Main.LocalPlayer.mouseInterface &&
                !Main.drawingPlayerChat &&
                !Main.editSign &&
                !Main.editChest &&
                !Main.blockInput &&
                !Main.mapFullscreen &&
                !Main.HoveringOverAnNPC &&
                Main.LocalPlayer.cursorItemIconID != -1 &&
                Main.LocalPlayer.talkNPC == -1 &&
                Main.LocalPlayer.itemTime == 0 && Main.LocalPlayer.itemAnimation == 0 &&
                !(Main.LocalPlayer.frozen || Main.LocalPlayer.webbed || Main.LocalPlayer.stoned);
            } 
        }

        public override void PostSetupContent()
        {
            if (!Main.dedServ && Main.netMode != NetmodeID.Server)
            {
                CircleUI = new CircleUI();
                CircleUI.Activate();
                CircleUIInterface = new UserInterface();
                CircleUIInterface.SetState(CircleUI);

                TooltipUI = new TooltipUI();
                TooltipUI.Activate();
                TooltipUIInterface = new UserInterface();
                TooltipUIInterface.SetState(TooltipUI);
            }
        }

        public override void Unload()
        {
            CircleUI = null;
            CircleUIInterface = null;

            TooltipUI = null;
            TooltipUIInterface = null;
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Hotbar"));
            if (inventoryIndex != -1)
            {
                int mouseItemIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Item / NPC Head"));


                if (ShowCircleUI)
                {
                    if (mouseItemIndex != -1) layers.RemoveAt(mouseItemIndex);
                    layers.Insert(++inventoryIndex, new LegacyGameInterfaceLayer
                        (
                        "HammerMode: Mode Select",
                        delegate
                        {
                            CircleUIInterface.Draw(Main.spriteBatch, new GameTime());
                            return true;
                        },
                        InterfaceScaleType.UI)
                    );
                }

                if (ShowTooltip)
                {
                    if (mouseItemIndex != -1)
                    {
                        layers.Insert(mouseItemIndex, new LegacyGameInterfaceLayer(
                            "HammerMode: Mode Tooltip",
                            delegate
                            {
                                TooltipUIInterface.Draw(Main.spriteBatch, new GameTime());
                                return true;
                            },
                            InterfaceScaleType.UI));
                    }
                }
            }
        }
    }
}
