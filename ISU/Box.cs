//Author:           Amy Wang
//File Name:        Box.cs
//Project Name:     ISU
//Creation Date:    January 6, 2019
//Modified Date:    January 20, 2019
//Description:      Create boxes as obstacles for the player

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
    class Box : Obstacle
    {
        //Store box images and rectangle
        Texture2D obsImg;
        Texture2D notObsImg;
        Rectangle boxRec;
        
        //Store location
        Vector2 loc;

        //Determine if box is obstacle or not
        bool isObs = false;

        public Box(Vector2 loc, bool isObs) : base(loc)
        {
            //Store location and boolean for obstacle
            this.loc = loc;
            this.isObs = isObs;
        }

        /// <summary>
        /// Retrieve box rectangle
        /// </summary>
        /// <returns>Box rectangle</returns>
        public override Rectangle GetRec()
        {
            return boxRec;
        }

        /// <summary>
        /// Retrieve type of box (obstacle or not)
        /// </summary>
        /// <returns>Boolean to determine if box is obstacle</returns>
        public override bool GetObsType()
        {
            return isObs;
        }
    
        /// <summary>
        /// Load box
        /// </summary>
        /// <param name="Content">Load content</param>
        public override void Load(ContentManager Content)
        {
            //Load box images and rectangles
            obsImg = Content.Load<Texture2D>("Obstacles/ObsBox");
            notObsImg = Content.Load<Texture2D>("Obstacles/NotObsBox");
            boxRec = new Rectangle((int)loc.X, (int)loc.Y, 30, 30);
        }

        /// <summary>
        /// Draw box on screen
        /// </summary>
        /// <param name="spriteBatch">Draw images</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Draw obstacle or non obstacle box
            switch (isObs)
            {
                case true:
                    spriteBatch.Draw(obsImg, boxRec, Color.White);
                    break;
                case false:
                    spriteBatch.Draw(notObsImg, boxRec, Color.White);
                    break;
            }
        }
    }
}
