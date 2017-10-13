using UnityEngine;
using System.Collections;

public class FreezeRotation : MonoBehaviour {

	public Transform tr;
	Transform myTransform;
	Vector3 shieldPosition;
	float x;
	float y;

	void Start()
	{
		shieldPosition = transform.position;
	}
	void Update () 
	{
		transform.position = new Vector3(tr.position.x,tr.position.y,transform.position.z);
	}
}
