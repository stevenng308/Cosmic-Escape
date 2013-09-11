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
        Texture2D texBg;
        float backgroundCounter = 0.0f;
        int pos;
        Vector2 playerPos;

        public Background(Texture2D t, int p)
        {
            texBg = t;
            pos = p;
        }

        public void MoveBackground(float elapsedTime, Vector2 alienPos)
        {
            playerPos = alienPos;
            backgroundCounter -= elapsedTime;

            if (backgroundCounter > 0)
                return;

            backgroundCounter = 1000f / BACKGROUND_RATE;
            pos--;

            if (pos < -texBg.Width)
            {
                pos += texBg.Width;
            }

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texBg, new Vector2(pos, 0), Color.White);                         //draws background of the player's current view
            sb.Draw(texBg, new Vector2(pos + texBg.Width, 0), Color.White);           //draws background to the right of player's current view
            sb.Draw(texBg, new Vector2(pos - texBg.Width, 0), Color.White);           //draws background to the left of player's current view
        }

    }
}
