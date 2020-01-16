//Author:           Amy Wang
//File Name:        Obstacle.cs
//Project Name:     ISU
//Creation Date:    January 6, 2019
//Modified Date:    January 20, 2019
//Description:      Manage obstacles in game

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
    class Obstacle
    {
        //Generate random numbers
        protected Random rng = new Random();

        public Obstacle(Vector2 loc)
        {

        }

        /// <summary>
        /// Retrieve obstacle rectangle
        /// </summary>
        /// <returns>Obstacle rectangle</returns>
        public virtual Rectangle GetRec()
        {
            return new Rectangle();
        }
        
        /// <summary>
        /// Retrieve collision rectangle
        /// </summary>
        /// <returns>Collision rectangle</returns>
        public virtual Rectangle GetColRec()
        {
            return new Rectangle();
        }

        /// <summary>
        /// Retrieve type of obstacle
        /// </summary>
        /// <returns>Boolean to determine if obstacle or not</returns>
        public virtual bool GetObsType()
        {
            return false;
        }

        /// <summary>
        /// Load obstacle content
        /// </summary>
        /// <param name="Content">Load content</param>
        public virtual void Load(ContentManager Content)
        {

        }
    
        /// <summary>
        /// Retrieve amount of health stolen
        /// </summary>
        /// <returns>Random number of health stolen</returns>
        public virtual int GetHealthStolen()
        {
            return rng.Next(20, 41);
        }

        /// <summary>
        /// Draw obstacle on screen
        /// </summary>
        /// <param name="spriteBatch">Draw images</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
