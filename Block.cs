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
    abstract class Block : GamePiece
    {
        //attributes


        //constructor
        public Block(Rectangle rect, Texture2D textur)
            : base(rect, textur)
        {
            //the foundation block. Will act as the boarder for the level and also as part of the puzzel.

        }

    }
}
