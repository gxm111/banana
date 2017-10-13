using UnityEngine;
using System.Collections;

public class BossScript : MonoBehaviour {


	static BossScript instance;
	Animator anim;
	bool run = false;
	public float maxSpeedX = 16;
	float moveForce = 500;
	bool stop = false;

	public static BossScript Instance
	{
		get
		{
			if(instance != null)
				instance = GameObject.FindObjectOfType(typeof(BossScript)) as BossScript;

			return instance;
		}
	}

	void Awake () 
	{
		instance = this;
		anim = transform.Find("Boss 1").GetComponent<Animator>();
	}

	public void comeIntoTheWorld()
	{
		StartCoroutine(ShowUp());
	}

	IEnumerator ShowUp()
	{
		float t=0.05f;
		while(transform.Find("Boss 1").localPosition != new Vector3(0,-1,0))
		{
			transform.Find("Boss 1").localPosition = Vector3.Lerp(transform.Find("Boss 1").localPosition,new Vector3(0,-1,0),t);
			//t += Time.timeScale/100;
			yield return null;
			Debug.Log("AASASDAD: " + t);
		}
		anim.SetTrigger("Stomp");

		Invoke("goPlayer",2);
	}

	void goPlayer()
	{
		MonkeyController2D.Instance.state = MonkeyController2D.State.running;
		MonkeyController2D.Instance.majmun.GetComponent<Animator>().SetBool("Run",true);
		GetComponent<Rigidbody2D>().isKinematic = false;
		run = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(run)
		{
			if(GetComponent<Rigidbody2D>().velocity.x < maxSpeedX && !stop)
			{
				GetComponent<Rigidbody2D>().AddForce(Vector2.right * moveForce);
			}
			
			if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeedX && !stop)
			{
				GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeedX, GetComponent<Rigidbody2D>().velocity.y);
			}
		}
	}
}
