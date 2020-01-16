//Author:           Amy Wang
//File Name:        Spring.cs
//Project Name:     ISU
//Creation Date:    December 27, 2018
//Modified Date:    January 20, 2019
//Description:      Display Spring world by loading Spring images and tiles

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
    class Spring : World
    {
        public Spring(Vector2 playerLoc) : base(playerLoc)
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
        /// Retrieve Spring world's initial player location
        /// </summary>
        /// <returns>Initial player location</returns>
        public override Vector2 GetInitialLoc()
        {
            return playerLoc;
        }

        /// <summary>
        /// Retrieve collectibles of spring world
        /// </summary>
        /// <returns>Collectibles of spring world</returns>
        public override CollectibleManager[] GetCollectibles()
        {
            return collectibles;
        }

        /// <summary>
        /// Retrieve platforms of spring world
        /// </summary>
        /// <returns>Platforms of spring world</returns>
        public override Platform[] GetPlatforms()
        {
            return platforms;
        }

        /// <summary>
        /// Retrieve snakes of Spring world
        /// </summary>
        /// <returns>Snakes array of Spring world</returns>
        public override Enemy[] GetSnakes()
        {
            return snakes;
        }

        /// <summary>
        /// Retrieve blades of Spring world
        /// </summary>
        /// <returns>Blades array of Spring world</returns>
        public override Obstacle[] GetBlades()
        {
            return blades;
        }

        /// <summary>
        /// Retrieve boxes of Spring world
        /// </summary>
        /// <returns>Boxes array of Spring world</returns>
        public override Obstacle[] GetBoxes()
        {
            return boxes;
        }

        /// <summary>
        /// Retrieve shop door of Spring world
        /// </summary>
        /// <returns>Shop door rectangle</returns>
        public override Rectangle GetShopRec()
        {
            return shopDoorRec;
        }

        /// <summary>
        /// Retrieve Spring world door
        /// </summary>
        /// <returns>World door rectangle</returns>
        public override Rectangle GetDoor()
        {
            return worldDoorRec;
        }
    
        /// <summary>
        /// Load images and other content of spring world
        /// </summary>
        /// <param name="Content">Load content</param>
        public override void Load(ContentManager Content, Texture2D time, Texture2D health, Texture2D shield)
        {
            //Load content from base class
            base.Load(Content, time, health, shield);

            //Load song
            bgSong = Content.Load<Song>("Songs/SpringSong");

            //Load Spring background
            bg = Content.Load<Texture2D>("Backgrounds/SpringBg");
        
            //Load platform grass
            baseTile = Content.Load<Texture2D>("SpringTiles/GrassMid");
            col = Content.Load<Texture2D>("SpringTiles/SpringColumn");
            top = Content.Load<Texture2D>("SpringTiles/GrassTop");
            leftSlant = Content.Load<Texture2D>("SpringTiles/GrassHillLeft");
            leftSlantMid = Content.Load<Texture2D>("SpringTiles/GrassHillLeftMid");
            inSlant = Content.Load<Texture2D>("SpringTiles/GrassSlant");
            dirt = Content.Load<Texture2D>("SpringTiles/Dirt");
                       
            //Load numbers of elements
            numPlatforms = 57;
            numSnakes = 5;
            numBlades = 4;
            numBoxes = 19;
            
            //Read in file
            FilePath("Spring");
            Platforms(Content, numPlatforms);
            CollectLocs(Content, "Mirror");
            Snakes(Content, numSnakes);
            Blades(Content, numBlades);
            Boxes(Content, numBoxes);

            //Load door
            worldDoor = Content.Load<Texture2D>("Sprites/SpringDoor");
            worldDoorRec = new Rectangle(2679, 167, (int)(worldDoor.Width * 0.1), (int)(worldDoor.Height * 0.1));

            //Load shop door
            shopDoor = Content.Load<Texture2D>("Sprites/SpringShopDoor");
            shopDoorRec = new Rectangle(1250, 47, (int)(shopDoor.Width * 0.1), (int)(shopDoor.Height * 0.1));
        }
    }
}
