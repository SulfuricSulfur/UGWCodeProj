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
        // various rectangles and textures
        StreamReader reader;
        string[] textures;
        Texture2D floor;
        Texture2D sides;
        Texture2D top;
        Rectangle floorrect;
        Rectangle siderectL;
        Rectangle siderectR;
        Rectangle toprect;

        // level attributes
        int level;
        string lvl;

        //The lists for the levels. Include all the instanses of objects being read in from the textfile
        //a list containing the names of the textfiles being read in sequentially
        //and a list containing a strain of the level data to be read in
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
        List<PhaseBlock> physicalPhaseB = new List<PhaseBlock>();

        // attributes for sprites
        int pauloffset2 = 0;
        int frame2;
        int numFrames2 = 2;
        int pauloffset = 0;
        int paulyset = 0;
        int spriteboxheight = 50;
        int frame;
        const double timePerFrame = 200;
        const int SIZES = 50;//will be used for the sizes of enemies+objects
        int numFrames = 3;
        int framesElapsed;
        int levelCurrent = 0;//The current level the player is on(in terms of the list)
        int totalMemories; //the total amount of memories
        int playerSpd; //keeping track of the current speed of the player
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
        //block textures
        Texture2D memorytexture;
        Texture2D phaseBlockTexture;
        Texture2D physicalPBlock;
        //deadly block textures
        Texture2D physdeadlyObjs;
        Texture2D physDeadlyObj2;
        Texture2D deadlyGhostObj;
        Texture2D deadlyGhostObject2;
        //other blocks
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

        //sprite in the ghost state will only be one state, so there does not need to be an enum for it.
        // animation enumerators
        enum PhysicalState { PaulFaceRight, PaulFaceLeft, PaulWalkRight, PaulWalkLeft, PaulJumpRight, PaulJumpLeft };
        PhysicalState paulPCurrent = PhysicalState.PaulFaceRight;//default
        enum GhostState { FloatRight, FloatLeft };
        GhostState ghoststate = GhostState.FloatRight;

        // GameState enumerator
        enum GameState
        {
            MainMenu,
            Playing,
            Help,
            Credits,
            EndScreen
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
        Button btnResume;

        // pause menu attributes
        bool paused = false;
        Texture2D pauseMenu;
        Rectangle pausedRect;


        public Game1()
            : base()
        {
            // game window related stuff
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            Content.RootDirectory = "Content";
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
            //each line represents each level
            //[0] floor, [1] side borders, [2] top border,  [3]  enemy1ghost, [4]  enemy 2ghost,[5] enemy1phys, [6] enemy2phys
            //[7] float1, [8] float2, [9] moving block, [10] transblock ghost [11] transblock physical [12] enemychargephys [13] enemychargeghost
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

            // initialize rectangles
            //playerPos = new Vector2(300, 300);
            toprect = new Rectangle(40, 0, 1000, 40);
            siderectL = new Rectangle(0, 0, 50, 942);
            siderectR = new Rectangle(1000, 0, 50, 942);
            floorrect = new Rectangle(39, 740, 1000, 40);

            // initialized menu buttons
            btnPlay = new Button(Content.Load<Texture2D>("playbutton"), graphics.GraphicsDevice);
            btnHelp = new Button(Content.Load<Texture2D>("helpbutton"), graphics.GraphicsDevice);
            btnCredit = new Button(Content.Load<Texture2D>("creditsbutton"), graphics.GraphicsDevice);
            btnQuit = new Button(Content.Load<Texture2D>("quitbutton"), graphics.GraphicsDevice);
            btnBack = new Button(Content.Load<Texture2D>("backbutton"), graphics.GraphicsDevice);
            btnBackPause = new Button(Content.Load<Texture2D>("mainbutton"), graphics.GraphicsDevice);
            btnRestart = new Button(Content.Load<Texture2D>("restartbutton"), graphics.GraphicsDevice);
            btnResume = new Button(Content.Load<Texture2D>("resumebutton"), graphics.GraphicsDevice);

            // setting button positions
            btnPlay.setPosition(new Vector2(448, 300));
            btnHelp.setPosition(new Vector2(448, 400));
            btnCredit.setPosition(new Vector2(448, 500)); //credits
            btnQuit.setPosition(new Vector2(448, 600));
            btnBack.setPosition(new Vector2(148, 600));
            btnBackPause.setPosition(new Vector2(448, 550)); //the main menu button in the pause menu
            btnRestart.setPosition(new Vector2(448, 450)); //restart button in the pause button
            btnResume.setPosition(new Vector2(448, 350));

            // pause menu
            pauseMenu = Content.Load<Texture2D>("pausebg");
            pausedRect = new Rectangle(0, 0, pauseMenu.Width, pauseMenu.Height);

            base.Initialize();
        }

        /// <summary>
        /// Levels are added into a list. The text file for each level is read in level by level
        /// and then stored in a list which contains the strings of each level 
        /// starting from 0 in the list (level 1) onwards
        /// levelcurrent keeps track of what level the player is currently on
        /// which will be used for reading the next level in the list.
        /// </summary>
        public void LoadLevels()
        {
            // streamreader for read in levels
            StreamReader lRead;
            string[] fileNames = Directory.GetFiles("Levels");
            try//if the level doesnt exist then it will auto advance onto the level after that
            {
                foreach(string names in fileNames)
                {
                    lFiles.Add(names);
                }
   
            }
            catch (Exception ex)
            {
                if (levelCurrent == lFiles.Count)
                {
                    CurrentGameState = GameState.EndScreen;
                }
                else
                {
                    levelCurrent++;
                }
            }


            // loop each line into game
            for (int i = 0; i < lFiles.Count; i++)
            {
                try
                {
                    lRead = new StreamReader(lFiles[i]);
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
                catch (IOException)
                {
                    //Console.WriteLine(l + " was not found");
                    lFiles.Remove(lFiles[i]);
                }
            }
        }


        /// <summary>
        /// Reads in each string in the list of levels (depending on what level the player is currently in)
        /// and loops through and puts each item based on character in a list based on its type
        /// </summary>
        /// <param name="lNum"></param>
        public void DrawLevel(int lNum)
        {
            mapX = 0;
            // when you beat all levels currently in files, game ends and win screen appears
            if (lNum >= lFiles.Count)
            {
                CurrentGameState = GameState.EndScreen;
                levelCurrent = 0;

            }
            else if (lFiles.Count == 0)
            {
                CurrentGameState = GameState.EndScreen;
            }

            else
            {
                mapY = 42;
                genBlocks.Add(new GeneralBlock(toprect, top));
                genBlocks.Add(new GeneralBlock(siderectR, sides));
                genBlocks.Add(new GeneralBlock(siderectL, sides));
                genBlocks.Add(new GeneralBlock(floorrect, floor));
                Random rnd = new Random();
                //Check for characters
                //Check for characters
                char[] levelLetters = levelDisplay[lNum].ToCharArray();
                foreach (char c in levelLetters)//this is a temporay thing. Textures will change
                {
                    //make this 1 player object
                    if (c == '@')
                    {
                        paulRect = new Rectangle(mapX, mapY, 48, 48);
                        playerPos = new Vector2(paulRect.X, paulRect.Y);
                        paulPlayer = new Player(paulRect, paulPlayer.GameTexture, playerPos, false);
                        playerSpd = paulPlayer.MoveSpeed;

                    }

                    else if (c == 'f')//floating grass blocks
                        genBlocks.Add(new GeneralBlock(new Rectangle(mapX, mapY, SIZES, SIZES), basicFloat));

                    else if (c == 'm')//memories(collect to pass to next level)
                    {
                        memories.Add(new Memories(new Rectangle(mapX, mapY, SIZES, SIZES), memorytexture));
                        totalMemories++;
                    }
                    else if (c == 'B')//The physical phase blocks
                    {
                        physicalPhaseB.Add(new PhaseBlock(new Rectangle(mapX, mapY, SIZES, SIZES), physicalPBlock));
                    }

                    else if (c == 'x')//ground solid blocks
                        genBlocks.Add(new GeneralBlock(new Rectangle(mapX, mapY, SIZES, SIZES), basicGround));

                        //Deadly blocks
                    else if (c == '~')
                        dbGhost.Add(new DeadlyBlock(new Rectangle(mapX, mapY, SIZES, SIZES), deadlyGhostObj));

                    else if (c == '!')
                        dbGhost.Add(new DeadlyBlock(new Rectangle(mapX, mapY, SIZES, SIZES), deadlyGhostObject2)); //will have a different texture

                    else if (c == 'd')
                        dbPhysical.Add(new DeadlyBlock(new Rectangle(mapX, mapY, SIZES, SIZES), physdeadlyObjs));

                    else if (c == 'D')
                        dbPhysical.Add(new DeadlyBlock(new Rectangle(mapX, mapY, SIZES, SIZES), physDeadlyObj2));//will have a different texture

                    //Enemies

                        //up and down ghost - starting up
                    else if (c == '^')
                    {
                        int speed = rnd.Next(1, 5);
                        enemyGhosts.Add(new Enemy(true, new Rectangle(mapX, mapY, SIZES, SIZES), paulPlayer.GameTexture, 2, speed, false));
                    }

                        //up and down ghost, starting down
                    else if (c == 'v')
                    {
                        int speed = rnd.Next(1, 5);
                        enemyGhosts.Add(new Enemy(true, new Rectangle(mapX, mapY, SIZES, SIZES), paulPlayer.GameTexture, 0, speed, false));
                    }

                        //physical deadly enemy (non chargable) starting off to the left
                    else if (c == '<')
                    {
                        int speed = rnd.Next(1, 5);
                        enemyGhosts.Add(new Enemy(true, new Rectangle(mapX, mapY, SIZES, SIZES), paulPlayer.GameTexture, 1, speed, false));
                    }

                    //physical deadly enemy (non chargable) starting off to the right
                    if (c == '>')
                    {
                        int speed = rnd.Next(1, 5);
                        enemyGhosts.Add(new Enemy(true, new Rectangle(mapX, mapY, SIZES, SIZES), paulPlayer.GameTexture, 3, speed, false));
                    }

                        //physical deadly enemy (non chargable) starting off to the left
                    else if (c == '{')
                    {
                        int speed = rnd.Next(1, 5);
                        enemyPhys.Add(new Enemy(false, new Rectangle(mapX, mapY, SIZES, SIZES), paulPlayer.GameTexture, 1, speed, false));
                    }

                         //physical deadly enemy (non chargable) starting off to the right
                    else if (c == '}')
                    {
                        int speed = rnd.Next(1, 5);
                        enemyPhys.Add(new Enemy(false, new Rectangle(mapX, mapY, SIZES, SIZES), paulPlayer.GameTexture, 3, speed, false));
                    }

                        //physical deadly enemy (chargable) starting off to the left
                    else if (c == '[')
                    {

                        enemyPhys.Add(new Enemy(false, new Rectangle(mapX, mapY, SIZES, SIZES), paulPlayer.GameTexture, 1, 3, true));
                    }

                        //physical deadly enemy (chargable) starting off to the right
                    else if (c == ']')
                    {
                        enemyPhys.Add(new Enemy(false, new Rectangle(mapX, mapY, SIZES, SIZES), paulPlayer.GameTexture, 3, 3, true));
                    }

                    else if (c == 'p')
                        phaseBlocks.Add(new PhaseBlock(new Rectangle(mapX, mapY, SIZES, SIZES), phaseBlockTexture));

                    else if (c == 'n')
                    {
                        mapY += SIZES;
                        mapX = 0;
                    }
                    //Window Dimensions: 1024 x 768
                    mapX += SIZES;
                    //
                }
            }

        }



        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // loading in spritesheets + textures
            spritesheet = Content.Load<Texture2D>("Paul_Spritesheet.png");
            paulPhysical = Content.Load<Texture2D>("paulstand1.png");
            floor = Content.Load<Texture2D>(textures[0]);
            sides = Content.Load<Texture2D>(textures[1]);
            top = Content.Load<Texture2D>(textures[2]);
            phaseBlockTexture = Content.Load<Texture2D>(textures[10]);

            // loading player and interactable objects to player
            paulRect = new Rectangle(300, 300, paulPhysical.Width, paulPhysical.Height);
            paulPlayer = new Player(paulRect, paulPhysical, playerPos, false);
            physdeadlyObjs = Content.Load<Texture2D>("DeadlyBlockPhys.png");
            physDeadlyObj2 = Content.Load<Texture2D>("DeadlyBlockPhys2.png");
            deadlyGhostObj = Content.Load<Texture2D>("DeadlyBlockGhost.png");
            deadlyGhostObject2 = Content.Load<Texture2D>("DeadlyBlockGhost2.png");
            paulGhost = Content.Load<Texture2D>("paulfloat.png");
            basicFloat = Content.Load<Texture2D>("floatgrass.png");
            basicGround = Content.Load<Texture2D>("connectivebottom.png");
            memorytexture = Content.Load<Texture2D>("movableblockgrass.png");
            physicalPBlock = Content.Load<Texture2D>("TransBlockPhysGrass.png");
            genBlocks.Add(new GeneralBlock(toprect, top));
            genBlocks.Add(new GeneralBlock(siderectR, sides));
            genBlocks.Add(new GeneralBlock(siderectL, sides));
            genBlocks.Add(new GeneralBlock(floorrect, floor));

            // load levels
            LoadLevels();
        }


        /// <summary>
        /// Player.Move method has been added here to make it easier with the enum/state machine
        /// </summary>
        protected void ProcessInput()
        {
            //if player lands on ground .hasJumped = false so that the player
            //can jump again
            //the key controls change depending on if paul is dead or alive
            //control handling
            kboardstate = Keyboard.GetState();
            if (paulPlayer.IsDead == false)
            {
                playerPos += velocity;
                paulPlayer.MoveSpeed = playerSpd;
                paulPlayer.ObjRect = new Rectangle((int)playerPos.X, (int)playerPos.Y, paulPlayer.ObjRect.Width, paulPlayer.ObjRect.Height);
                //this.ObjRect = new Rectangle( )
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
                
                //Gravity and jumping v
                if (kboardstate.IsKeyDown(Keys.Space) && hasJumped == false)
                {
                    //jumping  
                    playerPos.Y += 4f;
                    velocity.Y += -5.1f;
                    hasJumped = true;
                    playerPos += velocity;
                    // jumping animations
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
                    velocity.Y += 0.21f;
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
            // animations for ghost player
            else if (paulPlayer.IsDead == true)
            {
                paulPlayer.ObjRect = new Rectangle((int)playerPos.X, (int)playerPos.Y, paulPlayer.ObjRect.Width, paulPlayer.ObjRect.Height);
                if (kboardstate.IsKeyDown(Keys.A))
                {
                    ghoststate = GhostState.FloatLeft;
                    playerPos.X -= paulPlayer.MoveSpeed;
                }
                if (kboardstate.IsKeyDown(Keys.D))
                {
                    ghoststate = GhostState.FloatRight;
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
        /// detects collison of the blocks and the enemy and players colliding with said blocks
        /// </summary>
        protected void DetectCollison()
        {
            if(paulPlayer.IsDead == true)
            {
                for(int i = 0; i < physicalPhaseB.Count; i++)
                {
                    BlockCollison(physicalPhaseB[i].ObjRect);
                }
            }

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
                for (int b = 0; b < enemyGhosts.Count; b++)
                {
                    enemyGhosts[b].EnemyCollide(phaseBlocks[i].ObjRect, true);
                }
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
            //Testing ghost enemy collision with other blocks. 
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
                for(int b = 0; b < physicalPhaseB.Count; b++)
                {
                    enemyGhosts[i].EnemyCollide(physicalPhaseB[b].ObjRect, true);
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
                    enemyPhys[i].EnemyCollide(phaseBlocks[q].ObjRect, false);
                }
                for (int r = 0; r < genBlocks.Count; r++)
                {
                    enemyPhys[i].EnemyCollide(genBlocks[r].ObjRect, false);
                }

            }
        }



        /// <summary>
        /// Checks to see if the top, bottom, or side collides with the player
        /// takes in the rectangle of a block object(general,phase,deadly)
        /// and returns a bool if the player is intersecting 
        /// </summary>
        /// <param name="blockRec"></param>
        /// <returns></returns>
        protected bool BlockCollison(Rectangle blockRec)
        {
            //First checks for intersection with player
            if (paulPlayer.ObjRect.Intersects(blockRec))
            {
                //If on top, will reset has jumped
                if (blockRec.Top - paulPlayer.ObjRect.Bottom >= -15 && blockRec.Top - paulPlayer.ObjRect.Bottom < 0 && blockRec.Left + 15 <= paulPlayer.ObjRect.Right && blockRec.Right - 15 >= paulPlayer.ObjRect.X)
                {
                    paulPlayer.ObjRect = new Rectangle((int)playerPos.X, blockRec.Top - paulPlayer.ObjRect.Height, paulPlayer.ObjRect.Width, paulPlayer.ObjRect.Height);
                    playerPos += velocity;
                    playerPos = new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y);
                    hasJumped = false;
                    velocity.Y = 0f;
                }
                //Checks left side of block
                else if ((blockRec.Left - paulPlayer.ObjRect.Right) >= -15 && (blockRec.Left - paulPlayer.ObjRect.Right) < 0)
                {
                    paulPlayer.ObjRect = new Rectangle(blockRec.Left - paulPlayer.ObjRect.Width, (int)playerPos.Y, paulPlayer.ObjRect.Width, paulPlayer.ObjRect.Height);
                    playerPos += velocity;
                    playerPos = new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y);
                }
                //Checks right side of block
                else if ((paulPlayer.ObjRect.Left - blockRec.Right) >= -15 && (paulPlayer.ObjRect.Left - blockRec.Right) < 0)
                {
                    paulPlayer.ObjRect = new Rectangle(blockRec.Right, (int)playerPos.Y, paulPlayer.ObjRect.Width, paulPlayer.ObjRect.Height);
                    playerPos += velocity;
                    playerPos = new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y);
                }
                //Checks under side of block
                else if (paulPlayer.ObjRect.Top - blockRec.Bottom >= -15 && paulPlayer.ObjRect.Top - blockRec.Bottom < 0)
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
        /// This method will be called whenever we intend to clean the screen/restart
        /// instead of constantly writing all this code out.
        /// Clears the lists, memories, and player position 
        /// </summary>
        public void ClearingScreen()
        {

            dbGhost.Clear();
            dbPhysical.Clear();
            genBlocks.Clear();
            phaseBlocks.Clear();
            dbPhysical.Clear();
            enemyGhosts.Clear();
            enemyPhys.Clear();
            memories.Clear();
            physicalPhaseB.Clear();
            totalMemories = 0;
            paulPlayer.MemsColl = 0;
            playerPos = new Vector2(paulRect.X, paulRect.Y);
            paulPlayer = new Player(paulRect, paulPlayer.GameTexture, playerPos, false);
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
            // mouse state, used for menus
            MouseState mouse = Mouse.GetState();

            // menu gamestates
            switch (CurrentGameState)
            {
                // main menu
                case GameState.MainMenu:
                    // changing gamestates by clicking a button
                    if (btnHelp.isClicked == true) CurrentGameState = GameState.Help;
                    if (btnCredit.isClicked == true) CurrentGameState = GameState.Credits;
                    if (btnQuit.isClicked == true) Environment.Exit(1);

                    // button reacts when mouse is hovering over it
                    btnPlay.Update(mouse);
                    btnHelp.Update(mouse);
                    btnCredit.Update(mouse);
                    btnQuit.Update(mouse);
                    IsMouseVisible = true;

                    // starting the game
                    if (btnPlay.isClicked == true)
                    {
                        btnPlay.isClicked = false;
                        levelCurrent = 0;

                        DrawLevel(levelCurrent);
                        playerPos = new Vector2(paulRect.X, paulRect.Y);
                        paulPlayer = new Player(paulRect, paulPlayer.GameTexture, playerPos, false);
                        CurrentGameState = GameState.Playing;
                    }
                    break;

                //the levels and the code for the game itself
                case GameState.Playing:

                    // when game is not paused
                    if (paused == false)
                    {
                        //if the player has collected all the memories
                        if (paulPlayer.MemsColl == memories.Count)
                        {
                            ClearingScreen();
                            levelCurrent++;
                            DrawLevel(levelCurrent);
                        }

                        // entering pause menu
                        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                        {
                            paused = true;
                            btnResume.isClicked = false;
                            btnPlay.isClicked = false;
                            btnBackPause.isClicked = false;
                            btnRestart.isClicked = false;
                            IsMouseVisible = true;
                        }

                        // is player alive or dead?
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

                    // game is paused
                    else
                    {
                        // unpause game
                        if (btnResume.isClicked == true)
                        {
                            paused = false;
                            IsMouseVisible = false;
                        }
                        // back to main menu
                        if (btnBackPause.isClicked == true)
                        {
                            paused = false;
                            //if the player goes back to the main menu, they will go back to level 1.
                            levelCurrent = 0;
                            ClearingScreen();
                            CurrentGameState = GameState.MainMenu;
                        }
                        //the player restarts the stage, everything on the level is reset back to the position they are on
                        //the text file
                        if (btnRestart.isClicked == true)
                        {
                            paused = false;
                            ClearingScreen();
                            DrawLevel(levelCurrent);
                            CurrentGameState = GameState.Playing;
                        }

                        // buttons react to mouse hovering
                        btnPlay.Update(mouse);
                        btnResume.Update(mouse);
                        btnBackPause.Update(mouse);
                        btnRestart.Update(mouse);
                    }
                    break;

                // help screen
                case GameState.Help:
                    if (btnBack.isClicked == true) CurrentGameState = GameState.MainMenu;
                    btnBack.Update(mouse);
                    IsMouseVisible = true;
                    break;

                // credits screen
                case GameState.Credits:
                    if (btnBack.isClicked == true) CurrentGameState = GameState.MainMenu;
                    btnBack.Update(mouse);
                    IsMouseVisible = true;
                    break;

                // win screen
                case GameState.EndScreen:
                    if (btnBackPause.isClicked == true) CurrentGameState = GameState.MainMenu;
                    btnBackPause.Update(mouse);
                    IsMouseVisible = true;
                    break;
            }

            // call other methods into update
            ProcessInput();
            DetectCollison();
            framesElapsed = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerFrame);
            frame = framesElapsed % numFrames;
            frame2 = framesElapsed % numFrames2;

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // default color
            GraphicsDevice.Clear(Color.DarkMagenta);
            spriteBatch.Begin();

            // gamestates
            switch (CurrentGameState)
            {
                // main menu
                case GameState.MainMenu:
                    // draw buttons and menu background
                    spriteBatch.Draw(Content.Load<Texture2D>("mainMenu"), new Rectangle(0, 0, 1024, 768), Color.White);
                    btnPlay.Draw(spriteBatch);
                    btnHelp.Draw(spriteBatch);
                    btnCredit.Draw(spriteBatch);
                    btnQuit.Draw(spriteBatch);
                    break;

                // the game
                case GameState.Playing:

                    // pause menu
                    if (paused == true)
                    {
                        spriteBatch.Draw(pauseMenu, pausedRect, Color.White);
                        btnResume.Draw(spriteBatch);
                        btnRestart.Draw(spriteBatch);
                        btnBackPause.Draw(spriteBatch);
                    }

                    // not paused menu
                    else
                    {
                        
                        // loop loads in blocks
                        for (int i = 0; i < genBlocks.Count; i++)
                        {
                            spriteBatch.Draw(genBlocks[i].GameTexture, genBlocks[i].ObjRect, Color.White);
                        }
                        pauloffset = (SIZES * frame);

                        for (int i = 0; i < physicalPhaseB.Count; i++ )
                        {
                            spriteBatch.Draw(physicalPhaseB[i].GameTexture, physicalPhaseB[i].ObjRect, Color.White);//drawing the phase block that you can phase through as a person
                        }
                            // draw when paul is dead
                            if (paulPlayer.IsDead == true)
                            {
                                //deadly blocks
                                for (int i = 0; i < genBlocks.Count; i++)
                                {
                                    spriteBatch.Draw(genBlocks[i].GameTexture, genBlocks[i].ObjRect, Color.Red);
                                }
                                paulyset = spriteboxheight * 3;
                                numFrames = 4;
                                switch (ghoststate)
                                {
                                    case GhostState.FloatRight:
                                        spriteBatch.Draw(spritesheet, new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, SIZES, SIZES), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                                        break;
                                    case GhostState.FloatLeft:
                                        spriteBatch.Draw(spritesheet, new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, SIZES, SIZES), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                                        break;
                                }

                                for (int i = 0; i < dbGhost.Count; i++)
                                {
                                    spriteBatch.Draw(dbGhost[i].GameTexture, dbGhost[i].ObjRect, Color.White);
                                }
                                for (int i = 0; i < phaseBlocks.Count; i++)
                                {
                                    spriteBatch.Draw(phaseBlocks[i].GameTexture, phaseBlocks[i].ObjRect, Color.White);//this will be transparent one
                                }

                                //ghost enemies
                                for (int i = 0; i < enemyGhosts.Count; i++)
                                {

                                    numFrames = 2;
                                    if (enemyGhosts[i].MovingDirection == 1)
                                    {
                                        if (enemyGhosts[i].CanCharge)
                                        {
                                            paulyset = spriteboxheight * int.Parse(textures[3]);
                                            spriteBatch.Draw(spritesheet, new Vector2(enemyGhosts[i].ObjRect.X, enemyGhosts[i].ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, SIZES, SIZES), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

                                        }
                                        else
                                        {
                                            paulyset = spriteboxheight * int.Parse(textures[3]);
                                            spriteBatch.Draw(spritesheet, new Vector2(enemyGhosts[i].ObjRect.X, enemyGhosts[i].ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, SIZES, SIZES), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                                        }
                                    }
                                    if (enemyGhosts[i].MovingDirection == 3)
                                    {
                                        if (enemyGhosts[i].CanCharge)
                                        {
                                            paulyset = spriteboxheight * int.Parse(textures[3]);
                                            spriteBatch.Draw(spritesheet, new Vector2(enemyGhosts[i].ObjRect.X, enemyGhosts[i].ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, SIZES, SIZES), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                                        }
                                        else
                                        {
                                            paulyset = spriteboxheight * int.Parse(textures[3]);
                                            spriteBatch.Draw(spritesheet, new Vector2(enemyGhosts[i].ObjRect.X, enemyGhosts[i].ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, SIZES, SIZES), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                                        }
                                    }
                                    if (enemyGhosts[i].MovingDirection == 0 || enemyGhosts[i].MovingDirection == 2) //south and north same animation
                                    {
                                        paulyset = spriteboxheight * int.Parse(textures[4]);
                                        spriteBatch.Draw(spritesheet, new Vector2(enemyGhosts[i].ObjRect.X, enemyGhosts[i].ObjRect.Y), new Rectangle(pauloffset, paulyset, SIZES, SIZES), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                                    }

                                }

                            }

                            // draw if paul is alive
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
                                    numFrames2 = 2;
                                    if (frame2 > 1)
                                    {
                                        frame2 = 0;
                                    }
                                    pauloffset2 = (SIZES * frame2);
                                    if (enemyPhys[i].MovingDirection == 1)
                                    {
                                        if (enemyPhys[i].CanCharge)
                                        {
                                            paulyset = spriteboxheight * int.Parse(textures[12]);
                                            spriteBatch.Draw(spritesheet, new Vector2(enemyPhys[i].ObjRect.X, enemyPhys[i].ObjRect.Y), new Rectangle(pauloffset2 + frame2, paulyset, SIZES, SIZES), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

                                        }
                                        else
                                        {
                                            paulyset = spriteboxheight * int.Parse(textures[6]);
                                            spriteBatch.Draw(spritesheet, new Vector2(enemyPhys[i].ObjRect.X, enemyPhys[i].ObjRect.Y), new Rectangle(pauloffset2 + frame2, paulyset, SIZES, SIZES), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                                        }
                                    }
                                    if (enemyPhys[i].MovingDirection == 3)
                                    {
                                        if (enemyPhys[i].CanCharge)
                                        {
                                            paulyset = spriteboxheight * int.Parse(textures[12]);
                                            spriteBatch.Draw(spritesheet, new Vector2(enemyPhys[i].ObjRect.X, enemyPhys[i].ObjRect.Y), new Rectangle(pauloffset2 + frame2, paulyset, SIZES, SIZES), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);

                                        }
                                        else
                                        {
                                            paulyset = spriteboxheight * int.Parse(textures[6]);
                                            spriteBatch.Draw(spritesheet, new Vector2(enemyPhys[i].ObjRect.X, enemyPhys[i].ObjRect.Y), new Rectangle(pauloffset2 + frame2, paulyset, SIZES, SIZES), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                                        }
                                    }
                                }

                                //player's sprite animation
                                switch (paulPCurrent)
                                {
                                    case PhysicalState.PaulWalkRight:
                                        numFrames = 5;
                                        paulyset = 0;
                                        spriteBatch.Draw(spritesheet, new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, SIZES, SIZES), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                                        break;
                                    case PhysicalState.PaulWalkLeft:
                                        numFrames = 5;
                                        paulyset = 0;
                                        spriteBatch.Draw(spritesheet, new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, SIZES, SIZES), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                                        break;

                                    case PhysicalState.PaulFaceLeft:
                                        numFrames = 4;
                                        paulyset = spriteboxheight * 4;
                                        spriteBatch.Draw(spritesheet, new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, SIZES, SIZES), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                                        break;
                                    case PhysicalState.PaulFaceRight:
                                        numFrames = 4;
                                        paulyset = spriteboxheight * 4;
                                        spriteBatch.Draw(spritesheet, new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, SIZES, SIZES), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                                        break;
                                    case PhysicalState.PaulJumpLeft:
                                        paulyset = spriteboxheight * 1;
                                        numFrames = 3;
                                        spriteBatch.Draw(spritesheet, new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, SIZES, SIZES), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                                        break;
                                    case PhysicalState.PaulJumpRight:
                                        paulyset = spriteboxheight * 1;
                                        numFrames = 3;
                                        spriteBatch.Draw(spritesheet, new Vector2(paulPlayer.ObjRect.X, paulPlayer.ObjRect.Y), new Rectangle(pauloffset + frame, paulyset, SIZES, SIZES), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

                                        break;
                                    default:
                                        break;
                                }

                            }

                     
                        // memories
                        for (int i = 0; i < memories.Count; i++)
                        {
                            if (memories[i].HasCollected == false)
                            {               
                                paulyset = spriteboxheight * 10;
                                spriteBatch.Draw(spritesheet, new Vector2(memories[i].ObjRect.X, memories[i].ObjRect.Y), new Rectangle(pauloffset2 + frame2, paulyset, SIZES, SIZES), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                            }
                        }

                    }
                    break;

                // help screen
                case GameState.Help:
                    spriteBatch.Draw(Content.Load<Texture2D>("helpbg"), new Rectangle(0, 0, 1024, 768), Color.White);
                    btnBack.Draw(spriteBatch);
                    break;

                // credits screen
                case GameState.Credits:
                    spriteBatch.Draw(Content.Load<Texture2D>("creditsbg"), new Rectangle(0, 0, 1024, 768), Color.White);
                    btnBack.Draw(spriteBatch);
                    break;

                // win screen
                case GameState.EndScreen:
                    spriteBatch.Draw(Content.Load<Texture2D>("endscreen"), new Rectangle(0, 0, 1024, 768), Color.White);
                    btnBackPause.Draw(spriteBatch);
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
