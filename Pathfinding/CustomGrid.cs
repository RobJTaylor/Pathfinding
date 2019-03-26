using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinding
{
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

            //PrintGrid();
            PopulateGrid();
        }

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

        public void PopulateGrid()
        {
            Random random = new Random();

            int spaces = this.y * this.x;
            int currentSpace = 0;
            int usableTerrain = 0;

            for (int y = 0; y < this.y; y++)
            {
                for (int x = 0; x < this.x; x++)
                {
                    int rand = random.Next(100);

                    if(spaces - currentSpace == this.units - usableTerrain || spaces - currentSpace < this.units - usableTerrain)
                    {
                        GridSpace space = new GridSpace(0);
                        this.grid[x, y] = space;
                        usableTerrain++;
                    }
                    else if (rand < this.probability)
                    {
                        GridSpace space = new GridSpace(0);
                        this.grid[x, y] = space;
                        usableTerrain++;
                    } else
                    {
                        GridSpace space = new GridSpace(1);
                        this.grid[x, y] = space;
                    }

                    currentSpace++;
                }
            }
        }

        public void SetPlace(int oldX, int oldY, int unit, int newX, int newY)
        {
            this.grid[oldX, oldY].RemoveOccupant();
            this.grid[newX, newY].SetSpaceOccupant(unit);
        }

        // Getters and Setters
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
