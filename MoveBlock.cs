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
    class MoveBlock : Block
    {
        //attributes
        private int blockSpeed;//the speed of the block will be equal to 1/2 of the player speed. (speedwithBlock)
        protected bool isFalling; //determins if a move block is falling
        protected Vector2 blockPos; //position of the block
        protected Vector2 velocity; //block falling gravity
        protected bool isMoving;//if the block is currently moving

        //properties
        public int BlockSpeed
        {
            get { return blockSpeed; }
            set { blockSpeed = value; }
        }

        public bool IsFalling
        {
            get { return isFalling; }
            set { isFalling = value; }
        }

        public Vector2 BlockPos
        {
            get { return blockPos; }
            set { blockPos = value; }
        }

        public bool IsMoving
        {
            get { return isMoving; }
            set { isMoving = value; }
        }

        //constructor
        public MoveBlock(Rectangle blokrect, Texture2D bloktext)
            : base(blokrect, bloktext)
        {
            blockPos = new Vector2(ObjRect.X, ObjRect.Y);
            isFalling = false;
            isMoving = false;
        }

        //push pull method that determines direction/motion
        public void PushingPulling(int direction, Player paulPlayer)
        {

            if (direction == 3 && (paulPlayer.ObjRect.X - ObjRect.X >= -50) && (paulPlayer.ObjRect.X - ObjRect.X <= 50) && ((ObjRect.Y - paulPlayer.ObjRect.Y >= -50) && (ObjRect.Y - paulPlayer.ObjRect.Y <= 50)))//what direction the block will go. 3 = right. Checking left side of block
            {
                //((ObjRect.Left - paulPlayer.ObjRect.Right) >= -10 && (ObjRect.Left - paulPlayer.ObjRect.Right) < 0 )
                blockPos.X += paulPlayer.SpeedWithBlock;
                ObjRect = new Rectangle((int)blockPos.X, (int)blockPos.Y, ObjRect.Width, ObjRect.Height);
                blockPos = new Vector2(ObjRect.X, ObjRect.Y);

                isFalling = false;
                isMoving = true;
            }
            else if (direction == 1 && (ObjRect.X - paulPlayer.ObjRect.X >= -50) && (ObjRect.X - paulPlayer.ObjRect.X <= 50) && ((ObjRect.Y - paulPlayer.ObjRect.Y >= -50) && (ObjRect.Y - paulPlayer.ObjRect.Y <= 50)))//the object moving to the left and the player pushing from the right side
            {
                blockPos.X -= paulPlayer.SpeedWithBlock;
                ObjRect = new Rectangle((int)blockPos.X, (int)blockPos.Y, ObjRect.Width, ObjRect.Height);
                blockPos = new Vector2(ObjRect.X, ObjRect.Y);

                isFalling = false;
                isMoving = true;

            }
            else
            {
                isMoving = false;
            }

            ObjRect = new Rectangle((int)blockPos.X, (int)blockPos.Y, ObjRect.Width, ObjRect.Height);
        }

        public void CollidingWith(Rectangle blockRec)
        {
            if (ObjRect.Intersects(blockRec))
            {
                blockPos = new Vector2(ObjRect.X, ObjRect.Y);
                ObjRect = new Rectangle((int)blockPos.X, (int)blockPos.Y, ObjRect.Width, ObjRect.Height);
                if (blockRec.Top - ObjRect.Bottom >= -10 && blockRec.Top - ObjRect.Bottom < 0 && blockRec.Left + 10 <= ObjRect.Right && blockRec.Right - 10 >= ObjRect.X)
                {
                    ObjRect = new Rectangle((int)blockPos.X, blockRec.Top - ObjRect.Height, ObjRect.Width, ObjRect.Height);

                    isFalling = false;
                    blockPos += velocity;
                    velocity.Y = 0f;
                    blockPos = new Vector2(ObjRect.X, ObjRect.Y);
                }
                if (blockRec.Top - ObjRect.Bottom < -10 && blockRec.Top - ObjRect.Bottom < 0 && blockRec.Left + 10 > ObjRect.Right && blockRec.Right - 10 < ObjRect.X)
                {
                    isFalling = true;
                    velocity.Y += 0.23f * 1;
                    blockPos += velocity;
                    blockPos = new Vector2(ObjRect.X, ObjRect.Y);
                    ObjRect = new Rectangle((int)blockPos.X, (int)blockPos.Y, ObjRect.Width, ObjRect.Height);
                }
                //Checks left side of block
                else if ((blockRec.Left - ObjRect.Right) >= -10 && (blockRec.Left - ObjRect.Right) < 0)
                {
                    ObjRect = new Rectangle(blockRec.Left - ObjRect.Width, (int)blockPos.Y, ObjRect.Width, ObjRect.Height);
                    blockPos = new Vector2(ObjRect.X, ObjRect.Y);
                }
                //Checks right side of block
                else if ((ObjRect.Left - blockRec.Right) >= -10 && (ObjRect.Left - blockRec.Right) < 0)
                {
                    ObjRect = new Rectangle(blockRec.Right, (int)blockPos.Y, ObjRect.Width, ObjRect.Height);
                    blockPos = new Vector2(ObjRect.X, ObjRect.Y);
                }
                //Checks under side of block
                else if (ObjRect.Top - blockRec.Bottom >= -10 && ObjRect.Top - blockRec.Bottom < 0)
                {
                    ObjRect = new Rectangle((int)blockPos.X, blockRec.Bottom, ObjRect.Width, ObjRect.Height);
                    IsFalling = false;
                    blockPos += velocity;
                    velocity.Y = 0f;
                    blockPos = new Vector2(ObjRect.X, ObjRect.Y);
                }
                else
                {
                    isFalling = true;
                    velocity.Y += 0.23f * 1;
                    blockPos += velocity;

                }
                blockPos = new Vector2(ObjRect.X, ObjRect.Y);
                ObjRect = new Rectangle((int)blockPos.X, (int)blockPos.Y, ObjRect.Width, ObjRect.Height);
            }
        }

    }
}
