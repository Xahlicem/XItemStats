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
                XItemStats.Visible = true;
                return;
            }
            if (args.Length != 2) return;
            int choice = 1;
            if (args[1].ToLower().Equals("off")) choice = 0;
            if (args[1].ToLower().Equals("alt")) choice = 2;
            switch (args[0].ToLower()) {
                case "debug":
                    XItemStats.Debug = choice;
                    break;
                case "damage":
                    XItemStats.Damage = choice;
                    break;
                case "crit":
                    XItemStats.Crit = choice;
                    break;
                case "speed":
                    XItemStats.Speed = choice;
                    break;
                case "knock":
                    XItemStats.Knock = choice;
                    break;
                case "mana":
                    XItemStats.Mana = choice;
                    break;
                case "all":
                    XItemStats.Damage = choice;
                    XItemStats.Crit = choice;
                    XItemStats.Speed = choice;
                    XItemStats.Knock = choice;
                    XItemStats.Mana = choice;
                    break;
            }
            (mod as XItemStats).SetRadio();
        }

    }
}