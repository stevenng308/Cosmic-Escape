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
        const float WALK_SPEED = 1.4f;
        //bool walkLeft = true;
        bool chase = false;
        bool facingLeft = true;
        Platform targetPlat;

        protected GameObject player;          //instantiates a instance of player class so we can access player position and use it to move enemies towards the player.

        protected int frameCounter;     // Which frame of the animation we're in (a value between 0 and 23)
        protected float frameRate;      // This should always be 1/24 (or 0.04167 seconds)
        protected float timeCounter;      // How much time has elapsed since the last time we incremented the frame counter
        float totalTime;        // Total time elapsed

        public Enemy(Texture2D t, Vector2 p, Game1 g, GameObject user) : base(t, p, g)
        {
            totalTime = 0.0f;
            frameCounter = 0;
            frameRate = 1.0f / 5.0f;
            player = user;
            srcRect = new Rectangle(0, 0, tex.Width / 5, tex.Height);
            destRect = new Rectangle(0, 0, tex.Width / 5, tex.Height);
            updatePoints();
        }
       
        public override void Update(GameTime gameTime, List<Platform> l)
        {

            totalTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;

            if (Math.Abs((pos.X - player.getPos().X)) < 350 || chase == true)//calculate proximity of player
            {
                if (player.getPos().X < pos.X)
                {
                    srcRect.X = frameCounter * tex.Width / 5;
                    pos.X -= WALK_SPEED;
                    facingLeft = true;
                }

                if (player.getPos().X > pos.X)
                {
                    srcRect.X = frameCounter * tex.Width / 5;
                    pos.X += WALK_SPEED;
                    facingLeft = false;
                }
                chase = true;
            }

            pos.Y += gravity;
            // Update the destination rectangle based on our position.
            destRect.X = (int)pos.X;
            destRect.Y = (int)pos.Y;

            //update the 4 points of the rectangle with new position
            updatePoints();

            //check for collision with platforms;
            targetPlat = isColliding2(l); //check for collision with platforms

            //check if colliding with player
            if (destRect.Intersects(player.getDestRect()) && player.getMode())
            {
                player.setMode(false);
                player.setHealth();
                if (player.getPos().X < pos.X)
                {
                    player.setCollideFeedback(-WALK_SPEED * 1.25f);
                }
                else if (player.getPos().X > pos.X)
                {
                    player.setCollideFeedback(WALK_SPEED * 1.25f);
                }
            }

            //frame rate
            timeCounter += gameTime.ElapsedGameTime.Milliseconds / 1000f;
            if (timeCounter >= frameRate)
            {
                if (frameCounter != 1) //check so it does not go to the 24th frame
                {
                    frameCounter++; //increment frame by 1
                    timeCounter -= frameRate; //decrement updatecounter
                }
                else
                {
                    frameCounter = 0; //reset frameCounter
                    timeCounter -= frameRate; //decrement updatecounter
                }
            }
        }


        //update the 4 corners of a sprite's rectangle
        //change this if the sprite sheet changes that has animations
        public override void updatePoints()
        {
            point1 = pos;
            point2.X = pos.X + tex.Width / 5;
            point2.Y = pos.Y;
            point3.X = pos.X + tex.Width / 5;
            point3.Y = pos.Y + tex.Height;
            point4.X = pos.X;
            point4.Y = pos.Y + tex.Height;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawString(parent.theFont, "     p2Y: " + point2.Y + "\n      p3Y: " + point3.Y, pos, Color.White);
            if (facingLeft == true)
            {
                spriteBatch.Draw(tex, destRect, srcRect, Color.White, 0.0f, origin, SpriteEffects.None, 1.0f);
            }
            if (facingLeft == false)
            {
                //spriteBatch.Draw(tex, pos, destRect, srcRect, Color.White, 0.0f, origin, SpriteEffects.FlipHorizontally, 1.0f);
                spriteBatch.Draw(tex, destRect, srcRect, Color.White, 0.0f, origin, SpriteEffects.FlipHorizontally, 1.0f);
            }

            /*if (targetPlat != null)
            {
                spriteBatch.DrawString(parent.theFont, "     Rectangle X: " + targetPlat.getDestRect().X + "\n     Rectangle Y: " + targetPlat.getDestRect().Y, pos, Color.White);
            }*/

            //spriteBatch.DrawString(parent.theFont, "p2X: " + point2.X + "\np2Y: " + point2.Y, point2, Color.Red);
            //spriteBatch.DrawString(parent.theFont, "p3X: " + point3.X + "\np3Y: " + point3.Y, point3, Color.Red);
            //spriteBatch.Draw(parent.dot, point2, Color.White);
            //spriteBatch.Draw(parent.dot, point3, Color.White);
        }


        public Vector2 getPosition
        {
            get
            {
                return pos;
            }
        }

        public override float getWalkSpeed()
        {
            return WALK_SPEED;
        }
    }
}
