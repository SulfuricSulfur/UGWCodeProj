﻿using System;
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
    class Player : Character
    {
        //attributes
        private int memsColl;//Memories collected by the player
        private int moveSpd;//player speed default;
        private int spdWithBlock;//the speed of the player while moving the block
        private bool hasJumped;

        //the x and y to parse float to int and use for x and y in rectangle
        private int xPosV;
        private int yPosV;

        // random objecy
        Random plyRand = new Random();
        
        //properties
        public int MemsColl
        {
            get { return memsColl; }
            set { memsColl = value; }
        }

        public int MoveSpeed
        {
            get { return moveSpd; }
            set { moveSpd = value; }
        }

        public int SpeedWithBlock
        {
            get { return spdWithBlock; }
            set { spdWithBlock = value; }
        }

        //constructor
        public Player(Rectangle playrect, Texture2D playtext, Vector2 playerPos, bool Jumped)
            : base(false, playrect, playtext)
        {
            hasJumped = Jumped; //default, no jump
            playerPos = new Vector2(this.ObjRect.X, this.ObjRect.Y);//setting the position equal to the vector
            moveSpd = 4;//sets the player speed as 4
            spdWithBlock = 2;
        }
    }
}
