using System.IO;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.UI;
using XItemStats.UI;
using Terraria.ID;

namespace XItemStats {
    class XItemStats : Mod {

        private UserInterface ui;
        internal XItemUI xItemUi;

        private static int debug = 0;
        public static int Debug { get { return debug; } set { debug = value; Configuration.Put("Debug", debug); } }
        private static int damage = 1;
        public static int Damage { get { return damage; } set { damage = value; Configuration.Put("Damage", damage); } }
        private static int crit = 1;
        public static int Crit { get { return crit; } set { crit = value; Configuration.Put("Crit", crit); } }
        private static int speed = 1;
        public static int Speed { get { return speed; } set { speed = value; Configuration.Put("Speed", speed); } }
        private static int knock = 1;
        public static int Knock { get { return knock; } set { knock = value; Configuration.Put("Knock", knock); } }
        private static int mana = 1;
        public static int Mana { get { return mana; } set { mana = value; Configuration.Put("Mana", mana); } }
        private static bool visible = true;
        public static bool Visible { get { return visible; } set { visible = value; Configuration.Put("Visible", visible); } }

        static Preferences Configuration = new Preferences(Path.Combine(Main.SavePath, "Mod Configs", "ItemStats+.json"));

        public XItemStats() {
            Properties = new ModProperties() {
            Autoload = true,
            AutoloadGores = true,
            AutoloadSounds = true
            };
        }

        public override void Load() {
            if (Main.dedServ) return;
            xItemUi = new XItemUI();
            xItemUi.Activate();
            ui = new UserInterface();
            ui.SetState(xItemUi);

            if (Configuration.Load()) {
                Configuration.Get("Debug", ref debug);
                Configuration.Get("Damage", ref damage);
                Configuration.Get("Crit", ref crit);
                Configuration.Get("Speed", ref speed);
                Configuration.Get("Knock", ref knock);
                Configuration.Get("Mana", ref mana);
                SetRadio();

                Configuration.Get("Visible", ref visible);
                Configuration.Get("Top", ref xItemUi.Panel.Top.Pixels);
                Configuration.Get("Left", ref xItemUi.Panel.Left.Pixels);
                xItemUi.Recalculate();
            } else {
                Configuration.Put("Debug", Debug);
                Configuration.Put("Damage", Damage);
                Configuration.Put("Crit", Crit);
                Configuration.Put("Speed", Speed);
                Configuration.Put("Knock", Knock);
                Configuration.Put("Mana", Mana);

                Configuration.Put("Visible", visible);
                SetUIPoint(xItemUi);
                Configuration.Save();
            }
            Configuration.AutoSave = true;
        }

        public override void ModifyInterfaceLayers(System.Collections.Generic.List<GameInterfaceLayer> layers) {
            int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (MouseTextIndex != -1) {
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "XItemStats: Settings",
                    delegate {
                        if (Visible) {
                            ui.Update(Main._drawInterfaceGameTime);
                            xItemUi.Draw(Main.spriteBatch);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI));
            }
        }

        public void SetRadio() {
            xItemUi.Damage.SetValue(Damage);
            xItemUi.Crit.SetValue(Crit);
            xItemUi.Speed.SetValue(Speed);
            xItemUi.Knock.SetValue(Knock);
            xItemUi.Mana.SetValue(Mana);
        }

        public static void SetUIPoint(XItemUI xui) {
            if (xui == null || xui.Panel == null) return;
            Configuration.Put("Top", xui.Panel.Top.Pixels);
            Configuration.Put("Left", xui.Panel.Left.Pixels);
        }
    }
}