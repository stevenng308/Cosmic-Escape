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
    class Space : Background
    {

        public Space(Texture2D t)
        {
            texBg = t;
        }

        public void MoveBackground(float elapsedTime, GameObject p)
        {
            playerPos = p.getPos();
            backgroundCounter -= elapsedTime;
            
            if (backgroundCounter > 0)
                return;

            backgroundCounter = 1000f / BACKGROUND_RATE;
            bgPos--;

            if (bgPos < -texBg.Width)
            {
                bgPos += texBg.Width;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texBg, new Vector2(bgPos, 0), Color.White);                         //draws background of the player's current view
            sb.Draw(texBg, new Vector2(bgPos + texBg.Width, 0), Color.White);           //draws background to the right of player's current view
            sb.Draw(texBg, new Vector2(bgPos - texBg.Width, 0), Color.White);           //draws background to the left of player's current view
        }

    }
}
