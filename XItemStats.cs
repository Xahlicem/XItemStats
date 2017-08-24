using Terraria;
using Terraria.GameContent.UI;
using Terraria.ModLoader;
using Terraria.UI;
using XItemStats.UI;

namespace XItemStats {
    class XItemStats : Mod {

        private UserInterface exampleUserInterface;
        internal XItemUI ui;

        public static int debug = 0;
        public static int damage = 1;
        public static int crit = 1;
        public static int speed = 1;
        public static int knock = 1;
        public static int mana = 1;

        public XItemStats() {
            Properties = new ModProperties() {
            Autoload = true,
            AutoloadGores = true,
            AutoloadSounds = true
            };
        }

        public override void Load() {
            ui = new XItemUI();
            ui.Activate();
            exampleUserInterface = new UserInterface();
            exampleUserInterface.SetState(ui);
        }

        public override void ModifyInterfaceLayers(System.Collections.Generic.List<GameInterfaceLayer> layers) {
            int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (MouseTextIndex != -1) {
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "XItemStats: Settings",
                    delegate {
                        if (XItemUI.visible) {
                            exampleUserInterface.Update(Main._drawInterfaceGameTime);
                            ui.Draw(Main.spriteBatch);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI));
            }
        }

        public void setRadio() {
            ui.damage.SetValue(damage);
            ui.crit.SetValue(crit);
            ui.speed.SetValue(speed);
            ui.knock.SetValue(knock);
            ui.mana.SetValue(mana);
        }
    }
}