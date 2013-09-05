using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cosmic_Escape
{
    class Camera
    {
        public Matrix transform;
        Viewport view;
        Vector2 center;

        public Camera(Viewport newView)
        {
            view = newView;
        }

        public void Update(GameTime gameTime, int mid, Player alien)
        {
            center = new Vector2(alien.getPos().X + (mid / 2) - 325, 0);
            transform = Matrix.CreateScale(new Vector3(1,1,0)) * 
                Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0));
        }

    }
}
