using UnityEngine;
using System.Collections;

public class FinishEvent : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag == "Monkey")
		{
			GetComponent<Collider2D>().enabled = false;
			GameObject.FindGameObjectWithTag("Monkey").GetComponent<MonkeyController2D>().Finish();
		}
	}
}
