//File Name:        Autumn.cs
//Creation Date:    December 27, 2018
//Modified Date:    January 20, 2019
//Description:      Display Autumn world by loading Autumn images and tiles

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
    class Autumn : World 
    {
        public Autumn(Vector2 playerLoc) : base(playerLoc)
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
        /// Retrieve Autumn world's initial player location
        /// </summary>
        /// <returns>Initial player location</returns>
        public override Vector2 GetInitialLoc()
        {
            return playerLoc;
        }

        /// <summary>
        /// Retrieve collectibles of Autumn world
        /// </summary>
        /// <returns>Collectibles array of Autumn world</returns>
        public override CollectibleManager[] GetCollectibles()
        {
            return collectibles;
        }

        /// <summary>
        /// Retrieve platforms of Autumn world
        /// </summary>
        /// <returns>Platforms array of Autumn world</returns>
        public override Platform[] GetPlatforms()
        {
            return platforms;
        }

        /// <summary>
        /// Retrieve snakes of Autumn world
        /// </summary>
        /// <returns>Snakes array of Autumn world</returns>
        public override Enemy[] GetSnakes()
        {
            return snakes;
        }

        /// <summary>
        /// Retrieve blades of Autumn world
        /// </summary>
        /// <returns>Blades array of Autumn world</returns>
        public override Obstacle[] GetBlades()
        {
            return blades;
        }

        /// <summary>
        /// Retrieve boxes of Autumn world
        /// </summary>
        /// <returns>Boxes array of Autumn world</returns>
        public override Obstacle[] GetBoxes()
        {
            return boxes;
        }
    
        /// <summary>
        /// Retrieve shop door of Autumn world
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
        /// Load images and other content of Autumn world
        /// </summary>
        /// <param name="Content">Load content</param>
        public override void Load(ContentManager Content, Texture2D time, Texture2D health, Texture2D shield)
        {
            //Load content from world class
            base.Load(Content, time, health, shield);

            //Load song
            bgSong = Content.Load<Song>("Songs/AutumnSong");

            //Load Autumn background
            bg = Content.Load<Texture2D>("Backgrounds/AutumnBg");
     
            //Load platform tiles
            baseTile = Content.Load<Texture2D>("AutumnTiles/ABaseTile");
            colDown = Content.Load<Texture2D>("AutumnTiles/AColDown");
            leftSlant = Content.Load<Texture2D>("AutumnTiles/ALeft");
            rightSlant = Content.Load<Texture2D>("AutumnTiles/ARight");
            dirt = Content.Load<Texture2D>("AutumnTiles/ADirt");
            leftSlantMid = Content.Load<Texture2D>("AutumnTiles/ALeftSlantMid");
            rightSlantMid = Content.Load<Texture2D>("AutumnTiles/ARightSlantMid");
    
            //Load numbers of elements
            numPlatforms = 63;
            numSnakes = 4;
            numBlades = 4;
            numBoxes = 17;
 
            //Read in file
            FilePath("Autumn");
            Platforms(Content, numPlatforms);
            CollectLocs(Content, "Diamond");
            Snakes(Content, numSnakes);
            Blades(Content, numBlades);
            Boxes(Content, numBoxes);

            //Load door
            worldDoor = Content.Load<Texture2D>("Sprites/AutumnDoor");
            worldDoorRec = new Rectangle(2690, 197, (int)(worldDoor.Width * 0.1), (int)(worldDoor.Height * 0.1));
        
            //Load shop door
            shopDoor = Content.Load<Texture2D>("Sprites/AutumnShopDoor");
            shopDoorRec = new Rectangle(990, 527, (int)(shopDoor.Width * 0.1), (int)(shopDoor.Height * 0.1));
        }
    }
}
