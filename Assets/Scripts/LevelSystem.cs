using UnityEngine;
using System.Collections;

public class LevelSystem : MonoSingleton<LevelSystem>
{
	public void Start() {}
	
	public void Awake()
	{
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
}