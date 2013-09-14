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
    class Power
    {
        public Power()
        {

        }

        public void zeroGravity(Player p)
        {
            float gravity = p.getGravity();
            gravity = -(gravity + gravity * 0.5f);
            p.setGravity(gravity);
        }
    }
}
