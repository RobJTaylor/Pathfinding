using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinding
{
    class Unit
    {
        private int id;
        private int positionX;
        private int positionY;

        public Unit(int id, CustomGrid grid)
        {
            this.id = id;
            bool unitSet = false;
            Random rand = new Random();

            while (unitSet == false)
            {
                int x = rand.Next(grid.GetGridX());
                int y = rand.Next(grid.GetGridY());

                if (grid.GetGridPosition(x,y) == 0)
                {
                    this.positionX = x;
                    this.positionY = y;
                    unitSet = true;
                }
            }
        }

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
