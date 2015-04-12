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

        //properties
        public int BlockSpeed
        {
            get { return blockSpeed; }
            set { blockSpeed = value; }
        }

        //constructor
        public MoveBlock(Rectangle blokrect, Texture2D bloktext)
            : base(blokrect, bloktext)
        {

        }

        //push pull method that determines direction/motion
        public void PushPull(Player playguy)
        {
            //stub. Need to get direction player is going in and based on that (if button down) block will move in certain direction. 
            //player will not be able to move if dead. 
        }
    }
}
