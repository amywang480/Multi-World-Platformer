//Author:           Amy Wang
//File Name:        TreasureChest.cs
//Project Name:     ISU
//Creation Date:    December 27, 2018
//Modified Date:    January 20, 2019
//Description:      Display Treasure Chest world by loading Treasure Chest images and tiles

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
    class TreasureChest : World
    {
        //Store chest image and animation
        Texture2D chestImg;
        Animation chest;

        //Store font for chest message
        SpriteFont chestFont;

        public TreasureChest(Vector2 playerLoc) : base(playerLoc)
        {
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
        /// Retrieve Chest world's initial player location
        /// </summary>
        /// <returns>Initial player location</returns>
        public override Vector2 GetInitialLoc()
        {
            return playerLoc;
        }

        /// <summary>
        /// Retrieve world bounds
        /// </summary>
        /// <returns>Rectangle of world bounds</returns>
        public override Rectangle GetWorldBounds()
        {
            return worldBounds;
        }

        /// <summary>
        /// Retrieve platforms of chest world
        /// </summary>
        /// <returns>Platforms of chest world</returns>
        public override Platform[] GetPlatforms()
        {
            return platforms;
        }

        /// <summary>
        /// Retrieve collectibles of chest world
        /// </summary>
        /// <returns>Collectibles of chest world</returns>
        public override CollectibleManager[] GetCollectibles()
        {
            return collectibles;
        }

        /// <summary>
        /// Retrieve snakes of chest world
        /// </summary>
        /// <returns>Snakes array of chest world</returns>
        public override Enemy[] GetSnakes()
        {
            return snakes;
        }

        /// <summary>
        /// Retrieve blades of chest world
        /// </summary>
        /// <returns>Blades array of chest world</returns>
        public override Obstacle[] GetBlades()
        {
            return blades;
        }

        /// <summary>
        /// Retrieve boxes of chest world
        /// </summary>
        /// <returns>Boxes array of chest world</returns>
        public override Obstacle[] GetBoxes()
        {
            return boxes;
        }

        /// <summary>
        /// Retrieve shop door of chest world
        /// </summary>
        /// <returns>Shop door rectangle</returns>
        public override Rectangle GetShopRec()
        {
            return shopDoorRec;
        }

        /// <summary>
        /// Retrieve Autumn world door
        /// </summary>
        /// <returns>World door rectangle</returns>
        public override Rectangle GetDoor()
        {
            return worldDoorRec;
        }

        /// <summary>
        /// Retrieve current frame of chest animation
        /// </summary>
        /// <returns>Current frame number</returns>
        public override int GetChestFrame()
        {
            return chest.curFrame;
        }

        /// <summary>
        /// Set when chest is animating
        /// </summary>
        /// <param name="isAnimating">Boolean to determine when chest animates</param>
        public override void SetIsAnimating(bool isAnimating)
        {
            chest.isAnimating = isAnimating;
        }

        /// <summary>
        /// Load world bounds
        /// </summary>
        public override void Load()
        {
            worldBounds = new Rectangle(0, 0, 1300, 780);
            bgBoundsLeft = new Rectangle(0, 0, 1300, 780);
        }

        /// <summary>
        /// Load images and other content of chest world
        /// </summary>
        /// <param name="Content">Load content</param>
        public override void Load(ContentManager Content, Texture2D time, Texture2D health, Texture2D shield)
        {
            //Load content from world class
            base.Load(Content, time, health, shield);

            //Load song
            bgSong = Content.Load<Song>("Songs/TreasureChestSong");

            //Load chest world background
            bg = Content.Load<Texture2D>("Backgrounds/TreasureChestBg");
       
            //Load platform tiles
            baseTile = Content.Load<Texture2D>("TreasureChestTiles/TBaseTile");
            dirt = Content.Load<Texture2D>("TreasureChestTiles/TDirt");
            colDown = Content.Load<Texture2D>("TreasureChestTiles/TColDown");

            //Load numbers of elements
            numPlatforms = 27;
            numSnakes = 0;
            numBlades = 0;
            numBoxes = 0;

            //Read in file
            FilePath("TreasureChest");
            Platforms(Content, numPlatforms);
       
            //Load chest image and animation
            chestImg = Content.Load<Texture2D>("Sprites/Chest");
            chest = new Animation(chestImg, 6, 1, 6, 0, 4, Animation.ANIMATE_ONCE, 15, new Vector2(980, 190), 0.45f, false);

            //Load chest font
            chestFont = Content.Load<SpriteFont>("Fonts/Chest");
        }
    
        /// <summary>
        /// Update chest animation
        /// </summary>
        /// <param name="gameTime">Time passing in game</param>
        /// <param name="playerRec">Player rectangle</param>
        /// <param name="curWorld">World player is currently in</param>
        public override void UpdateWorld(GameTime gameTime, Rectangle playerRec, World curWorld)
        {
            chest.Update(gameTime);
        }

        /// <summary>
        /// Draw world on screen
        /// </summary>
        /// <param name="graphics">Graphics</param>
        /// <param name="spriteBatch">Draw images</param>
        public override void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, World curWorld)
        {
            //Display Background
            spriteBatch.Draw(bg, bgBoundsLeft, Color.White);

            //Display platforms
            DisplayPlatforms(graphics, spriteBatch);

            //Display treasure chest
            chest.Draw(spriteBatch, Color.White, SpriteEffects.None);
        
            //Display message
            spriteBatch.DrawString(chestFont, "Stand here to \n" + "open chest", new Vector2(810, 370), Color.Black);
        }
    }
}
