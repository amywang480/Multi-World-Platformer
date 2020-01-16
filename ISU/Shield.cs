//Author:           Amy Wang
//File Name:        Shield.cs
//Project Name:     ISU
//Creation Date:    January 7, 2019
//Modified Date:    January 20, 2019
//Description:      Create shield potion to disable collision

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISU
{
    class Shield : Item 
    {
        //Store next potion
        Item next;

        public Shield(int cost, int randomAmount) : base()
        {
            //Store cost and random amount of time
            this.cost = cost;
            this.randomAmount = randomAmount;
        }

        /// <summary>
        /// Retrieve cost
        /// </summary>
        /// <returns>Cost of potion</returns>
        public override int GetCost()
        {
            return cost;
        }

        /// <summary>
        /// Retrive amount of time
        /// </summary>
        /// <returns>Amount of time</returns>
        public override int GetPower()
        {
            return randomAmount;
        }

        /// <summary>
        /// Retrieve next potion in queue
        /// </summary>
        /// <returns>Next Item in queue</returns>
        public override Item GetNext()
        {
            return next;
        }

        /// <summary>
        /// Set next potion in queue
        /// </summary>
        /// <param name="newItem">Next Item in queue</param>
        public override void SetNext(Item newItem)
        {
            next = newItem;
        }
    }
}
