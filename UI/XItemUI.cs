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
        public UIPanel Panel { get; set; }

        public UIRadio Damage { get; set; }
        public UIRadio Crit { get; set; }
        public UIRadio Speed { get; set; }
        public UIRadio Knock { get; set; }
        public UIRadio Mana { get; set; }

        private UIText exit;

        public override void OnInitialize() {
            Panel = new UIPanel();
            Panel.SetPadding(0);
            Panel.Left.Set(150, 0f);
            Panel.Top.Set(150, 0f);
            Panel.Width.Set(175f, 0f);
            Panel.Height.Set(155f, 0f);
            Panel.BackgroundColor = new Color(73, 94, 171);
            Panel.OnMouseDown += new UIElement.MouseEvent(DragStart);
            Panel.OnMouseUp += new UIElement.MouseEvent(DragEnd);

            UIText title = new UIText("Item Stats+");
            title.Top.Set(10, 0f);
            title.Left.Set(10, 0f);
            title.Width.Set(150, 0f);
            title.Height.Set(15, 0f);
            Panel.Append(title);

            exit = new UIText("X");
            exit.Top.Set(5, 0f);
            exit.Left.Set(150, 0f);
            exit.Width.Set(25, 0f);
            exit.Height.Set(25, 0f);
            exit.TextColor = Color.DarkRed;
            exit.OnMouseOver += ExitEnter;
            exit.OnMouseOut += ExitExit;
            exit.OnClick += ExitClick;
            Panel.Append(exit);

            UIText r0 = new UIText("Off", 0.65f);
            r0.Top.Set(30, 0f);
            r0.Left.Set(5, 0f);
            r0.Width.Set(25, 0f);
            r0.Height.Set(15, 0f);
            Panel.Append(r0);

            UIText r1 = new UIText("On", 0.65f);
            r1.Top.Set(30, 0f);
            r1.Left.Set(25, 0f);
            r1.Width.Set(25, 0f);
            r1.Height.Set(15, 0f);
            Panel.Append(r1);

            UIText r2 = new UIText("Alt", 0.65f);
            r2.Top.Set(30, 0f);
            r2.Left.Set(45, 0f);
            r2.Width.Set(25, 0f);
            r2.Height.Set(15, 0f);
            Panel.Append(r2);

            Damage = new UIRadio("Damage", 3, Panel, 10, 50, XItemStats.Damage);

            Crit = new UIRadio("Critical", 3, Panel, 10, 70, XItemStats.Crit);

            Speed = new UIRadio("Speed", 3, Panel, 10, 90, XItemStats.Speed);

            Knock = new UIRadio("Knockback", 3, Panel, 10, 110, XItemStats.Knock);

            Mana = new UIRadio("Mana Use", 3, Panel, 10, 130, XItemStats.Mana);

            base.Append(Panel);
        }

        private void ExitClick(UIMouseEvent evt, UIElement listeningElement) {
            Main.PlaySound(SoundID.MenuTick);
            XItemStats.Visible = false;
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
            offset = new Vector2(evt.MousePosition.X - Panel.Left.Pixels, evt.MousePosition.Y - Panel.Top.Pixels);
            dragging = true;
        }

        private void DragEnd(UIMouseEvent evt, UIElement listeningElement) {
            Vector2 end = evt.MousePosition;
            dragging = false;
            Panel.Left.Set(end.X - offset.X, 0f);
            Panel.Top.Set(end.Y - offset.Y, 0f);
            Recalculate();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch) {
            Vector2 MousePosition = new Vector2((float) Main.mouseX, (float) Main.mouseY);
            if (Panel.ContainsPoint(MousePosition)) {
                Main.LocalPlayer.mouseInterface = true;
            }
            if (dragging) {
                Panel.Left.Set(MousePosition.X - offset.X, 0f);
                Panel.Top.Set(MousePosition.Y - offset.Y, 0f);
                Recalculate();
            }
            if (Panel.Left.Pixels > Main.screenWidth - 175) {
                Panel.Left.Set(Main.screenWidth - 175, 0f);
                Recalculate();
            }
            if (Panel.Top.Pixels > Main.screenHeight - 155) {
                Panel.Top.Set(Main.screenHeight - 155, 0f);
                Recalculate();
            }
            if (Panel.Left.Pixels < 0) {
                Panel.Left.Set(0, 0f);
                Recalculate();
            }
            if (Panel.Top.Pixels < 0) {
                Panel.Top.Set(0, 0f);
                Recalculate();
            }

        }

        public override void Recalculate() {
            base.Recalculate();
            XItemStats.SetUIPoint(this);
        }
    }

    public class UIRadio : UIElement {
        private UIToggleImage[] group;
        private string name;
        static Texture2D Texture = ModLoader.GetTexture("Terraria/UI/Settings_Toggle");
        public UIRadio(string name, int number, UIPanel panel, int left, int top, int on) {
            group = new UIToggleImage[number];
            for (int i = 0; i < number; i++) {
                group[i] = new UIToggleImage(Texture, 14, 14, new Point(16, 0), new Point(0, 0));
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
                    XItemStats.Damage = GetValue();
                    break;
                case "Critical":
                    XItemStats.Crit = GetValue();
                    break;
                case "Speed":
                    XItemStats.Speed = GetValue();
                    break;
                case "Knockback":
                    XItemStats.Knock = GetValue();
                    break;
                case "Mana Use":
                    XItemStats.Mana = GetValue();
                    break;
            }
        }

        public void SetValue(int value) {
            for (int i = 0; i < group.Length; i++) group[i].SetState(false);
            group[value].SetState(true);
        }
    }
}