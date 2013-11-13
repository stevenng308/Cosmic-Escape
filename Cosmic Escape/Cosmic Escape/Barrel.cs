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
    class Barrel : Platform
    {
        Vector2 vel;
        GameObject alien;
        int hp;

        public Barrel(Texture2D t, Vector2 p, GameObject user, Game1 g)
            : base(t, p, g)
        {
            alien = user;
            hp = 3;
            updatePoints();
            destRect = new Rectangle(0, 0, t.Width, t.Height);
        }

        public override void Update(GameTime gt, List<Platform> l)
        {
            pos.Y += gravity;
            // Update the destination rectangle based on our position.
            destRect.X = (int)pos.X;
            destRect.Y = (int)pos.Y;

            //update the 4 points of the rectangle with new position
            updatePoints();

            isColliding2(l);
            isCollidingDeath();
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
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, pos, Color.White);
            sb.DrawString(parent.theFont, "     Rectangle X: " + hp + "\n     Rectangle Y: " + this.getDestRect().Y, pos, Color.White);
        }

        public override void setVel(Vector2 v)
        {
            vel = v;
        }

        public override void setHealth()
        {
            hp--;
        }

        public override int getHealth()
        {
            return hp;
        }
    }
}
