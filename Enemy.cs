using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace UGWProjCode
{
    class Enemy : Character
    {
        //attributes
        private int movingDirection; //this is what direction the enemy is moving
        //and what the opposite direction will be if they collide with something or reach the edge of something
        //0 is down(south, S) 1 is left(west, A) 2 is up(north, W) and 3 is right(east, D)

        private int enemyMoveSpd; //the moving speed of the enemy will change depending on what is up in 
        //some enemies will be faster than others

        //lets the enemy speed up and charge correctly
        private Vector2 velocity;
        private Vector2 enemyPos;

        private bool canCharge; //determines if it is the enemy that can charge

        private bool chargingState; //for the 1 charging enemy, it will determin if it is currently charging so
        //there can be different animations depending on if the enemy is walking or running

        // properties
        public int MovingDirection
        {
            get { return movingDirection; }
            set { movingDirection = value; }
        }

        public int EnemyMoveSpeed
        {
            get { return enemyMoveSpd; }
            set { enemyMoveSpd = value; }
        }

        public Vector2 EnemyPosition
        {
            get { return enemyPos; }
            set { enemyPos = value; }
        }

        public bool CanCharge
        {
            get { return canCharge; }
        }

        public bool ChargingState
        {
            get { return chargingState; }
            set { chargingState = value; }
        }

        //constructor
        public Enemy(Boolean deadstatus, Rectangle enemyrect, Texture2D enemytext, int moveDir, int enemySpd, bool chargeable)
            : base(deadstatus, enemyrect, enemytext)
        {
            movingDirection = moveDir;
            enemyMoveSpd = enemySpd;
            enemyPos = new Vector2(ObjRect.X, ObjRect.Y);
            canCharge = chargeable;
        }

        //kill method
        /// <summary>
        /// The method for switching the player between worlds depending on what world they are
        /// currently in.
        /// </summary>
        /// <param name="player1"> the player object passed in</param> 
        public void Kill(Player player1)
        {
            if (player1.IsDead == false)
            {
                player1.IsDead = true;
            }
            else if (player1.IsDead == true)
            {
                player1.IsDead = false;
            }
        }

        /// <summary>
        /// this will be called for every enemy in the enemy arrays to see if they are colliding with the player while in the same state
        /// </summary>
        /// <param name="plyr"></param>
        public void KillingPlayer(Player plyr)
        {
            //if the player is dead and the enemy is dead
            if (isDead == true && plyr.IsDead == true)
            {
                if (objRect.Intersects(plyr.ObjRect))
                {
                    Kill(plyr);
                }
            }

            //if the player is alive and the enemy is alive
            else if (isDead == false && plyr.IsDead == false)
            {
                if (objRect.Intersects(plyr.ObjRect))
                {
                    Kill(plyr);
                }
            }
        }

        /// <summary>
        /// The enemy moves. If dead, the enemy will move up and down or side to side (depending on what direction first starts out with)
        /// if allive, the enemy can only move side to side
        /// ONE enemy can charge. It pauses for a short amount of time before it speeds up.
        /// //takes in the player character.
        /// The enemies only move if the player is in the same state as them
        /// we will NOT have enemies of different states on top of eachother
        /// </summary>
        /// <param name="plyr"></param>
        public void Move(Player plyr)
        {
            if (isDead == true && plyr.IsDead == true)//dead
            {
                ObjRect = new Rectangle((int)enemyPos.X, (int)enemyPos.Y, ObjRect.Width, ObjRect.Height);
                if (movingDirection == 0)//down
                {
                    enemyPos.Y += enemyMoveSpd;
                }
                if (movingDirection == 1)//left
                {
                    enemyPos.X -= enemyMoveSpd;
                }
                if (movingDirection == 2)//up
                {
                    enemyPos.Y -= enemyMoveSpd;
                }
                if (movingDirection == 3)//right
                {
                    enemyPos.X += enemyMoveSpd;
                }

                ObjRect = new Rectangle((int)enemyPos.X, (int)enemyPos.Y, ObjRect.Width, ObjRect.Height);
            }
            else if (isDead == false && plyr.IsDead == false)//alive
            {

                ObjRect = new Rectangle((int)enemyPos.X, (int)enemyPos.Y, ObjRect.Width, ObjRect.Height);
                if (movingDirection == 1)
                {
                    //charging to the left
                    if (canCharge == true && enemyPos.X - plyr.ObjRect.X <= 250 && enemyPos.X - plyr.ObjRect.X > -2 && (enemyPos.Y - plyr.ObjRect.Y <= 80 && enemyPos.Y - plyr.ObjRect.Y >= -80))//only 1 type of enemy can charge
                    {
                        velocity.X += -1.3f;
                        enemyPos += velocity;
                        chargingState = true;

                    }
                    //moving normally
                    else
                    {
                        enemyPos.X -= enemyMoveSpd;
                        velocity = Vector2.Zero;
                        chargingState = false;
                    }
                }
                if (movingDirection == 3)
                {    //charging to the right
                    if (canCharge == true && enemyPos.X - plyr.ObjRect.X < 2 && enemyPos.X - plyr.ObjRect.X >= -250 && (enemyPos.Y - plyr.ObjRect.Y <= 80 && enemyPos.Y - plyr.ObjRect.Y >= -80)) //only 1 type of enemy can charge
                    {

                        velocity.X += 1.3f;
                        enemyPos += velocity;
                        chargingState = true;

                    }
                    //moving normally
                    else
                    {
                        enemyPos.X += enemyMoveSpd;
                        velocity = Vector2.Zero;
                        chargingState = false;
                    }
                }
                ObjRect = new Rectangle((int)enemyPos.X, (int)enemyPos.Y, ObjRect.Width, ObjRect.Height);
            }
        }
        
        //test collision method
        /// <summary>
        /// takes in the rectangle of an object and if it is alive or dead
        /// </summary>
        /// <param name="otherRect">the rectangle of any object</param>
        /// <param name="isDeadObj">if the object(same one the rectangle is) is alive or dead</param>
        public void EnemyCollide(Rectangle otherRect, bool isDeadObj)
        {
            if (this.ObjRect.Intersects(otherRect))
            {
                if (this.IsDead == true && isDeadObj == true)
                {
                    //collision on for the enemy
                    //in the spooky zone
                    if ((otherRect.Top - this.ObjRect.Bottom) >= -10 && (otherRect.Top - this.ObjRect.Bottom) < 0 && movingDirection == 0)//enemy collides with top of block
                    {
                        movingDirection = 2;
                    }
                    else if ((otherRect.Left - this.ObjRect.Right) >= -10 && (otherRect.Left - this.ObjRect.Right) < 0 && movingDirection == 3)//left side of the block collision
                    {
                        movingDirection = 1;
                    }
                    else if ((this.ObjRect.Left - otherRect.Right) >= -10 && (this.ObjRect.Left - otherRect.Right) < 0 && movingDirection == 1)//right side of block
                    {
                        movingDirection = 3;
                    }
                    else if ((this.ObjRect.Top - otherRect.Bottom) >= -10 && (this.ObjRect.Top - otherRect.Bottom) < 0 && movingDirection == 2)//cloding with bottom of block
                    {
                        movingDirection = 0;
                    }

                }
                else if (this.IsDead == false && isDeadObj == false)
                {
                    //physical world only requires left and right moving
                    if ((otherRect.Left - this.ObjRect.Right) >= -10 && (otherRect.Left - this.ObjRect.Right) < 0 && movingDirection == 3)//left side of the block collision
                    {
                        movingDirection = 1;
                        if(chargingState == true)
                        {
                            movingDirection = 1;
                        }
                    }
                    if ((this.ObjRect.Left - otherRect.Right) >= -10 && (this.ObjRect.Left - otherRect.Right) < 0 && movingDirection == 1)//right side of block
                    {
                        movingDirection = 3;
                        if (chargingState == true)
                        {
                            movingDirection = 3;
                        }
                    }
                }
            }
        }
    }
}
