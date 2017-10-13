using UnityEngine;
using System.Collections;

public class BabunDogadjaji_new : MonoBehaviour {

	public bool IdleUdaranje;
	Animator anim;
	Animator parentAnim;
	public GameObject bureRaketa;
	public Transform senka;
	public bool patrol;
	public bool jump;
	public bool fly;
	public bool shooting;
	public bool run;
	public bool runAndJump;

	public bool animateInstantly;
	CircleCollider2D[] colliders;
	
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
	public float maxSpeedX = 17;
	public float distance;
	Vector3 baboonLocPos;

	Vector2 pravac;
	Vector2 pravacFly;

	bool patrolinjo = false;
	bool flyinjo = false;
	bool runinjo = false;
	Rigidbody2D parentRigidbody2D;
	Transform baboonShadow;
	RaycastHit2D hit;
	public bool canDo;
	public Vector3 baboonRealOrgPos;
	bool kontrolaZaBrzinuY = false;
	float smanjivac;
	//Manage manage;
	bool ugasen = true;
	bool impact = false;
	TextMesh coinsCollectedText;

	//Dodatak za gorilu
	public GameObject Koplje;
	public GameObject Boomerang;
	public bool koplje;
	public bool udaranjePoGrudi;

	public bool boomerang;
	bool isGorilla = false;
	int kolicinaPoena = 0;
	bool runTurnedOff = false;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Monkey").GetComponent<MonkeyController2D>();
		coinsCollectedText = GameObject.Find("CoinsGamePlayText").GetComponent<TextMesh>();
		babun = transform;//.GetChild(0);
		reqHeight = transform.parent.Find("_BabunNadrlja");
		anim = babun.parent.GetComponent<Animator>();
		//parentAnim = transform.parent.GetComponent<Animator>();
		colliders = GetComponents<CircleCollider2D>();
		//transform.parent.gameObject.SetActive(false);
		//parentAnim.enabled = false;
		//anim.enabled = false;
		fireEvent = false;
		//manage = GameObject.Find("_GameManager").GetComponent<Manage>();
		parentRigidbody2D = transform.parent.GetComponent<Rigidbody2D>();
		//canDo = false;
		//this.enabled = false;
		pravac = -Vector2.right;
		pravacFly = Vector2.up;
		
		if(runAndJump)
			baboonShadow = transform.parent.Find("shadow");
		baboonRealOrgPos = transform.parent.localPosition;

		if(transform.parent.parent.name.Contains("Gorilla"))
		{
			isGorilla = true;
		}
		PogasiBabuna();
	}

	void Start()
	{
//		if(!IdleUdaranje)
//		{
//			anim.Play(idle_state);
//		}
//		else
//		{
//			anim.Play(idle_udaranje_state);
//		}
		//baboonLocPos = transform.localPosition;



	}


	void Update()
	{
		hit = Physics2D.Linecast(transform.position + new Vector3(0.8f,1f,0), transform.position + new Vector3(0.8f,-35.5f,0), 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Platform"));
		if(hit)
			senka.position = new Vector3(senka.position.x,hit.point.y-0f,senka.position.z);
		//if(canDo)
		{
			if(transform.position.x + 5 < Camera.main.ViewportToWorldPoint(Vector3.zero).x && !ugasen)
			{
				ugasen = true;
				PogasiBabuna();
				//transform.parent.position = baboonRealOrgPos;
				//anim.enabled = false;
				//this.enabled = false;


			}
			if(runAndJump)
			{
				//Debug.Log("treba skoci: " + jumpNow);
				if(transform.position.x < Camera.main.ViewportToWorldPoint(Vector3.one).x - 30f && !skocioOdmah)
				{
					anim.Play("Jump");
					//parentAnim.Play("BaboonJumpOnce");
					parentRigidbody2D.velocity = new Vector2(-maxSpeedX*0.7f,0);
					parentRigidbody2D.velocity = new Vector2(-maxSpeedX*0.7f,43f); 
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
				ugasen = false;
			}
			if(transform.parent.name.Equals("BaboonRealll"))
			{
				//Debug.Log("RR: " + parentRigidbody2D.velocity);
	//			rigidbody2D.velocity = new Vector2(10,0);
			}
			if(patrol)
			{
				if(fireEvent)	
				{
					fireEvent = false;
					parentRigidbody2D.isKinematic = false;
					//StartCoroutine(Patrol());
					anim.Play("Walking_left");
					//parentAnim.Play("BaboonPatrol");
					//rigidbody2D.velocity = new Vector2(40,0);

					patrolinjo = true;
					InvokeRepeating("ObrniSe",2.65f,2.65f);
				}
			}
			else if(run)
			{
				if(fireEvent)
				{
					fireEvent = false;
					anim.Play("Running");
					parentRigidbody2D.isKinematic = false;

					runinjo = true;
				}
			}
			else if(jump)
			{
				if(fireEvent)
				{
					//parentRigidbody2D.isKinematic = false;
					fireEvent = false;
					anim.Play("Jump2");
					anim.SetBool("Land",false);
					anim.applyRootMotion = true;
					//parentRigidbody2D.velocity = new Vector2(0,30);
					//canJump = true;
					//parentAnim.Play("BaboonJump");
				}
			}
			else if(fly)
			{
				if(fireEvent)
				{
					bureRaketa.SetActive(true);
					anim.SetBool("Land",false);
					fireEvent = false;
					//rigidbody2D.isKinematic = true;
					//parentAnim.Play("BaboonFly");
					anim.Play("Fly");

					flyinjo = true;
					InvokeRepeating("ObrniSeVertikalno",1.5f,1.5f);
				}
			}
			else if(runAndJump)
			{
				if(fireEvent)
				{
					fireEvent = false;
					anim.Play("Running");
					parentRigidbody2D.isKinematic = false;

					runinjo = true;
				}

			}
			else if(IdleUdaranje)
			{
				if(fireEvent)
				{
					fireEvent = false;
					int sansa = Random.Range(1,3);
					if(sansa > 1)
						anim.Play(idle_udaranje_state);
					else
						anim.Play(idle_state);
				}
			}
			else if(udaranjePoGrudi)
			{
				if(fireEvent)
				{
					fireEvent = false;
					anim.Play("Chest_drum");
				}
			}
			else if(koplje)
			{
				if(fireEvent)
				{
					fireEvent = false;
					anim.Play("koplje");
				}
			}
			else if(boomerang)
			{
				if(fireEvent)
				{
					fireEvent = false;
					anim.Play("Boomerang");
				}
			}
//			else if(shooting)
//			{
//				if(bullet.localPosition.y < -200 && !fireEvent)
//				{
//					bullet.localPosition = new Vector3(2,3,1);
//					canShoot = true;
//					anim.Play("Shoot");
//				}
//				if(fireEvent)
//				{
//					bullet.gameObject.SetActive(true);
//					bullet.rigidbody2D.isKinematic = false;
//					fireEvent = false;
//					rigidbody2D.isKinematic = true;
//					canShoot = true;
//					anim.Play("Shoot");
//				}
//			}
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
	}


	void FixedUpdate()
	{
		if(patrolinjo)
		{
			//parentRigidbody2D.MovePosition(parentRigidbody2D.position + new Vector2(pravac.x,-5)*Time.deltaTime*5);
			parentRigidbody2D.velocity = /*pravac.x*5*parentRigidbody2D.velocity.normalized;*/new Vector2(pravac.x*5,parentRigidbody2D.velocity.y);
			//if(parentRigidbody2D.velocity.x > pravac.x*5)
			//	parentRigidbody2D.velocity = new Vector2(pravac.x*6.75f,-2);
			//else if(parentRigidbody2D.velocity.x < pravac.x*5)
			//	parentRigidbody2D.velocity = new Vector2(pravac.x*4.65f,-2);
			if(parentRigidbody2D.velocity.y > 2)
				parentRigidbody2D.velocity = new Vector2(pravac.x*6.25f,parentRigidbody2D.velocity.y);
			else if(parentRigidbody2D.velocity.y < -2)
				parentRigidbody2D.velocity = new Vector2(pravac.x*3.75f,parentRigidbody2D.velocity.y);
		}

		else if(flyinjo)
		{
			transform.parent.Translate(pravacFly * Time.deltaTime * 3);
		}

		else if((run || runAndJump) && runinjo)
		{
			//Debug.Log("Sto ne krene");
			//if(rigidbody2D.velocity.x < maxSpeedX)
			//parentRigidbody2D.AddForce(new Vector2(-3500,0));
			//if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeedX)
			{
				if(jumpNow)
				{
					parentRigidbody2D.velocity = new Vector2(-maxSpeedX*0.45f, parentRigidbody2D.velocity.y);
				}
				else
				{
					parentRigidbody2D.velocity = new Vector2(-maxSpeedX, parentRigidbody2D.velocity.y);
					//parentRigidbody2D.velocity = new Vector2(-17f, parentRigidbody2D.velocity.y);
				}

				if(runAndJump)
				{
					hit = Physics2D.Linecast(transform.position + new Vector3(0.8f,1f,0), transform.position + new Vector3(0.8f,-35.5f,0), 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Platform"));
					if(hit/* && senka.position.y < transform.position.y + 2f*/)
					{
						baboonShadow.position = new Vector3(baboonShadow.position.x,hit.point.y+0.3f,baboonShadow.position.z);
					}
				}

//				if(kontrolaZaBrzinuY)
//				{
//					Debug.Log("smanjivac: " + smanjivac);
//					parentRigidbody2D.velocity = new Vector2(parentRigidbody2D.velocity.x, smanjivac--);
//				}

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

	void ObrniSe()
	{
		pravac = -pravac;
		//Debug.Log("p: " + pravac);
		anim.SetBool("changeSide",!anim.GetBool("changeSide"));
	}

	void ObrniSeVertikalno()
	{
		pravacFly = -pravacFly;
		//Debug.Log("p: " + pravacFly);
		//anim.SetBool("changeSide",!anim.GetBool("changeSide"));
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
		else if(col.name == "Impact")
		{
			impact = true;
			reqHeight.GetComponent<KillTheBaboon>().turnOffColliders();
			killBaboonStuff();
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
					//transform.parent = transform.parent.parent;
					anim.Play("Running");
				}
			}
		}

		if(col.gameObject.tag == "Monkey" && player.state != MonkeyController2D.State.wasted)
		{
			Interakcija();
		}
	}

	IEnumerator destroyBabun()
	{
		if(isGorilla)
		{
			Manage.gorillasKilled++;
			MissionManager.Instance.GorillaEvent(Manage.gorillasKilled);
			kolicinaPoena = 40;
			if(fly)
			{
				Manage.fly_GorillasKilled++;
				MissionManager.Instance.Fly_GorillaEvent(Manage.fly_GorillasKilled);
				kolicinaPoena = 60;
			}
			else if(koplje)
			{
				Manage.koplje_GorillasKilled++;
				MissionManager.Instance.Koplje_GorillaEvent(Manage.koplje_GorillasKilled);
				kolicinaPoena = 70;
				Koplje.GetComponent<Collider2D>().enabled = false;
			}
			else if(jump)
			{
				kolicinaPoena = 50;
			}
			else if(patrol)
			{
				kolicinaPoena = 40;
			}
			else if(run)
			{
				kolicinaPoena = 70;
			}
			else if(runAndJump)
			{
				kolicinaPoena = 80;
			}
			
		}
		else
		{

			Manage.baboonsKilled++;
			MissionManager.Instance.BaboonEvent(Manage.baboonsKilled);
			kolicinaPoena = 40;
			if(fly)
			{
				Manage.fly_BaboonsKilled++;
				MissionManager.Instance.Fly_BaboonEvent(Manage.fly_BaboonsKilled);
				kolicinaPoena = 60;
			}
			else if(boomerang)
			{
				Manage.boomerang_BaboonsKilled++;
				MissionManager.Instance.Boomerang_BaboonEvent(Manage.boomerang_BaboonsKilled);
				kolicinaPoena = 70;
				//Boomerang.collider2D.enabled = false;
				Boomerang.SetActive(false);
			}
			else if(jump)
			{
				kolicinaPoena = 50;
			}
			else if(patrol)
			{
				kolicinaPoena = 40;
			}
			else if(run)
			{
				kolicinaPoena = 70;
			}
			else if(runAndJump)
			{
				kolicinaPoena = 80;
			}
		}
		//manage.baboonSmashed++; // ZA FINALNU VERZIJU
		//manage.AddPoints(150); // ZA FINALNU VERZIJU
		if(fly || run)
		{
			senka.GetComponent<Renderer>().enabled = false;
		}

		if(PlaySounds.soundOn)
			PlaySounds.Play_SmashBaboon();

		int value = 3;
		if(jump)		value = 4;
		else if(fly)	value = 4;
		else if(patrol)	value = 3;
		else if(run)	value = 4;
		else if(runAndJump) value = 5;
		else if(boomerang || koplje) value = 6;

		Transform ThreeCoinsHolder = transform.parent.Find("+3CoinsHolder");

		ThreeCoinsHolder.Find("+3Coins").GetComponent<TextMesh>().text = ThreeCoinsHolder.Find("+3Coins/+3CoinsShadow").GetComponent<TextMesh>().text = "+"+value;
		ThreeCoinsHolder.parent = transform.parent.parent; //OVDE IZBACIO NULL REFERENCE EXCEPTION
		//"U RED IZNAD IZBACIO NULL REFERENCE ZA JUMP GORILLU, A DRUGI PUT SAM GA UBIO SLIDE-OM ALI JE I MAJMUNCE POGINUO, VEROVATNO ZATO STO JE OSTAO ONAJ 3 COLLIDER VISKA U PREFAB-U, TREBA DA SE PROVERI!!!!!"
		//"ZA +3COINS JE SVUDA MISSING SPRITE, ILI DA SE UBACI NOVI ILI DA SE ZAMENI FONTOM, PA DA MU SE DAJE NA RANDOM OD 3 DO 6 NOVCICA RECIMO, S TIM STO TREBA DA SE PROVERI DA LI CE RADITI ANIMACIJA ZA FONT!!!!!"
		ThreeCoinsHolder.GetComponent<Animator>().Play("FadeOutCoins");
		Manage.coinsCollected+=value;
		//StagesParser.currentMoney+=3;
		MissionManager.Instance.CoinEvent(Manage.coinsCollected);
		coinsCollectedText.text = Manage.coinsCollected.ToString();
		coinsCollectedText.GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		Manage.points+=kolicinaPoena;
		Manage.pointsText.text = Manage.points.ToString();
		Manage.pointsEffects.RefreshTextOutline(false,true);
		yield return new WaitForSeconds(1.2f);
		transform.parent.parent.Find("+3CoinsHolder").parent = transform.parent;

		//Destroy(this.gameObject);

//		if(MonkeyController2D.canRespawnThings)
//		{
//			//transform.parent.gameObject.SetActive(false);
//			//this.enabled = false;
//			anim.enabled = false;
//			colliders[0].enabled = true;
//			colliders[1].enabled = true;
//			//collider2D.enabled = true;
//			//parentAnim.enabled = true;

		//	@@@@@@ ZASTO JE OVO BILO UKLJUCENO ISPOD?
		//	reqHeight.GetComponent<KillTheBaboon>().turnOnColliders();

//			transform.localPosition = baboonLocPos;
//		}
	}

	public void killBaboonStuff()
	{
		//mozda da se ovo ubaci dole ili da se skloni odavde
		if(player.powerfullImpact)
			player.cancelPowerfullImpact();
		if(patrolinjo)
			patrolinjo = false;
		else if(flyinjo)
			flyinjo = false;
		else if(runinjo)
			runinjo = false;
		if(parentRigidbody2D != null)
			parentRigidbody2D.isKinematic = true;
		anim.applyRootMotion = true;
		
		if(anim.GetBool("Land"))
		{
			if(!runAndJump)
				anim.Play(death_state);
			else
			{
				anim.Play(deathJump_state);
				Invoke("UgasiBabunaPoslePada",1f);
			}
			//parentAnim.enabled = false;

		}
		else
		{
			//jump = false;
			anim.Play(deathJump_state);
			Invoke("UgasiBabunaPoslePada",1f);
			//parentAnim.enabled = false;
			//rigidbody2D.isKinematic = false;
			//parentAnim.Play("BaboonFallDown");
		}
		oblak.Play();
		//Debug.Log("Zgazeno");
		//rigidbody2D.isKinematic = false;
		//rigidbody2D.AddForce(new Vector2(0,600));
		//collider2D.enabled = false;
		colliders[0].enabled = false;
		colliders[1].enabled = false;
		//colliders[2].enabled = false;
		//reqHeight.GetComponent<KillTheBaboon>().turnOffColliders();
		//collider2D.enabled = false;
		if(!impact)
		{
			//player.rigidbody2D.velocity = new Vector2(player.maxSpeedX,0);// Vector2.zero;
			//player.rigidbody2D.isKinematic = true;
			//player.rigidbody2D.AddForce(new Vector2(0, player.jumpForce - rigidbody2D.velocity.y*player.doubleJumpForce));
			StartCoroutine(bounceOffEnemy());

			//player.rigidbody2D.AddForce(new Vector2(0, 1500));
			//player.animator.Play(player.jump_State);
		}
		else
		{
			impact = false;
		}
		player.GetComponent<Rigidbody2D>().drag = 0;
		player.canGlide = false;
		StartCoroutine(destroyBabun());
	}

	IEnumerator bounceOffEnemy()
	{
		//yield return new WaitForFixedUpdate();
		yield return new WaitForFixedUpdate();
		if(!player.isSliding)
		{
			player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.maxSpeedX,0);
			player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1500));
			player.animator.Play(player.jump_State);
		}
		else
		{
			if(player.state != MonkeyController2D.State.running)
			{
				player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.maxSpeedX,0);
				player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -2500));
				//player.isSliding = false;
				//player.animator.Play(player.jump_State);
			}
		}

		//yield return new WaitForSeconds(0.25f);

	}

	void Interakcija()
	{
		if(patrolinjo)
			patrolinjo = false;
		else if(flyinjo)
			flyinjo = false;
//		if(player.transform.position.y + 1.25f > reqHeight.position.y)
//		{
//
//		}
//		else
//		if(player.transform.position.y + 1.25f < reqHeight.position.y && player.activeShield)
	//	Debug.Log("Stiti: " + player.activeShield);
		if(player.activeShield || player.invincible || player.powerfullImpact)
		{
			if(runinjo)
				runinjo = false;
			reqHeight.GetComponent<KillTheBaboon>().turnOffColliders();
			if(parentRigidbody2D != null)
				parentRigidbody2D.isKinematic = true;
			
			anim.applyRootMotion = true;
			if(anim.GetBool("Land"))
			{
				anim.Play(death_state);
				//parentAnim.enabled = false;
			}
			else
			{
				//jump = false;
				anim.Play(deathJump_state);
				Invoke("UgasiBabunaPoslePada",1f);
			}
			oblak.Play();
			//collider2D.enabled = false;
			colliders[0].enabled = false;
			colliders[1].enabled = false;
			//colliders[2].enabled = false;
			player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.maxSpeedX,0);
			//player.rigidbody2D.AddForce(new Vector2(0, player.jumpForce - rigidbody2D.velocity.y*player.doubleJumpForce));
			if(player.activeShield)
			{
				if(PlaySounds.soundOn)
					PlaySounds.Play_LooseShield();
				player.activeShield = false;
				player.transform.Find("Particles/ShieldDestroyParticle").GetComponent<ParticleSystem>().Play();
				player.transform.Find("Particles/ShieldDestroyParticle").GetChild(0).GetComponent<ParticleSystem>().Play();
				GameObject.Find("_GameManager").SendMessage("ApplyPowerUp",-3);
				if(player.state != MonkeyController2D.State.running)
				{
					//player.rigidbody2D.AddForce(new Vector2(0, 1500));
					player.GetComponent<Rigidbody2D>().drag = 0;
					player.canGlide = false;
					player.animator.Play(player.jump_State);
				}
			}
			else if(!player.isSliding)
			{
				if(player.state != MonkeyController2D.State.running)
					player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1500));
			}

			StartCoroutine(destroyBabun());

		}
		else if(!player.killed)
		{
			if(PlaySounds.soundOn)
			PlaySounds.Play_SmashBaboon();
			//anim.applyRootMotion = true;
			player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			player.killed = true;

			//if(runinjo)
			//	runinjo = false;
			if(run)
			{
				run = false;
				runTurnedOff = true;
			}

			//collider2D.enabled = false;
			reqHeight.GetComponent<KillTheBaboon>().turnOffColliders();

			//collider2D.enabled = false;
			//if(!jump)
			//	parentAnim.enabled = false;

			colliders[0].enabled = false;
			colliders[1].enabled = false;
			//colliders[2].enabled = false;
			if(anim.GetBool("Land"))
			{
				if(!run)
				{
					anim.Play(strike_state);
					if(parentRigidbody2D != null)
						parentRigidbody2D.isKinematic = true;

					maxSpeedX = 0;
				}

			}
			else
			{
				Invoke("UkljuciCollidereOpet",0.35f);
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

			kontrolaZaBrzinuY = true;
			//smanjivac = parentRigidbody2D.velocity.y;
			//Invoke("resetujKontroluZaBrzinu",1.5f);
		}
	}

	void UkljuciCollidereOpet()
	{
		colliders[0].enabled = true;
		colliders[1].enabled = true;
		//colliders[2].enabled = true;
	}

	void resetujKontroluZaBrzinu()
	{
		kontrolaZaBrzinuY = false;
	}

	void PogasiBabuna()
	{
		if(fly)
		{
			if(bureRaketa.activeSelf)
				bureRaketa.SetActive(false);
			CancelInvoke("ObrniSeVertikalno");
		}
		else if(patrol)
		{
			CancelInvoke("ObrniSe");
			anim.Play("New State");
			anim.SetBool("changeSide",false);
			pravac = -Vector2.right;
		}
		else if(jump)
		{
			anim.Play("New State");
		}

		//Debug.Log("pogasi " + transform.parent.parent.name);
		if(parentRigidbody2D != null)
			parentRigidbody2D.isKinematic = true;
		colliders[0].enabled = false;
		colliders[1].enabled = false;
		//colliders[2].enabled = false;

		transform.parent.Find("Baboon").GetComponent<Renderer>().enabled = false;
		anim.enabled = false;
		patrolinjo = false;
		flyinjo = false;
		runinjo = false;
		anim.applyRootMotion = false;
		//pravac = -Vector2.right;
		//pravacFly = Vector2.up;
		this.enabled = false;

	}

	void UgasiBabunaPoslePada()
	{
		oblak.Play();
		transform.parent.Find("Baboon").GetComponent<Renderer>().enabled = false;
		if(fly)
		{
			bureRaketa.SetActive(false);
		}
	}

	public void ResetujBabuna()
	{
		//Debug.Log("resetuj " + transform.parent.parent.name);
		transform.parent.localPosition = baboonRealOrgPos;
		colliders[0].enabled = true;
		colliders[1].enabled = true;
		//colliders[2].enabled = true;
		reqHeight.GetComponent<KillTheBaboon>().turnOnColliders();
		transform.parent.Find("Baboon").GetComponent<Renderer>().enabled = true;
		anim.enabled = true;
		this.enabled = true;
		ugasen = false;
		proveraJednom = true;
		skocioOdmah = false;
		if(fly)
			bureRaketa.SetActive(true);
		if(boomerang)
			Boomerang.SetActive(true);
			//Boomerang.collider2D.enabled = true;
		if(koplje)
			Koplje.GetComponent<Collider2D>().enabled = true;

		if(runTurnedOff)
		{
			runTurnedOff = false;
			run = true;
		}
		if(fly || run)
		{
			senka.GetComponent<Renderer>().enabled = true;
		}

		//if(transform.parent.parent.name.Contains("Run") && !transform.parent.parent.name.Contains("Jump"))
		//	run = true;
		maxSpeedX = 17;
	}
}
