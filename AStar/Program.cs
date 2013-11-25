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
            myMap.DrawMap();
            Pathfinder pathfinder = new Pathfinder(myMap);

        }
    }
}
