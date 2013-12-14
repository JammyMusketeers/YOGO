using System.Collections.Generic;
using System.Collections;
using Algorithms;
using UnityEngine;
using System;

public class DungeonGenerator
{
	#region Static Members
	public static byte WALL = 0;
	public static byte DOOR = 40;
	public static byte HALL = 1;
	public static byte FLOOR = 0;
	public static byte EMPTY = 50;
	
	/*
	public static byte WALL = 1;
	public static byte DOOR = 0;
	public static byte HALL = 0;
	public static byte FLOOR = 0;
	*/
	#endregion
	
	#region Public Members
	public List<RoomObject> roomList;
	public byte[,] tileMap;
	#endregion
	
	#region Private Members
	private int mapSize;
	#endregion
	
	public void Generate()
	{
		MakeRooms(); ConnectRooms();
	}
	
	private float Rand()
	{
		return UnityEngine.Random.Range(0f, 100f) / 100f;
	}
	
	private void MakeRooms()
	{
		var roomChance = GameSettings.Instance.roomChance;
		
		for (int x = 0; x < mapSize; x++)
		{
			for (int y = 0; y < mapSize; y++)
			{
				if (Rand() <= roomChance)
					MakeRoom(x, y);
			}
		}
	}
	
	public void MakeRoom(int x, int y)
	{
		var roomGo = LevelSystem.Instance.GetObject("Environment/Rooms/Room01");
		var roomObject = roomGo.GetComponent<RoomObject>();
		
		for (int rx = -1; rx <= roomObject.width + 2; rx++)
		{
			for (int ry = -1; ry <= roomObject.height + 2; ry++)
			{
				var thisX = x + rx;
				var thisY = y + ry;
				
				if (GetTileValue(thisX, thisY) != EMPTY)
					return;
				
				if (OutOfRange(thisX, thisY))
					return;
			}
		}
		
		roomGo = GameObject.Instantiate(roomGo) as GameObject;
		roomObject = roomGo.GetComponent<RoomObject>();
		
		for (int rx = 0; rx < roomObject.width; rx++)
		{
			for (int ry = 0; ry < roomObject.height; ry++)
			{
				SetTileValue(x + rx, y + ry, FLOOR);
			}
		}
		
		roomGo.transform.position = LevelSystem.Instance.ToVec3(x, y);
		roomList.Add(roomObject);
		
		foreach (var doorGo in roomObject.doors)
		{
			var toVec2i = LevelSystem.Instance.ToVec2i(
				doorGo.transform.position);
			
			SetTileValue(toVec2i.x, toVec2i.y, DOOR, null, "Door");
		}
		
		for (int rx = x - 1; rx <= x + roomObject.width; rx++)
		{
			SetTileValue(rx, y + roomObject.height, WALL, EMPTY, "Wall");
			SetTileValue(rx, y - 1, WALL, EMPTY, "Wall");
		}
		
		for (int ry = y - 1; ry <= y + roomObject.height; ry++)
		{
			SetTileValue(x + roomObject.width, ry, WALL, EMPTY, "Wall");
			SetTileValue(x - 1, ry, WALL, EMPTY, "Wall");
		}
	}
	
	private void ConnectRooms()
	{
		var canRoomsTangle = GameSettings.Instance.roomTangling;
		
		foreach (var roomObject in roomList)
		{
			foreach (var doorObject in roomObject.doors)
			{
				RoomObject other = null;
				
				while (other == null || (other == roomObject && !canRoomsTangle))
				{
					var randomIdx = UnityEngine.Random.Range(0, roomList.Count - 1);
					other = roomList[randomIdx];
				}
				
				if (other != null)
					ConnectRoom(doorObject, other);
			}
		}
	}
	
	private void ConnectRoom(GameObject doorObject, RoomObject b)
	{
		var randomIdx = UnityEngine.Random.Range(0, b.doors.Length - 1);
		ConnectDoors(doorObject, b.doors[randomIdx]);
	}
	
	private void ConnectDoors(GameObject a, GameObject b)
	{
		var positionA = LevelSystem.Instance.ToVec2i(
			a.transform.position);
		
		var positionB = LevelSystem.Instance.ToVec2i(
			b.transform.position);
		
		PathFinderUtil.SetMap(tileMap);
		
		var pointA = new PathFinderPoint(positionA.x, positionA.y);
		var pointB = new PathFinderPoint(positionB.x, positionB.y);
		var pathData = PathFinderUtil.FindPath(pointA, pointB);
		
		foreach (var path in pathData.path)
			SetTileValue((int)path.x, (int)path.z, HALL, null, "Path");
	}
	
	public void SetTileValue(int x, int y, byte val, byte? condition = null, string tile = null)
	{
		if (OutOfRange(x, y) || (condition.HasValue && GetTileValue(x, y) != condition.Value))
			return;
		
		if (!string.IsNullOrEmpty(tile))
		{
			var blank = LevelSystem.Instance.LoadObject("Environment/Temp/" + tile);
				blank.transform.position = LevelSystem.Instance.ToVec3(x, y);
		}
		
		tileMap[x, y] = val;
	}
	
	public byte GetTileValue(int x, int y)
	{
		if (OutOfRange(x, y)) { return 0; }
		return tileMap[x, y];
	}
	
	public bool OutOfRange(int x, int y)
	{
		if (x < 0 || y < 0 || x >= mapSize || y >= mapSize)
			return true;
		
		return false;
	}
	
	public DungeonGenerator()
	{
		mapSize = GameSettings.Instance.mapSize;
		roomList = new List<RoomObject>();
		tileMap = new byte[mapSize,mapSize];
		
		for (int x = 0; x < mapSize; x++)
		{
			for (int y = 0; y < mapSize; y++)
				tileMap[x,y] = EMPTY;
		}
	}
}