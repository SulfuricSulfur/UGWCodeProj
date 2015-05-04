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
    class GeneralBlock : Block
    {
        // constructor
        public GeneralBlock(Rectangle generalRect, Texture2D generalBTexture)
            : base(generalRect, generalBTexture)
        {

        }
    }
}
