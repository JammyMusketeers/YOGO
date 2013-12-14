using System;
using Algorithms;
using UnityEngine;
using System.Collections.Generic;

public static class PathFinderUtil
{
	#region Private Members
	private static IPathFinder pathfinder;
	#endregion
	
	public static void SetMap(byte[,] map)
	{
		pathfinder = new PathFinderFast(map);
		pathfinder.Diagonals = false;
	}
	
	public static PathData FindPath(PathFinderPoint start, PathFinderPoint end, Vector2 offset)
	{
		var path = pathfinder.FindPath(start, end);
		var pathData = new PathData(path, offset);
		
		return pathData;
	}
	
	public static PathData FindPath(PathFinderPoint start, PathFinderPoint end)
	{
		var path = pathfinder.FindPath(start, end);
		var pathData = new PathData(path);
		
		return pathData;
	}
	
	#region Constructor
	static PathFinderUtil() {}
	#endregion
}