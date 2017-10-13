using UnityEngine;
using System.Collections;

public class BabunDogadjaji : MonoBehaviour {

	public bool IdleUdaranje;
	Animator anim;
	Animator parentAnim;
	public GameObject bureRaketa;
	public bool patrol;
	public bool jump;
	public bool fly;
	public bool shooting;
	public bool run;
	public bool runAndJump;

	public bool animateInstantly;
	BoxCollider2D[] colliders;
	
	bool fireEvent = true;
	bool canJump = false;
	bool canShoot = false;
	bool proveraJednom = true;
	bool jumpNow = false;
	bool skocioOdmah = false;
	public ParticleSystem oblak;
	MonkeyController2D player;
	Transform babun;
	Transform reqHeight;
	int idle_state = Animator.StringToHash("Base Layer.Idle");
	int idle_udaranje_state = Animator.StringToHash("Base Layer.Idle_Udaranje");
	int death_state = Animator.StringToHash("Base Layer.Death");
	int deathJump_state = Animator.StringToHash("Base Layer.DeathJump");
	int strike_state = Animator.StringToHash("Base Layer.Strike_1");
	float maxSpeedX = 15;
	public float distance;
	Vector3 baboonLocPos;
	//Manage manage;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Monkey").GetComponent<MonkeyController2D>();
		babun = transform;//.GetChild(0);
		reqHeight = transform.Find("BabunHeight");
		anim = babun.GetComponent<Animator>();
		//parentAnim = transform.parent.GetComponent<Animator>();
		colliders = GetComponents<BoxCollider2D>();
		//transform.parent.gameObject.SetActive(false);
		//parentAnim.enabled = false;
		anim.enabled = false;
		fireEvent = false;
		//manage = GameObject.Find("_GameManager").GetComponent<Manage>();
	}

	void Start()
	{
		if(!IdleUdaranje)
		{
			anim.Play(idle_state);
		}
		else
		{
			anim.Play(idle_udaranje_state);
		}
		baboonLocPos = transform.localPosition;
	}


	void Update()
	{
//		if(transform.position.x + 5 < Camera.main.ViewportToWorldPoint(Vector3.zero).x)
//		{
//			transform.parent.gameObject.SetActive(false);
//		}

		if(runAndJump)
		{
			//Debug.Log("treba skoci: " + jumpNow);
			if(transform.position.x < Camera.main.ViewportToWorldPoint(Vector3.one).x - 7.5f && !skocioOdmah)
			{
				anim.Play("Jump");
				parentAnim.Play("BaboonJumpOnce");
				jumpNow = true;
				skocioOdmah = true;
				//bureRaketa.SetActive(true);
				anim.SetBool("Land",false);
			}
		}

		if(((transform.position.x < Camera.main.ViewportToWorldPoint(Vector3.one).x + distance) || animateInstantly) && proveraJednom)
		{
			proveraJednom = false;
			//parentAnim.enabled = true;
			anim.enabled = true;
			fireEvent = true;

		}
//		if(name.Equals("BaboonReall"))
//		{
//			Debug.Log("RR: " + rigidbody2D.velocity);
//			rigidbody2D.velocity = new Vector2(10,0);
//		}
		if(patrol)
		{
			if(fireEvent)	
			{
				fireEvent = false;
				//rigidbody2D.isKinematic = false;
				//StartCoroutine(Patrol());
				anim.Play("Walking_left");
				//parentAnim.Play("BaboonPatrol");
				//rigidbody2D.velocity = new Vector2(40,0);
			}
		}
		else if(run)
		{
			if(fireEvent)
			{
				fireEvent = false;
				anim.Play("Running");
				GetComponent<Rigidbody2D>().isKinematic = false;
			}
		}
		else if(jump)
		{
			if(fireEvent)
			{
				GetComponent<Rigidbody2D>().isKinematic = true;
				fireEvent = false;
				anim.Play("Jump");
				//canJump = true;
				parentAnim.Play("BaboonJump");
			}
		}
		else if(fly)
		{
			if(fireEvent)
			{
				bureRaketa.SetActive(true);
				anim.SetBool("Land",false);
				fireEvent = false;
				GetComponent<Rigidbody2D>().isKinematic = true;
				parentAnim.Play("BaboonFly");
				anim.Play("Fly");
			}
		}
		else if(runAndJump)
		{
			if(fireEvent)
			{
				fireEvent = false;
				anim.Play("Running");
				GetComponent<Rigidbody2D>().isKinematic = false;
			}

		}
//		else if(shooting)
//		{
//			if(bullet.localPosition.y < -200 && !fireEvent)
//			{
//				bullet.localPosition = new Vector3(2,3,1);
//				canShoot = true;
//				anim.Play("Shoot");
//			}
//			if(fireEvent)
//			{
//				bullet.gameObject.SetActive(true);
//				bullet.rigidbody2D.isKinematic = false;
//				fireEvent = false;
//				rigidbody2D.isKinematic = true;
//				canShoot = true;
//				anim.Play("Shoot");
//			}
//		}
////		else if(shooting)
////		{
////			if(bullet.localPosition.y < -200 && !fireEvent)
////			{
////				bullet.localPosition = new Vector3(2,16,-78);
////				//canShoot = true;
////				anim.Play("Shoot");
////				bullet.GetComponent<Animator>().Play("Bullet_FlyLeftDown");
////			}
////			if(fireEvent)
////			{
////				bullet.gameObject.SetActive(true);
////				//bullet.rigidbody2D.isKinematic = false;
////				fireEvent = false;
////				rigidbody2D.isKinematic = true;
////				//canShoot = true;
////				anim.Play("Shoot");
////				bullet.GetComponent<Animator>().Play("Bullet_FlyLeftDown");
////			}
////		}
	}

	void FixedUpdate()
	{
		if(run || runAndJump)
		{
			//Debug.Log("Sto ne krene");
			//if(rigidbody2D.velocity.x < maxSpeedX)
			GetComponent<Rigidbody2D>().AddForce(new Vector2(-3500,0));
			if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeedX)
			{
				GetComponent<Rigidbody2D>().velocity = new Vector2(-maxSpeedX, GetComponent<Rigidbody2D>().velocity.y);
			}
		}
//		else if(canJump)
//		{
//			if(anim.GetBool("Land") == true)
//				anim.SetBool("Land",false);
//			rigidbody2D.AddForce(new Vector2(0,force));
//			Invoke("SobaliJump",0.25f);
//		}
//		if(canShoot) 
//		{
//			bullet.rigidbody2D.velocity = Vector2.zero;
//			bullet.rigidbody2D.AddForce(new Vector2(-2300,0));
//			canShoot = false;
//		}
//		if(jumpNow)
//		{
//			Debug.Log("A KOJO MOOO");
//			//rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 200);
//			//rigidbody2D.AddForce(new Vector2(0,3000));
//			rigidbody2D.AddForce(new Vector2(0,-4500));
//			jumpNow = false;
//		}

	}

	void SobaliJump()
	{
		canJump = false;
	}
	void OdvaliJump()
	{
		fireEvent = true;
		//canJump = true;
	}

	IEnumerator Patrol()
	{
		yield return null;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Monkey" && player.state != MonkeyController2D.State.wasted)
		{
			Interakcija();
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{

//		if(col.gameObject.tag == "Footer" && jump)
//		{
//			if(anim.GetBool("Land") == false)
//			{
//				anim.SetBool("Land",true);
//				Invoke("OdvaliJump",1f);
//			}
//		}
		if(runAndJump)
		{
			if(col.gameObject.tag == "Footer")
			{
				if(jumpNow)
				{
					anim.SetBool("Land",true);
					jumpNow = false;
					transform.parent = transform.parent.parent;
					anim.Play("Running");
				}
			}
		}

		if(col.gameObject.tag == "Monkey" && player.state != MonkeyController2D.State.wasted)
		{
			Debug.Log("OVDEJUSO");
			Interakcija();
		}
	}

	IEnumerator destroyBabun()
	{
		//manage.baboonSmashed++; // ZA FINALNU VERZIJU
		//manage.AddPoints(150); // ZA FINALNU VERZIJU
		transform.parent.parent.Find("+3CoinsHolder").GetComponent<Animator>().Play("FadeOutCoins");
		Manage.coinsCollected+=3;
		//StagesParser.currentMoney+=3;
		GameObject.Find("CoinsGamePlayText").GetComponent<TextMesh>().text = Manage.coinsCollected.ToString();
		yield return new WaitForSeconds(1.2f);
		//Destroy(this.gameObject);

		if(MonkeyController2D.canRespawnThings)
		{
			//transform.parent.gameObject.SetActive(false);
			//this.enabled = false;
			if(!IdleUdaranje)
			{
				anim.Play(idle_state);
			}
			else
			{
				anim.Play(idle_udaranje_state);
			}
			colliders[0].enabled = true;
			colliders[1].enabled = true;
			//parentAnim.enabled = true;
			reqHeight.GetComponent<KillTheBaboon>().turnOnColliders();
			transform.localPosition = baboonLocPos;
		}
	}

	public void killBaboonStuff()
	{
		GetComponent<Rigidbody2D>().isKinematic = true;
		if(PlaySounds.soundOn)
		PlaySounds.Play_SmashBaboon();
		anim.applyRootMotion = true;
		
		if(anim.GetBool("Land"))
		{
			anim.Play(death_state);
			//parentAnim.enabled = false;
		}
		else
		{
			jump = false;
			anim.Play(death_state);
			//parentAnim.enabled = false;
			//rigidbody2D.isKinematic = false;
			//parentAnim.Play("BaboonFallDown");
		}
		oblak.Play();
		//Debug.Log("Zgazeno");
		//rigidbody2D.isKinematic = false;
		//rigidbody2D.AddForce(new Vector2(0,600));
		//collider2D.enabled = false;
		//colliders[0].enabled = false;
		//colliders[1].enabled = false;
		player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.maxSpeedX,0);// Vector2.zero;
		//player.rigidbody2D.AddForce(new Vector2(0, player.jumpForce - rigidbody2D.velocity.y*player.doubleJumpForce));
		player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1500));
		player.GetComponent<Rigidbody2D>().drag = 0;
		player.canGlide = false;
		player.animator.Play(player.jump_State);
		StartCoroutine(destroyBabun());
	}

	void Interakcija()
	{
//		if(player.transform.position.y + 1.25f > reqHeight.position.y)
//		{
//
//		}
//		else
//		if(player.transform.position.y + 1.25f < reqHeight.position.y && player.activeShield)
	//	Debug.Log("Stiti: " + player.activeShield);
		if(player.activeShield)
		{
			Debug.Log("Stitulj");
			reqHeight.GetComponent<KillTheBaboon>().turnOffColliders();
			GetComponent<Rigidbody2D>().isKinematic = true;
			if(PlaySounds.soundOn)
			PlaySounds.Play_SmashBaboon();
			
			anim.applyRootMotion = true;
			if(anim.GetBool("Land"))
			{
				anim.Play(death_state);
				//parentAnim.enabled = false;
			}
			else
			{
				jump = false;
				anim.Play(death_state);
			}
			oblak.Play();
			//collider2D.enabled = false;
			colliders[0].enabled = false;
			colliders[1].enabled = false;
			player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.maxSpeedX,0);;
			//player.rigidbody2D.AddForce(new Vector2(0, player.jumpForce - rigidbody2D.velocity.y*player.doubleJumpForce));
			if(player.activeShield)
			{
				player.activeShield = false;
				GameObject.Find("_GameManager").SendMessage("ApplyPowerUp",-3);
				if(player.state != MonkeyController2D.State.running)
				{
					player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1500));
					player.GetComponent<Rigidbody2D>().drag = 0;
					player.canGlide = false;
					player.animator.Play(player.jump_State);
				}
			}
			else
				player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1500));

			StartCoroutine(destroyBabun());
		}
		else if(!player.killed)
		{
			Debug.Log("nema stitulj");
			if(PlaySounds.soundOn)
			PlaySounds.Play_SmashBaboon();
			//anim.applyRootMotion = true;
			player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			player.killed = true;
			maxSpeedX = 0;
			run = false;
			//collider2D.enabled = false;
			//reqHeight.GetComponent<KillTheBaboon>().turnOffColliders();
			//colliders[0].enabled = false;
			//colliders[1].enabled = false;
			//if(!jump)
			//	parentAnim.enabled = false;
			GetComponent<Rigidbody2D>().isKinematic = true;
			if(anim.GetBool("Land"))
			{
				anim.Play(strike_state);
			}
			else
			{
				//parentAnim.enabled = true;
			}
			oblak.Play();
			//collider2D.enabled = false;
			//colliders[0].enabled = false;
			//colliders[1].enabled = false;
			//transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
			//Debug.Log("Utepan");
			if(player.state == MonkeyController2D.State.running)
				player.majmunUtepan();
			else
				player.majmunUtepanULetu();
		}
	}
	
}
