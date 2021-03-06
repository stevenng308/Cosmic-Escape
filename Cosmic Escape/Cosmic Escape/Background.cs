﻿using System;
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
        protected const int BACKGROUND_RATE = 60;     //controls speed of space and stars background outside the ship
        protected Texture2D texBg;
        protected float backgroundCounter = 0.0f;
        protected int bgPos;
        int counter;
        protected Vector2 playerPos, startPos;
        protected Player alien;
        
        public Background()
        {
        }

        public Background(Texture2D t)
        {
            texBg = t;
            playerPos = new Vector2(0, 0);
            counter = 0;
            bgPos = 0;
        }

        public void Update(GameObject p)
        {
            playerPos.X = p.getPos().X; 
        }

        public virtual void Draw(SpriteBatch sb)
        {
            for (counter = 0; counter < 7; counter++)
            {
                sb.Draw(texBg, new Vector2((800 * counter), 0), Color.White);            //draws background to the right of player's current view
            }
            sb.Draw(texBg, new Vector2(bgPos - texBg.Width, 0), Color.White);           //draws background to the left of player's current view
        }

    }
}
