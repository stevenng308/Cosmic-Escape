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
    public class Platform
    {
        Texture2D tex;
        Rectangle destRect;
        Vector2 pos;
        //string blockInfo;

        public Platform(Texture2D t, Vector2 p)
        {
            tex = t;
            pos = p;
            destRect = new Rectangle((int)pos.X, (int)pos.Y, tex.Width, tex.Height);
            //blockInfo = info;
        }

        public void Update(Player play)
        {
            
        }

        public void Draw(SpriteBatch sb)
        {
            /*string[] tempArray = null;
            for (int i = 0; i < blockInfo.Length; i++)
            {
                tempArray = blockInfo.Split(',');
            }

            destRect.X = Convert.ToInt32(tempArray[0]);
            destRect.Y = Convert.ToInt32(tempArray[1]);*/
            sb.Draw(tex, destRect, Color.White);
        }

        public Rectangle getDestRect()
        {
            return destRect;
        }
    }
}
