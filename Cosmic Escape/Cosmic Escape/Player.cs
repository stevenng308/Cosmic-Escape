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
    public class Player : GameObject
    {
        //Texture2D tex;          // The *entire* spritesheet
        //Rectangle srcRect;      // Where in the *spritesheet* we're drawing
        //Rectangle destRect;     // Where on the *screen* we're drawing
        //Vector2 pos;            // Position of the player
        //Vector2 origin;         // We're not using this, but have to have it for drawing
        //Game1 parent;           // Game1
        int state;              // The state that the player is in IDLE/WALKING/JUMPING/RUNNING...
        int facing;             // Either facing LEFT or RIGHT.
        public bool cooldown, cooldownF;          // allow for jumping
        //bool isCollide;         // flag for rectangle collision
        const int IDLE = 0;
        const int WALKING = 1;
        //const int JUMPING = 0;
        //const int RUNNING = 2;
        const int LEFT = 0;
        const int RIGHT = 1;
        const float STOPPED = 0.0f;
        const float WALK_SPEED = 3.0f;
        //const float RUN_SPEED = 4.0f;
        Rectangle tempRect;     // debugging purpose to show what rectangle is in contact
        Platform targetPlat;
        float timer, timerInvulnerable;
        Power power;
        int health;
        int powerIndex = 0;
        bool godMode;
        bool lastKey;

        int frameCounter;       // Which frame of the animation we're in (a value between 0 and 23)
        float frameRate;        // This should always be 1/24 (or 0.04167 seconds)
        float timeCounter;      // How much time has elapsed since the last time we incremented the frame counter
        float totalTime;        // Total time elapsed

        public Player(Texture2D t, Vector2 p, Game1 g) : base(t, p, g)
        {
            state = IDLE;
            frameCounter = 0;
            frameRate = 1.0f / 2.0f;
            totalTime = 0.0f;
            targetPlat = null;
            cooldown = cooldownF = false;
            isCollide = false;
            timer = 0.0f;
            timerInvulnerable = 0.0f;
            power = new Power(g);
            health = 5;
            godMode = true;
            lastKey = false;

            // we're not using this, since we're not doing rotation...
            origin = new Vector2(0, 0);
            // we'll start the source rectangle to be the idle row, frame 0
            srcRect = new Rectangle(0, 0, tex.Width / 2, tex.Height / 2);
            // the destination rect is where we're drawing on the screen
            destRect = new Rectangle((int)pos.X, (int)pos.Y, tex.Width / 2, tex.Height / 2);
        }

        public override void Update(GameTime gameTime, List<Platform> l)
        {
            // Determine which keys are down
            //bool shiftDown = Keyboard.GetState().IsKeyDown(Keys.LeftShift);
            bool aKeyDown = Keyboard.GetState().IsKeyDown(Keys.A);
            bool dKeyDown = Keyboard.GetState().IsKeyDown(Keys.D);
            bool fKeyDown = Keyboard.GetState().IsKeyDown(Keys.F);//select power
            bool fKeyUp = Keyboard.GetState().IsKeyUp(Keys.F);
            bool leftClick = (Mouse.GetState().LeftButton == ButtonState.Pressed);
            bool spaceKeyDown = Keyboard.GetState().IsKeyDown(Keys.Space);

            /*Vector2 mousePosition = new Vector2(0, 0);
            Matrix viewMatrix = parent.camera.transform;
            mousePosition.X = Mouse.GetState().X;
            mousePosition.Y = Mouse.GetState().Y;

            viewMatrix = Matrix.Invert(viewMatrix);
            mouseXZ = mousePosition.X ;
            mouseYZ = mousePosition.Y;*/

            // This aggregates the amount of time that has elapsed since the last frame was called
            totalTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;

            //moving left
            if (aKeyDown)
            {
                facing = LEFT;
                state = WALKING;
                srcRect.Y = state * tex.Height / 2;
                srcRect.X = frameCounter * tex.Width / 2;
                pos.X -= WALK_SPEED;
                
            }

            //moving right
            else if (dKeyDown)
            {
                facing = RIGHT;
                state = WALKING;
                srcRect.Y = state * tex.Height / 2 ;
                srcRect.X = frameCounter * tex.Width / 2;
                pos.X += WALK_SPEED;
                
            }

            //idle
            else
            {
                state = IDLE;
                srcRect.Y = state * tex.Height / 2;
                srcRect.X = frameCounter * tex.Width / 2;
            }

            //jumping
            if (spaceKeyDown && cooldown != true)
            {
                //state = JUMPING;
                srcRect.Y = state * tex.Height / 2;
                srcRect.X = frameCounter * tex.Width / 2;
                //pos.Y += -7.5f * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                power.jumping(this);
                timer = totalTime + 0.03f;
                //cooldown = true;
            }

            //select power
            if (fKeyDown)
            {
                lastKey = true; //fkey has been pressed now wait for release
            }
            if (fKeyUp && lastKey) //makes the change when the user releases the f key
            {
                    if (powerIndex + 1 == 2)
                        powerIndex = 0;
                    else
                        powerIndex++;
                    lastKey = false; //released
            }

            //activate power
            if (leftClick && cooldownF != true)
            {
                power.powerSelect(powerIndex, this);
            }

            //gravity
            /*foreach (Platform p in l)
            {
                isCollide = p.getDestRect().Intersects(this.destRect);
                if (isCollide || pos.Y > parent.screenHeight - 61)
                {
                    if (totalTime >= timer)
                    {
                        cooldown = false;
                    }
                    GRAVITY = 0.0f;
                    tempRect = p.getDestRect();
                    break;
                }
                else
                {
                    GRAVITY = 1.5f;
                }
            }
            if (isCollide || (pos.Y > parent.screenHeight - (tex.Height / 2 - 3)))
            {
                //gravity = 0.0f;
                if (totalTime >= timer)
                {
                    cooldown = false;
                }
            }
            else
            {
                //gravity = 1.5f;
                //targetPlat = isColliding(l); //chack for collision
            }*/

            pos.Y += gravity;
            // Update the destination rectangle based on our position.
            destRect.X = (int)pos.X;
            destRect.Y = (int)pos.Y;

            //update the 4 points of the rectangle with new position
            updatePoints();

            //check for collision with other gameobjects;
            //base.Update(gameTime, targetPlat);
            targetPlat = isColliding(l); //check for collision

            //recharge timer for power
            if (cooldownF || isCollideBot)
            {
                power.rechargeTimer(0.15, this);
            }

            //invulnerable timer
            if (!godMode)
            {
                timerInvulnerable -= 0.1f;
                if (timerInvulnerable < 0)
                {
                    godMode = true;
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
        public override void Draw(SpriteBatch sb)
        {
            //sb.DrawString(parent.theFont, "Total Time: " + totalTime + "\nFrame: " + frameCounter, parent.textPos, Color.White);
            sb.DrawString(parent.theFont, "      powerNum: " + powerIndex + "\n      PowerTime: " + (int)power.getTimer(), parent.powerPos, Color.White);
            //draw health hearts
            sb.DrawString(parent.theFont, "HP: ", parent.healthPos, Color.White);
            for (int i = 1; i <= health ; i++)
            {
                //Vector2 heartPos;
                //heartPos.X = 0 * i;
                //heartPos.Y = 0;
                parent.healthPos.X += 32 + i;
                sb.Draw(parent.heartTex, parent.healthPos, Color.White);
            }

            if (targetPlat != null)
            {
                sb.DrawString(parent.theFont, "Rectangle X: " + targetPlat.getDestRect().X + " Rectangle Y: " + targetPlat.getDestRect().Y, parent.textPos, Color.White);
            }
            // Using Draw method 5 of 7
            if (facing == LEFT)
                sb.Draw(tex, destRect, srcRect, Color.White, 0.0f, origin, SpriteEffects.FlipHorizontally, 1.0f);
            else
                sb.Draw(tex, destRect, srcRect, Color.White, 0.0f, origin, SpriteEffects.None, 1.0f);
        }
        
        //get iscolliding
        public bool getIsCollide()
        {
            return isCollide;
        }
        public bool getIsCollideTop()
        {
            return isCollideTop;
        }
        public bool getIsCollideBot()
        {
            return isCollideBot;
        }

        public override bool getMode()
        {
            return godMode;
        }

        public override void setMode(bool m)
        {
            godMode = m;
            timerInvulnerable = 10;
        }

        public override void setHealth()
        {
            health--;
        }

        public override void setCollideFeedback(float i)
        {
            if (!isCollide)
            {
                for (int j = 0; j < 10; j++)
                {
                    pos.X += i * (float)j;
                }
            }
        }

        //get play position
        public override Vector2 getPos()
        {
            return pos;
        }

        //get walk speed
        public override float getWalkSpeed()
        {
            return WALK_SPEED;
        }
    }
}
