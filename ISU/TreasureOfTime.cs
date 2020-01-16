//Author:           Amy Wang
//File Name:        TreasureOfTime.cs
//Project Name:     ISU
//Creation Date:    December 17, 2018
//Modified Date:    January 20, 2019
/*Description:      Run the game, Treasure of Time, where the player transports between worlds to collect
                    items while avoiding obstacles and enemies*/

using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Animation2D;
using Camera;

namespace ISU
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TreasureOfTime : Game
    {
        //Generate random numbers
        Random rng = new Random();

        //Store screen width and height
        int screenWidth;
        int screenHeight;

        //Control screen displayed
        string switchScreen = "MENU";

        //Store username
        string username = "";

        //Track current world as part of linked list
        World curWorld;
        World head;
        int count;

        //Store total game time
        int gameTimer = 300000;
        int totalTime = 300000;

        //Save time taken for player to pass through worlds
        int timeTaken = 0;

        //Store file path for scoreboard file IO
        string filePath = "";

        //Read and write to scoreboard file
        StreamReader inFile;
        StreamWriter outFile;

        //Control when game timer counts down
        bool isCounting = false;

        //Store required total numbers of items the player
        //must reach to win game
        int[] totals = new int[5];

        //Store keyboard and mouse states
        KeyboardState kb;
        KeyboardState prevKb;
        MouseState mouse;
        MouseState prevMouse;
        
        //Store camera
        Cam2D cam;

        //Store zoom of camera
        float zoom = 1.9f;

        //Store screen rectangle
        Rectangle screenRec;

        //Store background for HUD
        Texture2D HUDBg;

        //Store HUD background rectangles
        Rectangle HUDLeft;
        Rectangle HUDRight;
        Rectangle HUDTop1;
        Rectangle HUDTop2;

        //Store collectible images for HUD
        Texture2D coinImg;
        Texture2D starImg;
        Texture2D mirrorImg;
        Texture2D chaliceImg;
        Texture2D diamondImg;
        Texture2D crownImg;
        
        //Store HUD collectible rectangles
        Rectangle coinIconRec;
        Rectangle starIconRec;
        Rectangle mirrorIconRec;
        Rectangle chaliceIconRec;
        Rectangle diamondIconRec;
        Rectangle crownIconRec;

        //Store HUD potion images
        Texture2D timePotionImg;
        Texture2D healthPotionImg;
        Texture2D shieldPotionImg;

        //Store HUD potion rectangles
        Rectangle timeIconRec;
        Rectangle healthIconRec;
        Rectangle shieldIconRec;
        
        //Store menu images and rectangles
        Texture2D menuBg;
        Texture2D chestImg;
        Rectangle chestRec;
        Rectangle chestRec2;
        Rectangle chestRec3;

        //Store buttons
        Texture2D btnImg;
        Rectangle playRec;
        Rectangle scoreRec;
        Rectangle exitRec;
        Rectangle rulesRec;
        Rectangle btnMenu;
        Rectangle submitRec;
        Rectangle scoreMenuRec;
    
        //Store fonts
        SpriteFont finalTime;
        SpriteFont HUDNumFont;
        SpriteFont HUDFont;
        SpriteFont titleFont;
        SpriteFont btnFont;

        //Store winning and losing backgrounds
        Texture2D loseBg;
        Texture2D winBg;

        //Store scoreboard background
        Texture2D scoreBg;

        //Store rules screen images and buttons
        Texture2D rulesBg;
        Rectangle nextRec;
        Rectangle nextRec2;
        Rectangle rulesMenuBtnRec;

        //Store enemy and obstacle images for rules screen
        Texture2D obsBoxImg;
        Texture2D notObsBoxImg;
        Texture2D bladeImg;
        Texture2D snakeImg;
        Texture2D fireballImg;

        //Store door images for rules screen
        Texture2D doorImg;
        Texture2D shopImg;

        //Store sound effects
        SoundEffect btnSE;
        SoundEffect doorUnlockSE;
        SoundEffect usePotionsSE;
        SoundEffect cashRegisterSE;
        SoundEffect attackSE;
        SoundEffect collectSE;
        SoundEffect winSE;
        SoundEffect loseSE;

        //Store names and scores for scoreboard
        List<int> scoreboard = new List<int>();
        List<string> names = new List<string>();

        //Store max size of scoreboard
        int maxSize = 10;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public TreasureOfTime()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Show mouse on screen
            IsMouseVisible = true;
        
            //Set screen size
            this.graphics.PreferredBackBufferWidth = 1300;
            this.graphics.PreferredBackBufferHeight = 780;

            //Set up multisampling and take off VSync
            graphics.PreferMultiSampling = true;
            graphics.SynchronizeWithVerticalRetrace = false;

            //Apply changes made
            graphics.ApplyChanges();

            //Set screen width and height
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;
            
            base.Initialize();
        }
  
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //Create a new SpriteBatch to draw textures
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            //Add all worlds to linked list
            AddToTail(new Spring(new Vector2(150, 35)));
            AddToTail(new Summer(new Vector2(150, 35)));
            AddToTail(new Autumn(new Vector2(40, 712)));
            AddToTail(new Winter(new Vector2(150, 388)));
            AddToTail(new Final(new Vector2(615, 52)));
            AddToTail(new TreasureChest(new Vector2(30, 712)));

            //Load HUD elements
            LoadHUD();

            //Load screen rectangle
            screenRec = new Rectangle(0, 0, 1300, 780);

            //Load screens
            LoadScreens();

            //Load worlds by tracking current world
            World nextWorld = curWorld;
            
            //Load worlds
            for (int i = 0; i < 6; ++i)
            {
                nextWorld.Load();
                nextWorld.Load(Content, timePotionImg, healthPotionImg, shieldPotionImg);
                
                if (nextWorld.GetNext() != null)
                {
                    nextWorld = nextWorld.GetNext();
                }
            }
            
            //Load player and set player location
            curWorld.GetPlayer().Load(Content, curWorld);
            curWorld.GetPlayer().LoadItems();
            curWorld.GetPlayer().SetPlayerLoc(curWorld.GetInitialLoc());

            //Randomize total stars required
            totals[0] = rng.Next(10, 15);

            //Randomize totals for all other collectibles
            for (int i = 1; i < 5; ++i)
            {
                totals[i] = rng.Next(6, 10);
            }

            //Load camera
            cam = new Cam2D(GraphicsDevice.Viewport, curWorld.GetWorldBounds(), 1.0f, 4.0f, 0f, curWorld.GetPlayer().GetPlayerBounds());
            cam.SetZoom(zoom);
        
            //Store final world font
            finalTime = Content.Load<SpriteFont>("Fonts/FinalTime");

            //Load music and sound effects
            LoadSound();

            //Set initial scoreboard values
            for (int i = 0; i < 10; i++)
            {
                names.Add("name");
                scoreboard.Add(300);
            }
        }
    
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }
   
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            //Store keyboard and mouse states
            prevKb = kb;
            prevMouse = mouse;

            //Get keyboard and mouse states
            kb = Keyboard.GetState();
            mouse = Mouse.GetState();
            
            //Control button clicks
            ButtonClick(gameTime, mouse, prevMouse);

            if (switchScreen == "USERNAME")
            {
                //Determine input for typing username
                DetermineUserInput();

                //Allow for backspacing
                Backspace();
            }
        
            if (isCounting)
            {
                gameTimer -= gameTime.ElapsedGameTime.Milliseconds;
                
                //While the time has not run out:
                if (gameTimer > 0)
                {
                    //Reset camera when transporting to next world
                    cam = new Cam2D(GraphicsDevice.Viewport, curWorld.GetWorldBounds(), 1.0f, 4.0f, 0f,
                    curWorld.GetPlayer().GetPlayerBounds());
                    cam.SetZoom(zoom);
                    curWorld.Camera(cam);

                    //Run the game with all elements
                    RunGame(gameTime);
                    
                    //Allow player to transport to next world
                    Transport();
                }
                else
                {
                    //Execute actions for when game timer or final timer ends
                    TimeEnds();
                }

                //When the player's health reaches 0, they lose the game
                if (curWorld.GetPlayer().GetHealth() == 0)
                {
                    //Pause song
                    curWorld.PlaySong(true);

                    //Play losing sound effect
                    loseSE.CreateInstance().Play();
                   
                    //Save time taken
                    timeTaken = (totalTime - gameTimer) / 1000;
                    
                    //Display losing screen
                    switchScreen = "LOSE";
                    
                    //Reset player health
                    Reset();
                }

                //Make adjustments for when player is in last world
                TreasureChestWorld(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            //Begin displaying images for moving screen
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, 
            cam.GetTransformation());
          
            //Draw world
            if (switchScreen == "WORLD")
            {
                curWorld.Draw(graphics, spriteBatch, curWorld);
                curWorld.GetPlayer().Draw(graphics, spriteBatch);
            }
            
            //Finish displaying images
            spriteBatch.End();

            //Begin displaying non moving screen
            spriteBatch.Begin();
        
            //Display HUD
            DisplayHUD();
            
            //Determine screen to display
            switch (switchScreen)
            {
                case "MENU":
                    DisplayMenu();
                    break;
                case "USERNAME":
                    DisplayUsername();
                    break;
                case "SHOP":
                    DisplayShop();
                    break;
                case "LOSE":
                    DisplayLose();
                    break;
                case "WIN":
                    DisplayWin();
                    break;
                case "SCORES":
                    DisplayScores();
                    break;
                case "RULES":
                    DisplayRules();
                    break;
                case "NEXT1":
                    DisplayNext1();
                    break;
                case "NEXT2":
                    DisplayNext2();
                    break;
            }
            
            //Finish displaying images
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Retrieve first world (head) in linked list
        /// </summary>
        /// <returns>Head World of linked list</returns>
        private World GetHead()
        {
            return head;
        }
    
        /// <summary>
        /// Add world to the head of the linked list (first world in list)
        /// </summary>
        /// <param name="newWorld">World to be added as the head</param>
        private void AddToHead(World newWorld)
        {
            newWorld.SetNext(curWorld);
            curWorld = newWorld;
            head = curWorld;
            count++;
        }

        /// <summary>
        /// Add world to the end of the linked list
        /// </summary>
        /// <param name="newWorld">World to be added to the end</param>
        private void AddToTail(World newWorld)
        {
            if (count == 0)
            {
                //Add world to head when linked list is empty
                AddToHead(newWorld);
            }
            else
            {
                //Start from current world
                World world = curWorld;

                //Loop through to the end of the list
                while (world.GetNext() != null)
                {
                    world = world.GetNext();
                }

                //Set next world as new world
                world.SetNext(newWorld);

                //Increment count
                count++;
            }
        }

        /// <summary>
        /// Determine if player has clicked a rectangle or not
        /// </summary>
        /// <param name="rec">Rectangle to be checked</param>
        /// <param name="mouse">MouseState revealing player's current mouse state</param>
        /// <param name="prevMouse">MouseState revealing player's previous mouse state</param>
        /// <returns>Boolean determining if a rectangle has been clicked</returns>
        public bool Click(Rectangle rec, MouseState mouse, MouseState prevMouse)
        {
            //Check if player presses left mouse button once
            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
            {
                //If player clicks in location of rectangle, rectangle has been clicked
                if (rec.X <= mouse.X && mouse.X <= rec.Right && rec.Y <= mouse.Y && mouse.Y <= rec.Bottom)
                {
                    return true;
                }
            }

            //Return false if rectangle has not been clicked
            return false;
        }

        /// <summary>
        /// Control button clicks and screen changes
        /// </summary>
        /// <param name="gameTime">Time passing in game</param>
        private void ButtonClick(GameTime gameTime, MouseState mouse, MouseState prevMouse)
        {
            //Determine button clicked
            if (Click(playRec, mouse, prevMouse) && switchScreen == "MENU")
            {
                //Reset game
                Reset();

                //Play button sound effect
                btnSE.CreateInstance().Play();

                switchScreen = "USERNAME";
            }
            else if (Click(submitRec, mouse, prevMouse) && switchScreen == "USERNAME")
            {
                //Play button sound effect
                btnSE.CreateInstance().Play();

                //Decrease time left in game
                isCounting = true;

                //Go to world screen
                switchScreen = "WORLD";
            }
            else if (Click(scoreRec, mouse, prevMouse) && switchScreen == "MENU")
            {
                //Play button sound effect
                btnSE.CreateInstance().Play();

                //Go to scoreboard
                switchScreen = "SCORES";
            }
            else if (Click(scoreMenuRec, mouse, prevMouse) && switchScreen == "SCORES")
            {
                //Play button sound effect
                btnSE.CreateInstance().Play();

                //Go back to menu
                switchScreen = "MENU";
            }
            else if (Click(rulesRec, mouse, prevMouse) && switchScreen == "MENU")
            {
                //Play button sound effect
                btnSE.CreateInstance().Play();

                //Display rules when rules button is clicked
                switchScreen = "RULES";
            }
            else if (Click(nextRec, mouse, prevMouse) && switchScreen == "RULES")
            {
                //Play button sound effect
                btnSE.CreateInstance().Play();

                //Go to next screen when next button on rules screen is clicked
                switchScreen = "NEXT1";
            }
            else if (Click(nextRec2, mouse, prevMouse) && switchScreen == "NEXT1")
            {
                //Play button sound effect
                btnSE.CreateInstance().Play();

                //Go to next screen when next button on rules screen is clicked
                switchScreen = "NEXT2";
            }
            else if (Click(rulesMenuBtnRec, mouse, prevMouse) && switchScreen == "NEXT2")
            {
                //Play button sound effect
                btnSE.CreateInstance().Play();
            
                //Go back to menu screen when menu button is clicked from rules screen
                switchScreen = "MENU";
            }
            else if (Click(exitRec, mouse, prevMouse) && switchScreen == "MENU")
            {
                //Play button sound effect
                btnSE.CreateInstance().Play();

                //Exit game when exit button is clicked
                Exit();
            }
            else if (Click(btnMenu, mouse, prevMouse) && switchScreen == "LOSE" || switchScreen == "WIN")
            {
                //Play button sound effect
                btnSE.CreateInstance().Play();

                //Go back to menu
                switchScreen = "MENU";
            }
        
            //Switch between shop and world screens
            if (switchScreen == "WORLD" && curWorld.GetIsInShop())
            {
                //Go to shop screen
                switchScreen = "SHOP";

                //Play door unlock sound effect
                doorUnlockSE.CreateInstance().Play();
            }
            else if (switchScreen == "SHOP" && !curWorld.GetIsInShop())
            {
                //Go to world screen
                switchScreen = "WORLD";
            }
        }
   
        /// <summary>
        /// Reset game by reseting player data and game time
        /// </summary>
        private void Reset()
        {
            Player player = curWorld.GetPlayer();
            curWorld = GetHead();

            curWorld.SetPlayer(player);
            curWorld.GetPlayer().Reset();
            curWorld.SetPlayerLoc(curWorld.GetInitialLoc());
            curWorld.GetPlayer().Load(Content, curWorld);
            
            isCounting = false;
            gameTimer = 300000;
            username = "";
        }

        /// <summary>
        /// Run the game by executing all player actions and game events
        /// </summary>
        /// <param name="gameTime"></param>
        private void RunGame(GameTime gameTime)
        {
            //Play song
            curWorld.PlaySong(false);
        
            //Start final timer when in final world
            curWorld.FinalTimer(gameTime);

            //Reset time boosted from time potion
            curWorld.ResetTimeBoost();

            //Allow for player movement
            curWorld.PlayerMovement(kb, curWorld.GetWorldBounds());
        
            //Update player location and collisions
            curWorld.UpdatePlayer(gameTime, curWorld.GetPlatforms(), curWorld.GetBoxes(), curWorld.GetShopRec());

            //Update world elements (e.g., animations)
            curWorld.UpdateWorld(gameTime, curWorld.GetPlayer().GetPlayerRec(), curWorld);

            //Allow for player to collect collectibles
            curWorld.Collect(gameTime, curWorld.GetCollectibles(), curWorld, collectSE);

            //Allow for shield potion to be used
            if (curWorld.GetIsColDisabled())
            {
                //Use shield potion
                curWorld.UseShield(gameTime);
            }
            else
            {
                //Collision is enabled
                curWorld.Hit(curWorld.GetSnakes(), curWorld.GetBlades(), curWorld.GetBoxes(), curWorld.GetPlatforms(), 
                attackSE);
            }
     
            //Allow player to buy potions
            curWorld.Buy(mouse, prevMouse, curWorld, cashRegisterSE);

            //Allow player to use potions
            curWorld.Use(timeIconRec, "TIME", mouse, prevMouse, usePotionsSE);
            curWorld.Use(healthIconRec, "HEALTH", mouse, prevMouse, usePotionsSE);
            curWorld.Use(shieldIconRec, "SHIELD", mouse, prevMouse, usePotionsSE);

            //Adjust game time when time potion is used
            gameTimer += curWorld.GetTimeBoost();
        }

        /// <summary>
        /// Allow player to transport to the next world
        /// </summary>
        private void Transport()
        {
            //Check if player collides with door
            if (curWorld.GetPlayer().Transport(curWorld.GetDoor()))
            {
                //Stop song
                curWorld.PlaySong(true);

                //Play door unlock sound effect
                doorUnlockSE.CreateInstance().Play();

                //Store player
                Player player = curWorld.GetPlayer();

                //Move to next world
                curWorld = curWorld.GetNext();

                //Store player in new world
                curWorld.SetPlayer(player);
                curWorld.SetPlayerLoc(curWorld.GetInitialLoc());
                curWorld.GetPlayer().Load(Content, curWorld);

                //When transporting to final world, start final timer
                if (curWorld is Final)
                {
                    //Save time taken
                    timeTaken = (totalTime - gameTimer) / 1000;

                    //Start final world timer
                    curWorld.isStarting = true;

                    //Switch to final world timer
                    gameTimer = curWorld.GetTimeLeft();
                }
            }
        }

        /// <summary>
        /// Execute actions for when game timer or final timer ends
        /// </summary>
        private void TimeEnds()
        {
            //Check if the next world is the Treasure Chest world
            if (curWorld.GetNext() is TreasureChest)
            {
                //Pause song
                curWorld.PlaySong(true);

                //Store player
                Player player = curWorld.GetPlayer();

                //Move to treasure chest world
                curWorld = curWorld.GetNext();

                //Store player in new world
                curWorld.SetPlayer(player);
                curWorld.SetPlayerLoc(curWorld.GetInitialLoc());
                curWorld.GetPlayer().Load(Content, curWorld);
            }

            //When the current world is not the Treasure Chest world:
            if (!(curWorld is TreasureChest))
            {
                //Stop timer
                isCounting = false;

                //Pause song
                curWorld.PlaySong(true);

                //Play losing sound effect
                loseSE.CreateInstance().Play();

                //Save time taken
                timeTaken = (totalTime - gameTimer) / 1000;
            
                //Display losing screen
                switchScreen = "LOSE";

                //Reset game
                Reset();
            }
        }

        /// <summary>
        /// Make adjustments for player's abilities in Treasure Chest world
        /// </summary>
        /// <param name="gameTime">Time passing in game</param>
        private void TreasureChestWorld(GameTime gameTime)
        {
            //Store treasure chest animation's last frame number
            int chestEndFrame = 5;

            //Check if player is in Treasure Chest world
            if (curWorld is TreasureChest)
            {
                curWorld.PlaySong(false);
            
                //Reset camera
                cam = new Cam2D(GraphicsDevice.Viewport, curWorld.GetWorldBounds(), 1.0f, 4.0f, 0f,
                curWorld.GetPlayer().GetPlayerBounds());
                cam.SetZoom(zoom);
                curWorld.Camera(cam);

                //Allow for player movement
                curWorld.PlayerMovement(kb, curWorld.GetWorldBounds());
            
                //Update player location
                curWorld.UpdatePlayer(gameTime, curWorld.GetPlatforms(), curWorld.GetBoxes(), curWorld.GetShopRec());

                //Update world elements
                curWorld.UpdateWorld(gameTime, curWorld.GetPlayer().GetPlayerRec(), curWorld);

                //If player opens chest:
                if (curWorld.GetPlayer().OpenChest(curWorld))
                {
                    //Animate chest
                    curWorld.SetIsAnimating(true);

                    //When chest animations finishes:
                    if (curWorld.GetChestFrame() == chestEndFrame)
                    {
                        //Pause song
                        curWorld.PlaySong(true);

                        //Stop animation
                        curWorld.SetIsAnimating(false);

                        //Check if player wins or loses
                        CheckWinLose();
                    }
                }
            }
        }
   
        /// <summary>
        /// Check when player wins or loses the game
        /// </summary>
        private void CheckWinLose()
        {
            //Store player
            Player player = curWorld.GetPlayer();

            //Check if player has collected all required numbers of collectibles
            if (player.GetNumStars() >= totals[0] && player.GetNumMirrors() >= totals[1] && player.GetNumChalices() >= totals[2]
                && player.GetNumDiamonds() >= totals[3] && player.GetNumCrowns() >= totals[4])
            {
                //Pause song
                curWorld.PlaySong(true);

                //Play winning sound effect
                winSE.CreateInstance().Play();
                
                //Add to scoreboard
                AddToScoreboard();

                //If so, display winning screen
                switchScreen = "WIN";

                //Reset game
                Reset();
            }
            else
            {
                //Pause song
                curWorld.PlaySong(true);

                //Play losing sound effect
                loseSE.CreateInstance().Play();
                
                //If not, display losing screen
                switchScreen = "LOSE";
                
                //Reset game
                Reset();
            }
        }
        
        /// <summary>
        /// Load music and sound effects
        /// </summary>
        private void LoadSound()
        {
            //Load sound effects
            btnSE = Content.Load<SoundEffect>("SoundEffects/ButtonSE");
            doorUnlockSE = Content.Load<SoundEffect>("SoundEffects/DoorUnlockSE");
            usePotionsSE = Content.Load<SoundEffect>("SoundEffects/PotionsSE");
            cashRegisterSE = Content.Load<SoundEffect>("SoundEffects/CashRegisterSE");
            attackSE = Content.Load<SoundEffect>("SoundEffects/AttackSE");
            collectSE = Content.Load<SoundEffect>("SoundEffects/CollectSE");
            winSE = Content.Load<SoundEffect>("SoundEffects/WinSE");
            loseSE = Content.Load<SoundEffect>("SoundEffects/LoseSE");
        }

        /// <summary>
        /// Load menu images and fonts
        /// </summary>
        private void LoadScreens()
        {
            //Load menu background
            menuBg = Content.Load<Texture2D>("Backgrounds/MenuBg");
        
            //Load chests
            chestImg = Content.Load<Texture2D>("Sprites/SingleChest");
            chestRec = new Rectangle(380, 450, (int)(chestImg.Width * 0.3), (int)(chestImg.Height * 0.3));
            chestRec2 = new Rectangle(280, 450, (int)(chestImg.Width * 0.3), (int)(chestImg.Height * 0.3));
            chestRec3 = new Rectangle(480, 450, (int)(chestImg.Width * 0.3), (int)(chestImg.Height * 0.3));

            //Load buttons
            btnImg = Content.Load<Texture2D>("Sprites/Button");

            playRec = new Rectangle(890, 100, (int)(btnImg.Width * 0.7), (int)(btnImg.Height * 0.7));
            rulesRec = new Rectangle(890, 250, (int)(btnImg.Width * 0.7), (int)(btnImg.Height * 0.7));
            scoreRec = new Rectangle(890, 400, (int)(btnImg.Width * 0.7), (int)(btnImg.Height * 0.7));
            exitRec = new Rectangle(890, 550, (int)(btnImg.Width * 0.7), (int)(btnImg.Height * 0.7));

            btnMenu = new Rectangle(500, 420, (int)(btnImg.Width * 0.7), (int)(btnImg.Height * 0.7));
            nextRec = new Rectangle(220, 655, (int)(btnImg.Width * 0.5), (int)(btnImg.Height * 0.5));
            nextRec2 = new Rectangle(1030, 655, (int)(btnImg.Width * 0.5), (int)(btnImg.Height * 0.5));
            rulesMenuBtnRec = new Rectangle(220, 655, (int)(btnImg.Width * 0.5), (int)(btnImg.Height * 0.5));
            submitRec = new Rectangle(500, 470, (int)(btnImg.Width * 0.7), (int)(btnImg.Height * 0.7));
            scoreMenuRec = new Rectangle(540, 660, (int)(btnImg.Width * 0.5), (int)(btnImg.Height * 0.5));

            //Load losing background
            loseBg = Content.Load<Texture2D>("Backgrounds/LoseBg");
        
            //Load winning background
            winBg = Content.Load<Texture2D>("Backgrounds/WinBg");

            //Load scoreboard background
            scoreBg = Content.Load<Texture2D>("Backgrounds/ScoreboardBg");

            //Load rules background
            rulesBg = Content.Load<Texture2D>("Backgrounds/RulesBg");

            //Load enemy and obstacle images
            obsBoxImg = Content.Load<Texture2D>("Obstacles/ObsBox");
            notObsBoxImg = Content.Load<Texture2D>("Obstacles/NotObsBox");
            bladeImg = Content.Load<Texture2D>("Obstacles/Blade");
            snakeImg = Content.Load<Texture2D>("Sprites/SingleSnake");
            fireballImg = Content.Load<Texture2D>("Sprites/SingleFireball");

            //Load door images
            doorImg = Content.Load<Texture2D>("Sprites/AutumnDoor");
            shopImg = Content.Load<Texture2D>("Sprites/AutumnShopDoor");

            //Load fonts
            titleFont = Content.Load<SpriteFont>("Fonts/Title");
            btnFont = Content.Load<SpriteFont>("Fonts/Btn");
        }

        /// <summary>
        /// Display menu with buttons
        /// </summary>
        private void DisplayMenu()
        {
            //Display background
            spriteBatch.Draw(menuBg, screenRec, Color.White);

            //Display title
            spriteBatch.DrawString(titleFont, "Treasure of", new Vector2(150, 150), Color.White);
            spriteBatch.DrawString(titleFont, "Time", new Vector2(290, 270), Color.White);

            //Display treasure chests
            spriteBatch.Draw(chestImg, chestRec, Color.White);
            spriteBatch.Draw(chestImg, chestRec2, Color.White);
            spriteBatch.Draw(chestImg, chestRec3, Color.White);

            //Display buttons
            spriteBatch.Draw(btnImg, playRec, Color.White);
            spriteBatch.Draw(btnImg, rulesRec, Color.White);
            spriteBatch.Draw(btnImg, scoreRec, Color.White);
            spriteBatch.Draw(btnImg, exitRec, Color.White);
        
            //Display names
            spriteBatch.DrawString(btnFont, "Play", new Vector2(980, 130), Color.Black);
            spriteBatch.DrawString(btnFont, "Rules", new Vector2(970, 280), Color.Black);
            spriteBatch.DrawString(btnFont, "Scores", new Vector2(955, 430), Color.Black);
            spriteBatch.DrawString(btnFont, "Exit", new Vector2(980, 580), Color.Black);
        }

        /// <summary>
        /// Display username screen
        /// </summary>
        private void DisplayUsername()
        {
            //Display background
            spriteBatch.Draw(menuBg, screenRec, Color.White);

            //Display username
            spriteBatch.DrawString(titleFont, "Enter Username:", new Vector2(150, 100), Color.White);
            spriteBatch.DrawString(btnFont, "Max Length is 8 Letters!", new Vector2(150, 210), Color.White);
            spriteBatch.DrawString(btnFont, "" + username, new Vector2(150, 310), Color.White);
        
            //Display submit button
            spriteBatch.Draw(btnImg, submitRec, Color.White);
            spriteBatch.DrawString(btnFont, "Submit", new Vector2(560, 500), Color.Black);
        }

        /// <summary>
        /// Determine letters typed for username
        /// </summary>
        private void DetermineUserInput()
        {
            UserInputLetters(Keys.A, "a");
            UserInputLetters(Keys.B, "b");
            UserInputLetters(Keys.C, "c");
            UserInputLetters(Keys.D, "d");
            UserInputLetters(Keys.E, "e");
            UserInputLetters(Keys.F, "f");
            UserInputLetters(Keys.G, "g");
            UserInputLetters(Keys.H, "h");
            UserInputLetters(Keys.I, "i");
            UserInputLetters(Keys.J, "j");
            UserInputLetters(Keys.K, "k");
            UserInputLetters(Keys.L, "l");
            UserInputLetters(Keys.M, "m");
            UserInputLetters(Keys.N, "n");
            UserInputLetters(Keys.O, "o");
            UserInputLetters(Keys.P, "p");
            UserInputLetters(Keys.Q, "q");
            UserInputLetters(Keys.R, "r");
            UserInputLetters(Keys.S, "s");
            UserInputLetters(Keys.T, "t");
            UserInputLetters(Keys.U, "u");
            UserInputLetters(Keys.V, "v");
            UserInputLetters(Keys.W, "w");
            UserInputLetters(Keys.X, "x");
            UserInputLetters(Keys.Y, "y");
            UserInputLetters(Keys.Z, "z");
        }

        /// <summary>
        /// Add letters to username as username is typed
        /// </summary>
        /// <param name="key">Key pressed</param>
        /// <param name="text">Key's corresponding letter</param>
        private void UserInputLetters(Keys key, string text)
        {
            //Store max length
            int maxLength = 8;

            //Create username
            if (!prevKb.IsKeyDown(key) && kb.IsKeyDown(key))
            {
                if (username.Length < maxLength)
                {
                    username += text;
                }
            }
        }

        /// <summary>
        /// Allow for backspacing when typing username
        /// </summary>
        private void Backspace()
        {
            if (!prevKb.IsKeyDown(Keys.Back) && kb.IsKeyDown(Keys.Back))
            {
                //Remove last letter/number
                if (username.Length > 0)
                {
                    username = username.Substring(0, username.Length - 1);
                }
                else
                {
                    username = "";
                }
            }
        }

        /// <summary>
        /// Load HUD elements
        /// </summary>
        private void LoadHUD()
        {
            //Load fonts
            HUDFont = Content.Load<SpriteFont>("Fonts/HUD");
            HUDNumFont = Content.Load<SpriteFont>("Fonts/HUDNum");

            //Load background
            HUDBg = Content.Load<Texture2D>("Sprites/HUD");

            //Load rectangles for background
            HUDLeft = new Rectangle(12, 90, 63, 350);
            HUDRight = new Rectangle(1227, 90, 63, 635);
            HUDTop1 = new Rectangle(10, 10, 200, 40);
            HUDTop2 = new Rectangle(250, 10, 200, 40);

            //Load collectible icon images
            coinImg = Content.Load<Texture2D>("Collectibles/SingleCoin");
            starImg = Content.Load<Texture2D>("Collectibles/Star");
            mirrorImg = Content.Load<Texture2D>("Collectibles/Mirror");
            chaliceImg = Content.Load<Texture2D>("Collectibles/Chalice");
            diamondImg = Content.Load<Texture2D>("Collectibles/Diamond");
            crownImg = Content.Load<Texture2D>("Collectibles/Crown");
        
            //Load rectangles for collectibles
            coinIconRec = new Rectangle(1232, 100, 50, 50);
            starIconRec = new Rectangle(1233, 205, 50, 50);
            mirrorIconRec = new Rectangle(1234, 310, 50, 50);
            chaliceIconRec = new Rectangle(1242, 415, 35, 65);
            diamondIconRec = new Rectangle(1234, 535, 50, 50);
            crownIconRec = new Rectangle(1233, 640, 50, 50);

            //Load potion icon images
            timePotionImg = Content.Load<Texture2D>("Shop/TimePotion");
            healthPotionImg = Content.Load<Texture2D>("Shop/HealthPotion");
            shieldPotionImg = Content.Load<Texture2D>("Shop/ShieldPotion");

            //Load rectangles for potions
            timeIconRec = new Rectangle(20, 100, 50, 50);
            healthIconRec = new Rectangle(20, 220, 50, 50);
            shieldIconRec = new Rectangle(20, 340, 55, 50);
        }
   
        /// <summary>
        /// Display HUD
        /// </summary>
        private void DisplayHUD()
        {
            //Check if the world screen is displayed and the player is not in Treasure Chest world
            if (switchScreen == "WORLD" && !(curWorld is TreasureChest))
            {
                //Display time left in game
                spriteBatch.Draw(HUDBg, HUDTop1, Color.White * 0.8f);
                spriteBatch.DrawString(HUDFont, "Time Left: " + gameTimer / 1000 + " s", new Vector2(15, 15), Color.Black);

                //Display player health
                spriteBatch.Draw(HUDBg, HUDTop2, Color.White * 0.8f);
                spriteBatch.DrawString(HUDFont, "Health: " + curWorld.GetPlayer().GetHealth(), new Vector2(255, 15), Color.Black);

                //Display collectibles
                spriteBatch.Draw(HUDBg, HUDRight, Color.White * 0.8f);
                spriteBatch.Draw(coinImg, coinIconRec, Color.White);
                spriteBatch.Draw(starImg, starIconRec, Color.White);
                spriteBatch.Draw(mirrorImg, mirrorIconRec, Color.White);
                spriteBatch.Draw(chaliceImg, chaliceIconRec, Color.White);
                spriteBatch.Draw(diamondImg, diamondIconRec, Color.White);
                spriteBatch.Draw(crownImg, crownIconRec, Color.White);
            
                //Dipslay numbers of collectibles
                spriteBatch.DrawString(HUDNumFont, "" + curWorld.GetPlayer().GetNumCoins(), 
                new Vector2(1240, 155), Color.Black);
                spriteBatch.DrawString(HUDNumFont, "" + curWorld.GetPlayer().GetNumStars() + 
                "/" + totals[0], new Vector2(1241, 260), Color.Black);
                spriteBatch.DrawString(HUDNumFont, "" + curWorld.GetPlayer().GetNumMirrors() + 
                "/" + totals[1], new Vector2(1242, 365), Color.Black);
                spriteBatch.DrawString(HUDNumFont, "" + curWorld.GetPlayer().GetNumChalices() + 
                "/" + totals[2], new Vector2(1241, 485), Color.Black);
                spriteBatch.DrawString(HUDNumFont, "" + curWorld.GetPlayer().GetNumDiamonds() + 
                "/" + totals[3], new Vector2(1242, 590), Color.Black);
                spriteBatch.DrawString(HUDNumFont, "" + curWorld.GetPlayer().GetNumCrowns() + 
                "/" + totals[4], new Vector2(1241, 695), Color.Black);
            }
        
            //Check when the player is not in the Final world and Treasure Chest world
            if (switchScreen == "WORLD" && !curWorld.isStarting && !(curWorld is TreasureChest))
            {
                //Display potions
                spriteBatch.Draw(HUDBg, HUDLeft, Color.White * 0.8f);
                spriteBatch.Draw(timePotionImg, timeIconRec, Color.White);
                spriteBatch.Draw(healthPotionImg, healthIconRec, Color.White);
                spriteBatch.Draw(shieldPotionImg, shieldIconRec, Color.White);
            
                //Display number of potions
                spriteBatch.DrawString(HUDNumFont, "Time:\n" + curWorld.GetPlayer().GetNumTime(), 
                new Vector2(25, 155), Color.Black);
                spriteBatch.DrawString(HUDNumFont, "Health:\n" + curWorld.GetPlayer().GetNumHealth(), 
                new Vector2(25, 275), Color.Black);
                spriteBatch.DrawString(HUDNumFont, "Shield:\n" + curWorld.GetPlayer().GetNumShield(), 
                new Vector2(25, 395), Color.Black);
            }
       
            //Display shield time when shield potion is used
            if (curWorld.GetIsColDisabled())
            {
                spriteBatch.DrawString(HUDFont, "Shield Time: " + curWorld.GetShieldTime() / 1000 + " sec.", 
                new Vector2(110, 200), Color.Red);
            }
       
            //Display final time when in final world
            if (curWorld.isStarting)
            {
                spriteBatch.DrawString(finalTime, "Time Left: " + curWorld.GetTimeLeft() / 1000, 
                new Vector2(500, 150), Color.Red);
            }
        }

        /// <summary>
        /// Display shop
        /// </summary>
        private void DisplayShop()
        {
            curWorld.GetShop().Draw(graphics, spriteBatch, curWorld.GetPlayer().GetNumCoins());
        }

        /// <summary>
        /// Display losing screen
        /// </summary>
        private void DisplayLose()
        {
            //Display background
            spriteBatch.Draw(loseBg, screenRec, Color.White);

            //Display losing message
            spriteBatch.DrawString(btnFont, "Congratulations, You Lost the Game!", new Vector2(200, 100), Color.White);

            //Display time taken
            spriteBatch.DrawString(btnFont, "Time Taken: " + timeTaken + " s", new Vector2(390, 220), Color.White);

            //Display menu button
            spriteBatch.Draw(btnImg, btnMenu, Color.White);
            spriteBatch.DrawString(btnFont, "Menu", new Vector2(570, 450), Color.Black);
        }
   
        /// <summary>
        /// Display winning screen
        /// </summary>
        private void DisplayWin()
        {
            //Display background
            spriteBatch.Draw(winBg, screenRec, Color.White);

            //Display winning message
            spriteBatch.DrawString(btnFont, "Congratulations, You Won the Game!", new Vector2(190, 100), Color.Black);
        
            //Display time taken
            spriteBatch.DrawString(btnFont, "Time Taken: " + timeTaken + " s", new Vector2(390, 220), Color.Black);

            //Display menu button
            spriteBatch.Draw(btnImg, btnMenu, Color.White);
            spriteBatch.DrawString(btnFont, "Menu", new Vector2(570, 450), Color.Black);
        }
 
        /// <summary>
        /// Display first page of rules of game
        /// </summary>
        private void DisplayRules()
        {
            //Display background
            spriteBatch.Draw(rulesBg, screenRec, Color.White);

            //Display title
            spriteBatch.DrawString(btnFont, "Playing the Game", new Vector2(445, 40), Color.White);

            //Display worlds
            spriteBatch.DrawString(HUDFont, "Spring --> Summer --> Autumn --> Winter --> Final --> Treasure Chest", 
            new Vector2(250, 150), Color.Yellow);
       
            //Display worlds explanation
            spriteBatch.DrawString(HUDFont, 
            "Playing the game involves passing through the four main worlds represented by the seasons: Spring, Summer,\n" +
            "Autumn, and Winter. Each world contains a collectible unique to the world and to win the game, the player must\n" +
            "collect the required numbers of collectibles by the time they reach the Treasure Chest World.\n", new Vector2(50, 230), 
            Color.White);

            //Display collectibles
            spriteBatch.Draw(mirrorImg, new Rectangle(120, 350, 50, 50), Color.White);
            spriteBatch.Draw(chaliceImg, new Rectangle(320, 340, 35, 65), Color.White);
            spriteBatch.Draw(diamondImg, new Rectangle(520, 350, 50, 50), Color.White);
            spriteBatch.Draw(crownImg, new Rectangle(720, 350, 50, 50), Color.White);
            spriteBatch.Draw(starImg, new Rectangle(920, 350, 50, 50), Color.White);
            spriteBatch.Draw(coinImg, new Rectangle(1120, 350, 50, 50), Color.White);

            //Display collectible names
            spriteBatch.DrawString(HUDFont, "Mirrors\nSpring World", new Vector2(100, 430), Color.Yellow);
            spriteBatch.DrawString(HUDFont, "Chalices\nSummer World", new Vector2(300, 430), Color.Yellow);
            spriteBatch.DrawString(HUDFont, "Diamonds\nAutumn World", new Vector2(500, 430), Color.Yellow);
            spriteBatch.DrawString(HUDFont, "Crowns\nWinter World", new Vector2(700, 430), Color.Yellow);
            spriteBatch.DrawString(HUDFont, "Stars\nAll Worlds", new Vector2(910, 430), Color.Yellow);
            spriteBatch.DrawString(HUDFont, "Coins\nAll Worlds", new Vector2(1110, 430), Color.Yellow);

            //Display potions explanation
            spriteBatch.DrawString(HUDFont, "Coins are used to purchase potions in shops found in the\nfirst four worlds. " +
            "The potions' powers are randomized\nand the player clicks on the potions in the left side\nbar to use them", 
            new Vector2(50, 540), Color.White);

            //Display potions
            spriteBatch.Draw(timePotionImg, new Rectangle(700, 540, 90, 90), Color.White);
            spriteBatch.Draw(healthPotionImg, new Rectangle(910, 540, 83, 90), Color.White);
            spriteBatch.Draw(shieldPotionImg, new Rectangle(1110, 540, 90, 90), Color.White);

            //Display potion names
            spriteBatch.DrawString(HUDFont, "Time Potion\nBoosts time left", new Vector2(700, 650), Color.Yellow);
            spriteBatch.DrawString(HUDFont, "Health Potion\nBoosts health", new Vector2(910, 650), Color.Yellow);
            spriteBatch.DrawString(HUDFont, "Shield Potion\nShields against\nenemies and\nobstacles", new Vector2(1110, 650), 
            Color.Yellow);

            //Display next page button
            spriteBatch.Draw(btnImg, nextRec, Color.White);
            spriteBatch.DrawString(btnFont, "Next", new Vector2(263, 668), Color.Black);
        }

        /// <summary>
        /// Display second page of rules of game
        /// </summary>
        private void DisplayNext1()
        {
            //Display background
            spriteBatch.Draw(rulesBg, screenRec, Color.White);

            //Display title
            spriteBatch.DrawString(btnFont, "Worlds", new Vector2(560, 40), Color.White);

            //Display worlds explanation
            spriteBatch.DrawString(HUDFont,
            "Once the player passes through the world door, the player is transported to the next world and cannot return back,\n" +
            "to the previous world. As well, the player must keep in mind the differences in the Final and Treasure Chest World:\n\n" +
            "The Final World contains all collectibles but only lasts for 15 seconds, providing the player with their last chance\n" +
            "to obtain collectibles while the Treasure Chest World does not contain collectibles and only allows the player to open\n" +
            "the treasure chest to determine if the player has won the game or not. It is checked if the player collected the\n" +
            "required numbers of collectibles and the chest opens when the player stands on a specific platform indicated in the world", 
            new Vector2(50, 150), Color.White);

            //Display title
            spriteBatch.DrawString(btnFont, "Enemies and Obstacles", new Vector2(370, 380), Color.White);

            //Display enemies and obstacles
            spriteBatch.Draw(snakeImg, new Rectangle(100, 480, 180, 130), Color.White);
            spriteBatch.Draw(fireballImg, new Rectangle(300, 480, 120, 120), Color.White);
            spriteBatch.Draw(bladeImg, new Rectangle(500, 480, 70, 120), Color.White);
            spriteBatch.Draw(notObsBoxImg, new Rectangle(700, 500, 90, 90), Color.White);
            spriteBatch.Draw(obsBoxImg, new Rectangle(900, 500, 90, 90), Color.White);

            //Display names
            spriteBatch.DrawString(HUDFont, "Snake\nEnemy\nSteals random\ncollectibles", new Vector2(100, 610), Color.Yellow);
            spriteBatch.DrawString(HUDFont, "Fireball\nEnemy\nSteals random\ncollectibles", new Vector2(300, 610), Color.Yellow);
            spriteBatch.DrawString(HUDFont, "Blade\nObstacle\nSteals random\nhealth", new Vector2(500, 610), Color.Yellow);
            spriteBatch.DrawString(HUDFont, "Box\nNOT\nobstacle", new Vector2(700, 610), Color.Yellow);
            spriteBatch.DrawString(HUDFont, "Box\nObstacle", new Vector2(900, 610), Color.Yellow);
            spriteBatch.DrawString(HUDFont, "There are two boxes but\nonly one is an obstacle\nthat steals health", new Vector2(1015, 500), 
            Color.Yellow);
        
            //Display next page button
            spriteBatch.Draw(btnImg, nextRec2, Color.White);
            spriteBatch.DrawString(btnFont, "Next", new Vector2(1070, 668), Color.Black);
        }

        /// <summary>
        /// Display third page of rules of game
        /// </summary>
        private void DisplayNext2()
        {
            //Display background
            spriteBatch.Draw(rulesBg, screenRec, Color.White);

            //Display title
            spriteBatch.DrawString(btnFont, "Winning and Losing", new Vector2(410, 40), Color.White);

            //Display worlds explanation
            spriteBatch.DrawString(HUDFont,
            "The player loses when the time runs out for the player to reach the Final World and when the player reaches\n" +
            "0 health. If the player survives until the Final World, the player then has 15 seconds left before automatically\n" +
            "being transported to the Treasure Chest World. To win, the player must collect all required numbers of collectibles.\n",
            new Vector2(50, 150), Color.White);

            //Display player movement explanation
            spriteBatch.DrawString(btnFont, "Player Movement", new Vector2(110, 300), Color.White);
            spriteBatch.DrawString(HUDFont,
            "In addition to running, jumping, and ducking, the\nplayer is also able to jump while falling, allowing\n" +
            "the player to easily move between platforms.", new Vector2(50, 400), Color.White);

            //Display doors explanation
            spriteBatch.DrawString(btnFont, "Doors", new Vector2(880, 300), Color.White);
            spriteBatch.DrawString(HUDFont,
            "To transport to the next world, the player must simply\nplace themselves in front of the world door's image.\n" +
            "The main four worlds contain two doors: the world door\nand the shop door (which is labelled as shop).", 
            new Vector2(670, 400), Color.White);

            //Display doors
            spriteBatch.Draw(doorImg, new Rectangle(800, 580, 110, 170), Color.White);
            spriteBatch.Draw(shopImg, new Rectangle(1000, 580, 110, 170), Color.White);
        
            //Display menu button
            spriteBatch.Draw(btnImg, rulesMenuBtnRec, Color.White);
            spriteBatch.DrawString(btnFont, "Menu", new Vector2(255, 668), Color.Black);
        }
    
        /// <summary>
        /// Display top 10 scoreboard
        /// </summary>
        private void DisplayScores()
        {
            //Store initial score and name Y values
            int scoreY = 130;
            int nameY = 130;

            //Store amount for adding to Y values
            int add = 50;

            //Display background
            spriteBatch.Draw(scoreBg, screenRec, Color.White);
        
            //Display title
            spriteBatch.DrawString(btnFont, "Scoreboard", new Vector2(500, 40), Color.Black);

            //Display scores
            foreach (int score in scoreboard)
            {
                spriteBatch.DrawString(btnFont, "" + score, new Vector2(800, scoreY), Color.Black);
                scoreY += add;
            }

            //Display names
            foreach (string name in names)
            {
                spriteBatch.DrawString(btnFont, name, new Vector2(300, nameY), Color.Black);
                nameY += add;
            }

            //Display menu button
            spriteBatch.Draw(btnImg, scoreMenuRec, Color.White);
            spriteBatch.DrawString(btnFont, "Menu", new Vector2(573, 670), Color.Black);
        }
        
        /// <summary>
        /// Add to scoreboard if time is low enough to make the scoreboard
        /// </summary>
        private void AddToScoreboard()
        {
            //Add to scoreboard if lowest time
            if (timeTaken <= scoreboard[0])
            {
                scoreboard.Insert(0, timeTaken);
                names.Insert(0, username);

                scoreboard.RemoveAt(maxSize - 1);
                names.RemoveAt(maxSize - 1);
            }
        
            //Determine position on scoreboard
            for (int i = maxSize - 1; i >= 1; --i)
            {
                if (totalTime <= scoreboard[i] && totalTime > scoreboard[i - 1])
                {
                    scoreboard.Insert(i, timeTaken);
                    names.Insert(i, username);

                    scoreboard.RemoveAt(maxSize - 1);
                    names.RemoveAt(maxSize - 1);
                }
            }
        }
    }
}
