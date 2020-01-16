//Author:           Amy Wang
//File Name:        Blade.cs
//Project Name:     ISU
//Creation Date:    January 6, 2019
//Modified Date:    January 20, 2019
//Description:      Create blades as obstacles for player

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
    class Blade : Obstacle
    { 
        //Store blade image and rectangle
        Texture2D bladeImg;
        Rectangle bladeRec;

        //Store blade collision rectangle
        Rectangle colRec;

        //Store blade location
        Vector2 loc;

        //Store scaling for blade
        double scale;
        
        public Blade(Vector2 loc, double scale) : base(loc)
        {
            //Store location and scale
            this.loc = loc;
            this.scale = scale;
        }
    
        /// <summary>
        /// Retrieve blade rectangle
        /// </summary>
        /// <returns>Blade rectangle</returns>
        public override Rectangle GetRec()
        {
            return bladeRec;
        }

        /// <summary>
        /// Retrieve blade collsion rectangle
        /// </summary>
        /// <returns>Blade collision rectangle</returns>
        public override Rectangle GetColRec()
        {
            return colRec;
        }
    
        /// <summary>
        /// Load blade content
        /// </summary>
        /// <param name="Content">Load content</param>
        public override void Load(ContentManager Content)
        {
            //Load blade image and rectangle
            bladeImg = Content.Load<Texture2D>("Obstacles/Blade");
            bladeRec = new Rectangle((int)loc.X, (int)loc.Y, (int)(bladeImg.Width * scale), (int)(bladeImg.Height * scale));

            //Load collision rectangle
            colRec = new Rectangle((int)loc.X, (int)loc.Y + (int)(bladeImg.Height * scale * 0.5), (int)(bladeImg.Width * scale), 
            (int)(bladeImg.Height * scale * 0.5));
        }

        /// <summary>
        /// Draw blade on screen
        /// </summary>
        /// <param name="spriteBatch">Draw images</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bladeImg, bladeRec, Color.White);
        }
    }
}
