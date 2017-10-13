using UnityEngine;
using System.Collections;

public class KillTheBaboon : MonoBehaviour {

	Transform babun;
	BoxCollider2D[] boxColliders;
	BabunDogadjaji_new babunScript;
	bool collidersTurnedOff = false;
	
	void Awake()
	{
		babun = transform.parent.Find("_MajmunceNadrlja");
		boxColliders = GetComponents<BoxCollider2D>();
		babunScript = babun.GetComponent<BabunDogadjaji_new>();
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == "Monkey")
		{
			turnOffColliders();
			babunScript.killBaboonStuff();
			//turnOffColliders();
		}
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Monkey")
		{
			turnOffColliders();
			babunScript.killBaboonStuff();
			//turnOffColliders();
		}
	}
	
	public void turnOffColliders()
	{
		boxColliders[0].enabled = false;
		//boxColliders[1].enabled = false;
		//boxColliders[2].enabled = false;
		collidersTurnedOff = true;
	}

	public void turnOnColliders()
	{
		if(collidersTurnedOff)
		{
			boxColliders[0].enabled = true;
			//boxColliders[2].enabled = true;
			collidersTurnedOff = false;
		}
	}

	public void DestoyEnemy()
	{
		turnOffColliders();
		babunScript.killBaboonStuff();
	}
}
