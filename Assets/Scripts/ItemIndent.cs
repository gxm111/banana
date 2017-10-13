using UnityEngine;
using System.Collections;

public class ItemIndent : MonoBehaviour {

	VerticalScroll parentScript;
	Transform myTransform;
	float centerOfScreen;
	float startPosX;
	bool regulate;

	void Start ()
	{
		parentScript = transform.parent.parent.GetComponent<VerticalScroll>();
		myTransform = transform;
		centerOfScreen = Camera.main.ViewportToWorldPoint(Vector3.one/2).y;
		startPosX = myTransform.position.x;
		Debug.Log("CenterOfScreen:" + centerOfScreen);
	}
	
	void Update () 
	{
		if(parentScript.canScroll)
		{
			if(myTransform.position.y >= centerOfScreen-2.5f && myTransform.position.y <= centerOfScreen+2.5f)
			{
				myTransform.position = new Vector3(Mathf.Clamp(Mathf.MoveTowards(myTransform.position.x,(startPosX+0.5f-myTransform.position.y),1f),startPosX,startPosX+0.5f),myTransform.position.y,myTransform.position.z);
				//if(myTransform.position.y != centerOfScreen)
			}
			else
			{
				myTransform.position = new Vector3(Mathf.Clamp(Mathf.MoveTowards(myTransform.position.x,startPosX,1f),startPosX,startPosX+0.5f),myTransform.position.y,myTransform.position.z);
			}
		}
	}
}
