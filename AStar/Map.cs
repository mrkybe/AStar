using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar
{
    class Map
    {
        private int[,] map;
        private int height;
        private int width;

        public Map()
        {
            SetMap(0);
        }

        public void SetMap(int id)
        {
            switch (id)
            {
                case 0:
                    {
                        map = new[,] {{0,0,0,0,1,0,0},
                                      {1,1,1,1,1,1,0},
                                      {0,1,0,0,0,1,0},
                                      {0,1,0,1,0,1,0},
                                      {0,0,0,1,0,0,0}};
                        height = 5;
                        width = 7;
                        break;
                    }
                case 1:
                    {
                        map = new[,] {{0,1,0,0,0,0,0,0,0,0,0,0,0,1,0},
                                      {0,1,1,1,1,1,0,1,0,0,0,0,0,1,0},
                                      {0,1,0,0,0,1,0,1,0,0,0,0,0,1,0},
                                      {0,1,0,1,0,1,0,1,0,0,0,0,0,1,0},
                                      {0,1,0,1,1,1,0,1,0,0,0,0,0,1,0},
                                      {0,0,0,0,0,0,0,1,0,0,0,0,0,1,0},
                                      {1,1,1,1,1,1,0,1,0,0,0,0,0,1,0},
                                      {0,0,0,0,0,0,0,1,0,0,0,0,0,1,0},
                                      {0,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
                                      {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}};
                                      
                        height = 10;
                        width = 15;
                        break;
                    }
            }
        }

        public void Mark(int x, int y)
        {
            map[y, x] = 2;
        }

        public void Mark(Point input, int val)
        {
            map[input.Y, input.X] = val;
        }

        public void DrawMap()
        {
            for (int y = 0; y < height; y++)
            {
                String buffer = "";
                for (int x = 0; x < width; x++)
                {
                    if (map[y, x] == 0)
                    {
                        buffer += ' ';
                    }
                    if (map[y, x] == 1)
                    {
                        buffer += 'X';
                    }
                    if (map[y, x] == 2)
                    {
                        buffer += '.';
                    }
                    if (map[y, x] == 3)
                    {
                        buffer += 'S';
                    }
                    if (map[y, x] == 4)
                    {
                        buffer += 'G';
                    }
                }
                Console.Write(buffer + "\n");
            }
            Console.WriteLine("--------------------------------");
            Console.ReadLine();
        }

        public int Height
        {
            get { return height; }
        }

        public int Width
        {
            get { return width; }
        }

        public bool GetWalkable(int x,int y)
        {
            return map[y,x] == 0;
        }
    }

    
}
