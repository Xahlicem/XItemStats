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

namespace XItemStats.Items {
    public class GlobalItemStatsMod : GlobalItem {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            Player player = Main.player[item.owner];
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
                if (tooltips[i].Name.Equals("CritChance")) {
                    string[] text = tooltips[i].text.Split(' ');
                    Item baseItem = new Item();
                    baseItem.CloneDefaults(item.type);
                    int crit = -4;
                    if (item.magic) crit += player.magicCrit;
                    else if (item.ranged) crit += player.rangedCrit;
                    else if (item.thrown) crit += player.thrownCrit;
                    else if (item.melee) crit += player.meleeCrit;
                    crit += (item.crit - baseItem.crit);

                    if (crit != 0) {
                        tooltips[i].text = text[0] + "(" + ((crit > 0) ? "+" : "-") + Math.Abs(crit) + ")";
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
                        speed = (int)((float) speed * player.meleeSpeed);
                        speedMod = speed - (baseItem.useAnimation - 1);
                    } else {
                        speed = item.useTime;
                        speedMod = speed - baseItem.useTime;
                    }

                    tooltips[i].text = speed.ToString();
                    if (speedMod != 0) tooltips[i].text += "(" + ((speedMod > 0) ? "+" : "-") + Math.Abs(speedMod) + ")";
                    for (int j = 0; j < text.Length; j++)
                        tooltips[i].text += ((j == 0) ? " (" : " ") + text[j] + ((j == text.Length - 2) ? ")" : "");
                }
                if (tooltips[i].Name.Equals("Knockback")) {
                    string[] text = tooltips[i].text.Split(' ');
                    Item baseItem = new Item();
                    baseItem.CloneDefaults(item.type);
                    float knockBack = item.knockBack;
                    float knockBackMod = knockBack - baseItem.knockBack;

                    tooltips[i].text = knockBack.ToString();
                    if (knockBackMod != 0) tooltips[i].text += "(" + ((knockBackMod > 0) ? "+" : "-") + Math.Abs(knockBackMod) + ")";
                    for (int j = 0; j < text.Length; j++)
                        tooltips[i].text += ((j == 0) ? " (" : " ") + text[j] + ((j == text.Length - 2) ? ")" : "");

                }
                if (tooltips[i].Name.Equals("UseMana")) {
                    string[] text = tooltips[i].text.Split(' ');
                    Item baseItem = new Item();
                    baseItem.CloneDefaults(item.type);
                    int damage = int.Parse(text[1]);
                    damage -= baseItem.mana;

                    if (damage != 0) {
                        tooltips[i].text = text[0] + " " + text[1] + "(" + ((damage > 0) ? "+" : "-") + Math.Abs(damage) + ")";
                        for (int j = 2; j < text.Length; j++)
                            tooltips[i].text += " " + text[j];
                    }
                }
                //tooltips[i].text += " " + tooltips[i].Name;
            }
        }
    }
}