//Author:           Amy Wang
//File Name:        ItemQueue.cs
//Project Name:     ISU
//Creation Date:    January 8, 2019
//Modified Date:    January 20, 2019
//Description:      Create item queues for potions

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISU
{
    class ItemQueue
    {
        //Store head of queue
        Item head;

        //Store number of elements
        int count = 0;

        /// <summary>
        /// Retrieve size of queue
        /// </summary>
        /// <returns>Number of elements</returns>
        public int GetSize()
        {
            return count;
        }

        /// <summary>
        /// Peek at the head of the queue
        /// </summary>
        /// <returns>Head of queue</returns>
        public Item Peek()
        {
            return head;
        }

        /// <summary>
        /// Add items to the queue
        /// </summary>
        /// <param name="newItem">New item to be added</param>
        public void Enqueue(Item newItem)
        {
            if (count == 0)
            {
                //Add to head when queue is empty
                head = newItem;
            }
            else
            {
                //Start with head of queue
                Item curItem = head;

                //Loop through until end of queue
                while (curItem.GetNext() != null)
                {
                    curItem = curItem.GetNext();
                }

                //Set next queue to new item
                curItem.SetNext(newItem);
            }

            //Increment count
            count++;
        }

        /// <summary>
        /// Remove first element of queue (head)
        /// </summary>
        public void Dequeue()
        {
            head = head.GetNext();
            count--;
        }
    }
}
