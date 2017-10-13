using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	public GameObject cameraTarget;					// object to look at/follow
	public GameObject player;						// player object for moving
	
	public float smoothTime = 0.1f;					// time for camera dampen
	public bool cameraFollowX = true;				// camera follows on horizontal
	public bool cameraFollowY = true;				// camera follows on vertical
	public bool cameraFollowHeight = false;			// camera follows cameraTarget object height, not the Y
	public float cameraHeight = 2.5f;				// height of camera
	public Vector2 velocity;						// speed of the camera movement
	Transform thisTransform;						// camera's transform
	public bool changeHeight = false;				// for gradually changing camera Y position
	MonkeyController2D monkeyControll;
	
	// Use this for initialization
	void Start () 
	{
		thisTransform = transform;
		monkeyControll = GameObject.Find("Player").GetComponent<MonkeyController2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(cameraFollowX == true)
		{
			float step = Mathf.SmoothDamp(thisTransform.position.x, cameraTarget.transform.position.x, ref velocity.x, smoothTime);
			thisTransform.position = new Vector3(step ,thisTransform.position.y, thisTransform.position.z);
			//Camera.main.transform.position = thisTransform.position;
		}
		if(cameraFollowY == true)
		{
			//if(monkeyControll != null)
			{
				//if(!monkeyControll.inAir)
				{
					float step = Mathf.SmoothDamp(thisTransform.position.y, cameraTarget.transform.position.y, ref velocity.y, smoothTime);
					thisTransform.position = new Vector3(thisTransform.position.x, step, thisTransform.position.z);
				}
			}
		}
		if(!cameraFollowY && cameraFollowHeight)
		{
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, cameraHeight, Camera.main.transform.position.z);
		}
		if(changeHeight == true)
		{
			StartCoroutine(catchCameraY());
		}
		if(changeHeight == false)
		{
			//StopAllCoroutines();
		}
	}
	
	IEnumerator catchCameraY()
	{
		float i = 0;
		while( i < 1 )
		{
			Debug.Log("Usao u korutinu: " + i);
			yield return null;
			if(changeHeight)
			{
				thisTransform.position = new Vector3(thisTransform.position.x, Mathf.MoveTowards(thisTransform.position.y,cameraTarget.transform.position.y,i), thisTransform.position.z);
				i+=0.001f;
			}
			else break;
		}
		changeHeight = false;
	}
}
