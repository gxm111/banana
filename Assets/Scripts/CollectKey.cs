using UnityEngine;
using System.Collections;

public class CollectKey : MonoBehaviour {

	ManageFull manage;
	Animator anim;

	void Start () 
	{
		manage = GameObject.Find("_GameManager").GetComponent<ManageFull>();
		anim = GetComponent<Animator>();
	}
	

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag == "Monkey")
		{
			//Debug.Log("kolko bre ovo");
			GetComponent<Collider2D>().enabled = false;
			anim.Play("CollectKey");
			Invoke("NotifyManager",0.25f);
		}
	}

	void NotifyManager()
	{
		GameObject.Find("_GameManager").SendMessage("KeyCollected");
	}
}
