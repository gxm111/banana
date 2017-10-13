using UnityEngine;
using System.Collections;

public class RotationInfinity : MonoBehaviour {
	
	bool jump = false;
	bool landed = true;
	// Update is called once per frame
	void Update () 
	{
		transform.GetChild(0).Rotate(0,500*Time.deltaTime,0);
		if(Random.Range(1,100) < 5 && landed)
		{
			jump = true;
			landed = false;
		}
	}
	void FixedUpdate()
	{
		if(jump)
		{
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0,15000));
			jump = false;
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == "Footer")
		{
			landed = true;
		}
	}
}
