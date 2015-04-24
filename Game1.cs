#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.IO;
#endregion

namespace UGWProjCode
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {

        StreamReader reader;
        string[] textures;
        Texture2D floor;
        Texture2D sides;
        Texture2D top;
        Rectangle floorrect;
        Rectangle siderectL;
        Rectangle siderectR;
        Rectangle toprect;

        int level;
        string lvl;

  

        private Dictionary<int, string> levels = new Dictionary<int, string>();
        private List<string> lFiles = new List<string>();
        private List<GamePiece> gamePieces = new List<GamePiece>();
        private List<Enemy> enemyPhys = new List<Enemy>();
        private List<Enemy> enemyGhosts = new List<Enemy>();
        private List<DeadlyBlock> dbPhysical = new List<DeadlyBlock>();
        private List<DeadlyBlock> dbGhost = new List<DeadlyBlock>();
        private List<PhaseBlock> phaseBlocks = new List<PhaseBlock>();
        private List<GeneralBlock> genBlocks = new List<GeneralBlock>();
        List<Memories> memories = new List<Memories>();
        List<string> levelDisplay = new List<string>();



        int pauloffset = 0;
        int paulyset = 0;
        int spriteboxwidth = 54;
        int spriteboxheight = 72;
        GameTime gametime;
        int frame;
        const double timePerFrame = 200;
        int numFrames = 3;
        int framesElapsed;
        int numberLevel = 0;
        int levelCurrent = 1;//The current level the player is on


        //private List<string> levels = new List<string>();
        //private string[] lFiles;
        protected int mapX = 42;
        protected int mapY = 42;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //attributes we need to add:
        //the textures for different enemies/objects/backgrounds will change
        //the paul and the memories will always stay no matter the change in level
        Texture2D paulPhysical;
        Texture2D spritesheet;
        Texture2D paulGhost;
        Texture2D memorytexture;
        //the background will change
        Texture2D enemyPhysical1;
        Texture2D enemyPhysical2;
        Texture2D enemyGhost1;
        Texture2D enemyGhost2;
        Texture2D phaseBlockTexture;
        Texture2D deadlyObjs;
        Texture2D deadlyGhostObj;
        Texture2D backGround;
        Texture2D moveBlockTexture;
        Texture2D basicFloat;
        Texture2D basicGround;

        //this clss will be changed as we get more things done
        //these are the objects
        Player paulPlayer;
        //most of the classes will have methods and attributes that you can call
        //like the player.HasJumped will be called and set to false when the player 
        //is interacting with the ground
        Rectangle paulRect;

        //setting up input
        KeyboardState kboardstate;//getting the keyboard state;
        KeyboardState prevKeyPressed; //takes the previous key that was pressed
        private bool hasJumped; //will set it so that the player can not constantly jump
        private Vector2 velocity;//the velcotiy of the player jumping/falling
        protected Vector2 playerPos; //the position in relation to the rectangle so it can jump;
        //enumerator
        enum PhysicalState { PaulFaceRight, PaulFaceLeft, PaulWalkRight, PaulWalkLeft, PaulJumpRight, PaulJumpLeft, PaulPushLeft, PaulPushRight };
        PhysicalState paulPCurrent = PhysicalState.PaulFaceRight;//default

        // GameState enumerator
        enum GameState
        {
            MainMenu,
            Playing,
            Help,
            Credits
        }

        // gamestate begins in main menu
        GameState CurrentGameState = GameState.MainMenu;
        // button objects
        Button btnPlay;
        Button btnHelp;
        Button btnCredit;
        Button btnQuit;
        Button btnBack;
        Button btnBackPause;
        Button btnRestart;

        // pause menu attributes
        bool paused = false;
        Texture2D pauseMenu;
        Rectangle pausedRect;

        //sprite in the ghost state will only be one state, so there does not need to be an enum for it.

        //testing attributes

        Random rnd;

        int totalMemories; //the total amount of memories
        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            Content.RootDirectory = "Content";
            level = 1;
            reader = new StreamReader("Textures.txt");



        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here


            //each line represents each level
            //[0] floor, [1] side borders, [2] top border,  [3]  enemy1ghost, [4]  enemy 2ghost,[5] enemy1phys, [6] enemy2phys
            //[7] float1, [8] float2, [9] moving block, [10] transblock ghost [11] transblock physical
            if (level > 1)
            {
                string[] lines = reader.ReadToEnd().Split(new char[] { '\n' });
                textures = lines[level - 1].Split(',');

                for (int i = 0; i < textures.Length; i++)
                {
                    textures[i] = textures[i].Replace("\r", "");
                }
            }
            else
            {
                textures = reader.ReadLine().Split(',');
            }


            reader.Close();
            //playerPos = new Vector2(300, 300);
            toprect = new Rectangle(40, 0, 1000, 40);
            siderectL = new Rectangle(0, 0, 50, 942);
            siderectR = new Rectangle(1000, 0, 50, 942);
            floorrect = new Rectangle(39, 740, 1000, 40);

            // initialized objects
            btnPlay = new Button(Content.Load<Texture2D>("playbutton"), graphics.GraphicsDevice);
            btnHelp = new Button(Content.Load<Texture2D>("helpbutton"), graphics.GraphicsDevice);
            btnCredit = new Button(Content.Load<Texture2D>("creditsbutton"), graphics.GraphicsDevice);
            btnQuit = new Button(Content.Load<Texture2D>("quitbutton"), graphics.GraphicsDevice);
            btnBack = new Button(Content.Load<Texture2D>("backbutton"), graphics.GraphicsDevice);
            btnBackPause = new Button(Content.Load<Texture2D>("backbutton"), graphics.GraphicsDevice);
            btnRestart = new Button(Content.Load<Texture2D>("restartbutton"), graphics.GraphicsDevice);

            // button positions
            btnPlay.setPosition(new Vector2(448, 300));
            btnHelp.setPosition(new Vector2(448, 400));
            btnCredit.setPosition(new Vector2(448, 500));
            btnQuit.setPosition(new Vector2(448, 600));
            btnBack.setPosition(new Vector2(148, 600));
            btnBackPause.setPosition(new Vector2(148, 500));
            btnRestart.setPosition(new Vector2(448, 400));

            // pause stuff
            pauseMenu = Content.Load<Texture2D>("pausebg");
            pausedRect = new Rectangle(0, 0, pauseMenu.Width, pauseMenu.Height);


            base.Initialize();
        }



        public void LoadLevels()
        {
            StreamReader lRead;

            lFiles.Add("level.txt");
            lFiles.Add("level666.txt");// = Directory.GetFiles(@".", "*level*");




            // if (lFiles.Length == 0)
            // {
            //      Write "no levels found" somewhere
            // }

            foreach (string l in lFiles)
            {
                numberLevel++; //keep track of what level is where

                lRead = new StreamReader(l);

                lvl = " "; //empty string
                string lvlIn = " "; //String being read
                //string lCheck = " "; Tells when reader should stop reading. So far have not seen use for it, but keep it just in case.
                mapX = 0;



                while ((lvlIn = lRead.ReadLine()) != null)
                {

                    //lvlIn = lRead.ReadLine();
                    lvl += lvlIn;
                }

                levelDisplay.Add(lvl);
                lRead.Close();

            }
        }

        public void DrawLevel(int lNum)
        {
            genBlocks.Add(new GeneralBlock(toprect, top));
            genBlocks.Add(new GeneralBlock(siderectR, sides));
            genBlocks.Add(new GeneralBlock(siderectL, sides));
            genBlocks.Add(new GeneralBlock(floorrect, floor));
            Random rnd = new Random();
            //Check for characters
            foreach (char c in levelDisplay[lNum - 1].ToCharArray())//this is a temporay thing. Textures will change
            {
                if (c == '@')
                {
                    //make this 1 player object

                    paulRect = new Rectangle(mapX, mapY, 48, 48);
                    playerPos = new Vector2(paulRect.X, paulRect.Y);
                    paulPlayer = new Player(paulRect, paulPlayer.GameTexture, playerPos, false);

                }


                else if (c == 'f')
                {
                    genBlocks.Add(new GeneralBlock(new Rectangle(mapX, mapY, 50, 50), basicFloat));
                }

                else if (c == 'm')
                {
                    memories.Add(new Memories(new Rectangle(mapX, mapY, 50, 50), memorytexture));
                    totalMemories++;

                }
                else if (c == 'x')
                {
                    genBlocks.Add(new GeneralBlock(new Rectangle(mapX, mapY, 50, 50), basicGround));
                }
                else if (c == '~')
                {
                    dbGhost.Add(new DeadlyBlock(new Rectangle(mapX, mapY, 50, 50), deadlyGhostObj));
                }

                else if (c == '!')
                {
                    //spriteBatch.Draw(ghostEnemy2.GameTexture, new Rectangle(mapX, mapY, 50, 50), Color.White);
                    dbGhost.Add(new DeadlyBlock(new Rectangle(mapX, mapY, 50, 50), deadlyGhostObj)); //will have a different texture
                }



                else if (c == 'd')
                {

                    dbPhysical.Add(new DeadlyBlock(new Rectangle(mapX, mapY, 50, 50), deadlyObjs));
                }

                else if (c == 'D')
                {

                    dbPhysical.Add(new DeadlyBlock(new Rectangle(mapX, mapY, 50, 50), paulPlayer.GameTexture));//will have a different texture
                }


                //enemies
                else if (c == '^')
                {
                    
                    int speed = rnd.Next(1, 7);
                    enemyGhosts.Add(new Enemy(true, new Rectangle(mapX, mapY, 50, 50), paulPlayer.GameTexture, 2, speed, false));//up and down ghost - starting up
                }

                else if (c == 'v')
                {
                   
                    int speed = rnd.Next(1, 7);
                    enemyGhosts.Add(new Enemy(true, new Rectangle(mapX, mapY, 50, 50), paulPlayer.GameTexture, 0, speed, false));//up and down ghost, starting down
                }
                else if (c == '<')
                {
                    
                    int speed = rnd.Next(1, 7);
                    enemyGhosts.Add(new Enemy(true, new Rectangle(mapX, mapY, 50, 50), paulPlayer.GameTexture, 1, speed, false));//physical deadly enemy (non chargable) starting off to the left
                }

                if (c == '>')
                {
                    
                    int speed = rnd.Next(1, 7);
                    enemyGhosts.Add(new Enemy(true, new Rectangle(mapX, mapY, 50, 50), paulPlayer.GameTexture, 3, speed, false));//physical deadly enemy (non chargable) starting off to the right
                }

                else if (c == '{')
                {
                   
                    int speed = rnd.Next(1, 7);
                    enemyPhys.Add(new Enemy(false, new Rectangle(mapX, mapY, 50, 50), paulPlayer.GameTexture, 1, speed, false));//physical deadly enemy (non chargable) starting off to the left
                }

                else if (c == '}')
                {
                    
                    int speed = rnd.Next(1, 7);
                    enemyPhys.Add(new Enemy(false, new Rectangle(mapX, mapY, 50, 50), paulPlayer.GameTexture, 3, speed, false));//physical deadly enemy (non chargable) starting off to the right
                }
                else if (c == '[')
                {
                    
                    int speed = rnd.Next(1, 7);
                    enemyPhys.Add(new Enemy(false, new Rectangle(mapX, mapY, 50, 50), paulPlayer.GameTexture, 1, speed, true));//physical deadly enemy ( chargable) starting off to the left
                }

                else if (c == ']')
                {
                    
                    int speed = rnd.Next(1, 7);
                    enemyPhys.Add(new Enemy(false, new Rectangle(mapX, mapY, 50, 50), paulPlayer.GameTexture, 3, speed, true));//physical deadly enemy ( chargable) starting off to the right
                }


                else if (c == 'p')
                {
                    phaseBlocks.Add(new PhaseBlock(new Rectangle(mapX, mapY, 50, 50), phaseBlockTexture));
                }

                else if (c == 'n')
                {
                    mapY += 50;
                    mapX = 0;
                }
                //Window Dimensions: 1024 x 768
                mapX += 50;
                //
            }
            //levels.Add(count, lvl);
            //count++;
        }           
           
    
        



        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            spritesheet = Content.Load<Texture2D>("Paul_Spritesheet.png");
            paulPhysical = Content.Load<Texture2D>("paulstand1.png");
            floor = Content.Load<Texture2D>(textures[0]);
            sides = Content.Load<Texture2D>(textures[1]);
            top = Content.Load<Texture2D>(textures[2]);
            phaseBlockTexture = Content.Load<Texture2D>(textures[10]);
            moveBlockTexture = Content.Load<Texture2D>(textures[9]);

            paulRect = new Rectangle(300, 300, paulPhysical.Width, paulPhysical.Height);
            paulPlayer = new Player(paulRect, paulPhysical, playerPos, false);
            deadlyObjs = Content.Load<Texture2D>("DeadlyBlockPhys.png");
            deadlyGhostObj = Content.Load<Texture2D>("DeadlyBlockGhost.png");
            paulGhost = Content.Load<Texture2D>("paulfloat.png");
            basicFloat = Content.Load<Texture2D>("floatgrass.png");
            basicGround = Content.Load<Texture2D>("connectivebottom.png");
            memorytexture = Content.Load<Texture2D>("movableblockgrass.png");//temp texture

            genBlocks.Add(new GeneralBlock(toprect,top));
            genBlocks.Add(new GeneralBlock(siderectR, sides));
            genBlocks.Add(new GeneralBlock(siderectL, sides));
            genBlocks.Add(new GeneralBlock(floorrect, floor));



            LoadLevels();
        }

        /// <summary>
        /// Player.Move method has been added here to make it easier with the enum/state machine
        /// </summary>
        protected void ProcessInput()
        {
            //Add the player.Move here!
            //going to need a collision detection so that if the player is colliding with the ground then
            // (player object).HasJumped = false so that it detects that player has landed on the ground and
            //can jump again
            //the key controls change depending on if paul is dead or alive
            //control handling

            //THIS STATE MACHINE IS WEIRD! NEED TO FIX IT!

            kboardstate = Keyboard.GetState();
            if (paulPlayer.IsDead == false)
            {

                playerPos += velocity;
                paulPlayer.ObjRect = new Rectangle((int)playerPos.X, (int)playerPos.Y, paulPlayer.ObjRect.Width, paulPlayer.ObjRect.Height);
                //this.ObjRect = new Rectangle( ) 
                if (kboardstate.IsKeyDown(Keys.Escape))
                {
                    //pause menu
                }
                if (prevKeyPressed.IsKeyDown(Keys.A) && hasJumped == false)
                {
                    paulPCurrent = PhysicalState.PaulFaceLeft;
                }
                if (prevKeyPressed.IsKeyDown(Keys.D) && hasJumped == false)
                {
                    paulPCurrent = PhysicalState.PaulFaceRight;
                }
                if (kboardstate.IsKeyDown(Keys.A) && prevKeyPressed.IsKeyDown(Keys.A))//IF going left and last state was facing left
                {

                    playerPos.X -= paulPlayer.MoveSpeed;

                }
                if (kboardstate.IsKeyDown(Keys.D) && prevKeyPressed.IsKeyDown(Keys.D))
                {
                    playerPos.X += paulPlayer.MoveSpeed;

                }
                if (kboardstate.IsKeyDown(Keys.D) && prevKeyPressed.IsKeyDown(Keys.D) && hasJumped == false)
                {
                    paulPCurrent = PhysicalState.PaulWalkRight;
                }
                if (kboardstate.IsKeyDown(Keys.A) && prevKeyPressed.IsKeyDown(Keys.A) && hasJumped == false)
                {
                    paulPCurrent = PhysicalState.PaulWalkLeft;
                }


                if (kboardstate.IsKeyDown(Keys.F) && prevKeyPressed.IsKeyDown(Keys.D) && hasJumped == false)
                {
                    //pushing/pulling the block from the right side.
                    playerPos.X += paulPlayer.SpeedWithBlock;
                    paulPCurrent = PhysicalState.PaulPushRight;
                }

                if (kboardstate.IsKeyDown(Keys.F) && prevKeyPressed.IsKeyDown(Keys.A) && hasJumped == false)
                {
                    //pushing/pulling from the left side of the block
                    playerPos.X -= paulPlayer.SpeedWithBlock;
                    paulPCurrent = PhysicalState.PaulPushLeft;
                }
                //Gravity and jumping v
                if (kboardstate.IsKeyDown(Keys.Space) && hasJumped == false)
                {
                    //jumping  
                    playerPos.Y += 4f;
                    velocity.Y += -5.1f;
                    hasJumped = true;
                    playerPos += velocity;
                    if (paulPCurrent == PhysicalState.PaulFaceLeft)
                    {
                        paulPCurrent = PhysicalState.PaulJumpLeft;
                    }
                    if (paulPCurrent == PhysicalState.PaulFaceRight)
                    {
                        paulPCurrent = PhysicalState.PaulJumpRight;
                    }
                    if (kboardstate.IsKeyDown(Keys.A))
                    {
                        paulPCurrent = PhysicalState.PaulJumpLeft;
                    }
                    if (kboardstate.IsKeyDown(Keys.D))
                    {
                        paulPCurrent = PhysicalState.PaulJumpRight;
                    }

                }
                if (hasJumped == true)
                {
                    //gravity so the player will fall
                    float i = 1;
                    velocity.Y += 0.23f * i;
                    playerPos += velocity;

                }
                if (hasJumped == false)
                {
                    velocity.Y += 0.197f;
                    //need to make hasjumped = false in the collision method
                    paulPlayer.ObjRect = new Rectangle((int)playerPos.X, (int)playerPos.Y, paulPlayer.ObjRect.Width, paulPlayer.ObjRect.Height);
                    playerPos = new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y);
                    if (paulPCurrent == PhysicalState.PaulJumpRight)
                    {
                        paulPCurrent = PhysicalState.PaulFaceRight;
                    }
                    if (paulPCurrent == PhysicalState.PaulJumpLeft)
                    {
                        paulPCurrent = PhysicalState.PaulFaceLeft;
                    }

                }
                paulPlayer.ObjRect = new Rectangle((int)playerPos.X, (int)playerPos.Y, paulPlayer.ObjRect.Width, paulPlayer.ObjRect.Height);

                prevKeyPressed = kboardstate;
            }
            else if (paulPlayer.IsDead == true)
            {
                paulPlayer.ObjRect = new Rectangle((int)playerPos.X, (int)playerPos.Y, paulPlayer.ObjRect.Width, paulPlayer.ObjRect.Height);
                if (kboardstate.IsKeyDown(Keys.Escape))
                {
                    //pause menu
                }
                if (kboardstate.IsKeyDown(Keys.A))
                {

                    playerPos.X -= paulPlayer.MoveSpeed;
                }
                if (kboardstate.IsKeyDown(Keys.D))
                {
                    playerPos.X += paulPlayer.MoveSpeed;
                }
                if (kboardstate.IsKeyDown(Keys.W))
                {
                    playerPos.Y -= paulPlayer.MoveSpeed;
                }
                if (kboardstate.IsKeyDown(Keys.S))
                {
                    playerPos.Y += paulPlayer.MoveSpeed;
                }
                paulPlayer.ObjRect = new Rectangle((int)playerPos.X, (int)playerPos.Y, paulPlayer.ObjRect.Width, paulPlayer.ObjRect.Height);

                prevKeyPressed = kboardstate;
            }
        }

        /// <summary>
        /// detects collison of the blocks
        /// </summary>
        protected void DetectCollison()
        {
            if (playerPos.Y > 720)
            {
                playerPos.Y = floorrect.Top - paulPlayer.ObjRect.Height;
                velocity.Y = 0f;
            }
            for (int k = 0; k < memories.Count; k++)
            {
                if (paulPlayer.ObjRect.Intersects(memories[k].ObjRect) && memories[k].HasCollected == false)
                {
                    paulPlayer.MemsColl++;
                    memories[k].HasCollected = true;
                }
            }
            for (int i = 0; i < genBlocks.Count; i++)
            {
                BlockCollison(genBlocks[i].ObjRect);
            }

            for (int i = 0; i < dbPhysical.Count; i++)
            {
                if (!paulPlayer.IsDead)
                {
                    if (BlockCollison(dbPhysical[i].ObjRect))
                        dbPhysical[i].Kill(paulPlayer);
                }
            }
            for (int i = 0; i < phaseBlocks.Count; i++)
            {
                if (!paulPlayer.IsDead)
                {
                    BlockCollison(phaseBlocks[i].ObjRect);
                }
            }
            for (int i = 0; i < dbGhost.Count; i++)
            {
                if (paulPlayer.IsDead)
                {
                    if (BlockCollison(dbGhost[i].ObjRect))//this will also need to loop through the (eventuall) array of blocks
                        dbGhost[i].Kill(paulPlayer);
                }
            }
            for (int i = 0; i < enemyGhosts.Count; i++)
            {
                for (int j = 0; j < genBlocks.Count; j++)
                {
                    enemyGhosts[i].EnemyCollide(genBlocks[j].ObjRect, true);
                }
                enemyGhosts[i].KillingPlayer(paulPlayer);
                for (int q = 0; q < dbGhost.Count; q++)
                {
                    enemyGhosts[i].EnemyCollide(dbGhost[q].ObjRect, true);
                }
                for (int r = 0; r < genBlocks.Count; r++)
                {
                    enemyGhosts[i].EnemyCollide(genBlocks[r].ObjRect, true);
                }
            }

            for (int i = 0; i < enemyPhys.Count; i++)
            {
                for (int j = 0; j < genBlocks.Count; j++)
                {
                    enemyPhys[i].EnemyCollide(genBlocks[j].ObjRect, false);
                }
                enemyPhys[i].KillingPlayer(paulPlayer);
                //colliding with deadly blocks of the same state
                for (int k = 0; k < phaseBlocks.Count; k++)
                {
                    enemyPhys[i].EnemyCollide(phaseBlocks[k].ObjRect, false);
                }
                for (int q = 0; q < dbPhysical.Count; q++)
                {
                    //enemyPhys[i].EnemyCollide(phaseBlocks[q].ObjRect, false);
                }
                for (int r = 0; r < genBlocks.Count; r++)
                {
                    enemyPhys[i].EnemyCollide(genBlocks[r].ObjRect, false);
                }
            }
        }



        protected bool BlockCollison(Rectangle blockRec)
        {
            //First checks for intersection with player
            if (paulPlayer.ObjRect.Intersects(blockRec))
            {
                //If on top, will reset has jumped
                if (blockRec.Top - paulPlayer.ObjRect.Bottom >= -10 && blockRec.Top - paulPlayer.ObjRect.Bottom < 0 && blockRec.Left + 10 <= paulPlayer.ObjRect.Right && blockRec.Right - 10 >= paulPlayer.ObjRect.X)
                {
                    paulPlayer.ObjRect = new Rectangle((int)playerPos.X, blockRec.Top - paulPlayer.ObjRect.Height, paulPlayer.ObjRect.Width, paulPlayer.ObjRect.Height);
                    playerPos += velocity;
                    playerPos = new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y);
                    hasJumped = false;
                    velocity.Y = 0f;
                }
                //Checks left side of block
                else if ((blockRec.Left - paulPlayer.ObjRect.Right) >= -10 && (blockRec.Left - paulPlayer.ObjRect.Right) < 0)
                {
                    paulPlayer.ObjRect = new Rectangle(blockRec.Left - paulPlayer.ObjRect.Width, (int)playerPos.Y, paulPlayer.ObjRect.Width, paulPlayer.ObjRect.Height);
                    playerPos += velocity;
                    playerPos = new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y);
                }
                //Checks right side of block
                else if ((paulPlayer.ObjRect.Left - blockRec.Right) >= -10 && (paulPlayer.ObjRect.Left - blockRec.Right) < 0)
                {
                    paulPlayer.ObjRect = new Rectangle(blockRec.Right, (int)playerPos.Y, paulPlayer.ObjRect.Width, paulPlayer.ObjRect.Height);
                    playerPos += velocity;
                    playerPos = new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y);
                }
                //Checks under side of block
                else if (paulPlayer.ObjRect.Top - blockRec.Bottom >= -10 && paulPlayer.ObjRect.Top - blockRec.Bottom < 0)
                {
                    paulPlayer.ObjRect = new Rectangle((int)playerPos.X, blockRec.Bottom, paulPlayer.ObjRect.Width, paulPlayer.ObjRect.Height);
                    playerPos += velocity;
                    playerPos = new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y);
                    velocity.Y = 0f;
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //Exit();

            // TODO: Add your update logic here
            MouseState mouse = Mouse.GetState();

            // TODO: Add your update logic here
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    if (btnPlay.isClicked == true)
                    {
                        playerPos = new Vector2(paulRect.X, paulRect.Y);
                        paulPlayer = new Player(paulRect, paulPlayer.GameTexture, playerPos, false);
                        CurrentGameState = GameState.Playing; 
                    }
                    if (btnHelp.isClicked == true) CurrentGameState = GameState.Help;
                    if (btnCredit.isClicked == true) CurrentGameState = GameState.Credits;
                    if (btnQuit.isClicked == true) Environment.Exit(1);
                    btnPlay.Update(mouse);
                    btnHelp.Update(mouse);
                    btnCredit.Update(mouse);
                    btnQuit.Update(mouse);
                    IsMouseVisible = true;
                    break;

                case GameState.Playing:
                    if (paused == false)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                        {
                            paused = true;
                            btnPlay.isClicked = false;
                            btnBackPause.isClicked = false;
                            btnRestart.isClicked = false;
                            IsMouseVisible = true;
                        }
                        if (!paulPlayer.IsDead)
                        {
                            for (int i = 0; i < enemyPhys.Count; i++)
                            {
                                enemyPhys[i].Move(paulPlayer);
                            }
                        }
                        if (paulPlayer.IsDead)
                        {
                            for (int i = 0; i < enemyGhosts.Count; i++)
                            {
                                enemyGhosts[i].Move(paulPlayer);
                            }
                        }
                    }
                    else
                    {
                        if (btnPlay.isClicked == true)
                        {
                            paused = false;
                            IsMouseVisible = false;
                        }
                        if (btnBackPause.isClicked == true)
                        {
                            paused = false;
                            CurrentGameState = GameState.MainMenu;
                        }
                        if (btnRestart.isClicked == true)
                        {
                            paused = false;
                            playerPos = new Vector2(paulRect.X, paulRect.Y);
                            paulPlayer = new Player(paulRect, paulPlayer.GameTexture, playerPos, false);
                            CurrentGameState = GameState.Playing;
                        }

                        btnPlay.Update(mouse);
                        btnBackPause.Update(mouse);
                        btnRestart.Update(mouse);
                    }
                    break;

                case GameState.Help:
                    if (btnBack.isClicked == true) CurrentGameState = GameState.MainMenu;
                    btnBack.Update(mouse);
                    IsMouseVisible = true;
                    break;

                case GameState.Credits:
                    if (btnBack.isClicked == true) CurrentGameState = GameState.MainMenu;
                    btnBack.Update(mouse);
                    IsMouseVisible = true;
                    break;
            }
            ProcessInput();
            DetectCollison();
            //moving on to the next level (BUGGY!)
            if (paulPlayer.MemsColl == memories.Count)
            {


                dbGhost.Clear();
                paulPlayer.ObjRect = new Rectangle(400, 400, 50, 50);
                dbPhysical.Clear();
                genBlocks.Clear();
                phaseBlocks.Clear();
                dbPhysical.Clear();
                enemyGhosts.Clear();
                enemyPhys.Clear();
                totalMemories = 0;
                memories.Clear();
                paulPlayer.MemsColl = 0;
                levelCurrent++;
                DrawLevel(levelCurrent);
            }
            framesElapsed = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerFrame);
            frame = framesElapsed % numFrames;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkMagenta);


            // TODO: Add your drawing code here
            spriteBatch.Begin();

            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(Content.Load<Texture2D>("mainbg"), new Rectangle(0, 0, 1024, 768), Color.White);
                    btnPlay.Draw(spriteBatch);
                    btnHelp.Draw(spriteBatch);
                    btnCredit.Draw(spriteBatch);
                    btnQuit.Draw(spriteBatch);
                    break;

                case GameState.Playing:
                    if (paused == true)
                    {
                        spriteBatch.Draw(pauseMenu, pausedRect, Color.White);
                        btnPlay.Draw(spriteBatch);
                        btnRestart.Draw(spriteBatch);
                        btnBackPause.Draw(spriteBatch);
                    }
                    else
                    {
                        for (int i = 0; i < genBlocks.Count; i++)
                        {
                            spriteBatch.Draw(genBlocks[i].GameTexture, genBlocks[i].ObjRect, Color.White);
                        }
                        pauloffset = (54 * frame);
                        if (paulPlayer.IsDead == true)
                        {
                            paulyset = spriteboxheight * 3;
                            numFrames = 4;
                            spriteBatch.Draw(spritesheet, new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, 54, 72), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                            
                            //deadly blocks
                               for (int i = 0; i < dbGhost.Count; i++)
                            {
                                spriteBatch.Draw(dbGhost[i].GameTexture, dbGhost[i].ObjRect, Color.White);
                            }
                            for (int i = 0; i < phaseBlocks.Count; i++)
                            {
                                spriteBatch.Draw(phaseBlocks[i].GameTexture, phaseBlocks[i].ObjRect, Color.White);//this will be transparent one
                            }
                            for (int i = 0; i < genBlocks.Count; i++)
                            {
                                spriteBatch.Draw(genBlocks[i].GameTexture, genBlocks[i].ObjRect, Color.Red);
                            }
                            //ghost enemies
                            for (int i = 0; i < enemyGhosts.Count; i++)
                            {

                                numFrames = 2;
                                if (enemyGhosts[i].MovingDirection == 1)
                                {
                                    // paulyset = spriteboxheight * int.Parse(textures[3]);
                                    spriteBatch.Draw(spritesheet, new Vector2(enemyGhosts[i].ObjRect.X, enemyGhosts[i].ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, 54, 72), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                                }
                                if (enemyGhosts[i].MovingDirection == 3)
                                {
                                    // paulyset = spriteboxheight * int.Parse(textures[3]);
                                    spriteBatch.Draw(spritesheet, new Vector2(enemyGhosts[i].ObjRect.X, enemyGhosts[i].ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, 54, 72), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                                }
                                if (enemyGhosts[i].CanCharge == true && enemyGhosts[i].ChargingState == true)
                                {
                                    if (enemyGhosts[i].MovingDirection == 1)
                                    {
                                        // paulyset = spriteboxheight * int.Parse(textures[12]);
                                        spriteBatch.Draw(spritesheet, new Vector2(enemyGhosts[i].ObjRect.X, enemyGhosts[i].ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, 54, 72), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                                    }
                                    if (enemyGhosts[i].MovingDirection == 3)
                                    {
                                        //  paulyset = spriteboxheight * int.Parse(textures[12]);
                                        spriteBatch.Draw(spritesheet, new Vector2(enemyGhosts[i].ObjRect.X, enemyGhosts[i].ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, 54, 72), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                                    }
                                }
                                if (enemyGhosts[i].MovingDirection == 0 || enemyGhosts[i].MovingDirection == 2) //south and north same animation
                                {
                                    //paulyset = spriteboxheight * int.Parse(textures[4]);
                                    spriteBatch.Draw(spritesheet, new Vector2(enemyGhosts[i].ObjRect.X, enemyGhosts[i].ObjRect.Y), new Rectangle(pauloffset, paulyset, 54, 72), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                                }

                            }
                         
                        }
                        else if (paulPlayer.IsDead == false)
                        {
                            for (int i = 0; i < dbPhysical.Count; i++)
                            {
                                spriteBatch.Draw(dbPhysical[i].GameTexture, dbPhysical[i].ObjRect, Color.White);
                            }
                            for (int i = 0; i < phaseBlocks.Count; i++)
                            {
                                spriteBatch.Draw(phaseBlocks[i].GameTexture, phaseBlocks[i].ObjRect, Color.White);
                            }
                            for (int i = 0; i < genBlocks.Count; i++)
                            {
                                spriteBatch.Draw(genBlocks[i].GameTexture, genBlocks[i].ObjRect, Color.White);
                            }

                            //enemy
                            for (int i = 0; i < enemyPhys.Count; i++)
                            {
                                numFrames = 2;
                                if (enemyPhys[i].MovingDirection == 1)
                                {
                                    //paulyset = spriteboxheight * int.Parse(textures[5]);
                                    spriteBatch.Draw(spritesheet, new Vector2(enemyPhys[i].ObjRect.X, enemyPhys[i].ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, 54, 72), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                                }
                                if (enemyPhys[i].MovingDirection == 3)
                                {
                                    //paulyset = spriteboxheight * int.Parse(textures[5]);
                                    spriteBatch.Draw(spritesheet, new Vector2(enemyPhys[i].ObjRect.X, enemyPhys[i].ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, 54, 72), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                                }
                                if (enemyPhys[i].CanCharge == true && enemyPhys[i].ChargingState == true)
                                {
                                    if (enemyPhys[i].MovingDirection == 1)
                                    {
                                        //paulyset = spriteboxheight * int.Parse(textures[13]);
                                        spriteBatch.Draw(spritesheet, new Vector2(enemyPhys[i].ObjRect.X, enemyPhys[i].ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, 54, 72), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                                    }
                                    if (enemyPhys[i].MovingDirection == 3)
                                    {
                                        //paulyset = spriteboxheight * int.Parse(textures[13]);
                                        spriteBatch.Draw(spritesheet, new Vector2(enemyPhys[i].ObjRect.X, enemyPhys[i].ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, 54, 72), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                                    }
                                }

                                if (enemyPhys[i].MovingDirection == 0 || enemyPhys[i].MovingDirection == 2) //south and north same animation
                                {
                                    //paulyset = spriteboxheight * int.Parse(textures[6]);
                                    spriteBatch.Draw(spritesheet, new Vector2(enemyPhys[i].ObjRect.X, enemyPhys[i].ObjRect.Y), new Rectangle(pauloffset, paulyset, 54, 72), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                                }
                            }
                         

                            //Paul(player)'s sprite animation
                            switch (paulPCurrent)
                            {
                                case PhysicalState.PaulWalkRight:
                                    numFrames = 5;
                                    paulyset = 0;
                                    spriteBatch.Draw(spritesheet, new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, 54, 72), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                                    break;
                                case PhysicalState.PaulWalkLeft:
                                    numFrames = 5;
                                    paulyset = 0;
                                    spriteBatch.Draw(spritesheet, new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, 54, 72), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                                    break;

                                case PhysicalState.PaulFaceLeft:
                                    numFrames = 4;
                                    paulyset = spriteboxheight * 4;
                                    spriteBatch.Draw(spritesheet, new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, 54, 72), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                                    break;
                                case PhysicalState.PaulFaceRight:
                                    numFrames = 4;
                                    paulyset = spriteboxheight * 4;
                                    spriteBatch.Draw(spritesheet, new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, 54, 72), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                                    break;
                                case PhysicalState.PaulJumpLeft:
                                    paulyset = spriteboxheight * 1;
                                    numFrames = 3;
                                    spriteBatch.Draw(spritesheet, new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, 54, 72), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                                    break;
                                case PhysicalState.PaulJumpRight:
                                    paulyset = spriteboxheight * 1;
                                    numFrames = 3;
                                    spriteBatch.Draw(spritesheet, new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, 54, 72), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

                                    break;
                                case PhysicalState.PaulPushLeft:
                                    numFrames = 4;
                                    paulyset = spriteboxheight * 2; ;
                                    spriteBatch.Draw(spritesheet, new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, 54, 72), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);

                                    break;
                                case PhysicalState.PaulPushRight:
                                    numFrames = 4;
                                    paulyset = spriteboxheight * 2;
                                    spriteBatch.Draw(spritesheet, new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, 54, 72), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

                                    break;

                                default:
                                    break;
                            }

                        }
                        for (int i = 0; i < memories.Count; i++)
                        {
                            if (memories[i].HasCollected == false)
                            {
                                spriteBatch.Draw(memories[i].GameTexture, memories[i].ObjRect, Color.White);
                            }
                        }
                    }
                    break;

                case GameState.Help:
                    spriteBatch.Draw(Content.Load<Texture2D>("helpbg"), new Rectangle(0, 0, 1024, 768), Color.White);
                    btnBack.Draw(spriteBatch);
                    break;

                case GameState.Credits:
                    spriteBatch.Draw(Content.Load<Texture2D>("creditsbg"), new Rectangle(0, 0, 1024, 768), Color.White);
                    btnBack.Draw(spriteBatch);
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
