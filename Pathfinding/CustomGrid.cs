using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinding
{
    class CustomGrid
    {
        private int[,] grid;
        private int x;
        private int y;
        private int probability;
        private int units;

        public CustomGrid(int x, int y, int probability, int units)
        {
            this.grid = new int[x, y];
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
                    Console.Write(this.grid[x,y] + " ");
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
                        this.grid[x, y] = 0;
                        usableTerrain++;
                    }
                    else if (rand < this.probability)
                    {
                        this.grid[x, y] = 0;
                        usableTerrain++;
                    } else
                    {
                        this.grid[x, y] = 1;
                    }

                    currentSpace++;
                }
            }
        }

        public void SetUnitPlaces(Unit[] units)
        {
            for(int unit = 0; unit <= units.Length; unit++)
            {
                int positionX = units[unit].GetPositionX();
                int positionY = units[unit].GetPositionY();
                this.grid[positionX,positionY] = 3;
            }

            Console.WriteLine(" === Grid with Units ===");
            PrintGrid();
        }

        public void SetPlace(int x,int y,int value)
        {
            this.grid[x, y] = value;
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

        public int[,] GetGrid()
        {
            return this.grid;
        }

        public int GetGridPosition(int x, int y)
        {
            return this.grid[x, y];
        }

    }
}
