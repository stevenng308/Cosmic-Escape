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
    public class Platform : GameObject
    {
        //Texture2D tex;
        //Rectangle destRect;
        //Vector2 pos;

        public Platform(Texture2D t, Vector2 p)
        {
            tex = t;
            pos = p;
            destRect = new Rectangle((int)pos.X, (int)pos.Y, tex.Width, tex.Height);
        }

        public void Update(Player play)
        {
            //default update is invoked from GameObject
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, destRect, Color.White);
        }

        //get the rectangle of the platform
        public Rectangle getDestRect()
        {
            return destRect;
        }
    }
}
