using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace XItemStats.Items {

    public class GlobalItemDamageMod : GlobalItem {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (XItemStats.Damage == 0 || item.damage <= 0 || Main.netMode == NetmodeID.Server) return;
            Player player = Main.player[item.owner];
            for (int i = 0; i < tooltips.Count; i++)
                if (tooltips[i].Name.Equals("Damage")) try {
                    string[] text = tooltips[i].text.Split(' ');
                    if (!text[text.Length - 1].Equals("damage")) return;
                    Item baseItem = new Item();
                    baseItem.CloneDefaults(item.type);
                    int damage = int.Parse(text[0]);
                    damage -= baseItem.damage;

                    if (damage != 0) {
                        tooltips[i].text = ((XItemStats.Damage == 1) ? text[0] : baseItem.damage.ToString()) + "(" + ((damage > 0) ? "+" : "-") + Math.Abs(damage) + ")";
                        for (int j = 1; j < text.Length; j++)
                            tooltips[i].text += " " + text[j];
                        if (XItemStats.Color) {
                            tooltips[i].isModifier = true;
                            tooltips[i].isModifierBad = (damage < 0);
                        }
                    }
                } catch (Exception) { }

        }
    }

    public class GlobalItemCritMod : GlobalItem {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (XItemStats.Crit == 0 || item.damage <= 0 || Main.netMode == NetmodeID.Server) return;
            Player player = Main.player[item.owner];
            for (int i = 0; i < tooltips.Count; i++)
                if (tooltips[i].Name.Equals("CritChance")) try {
                    string[] text = tooltips[i].text.Split(' ');
                    if (!text[text.Length - 1].Equals("chance")) return;
                    Item baseItem = new Item();
                    baseItem.CloneDefaults(item.type);
                    int crit = -4;
                    if (item.magic) crit += player.magicCrit;
                    else if (item.ranged) crit += player.rangedCrit;
                    else if (item.thrown) crit += player.thrownCrit;
                    else if (item.melee) crit += player.meleeCrit;
                    crit += (item.crit - baseItem.crit);

                    if (crit != 0) {
                        tooltips[i].text = ((XItemStats.Crit == 1) ? text[0] : (baseItem.crit + 4).ToString() + "%") + "(" + ((crit > 0) ? "+" : "-") + Math.Abs(crit) + ")";
                        for (int j = 1; j < text.Length; j++)
                            tooltips[i].text += " " + text[j];
                        if (XItemStats.Color) {
                            tooltips[i].isModifier = true;
                            tooltips[i].isModifierBad = (crit < 0);
                        }
                    }
                } catch (Exception) { }
        }
    }

    public class GlobalItemSpeedMod : GlobalItem {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (XItemStats.Speed == 0 || Main.netMode == NetmodeID.Server) return;
            Player player = Main.player[item.owner];
            for (int i = 0; i < tooltips.Count; i++)
                if (tooltips[i].Name.Equals("Speed")) try {
                    string[] text = tooltips[i].text.Split(' ');
                    if (!text[text.Length - 1].Equals("speed")) return;
                    Item baseItem = new Item();
                    baseItem.CloneDefaults(item.type);
                    int speed = 0;
                    int speedMod = 0;
                    if (item.melee) {
                        speed = item.useAnimation - 1;
                        speed = (int) ((float) speed * player.meleeSpeed);
                        speedMod = speed - (baseItem.useAnimation - 1);
                    } else {
                        speed = item.useTime;
                        speedMod = speed - baseItem.useTime;
                    }

                    tooltips[i].text = (XItemStats.Speed == 1 || XItemStats.Speed == 3) ? speed.ToString() : (speed - speedMod).ToString();
                    if (speedMod != 0 && XItemStats.Speed != 3) tooltips[i].text += "(" + ((speedMod > 0) ? "+" : "-") + Math.Abs(speedMod) + ")";
                    for (int j = 0; j < text.Length; j++)
                        tooltips[i].text += ((j == 0) ? " (" : " ") + text[j] + ((j == text.Length - 2) ? ")" : "");
                    if (XItemStats.Color && speedMod != 0) {
                        tooltips[i].isModifier = true;
                        tooltips[i].isModifierBad = (speedMod > 0);
                    }
                } catch (Exception) { }
        }
    }

    public class GlobalItemKnockMod : GlobalItem {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (XItemStats.Knock == 0 || item.knockBack == 0 || Main.netMode == NetmodeID.Server) return;
            Player player = Main.player[item.owner];
            for (int i = 0; i < tooltips.Count; i++)
                if (tooltips[i].Name.Equals("Knockback")) try {
                    string[] text = tooltips[i].text.Split(' ');
                    if (!text[text.Length - 1].Equals("knockback")) return;
                    Item baseItem = new Item();
                    baseItem.CloneDefaults(item.type);
                    float knockBack = item.knockBack;
                    float knockBackMod = (float) Math.Round(knockBack - baseItem.knockBack, 3);

                    tooltips[i].text = (XItemStats.Knock == 1 || XItemStats.Knock == 3) ? knockBack.ToString() : baseItem.knockBack.ToString();
                    if (knockBackMod != 0 && XItemStats.Knock != 3) tooltips[i].text += "(" + ((knockBackMod > 0) ? "+" : "-") + Math.Abs(knockBackMod) + ")";
                    for (int j = 0; j < text.Length; j++)
                        tooltips[i].text += ((j == 0) ? " (" : " ") + text[j] + ((j == text.Length - 2) ? ")" : "");
                    if (XItemStats.Color && knockBackMod != 0) {
                        tooltips[i].isModifier = true;
                        tooltips[i].isModifierBad = (knockBackMod < 0);
                    }
                } catch (Exception) { }
        }
    }

    public class GlobalItemManaMod : GlobalItem {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (XItemStats.Mana == 0 || item.mana <= 0 || Main.netMode == NetmodeID.Server) return;
            Player player = Main.player[item.owner];
            for (int i = 0; i < tooltips.Count; i++)
                if (tooltips[i].Name.Equals("UseMana")) try {
                    string[] text = tooltips[i].text.Split(' ');
                    if (!text[text.Length - 1].Equals("mana")) return;
                    Item baseItem = new Item();
                    baseItem.CloneDefaults(item.type);
                    int mana = int.Parse(text[1]);
                    mana -= baseItem.mana;

                    if (mana != 0) {
                        tooltips[i].text = text[0] + " " + ((XItemStats.Mana == 1) ? text[1] : baseItem.mana.ToString()) + "(" + ((mana > 0) ? "+" : "-") + Math.Abs(mana) + ")";
                        for (int j = 2; j < text.Length; j++)
                            tooltips[i].text += " " + text[j];
                        if (XItemStats.Color) {
                            tooltips[i].isModifier = true;
                            tooltips[i].isModifierBad = (mana > 0);
                        }
                    }
                } catch (Exception) { }
        }
    }

    public class GlobalItemAmmoMod : GlobalItem {
        static Dictionary<int, string> ammoTypeList = new Dictionary<int, string>(){
            {23,"Gel"},
            {40,"Arrow"},
            {97,"Bullet"},
            {283,"Dart"},
            {771,"Rocket"},
            {780,"Solution"}
        };
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (Main.netMode == NetmodeID.Server) return;
            if (item.ammo > 0) {
                for (int i = 0; i < tooltips.Count; i++){
                    if (tooltips[i].Name.Equals("Ammo")) try {
                        string ammoType = Lang.GetItemNameValue(item.ammo);
                        if (ammoTypeList.ContainsKey(item.ammo)) ammoType = ammoTypeList[item.ammo];
                        if (item.Name != ammoType)
                            tooltips[i].text = "Ammo in " + ammoType + " ([i:" + item.ammo + "]) category";
                    } catch (Exception) { }
                }
            }
            if (item.useAmmo > 0) {
                try {
                    string ammoType = Lang.GetItemNameValue(item.useAmmo);
                    if (ammoTypeList.ContainsKey(item.useAmmo)) ammoType = ammoTypeList[item.useAmmo];
                    TooltipLine line = new TooltipLine(mod, "useAmmo", "Uses " + ammoType + " ([i:" + item.useAmmo + "]) for ammo");
                    tooltips.Add(line);
                } catch (Exception) { }
            }
        }
    }

    public class GlobalItemMod : GlobalItem {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (XItemStats.Debug == 0 || Main.netMode == NetmodeID.Server) return;
            Player player = Main.player[item.owner];
            for (int i = 0; i < tooltips.Count; i++) {
                tooltips[i].text += " " + tooltips[i].Name + "-" + tooltips[i].mod;
            }
        }
    }

    public class ColorItemMod : GlobalItem {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            if (!XItemStats.Color || Main.netMode == NetmodeID.Server) return;
            Player player = Main.player[item.owner];
            for (int i = 0; i < tooltips.Count; i++)
                if (tooltips[i].mod.Equals("Terraria") && tooltips[i].Name.Contains("Prefix") && !tooltips[i].Name.Contains("PrefixAcc") && !tooltips[i].Name.Equals("PrefixSize") && !tooltips[i].Name.Equals("PrefixShootSpeed")) {
                    tooltips.RemoveAt(i);
                    i--;
                }

        }
    }
}
