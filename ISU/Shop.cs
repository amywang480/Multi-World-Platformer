//Author:           Amy Wang
//File Name:        Shop.cs
//Project Name:     ISU
//Creation Date:    January 7, 2019 
//Modified Date:    January 20, 2019
//Description:      Create and manage shops selling potions

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
    class Shop
    {
        //Store background image
        Texture2D bgImg;
        Rectangle bgRec;

        //Store potion images and rectangles
        Texture2D timePotionImg;
        Rectangle timeRec;
        Texture2D healthPotionImg;
        Rectangle healthRec;
        Texture2D shieldPotionImg;
        Rectangle shieldRec;

        //Store shelf image
        Texture2D shelfImg;
        Rectangle shelfRec;

        //Store shop sign image
        Texture2D signImg;
        Rectangle signRec;

        //Store button image and rectangles for buttons
        Texture2D btnImg;
        Rectangle exitRec;
        Rectangle coinRec;
        Rectangle buyTimeRec;
        Rectangle buyHealthRec;
        Rectangle buyShieldRec;

        //Store scroll image
        Texture2D scrollImg;
        Rectangle scrollRec;
        
        //Store fonts
        SpriteFont buyFont;
        SpriteFont nameFont;

        //Store items shop sells
        Item[] items = new Item[3];

        //Generate random numbers
        Random rng = new Random();

        //Determine when potion images are drawn
        public bool isTimeDrawn = true;
        public bool isHealthDrawn = true;
        public bool isShieldDrawn = true;

        //Determine when potion info is drawn
        public bool isTimeInfoDrawn = false;
        public bool isHealthInfoDrawn = false;
        public bool isShieldInfoDrawn = false;
    
        public Shop(Texture2D timePotionImg, Texture2D healthPotionImg, Texture2D shieldPotionImg)
        {
            //Store potion images
            this.timePotionImg = timePotionImg;
            this.healthPotionImg = healthPotionImg;
            this.shieldPotionImg = shieldPotionImg;
           
            //Instantiate potions shop sells
            items[0] = new Time(rng.Next(10, 21), rng.Next(10000, 20001));
            items[1] = new Health(rng.Next(10, 21), rng.Next(50, 151));
            items[2] = new Shield(rng.Next(10, 21), rng.Next(10000, 15001));
        }

        /// <summary>
        /// Retrieve time potion shop is selling
        /// </summary>
        /// <returns>Time potion as an Item</returns>
        public Item GetTime()
        {
            return items[0];
        }

        /// <summary>
        /// Retrieve health potion shop is selling
        /// </summary>
        /// <returns>Health potion as an Item</returns>
        public Item GetHealth()
        {
            return items[1];
        }

        /// <summary>
        /// Retrieve shield potion shop is selling
        /// </summary>
        /// <returns>Shield potion as an Item</returns>
        public Item GetShield()
        {
            return items[2];
        }

        /// <summary>
        /// Retrieve time potion rectangle
        /// </summary>
        /// <returns>Time potion rectangle</returns>
        public Rectangle GetTimeRec()
        {
            return timeRec;
        }

        /// <summary>
        /// Retrieve health potion rectangle
        /// </summary>
        /// <returns>Health potion rectangle</returns>
        public Rectangle GetHealthRec()
        {
            return healthRec;
        }

        /// <summary>
        /// Retrieve shield potion rectangle
        /// </summary>
        /// <returns>Shield potion rectangle</returns>
        public Rectangle GetShieldRec()
        {
            return shieldRec;
        }

        /// <summary>
        /// Retrieve buy time potion rectangle
        /// </summary>
        /// <returns>Buy time potion rectangle</returns>
        public Rectangle GetBuyTimeRec()
        {
            return buyTimeRec;
        }

        /// <summary>
        /// Retrieve buy health potion rectangle
        /// </summary>
        /// <returns>Buy health potion rectangle</returns>
        public Rectangle GetBuyHealthRec()
        {
            return buyHealthRec;
        }

        /// <summary>
        /// Retrieve buy shield potion rectangle
        /// </summary>
        /// <returns>Buy shield potion rectangle</returns>
        public Rectangle GetBuyShieldRec()
        {
            return buyShieldRec;
        }

        /// <summary>
        /// Retrieve exit rectangle
        /// </summary>
        /// <returns>Exit rectangle</returns>
        public Rectangle GetExitRec()
        {
            return exitRec;
        }
        
        /// <summary>
        /// Load shop images, rectangles, and font
        /// </summary>
        /// <param name="Content"></param>
        public void Load(ContentManager Content)
        {
            //Load background image
            bgImg = Content.Load<Texture2D>("Shop/ShopBg");
            bgRec = new Rectangle(0, 0, 1300, 780);

            //Load potion rectangles
            timeRec = new Rectangle(150, 340, 100, 100);
            healthRec = new Rectangle(150, 490, 93, 100);
            shieldRec = new Rectangle(145, 640, 109, 100);
        
            //Load shelf image
            shelfImg = Content.Load<Texture2D>("Shop/Shelf");
            shelfRec = new Rectangle(40, 300, shelfImg.Width, shelfImg.Height);

            //Load shop sign image
            signImg = Content.Load<Texture2D>("Shop/Sign");
            signRec = new Rectangle(350, 30, (int)(signImg.Width * 0.5), (int)(signImg.Height * 0.35));

            //Load shop name font
            nameFont = Content.Load<SpriteFont>("Fonts/ShopName");

            //Load buttons
            btnImg = Content.Load<Texture2D>("Shop/Exit");
            exitRec = new Rectangle(1100, -30, (int)(btnImg.Width * 0.4), (int)(btnImg.Height * 0.4));
            coinRec = new Rectangle(30, -30, (int)(btnImg.Width * 0.4), (int)(btnImg.Height * 0.4));
            buyTimeRec = new Rectangle(280, 290, (int)(btnImg.Width * 0.4), (int)(btnImg.Height * 0.4));
            buyHealthRec = new Rectangle(280, 440, (int)(btnImg.Width * 0.4), (int)(btnImg.Height * 0.4));
            buyShieldRec = new Rectangle(280, 590, (int)(btnImg.Width * 0.4), (int)(btnImg.Height * 0.4));

            //Load scroll image
            scrollImg = Content.Load<Texture2D>("Shop/Scroll");
            scrollRec = new Rectangle(600, 300, 350, 200);
            
            //Load buying font
            buyFont = Content.Load<SpriteFont>("Fonts/Buy");
        }
    
        /// <summary>
        /// Restock the shop once the player leaves
        /// </summary>
        public void Restock()
        {
            //Sell new items
            items[0] = new Time(rng.Next(10, 21), rng.Next(10000, 20001));
            items[1] = new Health(rng.Next(10, 21), rng.Next(50, 151));
            items[2] = new Shield(rng.Next(10, 21), rng.Next(10000, 15001));

            //Draw potions in shop
            isTimeDrawn = true;
            isHealthDrawn = true;
            isShieldDrawn = true;

            //Remove info from screen
            isTimeInfoDrawn = false;
            isHealthInfoDrawn = false;
            isShieldInfoDrawn = false;
        }
    
        /// <summary>
        /// Draw shop images and fonts
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="numCoins">Number of coins player currently has</param>
        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, int numCoins)
        {
            //Display background image
            spriteBatch.Draw(bgImg, bgRec, Color.White);
    
            //Display shop sign
            spriteBatch.Draw(signImg, signRec, Color.White);
            spriteBatch.DrawString(nameFont, "Ozwald Ackerman's Potions\n" + "      That Won't Kill You", 
                                   new Vector2(400, 95), Color.Black);

            //Display exit button
            spriteBatch.Draw(btnImg, exitRec, Color.White);
            spriteBatch.DrawString(buyFont, "Exit", new Vector2(1173, 47), Color.Black);
        
            //Display player's number of coins
            spriteBatch.Draw(btnImg, coinRec, Color.White);
            spriteBatch.DrawString(buyFont, "Coins\n" + numCoins, new Vector2(80, 35), Color.Black);
        
            //Display shelf image
            spriteBatch.Draw(shelfImg, shelfRec, Color.White);

            //Display scroll image
            spriteBatch.Draw(scrollImg, scrollRec, Color.White);

            //When no potions are clicked, display message for player
            if (!isTimeInfoDrawn && !isHealthInfoDrawn && !isShieldInfoDrawn)
            {
                spriteBatch.DrawString(buyFont, "Please select a potion\n" + "to purchase", new Vector2(650, 360), Color.Black);
            }
      
            //When time potion is displayed:
            if (isTimeDrawn)
            {
                //Display time potion image
                spriteBatch.Draw(timePotionImg, timeRec, Color.White);

                //When time potion info is displayed:
                if (isTimeInfoDrawn)
                {
                    //Display time potion info
                    spriteBatch.DrawString(buyFont, "Time Potion\n" + "Cost: " + items[0].GetCost() + " coins" + "\n" + "Time Gained: " + 
                    items[0].GetPower() / 1000 + " sec.", new Vector2(650, 350), Color.Black);

                    //Display buy button
                    spriteBatch.Draw(btnImg, buyTimeRec, Color.White);
                    spriteBatch.DrawString(buyFont, "Buy", new Vector2(353, 365), Color.Black);
                }
            }

            //When health potion is displayed:
            if (isHealthDrawn)
            {
                //Display health potion image
                spriteBatch.Draw(healthPotionImg, healthRec, Color.White);
            
                //When health potion info is displayed:
                if (isHealthInfoDrawn)
                {
                    //Display health potion info
                    spriteBatch.DrawString(buyFont, "Health Potion\n" + "Cost: " + items[1].GetCost() + " coins" + "\n" + 
                    "Health Gained: " + items[1].GetPower(), new Vector2(650, 350), Color.Black);

                    //Display buy button
                    spriteBatch.Draw(btnImg, buyHealthRec, Color.White);
                    spriteBatch.DrawString(buyFont, "Buy", new Vector2(353, 515), Color.Black);
                }
            }

            //When shield potion is displayed:
            if (isShieldDrawn)
            {
                //Display shield potion image
                spriteBatch.Draw(shieldPotionImg, shieldRec, Color.White);

                //When shield potion info is displayed:
                if (isShieldInfoDrawn)
                {
                    //Display shield potion info
                    spriteBatch.DrawString(buyFont, "Shield Potion\n" + "Cost: " + items[2].GetCost() + " coins" + "\n" +
                    "Length of Time: " + items[2].GetPower() / 1000 + " sec.", new Vector2(650, 350), Color.Black);
                
                    //Display buy button
                    spriteBatch.Draw(btnImg, buyShieldRec, Color.White);
                    spriteBatch.DrawString(buyFont, "Buy", new Vector2(353, 667), Color.Black);
                }
            }
        }
    }
}
