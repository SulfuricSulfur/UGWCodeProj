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
        //test
        //attributes
        private int movingDirection;//this is what direction the enemy is moving
        //and what the opposite direction will be if they collide with something or reach the edge of something
        //0 is down(south, S) 1 is left(west, A) 2 is up(north, W) and 3 is right(east, D)
        private int enemyMoveSpd;//the moving speed of the enemy will change depending on what is up in 
        //some enemies will be faster than others


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

        //constructor
        public Enemy(Boolean deadstatus, Rectangle enemyrect, Texture2D enemytext, int moveDir, int enemySpd)
            : base(deadstatus, enemyrect, enemytext)
        {
            movingDirection = moveDir;
            enemyMoveSpd = enemySpd;
        }

        //kill method
        /// <summary>
        /// The method for switching the player between worlds depending on what world they are
        /// currently in.
        /// </summary>
        /// <param name="player1"> the player object passed in</param> 
        public void Kill(Player player1)
        {
            //stub method. 
            if (player1.RealWorld == false)
            {
                player1.RealWorld = true;
            }
            else if (player1.RealWorld == true)
            {
                player1.RealWorld = false;
            }
        }

        public void Move()
        {
            if (movingDirection == 0)//down
            {
                ObjRect = new Rectangle(ObjRect.X, ObjRect.Y + enemyMoveSpd, ObjRect.Width, ObjRect.Height);
            }
            else if (movingDirection == 1)//left
            {
                ObjRect = new Rectangle(ObjRect.X - enemyMoveSpd, ObjRect.Y, ObjRect.Width, ObjRect.Height);
            }
            else if (movingDirection == 2)//right
            {
                ObjRect = new Rectangle(ObjRect.X + enemyMoveSpd, ObjRect.Y, ObjRect.Width, ObjRect.Height);
            }
            else if (movingDirection == 3)//up
            {
                ObjRect = new Rectangle(ObjRect.X, ObjRect.Y - enemyMoveSpd, ObjRect.Width, ObjRect.Height);
            }
        }
    }
}
