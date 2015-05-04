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
    abstract class Character : GamePiece
    {
        //attributes that will be used for both enemies and the player
        protected bool isDead;

        // property for isdead
        public bool IsDead
        {
            get { return isDead; }
            set { isDead = value; }
        }

        //constructor
        public Character(Boolean deadstatus, Rectangle characterrectangle, Texture2D chartext)
            : base(characterrectangle, chartext)
        {
            isDead = deadstatus;
        }
    }
}
