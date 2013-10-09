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
        Rectangle curRect;

        public Cursor(Texture2D t, Vector2 p, Game1 g, GameObject play, List<GameObject> e) : base(t, p, g)
        {
            alien = play;
            eList = e;
            curRect = new Rectangle(0, 0, t.Width, t.Height);
        }

        public override void Update()
        {
            if (Mouse.GetState().X > 1080 || Mouse.GetState().X < 270)
            {
                Mouse.SetPosition(parent.screenWidth / 2, Mouse.GetState().Y);
            }

            if (Mouse.GetState().Y > 600 || Mouse.GetState().Y < 50)
            {
                Mouse.SetPosition(Mouse.GetState().X, parent.screenHeight / 2);
            }
            cursorState = Mouse.GetState();
            pos.X = cursorState.X - 650;
            pos.Y = cursorState.Y - 6;
            pos.X += alien.getPos().X;
            curRect.X = (int)pos.X;
            curRect.Y = (int)pos.Y;

            //throwing enemies
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                foreach (Enemy e in parent.enemyList)
                {
                    if (curRect.Intersects(e.getDestRect()))
                    {
                        //allows moving the enemy
                        e.setPos(pos);
                    }

                }

            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, curRect, Color.White);
            sb.DrawString(parent.theFont, "      GameMouse X: " + pos.X + "\n      GameMouseY: " + pos.Y, pos, Color.White);
            sb.DrawString(parent.theFont, "      MainMouse X: " + cursorState.X + "\n      MainMouseY: " + cursorState.Y, alien.getPos(), Color.White);
        }
    }
}
