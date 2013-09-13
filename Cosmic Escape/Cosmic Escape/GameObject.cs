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
        protected bool isCollide;         // flag for rectangle collision
        protected Vector2 point1, point2, point3, point4;

        public GameObject()
        {
            //default constructor will never be invoked with 0 arguments
            tex = null;
            pos = new Vector2(0, 0);
            parent = new Game1();
            isCollide = false;

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

        public virtual void Update(GameTime gametime, List<Platform> l)
        {
            foreach (Platform plat in l)
            {
                //this.point3.X < plat.point2.X && this.point4.X > plat.point1.X && 
                //&& this.point3.Y >= plat.getDestRect().Top && this.point4.Y >= plat.getDestRect().Top
                if (this.point3.X < plat.point2.X && this.point4.X > plat.point1.X && this.point3.Y >= plat.getDestRect().Top + 7 && this.point4.Y >= plat.getDestRect().Top + 7)
                //if (this.destRect.Intersects(plat.getDestRect()))
                {
                    this.isCollide = true;
                    break;
                }
                else
                {
                    this.isCollide = false;
                }
            }
        }

        public virtual void Draw(SpriteBatch sb)
        {
            return;
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
    }
}
