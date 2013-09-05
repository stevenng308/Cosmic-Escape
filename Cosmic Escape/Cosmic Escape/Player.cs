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
    public class Player
    {
        Texture2D tex;          // The *entire* spritesheet
        Rectangle srcRect;      // Where in the *spritesheet* we're drawing
        Rectangle destRect;     // Where on the *screen* we're drawing
        Vector2 pos;            // Position of the player
        Vector2 origin;         // We're not using this, but have to have it for drawing
        Game1 parent;           // Game1
        int state;              // The state that the player is in IDLE/WALKING/JUMPING/RUNNING...
        int facing;             // Either facing LEFT or RIGHT.
        bool cooldown;          // allow for jumping
        bool isCollide;         // flag for rectangle collision
        const int IDLE = 0;
        const int WALKING = 1;
        //const int JUMPING = 0;
        //const int RUNNING = 2;
        const int LEFT = 0;
        const int RIGHT = 1;
        const float STOPPED = 0.0f;
        const float WALK_SPEED = 1.5f;
        //const float RUN_SPEED = 4.0f;
        float GRAVITY = 1.5f;
        Rectangle tempRect;     // debugging purpose to show what rectangle is in contact

        int frameCounter;       // Which frame of the animation we're in (a value between 0 and 23)
        float frameRate;        // This should always be 1/24 (or 0.04167 seconds)
        float timeCounter;      // How much time has elapsed since the last time we incremented the frame counter
        float totalTime;        // Total time elapsed

        public Player(Texture2D t, Vector2 p, Game1 g)
        {
            tex = t;
            pos = p;
            parent = g;
            state = IDLE;
            frameCounter = 0;
            frameRate = 1.0f / 2.0f;
            totalTime = 0.0f;
            cooldown = false;
            isCollide = false;

            // we're not using this, since we're not doing rotation...
            origin = new Vector2(0, 0);
            // we'll start the source rectangle to be the idle row, frame 0
            srcRect = new Rectangle(0, 0, tex.Width / 2, tex.Height / 2);
            // the destination rect is where we're drawing on the screen
            destRect = new Rectangle((int)pos.X, (int)pos.Y, 64, 64);
        }
        public void Update(GameTime gameTime, List<Platform> l)
        {
            // Determine which keys are down
            //bool shiftDown = Keyboard.GetState().IsKeyDown(Keys.LeftShift);
            bool aKeyDown = Keyboard.GetState().IsKeyDown(Keys.A);
            bool dKeyDown = Keyboard.GetState().IsKeyDown(Keys.D);
            bool spaceKeyDown = Keyboard.GetState().IsKeyDown(Keys.Space);

            // This aggregates the amount of time that has elapsed since the last frame was called
            totalTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;

            //moving left
            if (aKeyDown)
            {
                facing = LEFT;
                state = WALKING;
                srcRect.Y = state * 64;
                srcRect.X = frameCounter * 64;
                pos.X -= WALK_SPEED;
                
            }

            //moving right
            else if (dKeyDown)
            {
                facing = RIGHT;
                state = WALKING;
                srcRect.Y = state * 64;
                srcRect.X = frameCounter * 64;
                pos.X += WALK_SPEED;
                
            }

            //idle
            else
            {
                state = IDLE;
                srcRect.Y = state * 64;
                srcRect.X = frameCounter * 64;
            }

            //jumping
            if (spaceKeyDown && cooldown != true)
            {
                //state = JUMPING;
                srcRect.Y = state * 64;
                srcRect.X = frameCounter * 64;
                pos.Y += -125.0f;
                cooldown = true;
            }

            //gravity
            foreach (Platform p in l)
            {
                isCollide = p.getDestRect().Intersects(this.destRect);
                if (isCollide || pos.Y > parent.screenHeight - 59)
                {
                    cooldown = false;
                    GRAVITY = 0.0f;
                    tempRect = p.getDestRect();
                    break;   
                }
                else
                {
                    GRAVITY = 1.5f;
                }
                /*if (pos.Y < parent.screenHeight - 59)
                //if (pos.Y < parent.screenHeight - 59 && isCollide != true)
                {
                    pos.Y += 1.5f;
                    //state = IDLE;
                }
                else //have the sprite reach the ground?
                {
                    //pos.Y += 0.5f;
                    //state = IDLE;
                    cooldown = false;
                }*/
            }

            // gravity
            pos.Y += GRAVITY;

            // Update the destination rectangle based on our position.
            destRect.X = (int)pos.X;
            destRect.Y = (int)pos.Y;

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
        public void Draw(SpriteBatch sb)
        {
            //sb.DrawString(parent.theFont, "Total Time: " + totalTime + "\nFrame: " + frameCounter, parent.textPos, Color.White);
            sb.DrawString(parent.theFont, "Rectangle X: " + tempRect.X + "\nRectangle Y: " + tempRect.Y, parent.textPos, Color.White);
            // Using Draw method 5 of 7
            if (facing == LEFT)
                sb.Draw(tex, destRect, srcRect, Color.White, 0.0f, origin, SpriteEffects.FlipHorizontally, 1.0f);
            else
                sb.Draw(tex, destRect, srcRect, Color.White, 0.0f, origin, SpriteEffects.None, 1.0f);
        }

        //setter when collision detected
        public void setIsCollide(bool flag)
        {
            isCollide = flag;
        }

        //get play position
        public Vector2 getPos()
        {
            return pos;
        }
    }
}
