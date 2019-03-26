using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinding
{
    /**
     * GridSpace - Placed on the grid. Contains info about space type and unit occupying it.
     */
    class GridSpace
    {
        private int spaceType;
        private int occupant;

        public GridSpace(int spaceType)
        {
            this.spaceType = spaceType;
            //Set to -1 by default, a unit will change this if necessary
            this.occupant = -1;
        }

        /**
         * GetSpaceType - get the space type
         * Returns int - 0 = moveable terrain, 1 = immoveable terrain
         */
        public int GetSpaceType()
        {
            return this.spaceType;
        }
        
        /**
         * GetSpaceOccupant - gets a space occupant, if they exist
         * Returns int - -1 = no unit | greater than 0 = unit
         */
        public int GetSpaceOccupant()
        {
            return this.occupant;
        }

        /**
         * SetSpaceOccupant - set the space occupant
         * int occupantId - the unit id occupying the space
         */
        public void SetSpaceOccupant(int occupantId)
        {
            this.occupant = occupantId;
        }

        /**
         * RemoveOccupant - remove the occupant from the space
         */
        public void RemoveOccupant()
        {
            this.occupant = -1;
        }
    }
}
