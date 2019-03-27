using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinding
{
    /**
     * Unit - units that can be moved
     */
    class Unit
    {
        private int id;
        private int positionX;
        private int positionY;

        public Unit(int id, CustomGrid grid)
        {
            this.id = id;

            //Spawn unit at a random, valid location
            bool unitSet = false;
            Random rand = new Random();

            //Loop while unit is not set at location
            while (unitSet == false)
            {
                int x = rand.Next(grid.GetGridX());
                int y = rand.Next(grid.GetGridY());

                //If the location is valid, set unit to that location
                if (grid.GetGridPosition(x,y).GetSpaceType() == 0 && grid.GetGridPosition(x,y).GetSpaceOccupant() < 0)
                {
                    this.positionX = x;
                    this.positionY = y;
                    grid.GetGridPosition(x, y).SetSpaceOccupant(id);
                    Console.WriteLine(x + y + " OCCUPIED");
                    unitSet = true;
                }
            }
        }

        //Getters
        public int GetPositionX()
        {
            return this.positionX;
        }

        public int GetPositionY()
        {
            return this.positionY;
        }
    }
}
