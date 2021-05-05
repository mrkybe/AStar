using Microsoft.Xna.Framework;
using System;

namespace AStar
{
	/// <summary>
	/// The minimum interface the a* algorithm needs to interact with a 2d map.
	/// </summary>
	public interface IMap
	{
		/// <summary>
		/// The width of a map
		/// </summary>
		int Width { get; }

		/// <summary>
		/// The height of this map
		/// </summary>
		int Height { get; }

		/// <summary>
		/// Check whether or not a 2d point is walkable
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		bool GetWalkable(int x, int y);
	}
}
