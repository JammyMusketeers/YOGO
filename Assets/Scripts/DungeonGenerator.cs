using System.Collections.Generic;
using System.Collections;
using Algorithms;
using UnityEngine;
using System;

public class DungeonGenerator
{
	#region Public Members
	public List<RoomObject> roomList;
	public byte[,] tileMap;
	#endregion
	
	#region Private Members
	private int mapSize;
	#endregion
	
	public void Generate()
	{
		MakeRooms();
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
				if (GetTileValue(x + rx, y + ry) != 0)
					return;
			}
		}
		
		roomGo = GameObject.Instantiate(roomGo) as GameObject;
		roomObject = roomGo.GetComponent<RoomObject>();
		
		for (int rx = 0; rx < roomObject.width; rx++)
		{
			for (int ry = 0; ry < roomObject.height; ry++)
			{
				SetTileValue(x + rx, y + ry, 2);
			}
		}
		
		roomGo.transform.position = LevelSystem.Instance.ToVec3(x, y);
		roomList.Add(roomObject);
		
		foreach (var doorGo in roomObject.doors)
		{
			var toVec2i = LevelSystem.Instance.ToVec2i(
				doorGo.transform.position);
			
			SetTileValue(toVec2i.x, toVec2i.y, 1);
		}
		
		for (int rx = x - 1; rx <= x + roomObject.width; rx++)
		{
			SetTileValue(rx, y + roomObject.height, 10, 0);
			SetTileValue(rx, y - 1, 10, 0);
		}
		
		for (int ry = y - 1; ry <= y + roomObject.height; ry++)
		{
			SetTileValue(x + roomObject.width, ry, 10, 0);
			SetTileValue(x - 1, ry, 10, 0);
		}
	}
	
	public void SetTileValue(int x, int y, byte val, byte? condition = null)
	{
		if (OutOfRange(x, y) || (condition.HasValue && GetTileValue(x, y) != condition.Value))
			return;
		
		if (val == 1)
		{
			var blank = LevelSystem.Instance.LoadObject("Environment/Temp/Door");
				blank.transform.position = LevelSystem.Instance.ToVec3(x, y);
		}
		else if (val == 10)
		{
			var blank = LevelSystem.Instance.LoadObject("Environment/Temp/Wall");
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
	}
}