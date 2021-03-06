﻿using System;
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
    public class GameObject
    {
        protected Texture2D tex;          // The *entire* spritesheet
        protected Rectangle srcRect;      // Where in the *spritesheet* we're drawing
        protected Rectangle destRect;     // Where on the *screen* we're drawing
        protected Vector2 pos;            // Position of the player
        protected Vector2 origin;         // We're not using this, but have to have it for drawing
        protected Game1 parent;
        protected bool isCollide, isCollideTop, isCollideBot;         // flags for rectangle collision
        protected Vector2 point1, point2, point3, point4;   //4 points of any rectangle
        protected float gravity;

        public GameObject()
        {
            //default constructor will never be invoked with 0 arguments
            tex = null;
            pos = new Vector2(0, 0);
            parent = new Game1();
            isCollide = isCollideBot = isCollideTop = false;
            gravity = 1.5f;

            // we're not using this, since we're not doing rotation...
            origin = new Vector2(0, 0);
            // we'll start the source rectangle to be the idle row, frame 0
            srcRect = new Rectangle(0, 0, 0, 0);
            // the destination rect is where we're drawing on the screen
            destRect = new Rectangle(0, 0, 64, 64);
        }
        public GameObject(Texture2D t, Vector2 p, Game1 g)
        {
            tex = t;
            pos = p;
            parent = g;

            //4 corners of a gameobject's rectangle
            point1 = p;
            point2 = new Vector2(pos.X + tex.Width / 2, pos.Y);
            point3 = new Vector2(pos.X + tex.Width / 2, pos.Y + tex.Height / 2);
            point4 = new Vector2(pos.X, pos.Y + tex.Height / 2);
        }

        public virtual void Update()
        {
        }

        public virtual void Update(GameTime gameTime, List<Platform> l)
        {
            /*if (plat == null)
            {
                return;
            }
            else if ((this.point4.X < plat.point1.X && this.point3.X < plat.point1.X) || (this.point4.X > plat.point2.X && this.point3.X > plat.point2.X) && this.isCollide)//allows falling on edge of platforms
            {
                this.isCollide = false;
                    //break;
            }
            
            else if (this.point3.Y < (plat.point2.Y - 10))
            {
                this.isCollide = false;
            }*/
        }

        public virtual void Draw(SpriteBatch sb)
        {
            
        }

        //update the 4 corners of a sprite's rectangle
        //change this if the sprite sheet changes
        public virtual void updatePoints()
        {
            point1 = pos;
            point2.X = pos.X + tex.Width / 2;
            point2.Y = pos.Y;
            point3.X = pos.X + tex.Width / 2;
            point3.Y = pos.Y + tex.Height / 2;
            point4.X = pos.X;
            point4.Y = pos.Y + tex.Height / 2;
        }

        //collision detection with platforms
        public Platform isColliding(List<Platform> l)
        {
            foreach (Platform plat in l)
            {
                //if (this.point3.X < plat.point2.X && this.point4.X > plat.point1.X && this.point3.Y >= plat.point2.Y + 7 && this.point4.Y >= plat.point1.Y + 7
                        //&& this.point3.Y < plat.point3.Y && this.point4.Y < plat.point4.Y) //stop falling through platforms
                if (((this.point3.X > plat.point1.X && this.point3.X < plat.point2.X) || (this.point4.X > plat.point1.X && this.point4.X < plat.point2.X)) && 
                        (this.point3.Y >= plat.point2.Y && this.point3.Y < plat.point2.Y + 9))//don't fall through platform
                {
                    this.isCollideBot = true;
                    this.gravity = 0;
                    return plat;
                }
                //else if (this.point2.X < plat.point3.X && this.point1.X > plat.point4.X && this.point2.Y <= plat.point3.Y && this.point1.Y <= plat.point4.Y
                    //&& this.point2.Y > plat.point2.Y && this.point1.Y > plat.point1.Y) //cannot go from below platforms
                else if (((this.point3.X > plat.point1.X && this.point3.X < plat.point2.X) || (this.point4.X > plat.point1.X && this.point4.X < plat.point2.X)) &&
                        (this.point2.Y <= plat.point3.Y && this.point2.Y > plat.point3.Y - 9))//cannot go through the bottom to up
                {
                    this.isCollideTop = true;
                    this.pos.Y += 0;
                    return plat;
                }
                //else if (this.point4.X <= plat.point3.X && this.point4.X > plat.point4.X && this.point1.Y > plat.point2.Y && this.point1.Y < plat.point3.Y)//cannot go into platform from the right
                else if (((this.point2.Y > plat.point1.Y && this.point2.Y < plat.point4.Y) || this.point3.Y > plat.point1.Y && this.point3.Y < plat.point4.Y) && 
                        (this.point1.X <= plat.point2.X && this.point1.X > plat.point2.X - 9)) //cannot go into platform from the right to left
                {
                    this.isCollide = true;
                    this.isCollideBot = true;
                    this.pos.X += this.getWalkSpeed();
                    return plat;
                }
                //else if (this.point3.X >= plat.point4.X && this.point3.X < plat.point3.X && this.point2.Y > plat.point1.Y && this.point3.Y < plat.point3.Y)//cannot go into platform from the left
                else if (((this.point2.Y > plat.point1.Y && this.point2.Y < plat.point4.Y) || this.point3.Y > plat.point1.Y && this.point3.Y < plat.point4.Y) && 
                        (this.point2.X >= plat.point1.X && this.point2.X < plat.point1.X + 9)) //cannot go into platform from the left to right
                {
                    this.isCollide = true;
                    //this.isCollideBot = true;
                    this.pos.X -= this.getWalkSpeed();
                    return plat;
                }
                else if ((pos.Y > parent.screenHeight - (tex.Height + 10))) // fall off the screen on the bottom
                {
                    this.isCollideBot = true;
                    this.gravity = 0;
                }
                else
                {
                    this.isCollide = false;
                    this.isCollideTop = false;
                    this.isCollideBot = false;
                    this.gravity = 1.5f;
                }
            }
            return null;
        }

        public Platform isColliding2(List<Platform> l)
        {
            foreach (Platform plat in l)
            {
                //if (this.point3.X < plat.point2.X && this.point4.X > plat.point1.X && this.point3.Y >= plat.point2.Y + 7 && this.point4.Y >= plat.point1.Y + 7
                //&& this.point3.Y < plat.point3.Y && this.point4.Y < plat.point4.Y) //stop falling through platforms
                if (((this.point3.X > plat.point1.X && this.point3.X < plat.point2.X) || (this.point4.X > plat.point1.X && this.point4.X < plat.point2.X)) &&
                        (this.point3.Y >= plat.point2.Y - 3 && this.point3.Y < plat.point3.Y + 9))//don't fall through platform
                {
                    this.isCollideBot = true;
                    this.gravity = 0.0f;
                    //this.pos.Y -= 2f;
                    return plat;
                }
                //else if (this.point2.X < plat.point3.X && this.point1.X > plat.point4.X && this.point2.Y <= plat.point3.Y && this.point1.Y <= plat.point4.Y
                //&& this.point2.Y > plat.point2.Y && this.point1.Y > plat.point1.Y) //cannot go from below platforms
                else if (((this.point3.X > plat.point1.X && this.point3.X < plat.point2.X) || (this.point4.X > plat.point1.X && this.point4.X < plat.point2.X)) &&
                        (this.point2.Y <= plat.point3.Y && this.point2.Y > plat.point3.Y - 15))//cannot go through the bottom
                {
                    this.isCollideTop = true;
                    this.pos.Y += 8;
                    return plat;
                }
                
                //else if (this.point4.X <= plat.point3.X && this.point4.X > plat.point4.X && this.point1.Y > plat.point2.Y && this.point1.Y < plat.point3.Y)//cannot go into platform from the right
                else if (((this.point2.Y > plat.point1.Y && this.point2.Y < plat.point4.Y) || this.point3.Y > plat.point1.Y && this.point3.Y < plat.point4.Y) &&
                        (this.point1.X <= plat.point2.X && this.point1.X > plat.point2.X - 3)) //cannot go into platform from the right to left
                {
                    this.isCollide = true;
                    this.isCollideBot = true;
                    this.pos.X += this.getWalkSpeed();
                    return plat;
                }
                //else if (this.point3.X >= plat.point4.X && this.point3.X < plat.point3.X && this.point2.Y > plat.point1.Y && this.point3.Y < plat.point3.Y)//cannot go into platform from the left
                else if (((this.point2.Y > plat.point1.Y && this.point2.Y < plat.point4.Y) || this.point3.Y > plat.point1.Y && this.point3.Y < plat.point4.Y) &&
                        (this.point2.X >= plat.point1.X && this.point2.X < plat.point1.X + 3
                    )) //cannot go into platform from the left to right
                {
                    this.isCollide = true;
                    this.isCollideBot = true;
                    this.pos.X -= this.getWalkSpeed();
                    return plat;
                }
                else if ((pos.Y > parent.screenHeight - (tex.Height + 55))) // fall off the screen on the bottom
                {
                    this.isCollideBot = true;
                    this.gravity = 0;
                }
                else
                {
                    this.isCollide = false;
                    this.isCollideTop = false;
                    this.isCollideBot = false;
                    this.gravity = 1.5f;
                }
            }
            return null;
        }

        public void isCollidingDeath()
        {
            List<GameObject> targetEnemies = new List<GameObject>();
            foreach (Enemy e in parent.enemyList)
            {
                if (this.getDestRect().Intersects(e.getDestRect()))
                {
                    targetEnemies.Add(e);
                    parent.bumpSound.Play();
                    this.setHealth();
                }
            }

            foreach (Enemy e in targetEnemies)
            {
                parent.enemyList.Remove(e);
            }
        }

        //change gravity if power is used for all gameobjects
        public virtual void setGravity(float g)
        {
            gravity = g;
            pos.Y += gravity;
        }

        //get gravity
        public virtual float getGravity()
        {
            return gravity;
        }

        public virtual Rectangle getDestRect()
        {
            return destRect;
        }

        public virtual int getHealth()
        {
            return getHealth();
        }

        public virtual void reset()
        {

        }
        
        public virtual void setHealth()
        {

        }

        public virtual void setMode(bool m)
        {

        }

        public virtual bool getMode()
        {
            return false;
        }

        public virtual float getWalkSpeed()
        {
            return 0.0f;
        }

        public virtual void setCollideFeedback(float i)
        {

        }

        public virtual Vector2 getPos()
        {
            return pos;
        }

        //set gameobject position
        public virtual void setPos(Vector2 v)
        {
            pos = v;
        }

        public virtual void setVel(Vector2 v)
        {

        }
    }

}
