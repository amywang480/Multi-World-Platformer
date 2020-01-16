//Author:           Amy Wang
//File Name:        Final.cs
//Project Name:     ISU
//Creation Date:    December 27, 2018
//Modified Date:    January 20, 2019
//Description:      Display Final world by loading Final world images and tiles

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Animation2D;
using Microsoft.Xna.Framework.Content;

namespace ISU
{
    class Final : World
    {
        //Store time left upon entering Final world
        int timeLeft = 15000;
        
        public Final(Vector2 playerLoc) : base(playerLoc)
        {
            //Store player location
            this.playerLoc = playerLoc;
        }

        /// <summary>
        /// Retrieve background song
        /// </summary>
        /// <returns>Background song</returns>
        public override Song GetSong()
        {
            return bgSong;
        }

        /// <summary>
        /// Retrieve player location
        /// </summary>
        /// <returns>Player location</returns>
        public override Vector2 GetInitialLoc()
        {
            return playerLoc;
        }

        /// <summary>
        /// Retrieve time left in Final world
        /// </summary>
        /// <returns>Time left</returns>
        public override int GetTimeLeft()
        {
            return timeLeft;
        }

        /// <summary>
        /// Retrieve world bounds
        /// </summary>
        /// <returns>World bounds rectangle</returns>
        public override Rectangle GetWorldBounds()
        {
            return worldBounds;
        }

        /// <summary>
        /// Retrieve collectibles of Final world
        /// </summary>
        /// <returns>Collectibles array of Final world</returns>
        public override CollectibleManager[] GetCollectibles()
        {
            return collectibles;
        }

        /// <summary>
        /// Retrieve platforms of Final world
        /// </summary>
        /// <returns>Platforms array of Final world</returns>
        public override Platform[] GetPlatforms()
        {
            return platforms;
        }

        /// <summary>
        /// Retrieve snakes of Final world
        /// </summary>
        /// <returns>Snakes array of Final world</returns>
        public override Enemy[] GetSnakes()
        {
            return snakes;
        }

        /// <summary>
        /// Retrieve blades of Final world
        /// </summary>
        /// <returns>Blades array of Final world</returns>
        public override Obstacle[] GetBlades()
        {
            return blades;
        }

        /// <summary>
        /// Retrieve boxes of Final world
        /// </summary>
        /// <returns>Boxes array of Final world</returns>
        public override Obstacle[] GetBoxes()
        {
            return boxes;
        }

        /// <summary>
        /// Retrieve shop door of Final world
        /// </summary>
        /// <returns>Shop door rectangle</returns>
        public override Rectangle GetShopRec()
        {
            return shopDoorRec;
        }

        /// <summary>
        /// Retrieve Final world door
        /// </summary>
        /// <returns>World door rectangle</returns>
        public override Rectangle GetDoor()
        {
            return worldDoorRec;
        }
   
        /// <summary>
        /// Load Final world's world bounds
        /// </summary>
        public override void Load()
        {
            worldBounds = new Rectangle(0, 0, 1300, 780);
            bgBoundsLeft = new Rectangle(0, 0, 1300, 780);
            bgBoundsRight = new Rectangle(0, 0, 0, 0);
            bgBoundsEnd = new Rectangle(0, 0, 0, 0);
        }
        
        /// <summary>
        /// Load images and other content of Final world
        /// </summary>
        /// <param name="Content"></param>
        public override void Load(ContentManager Content, Texture2D time, Texture2D health, Texture2D shield)
        {
            //Load content from world class
            base.Load(Content, time, health, shield);

            //Load song
            bgSong = Content.Load<Song>("Songs/FinalSong");

            //Load Final world background
            bg = Content.Load<Texture2D>("Backgrounds/FinalBg");

            //Load platform tiles
            baseTile = Content.Load<Texture2D>("Sprites/FinalBaseTile");

            //Load numbers of elements
            numPlatforms = 18;
            numSnakes = 3;
            numBlades = 2;
            numBoxes = 10;
       
            //Read in file
            FilePath("Final");
            Platforms(Content, numPlatforms);
            CollectLocs(Content);
            Snakes(Content, numSnakes);
            Blades(Content, numBlades);
            Boxes(Content, numBoxes);
        
            //Load door
            worldDoor = Content.Load<Texture2D>("Sprites/SpringDoor");
            worldDoorRec = new Rectangle(-2679, 167, (int)(worldDoor.Width * 0.1), (int)(worldDoor.Height * 0.1));

            //Load shop door
            shopDoor = Content.Load<Texture2D>("Sprites/SpringShopDoor");
            shopDoorRec = new Rectangle(-1250, 47, (int)(shopDoor.Width * 0.1), (int)(shopDoor.Height * 0.1));
        }

        /// <summary>
        /// Start final world timer
        /// </summary>
        /// <param name="gameTime">Time passing in game</param>
        public override void FinalTimer(GameTime gameTime)
        {
            if (isStarting)
            {
                timeLeft -= gameTime.ElapsedGameTime.Milliseconds;
            }
        }
    }
}
