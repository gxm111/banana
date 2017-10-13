using UnityEngine;
using System.Collections;

public class UbrzanjeTest : MonoBehaviour {

	float force = 100;
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		GetComponent<Rigidbody2D>().AddForce(new Vector2(0,force));
		force += 5f;
	
	}
}
