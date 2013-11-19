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
        Rectangle rect1 = new Rectangle(-1920, -20, 1920, 1024);  //this is the rectangle that is behind the player initially
        Rectangle rect2 = new Rectangle(0, -20, 1920, 1024);      //this is the rectangle the player starts inside
        Rectangle rect3 = new Rectangle(1920, -20, 1920, 1024);   //this is the rectangele that is in front of the player initially

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

            //Scrolling background
            rect1.X--;
            rect2.X--;
            rect3.X--;

            //If statements draw rectangles based on player's position. This gives a rectangle behind and in front of the player at all times.
            if(rect1.X + texBg.Width <= (playerPos.X))
            {
                rect3.X = rect2.X + texBg.Width;
            }

            if (rect2.X + texBg.Width <= (playerPos.X))
            {
                rect1.X = rect3.X + texBg.Width;
            }

            if (rect3.X + texBg.Width <= (playerPos.X))
            {
                rect2.X = rect1.X + texBg.Width;
            }

        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texBg, rect1, Color.White);
            sb.Draw(texBg, rect2, Color.White);
            sb.Draw(texBg, rect3, Color.White);
        }

    }
}
