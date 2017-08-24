using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace XItemDamage.Items {
    public class GlobalItemDamageMod : GlobalItem {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (!(item.damage > 0)) return;
            for (int i = 0; i < tooltips.Count; i++) {
                if (tooltips[i].Name.Equals("Damage")) {
                    string[] text = tooltips[i].text.Split(' ');
                    Item baseItem = new Item();
                    baseItem.CloneDefaults(item.type);
                    int damage = int.Parse(text[0]);
                    damage -= baseItem.damage;
                    if (damage != 0) {
                        tooltips[i].text = text[0] + "(" + ((damage > 0) ? "+" : "-") + Math.Abs(damage) + ")";
                        for (int j = 1; j < text.Length; j++)
                            tooltips[i].text += " " + text[j];
                    }
                }
                if (tooltips[i].Name.Equals("Speed")) {
                    string[] text = tooltips[i].text.Split(' ');
                    Item baseItem = new Item();
                    baseItem.CloneDefaults(item.type);
                    int speed = 0;
                    int speedMod = 0;
                    if (item.melee) {
                        speed = item.useAnimation - 1;
                        speed = (int)((float) speed * Main.player[item.owner].meleeSpeed);
                        speedMod = speed - (baseItem.useAnimation - 1);
                    } else {
                        speed = item.useTime;
                        speedMod = speed - baseItem.useTime;
                    }
                    tooltips[i].text = speed.ToString();
                    if (speedMod != 0) tooltips[i].text += "(" + ((speedMod > 0) ? "+" : "-") + Math.Abs(speedMod) + ")";
                    for (int j = 1; j < text.Length; j++)
                        tooltips[i].text += " " + text[j];
                }
                //tooltips[i].text += " " + tooltips[i].Name;
            }
        }
    }
}