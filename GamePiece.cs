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
    abstract class GamePiece
    {
        //attributes
        protected Texture2D gameTexture;
        protected Rectangle objRect;
        protected bool realWorld;

        //properties
        public Texture2D GameTexture
        {
            get { return gameTexture; }
            set { gameTexture = value; }
        }

        public Rectangle ObjRect
        {
            get { return objRect; }
            set { objRect = value; }
        }
        public bool RealWorld
        {
            get { return realWorld; }
            set { realWorld = value; }
        }

        //constructor
        public GamePiece(Rectangle rectangle, Texture2D texture)
        {
            objRect = rectangle;
            gameTexture = texture;
        }
    }
}
