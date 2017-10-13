using UnityEngine;
using System.Collections;

public class ManageFull : MonoBehaviour {
	
	[HideInInspector]
	public int coinsCollected = 0;
	[HideInInspector]
	public int starsGained = 0;
	[HideInInspector]
	public int keysCollected = 0;
	[HideInInspector]
	public int collectedPoints = 0;
	[HideInInspector]
	public int baboonSmashed = 0;
	int brojDoubleCoins = 1;
	int brojMagneta = 1;
	int brojShieldova = 1;
	bool playerDead = false;
	public GameObject goScreen;
	public GameObject goScreen2;
	GameObject player;
	MonkeyController2D playerController;
	float camera_z;
	CameraFollow2D_new cameraFollow;

	Transform pauseButton;
	Transform coinsHolder;
	TextMesh coinsCollectedText;
	GameObject pauseScreenHolder;
	GameObject Win_CompletedScreenHolder;
	GameObject FailedScreenHolder;
	GameObject Win_ShineHolder;
	GameObject star1;
	GameObject star2;
	GameObject star3;
	Transform holderKeys;
	
	GameObject newHighScore;
	GameObject holderFinishPts;
	GameObject holderFinishKeys;
	GameObject buttonFacebookShare;
	GameObject buttonBuyKeys;
	GameObject buttonPlay_Finish;
	GameObject holderFinishInfo;
	GameObject holderTextCompleted;
	GameObject holderKeepPlaying;
	GameObject keyHole1;
	GameObject keyHole2;
	GameObject keyHole3;
	[HideInInspector]
	public Transform progressBarScale;
	Transform wonStar1;
	Transform wonStar2;
	Transform wonStar3;
	TextMesh textKeyPrice1;
	TextMesh textKeyPrice2;
	Transform shopHolder;
	Transform shopLevaIvica;
	Transform shopDesnaIvica;
	GameObject shopHeaderOn;
	GameObject shopHeaderOff;
	GameObject freeCoinsHeaderOn;
	GameObject freeCoinsHeaderOff;
	GameObject holderShopCard;
	GameObject holderFreeCoinsCard;
	Transform buttonShopBack;
	Transform PickPowers;
	Transform powerCard_CoinX2;
	Transform powerCard_Magnet;
	Transform powerCard_Shield;

	bool kupljenShield;
	bool kupljenDoubleCoins;
	bool kupljenMagnet;
	
	public AnimationClip showPauseAnimation;
	public AnimationClip dropPauseAnimation;
	
	bool helpBool;
	bool playerStopiran = false;
	System.Action command;
	//NivoManager nivoManager;
	string releasedItem;
	SetRandomStarsManager starManager;
	
	bool PowerUp_magnet = false;
	[HideInInspector]
	public bool PowerUp_doubleCoins = false;
	[HideInInspector]
	public bool PowerUp_shield = false;
	
	GameObject coinMagnet;
	GameObject shield;
	
	TextMesh textPtsGameplay;
	TextMesh textPtsFinish;
	
	void Awake ()
	{
		cameraFollow = Camera.main.transform.parent.GetComponent<CameraFollow2D_new>();
		goScreen.SetActive(true);
		goScreen2.SetActive(true);
		//Time.timeScale = 0;
		starManager = GetComponent<SetRandomStarsManager>();
		player = GameObject.FindGameObjectWithTag("Monkey");
		playerController = player.GetComponent<MonkeyController2D>();
		coinsCollectedText = GameObject.Find("TextCoins").GetComponent<TextMesh>();
		pauseButton = GameObject.Find("HolderPause").transform;
		coinsHolder = GameObject.Find("HolderCoins").transform;
		pauseScreenHolder = GameObject.Find("HolderPauseScreen");
		FailedScreenHolder = GameObject.Find("HolderFailed");
		Win_CompletedScreenHolder = GameObject.Find("HolderFinish");
		Win_ShineHolder = GameObject.Find("HolderShineFinish");
		star1 = GameObject.Find("FinishStar1");
		star2 = GameObject.Find("FinishStar2");
		star3 = GameObject.Find("FinishStar3");
		holderKeys = GameObject.Find("HolderKeys").transform;
		//star1.SetActive(false);
		//star2.SetActive(false);
		//star3.SetActive(false);
		
		camera_z = Camera.main.transform.position.z;
		
		coinMagnet = player.transform.Find("CoinMagnet").gameObject;
		shield = GameObject.Find("Shield");
		shield.SetActive(false);
		
//		GameObject.Find("TextPauseHeader1").renderer.sortingLayerID = 1;
//		GameObject.Find("TextPauseHeader2").renderer.sortingLayerID = 1;
//		GameObject.Find("FailedTextHeader1").renderer.sortingLayerID = 1;
//		GameObject.Find("FailedTextHeader2").renderer.sortingLayerID = 1;
//		GameObject.Find("TextEarnCoins1").renderer.sortingLayerID = 1;
//		GameObject.Find("TextEarnCoins2").renderer.sortingLayerID = 1;
//		GameObject.Find("TextKeyInfo1").renderer.sortingLayerID = 1;
//		GameObject.Find("TextPts").renderer.sortingLayerID = 1;
//		GameObject.Find("TextKeyPrice1").renderer.sortingLayerID = 1;
//		GameObject.Find("TextKeyPrice2").renderer.sortingLayerID = 1;
//		GameObject.Find("TaxtNewScore1").renderer.sortingLayerID = 1;
//		GameObject.Find("TaxtNewScore2").renderer.sortingLayerID = 1;
//		GameObject.Find("TextKeepPlayingHeader1").renderer.sortingLayerID = 1;
//		GameObject.Find("TextKeepPlayingHeader2").renderer.sortingLayerID = 1;
//		GameObject.Find("TextEnd1").renderer.sortingLayerID = 1;
//		GameObject.Find("TextEnd2").renderer.sortingLayerID = 1;
//		GameObject.Find("TextCoins").renderer.sortingLayerID = 1;
//		GameObject.Find("TextPtsGameplay").renderer.sortingLayerID = 1;
//		GameObject.Find("TextKeepPlayingHeader1").renderer.sortingLayerID = 1;
//		GameObject.Find("TextFreeCoinsUp1").renderer.sortingLayerID = 1;
//		GameObject.Find("TextShopDown").renderer.sortingLayerID = 1;
//		GameObject.Find("TextShopUp").renderer.sortingLayerID = 1;
//		GameObject.Find("TextFreeCoinsUp").renderer.sortingLayerID = 1;
//		GameObject.Find("TextFreeCoinsDown").renderer.sortingLayerID = 1;
		
		newHighScore = GameObject.Find("NewHighScore");
		holderFinishPts = GameObject.Find("HolderFinishPts");
		holderFinishInfo = GameObject.Find("HolderFinishInfo");
		buttonFacebookShare = GameObject.Find("FinishFacebook");
		buttonBuyKeys = GameObject.Find("FinishKeyPrice");
		holderFinishKeys = GameObject.Find("HolderFinishKeys");
		buttonPlay_Finish = GameObject.Find("ButtonHolePlay");
		holderTextCompleted = GameObject.Find("HolderTextCompleted");
		keyHole1 = GameObject.Find("FinishKeyHole1_");
		keyHole2 = GameObject.Find("FinishKeyHole2_");
		keyHole3 = GameObject.Find("FinishKeyHole3_");
		holderKeepPlaying = GameObject.Find("HolderKeepPlaying");
		progressBarScale = GameObject.Find("ProgressBar_ScaleY").transform;
		wonStar1 = GameObject.Find("HolderWonStar1").transform;
		wonStar2 = GameObject.Find("HolderWonStar2").transform;
		wonStar3 = GameObject.Find("HolderWonStar3").transform;
		textPtsGameplay = GameObject.Find("TextPtsGameplay").GetComponent<TextMesh>();
		textPtsFinish = GameObject.Find("TextPts").GetComponent<TextMesh>();
		textKeyPrice1 = GameObject.Find("TextKeyPrice1").GetComponent<TextMesh>();
		textKeyPrice2 = GameObject.Find("TextKeyPrice2").GetComponent<TextMesh>();
		shopHolder = GameObject.Find("_HolderShop").transform;
		shopLevaIvica = GameObject.Find("ShopRamLevoHolder").transform;
		shopDesnaIvica = GameObject.Find("ShopRamDesnoHolder").transform;
		shopHeaderOn = GameObject.Find("ShopHeaderOn");
		shopHeaderOff = GameObject.Find("ShopHeaderOff1");
		freeCoinsHeaderOn = GameObject.Find("ShopHeaderOn1");
		freeCoinsHeaderOff = GameObject.Find("ShopHeaderOff");
		holderShopCard = GameObject.Find("HolderShopCard");
		holderFreeCoinsCard = GameObject.Find("HolderFreeCoinsCard");
		buttonShopBack = GameObject.Find("HolderBack").transform;
		PickPowers = GameObject.Find("HolderPowersMove").transform;
		powerCard_CoinX2 = GameObject.Find("PowersCardCoinx2").transform;
		powerCard_Magnet = GameObject.Find("PowersCardMagnet").transform;
		powerCard_Shield = GameObject.Find("PowersCardShield").transform;

		shopHeaderOff.SetActive(false);
		freeCoinsHeaderOn.SetActive(false);
		holderFreeCoinsCard.SetActive(false);
		
		holderKeepPlaying.SetActive(false);
		newHighScore.SetActive(false);
		//holderFinishPts.SetActive(false);
		//holderFinishInfo.SetActive(false);
		//buttonFacebookShare.SetActive(false);
		//buttonBuyKeys.SetActive(false);
		//holderFinishKeys.SetActive(false);
		//buttonPlay_Finish.SetActive(false);
		holderTextCompleted.SetActive(false);
		
		pauseScreenHolder.SetActive(false);
		FailedScreenHolder.SetActive(false);
		Win_CompletedScreenHolder.SetActive(false);
		Win_ShineHolder.SetActive(false);
		
		if(Camera.main.aspect <= 1.5f)
		{
			Camera.main.orthographicSize = 18f;
			shopHolder.localScale = shopHolder.localScale * 18/5f;
		}
		if(Camera.main.aspect > 1.5f)
		{
			Camera.main.orthographicSize = 15f;
			shopHolder.localScale = shopHolder.localScale * 15/5f;
		}
		else 
		{
			Camera.main.orthographicSize = 16.5f;
			shopHolder.localScale = shopHolder.localScale * 16.5f/5f;
		}
		
		pauseButton.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one).x, Camera.main.ViewportToWorldPoint(Vector3.one).y, pauseButton.position.z);
		coinsHolder.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x, Camera.main.ViewportToWorldPoint(Vector3.one).y, coinsHolder.position.z);
		holderKeys.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one/2).x, Camera.main.ViewportToWorldPoint(Vector3.one).y, pauseButton.position.z);

		shopHolder.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one/2).x, shopHolder.position.y, shopHolder.position.z);
		shopLevaIvica.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x, shopLevaIvica.position.y, shopLevaIvica.position.z);
		shopDesnaIvica.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one).x, shopLevaIvica.position.y, shopLevaIvica.position.z);
		shopHolder.gameObject.SetActive(false);
		
		pauseButton.parent = Camera.main.transform;
		coinsHolder.parent = Camera.main.transform;
		holderKeys.parent = Camera.main.transform;

		PickPowers.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one/2).x, Camera.main.ViewportToWorldPoint(Vector3.one).y * 0.65f, PickPowers.position.z);
		PickPowers.gameObject.SetActive(false);
		
		goScreen.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y,goScreen.transform.position.z);
		goScreen2.transform.position = goScreen.transform.position + new Vector3(0.1f,0,1);
		
		goScreen.transform.parent = Camera.main.transform;
		goScreen2.transform.parent = Camera.main.transform;

		PickPowers.parent = Camera.main.transform;


		
		//nivoManager = GameObject.Find("NivoManager").GetComponent<NivoManager>();
		if(PlaySounds.musicOn)
		{
			PlaySounds.Play_BackgroundMusic_Gameplay();
			if(PlaySounds.Level_Failed_Popup.isPlaying)
				PlaySounds.Stop_Level_Failed_Popup();
		}

		powerCard_CoinX2.GetChild(3).GetChild(0).GetComponent<TextMesh>().text = brojDoubleCoins.ToString();
		powerCard_CoinX2.GetChild(3).GetChild(1).GetComponent<TextMesh>().text = brojDoubleCoins.ToString();

		powerCard_Magnet.GetChild(3).GetChild(0).GetComponent<TextMesh>().text = brojMagneta.ToString();
		powerCard_Magnet.GetChild(3).GetChild(1).GetComponent<TextMesh>().text = brojMagneta.ToString();

		powerCard_Shield.GetChild(3).GetChild(0).GetComponent<TextMesh>().text = brojShieldova.ToString();
		powerCard_Shield.GetChild(3).GetChild(1).GetComponent<TextMesh>().text = brojShieldova.ToString();
		
		
	}

	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Space) && goScreen.activeSelf)
		{
			Time.timeScale = 1;
			goScreen.transform.parent = transform;
			goScreen2.transform.parent = transform;
			goScreen.SetActive(false);
			goScreen2.SetActive(false);
			playerController.state = MonkeyController2D.State.running;
			PlaySounds.Play_Run();
		}
		
		if(Input.GetMouseButtonUp(0))// || Input.GetKeyDown(KeyCode.Space))
		{
			releasedItem = RaycastFunction(Input.mousePosition);
			if(releasedItem == "GO screen")
			{
				Time.timeScale = 1;
				goScreen.transform.parent = transform;
				goScreen2.transform.parent = transform;
				goScreen.SetActive(false);
				goScreen2.SetActive(false);
				playerController.state = MonkeyController2D.State.running;
				GameObject.Find("PrinceGorilla").GetComponent<Animator>().SetBool("Run",true); //MOZDA DA SE VRATI U PrinceGorilla
				PlaySounds.Play_Run();
				StartCoroutine(showPickPowers());
				
			}
			else if(releasedItem == "ButtonPause")
			{
				PlaySounds.Play_Button_Pause();
				//playerController.state = MonkeyController2D.State.idle;
				//pauseButton.GetChild(0).animation.Play();
				pauseScreenHolder.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y, pauseScreenHolder.transform.position.z);
				pauseScreenHolder.SetActive(true);
				
				if(Time.timeScale == 1)
				{
					Time.timeScale = 0;
					StopAllCoroutines();
					StartCoroutine(showPauseScreen());
				}
				else
				{
					StartCoroutine(dropPauseScreen());
				}
				
				
			}
			else if(releasedItem == "PauseHoleMain") // KLIKNUTO NA MAIN DUGME IZ PAUSE MENIJA
			{
				PlaySounds.Play_Button_GoBack();
				StartCoroutine(backToMenu());
			}
			
			else if(releasedItem == "PauseHolePlay") // KLIKNUTO NA PLAY DUGME IZ PAUSE MENIJA
			{
				PlaySounds.Play_Button_Pause();
				StartCoroutine(unPause());
				if(playerStopiran)
				{
					playerController.heCanJump = true;
					buttonShopBack.GetChild(0).GetComponent<Animation>().Play("BackButtonClick");
					StartCoroutine(closeShop());
					playerStopiran = false;
					GameObject.Find("ButtonPause").GetComponent<Collider>().enabled = true;
					GameObject.Find("OBLACI").GetComponent<RunWithSpeed>().continueMoving = true;
					playerController.GetComponent<Rigidbody2D>().isKinematic = false;
					playerController.animator.enabled = true;
					playerController.maxSpeedX = playerController.startSpeedX;
					cameraFollow.cameraFollowX = true;
					//cameraFollow.cameraFollowY = false;
					//cameraFollow.moveUp = true;
					//cameraFollow.moveDown = false;
				}
			}
			
			else if(releasedItem == "PauseHoleRestart") // KLIKNUTO NA RESTART DUGME IZ PAUSE MENIJA
			{
				PlaySounds.Play_Button_RestartLevel();
				StartCoroutine(restartLevel());
			}
			
			else if(releasedItem == "FailedMainHole") // KLIKNUTO NA MAIN DUGME IZ FAILED MENIJA
			{
				PlaySounds.Play_Button_GoBack();
				GameObject temp = GameObject.Find("ButtonMain_Failed");
				temp.GetComponent<Animation>().Play("FinishButtonsClick");
				if(PlaySounds.BackgroundMusic_Gameplay.isPlaying)
					PlaySounds.Stop_BackgroundMusic_Gameplay();
				//nivoManager.currentLevel = 0;
				command = delegate {Application.LoadLevel(4);};
				StartCoroutine(FailedScreenHolder.transform.GetChild(0).GetComponent<Animation>().Play("FailedGo", false, what => {helpBool=true;}) );
				StartCoroutine(DoAfterAnimation(temp,"FinishButtonsClick"));
			}
			
			else if(releasedItem == "FailedRestartHole") // KLIKNUTO NA RESTART DUGME IZ FAILED MENIJA
			{
				PlaySounds.Play_Button_RestartLevel();
				GameObject temp = GameObject.Find("ButtonRestart_Failed");
				temp.GetComponent<Animation>().Play("FinishButtonsClick");
				command = delegate {Application.LoadLevel(Application.loadedLevel);};
				StartCoroutine(FailedScreenHolder.transform.GetChild(0).GetComponent<Animation>().Play("FailedGo", false, what => {helpBool=true;}) );
				StartCoroutine(DoAfterAnimation(temp,"FinishButtonsClick"));
				
			}
			
			else if(releasedItem == "ButtonRestart1") // KLIKNUTO NA RESTART DUGME IZ FINISH MENIJA
			{
				PlaySounds.Play_Button_RestartLevel();
				GameObject temp = GameObject.Find("ButtonRestart1");
				temp.GetComponent<Animation>().Play("FinishButtonsClick");
				command = delegate {Application.LoadLevel(Application.loadedLevel);};
				StartCoroutine(Win_CompletedScreenHolder.transform.GetChild(0).GetComponent<Animation>().Play("FinishTableGo1", false, what => {helpBool=true;}) );
				StartCoroutine(DoAfterAnimation(temp,"FinishButtonsClick"));
				
			}
			
			else if(releasedItem == "ButtonMain1") // KLIKNUTO NA MAIN DUGME IZ FINISH MENIJA
			{
				PlaySounds.Play_Button_GoBack();
				GameObject temp = GameObject.Find("ButtonMain1");
				temp.GetComponent<Animation>().Play("FinishButtonsClick");
				if(PlaySounds.BackgroundMusic_Gameplay.isPlaying)
					PlaySounds.Stop_BackgroundMusic_Gameplay();
				//nivoManager.currentLevel = 0;
				command = delegate {Application.LoadLevel(4);};
				StartCoroutine(Win_CompletedScreenHolder.transform.GetChild(0).GetComponent<Animation>().Play("FinishTableGo1", false, what => {helpBool=true;}) );
				StartCoroutine(DoAfterAnimation(temp,"FinishButtonsClick"));
			}
			
			else if(releasedItem == "ButtonPlay1") // KLIKNUTO NA PLAY DUGME IZ FINISH MENIJA
			{
				PlaySounds.Play_Button_NextLevel();
				GameObject temp = GameObject.Find("ButtonPlay1");
				temp.GetComponent<Animation>().Play("FinishButtonsClick");
				if(PlaySounds.BackgroundMusic_Gameplay.isPlaying)
					PlaySounds.Stop_BackgroundMusic_Gameplay();
				//nivoManager.currentLevel = Application.loadedLevel;
				StagesParser.currStageIndex++;
				command = delegate {Application.LoadLevel("LoadingScene");};
				StartCoroutine(Win_CompletedScreenHolder.transform.GetChild(0).GetComponent<Animation>().Play("FinishTableGo1", false, what => {helpBool=true;}) );
				StartCoroutine(DoAfterAnimation(temp,"FinishButtonsClick"));
			}

			else if(releasedItem == "PauseHoleFreeCoins") // KLIKNUTO NA FREE COINS IZ PAUSE MENIJA
			{
				playerStopiran = true;
				playerController.heCanJump = false;
				GameObject.Find("ButtonPause").GetComponent<Collider>().enabled = false;
				GameObject.Find("OBLACI").GetComponent<RunWithSpeed>().continueMoving = false;
				playerController.GetComponent<Rigidbody2D>().isKinematic = true;
				//playerController.state = MonkeyController2D.State.idle;
				playerController.maxSpeedX = 0;
				playerController.animator.enabled = false;
				cameraFollow.cameraFollowX = false;
				cameraFollow.cameraFollowY = false;
				cameraFollow.moveUp = false;
				cameraFollow.moveDown = false;
				Time.timeScale = 1;
				StartCoroutine(OpenFreeCoinsCard());
			}

			else if(releasedItem == "PauseHoleShop") // KLIKNUTO NA SHOP IZ PAUSE MENIJA
			{
				playerStopiran = true;
				playerController.heCanJump = false;
				GameObject.Find("ButtonPause").GetComponent<Collider>().enabled = false;
				GameObject.Find("OBLACI").GetComponent<RunWithSpeed>().continueMoving = false;
				playerController.GetComponent<Rigidbody2D>().isKinematic = true;
				//playerController.state = MonkeyController2D.State.idle;
				playerController.maxSpeedX = 0;
				playerController.animator.enabled = false;
				cameraFollow.cameraFollowX = false;
				cameraFollow.cameraFollowY = false;
				cameraFollow.moveUp = false;
				cameraFollow.moveDown = false;
				Time.timeScale = 1;
				StartCoroutine(OpenFreeCoinsCard());
			}

			else if(releasedItem == "FinishKeyPrice")
			{
				StartCoroutine(BuyKeys());
			}
			else if(releasedItem == "ButtonFreeCoins1") // KLIKNUTO NA FREE COINS IZ FINISH MENIJA
			{
				GameObject.Find(releasedItem).GetComponent<Animation>().Play("FinishButtonsClick");
				shopHolder.transform.position = Camera.main.transform.position + Vector3.forward*5;
				StartCoroutine(OpenFreeCoinsCard());
			}
			else if(releasedItem == "ButtonShop1") // KLIKNUTO NA SHOP IZ FINISH MENIJA
			{
				GameObject.Find(releasedItem).GetComponent<Animation>().Play("FinishButtonsClick");
				shopHolder.transform.position = Camera.main.transform.position + Vector3.forward*5;
				StartCoroutine(OpenShopCard());
			}
			else if(releasedItem == "FailedFreeCoinsHole") // KLIKNUTO NA FREE COINS IZ FAILED MENIJA
			{
				GameObject.Find(releasedItem).transform.GetChild(0).GetComponent<Animation>().Play("FinishButtonsClick");
				shopHolder.transform.position = Camera.main.transform.position + Vector3.forward*5;
				StartCoroutine(OpenFreeCoinsCard());
			}
			else if(releasedItem == "FailedShopHole") // KLIKNUTO NA SHOP IZ FAILED MENIJA
			{
				GameObject.Find(releasedItem).transform.GetChild(0).GetComponent<Animation>().Play("FinishButtonsClick");
				shopHolder.transform.position = Camera.main.transform.position + Vector3.forward*5;
				StartCoroutine(OpenShopCard());
			}
			else if(releasedItem == "HolderBack") // KLIKNUTO NA BACK IZ SHOP MENIJA
			{
				Debug.Log("ime: " + GameObject.Find(releasedItem));
				buttonShopBack.GetChild(0).GetComponent<Animation>().Play("BackButtonClick");
				StartCoroutine(closeShop());
			}
			else if(releasedItem == "ShopHeaderOff1") // TREBA DA SE AKTIVIRA SHOP TAB
			{
				shopHeaderOff.SetActive(false);
				shopHeaderOn.SetActive(true);
				freeCoinsHeaderOn.SetActive(false);
				freeCoinsHeaderOff.SetActive(true);
				holderFreeCoinsCard.SetActive(false);
				holderShopCard.SetActive(true);
			}
			else if(releasedItem == "ShopHeaderOff") // TREBA DA SE AKTIVIRA FREE COINS TAB
			{
				shopHeaderOn.SetActive(false);
				shopHeaderOff.SetActive(true);
				freeCoinsHeaderOff.SetActive(false);
				freeCoinsHeaderOn.SetActive(true);
				holderShopCard.SetActive(false);
				holderFreeCoinsCard.SetActive(true);
			}
			else if(releasedItem == "PowersCardCoinx2")
			{
				//GameObject temp = GameObject.Find(releasedItem);
				powerCard_CoinX2.GetComponent<Collider>().enabled = false;
				brojDoubleCoins--;
				powerCard_CoinX2.GetChild(3).GetChild(0).GetComponent<TextMesh>().text = brojDoubleCoins.ToString();
				powerCard_CoinX2.GetChild(3).GetChild(1).GetComponent<TextMesh>().text = brojDoubleCoins.ToString();
				kupljenDoubleCoins = true;
				powerCard_CoinX2.GetComponent<Animator>().Play("GameplayPowerClick2");
				ApplyPowerUp(2);
			}
			else if(releasedItem == "PowersCardMagnet")
			{
				//GameObject temp = GameObject.Find(releasedItem);
				powerCard_Magnet.GetComponent<Collider>().enabled = false;
				brojMagneta--;
				powerCard_Magnet.GetChild(3).GetChild(0).GetComponent<TextMesh>().text = brojMagneta.ToString();
				powerCard_Magnet.GetChild(3).GetChild(1).GetComponent<TextMesh>().text = brojMagneta.ToString();
				kupljenMagnet = true;
				powerCard_Magnet.GetComponent<Animator>().Play("GameplayPowerClick2");
				ApplyPowerUp(1);
			}
			else if(releasedItem == "PowersCardShield")
			{
				//GameObject temp = GameObject.Find(releasedItem);
				powerCard_Shield.GetComponent<Collider>().enabled = false;
				brojShieldova--;
				powerCard_Shield.GetChild(3).GetChild(0).GetComponent<TextMesh>().text = brojShieldova.ToString();
				powerCard_Shield.GetChild(3).GetChild(1).GetComponent<TextMesh>().text = brojShieldova.ToString();
				kupljenShield = true;
				powerCard_Shield.GetComponent<Animator>().Play("GameplayPowerClick2");
				ApplyPowerUp(3);
			}

		}
	}

	IEnumerator OpenShopCard()
	{
		shopHeaderOff.SetActive(false);
		shopHeaderOn.SetActive(true);
		freeCoinsHeaderOn.SetActive(false);
		freeCoinsHeaderOff.SetActive(true);
		holderFreeCoinsCard.SetActive(false);
		holderShopCard.SetActive(true);

		yield return new WaitForSeconds(0.5f);
		shopHolder.gameObject.SetActive(true);
	}

	IEnumerator OpenFreeCoinsCard()
	{
		shopHeaderOn.SetActive(false);
		shopHeaderOff.SetActive(true);
		freeCoinsHeaderOff.SetActive(false);
		freeCoinsHeaderOn.SetActive(true);
		holderShopCard.SetActive(false);
		holderFreeCoinsCard.SetActive(true);

		yield return new WaitForSeconds(0.5f);
		shopHolder.gameObject.SetActive(true);
	}

	IEnumerator closeShop()
	{
		yield return new WaitForSeconds(0.85f);
		shopHolder.gameObject.SetActive(false);
		shopHolder.position = new Vector3(-5,-5,shopHolder.position.z);
		buttonShopBack.GetChild(0).localPosition = Vector3.zero;
	}

	IEnumerator DoAfterAnimation(GameObject obj, string animationName)
	{
		while(obj.GetComponent<Animation>().IsPlaying(animationName))
			yield return null;
		command();
		
	}
	
	IEnumerator showPauseScreen()
	{
		StartCoroutine(pauseButton.transform.GetChild(0).GetComponent<Animation>().Play("FinishButtonsClick", false, what => {helpBool=true;}) );
		StartCoroutine( pauseScreenHolder.transform.GetChild(0).GetComponent<Animation>().Play("PauseShow", false, what => {helpBool=true;}) );
		while(!helpBool)
		{
			yield return null;
		}
		helpBool=false;
	}
	
	IEnumerator dropPauseScreen()
	{
		StartCoroutine(pauseButton.transform.GetChild(0).GetComponent<Animation>().Play("FinishButtonsClick", false, what => {helpBool=true;}) );
		StartCoroutine( pauseScreenHolder.transform.GetChild(0).GetComponent<Animation>().Play("PauseGo", false, what => {helpBool=true;}) );
		while(!helpBool)
		{
			yield return null;
		}
		helpBool=false;
		Time.timeScale = 1;
		Invoke("HidePauseScreen",0.75f);
		
	}
	
	void HidePauseScreen()
	{
		pauseScreenHolder.SetActive(false);
	}
	
	IEnumerator backToMenu()
	{
		StartCoroutine( GameObject.Find("ButtonMain_Pause").GetComponent<Animation>().Play("FinishButtonsClick", false, what => {helpBool=true;}) );
		StartCoroutine( pauseScreenHolder.transform.GetChild(0).GetComponent<Animation>().Play("PauseGo", false, what => {helpBool=true;}) );
		while(!helpBool)
		{
			yield return null;
		}
		helpBool=false;
		Application.LoadLevel(4);
		Time.timeScale = 1;
	}
	IEnumerator restartLevel()
	{
		StartCoroutine( GameObject.Find("ButtonRestart_Pause").GetComponent<Animation>().Play("FinishButtonsClick", false, what => {helpBool=true;}) );
		StartCoroutine( pauseScreenHolder.transform.GetChild(0).GetComponent<Animation>().Play("PauseGo", false, what => {helpBool=true;}) );
		while(!helpBool)
		{
			yield return null;
		}
		helpBool=false;
		Application.LoadLevel(Application.loadedLevel);
		Time.timeScale = 1;
	}
	IEnumerator unPause()
	{
		StartCoroutine( GameObject.Find("ButtonPlay_Pause").GetComponent<Animation>().Play("FinishButtonsClick", false, what => {helpBool=true;}) );
		StartCoroutine(dropPauseScreen());
		while(!helpBool)
		{
			yield return null;
		}
		helpBool=false;
		//Invoke("HidePauseScreen",0.5f);
	}
	
	void showFailedScreen()
	{
		//if(PlaySounds.BackgroundMusic_Gameplay.isPlaying)
		//	PlaySounds.Stop_BackgroundMusic_Gameplay();
		//PlaySounds.Play_Level_Failed_Popup();
		FailedScreenHolder.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y, FailedScreenHolder.transform.position.z);
		FailedScreenHolder.SetActive(true);
		FailedScreenHolder.transform.GetChild(0).GetComponent<Animation>().Play("FailedShow");
	}
	
	void ShowWinScreen()
	{
		textKeyPrice1.text = textKeyPrice2.text = ((3 - keysCollected) * 800).ToString();
		
		if(PlaySounds.BackgroundMusic_Gameplay.isPlaying)
			PlaySounds.Stop_BackgroundMusic_Gameplay();
		PlaySounds.Play_Level_Completed_Popup();
		Win_CompletedScreenHolder.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y-3, Win_CompletedScreenHolder.transform.position.z);
		Win_CompletedScreenHolder.SetActive(true);
		Win_ShineHolder.transform.position = new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y, Win_ShineHolder.transform.position.z);
		Win_CompletedScreenHolder.transform.GetChild(0).GetComponent<Animation>().Play("FinishTableShow");
		
		StartCoroutine(CheckKeys());
		
		//StartCoroutine(waitForStars());
	}
	
	IEnumerator CheckKeys()
	{
		if(keysCollected == 1)
		{
			yield return new WaitForSeconds(0.75f);
			Debug.Log("Kljuc");
			keyHole1.GetComponent<Animation>().Play("FinishKeyYes");
			yield return new WaitForSeconds(0.75f);
			keyHole2.GetComponent<Animation>().Play("FinishKeyNo");
			yield return new WaitForSeconds(0.25f);
			keyHole3.GetComponent<Animation>().Play("FinishKeyNo");
		}
		else if(keysCollected == 2)
		{
			yield return new WaitForSeconds(0.75f);
			keyHole1.GetComponent<Animation>().Play("FinishKeyYes");
			yield return new WaitForSeconds(0.25f);
			keyHole2.GetComponent<Animation>().Play("FinishKeyYes");
			yield return new WaitForSeconds(0.75f);
			keyHole3.GetComponent<Animation>().Play("FinishKeyNo");
		}
		else if(keysCollected == 3)
		{
			textPtsFinish.text = textPtsGameplay.text;
			buttonFacebookShare.transform.parent.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
			holderFinishInfo.SetActive(false);
			buttonBuyKeys.SetActive(false);
			buttonBuyKeys.transform.parent.gameObject.SetActive(false);
			yield return new WaitForSeconds(0.75f);
			keyHole1.GetComponent<Animation>().Play("FinishKeyYes");
			yield return new WaitForSeconds(0.25f);
			keyHole2.GetComponent<Animation>().Play("FinishKeyYes");
			yield return new WaitForSeconds(0.25f);
			keyHole3.GetComponent<Animation>().Play("FinishKeyYes");
			yield return new WaitForSeconds(0.5f);
			Win_CompletedScreenHolder.transform.GetChild(0).GetComponent<Animation>().Play("FinishPriceClick");
			holderTextCompleted.SetActive(true);
			yield return new WaitForSeconds(0.35f);
			buttonFacebookShare.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
			yield return new WaitForSeconds(0.45f);
			buttonFacebookShare.transform.parent.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
			StartCoroutine(waitForStars());
		}
		else
		{
			yield return new WaitForSeconds(0.75f);
			keyHole1.GetComponent<Animation>().Play("FinishKeyNo");
			yield return new WaitForSeconds(0.25f);
			keyHole2.GetComponent<Animation>().Play("FinishKeyNo");
			yield return new WaitForSeconds(0.25f);
			keyHole3.GetComponent<Animation>().Play("FinishKeyNo");
		}
		
	}
	
	IEnumerator waitForStars()
	{
		Win_ShineHolder.SetActive(true);
		yield return new WaitForSeconds(0.75f);
		//star1.SetActive(true);
		star1.GetComponent<Animation>().Play("FinishStars1");
		star1.transform.GetChild(0).gameObject.SetActive(true);
		yield return new WaitForSeconds(0.5f);
		GameObject.Find("FinishButtonTable").GetComponent<Animation>().Play("FinishShakingTableStars");
		//star1.transform.GetChild(0).gameObject.SetActive(true);
		starsGained = 1;
		PlaySounds.Play_GetStar();
		yield return new WaitForSeconds(0.25f);
		if(coinsCollected >= 70)
		{
			starsGained = 2;
			//star2.SetActive(true);
			star2.GetComponent<Animation>().Play("FinishStars2");
			star2.transform.GetChild(0).gameObject.SetActive(true);
			yield return new WaitForSeconds(0.5f);
			GameObject.Find("FinishButtonTable").GetComponent<Animation>().Play("FinishShakingTableStars");
			PlaySounds.Play_GetStar2();
			yield return new WaitForSeconds(0.25f);
		}
		if(coinsCollected >= 90)
		{
			starsGained = 3;
			//star3.SetActive(true);
			star3.GetComponent<Animation>().Play("FinishStars3");
			star3.transform.GetChild(0).gameObject.SetActive(true);
			yield return new WaitForSeconds(0.5f);
			GameObject.Find("FinishButtonTable").GetComponent<Animation>().Play("FinishShakingTableStars");
			PlaySounds.Play_GetStar3();
		}
		
		starManager.GoBack();
	}
	
	void CoinAdded()
	{
		if(PowerUp_doubleCoins)
			coinsCollected += 2;
		else 
			coinsCollected++;
		coinsCollectedText.text = coinsCollected.ToString();
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
	
	void ApplyPowerUp(int x)
	{
		if(x == 1)
		{
			PowerUp_magnet = true;
			coinMagnet.SetActive(true);
		}
		else if(x == 2)
		{
			PowerUp_doubleCoins = true;
		}
		else if(x == 3)
		{
			PowerUp_shield = true;
			shield.SetActive(true);
			playerController.activeShield = true;
		}
		else if(x == -3)
		{
			PowerUp_shield = false;
			shield.SetActive(false);
		}
	}
	
	void KeyCollected()
	{
		keysCollected++;
		if(keysCollected == 1)
			GameObject.Find("GamePlayKeyHole1").GetComponent<Animation>().Play();
		else if(keysCollected == 2)
			GameObject.Find("GamePlayKeyHole2").GetComponent<Animation>().Play();
		else if(keysCollected == 3)
			GameObject.Find("GamePlayKeyHole3").GetComponent<Animation>().Play();
	}
	
	public void AddPoints(int value)
	{
		collectedPoints += value;
		textPtsGameplay.text = collectedPoints.ToString();
		if(/*collectedPoints <= 1000 && */coinsCollected <= 100 && baboonSmashed <= 20 && progressBarScale.localScale.y <= 1)
		{
			//progressBarScale.localScale = new Vector3(progressBarScale.localScale.x, progressBarScale.localScale.y + value/1000f, progressBarScale.localScale.z);
			//progressBarScale.GetChild(0).GetChild(0).renderer.material.mainTextureScale = new Vector2(1,progressBarScale.GetChild(0).GetChild(0).renderer.material.mainTextureScale.y + value/1000f);
			StartCoroutine(graduallyFillScale(value/1000f));
			StartCoroutine(graduallyFillTile(value/1000f));
		}
//		if(progressBarScale.localScale.y >= 0.8f)
//		{
//			if(wonStar3.GetChild(0).localScale.x == 0)
//				wonStar3.animation.Play();
//		}
//		else if(progressBarScale.localScale.y < 0.8f && progressBarScale.localScale.y >= 0.5f)
//		{
//			if(wonStar2.GetChild(0).localScale.x == 0)
//				wonStar2.animation.Play();
//		}
//		else if(progressBarScale.localScale.y < 0.5f && progressBarScale.localScale.y >= 0.2f)
//		{
//			if(wonStar1.GetChild(0).localScale.x == 0)
//				wonStar1.animation.Play();
//		}
		if(progressBarScale.localScale.y >= 1)
		{
			progressBarScale.localScale = Vector3.one;
			progressBarScale.GetChild(0).GetChild(0).GetComponent<Renderer>().material.mainTextureScale = new Vector2(1,1);
		}
	}



	IEnumerator graduallyFillScale(float value)
	{
		Debug.Log("ulaazkak scale");
		float result = progressBarScale.localScale.y + value;
		float t = 0;
		while(t < result)
		{
			yield return null;
			if(progressBarScale.localScale.y <= 1)
			progressBarScale.localScale = Vector3.Lerp(progressBarScale.localScale, new Vector3(progressBarScale.localScale.x, result, progressBarScale.localScale.z), 0.2f);
			t += Time.deltaTime*2;

			if(progressBarScale.localScale.y >= 0.8f)
			{
				if(wonStar3.GetChild(0).localScale.x == 0)
					wonStar3.GetComponent<Animation>().Play();
			}
			else if(progressBarScale.localScale.y < 0.8f && progressBarScale.localScale.y >= 0.5f)
			{
				if(wonStar2.GetChild(0).localScale.x == 0)
					wonStar2.GetComponent<Animation>().Play();
			}
			else if(progressBarScale.localScale.y < 0.5f && progressBarScale.localScale.y >= 0.2f)
			{
				if(wonStar1.GetChild(0).localScale.x == 0)
					wonStar1.GetComponent<Animation>().Play();
			}
			if(progressBarScale.localScale.y >= 1)
			{
				progressBarScale.localScale = Vector3.one;
				progressBarScale.GetChild(0).GetChild(0).GetComponent<Renderer>().material.mainTextureScale = new Vector2(1,1);
			}
		}
	}

	IEnumerator graduallyFillTile(float value)
	{
		Debug.Log("ulaazkak tile");
		float result = progressBarScale.GetChild(0).GetChild(0).GetComponent<Renderer>().material.mainTextureScale.y + value;
		float t = 0;
		while(t < result)
		{
			yield return null;
			if(progressBarScale.localScale.y < 1)
			progressBarScale.GetChild(0).GetChild(0).GetComponent<Renderer>().material.mainTextureScale = Vector2.Lerp(progressBarScale.GetChild(0).GetChild(0).GetComponent<Renderer>().material.mainTextureScale, new Vector2(1, result), 0.2f);
			t += Time.deltaTime*2;
		}
	}
	
	IEnumerator BuyKeys()
	{
		buttonBuyKeys.GetComponent<Collider>().enabled = false;
		if(keysCollected == 0)
		{
			keyHole1.transform.GetChild(1).localScale = Vector3.zero;
			keyHole2.transform.GetChild(1).localScale = Vector3.zero;
			keyHole3.transform.GetChild(1).localScale = Vector3.zero;
			//yield return new WaitForSeconds(0.25f);
			keyHole1.GetComponent<Animation>().Play("FinishKeyYes");
			yield return new WaitForSeconds(0.25f);
			keyHole2.GetComponent<Animation>().Play("FinishKeyYes");
			yield return new WaitForSeconds(0.25f);
			keyHole3.GetComponent<Animation>().Play("FinishKeyYes");
		}
		if(keysCollected == 1)
		{
			keyHole2.transform.GetChild(1).localScale = Vector3.zero;
			keyHole3.transform.GetChild(1).localScale = Vector3.zero;
			//yield return new WaitForSeconds(0.25f);
			keyHole2.GetComponent<Animation>().Play("FinishKeyYes");
			yield return new WaitForSeconds(0.25f);
			keyHole3.GetComponent<Animation>().Play("FinishKeyYes");
		}
		if(keysCollected == 2)
		{
			keyHole3.transform.GetChild(1).localScale = Vector3.zero;
			//yield return new WaitForSeconds(0.25f);
			keyHole3.GetComponent<Animation>().Play("FinishKeyYes");
		}
		
		
		yield return new WaitForSeconds(0.45f);
		textPtsFinish.text = textPtsGameplay.text;
		buttonFacebookShare.transform.parent.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
		holderFinishInfo.SetActive(false);
		//buttonBuyKeys.SetActive(false);
		
		//yield return new WaitForSeconds(0.75f);
		//keyHole1.animation.Play("FinishKeyYes");
		//yield return new WaitForSeconds(0.25f);
		//keyHole2.animation.Play("FinishKeyYes");
		//yield return new WaitForSeconds(0.25f);
		//keyHole3.animation.Play("FinishKeyYes");
		//yield return new WaitForSeconds(0.5f);
		Win_CompletedScreenHolder.transform.GetChild(0).GetComponent<Animation>().Play("FinishPriceClick");
		holderTextCompleted.SetActive(true);
		yield return new WaitForSeconds(0.35f);
		buttonBuyKeys.transform.parent.gameObject.SetActive(false);
		buttonFacebookShare.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
		yield return new WaitForSeconds(0.45f);
		buttonFacebookShare.transform.parent.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
		StartCoroutine(waitForStars());
	}

	IEnumerator showPickPowers()
	{
		PickPowers.gameObject.SetActive(true);
		Invoke("DisappearPickPowers",3.5f);
		yield return new WaitForSeconds(0.85f);
		powerCard_CoinX2.GetComponent<Animator>().enabled = true;
		powerCard_Magnet.GetComponent<Animator>().enabled = true;
		powerCard_Shield.GetComponent<Animator>().enabled = true;
	}

	void DisappearPickPowers()
	{
		if(!kupljenMagnet)
		{
			//GameObject temp = GameObject.Find("PowersCardMagnet");
			powerCard_Magnet.GetComponent<Animator>().Play("PowerCardCoinx2_Disappear");
			powerCard_Magnet.GetComponent<Collider>().enabled = false;
		}
		if(!kupljenShield)
		{
			//GameObject temp = GameObject.Find("PowersCardShield");
			powerCard_Shield.GetComponent<Animator>().Play("PowerCardCoinx2_Disappear");
			powerCard_Shield.GetComponent<Collider>().enabled = false;
		}
		if(!kupljenDoubleCoins)
		{
			//GameObject temp = GameObject.Find("PowersCardCoinx2");
			powerCard_CoinX2.GetComponent<Animator>().Play("PowerCardCoinx2_Disappear");
			powerCard_CoinX2.GetComponent<Collider>().enabled = false;
		}
	}
}
