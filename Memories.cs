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
    class Memories:GamePiece
    {

        //attributes
        private int totMem; //the total memories in that level
        private bool advanceLevel;//if all the memories have been collected, this will be true and advance on

        //properties
        public int TotMem
        {
            get { return totMem; }
            set { totMem = value; }
        }

        public bool AdvanceLevel
        {
            get { return advanceLevel; }
        }

        //constructor
        public Memories(int numofmemories, Rectangle memorect, Texture2D memotexture)
            : base(memorect, memotexture)
        {
            totMem = numofmemories;
        }

        /// <summary>
        /// When a player collides with a memory, it will add it to the memory counter
        /// </summary>
        /// <param name="playr"></param>
        public void AddMemory(Player playr)
        {

            playr.MemsColl++;
        }

        public void memsAllCollected(Player plr)
        {
            if (plr.MemsColl == totMem)
            {
                advanceLevel = true;
            }
            else
            {
                advanceLevel = false;
            }
        }
    }
}
