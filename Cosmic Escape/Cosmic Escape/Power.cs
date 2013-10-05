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
        public Power()
        {
            timer = 30.0;
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
                }
                timer -= 0.8;
                if (timer <= 0)
                {
                    p.cooldownF = true;
                }
            }
            //p.cooldownF = true;
            p.setGravity(1.5f); //reset to default gravity
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
