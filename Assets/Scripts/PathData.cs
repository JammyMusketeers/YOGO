using System;
using UnityEngine;
using Algorithms;
using System.Collections.Generic;

public class PathData
{
	#region Public Members
	public List<Vector4> path;
	public float totalCost;
	#endregion
	
	private void ProcessPath(List<PathFinderNode> pathnodes, Vector2 offset)
	{
		if (pathnodes == null || pathnodes.Count < 2)
			return;
		
		pathnodes.Reverse();
			PathFinderNode startNode;
			PathFinderNode endNode;
		float cost;
		
		for (int i = 0; i < pathnodes.Count-1; i++)
		{
			startNode = pathnodes[i];
			endNode = pathnodes[i+1];
			cost = GetCost(startNode, endNode);
			totalCost += cost;
			
			path.Add(new Vector4(offset.x + endNode.X, 0, offset.y + endNode.Y, cost));
		}
	}
	
	private float GetCost(PathFinderNode startNode, PathFinderNode endNode)
	{
		if (startNode.X != endNode.X && startNode.Y != endNode.Y)
		{
			return 1.5f;
		}
		
		return 1;
	}
	
	#region Constructors
	public PathData(List<PathFinderNode> pathnodes) : this(pathnodes, new Vector2(0, 0)) {}
	
	public PathData(List<PathFinderNode> pathnodes, Vector2 offset)
	{
		path = new List<Vector4>();
		totalCost = 0;
		ProcessPath(pathnodes, offset);
	}
	#endregion
}