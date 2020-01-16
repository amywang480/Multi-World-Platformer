//Author:           Amy Wang
//File Name:        Enemy.cs
//Project Name:     ISU
//Creation Date:    January 5, 2019
//Modified Date:    January 20, 2019
//Description:      Manage enemies in game

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
    class Enemy
    {
        //Generate random numbers
        protected Random rng = new Random();

        //Determine when to move fireball back
        public bool moveBack = false;

        public Enemy()
        {

        }

        public Enemy(Vector2 startLoc)
        {

        }

        /// <summary>
        /// Retrieve enemy rectangle
        /// </summary>
        /// <returns>Enemy rectangle</returns>
        public virtual Rectangle GetRec()
        {
            return new Rectangle();
        }

        /// <summary>
        /// Retrieve collision rectangle
        /// </summary>
        /// <returns>Enemy collision rectangle</returns>
        public virtual Rectangle GetColRec()
        {
            return new Rectangle();
        }

        /// <summary>
        /// Load enemy content
        /// </summary>
        /// <param name="Content">Load content</param>
        public virtual void Load(ContentManager Content)
        {

        }

        /// <summary>
        /// Update enemy animation
        /// </summary>
        /// <param name="gameTime">Time passing in game</param>
        public virtual void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Control enemy movement
        /// </summary>
        /// <param name="gameTime">Time passing in game</param>
        public virtual void Move(GameTime gameTime)
        {

        }

        /// <summary>
        /// Steal collectible from player upon collision
        /// </summary>
        /// <param name="collected">Array of all collectibles player collected</param>
        public virtual void Steal(int[] collected)
        {
            //Generate random number
            int randomNum = rng.Next(1, 6);

            //Determine collectible to steal
            while (true)
            {
                //Do not steal collectible if all are at 0
                if (collected[1] == 0 && collected[2] == 0 && collected[3] == 0 && collected[4] == 0 && 
                    collected[5] == 0)
                {
                    break;
                }

                //Steal collectible
                if (randomNum == 1 && collected[1] > 0)
                {
                    //Decrement stars
                    collected[1]--;
                    break;
                }
                else if (randomNum == 2 && collected[2] > 0)
                {
                    //Decrement mirrors
                    collected[2]--;
                    break;
                }
                else if (randomNum == 3 && collected[3] > 0)
                {
                    //Decrement chalices
                    collected[3]--;
                    break;
                }
                else if (randomNum == 4 && collected[4] > 0)
                {
                    //Decrement diamonds
                    collected[4]--;
                    break;
                }
                else if (randomNum == 5 && collected[5] > 0)
                {
                    //Decrement crowns
                    collected[5]--;
                    break;
                }

                //Generate new random number
                randomNum = rng.Next(1, 6);
            }
        }
        
        /// <summary>
        /// Draw enemy animation
        /// </summary>
        /// <param name="spriteBatch">Draw images</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
