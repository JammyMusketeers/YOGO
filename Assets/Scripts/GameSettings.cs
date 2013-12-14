using UnityEngine;
using System.Collections;

public class GameSettings : MonoSingleton<GameSettings>
{
	#region Public Members
	public int tileSize = 5;
	public int mapSize = 150;
	#endregion
	
	public void Start() {}
	
	public void Awake() {}
}