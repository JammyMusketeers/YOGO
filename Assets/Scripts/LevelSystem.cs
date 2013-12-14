using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSystem : MonoSingleton<LevelSystem>
{
	#region Private Members
	private Dictionary<string, GameObject> objectCache;
	#endregion
	
	public void Start() {}
	
	public void Awake()
	{
		objectCache = new Dictionary<string, GameObject>();
	}
	
	public void Hello()
	{
		GenerateDungeon();
	}
	
	public void GenerateDungeon()
	{
		var generator = new DungeonGenerator();
			generator.Generate();
	}
	
	public Vector3 ToVec3(int x, int y)
	{
		var tileSize = (float)GameSettings.Instance.tileSize;
		return new Vector3((float)x * tileSize, 0f, (float)y * tileSize);
	}
	
	public Vec2i ToVec2i(Vector3 vector)
	{
		var tileSize = (float)GameSettings.Instance.tileSize;
		return new Vec2i(Mathf.CeilToInt(vector.x / tileSize), Mathf.CeilToInt(vector.z / tileSize));
	}
	
	public GameObject GetObject(string name)
	{
		if (!objectCache.ContainsKey(name))
		{
			var resource = Resources.Load(name) as GameObject;
			if (resource == null) { return null; }
			
			objectCache[name] = resource;
		}
		
		return objectCache[name];
	}
	
	public GameObject LoadObject(string name)
	{
		return Instantiate(GetObject(name)) as GameObject;
	}
}