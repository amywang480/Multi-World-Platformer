//Author:           Amy Wang
//File Name:        Platform.cs
//Project Name:     ISU
//Creation Date:    December 19, 2018
//Modified Date:    January 20, 2019
//Description:      Create platforms in each world

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

namespace ISU
{
    class Platform
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
   
        //Store tile image depending on type
        Texture2D tileType;

        //Store 2D array of tiles
        Tile[,] tiles;

        //Store platform location and dimensions
        int x;
        int y;
        int width;
        int bottom;
    
        public Platform(int x, int y, int tilesW, int tilesH, Texture2D tileType, double scale)
        {
            //Store type of tile
            this.tileType = tileType;

            //Store X and Y coordinate
            this.x = x;
            this.y = y;

            //Calculate width of platform
            width = tilesW * (int)(tileType.Width * scale);

            //Calculate bottom value of platform
            bottom = y + (int)(tileType.Width * scale) * tilesH;

            //Create 2D array of tiles in platform
            tiles = new Tile[tilesH, tilesW];
            
            //Calculate x and y values for each tile in platform
            for (int i = 0; i < tiles.GetLength(0); ++i)
            {
                for (int j = 0; j < tiles.GetLength(1); ++j)
                {
                    int newX = x + j * (int)(tileType.Width * scale);
                    int newY = y + i * (int)(tileType.Width * scale);

                    tiles[i, j] = new Tile(newX, newY, tileType, scale);
                }
            }
        }

        /// <summary>
        /// Retrieve platform X value
        /// </summary>
        /// <returns>X value</returns>
        public int GetX()
        {
            return x;
        }

        /// <summary>
        /// Retrieve platform Y value
        /// </summary>
        /// <returns>Y value</returns>
        public int GetY()
        {
            return y;
        }
        
        /// <summary>
        /// Retrieve platform width
        /// </summary>
        /// <returns>Width of platform</returns>
        public int GetWidth()
        {
            return width;
        }

        /// <summary>
        /// Retrieve bottom value of platform
        /// </summary>
        /// <returns>Platform bottom value (y value + height)</returns>
        public int GetBottom()
        {
            return bottom;
        }

        /// <summary>
        /// Retrieve tiles of platform
        /// </summary>
        /// <returns>2D array of tiles forming the platform</returns>
        public Tile[,] GetTiles()
        {
            return tiles;
        }
        
        /// <summary>
        /// Draw platform on screen
        /// </summary>
        /// <param name="graphics">Graphics</param>
        /// <param name="spriteBatch">Draw images</param>
        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;

            //Draw platform by drawing tiles
            for (int i = 0; i < tiles.GetLength(0); ++i)
            {
                for (int j = 0; j < tiles.GetLength(1); ++j)
                {
                    spriteBatch.Draw(tileType, tiles[i, j].GetTileRec(), Color.White);
                }
            }
        }

        /// <summary>
        /// Detect rectangle collision with platform
        /// </summary>
        /// <param name="playerRec">Player rectangle</param>
        /// <returns>Boolean to determine if collision occurred</returns>
        public bool RecCollision(Rectangle playerRec)
        {
            //Determine if a collision occurred
            bool collision = false;

            //Loop through tiles of platform
            for (int i = 0; i < tiles.GetLength(0); ++i)
            {
                for (int j = 0; j < tiles.GetLength(1); ++j)
                {
                    //Test for all the impossible cases of collision
                    if (playerRec.Left > tiles[i, j].GetTileRec().Right || playerRec.Top > tiles[i, j].GetTileRec().Bottom 
                        || playerRec.Right < tiles[i, j].GetTileRec().Left || playerRec.Bottom < tiles[i, j].GetTileRec().Top)
                    {
                        collision = false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            //Return collision
            return collision;
        }
    }
}
