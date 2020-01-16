//Author:           Amy Wang
//File Name:        Item.cs
//Project Name:     ISU
//Creation Date:    January 7, 2019
//Modified Date:    January 20, 2019
//Description:      Manage all items for player to use (potions)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISU
{
    class Item
    {
        //Generate random numbers
        protected Random rng = new Random();

        //Store cost of item and random amounts
        protected int cost;
        protected int randomAmount;

        public Item()
        {
            
        }

        /// <summary>
        /// Retrieve cost
        /// </summary>
        /// <returns>Item cost</returns>
        public virtual int GetCost()
        {
            return 0;
        }

        /// <summary>
        /// Retrieve power
        /// </summary>
        /// <returns>Power of item</returns>
        public virtual int GetPower()
        {
            return 0;
        }

        /// <summary>
        /// Retrieve next item in queue
        /// </summary>
        /// <returns>Next item in queue</returns>
        public virtual Item GetNext()
        {
            return null;
        }

        /// <summary>
        /// Set next item in queue
        /// </summary>
        /// <param name="newItem">Next item in queue</param>
        public virtual void SetNext(Item newItem)
        {
            
        }
    }
}
