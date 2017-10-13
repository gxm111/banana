using UnityEngine;
using System.Collections;

public class DestroySpearGorilla : MonoBehaviour 
{
	public Transform gorilla;

	public void DestroyGorilla()
	{
		gorilla.GetComponent<KillTheBaboon>().DestoyEnemy();
	}
}


