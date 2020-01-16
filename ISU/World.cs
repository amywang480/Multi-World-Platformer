//Author:           Amy Wang
//File Name:        World.cs
//Project Name:     ISU
//Creation Date:    December 19, 2018
//Modified Date:    January 20, 2019
//Description:      Manage all worlds in game by drawing worlds and executing player actions

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Animation2D;
using Camera;
using System.IO;

namespace ISU
{
    class World
    {
        //Read in file
        protected StreamReader inFile;

        //Store file path
        protected string filePath = "";
    
        //Generate random numbers
        protected Random rng = new Random();
        
        //Create player
        Player player = new Player();
  
        //Store next world for player to enter
        World nextWorld;

        //Store world and background bounds
        protected Rectangle worldBounds;
        protected Rectangle bgBoundsLeft;
        protected Rectangle bgBoundsRight;
        protected Rectangle bgBoundsEnd;

        //Store world door image and rec
        protected Texture2D worldDoor;
        protected Rectangle worldDoorRec;

        //Store world shop image and rec
        protected Texture2D shopDoor;
        protected Rectangle shopDoorRec;

        //Store player location
        protected Vector2 playerLoc;

        //Store image used when reading in platforms from file
        protected Texture2D img;

        //Determine if box is obstacle when reading in boxes from file
        protected bool isObs;
    
        //Determine when Final world timer starts
        public bool isStarting = false;

        //Store tile images
        protected Texture2D bg;
        protected Texture2D baseTile;
        protected Texture2D col;
        protected Texture2D top;
        protected Texture2D leftSlant;
        protected Texture2D rightSlant;
        protected Texture2D leftSlantMid;
        protected Texture2D rightSlantMid;
        protected Texture2D inSlant;
        protected Texture2D dirt;
        protected Texture2D colDown;

        //Store world platforms
        protected Platform[] platforms = new Platform[100];
        
        //Store locations of collectibles and collectibles
        protected Vector2[] collectLocs = new Vector2[20];
        protected CollectibleManager[] collectibles = new CollectibleManager[20];

        //Store enemies
        protected Enemy[] snakes = new Snake[10];
        protected Enemy fireball = new Fireball();

        //Store obstacles
        protected Obstacle[] blades = new Obstacle[10];
        protected Obstacle[] boxes = new Obstacle[20];

        //Store numbers of world elements
        protected int numPlatforms;
        protected int numSnakes;
        protected int numBlades;
        protected int numBoxes;

        //Store shop
        Shop shop;

        //Store world song
        protected Song bgSong;

        public World(Vector2 playerLoc)
        {

        }

        /// <summary>
        /// Retrieve initial player location
        /// </summary>
        /// <returns>Initial player location</returns>
        public virtual Vector2 GetInitialLoc()
        {
            return new Vector2();
        }

        /// <summary>
        /// Retrieve player
        /// </summary>
        /// <returns>The player created from Player class</returns>
        public virtual Player GetPlayer()
        {
            return player;
        }

        /// <summary>
        /// Retrieve world platforms
        /// </summary>
        /// <returns>null when base class version is called</returns>
        public virtual Platform[] GetPlatforms()
        {
            return null;
        }

        /// <summary>
        /// Retrieve snakes in world
        /// </summary>
        /// <returns>Array of snakes</returns>
        public virtual Enemy[] GetSnakes()
        {
            return null;
        }

        /// <summary>
        /// Retrieve blades in world
        /// </summary>
        /// <returns>Array of blades</returns>
        public virtual Obstacle[] GetBlades()
        {
            return null;
        }

        /// <summary>
        /// Retrieve boxes in world
        /// </summary>
        /// <returns>Array of boxes</returns>
        public virtual Obstacle[] GetBoxes()
        {
            return null;
        }
    
        /// <summary>
        /// Retrieve world bounds
        /// </summary>
        /// <returns>World bounds as rectangles</returns>
        public virtual Rectangle GetWorldBounds()
        {
            return worldBounds;
        }

        /// <summary>
        /// Retrieve shop in world
        /// </summary>
        /// <returns>Shop</returns>
        public Shop GetShop()
        {
            return shop;
        }
       
        /// <summary>
        /// Retrieve shop door rectangle
        /// </summary>
        /// <returns>Shop door rectangle</returns>
        public virtual Rectangle GetShopRec()
        {
            return new Rectangle();
        }

        /// <summary>
        /// Retrieve to determine if player is in shop
        /// </summary>
        /// <returns>Boolean determining if player is in shop</returns>
        public bool GetIsInShop()
        {
            return player.GetIsInShop();
        }
        
        /// <summary>
        /// Retrieve time boost from time potion
        /// </summary>
        /// <returns>Amount of time from time potion</returns>
        public int GetTimeBoost()
        {
            return player.timeBoost;
        }

        /// <summary>
        /// Retrieve shield time from shield potion
        /// </summary>
        /// <returns>Shield time</returns>
        public int GetShieldTime()
        {
            return player.shieldTimer;
        }

        /// <summary>
        /// Retrievde to determine if collision is disabled
        /// </summary>
        /// <returns>Boolean determining if collision is disabled</returns>
        public bool GetIsColDisabled()
        {
            return player.isColDisabled;
        }
        
        /// <summary>
        /// Retrieve player health
        /// </summary>
        /// <returns>Player health</returns>
        public int GetHealth()
        {
            return player.GetHealth();
        }
        
        /// <summary>
        /// Retrieve world door
        /// </summary>
        /// <returns>Door rectangle</returns>
        public virtual Rectangle GetDoor()
        {
            return new Rectangle();
        }

        /// <summary>
        /// Retrieve time left in Final world
        /// </summary>
        /// <returns>Time left</returns>
        public virtual int GetTimeLeft()
        {
            return 0;
        }

        /// <summary>
        /// Retrieve frame number of chest animation
        /// </summary>
        /// <returns>Frame number</returns>
        public virtual int GetChestFrame()
        {
            return 0;
        }

        /// <summary>
        /// Retrieve background song
        /// </summary>
        /// <returns>Background song</returns>
        public virtual Song GetSong()
        {
            return null;
        }

        /// <summary>
        /// Set world's player
        /// </summary>
        /// <param name="player">Player</param>
        public virtual void SetPlayer(Player player)
        {
            this.player = player;
        }

        /// <summary>
        /// Set player location
        /// </summary>
        /// <param name="playerLoc">New player location</param>
        public virtual void SetPlayerLoc(Vector2 playerLoc)
        {
            player.SetPlayerLoc(playerLoc);
        }

        /// <summary>
        /// Retrieve world collectibles
        /// </summary>
        /// <returns>null when base class version is called</returns>
        public virtual CollectibleManager[] GetCollectibles()
        {
            return null;
        }
        
        /// <summary>
        /// Set when chest from Treasure Chest world animates
        /// </summary>
        /// <param name="isAnimating">Boolean to determine if animating</param>
        public virtual void SetIsAnimating(bool isAnimating)
        {

        }

        /// <summary>
        /// Access next world for player to enter
        /// </summary>
        /// <returns>Next world after player's current world</returns>
        public World GetNext()
        {
            return nextWorld;
        }

        /// <summary>
        /// Set the next world for player to enter
        /// </summary>
        /// <param name="nextWorld">Next world in linked list of worlds</param>
        public void SetNext(World nextWorld)
        {
            this.nextWorld = nextWorld;
        }
        
        /// <summary>
        /// Determine file path
        /// </summary>
        /// <param name="file">Name of file</param>
        protected void FilePath(string file)
        {
            //Determine file path
            filePath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            filePath = Path.GetDirectoryName(filePath);
            filePath = filePath.Substring(6);
            filePath += "/" + file + ".txt";

            //Open file
            inFile = File.OpenText(filePath);
        }

        /// <summary>
        /// Read in data for platforms to create them
        /// </summary>
        /// <param name="num">Number of platforms</param>
        protected void Platforms(ContentManager Content, int num)
        {
            string[] data;

            for (int i = 0; i < num; ++i)
            {
                data = inFile.ReadLine().Split(' ');
            
                switch (data[4])
                {
                    case "BASETILE":
                        img = baseTile;
                        break;
                    case "TOP":
                        img = top;
                        break;
                    case "COL":
                        img = col;
                        break;
                    case "LEFTSLANT":
                        img = leftSlant;
                        break;
                    case "RIGHTSLANT":
                        img = rightSlant;
                        break;
                    case "LEFTSLANTMID":
                        img = leftSlantMid;
                        break;
                    case "RIGHTSLANTMID":
                        img = rightSlantMid;
                        break;
                    case "DIRT":
                        img = dirt;
                        break;
                    case "INSLANT":
                        img = inSlant;
                        break;
                    case "COLDOWN":
                        img = colDown;
                        break;
                }

                int x = Convert.ToInt32(data[0]);
                int y = Convert.ToInt32(data[1]);
                int tilesW = Convert.ToInt32(data[2]);
                int tilesH = Convert.ToInt32(data[3]);
                double scale = Convert.ToDouble(data[5]);

                platforms[i] = new Platform(x, y, tilesW, tilesH, img, scale);
            }
        }
  
        /// <summary>
        /// Read in data for collectible locations to create them (for Final World)
        /// </summary>
        /// <param name="Content"></param>
        protected void CollectLocs(ContentManager Content)
        {
            string line;
            string[] data;

            line = inFile.ReadLine();

            for (int i = 0; i < collectibles.Length; ++i)
            {
                data = inFile.ReadLine().Split(' ');

                int x = Convert.ToInt32(data[0]);
                int y = Convert.ToInt32(data[1]);
               
                collectibles[i] = new CollectibleManager(new Vector2(x, y), Content);
            }
        }

        /// <summary>
        /// Read in data for collectible locations to create them (for all other worlds)
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="item">Name of collectible unique to world</param>
        protected void CollectLocs(ContentManager Content, string item)
        {
            string line;
            string[] data;

            line = inFile.ReadLine();

            for (int i = 0; i < collectibles.Length; ++i)
            {
                data = inFile.ReadLine().Split(' ');

                int x = Convert.ToInt32(data[0]);
                int y = Convert.ToInt32(data[1]);
                
                collectibles[i] = new CollectibleManager(new Vector2(x, y), item, Content);
            }
        }

        /// <summary>
        /// Read in data for snakes to create them
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="num">Number of snakes</param>
        protected void Snakes(ContentManager Content, int num)
        {
            string line;
            string[] data;

            line = inFile.ReadLine();

            for (int i = 0; i < num; ++i)
            {
                data = inFile.ReadLine().Split(' ');

                int startX = Convert.ToInt32(data[0]);
                int startY = Convert.ToInt32(data[1]);
                int endX = Convert.ToInt32(data[2]);
                int endY = Convert.ToInt32(data[3]);

                snakes[i] = new Snake(new Vector2(startX, startY), new Vector2(endX, endY));
                snakes[i].Load(Content);
            }
        }

        /// <summary>
        /// Read in data for blades to create them
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="num">Number of blades</param>
        protected void Blades(ContentManager Content, int num)
        {
            string line;
            string[] data;

            line = inFile.ReadLine();

            for (int i = 0; i < num; ++i)
            {
                data = inFile.ReadLine().Split(' ');

                int x = Convert.ToInt32(data[0]);
                int y = Convert.ToInt32(data[1]);
                double scale = Convert.ToDouble(data[2]);

                blades[i] = new Blade(new Vector2(x, y), scale);
                blades[i].Load(Content);
            }
        }
        
        /// <summary>
        /// Read in data for boxes to create them
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="num">Number of boxes</param>
        protected void Boxes(ContentManager Content, int num)
        {
            string line;
            string[] data;

            line = inFile.ReadLine();

            for (int i = 0; i < num; ++i)
            {
                data = inFile.ReadLine().Split(' ');

                int x = Convert.ToInt32(data[0]);
                int y = Convert.ToInt32(data[1]);

                switch (data[2])
                {
                    case "TRUE":
                        isObs = true;
                        break;
                    case "FALSE":
                        isObs = false;
                        break;
                }

                boxes[i] = new Box(new Vector2(x, y), isObs);
                boxes[i].Load(Content);
            }
        }
      
        /// <summary>
        /// Load world bounds
        /// </summary>
        public virtual void Load()
        {
            worldBounds = new Rectangle(0, 0, 2850, 780);
            bgBoundsLeft = new Rectangle(0, 0, 1300, 780);
            bgBoundsRight = new Rectangle(1300, 0, 1300, 780);
            bgBoundsEnd = new Rectangle(2600, 0, 1300, 780);
        }

        /// <summary>
        /// Load shop and fireball
        /// </summary>
        /// <param name="Content">Load content</param>
        /// <param name="time">Time potion image</param>
        /// <param name="health">Health potion image</param>
        /// <param name="shield">Shield potion image</param>
        public virtual void Load(ContentManager Content, Texture2D time, Texture2D health, Texture2D shield)
        {
            shop = new Shop(time, health, shield);
            shop.Load(Content);
            fireball.Load(Content);
        }

        /// <summary>
        /// Play background songs in worlds
        /// </summary>
        public virtual void PlaySong(bool isPaused)
        {
            if (MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(bgSong);
            }

            if (isPaused)
            {
                MediaPlayer.Pause();
            }
        }

        /// <summary>
        /// Allow for player movement (running, jumping, and ducking)
        /// </summary>
        /// <param name="kb">Keyboard state to determine buttons currently pressed</param>
        /// <param name="worldBounds">Rectangle for world bounds</param>
        public virtual void PlayerMovement(KeyboardState kb, Rectangle worldBounds)
        {
            player.PlayerMovement(kb, worldBounds);
        }

        /// <summary>
        /// Update player data
        /// </summary>
        /// <param name="gameTime">Time passing in game</param>
        /// <param name="platforms">Platforms of current world</param>
        /// <param name="boxes">Boxes in world</param>
        /// <param name="shopRec">Shop door rectangle</param>
        public virtual void UpdatePlayer(GameTime gameTime, Platform[] platforms, Obstacle[] boxes,
                                         Rectangle shopRec)
        {
            player.Update(gameTime, platforms, boxes, shopRec);
        }

        /// <summary>
        /// Determine when player is hit by enemies and obstacles
        /// </summary>
        /// <param name="snakes">Array of snakes</param>
        /// <param name="blades">Array of blades</param>
        /// <param name="boxes">Array of boxes</param>
        /// <param name="platforms">Array of platforms</param>
        /// <param name="attack">Attack sound effect</param>
        public virtual void Hit(Enemy[] snakes, Obstacle[] blades, Obstacle[] boxes, Platform[] platforms, SoundEffect attack)
        {
            player.SnakeCollision(snakes, platforms, attack);
            player.FireballCollision(fireball, attack);
            player.BladeCollision(blades, attack);
            player.BoxObsCollision(boxes, attack);
        }

        /// <summary>
        /// Reset time boost from time potion
        /// </summary>
        public void ResetTimeBoost()
        {
            player.timeBoost = 0;
        }

        /// <summary>
        /// Have camera follow player
        /// </summary>
        /// <param name="cam">Camera to follow player</param>
        public void Camera(Cam2D cam)
        {
            player.Camera(cam);
        }
   
        /// <summary>
        /// Update worlds (includes starting timers for collectibles to appear and disappear)
        /// </summary>
        /// <param name="gameTime">Time passing in game</param>
        /// <param name="playerRec">Player rectangle for player's current location</param>
        public virtual void UpdateWorld(GameTime gameTime, Rectangle playerRec, World curWorld)
        {
            //Move fireball
            fireball.Move(gameTime);

            //Loop through all collectibles
            for (int i = 0; i < collectibles.Length; ++i)
            {
                //Start timers
                collectibles[i].StartTimer(gameTime, playerRec, curWorld);

                //Update coin animation
                if (!(curWorld is Final))
                {
                    collectibles[i].UpdateCoin(gameTime);
                }
            }

            //Loop through snakes to update animation
            for (int i = 0; i < numSnakes; ++i)
            {
                snakes[i].Update(gameTime);
                snakes[i].Move(gameTime);
            }
        }

        /// <summary>
        /// Allow player to collect collectibles within their world
        /// </summary>
        /// <param name="gameTime">Time passing in game</param>
        /// <param name="collectibles">Collectibles in current world</param>
        /// <param name="curWorld">Current world player is in</param>
        /// <param name="collect">Collect sound effect</param>
        public virtual void Collect(GameTime gameTime, CollectibleManager[] collectibles, World curWorld, SoundEffect collect)
        {
            player.Collect(gameTime, collectibles, curWorld, collect);
        }

        /// <summary>
        /// Allow player to buy potions
        /// </summary>
        /// <param name="mouse">Mouse state</param>
        /// <param name="prevMouse">Previous mouse state</param>
        /// <param name="curWorld">Curernt world</param>
        /// <param name="cashRegister">Cash register sound effect</param>
        public virtual void Buy(MouseState mouse, MouseState prevMouse, World curWorld, SoundEffect cashRegister)
        {
            player.Buy(mouse, prevMouse, shop, curWorld, cashRegister);
        }

        /// <summary>
        /// Allow player to use potions
        /// </summary>
        /// <param name="rec">Potions rectangle</param>
        /// <param name="type">Name of potion</param>
        /// <param name="mouse">Mouse state</param>
        /// <param name="prevMouse">Previous mouse state</param>
        /// <param name="potions">Potions sound effect</param>
        public virtual void Use(Rectangle rec, string type, MouseState mouse, MouseState prevMouse, SoundEffect potions)
        {
            player.Use(rec, type, mouse, prevMouse, potions);
        }
        
        /// <summary>
        /// Allow player to use shield potion
        /// </summary>
        /// <param name="gameTime">Time passing in game</param>
        public virtual void UseShield(GameTime gameTime)
        {
            player.UseShield(gameTime);
        }

        /// <summary>
        /// Start Final world timer
        /// </summary>
        /// <param name="gameTime">Time passing in game</param>
        public virtual void FinalTimer(GameTime gameTime)
        {

        }
        
        /// <summary>
        /// Draw world on screen
        /// </summary>
        /// <param name="graphics">Graphics</param>
        /// <param name="spriteBatch">Draw images</param>
        public virtual void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, World curWorld)
        {
            //Draw Background
            spriteBatch.Draw(bg, bgBoundsLeft, Color.White);
            spriteBatch.Draw(bg, bgBoundsRight, Color.White);
            spriteBatch.Draw(bg, bgBoundsEnd, Color.White);

            //Display platforms
            DisplayPlatforms(graphics, spriteBatch);

            //Display door
            spriteBatch.Draw(worldDoor, worldDoorRec, Color.White);

            //Display shop door
            spriteBatch.Draw(shopDoor, shopDoorRec, Color.White);

            //Display collectibles
            for (int i = 0; i < collectibles.Length; ++i)
            {
                collectibles[i].Draw(spriteBatch, curWorld);
            }

            //Display snakes
            for (int i = 0; i < numSnakes; ++i)
            {
                snakes[i].Draw(spriteBatch);
            }

            //Display blades
            for (int i = 0; i < numBlades; ++i)
            {
                blades[i].Draw(spriteBatch);
            }

            //Display boxes
            for (int i = 0; i < numBoxes; ++i)
            {
                boxes[i].Draw(spriteBatch);
            }

            //Display fireball
            fireball.Draw(spriteBatch);
        }
    
        /// <summary>
        /// Display platforms of the world
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="spriteBatch"></param>
        protected void DisplayPlatforms(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            //Loop through all platforms
            for (int i = 0; i < platforms.Length; ++i)
            {
                //Display platforms
                if (platforms[i] != null)
                {
                    platforms[i].Draw(graphics, spriteBatch);
                }
            }
        }
    }
}
