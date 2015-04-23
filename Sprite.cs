using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
//using Microsoft.Xna.Framework.GamerServices;
using System.IO;

namespace UGWProjCode
{
    class Sprite
    {
        private List<string> levels = new List<string>();
        private List<string> lFiles = new List<string>();
        List<Sprite> sprites = new List<Sprite>();
        protected int mapX = 42;
        protected int mapY = 42;
        protected bool hasRead = false;
        
        
        Texture2D floor;
        Texture2D sides;
        Texture2D top;
        Rectangle floorrect;
        Rectangle siderectL;
        Rectangle siderectR;
        Rectangle toprect;
        GeneralBlock ceiling;
        GeneralBlock sideL;
        GeneralBlock sideR;
        GeneralBlock ground;
        Enemy enemy1;
        Enemy enemy2;
        Enemy enemy1ghost;
        Enemy enemy2ghost;
        Memories memory;
        Player paulPlayer;
        int level;


        DeadlyBlock DBTest;
        DeadlyBlock DBGTest;
        DeadlyBlock testDeadly;
        Texture2D DBTextureTest;
        Texture2D DBGhostText;
        public Sprite(GamePiece image,Rectangle rect)
        { 
        
        }


        public void LoadLevels(SpriteBatch spriteBatch)
        {
            StreamReader lRead;



            lFiles.Add("level.txt"); //Directory.GetFiles(@".", "*level*");

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
                            sprites.Add(new Sprite(paulPlayer,new Rectangle(mapX,mapY,64,64)));
                            //spriteBatch.Draw(paulPlayer.GameTexture, new Rectangle(mapX, mapY, 50, 50), Color.White);
                        }


                        if (c == 'f')
                        {
                            //create floaty block at location
                        }

                        if (c == 'd')
                        {

                            spriteBatch.Draw(testDeadly.GameTexture, new Rectangle(mapX, mapY, 50, 50), Color.White);
                        }

                        if (c == 'D')
                        {

                            spriteBatch.Draw(testDeadly.GameTexture, new Rectangle(mapX, mapY, 50, 50), Color.White);
                        }

                        if (c == 'o')
                        {
                            spriteBatch.Draw(DBGTest.GameTexture, new Rectangle(mapX, mapY, 50, 50), Color.White);
                        }

                        if (c == 'p')
                        {
                            //spriteBatch.Draw(phase.GameTexture, new Rectangle(mapX, mapY, 50, 50), Color.White);
                        }



                        if (c == 'x')
                        {
                            //Create switch at location. If we even have switches.
                        }

                        if (c == 'e')
                        {
                            spriteBatch.Draw(enemy1.GameTexture, new Rectangle(mapX, mapY, 50, 50), Color.White);
                        }

                        if (c == 'E')
                        {
                            spriteBatch.Draw(enemy2.GameTexture, new Rectangle(mapX, mapY, 50, 50), Color.White);
                        }

                        if (c == 'g')
                        {
                            // spriteBatch.Draw(ghostEnemy1.GameTexture, new Rectangle(mapX, mapY, 50, 50), Color.White);
                        }

                        if (c == 'G')
                        {
                            //spriteBatch.Draw(ghostEnemy2.GameTexture, new Rectangle(mapX, mapY, 50, 50), Color.White);
                        }

                        if (c == 'm')
                        {
                            //spriteBatch.Draw(memory.GameTexture, new Rectangle(mapX, mapY, 50, 50), Color.White);
                        }

                        if (c == 'z')
                        {
                            //create enemy type 3. If we do.
                        }

                        if (c == 'Z')
                        {
                            //create enemy type 4. If we do.
                        }



                        if (c == 'n')
                        {
                            mapY += 5;
                            mapX = 42;
                        }
                        //Window Dimensions: 1024 x 768
                        mapX += 10;
                        //
                    }
                }

                lRead.Close();
            }
        }
    }
}
