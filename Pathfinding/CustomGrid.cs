using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinding
{
    /**
     * CustomGrid - this is the grid in which the game takes place
     * The game takes place on a 2D array of type GridSpace
     */
    class CustomGrid
    {
        private GridSpace[,] grid;
        private int x;
        private int y;
        private int probability;
        private int units;

        public CustomGrid(int x, int y, int probability, int units)
        {
            this.grid = new GridSpace[x, y];
            this.x = x;
            this.y = y;
            this.probability = probability;
            this.units = units + 1;

            //Populate the grid on construct
            PopulateGrid();
        }

        /**
         * PrintGrid - Print the grid to console
         */
        public void PrintGrid()
        {
            Console.WriteLine(" === Grid Contents ===");
            for (int y = 0; y < this.y; y++)
            {
                Console.WriteLine("");
                for (int x = 0; x < this.x; x++)
                {
                    if(this.grid[x,y].GetSpaceOccupant() > 0)
                    {
                        Console.Write(this.grid[x,y].GetSpaceOccupant());
                    }
                    else
                    {
                        Console.Write(this.grid[x, y].GetSpaceType() + " ");
                    }
                }
            }
        }

        /**
         * PopulateGrid - populate the grid with GridSpaces
         */
        public void PopulateGrid()
        {
            Random random = new Random();

            int spaces = this.y * this.x;
            int currentSpace = 0;
            int usableTerrain = 0;

            //Loop through the grid columns and rows
            for (int y = 0; y < this.y; y++)
            {
                for (int x = 0; x < this.x; x++)
                {
                    int rand = random.Next(100);

                    //If unit requires this space to be useable terrain, set it correctly
                    if(spaces - currentSpace == this.units - usableTerrain || spaces - currentSpace < this.units - usableTerrain)
                    {
                        GridSpace space = new GridSpace(0);
                        this.grid[x, y] = space;
                        usableTerrain++;
                    }
                    //If random is less than user set probability make useable terrain
                    else if (rand < this.probability)
                    {
                        GridSpace space = new GridSpace(0);
                        this.grid[x, y] = space;
                        usableTerrain++;
                    }
                    //Else make it unusable terrain
                    else
                    {
                        GridSpace space = new GridSpace(1);
                        this.grid[x, y] = space;
                    }

                    currentSpace++;
                }
            }
        }

        /**
         * SetPlace - set a place occupant
         * int oldX - old space x location to remove occupant from
         * int oldY - old space y location to remove occupant from
         * int newX - new space x location to add occupant to
         * int newY - new space y location to add occupant to
         * int unit - the unit to be added to the space
         */
        public void SetPlace(int oldX, int oldY, int newX, int newY, int unit)
        {
            this.grid[oldX, oldY].RemoveOccupant();
            this.grid[newX, newY].SetSpaceOccupant(unit);
        }

        // Getters
        public int GetGridX()
        {
            return this.x;
        }

        public int GetGridY()
        {
            return this.y;
        }

        public GridSpace[,] GetGrid()
        {
            return this.grid;
        }

        public GridSpace GetGridPosition(int x, int y)
        {
            return this.grid[x, y];
        }

    }
}
