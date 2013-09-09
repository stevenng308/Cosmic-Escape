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

namespace Cosmic_Escape
{
    public class Background
    {

        const int BACKGROUND_RATE = 60;     //controls speed of space and stars background outside the ship
        SpriteBatch spriteBatch;
        Texture2D spaceBackground;
        Texture2D shipBackground;
        int space_bg_pos;
        int ship_bg_pos;
        float backgroundCounter = 0.0f;

        Texture2D tex;
        int pos;

        public Background(Texture2D t, int p)
        {
            tex = t;
            pos = p;
        }

        public void MoveBackground(float elapsedTime)
        {
            backgroundCounter -= elapsedTime;

            if (backgroundCounter > 0)
                return;

            backgroundCounter = 1000f / BACKGROUND_RATE;

            space_bg_pos--;


            if (space_bg_pos < -spaceBackground.Width)
            {
                space_bg_pos += spaceBackground.Width;
            }

            if (ship_bg_pos < -shipBackground.Width)
            {
                ship_bg_pos += shipBackground.Width;
            }

        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(tex, new Vector2(pos, 0), Color.White);                       //draws background of the player's current view
            spriteBatch.Draw(tex, new Vector2(pos + tex.Width, 0), Color.White);           //draws background to the right of player's current view
            spriteBatch.Draw(tex, new Vector2(pos - tex.Width, 0), Color.White);           //draws background to the left of player's current view
        }

    }
}
