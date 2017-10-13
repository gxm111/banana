using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {
	
	public GameObject obj;
	// Use this for initialization
	void Start () {
	obj.transform.position=GetComponent<Camera>().ScreenToWorldPoint( new Vector3(Screen.width,Screen.height,10));
	
	//obj.transform.position= new Vector3(d.x-obj.transform.localScale.x/2,d.y,d.z);
	}
	
	// Update is called once per frame
	void Update () {
	
		
	}
}
