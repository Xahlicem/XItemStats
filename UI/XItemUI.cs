using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace XItemStats.UI {

    class XItemUI : UIState {
        public UIPanel panel;
        public UIRadio damage, crit, speed, knock, mana;
        public static bool visible = true;

        private UIText exit;

        public override void OnInitialize() {
            panel = new UIPanel();
            panel.SetPadding(0);
            panel.Left.Set(150, 0f);
            panel.Top.Set(150, 0f);
            panel.Width.Set(175f, 0f);
            panel.Height.Set(155f, 0f);
            panel.BackgroundColor = new Color(73, 94, 171);
            panel.OnMouseDown += new UIElement.MouseEvent(DragStart);
            panel.OnMouseUp += new UIElement.MouseEvent(DragEnd);

            UIText title = new UIText("Item Stats+");
            title.Top.Set(10, 0f);
            title.Left.Set(10, 0f);
            title.Width.Set(150, 0f);
            title.Height.Set(15, 0f);
            panel.Append(title);

            exit = new UIText("X");
            exit.Top.Set(5, 0f);
            exit.Left.Set(150, 0f);
            exit.Width.Set(25, 0f);
            exit.Height.Set(25, 0f);
            exit.TextColor = Color.DarkRed;
            exit.OnMouseOver += ExitEnter;
            exit.OnMouseOut += ExitExit;
            exit.OnClick += ExitClick;
            panel.Append(exit);

            UIText r0 = new UIText("Off", 0.65f);
            r0.Top.Set(30, 0f);
            r0.Left.Set(5, 0f);
            r0.Width.Set(25, 0f);
            r0.Height.Set(15, 0f);
            panel.Append(r0);

            UIText r1 = new UIText("On", 0.65f);
            r1.Top.Set(30, 0f);
            r1.Left.Set(25, 0f);
            r1.Width.Set(25, 0f);
            r1.Height.Set(15, 0f);
            panel.Append(r1);

            UIText r2 = new UIText("Alt", 0.65f);
            r2.Top.Set(30, 0f);
            r2.Left.Set(45, 0f);
            r2.Width.Set(25, 0f);
            r2.Height.Set(15, 0f);
            panel.Append(r2);

            damage = new UIRadio("Damage", 3, panel, 10, 50, XItemStats.damage);

            crit = new UIRadio("Cirtial", 3, panel, 10, 70, XItemStats.crit);

            speed = new UIRadio("Speed", 3, panel, 10, 90, XItemStats.speed);

            knock = new UIRadio("Knockback", 3, panel, 10, 110, XItemStats.knock);

            mana = new UIRadio("Mana Use", 3, panel, 10, 130, XItemStats.mana);

            base.Append(panel);
        }

        private void ExitClick(UIMouseEvent evt, UIElement listeningElement) {
            Main.PlaySound(SoundID.MenuTick);
            visible = false;
            Main.NewText("Type /Item to bring up Item Stats+ menu", Color.Gold);
        }

        private void ExitEnter(UIMouseEvent evt, UIElement listeningElement) {
            exit.TextColor = Color.Red;
        }

        private void ExitExit(UIMouseEvent evt, UIElement listeningElement) {
            exit.TextColor = Color.DarkRed;
        }

        Vector2 offset;

        public bool dragging = false;

        private void DragStart(UIMouseEvent evt, UIElement listeningElement) {
            offset = new Vector2(evt.MousePosition.X - panel.Left.Pixels, evt.MousePosition.Y - panel.Top.Pixels);
            dragging = true;
        }

        private void DragEnd(UIMouseEvent evt, UIElement listeningElement) {
            Vector2 end = evt.MousePosition;
            dragging = false;
            panel.Left.Set(end.X - offset.X, 0f);
            panel.Top.Set(end.Y - offset.Y, 0f);
            Recalculate();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch) {
            Vector2 MousePosition = new Vector2((float) Main.mouseX, (float) Main.mouseY);
            if (panel.ContainsPoint(MousePosition)) {
                Main.LocalPlayer.mouseInterface = true;
            }
            if (dragging) {
                panel.Left.Set(MousePosition.X - offset.X, 0f);
                panel.Top.Set(MousePosition.Y - offset.Y, 0f);
                Recalculate();
            }

            if (panel.Left.Pixels > Main.screenWidth - 160 || panel.Top.Pixels > Main.screenHeight - 50) {
                panel.Left.Set(Main.screenWidth - 160, 0f);
                panel.Top.Set(Main.screenHeight - 50, 0f);
                Recalculate();
            }
        }
    }

    public class UIRadio : UIElement {
        private UIToggleImage[] group;
        private string name;
        Texture2D texture = ModLoader.GetTexture("Terraria/UI/Settings_Toggle");
        public UIRadio(string name, int number, UIPanel panel, int left, int top, int on) {
            group = new UIToggleImage[number];
            for (int i = 0; i < number; i++) {
                group[i] = new UIToggleImage(texture, 14, 14, new Point(16, 0), new Point(0, 0));
                group[i].Left.Set(left + i * 20, 0f);
                group[i].Top.Set(top, 0f);
                group[i].Width.Set(16, 0f);
                group[i].Height.Set(16, 0f);
                group[i].OnClick += ButtonClicked;
                panel.Append(group[i]);
            }
            this.name = name;
            group[on].SetState(true);
            UIText text = new UIText(name);
            text.Top.Set(top, 0f);
            text.Left.Set(left + number * 20, 0f);
            text.Height.Set(15, 0f);
            text.Width.Set(95, 0f);
            panel.Append(text);
        }

        private void ButtonClicked(UIMouseEvent evt, UIElement listeningElement) {
            for (int i = 0; i < group.Length; i++) group[i].SetState(false);
            (listeningElement as UIToggleImage).SetState(true);
            Main.PlaySound(SoundID.MenuTick);

            Set();
        }

        public int GetValue() {
            int value = 0;
            for (int i = 0; i < group.Length; i++)
                if (group[i].IsOn) value = i;
            return value;
        }

        private void Set() {
            switch (name) {
                case "Damage":
                    XItemStats.damage = GetValue();
                    break;
                case "Critical":
                    XItemStats.crit = GetValue();
                    break;
                case "Speed":
                    XItemStats.speed = GetValue();
                    break;
                case "Knockback":
                    XItemStats.knock = GetValue();
                    break;
                case "Mana Use":
                    XItemStats.mana = GetValue();
                    break;
            }
        }

        public void SetValue(int value) {
            for (int i = 0; i < group.Length; i++) group[i].SetState(false);
            group[value].SetState(true);
        }
    }
}