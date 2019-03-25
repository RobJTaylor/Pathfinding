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

        public CustomGrid(int x, int y)
        {
            this.grid = new int[x, y];
            this.x = x;
            this.y = y;

            writeGrid();
        }

        public int GetGridX()
        {
            return this.x;
        }

        public int GetGridY()
        {
            return this.y;
        }

        public void writeGrid()
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
    }
}
