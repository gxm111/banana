#pragma strict

	var cameraTarget : GameObject;					// object to look at/follow
	var cameraTarget_down : GameObject;
	var player : GameObject;						// player object for moving
	
	var smoothTime : float = 0.1f;					// time for camera dampen
	var cameraFollowX : boolean = true;				// camera follows on horizontal
	var cameraFollowY : boolean = true;				// camera follows on vertical
	var cameraFollowHeight : boolean = false;		// camera follows cameraTarget object height, not the Y
	var cameraHeight : float = 2.5f;				// height of camera
	var velocity : Vector2;							// speed of the camera movement
	private var thisTransform : Transform;			// camera's transform
	var borderY : float = 0.1f;
	var moveUp : boolean = false;
	var moveDown : boolean = false;
	var groundCheck : Transform;
	var grounded : boolean = false;
	var limitY : float;
	var cameraBottomLimit : float = 2.1f;
	@HideInInspector
	var rotatingPlayer: Transform;
	@HideInInspector
	var stopFollow : boolean = false;
	
	// Use this for initialization
	function Awake () 
	{
		thisTransform = transform;
		groundCheck = GameObject.Find("GroundCheck").transform;
		limitY = GetComponent.<Camera>().ViewportToWorldPoint(Vector3.one*0.7f).y; 
		//transform.position.y = cameraTarget.transform.position.y;
		rotatingPlayer = GameObject.Find("RotatingPlayer").transform;
		cameraTarget.transform.position.y = GetComponent.<Camera>().transform.position.y;
	}
	
	// Update is called once per frame
	function Update ()
	{
		if(GetComponent.<Camera>().transform.position.y < cameraBottomLimit)
		{
			GetComponent.<Camera>().transform.position.y = cameraBottomLimit;
			stopFollow = true;
			GameObject.Find("Follow 2_plan").SetActive(false);
			GameObject.Find("Follow 3_plan").SetActive(false);
		}
		if(!stopFollow)
		{
		grounded = Physics2D.Linecast(player.transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		//Debug.Log("Kamera: " + transform.position.y + ", gg: " + cameraTarget.transform.position.y + ", grounded: " + grounded);
		//Debug.Log("target: " + cameraTarget.transform.position.x);
		if(cameraFollowX == true)
		{
			thisTransform.position.x = Mathf.SmoothDamp(thisTransform.position.x, cameraTarget.transform.position.x, velocity.x, smoothTime);
		}
		if(cameraFollowY == true)
		{
			thisTransform.position.y = Mathf.SmoothDamp(thisTransform.position.y, cameraTarget.transform.position.y, velocity.y, smoothTime);
		}
		if(!cameraFollowY && cameraFollowHeight)
		{
			Camera.main.transform.position.y = cameraHeight;
		}
		if((cameraTarget.transform.position.y > GetComponent.<Camera>().transform.position.y + borderY) || (rotatingPlayer.rotation.z*180/Mathf.PI > 5 && rotatingPlayer.rotation.z*180/Mathf.PI < 355))// && !grounded))// || (cameraTarget.transform.position.y > camera.transform.position.y && grounded))// (player.transform.position.y > limitY && grounded));
		{
			//Debug.Log("Usao u move up");
			//Debug.Log(cameraTarget.transform.position.y + " > " + camera.transform.position.y + borderY);
			moveUp = true;
		}
		if(moveUp)
		{
			GetComponent.<Camera>().transform.position.y = Mathf.SmoothDamp(GetComponent.<Camera>().transform.position.y, cameraTarget.transform.position.y, velocity.y, smoothTime, 15, Time.smoothDeltaTime);
			//transform.position.y = Mathf.Lerp(transform.position.y, cameraTarget.transform.position.y, smoothTime*Time.deltaTime*10);
		}
		if(Mathf.Abs(cameraTarget.transform.position.y - GetComponent.<Camera>().transform.position.y) <= 0.1f && moveUp)
		{
			moveUp = false;
			moveDown = true;
		}
		if(cameraTarget.transform.position.y <= GetComponent.<Camera>().transform.position.y)// - borderY)
		{
			//Debug.Log("Usao u move down");
			moveDown = true;
		}
		if(moveDown)
		{
			//camera.transform.position.y = Mathf.SmoothDamp(camera.transform.position.y, cameraTarget.transform.position.y, velocity.y, smoothTime, 25, Time.smoothDeltaTime);
			GetComponent.<Camera>().transform.position.y = cameraTarget.transform.position.y;
			//transform.position.y = Mathf.MoveTowards(transform.position.y, cameraTarget.transform.position.y, smoothTime*Time.deltaTime*10);
		}
		if(cameraTarget.transform.position.y >= GetComponent.<Camera>().transform.position.y)
		{
			moveDown = false;
		}
		}
	}