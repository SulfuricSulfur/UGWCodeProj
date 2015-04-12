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
    class PhaseBlock : Block
    {
        //attributes

        //constructor
        public PhaseBlock(Rectangle phaserect, Texture2D phasetext)
            : base(phaserect, phasetext)
        {
            //this will be a non-interractable block. Will act like the Boarder block "Block"
            //unless the player is dead. Then the collisions will be turned off but the block will
            //still be drawn in the level(transparent) and the player can go through them
        }
    }
}
