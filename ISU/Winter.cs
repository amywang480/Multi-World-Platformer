//Author:           Amy Wang
//File Name:        Winter.cs
//Project Name:     ISU
//Creation Date:    December 27, 2018
//Modified Date:    January 20, 2019
//Description:      Display Winter world by loading Winter images and tiles

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
    class Winter : World
    {
        public Winter(Vector2 playerLoc) : base(playerLoc)
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
        /// Retrieve Winter world's initial player location
        /// </summary>
        /// <returns>Initial player location</returns>
        public override Vector2 GetInitialLoc()
        {
            return playerLoc;
        }

        /// <summary>
        /// Retrieve collectibles of Winter world
        /// </summary>
        /// <returns>Collectibles of Winter world</returns>
        public override CollectibleManager[] GetCollectibles()
        {
            return collectibles;
        }

        /// <summary>
        /// Retrieve platforms of Winter world
        /// </summary>
        /// <returns>Platforms of Winter world</returns>
        public override Platform[] GetPlatforms()
        {
            return platforms;
        }

        /// <summary>
        /// Retrieve snakes of Winter world
        /// </summary>
        /// <returns>Snakes array of Winter world</returns>
        public override Enemy[] GetSnakes()
        {
            return snakes;
        }

        /// <summary>
        /// Retrieve blades of Winter world
        /// </summary>
        /// <returns>Blades array of Winter world</returns>
        public override Obstacle[] GetBlades()
        {
            return blades;
        }

        /// <summary>
        /// Retrieve boxes of Winter world
        /// </summary>
        /// <returns>Boxes array of Winter world</returns>
        public override Obstacle[] GetBoxes()
        {
            return boxes;
        }

        /// <summary>
        /// Retrieve shop door of Winter world
        /// </summary>
        /// <returns>Shop door rectangle</returns>
        public override Rectangle GetShopRec()
        {
            return shopDoorRec;
        }

        /// <summary>
        /// Retrieve Winter world door
        /// </summary>
        /// <returns>World door rectangle</returns>
        public override Rectangle GetDoor()
        {
            return worldDoorRec;
        }
   
        /// <summary>
        /// Load images and other content of Winter world
        /// </summary>
        /// <param name="Content">Load Content</param>
        public override void Load(ContentManager Content, Texture2D time, Texture2D health, Texture2D shield)
        {
            //Load content from world class
            base.Load(Content, time, health, shield);

            //Load song
            bgSong = Content.Load<Song>("Songs/WinterSong");

            //Load Winter background
            bg = Content.Load<Texture2D>("Backgrounds/WinterBg");
      
            //Load platform tiles
            baseTile = Content.Load<Texture2D>("WinterTiles/WBaseTile");
            colDown = Content.Load<Texture2D>("WinterTiles/WColDown");
            leftSlant = Content.Load<Texture2D>("WinterTiles/WinterLeftSlant");
            rightSlant = Content.Load<Texture2D>("WinterTiles/WinterRightSlant");
            dirt = Content.Load<Texture2D>("WinterTiles/WIce");
       
            //Load numbers of elements
            numPlatforms = 62;
            numSnakes = 5;
            numBlades = 5;
            numBoxes = 16;
  
            //Read in file
            FilePath("Winter");
            Platforms(Content, numPlatforms);
            CollectLocs(Content, "Crown");
            Snakes(Content, numSnakes);
            Blades(Content, numBlades);
            Boxes(Content, numBoxes);
        
            //Load door
            worldDoor = Content.Load<Texture2D>("Sprites/WinterDoor");
            worldDoorRec = new Rectangle(2750, 346, (int)(worldDoor.Width * 0.1), (int)(worldDoor.Height * 0.1));
        
            //Load shop door
            shopDoor = Content.Load<Texture2D>("Sprites/WinterShopDoor");
            shopDoorRec = new Rectangle(1110, 105, (int)(shopDoor.Width * 0.1), (int)(shopDoor.Height * 0.1));
        }
    }
}
