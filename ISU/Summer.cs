//Author:           Amy Wang
//File Name:        Summer.cs
//Project Name:     ISU
//Creation Date:    December 27, 2018
//Modified Date:    January 20, 2019
//Description:      Display Summer world by loading Summer images and tiles

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
using System.IO;

namespace ISU
{
    class Summer : World
    {
        public Summer(Vector2 playerLoc) : base(playerLoc)
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
        /// Retrieve Summer world's initial player location
        /// </summary>
        /// <returns>Initial player location</returns>
        public override Vector2 GetInitialLoc()
        {
            return playerLoc;
        }

        /// <summary>
        /// Retrieve collectibles of Summer world
        /// </summary>
        /// <returns>Collectibles of Summer world</returns>
        public override CollectibleManager[] GetCollectibles()
        {
            return collectibles;
        }

        /// <summary>
        /// Retrieve platforms of Summer world
        /// </summary>
        /// <returns>Platforms of Summer world</returns>
        public override Platform[] GetPlatforms()
        {
            return platforms;
        }

        /// <summary>
        /// Retrieve snakes of Summer world
        /// </summary>
        /// <returns>Snakes array of Summer world</returns>
        public override Enemy[] GetSnakes()
        {
            return snakes;
        }

        /// <summary>
        /// Retrieve blades of Summer world
        /// </summary>
        /// <returns>Blades array of Summer world</returns>
        public override Obstacle[] GetBlades()
        {
            return blades;
        }

        /// <summary>
        /// Retrieve boxes of Summer world
        /// </summary>
        /// <returns>Boxes array of Summer world</returns>
        public override Obstacle[] GetBoxes()
        {
            return boxes;
        }

        /// <summary>
        /// Retrieve shop door of Summer world
        /// </summary>
        /// <returns>Shop door rectangle</returns>
        public override Rectangle GetShopRec()
        {
            return shopDoorRec;
        }

        /// <summary>
        /// Retrieve Summer world door
        /// </summary>
        /// <returns>World door rectangle</returns>
        public override Rectangle GetDoor()
        {
            return worldDoorRec;
        }

        /// <summary>
        /// Load images and other content of Summer world
        /// </summary>
        /// <param name="Content">Load content</param>
        public override void Load(ContentManager Content, Texture2D time, Texture2D health, Texture2D shield)
        {
            //Load content from world class
            base.Load(Content, time, health, shield);

            //Load song
            bgSong = Content.Load<Song>("Songs/SummerSong");

            //Load Summer background
            bg = Content.Load<Texture2D>("Backgrounds/SummerBg");
      
            //Load platform tiles
            baseTile = Content.Load<Texture2D>("SummerTiles/SummerBaseTile");
            colDown = Content.Load<Texture2D>("SummerTiles/SummerColDown");
            leftSlant = Content.Load<Texture2D>("SummerTiles/SummerLeftSlant");
            rightSlant = Content.Load<Texture2D>("SummerTiles/SummerRightSlant");
            dirt = Content.Load<Texture2D>("SummerTiles/SDirt");
      
            //Load numbers of elements
            numPlatforms = 55;
            numSnakes = 5;
            numBlades = 5;
            numBoxes = 18;
        
            //Read in file
            FilePath("Summer");
            Platforms(Content, numPlatforms);
            CollectLocs(Content, "Chalice");
            Snakes(Content, numSnakes);
            Blades(Content, numBlades);
            Boxes(Content, numBoxes);
        
            //Load door
            worldDoor = Content.Load<Texture2D>("Sprites/SummerDoor");
            worldDoorRec = new Rectangle(2545, 285, (int)(worldDoor.Width * 0.1), (int)(worldDoor.Height * 0.1));
        
            //Load shop door
            shopDoor = Content.Load<Texture2D>("Sprites/SummerShopDoor");
            shopDoorRec = new Rectangle(1730, 525, (int)(shopDoor.Width * 0.1), (int)(shopDoor.Height * 0.1));
        }
    }
}
