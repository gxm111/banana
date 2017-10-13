using UnityEngine;
using System.Collections;

public class CameraFollow2D_new : MonoBehaviour {

	public GameObject cameraTarget;						// object to look at/follow
	//var cameraTarget_down : GameObject;
	public GameObject player;							// player object for moving
	
	public float smoothTime = 0.01f;						// time for camera dampen
	public bool cameraFollowX = true;					// camera follows on horizontal
	public bool cameraFollowY = false;					// camera follows on vertical
	public bool cameraFollowHeight = false;				// camera follows cameraTarget object height, not the Y
	public float cameraHeight = 2.5f;					// height of camera
	public Vector3 velocity;							// speed of the camera movement
	private Transform thisTransform;					// camera's transform
	public float borderY;// = 8f;
	public bool moveUp = false;
	public bool moveDown = false;
	//public Transform groundCheck;
	public bool grounded = false;
	public float limitY;
	public Transform cameraBottomLimit;
	float cameraBottomLimit_y = 2.1f;
	[HideInInspector]
		public 	Transform rotatingPlayer;
	[HideInInspector]
		public 	bool stopFollow = false;
	MonkeyController2D playerController;
	[HideInInspector]
	public bool transition = false;
	[HideInInspector]
	public float upperLimit = 0.7f;
	// Use this for initialization


	void Awake () 
	{
		cameraBottomLimit_y = cameraBottomLimit.position.y;
		thisTransform = transform;
		limitY = Camera.main.ViewportToWorldPoint(Vector3.one*0.7f).y;
		//rotatingPlayer = GameObject.Find("RotatingPlayer3D").transform; //MOZDA CE DA SE VRATI
		cameraTarget.transform.position = new Vector3(cameraTarget.transform.position.x,Camera.main.transform.position.y,cameraTarget.transform.position.z);
		playerController = player.GetComponent<MonkeyController2D>();
		borderY = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,0)).y*0.8f;
	}

	void Start()
	{
		//upperLimit /= Camera.main.orthographicSize;
		if(Camera.main.aspect < 1.5f)
		{
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,7.5f,Camera.main.transform.position.z);
			cameraTarget.transform.position = new Vector3(cameraTarget.transform.position.x,Camera.main.transform.position.y,cameraTarget.transform.position.z);
			//borderY = 15f;
			borderY = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,0)).y*0.75f;
		}
		else
		{
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,8.5f,Camera.main.transform.position.z);
			cameraTarget.transform.position = new Vector3(cameraTarget.transform.position.x,Camera.main.transform.position.y,cameraTarget.transform.position.z);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		//Debug.Log("Player: " + player.transform.position.y + ", limit: " + (Camera.main.ScreenToWorldPoint(new Vector3(0,Screen.height*upperLimit,0)).y) + ", moveUp: " + moveUp);
//		if(Camera.main.transform.position.y < cameraBottomLimit_y && !stopFollow)
//		{
//			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,cameraBottomLimit_y,Camera.main.transform.position.z);
//			stopFollow = true;
//			//GameObject.Find("Follow 2_plan").SetActive(false);
//			//GameObject.Find("Follow 3_plan").SetActive(false);
//			//GameObject.Find("Follow 2.5_plan").SetActive(false);
//			playerController.state = MonkeyController2D.State.wasted;
//			GameObject.Find("ButtonPause").collider.enabled = false;
//			GameObject.Find("_GameManager").SendMessage("showFailedScreen");
//
//		}
		if(!stopFollow)
		{
			if(cameraFollowX == true)
			{
				//thisTransform.position = Vector3.SmoothDamp(thisTransform.position, new Vector3(cameraTarget.transform.position.x, thisTransform.position.y, thisTransform.position.z),ref velocity, smoothTime);

				thisTransform.position = Vector3.Lerp(thisTransform.position, new Vector3(cameraTarget.transform.position.x, thisTransform.position.y, thisTransform.position.z), 5*Time.deltaTime);
			}
			if(cameraFollowY == true)
			{
				thisTransform.position = Vector3.SmoothDamp(thisTransform.position, new Vector3(thisTransform.position.x, cameraTarget.transform.position.y, thisTransform.position.z),ref velocity, smoothTime);
			}
			if(!cameraFollowY && cameraFollowHeight)
			{
				Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, cameraHeight, Camera.main.transform.position.z);
			}
			//if(/*(cameraTarget.transform.position.y > camera.transform.position.y + borderY)*/player.transform.position.y > Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height*upperLimit,0)).y || (playerController.collisionAngle/**180/Mathf.PI */> 1 && playerController.collisionAngle/**180/Mathf.PI */< 355))// || (playerController.state == MonkeyController2D.State.running && cameraTarget.transform.position.y > Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,0)).y*0.5f + 5))
			if((playerController.state == MonkeyController2D.State.jumped && player.transform.position.y > Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height*upperLimit,0)).y) || (playerController.state != MonkeyController2D.State.jumped && player.transform.position.y > Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height*0.25f,0)).y))
			{
				moveUp = true;
				//cameraTarget = playerController.cameraTarget;
			}
			if(moveUp)
			{
				if(playerController.heCanJump)
					thisTransform.position = Vector3.SmoothDamp(thisTransform.position, new Vector3(thisTransform.position.x, cameraTarget.transform.position.y,thisTransform.position.z),ref velocity, smoothTime, 2000*Time.deltaTime, Time.smoothDeltaTime); // BILO JE 1000 * Time.deltaTime
			}
			if(playerController.state == MonkeyController2D.State.jumped && Mathf.Abs(cameraTarget.transform.position.y - Camera.main.transform.position.y) <= 0.1f && moveUp)
			{
				moveUp = false;
				//moveDown = true;
			}
			if(cameraTarget.transform.position.y <= Camera.main.transform.position.y)
			{
				moveDown = true;
			}
			if(moveDown)
			{
				if(!transition)
					thisTransform.position = new Vector3(thisTransform.position.x, cameraTarget.transform.position.y, thisTransform.position.z);
			}
			if(cameraTarget.transform.position.y>= Camera.main.transform.position.y)
			{
				moveDown = false;
			}
		}
	}
}

