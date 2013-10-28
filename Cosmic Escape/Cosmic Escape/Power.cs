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
        double timer;
        Game1 parent;
        public Power(Game1 g)
        {
            timer = 30.0;
            parent = g;
        }

        public void jumping(Player p)
        {
            if (p.getIsCollideBot())
            {
                float gravity = p.getGravity();
                double time = 15.0;
                for (double t = 0.1; t < time; t += 0.005)
                {
                    gravity = -(float)(gravity + t * 0.0025);
                    p.setGravity(gravity);
                }
                //p.cooldown = true;
            }
            p.setGravity(1.5f);
        }

        public void powerSelect(int i, Player p)
        {
            switch (i)
            {
                case 0: telekinesis(p, 30.0);
                    break;
                case 1: zeroGravity(p, 30.0);
                    break;
            }
        }

        //zero gravity power
        public void zeroGravity(Player p, double timeLimit)
        {
            if (timer == 0 || timer >= 30) //cannot assign the timer until it is recharged or when initialized
            {
                timer = timeLimit;
            }
            if (!p.getIsCollideTop() && timer != 0) //cannot continue floating if hitting platform from the bottom or the timer is zero
            {
                float gravity = p.getGravity();
                double time = 10.0;
                for (double t = 0.1; t < time; t += 0.005)
                {
                    gravity = -(float)(gravity + t * 0.0015);
                    p.setGravity(gravity);
                    foreach (Enemy e in parent.enemyList)
                    {
                        e.setGravity(gravity);
                    }
                    foreach (Barrel b in parent.objectList)
                    {
                        b.setGravity(gravity);
                    }
                }
                timer -= 0.8;
                if (timer <= 0)
                {
                    p.cooldownF = true;
                }
            }
            p.setGravity(1.5f); //reset to default gravity
        }

        public void telekinesis(Player p, double timeLimit)
        {
            if (timer == 0 || timer >= 30) //cannot assign the timer until it is recharged or when initialized
            {
                timer = timeLimit;
            }
            if ((Mouse.GetState().LeftButton == ButtonState.Pressed) && timer != 0)
            {
                foreach (Enemy e in parent.enemyList)
                {
                    if (parent.mouse.getDestRect().Intersects(e.getDestRect()))
                    {
                        //allows moving the enemy
                        Vector2 tempV = parent.mouse.getPos();
                        tempV.X = parent.mouse.getPos().X - 30;
                        tempV.Y = parent.mouse.getPos().Y - 30;
                        e.setPos(tempV);
                    }
                }

                foreach (Barrel b in parent.objectList)
                {
                    if (parent.mouse.getDestRect().Intersects(b.getDestRect()))
                    {
                        //allows moving the interactable objects
                        Vector2 tempV = parent.mouse.getPos();
                        tempV.X = parent.mouse.getPos().X - 30;
                        tempV.Y = parent.mouse.getPos().Y - 30;
                        b.setPos(tempV);
                    }
                }
                timer -= 0.8;
                if (timer <= 0)
                {
                    p.cooldownF = true;
                }
            }
        }

        //recharging timer gets called from update when cooldownF is true
        public void rechargeTimer(double num, Player p)
        {
            if (timer <= 30)//recharge until 30
            {
                timer += num;
            }
            else
            {
                p.cooldownF = false;//allow for power activation
            }
        }

        public double getTimer()
        {
            return timer;
        }
    }
}
