using System;
using System.Collections.Generic;
using System.Linq;
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
    public class Explosion
    {
        public Texture2D tex;
        public Rectangle rect;
        public Vector2 pos, vel, origin;
        public float rotation;
        public bool isAlive;
        public int age;
        public Color color;

        public Explosion(Texture2D t, Vector2 initialPosition, Vector2 initialVelocity)
        {
            tex = t;
            pos = initialPosition;
            rect = new Rectangle((int)pos.X, (int)pos.Y, t.Width, t.Height / 4);
            vel = initialVelocity;
            origin = new Vector2(t.Width / 2, t.Height / 2);
            rotation = 0;
            isAlive = true;
            age = 255;
            color = new Color(255, 255, 255, 255);
        }

        public void Update()
        {
            age -= 3;
            if (age < 0)
            {
                isAlive = false;
            }

            //bounce (gravity)
            vel.Y += 0.03f;
            if (pos.Y > 500)
            {
                vel.Y = -vel.Y * 0.8f;
                pos += vel;
                rect.Width += (int)vel.Y * 8;
            }

            pos += vel * 4;

            //stretch texture
            rect.Width += (int)vel.Y * 4;

            //rotation
            rotation = (float)Math.Atan2(vel.Y, vel.X);

            //rect.Height = (int)vel.Y;
            rect.X = (int)pos.X;
            rect.Y = (int)pos.Y;
        }

        public void Draw(SpriteBatch sb)
        {
            color.G = color.A = (byte)age;
            color.B = (byte)(age / 2);
            sb.Draw(tex, rect, null, color, (float)rotation, origin, SpriteEffects.None, 1.0f);
        }
    }
}
