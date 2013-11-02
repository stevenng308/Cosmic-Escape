using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Cosmic_Escape
{
    public class MenuComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        string[] menuItems;
        int selectedIndex;

        Color normal = Color.White;
        Color hilite = Color.LawnGreen;

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;

        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        Vector2 position;
        float width = 0f;
        float height = 0f;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                if (selectedIndex < 0)
                    selectedIndex = 0;
                if (selectedIndex >= menuItems.Length)
                    selectedIndex = menuItems.Length - 1;
            }
        }

        public MenuComponent(Game game,
            SpriteBatch spriteBatch,
            SpriteFont spriteFont,
            string[] menuItems)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.menuItems = menuItems;
            MeasureMenu();
        }

        private void MeasureMenu()
        {
            height = 0;
            width = 700;
            foreach (string item in menuItems)
            {
                Vector2 size = spriteFont.MeasureString(item);
                if (size.X > width)
                    width = size.X;
                height += spriteFont.LineSpacing + 5;
            }
            //position of game menu text
            position = new Vector2(
                (Game.Window.ClientBounds.Width + 400) / 2,
                (Game.Window.ClientBounds.Height + 325) / 2);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) &&
                oldKeyboardState.IsKeyDown(theKey);
        }

        public override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            if (CheckKey(Keys.Down))
            {
                selectedIndex++;
                if (selectedIndex == menuItems.Length)
                    selectedIndex = 0;
            }
            if (CheckKey(Keys.Up))
            {
                selectedIndex--;
                if (selectedIndex < 0)
                    selectedIndex = menuItems.Length - 1;
            }
            base.Update(gameTime);

            oldKeyboardState = keyboardState;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            Vector2 location = position;
            Color tint;

            for (int i = 0; i < menuItems.Length; i++)
            {
                if (i == selectedIndex)
                    tint = hilite;
                else
                    tint = normal;
                spriteBatch.DrawString(
                    spriteFont,
                    menuItems[i],
                    location,
                    tint);
                location.Y += spriteFont.LineSpacing + 50; //spacing between menu options
                
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}