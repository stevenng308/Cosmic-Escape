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
    public class Cursor : GameObject
    {
        MouseState cursorState;
        GameObject alien;
        List<GameObject> eList;

        public Cursor(Texture2D t, Vector2 p, Game1 g, GameObject play, List<GameObject> e) : base(t, p, g)
        {
            alien = play;
            eList = e;
        }

        public override void Update()
        {
            /*if (Mouse.GetState().X > 1080 || Mouse.GetState().X < 270)
            {
                Mouse.SetPosition(parent.screenWidth / 2, Mouse.GetState().Y);
            }

            if (Mouse.GetState().Y > 500 || Mouse.GetState().Y < 120)
            {
                Mouse.SetPosition(Mouse.GetState().X, parent.screenHeight / 2);
            }*/
            cursorState = Mouse.GetState();
            pos.X = cursorState.X - 650;
            pos.Y = cursorState.Y - 6;
            pos.X += alien.getPos().X;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, pos, Color.White);
        }
    }
}
