//Author:           Amy Wang
//File Name:        Collectible.cs
//Project Name:     ISU
//Creation Date:    December 27, 2018
//Modified Date:    January 20, 2019
//Description:      Create collectibles for player to collect

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
    class Collectible
    {
        //Store collectible image and rectangle
        Texture2D img;
        Rectangle imgRec;

        //Store coin animation
        Animation coin;

        //Store name of collectible
        string name;

        //Determine when to draw collectible on screen
        public bool isDrawn = false;

        public Collectible(string name)
        {
            //Store name of collectible
            this.name = name;
        }
   
        /// <summary>
        /// Retrieve collectible rectangle
        /// </summary>
        /// <returns>Collectible rectangle</returns>
        public Rectangle GetRec()
        {
            return imgRec;
        }
        
        /// <summary>
        /// Retrieve name of collectible
        /// </summary>
        /// <returns>Name of collectible</returns>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// Load collectible image and rectangle
        /// </summary>
        /// <param name="Content">Load content</param>
        /// <param name="loc">Location of collectible</param>
        public void Load(ContentManager Content, Vector2 loc)
        {
            //Adjust chalice collectible X value
            int addChaliceX = 5;
        
            //Load collectible images
            img = Content.Load<Texture2D>("Collectibles/" + name);
            
            if (name == "Chalice")
            {
                //Make adjustments for chalice rectangle
                imgRec = new Rectangle((int)loc.X + addChaliceX, (int)loc.Y, 18, 36);
            }
            else
            {
                //Load all other rectangles
                imgRec = new Rectangle((int)loc.X, (int)loc.Y, 25, 25);
            }
        }

        /// <summary>
        /// Load coin animation
        /// </summary>
        /// <param name="Content">Load content</param>
        /// <param name="loc">Location of coin</param>
        public void LoadCoin(ContentManager Content, Vector2 loc)
        {
            coin = new Animation(img, 10, 1, 10, 0, 0, Animation.ANIMATE_FOREVER, 4, loc, 0.27f, true);
        }

        /// <summary>
        /// Update coin animation
        /// </summary>
        /// <param name="gameTime">Time passing in game</param>
        public void Update(GameTime gameTime)
        {
            coin.Update(gameTime);
        }

        /// <summary>
        /// Draw collectible on screen
        /// </summary>
        /// <param name="spriteBatch">Draw images</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isDrawn)
            {
                spriteBatch.Draw(img, imgRec, Color.White);
            }
        }
    
        /// <summary>
        /// Draw coin animation on screen
        /// </summary>
        /// <param name="spriteBatch">Draw images</param>
        public void DrawCoin(SpriteBatch spriteBatch)
        {
            if (isDrawn)
            {
                coin.Draw(spriteBatch, Color.White, SpriteEffects.None);
            }
        }
    }
}
