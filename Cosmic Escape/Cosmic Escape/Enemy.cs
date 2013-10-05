﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Cosmic_Escape
{
    class Enemy : GameObject
    {
        //protected Texture2D sprite;
        //protected Vector2 pos;
        //int updateRate;
        //float updateCounter = 0.0f;
        const float WALK_SPEED = 1.0f;
        //bool walkLeft = true;

        GameObject player;          //instantiates a instance of player class so we can access player position and use it to move enemies towards the player.

        //int frameCounter;     // Which frame of the animation we're in (a value between 0 and 23)
        //float frameRate;      // This should always be 1/24 (or 0.04167 seconds)
        //float timeCounter;      // How much time has elapsed since the last time we incremented the frame counter
        float totalTime;        // Total time elapsed

        public Enemy(Texture2D t, Vector2 p, Game1 g, GameObject user) : base(t, p, g)
        {
            totalTime = 0.0f;
            //timeCounter = 0.0f;
            //updateCounter = 0;
            //updateRate = 60;
            player = user;
        }
       
        public override void Update(GameTime gameTime, List<Platform> l)
        {

            totalTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
            
            if (player.getPos().X < pos.X)
            {
                pos.X -= WALK_SPEED;
            }
            
            if (player.getPos().X > pos.X)
            {
                pos.X += WALK_SPEED;
            }

            //if (walkLeft == true)
            //    pos.X -= moveRate;
            //else
            //    pos.X += moveRate;

            //if (pos.X <= 150)
            //    walkLeft = false;
            //if (pos.X >= 350)
            //    walkLeft = true;

            pos.Y += gravity;

            //update the 4 points of the rectangle with new position
            updatePoints();

            //check for collision with other gameobjects;
            isColliding(l); //check for collision
        }


        //update the 4 corners of a sprite's rectangle
        //change this if the sprite sheet changes
        public override void updatePoints()
        {
            point1 = pos;
            point2.X = pos.X + tex.Width;
            point2.Y = pos.Y;
            point3.X = pos.X + tex.Width;
            point3.Y = pos.Y + tex.Height;
            point4.X = pos.X;
            point4.Y = pos.Y + tex.Height;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, pos, Color.White);
        }


        public Vector2 getPosition
        {
            get
            {
                return pos;
            }
        }
    }
}