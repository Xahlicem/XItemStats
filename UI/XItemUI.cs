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
        internal const float width = 195f;
        internal const float height = 155f;
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
            Panel.Width.Set(width, 0f);
            Panel.Height.Set(height, 0f);
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
            exit.Left.Set(width - 25f, 0f);
            exit.Width.Set(25, 0f);
            exit.Height.Set(25, 0f);
            exit.TextColor = Color.DarkRed;
            exit.OnMouseOver += ExitEnter;
            exit.OnMouseOut += ExitExit;
            exit.OnClick += ExitClick;
            Panel.Append(exit);

            UIText textOff = new UIText("Off", 0.65f);
            textOff.Top.Set(30, 0f);
            textOff.Left.Set(5, 0f);
            textOff.Width.Set(25, 0f);
            textOff.Height.Set(15, 0f);
            textOff.TextColor = Color.LightGray;
            textOff.OnMouseOver += ExitEnter;
            textOff.OnMouseOut += ExitExit;
            textOff.OnClick += SetAll;
            Panel.Append(textOff);

            UIText textOn = new UIText("On", 0.65f);
            textOn.Top.Set(30, 0f);
            textOn.Left.Set(25, 0f);
            textOn.Width.Set(25, 0f);
            textOn.Height.Set(15, 0f);
            textOn.TextColor = Color.LightGray;
            textOn.OnMouseOver += ExitEnter;
            textOn.OnMouseOut += ExitExit;
            textOn.OnClick += SetAll;
            Panel.Append(textOn);

            UIText textAlt = new UIText("Alt", 0.65f);
            textAlt.Top.Set(30, 0f);
            textAlt.Left.Set(45, 0f);
            textAlt.Width.Set(25, 0f);
            textAlt.Height.Set(15, 0f);
            textAlt.TextColor = Color.LightGray;
            textAlt.OnMouseOver += ExitEnter;
            textAlt.OnMouseOut += ExitExit;
            textAlt.OnClick += SetAll;
            Panel.Append(textAlt);

            UIText textNum = new UIText("#", 0.65f);
            textNum.Top.Set(30, 0f);
            textNum.Left.Set(65, 0f);
            textNum.Width.Set(25, 0f);
            textNum.Height.Set(15, 0f);
            Panel.Append(textNum);

            Damage = new UIRadio("Damage", 3, Panel, 10, 50, XItemStats.Damage);

            Crit = new UIRadio("Critical", 3, Panel, 10, 70, XItemStats.Crit);

            Speed = new UIRadio("Speed", 4, Panel, 10, 90, XItemStats.Speed);

            Knock = new UIRadio("Knockback", 4, Panel, 10, 110, XItemStats.Knock);

            Mana = new UIRadio("Mana Use", 3, Panel, 10, 130, XItemStats.Mana);

            base.Append(Panel);
        }

        private void SetAll(UIMouseEvent evt, UIElement listeningElement) {
            int i = 0;
            switch ((listeningElement as UIText).Text) {
                case "Off":
                    i = 0;
                    break;
                case "On":
                    i = 1;
                    break;
                case "Alt":
                    i = 2;
                    break;
            }
            Damage.SetValue(i);
            Crit.SetValue(i);
            Speed.SetValue(i);
            Knock.SetValue(i);
            Mana.SetValue(i);

            Main.PlaySound(SoundID.MenuTick);
        }

        private void ExitClick(UIMouseEvent evt, UIElement listeningElement) {
            Main.PlaySound(SoundID.MenuTick);
            XItemStats.Visible = false;
            Main.NewText("Type /Item to bring up Item Stats+ menu", Color.Gold);
        }

        private void ExitEnter(UIMouseEvent evt, UIElement listeningElement) {
            UIText text = (UIText) listeningElement;
            switch (((UIText) listeningElement).Text) {
                default : text.TextColor = Color.White;
                break;
                case "X":
                        text.TextColor = Color.Red;
                    break;
            }
        }

        private void ExitExit(UIMouseEvent evt, UIElement listeningElement) {
            UIText text = (UIText) listeningElement;
            switch (((UIText) listeningElement).Text) {
                default : text.TextColor = Color.LightGray;
                break;
                case "X":
                        text.TextColor = Color.DarkRed;
                    break;
            }
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
            if (Panel.Left.Pixels > Main.screenWidth - width) {
                Panel.Left.Set(Main.screenWidth - width, 0f);
                Recalculate();
            }
            if (Panel.Top.Pixels > Main.screenHeight - height) {
                Panel.Top.Set(Main.screenHeight - height, 0f);
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
            text.Left.Set(left + 4 * 20, 0f);
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

            Set();
        }
    }
}