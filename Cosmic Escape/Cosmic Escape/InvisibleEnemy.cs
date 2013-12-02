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
    class InvisibleEnemy : Enemy
    {
        Texture2D invisibleSprite;
        //Texture2D visible;

        const float WALK_SPEED = 2.3f;

        bool isVisible = false;
        bool facingLeft = true;
        //bool chase = false;

        float totalTime;
        Platform targetPlat;

        public InvisibleEnemy(Texture2D t1, Texture2D t2,  Vector2 p, Game1 g, GameObject user)
            : base(t1, p, g, user)
        {
            invisibleSprite = t2;
            totalTime = 0.0f;
            //destRect = new Rectangle(0, 0, tex.Width, tex.Height);
            updatePoints();
        }

        public override void Update(GameTime gameTime, List<Platform> l)
        {

            totalTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;

            if (Math.Abs((pos.X - player.getPos().X)) < 200)//calculate proximity of player
            {
                if (player.getPos().X < pos.X)
                {
                    pos.X -= WALK_SPEED;
                    srcRect.X = frameCounter * tex.Width / 5;
                    facingLeft = true;
                }

                if (player.getPos().X > pos.X)
                {
                    pos.X += WALK_SPEED;
                    srcRect.X = frameCounter * tex.Width / 5;
                    facingLeft = false;
                }
                isVisible = true;
            }
            else
            {
                isVisible = false;
            }

            pos.Y += gravity;
            // Update the destination rectangle based on our position.
            destRect.X = (int)pos.X;
            destRect.Y = (int)pos.Y;

            //update the 4 points of the rectangle with new position
            updatePoints();

            //check for collision with other gameobjects;
            targetPlat = isColliding2(l); //check for collision with platforms

            //check if colliding with player
            if (destRect.Intersects(player.getDestRect()) && player.getMode())
            {
                player.setMode(false);
                player.setHealth();
                if (player.getPos().X < pos.X)
                {
                    player.setCollideFeedback(-WALK_SPEED);
                }
                else if (player.getPos().X > pos.X)
                {
                    player.setCollideFeedback(WALK_SPEED);
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
            if (isVisible == true)
            {
                if (facingLeft == true)
                {
                    spriteBatch.Draw(tex, destRect, srcRect, Color.White, 0.0f, origin, SpriteEffects.None, 1.0f);
                }
                if (facingLeft == false)
                {
                    //spriteBatch.Draw(tex, pos, destRect, srcRect, Color.White, 0.0f, origin, SpriteEffects.FlipHorizontally, 1.0f);
                    spriteBatch.Draw(tex, destRect, srcRect, Color.White, 0.0f, origin, SpriteEffects.FlipHorizontally, 1.0f);
                }
            }

            if (isVisible == false)
            {
                if (facingLeft == true)
                {
                    spriteBatch.Draw(invisibleSprite, destRect, srcRect, Color.White, 0.0f, origin, SpriteEffects.None, 1.0f);
                }
                if (facingLeft == false)
                {
                    //spriteBatch.Draw(tex, pos, destRect, srcRect, Color.White, 0.0f, origin, SpriteEffects.FlipHorizontally, 1.0f);
                    spriteBatch.Draw(invisibleSprite, destRect, srcRect, Color.White, 0.0f, origin, SpriteEffects.FlipHorizontally, 1.0f);
                }
            }

            /*if (targetPlat != null)
            {
                spriteBatch.DrawString(parent.theFont, "     Rectangle X: " + targetPlat.getDestRect().X + "\n     Rectangle Y: " + targetPlat.getDestRect().Y, pos, Color.White);
            }*/

            //spriteBatch.DrawString(parent.theFont, "p2X: " + point2.X + "\np2Y: " + point2.Y, point2, Color.Red);
            //spriteBatch.DrawString(parent.theFont, "p3X: " + point3.X + "\np3Y: " + point3.Y, point3, Color.Red);
            //spriteBatch.Draw(parent.dot, point1, Color.White);
            //spriteBatch.Draw(parent.dot, point4, Color.White);
        }

    }
}
