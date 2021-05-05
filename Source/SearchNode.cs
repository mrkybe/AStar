using Microsoft.Xna.Framework;

namespace AStar
{
	/// <summary>
	/// This is a node that keeps track of where the connecting nodes are all located.
	/// </summary>
	class SearchNode
	{
		#region Properties

		/// <summary>
		/// The previous node in the search path. 
		/// null when search is started, and set during path finding phase.
		/// </summary>
		public SearchNode Parent { get; set; }
		public bool InOpenList;
		public bool InClosedList;
		public float DistanceToGoal;
		public float DistanceTraveled;

		/// <summary>
		/// Where this node is located.
		/// </summary>
		public Point Position { get; private set; }

		/// <summary>
		/// The four neighbors of this search node.
		/// A neighbor will be null if there is no walkable node there.
		/// </summary>
		public SearchNode[] Neighbors { get; private set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// constructor!!!!!
		/// </summary>
		/// <param name="pos">the location of this node</param>
		public SearchNode(Point pos)
		{
			Position = pos;

			//create the empty neighbors array
			Neighbors = new SearchNode[4];
		}

		/// <summary>
		/// Reset the search node before we do a search
		/// </summary>
		public void Reset()
		{
			Parent = null;
			InOpenList = false;
			InClosedList = false;
			DistanceToGoal = float.MaxValue;
			DistanceTraveled = float.MaxValue;
		}

		#endregion //Methods
	}
}
