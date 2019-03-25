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

        public CustomGrid(int x, int y, int probability)
        {
            this.grid = new int[x, y];
            this.x = x;
            this.y = y;
            this.probability = probability;

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

            for (int y = 0; y < this.y; y++)
            {
                for (int x = 0; x < this.x; x++)
                {
                    int rand = random.Next(100);

                    if (rand < this.probability)
                    {
                        this.grid[x, y] = 0;
                    } else
                    {
                        this.grid[x, y] = 1;
                    }
                }
            }

            Console.WriteLine(" === Populated Grid ===");
            PrintGrid();
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

    }
}
