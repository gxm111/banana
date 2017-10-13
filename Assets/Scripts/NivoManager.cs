using UnityEngine;
using System.Collections;

public class NivoManager : MonoBehaviour {

	public int currentLevel;
	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

//	void Update()
//	{
//		//Debug.Log("Current Level: " + currentLevel);
//	}
}
