using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar
{
    class Program
    {
        static void Main(string[] args)
        {
            Map myMap = new Map();
            Pathfinder pathfinder = new Pathfinder(myMap);
            Point startP = new Point(0,0);
            Point endP = new Point(4, 4);
            myMap.Mark(startP, 3);
            myMap.Mark(endP, 4);
            myMap.DrawMap();
            
            List<Point> path = pathfinder.FindPath(startP, endP);
            Console.WriteLine("Step count: " + path.Count);
            for (int i = 0; i < path.Count; i++)
            {
                myMap.Mark(path[i].X, path[i].Y);
                
            }
            myMap.Mark(startP, 3);
            myMap.Mark(endP, 4);
            myMap.DrawMap();
        }
    }
}
