//Author:           Amy Wang
//File Name:        Platform.cs
//Project Name:     ISU
//Creation Date:    December 19, 2018
//Modified Date:    January 20, 2019
//Description:      Create tiles a part of platforms

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
    class Tile
    {
        //Store tile rectangle
        Rectangle tileRec;

        public Tile(int x, int y, Texture2D tileType, double scale)
        {
            //Create tile rectangle
            tileRec = new Rectangle(x, y, (int)(tileType.Width * scale), (int)(tileType.Height * scale));
        }

        /// <summary>
        /// Retrieve tile rectangle
        /// </summary>
        /// <returns>Tile rectangle</returns>
        public Rectangle GetTileRec()
        {
            return tileRec;
        }
    }
}
