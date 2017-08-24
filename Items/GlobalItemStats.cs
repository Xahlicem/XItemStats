using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace XItemStats.Items {
    public class GlobalItemDamageMod : GlobalItem {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (XItemStats.damage == 0 || item.damage <= 0) return;
            Player player = Main.player[item.owner];
            for (int i = 0; i < tooltips.Count; i++) {
                if (tooltips[i].Name.Equals("Damage")) {
                    string[] text = tooltips[i].text.Split(' ');
                    Item baseItem = new Item();
                    baseItem.CloneDefaults(item.type);
                    int damage = int.Parse(text[0]);
                    damage -= baseItem.damage;

                    if (damage != 0) {
                        tooltips[i].text = ((XItemStats.damage == 1) ? text[0] : baseItem.damage.ToString()) + "(" + ((damage > 0) ? "+" : "-") + Math.Abs(damage) + ")";
                        for (int j = 1; j < text.Length; j++)
                            tooltips[i].text += " " + text[j];
                    }
                }
            }
        }
    }
    public class GlobalItemCritMod : GlobalItem {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (XItemStats.crit == 0 || item.damage <= 0) return;
            Player player = Main.player[item.owner];
            for (int i = 0; i < tooltips.Count; i++) {
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
                        tooltips[i].text = ((XItemStats.crit == 1) ? text[0] : (baseItem.crit + 4).ToString() + "%") + "(" + ((crit > 0) ? "+" : "-") + Math.Abs(crit) + ")";
                        for (int j = 1; j < text.Length; j++)
                            tooltips[i].text += " " + text[j];
                    }
                }
            }
        }
    }
    public class GlobalItemSpeedMod : GlobalItem {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (XItemStats.speed == 0) return;
            Player player = Main.player[item.owner];
            for (int i = 0; i < tooltips.Count; i++) {
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

                    tooltips[i].text = (XItemStats.speed == 1) ? speed.ToString() : (speed - speedMod).ToString();
                    if (speedMod != 0) tooltips[i].text += "(" + ((speedMod > 0) ? "+" : "-") + Math.Abs(speedMod) + ")";
                    for (int j = 0; j < text.Length; j++)
                        tooltips[i].text += ((j == 0) ? " (" : " ") + text[j] + ((j == text.Length - 2) ? ")" : "");
                }
            }
        }
    }
    public class GlobalItemKnockMod : GlobalItem {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (XItemStats.knock == 0) return;
            Player player = Main.player[item.owner];
            for (int i = 0; i < tooltips.Count; i++) {
                if (tooltips[i].Name.Equals("Knockback")) {
                    string[] text = tooltips[i].text.Split(' ');
                    Item baseItem = new Item();
                    baseItem.CloneDefaults(item.type);
                    float knockBack = item.knockBack;
                    float knockBackMod = knockBack - baseItem.knockBack;

                    tooltips[i].text = (XItemStats.knock == 1) ? knockBack.ToString() : baseItem.knockBack.ToString();
                    if (knockBackMod != 0) tooltips[i].text += "(" + ((knockBackMod > 0) ? "+" : "-") + Math.Abs(knockBackMod) + ")";
                    for (int j = 0; j < text.Length; j++)
                        tooltips[i].text += ((j == 0) ? " (" : " ") + text[j] + ((j == text.Length - 2) ? ")" : "");

                }
            }
        }
    }
    public class GlobalItemManaMod : GlobalItem {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (XItemStats.mana == 0 || item.mana <= 0) return;
            Player player = Main.player[item.owner];
            for (int i = 0; i < tooltips.Count; i++) {
                if (tooltips[i].Name.Equals("UseMana")) {
                    string[] text = tooltips[i].text.Split(' ');
                    Item baseItem = new Item();
                    baseItem.CloneDefaults(item.type);
                    int mana = int.Parse(text[1]);
                    mana -= baseItem.mana;

                    if (mana != 0) {
                        tooltips[i].text = text[0] + " " + ((XItemStats.mana == 1) ? text[1] : baseItem.mana.ToString()) + "(" + ((mana > 0) ? "+" : "-") + Math.Abs(mana) + ")";
                        for (int j = 2; j < text.Length; j++)
                            tooltips[i].text += " " + text[j];
                    }
                }
            }
        }
    }
    public class GlobalItemMod : GlobalItem {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (XItemStats.debug == 0) return;
            Player player = Main.player[item.owner];
            for (int i = 0; i < tooltips.Count; i++) {
                tooltips[i].text += " " + tooltips[i].Name;
            }
        }
    }
}