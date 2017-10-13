using UnityEngine;
using System.Collections;

public class MonkeyController2D : MonoBehaviour {
	
	// Use this for initialization
	public float moveForce 			= 300f;
	public float maxSpeedX 			= 8f;
	public float jumpForce 			= 700f;
	public float doubleJumpForce 	= 100f;
	public float gravity 			= 200f;
	public float maxSpeedY			= 8f;
	public float jumpSpeedX 		= 12f;
	
	bool jump 						= false;
	bool doubleJump 				= false;
	[HideInInspector]
	public bool inAir				= false;
	bool hasJumped					= false;
	[HideInInspector]
	public bool zgaziEnemija		= false;
	[HideInInspector]
	public bool killed				= false;
	
	[HideInInspector]
	public bool stop				= false;
	[HideInInspector]
	public bool triggerCheckDownTrigger	= false;
	[HideInInspector]
	public bool triggerCheckDownBehind		= false;
	
	bool CheckWallHitNear			= false;
	bool CheckWallHitNear_low		= false;
	
	bool startSpustanje				= false;
	bool startPenjanje				= false;
	
	float pocetniY_spustanje;
	
	[HideInInspector]
	public float collisionAngle			= 0;
	float duzinaPritiskaZaSkok;
	bool mozeDaSkociOpet			= false;
	
	float startY;
	float endX;
	float endY;
	bool swoosh;
	Vector3 colliderForClimb;
	
	public bool Glide;
	public bool DupliSkok;
	public bool KontrolisaniSkok;
	public bool SlideNaDole;
	public bool Zaustavljanje;
	
	Ray2D ray;
	RaycastHit2D hit;

	Transform ceilingCheck;
	Transform groundCheck;
	[HideInInspector] public Transform majmun;
	public ParticleSystem trava;
	public ParticleSystem oblak;
	public ParticleSystem particleSkok;
	//public ParticleSystem dupliSkokEfekat;
	//public ParticleSystem dupliSkokEfekat2;
	public ParticleSystem dupliSkokOblaci;
	public ParticleSystem runParticle;
	public ParticleSystem klizanje;
	public ParticleSystem izrazitiPad;
	public Transform zutiGlowSwooshVisoki;
	public ParticleSystem collectItem;
	Transform whatToClimb;
	float currentSpeed;
	[HideInInspector]
	public GameObject cameraTarget;
	[HideInInspector]
	public GameObject cameraTarget_down;
	float cameraTarget_down_y;
	CameraFollow2D_new cameraFollow;
	
	[HideInInspector]
	public enum State { running, jumped, wallhit, climbUp, actualClimbing, wasted, idle, completed, lijana, saZidaNaZid, preNegoDaSeOdbije };
	
	//[HideInInspector]
	public State state;// = State.running;
	
	[HideInInspector]
	public float startSpeedX;
	[HideInInspector]
	public float startJumpSpeedX;
	
	public bool neTrebaDaProdje = false;
	
	[HideInInspector]
	public Animator animator;
	//Animator parentAnim;
	
	[HideInInspector]
	public string lastPlayedAnim;
	bool helpBool;

	AnimatorStateInfo currentBaseState;
	[HideInInspector] public int run_State = Animator.StringToHash("Base Layer.Running");
	[HideInInspector] public int jump_State = Animator.StringToHash("Base Layer.Jump_Start");
	[HideInInspector] public int fall_State = Animator.StringToHash("Base Layer.Jump_Falling");
	[HideInInspector] public int landing_State = Animator.StringToHash("Base Layer.Landing");
	[HideInInspector] public int doublejump_State = Animator.StringToHash("Base Layer.DoubleJump_Start");
	[HideInInspector] public int glide_start_State = Animator.StringToHash("Base Layer.Glide_Start");
	[HideInInspector] public int glide_loop_State = Animator.StringToHash("Base Layer.Glide_Loop");
	[HideInInspector] public int grab_State = Animator.StringToHash("Base Layer.Grab");
	[HideInInspector] public int wall_stop_State = Animator.StringToHash("Base Layer.WallStop");
	[HideInInspector] public int wall_stop_from_jump_State = Animator.StringToHash("Base Layer.WallStop_From_Jump");
	[HideInInspector] public int swoosh_State = Animator.StringToHash("Base Layer.Swoosh_Down");
	[HideInInspector] public int spikedeath_State = Animator.StringToHash("Base Layer.Spike_death");
	[HideInInspector] public int lijana_State = Animator.StringToHash("Base Layer.Lijana");
	
	Transform lookAtPos;
	float lookWeight = 0;
	bool disableGlide = false;
	bool usporavanje = false;
	[HideInInspector]
	public bool lijana = false;
	Transform grabLianaTransform;
	[HideInInspector]
	public bool heCanJump = true;
	bool saZidaNaZid = false;
	
	int povrsinaZaClick = 0;
	bool jumpControlled = false;
	float tempForce;
	
	[HideInInspector]
	public bool activeShield = false;
	
	float razmrk;
	Vector3 pocScale;
	Transform senka;
	
	public static bool canRespawnThings = true;
	
	[SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character
	
	float groundedRadius = 0.2f;	
	[HideInInspector] public bool grounded = false;
	float korakce;
	[HideInInspector] public bool canGlide = false;
	int proveraGround = 16;
	bool jumpHolding = false;
	TrailRenderer trail;
	[HideInInspector] public bool powerfullImpact = false;
	public float trailTime = 0.5f;
	public bool invincible = false;
	bool utepanULetu = false;
	public bool magnet = false;
	public bool doublecoins = false;
	Manage manage;
	public bool measureDistance = false;
	public float distance = 0;
	float startPosX = 0;
	public bool misijaSaDistance = false;
	bool pustenVoiceJump = false;
	Camera guiCamera;
	[HideInInspector] public bool isSliding = false;
	Collider2D enemyCollider = null;
	[HideInInspector] public float originalCameraTargetPosition;
	bool podigniMaloKameru = false;
	bool dancing = false;
	public PhysicsMaterial2D finishDontMove;
	[HideInInspector] public bool mushroomJumped = false;

	//@@@@@@ DODATAK ZA SA ZIDA NA ZID
	[HideInInspector] public bool wallHitGlide = false;

	static MonkeyController2D instance;
	
	public static MonkeyController2D Instance
	{
		get
		{
			if(instance == null)
				instance = FindObjectOfType(typeof(MonkeyController2D)) as MonkeyController2D;
			
			return instance;
		}
	}

	void Awake()
	{
		majmun = GameObject.Find("PrinceGorilla").transform;
		animator = majmun.GetComponent<Animator>();
		//Application.targetFrameRate = 36;
		cameraTarget = transform.Find("PlayerFocus2D").gameObject;
		cameraTarget_down = transform.Find("PlayerFocus2D_down").gameObject;
		cameraFollow = Camera.main.transform.parent.GetComponent<CameraFollow2D_new>();
		lookAtPos = transform.Find("LookAtPos");
		Input.multiTouchEnabled = true;
		//parentAnim = majmun.parent.GetComponent<Animator>();
		//#if UNITY_EDITOR
		//QualitySettings.vSyncCount = 2;
		//#endif
		manage = GameObject.Find("_GameManager").GetComponent<Manage>();
		guiCamera = GameObject.Find("GUICamera").GetComponent<Camera>();
		instance = this;
	}
	
	void Start ()
	{
//		if(StagesParser.maska)
//		{
//			customizable.gameObject.SetActive(true);
//			//customizable2.gameObject.SetActive(true);
//			majmun.Find("Kosa").gameObject.SetActive(false);
//		}

		//DA SE PREPRAVI!!!
		if(!StagesParser.imaKosu)
		{
			majmun.Find("Kosa").gameObject.SetActive(false);
		}
		if(!StagesParser.imaUsi)
		{
			majmun.Find("Usi").gameObject.SetActive(false);
		}
		if(StagesParser.glava != -1)
		{
			majmun.Find("ROOT/Hip/Spine/Chest/Neck/Head/"+StagesParser.glava).GetChild(0).gameObject.SetActive(true);
		}
		if(StagesParser.majica != -1)
		{
			majmun.Find("custom_Majica").gameObject.SetActive(true);
			Texture MajicaTekstura = Resources.Load("Majice/Bg"+StagesParser.majica) as Texture;
			majmun.Find("custom_Majica").GetComponent<Renderer>().material.SetTexture("_MainTex", MajicaTekstura);
			majmun.Find("custom_Majica").GetComponent<Renderer>().material.color = StagesParser.bojaMajice;
		}
		if(StagesParser.ledja != -1)
		{
			majmun.Find("ROOT/Hip/Spine/"+StagesParser.ledja).GetChild(0).gameObject.SetActive(true);
		}

		startSpeedX = maxSpeedX;
		startJumpSpeedX = jumpSpeedX;
		state = State.idle;
		Resources.UnloadUnusedAssets();
		cameraTarget_down.transform.position = cameraTarget.transform.position;
		
		currentBaseState = animator.GetCurrentAnimatorStateInfo(0);
		animator.speed = 1.5f;
		//animator.speed = 2;
		animator.SetLookAtWeight(lookWeight);
		animator.SetLookAtPosition(lookAtPos.position);
		tempForce = jumpForce;
		senka = GameObject.Find("shadowMonkey").transform;
		canRespawnThings = true;
		groundCheck = transform.Find("GroundCheck");
		ceilingCheck = transform.Find("CeilingCheck");
		trail = transform.Find("Trail").GetComponent<TrailRenderer>();
		startPosX = Camera.main.transform.position.x;
		
		//RaycastHit2D hit = Physics2D.Linecast(transform.position + new Vector3(0.8f,2.5f,0), transform.position + new Vector3(0.8f,-15.5f,0), 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Platform"));
		//Debug.DrawLine(transform.position + new Vector3(0.8f,2.5f,0), transform.position + new Vector3(0.8f,-5.5f,0),Color.white);
		//GameObject.Find("shadowMonkey").transform.position = new Vector3(GameObject.Find("shadowMonkey").transform.position.x,hit.point.y,GameObject.Find("shadowMonkey").transform.position.z);
		//razmrk = transform.position.y - GameObject.Find("shadowMonkey").transform.position.y;
		originalCameraTargetPosition = cameraTarget.transform.localPosition.y;
	}

	//"DA SE ZAMENE SLIKE ZA BANKU, BROJ ZVEZDICA NA OSTRVU I LIKE NA FREE COINS U TIPS & TRICKS!!!!!"
	//"DA SE VIDI DA MAJMUNCE NE SKACE KAD U SKOKU ZAKACI BILJKU ILI BUMERANG ILI BILO STA STO SKIDA SHIELD, DA NE DOBIJE VELIKI BOOST!!!!!"
	//"DA SE ZAVRSI BONUS NIVO!!!!!"
	//"DA SE UBACE REKLAME SVUDA GDE TREBA!!!!!"
	
	void Update()
	{																//2.5f
		hit = Physics2D.Linecast(transform.position + new Vector3(0.8f,0f,0), transform.position + new Vector3(0.8f,-15.5f,0), 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Platform"));
		if(hit/* && senka.position.y < transform.position.y + 2f*/)
		{
			senka.position = new Vector3(senka.position.x,hit.point.y-0.3f,senka.position.z);
			senka.localScale = Vector3.one;
			senka.rotation = Quaternion.Euler(0,0,Mathf.Asin(-hit.normal.x)*180/Mathf.PI);
		}
		else
		{
			senka.localScale = Vector3.zero;
		}

		if(measureDistance)
		{
			distance = (int)(Camera.main.transform.position.x - startPosX)/4;
			MissionManager.Instance.DistanceEvent(distance);
		}
		
//		if(snappingToClimb)
//		{
//			StartCoroutine(snappingProcess());
//			snappingToClimb = false;
//		}
		
//		if(grab)
//		{
//			StartCoroutine(ProceduraPenjanja(null));
//			grab = false;
//		}
		
		if(startSpustanje)
		{
			if(startPenjanje)
			{
				pocetniY_spustanje = Mathf.Lerp(pocetniY_spustanje,cameraTarget.transform.position.y,0.2f);
				cameraTarget_down.transform.position = new Vector3(cameraTarget.transform.position.x, pocetniY_spustanje, cameraTarget_down.transform.position.z);
				if(cameraTarget.transform.position.y <= cameraTarget_down.transform.position.y)
				{
					cameraFollow.cameraTarget = cameraTarget;
					cameraFollow.transition = false;
				}
			}
			else
			{
				pocetniY_spustanje = Mathf.Lerp(pocetniY_spustanje, cameraTarget_down_y, 0.15f);
				//Debug.Log("poc: " + pocetniY_spustanje + ", end: " + cameraTarget_down_y);
				cameraTarget_down.transform.position = new Vector3(cameraTarget.transform.position.x, pocetniY_spustanje, cameraTarget_down.transform.position.z);
				if(cameraTarget.transform.position.y <= cameraTarget_down.transform.position.y)
				{
					cameraFollow.cameraTarget = cameraTarget;
					cameraFollow.transition = false;
				}
			}
		}
		
//		if(currentBaseState.nameHash == doublejump_State)
//		{
//			if(animator.GetBool("DoubleJump"))
//				animator.SetBool("DoubleJump",false);
//		}
		
		
		else if(currentBaseState.nameHash == fall_State)
		{
			if(animator.GetBool("Falling"))
				animator.SetBool("Falling",false);
		}
//		else if(currentBaseState.nameHash == glide_loop_State)
//		{
//			if(Time.frameCount % 60 == 0)
//			{
//				if(Random.Range(1,100) <= 10)
//					StartCoroutine(turnHead(0.1f));
//			}
//		}

		if(proveraGround == 0)
		{
			grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		}
		else
			proveraGround--;

		//CheckWallHitNear 			= Physics2D.Linecast(transform.position + new Vector3(0.8f,2.5f,0), transform.position + new Vector3(3.5f,2.5f,0), (1 << LayerMask.NameToLayer("WallHit")));// | (1 << LayerMask.NameToLayer("Ground")));
		//CheckWallHitNear_low 		= Physics2D.Linecast(transform.position + new Vector3(0.8f,0f,0), transform.position + new Vector3(3.5f,0f,0), (1 << LayerMask.NameToLayer("WallHit")));
		//triggerCheckDown 			= Physics2D.Linecast(transform.position + new Vector3(0.8f,2.5f,0), transform.position + new Vector3(0.8f,-0.5f,0), 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Platform"));
		triggerCheckDownTrigger 	= Physics2D.Linecast(transform.position + new Vector3(0.8f,2.5f,0), transform.position + new Vector3(0.8f,-4.5f,0), /*1 << LayerMask.NameToLayer("Ground") | */1 << LayerMask.NameToLayer("Platform"));
		triggerCheckDownBehind	 	= Physics2D.Linecast(transform.position + new Vector3(-0.8f,2.5f,0), transform.position + new Vector3(-0.8f,-4.5f,0), 1 << LayerMask.NameToLayer("Platform"));
		//proveriTerenIspredY			= Physics2D.Linecast(transform.position + new Vector3(4.4f,1.2f,0), transform.position + new Vector3(4.4f,-3.2f,0), 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Platform"));
		//downHit						= Physics2D.Linecast(transform.position + new Vector3(0.2f,0.1f,0), transform.position + new Vector3(0.2f,-0.65f,0), 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Platform"));
		//spustanjeRastojanje 		= Physics2D.Linecast(transform.position + new Vector3(2.3f,1.25f,0), transform.position + new Vector3(2.3f,-Camera.main.orthographicSize,0), 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Platform"));

		//		Debug.DrawLine(transform.position+Vector3.up, groundCheck.position + Vector3.down/2,Color.white); //grounded
		//		Debug.DrawLine(transform.position + new Vector3(0.8f,2.5f,0), transform.position + new Vector3(3.5f,2.5f,0), Color.cyan); //check for wall front
		//		Debug.DrawLine(transform.position + new Vector3(0.8f,0f,0), transform.position + new Vector3(3.5f,0f,0), Color.cyan); //check for wall front low
		//		Debug.DrawLine(transform.position + new Vector3(1.1f,2.5f,0), transform.position + new Vector3(3.65f,2.5f,0), Color.red); //sideHitUp
		//		Debug.DrawLine(transform.position + new Vector3(0.8f,2.5f,0), transform.position + new Vector3(0.8f,-0.5f,0), Color.magenta); //check trigger down, bilo je -3 umesto -1.5, i gore isto
		//		Debug.DrawLine(transform.position + new Vector3(0.8f,2.5f,0), transform.position + new Vector3(0.8f,-4.5f,0), Color.magenta); //check trigger down front, bilo je -3 umesto -1.5, i gore isto
		//		Debug.DrawLine(transform.position + new Vector3(-0.8f,2.5f,0), transform.position + new Vector3(-0.8f,-4.5f,0), Color.magenta); //check trigger down behind, bilo je -3 umesto -1.5, i gore isto
		//
		//
		//		Debug.DrawLine(transform.position + new Vector3(4.4f,1.2f,0), transform.position + new Vector3(4.4f,-3.2f,0),Color.white); //proveri teren ispred y
		//		Debug.DrawLine(transform.position + new Vector3(0.2f,0.1f,0), transform.position + new Vector3(0.2f,-0.65f,0),Color.green); //downHit
		//		Debug.DrawLine(transform.position + new Vector3(2.3f,1.25f,0), transform.position + new Vector3(2.3f,-Camera.main.orthographicSize,0), Color.blue);
		//		Debug.DrawLine(transform.position + new Vector3(0.2f,2.1f,0), transform.position + new Vector3(0.2f,4f,0), Color.blue);

		
		if(state == State.jumped || state == State.climbUp)
		{


			if(DupliSkok) //Opcija za dupli skok
			{
				
				if((Input.GetMouseButtonDown(0) && Input.mousePosition.x > povrsinaZaClick) || Input.GetKeyDown(KeyCode.Space))
				{
					if(mozeDaSkociOpet)
					{
						if(hasJumped)
						{
							//parentAnim.Play("DoubleJumpRotate");
							animator.SetBool("DoubleJump",true);
							//animator.Play(doublejump_State);
							if(PlaySounds.soundOn)
							{
								PlaySounds.Stop_Run();
								PlaySounds.Play_Jump();
							}
							doubleJump = true;
							animator.SetBool("Glide",false);
							swoosh = false;
							GetComponent<Rigidbody2D>().drag = 0;
						}
						
					}
				}
				
				
				// INPUT WITH TOUCHES
				else if(Input.touchCount > 1 && Input.GetTouch(1).phase == TouchPhase.Began)// && Input.GetTouch(1).position.x > Screen.width/2)
				{
					if(mozeDaSkociOpet)
					{
						
						if(hasJumped)
						{
							disableGlide = true;
							animator.SetBool("DoubleJump",true);
							//parentAnim.Play("DoubleJumpRotate");
							//animator.Play(doublejump_State);
							doubleJump = true;
							animator.SetBool("Glide",false);
							swoosh = false;
							GetComponent<Rigidbody2D>().drag = 0;
						}
						
					}
				}
			}
			if(SlideNaDole || Glide)
			{
				if((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))//  && Input.mousePosition.x < Screen.width/2)
				{
					duzinaPritiskaZaSkok = Time.time;
					startY = Input.mousePosition.y;
					canGlide = true;
					if(mushroomJumped)
					{
						GetComponent<Rigidbody2D>().isKinematic = false;
						mushroomJumped = false;
					}
				}
				
				
				else if((Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space)))// && Input.mousePosition.x < Screen.width/2) 
				{
					startY = endY = 0;
					GetComponent<Rigidbody2D>().drag = 0;
					animator.SetBool("Glide",false);
					disableGlide = false;
					canGlide = false;
					if(PlaySounds.Glide_NEW.isPlaying)
						PlaySounds.Stop_Glide();
					if(trail.time > 0)
						StartCoroutine(nestaniTrail(2));
				}
				
				else if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
				{
					startY = endY = 0;
					GetComponent<Rigidbody2D>().drag = 0;
					animator.SetBool("Glide",false);
					canGlide = false;
					if(PlaySounds.Glide_NEW.isPlaying)
						PlaySounds.Stop_Glide();
					if(trail.time > 0)
					StartCoroutine(nestaniTrail(2));
				}
			}
			
			if((Input.GetMouseButton(0)))// && Input.mousePosition.x < Screen.width/2)
			{
				endY = Input.mousePosition.y;
				if(SlideNaDole)
				{
					if((startY - endY) > ( (1f/8f*Camera.main.orthographicSize)*(Screen.height/(2*Camera.main.orthographicSize))))
					{
						if(!Physics2D.Linecast(groundCheck.position, groundCheck.position - Vector3.up*20, whatIsGround))
						{
							powerfullImpact = true;
							zutiGlowSwooshVisoki.gameObject.SetActive(true);
						}
						swoosh = true;
						isSliding = true;
						animator.Play(swoosh_State);
					}
				}
				
				if(Glide && canGlide)
				{
					if(!disableGlide)
					{
						if(!animator.GetBool("Glide"))
						{
							animator.Play(glide_start_State);
							animator.SetBool("Glide",true);

						}
						GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeedX,GetComponent<Rigidbody2D>().velocity.y);
						GetComponent<Rigidbody2D>().drag = 7.5f;
						trail.time = trailTime;
						if(!PlaySounds.Glide_NEW.isPlaying && PlaySounds.soundOn)
							PlaySounds.Play_Glide();

					}
				}
				
			}
			
			if(KontrolisaniSkok)
			{
				if(korakce < 1.5f)
				{
					if(!pustenVoiceJump)
					{
						pustenVoiceJump = true;
						if(PlaySounds.soundOn)
							PlaySounds.Play_VoiceJump();
					}
				}
				
				if((Input.GetMouseButtonUp(0) || Time.time - duzinaPritiskaZaSkok > 1f) && jumpControlled) //njanjanjanja 0.35 //zadnje 0.3
				{
					jumpControlled = false;
					jumpHolding = false;
					tempForce = jumpForce;
					canGlide = false;
					if(PlaySounds.Glide_NEW.isPlaying)
						PlaySounds.Stop_Glide();
					if(trail.time > 0)
					StartCoroutine(nestaniTrail(2));
				}
			}
			
			if(trava.isPlaying)
				trava.Stop();
			if(runParticle.isPlaying)
				runParticle.Stop();
			
		}
		if(state == State.wallhit)
		{
			if(KontrolisaniSkok)
			{
				if((Input.GetMouseButtonDown(0) && Input.mousePosition.x > povrsinaZaClick) || Input.GetKeyDown(KeyCode.Space))
				{
					//if(heCanJump && RaycastFunction(Input.mousePosition) != "Pause" && RaycastFunction(Input.mousePosition) != "HoleButtonsPlay_Pause" && RaycastFunction(Input.mousePosition) != "PauseHoleShop" && RaycastFunction(Input.mousePosition) != "PauseHoleFreeCoins" && RaycastFunction(Input.mousePosition) != "PowersCardShield" && RaycastFunction(Input.mousePosition) != "PowersCardMagnet" && RaycastFunction(Input.mousePosition) != "PowersCardCoinx2")
					if(RaycastFunction(Input.mousePosition) != "Pause Button" && RaycastFunction(Input.mousePosition) != "Play Button_Pause" && 
					   RaycastFunction(Input.mousePosition) != "Buy Button" && RaycastFunction(Input.mousePosition) != "ButtonMain_NoMoreLives" && 
					   RaycastFunction(Input.mousePosition) != "Restart Button_Pause" && !RaycastFunction(Input.mousePosition).Contains("Tutorial") &&
					   !RaycastFunction(Input.mousePosition).Contains("Power_") && RaycastFunction(Input.mousePosition) != "Menu Button_Pause") // ZA PRVU VERZIJU
					{
						duzinaPritiskaZaSkok = Time.time;
						
						if(!inAir)
						{
							state = State.climbUp;
							if(PlaySounds.soundOn)
							{
								PlaySounds.Stop_Run();
								PlaySounds.Play_Jump();
							}
							jumpControlled = true;
							animator.Play(jump_State);
							animator.SetBool("Landing",false);
							animator.SetBool("WallStop",false);
							inAir = true;
							tempForce = jumpForce;
							particleSkok.Emit(20);
						}
					}
				}
				
				else if(Input.GetMouseButtonUp(0) && usporavanje)
				{
					if(Zaustavljanje)
					{
						usporavanje = false;
						state = State.running;
						maxSpeedX = startSpeedX;
						animator.Play(run_State);
						animator.SetBool("WallStop",false);
					}
				}
				
			}
			else
			{
				if((Input.GetMouseButtonDown(0)/* && Input.mousePosition.x > Screen.width/2*/) || Input.GetKeyDown(KeyCode.Space))
				{
					//if(heCanJump && RaycastFunction(Input.mousePosition) != "Pause" && RaycastFunction(Input.mousePosition) != "HoleButtonsPlay_Pause" && RaycastFunction(Input.mousePosition) != "PauseHoleShop" && RaycastFunction(Input.mousePosition) != "PauseHoleFreeCoins" && RaycastFunction(Input.mousePosition) != "PowersCardShield" && RaycastFunction(Input.mousePosition) != "PowersCardMagnet" && RaycastFunction(Input.mousePosition) != "PowersCardCoinx2")
					if(RaycastFunction(Input.mousePosition) != "Pause Button" && RaycastFunction(Input.mousePosition) != "Play Button_Pause" && 
					   RaycastFunction(Input.mousePosition) != "Buy Button" && RaycastFunction(Input.mousePosition) != "ButtonMain_NoMoreLives" && 
					   RaycastFunction(Input.mousePosition) != "Restart Button_Pause" && !RaycastFunction(Input.mousePosition).Contains("Tutorial") &&
					   !RaycastFunction(Input.mousePosition).Contains("Power_") && RaycastFunction(Input.mousePosition) != "Menu Button_Pause") // ZA PRVU VERZIJU
					{
						if(!inAir)
						{
							state = State.climbUp;
							jumpSpeedX = 5;
							if(PlaySounds.soundOn)
							{
								PlaySounds.Stop_Run();
								PlaySounds.Play_Jump();
							}
							jump = true;
							animator.Play(jump_State);
							animator.SetBool("Landing",false);
							animator.SetBool("WallStop",false);
							inAir = true;
							particleSkok.Emit(20);
						}
					}
				}
				else if(Input.GetMouseButtonUp(0) && usporavanje)
				{
					if(Zaustavljanje)
					{
						usporavanje = false;
						state = State.running;
						maxSpeedX = startSpeedX;
						animator.Play(run_State);
						animator.SetBool("WallStop",false);
					}
				}
			}
		}
		if(state == State.running)
		{
//			if(Time.frameCount % 300 == 0)
//			{
//				if(Random.Range(1,100) <= 25)
//					StartCoroutine(turnHead(0.1f));
//			}
			
			if(KontrolisaniSkok)
			{
				
				if((Input.GetMouseButtonDown(0) && Input.mousePosition.x > povrsinaZaClick) || Input.GetKeyDown(KeyCode.Space))
				{
					if(RaycastFunction(Input.mousePosition) != "Pause Button" && RaycastFunction(Input.mousePosition) != "Play Button_Pause" && 
					   RaycastFunction(Input.mousePosition) != "Buy Button" && RaycastFunction(Input.mousePosition) != "ButtonMain_NoMoreLives" && 
					   RaycastFunction(Input.mousePosition) != "Restart Button_Pause" && !RaycastFunction(Input.mousePosition).Contains("Tutorial") &&
					   !RaycastFunction(Input.mousePosition).Contains("Power_") && RaycastFunction(Input.mousePosition) != "Menu Button_Pause") // ZA PRVU VERZIJU
						//if(heCanJump && RaycastFunction(Input.mousePosition) != "ButtonPause" && RaycastFunction(Input.mousePosition) != "PauseHolePlay" && RaycastFunction(Input.mousePosition) != "PauseHoleShop" && RaycastFunction(Input.mousePosition) != "PauseHoleFreeCoins" && RaycastFunction(Input.mousePosition) != "PowersCardShield" && RaycastFunction(Input.mousePosition) != "PowersCardMagnet" && RaycastFunction(Input.mousePosition) != "PowersCardCoinx2") // ZA FINALNU VERZIJU
					{	
						jumpHolding = true;
						grounded = false;
						proveraGround = 16;
						korakce = 3;
						GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeedX,maxSpeedY);
						neTrebaDaProdje = false;
						duzinaPritiskaZaSkok = Time.time;
						state = State.jumped;
						if(PlaySounds.soundOn)
						{
							PlaySounds.Stop_Run();
							PlaySounds.Play_Jump();
							Invoke("PustiVoiceJump",0.35f);
						}
						jumpControlled = true;
						animator.Play(jump_State);
						animator.SetBool("Landing",false);
						inAir = true;
						particleSkok.Emit(20);
					}
				}
				

				if((Input.GetMouseButtonUp(0) || Time.time - duzinaPritiskaZaSkok > 2.0f) && jumpControlled) //0.275
				{
					jumpControlled = false;
					tempForce = jumpForce;
				}
				
			}
//			else
//			{
//				if((Input.GetMouseButtonDown(0) && Input.mousePosition.x > povrsinaZaClick) || Input.GetKeyDown(KeyCode.Space))
//				{
//					if(RaycastFunction(Input.mousePosition) != "Pause Button" && RaycastFunction(Input.mousePosition) != "Play Button_Pause" && 
//					   RaycastFunction(Input.mousePosition) != "Buy Button" && RaycastFunction(Input.mousePosition) != "ButtonMain_NoMoreLives" && 
//					   RaycastFunction(Input.mousePosition) != "Restart Button_Pause" && !RaycastFunction(Input.mousePosition).Contains("Tutorial") &&
//					   !RaycastFunction(Input.mousePosition).Contains("Power_")) // ZA PRVU VERZIJU
//						//if(heCanJump && RaycastFunction(Input.mousePosition) != "ButtonPause" && RaycastFunction(Input.mousePosition) != "PauseHolePlay" && RaycastFunction(Input.mousePosition) != "PauseHoleShop" && RaycastFunction(Input.mousePosition) != "PauseHoleFreeCoins" && RaycastFunction(Input.mousePosition) != "PowersCardShield" && RaycastFunction(Input.mousePosition) != "PowersCardMagnet" && RaycastFunction(Input.mousePosition) != "PowersCardCoinx2") // ZA FINALNU VERZIJU
//					{
//						neTrebaDaProdje = false;
//						state = State.jumped;
//						if(PlaySounds.soundOn)
//						{
//							PlaySounds.Stop_Run();
//							PlaySounds.Play_Jump();
//						}
//						jump = true;
//						Debug.Log("JEL JE OVDE SELJAK");
//						animator.Play(jump_State);
//						animator.SetBool("Landing",false);
//						inAir = true;
//						particleSkok.Emit(20);
//					}
//				}
//			}
			if(Zaustavljanje && povrsinaZaClick != 0)
			{
				if(Input.GetMouseButton(0)/* && Input.mousePosition.x < Screen.width/2*/)
				{
					{
						usporavanje = true;
						maxSpeedX = 0;
						state = State.wallhit;
						animator.SetBool("WallStop",true);
					}
				}
			}
			
			if(!trava.isPlaying)
				trava.Play();
			if(!runParticle.isPlaying)
				runParticle.Play();
		}
		
		if(state == State.lijana)
		{
			povrsinaZaClick = 0;
			
			if(Input.GetMouseButtonDown(0) && Input.mousePosition.x > povrsinaZaClick)
			{
				if(heCanJump && RaycastFunction(Input.mousePosition) != "Pause Button" && RaycastFunction(Input.mousePosition) != "Play Button_Pause" && 
				                   RaycastFunction(Input.mousePosition) != "Buy Button" && RaycastFunction(Input.mousePosition) != "ButtonMain_NoMoreLives" && 
				                   RaycastFunction(Input.mousePosition) != "Restart Button_Pause" && !RaycastFunction(Input.mousePosition).Contains("Tutorial") &&
				                   !RaycastFunction(Input.mousePosition).Contains("Power_") && RaycastFunction(Input.mousePosition) != "Menu Button_Pause")
				{
					if(Input.mousePosition.x < Screen.width/2)
					{
						startY = Input.mousePosition.y;
					}
					else
					{
						animator.Play(jump_State);
						OtkaciMajmuna();
					}
				}
			}
			
			if(SlideNaDole)
			{
				if(Input.GetMouseButton(0)/* && Input.mousePosition.x < Screen.width/2*/)
				{
					endY = Input.mousePosition.y;
					if(SlideNaDole)
					{
						if((startY - endY) > ( (1f/8f*Camera.main.orthographicSize)*(Screen.height/(2*Camera.main.orthographicSize))))
						{
							SpustiMajmunaSaLijaneBrzo();
							swoosh = true;
							isSliding = true;
							animator.Play(swoosh_State);
						}
					}
				}
				if(Input.GetMouseButtonUp(0))
				{
					startY = endY = 0;
				}
			}
		}
		
		if(state == State.saZidaNaZid)
		{
			if(Input.GetMouseButtonDown(0))
			{
				if(heCanJump && RaycastFunction(Input.mousePosition) != "Pause Button" && RaycastFunction(Input.mousePosition) != "Play Button_Pause" && 
				                   RaycastFunction(Input.mousePosition) != "Buy Button" && RaycastFunction(Input.mousePosition) != "ButtonMain_NoMoreLives" && 
				                   RaycastFunction(Input.mousePosition) != "Restart Button_Pause" && !RaycastFunction(Input.mousePosition).Contains("Tutorial") &&
				                   !RaycastFunction(Input.mousePosition).Contains("Power_") && RaycastFunction(Input.mousePosition) != "Menu Button_Pause")
				{
					if(!inAir)
					{
						neTrebaDaProdje = false;
						if(PlaySounds.soundOn)
						{
							PlaySounds.Stop_Run();
							PlaySounds.Play_Jump();
						}
						jump = true;
						animator.Play(jump_State);
						animator.SetBool("Landing",false);
						inAir = true;
						if(klizanje.isPlaying)
							klizanje.Stop();
						particleSkok.Emit(20);
						jumpSpeedX = -jumpSpeedX;
						GetComponent<Rigidbody2D>().drag = 0;
						moveForce = -moveForce;
						maxSpeedX = -maxSpeedX;
						majmun.localScale = new Vector3(majmun.localScale.x, majmun.localScale.y, -majmun.localScale.z);
					}
				}
			}
		}
		else if(state == State.preNegoDaSeOdbije && saZidaNaZid)
		{
			if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
			{
				jump = true;
				GetComponent<Rigidbody2D>().drag = 0;
				state = State.saZidaNaZid;
				animator.Play(jump_State);
				animator.SetBool("Landing",false);
				animator.SetBool("WallStop",false);
				particleSkok.Emit(20);
			}
		}

	}
	
	void FixedUpdate ()
	{
		currentBaseState = animator.GetCurrentAnimatorStateInfo(0);
		
		//------------------------------------------------------------------------
		if(state == State.saZidaNaZid) 		// ************* Player is bouncing from wall to wall
		{
			if(jump)
			{
				hasJumped = true;
				mozeDaSkociOpet = true;
				if(GetComponent<Rigidbody2D>().velocity.y < maxSpeedY)
				{
					GetComponent<Rigidbody2D>().velocity = Vector2.zero;
					//rigidbody2D.AddForce(new Vector2(rigidbody2D.velocity.x, 2500)); //promenjeno sa 0
					GetComponent<Rigidbody2D>().AddForce(new Vector2(maxSpeedX, 2500));
				}
				
				if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > maxSpeedY)
					//rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, maxSpeedY);
					GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeedX, maxSpeedY);
				
				jump = false;
				
			}
			GetComponent<Rigidbody2D>().velocity = new Vector2(jumpSpeedX, GetComponent<Rigidbody2D>().velocity.y);
		}
		
		//------------------------------------------------------------------------
		else if(state == State.preNegoDaSeOdbije) 		// ************* Player is bouncing from wall to wall
		{
			if(jump)
			{
				hasJumped = true;
				mozeDaSkociOpet = true;
				if(GetComponent<Rigidbody2D>().velocity.y < maxSpeedY)
				{
					GetComponent<Rigidbody2D>().velocity = Vector2.zero;
					//rigidbody2D.AddForce(new Vector2(rigidbody2D.velocity.x, 2500)); //promenjeno sa 0
					GetComponent<Rigidbody2D>().AddForce(new Vector2(maxSpeedX, 2500));
				}
				
				if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > maxSpeedY)
					//rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, maxSpeedY);
					GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeedX, maxSpeedY);
				
				jump = false;
				
			}
			GetComponent<Rigidbody2D>().velocity = new Vector2(jumpSpeedX, GetComponent<Rigidbody2D>().velocity.y);
			
			if(GetComponent<Rigidbody2D>().velocity.y < -0.01f)
				GetComponent<Rigidbody2D>().drag = 5;
		}
		
		
		//------------------------------------------------------------------------
		else if(state == State.wasted) 		// ************* Player is wasted
		{
			if(measureDistance)
				measureDistance = false;
//			if(rigidbody2D.velocity.x < maxSpeedX && !stop)
//			{
//				rigidbody2D.AddForce(Vector2.right * moveForce);
//			}
//			
//			if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeedX && !stop)
//				rigidbody2D.velocity = new Vector2(maxSpeedX, rigidbody2D.velocity.y);
			
		}
		//------------------------------------------------------------------------
		
		//------------------------------------------------------------------------
		if(state == State.running) 		// ************* Player is running
		{
			if(!PlaySounds.Run.isPlaying && PlaySounds.soundOn)
				PlaySounds.Play_Run();
			//maxSpeedX = startSpeedX;
			if(GetComponent<Rigidbody2D>().velocity.x < maxSpeedX && !stop)
			{
				GetComponent<Rigidbody2D>().AddForce(Vector2.right * moveForce);
			}
			
			if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeedX && !stop)
			{
				GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeedX, GetComponent<Rigidbody2D>().velocity.y);
			}
		}
		//------------------------------------------------------------------------
		else if(state == State.completed)	// ************* Player has completed the stage
		{
			if(dancing)
			{
				proveraGround = 0;
				if(grounded)
					GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
				else
					GetComponent<Rigidbody2D>().velocity = new Vector2(0,-30);
			}
			else
			{
				if(GetComponent<Rigidbody2D>().velocity.x < maxSpeedX && !stop)
				{
					GetComponent<Rigidbody2D>().AddForce(Vector2.right * moveForce);
				}
				
				if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeedX && !stop)
					GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeedX, GetComponent<Rigidbody2D>().velocity.y);
			}
			//if(Physics2D.Linecast(transform.position + new Vector3(0.8f,2.5f,0), transform.position + new Vector3(0.8f,-0.5f,0), 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Platform")))
			//	animator.Play(run_State);
			
		}
		
		
		
		//------------------------------------------------------------------------
		else if(state == State.jumped) 	// ************* Player is in the air
		{
			if(jumpHolding)
			{
				GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeedX, GetComponent<Rigidbody2D>().velocity.y + korakce);
				if(korakce > 0)
					korakce -= 0.085f;
					//korakce -= Time.deltaTime * 5.5f;
			}
			
			if(swoosh)
			{
				//rigidbody2D.AddForce(new Vector2(0, -jumpForce*2));
				//rigidbody2D.AddForce(new Vector2(rigidbody2D.velocity.x,-3000)); //promenjeno sa 0
				GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeedX,0);
				GetComponent<Rigidbody2D>().AddForce(new Vector2(maxSpeedX,-4500));
				//rigidbody2D.velocity = new Vector2(maxSpeedX,-100);
				swoosh = false;
			}
			if(doubleJump)
			{
				//dupliSkokEfekat.Emit(1);
				//dupliSkokEfekat2.Emit(1);
				dupliSkokOblaci.Emit(25);
				jumpSpeedX = startJumpSpeedX;
				//rigidbody2D.gravityScale = 10;
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				GetComponent<Rigidbody2D>().AddForce(new Vector2(2000, doubleJumpForce));
				doubleJump = false;
				hasJumped = false;
				
			}
			if(powerfullImpact)
			{
				if(!PlaySounds.Glide_NEW.isPlaying && PlaySounds.soundOn)
					PlaySounds.Play_Glide();
			}
			
			else if(jumpControlled)
			{
				hasJumped = true;
				mozeDaSkociOpet = true;

			}
			
		}
		//------------------------------------------------------------------------
		
		
		
		//------------------------------------------------------------------------
		else if(state == State.wallhit)	// ************* Player has hit the wall
		{
			//maxSpeedX = 0;
			if(trava.isPlaying)
				trava.Stop();
			if(runParticle.isPlaying)
				runParticle.Stop();
		}
		//------------------------------------------------------------------------
		
		
		//------------------------------------------------------------------------
		else if(state == State.climbUp)	// ************* Player is climbing
		{
			if(doubleJump)
			{
				//dupliSkokEfekat.Emit(1);
				//dupliSkokEfekat2.Emit(1);
				dupliSkokOblaci.Emit(25);
				jumpSpeedX = startJumpSpeedX;
				//rigidbody2D.gravityScale = 10;
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				GetComponent<Rigidbody2D>().AddForce(new Vector2(2000, doubleJumpForce));
				doubleJump = false;
				hasJumped = false;
			}
			else if(jump)
			{
				
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				//rigidbody2D.AddForce(new Vector2(rigidbody2D.velocity.x, jumpForce)); //promenjeno sa 0
				GetComponent<Rigidbody2D>().AddForce(new Vector2(maxSpeedX, jumpForce));
				hasJumped = true;
				jump = false;
			}
			else if(jumpControlled)
			{
				hasJumped = true;
				mozeDaSkociOpet = true;
				if(GetComponent<Rigidbody2D>().velocity.y < maxSpeedY)
				{
					GetComponent<Rigidbody2D>().AddForce(new Vector2(maxSpeedX, tempForce));
				}
				

				
				if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > maxSpeedY)
					GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, maxSpeedY);
			}
			
			GetComponent<Rigidbody2D>().velocity = new Vector2(jumpSpeedX, GetComponent<Rigidbody2D>().velocity.y);
			

		}
		//------------------------------------------------------------------------
		
		
	}
	
	void OnTriggerExit2D(Collider2D col)
	{
		if(col.tag == "Footer")
		{
			float triggerPositionDown;
			float triggerPositionUp;
			if(col.transform.childCount > 0)
			{
				triggerPositionUp = col.transform.Find("TriggerPositionUp").position.y;
				triggerPositionDown = col.transform.Find("TriggerPositionDown").position.y;
			}
			else
				triggerPositionUp = triggerPositionDown = col.transform.position.y;
			//Debug.Log("exit");
			if(GetComponent<Collider2D>().isTrigger && (groundCheck.position.y +0.2f > triggerPositionUp || ceilingCheck.position.y < triggerPositionDown))// && !triggerCheckDownTrigger)
			{
				//Physics2D.IgnoreLayerCollision(13,18,false);
				GetComponent<Collider2D>().isTrigger = false;
				neTrebaDaProdje = false;
				//Debug.Log("exitfalse");
			}
		}
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{
		if(state == State.completed && inAir)
		{
			if(powerfullImpact)
			{
				transform.Find("Impact").gameObject.SetActive(true);
				Invoke("DisableImpact",0.25f);
				powerfullImpact = false;
				if(PlaySounds.Glide_NEW.isPlaying)
					PlaySounds.Stop_Glide();
				zutiGlowSwooshVisoki.gameObject.SetActive(false);
				//majmun.Find("PorinceGorilla_LP").renderer.material.color = new Color(0.53f,0.53f,0.53f,1);
				Camera.main.GetComponent<Animator>().Play("CameraShakeTrasGround");
				izrazitiPad.Play();
				animator.Play("Landing");
			}
			else
			{
				//animator.SetBool("Glide",false);
				//disableGlide = false;
				animator.SetBool("Landing",true);
				//rigidbody2D.drag = 0;
				grounded = true;
				//canGlide = false;
				//if(PlaySounds.Glide_NEW.isPlaying)
				//	PlaySounds.Stop_Glide();
				oblak.Play();
			}
			inAir = false;
			if(isSliding)
				isSliding = false;
			Finish();
		}
		else if(state != State.completed || state != State.wasted)
		{
			if(col.gameObject.tag == "ZidZaOdbijanje")
			{
				if(state == State.running && CheckWallHitNear)
				{
					if(klizanje.isPlaying)
						klizanje.Stop();
					animator.SetBool("WallStop",true);
					animator.Play(wall_stop_State);
					state = State.preNegoDaSeOdbije;
					if(trava.isPlaying)
						trava.Stop();
					if(runParticle.isPlaying)
						runParticle.Stop();
				}
				else if(state != State.preNegoDaSeOdbije)
				{
					inAir = false;
					heCanJump = true;
					GetComponent<Rigidbody2D>().drag = 5;
					klizanje.Play();
					animator.Play("Klizanje");
					state = State.saZidaNaZid;
				}

				//@@@@@@ DODATAK ZA SVAKI SLUCAJ ZA SA ZIDA NA ZID
				wallHitGlide = true;
			}
			
			else if(col.gameObject.tag == "Footer")
			{
				startPenjanje = false;
				startSpustanje = false;
				if(cameraTarget_down.transform.parent == null)
				{
					cameraTarget_down.transform.position = cameraTarget.transform.position;
					cameraTarget_down.transform.parent = transform;
				}

				cameraTarget_down.transform.position = cameraTarget.transform.position;
				
				if(state == State.saZidaNaZid)
				{
					moveForce = Mathf.Abs(moveForce);
					jumpSpeedX = Mathf.Abs(jumpSpeedX);
					maxSpeedX = Mathf.Abs(maxSpeedX);
					majmun.localScale = new Vector3(majmun.localScale.x, majmun.localScale.y, Mathf.Abs(majmun.localScale.z));
					if(klizanje.isPlaying)
						klizanje.Stop();
					if(CheckWallHitNear || CheckWallHitNear_low)
					{
						animator.SetBool("WallStop",true);
						animator.Play(wall_stop_from_jump_State);
						state = State.preNegoDaSeOdbije;
						if(trava.isPlaying)
							trava.Stop();
					}
					else if(!CheckWallHitNear && !CheckWallHitNear_low)
					{
						mozeDaSkociOpet = false;
						animator.SetBool("Jump",false);
						animator.SetBool("DoubleJump",false);
						animator.SetBool("Glide",false);
						disableGlide = false;
						//animator.SetBool("Landing",true);
						animator.Play(run_State);
						GetComponent<Rigidbody2D>().drag = 0;
						state = State.running;
						canGlide = false;
						if(PlaySounds.Glide_NEW.isPlaying)
							PlaySounds.Stop_Glide();
						//trail.time = 0;
						if(trail.time > 0)
					StartCoroutine(nestaniTrail(2));
						if(PlaySounds.soundOn)
							PlaySounds.Play_Run();
						hasJumped = false;
						startY = endY = 0;
						inAir = false;
						//rigidbody2D.gravityScale = 10;
					}
				}
				
				if(state == State.jumped)
				{
					oblak.Play();
					//if(collider2D.isTrigger == false && triggerCheckDownTrigger) // DODATO JE "&& triggerCheckDownTrigger" i promenjen je x u triggerCheckDown sa 1.1 na 0.8, zbog provere ako si skocio bas na samu ivicu, on udje u running ali odmah padne
					if(proveraGround < 12)
					{
						if(startSpustanje)
						{
							startSpustanje = false;
							cameraTarget_down.transform.parent = transform;
							cameraTarget_down.transform.position = cameraTarget.transform.position;
							
						}

						if(powerfullImpact)
						{
							transform.Find("Impact").gameObject.SetActive(true);
							Invoke("DisableImpact",0.25f);
							powerfullImpact = false;
							if(PlaySounds.Glide_NEW.isPlaying)
								PlaySounds.Stop_Glide();
							zutiGlowSwooshVisoki.gameObject.SetActive(false);
							//majmun.Find("PorinceGorilla_LP").renderer.material.color = new Color(0.53f,0.53f,0.53f,1);
							Camera.main.GetComponent<Animator>().Play("CameraShakeTrasGround");
							izrazitiPad.Play();

							#if UNITY_ANDROID
							Handheld.Vibrate();
							#endif
							if(PlaySounds.soundOn)
								PlaySounds.Play_Landing_Strong();
						}
						else
						{
							if(PlaySounds.soundOn)					
								PlaySounds.Play_Landing();
						}

						if(lijana)
							lijana = false;

						jumpSpeedX = startJumpSpeedX;
						mozeDaSkociOpet = false;
						animator.SetBool("Jump",false);
						animator.SetBool("DoubleJump",false);
						animator.SetBool("Glide",false);
						disableGlide = false;
						animator.SetBool("Landing",true);
						GetComponent<Rigidbody2D>().drag = 0;
						state = State.running;
						grounded = true;
						canGlide = false;
						if(PlaySounds.Glide_NEW.isPlaying)
							PlaySounds.Stop_Glide();
						//trail.time = 0;
						if(trail.time > 0)
					StartCoroutine(nestaniTrail(2));
						if(PlaySounds.soundOn)
							PlaySounds.Play_Run();
						hasJumped = false;
						startY = endY = 0;
						inAir = false;
						if(isSliding)
							isSliding = false;
						//rigidbody2D.gravityScale = 10;
					}
				}

				if(state == State.wasted)
				{
					if(utepanULetu)
					{
						if(magnet)
							manage.ApplyPowerUp(-1);
						if(doublecoins)
							manage.ApplyPowerUp(-2);
						StartCoroutine(AfterFallDown());
					}
				}
			}
			
			
			else if(col.gameObject.tag == "Enemy")
			{
				if(activeShield)
				{
					if(PlaySounds.soundOn)
						PlaySounds.Play_LooseShield();
					enemyCollider = col.transform.GetComponent<Collider2D>();
					enemyCollider.enabled = false;
					Invoke("EnableColliderBackOnEnemy",1f);
					activeShield = false;
					transform.Find("Particles/ShieldDestroyParticle").GetComponent<ParticleSystem>().Play();
					transform.Find("Particles/ShieldDestroyParticle").GetChild(0).GetComponent<ParticleSystem>().Play();
					manage.SendMessage("ApplyPowerUp",-3);
					if(state != State.running)
					{
						//rigidbody2D.AddForce(new Vector2(rigidbody2D.velocity.x, 1100)); //promenjeno sa 0
						//rigidbody2D.AddForce(new Vector2(maxSpeedX,0));
						//rigidbody2D.AddForce(new Vector2(maxSpeedX, 1100));
						//"DA SE PROVERI STA SE DESAVA KADA SA SHIELDOM NASKOCIM NA BOOMERANG!!!!!"
						//"DA SE UBACI OUTLINE ZA +3COINS!!!!!"
						//"DA SE UBACI TEKSTURA ZA TIPS & TRICKS!!!!!"
						//"DA SE UBACI PREFAB ZA POENIMA KADA JE OSTRVO ZAKLJUCANO I NOVA ANIMACIJA OTKLJUCAVANJA!!!!!"
					}
				}
				else if(!killed && !invincible)
				{
					if(col.gameObject.name.Contains("Biljka"))
					{
						if(PlaySounds.soundOn)
							PlaySounds.Play_BiljkaUgriz();
						col.transform.Find("Biljka_mesozder").GetComponent<Animator>().Play("Attack");
					}
					else
					{
						if(PlaySounds.soundOn)
							PlaySounds.Play_Siljci();
					}

					transform.Find("Particles/OblakKill").GetComponent<ParticleSystem>().Play();

					if(state == State.running)
					{
						//PlaySounds.Play_SmashBaboon();
						GetComponent<Rigidbody2D>().velocity = Vector2.zero;
						killed = true;
						oblak.Play();
						//Debug.Log("Utepan");
						majmunUtepan();
					}
					else 
					{
						majmunUtepanULetu();
						//rigidbody2D.isKinematic = true;
						
						//if(PlaySounds.BackgroundMusic_Gameplay.isPlaying)
						//	PlaySounds.Stop_BackgroundMusic_Gameplay();
					}
				}
			}

			//@@@@@@ DODATAK ZA SA ZIDA NA ZID
			else if(col.gameObject.tag.Equals("WallHit"))
			{
				wallHitGlide = true;
			}
		}
	}

	void EnableColliderBackOnEnemy()
	{
		enemyCollider.enabled = true;
		enemyCollider = null;
	}
	
	public void majmunUtepanULetu()
	{
		StartCoroutine(FallDownAfterSpikes());
	}
	
	IEnumerator FallDownAfterSpikes()
	{
		if(GetComponent<Rigidbody2D>().drag != 0)
		{
			GetComponent<Rigidbody2D>().drag = 0;
			canGlide = false;
			if(PlaySounds.Glide_NEW.isPlaying)
				PlaySounds.Stop_Glide();
			if(trail.time != 0)
				StartCoroutine(nestaniTrail(2));
		}
		if(powerfullImpact)
		{
			powerfullImpact = false;
			if(PlaySounds.Glide_NEW.isPlaying)
				PlaySounds.Stop_Glide();
			zutiGlowSwooshVisoki.gameObject.SetActive(false);
		}
		majmun.GetComponent<Animator>().speed = 1.5f;
		gameObject.layer = LayerMask.NameToLayer("MonkeyShadow");
		utepanULetu = true;
		canRespawnThings = false;
		//majmun.GetComponent<Animator>().speed = 1.5f;
		//if(PlaySounds.BackgroundMusic_Gameplay.isPlaying)
		//	PlaySounds.Stop_BackgroundMusic_Gameplay();

		//rigidbody2D.velocity = Vector2.zero;
		killed = true;
		oblak.Play();
		animator.Play(spikedeath_State);
		//parentAnim.Play("FallDown");
		state = State.wasted;
		manage.transform.Find("Gameplay Scena Interface/_TopRight/Pause Button").GetComponent<Collider>().enabled = false;
		//GameObject.Find("Pause").collider.enabled = false; // ZA PRVU VERZIJU
		//GameObject.Find("ButtonPause").collider.enabled = false; // ZA FINALNU VERZIJU
		if(trava.isPlaying)
			trava.Stop();
		if(runParticle.isPlaying)
			runParticle.Stop();
		//maxSpeedX = 0;
		//rigidbody2D.isKinematic = true;
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		GetComponent<Rigidbody2D>().velocity = new Vector2(-8,30);
		yield return new WaitForSeconds(1f);

	}
	IEnumerator AfterFallDown()
	{
		utepanULetu = false;
		animator.SetTrigger("ToLand");
		GetComponent<Rigidbody2D>().velocity = new Vector2(-4,22);
		yield return new WaitForSeconds(0.65f);
		while(!grounded)
		{
			yield return null;
		}
		GetComponent<Rigidbody2D>().isKinematic = true;
		yield return new WaitForSeconds(0.35f);
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		if(manage.keepPlayingCount >= 1)
			manage.SendMessage("showFailedScreen");
		else
			manage.SendMessage("ShowKeepPlayingScreen");
		cameraFollow.stopFollow = true;
		if(!invincible)
			transform.Find("Particles/HolderKillStars").gameObject.SetActive(true);

	}
	
	void OnCollisionExit2D(Collision2D col)
	{
		if(col.gameObject.tag == "Footer")
		{
			if(state == State.running)
			{
				neTrebaDaProdje = false;
				//if(!proveriTerenIspredY && !downHit) 	// ************* 2.5m ispred nema nicega ispod
				if(!Physics2D.Linecast(transform.position + new Vector3(4.4f,1.2f,0), transform.position + new Vector3(4.4f,-3.2f,0), 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Platform")) && !Physics2D.Linecast(transform.position + new Vector3(0.2f,0.1f,0), transform.position + new Vector3(0.2f,-0.65f,0), 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Platform")))
				{
					state = State.jumped;
					animator.SetBool("Landing",false);
					animator.Play(fall_State);
					if(runParticle.isPlaying)
						runParticle.Stop();
					//if(!spustanjeRastojanje)
					if(!Physics2D.Linecast(transform.position + new Vector3(2.3f,1.25f,0), transform.position + new Vector3(2.3f,-Camera.main.orthographicSize,0), 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Platform")))
					{
						startSpustanje = true;
						cameraTarget_down.transform.parent = null;
						pocetniY_spustanje = cameraTarget.transform.position.y;
						cameraTarget_down_y = transform.position.y -7.5f;
						cameraFollow.cameraTarget = cameraTarget_down;
					}
				}
			}
		}
		else if(col.gameObject.tag == "ZidZaOdbijanje")
		{
			if(klizanje.isPlaying)
				klizanje.Stop();
			GetComponent<Rigidbody2D>().drag = 0;
			if(trava.isPlaying)
				trava.Stop();
			if(state != State.wasted)
				animator.Play(fall_State);
			animator.SetBool("Landing",false);

			//@@@@@@ DODATAK ZA SVAKI SLUCAJ ZA SA ZIDA NA ZID
			wallHitGlide = false;
		}

		//@@@@@@ DODATAK ZA SA ZIDA NA ZID
		else if(col.gameObject.tag.Equals("WallHit"))
		{
			wallHitGlide = false;
		}
		
	}
	
	void NotifyManagerForFinish()
	{
		manage.SendMessage("ShowWinScreen");
	}
	
	IEnumerator TutorialPlay(Transform obj, string ime, int next)
	{
		StartCoroutine( obj.GetComponent<Animation>().Play(ime, false, what => {helpBool=true;}) );
		while(!helpBool)
		{
			yield return null;
		}
		helpBool=false;
		
		if(next == 1)
			StartCoroutine(TutorialPlay(obj,"TutorialIdle1_A",-1));
		else if(next == 2)
			StartCoroutine(TutorialPlay(obj,"TutorialIdle2_A",-1));
		
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if(state != State.completed || state != State.wasted)
		{
			//			if(col.name.StartsWith("Tutorial"))
			//			{
			//				int koji=0;
			//				if(col.name.Contains("1"))
			//					koji=1;
			//				else if(col.name.Contains("2"))
			//					koji=2;
			//				Time.timeScale = 0;
			//				col.collider2D.enabled = false;
			//				col.transform.position = Camera.main.transform.position + Vector3.forward*10;
			//				col.transform.GetChild(0).gameObject.SetActive(true);
			//				StartCoroutine(TutorialPlay(col.transform.GetChild(0).GetChild(0),"TutorialUlaz_A",koji));
			//			}
			//			if(col.name.StartsWith("Tutorial2"))
			//			{
			//				Time.timeScale = 0;
			//				col.transform.GetChild(0).gameObject.SetActive(true);
			//			}
			
//			if(col.tag == "Barrel")
//			{
//				col.transform.GetChild(0).GetComponent<Animator>().Play("BarrelBoom");
//			}
			if(col.name == "Magnet_collect")
			{
				if(PlaySounds.soundOn)
					PlaySounds.Play_CollectPowerUp();

				LevelFactory.instance.magnetCollected = true;
				magnet = true;
				manage.SendMessage("ApplyPowerUp",1);
				col.gameObject.SetActive(false);
				collectItem.Play();
				StartCoroutine(reappearItem(col.gameObject));
			}
			else if(col.name == "BananaCoinX2_collect")
			{
				if(PlaySounds.soundOn)
					PlaySounds.Play_CollectPowerUp();

				doublecoins = true;
				manage.SendMessage("ApplyPowerUp",2);
				col.gameObject.SetActive(false);
				collectItem.Play();
				StartCoroutine(reappearItem(col.gameObject));
			}
			else if(col.name == "Shield_collect")
			{
				if(PlaySounds.soundOn)
					PlaySounds.Play_CollectPowerUp();

				LevelFactory.instance.shieldCollected = true;
				manage.SendMessage("ApplyPowerUp",3);
				col.gameObject.SetActive(false);
				activeShield = true;
				collectItem.Play();
				StartCoroutine(reappearItem(col.gameObject));
			}
			else if(col.name == "Banana_collect")
			{
				if(PlaySounds.soundOn)
					PlaySounds.Play_CollectBanana();
				col.gameObject.SetActive(false);
				Manage.points += 200;
				Manage.pointsText.text = Manage.points.ToString();
				Manage.pointsEffects.RefreshTextOutline(false,true);
				collectItem.Play();
				StartCoroutine(reappearItem(col.gameObject));
				//Manage.bananas++;
				//odmah da mu doda bananu koju skupi u ukupan broj
				StagesParser.currentBananas++;
				PlayerPrefs.SetInt("TotalBananas",StagesParser.currentBananas);
				PlayerPrefs.Save();
			}
			else if(col.name == "Srce_collect")
			{
				col.gameObject.SetActive(false);
				GameObject.Find("LifeManager").SendMessage("AddLife");
				StartCoroutine(reappearItem(col.gameObject));
			}
			else if(col.name.Contains("Diamond_collect"))
			{
				if(PlaySounds.soundOn)
					PlaySounds.Play_CollectDiamond();
				col.gameObject.SetActive(false);
				Manage.points += 50;
				Manage.pointsText.text = Manage.points.ToString();
				Manage.pointsEffects.RefreshTextOutline(false,true);
				collectItem.Play();
				StartCoroutine(reappearItem(col.gameObject));
				if(col.name.Contains("Red"))
				{
					Manage.redDiamonds++;
					MissionManager.Instance.RedDiamondEvent(Manage.redDiamonds);
				}
				else if(col.name.Contains("Blue"))
				{
					Manage.blueDiamonds++;
					MissionManager.Instance.BlueDiamondEvent(Manage.blueDiamonds);
				}
				else if(col.name.Contains("Green"))
				{
					Manage.greenDiamonds++;
					MissionManager.Instance.GreenDiamondEvent(Manage.greenDiamonds);
				}
				MissionManager.Instance.DiamondEvent(Manage.redDiamonds + Manage.greenDiamonds + Manage.blueDiamonds);
			}
			else if(col.name == "BananaFog")
			{
				col.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
				Camera.main.transform.Find("FogOfWar").GetComponent<Renderer>().enabled = true;
				//Camera.main.GetComponent<Animator>().Play("FogOfWar_Appear");
				//StartCoroutine(RemoveFog(5f,col));
				StartCoroutine(pojaviMaglu(col));
			}
			else if(col.name.Contains("CoinsBagBig"))
			{
				col.transform.Find("+3CoinsHolder/+3Coins").GetComponent<TextMesh>().text = col.transform.transform.Find("+3CoinsHolder/+3Coins/+3CoinsShadow").GetComponent<TextMesh>().text = "+500";
				col.transform.Find("+3CoinsHolder").GetComponent<Animator>().Play("FadeOutCoins");
				col.transform.Find("AnimationHolder").GetComponent<Animation>().Play("CoinsBagCollect");
				Manage.coinsCollected+=500;
				Manage.Instance.coinsCollectedText.text = Manage.coinsCollected.ToString();
				Manage.Instance.coinsCollectedText.GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
				collectItem.Play();
			}
			else if(col.name.Contains("CoinsBagMedium"))
			{
				col.transform.Find("+3CoinsHolder/+3Coins").GetComponent<TextMesh>().text = col.transform.transform.Find("+3CoinsHolder/+3Coins/+3CoinsShadow").GetComponent<TextMesh>().text = "+250";
				col.transform.Find("+3CoinsHolder").GetComponent<Animator>().Play("FadeOutCoins");
				col.transform.Find("AnimationHolder").GetComponent<Animation>().Play("CoinsBagCollect");
				Manage.coinsCollected+=250;
				Manage.Instance.coinsCollectedText.text = Manage.coinsCollected.ToString();
				Manage.Instance.coinsCollectedText.GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
				collectItem.Play();
			}
			else if(col.name.Contains("CoinsBagSmall"))
			{
				col.transform.Find("+3CoinsHolder/+3Coins").GetComponent<TextMesh>().text = col.transform.transform.Find("+3CoinsHolder/+3Coins/+3CoinsShadow").GetComponent<TextMesh>().text = "+100";
				col.transform.Find("+3CoinsHolder").GetComponent<Animator>().Play("FadeOutCoins");
				col.transform.Find("AnimationHolder").GetComponent<Animation>().Play("CoinsBagCollect");
				Manage.coinsCollected+=100;
				Manage.Instance.coinsCollectedText.text = Manage.coinsCollected.ToString();
				Manage.Instance.coinsCollectedText.GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
				collectItem.Play();
			}
			if(col.tag == "Finish")
			{
				col.GetComponent<Collider2D>().enabled = false;
				Finish();
			}
//				Debug.Log(name);
//				col.collider2D.enabled = false;
//				cameraFollow.cameraFollowX = false;
//				Invoke("NotifyManagerForFinish",1.25f);
//				GameObject.Find("Pause").collider.enabled = false; // ZA PRVU VERZIJU
//				//GameObject.Find("ButtonPause").collider.enabled = false; // ZA FINALNU VERZIJU
//				state = State.completed;
//			}
			
			
			else if(col.tag == "Footer" && col.gameObject.layer == LayerMask.NameToLayer("Platform"))
			{
				float triggerPosition;
				if(col.transform.childCount > 0)
					triggerPosition = col.transform.Find("TriggerPositionUp").position.y;
				else
					triggerPosition = col.transform.position.y;
				//Debug.Log("enter");
				if((transform.position.y + 0.25f > triggerPosition) && triggerCheckDownTrigger && triggerCheckDownBehind && GetComponent<Collider2D>().isTrigger)
				{
					GetComponent<Collider2D>().isTrigger = false;
					//Debug.Log("enterfalse");
				}
				//collider2D.isTrigger = true;
				//col.gameObject.GetComponent<EdgeCollider2D>().enabled = false;
			}
			
			else if(col.tag == "Enemy")
			{
				//col.collider2D.enabled = false;
				if(activeShield)
				{
					if(PlaySounds.soundOn)
						PlaySounds.Play_LooseShield();
					enemyCollider = col.transform.GetComponent<Collider2D>();
					enemyCollider.enabled = false;
					Invoke("EnableColliderBackOnEnemy",1f);
					activeShield = false;
					transform.Find("Particles/ShieldDestroyParticle").GetComponent<ParticleSystem>().Play();
					transform.Find("Particles/ShieldDestroyParticle").GetChild(0).GetComponent<ParticleSystem>().Play();
					manage.SendMessage("ApplyPowerUp",-3);

					//if(state != State.running)
					{
						//rigidbody2D.AddForce(new Vector2(rigidbody2D.velocity.x, 1100)); // promenjeno sa 0
						//rigidbody2D.AddForce(new Vector2(maxSpeedX, 1100));
					}

					if(col.name.Equals("Koplje"))
					{
						//col.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.Find("_BabunNadrlja").GetComponent<KillTheBaboon>().
						col.GetComponent<DestroySpearGorilla>().DestroyGorilla();
					}
				}
				else if(!killed && !invincible)
				{
					if(col.gameObject.name.Contains("Biljka"))
					{
						if(PlaySounds.soundOn)
							PlaySounds.Play_BiljkaUgriz();
						col.transform.Find("Biljka_mesozder").GetComponent<Animator>().Play("Attack");
						//col.transform.Find("Biljka_mesozder").GetComponent<Animator>().SetTrigger("Bite");
					}
					else
					{
						if(PlaySounds.soundOn)
							PlaySounds.Play_Siljci();
					}

					transform.Find("Particles/OblakKill").GetComponent<ParticleSystem>().Play();

					if(state == State.running)
					{
						//PlaySounds.Play_SmashBaboon();
						GetComponent<Rigidbody2D>().velocity = Vector2.zero;
						killed = true;
						oblak.Play();
						//Debug.Log("Utepan");
						majmunUtepan();
					}
					else 
					{
						StartCoroutine(FallDownAfterSpikes());
						//rigidbody2D.isKinematic = true;
						
						
					}
				}
			}
			else if(col.tag == "0_Plan")
			{
				col.GetComponent<RunWithSpeed>().enabled = true;
			}
			
			else if(col.tag == "SaZidaNaZid")
			{
				if(saZidaNaZid)
				{
					saZidaNaZid = false;
					state = State.jumped;
				}
				else
					saZidaNaZid = true;
			}
			
			else if(col.tag == "Lijana")
			{
//				grabLianaTransform = col.transform.GetChild(0);
//				lijana = true;
//				state = State.lijana;
//				//cameraFollow.cameraFollowX = false;
//				col.enabled = false;
//				rigidbody2D.isKinematic = true;
//				maxSpeedX = 0;
//				jumpSpeedX = 0;
//				//transform.parent = col.transform.parent;
//				col.transform.parent.GetComponent<Animator>().Play("RotateLianaHolder");
//				animator.Play(lijana_State);
//				StartCoroutine("pratiLijanaTarget",grabLianaTransform);

				if(GetComponent<Rigidbody2D>().drag != 0)
				{
					GetComponent<Rigidbody2D>().drag = 0;
					canGlide = false;
					if(PlaySounds.Glide_NEW.isPlaying)
						PlaySounds.Stop_Glide();
					if(trail.time != 0)
						StartCoroutine(nestaniTrail(2));
				}
				if(powerfullImpact)
				{
					powerfullImpact = false;
					if(PlaySounds.Glide_NEW.isPlaying)
						PlaySounds.Stop_Glide();
					zutiGlowSwooshVisoki.gameObject.SetActive(false);
				}

				animator.SetBool("LianaLetGo",false);
				//grabLianaPosition = col.transform.GetChild(0).position;
				grabLianaTransform = col.GetComponent<LianaAnimationEvent>().lijanaTarget;
				if(!lijana)
				{
					lijana = true;
					animator.Play(lijana_State);
				}
				else
				{
					animator.Play("Lijana_mirror");
					lijana = false;
				}
				state = State.lijana;
				col.enabled = false;
				GetComponent<Rigidbody2D>().isKinematic = true;
				maxSpeedX = 0;
				jumpSpeedX = 0;
				col.transform.GetChild(0).GetComponent<Animator>().Play("Glide_liana");
				//col.GetComponent<LianaAnimationEvent>().OtkaciMajmuna();
				Invoke("OtkaciMajmuna",0.6f);
				
				StartCoroutine("pratiLijanaTarget",grabLianaTransform);
			}
			
		}
	}

	public void Finish()
	{
		canRespawnThings = false;
		state = State.completed;
		if(!inAir)
		{
			if(trail.time != 0)
				StartCoroutine(nestaniTrail(2));
			//col.collider2D.enabled = false;
			cameraFollow.cameraFollowX = false;
			StartCoroutine(ReduceMaxSpeedGradually());
			//Invoke("NotifyManagerForFinish",1.25f);
			//Camera.main.transform.Find("HolderPause").GetChild(0).collider.enabled = false;
			manage.transform.Find("Gameplay Scena Interface/_TopRight/Pause Button").GetComponent<Collider>().enabled = false;
			//GameObject.Find("Pause").collider.enabled = false; // ZA PRVU VERZIJU
			//GameObject.Find("ButtonPause").collider.enabled = false; // ZA FINALNU VERZIJU
			gameObject.layer = LayerMask.NameToLayer("MonkeyShadow");
			//cameraFollow.stopFollow = true;
			//Invoke("TurnOnKinematic",5f);
			Invoke("TurnOnKinematic",8f);
		}
		else
		{
			if(canGlide)
			{
				GetComponent<Rigidbody2D>().drag = 0;
				animator.SetBool("Glide",false);
				disableGlide = false;
				canGlide = false;
				if(PlaySounds.Glide_NEW.isPlaying)
					PlaySounds.Stop_Glide();
			}
		}
	}

	IEnumerator ReduceMaxSpeedGradually()
	{

		while(maxSpeedX > 0.1f)
		{
			yield return null;
			maxSpeedX = Mathf.Lerp(maxSpeedX,0,0.2f);
		}
		maxSpeedX = 0;
		dancing = true;
		GetComponent<Collider2D>().sharedMaterial = finishDontMove;
		//rigidbody2D.mass = 350;

		if(currentBaseState.nameHash == run_State)
		{
			runParticle.Stop();
			trava.Stop();
			animator.SetTrigger("Finished");
		}
		else
		{
			animator.Play("Dancing");
		}
		//rigidbody2D.isKinematic = true;
		Invoke("RestoreMaxSpeed",2.15f);//bilo je 2.15
	}

	void RestoreMaxSpeed()
	{
		//rigidbody2D.isKinematic = false;
		dancing = false;
		GetComponent<Rigidbody2D>().mass = 1.25f;
		animator.SetTrigger("DancingDone");
		maxSpeedX = 16;
		Invoke("NotifyManagerForFinish",1.25f);
	}

	void TurnOnKinematic()
	{
		GetComponent<Rigidbody2D>().isKinematic = true;
	}
	
	IEnumerator pratiLijanaTarget(Transform target)
	{
//		while(lijana)
//		{
//			yield return null;
//			transform.position = Vector3.Lerp(transform.position ,new Vector3(target.position.x,target.position.y, transform.position.z), 0.2f);
//			//transform.position = new Vector3(target.position.x,target.position.y, transform.position.z);
//		}
		while(true)
		{
			yield return null;
			transform.position = Vector3.Lerp(transform.position ,new Vector3(target.position.x,target.position.y, transform.position.z), 0.25f);
		}
	}
	
	public void OtkaciMajmuna()
	{
//		lijana = false;
//		StopCoroutine("pratiLijanaTarget");
//		state = State.jumped;
//		cameraFollow.cameraFollowX = true;
//		maxSpeedX = startSpeedX;
//		jumpSpeedX = startJumpSpeedX;
//		transform.parent = null;
//		transform.rotation = Quaternion.identity;
//		rigidbody2D.isKinematic = false;
//		rigidbody2D.AddForce(new Vector2(200,2500));
//		animator.Play(jump_State);
		if(state == State.lijana)
		{
			StopCoroutine("pratiLijanaTarget");
			state = State.jumped;
			cameraFollow.cameraFollowX = true;
			maxSpeedX = startSpeedX;
			jumpSpeedX = startJumpSpeedX;
			transform.parent = null;
			transform.rotation = Quaternion.identity;
			GetComponent<Rigidbody2D>().isKinematic = false;
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			GetComponent<Rigidbody2D>().AddForce(new Vector2(1000,2500));
			animator.SetBool("LianaLetGo",true);
		}
	}
	
	void SpustiMajmunaSaLijaneBrzo()
	{
//		lijana = false;
//		StopCoroutine("pratiLijanaTarget");
//		state = State.jumped;
//		cameraFollow.cameraFollowX = true;
//		maxSpeedX = startSpeedX;
//		jumpSpeedX = startJumpSpeedX;
//		transform.parent = null;
//		transform.rotation = Quaternion.identity;
//		rigidbody2D.isKinematic = false;
//		animator.Play(jump_State);
		StopCoroutine("pratiLijanaTarget");
		state = State.jumped;
		cameraFollow.cameraFollowX = true;
		maxSpeedX = startSpeedX;
		jumpSpeedX = startJumpSpeedX;
		transform.parent = null;
		transform.rotation = Quaternion.identity;
		GetComponent<Rigidbody2D>().isKinematic = false;
		animator.Play(jump_State);
	}
	
	
	IEnumerator Bounce(float time)
	{
		yield return new WaitForSeconds(time);
		stop = false;
	}

	public void majmunUtepan()
	{
		if(magnet)
			manage.ApplyPowerUp(-1);
		if(doublecoins)
			manage.ApplyPowerUp(-2);
		canRespawnThings = false;
		majmun.GetComponent<Animator>().speed = 1.5f;
		gameObject.layer = LayerMask.NameToLayer("MonkeyShadow");
		//transform.Find("PlayerFocus2D").parent = transform.parent;
		state = State.wasted;
		//Camera.main.transform.Find("HolderPause").GetChild(0).collider.enabled = false;
		manage.transform.Find("Gameplay Scena Interface/_TopRight/Pause Button").GetComponent<Collider>().enabled = false;
		//GameObject.Find("Pause").collider.enabled = false; // ZA PRVU VERZIJU
		//GameObject.Find("ButtonPause").collider.enabled = false; // ZA FINALNU VERZIJU
		//Debug.Log("Pushci animaciju");
		//majmun.animation.Stop();
		//majmun.animation.Play(deathAnimation.name); //ZBOG ANIMATORA!!!!

		//animator.Play("Death1"); //******* DODATA NOVA STANJA U ANIMATOR CONTROLLERU, POZIVA SE ANIMACIJA ISPOD I KORUTINA checkGrounded(), KOJA SADRZI STVARI IZ slowDown() POSLE WHILE
		animator.Play("DeathStart");

		//maxSpeedX = 0;
		//rigidbody2D.AddForce(new Vector2(150,0));
		StartCoroutine(slowDown());
		StartCoroutine(checkGrounded());
		if(trava.isPlaying)
			trava.Stop();
		if(runParticle.isPlaying)
			runParticle.Stop();
		maxSpeedX = 0;
		
		//if(PlaySounds.BackgroundMusic_Gameplay.isPlaying)
		//	PlaySounds.Stop_BackgroundMusic_Gameplay();

	}

	IEnumerator checkGrounded()
	{
		yield return new WaitForSeconds(0.5f);
		while(!grounded)
		{
			yield return null;
		}
		animator.SetTrigger("Grounded");
		if(manage.keepPlayingCount >= 1)
			manage.SendMessage("showFailedScreen");
		else
			manage.SendMessage("ShowKeepPlayingScreen");
		cameraFollow.stopFollow = true;
		GetComponent<Rigidbody2D>().isKinematic = true;
		yield return new WaitForSeconds(1.75f);
		if(!invincible)
			transform.Find("Particles/HolderKillStars").gameObject.SetActive(true);
	}
	
	IEnumerator slowDown()
	{
		float finish = transform.position.x - 5;
		float t = 0;
		while(t < 0.5f)
		{
			transform.position = Vector3.Lerp(transform.position, new Vector3(finish, transform.position.y, transform.position.z),t);
			t+= Time.deltaTime/2;
			yield return null;
		}
		
		//		animator.SetTrigger("Grounded");
		//		GameObject.Find("_GameManager").SendMessage("showFailedScreen");
		//		cameraFollow.stopFollow = true;
		//		rigidbody2D.isKinematic = true;
		//		yield return new WaitForSeconds(1.75f);
		//		if(!invincible)
		//		transform.Find("Particles/HolderKillStars").gameObject.SetActive(true);
	}
	
	string RaycastFunction(Vector3 vector)
	{
		Ray ray = guiCamera.ScreenPointToRay(vector);
		RaycastHit hit;
		if(Physics.Raycast(ray,out hit))
		{
			return hit.collider.name;
		}
		ray = Camera.main.ScreenPointToRay(vector);
		if(Physics.Raycast(ray,out hit))
		{
			return hit.collider.name;
		}
		return "";
	}
	
	IEnumerator turnHead(float step)
	{
		float t = 0;
		while(t < 0.175f)
		{
			animator.SetLookAtWeight(lookWeight);
			animator.SetLookAtPosition(lookAtPos.position);
			lookWeight = Mathf.Lerp(lookWeight,1,t);
			t += step*Time.deltaTime;
			yield return null;
		}
		t=0;
		while(t < 0.175f)
		{
			animator.SetLookAtWeight(lookWeight);
			animator.SetLookAtPosition(lookAtPos.position);
			lookWeight = Mathf.Lerp(lookWeight,0,t);
			t += step*Time.deltaTime;
			yield return null;
		}
	}
	IEnumerator easyStop()
	{
		//Debug.Log("STAOOOOOO");
		while(maxSpeedX > 0 && usporavanje)
		{
			//Debug.Log("Usporeoooo");
			if(maxSpeedX < 0.5f)
			{
				maxSpeedX = 0;
				//rigidbody2D.velocity = Vector2.zero;
			}
			maxSpeedX = Mathf.Lerp(maxSpeedX, 0, 10*Time.deltaTime);
			yield return null;
		}
	}
	IEnumerator easyGo()
	{
		
		while(maxSpeedX < startSpeedX && !usporavanje)
		{
			//Debug.Log("Ubrzao");
			if(maxSpeedX > startSpeedX - 0.5f)
				maxSpeedX = startSpeedX;
			maxSpeedX = Mathf.Lerp(maxSpeedX, startSpeedX, 10*Time.deltaTime);
			yield return null;
		}
	}

	IEnumerator reappearItem(GameObject obj)
	{
		if(canRespawnThings)
		{
			yield return new WaitForSeconds(5.5f);
			obj.SetActive(true);
		}
	}

	IEnumerator nestaniTrail(float step)
	{
		float t = 0;
		while(t < 0.4f)
		{
			trail.time = Mathf.Lerp(trail.time,0,t);
			t += Time.deltaTime*5;
			yield return null;
		}
		trail.time = 0;
	}

	void PustiVoiceJump()
	{
		PlaySounds.Play_VoiceJump();
	}

	public void SetInvincible()
	{
		jumpSpeedX = Mathf.Abs(jumpSpeedX);
		moveForce = Mathf.Abs(moveForce);
		maxSpeedX = Mathf.Abs(maxSpeedX);
		majmun.localScale = new Vector3(majmun.localScale.x, majmun.localScale.y, Mathf.Abs(majmun.localScale.z));

		if(misijaSaDistance)
			measureDistance = true;
		//animator.SetTrigger("Invincible");
		invincible = true;
		StartCoroutine(blink());
		//Camera.main.transform.Find("HolderPause").GetChild(0).collider.enabled = true;
		//manage.transform.Find("Gameplay Scena Interface/_TopRight/Pause Button").collider.enabled = true;
		StopCoroutine(slowDown());
		killed = false;
		transform.Find("Particles/HolderKillStars").gameObject.SetActive(false);
		animator.Play(jump_State);
		grounded = false;
		proveraGround = 16;
		maxSpeedX = startSpeedX;
		state = State.jumped;
		GetComponent<Rigidbody2D>().isKinematic = false;
		//rigidbody2D.AddForce(new Vector2(1500,1000));
		GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeedX,50);
		cameraFollow.stopFollow = false;

	}

	IEnumerator blink()
	{
		float t=0;
		int i = 0;
		bool radi = false;
		Renderer meshrenderer = majmun.Find("PorinceGorilla_LP").GetComponent<Renderer>();
		Renderer usi = majmun.Find("Usi").GetComponent<Renderer>();
		Renderer kosa = majmun.Find("Kosa").GetComponent<Renderer>();
		Renderer glava = null;
		Renderer majica = null;
		Renderer ledja = null;
		gameObject.layer = LayerMask.NameToLayer("Monkey3D");

		if(StagesParser.glava != -1)
		{
			glava = majmun.Find("ROOT/Hip/Spine/Chest/Neck/Head/"+StagesParser.glava).GetChild(0).GetComponent<Renderer>();
		}
		if(StagesParser.majica != -1)
		{
			majica = majmun.Find("custom_Majica").GetComponent<Renderer>();
		}
		if(StagesParser.ledja != -1)
		{
			ledja = majmun.Find("ROOT/Hip/Spine/"+StagesParser.ledja).GetChild(0).GetComponent<Renderer>();
		}

		while(t<1)
		{
			if(StagesParser.imaUsi)
				usi.enabled = radi;
			if(StagesParser.imaKosu)
				kosa.enabled = radi;
			if(StagesParser.glava != -1)
			{
				glava.enabled = radi;
			}
			if(StagesParser.majica != -1)
			{
				majica.enabled = radi;
			}
			if(StagesParser.ledja != -1)
			{
				ledja.enabled = radi;
			}
			meshrenderer.enabled = radi;
			if(i == 3)
			{
				radi = !radi;
				i=0;
			}
			i++;
			t+=Time.deltaTime/3;
			yield return null;
		}
		canRespawnThings = true;
		//gameObject.layer = LayerMask.NameToLayer("Monkey3D");
		meshrenderer.enabled = true;
		usi.enabled = true;
		kosa.enabled = true;
		if(glava != null)
		glava.enabled = true;
		if(majica != null)
		majica.enabled = true;
		if(ledja != null)
		ledja.enabled = true;
		invincible = false;
	}

	IEnumerator RemoveFog(float time, Collider2D col)
	{
		yield return new WaitForSeconds(time);
		col.enabled = true;
		col.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
		Camera.main.GetComponent<Animator>().Play("FogOfWar_Remove");
	}
	
	void DisableImpact()
	{
		transform.Find("Impact").gameObject.SetActive(false);
	}
	
	public void cancelPowerfullImpact()
	{
		//transform.Find("Impact").gameObject.SetActive(true);
		//Invoke("DisableImpact",0.25f);
		powerfullImpact = false;
		if(PlaySounds.Glide_NEW.isPlaying)
			PlaySounds.Stop_Glide();
		zutiGlowSwooshVisoki.gameObject.SetActive(false);
		//majmun.Find("PorinceGorilla_LP").renderer.material.color = new Color(0.53f,0.53f,0.53f,1);
		//Camera.main.GetComponent<Animator>().Play("CameraShakeTrasGround");
		//izrazitiPad.Play();
	}
	
	void reaktivirajVoiceJump()
	{
		pustenVoiceJump = false;
	}
	
	IEnumerator pojaviMaglu(Collider2D col)
	{
		Transform fogOfWar = Camera.main.transform.Find("FogOfWar");
		float value = fogOfWar.localScale.x;
		float target = 12;
		float t=0;
		while(t<1)
		{
			fogOfWar.localScale = new Vector3(Mathf.Lerp(fogOfWar.localScale.x,target,t), fogOfWar.localScale.y, fogOfWar.localScale.z);
			//fogOfWar.localScale = Vector3.Lerp(fogOfWar.localScale, new Vector3(target,fogOfWar.localScale.y,fogOfWar.localScale.z),t);
			t += Time.deltaTime/1.2f;
			yield return null;
		}
		col.enabled = true;
		yield return new WaitForSeconds(5f);
		t=0;
		while(t<1)
		{
			fogOfWar.localScale = new Vector3(Mathf.Lerp(fogOfWar.localScale.x,value,t), fogOfWar.localScale.y, fogOfWar.localScale.z);
			//fogOfWar.localScale = Vector3.Lerp(fogOfWar.localScale, new Vector3(target,fogOfWar.localScale.y,fogOfWar.localScale.z),t);
			t += Time.deltaTime/1.2f;
			yield return null;
		}
		col.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
		fogOfWar.GetComponent<Renderer>().enabled = false;
	}

	IEnumerator Spusti2DFollow()
	{
		Vector3 target = cameraTarget.transform.localPosition-new Vector3(0,4.5f,0);
		while(cameraTarget.transform.localPosition.y >= target.y + 0.1f)
		{
			yield return null;
			cameraTarget.transform.localPosition = new Vector3(cameraTarget.transform.localPosition.x, Mathf.MoveTowards(cameraTarget.transform.localPosition.y,target.y,0.2f), cameraTarget.transform.localPosition.z);
		}
	}

	IEnumerator Podigni2DFollow()
	{
		while(cameraTarget.transform.localPosition.y <= originalCameraTargetPosition - 0.1f)
		{
			yield return null;
			cameraTarget.transform.localPosition = new Vector3(cameraTarget.transform.localPosition.x, Mathf.MoveTowards(cameraTarget.transform.localPosition.y,originalCameraTargetPosition,0.1f), cameraTarget.transform.localPosition.z);
		}
		cameraTarget.transform.localPosition = new Vector3(cameraTarget.transform.localPosition.x, originalCameraTargetPosition, cameraTarget.transform.localPosition.z);
	}

	void ReturnFromMushroom()
	{
		GetComponent<Rigidbody2D>().isKinematic = false;
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
	}
}

