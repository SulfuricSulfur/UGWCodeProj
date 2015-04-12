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
using System.IO;

namespace UGWProjCode
{
    class Map
    {
        private Dictionary<int, string> levels;
        private string[] lFiles;
        protected int mapX;
        protected int mapY;

        public Map()
        {
            levels = new Dictionary<int, string>();
        }

        public void LoadLevels()
        {
            StreamReader lRead;

            lFiles = Directory.GetFiles(@".", "*level*");

            // if (lFiles.Length == 0)
            // {
            //      Write "no levels found" somewhere
            // }

            foreach (string l in lFiles)
            {
                lRead = new StreamReader(l);

                string lvl = " "; //empty string
                string lvlIn = " "; //String being read
                string lCheck = " "; //Tells when reader should stop reading
                mapX = 42;

                while ((lvlIn = lRead.ReadLine()) != null)
                {

                    //Check for characters
                    foreach (char c in lvlIn)
                    {
                        if (c == '@')
                        {
                            // Player paul = new Player(new Microsoft.Xna.Framework.Rectangle(mapX,mapY,64,64),)
                        }

                        if (c == 'g')
                        {
                            //create grass block at location
                        }

                        if (c == 'f')
                        {
                            //create floaty block at location
                        }

                        if (c == 'd')
                        {
                            //create deadly block here
                        }

                        if (c == '|')
                        {
                            //Create wall at location
                        }

                        if (c == 'x')
                        {
                            //Create switch at location
                        }

                        if (c == 'e')
                        {
                            //create enemy type 1
                        }

                        if (c == 'E')
                        {
                            //create enemy type 2
                        }

                        if (c == 'z')
                        {
                            //create enemy type 3
                        }

                        if (c == 'Z')
                        {
                            //create enemy type 4
                        }

                        if (c == 'n')
                        {
                            //mapY += particular amount;
                            //mapX = 42;
                        }
                        //Window Dimensions: 1024 x 768
                        //mapX += 54;
                        //
                    }
                }
            }
        }
    }
}
