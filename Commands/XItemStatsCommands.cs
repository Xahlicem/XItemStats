using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using XItemStats.UI;

namespace XItemStats.Commands {

    public class XItemStatsCommands : ModCommand {
        public override CommandType Type {
            get { return CommandType.Chat; }
        }

        public override string Command {
            get { return "Item"; }
        }

        public override string Description {
            get { return "Usage: /Item (damage/crit/speed/knock/mana/all) (off/on/alt) "; }
        }

        public override void Action(CommandCaller caller, string input, string[] args) {
            if (args.Length == 0) {
                XItemUI.visible = true;
                return;
            }
            if (args.Length != 2) return;
            int choice = 1;
            if (args[1].ToLower().Equals("off")) choice = 0;
            if (args[1].ToLower().Equals("alt")) choice = 2;
            switch (args[0].ToLower()) {
                case "debug":
                    XItemStats.debug = choice;
                    break;
                case "damage":
                    XItemStats.damage = choice;
                    break;
                case "crit":
                    XItemStats.crit = choice;
                    break;
                case "speed":
                    XItemStats.speed = choice;
                    break;
                case "knock":
                    XItemStats.knock = choice;
                    break;
                case "mana":
                    XItemStats.mana = choice;
                    break;
                case "all":
                    XItemStats.damage = choice;
                    XItemStats.crit = choice;
                    XItemStats.speed = choice;
                    XItemStats.knock = choice;
                    XItemStats.mana = choice;
                    break;
            }
            (mod as XItemStats).setRadio();
        }

    }
}