using UnityEngine;
using System.Collections;


public class RunWithSpeed_jednakamera : MonoBehaviour {
	
	public float speed = 5;
	public bool continueMoving = false;
	MonkeyController2D playerController;
	GameObject player;
	float offset;
	public bool FollowCameraHeight = false;
	public bool IskljuciKadIzadjeIzKadra = false;
	public bool smooth;
	bool smoothMove = false;
	float startSpeed;
	public Transform desnaGranica;
	Camera bgCamera;
	float bgCameraX;
	bool dovoljno = false;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Monkey");
		playerController = player.GetComponent<MonkeyController2D>();
		bgCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		bgCameraX = GameObject.FindGameObjectWithTag("MainCamera").transform.position.x;
		desnaGranica = transform.Find("DesnaGranica");
		offset = transform.position.y - Camera.main.transform.position.y;
		startSpeed = speed;
	}

	void Start()
	{

	}

	void Update () // BILO JE FixedUpdate
	{
		if(desnaGranica != null)
		{
			bgCameraX = bgCamera.transform.position.x;
			if(desnaGranica.position.x < bgCameraX - bgCamera.orthographicSize * bgCamera.aspect)
			{
				transform.position = new Vector3(bgCameraX + bgCamera.orthographicSize * bgCamera.aspect,transform.position.y, transform.position.z);
			}
		}

		if(((playerController.state == MonkeyController2D.State.running || playerController.state == MonkeyController2D.State.jumped) && playerController.GetComponent<Rigidbody2D>().velocity.x > 0.05f)  || continueMoving)
		{
			if(smooth)
			smoothMove = true;

			if(speed != startSpeed)
				speed = startSpeed;
			if(!dovoljno)
				Invoke("startSpeedDaj",0.15f);

//			if(smooth)
//			{
//				Debug.Log("Mrklj " + p);
//				transform.position = Vector3.Lerp(transform.position, new Vector3(p, transform.position.y, transform.position.z), Time.deltaTime * Mathf.Abs(speed)/2);
//			}
			//else
			{
				StopCoroutine("SmoothMovePlan");
				//if(dovoljno)
				//transform.Translate(Vector3.right*(playerController.maxSpeedX-speed)*Time.deltaTime,Space.World);
				transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x-speed, transform.position.y, transform.position.z), 5 * Time.deltaTime); 
			}
		}
		if(smoothMove && (playerController.state == MonkeyController2D.State.wallhit || playerController.state == MonkeyController2D.State.climbUp))
		{
			//continueMoving = true;
			smoothMove = false;

			StartCoroutine("SmoothMovePlan");
		}
//		if(!player.stop || continueMoving)
//			transform.Translate(Vector3.right*speed*Time.deltaTime,Space.World);

		if(FollowCameraHeight)
		{
			transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y + offset, transform.position.z);
		}
		if(IskljuciKadIzadjeIzKadra)
		{
			if(transform.position.x + 25 < Camera.main.ViewportToWorldPoint(Vector3.zero).x)
				gameObject.SetActive(false);
		}
	}

	IEnumerator SmoothMovePlan()
	{
		float targetPos = transform.position.x - 5;
		//continueMoving = false;
		float t = 0;
		while(t < 1)
		{
			yield return null;
			transform.position = Vector3.Lerp(transform.position, new Vector3(targetPos, transform.position.y, transform.position.z), t);
			//transform.Translate(Vector3.right*speed*Time.deltaTime,Space.World);
			//speed += 1f;
			t += Time.deltaTime/10;// * Mathf.Abs(speed)/25;
		}

	}

	void izracunajOffset()
	{
		offset = transform.position.y - Camera.main.transform.position.y;
		startSpeed = speed;
	}

	void startSpeedDaj()
	{
		//if(speed != startSpeed)
		//	speed = startSpeed;
		dovoljno = true;
	}
}
