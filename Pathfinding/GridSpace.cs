using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinding
{
    class GridSpace
    {
        private int spaceType;
        private int occupant;

        public GridSpace(int spaceType)
        {
            this.spaceType = spaceType;
            this.occupant = -1;
        }

        public int GetSpaceType()
        {
            return this.spaceType;
        }
        
        public int GetSpaceOccupant()
        {
            return this.occupant;
        }

        public void SetSpaceOccupant(int occupantId)
        {
            this.occupant = occupantId;
        }

        public void removeOccupant()
        {
            this.occupant = -1;
        }
    }
}
