using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;
using GoogleMobileAds.Api;

public class Manage : MonoBehaviour {
	
	[HideInInspector]
	public static int coinsCollected = 0;
	[HideInInspector]
	public int starsGained = 0;
	public static int baboonsKilled = 0;
	public static int fly_BaboonsKilled = 0;
	public static int boomerang_BaboonsKilled = 0;
	public static int gorillasKilled = 0;
	public static int fly_GorillasKilled = 0;
	public static int koplje_GorillasKilled = 0;
	public static int barrelsSmashed = 0;
	public static int redDiamonds = 0;
	public static int blueDiamonds = 0;
	public static int greenDiamonds = 0;
	GameObject goScreen;
	GameObject player;
	MonkeyController2D playerController; 
	
	Transform pauseButton;
	
	[HideInInspector] public TextMesh coinsCollectedText;
	Transform pauseScreenHolder;
	Transform Win_CompletedScreenHolder;
	Transform FailedScreenHolder;
	Transform Win_ShineHolder;
	GameObject star1;
	GameObject star2;
	GameObject star3;
	
	
	bool helpBool;
	string clickedItem;
	string releasedItem;
	SetRandomStarsManager starManager;
	
	[HideInInspector]
	public bool PowerUp_doubleCoins = false;
	[HideInInspector]
	public bool PowerUp_shield = false;
	
	GameObject coinMagnet;
	GameObject shield;
	
	DateTime timeToShowNextElement;
	TextMesh zivotiText;
	TextMesh zivotiText2;
	
	Transform rateHolder;
	int kliknuoYes = 0;
	Vector3 originalScale;
	public static bool pauseEnabled = false;
	bool nemaReklame = false;

	float timeElapsed = 0;
	bool pocniVreme = false;
	public static float goTrenutak = 0;
	int stepenBrzine = 0;
	public static TextMesh pointsText;
	public static TextMeshEffects pointsEffects;
	public static int points = 0;
	public static int bananas = 0;

	Transform holderLife;
	public static bool shouldRaycast = false;

	Transform PickPowers;
	Transform powerCard_CoinX2;
	Transform powerCard_Magnet;
	Transform powerCard_Shield;
	
	bool kupljenShield;
	bool kupljenDoubleCoins;
	bool kupljenMagnet;

	int povecanaTezina = 0;
	bool postavljenFinish = false;
	GameObject temp;
	Transform _GUI;
	Camera guiCamera;
	int watchVideoReward = 1000;
	int makniPopup = 0; // 1 - Rate, 2 - WatchVideo, 3 - Keep Playing

	bool measureTime = true;
	[HideInInspector] public float aktivnoVreme = 0;
	[HideInInspector] public int keepPlayingCount = 1;

	Transform popupZaSpustanje = null;
	int pointsForDisplay = 0;
	bool neDozvoliPauzu = false;
	// Initialize an InterstitialAd.
	InterstitialAd interstitial;
	// Create an empty ad request.
	AdRequest request;
	// Load the interstitial with the request.

	static Manage instance;

	public static Manage Instance
	{
		get
		{
			if(instance == null)
				instance = GameObject.FindObjectOfType(typeof(Manage)) as Manage;

			return instance;
		}
	}
	
	void Awake ()
	{
		try
		{
			interstitial = new InterstitialAd(StagesParser.Instance.AdMobInterstitialID);
			request = new AdRequest.Builder().Build();
			interstitial.LoadAd(request);
		}
		catch
		{
			Debug.Log("AD NOT INITIALIZED");
		}
		instance = this;
		goScreen = transform.Find("GO screen").GetChild(0).gameObject;
		guiCamera = GameObject.Find("GUICamera").GetComponent<Camera>();

		starManager = GetComponent<SetRandomStarsManager>();
		player = GameObject.FindGameObjectWithTag("Monkey");
		playerController = player.GetComponent<MonkeyController2D>();
		coinsCollectedText = transform.Find("Gameplay Scena Interface/_TopLeft/Coins/CoinsGamePlayText").GetComponent<TextMesh>();
		pauseButton = transform.Find("Gameplay Scena Interface/_TopRight/Pause Button");

		pointsText = transform.Find("Gameplay Scena Interface/_TopLeft/PTS/PTS Number").GetComponent<TextMesh>();
		pointsEffects = pointsText.GetComponent<TextMeshEffects>();

		pauseScreenHolder = transform.Find("PAUSE HOLDER");
		FailedScreenHolder = transform.Find("Level Win_Lose SCENA/Popup za LOSE HOLDER");
		if(StagesParser.bonusLevel)
			Win_CompletedScreenHolder = transform.Find("Level Win_Lose SCENA/Popup za WIN HOLDER_BONUS");
		else
			Win_CompletedScreenHolder = transform.Find("Level Win_Lose SCENA/Popup za WIN HOLDER");
		star1 = GameObject.Find("Stars Polja 1");
		star2 = GameObject.Find("Stars Polja 2");
		star3 = GameObject.Find("Stars Polja 3");

		coinMagnet = player.transform.Find("CoinMagnet").gameObject;
		shield = GameObject.Find("Shield");
		shield.SetActive(false);

		
		rateHolder = transform.Find("RATE HOLDER/AnimationHolderGlavni/AnimationHolder/RATE Popup").transform;

		PickPowers = GameObject.Find("POWERS HOLDER").transform;
		powerCard_CoinX2 = GameObject.Find("Power_Double Coins Interface").transform;
		powerCard_Magnet = GameObject.Find("Power_Magnet Interface").transform;
		powerCard_Shield = GameObject.Find("Power_Shield Interface").transform;
		
		if(Camera.main.aspect < 1.5f)
		{
			Camera.main.orthographicSize = 18f;
		}
		else if(Camera.main.aspect > 1.5f)
		{
			Camera.main.orthographicSize = 15f;
		}
		else
		{
			Camera.main.orthographicSize = 16.5f;
		}
		
		if(PlaySounds.musicOn)
		{
			PlaySounds.Play_BackgroundMusic_Gameplay();
			if(PlaySounds.Level_Failed_Popup.isPlaying)
				PlaySounds.Stop_Level_Failed_Popup();
		}

		shouldRaycast = false;

		coinsCollected = 0;
		baboonsKilled = 0;
		fly_BaboonsKilled = 0;
		boomerang_BaboonsKilled = 0;
		gorillasKilled = 0;
		fly_GorillasKilled = 0;
		koplje_GorillasKilled = 0;
		points = 0;
		barrelsSmashed = 0;
		redDiamonds = 0;
		blueDiamonds = 0;
		greenDiamonds = 0;
		bananas = 0;
	}

	void Start ()
	{
		refreshText();

		if(Loading.Instance != null)
		StartCoroutine(Loading.Instance.UcitanaScena(guiCamera,1,0.5f));
		pauseEnabled = false;
		if(StagesParser.bonusLevel)
		{
			goScreen.GetComponent<TextMesh>().text += "\n" + LanguageManager.BonusLevel;
		}
		else
		{
			goScreen.GetComponent<TextMesh>().text += "\n" + LanguageManager.Level + " " + (StagesParser.currStageIndex+1);
		}
		goScreen.GetComponent<TextMeshEffects>().RefreshTextOutline(false,true,false);
		powerCard_CoinX2.parent.Find("CoinsHolder/Coins/Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
		powerCard_CoinX2.parent.Find("CoinsHolder/Coins/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		powerCard_CoinX2.Find("Number").GetComponent<TextMesh>().text = StagesParser.powerup_doublecoins.ToString();
		powerCard_Magnet.Find("Number").GetComponent<TextMesh>().text = StagesParser.powerup_magnets.ToString();
		powerCard_Shield.Find("Number").GetComponent<TextMesh>().text = StagesParser.powerup_shields.ToString();
		powerCard_CoinX2.Find("Text/Cost Number").GetComponent<TextMesh>().text = StagesParser.cost_doublecoins.ToString();
		powerCard_CoinX2.Find("Text/Cost Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		powerCard_Magnet.Find("Text/Cost Number").GetComponent<TextMesh>().text = StagesParser.cost_magnet.ToString();
		powerCard_Magnet.Find("Text/Cost Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		powerCard_Shield.Find("Text/Cost Number").GetComponent<TextMesh>().text = StagesParser.cost_shield.ToString();
		powerCard_Shield.Find("Text/Cost Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);

		Win_CompletedScreenHolder.Find("FB Invite Large/Text/Number").GetComponent<TextMesh>().text = "+" + StagesParser.InviteReward;
		Win_CompletedScreenHolder.Find("FB Invite Large/Text/Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,false);
		if(!StagesParser.bonusLevel)
		{
			Win_CompletedScreenHolder.Find("FB Share/Text/Number").GetComponent<TextMesh>().text = "+" + StagesParser.ShareReward;
			Win_CompletedScreenHolder.Find("FB Share/Text/Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,false);
		}

		FailedScreenHolder.gameObject.SetActive(false);
		Win_CompletedScreenHolder.gameObject.SetActive(false);

		if(guiCamera.aspect <= 1.4f && MissionManager.NumberOfQuests == 3)
		{
			transform.Find("Gameplay Scena Interface").localScale = Vector3.one * 0.91f;
			transform.Find("Gameplay Scena Interface").localPosition = new Vector3(-1.2f,13.5f,5f);
			transform.Find("Gameplay Scena Interface/_TopMissions").localPosition = new Vector3(1.6f,0,0);
		}
		transform.Find("Gameplay Scena Interface/_TopLeft").position = new Vector3(guiCamera.ViewportToWorldPoint(Vector3.zero).x,transform.Find("Gameplay Scena Interface/_TopLeft").position.y,transform.Find("Gameplay Scena Interface/_TopLeft").position.z);
		transform.Find("Gameplay Scena Interface/_TopRight").position = new Vector3(guiCamera.ViewportToWorldPoint(Vector3.one).x,transform.Find("Gameplay Scena Interface/_TopRight").position.y,transform.Find("Gameplay Scena Interface/_TopRight").position.z);

		if(Application.loadedLevelName.Equals("_Tutorial Level"))
		{
			if(PlayerPrefs.HasKey("VecPokrenuto"))
			{
				GameObject.Find("Banana_collect_Holder").SetActive(false);
			}
		}
	}

	void Update ()
	{

		if(pocniVreme)
		{
			if(measureTime)
				aktivnoVreme += 1*Time.deltaTime;

			if(aktivnoVreme >= 12)//if(Time.time - goTrenutak >= 12)
			{
				if(StagesParser.odgledaoTutorial == 0 && aktivnoVreme >= 35 && !postavljenFinish)//if(StagesParser.odgledaoTutorial == 0 && Time.time - goTrenutak >= 35 && !postavljenFinish)
				{
					postavljenFinish = true;
					playerController.Finish();
				}

//				if(aktivnoVreme >= 55)
//				{
//					if(StagesParser.bonusLevel && !postavljenFinish)
//					{
//						postavljenFinish = true;
//						playerController.Finish();
//					}
//				}
				if(aktivnoVreme >= 55)//if(Time.time - goTrenutak >= 55)
				{
					if(povecanaTezina == 0)
					{
						povecanaTezina = 1;
						LevelFactory.instance.overallDifficulty = 11;
					}
				}
				if(aktivnoVreme >= 70)//if(Time.time - goTrenutak >= 70)
				{
					if(povecanaTezina == 1)
					{
						povecanaTezina = 2;
						LevelFactory.instance.overallDifficulty = 16;
					}
				}
			}
			//UBRZANJE
			if(aktivnoVreme - timeElapsed >= 20 && stepenBrzine <= 7)//if(Time.time - timeElapsed >= 20 && stepenBrzine <= 7 && measureTime)
			{
				playerController.startSpeedX+=1;
				playerController.maxSpeedX+=1;
				playerController.majmun.GetComponent<Animator>().speed += 0.075f;// 0.15f;
				timeElapsed = aktivnoVreme;//timeElapsed = Time.time;
				stepenBrzine++;
				if(StagesParser.bossStage)
					BossScript.Instance.maxSpeedX=playerController.maxSpeedX;

			}

		}

		if(Input.GetKeyDown(KeyCode.Space) && goScreen.activeSelf)
		{
			Time.timeScale = 1;
			goScreen.transform.parent = transform;
			goScreen.SetActive(false);
			playerController.state = MonkeyController2D.State.running;

			if(PlaySounds.soundOn)
				PlaySounds.Play_Run();
			
			pocniVreme = true;
			goTrenutak = Time.time;
			//timeElapsed = Time.time;
		}
		
		else if(Input.GetKeyUp(KeyCode.Escape))
		{
			if(pauseEnabled && makniPopup == 0)
			{
				if(PlaySounds.soundOn)
					PlaySounds.Play_Button_Pause();
				
				PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
				PlayerPrefs.Save();
				
				if(Time.timeScale == 1)
				{
					if(!neDozvoliPauzu)
					{
						Time.timeScale = 0;
						StopAllCoroutines();
						pauseScreenHolder.GetChild(0).localPosition += new Vector3(0,35,0);
						pauseScreenHolder.GetChild(0).GetComponent<Animator>().Play("OpenPopup");
						pauseButton.GetComponent<Collider>().enabled = false;
						neDozvoliPauzu = true;
					}
				}
				else
				{
					popupZaSpustanje = pauseScreenHolder.GetChild(0);
					Invoke("spustiPopup",0.5f);
					pauseScreenHolder.GetChild(0).GetComponent<Animator>().Play("ClosePopup");
					//pauseButton.collider.enabled = true;
					Time.timeScale = 1;
				}

			}
			else if(makniPopup == 1)
			{
				popupZaSpustanje = transform.Find("RATE HOLDER").GetChild(0);
				Invoke("spustiPopup",0.5f);
				transform.Find("RATE HOLDER").GetChild(0).GetComponent<Animator>().Play("ClosePopup");
				
				kliknuoYes = 0;
				makniPopup = 0;
			}
			else if(makniPopup == 2)
			{
				popupZaSpustanje = transform.Find("WATCH VIDEO HOLDER");
				Invoke("spustiPopup",0.5f);
				transform.Find("WATCH VIDEO HOLDER").GetChild(0).GetComponent<Animator>().Play("ClosePopup");
				makniPopup = 0;
			}
			else if(makniPopup == 3)
			{
				showFailedScreen();
				makniPopup = 0;
			}
			else if(makniPopup == 4)
			{
				makniPopup = 0;
				StartCoroutine(CheckInternetConnection.Instance.ClosePopup());
			}
			else if(makniPopup == 5)
			{
				makniPopup = 2;
				StartCoroutine(CheckInternetConnection.Instance.ClosePopup());
			}
		}
		
		if(Input.GetMouseButtonDown(0))
		{
			//if(shouldRaycast)
			{
				clickedItem = RaycastFunction(Input.mousePosition);
				if(clickedItem == "RateButtonNO" || clickedItem == "RateButtonYES" || clickedItem == "WatchButtonNO" || clickedItem == "WatchButtonYES" || clickedItem == "Button Buy Banana"
				   || clickedItem == "Button Cancel" || clickedItem == "Button Play_Revive" || clickedItem == "Button Mapa_Win" || clickedItem == "Button Next_Win" || clickedItem == "Button Restart_Win"
				   || clickedItem == "Button Home_Failed" || clickedItem == "Button Mapa_Failed" || clickedItem == "Button Restart_Failed" || clickedItem == "Menu Button_Pause" || clickedItem == "Play Button_Pause" || clickedItem == "Restart Button_Pause")
				{
					temp = GameObject.Find(clickedItem);
					originalScale = temp.transform.localScale;
					temp.transform.localScale = originalScale * 0.8f;
				}
				else if(clickedItem != System.String.Empty && clickedItem != "Buy Button")
				{
					temp = GameObject.Find(clickedItem);
					originalScale = temp.transform.localScale;
				}
			}
		}
		
		
		if(Input.GetMouseButtonUp(0))// || Input.GetKeyDown(KeyCode.Space))
		{
			//if(shouldRaycast)
			{
				releasedItem = RaycastFunction(Input.mousePosition);
				if(clickedItem != System.String.Empty && clickedItem != "Buy Button")
				{
					if(temp != null)
						temp.transform.localScale = originalScale;
				}
				if(clickedItem == releasedItem)
				{
					if(releasedItem == "GO screen")
					{
						if(!StagesParser.bossStage)
						{
							Time.timeScale = 1;
							//playerController.originalCameraTargetPosition = player.transform.Find("PlayerFocus2D").localPosition.y;
							//goScreen.transform.parent = transform;
							goScreen.transform.parent.gameObject.SetActive(false);

							playerController.state = MonkeyController2D.State.running;

							playerController.majmun.GetComponent<Animator>().SetBool("Run",true);
							if(PlaySounds.soundOn)
								PlaySounds.Play_Run();
							int ukupanNivo = StagesParser.currSetIndex*20 + StagesParser.currStageIndex + 1;
							pauseEnabled = true;

							pocniVreme = true;
							goTrenutak = Time.time;
							shouldRaycast = false;
							if(!StagesParser.bonusLevel && MissionManager.missions[LevelFactory.level-1].distance > 0 && StagesParser.odgledaoTutorial > 0)
							{
								playerController.measureDistance = true;
								playerController.misijaSaDistance = true;
							}
							if(StagesParser.odgledaoTutorial > 0 && !StagesParser.bonusLevel)
							{
								transform.Find("POWERS HOLDER").GetChild(0).GetComponent<Animator>().Play("PowerUpDolazak");
								Invoke("DeaktivirajPowerUpAnimator",4f);
								StagesParser.brojIgranja++;
							}
						}
						else
						{
							BossScript.Instance.comeIntoTheWorld();
							goScreen.transform.parent.gameObject.SetActive(false);
							pauseEnabled = true;
							pocniVreme = true;
							goTrenutak = Time.time;
						}
					}
					else if(releasedItem == "Pause Button")
					{
						if(pauseEnabled)
						{
							if(PlaySounds.soundOn)
								PlaySounds.Play_Button_Pause();
							
							PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
							PlayerPrefs.Save();
							
							if(Time.timeScale == 1)
							{
								if(!neDozvoliPauzu)
								{
									Time.timeScale = 0;
									StopAllCoroutines();
									pauseScreenHolder.GetChild(0).localPosition += new Vector3(0,35,0);
									pauseScreenHolder.GetChild(0).GetComponent<Animator>().Play("OpenPopup");
									neDozvoliPauzu = true;
									pauseButton.GetComponent<Collider>().enabled = false;
								}
							}
							else
							{
								popupZaSpustanje = pauseScreenHolder.GetChild(0);
								Invoke("spustiPopup",0.5f);
								pauseScreenHolder.GetChild(0).GetComponent<Animator>().Play("ClosePopup");
								//pauseButton.collider.enabled = true;
								Time.timeScale = 1;
							}
						}
						
					}
					else if(releasedItem == "Menu Button_Pause")
					{
						if(StagesParser.bonusLevel)
						{
							StagesParser.bonusLevel = false;
							StagesParser.dodatnaProveraIzasaoIzBonusa = true;
						}

						playerController.GetComponent<Rigidbody2D>().isKinematic = true;

						if(PlaySounds.soundOn)
							PlaySounds.Play_Button_GoBack();
						//if(LifeManager_new.lifeLeft > 1)
						if(StagesParser.odgledaoTutorial == 0)
						{
							StagesParser.nivoZaUcitavanje = 1;
							StartCoroutine(closeDoorAndPlay());
							Time.timeScale = 1;
						}
						else
						{
							StagesParser.nivoZaUcitavanje = 4 + StagesParser.currSetIndex;
							StartCoroutine(closeDoorAndPlay());
							Time.timeScale = 1;
						}

					}
					
					else if(releasedItem == "Play Button_Pause") //otpauzirao
					{
						if(PlaySounds.soundOn)
							PlaySounds.Play_Button_Pause();
						popupZaSpustanje = pauseScreenHolder.GetChild(0);
						Invoke("spustiPopup",0.5f);
						pauseScreenHolder.GetChild(0).GetComponent<Animator>().Play("ClosePopup");
						//pauseButton.collider.enabled = true;
						Time.timeScale = 1;
					}
					
					else if(releasedItem == "Restart Button_Pause")
					{
						if(PlaySounds.soundOn)
							PlaySounds.Play_Button_RestartLevel();

						playerController.GetComponent<Rigidbody2D>().isKinematic = true;
						
						StagesParser.nivoZaUcitavanje = Application.loadedLevel;
						StartCoroutine(closeDoorAndPlay());

						Time.timeScale = 1;

					}
					
					else if(releasedItem == "Button Mapa_Failed")
					{
						if(PlaySounds.soundOn)
							PlaySounds.Play_Button_GoBack();
						if(PlaySounds.BackgroundMusic_Gameplay.isPlaying)
							PlaySounds.Stop_BackgroundMusic_Gameplay();
						StagesParser.nemojDaAnimirasZvezdice = true;
						StagesParser.nivoZaUcitavanje = 4 + StagesParser.currSetIndex;
						StartCoroutine(closeDoorAndPlay());
						
					}
					else if(releasedItem == "Button Home_Failed")
					{
						if(StagesParser.bonusLevel)
							StagesParser.bonusLevel = false;

						if(PlaySounds.soundOn)
							PlaySounds.Play_Button_GoBack();
						if(PlaySounds.BackgroundMusic_Gameplay.isPlaying)
							PlaySounds.Stop_BackgroundMusic_Gameplay();
						StagesParser.nivoZaUcitavanje = 1;
						StartCoroutine(closeDoorAndPlay());
					}

					else if(releasedItem == "Button Play_Revive")
					{
						pauseButton.GetComponent<Collider>().enabled = true;
						measureTime = true;
						makniPopup = 0;
						if(keepPlayingCount == 0)
							keepPlayingCount = 1;

						if(StagesParser.currentBananas >= keepPlayingCount)
						{
							if(PlaySounds.soundOn)
								PlaySounds.Play_Button_OpenLevel();
							popupZaSpustanje = transform.Find("Keep Playing HOLDER");
							Invoke("spustiPopup",0.5f);
							transform.Find("Keep Playing HOLDER").GetChild(0).GetComponent<Animator>().Play("ClosePopup");
							StagesParser.currentBananas-=keepPlayingCount;
							PlayerPrefs.SetInt("TotalBananas",StagesParser.currentBananas);
							PlayerPrefs.Save();
							transform.Find("Keep Playing HOLDER/AnimationHolderGlavni/AnimationHolder/Popup za Keep Playing/Bananas/Banana Number").GetComponent<TextMesh>().text = StagesParser.currentBananas.ToString();
							transform.Find("Keep Playing HOLDER/AnimationHolderGlavni/AnimationHolder/Popup za Keep Playing/Bananas/Banana Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
							playerController.SetInvincible();
							keepPlayingCount++;
						}
						else
							transform.Find("Keep Playing HOLDER/AnimationHolderGlavni/AnimationHolder/Popup za Keep Playing/Bananas").GetComponent<Animation>().Play();
					}

					else if(releasedItem.Equals("Button Cancel"))
					{
						showFailedScreen();
					}

					else if(releasedItem.Equals("Button Buy Banana"))
					{
						if(StagesParser.currentMoney > StagesParser.bananaCost)
						{
							if(PlaySounds.soundOn)
								PlaySounds.Play_CollectBanana();
							StagesParser.currentMoney -= StagesParser.bananaCost;
							StagesParser.currentBananas += 1;
							PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
							PlayerPrefs.Save();
							PlayerPrefs.SetInt("TotalBananas",StagesParser.currentBananas);
							PlayerPrefs.Save();
							StagesParser.ServerUpdate = 1;
							transform.Find("Keep Playing HOLDER/AnimationHolderGlavni/AnimationHolder/Popup za Keep Playing/Bananas/Banana Number").GetComponent<TextMesh>().text = StagesParser.currentBananas.ToString();
							transform.Find("Keep Playing HOLDER/AnimationHolderGlavni/AnimationHolder/Popup za Keep Playing/Bananas/Banana Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
							StartCoroutine(StagesParser.Instance.moneyCounter(-StagesParser.bananaCost,transform.Find("Keep Playing HOLDER/AnimationHolderGlavni/AnimationHolder/Popup za Keep Playing/CoinsHolder/Coins/Coins Number").GetComponent<TextMesh>(),true));
						}
						else
						{
							transform.Find("Keep Playing HOLDER/AnimationHolderGlavni/AnimationHolder/Popup za Keep Playing/CoinsHolder/Coins").GetComponent<Animation>().Play();
						}
					}
					
					else if(releasedItem == "Button Restart_Failed")
					{
						if(PlaySounds.soundOn)
							PlaySounds.Play_Button_RestartLevel();
						
						StagesParser.nivoZaUcitavanje = Application.loadedLevel;
						StartCoroutine(closeDoorAndPlay());
							
					}
					
					else if(releasedItem == "Button Mapa_Win")
					{
						if(PlaySounds.soundOn)
							PlaySounds.Play_Button_GoBack();

						if(StagesParser.odgledaoTutorial == 0)
						{
							StagesParser.loadingTip = 2;
							StagesParser.odgledaoTutorial = 1;
							PlayerPrefs.SetString("OdgledaoTutorial",StagesParser.odgledaoTutorial.ToString()+"#"+StagesParser.otvaraoShopNekad.ToString());
							if(!PlayerPrefs.HasKey("VecPokrenuto"))
							{
								PlayerPrefs.SetInt("VecPokrenuto",1);
							}
							PlayerPrefs.Save();
						}

						else if(StagesParser.odgledaoTutorial == 1)
						{
							StagesParser.odgledaoTutorial = 2;
							PlayerPrefs.SetString("OdgledaoTutorial",StagesParser.odgledaoTutorial.ToString()+"#"+StagesParser.otvaraoShopNekad.ToString());
							PlayerPrefs.Save();
						}

						if(StagesParser.currSetIndex == 5 && StagesParser.currStageIndex == 19) //@@@@@@
						{
							StagesParser.nivoZaUcitavanje = 18;
							StartCoroutine(closeDoorAndPlay());
						}
						else
						{
							if(StagesParser.isJustOpened)
							{
								StagesParser.isJustOpened = false;

								StagesParser.currStageIndex = 0;
								StagesParser.currSetIndex = StagesParser.lastUnlockedWorldIndex;
								StagesParser.worldToFocus = StagesParser.currSetIndex;
								StagesParser.nivoZaUcitavanje = 3;
								StartCoroutine(closeDoorAndPlay());
							}
							else
							{
								StagesParser.nivoZaUcitavanje = 3;
								StartCoroutine(closeDoorAndPlay());
							}
						}
					}
					
					else if(releasedItem == "Button Next_Win")
					{
						if(PlaySounds.soundOn)
							PlaySounds.Play_Button_NextLevel();

						if(StagesParser.otvaraoShopNekad == 0)
						{
							StagesParser.otvaraoShopNekad = 2;
							PlayerPrefs.SetString("OdgledaoTutorial",StagesParser.odgledaoTutorial.ToString()+"#"+StagesParser.otvaraoShopNekad.ToString());
							PlayerPrefs.Save();
						}

						if(StagesParser.odgledaoTutorial == 0)
						{
							StagesParser.loadingTip = 2;
							StagesParser.odgledaoTutorial = 1;
							PlayerPrefs.SetString("OdgledaoTutorial",StagesParser.odgledaoTutorial.ToString()+"#"+StagesParser.otvaraoShopNekad.ToString());
							if(!PlayerPrefs.HasKey("VecPokrenuto"))
							{
								PlayerPrefs.SetInt("VecPokrenuto",1);
							}
							PlayerPrefs.Save();

						}
						
						else if(StagesParser.odgledaoTutorial == 1)
						{
							StagesParser.odgledaoTutorial = 2;
							PlayerPrefs.SetString("OdgledaoTutorial",StagesParser.odgledaoTutorial.ToString()+"#"+StagesParser.otvaraoShopNekad.ToString());
							PlayerPrefs.Save();
						}

						if(StagesParser.bonusLevel)
						{
							StagesParser.bonusLevel = false;
							StagesParser.nivoZaUcitavanje = 4 + StagesParser.currSetIndex;
							StartCoroutine(closeDoorAndPlay());

						}
						if(StagesParser.currSetIndex == 5 && StagesParser.currStageIndex == 19) //@@@@@@ CHANGE
						{
							StagesParser.nivoZaUcitavanje = 18;
							StartCoroutine(closeDoorAndPlay());
						}
						else
						{
							if(StagesParser.isJustOpened)
							{
								StagesParser.nemojDaAnimirasZvezdice = true;
								StagesParser.isJustOpened = false;
								StagesParser.currStageIndex = 0;
								StagesParser.currSetIndex = StagesParser.lastUnlockedWorldIndex;
								StagesParser.worldToFocus = StagesParser.currSetIndex;
								StagesParser.nivoZaUcitavanje = 3;
								StartCoroutine(closeDoorAndPlay());
							}
							else if(StagesParser.NemaRequiredStars_VratiULevele)
							{
								StagesParser.nivoZaUcitavanje = 4 + StagesParser.currSetIndex;
								StartCoroutine(closeDoorAndPlay());
							}
						}

					}
					
					else if(releasedItem == "Button Restart_Win")
					{
						if(PlaySounds.soundOn)
							PlaySounds.Play_Button_RestartLevel();

						if(StagesParser.odgledaoTutorial == 0)
						{
							if(!PlayerPrefs.HasKey("VecPokrenuto"))
							{
								PlayerPrefs.SetInt("VecPokrenuto",1);
							}
							PlayerPrefs.Save();
						}

						StagesParser.nivoZaUcitavanje = Application.loadedLevel;
						StartCoroutine(closeDoorAndPlay());
						
					}

					else if(releasedItem == "RateButtonNO")
					{
						if(PlaySounds.soundOn)
							PlaySounds.Play_Button_OpenLevel();

						popupZaSpustanje = transform.Find("RATE HOLDER").GetChild(0);
						Invoke("spustiPopup",0.5f);
						transform.Find("RATE HOLDER").GetChild(0).GetComponent<Animator>().Play("ClosePopup");
						kliknuoYes = 0;
					}
					else if(releasedItem == "WatchButtonYES")
					{
						if(PlaySounds.soundOn)
							PlaySounds.Play_Button_OpenLevel();
						makniPopup = 5;
						StartCoroutine(checkConnectionForWatchVideo());
					}
					else if(releasedItem == "WatchButtonNO")
					{
						if(PlaySounds.soundOn)
							PlaySounds.Play_Button_OpenLevel();
						popupZaSpustanje = transform.Find("WATCH VIDEO HOLDER");
						Invoke("spustiPopup",0.5f);
						transform.Find("WATCH VIDEO HOLDER").GetChild(0).GetComponent<Animator>().Play("ClosePopup");
					}
					else if(releasedItem == "Tutorial1_Screen")
					{
						if(TutorialEvents.postavljenCollider)
						{
							if(PlaySounds.soundOn)
								PlaySounds.Play_Button_OpenLevel();
							GameObject.Find(releasedItem).SetActive(false);
							Time.timeScale = 1;
							pauseEnabled = true;
						}
					}
					else if(releasedItem == "Tutorial2_Screen")
					{
						if(TutorialEvents.postavljenCollider)
						{
							if(PlaySounds.soundOn)
								PlaySounds.Play_Button_OpenLevel();
							GameObject.Find(releasedItem).SetActive(false);
							Time.timeScale = 1;
							pauseEnabled = true;
						}
					}
					else if(releasedItem.Contains("Tutorial"))
					{
						GameObject.Find(releasedItem).transform.parent.parent.GetComponent<Animator>().Play("ClosePopup");
						Time.timeScale = 1;
						pauseEnabled = true;
					}
					else if(releasedItem == "Power_Double Coins Interface")
					{
						if(StagesParser.powerup_doublecoins > 0)
						{
							powerCard_CoinX2.GetComponent<Collider>().enabled = false;
							powerCard_CoinX2.Find("Disabled").GetComponent<SpriteRenderer>().enabled = true;
							StagesParser.powerup_doublecoins--;
							powerCard_CoinX2.Find("Number").GetComponent<TextMesh>().text = StagesParser.powerup_doublecoins.ToString();
							kupljenDoubleCoins = true;
							ApplyPowerUp(2);
							playerController.doublecoins = true;
							if(PlaySounds.soundOn)
								PlaySounds.Play_CollectPowerUp();
							PlayerPrefs.SetString("PowerUps",(StagesParser.powerup_doublecoins+"#"+StagesParser.powerup_magnets+"#"+StagesParser.powerup_shields));
							PlayerPrefs.Save();
						}
						else if(StagesParser.cost_doublecoins < StagesParser.currentMoney)
						{
							powerCard_CoinX2.GetComponent<Collider>().enabled = false;
							powerCard_CoinX2.Find("Disabled").GetComponent<SpriteRenderer>().enabled = true;
							kupljenDoubleCoins = true;
							ApplyPowerUp(2);
							playerController.doublecoins = true;
							StagesParser.currentMoney -= StagesParser.cost_doublecoins;
							PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
							PlayerPrefs.Save();
							StartCoroutine(StagesParser.Instance.moneyCounter(-StagesParser.cost_doublecoins,powerCard_Magnet.parent.Find("CoinsHolder/Coins/Coins Number").GetComponent<TextMesh>(),true));
							if(PlaySounds.soundOn)
								PlaySounds.Play_CollectPowerUp();
						}
						else
							powerCard_CoinX2.parent.Find("CoinsHolder/Coins").GetComponent<Animation>().Play();
					}
					else if(releasedItem == "Power_Magnet Interface")
					{
						if(StagesParser.powerup_magnets > 0)
						{
							powerCard_Magnet.GetComponent<Collider>().enabled = false;
							powerCard_Magnet.Find("Disabled").GetComponent<SpriteRenderer>().enabled = true;
							StagesParser.powerup_magnets--;
							powerCard_Magnet.Find("Number").GetComponent<TextMesh>().text = StagesParser.powerup_magnets.ToString();
							kupljenMagnet = true;
							ApplyPowerUp(1);
							playerController.magnet = true;
							if(PlaySounds.soundOn)
								PlaySounds.Play_CollectPowerUp();
							PlayerPrefs.SetString("PowerUps",(StagesParser.powerup_doublecoins+"#"+StagesParser.powerup_magnets+"#"+StagesParser.powerup_shields));
							PlayerPrefs.Save();
						}
						else if(StagesParser.cost_magnet < StagesParser.currentMoney)
						{
							powerCard_Magnet.GetComponent<Collider>().enabled = false;
							powerCard_Magnet.Find("Disabled").GetComponent<SpriteRenderer>().enabled = true;
							kupljenMagnet = true;
							ApplyPowerUp(1);
							playerController.magnet = true;
							StagesParser.currentMoney -= StagesParser.cost_magnet;
							PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
							PlayerPrefs.Save();
							StartCoroutine(StagesParser.Instance.moneyCounter(-StagesParser.cost_magnet,powerCard_Magnet.parent.Find("CoinsHolder/Coins/Coins Number").GetComponent<TextMesh>(),true));
							if(PlaySounds.soundOn)
								PlaySounds.Play_CollectPowerUp();
						}
						else
							powerCard_Magnet.parent.Find("CoinsHolder/Coins").GetComponent<Animation>().Play();
					}
					else if(releasedItem == "Power_Shield Interface")
					{
						if(StagesParser.powerup_shields > 0)
						{
							powerCard_Shield.GetComponent<Collider>().enabled = false;
							powerCard_Shield.Find("Disabled").GetComponent<SpriteRenderer>().enabled = true;
							StagesParser.powerup_shields--;
							powerCard_Shield.Find("Number").GetComponent<TextMesh>().text = StagesParser.powerup_shields.ToString();
							kupljenShield = true;
							ApplyPowerUp(3);
							playerController.activeShield = true;
							if(PlaySounds.soundOn)
								PlaySounds.Play_CollectPowerUp();
							PlayerPrefs.SetString("PowerUps",(StagesParser.powerup_doublecoins+"#"+StagesParser.powerup_magnets+"#"+StagesParser.powerup_shields));
							PlayerPrefs.Save();
						}
						else if(StagesParser.cost_shield < StagesParser.currentMoney)
						{
							powerCard_Shield.GetComponent<Collider>().enabled = false;
							powerCard_Shield.Find("Disabled").GetComponent<SpriteRenderer>().enabled = true;
							kupljenShield = true;
							ApplyPowerUp(3);
							playerController.activeShield = true;
							StagesParser.currentMoney -= StagesParser.cost_shield;
							PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
							PlayerPrefs.Save();
							StartCoroutine(StagesParser.Instance.moneyCounter(-StagesParser.cost_shield,powerCard_Magnet.parent.Find("CoinsHolder/Coins/Coins Number").GetComponent<TextMesh>(),true));
							if(PlaySounds.soundOn)
								PlaySounds.Play_CollectPowerUp();
						}
						else
							powerCard_Shield.parent.Find("CoinsHolder/Coins").GetComponent<Animation>().Play();
					}

					else if(releasedItem.Contains("RateGame"))
					{
						if(PlaySounds.soundOn)
							PlaySounds.Play_GetStar();
						int starsGiven = int.Parse(releasedItem.Substring(8))+1;
						Transform rateGameHolder = rateHolder.Find("RateGameHolder");
						for(int i=0;i<rateGameHolder.childCount;i++)
						{
							rateGameHolder.GetChild(i).GetChild(0).GetComponent<Renderer>().enabled = false;
						}

						for(int i=0;i<starsGiven;i++)
						{
							rateGameHolder.Find("RateGame"+i.ToString()).GetChild(0).GetComponent<Renderer>().enabled = true;
						}
						if(starsGiven > 3)
						{
							StartCoroutine(checkConnectionForRate());
						}
						transform.Find("RATE HOLDER").GetChild(0).GetComponent<Animator>().Play("ClosePopup");
					}
					else if(releasedItem.Equals("FB Share"))
					{
						makniPopup = 4;
						StartCoroutine(checkConnectionForShare());
					}
					else if(releasedItem.Equals("FB Invite Large"))
					{
						makniPopup = 4;
						StartCoroutine(checkConnectionForInvite());						
					}
					else if(releasedItem.Contains("Friend Level"))
					{
						makniPopup = 4;
						StartCoroutine(checkConnectionForInvite());
					}
					else if(releasedItem.Equals("Button_CheckOK"))
					{
						if(PlaySounds.soundOn)
							PlaySounds.Play_Button_OpenLevel();
						makniPopup = 0;
						StartCoroutine(CheckInternetConnection.Instance.ClosePopup());
					}
				}
			}
		}
	}

	IEnumerator RateGame(string url)
	{
		StartCoroutine(CheckInternetConnection.Instance.checkInternetConnection());
		while(!CheckInternetConnection.Instance.checkDone)
		{
			yield return null;
		}
		if(CheckInternetConnection.Instance.internetOK)
		{
			PlayerPrefs.SetInt("AlreadyRated",1);
			PlayerPrefs.Save();
			Application.OpenURL(url);
		}
		else
		{
			CheckInternetConnection.Instance.openPopup();
		}
	}


	IEnumerator openingPage(string url)
	{
		WWW www = new WWW("http://www.google.com");
		yield return www;
		
		if (!string.IsNullOrEmpty(www.error))
		{
			rateHolder.Find("RateButtonYES/Text").GetComponent<TextMesh>().text = "Retry";
			rateHolder.Find("RateButtonYES/Text").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			rateHolder.Find("Text 2").GetComponent<TextMesh>().text = "No Internet\nConnection!";
			rateHolder.Find("Text 2").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		}
		else
		{
			popupZaSpustanje = transform.Find("RATE HOLDER").GetChild(0);
			Invoke("spustiPopup",0.5f);
			transform.Find("RATE HOLDER").GetChild(0).GetComponent<Animator>().Play("ClosePopup");
			PlayerPrefs.SetInt("AlreadyRated",1);
			PlayerPrefs.Save();
			Application.OpenURL(url);
		}
		
	}
	
	IEnumerator backToMenu()
	{
		StartCoroutine( GameObject.Find("Menu_Pause").GetComponent<Animation>().Play("ClickMenu", false, what => {helpBool=true;}) );
		while(!helpBool)
		{
			yield return null;
		}
		helpBool=false;
		StagesParser.nivoZaUcitavanje = 4 + StagesParser.currSetIndex;
		StartCoroutine(closeDoorAndPlay());
		Time.timeScale = 1;
	}
	IEnumerator restartLevel()
	{
		StartCoroutine( GameObject.Find("Restart_Pause").GetComponent<Animation>().Play("ClickRestart", false, what => {helpBool=true;}) );
		while(!helpBool)
		{
			yield return null;
		}
		helpBool=false;
		StagesParser.nivoZaUcitavanje = Application.loadedLevel;
		StartCoroutine(closeDoorAndPlay());
		Time.timeScale = 1;
	}
	IEnumerator unPause()
	{
		StartCoroutine( GameObject.Find("Play_Pause").GetComponent<Animation>().Play("ClickMenu", false, what => {helpBool=true;}) );
		while(!helpBool)
		{
			yield return null;
		}
		helpBool=false;
	}
	
	void showFailedScreen()
	{
		if(PlaySounds.BackgroundMusic_Gameplay.isPlaying)
			PlaySounds.Stop_BackgroundMusic_Gameplay();
		if(PlaySounds.soundOn)
			PlaySounds.Play_Level_Failed_Popup();
		pauseEnabled = false;
		StagesParser.numberGotKilled++;
		if(StagesParser.numberGotKilled % 1 == 0)
		{
			StagesParser.numberGotKilled = 0;
		}
		FailedScreenHolder.parent.transform.position = new Vector3(guiCamera.transform.position.x,guiCamera.transform.position.y, FailedScreenHolder.transform.position.z);
		FailedScreenHolder.gameObject.SetActive(true);
		FailedScreenHolder.parent.GetComponent<Animator>().Play("LevelWinLoseDolazak");
		FailedScreenHolder.GetComponent<Animator>().Play("LevelLoseUlaz");
		
		PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
		PlayerPrefs.Save();
		try
		{
			if (interstitial.IsLoaded()) 
			{
				interstitial.Show();
			}
		}
		catch
		{
			Debug.Log("LEVEL FAILED - INTERSTITIAL NOT INITIALIZED");
		}
	}
	
	void OpaliPartikle()
	{
		Win_CompletedScreenHolder.parent.Find("Partikli Level Finish Win").gameObject.SetActive(true);
	}

	void ShowWinScreen()
	{
		pauseEnabled = false;
		measureTime = false;

		if(StagesParser.bonusLevel)
		{
			transform.Find("Level Win_Lose SCENA/Popup za WIN HOLDER_BONUS/Popup za WIN/Header za win popup/Text/Level").GetComponent<TextMesh>().text = LanguageManager.BonusLevel;
			transform.Find("Level Win_Lose SCENA/Popup za WIN HOLDER_BONUS/Popup za WIN/Header za win popup/Text/Level").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			transform.Find("Level Win_Lose SCENA/Popup za WIN HOLDER_BONUS/Popup za WIN/Header za win popup/Text/Completed").GetComponent<TextMesh>().text = LanguageManager.Completed;
			transform.Find("Level Win_Lose SCENA/Popup za WIN HOLDER_BONUS/Popup za WIN/Header za win popup/Text/Completed").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

			Win_CompletedScreenHolder.Find("Friends FB level WIN").gameObject.SetActive(false);
			Win_CompletedScreenHolder.Find("FB Share").gameObject.SetActive(false);
			Win_CompletedScreenHolder.gameObject.SetActive(true);

			if(!FB.IsLoggedIn)
			{
				Win_CompletedScreenHolder.Find("FB Invite Large").gameObject.SetActive(false);
			}

			if(PlaySounds.BackgroundMusic_Gameplay.isPlaying)
				PlaySounds.Stop_BackgroundMusic_Gameplay();
			if(PlaySounds.soundOn)
				PlaySounds.Play_Level_Completed_Popup();
			Win_CompletedScreenHolder.parent.position = new Vector3(guiCamera.transform.position.x,guiCamera.transform.position.y, Win_CompletedScreenHolder.transform.position.z);
			Win_CompletedScreenHolder.parent.GetComponent<Animator>().Play("LevelWinLoseDolazak");
			Win_CompletedScreenHolder.GetComponent<Animator>().Play("LevelWinUlaz");
			Invoke("OpaliPartikle",1.2f);
			StagesParser.currentMoney += coinsCollected;
			PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
			PlayerPrefs.Save();

			starManager.GoBack();
			StartCoroutine(WaitForSave());

	
		}
		else
		{
			Win_CompletedScreenHolder.Find("Popup za WIN/Header za win popup/Text/Level No").GetComponent<TextMesh>().text = (StagesParser.currStageIndex+1).ToString();
			Win_CompletedScreenHolder.Find("Popup za WIN/Header za win popup/Text/Level No").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);

			StagesParser.currentMoney += coinsCollected;
			PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
			PlayerPrefs.Save();
			
			if(aktivnoVreme <= 40)
				points += MissionManager.points3Stars;


			pointsForDisplay = points;
			
			string[] levelValues = StagesParser.allLevels[StagesParser.currentLevel-1].Split('#');
			int previousPoints = int.Parse(levelValues[2]);

			if(previousPoints >= points)
			{
				points = previousPoints;
			}
			else
			{
				int razlika = points - previousPoints;
				StagesParser.currentPoints += razlika;
				PlayerPrefs.SetInt("TotalPoints",StagesParser.currentPoints);
				PlayerPrefs.Save();
			}



			if(!FB.IsLoggedIn)
			{
				Win_CompletedScreenHolder.Find("FB Invite Large").gameObject.SetActive(false);
				Win_CompletedScreenHolder.Find("FB Share").gameObject.SetActive(false);
				Win_CompletedScreenHolder.Find("Friends FB level WIN").gameObject.SetActive(false);
			}
			else
			{
				getFriendsScoresOnLevel(StagesParser.currentLevel);
			}

			Win_CompletedScreenHolder.gameObject.SetActive(true);

			if(PlaySounds.BackgroundMusic_Gameplay.isPlaying)
				PlaySounds.Stop_BackgroundMusic_Gameplay();
			if(PlaySounds.soundOn)
				PlaySounds.Play_Level_Completed_Popup();
			Win_CompletedScreenHolder.parent.position = new Vector3(guiCamera.transform.position.x,guiCamera.transform.position.y, Win_CompletedScreenHolder.transform.position.z);
			Win_CompletedScreenHolder.parent.GetComponent<Animator>().Play("LevelWinLoseDolazak");
			Win_CompletedScreenHolder.GetComponent<Animator>().Play("LevelWinUlaz");
			Invoke("OpaliPartikle",1.2f);

			StagesParser.currentBananas += bananas;
			PlayerPrefs.SetInt("TotalBananas",StagesParser.currentBananas);
			PlayerPrefs.Save();

			StagesParser.ServerUpdate = 1;

			StartCoroutine(waitForStars());
		}
	

		
	}

	IEnumerator waitForStars()
	{
		if(MissionManager.points3Stars > 0)
			Win_CompletedScreenHolder.Find("Popup za WIN/ProgressBarHolder/3StarsPtsText").GetComponent<TextMesh>().text = MissionManager.points3Stars.ToString();
		else //Tutorial Level
			Win_CompletedScreenHolder.Find("Popup za WIN/ProgressBarHolder/3StarsPtsText").GetComponent<TextMesh>().text = "500";
		Win_CompletedScreenHolder.Find("Popup za WIN/ProgressBarHolder/3StarsPtsText").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		StagesParser.saving = false;
		yield return new WaitForSeconds(2);

		Transform progressBarPivot = Win_CompletedScreenHolder.Find("Popup za WIN/ProgressBarHolder/ProgressBarPivot");
		TextMesh scoreDisplay = Win_CompletedScreenHolder.Find("Popup za WIN/Polje za unos poena Na Level WIN/Points Number level win").GetComponent<TextMesh>();
		int currentProgressBarPoints = 0;
		bool starActivated = false;
		float targetScaleX = Mathf.Clamp01((float)pointsForDisplay/MissionManager.points3Stars);
		star1.GetComponent<Animation>().Play();
		star1.transform.Find("Star Vatromet").gameObject.SetActive(true);
		starsGained = 1;
		if(PlaySounds.soundOn)
			PlaySounds.Play_GetStar();

		int step = (int)((pointsForDisplay*Time.deltaTime/2)/targetScaleX);

		while(progressBarPivot.localScale.x < targetScaleX || currentProgressBarPoints <= pointsForDisplay)
		{
			if(progressBarPivot.localScale.x >= 0.7f && !starActivated)
			{
				starActivated = true;
				starsGained = 2;
				if(PlaySounds.soundOn)
					PlaySounds.Play_GetStar2();
				star2.GetComponent<Animation>().Play();
				star2.transform.Find("Star Vatromet").gameObject.SetActive(true);
			}
			currentProgressBarPoints += step;
			scoreDisplay.text = currentProgressBarPoints.ToString();
			scoreDisplay.GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			if(currentProgressBarPoints > pointsForDisplay)
			{
				scoreDisplay.text = pointsForDisplay.ToString();
				scoreDisplay.GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			}
			yield return null;
			progressBarPivot.localScale = new Vector3(Mathf.MoveTowards(progressBarPivot.localScale.x,targetScaleX,Time.deltaTime/2), progressBarPivot.localScale.y, progressBarPivot.localScale.z);

		}
		starActivated = false;
		if(progressBarPivot.localScale.x == 1 && !starActivated)
		{
			starActivated = true;
			starsGained = 3;
			if(PlaySounds.soundOn)
				PlaySounds.Play_GetStar3();
			star3.GetComponent<Animation>().Play();
			star3.transform.Find("Star Vatromet").gameObject.SetActive(true);
		}
		starManager.GoBack();
		StartCoroutine(WaitForSave());
	}
	
	IEnumerator waitForStars1()
	{
		StagesParser.saving = false;
		yield return new WaitForSeconds(2);
		star1.GetComponent<Animation>().Play();
		star1.transform.Find("Star Vatromet").gameObject.SetActive(true);
		starsGained = 1;
		if(PlaySounds.soundOn)
			PlaySounds.Play_GetStar();
		yield return new WaitForSeconds(0.25f);
		if(points >= MissionManager.points3Stars*0.7f)
		{
			starsGained = 2;
			if(PlaySounds.soundOn)
				PlaySounds.Play_GetStar2();
			star2.GetComponent<Animation>().Play();
			star2.transform.Find("Star Vatromet").gameObject.SetActive(true);
			yield return new WaitForSeconds(0.25f);
		}
		if(points >= MissionManager.points3Stars)
		{
			starsGained = 3;
			if(PlaySounds.soundOn)
				PlaySounds.Play_GetStar3();
			star3.GetComponent<Animation>().Play();
			star3.transform.Find("Star Vatromet").gameObject.SetActive(true);
		}
		starManager.GoBack();
		StartCoroutine(WaitForSave());
	}
	
	
	IEnumerator WaitForSave()
	{
		try
		{
			if (interstitial.IsLoaded()) 
			{
				interstitial.Show();
			}
		}
		catch
		{
			Debug.Log("LEVEL COMPLETED - INTERSTITIAL NOT INITIALIZED");
		}
		while(StagesParser.saving == false)
		{
			yield return null;
		}

		if(FB.IsLoggedIn)
		{
			FacebookManager.FacebookObject.scoreToSet = StagesParser.currentPoints;
			FacebookManager.FacebookObject.proveraPublish_ActionPermisije();
		}

		Transform a1 = Win_CompletedScreenHolder.Find("Popup za WIN/Button Mapa_Win");
		Transform a2 = Win_CompletedScreenHolder.Find("Popup za WIN/Button Next_Win");
		Transform a3 = Win_CompletedScreenHolder.Find("Popup za WIN/Button Restart_Win");

		a1.GetComponent<Collider>().enabled = true;
		a2.GetComponent<Collider>().enabled = true;
		a3.GetComponent<Collider>().enabled = true;
		
		StagesParser.saving = false;

		if(!StagesParser.bonusLevel)
		{

			if(!PlayerPrefs.HasKey("AlreadyRated") && StagesParser.currStageIndex == 5) //5
			{
				transform.Find("RATE HOLDER").GetChild(0).localPosition += new Vector3(0,35,0);
				transform.Find("RATE HOLDER").GetChild(0).GetComponent<Animator>().Play("OpenPopup");
				makniPopup = 1;
			}
	//		else
	//		{
	//			if(StagesParser.brojIgranja >= 8)
	//			{
	//				StagesParser.brojIgranja = 0;
	//				Transform temp = transform.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup");
	//				temp.Find("Coins/Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
	//				temp.Find("Coins/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
	//				temp.Find("Reward").GetComponent<TextMesh>().text = watchVideoReward.ToString();
	//				temp.Find("Reward").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
	//				temp.Find("NotAvailableText").gameObject.SetActive(false);
	//				
	//				transform.Find("WATCH VIDEO HOLDER").GetChild(0).localPosition += new Vector3(0,35,0);
	//				transform.Find("WATCH VIDEO HOLDER").GetChild(0).GetComponent<Animator>().Play("OpenPopup");
	//				makniPopup = 2;
	//			}
	//		}
			//@@@@@@@@@@@@@@@@@@ BEZ WATCH VIDEO U GAMEPLAY
//			if(StagesParser.currStageIndex == 1 || StagesParser.currStageIndex == 14)
//			{
//				Transform temp = transform.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup");
//				temp.Find("Coins/Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
//				temp.Find("Coins/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
//				temp.Find("Reward").GetComponent<TextMesh>().text = watchVideoReward.ToString();
//				temp.Find("Reward").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
//				temp.Find("NotAvailableText").gameObject.SetActive(false);
//
//				transform.Find("WATCH VIDEO HOLDER").GetChild(0).localPosition += new Vector3(0,35,0);
//				transform.Find("WATCH VIDEO HOLDER").GetChild(0).GetComponent<Animator>().Play("OpenPopup");
//				makniPopup = 2;
//			}
			
			int ukupanNivo = StagesParser.currentLevel;

			string poslednjiOdigranNivo = ukupanNivo.ToString();

			if(PlayerPrefs.HasKey("PoslednjiOdigranNivo"))
			{
				if(PlayerPrefs.GetInt("PoslednjiOdigranNivo") >= ukupanNivo) // ponavlja nivo koji je vec igrao
				{
					ukupanNivo = PlayerPrefs.GetInt("PoslednjiOdigranNivo");
				}
				else // odigrao je novi nivo
				{
					PlayerPrefs.SetInt("PoslednjiOdigranNivo",ukupanNivo);
					PlayerPrefs.Save();
				}
			}
			else
			{
				PlayerPrefs.SetInt("PoslednjiOdigranNivo",ukupanNivo);
				PlayerPrefs.Save();
			}
		}
		
	}
	
	public void UnlockWorld(int world)
	{
		Transform worldUnlocked = transform.Find("WORLD UNLOCKED TURBO HOLDER");
		worldUnlocked.Find("WORLD UNLOCKED HOLDER/AllWorldPicturesHolder/WorldBg"+world).gameObject.SetActive(true);
		worldUnlocked.Find("WORLD UNLOCKED HOLDER/Number Holder/WorldNumber"+world).gameObject.SetActive(true);
		worldUnlocked.position = Camera.main.transform.position + Vector3.forward*2;
		worldUnlocked.localScale = worldUnlocked.localScale * Camera.main.orthographicSize/7.5f;
		worldUnlocked.gameObject.SetActive(true);
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
	
	public void ApplyPowerUp(int x)
	{
		if(x == 1)
		{
			CancelInvoke("pustiAnimacijuBlinkanja");
			transform.Find("Gameplay Scena Interface/_TopLeft/Magnet Icon Holder").GetComponent<Animation>().Play();
			Invoke("pustiAnimacijuBlinkanja",17.5f);
			coinMagnet.SetActive(true);

		}
		else if(x == -1)
		{
			coinMagnet.SetActive(false);
			CancelInvoke("pustiAnimacijuBlinkanja");
			transform.Find("Gameplay Scena Interface/_TopLeft/Magnet Icon Holder/Icon Animation").localScale = new Vector3(0.0001f,0.0001f,1);
		}
		else if(x == 2)
		{
			if(!PowerUp_doubleCoins)
			{
				transform.Find("Gameplay Scena Interface/_TopLeft/DoubleCoins Icon Holder").GetComponent<Animation>().Play();
				PowerUp_doubleCoins = true;
				LevelFactory.instance.doubleCoinsCollected = true;
			}
		}
		else if(x == -2)
		{
			PowerUp_doubleCoins = false;
			transform.Find("Gameplay Scena Interface/_TopLeft/DoubleCoins Icon Holder/Icon Animation").localScale = new Vector3(0.0001f,0.0001f,1);
		}
		else if(x == 3)
		{
			PowerUp_shield = true;
			shield.SetActive(true);
		}
		else if(x == -3)
		{
			PowerUp_shield = false;
			shield.SetActive(false);
		}
	}

	void pustiAnimacijuBlinkanja()
	{
		if(!MissionManager.missionsComplete)
		{
			transform.Find("Gameplay Scena Interface/_TopLeft/Magnet Icon Holder").GetComponent<Animation>().Play("PowerUp Icon Disappear NEW");
			Invoke("UgasiMagnet",1.5f);
		}
	}

	void UgasiMagnet()
	{
		coinMagnet.SetActive(false);
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
		PickPowers.parent = null;
	}

	void RemoveFog()
	{
		Camera.main.GetComponent<Animator>().Play("FogOfWar_Remove");
	}

	void ShowKeepPlayingScreen()
	{
		measureTime = false;
		Transform temp = transform.Find("Keep Playing HOLDER/AnimationHolderGlavni/AnimationHolder/Popup za Keep Playing");
		temp.Find("Text/Banana Number").GetComponent<TextMesh>().text = (keepPlayingCount+1).ToString();
		temp.Find("Text/Banana Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		temp.Find("CoinsHolder/Coins/Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
		temp.Find("CoinsHolder/Coins/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		temp.Find("Bananas/Banana Number").GetComponent<TextMesh>().text = StagesParser.currentBananas.ToString();
		temp.Find("Bananas/Banana Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		temp.Find("Text/BananaCost").GetComponent<TextMesh>().text = StagesParser.bananaCost.ToString();
		temp.Find("Text/BananaCost").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);

		transform.Find("Keep Playing HOLDER").GetChild(0).localPosition += new Vector3(0,35,0);
		transform.Find("Keep Playing HOLDER").GetChild(0).GetComponent<Animator>().Play("OpenPopup");
		makniPopup = 3;
	}

	IEnumerator closeDoorAndPlay()
	{
		transform.Find("LOADING HOLDER NEW/Loading Animation Vrata").GetComponent<Animator>().Play("Loading Zidovi Dolazak");
		yield return new WaitForSeconds(0.75f);
		Application.LoadLevel(2);
	}

	void refreshText()
	{
		transform.Find("GO screen/GO screen Text").GetComponent<TextMesh>().text = LanguageManager.TapScreenToStart;
		transform.Find("GO screen/GO screen Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true,false);
		transform.Find("Keep Playing HOLDER/AnimationHolderGlavni/AnimationHolder/Popup za Keep Playing/Button Buy Banana/BuyText").GetComponent<TextMesh>().text = LanguageManager.Buy;
		transform.Find("Keep Playing HOLDER/AnimationHolderGlavni/AnimationHolder/Popup za Keep Playing/Button Buy Banana/BuyText").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		transform.Find("Keep Playing HOLDER/AnimationHolderGlavni/AnimationHolder/Popup za Keep Playing/Text/Keep Playing").GetComponent<TextMesh>().text = LanguageManager.KeepPlaying;
		transform.Find("Keep Playing HOLDER/AnimationHolderGlavni/AnimationHolder/Popup za Keep Playing/Text/Keep Playing").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		transform.Find("Level Win_Lose SCENA/Popup za LOSE HOLDER/Popup za LOSE/Header za LOSE popup/Text/Level Failed").GetComponent<TextMesh>().text = LanguageManager.LevelFailed;
		transform.Find("Level Win_Lose SCENA/Popup za LOSE HOLDER/Popup za LOSE/Header za LOSE popup/Text/Level Failed").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		Win_CompletedScreenHolder.Find("FB Invite Large/Text/Invite").GetComponent<TextMesh>().text = LanguageManager.Invite;
		Win_CompletedScreenHolder.Find("FB Invite Large/Text/Invite").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		if(!StagesParser.bonusLevel)
		{
			Win_CompletedScreenHolder.Find("FB Share/Text/Share").GetComponent<TextMesh>().text = LanguageManager.Share;
			Win_CompletedScreenHolder.Find("FB Share/Text/Share").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		}
		Win_CompletedScreenHolder.Find("Friends FB level WIN/Friends Level Win 1 HOLDER/FB/Fb Invite 1").GetComponent<TextMesh>().text = LanguageManager.Invite;
		Win_CompletedScreenHolder.Find("Friends FB level WIN/Friends Level Win 2 HOLDER/FB/Fb Invite 1").GetComponent<TextMesh>().text = LanguageManager.Invite;
		Win_CompletedScreenHolder.Find("Friends FB level WIN/Friends Level Win 3 HOLDER/FB/Fb Invite 1").GetComponent<TextMesh>().text = LanguageManager.Invite;
		Win_CompletedScreenHolder.Find("Friends FB level WIN/Friends Level Win 4 HOLDER/FB/Fb Invite 1").GetComponent<TextMesh>().text = LanguageManager.Invite;
		Win_CompletedScreenHolder.Find("Friends FB level WIN/Friends Level Win 5 HOLDER/FB/Fb Invite 1").GetComponent<TextMesh>().text = LanguageManager.Invite;
		Win_CompletedScreenHolder.Find("Friends FB level WIN/Friends Level Win 1 HOLDER/FB/Fb Invite 1").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		Win_CompletedScreenHolder.Find("Friends FB level WIN/Friends Level Win 2 HOLDER/FB/Fb Invite 1").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		Win_CompletedScreenHolder.Find("Friends FB level WIN/Friends Level Win 3 HOLDER/FB/Fb Invite 1").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		Win_CompletedScreenHolder.Find("Friends FB level WIN/Friends Level Win 4 HOLDER/FB/Fb Invite 1").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		Win_CompletedScreenHolder.Find("Friends FB level WIN/Friends Level Win 5 HOLDER/FB/Fb Invite 1").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		Win_CompletedScreenHolder.Find("Popup za WIN/Header za win popup/Text/Level").GetComponent<TextMesh>().text = LanguageManager.Level;
		Win_CompletedScreenHolder.Find("Popup za WIN/Header za win popup/Text/Level").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		Win_CompletedScreenHolder.Find("Popup za WIN/Header za win popup/Text/Completed").GetComponent<TextMesh>().text = LanguageManager.Completed;
		Win_CompletedScreenHolder.Find("Popup za WIN/Header za win popup/Text/Completed").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		transform.Find("PAUSE HOLDER/AnimationHolderGlavni/AnimationHolder/PAUSE PopUp/PauseText").GetComponent<TextMesh>().text = LanguageManager.Pause;
		transform.Find("PAUSE HOLDER/AnimationHolderGlavni/AnimationHolder/PAUSE PopUp/PauseText").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		transform.Find("RATE HOLDER/AnimationHolderGlavni/AnimationHolder/RATE Popup/Rate").GetComponent<TextMesh>().text = LanguageManager.RateThisGame;
		transform.Find("RATE HOLDER/AnimationHolderGlavni/AnimationHolder/RATE Popup/Rate").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		transform.Find("RATE HOLDER/AnimationHolderGlavni/AnimationHolder/RATE Popup/Text 1").GetComponent<TextMesh>().text = LanguageManager.HowWouldYouRate;
		transform.Find("RATE HOLDER/AnimationHolderGlavni/AnimationHolder/RATE Popup/Text 1").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		transform.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup/Free Coins").GetComponent<TextMesh>().text = LanguageManager.FreeCoins;
		transform.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup/Free Coins").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		transform.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup/WatchButtonNO/Text").GetComponent<TextMesh>().text = LanguageManager.No;
		transform.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup/WatchButtonNO/Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		transform.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup/WatchButtonYES/Text").GetComponent<TextMesh>().text = LanguageManager.Yes;
		transform.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup/WatchButtonYES/Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		transform.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup/WatchVideoText").GetComponent<TextMesh>().text = LanguageManager.WatchVideo;
		transform.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup/WatchVideoText").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		transform.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup/NotAvailableText").GetComponent<TextMesh>().text = LanguageManager.NoVideo;
		transform.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup/NotAvailableText").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
	}

	struct scoreAndIndex
	{
		public int score;
		public int index;
		
		public scoreAndIndex(int score, int index)
		{
			this.score = score;
			this.index = index;
		}
	}

	bool popunioSlike = false;
	void getFriendsScoresOnLevel(int level)
	{
		if(!popunioSlike)
		{
			popunioSlike = true;
			for(int i=0;i<FacebookManager.ListaStructPrijatelja.Count;i++)
			{
				for(int j=0;j<FacebookManager.ProfileSlikePrijatelja.Count;j++)
				{
					//Debug.Log("JOPET LISTA PRIJATELJA PA ID: " + FacebookManager.ListaStructPrijatelja[i].PrijateljID + ", PROFILNE SLIKE: " + FacebookManager.ProfileSlikePrijatelja[j].PrijateljID);
					if(FacebookManager.ListaStructPrijatelja[i].PrijateljID == FacebookManager.ProfileSlikePrijatelja[j].PrijateljID)
					{
						//Debug.Log("ULETEO SA ID: " + FacebookManager.ListaStructPrijatelja[i].PrijateljID);
						FacebookManager.StrukturaPrijatelja tempPrijatelj = FacebookManager.ListaStructPrijatelja[i];
						tempPrijatelj.profilePicture = FacebookManager.ProfileSlikePrijatelja[j].profilePicture;
						FacebookManager.ListaStructPrijatelja[i] = tempPrijatelj;
					}
				}
			}
		}

		List<scoreAndIndex> scoresAndIndexes = new List<scoreAndIndex>();
		//Dictionary<int,int> scoresAndIndexes = new Dictionary<int, int>();
		for(int i=0;i<FacebookManager.ListaStructPrijatelja.Count;i++)
		{
			scoreAndIndex pom = new scoreAndIndex();
			if(level <= FacebookManager.ListaStructPrijatelja[i].scores.Count) //@@@@@@ DODATAK ZA NOVA OSTRVA
			{
				pom.index = i;
				pom.score = FacebookManager.ListaStructPrijatelja[i].scores[level-1];
				
				//scoresAndIndexes.Add(i,FacebookManager.ListaStructPrijatelja[i].scores[level-1]);
				
				if(FacebookManager.ListaStructPrijatelja[i].PrijateljID==FacebookManager.User) //korisnik
				{
					int localniScore = points;
					if(localniScore > FacebookManager.ListaStructPrijatelja[i].scores[level-1])
					{
						pom.score = localniScore;
					}
				}
				
				scoresAndIndexes.Add(pom);
			}
		}
		
		// I Nacin: ubacivanje scores u pomocnu listu, built-in sortiranje liste i pravljenje nove liste struktura na osnovu uredjene liste score-ova
		////////////////////////////////////////////////////
		//		List<int> sortList = new List<int>();
		//		for(int i=0;i<scoresAndIndexes.Count;i++)
		//		{
		//			sortList.Add(scoresAndIndexes[i].score);
		//		}
		//		sortList.Sort();
		//		sortList.Reverse();
		//
		//		List<scoreAndIndex> scoresAndIndexesSorted = new List<scoreAndIndex>();
		//		for(int i=0;i<sortList.Count;i++)
		//		{
		//			for(int j=0;j<scoresAndIndexes.Count;j++)
		//			{
		//				if(sortList[i] == scoresAndIndexes[j].score)
		//				{
		//					scoresAndIndexesSorted.Add(scoresAndIndexes[j]);
		//					break;
		//				}
		//			}
		//		}
		
		// II Nacin: koriscenje jedne liste struktura i rucno sortiranje metodom insertion sort
		////////////////////////////////////////////////////
		
		//scoresAndIndexes.Clear();
		//scoreAndIndex[] scoresAndIndexes2 = new scoreAndIndex[] {new scoreAndIndex(2350,0),new scoreAndIndex(1350,0),new scoreAndIndex(3500,0),new scoreAndIndex(4570,0),new scoreAndIndex(4350,0),new scoreAndIndex(2770,0),new scoreAndIndex(3950,0),new scoreAndIndex(2750,0)};
		//List<scoreAndIndex> scoresAndIndexes2 = new List<scoreAndIndex>(new scoreAndIndex[] {new scoreAndIndex(2350,0),new scoreAndIndex(1350,0),new scoreAndIndex(3500,0),new scoreAndIndex(4570,0),new scoreAndIndex(4350,0),new scoreAndIndex(2770,0),new scoreAndIndex(3950,0),new scoreAndIndex(2750,0),});
		
		scoreAndIndex max = new scoreAndIndex();
		scoreAndIndex pomm = new scoreAndIndex();
		pomm.score = 0;
		int maxIndex = 0;
		bool odradimuga = false;
		for(int i=0;i<scoresAndIndexes.Count;i++)
		{
			max = scoresAndIndexes[i];
			maxIndex = i;
			odradimuga = false;
			
			for(int j=i+1;j<scoresAndIndexes.Count;j++)
			{
				if(max.score < scoresAndIndexes[j].score)
				{
					max = scoresAndIndexes[j];
					maxIndex = j;
					odradimuga = true;
				}
			}
			//if(i != maxIndex)
			if(odradimuga)
			{
				pomm = scoresAndIndexes[i];
				scoresAndIndexes[i] = scoresAndIndexes[maxIndex];
				scoresAndIndexes[maxIndex] = pomm;
			}
		}
		/////////////////////////////////////////////////////
		
		int rez=5;
		
		//		for(int i=0;i<rez;i++)
		//		{
		//
		//		}
		int redosled = 1;
		bool korisnik = false;
		int pozicijaKorisnika = 1;
		Transform trenutniPrijatelj;
		for(int i=0;i<scoresAndIndexes.Count;i++)
		{
			if(FacebookManager.ListaStructPrijatelja[scoresAndIndexes[i].index].PrijateljID==FacebookManager.User)
				pozicijaKorisnika = redosled;

			if(i<5)
			{
				if(scoresAndIndexes[i].score > 0 || FacebookManager.ListaStructPrijatelja[scoresAndIndexes[i].index].PrijateljID==FacebookManager.User)
				{
					trenutniPrijatelj = Win_CompletedScreenHolder.Find("Friends FB level WIN/Friends Level Win " + redosled + " HOLDER");
					trenutniPrijatelj.Find("FB").gameObject.SetActive(false);
					if(!trenutniPrijatelj.Find("Friends Level Win " + redosled).gameObject.activeSelf)
					{
						trenutniPrijatelj.Find("Friends Level Win " + redosled).gameObject.SetActive(true);
					}
					trenutniPrijatelj.Find("Friends Level Win " + redosled+"/Friends Level Win Picture " + redosled).GetComponent<Renderer>().material.mainTexture = FacebookManager.ProfileSlikePrijatelja[scoresAndIndexes[i].index].profilePicture;
					trenutniPrijatelj.Find("Friends Level Win " + redosled+"/Friends Level Win Picture " + redosled + "/Points Number level win fb").GetComponent<TextMesh>().text = scoresAndIndexes[i].score.ToString();
					if(FacebookManager.ListaStructPrijatelja[scoresAndIndexes[i].index].PrijateljID==FacebookManager.User)
					{
						korisnik = true;
						trenutniPrijatelj.Find("Friends Level Win " + redosled).GetComponent<SpriteRenderer>().sprite = trenutniPrijatelj.parent.Find("ReferencaYOU").GetComponent<SpriteRenderer>().sprite;
						
					}
					else
					{
						trenutniPrijatelj.Find("Friends Level Win " + redosled).GetComponent<SpriteRenderer>().sprite = trenutniPrijatelj.parent.Find("Referenca").GetComponent<SpriteRenderer>().sprite;
					}
				}
				else
				{
					if(redosled <= 5)
					{
						Win_CompletedScreenHolder.Find("Friends FB level WIN/Friends Level Win " + redosled + " HOLDER/FB").gameObject.SetActive(true);
						Win_CompletedScreenHolder.Find("Friends FB level WIN/Friends Level Win " + redosled + " HOLDER/Friends Level Win " + redosled).gameObject.SetActive(false);
					}
				}
			}
			redosled++;
		}
		if(scoresAndIndexes.Count < 5)
		{
			for(int i=redosled;i<=5;i++)
			{
				Win_CompletedScreenHolder.Find("Friends FB level WIN/Friends Level Win " + i + " HOLDER/Friends Level Win " + i).gameObject.SetActive(false);
			}
		}
		if(!korisnik)
		{
			trenutniPrijatelj = Win_CompletedScreenHolder.Find("Friends FB level WIN/Friends Level Win 5 HOLDER");
			trenutniPrijatelj.Find("Friends Level Win 5/Friends Level Win Picture 5").GetComponent<Renderer>().material.mainTexture = FacebookManager.ListaStructPrijatelja[scoresAndIndexes[pozicijaKorisnika-1].index].profilePicture;
			trenutniPrijatelj.Find("Friends Level Win 5/Friends Level Win Picture 5/Points Number level win fb").GetComponent<TextMesh>().text = scoresAndIndexes[pozicijaKorisnika-1].score.ToString();
			trenutniPrijatelj.Find("Friends Level Win 5/Friends Level Win Picture 5/Position Number").GetComponent<TextMesh>().text = pozicijaKorisnika.ToString();
			trenutniPrijatelj.Find("Friends Level Win 5").GetComponent<SpriteRenderer>().sprite = trenutniPrijatelj.parent.Find("ReferencaYOU").GetComponent<SpriteRenderer>().sprite;
		}
		scoresAndIndexes.Clear();
	}

	void spustiPopup()
	{
		popupZaSpustanje.localPosition += new Vector3(0,-35,0);
		popupZaSpustanje = null;
		if(pauseEnabled)
		{
			if(neDozvoliPauzu)
			{
				neDozvoliPauzu = false;
				pauseButton.GetComponent<Collider>().enabled = true;
			}
		}
	}

	void DeaktivirajPowerUpAnimator()
	{
		//transform.Find("POWERS HOLDER").GetChild(0).GetComponent<Animator>().enabled = false;
		transform.Find("POWERS HOLDER").gameObject.SetActive(false);
	}
	IEnumerator checkConnectionForWatchVideo()
	{
		StartCoroutine(CheckInternetConnection.Instance.checkInternetConnection());
		while(!CheckInternetConnection.Instance.checkDone)
		{
			yield return null;
		}
		if(CheckInternetConnection.Instance.internetOK)
		{
			//@@@@@@@@@@@@@ SKLANJA SE ADCOLONY
//			if(AdColony.IsVideoAvailable(AdsScript.zoneID1))
//			{
//				//AdsScript.sceneID = 2;
//				AdsScript.reward = watchVideoReward;
//				AdColony.ShowVideoAd();
//			}
//			else
//			{
//				CheckInternetConnection.Instance.NoVideosAvailable_OpenPopup();
//			}
			//@@@@@@@@@@@@@
			StagesParser.sceneID = 2;
			//WebelinxCMS.Instance.IsActionLoaded("watch_video");

//			if(!nemaReklame)
//			{
//				#if UNITY_ANDROID
//				try{
//					using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) 
//					{
//						using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) 
//						{
//							obj_Activity.Call("PozoviReklamu");
//						}
//					}
//				}
//				catch{}
//				#elif UNITY_IPHONE
//				//WebelinxBinding.sendMessage("ShowAdColony","vz5b3ca7d70f4d4326a4");
//				#endif
//			}
//			else
			{
				//StartCoroutine(makniRate());
				popupZaSpustanje = transform.Find("WATCH VIDEO HOLDER");
				Invoke("spustiPopup",0.5f);
				transform.Find("WATCH VIDEO HOLDER").GetChild(0).GetComponent<Animator>().Play("ClosePopup");
			}
		}
		else
		{
			CheckInternetConnection.Instance.openPopup();
		}
	}

	void WatchVideoCallback()
	{
		StagesParser.currentMoney += watchVideoReward;
		PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
		PlayerPrefs.Save();
		StartCoroutine(StagesParser.Instance.moneyCounter(watchVideoReward, transform.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup/Coins/Coins Number").GetComponent<TextMesh>(), true));
		StagesParser.ServerUpdate = 1;
	}

	IEnumerator checkConnectionForRate()
	{
		StartCoroutine(CheckInternetConnection.Instance.checkInternetConnection());
		while(!CheckInternetConnection.Instance.checkDone)
		{
			yield return null;
		}
		if(CheckInternetConnection.Instance.internetOK)
		{
			StartCoroutine(RateGame(StagesParser.Instance.rateLink));
		}
		else
		{
			CheckInternetConnection.Instance.openPopup();
		}
	}
	IEnumerator checkConnectionForInvite()
	{
		StartCoroutine(CheckInternetConnection.Instance.checkInternetConnection());
		while(!CheckInternetConnection.Instance.checkDone)
		{
			yield return null;
		}
		if(CheckInternetConnection.Instance.internetOK)
		{
			FacebookManager.FacebookObject.FaceInvite();
		}
		else
		{
			CheckInternetConnection.Instance.openPopup();
		}
	}
	IEnumerator checkConnectionForShare()
	{
		StartCoroutine(CheckInternetConnection.Instance.checkInternetConnection());
		while(!CheckInternetConnection.Instance.checkDone)
		{
			yield return null;
		}
		if(CheckInternetConnection.Instance.internetOK)
		{
			FacebookManager.FacebookObject.ProveriPermisije();
		}
		else
		{
			CheckInternetConnection.Instance.openPopup();
		}
	}

}
