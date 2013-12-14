using UnityEngine;


public class LevelSystem : MonoSingleton<LevelSystem>
{
	public void Start()
	{
	}
	
	public void Awake()
	{
	}
	
	public void Hello()
	{
		Debug.LogError("Hello!");
	}
}