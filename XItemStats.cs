using Terraria.ModLoader;

namespace XItemStats {
    class XItemStats : Mod {

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
    }
}