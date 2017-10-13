using UnityEngine;
using System.Collections;

public class MonkeyController2D_new : MonoBehaviour {
	
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
	bool jumpSafetyCheck			= false;
	bool proveriTerenIspredY		= false;
	bool downHit					= false;
	[HideInInspector]
	public bool zgaziEnemija		= false;
	[HideInInspector]
	public bool killed				= false;
	
	[HideInInspector]
	public bool stop				= false;
	bool triggerCheckDown			= false;
	[HideInInspector]
	public bool triggerCheckDownTrigger	= false;
	[HideInInspector]
	public bool triggerCheckDownBehind		= false;
	
	bool CheckWallHitNear			= false;
	bool CheckWallHitNear_low		= false;
	
	bool startSpustanje				= false;
	bool startPenjanje				= false;
	
	bool spustanjeRastojanje		= false;
	float pocetniY_spustanje;
	
	[HideInInspector]
	public float collisionAngle			= 0;
	float duzinaPritiskaZaSkok;
	bool mozeDaSkociOpet			= false;
	
	float startY;
	float endX;
	float endY;
	bool swoosh;
	bool grab 						= false;
	bool snappingToClimb			= false;
	Vector3 colliderForClimb;
	
	public bool Glide;
	public bool DupliSkok;
	public bool KontrolisaniSkok;
	public bool SlideNaDole;
	public bool Zaustavljanje;
	
	Ray2D ray;
	RaycastHit2D hit;
	
	Transform groundCheck;
	Transform majmun;
	public ParticleSystem trava;
	public ParticleSystem oblak;
	public ParticleSystem particleSkok;
	//public ParticleSystem dupliSkokEfekat;
	//public ParticleSystem dupliSkokEfekat2;
	public ParticleSystem dupliSkokOblaci;
	public ParticleSystem runParticle;
	public ParticleSystem klizanje;
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
	Animator parentAnim;
	
	[HideInInspector]
	public string lastPlayedAnim;
	bool helpBool;
	
	
	AnimatorStateInfo currentBaseState;
	int run_State = Animator.StringToHash("Base Layer.Running");
	int jump_State = Animator.StringToHash("Base Layer.Jump_Start");
	int fall_State = Animator.StringToHash("Base Layer.Jump_Falling");
	int landing_State = Animator.StringToHash("Base Layer.Landing");
	int doublejump_State = Animator.StringToHash("Base Layer.DoubleJump_Start");
	int glide_start_State = Animator.StringToHash("Base Layer.Glide_Start");
	int glide_loop_State = Animator.StringToHash("Base Layer.Glide_Loop");
	int grab_State = Animator.StringToHash("Base Layer.Grab");
	int wall_stop_State = Animator.StringToHash("Base Layer.WallStop");
	int wall_stop_from_jump_State = Animator.StringToHash("Base Layer.WallStop_From_Jump");
	int swoosh_State = Animator.StringToHash("Base Layer.Swoosh_Down");
	int spikedeath_State = Animator.StringToHash("Base Layer.Spike_death");
	int lijana_State = Animator.StringToHash("Base Layer.Lijana");
	
	Transform lookAtPos;
	float lookWeight = 0;
	bool disableGlide = false;
	bool helper_disableMoveAfterGrab = false;
	bool usporavanje = false;
	bool sudarioSeSaZidom = false;
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
	bool grounded = false;
	float startVelY;
	float korakce;
	bool canGlide = false;
	
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
		parentAnim = majmun.parent.GetComponent<Animator>();
		#if UNITY_EDITOR
		//QualitySettings.vSyncCount = 2;
		#endif
	}
	
	void Start ()
	{
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
		
		//RaycastHit2D hit = Physics2D.Linecast(transform.position + new Vector3(0.8f,2.5f,0), transform.position + new Vector3(0.8f,-15.5f,0), 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Platform"));
		//Debug.DrawLine(transform.position + new Vector3(0.8f,2.5f,0), transform.position + new Vector3(0.8f,-5.5f,0),Color.white);
		//GameObject.Find("shadowMonkey").transform.position = new Vector3(GameObject.Find("shadowMonkey").transform.position.x,hit.point.y,GameObject.Find("shadowMonkey").transform.position.z);
		//razmrk = transform.position.y - GameObject.Find("shadowMonkey").transform.position.y;
	}
	
	void Update()
	{
		
		hit = Physics2D.Linecast(transform.position + new Vector3(0.8f,2.5f,0), transform.position + new Vector3(0.8f,-15.5f,0), 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Platform"));
		if(hit)
		{
			senka.position = new Vector3(senka.position.x,hit.point.y-0.3f,senka.position.z);
			senka.localScale = Vector3.one;
			senka.rotation = Quaternion.Euler(0,0,Mathf.Asin(-hit.normal.x)*180/Mathf.PI);
		}
		else
		{
			senka.localScale = Vector3.zero;
		}
		
		if(snappingToClimb)
		{
			StartCoroutine(snappingProcess());
			snappingToClimb = false;
		}
		
		if(grab)
		{
			StartCoroutine(ProceduraPenjanja(null));
			grab = false;
		}
		
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
		
		if(currentBaseState.nameHash == doublejump_State)
		{
			if(animator.GetBool("DoubleJump"))
				animator.SetBool("DoubleJump",false);
		}
		
		
		else if(currentBaseState.nameHash == fall_State)
		{
			if(animator.GetBool("Falling"))
				animator.SetBool("Falling",false);
		}
		else if(currentBaseState.nameHash == glide_loop_State)
		{
			if(Time.frameCount % 60 == 0)
			{
				if(Random.Range(1,100) <= 10)
					StartCoroutine(turnHead(0.1f));
			}
		}
		//za 3D majmuna
		//		CheckWallHitNear 			= Physics2D.Linecast(transform.position + new Vector3(2f,0.8f,0), transform.position + new Vector3(3f,0.8f,0), (1 << LayerMask.NameToLayer("WallHit")));// | (1 << LayerMask.NameToLayer("Ground")));
		//		CheckWallHitNear_low 		= Physics2D.Linecast(transform.position + new Vector3(2f,-0.5f,0), transform.position + new Vector3(3f,-0.5f,0), (1 << LayerMask.NameToLayer("WallHit")));
		//		sideHitUp 					= Physics2D.Linecast(transform.position + new Vector3(2.2f,1.5f,0), transform.position + new Vector3(4.75f,1.5f,0), 1 << LayerMask.NameToLayer("Ground"));
		//		triggerCheckDown 			= Physics2D.Linecast(transform.position + new Vector3(2.2f,1.5f,0), transform.position + new Vector3(2.2f,-1.5f,0), 1 << LayerMask.NameToLayer("Ground"));
		//		triggerCheckDownTrigger 	= Physics2D.Linecast(transform.position + new Vector3(2.2f,1.5f,0), transform.position + new Vector3(2.2f,-3f,0), 1 << LayerMask.NameToLayer("Ground"));
		//		triggerCheckDownBehind	 	= Physics2D.Linecast(transform.position + new Vector3(0.2f,1.5f,0), transform.position + new Vector3(0.2f,-3f,0), 1 << LayerMask.NameToLayer("Ground"));
		//		proveriTerenIspredY			= Physics2D.Linecast(transform.position + new Vector3(5.5f,0,0), transform.position + new Vector3(5.5f,-4f,0), 1 << LayerMask.NameToLayer("Ground"));
		//		downHit						= Physics2D.Linecast(transform.position + new Vector3(1.25f,-0.75f,0), transform.position + new Vector3(1.25f,- 1.5f,0), 1 << LayerMask.NameToLayer("Ground"));
		//		spustanjeRastojanje 		= Physics2D.Linecast(transform.position + new Vector3(3.3f,0,0), transform.position + new Vector3(3.3f,-Camera.main.orthographicSize,0), 1 << LayerMask.NameToLayer("Ground"));
		//
		//		Debug.DrawLine(transform.position + new Vector3(2.2f,1.5f,0), transform.position + new Vector3(4.75f,1.5f,0), Color.red); //sideHitUp
		//		Debug.DrawLine(transform.position + new Vector3(2.2f,1.5f,0), transform.position + new Vector3(2.2f,-1.5f,0), Color.magenta); //check trigger down, bilo je -3 umesto -1.5, i gore isto
		//		Debug.DrawLine(transform.position + new Vector3(0.2f,1.5f,0), transform.position + new Vector3(0.2f,-3f,0), Color.magenta); //check trigger down behind, bilo je -3 umesto -1.5, i gore isto
		//		Debug.DrawLine(transform.position + new Vector3(2f,0.8f,0), transform.position + new Vector3(3f,0.8f,0), Color.cyan); //check for wall front
		//		Debug.DrawLine(transform.position + new Vector3(2f,-0.5f,0), transform.position + new Vector3(3f,-0.5f,0), Color.cyan); //check for wall front low
		//		Debug.DrawLine(transform.position + new Vector3(5.5f,0,0), transform.position + new Vector3(5.5f,-4f,0),Color.white); //proveri teren ispred y
		//		Debug.DrawLine(transform.position + new Vector3(1.25f,-0.75f,0), transform.position + new Vector3(1.25f,- 1.5f,0),Color.green); //downHit
		//		Debug.DrawLine(transform.position + new Vector3(3.3f,0,0), transform.position + new Vector3(3.3f,-Camera.main.orthographicSize,0), Color.blue);
		//		Debug.DrawLine(transform.position + new Vector3(1f,0,0), transform.position + new Vector3(1f,2f,0), Color.blue);
		
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		CheckWallHitNear 			= Physics2D.Linecast(transform.position + new Vector3(0.8f,2.5f,0), transform.position + new Vector3(3.5f,2.5f,0), (1 << LayerMask.NameToLayer("WallHit")));// | (1 << LayerMask.NameToLayer("Ground")));
		CheckWallHitNear_low 		= Physics2D.Linecast(transform.position + new Vector3(0.8f,0f,0), transform.position + new Vector3(3.5f,0f,0), (1 << LayerMask.NameToLayer("WallHit")));
		//triggerCheckDown 			= Physics2D.Linecast(transform.position + new Vector3(1.1f,2.5f,0), transform.position + new Vector3(1.1f,-0.5f,0), 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Platform"));
		triggerCheckDown 			= Physics2D.Linecast(transform.position + new Vector3(0.8f,2.5f,0), transform.position + new Vector3(0.8f,-0.5f,0), 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Platform"));
		triggerCheckDownTrigger 	= Physics2D.Linecast(transform.position + new Vector3(0.8f,2.5f,0), transform.position + new Vector3(0.8f,-4.5f,0), 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Platform"));
		triggerCheckDownBehind	 	= Physics2D.Linecast(transform.position + new Vector3(-0.8f,2.5f,0), transform.position + new Vector3(-0.8f,-4.5f,0), 1 << LayerMask.NameToLayer("Platform"));
		proveriTerenIspredY			= Physics2D.Linecast(transform.position + new Vector3(4.4f,1.2f,0), transform.position + new Vector3(4.4f,-3.2f,0), 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Platform"));
		downHit						= Physics2D.Linecast(transform.position + new Vector3(0.2f,0.1f,0), transform.position + new Vector3(0.2f,-0.65f,0), 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Platform"));
		spustanjeRastojanje 		= Physics2D.Linecast(transform.position + new Vector3(2.3f,1.25f,0), transform.position + new Vector3(2.3f,-Camera.main.orthographicSize,0), 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Platform"));
		
		//		Debug.DrawLine(transform.position + new Vector3(0.8f,2.5f,0), transform.position + new Vector3(3.5f,2.5f,0), Color.cyan); //check for wall front
		//		Debug.DrawLine(transform.position + new Vector3(0.8f,0f,0), transform.position + new Vector3(3.5f,0f,0), Color.cyan); //check for wall front low
		//		Debug.DrawLine(transform.position + new Vector3(1.1f,2.5f,0), transform.position + new Vector3(3.65f,2.5f,0), Color.red); //sideHitUp
		//		Debug.DrawLine(transform.position + new Vector3(1.1f,2.5f,0), transform.position + new Vector3(1.1f,-0.5f,0), Color.magenta); //check trigger down, bilo je -3 umesto -1.5, i gore isto
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
				//				if(Glide)
				//					povrsinaZaClick = Screen.width/2;
				//				else
				//					povrsinaZaClick = 0;
				
				if((Input.GetMouseButtonDown(0) && Input.mousePosition.x > povrsinaZaClick) || Input.GetKeyDown(KeyCode.Space))
				{
					if(mozeDaSkociOpet)
					{
						if(hasJumped)
						{
							parentAnim.Play("DoubleJumpRotate");
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
					//Debug.Log(Input.GetTouch(1).position.x + " > " + Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2,Screen.height,0)).x);
					if(mozeDaSkociOpet)
					{
						
						if(hasJumped)
						{
							disableGlide = true;
							animator.SetBool("DoubleJump",true);
							parentAnim.Play("DoubleJumpRotate");
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
					//Debug.Log("Trenutak pritiska: " + duzinaPritiskaZaSkok);
					startY = Input.mousePosition.y;
					canGlide = true;
				}
				
				
				else if((Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space)))// && Input.mousePosition.x < Screen.width/2) 
				{
					startY = endY = 0;
					GetComponent<Rigidbody2D>().drag = 0;
					animator.SetBool("Glide",false);
					disableGlide = false;
					canGlide = false;
				}
				
				else if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
				{
					startY = endY = 0;
					GetComponent<Rigidbody2D>().drag = 0;
					animator.SetBool("Glide",false);
					canGlide = false;
				}
			}
			
			if((Input.GetMouseButton(0)))// && Input.mousePosition.x < Screen.width/2)
			{
				endY = Input.mousePosition.y;
				if(SlideNaDole)
				{
					if((startY - endY) > ( (1f/4f*Camera.main.orthographicSize)*(Screen.height/(2*Camera.main.orthographicSize))))
					{
						//rigidbody2D.gravityScale = 20;
						swoosh = true;
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
						GetComponent<Rigidbody2D>().drag = 7.5f;						
					}
				}
				
			}
			
			if(KontrolisaniSkok)
			{
				if(Input.GetMouseButton(0) && jumpControlled)
				{
					GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y + korakce);
					if(korakce > 0)
						korakce -= 0.085f;
				}
				
				if((Input.GetMouseButtonUp(0) || Time.time - duzinaPritiskaZaSkok > 1f) && jumpControlled) //njanjanjanja 0.35 //zadnje 0.3
				{
					//Debug.Log("Gotovo!!!");
					//Debug.Log("time: " + (Time.time - duzinaPritiskaZaSkok));
					//duzinaPritiskaZaSkok = 0;
					jumpControlled = false;
					tempForce = jumpForce;
					canGlide = false;
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
				//				if(Glide)
				//					povrsinaZaClick = Screen.width/2;
				//				else
				//					povrsinaZaClick = 0;
				
				if((Input.GetMouseButtonDown(0) && Input.mousePosition.x > povrsinaZaClick) || Input.GetKeyDown(KeyCode.Space))
				{
					//if(heCanJump && RaycastFunction(Input.mousePosition) != "Pause" && RaycastFunction(Input.mousePosition) != "HoleButtonsPlay_Pause" && RaycastFunction(Input.mousePosition) != "PauseHoleShop" && RaycastFunction(Input.mousePosition) != "PauseHoleFreeCoins" && RaycastFunction(Input.mousePosition) != "PowersCardShield" && RaycastFunction(Input.mousePosition) != "PowersCardMagnet" && RaycastFunction(Input.mousePosition) != "PowersCardCoinx2")
					if(RaycastFunction(Input.mousePosition) != "Pause" && RaycastFunction(Input.mousePosition) != "HoleButtonsPlay_Pause" && RaycastFunction(Input.mousePosition) != "Buy Button" && RaycastFunction(Input.mousePosition) != "ButtonMain_NoMoreLives" && RaycastFunction(Input.mousePosition) != "HoleButtonsRestart_Pause" && !RaycastFunction(Input.mousePosition).Contains("Tutorial")) // ZA PRVU VERZIJU
					{
						duzinaPritiskaZaSkok = Time.time;
						//Debug.Log("start: " + duzinaPritiskaZaSkok);
						
						if(!inAir)
						{
							//rigidbody2D.gravityScale = 10;
							state = State.climbUp;
							if(PlaySounds.soundOn)
							{
								PlaySounds.Stop_Run();
								PlaySounds.Play_Jump();
							}
							jumpControlled = true;
							animator.Play(jump_State);
							animator.SetBool("Landing",false);
							jumpSafetyCheck = true;
							animator.SetBool("WallStop",false);
							inAir = true;
							tempForce = jumpForce;
							particleSkok.Emit(20);
						}
					}
				}
				
				if((Input.GetMouseButton(0) && Input.mousePosition.x > povrsinaZaClick) || Input.GetKey(KeyCode.Space))
				{
					
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
					if(RaycastFunction(Input.mousePosition) != "Pause" && RaycastFunction(Input.mousePosition) != "HoleButtonsPlay_Pause" && RaycastFunction(Input.mousePosition) != "Buy Button" && RaycastFunction(Input.mousePosition) != "ButtonMain_NoMoreLives" && RaycastFunction(Input.mousePosition) != "HoleButtonsRestart_Pause" && !RaycastFunction(Input.mousePosition).Contains("Tutorial")) // ZA PRVU VERZIJU
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
							jumpSafetyCheck = true;
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
			if(Time.frameCount % 300 == 0)
			{
				if(Random.Range(1,100) <= 25)
					StartCoroutine(turnHead(0.1f));
			}
			
			if(KontrolisaniSkok)
			{
				//				if(Glide)
				//					povrsinaZaClick = Screen.width/2;
				//				else
				//					povrsinaZaClick = 0;
				
				if((Input.GetMouseButtonDown(0) && Input.mousePosition.x > povrsinaZaClick) || Input.GetKeyDown(KeyCode.Space))
				{
					if(RaycastFunction(Input.mousePosition) != "Pause" && RaycastFunction(Input.mousePosition) != "HoleButtonsPlay_Pause" && RaycastFunction(Input.mousePosition) != "Buy Button" && RaycastFunction(Input.mousePosition) != "ButtonMain_NoMoreLives" && RaycastFunction(Input.mousePosition) != "HoleButtonsRestart_Pause" && !RaycastFunction(Input.mousePosition).Contains("Tutorial")) // ZA PRVU VERZIJU
						//if(heCanJump && RaycastFunction(Input.mousePosition) != "ButtonPause" && RaycastFunction(Input.mousePosition) != "PauseHolePlay" && RaycastFunction(Input.mousePosition) != "PauseHoleShop" && RaycastFunction(Input.mousePosition) != "PauseHoleFreeCoins" && RaycastFunction(Input.mousePosition) != "PowersCardShield" && RaycastFunction(Input.mousePosition) != "PowersCardMagnet" && RaycastFunction(Input.mousePosition) != "PowersCardCoinx2") // ZA FINALNU VERZIJU
					{	
						//rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
						startVelY = GetComponent<Rigidbody2D>().velocity.y;
						korakce = 3;
						GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x,startVelY + maxSpeedY);
						neTrebaDaProdje = false;
						duzinaPritiskaZaSkok = Time.time;
						//Debug.Log("start: " + duzinaPritiskaZaSkok);
						//Debug.Log("Usklikno");
						//rigidbody2D.gravityScale = 10;
						state = State.jumped;
						if(PlaySounds.soundOn)
						{
							PlaySounds.Stop_Run();
							PlaySounds.Play_Jump();
						}
						jumpControlled = true;
						animator.Play(jump_State);
						animator.SetBool("Landing",false);
						jumpSafetyCheck = true;
						inAir = true;
						particleSkok.Emit(20);
					}
				}
				
				if((Input.GetMouseButton(0) && Input.mousePosition.x > povrsinaZaClick) || Input.GetKey(KeyCode.Space))
				{
					
				}
				if((Input.GetMouseButtonUp(0) || Time.time - duzinaPritiskaZaSkok > 2.0f) && jumpControlled) //0.275
				{
					//Debug.Log("time: " + (Time.time - duzinaPritiskaZaSkok));
					//duzinaPritiskaZaSkok = 0;
					jumpControlled = false;
					tempForce = jumpForce;
					//if(rigidbody2D.velocity.y > 3)
					//	rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 3);
				}
				
			}
			else
			{
				//				if(Glide)
				//					povrsinaZaClick = Screen.width/2;
				//				else
				//					povrsinaZaClick = 0;
				
				if((Input.GetMouseButtonDown(0) && Input.mousePosition.x > povrsinaZaClick) || Input.GetKeyDown(KeyCode.Space))
				{
					if(RaycastFunction(Input.mousePosition) != "Pause" && RaycastFunction(Input.mousePosition) != "HoleButtonsPlay_Pause" && RaycastFunction(Input.mousePosition) != "Buy Button" && RaycastFunction(Input.mousePosition) != "ButtonMain_NoMoreLives" && RaycastFunction(Input.mousePosition) != "HoleButtonsRestart_Pause" && !RaycastFunction(Input.mousePosition).Contains("Tutorial")) // ZA PRVU VERZIJU
						//if(heCanJump && RaycastFunction(Input.mousePosition) != "ButtonPause" && RaycastFunction(Input.mousePosition) != "PauseHolePlay" && RaycastFunction(Input.mousePosition) != "PauseHoleShop" && RaycastFunction(Input.mousePosition) != "PauseHoleFreeCoins" && RaycastFunction(Input.mousePosition) != "PowersCardShield" && RaycastFunction(Input.mousePosition) != "PowersCardMagnet" && RaycastFunction(Input.mousePosition) != "PowersCardCoinx2") // ZA FINALNU VERZIJU
					{
						neTrebaDaProdje = false;
						//rigidbody2D.gravityScale = 10;
						state = State.jumped;
						if(PlaySounds.soundOn)
						{
							PlaySounds.Stop_Run();
							PlaySounds.Play_Jump();
						}
						jump = true;
						//animator.SetBool("Jump",true);
						animator.Play(jump_State);
						animator.SetBool("Landing",false);
						jumpSafetyCheck = true;
						inAir = true;
						particleSkok.Emit(20);
					}
				}
			}
			if(Zaustavljanje && povrsinaZaClick != 0)
			{
				if(Input.GetMouseButton(0)/* && Input.mousePosition.x < Screen.width/2*/)
				{
					//Debug.Log("Treba da stane i speed je: " + maxSpeedX);
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
			//if(SlideNaDole)
			//	povrsinaZaClick = Screen.width/2;
			//else
			povrsinaZaClick = 0;
			
			if(Input.GetMouseButtonDown(0) && Input.mousePosition.x > povrsinaZaClick)
			{
				if(heCanJump && RaycastFunction(Input.mousePosition) != "ButtonPause" && RaycastFunction(Input.mousePosition) != "PauseHolePlay" && RaycastFunction(Input.mousePosition) != "PauseHoleShop" && RaycastFunction(Input.mousePosition) != "PauseHoleFreeCoins" && RaycastFunction(Input.mousePosition) != "PowersCardShield" && RaycastFunction(Input.mousePosition) != "PowersCardMagnet" && RaycastFunction(Input.mousePosition) != "PowersCardCoinx2" && !RaycastFunction(Input.mousePosition).Contains("Tutorial"))
				{
					if(Input.mousePosition.x < Screen.width/2)
					{
						startY = Input.mousePosition.y;
					}
					else
						OtkaciMajmuna();
				}
			}
			
			if(SlideNaDole)
			{
				if(Input.GetMouseButton(0)/* && Input.mousePosition.x < Screen.width/2*/)
				{
					endY = Input.mousePosition.y;
					if(SlideNaDole)
					{
						if((startY - endY) > ( (1f/4f*Camera.main.orthographicSize)*(Screen.height/(2*Camera.main.orthographicSize))))
						{
							//rigidbody2D.gravityScale = 20;
							SpustiMajmunaSaLijaneBrzo();
							swoosh = true;
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
				if(heCanJump && RaycastFunction(Input.mousePosition) != "ButtonPause" && RaycastFunction(Input.mousePosition) != "PauseHolePlay" && RaycastFunction(Input.mousePosition) != "PauseHoleShop" && RaycastFunction(Input.mousePosition) != "PauseHoleFreeCoins" && RaycastFunction(Input.mousePosition) != "PowersCardShield" && RaycastFunction(Input.mousePosition) != "PowersCardMagnet" && RaycastFunction(Input.mousePosition) != "PowersCardCoinx2" && !RaycastFunction(Input.mousePosition).Contains("Tutorial"))
				{
					if(!inAir)
					{
						neTrebaDaProdje = false;
						//rigidbody2D.gravityScale = 10;
						//state = State.jumped;
						if(PlaySounds.soundOn)
						{
							PlaySounds.Stop_Run();
							PlaySounds.Play_Jump();
						}
						jump = true;
						//animator.SetBool("Jump",true);
						animator.Play(jump_State);
						animator.SetBool("Landing",false);
						jumpSafetyCheck = true;
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
				//rigidbody2D.gravityScale = 10;
				state = State.saZidaNaZid;
				animator.Play(jump_State);
				animator.SetBool("Landing",false);
				animator.SetBool("WallStop",false);
				particleSkok.Emit(20);
				jumpSafetyCheck = true;
			}
		}
		//Debug.Log("maxSpeedX: " + maxSpeedX + ", speedX: " + rigidbody2D.velocity.x + ", speedY: " + rigidbody2D.velocity.y);
	}
	
	void FixedUpdate ()
	{
		//Debug.Log("pitch: " + PlaySounds.Run.pitch);
		currentBaseState = animator.GetCurrentAnimatorStateInfo(0);
		//Debug.Log("anim state: " + currentBaseState.nameHash + ", run: " + run_State + ", jump: " + jump_State + ", fall: " + fall_State + ", land: " + landing_State + ", double: " + doublejump_State + ",grab: " + grab_State + ", wall: " + wall_stop_State);
		
		//Debug.Log ("grounded: " + grounded + ", in air: " + inAir + ", side hit: " + sideHit + ", v: " + rigidbody2D.velocity.y + ", jump: " + jump + ", gravity: " + rigidbody2D.gravityScale);
		//Debug.Log(state + ", pileci skok:" + jumpSafetyCheck + ", check down:" + triggerCheckDown + ", v: " + rigidbody2D.velocity.y + ", in air: " + inAir + ", tforce: " + tempForce + ", speed: " + rigidbody2D.velocity.x);
		//Debug.Log("Jump safety check: " + jumpSafetyCheck + ", state: " + state);
		//Debug.Log("WallNearLow: " + CheckWallHitNear_low + ", WallNearHigh: " + CheckWallHitNear);
		
		
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
					GetComponent<Rigidbody2D>().AddForce(new Vector2(GetComponent<Rigidbody2D>().velocity.x, 2500)); //promenjeno sa 0
				}
				
				if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > maxSpeedY)
					GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, maxSpeedY);
				
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
					GetComponent<Rigidbody2D>().AddForce(new Vector2(GetComponent<Rigidbody2D>().velocity.x, 2500)); //promenjeno sa 0
				}
				
				if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > maxSpeedY)
					GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, maxSpeedY);
				
				jump = false;
				
			}
			GetComponent<Rigidbody2D>().velocity = new Vector2(jumpSpeedX, GetComponent<Rigidbody2D>().velocity.y);
			
			if(GetComponent<Rigidbody2D>().velocity.y < -0.01f)
				GetComponent<Rigidbody2D>().drag = 5;
		}
		
		
		//------------------------------------------------------------------------
		else if(state == State.wasted) 		// ************* Player is wasted
		{
			if(GetComponent<Rigidbody2D>().velocity.x < maxSpeedX && !stop)
			{
				GetComponent<Rigidbody2D>().AddForce(Vector2.right * moveForce);
			}
			
			if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeedX && !stop)
				GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeedX, GetComponent<Rigidbody2D>().velocity.y);
			
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
			if(GetComponent<Rigidbody2D>().velocity.x < maxSpeedX && !stop)
			{
				GetComponent<Rigidbody2D>().AddForce(Vector2.right * moveForce);
			}
			
			if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeedX && !stop)
				GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeedX, GetComponent<Rigidbody2D>().velocity.y);
			
			if(triggerCheckDown)
				animator.Play(run_State);
			
		}
		
		
		
		//------------------------------------------------------------------------
		else if(state == State.jumped) 	// ************* Player is in the air
		{
			
			if(swoosh)
			{
				//rigidbody2D.AddForce(new Vector2(0, -jumpForce*2));
				GetComponent<Rigidbody2D>().AddForce(new Vector2(GetComponent<Rigidbody2D>().velocity.x,-3000)); //promenjeno sa 0
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
			
			//			else if(jump)
			//			{
			//				hasJumped = true;
			//				mozeDaSkociOpet = true;
			//				if(rigidbody2D.velocity.y < maxSpeedY)
			//				{
			//					rigidbody2D.velocity = Vector2.zero;
			//					rigidbody2D.AddForce(new Vector2(0, jumpForce));
			//				}
			//				
			//				if(Mathf.Abs(rigidbody2D.velocity.y) > maxSpeedY)
			//					rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, maxSpeedY);
			//				
			//				jump = false;
			//				
			//			}
			
			else if(jumpControlled)
			{
				//Debug.Log("passed: " + (Time.time - duzinaPritiskaZaSkok));
				hasJumped = true;
				mozeDaSkociOpet = true;
				//if(rigidbody2D.velocity.y < maxSpeedY)
				{
					//rigidbody2D.velocity = Vector2.zero;
					//rigidbody2D.AddForce(new Vector2(0, tempForce));
					//rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x,startVelY + maxSpeedY);
				}
				
				//if(Time.time - duzinaPritiskaZaSkok > 1.05f)
				{
					//tempForce -= korak;
					//korak += 20;
				}
				
				//if(Mathf.Abs(rigidbody2D.velocity.y) > maxSpeedY)
				//	rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, maxSpeedY);
			}
			
			//if(CheckWallHitNear_low)
			//	jumpSpeedX = 5;
			
			//rigidbody2D.velocity = new Vector2(jumpSpeedX, rigidbody2D.velocity.y);
			//rotatingPlayer.rotation = Quaternion.Euler(0,0,0);
			
			// -------------- Monkey started falling --------------------
			// ----------------------------------------------------------
			if(GetComponent<Rigidbody2D>().velocity.y < -0.01f && !triggerCheckDown)
			{
				//rigidbody2D.velocity = new Vector2(Mathf.MoveTowards(jumpSpeedX,jumpSpeedX*1.75f,0.35f), rigidbody2D.velocity.y);
				
				//majmun.animation.PlayQueued(fallingAnimation.name,QueueMode.CompleteOthers); //ZBOG ANIMATORA!!!!
				//jumpSafetyCheck = false;
				
				//rigidbody2D.gravityScale = 11.5f;
				//jumpSpeedX = 13;
				//jumpSpeedX = startJumpSpeedX - 3;
			}
			
			// -------------- Provera za pileci skok i tobogan --------------
			// --------------------------------------------------------------
			if(!triggerCheckDown)
			{
				if(jumpSafetyCheck)
					jumpSafetyCheck = false;
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
				GetComponent<Rigidbody2D>().AddForce(new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpForce)); //promenjeno sa 0
				hasJumped = true;
				jump = false;
			}
			else if(jumpControlled)
			{
				//Debug.Log("passed: " + (Time.time - duzinaPritiskaZaSkok));
				hasJumped = true;
				mozeDaSkociOpet = true;
				if(GetComponent<Rigidbody2D>().velocity.y < maxSpeedY)
				{
					//rigidbody2D.velocity = Vector2.zero;
					GetComponent<Rigidbody2D>().AddForce(new Vector2(GetComponent<Rigidbody2D>().velocity.x, tempForce)); // promenjeno sa 0
					//rigidbody2D.velocity = new Vector2(0,30);
				}
				
				//if(Time.time - duzinaPritiskaZaSkok > 1.05f)
				{
					//tempForce -= korak;
					//korak += 20;
				}
				
				if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) > maxSpeedY)
					GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, maxSpeedY);
			}
			
			GetComponent<Rigidbody2D>().velocity = new Vector2(jumpSpeedX, GetComponent<Rigidbody2D>().velocity.y);
			
			// -------------- Monkey started falling --------------------
			// ----------------------------------------------------------
			if(GetComponent<Rigidbody2D>().velocity.y < -0.01f && !triggerCheckDown)
			{
				//rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
				//rigidbody2D.gravityScale = 11.5f;
				//jumpSpeedX = 13;
				//jumpSpeedX = startJumpSpeedX - 3;
			}
		}
		//------------------------------------------------------------------------
		
		// -------------- Provera za pileci skok i tobogan --------------
		// --------------------------------------------------------------
		if(!triggerCheckDown || Physics2D.Linecast(transform.position + new Vector3(1f,0,0), transform.position + new Vector3(1f,2f,0), 1 << LayerMask.NameToLayer("Ground")))
		{
			if(jumpSafetyCheck)
				jumpSafetyCheck = false;
		}
		
	}
	
	void OnTriggerExit2D(Collider2D col)
	{
		if(col.tag == "Footer")
		{
			if(GetComponent<Collider2D>().isTrigger && transform.position.y > col.transform.position.y)
			{
				//Physics2D.IgnoreLayerCollision(13,18,false);
				GetComponent<Collider2D>().isTrigger = false;
				neTrebaDaProdje = false;
			}
		}
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{
		if(state != State.completed || state != State.wasted)
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
					
					if(PlaySounds.soundOn)					
						PlaySounds.Play_Landing();
					oblak.Play();
					if(GetComponent<Collider2D>().isTrigger == false && triggerCheckDownTrigger) // DODATO JE "&& triggerCheckDownTrigger" i promenjen je x u triggerCheckDown sa 1.1 na 0.8, zbog provere ako si skocio bas na samu ivicu, on udje u running ali odmah padne
					{
						if(startSpustanje)
						{
							startSpustanje = false;
							cameraTarget_down.transform.parent = transform;
							cameraTarget_down.transform.position = cameraTarget.transform.position;
							
						}
						jumpSpeedX = startJumpSpeedX;
						mozeDaSkociOpet = false;
						animator.SetBool("Jump",false);
						animator.SetBool("DoubleJump",false);
						animator.SetBool("Glide",false);
						disableGlide = false;
						animator.SetBool("Landing",true);
						GetComponent<Rigidbody2D>().drag = 0;
						state = State.running;
						canGlide = false;
						if(PlaySounds.soundOn)
							PlaySounds.Play_Run();
						hasJumped = false;
						startY = endY = 0;
						inAir = false;
						//rigidbody2D.gravityScale = 10;
					}
				}
			}
			
			
			else if(col.gameObject.tag == "Enemy")
			{
				//Debug.Log("adasd");
				if(activeShield)
				{
					col.transform.GetComponent<Collider2D>().enabled = false;
					activeShield = false;
					GameObject.Find("_GameManager").SendMessage("ApplyPowerUp",-3);
					if(state != State.running)
						GetComponent<Rigidbody2D>().AddForce(new Vector2(GetComponent<Rigidbody2D>().velocity.x, 1100)); //promenjeno sa 0
				}
				else if(!killed)
				{
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
						
						if(PlaySounds.BackgroundMusic_Gameplay.isPlaying)
							PlaySounds.Stop_BackgroundMusic_Gameplay();
						if(PlaySounds.soundOn)
							PlaySounds.Play_Level_Failed_Popup();
					}
				}
			}
		}
	}
	
	public void majmunUtepanULetu()
	{
		StartCoroutine(FallDownAfterSpikes());
	}
	
	IEnumerator FallDownAfterSpikes()
	{
		canRespawnThings = false;
		GameObject.Find("PrinceGorilla").GetComponent<Animator>().speed = 1.5f;
		
		if(PlaySounds.BackgroundMusic_Gameplay.isPlaying)
			PlaySounds.Stop_BackgroundMusic_Gameplay();
		if(PlaySounds.soundOn)
			PlaySounds.Play_Level_Failed_Popup();
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		killed = true;
		oblak.Play();
		animator.Play(spikedeath_State);
		parentAnim.Play("FallDown");
		state = State.wasted;
		GameObject.Find("Pause").GetComponent<Collider>().enabled = false; // ZA PRVU VERZIJU
		//GameObject.Find("ButtonPause").collider.enabled = false; // ZA FINALNU VERZIJU
		if(trava.isPlaying)
			trava.Stop();
		if(runParticle.isPlaying)
			runParticle.Stop();
		maxSpeedX = 0;
		GetComponent<Rigidbody2D>().isKinematic = true;
		yield return new WaitForSeconds(0.5f);
		GameObject.Find("_GameManager").SendMessage("showFailedScreen");
		cameraFollow.stopFollow = true;
	}
	
	IEnumerator ProceduraPenjanja(GameObject obj)
	{
		yield return new WaitForSeconds(0.01f);
		
		//Debug.Log("Trenutno stanje: " + currentBaseState.nameHash);
		while(currentBaseState.nameHash == grab_State)
		{
			yield return null;
			//Debug.Log("kontroler: " + transform.position + ", majmunce: " + majmun.transform.position);
			//Debug.Log("grab time: " + currentBaseState.normalizedTime);
			if(currentBaseState.normalizedTime > 0.82f && !helper_disableMoveAfterGrab)
			{
				helper_disableMoveAfterGrab = true;
				animator.CrossFade(run_State,0.01f);
				yield return new WaitForEndOfFrame();
				transform.position = GameObject.Find("GrabLanding").transform.position;
				
			}
		}
		state = State.running;
		//Debug.Log("gfgfg kontroler: " + transform.position + ", majmunce: " + majmun.transform.position);
		animator.applyRootMotion = false;
		GetComponent<Collider2D>().enabled = true;
		GetComponent<Rigidbody2D>().isKinematic = false;
		helper_disableMoveAfterGrab = false;
		
		GetComponent<Rigidbody2D>().isKinematic = false;
		//majmun.animation.Play(runAnimation.name); //ZBOG ANIMATORA!!!!
		mozeDaSkociOpet = false;
		animator.SetBool("Jump",false);
		animator.SetBool("DoubleJump",false);
		animator.SetBool("Glide",false);
		animator.SetBool("Landing",true);
		GetComponent<Rigidbody2D>().drag = 0;
		maxSpeedX = startSpeedX;
		state = State.running;
		if(PlaySounds.soundOn)
			PlaySounds.Play_Run();
		animator.SetBool("WallStop",false);
		inAir = false;
		hasJumped = false;
		GetComponent<Collider2D>().enabled = true;
	}
	
	IEnumerator snappingProcess()
	{
		float t = 0;
		float step = 0.25f;
		while(t < 0.02f && (Mathf.Abs(transform.position.x - colliderForClimb.x) > 0.01f && Mathf.Abs(transform.position.y - colliderForClimb.y) > 0.01f))//0.03f)
		{
			//Debug.Log("finy: " + colliderForClimb.y+ ", tempy: " + transform.position.y + ", finx: " + colliderForClimb.x + ", tempx: " + transform.position.x + ", t: " + t);
			transform.position = Vector3.Lerp(transform.position, new Vector3(colliderForClimb.x,colliderForClimb.y,transform.position.z), step);
			t += Time.deltaTime * step;
			yield return null;
		}
		grab = true;
		//animator.applyRootMotion = true;
		animator.Play("Grab");
	}
	
	void OnCollisionExit2D(Collision2D col)
	{
		if(col.gameObject.tag == "Footer")
		{
			if(state == State.running)
			{
				neTrebaDaProdje = false;
				if(!proveriTerenIspredY && !downHit) 	// ************* 2.5m ispred nema nicega ispod
				{
					state = State.jumped;
					//hasJumped = true;
					//mozeDaSkociOpet = true;
					animator.SetBool("Landing",false);
					//animator.SetBool("Falling",true);
					animator.Play(fall_State);
					if(runParticle.isPlaying)
						runParticle.Stop();
					//majmun.animation.Play(fallingAnimation.name); //ZBOG ANIMATORA!!!!
					if(!spustanjeRastojanje)
					{
						startSpustanje = true;
						cameraTarget_down.transform.parent = null;
						//cameraTarget_down.transform.position = new Vector3(cameraTarget.transform.position.x,transform.position.y - 5, cameraTarget.transform.position.z);
						pocetniY_spustanje = cameraTarget.transform.position.y;
						cameraTarget_down_y = transform.position.y -7.5f;
						//Debug.Log("Pozicija: " + (transform.position.y - 7.5f));
						cameraFollow.cameraTarget = cameraTarget_down;
						//cameraFollow.transition = true;
					}
					//spustenTarget = true;
				}
			}
		}
		else if(col.gameObject.tag == "ZidZaOdbijanje")
		{
			GetComponent<Rigidbody2D>().drag = 0;
			if(trava.isPlaying)
				trava.Stop();
			animator.Play(fall_State);
			animator.SetBool("Landing",false);
		}
		
	}
	
	void NotifyManagerForFinish()
	{
		GameObject.Find("_GameManager").SendMessage("ShowWinScreen");
	}
	
	IEnumerator TutorialPlay(Transform obj, string ime, int next)
	{
		//Debug.Log("Usao");
		StartCoroutine( obj.GetComponent<Animation>().Play(ime, false, what => {helpBool=true;}) );
		//animation.Play(
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
			
			if(col.tag == "Barrel")
			{
				col.transform.GetChild(0).GetComponent<Animator>().Play("BarrelBoom");
			}
			else if(col.name == "Magnet")
			{
				GameObject.Find("_GameManager").SendMessage("ApplyPowerUp",1);
				col.gameObject.SetActive(false);
			}
			else if(col.name == "Banana_Coin_X2")
			{
				GameObject.Find("_GameManager").SendMessage("ApplyPowerUp",2);
				col.gameObject.SetActive(false);
			}
			else if(col.name == "Banana_Shield")
			{
				GameObject.Find("_GameManager").SendMessage("ApplyPowerUp",3);
				col.gameObject.SetActive(false);
				activeShield = true;
			}
			if(col.tag == "Finish")
			{
				col.GetComponent<Collider2D>().enabled = false;
				cameraFollow.cameraFollowX = false;
				Invoke("NotifyManagerForFinish",1.25f);
				GameObject.Find("Pause").GetComponent<Collider>().enabled = false; // ZA PRVU VERZIJU
				//GameObject.Find("ButtonPause").collider.enabled = false; // ZA FINALNU VERZIJU
				state = State.completed;
			}
			
			
			else if(col.tag == "Footer")
			{
				if(transform.position.y + 0.5f > col.transform.position.y && triggerCheckDownTrigger && triggerCheckDownBehind && GetComponent<Collider2D>().isTrigger)
				{
					GetComponent<Collider2D>().isTrigger = false;
				}
				//collider2D.isTrigger = true;
				//col.gameObject.GetComponent<EdgeCollider2D>().enabled = false;
			}
			
			else if(col.tag == "Enemy")
			{
				col.GetComponent<Collider2D>().enabled = false;
				if(activeShield)
				{
					col.transform.GetComponent<Collider2D>().enabled = false;
					activeShield = false;
					GameObject.Find("_GameManager").SendMessage("ApplyPowerUp",-3);
					if(state != State.running)
						GetComponent<Rigidbody2D>().AddForce(new Vector2(GetComponent<Rigidbody2D>().velocity.x, 1100)); // promenjeno sa 0
				}
				else if(!killed)
				{
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
			
			else if(col.tag == "GrabLedge" && !downHit)// && transform.position.y < col.transform.position.y)
			{
				col.GetComponent<Collider2D>().enabled = false;
			}
			
			else if(col.tag == "Lijana")
			{
				grabLianaTransform = col.transform.GetChild(0);
				lijana = true;
				state = State.lijana;
				//cameraFollow.cameraFollowX = false;
				col.enabled = false;
				GetComponent<Rigidbody2D>().isKinematic = true;
				maxSpeedX = 0;
				jumpSpeedX = 0;
				//transform.parent = col.transform.parent;
				col.transform.parent.GetComponent<Animator>().Play("RotateLianaHolder");
				animator.Play(lijana_State);
				StartCoroutine("pratiLijanaTarget",grabLianaTransform);
			}
			
		}
	}
	
	void OnTriggerStay2D(Collider2D col)
	{
		if(col.tag == "GrabLedge")
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, Mathf.MoveTowards(GetComponent<Rigidbody2D>().velocity.y,0,0.2f));
	}
	
	IEnumerator pratiLijanaTarget(Transform target)
	{
		while(lijana)
		{
			yield return null;
			transform.position = Vector3.Lerp(transform.position ,new Vector3(target.position.x,target.position.y, transform.position.z), 0.2f);
			//transform.position = new Vector3(target.position.x,target.position.y, transform.position.z);
		}
	}
	
	public void OtkaciMajmuna()
	{
		lijana = false;
		StopCoroutine("pratiLijanaTarget");
		state = State.jumped;
		cameraFollow.cameraFollowX = true;
		maxSpeedX = startSpeedX;
		jumpSpeedX = startJumpSpeedX;
		transform.parent = null;
		transform.rotation = Quaternion.identity;
		GetComponent<Rigidbody2D>().isKinematic = false;
		GetComponent<Rigidbody2D>().AddForce(new Vector2(200,2500));
		animator.Play(jump_State);
	}
	
	void SpustiMajmunaSaLijaneBrzo()
	{
		lijana = false;
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
	void climb()
	{
		StartCoroutine(MoveUp(0.05f));
	}
	IEnumerator ClimbLedge(Transform target, float time)
	{
		//rigidbody2D.isKinematic = true;
		
		//while(majmun.animation.IsPlaying(climbAnimation.name))
		yield return null;
		//stop = false;
		if(transform.position.y > target.position.y)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
			//		while(t < 1)
			//		{
			//			transform.position = Vector2.MoveTowards(transform.position,new Vector2(target.position.x, target.position.y), t);
			//			t += Time.deltaTime/time;
			//			yield return null;
			//		}
			GetComponent<Rigidbody2D>().isKinematic = false;
			stop = false;
			//majmun.animation.Play(runAnimation.name); //ZBOG ANIMATORA!!!!
			mozeDaSkociOpet = false;
			animator.SetBool("Jump",false);
			animator.SetBool("DoubleJump",false);
			animator.SetBool("Glide",false);
			disableGlide = false;
			animator.SetBool("Landing",true);
			state = State.running;
			if(PlaySounds.soundOn)
				PlaySounds.Play_Run();
			hasJumped = false;
			//toggleRotate = true;
		}
	}
	IEnumerator MoveUp(float time)
	{
		float target = transform.position.y + 1.85f;
		float t = 0;
		float offsetX = 2f;
		float destX = transform.position.x + offsetX;
		float step = 0.03f;
		//#if UNITY_EDITOR
		//step = 0.01f;
		//#endif
		while(t < 1)
		{
			transform.position = Vector2.MoveTowards(transform.position,new Vector3(destX, target,transform.position.z), t);
			if(Time.timeScale != 0)
				offsetX = 2f;
			else
				offsetX = 0;
			//t += Time.deltaTime/1.75f;
			t+=step;
			yield return null;
		}
		
	}
	
	
	public void majmunUtepan()
	{
		canRespawnThings = false;
		GameObject.Find("PrinceGorilla").GetComponent<Animator>().speed = 1.5f;
		
		//transform.Find("PlayerFocus2D").parent = transform.parent;
		state = State.wasted;
		GameObject.Find("Pause").GetComponent<Collider>().enabled = false; // ZA PRVU VERZIJU
		//GameObject.Find("ButtonPause").collider.enabled = false; // ZA FINALNU VERZIJU
		//Debug.Log("Pushci animaciju");
		//majmun.animation.Stop();
		//majmun.animation.Play(deathAnimation.name); //ZBOG ANIMATORA!!!!
		animator.Play("Death1");
		//maxSpeedX = 0;
		//rigidbody2D.AddForce(new Vector2(150,0));
		StartCoroutine(slowDown());
		if(trava.isPlaying)
			trava.Stop();
		if(runParticle.isPlaying)
			runParticle.Stop();
		maxSpeedX = 0;
		
		if(PlaySounds.BackgroundMusic_Gameplay.isPlaying)
			PlaySounds.Stop_BackgroundMusic_Gameplay();
		if(PlaySounds.soundOn)
			PlaySounds.Play_Level_Failed_Popup();
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
		GameObject.Find("_GameManager").SendMessage("showFailedScreen");
		cameraFollow.stopFollow = true;
		GetComponent<Rigidbody2D>().isKinematic = true;
		yield return new WaitForSeconds(1.75f);
		transform.Find("HolderKillStars").gameObject.SetActive(true);
	}
	
	public void CallShake()
	{
		//StartCoroutine(shakeCamera());
	}
	
	IEnumerator shakeCamera()
	{
		yield return null;
	}
	
	string RaycastFunction(Vector3 vector)
	{
		Ray ray = Camera.main.ScreenPointToRay(vector);
		RaycastHit hit;
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
}

