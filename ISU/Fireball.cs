//Author:           Amy Wang
//File Name:        Fireball.cs
//Project Name:     ISU
//Creation Date:    January 5, 2019
//Modified Date:    January 20, 2019
//Description:      Create fireball as enemy for player

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
    class Fireball : Enemy
    {
        //Store fireball image and animation
        Texture2D fireballImg;
        Animation fireballAnim;

        //Store collision rectangle
        Rectangle colRec;
        
        //Store adjustments for collision rectangle
        int addColRecY = 13;
        int addColRecX = 15;

        //Store speed of fireball
        int speed = 5;

        public Fireball() : base()
        {

        }

        /// <summary>
        /// Retrieve fireball rectangle
        /// </summary>
        /// <returns>Fireball rectangle from fireball animation</returns>
        public override Rectangle GetRec()
        {
            return fireballAnim.destRec;
        }

        /// <summary>
        /// Retrieve collision rectangle
        /// </summary>
        /// <returns>Collsion rectangle</returns>
        public override Rectangle GetColRec()
        {
            return colRec;
        }

        /// <summary>
        /// Load fireball content
        /// </summary>
        /// <param name="Content">Load content</param>
        public override void Load(ContentManager Content)
        {
            //Load fireball image and animation
            fireballImg = Content.Load<Texture2D>("Enemies/Fireball");
            fireballAnim = new Animation(fireballImg, 6, 1, 6, 0, 0, Animation.ANIMATE_FOREVER, 4, new Vector2(2900, 300), 
                           0.15f, true);
            
            //Load collision rectangle
            colRec = new Rectangle(fireballAnim.destRec.X + addColRecX, fireballAnim.destRec.Y + addColRecY, 33, 25);
        }
    
        /// <summary>
        /// Control fireball movement
        /// </summary>
        /// <param name="gameTime">Time passing in game</param>
        public override void Move(GameTime gameTime)
        {
            //Make adjustments for collision rectangle
            colRec.X = fireballAnim.destRec.X + addColRecX;
            colRec.Y = fireballAnim.destRec.Y + addColRecY;

            //Update fireball animation
            fireballAnim.Update(gameTime);
            
            //Move fireball from right to left across screen
            fireballAnim.destRec.X -= speed;
        
            //When fireball moves past left side of screen:
            if (fireballAnim.destRec.Right < 0)
            {
                //Randomize new X value past right side of screen
                fireballAnim.destRec.X = rng.Next(2900, 5000);

                //Randomize new Y value
                fireballAnim.destRec.Y = rng.Next(50, 650);
            }
        }
    
        /// <summary>
        /// Steal collectible from player
        /// </summary>
        /// <param name="collected"></param>
        public override void Steal(int[] collected)
        {
            //Randomize new X value past right side of screen
            fireballAnim.destRec.X = rng.Next(2900, 5000);

            //Randomize new Y value
            fireballAnim.destRec.Y = rng.Next(50, 650);

            //Call base class version to steal collectible
            base.Steal(collected);
        }

        /// <summary>
        /// Draw fireball animation
        /// </summary>
        /// <param name="spriteBatch">Draw images</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            fireballAnim.Draw(spriteBatch, Color.White, SpriteEffects.FlipHorizontally);
        }
    }
}
