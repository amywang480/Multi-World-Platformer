//Author:           Amy Wang
//File Name:        CollectibleManager.cs
//Project Name:     ISU
//Creation Date:    December 28, 2018
//Modified Date:    January 20, 2019
/*Description:      Manage each collectible location which has three collectibles that randomly appear 
                    and disappear using timers*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISU
{
    class CollectibleManager
    {
        //Store location of collectible
        Vector2 loc;

        //Generate random numbers
        static Random rng = new Random();
    
        //Store data for timers
        int time = rng.Next(3000, 10001);
        int count;
        int randomCollect;
        
        //Store collectibles of current world
        Collectible coin;
        Collectible star;
        Collectible collectible;

        //Store all collectibles for final world
        Collectible[] allCollects = new Collectible[5];
        
        public CollectibleManager(Vector2 loc, ContentManager Content)
        {
            //Store location
            this.loc = loc;
            
            //Store all collectibles
            allCollects[0] = new Collectible("Star");
            allCollects[1] = new Collectible("Mirror");
            allCollects[2] = new Collectible("Chalice");
            allCollects[3] = new Collectible("Diamond");
            allCollects[4] = new Collectible("Crown");
            
            //Load all collectibles
            allCollects[0].Load(Content, loc);
            allCollects[1].Load(Content, loc);
            allCollects[2].Load(Content, loc);
            allCollects[3].Load(Content, loc);
            allCollects[4].Load(Content, loc);
        }

        public CollectibleManager(Vector2 loc, string name, ContentManager Content)
        {
            //Store location
            this.loc = loc;
            
            //Create collectibles
            coin = new Collectible("Coin");
            star = new Collectible("Star");
            collectible = new Collectible(name);

            //Load collectibles
            coin.Load(Content, loc);
            coin.LoadCoin(Content, loc);
            star.Load(Content, loc);
            collectible.Load(Content, loc);
        }
        
        /// <summary>
        /// Retrieve coin collectible
        /// </summary>
        /// <returns>Coin collectible</returns>
        public Collectible GetCoin()
        {
            return coin;
        }

        /// <summary>
        /// Retrieve star collectible
        /// </summary>
        /// <returns>Star collectible</returns>
        public Collectible GetStar()
        {
            return star;
        }

        /// <summary>
        /// Retrieve collectible unique to the current world
        /// </summary>
        /// <returns>Collectible only found in current world</returns>
        public Collectible GetCollect()
        {
            return collectible;
        }

        /// <summary>
        /// Retrieve all collectibles for Final world
        /// </summary>
        /// <returns>Array of all collectibles in Final world</returns>
        public Collectible[] GetAllCollect()
        {
            return allCollects;
        }

        /// <summary>
        /// Update coin animation
        /// </summary>
        /// <param name="gameTime">Time passing in game</param>
        public void UpdateCoin(GameTime gameTime)
        {
            coin.Update(gameTime);
        }

        /// <summary>
        /// Start timers for collectibles to appear and disappear
        /// </summary>
        /// <param name="gameTime">Time passing in game</param>
        /// <param name="playerRec">Player rectangle for player's current location</param>
        public void StartTimer(GameTime gameTime, Rectangle playerRec, World curWorld)
        {
            //Count downwards from randomly generated number
            time -= gameTime.ElapsedGameTime.Milliseconds;

            //When timer finishes:
            if (time <= 0)
            {
                //Increment count 
                count++;

                //Use count to determine if timer finished for first or second time
                if (count == 1)
                {
                    //If it finished for the first time, randomly generate a number
                    randomCollect = rng.Next(1, 6);
                
                    //Determine random collectible to draw
                    if (curWorld is Final)
                    {
                        //For final world, generate all collectibles with even chances
                        FinalRandomItem();
                    }
                    else
                    {
                        //For all other worlds, generate world collectibles with fewer chances
                        WorldRandomItem();
                    }
                }
                else if (count == 2)
                {
                    //Check if current world is Final world
                    if (curWorld is Final)
                    {
                        //Remove all collectibles from screen
                        for (int i = 0; i < allCollects.Length; ++i)
                        {
                            allCollects[i].isDrawn = false;
                        }
                    }
                    else
                    {
                        //If timer finished for second time, remove collectibles from screen
                        coin.isDrawn = false;
                        star.isDrawn = false;
                        collectible.isDrawn = false;
                    }

                    //Reset count
                    count = 0;
                }

                //Randomly generate new time for timer
                time = rng.Next(3000, 10001);
            }
        }

        /// <summary>
        /// Based on random number, determine collectible to draw on screen while giving 
        /// world collectible with smaller chance of being displayed
        /// </summary>
        public void WorldRandomItem()
        {
            switch (randomCollect)
            {
                case 1:
                    collectible.isDrawn = true;
                    break;
                case 2:
                    star.isDrawn = true;
                    break;
                case 3:
                    star.isDrawn = true;
                    break;
                case 4:
                    coin.isDrawn = true;
                    break;
                case 5:
                    coin.isDrawn = true;
                    break;
            }
        }

        /// <summary>
        /// Based on random number, determine collectible to draw on screen while giving
        /// all collectibles equal chances of being displayed
        /// </summary>
        public void FinalRandomItem()
        {
            switch (randomCollect)
            {
                case 1:
                    allCollects[0].isDrawn = true;
                    break;
                case 2:
                    allCollects[1].isDrawn = true;
                    break;
                case 3:
                    allCollects[2].isDrawn = true;
                    break;
                case 4:
                    allCollects[3].isDrawn = true;
                    break;
                case 5:
                    allCollects[4].isDrawn = true;
                    break;
            }
        }

        /// <summary>
        /// Draw collectibles on screen
        /// </summary>
        /// <param name="spriteBatch">Draw images</param>
        public void Draw(SpriteBatch spriteBatch, World curWorld)
        {
            //Check if in Final world
            if (curWorld is Final)
            {
                //Draw all collectibles
                allCollects[0].Draw(spriteBatch);
                allCollects[1].Draw(spriteBatch);
                allCollects[2].Draw(spriteBatch);
                allCollects[3].Draw(spriteBatch);
                allCollects[4].Draw(spriteBatch);
            }
            else
            {
                //Draw only coin, star, and world collectible
                coin.DrawCoin(spriteBatch);
                star.Draw(spriteBatch);
                collectible.Draw(spriteBatch);
            }
        }
    }
}
