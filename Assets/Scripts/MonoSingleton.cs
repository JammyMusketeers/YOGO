using UnityEngine;
using System.Collections;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	#region Private Members
	private static T _instance;
	#endregion
	
	public static T Instance
	{
		get
		{
			if (_instance != null)
				return _instance;
			
			var type = typeof(T).ToString();
			var go = GameObject.Find(type);
			
			if (go == null)
			{
				go = new GameObject(type);
				_instance = go.AddComponent<T>();
			}
			else
			{
				_instance = go.GetComponent<T>();
			}
			
			return _instance;
		}
	}
}
