using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar
{
    class Pathfinder
    {
        private SearchNode[,] searchNodes;
        private List<SearchNode> openList = new List<SearchNode>();
        private List<SearchNode> closedList = new List<SearchNode>();

        private int width;
        private int height;

        public Pathfinder(Map map)
        {
            width = map.Width;
            height = map.Height;
            InitializeSearchNodes(map);
        }

        private void InitializeSearchNodes(Map map)
        {
            searchNodes = new SearchNode[width,height];
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < width; y++)
                {
                    SearchNode node = new SearchNode();
                    node.Position = new Point(x,y);
                    node.Walkable = map.GetWalkable(x, y);
                    if (node.Walkable)
                    {
                        node.Neighbors = new SearchNode[4];
                        searchNodes[x, y] = node;
                    }
                }
            }
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    SearchNode node = searchNodes[x, y];
                    if (node == null || node.Walkable == false)
                    {
                        continue;
                    }

                    Point[] neighbors = new Point[]
                    {
                        new Point(x ,y - 1),
                        new Point(x ,y + 1),
                        new Point(x - 1, y),
                        new Point(x + 1, y),
                    };

                    for(int i = 0; i < neighbors.Length; i++)
                    {
                        Point position = neighbors[i];
                        if (position.X < 0 || position.X > width - 1 ||
                           position.Y < 0 || position.Y > height - 1)
                        {
                            continue;
                        }
                        SearchNode neighbor = searchNodes[position.X, position.Y];
                        if (neighbor == null || neighbor.Walkable == false)
                        {
                            continue;
                        }
                        node.Neighbors[i] = neighbor;
                    }
                }
            }
        }
    }
}
