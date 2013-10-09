using System;
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
        bool chase = false;
        bool facingLeft = true;

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
            destRect = new Rectangle(0, 0, tex.Width, tex.Height);
        }
       
        public override void Update(GameTime gameTime, List<Platform> l)
        {

            totalTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;

            if (Math.Abs((pos.X - player.getPos().X)) < 300 || chase == true)//calculate proximity of player
            {
                if (player.getPos().X < pos.X)
                {
                    pos.X -= WALK_SPEED;
                    facingLeft = true;
                }

                if (player.getPos().X > pos.X)
                {
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

            //check for collision with other gameobjects;
            isColliding(l); //check for collision with platforms

            //check if colliding with player
            if (destRect.Intersects(player.getDestRect()) && player.getMode())
            {
                player.setMode(false);
                player.setHealth(1);
                if (player.getPos().X < pos.X)
                {
                    player.setCollideFeedback(-WALK_SPEED);
                }
                else if (player.getPos().X > pos.X)
                {
                    player.setCollideFeedback(WALK_SPEED);
                }
            }
        }


        //update the 4 corners of a sprite's rectangle
        //change this if the sprite sheet changes that has animations
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
            if (facingLeft == true)
            {
                spriteBatch.Draw(tex, pos, Color.White);
            }
            if (facingLeft == false)
            {
                //spriteBatch.Draw(tex, pos, destRect, srcRect, Color.White, 0.0f, origin, SpriteEffects.FlipHorizontally, 1.0f);
                spriteBatch.Draw(tex, pos, null, Color.White, 0.0f, origin, 1, SpriteEffects.FlipHorizontally, 0.0f);
            }
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
