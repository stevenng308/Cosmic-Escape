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
    public class GameObject
    {
        protected Texture2D tex;          // The *entire* spritesheet
        protected Rectangle srcRect;      // Where in the *spritesheet* we're drawing
        protected Rectangle destRect;     // Where on the *screen* we're drawing
        protected Vector2 pos;            // Position of the player
        protected Vector2 origin;         // We're not using this, but have to have it for drawing
        protected Game1 parent;
        protected bool isCollide, isCollideTop, isCollideBot;         // flag for rectangle collision
        protected Vector2 point1, point2, point3, point4;
        protected float gravity;

        public GameObject()
        {
            //default constructor will never be invoked with 0 arguments
            tex = null;
            pos = new Vector2(0, 0);
            parent = new Game1();
            isCollide = isCollideBot = isCollideTop = false;
            gravity = 1.5f;

            // we're not using this, since we're not doing rotation...
            origin = new Vector2(0, 0);
            // we'll start the source rectangle to be the idle row, frame 0
            srcRect = new Rectangle(0, 0, 0, 0);
            // the destination rect is where we're drawing on the screen
            destRect = new Rectangle(0, 0, 64, 64);
        }
        public GameObject(Texture2D t, Vector2 p, Game1 g)
        {
            tex = t;
            pos = p;
            parent = g;
        }

        public virtual void Update(GameTime gametime, Platform plat)
        {
            if (plat == null)
            {
                return;
            }
            else if ((this.point4.X < plat.point1.X && this.point3.X < plat.point1.X) || (this.point4.X > plat.point2.X && this.point3.X > plat.point2.X) && this.isCollide)//allows falling on edge of platforms
            {
                this.isCollide = false;
                    //break;
            }
            /*
            else if (this.point3.Y < (plat.point2.Y - 10))
            {
                this.isCollide = false;
            }*/
        }

        public virtual void Draw(SpriteBatch sb)
        {
            
        }

        public virtual void updatePoints()
        {
            point1 = pos;
            point2.X = pos.X + tex.Width / 2;
            point2.Y = pos.Y;
            point3.X = pos.X + tex.Width / 2;
            point3.Y = pos.Y + tex.Height / 2;
            point4.X = pos.X;
            point4.Y = pos.Y + tex.Height / 2;
        }

        public Platform isColliding(List<Platform> l)
        {
            foreach (Platform plat in l)
            {
                if (this.point3.X < plat.point2.X && this.point4.X > plat.point1.X && this.point3.Y >= plat.point2.Y + 7 && this.point4.Y >= plat.point1.Y + 7
                        && this.point3.Y < plat.point3.Y && this.point4.Y < plat.point4.Y) //stop falling through platforms
                {
                    this.isCollideBot = true;
                    this.gravity = 0;
                    return plat;
                }
                else if (this.point2.X < plat.point3.X && this.point1.X > plat.point4.X && this.point2.Y <= plat.point3.Y && this.point1.Y <= plat.point4.Y
                    && this.point2.Y > plat.point2.Y && this.point1.Y > plat.point1.Y) //cannot go below platforms
                {
                    this.isCollideTop = true;
                    this.pos.Y += 0;
                    return plat;
                }
                else if (this.point4.X <= plat.point3.X && this.point4.X > plat.point4.X && this.point1.Y > plat.point2.Y && this.point1.Y < plat.point3.Y)//cannot go into platform from the right
                {
                    this.isCollide = true;
                    this.pos.X += this.getWalkSpeed();
                    return plat;
                }
                else if (this.point3.X >= plat.point4.X && this.point3.X < plat.point3.X && this.point1.Y > plat.point2.Y && this.point1.Y < plat.point3.Y)//cannot go into platform from the left
                {
                    this.isCollide = true;
                    this.pos.X -= this.getWalkSpeed();
                    return plat;
                }
                else if ((pos.Y > parent.screenHeight - (tex.Height / 2 - 3)))
                {
                    this.isCollideBot = true;
                    this.gravity = 0;
                }
                else
                {
                    this.isCollide = false;
                    this.isCollideTop = false;
                    this.isCollideBot = false;
                    this.gravity = 1.5f;
                }
            }
            return null;
        }

        public virtual float getWalkSpeed()
        {
            return 0.0f;
        }

        public virtual float getGravity()
        {
            return 0.0f;
        }

        public virtual Vector2 getPos()
        {
            return new Vector2(0,0);
        }
    }

}
