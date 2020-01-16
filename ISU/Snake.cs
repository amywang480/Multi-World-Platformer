//Author:           Amy Wang
//File Name:        Snake.cs
//Project Name:     ISU
//Creation Date:    January 5, 2019
//Modified Date:    January 20, 2019
//Description:      Create and manage snakes as enemies for player

using Animation2D;
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
    class Snake : Enemy
    {
        //Store snake image and animation
        Texture2D snakeImg;
        Animation snakeAnim;

        //Store starting and ending locations to move between
        Vector2 startLoc;
        Vector2 endLoc;
        
        //Store speed for movement
        int speed = 2;

        //Store direction snake faces
        string dir = "RIGHT";

        //Store rectangle for collision
        Rectangle colRec;
    
        //Store X adjustment for collision rectangle
        int colRecX = 20;

        public Snake(Vector2 startLoc, Vector2 endLoc) : base(startLoc)
        {
            //Store starting and ending locations
            this.startLoc = startLoc;
            this.endLoc = endLoc;
        }

        /// <summary>
        /// Retrieve snake rectangle
        /// </summary>
        /// <returns>Snake rectangle from snake animation</returns>
        public override Rectangle GetRec()
        {
            return snakeAnim.destRec;
        }
        
        /// <summary>
        /// Retrieve collision rectangle
        /// </summary>
        /// <returns>Collision rectangle</returns>
        public override Rectangle GetColRec()
        {
            return colRec;
        }

        /// <summary> 
        /// Load snake content
        /// </summary>
        /// <param name="Content">Load content</param>
        public override void Load(ContentManager Content)
        {
            //Load snake image and animation
            snakeImg = Content.Load<Texture2D>("Enemies/Snake");
            snakeAnim = new Animation(snakeImg, 10, 1, 10, 0, 0, Animation.ANIMATE_FOREVER, 4, startLoc, 0.55f, true);

            //Load collision rectangle
            colRec = new Rectangle(snakeAnim.destRec.X + colRecX, snakeAnim.destRec.Y, 48, 30);
        }

        /// <summary>
        /// Update snake animation
        /// </summary>
        /// <param name="gameTime">Time passing in game</param>
        public override void Update(GameTime gameTime)
        {
            colRec.X = snakeAnim.destRec.X + colRecX;
            snakeAnim.Update(gameTime);
        }

        /// <summary>
        /// Control snake movement
        /// </summary>
        /// <param name="gameTime">Time passing in game</param>
        public override void Move(GameTime gameTime)
        {
            //Snake moves according to speed
            snakeAnim.destRec.X += speed;

            //When facing right:
            if (dir == "RIGHT")
            {
                //Check if snake reaches end location
                if (snakeAnim.destRec.X >= endLoc.X)
                {
                    //Switch snake direction
                    speed *= -1;
                    dir = "LEFT";
                }
            }

            //When facing left:
            if (dir == "LEFT")
            {
                //Check if snake reaches start location
                if (snakeAnim.destRec.X <= startLoc.X)
                {
                    //Switch snake direction
                    speed *= -1;
                    dir = "RIGHT";
                }
            }
        }

        /// <summary>
        /// Draw snake on screen
        /// </summary>
        /// <param name="spriteBatch">Draw images</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Draw snake facing the current direction it moves in
            switch (dir)
            {
                case "RIGHT":
                    snakeAnim.Draw(spriteBatch, Color.White, SpriteEffects.FlipHorizontally);
                    break;
                case "LEFT":
                    snakeAnim.Draw(spriteBatch, Color.White, SpriteEffects.None);
                    break;
            }
        }
    }
}
