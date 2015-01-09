using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AStar
{
	/// <summary>
	/// A class used to find a path on a map.
	/// </summary>
	public class Pathfinder
	{
		#region Properties

		/// <summary>
		/// A 2d array of all the walkable nodes in teh map.
		/// </summary>
		private readonly SearchNode[,] searchNodes;

		/// <summary>
		/// The width of the map
		/// </summary>
		private readonly int width;

		/// <summary>
		/// The height of the map.
		/// </summary>
		private readonly int height;

		private readonly List<SearchNode> openList = new List<SearchNode>();

		#endregion //Properties

		#region Init

		/// <summary>
		/// Constructor!!!
		/// </summary>
		/// <param name="map">The map this object will be used to find paths on </param>
		public Pathfinder(IMap map)
		{
			//grab the height and width of the map
			width = map.Width;
			height = map.Height;

			//Create an empty array of nodes the smae size as the map
			searchNodes = new SearchNode[width, height];

			InitializeSearchNodes(map);
		}

		/// <summary>
		/// Set up the map with all the search nodes, and let each node find it's neighbors
		/// </summary>
		/// <param name="map"></param>
		private void InitializeSearchNodes(IMap map)
		{
			CreateSearchNodes(map);

			SetSearchNodeNeighbors();
		}

		/// <summary>
		/// Given a map, create a searchable node at each walkable position.
		/// </summary>
		/// <param name="map"></param>
		private void CreateSearchNodes(IMap map)
		{
			//loop through the whole map and create nodes at each walkable position
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					//If this is a walkable position on the map, create a node there
					if (map.GetWalkable(x, y))
					{
						searchNodes[x, y] = new SearchNode(new Point(x, y));
					}
				}
			}
		}

		/// <summary>
		/// Loop through the whole map of searchable nodes and let each node find it's neighbor nodes.
		/// </summary>
		private void SetSearchNodeNeighbors()
		{
			//Create an array of points to use for the neighbor locations
			var neighbors = new Point[]
                    {
                        new Point(), //top, new Point(x, y - 1)
                        new Point(), //bottom, new Point(x, y + 1)
                        new Point(), //left, new Point(x - 1, y)
                        new Point() //right, new Point(x + 1, y)
                    };

			//Now that there is a node at every walkable position, let the nodes find their neighbors.
			for (int x = 0; x < width; x++)
			{
				//Starting to check a new row, set the row location of all the neighbors
				neighbors[0].X = x;
				neighbors[1].X = x;
				neighbors[2].X = x - 1;
				neighbors[3].X = x + 1;

				for (int y = 0; y < height; y++)
				{
					//Check if there is a walkable node there
					SearchNode node = searchNodes[x, y];
					if (node == null)
					{
						continue;
					}

					//Set the Y location of all the neighbors
					neighbors[0].Y = y - 1;
					neighbors[1].Y = y + 1;
					neighbors[2].Y = y;
					neighbors[3].Y = y;

					for (int i = 0; i < neighbors.Length; i++)
					{
						//Check if this neighbor point is still on the map
						Point position = neighbors[i];
						if (position.X < 0 || position.X > width - 1 ||
						   position.Y < 0 || position.Y > height - 1)
						{
							continue;
						}

						//is the neighbor point a walkable position?
						SearchNode neighbor = searchNodes[position.X, position.Y];
						if (null != neighbor)
						{
							node.Neighbors[i] = neighbor;
						}
					}
				}
			}
		}

		#endregion //Init

		#region Methods

		/// <summary>
		/// Find the optimal path between two points on our map.
		/// </summary>
		/// <param name="startPoint">the start point on the map</param>
		/// <param name="endPoint">the end point on the map</param>
		/// <returns>a list containing the path from start to end.  Contains every point, not just way points</returns>
		public List<Point> FindPath(Point startPoint, Point endPoint)
		{
			//if they are the same point...
			if (startPoint == endPoint)
			{
				return new List<Point>();
			}

			//Reset all the search terms
			ResetSearchNodes();

			//Store references to start and end nodes for convience.
			SearchNode startNode = searchNodes[startPoint.X, startPoint.Y];
			SearchNode endNode = searchNodes[endPoint.X, endPoint.Y];

			//Set the start node's G value to 0 and its F value to the estimated distance between the start node and goal node.
			startNode.DistanceTraveled = 0;
			startNode.DistanceToGoal = Heuristic(startPoint, endPoint);

			//add it to the open list
			startNode.InOpenList = true;
			openList.Add(startNode);

			// Step 3: While there are still nodes on the open list...
			while (openList.Count > 0)
			{
				// Find the node with the lowest F value
				SearchNode currentNode = FindBestNode();

				//If the open list is empty or no node can be found, there is no path
				if (currentNode == null)
				{
					break;
				}

				//If we've reached our goal...
				if (currentNode == endNode)
				{
					return FindFinalPath(endNode);
				}

				// If not, keep going through the open list
				for (int i = 0; i < currentNode.Neighbors.Length; i++)
				{
					SearchNode neighbor = currentNode.Neighbors[i];
					if (neighbor == null)
					{
						continue;
					}

					float distanceTraveled = currentNode.DistanceTraveled + 1;
					float heuristic = Heuristic(neighbor.Position, endPoint);

					//If the neighbor isn't in the closed or open list
					if (neighbor.InOpenList == false && neighbor.InClosedList == false)
					{
						neighbor.DistanceTraveled = distanceTraveled;
						neighbor.DistanceToGoal = distanceTraveled + heuristic;
						neighbor.Parent = currentNode;
						neighbor.InOpenList = true;
						openList.Add(neighbor);
					}
					else if (neighbor.InOpenList || neighbor.InClosedList)
					{
						if (neighbor.DistanceTraveled > distanceTraveled)
						{
							neighbor.DistanceTraveled = distanceTraveled;
							neighbor.DistanceToGoal = distanceTraveled + heuristic;
							neighbor.Parent = currentNode;
						}
					}
				}
				openList.Remove(currentNode);
				currentNode.InClosedList = true;
			}

			//There was no path between the two points.
			return new List<Point>();
		}

		/// <summary>
		/// Clear the open and closed lists and reset all of the nodes F and G values incase they're still set from last time.
		/// </summary>
		private void ResetSearchNodes()
		{
			openList.Clear();

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					SearchNode node = searchNodes[x, y];
					if (node == null)
					{
						continue;
					}
					node.Reset();
				}
			}
		}

		/// <summary>
		/// Calculate the cost of the distance between two points
		/// </summary>
		/// <param name="point1"></param>
		/// <param name="point2"></param>
		/// <returns></returns>
		private static float Heuristic(Point point1, Point point2)
		{
			return Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y);
		}

		/// <summary>
		/// Given the current list of open nodes, find the open node with the smallest cost.
		/// </summary>
		/// <returns></returns>
		private SearchNode FindBestNode()
		{
			//Check if it's even possible to find a path
			if (openList.Count <= 0)
			{
				//This means the open list is empty and there is no path between the start & end node
				return null;
			}

			//Start with the first open node...
			SearchNode currentTile = openList[0];
			float smallestDistanceToGoal = currentTile.DistanceToGoal;

			//Check all nodes after that one, trying to find a node with smaller cost
			for (int i = 1; i < openList.Count; i++)
			{
				if (openList[i].DistanceToGoal < smallestDistanceToGoal)
				{
					currentTile = openList[i];
					smallestDistanceToGoal = currentTile.DistanceToGoal;
				}
			}

			//The node we found with the smallest cost
			return currentTile;
		}

		private static List<Point> FindFinalPath(SearchNode endNode)
		{
			//list to hold all the points
			var finalPath = new List<Point>();

			//Add each node to the list until we reach the beginning
			SearchNode node = endNode;
			while (null != node)
			{
				//Add the point to the path list
				finalPath.Add(node.Position);

				//move back a step
				node = node.Parent;
			}

			//Now reverse that list because we added everything backwards
			finalPath.Reverse();
			return finalPath;
		}

		#endregion //Methods
	}
}
