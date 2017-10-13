using UnityEngine;
using System.Collections;

public class EnemyPool : MonoBehaviour {

	public static int AvailableEnemies;

	void Start () 
	{
		AvailableEnemies = transform.childCount;
	}

	public GameObject getEnemy()
	{
		if(transform.childCount > 0)
			return transform.GetChild(Random.Range(0,transform.childCount)).gameObject;
		return null;
	}
}
