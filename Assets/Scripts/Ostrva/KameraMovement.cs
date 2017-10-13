using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Advertisements;

public class KameraMovement : MonoBehaviour {


	GameObject Kamera;
	Camera guiCamera;
	int currentLevelStars = 0;
	string clickedItem;
	string releasedItem;
	float trajanjeKlika = 0;
	float pomerajOdKlika_X = 0;
	float pomerajOdKlika_Y = 0;
	float startX;
	float startY;
	float endX;
	float endY;
	float pomerajX;
	float pomerajY;
	bool moved;
	bool released;
	bool bounce;
	float levaGranica = 9f;
	float desnaGranica = 31.95f;
	float donjaGranica = -15.35f;
	float gornjaGranica = -5.2f;
	Transform lifeManager;

	Vector2 prevDist = new Vector2(0,0);
	Vector2 curDist = new Vector2(0,0);
	float touchDelta = 0.0F;
	float touchDeltaY = 0.0F;
	float minPinchSpeed = 0.001F;
	float varianceInDistances = 9.0F;
	float speedTouch0 = 0.0F;
	float speedTouch1 = 0.0F;
	float moveFactor=0.07f;
	bool zoom = false;

	public Transform doleLevo;
	public Transform doleDesno;
	public Transform goreLevo;
	public Transform goreDesno;
	bool pomeriKameruUGranice = false;

	float ortSize;
	float aspect;

	Vector3 ivicaEkrana;
	Transform holderMajmun;
	Transform majmun;
	Animator animator;

	Vector3[] angles;
	public int angleIndex = 0;
	Vector3 newAngle;
	Vector3 monkeyDestination;

	public Transform izmedjneTacke;
	bool majmunceSeMrda = false;
	int monkeyCurrentLevelIndex = 0;
	int monkeyDestinationLevelIndex;
	int levelName;
	bool kretanjeDoKovcega = false;
	Transform trenutniKovceg;
	Transform kovcegNaPocetku;
	public Transform[] BonusNivoi;
	int zadnjiOtkljucanNivo_proveraZaBonus = 0;
	bool televizorNaMapi = false;
	public GameObject quad;
	int watchVideoIndex_pom;
	public Transform[] Televizori;
	int trenutniTelevizor;
	Transform _GUI;
	bool televizorIzabrao = false;
	bool prejasiTelevizor = false;

	public static Renderer aktivnaIkonicaMisija1 = null;
	public static Renderer aktivnaIkonicaMisija2 = null;
	public static Renderer aktivnaIkonicaMisija3 = null;

	float guiCameraY;

	int[] televizorCenePoSvetovima;

	public static int makniPopup = 0; // 1 - misija, 2 - unlock bonus, 3 - bonus reward, 4 - watch video, 5 - shop, 6 - Tutorial delovi, 7 - Reward
	public GameObject TutorialShopPrefab;
	GameObject TutorialShop;
	GameObject shop;

	int reward1Type = 0; // 0 - nista, 1 - magnet, 2 - double coins, 3 - shield, 4 - item
	int reward2Type = 0;
	int reward3Type = 0;
	int kolicinaReward1 = 0;
	int kolicinaReward2 = 0;
	int kolicinaReward3 = 0;

	string cetvrtiKovcegNagrada = "";
	int indexNagradeZaCetvrtiKovceg = -1;
	Transform popupZaSpustanje = null;

	void Awake()
	{
		if(Advertisement.isSupported)
		{
			Advertisement.Initialize(StagesParser.Instance.UnityAdsVideoGameID);
		}
		else
		{
			Debug.Log("UNITYADS Platform not supported");
		}
		//"KOLIKO MI SE CINI STO SE TICE OSTALOG SVE JE U REDU, TREBA SAMO NA UREDJAJU DA SE ISPROBA DA LI LEPO PROVALJUJE I DA SE VIDI STA CE DA BUDE SA SERVERSKIM DELOM, ONDA TREBA DA SE ISPROVERAVA GAMEPLAY"
		makniPopup = 0;
		#if UNITY_ANDROID
		televizorCenePoSvetovima = new int[6] {1000,1000,1000,1000,1000,1000} ; //CHANGE
		#elif UNITY_IPHONE
		televizorCenePoSvetovima = new int[5] {500,500,500,500,500} ;
		#endif
		string[] vallues = PlayerPrefs.GetString("WatchVideoWorld"+(StagesParser.currSetIndex+1)).Split('#');
		_GUI = GameObject.Find("_GUI").transform;

		if(Televizori != null)
		{
			for(int i=0;i<vallues.Length;i++)
			{
				for(int j=0;j<Televizori.Length;j++)
				{
					if(vallues[i] == Televizori[j].name.Substring(Televizori[j].name.Length-1))
						Televizori[j].gameObject.SetActive(false);
				}
			}
		}

		angles = new Vector3[] 
		{
			new Vector3(18,102,336),
			new Vector3(48,154,358),
			new Vector3(30,232,25),
			new Vector3(12,258,31),
			new Vector3(350,293,37),
			new Vector3(349,3,5),
			new Vector3(344,45,337),
			new Vector3(5,91,348)
		};

		ortSize = Camera.main.orthographicSize;
		aspect = Camera.main.aspect;

		//lifeManager = GameObject.Find("LifeManager").transform;
		//GameObject.Find("HolderBackToWorlds").GetComponent<Transform>().position=new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x, Camera.main.ViewportToWorldPoint(Vector3.one).y, -2f);
		//GameObject.Find("HolderHeaderLev").GetComponent<Transform>().position=new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one/2).x, Camera.main.ViewportToWorldPoint(Vector3.one).y, -2f);
		//GameObject.Find("HolderLife").GetComponent<Transform>().position=new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one).x - 2.5f, Camera.main.ViewportToWorldPoint(Vector3.one).y - 0.75f, -2f);
		
		Kamera = GameObject.Find("Main Camera");
		guiCamera = GameObject.Find("GUICamera").GetComponent<Camera>();
		holderMajmun = GameObject.Find("HolderMajmun").transform;
		majmun = holderMajmun.GetChild(0).GetChild(0);
		animator = majmun.GetComponent<Animator>();
		guiCameraY = guiCamera.transform.position.y;
		
		//GameObject.Find("HolderBackToWorlds").transform.parent=Camera.main.transform;
		//GameObject.Find("HolderHeaderLev").transform.parent=Camera.main.transform;
		//GameObject.Find("HolderLife").transform.parent=Camera.main.transform;

		levaGranica = doleLevo.position.x+ortSize*aspect;
		desnaGranica = doleDesno.position.x-ortSize*aspect;
		donjaGranica = doleLevo.position.y+ortSize;
		gornjaGranica = goreDesno.position.y-ortSize;

		//InvokeRepeating("UvecavajBrojac",1,0.05f);

		if(StagesParser.otvaraoShopNekad == 2 && StagesParser.trenutniNivoNaOstrvu[StagesParser.currSetIndex] == 3)//StagesParser.currentLevel == 3)
		{
			if(StagesParser.currSetIndex == 0)
			{
				makniPopup = 6;
				StartCoroutine(PokaziMuCustomize());
			}
		}

		InitLevels(false);

		Camera.main.transform.position = new Vector3( Mathf.Clamp(GameObject.Find("Level" + (StagesParser.trenutniNivoNaOstrvu[StagesParser.currSetIndex]).ToString()).transform.position.x, levaGranica,desnaGranica), Mathf.Clamp(GameObject.Find("Level" + (StagesParser.trenutniNivoNaOstrvu[StagesParser.currSetIndex]).ToString()).transform.position.y, donjaGranica,gornjaGranica), Camera.main.transform.position.z);

		if(StagesParser.pozicijaMajmuncetaNaMapi == Vector3.zero) //normalan nivo
		{
			if(/*StagesParser.trenutniNivoNaOstrvu[StagesParser.currSetIndex] > 1 && */StagesParser.SetsInGame[StagesParser.currSetIndex].CurrentStarsInStageNEW > 0)
			{
				holderMajmun.position = izmedjneTacke.Find(StagesParser.trenutniNivoNaOstrvu[StagesParser.currSetIndex].ToString()).position;
				monkeyCurrentLevelIndex = GetMapLevelIndex(StagesParser.trenutniNivoNaOstrvu[StagesParser.currSetIndex]);
			}
		}
		else //bonus nivo
		{

			holderMajmun.position = StagesParser.pozicijaMajmuncetaNaMapi;
			kovcegNaPocetku = trenutniKovceg = pronadjiKovceg(StagesParser.bonusName);
			string[] values = StagesParser.bonusName.Split('_');
			monkeyCurrentLevelIndex = GetMapLevelIndex(int.Parse(values[2]));
			Camera.main.transform.position = new Vector3( Mathf.Clamp(holderMajmun.position.x, levaGranica,desnaGranica), Mathf.Clamp(holderMajmun.position.y, donjaGranica,gornjaGranica), Camera.main.transform.position.z);

			if(!StagesParser.dodatnaProveraIzasaoIzBonusa)
			{
				kovcegNaPocetku.Find("Kovceg Zatvoren").GetComponent<Animator>().Play("Kovceg Otvaranje");
				if(PlaySounds.soundOn)
					PlaySounds.Play_Otvaranje_Kovcega();
				kovcegNaPocetku.GetComponent<Collider>().enabled = false;
				_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/PomocniColliderKodOtvaranjaKovcega").localScale = new Vector3(200,130,1);
				PodesiReward();
			}
			else
			{
				StagesParser.dodatnaProveraIzasaoIzBonusa = false;
			}
		}

		if(PlaySounds.musicOn && !PlaySounds.BackgroundMusic_Menu.isPlaying)
			PlaySounds.Play_BackgroundMusic_Menu();

	}

	void PodesiReward()
	{
		if(StagesParser.bonusID == 4) //cetvrti kovceg na mapi
		{
			reward1Type = 4;//daj mu prvi zakljucan item
			reward2Type = 0;
			reward3Type = 0;
			_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward2").gameObject.SetActive(false);
			_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward3").gameObject.SetActive(false);
			IspitajItem();

		}
		else
		{
			if(StagesParser.currSetIndex == 0) //za 1. svet daj mu jednu nagradu na prva 3 kovcega
			{
				reward2Type = 0;
				reward3Type = 0;
				_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward2").gameObject.SetActive(false);
				_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward3").gameObject.SetActive(false);

				kolicinaReward1 = 1;//Random.Range(StagesParser.currSetIndex+1,StagesParser.currSetIndex+4);
				_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward1/Count").GetComponent<TextMesh>().text = kolicinaReward1.ToString();

				if(StagesParser.powerup_magnets <= StagesParser.powerup_doublecoins)
				{
					if(StagesParser.powerup_magnets <= StagesParser.powerup_shields)
					{
						reward1Type = 1;
						_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward1").GetComponent<SpriteRenderer>().sprite = _GUI.Find("ShopHolder/Shop/4 Power-Ups/Power-Ups Tabovi/Shop POWERUP Magnet/Plavi Bedz/Magnet Icon").GetComponent<SpriteRenderer>().sprite;
					}
					else
					{
						reward1Type = 3;
						_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward1").GetComponent<SpriteRenderer>().sprite = _GUI.Find("ShopHolder/Shop/4 Power-Ups/Power-Ups Tabovi/Shop POWERUP Shield/Plavi Bedz/Shield Icon").GetComponent<SpriteRenderer>().sprite;
					}
				}
				else if(StagesParser.powerup_doublecoins <= StagesParser.powerup_shields)
				{
					reward1Type = 2;
					_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward1").GetComponent<SpriteRenderer>().sprite = _GUI.Find("ShopHolder/Shop/4 Power-Ups/Power-Ups Tabovi/Shop POWERUP Double Coins/Plavi Bedz/Double Coins Icon").GetComponent<SpriteRenderer>().sprite;
				}
				else
				{
					reward1Type = 3;
					_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward1").GetComponent<SpriteRenderer>().sprite = _GUI.Find("ShopHolder/Shop/4 Power-Ups/Power-Ups Tabovi/Shop POWERUP Shield/Plavi Bedz/Shield Icon").GetComponent<SpriteRenderer>().sprite;
				}
			}
			else if(StagesParser.currSetIndex < 3) //za 2. i 3. svet daj mu dve nagrade na prva 3 kovcega
			{
				reward3Type = 0;
				kolicinaReward1 = 1;//Random.Range(StagesParser.currSetIndex+1,StagesParser.currSetIndex+4);
				kolicinaReward2 = 1;//Random.Range(StagesParser.currSetIndex+1,StagesParser.currSetIndex+4);
				_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward1/Count").GetComponent<TextMesh>().text = kolicinaReward1.ToString();
				_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward2/Count").GetComponent<TextMesh>().text = kolicinaReward2.ToString();

				_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward1").localPosition = new Vector3(-0.75f,0.55f,-0.5f);
				_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward2").localPosition = new Vector3(0.75f,0.55f,-0.5f);
				_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward3").gameObject.SetActive(false);

				if(StagesParser.powerup_magnets <= StagesParser.powerup_doublecoins)
				{
					if(StagesParser.powerup_shields <= StagesParser.powerup_doublecoins)
					{
						reward1Type = 1;
						_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward1").GetComponent<SpriteRenderer>().sprite = _GUI.Find("ShopHolder/Shop/4 Power-Ups/Power-Ups Tabovi/Shop POWERUP Magnet/Plavi Bedz/Magnet Icon").GetComponent<SpriteRenderer>().sprite;
						reward2Type = 3;
						_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward2").GetComponent<SpriteRenderer>().sprite = _GUI.Find("ShopHolder/Shop/4 Power-Ups/Power-Ups Tabovi/Shop POWERUP Shield/Plavi Bedz/Shield Icon").GetComponent<SpriteRenderer>().sprite;
					}
					else
					{
						reward1Type = 1;
						_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward1").GetComponent<SpriteRenderer>().sprite = _GUI.Find("ShopHolder/Shop/4 Power-Ups/Power-Ups Tabovi/Shop POWERUP Magnet/Plavi Bedz/Magnet Icon").GetComponent<SpriteRenderer>().sprite;
						reward2Type = 2;
						_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward2").GetComponent<SpriteRenderer>().sprite = _GUI.Find("ShopHolder/Shop/4 Power-Ups/Power-Ups Tabovi/Shop POWERUP Double Coins/Plavi Bedz/Double Coins Icon").GetComponent<SpriteRenderer>().sprite;
					}
				}
				else if(StagesParser.powerup_magnets <= StagesParser.powerup_shields)
				{
					reward1Type = 2;
					_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward1").GetComponent<SpriteRenderer>().sprite = _GUI.Find("ShopHolder/Shop/4 Power-Ups/Power-Ups Tabovi/Shop POWERUP Double Coins/Plavi Bedz/Double Coins Icon").GetComponent<SpriteRenderer>().sprite;
					reward2Type = 1;
					_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward2").GetComponent<SpriteRenderer>().sprite = _GUI.Find("ShopHolder/Shop/4 Power-Ups/Power-Ups Tabovi/Shop POWERUP Magnet/Plavi Bedz/Magnet Icon").GetComponent<SpriteRenderer>().sprite;
				}
				else
				{
					reward1Type = 2;
					_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward1").GetComponent<SpriteRenderer>().sprite = _GUI.Find("ShopHolder/Shop/4 Power-Ups/Power-Ups Tabovi/Shop POWERUP Double Coins/Plavi Bedz/Double Coins Icon").GetComponent<SpriteRenderer>().sprite;
					reward2Type = 3;
					_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward2").GetComponent<SpriteRenderer>().sprite = _GUI.Find("ShopHolder/Shop/4 Power-Ups/Power-Ups Tabovi/Shop POWERUP Shield/Plavi Bedz/Shield Icon").GetComponent<SpriteRenderer>().sprite;
				}
			}
			else //za 4. svet pa navise daj mu 3 nagrade na prva 3 kovcega
			{
				kolicinaReward1 = 1;//Random.Range(StagesParser.currSetIndex+1,StagesParser.currSetIndex+4);
				kolicinaReward2 = 1;//Random.Range(StagesParser.currSetIndex+1,StagesParser.currSetIndex+4);
				kolicinaReward3 = 1;//Random.Range(StagesParser.currSetIndex+1,StagesParser.currSetIndex+4);
				_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward1/Count").GetComponent<TextMesh>().text = kolicinaReward1.ToString();
				_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward2/Count").GetComponent<TextMesh>().text = kolicinaReward2.ToString();
				_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward3/Count").GetComponent<TextMesh>().text = kolicinaReward3.ToString();

				_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward1").GetComponent<SpriteRenderer>().sprite = _GUI.Find("ShopHolder/Shop/4 Power-Ups/Power-Ups Tabovi/Shop POWERUP Magnet/Plavi Bedz/Magnet Icon").GetComponent<SpriteRenderer>().sprite;
				_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward2").GetComponent<SpriteRenderer>().sprite = _GUI.Find("ShopHolder/Shop/4 Power-Ups/Power-Ups Tabovi/Shop POWERUP Shield/Plavi Bedz/Shield Icon").GetComponent<SpriteRenderer>().sprite;
				_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward3").GetComponent<SpriteRenderer>().sprite = _GUI.Find("ShopHolder/Shop/4 Power-Ups/Power-Ups Tabovi/Shop POWERUP Double Coins/Plavi Bedz/Double Coins Icon").GetComponent<SpriteRenderer>().sprite;

				reward1Type = 1;
				reward2Type = 2;
				reward3Type = 3;
			}
		}
		Invoke("PozoviRewardPopup",4.15f);
	}

	void PozoviRewardPopup()
	{
		_GUI.Find("REWARD HOLDER/AnimationHolderGlavni").localPosition += new Vector3(0,35,0);
		_GUI.Find("REWARD HOLDER/AnimationHolderGlavni").GetComponent<Animator>().Play("OpenPopup");
		makniPopup = 7;
	}

	void RefreshScene()
	{
		//@@@@@@@ DODATAK DA PROVERI DA SLUCAJNO LOGOVAN KORISNIK IMA MANJE OTKLJUCANIH OSTRVA NEGO NELOGOVAN KORISNIK - I deo
		StagesParser.lastUnlockedWorldIndex = 0;
		for(int i=0;i<StagesParser.totalSets;i++)
		{
			StagesParser.unlockedWorlds[i] = false;
			if(StagesParser.currentStarsNEW >= StagesParser.SetsInGame[i].StarRequirement) //uslov za zvezdice je ok za i-to ostrvo
			{
				if(i>0)
				{
					string[] values = StagesParser.allLevels[(i-1)*20+19].Split('#');
					if(int.Parse(values[1]) > 0) //na prethodnom ostrvu je presao 20. nivo
					{
						StagesParser.unlockedWorlds[i] = true;
						StagesParser.lastUnlockedWorldIndex = i; //zadnje otkljucano ostrvo je i-to
					}
				}
			}
		}
		StagesParser.unlockedWorlds[0] = true;

		if(StagesParser.lastUnlockedWorldIndex < (Application.loadedLevel-4)) //@@@@@@@ DODATAK DA PROVERI DA SLUCAJNO LOGOVAN KORISNIK IMA MANJE OTKLJUCANIH OSTRVA NEGO NELOGOVAN KORISNIK - II deo
		{
			if(StagesParser.pozicijaMajmuncetaNaMapi != Vector3.zero)
				StagesParser.pozicijaMajmuncetaNaMapi = Vector3.zero;
			StagesParser.worldToFocus = StagesParser.currSetIndex;
			if(PlaySounds.soundOn)
				PlaySounds.Play_Button_GoBack();
			StagesParser.vratioSeNaSvaOstrva = true;
			StartCoroutine(UcitajOstrvo("All Maps"));
		}
		else
		{
			InitLevels(true);

			if(StagesParser.pozicijaMajmuncetaNaMapi == Vector3.zero) //normalan nivo
			{
				if(/*StagesParser.trenutniNivoNaOstrvu[StagesParser.currSetIndex] > 1 && */StagesParser.SetsInGame[StagesParser.currSetIndex].CurrentStarsInStageNEW > 0)
				{
					holderMajmun.position = izmedjneTacke.Find(StagesParser.trenutniNivoNaOstrvu[StagesParser.currSetIndex].ToString()).position;
					monkeyCurrentLevelIndex = GetMapLevelIndex(StagesParser.trenutniNivoNaOstrvu[StagesParser.currSetIndex]);
				}
				else //nije predjen nijedan nivo na ostrvu, da ga postavi na 1. nivo
				{
					holderMajmun.position = izmedjneTacke.Find("1").position;
					monkeyCurrentLevelIndex = GetMapLevelIndex(1);
				}
			}
			else //bonus nivo
			{
				
				holderMajmun.position = StagesParser.pozicijaMajmuncetaNaMapi;
				kovcegNaPocetku = trenutniKovceg = pronadjiKovceg(StagesParser.bonusName);
				string[] values = StagesParser.bonusName.Split('_');
				monkeyCurrentLevelIndex = GetMapLevelIndex(int.Parse(values[2]));
				Camera.main.transform.position = new Vector3( Mathf.Clamp(holderMajmun.position.x, levaGranica,desnaGranica), Mathf.Clamp(holderMajmun.position.y, donjaGranica,gornjaGranica), Camera.main.transform.position.z);
				
				if(!StagesParser.dodatnaProveraIzasaoIzBonusa)
				{
					kovcegNaPocetku.Find("Kovceg Zatvoren").GetComponent<Animator>().Play("Kovceg Otvaranje");
					if(PlaySounds.soundOn)
						PlaySounds.Play_Otvaranje_Kovcega();
					kovcegNaPocetku.GetComponent<Collider>().enabled = false;
					_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/PomocniColliderKodOtvaranjaKovcega").localScale = new Vector3(200,130,1);
					PodesiReward();
				}
				else
				{
					StagesParser.dodatnaProveraIzasaoIzBonusa = false;
				}
			}

			_GUI.Find("INTERFACE HOLDER/_TopLeft/Coins/Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
			_GUI.Find("INTERFACE HOLDER/_TopLeft/Coins/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			_GUI.Find("INTERFACE HOLDER/_TopLeft/PTS/PTS Number").GetComponent<TextMesh>().text = StagesParser.currentPoints.ToString();
			_GUI.Find("INTERFACE HOLDER/_TopLeft/PTS/PTS Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			_GUI.Find("INTERFACE HOLDER/_TopLeft/BananaHolder/Bananas/Banana Number").GetComponent<TextMesh>().text = StagesParser.currentBananas.ToString();
			_GUI.Find("INTERFACE HOLDER/_TopLeft/BananaHolder/Bananas/Banana Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			_GUI.Find("INTERFACE HOLDER/FB Login/Text/Number").GetComponent<TextMesh>().text = "+"+StagesParser.LoginReward.ToString();
			_GUI.Find("INTERFACE HOLDER/FB Login/Text/Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			_GUI.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup/WatchVideoReward").GetComponent<TextMesh>().text = "+"+televizorCenePoSvetovima[StagesParser.currSetIndex].ToString();
			_GUI.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup/WatchVideoReward").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			if(reward1Type > 0)
				_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward1/Count").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			if(reward2Type > 0)
				_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward2/Count").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			if(reward3Type > 0)
				_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward3/Count").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			_GUI.Find("INTERFACE HOLDER/TotalStars/Stars Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			changeLanguage();
			CheckInternetConnection.Instance.refreshText();
			StagesParser.LoadingPoruke.Clear();
			StagesParser.RedniBrojSlike.Clear();
			StagesParser.Instance.UcitajLoadingPoruke();
		}
	}

	void Start ()
	{
		changeLanguage();

		if(Loading.Instance != null)
			StartCoroutine(Loading.Instance.UcitanaScena(guiCamera,2,0));
		else _GUI.Find("LOADING HOLDER NEW/Loading Animation Vrata").GetComponent<Animator>().Play("Loading Zidovi Odlazak");

		_GUI.Find("INTERFACE HOLDER/_TopLeft/Coins/Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
		_GUI.Find("INTERFACE HOLDER/_TopLeft/Coins/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		_GUI.Find("INTERFACE HOLDER/_TopLeft/PTS/PTS Number").GetComponent<TextMesh>().text = StagesParser.currentPoints.ToString();
		_GUI.Find("INTERFACE HOLDER/_TopLeft/PTS/PTS Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		_GUI.Find("INTERFACE HOLDER/_TopLeft/BananaHolder/Bananas/Banana Number").GetComponent<TextMesh>().text = StagesParser.currentBananas.ToString();
		_GUI.Find("INTERFACE HOLDER/_TopLeft/BananaHolder/Bananas/Banana Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		_GUI.Find("INTERFACE HOLDER/FB Login/Text/Number").GetComponent<TextMesh>().text = "+"+StagesParser.LoginReward.ToString();
		_GUI.Find("INTERFACE HOLDER/FB Login/Text/Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		_GUI.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup/WatchVideoReward").GetComponent<TextMesh>().text = "+"+televizorCenePoSvetovima[StagesParser.currSetIndex].ToString();
		_GUI.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup/WatchVideoReward").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		_GUI.Find("INTERFACE HOLDER/_TopLeft").position = new Vector3(guiCamera.ViewportToWorldPoint(Vector3.zero).x,_GUI.Find("INTERFACE HOLDER/_TopLeft").position.y,_GUI.Find("INTERFACE HOLDER/_TopLeft").position.z);
		_GUI.Find("INTERFACE HOLDER/FB Login").position = new Vector3(guiCamera.ViewportToWorldPoint(new Vector3(0.91f,0,0)).x,_GUI.Find("INTERFACE HOLDER/FB Login").position.y,_GUI.Find("INTERFACE HOLDER/FB Login").position.z);
		_GUI.Find("INTERFACE HOLDER/TotalStars").position = new Vector3(guiCamera.ViewportToWorldPoint(new Vector3(0.89f,0,0)).x,_GUI.Find("INTERFACE HOLDER/TotalStars").position.y,_GUI.Find("INTERFACE HOLDER/TotalStars").position.z);
		_GUI.Find("INTERFACE HOLDER/TotalStars/Stars Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);

		//ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCBILikePage/Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text = StagesParser.likePageReward.ToString();
		ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCWatchVideo/Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text = StagesParser.watchVideoReward.ToString();
		//ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCWLLikePage/Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text = StagesParser.likePageReward.ToString();
		//ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCBILikePage/Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,false);
		ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCWatchVideo/Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,false);
		//ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCWLLikePage/Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,false);

		if(reward1Type > 0)
			_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward1/Count").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		if(reward2Type > 0)
			_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward2/Count").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		if(reward3Type > 0)
			_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward3/Count").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);

		GameObject.Find("NotAvailableText").SetActive(false);

		if(FB.IsLoggedIn)
		{
			GameObject.Find("FB Login").SetActive(false);
		}

		if(StagesParser.obucenSeLogovaoNaDrugojSceni)
		{
			StagesParser.obucenSeLogovaoNaDrugojSceni = false;
			Invoke("MaliDelayPreNegoDaSePozoveCompareScoresShopDeo",1f);
		}
	}

	void MaliDelayPreNegoDaSePozoveCompareScoresShopDeo()
	{
		StagesParser.Instance.ShopDeoIzCompareScores();
	}

	void UvecavajBrojac()
	{
		if(angleIndex > 7)
			angleIndex = 0;
		angleIndex++;
	}

	void InitLevels(bool refreshed)
	{
		string[] values;
		StagesParser.SetsInGame[StagesParser.currSetIndex].CurrentStarsInStageNEW = 0;
		if(StagesParser.zadnjiOtkljucanNivo != 0)
		{
			GameObject.Find("LevelsWorld" + StagesParser.currSetIndex + "/Level" + StagesParser.zadnjiOtkljucanNivo + "/Level" + StagesParser.zadnjiOtkljucanNivo + "Move").GetComponent<Animator>().Play("KatanacExplosion");

			if(PlaySounds.soundOn)
				PlaySounds.Play_OtkljucavanjeNivoa();
		}
		for(int i=0; i<StagesParser.SetsInGame[StagesParser.currSetIndex].StagesOnSet; i++)
		{
			//StagesParser.currentLevel = StagesParser.SetsInGame[StagesParser.currSetIndex]*10 + ;

			values = StagesParser.allLevels[StagesParser.currSetIndex*20+i].Split('#');
			StagesParser.SetsInGame[StagesParser.currSetIndex].SetStarOnStage(i,int.Parse(values[1]));
			if(int.Parse(values[1]) > -1)
				StagesParser.SetsInGame[StagesParser.currSetIndex].CurrentStarsInStageNEW += int.Parse(values[1]);

//			if(PlayerPrefs.HasKey("Level"+(StagesParser.currSetIndex*20+i+1)))
//			{
//				string level = PlayerPrefs.GetString("Level"+(StagesParser.currSetIndex*20+i+1));
//				values = level.Split('#');
//				StagesParser.SetsInGame[StagesParser.currSetIndex].SetStarOnStage(i,int.Parse(values[1]));
//				StagesParser.SetsInGame[StagesParser.currSetIndex].CurrentStarsInStageNEW += int.Parse(values[1]);
//			}
//			else
//			{
//				if(i==0)
//					StagesParser.SetsInGame[StagesParser.currSetIndex].SetStarOnStage(i,0);
//				else
//					StagesParser.SetsInGame[StagesParser.currSetIndex].SetStarOnStage(i,-1);
//			}


			//currentLevelStars = StagesParser.SetsInGame[StagesParser.currSetIndex].GetStarOnStage(i);
			currentLevelStars = int.Parse(values[1]);

			if(currentLevelStars == -1)
			{//locked
				GameObject.Find("LevelsWorld" + StagesParser.currSetIndex + "/Level" + (i+1) + "/Level" + (i+1) + "Move").GetComponent<SpriteRenderer>().sprite = GameObject.Find("RefCardClose").GetComponent<SpriteRenderer>().sprite;
				GameObject.Find("LevelsWorld" + StagesParser.currSetIndex + "/Level" + (i+1) + "/Level" + (i+1) + "Move").transform.Find("TextNumberLevel").GetComponent<TextMesh>().text = System.String.Empty;
				GameObject.Find("LevelsWorld" + StagesParser.currSetIndex + "/Level" + (i+1) + "/Level" + (i+1) + "Move").transform.Find("TextNumberLevel").GetChild(0).GetComponent<TextMesh>().text = System.String.Empty;
				GameObject.Find("LevelsWorld" + StagesParser.currSetIndex + "/Level" + (i+1) + "/Level" + (i+1) + "Move").transform.Find("Katanac").GetComponent<Renderer>().enabled = true;
				if(!GameObject.Find("LevelsWorld" + StagesParser.currSetIndex + "/Level" + (i+1) + "/Level" + (i+1) + "Move").transform.Find("Katanac").gameObject.activeSelf)
				{
					GameObject.Find("LevelsWorld" + StagesParser.currSetIndex + "/Level" + (i+1) + "/Level" + (i+1) + "Move").GetComponent<Animator>().enabled = false;
					GameObject.Find("LevelsWorld" + StagesParser.currSetIndex + "/Level" + (i+1) + "/Level" + (i+1) + "Move").transform.Find("Katanac").gameObject.SetActive(true);
				}
			}
			else
			{//unlocked
				//Debug.Log(GameObject.Find("LevelsWorld" + StagesParser.currSetIndex + "/Level" + (i+1) + "/Level" + (i+1) + "Move").name);

				if((i+1) != StagesParser.zadnjiOtkljucanNivo || StagesParser.pozicijaMajmuncetaNaMapi != Vector3.zero)
				{
					GameObject.Find("LevelsWorld" + StagesParser.currSetIndex + "/Level" + (i+1) + "/Level" + (i+1) + "Move").GetComponent<SpriteRenderer>().sprite = GameObject.Find("RefCardStar"+currentLevelStars).GetComponent<SpriteRenderer>().sprite;
					GameObject.Find("LevelsWorld" + StagesParser.currSetIndex + "/Level" + (i+1) + "/Level" + (i+1) + "Move").transform.Find("TextNumberLevel").GetComponent<TextMesh>().text = (i+1).ToString();
					GameObject.Find("LevelsWorld" + StagesParser.currSetIndex + "/Level" + (i+1) + "/Level" + (i+1) + "Move").transform.Find("Katanac").GetComponent<Renderer>().enabled = false;
					if(currentLevelStars == 0)
						GameObject.Find("LevelsWorld" + StagesParser.currSetIndex + "/Level" + (i+1) + "/Level" + (i+1) + "Move").GetComponent<Animator>().Play("NewLevelLoop");

				}
				zadnjiOtkljucanNivo_proveraZaBonus = i+1;
			}
		}
		_GUI.Find("INTERFACE HOLDER/TotalStars/Stars Number").GetComponent<TextMesh>().text = StagesParser.SetsInGame[StagesParser.currSetIndex].CurrentStarsInStageNEW + "/" + StagesParser.SetsInGame[StagesParser.currSetIndex].StagesOnSet*3;


		///////////////////////////////////////////////////////////////
		//Bonus Level cuvanje
		//Format: 		1#0#-1#-1_1#1#0#-1_1#0#1#-1_1#0#0#-1_0#-1#-1#-1
		//Separator: 	za ostrva "_", za kovcege "#"
		//Legenda: 		1 : predjen bonus, 0 : otkljucan ali nije igran, -1 : zakljucan

		string[] BonusValues = StagesParser.bonusLevels.Split('_');
		string kovcezi = BonusValues[StagesParser.currSetIndex];
		string[] kovceziValues = kovcezi.Split('#');

		//kovceziValues[0] = "0"; //@@@@@@@@@@@@ZA BRISANJE OBAVEZNO

		for(int i=0;i<BonusNivoi.Length;i++)
		{
			if(int.Parse(kovceziValues[i]) > -1)
			{
				BonusNivoi[i].Find("GateClosed").GetComponent<Renderer>().enabled = false;
				BonusNivoi[i].Find("GateOpen").GetComponent<Renderer>().enabled = true;
				if(int.Parse(kovceziValues[i]) > 0)
				{
					Transform temp = BonusNivoi[i].Find("Kovceg Zatvoren");
					temp.Find("Kovceg Otvoren").GetComponent<Renderer>().enabled = true;
					temp.Find("Kovceg Zatvoren").GetComponent<Renderer>().enabled = false;
					temp.GetComponent<Animator>().Play("Kovceg  Otvoren Idle");
					temp.parent.GetComponent<Collider>().enabled = false;
				}
			}

//			if(PlayerPrefs.HasKey("BonusLevel#"+StagesParser.currentWorld+"#"+(i+1)))
//			{
//				BonusNivoi[i].Find("GateClosed").renderer.enabled = false;
//				BonusNivoi[i].Find("GateOpen").renderer.enabled = true;
//				if(PlayerPrefs.GetInt("BonusLevel#"+StagesParser.currentWorld+"#"+(i+1)) == 1)
//				{
//					Transform temp = BonusNivoi[i].Find("Kovceg Zatvoren");
//					temp.Find("Kovceg Otvoren").renderer.enabled = true;
//					temp.Find("Kovceg Zatvoren").renderer.enabled = false;
//					temp.GetComponent<Animator>().Play("Kovceg  Otvoren Idle");
//					temp.parent.collider.enabled = false;
//				}
//			}
		}
		StagesParser.zadnjiOtkljucanNivo = 0;
	}

	void Update() {

		if(Input.GetKeyUp(KeyCode.Escape))
		{
			//GameObject.Find("HolderLife").transform.parent = lifeManager.GetChild(0);
			if(makniPopup == 1)
			{
				if(PlaySounds.soundOn)
					PlaySounds.Play_Button_GoBack();
				popupZaSpustanje = _GUI.Find("MISSION HOLDER/AnimationHolderGlavni");
				Invoke("spustiPopup",0.5f);
				_GUI.Find("MISSION HOLDER/AnimationHolderGlavni").GetComponent<Animator>().Play("ClosePopup");
				makniPopup = 0;
				ocistiMisije();
				prejasiTelevizor = false;
			}
			else if(makniPopup == 2)
			{
				if(PlaySounds.soundOn)
					PlaySounds.Play_Button_GoBack();
				popupZaSpustanje = _GUI.Find("UNLOCK HOLDER/AnimationHolderGlavni");
				Invoke("spustiPopup",0.5f);
				_GUI.Find("UNLOCK HOLDER/AnimationHolderGlavni").GetComponent<Animator>().Play("ClosePopup");
				makniPopup = 0;
			}
			else if(makniPopup == 3)
			{
				if(PlaySounds.soundOn)
					PlaySounds.Play_Button_GoBack();
				//zatvaranje bonus reward popup-a
			}
			else if(makniPopup == 4)
			{
				if(PlaySounds.soundOn)
					PlaySounds.Play_Button_GoBack();
				popupZaSpustanje = _GUI.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni");
				Invoke("spustiPopup",0.5f);
				_GUI.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni").GetComponent<Animator>().Play("ClosePopup");
				makniPopup = 0;
			}
			else if(makniPopup == 5)
			{
				if(PlaySounds.soundOn)
					PlaySounds.Play_Button_GoBack();
				_GUI.Find("INTERFACE HOLDER/_TopLeft/Coins/Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
				_GUI.Find("INTERFACE HOLDER/_TopLeft/Coins/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
				_GUI.Find("INTERFACE HOLDER/_TopLeft/BananaHolder/Bananas/Banana Number").GetComponent<TextMesh>().text = StagesParser.currentBananas.ToString();
				_GUI.Find("INTERFACE HOLDER/_TopLeft/BananaHolder/Bananas/Banana Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
				ShopManagerFull.ShopObject.SkloniShop();
				makniPopup = 0;
			}
			else if(makniPopup == 7)
			{
				DodeliNagrade();
				if(PlaySounds.soundOn)
					PlaySounds.Play_Button_GoBack();
				popupZaSpustanje = _GUI.Find("REWARD HOLDER/AnimationHolderGlavni");
				Invoke("spustiPopup",0.5f);
				_GUI.Find("REWARD HOLDER/AnimationHolderGlavni").GetComponent<Animator>().Play("ClosePopup");
				_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/PomocniColliderKodOtvaranjaKovcega").localScale = Vector3.zero;
				makniPopup = 0;
			}
			else if(makniPopup == 0)
			{
				if(StagesParser.pozicijaMajmuncetaNaMapi != Vector3.zero)
					StagesParser.pozicijaMajmuncetaNaMapi = Vector3.zero;
				StagesParser.worldToFocus = StagesParser.currSetIndex;
				if(PlaySounds.soundOn)
					PlaySounds.Play_Button_GoBack();
				StagesParser.vratioSeNaSvaOstrva = true;
				StartCoroutine(UcitajOstrvo("All Maps"));
			}
			else if(makniPopup == 8)
			{
				makniPopup = 0;
				StartCoroutine(CheckInternetConnection.Instance.ClosePopup());
			}
			else if(makniPopup == 9)
			{
				makniPopup = 4;
				StartCoroutine(CheckInternetConnection.Instance.ClosePopup());
			}

		}

		//AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		//AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		//AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		//AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		//AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
		//Debug.Log("ugao: " + majmun.localRotation + ", a UGAO: " + majmun.eulerAngles);
		if(angleIndex >=0 && angleIndex <8)
		{
			//Quaternion a = Quaternion.Euler(angles[angleIndex]);
			//majmun.rotation = Quaternion.Lerp(majmun.rotation,a,0.2f);
		}
			//majmun.eulerAngles = Vector3.RotateTowards(majmun.eulerAngles,angles[angleIndex],10.2f,0f);
			//majmun.eulerAngles = angles[angleIndex];

		if(Input.touchCount == 2)
		{
			zoom = true;
		}

		if(Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved && makniPopup == 0)
		//if(Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.X))
		{
			zoom = true;
			pomeriKameruUGranice = false;
			curDist = Input.GetTouch(0).position - Input.GetTouch(1).position;
			prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition));
			touchDelta = curDist.magnitude - prevDist.magnitude;
			float actualDistance = 0.07f*(prevDist.magnitude - curDist.magnitude);
			speedTouch0 = Input.GetTouch(0).deltaPosition.magnitude / Input.GetTouch(0).deltaTime;
			speedTouch1 = Input.GetTouch(1).deltaPosition.magnitude / Input.GetTouch(1).deltaTime;

			//float actualDistance = 0.5f;
			if ((touchDelta - varianceInDistances <= -10) && ((speedTouch0 > minPinchSpeed) || (speedTouch1 > minPinchSpeed)))
			//if(Input.GetKey(KeyCode.Z))
			{
				Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize,11,actualDistance/2);
			}
			if ((touchDelta +varianceInDistances > 10) && ((speedTouch0 > minPinchSpeed) || (speedTouch1 > minPinchSpeed)))
			//if(Input.GetKey(KeyCode.X))
			{
				Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize,4,-actualDistance/2); //za touch ovde je -actualDistance/2xzx
			}
			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize,4,11);
		}

		if(Input.touchCount == 0 && zoom)
		//if(Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.X) && zoom)
		{
			// da li je kamera izasla iz granica dole levo
			ivicaEkrana = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0));
			if(doleLevo.position.x > ivicaEkrana.x)// && doleLevo.position.y > ivicaEkrana.y && !pomeriKameruUGranice)
			{
				pomeriKameruUGranice = true;
				Vector3 rez = new Vector3(doleLevo.position.x - ivicaEkrana.x,0,0);
				if(doleLevo.position.y > ivicaEkrana.y)
					rez = doleLevo.position - ivicaEkrana;
				ivicaEkrana = Camera.main.ViewportToWorldPoint(new Vector3(0,1,0));
				if(goreLevo.position.y < ivicaEkrana.y)
					rez = goreLevo.position - ivicaEkrana;
				StartCoroutine(PostaviKameruUGranice(rez));
			}
			ivicaEkrana = Camera.main.ViewportToWorldPoint(new Vector3(1,0,0));
			if(doleDesno.position.x < ivicaEkrana.x)// && doleLevo.position.y > ivicaEkrana.y && !pomeriKameruUGranice)
			{
				pomeriKameruUGranice = true;
				Vector3 rez = new Vector3(doleDesno.position.x - ivicaEkrana.x,0,0);
				if(doleDesno.position.y > ivicaEkrana.y)
					rez = doleDesno.position - ivicaEkrana;
				ivicaEkrana = Camera.main.ViewportToWorldPoint(new Vector3(1,1,0));
				if(goreDesno.position.y < ivicaEkrana.y)
					rez = goreDesno.position - ivicaEkrana;
				StartCoroutine(PostaviKameruUGranice(rez));
			}
			zoom = false;
			ortSize = Camera.main.orthographicSize;
			aspect = Camera.main.aspect;
			levaGranica = doleLevo.position.x+ortSize*aspect;
			desnaGranica = doleDesno.position.x-ortSize*aspect;
			donjaGranica = doleLevo.position.y+ortSize;
			gornjaGranica = goreDesno.position.y-ortSize;
		}

		if(!zoom)
		{
			if(Input.GetMouseButtonDown(0))
			{
				pomeriKameruUGranice = false;
				if(released)
					released = false;

				clickedItem = RaycastFunction(Input.mousePosition);
				trajanjeKlika = Time.time;
				pomerajOdKlika_X  = startX = Input.mousePosition.x;
				pomerajOdKlika_Y  = startY = Input.mousePosition.y;
				//startX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
				//startY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
				if(clickedItem.Equals("ClearAll"))
				{
					ShopManagerFull.ShopObject.transform.Find("3 Customize/Costumization BG/ClearAll/ClearAll_Selected").GetComponent<SpriteRenderer>().enabled = true;
				}
			}

			if(Input.GetMouseButton(0) && makniPopup == 0)
			{
				endX = Input.mousePosition.x;//Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
				endY = Input.mousePosition.y;//Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
				//Debug.Log("startX: " + startX + ", endX: " + endX + ", startY: " + startY + ", endY: " + endY);
			 	pomerajX = Camera.main.orthographicSize*(endX - startX)/350;
				pomerajY = Camera.main.orthographicSize*(endY - startY)/350;

				if(pomerajX != 0 || pomerajY != 0)
					moved = true;

				Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x - pomerajX, levaGranica, desnaGranica), Mathf.Clamp(Camera.main.transform.position.y - pomerajY, donjaGranica, gornjaGranica), Camera.main.transform.position.z);

				startX = endX;
				startY = endY;
				//Debug.Log("pomerajX: " + pomerajX + ", pomerajY: " + pomerajY);
			}

			if(released && Mathf.Abs(pomerajX)>0.0001f)
			{
				if(Camera.main.transform.position.x <= levaGranica + 0.25f)
				{
					if(bounce)
					{	
						pomerajX = -0.04f;
						bounce = false;
					}
					//Debug.Log("Da ne prebaci levo");
				}
				else if(Camera.main.transform.position.x >= desnaGranica - 0.25f)
				{
					if(bounce)
					{	
						pomerajX = 0.04f;
						bounce = false;
					}
					//Debug.Log("Da ne prebaci desno");
				}

				else if(Camera.main.transform.position.y <= donjaGranica + 0.25f)
				{
					if(bounce)
					{
						pomerajY = -0.04f;
						bounce = false;
					}
					//Debug.Log("Da ne prebaci dole");
				}
				else if(Camera.main.transform.position.y >= gornjaGranica - 0.25f)
				{
					if(bounce)
					{
						pomerajY = 0.04f;
						bounce = false;
					}
					//Debug.Log("Da ne prebaci gore");
				}
				
				//if(Camera.main.transform.position.x > 8.87f + 1.2f && Camera.main.transform.position.x < 31.95f - 1.2f && Camera.main.transform.position.y > -15.35f + 0.6f && Camera.main.transform.position.y < -5.2f - 0.6f)
				{
					Camera.main.transform.Translate(-pomerajX,-pomerajY,0);
					pomerajX *= 0.92f;
					pomerajY *= 0.92f;
					Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x,levaGranica,desnaGranica),Mathf.Clamp(Camera.main.transform.position.y,donjaGranica,gornjaGranica),Camera.main.transform.position.z);
				}
			}
		}
		if(Input.GetMouseButtonUp(0))
		{
			if(moved)
			{
				moved = false;
				released = true;
				bounce = true;
			}

			releasedItem = RaycastFunction(Input.mousePosition);
			startX = endX = 0;
			startY = endY = 0;

			if(ShopManagerFull.ShopObject.transform.Find("3 Customize/Costumization BG/ClearAll/ClearAll_Selected").GetComponent<SpriteRenderer>().enabled)
				ShopManagerFull.ShopObject.transform.Find("3 Customize/Costumization BG/ClearAll/ClearAll_Selected").GetComponent<SpriteRenderer>().enabled = false;

			if(clickedItem == releasedItem && (Time.time - trajanjeKlika < 0.35f) && (Mathf.Abs(Input.mousePosition.x - pomerajOdKlika_X) < 80 && Mathf.Abs(Input.mousePosition.y - pomerajOdKlika_Y) < 80))
			{
				if(releasedItem.StartsWith("Level"))
				{
					if(int.TryParse(releasedItem.Substring(5), out levelName))
					{
						//Debug.Log(levelName + ", " + StagesParser.SetsInGame[StagesParser.currSetIndex].IsLvlUnlocked(levelName-1));
						//if(!StagesParser.SetsInGame[StagesParser.currSetIndex].IsLvlUnlocked(levelName-1))
						if(StagesParser.StarsPoNivoima[StagesParser.currSetIndex*20 + levelName - 1] == -1)
						{
							//GameObject.Find(releasedItem).transform.GetChild(0).animation.Play("LevelsLockCard");
							;
						}
						else
						{
							if(PlaySounds.soundOn)
								PlaySounds.Play_Button_OpenLevel();
							monkeyDestinationLevelIndex = GetMapLevelIndex(levelName);
							if(majmunceSeMrda)
								StopCoroutine("KretanjeMajmunceta");
							if((monkeyCurrentLevelIndex != monkeyDestinationLevelIndex || StagesParser.pozicijaMajmuncetaNaMapi != Vector3.zero) && makniPopup == 0)
							{
								if(kretanjeDoKovcega)
									kretanjeDoKovcega = false;

								animator.Play("Running");
								StartCoroutine("KretanjeMajmunceta");

							}
							else if(makniPopup == 0)
							{
								StagesParser.currStageIndex = levelName-1;
								StagesParser.currentLevel = StagesParser.currSetIndex*20 + levelName;
								//StartCoroutine(PlayAndWaitForAnimation(GameObject.Find(releasedItem).transform.GetChild(0).animation,"LevelsClickCard", true, 2));
								StagesParser.nivoZaUcitavanje = 10 + StagesParser.currSetIndex;
								//Application.LoadLevel(2);
								MissionManager.OdrediMisiju(StagesParser.currentLevel-1,true);
								//Transform temp = _GUI.Find("MISSION HOLDER");
								//temp.position = new Vector3(temp.position.x,guiCameraY,temp.position.z);
								if(!FB.IsLoggedIn)
								{
									if(_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN").gameObject.activeSelf)
										_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN").gameObject.SetActive(false);
								}
								else
								{
									if(!_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN").gameObject.activeSelf)
										_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN").gameObject.SetActive(true);
									getFriendsScoresOnLevel(StagesParser.currentLevel);
								}
								_GUI.Find("MISSION HOLDER/AnimationHolderGlavni").localPosition += new Vector3(0,35,0);
								_GUI.Find("MISSION HOLDER/AnimationHolderGlavni").GetComponent<Animator>().Play("OpenPopup");
								makniPopup = 1;
							}

						}
					}

				}


				else if(releasedItem == "HouseShop")
				{

						//Debug.Log("HouseShop");
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_GoBack();
					StartCoroutine(ShopManager.OpenShopCard());
				}
				else if(releasedItem == "HolderShipFreeCoins")
				{
						//Debug.Log("HolderShipFreeCoins");	
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_GoBack();
					StartCoroutine(ShopManager.OpenFreeCoinsCard());
				}
				else if(releasedItem == "ButtonBackToWorlds")
				{
					//Debug.Log("ButtonBackToWorlds");
					if(StagesParser.pozicijaMajmuncetaNaMapi != Vector3.zero)
						StagesParser.pozicijaMajmuncetaNaMapi = Vector3.zero;
					StagesParser.worldToFocus = StagesParser.currSetIndex;
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_GoBack();
					StagesParser.vratioSeNaSvaOstrva = true;
					StartCoroutine(UcitajOstrvo("All Maps"));
				}

				else if(releasedItem.StartsWith("Kovceg"))
				{
					StagesParser.bonusName = releasedItem;
					string[] values = StagesParser.bonusName.Split('_');
					int brKovcega = int.Parse(values[1]);
					StagesParser.bonusID = brKovcega;
					monkeyDestinationLevelIndex = GetMapLevelIndex(int.Parse(values[2]));
					_GUI.Find("UNLOCK HOLDER/AnimationHolderGlavni/AnimationHolder/Unlock Bonus Level Popup/Text/Number of Bananas").GetComponent<TextMesh>().text = values[3];
					_GUI.Find("UNLOCK HOLDER/AnimationHolderGlavni/AnimationHolder/Unlock Bonus Level Popup/Text/Number of Bananas").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);

					if(int.Parse(values[2]) < zadnjiOtkljucanNivo_proveraZaBonus && makniPopup == 0)
					{
						if(pronadjiKovceg(StagesParser.bonusName).Find("GateClosed").GetComponent<Renderer>().enabled)
						{
							//Transform temp = _GUI.Find("UNLOCK HOLDER");
							//temp.position = new Vector3(temp.position.x,guiCameraY,temp.position.z);
							_GUI.Find("UNLOCK HOLDER/AnimationHolderGlavni").localPosition += new Vector3(0,35,0);
							_GUI.Find("UNLOCK HOLDER/AnimationHolderGlavni").GetComponent<Animator>().Play("OpenPopup");
							makniPopup = 2;
						}
						else
						{
							if(PlaySounds.soundOn)
								PlaySounds.Play_Button_GoBack();
							kretanjeDoKovcega = true;

//							string[] BonusValues = PlayerPrefs.GetString("BonusLevel").Split('_');
//							string kovcezi = BonusValues[StagesParser.currSetIndex];
//							string[] kovceziValues = kovcezi.Split('#');
//							Debug.Log("kovcezi za trenutni svet pre updatea: " + kovcezi);
//							kovceziValues[StagesParser.bonusID-1] = "0";
//
//							string pom = System.String.Empty;
//							kovcezi = System.String.Empty;
//							for(int i=0;i<kovceziValues.Length;i++)
//							{
//								kovcezi+=kovceziValues[i] + "#";
//							}
//							kovcezi = kovcezi.Remove(kovcezi.Length-1);
//							BonusValues[StagesParser.currSetIndex] = kovcezi;
//							Debug.Log("kovcezi za trenutni svet posle updatea: " + kovcezi);
//
//							for(int i=0;i<StagesParser.totalSets;i++)
//							{
//								pom += BonusValues[i] + "_"; 
//							}
//							pom = pom.Remove(pom.Length-1);
//							PlayerPrefs.SetString("BonusLevel",pom);
//							PlayerPrefs.Save();
//							Debug.Log("Bonus values konacan: " + pom);
//
//							"OVO DA SE PREMESTI DOLE!!!!"

							//PlayerPrefs.SetInt("BonusLevel#"+StagesParser.currentWorld+"#"+StagesParser.bonusID,0); //ispravlja se trenutno
							//PlayerPrefs.Save();																	  //ispravlja se trenutno
							
							if(majmunceSeMrda)
								StopCoroutine("KretanjeMajmunceta");
							
							if(StagesParser.pozicijaMajmuncetaNaMapi == Vector3.zero)
							{
								kovcegNaPocetku = trenutniKovceg = pronadjiKovceg(StagesParser.bonusName);
							}
							else
							{
								trenutniKovceg = pronadjiKovceg(StagesParser.bonusName);
							}
							trenutniKovceg.Find("GateClosed").GetComponent<Renderer>().enabled = false;
							trenutniKovceg.Find("GateOpen").GetComponent<Renderer>().enabled = true;
							animator.Play("Running");
							StartCoroutine("KretanjeMajmunceta");
						}
					}
					else
					{
						//Locked
					}
				}
					
				else if(releasedItem.Equals("Button_UnlockBonusYES"))
				{
					popupZaSpustanje = _GUI.Find("UNLOCK HOLDER/AnimationHolderGlavni");
					Invoke("spustiPopup",0.5f);
					_GUI.Find("UNLOCK HOLDER/AnimationHolderGlavni").GetComponent<Animator>().Play("ClosePopup");
					makniPopup = 0;

					string[] values = StagesParser.bonusName.Split('_');
					int brKovcega = int.Parse(values[1]);
					StagesParser.bonusID = brKovcega;
					monkeyDestinationLevelIndex = GetMapLevelIndex(int.Parse(values[2]));

					if(int.Parse(values[2]) < zadnjiOtkljucanNivo_proveraZaBonus)
					{
						if(int.Parse(values[3]) <= StagesParser.currentBananas)
						{
							if(PlaySounds.soundOn)
								PlaySounds.Play_Button_GoBack();
							kretanjeDoKovcega = true;

							StagesParser.currentBananas-=int.Parse(values[3]);
							_GUI.Find("INTERFACE HOLDER/_TopLeft/BananaHolder/Bananas/Banana Number").GetComponent<TextMesh>().text = StagesParser.currentBananas.ToString();
							_GUI.Find("INTERFACE HOLDER/_TopLeft/BananaHolder/Bananas/Banana Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);


							string[] BonusValues = StagesParser.bonusLevels.Split('_');
							string kovcezi = BonusValues[StagesParser.currSetIndex];
							string[] kovceziValues = kovcezi.Split('#');
							kovceziValues[StagesParser.bonusID-1] = "0";
							
							string pom = System.String.Empty;
							kovcezi = System.String.Empty;
							for(int i=0;i<kovceziValues.Length;i++)
							{
								kovcezi+=kovceziValues[i] + "#";
							}
							kovcezi = kovcezi.Remove(kovcezi.Length-1);
							BonusValues[StagesParser.currSetIndex] = kovcezi;
							
							for(int i=0;i<StagesParser.totalSets;i++)
							{
								pom += BonusValues[i] + "_"; 
							}
							pom = pom.Remove(pom.Length-1);
							PlayerPrefs.SetString("BonusLevel",pom);
							PlayerPrefs.Save();
							StagesParser.bonusLevels = pom;
							StagesParser.ServerUpdate = 1;

							//PlayerPrefs.SetInt("BonusLevel#"+StagesParser.currentWorld+"#"+StagesParser.bonusID,0);
							//PlayerPrefs.Save();
							
							if(majmunceSeMrda)
								StopCoroutine("KretanjeMajmunceta");

							if(StagesParser.pozicijaMajmuncetaNaMapi == Vector3.zero)
							{
								kovcegNaPocetku = trenutniKovceg = pronadjiKovceg(StagesParser.bonusName);
							}
							else
							{
								trenutniKovceg = pronadjiKovceg(StagesParser.bonusName);
							}
							trenutniKovceg.Find("GateClosed").GetComponent<Renderer>().enabled = false;
							trenutniKovceg.Find("GateOpen").GetComponent<Renderer>().enabled = true;
							animator.Play("Running");
							StartCoroutine("KretanjeMajmunceta");
						}
						else
						{
							_GUI.Find("INTERFACE HOLDER/_TopLeft/BananaHolder/Bananas").GetComponent<Animation>().Play();
						}
					}
					else
					{
						//Locked
					}


				}
				else if(releasedItem.Equals("Button_UnlockBonusNO"))
				{
					popupZaSpustanje = _GUI.Find("UNLOCK HOLDER/AnimationHolderGlavni");
					Invoke("spustiPopup",0.5f);
					_GUI.Find("UNLOCK HOLDER/AnimationHolderGlavni").GetComponent<Animator>().Play("ClosePopup");
					makniPopup = 0;
				}

				else if(releasedItem.Contains("tvtv"))
				{
					televizorIzabrao = true;
					prejasiTelevizor = true;
					//4-5_1tvtv1
					//12-13_1tvtv1
					int nivoPreTV = int.Parse(releasedItem.Substring(0,releasedItem.IndexOf("-")));
					if(StagesParser.SetsInGame[StagesParser.currSetIndex].GetStarOnStage(nivoPreTV-1) > 0 && makniPopup == 0)
					{
						if(PlaySounds.soundOn)
							PlaySounds.Play_Button_GoBack();
						monkeyDestinationLevelIndex = GetWatchVideoIndex(releasedItem.Substring(releasedItem.Length-1));
						trenutniTelevizor = int.Parse(releasedItem.Substring(releasedItem.Length-1));
						watchVideoIndex_pom = monkeyDestinationLevelIndex;
						televizorNaMapi = true;
						if(majmunceSeMrda)
							StopCoroutine("KretanjeMajmunceta");
						animator.Play("Running");
						StartCoroutine("KretanjeMajmunceta");
					}
					else 
					{
						//Locked
					}
				}
				else if(releasedItem.Equals("Button_WatchVideoYES"))
				{
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_GoBack();
					makniPopup = 9;
					StartCoroutine(checkConnectionForTelevizor());

				}
				else if(releasedItem.Equals("Button_WatchVideoNO"))
				{
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_GoBack();
					popupZaSpustanje = _GUI.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni");
					Invoke("spustiPopup",0.5f);
					_GUI.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni").GetComponent<Animator>().Play("ClosePopup");
					makniPopup = 0;
					if(!televizorIzabrao)
					{
						prejasiTelevizor = true;
						animator.Play("Running");
						StartCoroutine("KretanjeMajmunceta");
					}
					else
					{
						televizorIzabrao = false;
						prejasiTelevizor = false;
					}
				}

				else if(releasedItem.Equals("Button_MissionCancel"))
				{
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_GoBack();
					popupZaSpustanje = _GUI.Find("MISSION HOLDER/AnimationHolderGlavni");
					Invoke("spustiPopup",0.5f);
					_GUI.Find("MISSION HOLDER/AnimationHolderGlavni").GetComponent<Animator>().Play("ClosePopup");
					makniPopup = 0;
					ocistiMisije();
					prejasiTelevizor = false;
				}

				else if(releasedItem.Equals("Button_MissionPlay"))
				{
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_GoBack();
					if(StagesParser.odgledaoTutorial == 1 && StagesParser.currentLevel == 2)
					{
						StagesParser.loadingTip = 3;
					}
					//Application.LoadLevel(2);
					StartCoroutine(closeDoorAndPlay());
				}
				else if(releasedItem.Equals("ShopKucica"))
				{
					if(StagesParser.otvaraoShopNekad == 0 || StagesParser.otvaraoShopNekad == 2)
					{
						if(makniPopup == 6)
						{
							StagesParser.currentMoney += int.Parse(ShopManagerFull.ShopObject.CoinsHats[0]);
							PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
							PlayerPrefs.Save();
							TutorialShop.SetActive(false);
							SwipeControlCustomizationHats.allowInput = false;
							Invoke("prebaciStrelicuNaItem",1.2f);
						}
						else
						{
							StagesParser.otvaraoShopNekad = 1;
							PlayerPrefs.SetString("OdgledaoTutorial",StagesParser.odgledaoTutorial.ToString()+"#"+StagesParser.otvaraoShopNekad.ToString());
							PlayerPrefs.Save();
						}

					}
					//Otvaranje Customize Shop-a
					_GUI.Find("ShopHolder/Shop").GetComponent<Animation>().Play("MeniDolazak");
					

					if(ShopManagerFull.AktivanCustomizationTab==1)
					{
						ShopManagerFull.AktivanItemSesir=ShopManagerFull.AktivanItemSesir+1;
					}
					else if(ShopManagerFull.AktivanCustomizationTab==2)
					{
						ShopManagerFull.AktivanItemMajica=ShopManagerFull.AktivanItemMajica+1;
					}
					else if(ShopManagerFull.AktivanCustomizationTab==3)
					{
						ShopManagerFull.AktivanItemRanac=ShopManagerFull.AktivanItemRanac+1;
					}

					ShopManagerFull.ShopObject.PozoviTab(3);
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
					if(makniPopup == 0) //uslov zbog tutoriala, da ne izadje iz shopa na back dok ne kupi prvi item
						makniPopup = 5;
				}
				else if(releasedItem.Equals("BankInApp") || releasedItem.Equals("Bananas"))
				{
					//Otvaranje Banana Shop-a
					_GUI.Find("ShopHolder/Shop").GetComponent<Animation>().Play("MeniDolazak");
					ShopManagerFull.ShopObject.PozoviTab(2);
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
					makniPopup = 5;
				}
				else if(releasedItem.Equals("Coins") || releasedItem.Equals("FreeCoins"))
				{
					//Otvaranje Banana Shop-a
					_GUI.Find("ShopHolder/Shop").GetComponent<Animation>().Play("MeniDolazak");
					ShopManagerFull.ShopObject.PozoviTab(1);
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
					makniPopup = 5;
				}
				else if(releasedItem.Equals("ButtonBackShop"))
				{
					_GUI.Find("INTERFACE HOLDER/_TopLeft/Coins/Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
					_GUI.Find("INTERFACE HOLDER/_TopLeft/Coins/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
					_GUI.Find("INTERFACE HOLDER/_TopLeft/BananaHolder/Bananas/Banana Number").GetComponent<TextMesh>().text = StagesParser.currentBananas.ToString();
					_GUI.Find("INTERFACE HOLDER/_TopLeft/BananaHolder/Bananas/Banana Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
					ShopManagerFull.ShopObject.SkloniShop();
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
					makniPopup = 0;
				}
				else if(releasedItem.Equals("ButtonCustomize"))
				{
					ShopManagerFull.ShopObject.PozoviTab(3);
					
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}
				else if(releasedItem.Equals("ButtonFreeCoins"))
				{
					ShopManagerFull.ShopObject.PozoviTab(1);
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}
				else if(releasedItem.Equals("ButtonPowerUps"))
				{
					ShopManagerFull.ShopObject.PozoviTab(4);
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}
				else if(releasedItem.Equals("ButtonShop"))
				{
					ShopManagerFull.ShopObject.PozoviTab(2);
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}
				else if(releasedItem.Equals("1HatsShopTab"))
				{
					ShopManagerFull.ShopObject.DeaktivirajCustomization();
					ShopManagerFull.AktivanItemSesir=ShopManagerFull.AktivanItemSesir+1;
					ShopManagerFull.ShopObject.PozoviCustomizationTab(1);
					
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}
				else if(releasedItem.Equals("2TShirtsShopTab"))
				{
					ShopManagerFull.ShopObject.DeaktivirajCustomization();
					ShopManagerFull.AktivanItemMajica=ShopManagerFull.AktivanItemMajica+1;
					ShopManagerFull.ShopObject.PozoviCustomizationTab(2);
					
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}
				else if(releasedItem.Equals("3BackPackShopTab"))
				{
					ShopManagerFull.ShopObject.DeaktivirajCustomization();
					ShopManagerFull.AktivanItemRanac=ShopManagerFull.AktivanItemRanac+1;
					ShopManagerFull.ShopObject.PozoviCustomizationTab(3);
					
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem.StartsWith("Hats"))
				{
					
					for(int i=0;i<ShopManagerFull.ShopObject.HatsObjects.Length;i++)
					{
						if(releasedItem.StartsWith("Hats "+(i+1)))
						{
							ObjCustomizationHats.swipeCtrl.currentValue=ShopManagerFull.ShopObject.HatsObjects.Length-i-1;
						}
						
					}
					
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}
				
				else if(clickedItem==releasedItem && releasedItem.StartsWith("Shirts"))
				{
					
					for(int i=0;i<ShopManagerFull.ShopObject.ShirtsObjects.Length;i++)
					{
						if(releasedItem.StartsWith("Shirts "+(i+1)))
						{
							ObjCustomizationShirts.swipeCtrl.currentValue=ShopManagerFull.ShopObject.ShirtsObjects.Length-i-1;
						}
						
					}
					
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}
				
				else if(clickedItem==releasedItem && releasedItem.StartsWith("BackPacks"))
				{
					
					for(int i=0;i<ShopManagerFull.ShopObject.BackPacksObjects.Length;i++)
					{
						if(releasedItem.StartsWith("BackPacks "+(i+1)))
						{
							ObjCustomizationBackPacks.swipeCtrl.currentValue=ShopManagerFull.ShopObject.BackPacksObjects.Length-i-1;
						}
						
					}
					
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(releasedItem.Equals("ClearAll"))
				{
					ShopManagerFull.ShopObject.OcistiMajmuna();
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}
				else if(releasedItem.Equals("Preview Button"))
				{
					if(ShopManagerFull.PreviewState)
					{
						ShopManagerFull.ShopObject.PreviewItem();
					}

					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}
				else if(releasedItem.Equals("Buy Button"))
				{
					if(makniPopup == 6 && ShopManagerFull.BuyButtonState == 2)
					{
						TutorialShop.SetActive(false);
						SwipeControlCustomizationHats.allowInput = true;
						StagesParser.otvaraoShopNekad = 1;
						StagesParser.odgledaoTutorial = 3;
						PlayerPrefs.SetString("OdgledaoTutorial",StagesParser.odgledaoTutorial.ToString()+"#"+StagesParser.otvaraoShopNekad.ToString());
						PlayerPrefs.Save();
						makniPopup = 5;
					}

					ShopManagerFull.ShopObject.KupiItem();
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}
				else if(releasedItem.Equals("Button_CollectReward"))
				{
					DodeliNagrade();
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_GoBack();
					popupZaSpustanje = _GUI.Find("REWARD HOLDER/AnimationHolderGlavni");
					Invoke("spustiPopup",0.5f);
					_GUI.Find("REWARD HOLDER/AnimationHolderGlavni").GetComponent<Animator>().Play("ClosePopup");
					_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/PomocniColliderKodOtvaranjaKovcega").localScale = Vector3.zero;
					makniPopup = 0;
				}
				else if(releasedItem.Equals("Shop Banana"))
				{
					ShopManagerFull.ShopObject.KupiBananu();
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}
				
				else if(releasedItem.Equals("Shop POWERUP Double Coins"))
				{
					ShopManagerFull.ShopObject.KupiDoubleCoins();
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}
				
				else if(releasedItem.Equals("Shop POWERUP Magnet"))
				{
					ShopManagerFull.ShopObject.KupiMagnet();
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}
				
				else if(releasedItem.Equals("Shop POWERUP Shield"))
				{
					ShopManagerFull.ShopObject.KupiShield();
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}
				else if(releasedItem.Equals("FB Login"))
				{
					makniPopup = 8;
					StartCoroutine(checkConnectionForLoginButton());
				}
				else if(releasedItem.StartsWith("Friends Level"))
				{
					makniPopup = 8;
					StartCoroutine(checkConnectionForInviteFriend());
				}
				else if(releasedItem.Equals("Button_CheckOK"))
				{
					makniPopup = 0;
					StartCoroutine(CheckInternetConnection.Instance.ClosePopup());
				}
				else if(releasedItem.Equals("ShopFCBILikePage"))
				{
					makniPopup = 8;
					StartCoroutine(checkConnectionForPageLike("https://www.facebook.com/pages/Banana-Island/636650059721490","BananaIsland"));
					//StartCoroutine(FacebookManager.FacebookObject.otvaranjeStranice("https://www.facebook.com/pages/Banana-Island/636650059721490","BananaIsland"));
				}
				else if(releasedItem.Equals("ShopFCWatchVideo"))
				{
					makniPopup = 8;
					StartCoroutine(checkConnectionForWatchVideo());
				}
				else if(releasedItem.Equals("ShopFCWLLikePage"))
				{
					makniPopup = 8;
					StartCoroutine(checkConnectionForPageLike("https://www.facebook.com/WebelinxGamesApps","Webelinx"));
					//StartCoroutine(FacebookManager.FacebookObject.otvaranjeStranice("https://www.facebook.com/WebelinxGamesApps","Webelinx"));
				}
			}
		}
	}

	void DodeliNagrade()
	{
		switch(reward1Type)
		{
		case 1: StagesParser.powerup_magnets += kolicinaReward1; break;
		case 2: StagesParser.powerup_doublecoins += kolicinaReward1; break;
		case 3: StagesParser.powerup_shields += kolicinaReward1; break;
		case 4: DajMuItem(); break;
		}
		switch(reward2Type)
		{
		case 1: StagesParser.powerup_magnets += kolicinaReward2; break;
		case 2: StagesParser.powerup_doublecoins += kolicinaReward2; break;
		case 3: StagesParser.powerup_shields += kolicinaReward2; break;
		}
		switch(reward3Type)
		{
		case 1: StagesParser.powerup_magnets += kolicinaReward3; break;
		case 2: StagesParser.powerup_doublecoins += kolicinaReward3; break;
		case 3: StagesParser.powerup_shields += kolicinaReward3; break;
		}
		PlayerPrefs.SetString("PowerUps",(StagesParser.powerup_doublecoins+"#"+StagesParser.powerup_magnets+"#"+StagesParser.powerup_shields));
		PlayerPrefs.Save();

		GameObject.Find ("Double Coins Number/Number").GetComponent<TextMesh> ().text = StagesParser.powerup_doublecoins.ToString ();
		GameObject.Find ("Double Coins Number/Number").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		
		GameObject.Find ("Magnet Number/Number").GetComponent<TextMesh> ().text = StagesParser.powerup_magnets.ToString ();
		GameObject.Find ("Magnet Number/Number").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		
		GameObject.Find ("Shield Number/Number").GetComponent<TextMesh> ().text = StagesParser.powerup_shields.ToString ();
		GameObject.Find ("Shield Number/Number").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		reward1Type = 0;
		reward2Type = 0;
		reward3Type = 0;
		kolicinaReward1 = 0;
		kolicinaReward2 = 0;
		kolicinaReward3 = 0;

		StagesParser.ServerUpdate = 1;
	}

	void DajMuItem()
	{
		string[] values;
		if(cetvrtiKovcegNagrada == "Glava")
		{
			values = StagesParser.svekupovineGlava.Split('#');
			values[indexNagradeZaCetvrtiKovceg] = "1";
			string pom = "";
			for(int i=0;i<values.Length;i++)
				pom+=values[i] + "#";
			
			pom = pom.Remove(pom.Length-1);
			StagesParser.svekupovineGlava = pom;
			ShopManagerFull.ShopObject.HatsObjects[indexNagradeZaCetvrtiKovceg].Find("Stikla").gameObject.SetActive(true);
			ShopManagerFull.SveStvariZaOblacenjeHats[indexNagradeZaCetvrtiKovceg] = 1;
			//ShopManagerFull.ShopObject.ProveriStanjeSesira(indexNagradeZaCetvrtiKovceg);
			PlayerPrefs.SetString("UserSveKupovineHats",StagesParser.svekupovineGlava);
		}
		else if(cetvrtiKovcegNagrada == "Majica")
		{
			values = StagesParser.svekupovineMajica.Split('#');
			values[indexNagradeZaCetvrtiKovceg] = "1";
			string pom = "";
			for(int i=0;i<values.Length;i++)
				pom+=values[i] + "#";
			
			pom = pom.Remove(pom.Length-1);
			StagesParser.svekupovineMajica = pom;
			ShopManagerFull.ShopObject.ShirtsObjects[indexNagradeZaCetvrtiKovceg].Find("Stikla").gameObject.SetActive(true);
			ShopManagerFull.SveStvariZaOblacenjeShirts[indexNagradeZaCetvrtiKovceg] = 1;
			//ShopManagerFull.ShopObject.ProveriStanjeMajica(indexNagradeZaCetvrtiKovceg);
			PlayerPrefs.SetString("UserSveKupovineShirts",StagesParser.svekupovineMajica);
		}
		else if(cetvrtiKovcegNagrada == "Ledja")
		{
			values = StagesParser.svekupovineLedja.Split('#');
			values[indexNagradeZaCetvrtiKovceg] = "1";
			string pom = "";
			for(int i=0;i<values.Length;i++)
				pom+=values[i] + "#";
			
			pom = pom.Remove(pom.Length-1);
			StagesParser.svekupovineLedja = pom;
			ShopManagerFull.ShopObject.BackPacksObjects[indexNagradeZaCetvrtiKovceg].Find("Stikla").gameObject.SetActive(true);
			ShopManagerFull.SveStvariZaOblacenjeBackPack[indexNagradeZaCetvrtiKovceg] = 1;
			//ShopManagerFull.ShopObject.ProveriStanjeRanca(indexNagradeZaCetvrtiKovceg);
			PlayerPrefs.SetString("UserSveKupovineBackPacks",StagesParser.svekupovineLedja);
		}
		else if(cetvrtiKovcegNagrada == "PowerUps")
		{
			StagesParser.powerup_magnets += kolicinaReward1;
			StagesParser.powerup_doublecoins += kolicinaReward2;
			StagesParser.powerup_shields += kolicinaReward3;
		}
		ShopManagerFull.ShopObject.ProveriStanjeCelogShopa();
		PlayerPrefs.Save();
		indexNagradeZaCetvrtiKovceg = -1;
	}

	void IspitajItem()
	{
		Transform kategorija;
		string itemIzKategorije = System.String.Empty;

		string[] values = StagesParser.svekupovineGlava.Split('#');
		for(int i=0; i<=ShopManagerFull.BrojOtkljucanihKapa; i++)
		{
			if(int.Parse(values[i]) == 0)
			{
				cetvrtiKovcegNagrada = "Glava";
				indexNagradeZaCetvrtiKovceg = i;
				break;
			}
		}
		if(indexNagradeZaCetvrtiKovceg == -1) //nema nijednu otkljucanu glavu da mu da, ispituje majice
		{
			values = StagesParser.svekupovineMajica.Split('#');
			for(int i=0;i<=ShopManagerFull.BrojOtkljucanihMajici; i++)
			{
				if(int.Parse(values[i]) == 0)
				{
					cetvrtiKovcegNagrada = "Majica";
					indexNagradeZaCetvrtiKovceg = i;
					break;
				}
			}
			if(indexNagradeZaCetvrtiKovceg == -1) //nema nijednu otkljucanu majicu da mu da, ispituje ledja
			{
				values = StagesParser.svekupovineLedja.Split('#');
				for(int i=0;i<=ShopManagerFull.BrojOtkljucanihRanceva; i++)
				{
					if(int.Parse(values[i]) == 0)
					{
						cetvrtiKovcegNagrada = "Ledja";
						indexNagradeZaCetvrtiKovceg = i;
						break;
					}
				}
				if(indexNagradeZaCetvrtiKovceg == -1) //nema nijedna otkljucana ledja da mu da, daj mu po 2 od svakog powerUp-a
				{
					cetvrtiKovcegNagrada = "PowerUps";

					kolicinaReward1 = 2;
					kolicinaReward2 = 2;
					kolicinaReward3 = 2;

					_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward2").gameObject.SetActive(true);
					_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward3").gameObject.SetActive(true);

					_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward1/Count").GetComponent<TextMesh>().text = kolicinaReward1.ToString();
					_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward2/Count").GetComponent<TextMesh>().text = kolicinaReward2.ToString();
					_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward3/Count").GetComponent<TextMesh>().text = kolicinaReward3.ToString();
					
					_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward1").GetComponent<SpriteRenderer>().sprite = _GUI.Find("ShopHolder/Shop/4 Power-Ups/Power-Ups Tabovi/Shop POWERUP Magnet/Plavi Bedz/Magnet Icon").GetComponent<SpriteRenderer>().sprite;
					_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward2").GetComponent<SpriteRenderer>().sprite = _GUI.Find("ShopHolder/Shop/4 Power-Ups/Power-Ups Tabovi/Shop POWERUP Shield/Plavi Bedz/Shield Icon").GetComponent<SpriteRenderer>().sprite;
					_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward3").GetComponent<SpriteRenderer>().sprite = _GUI.Find("ShopHolder/Shop/4 Power-Ups/Power-Ups Tabovi/Shop POWERUP Double Coins/Plavi Bedz/Double Coins Icon").GetComponent<SpriteRenderer>().sprite;

				}
				else
				{
					//ima slobodan item za ledja
//					values[indexNagradeZaCetvrtiKovceg] = "1";
//					string pom = "";
//					for(int i=0;i<values.Length;i++)
//						pom+=values[i] + "#";
//					
//					pom = pom.Remove(pom.Length-1);
//					StagesParser.svekupovineLedja = pom;
					kategorija = GameObject.Find("3 Customize/Customize Tabovi/3BackPack").transform;
					for(int i=0;i<kategorija.childCount;i++)
					{
						if(kategorija.GetChild(i).name.StartsWith("BackPacks " + (indexNagradeZaCetvrtiKovceg+1).ToString()))
						{
							itemIzKategorije = kategorija.GetChild(i).name;
							break;
						}
					}

					if(!itemIzKategorije.Equals(System.String.Empty))
					{
						string samoImePredmeta = itemIzKategorije.Substring(itemIzKategorije.IndexOf("- ")+2);
						_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward1").GetComponent<SpriteRenderer>().sprite = kategorija.Find(itemIzKategorije).Find("Plavi Bedz/"+samoImePredmeta + " Icon").GetComponent<SpriteRenderer>().sprite;
					}

				}

			}
			else
			{
				//ima slobodan item za majicu
//				values[indexNagradeZaCetvrtiKovceg] = "1";
//				string pom = "";
//				for(int i=0;i<values.Length;i++)
//					pom+=values[i] + "#";
//				
//				pom = pom.Remove(pom.Length-1);
//				StagesParser.svekupovineMajica = pom;
				kategorija = GameObject.Find("3 Customize/Customize Tabovi/2Shirts").transform;
				for(int i=0;i<kategorija.childCount;i++)
				{
					if(kategorija.GetChild(i).name.StartsWith("Shirts " + (indexNagradeZaCetvrtiKovceg+1).ToString()))
					{
						itemIzKategorije = kategorija.GetChild(i).name;
						break;
					}
				}
				
				if(!itemIzKategorije.Equals(System.String.Empty))
				{
					string samoImePredmeta = itemIzKategorije.Substring(itemIzKategorije.IndexOf("- ")+2);
					_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward1").GetComponent<SpriteRenderer>().sprite = kategorija.Find(itemIzKategorije).Find("Plavi Bedz/"+samoImePredmeta + " Icon").GetComponent<SpriteRenderer>().sprite;
					_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward1").GetComponent<SpriteRenderer>().color = kategorija.Find(itemIzKategorije).Find("Plavi Bedz/"+samoImePredmeta + " Icon").GetComponent<SpriteRenderer>().color;
				}
			}
		}
		else
		{
			//ima slobodan item za glavu
//			values[indexNagradeZaCetvrtiKovceg] = "1";
//			string pom = "";
//			for(int i=0;i<values.Length;i++)
//				pom+=values[i] + "#";
//
//			pom = pom.Remove(pom.Length-1);
//			StagesParser.svekupovineGlava = pom;
			kategorija = GameObject.Find("3 Customize/Customize Tabovi/1Hats").transform;
			for(int i=0;i<kategorija.childCount;i++)
			{
				if(kategorija.GetChild(i).name.StartsWith("Hats " + (indexNagradeZaCetvrtiKovceg+1).ToString()))
				{
					itemIzKategorije = kategorija.GetChild(i).name;
					break;
				}
			}
			
			if(!itemIzKategorije.Equals(System.String.Empty))
			{
				string samoImePredmeta = itemIzKategorije.Substring(itemIzKategorije.IndexOf("- ")+2);
				_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward1").GetComponent<SpriteRenderer>().sprite = kategorija.Find(itemIzKategorije).Find("Plavi Bedz/"+samoImePredmeta + " Icon").GetComponent<SpriteRenderer>().sprite;
			}
		}
		//_GUI.Find("REWARD HOLDER/AnimationHolderGlavni/AnimationHolder/RewardCollectPopup/Reward1").GetComponent<SpriteRenderer>().sprite = null;
	}

	Transform pronadjiKovceg(string name)
	{
		for(int i=0;i<BonusNivoi.Length;i++)
		{
			if(BonusNivoi[i].name.Equals(name))
			{
				return BonusNivoi[i];
			}
		}
		return null;
	}

	int GetMapLevelIndex(int value)
	{
		int levelName;
		for(int i=0;i<izmedjneTacke.childCount;i++)
		{
			if(int.TryParse(izmedjneTacke.GetChild(i).name, out levelName))
			{
				if(levelName == value)
					return i;
			}
		}
		return -1;
	}

	int GetWatchVideoIndex(string tvsuffix)
	{
		for(int i=0;i<izmedjneTacke.childCount;i++)
		{
			if(izmedjneTacke.GetChild(i).name.Contains("tvtv"+tvsuffix))
			{
				return i; //ovo treba da se doradi
			}
		}
		return -1;
	}

	int findAngleDir(Transform start, Vector3 target)
	{
		Vector3 dif = target - start.position;
		if(dif != Vector3.zero)
		{
			float angle = (dif.y <= start.right.y) ? Vector3.Angle(start.right,dif) : 360 - Vector3.Angle(start.right,dif);
			if(angle >= 22 && angle < 67) // osa 0
				return 0;
			else if(angle >= 67 && angle < 112) // osa 1
				return 1;
			else if(angle >= 112 && angle < 157) // osa 2
				return 2;
			else if(angle >= 157 && angle < 202) // osa 3
				return 3;
			else if(angle >= 202 && angle < 247) // osa 4
				return 4;
			else if(angle >= 247 && angle < 292) // osa 5
				return 5;
			else if(angle >= 292 && angle < 337) // osa 6
				return 6;
			else return 7;
		}
		return -1;


	}

	IEnumerator KretanjeMajmunceta()
	{
		majmunceSeMrda = true;
		int rastojanje = Mathf.Abs(monkeyCurrentLevelIndex - monkeyDestinationLevelIndex);
		float brzina = rastojanje*Time.deltaTime;
		if(rastojanje == 0)
			brzina = 4*Time.deltaTime;
		int angleDir;
		Quaternion a = Quaternion.identity;
		bool izadji = false;
		brzina = Mathf.Clamp(brzina, 0.065f, 1);

		if(StagesParser.pozicijaMajmuncetaNaMapi != Vector3.zero) //kretanje od kovcega
		{
			Transform koraciDoKovcega = kovcegNaPocetku.Find("Koraci do kovcega");
			for(int i=koraciDoKovcega.childCount-1;i>=0;i--)
			{
				angleDir = findAngleDir(holderMajmun,koraciDoKovcega.GetChild(i).position);
				if(angleDir != -1)
					a = Quaternion.Euler(angles[angleDir]);
				while(holderMajmun.position != koraciDoKovcega.GetChild(i).position)
				{
					if(!PlaySounds.Run.isPlaying && PlaySounds.soundOn)
						PlaySounds.Play_Run();
					holderMajmun.position = Vector3.MoveTowards(holderMajmun.position, koraciDoKovcega.GetChild(i).position, brzina);
					yield return null;
					if(angleDir != -1)
						majmun.rotation = Quaternion.Lerp(majmun.rotation,a,0.2f);
				}
			}
			StagesParser.pozicijaMajmuncetaNaMapi = Vector3.zero;
		}

		if(monkeyCurrentLevelIndex < monkeyDestinationLevelIndex) // od manjeg nivoa ka vecem
		{
			for(int i=monkeyCurrentLevelIndex;i<=monkeyDestinationLevelIndex;i++)
			{
				//Vector3 dif = izmedjneTacke.GetChild(i).position - holderMajmun.position;
				//Debug.Log("Dif: " + dif);
				//float angle = (dif.y <= holderMajmun.right.y) ? Vector3.Angle(holderMajmun.right,dif) : 360 - Vector3.Angle(holderMajmun.right,dif);
				//Debug.Log("angle: " + angle + ", .. " + i);
				//Debug.DrawLine(holderMajmun.position,izmedjneTacke.GetChild(i).position,Color.red,1000);
				angleDir = findAngleDir(holderMajmun,izmedjneTacke.GetChild(i).position);
				if(angleDir != -1)
					a = Quaternion.Euler(angles[angleDir]);
				while(holderMajmun.position != izmedjneTacke.GetChild(i).position && !izadji)
				{
					if(!PlaySounds.Run.isPlaying && PlaySounds.soundOn)
						PlaySounds.Play_Run();
					holderMajmun.position = Vector3.MoveTowards(holderMajmun.position,izmedjneTacke.GetChild(i).position,brzina);
					yield return null;
					monkeyCurrentLevelIndex = i;
					//brzina+=Time.deltaTime/2;
					if(angleDir != -1)
						majmun.rotation = Quaternion.Lerp(majmun.rotation,a,0.2f);

					if(izmedjneTacke.GetChild(i).name.Contains("tvtv") && izmedjneTacke.GetChild(i).gameObject.activeSelf && !prejasiTelevizor)// && trenutniTelevizor == int.Parse(izmedjneTacke.GetChild(i).name.Substring(izmedjneTacke.GetChild(i).name.Length-1)))
					{
						yield return new WaitForEndOfFrame();
						televizorNaMapi = true;
						majmunceSeMrda = false;
						//StopCoroutine("KretanjeMajmunceta"); NE SA StopCoroutine VEC SA NEKIM BOOLOOM U WHILE-U!!!
						izadji = true;
						trenutniTelevizor = int.Parse(izmedjneTacke.GetChild(i).name.Substring(izmedjneTacke.GetChild(i).name.IndexOf("tvtv")+4));
						animator.Play("Idle");
						monkeyCurrentLevelIndex = i+1;
					}
				}
			}
		}
		else // od veceg nivoa ka manjem
		{
			for(int i=monkeyCurrentLevelIndex;i>=monkeyDestinationLevelIndex;i--)
			{
				//Debug.Log("angle: " + Vector3.Angle(holderMajmun.position,izmedjneTacke.GetChild(i).position) + ", .. " + i);
				angleDir = findAngleDir(holderMajmun,izmedjneTacke.GetChild(i).position);
				if(angleDir != -1)
					a = Quaternion.Euler(angles[angleDir]);
				while(holderMajmun.position != izmedjneTacke.GetChild(i).position && !izadji)
				{
					if(!PlaySounds.Run.isPlaying && PlaySounds.soundOn)
						PlaySounds.Play_Run();
					holderMajmun.position = Vector3.MoveTowards(holderMajmun.position,izmedjneTacke.GetChild(i).position,brzina);
					yield return null;
					monkeyCurrentLevelIndex = i;
					//brzina+=Time.deltaTime/2;
					if(angleDir != -1)
						majmun.rotation = Quaternion.Lerp(majmun.rotation,a,0.2f);

					if(izmedjneTacke.GetChild(i).name.Contains("tvtv") && izmedjneTacke.GetChild(i).gameObject.activeSelf && !prejasiTelevizor)// && trenutniTelevizor == int.Parse(izmedjneTacke.GetChild(i).name.Substring(izmedjneTacke.GetChild(i).name.Length-1)))
					{
						yield return new WaitForEndOfFrame();
						televizorNaMapi = true;
						majmunceSeMrda = false;
						//StopCoroutine("KretanjeMajmunceta");
						izadji = true;
						trenutniTelevizor = int.Parse(izmedjneTacke.GetChild(i).name.Substring(izmedjneTacke.GetChild(i).name.IndexOf("tvtv")+4));
						animator.Play("Idle");
						monkeyCurrentLevelIndex = i-1;
					}
				}
			}
		}



		//monkeyCurrentLevelIndex = monkeyDestinationLevelIndex/;

		if(kretanjeDoKovcega && !televizorNaMapi)
		{
			if(StagesParser.pozicijaMajmuncetaNaMapi == Vector3.zero) //ide ka kovcegu
			{
				Transform koraciDoKovcega = trenutniKovceg.Find("Koraci do kovcega");
				//rastojanje = koraciDoKovcega.childCount;
				//brzina = 10*Time.deltaTime;
				for(int i=0;i<koraciDoKovcega.childCount;i++)
				{
					angleDir = findAngleDir(holderMajmun,koraciDoKovcega.GetChild(i).position);
					if(angleDir != -1)
						a = Quaternion.Euler(angles[angleDir]);
					while(holderMajmun.position != koraciDoKovcega.GetChild(i).position)
					{
						if(!PlaySounds.Run.isPlaying && PlaySounds.soundOn)
							PlaySounds.Play_Run();
						holderMajmun.position = Vector3.MoveTowards(holderMajmun.position, koraciDoKovcega.GetChild(i).position, brzina);
						yield return null;
						if(angleDir != -1)
							majmun.rotation = Quaternion.Lerp(majmun.rotation,a,0.2f);
					}
				}
			}
//			else //ide od kovcega
//			{
//				Transform koraciDoKovcega = trenutniKovceg.Find("Koraci do kovcega");
//				for(int i=koraciDoKovcega.childCount-1;i>=0;i--)
//				{
//					angleDir = findAngleDir(holderMajmun,koraciDoKovcega.GetChild(i).position);
//					if(angleDir != -1)
//						a = Quaternion.Euler(angles[angleDir]);
//					Debug.Log("pozicija tacke: " + koraciDoKovcega.GetChild(i).position);
//					while(holderMajmun.position != koraciDoKovcega.GetChild(i).position)
//					{
//						holderMajmun.position = Vector3.MoveTowards(holderMajmun.position, koraciDoKovcega.GetChild(i).position, brzina);
//						yield return null;
//						if(angleDir != -1)
//							majmun.rotation = Quaternion.Lerp(majmun.rotation,a,0.2f);
//					}
//				}
//			}
			StagesParser.bonusLevel = true;
			StagesParser.pozicijaMajmuncetaNaMapi = holderMajmun.position;

			animator.Play("Idle");
			majmunceSeMrda = false;
			float t = 0;
			while(t < 1)
			{
				Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,new Vector3(holderMajmun.position.x,holderMajmun.position.y,Camera.main.transform.position.z),t);
				t += Time.deltaTime;
				yield return null;
			}
			Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x, levaGranica,desnaGranica), Mathf.Clamp(Camera.main.transform.position.y, donjaGranica,gornjaGranica), Camera.main.transform.position.z);
			if(kretanjeDoKovcega)
			{
				kretanjeDoKovcega = false;
				//Application.LoadLevel(2);
				StartCoroutine(closeDoorAndPlay());
			}
			MissionManager.OdrediMisiju(StagesParser.currentLevel-1,true);
			//Transform temp = _GUI.Find("MISSION HOLDER");
			//temp.position = new Vector3(temp.position.x,guiCameraY,temp.position.z);
			if(!StagesParser.bonusLevel)
			{
				if(!FB.IsLoggedIn)
				{
					if(_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN").gameObject.activeSelf)
						_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN").gameObject.SetActive(false);
				}
				else
				{
					if(!_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN").gameObject.activeSelf)
						_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN").gameObject.SetActive(true);
					getFriendsScoresOnLevel(StagesParser.currentLevel);
				}
				_GUI.Find("MISSION HOLDER/AnimationHolderGlavni").localPosition += new Vector3(0,35,0);
				_GUI.Find("MISSION HOLDER/AnimationHolderGlavni").GetComponent<Animator>().Play("OpenPopup");
				makniPopup = 1;
			}
		}

		else if(televizorNaMapi)
		{
			televizorNaMapi = false;
			animator.Play("Idle");
			_GUI.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup/Coins/Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
			_GUI.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup/Coins/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			//Debug.Log("WATCH VIDEO!!! TREBA! ZA PARE!");
			//quad.SetActive(true);
			//Transform temp = _GUI.Find("WATCH VIDEO HOLDER");
			//temp.position = new Vector3(temp.position.x,guiCameraY,temp.position.z);
			_GUI.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni").localPosition += new Vector3(0,35,0);
			_GUI.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni").GetComponent<Animator>().Play("OpenPopup");
			makniPopup = 4;
		}

		else
		{
			animator.Play("Idle");
			majmunceSeMrda = false;
			float t = 0;

			float limitX;
			if(holderMajmun.position.x > desnaGranica)
				limitX = desnaGranica;
			else if(holderMajmun.position.x < levaGranica)
				limitX = levaGranica;
			else limitX = holderMajmun.position.x;
			while(Camera.main.transform.position.x != limitX)
			{
				Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,new Vector3(limitX,holderMajmun.position.y,Camera.main.transform.position.z),t);
				t += Time.deltaTime;
				yield return null;

			}
			//Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x, levaGranica,desnaGranica), Mathf.Clamp(Camera.main.transform.position.y, donjaGranica,gornjaGranica), Camera.main.transform.position.z);

			StagesParser.currStageIndex = levelName-1;
			StagesParser.currentLevel = StagesParser.currSetIndex*20 + levelName;
			//StartCoroutine(PlayAndWaitForAnimation(GameObject.Find(releasedItem).transform.GetChild(0).animation,"LevelsClickCard", true, 2));
			StagesParser.nivoZaUcitavanje = 10 + StagesParser.currSetIndex;
			//Application.LoadLevel(2);
			MissionManager.OdrediMisiju(StagesParser.currentLevel-1,true);
			//Transform temp = _GUI.Find("MISSION HOLDER");
			//temp.position = new Vector3(temp.position.x,guiCameraY,temp.position.z);
			if(!FB.IsLoggedIn)
			{
				if(_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN").gameObject.activeSelf)
					_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN").gameObject.SetActive(false);
			}
			else
			{
				if(!_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN").gameObject.activeSelf)
					_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN").gameObject.SetActive(true);
				getFriendsScoresOnLevel(StagesParser.currentLevel);
			}
			_GUI.Find("MISSION HOLDER/AnimationHolderGlavni").localPosition += new Vector3(0,35,0);
			_GUI.Find("MISSION HOLDER/AnimationHolderGlavni").GetComponent<Animator>().Play("OpenPopup");
			makniPopup = 1;
		}
	}

	void ocistiMisije()
	{
		Transform missionPopup = _GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Popup za Mission HOLDER/Popup za Mission");
		Transform mission1Icons = missionPopup.Find("Mission Icons/Mission 1");
		Transform mission2Icons = missionPopup.Find("Mission Icons/Mission 2");
		Transform mission3Icons = missionPopup.Find("Mission Icons/Mission 3");
		for(int i=0;i<mission1Icons.childCount;i++)
			mission1Icons.GetChild(i).GetComponent<Renderer>().enabled = false;
		for(int i=0;i<mission2Icons.childCount;i++)
			mission2Icons.GetChild(i).GetComponent<Renderer>().enabled = false;
		for(int i=0;i<mission3Icons.childCount;i++)
			mission3Icons.GetChild(i).GetComponent<Renderer>().enabled = false;

		missionPopup.Find("Text/Mission 1").GetComponent<TextMesh>().text = System.String.Empty;
		missionPopup.Find("Text/Mission 1").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		missionPopup.Find("Text/Mission 2").GetComponent<TextMesh>().text = System.String.Empty;
		missionPopup.Find("Text/Mission 2").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		missionPopup.Find("Text/Mission 3").GetComponent<TextMesh>().text = System.String.Empty;
		missionPopup.Find("Text/Mission 3").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
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

	IEnumerator closeDoorAndPlay()
	{
		_GUI.Find("LOADING HOLDER NEW/Loading Animation Vrata").GetComponent<Animator>().Play("Loading Zidovi Dolazak");
		yield return new WaitForSeconds(0.75f);
		Application.LoadLevel(2);
	}

	IEnumerator UcitajOstrvo(string ime)
	{
		//GameObject.Find("HolderLife").transform.parent = lifeManager.GetChild(0);
		//GameObject.Find("OblaciPomeranje").animation.Play("OblaciPostavljanje");
		_GUI.Find("LOADING HOLDER NEW/Loading Animation Vrata").GetComponent<Animator>().Play("Loading Zidovi Dolazak");
		yield return new WaitForSeconds(1.1f);
		Application.LoadLevel(ime);
	}

	private IEnumerator PlayAndWaitForAnimation ( Animation animation, string animName, bool loadAnotherScene, int indexOfSceneToLoad )
	{
		//GameObject.Find("HolderLife").transform.parent = lifeManager.GetChild(0);
//		animation.Play();
//		
//		do
//		{
			yield return null;
//		} 
//		while ( animation.IsPlaying(animName) );
		StagesParser.nivoZaUcitavanje = 10 + StagesParser.currSetIndex;
		if(loadAnotherScene)
			Application.LoadLevel(indexOfSceneToLoad);
		
	}

	IEnumerator PostaviKameruUGranice(Vector3 rez)
	{
		float t=0;
		Vector3 rez2 = Camera.main.transform.position+new Vector3(rez.x,rez.y,0);
		while(t < 1)
		{
			Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,rez2,t);
			t += Time.deltaTime/0.5f;
			yield return null;
		}
	}

	IEnumerator PokaziMuCustomize()
	{
		TutorialShop = Instantiate(TutorialShopPrefab,new Vector3(-33.2f,-95f,-60),Quaternion.identity) as GameObject;
		yield return new WaitForSeconds(1.2f);
		while(Camera.main.transform.position.y <= -18.45f)
		{
			Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position,new Vector3(Camera.main.transform.position.x,-18.4f,Camera.main.transform.position.z),0.055f);
			yield return null;
		}
		//TutorialShop = Instantiate(TutorialShopPrefab,new Vector3(11.2f,-44,-60),Quaternion.identity) as GameObject;

		//yield return new WaitForSeconds(0.55f);
		TutorialShop.transform.GetChild(0).GetComponent<Animation>().Play();
		yield return new WaitForSeconds(0.5f);
		TutorialShop.transform.GetChild(0).Find("RedArrowHolder/RedArrow").GetComponent<Renderer>().enabled = true;
		TutorialShop.transform.GetChild(0).Find("RedArrowHolder/RedArrow").GetComponent<Animation>().Play();
		TutorialShop.transform.GetChild(0).GetComponent<Collider>().enabled = false;

	}

	void changeLanguage()
	{
		if(!FB.IsLoggedIn)
		{
			GameObject.Find("Log In").GetComponent<TextMesh>().text = LanguageManager.LogIn;
			GameObject.Find("Log In").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		}
		GameObject.Find("Level No").GetComponent<TextMesh>().text = LanguageManager.Level;
		GameObject.Find("Level No").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		GameObject.Find("MissionText").GetComponent<TextMesh>().text = LanguageManager.Mission;
		GameObject.Find("MissionText").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN/Friends Level Win 1 HOLDER/FB/Fb Invite 1").GetComponent<TextMesh>().text = LanguageManager.Invite;
		_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN/Friends Level Win 2 HOLDER/FB/Fb Invite 1").GetComponent<TextMesh>().text = LanguageManager.Invite;
		_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN/Friends Level Win 3 HOLDER/FB/Fb Invite 1").GetComponent<TextMesh>().text = LanguageManager.Invite;
		_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN/Friends Level Win 4 HOLDER/FB/Fb Invite 1").GetComponent<TextMesh>().text = LanguageManager.Invite;
		_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN/Friends Level Win 5 HOLDER/FB/Fb Invite 1").GetComponent<TextMesh>().text = LanguageManager.Invite;
		_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN/Friends Level Win 1 HOLDER/FB/Fb Invite 1").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN/Friends Level Win 2 HOLDER/FB/Fb Invite 1").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN/Friends Level Win 3 HOLDER/FB/Fb Invite 1").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN/Friends Level Win 4 HOLDER/FB/Fb Invite 1").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN/Friends Level Win 5 HOLDER/FB/Fb Invite 1").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		GameObject.Find("RewardText").GetComponent<TextMesh>().text = LanguageManager.Reward;
		GameObject.Find("RewardText").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		GameObject.Find("Button_CollectReward/Text").GetComponent<TextMesh>().text = LanguageManager.Collect;
		GameObject.Find("Button_CollectReward/Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		GameObject.Find("Bonus Level").GetComponent<TextMesh>().text = LanguageManager.BonusLevel;
		GameObject.Find("Bonus Level").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		GameObject.Find("Unlock").GetComponent<TextMesh>().text = LanguageManager.Unlock;
		GameObject.Find("Unlock").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		GameObject.Find("Button_UnlockBonusYES/Text").GetComponent<TextMesh>().text = LanguageManager.Yes;
		GameObject.Find("Button_UnlockBonusYES/Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		GameObject.Find("Button_UnlockBonusNO/Text").GetComponent<TextMesh>().text = LanguageManager.No;
		GameObject.Find("Button_UnlockBonusNO/Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		GameObject.Find("Free Coins").GetComponent<TextMesh>().text = LanguageManager.FreeCoins;
		GameObject.Find("Free Coins").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		GameObject.Find("Button_WatchVideoYES/Text").GetComponent<TextMesh>().text = LanguageManager.Yes;
		GameObject.Find("Button_WatchVideoYES/Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		GameObject.Find("Button_WatchVideoNO/Text").GetComponent<TextMesh>().text = LanguageManager.No;
		GameObject.Find("Button_WatchVideoNO/Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		GameObject.Find("Button_WatchVideoNO/Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		_GUI.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup/WatchVideoText").GetComponent<TextMesh>().text = LanguageManager.WatchVideo;
		_GUI.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup/WatchVideoText").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		_GUI.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup/NotAvailableText").GetComponent<TextMesh>().text = LanguageManager.NoVideo;
		_GUI.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni/AnimationHolder/WATCH VIDEO Popup/NotAvailableText").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		GameObject.Find("ButtonFreeCoins/Text").GetComponent<TextMesh> ().text = LanguageManager.FreeCoins;
		GameObject.Find ("ButtonFreeCoins/Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		
		GameObject.Find("ButtonCustomize/Text").GetComponent<TextMesh> ().text = LanguageManager.Customize;
		GameObject.Find ("ButtonCustomize/Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		
		GameObject.Find("ButtonPowerUps/Text").GetComponent<TextMesh> ().text = LanguageManager.PowerUps;
		GameObject.Find ("ButtonPowerUps/Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		
		GameObject.Find("ButtonShop/Text").GetComponent<TextMesh> ().text = LanguageManager.Shop;
		GameObject.Find ("ButtonShop/Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		
		GameObject.Find("Shop Banana/Text/Banana").GetComponent<TextMesh> ().text = LanguageManager.Banana;
		GameObject.Find ("Shop Banana/Text/Banana").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		
		GameObject.Find("ShopFCWatchVideo/Text/Watch Video").GetComponent<TextMesh> ().text = LanguageManager.WatchVideo;
		GameObject.Find ("ShopFCWatchVideo/Text/Watch Video").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

//		GameObject.Find("ShopFCBILikePage/Text/FollowUsOnFacebook").GetComponent<TextMesh> ().text = LanguageManager.FollowUsOnFacebook;
//		GameObject.Find ("ShopFCBILikePage/Text/FollowUsOnFacebook").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		
//		GameObject.Find("ShopFCWLLikePage/Text/FollowUsOnFacebook").GetComponent<TextMesh> ().text = LanguageManager.FollowUsOnFacebook;
//		GameObject.Find ("ShopFCWLLikePage/Text/FollowUsOnFacebook").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		
//		GameObject.Find("ButtonPreview").GetComponent<TextMesh> ().text = LanguageManager.Preview;
//		GameObject.Find ("ButtonPreview").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		
		GameObject.Find("ButtonBuy").GetComponent<TextMesh> ().text = LanguageManager.Buy;
		GameObject.Find ("ButtonBuy").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		
		GameObject.Find("Shop POWERUP Double Coins/Text/ime").GetComponent<TextMesh> ().text = LanguageManager.DoubleCoins;
		GameObject.Find ("Shop POWERUP Double Coins/Text/ime").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		
		GameObject.Find("Shop POWERUP Magnet/Text/ime").GetComponent<TextMesh> ().text = LanguageManager.CoinsMagnet;
		GameObject.Find ("Shop POWERUP Magnet/Text/ime").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		
		GameObject.Find("Shop POWERUP Shield/Text/ime").GetComponent<TextMesh> ().text = LanguageManager.Shield;
		GameObject.Find ("Shop POWERUP Shield/Text/ime").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

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
					if(FacebookManager.ListaStructPrijatelja[i].PrijateljID == FacebookManager.ProfileSlikePrijatelja[j].PrijateljID)
					{
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
				if(pom.score == 0 && FacebookManager.ListaStructPrijatelja[i].PrijateljID!=FacebookManager.User) // lokalno u listi smanjiti score igraca koji nisu stigli do nekog nivoa zbog sortiranja posle, da ne izbacuje prvo invite pa usera
					pom.score = -1;
			
				//scoresAndIndexes.Add(i,FacebookManager.ListaStructPrijatelja[i].scores[level-1]);

				if(FacebookManager.ListaStructPrijatelja[i].PrijateljID==FacebookManager.User) //korisnik
				{
					int localniScore = int.Parse(StagesParser.allLevels[StagesParser.currentLevel-1].Split('#')[2]);
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

//		for(int i=0;i<rez;i++)
//		{
//
//		}
		int redosled = 1;
		bool korisnik = false;
		int pozicijaKorisnika = 1;
		Transform trenutniPrijatelj;
		for(int i=0;i<scoresAndIndexes.Count;i++) //scoresAndIndexes.Count
		{
			//Debug.Log("i = " + i + ", FACEBOOK KORISNIK: " + FacebookManager.ListaStructPrijatelja[scoresAndIndexes[i].index].PrijateljID + ", SCORE: " + scoresAndIndexes[i].score + ", slika: " + FacebookManager.ListaStructPrijatelja[scoresAndIndexes[i].index] + ", a ukupno slika: " + FacebookManager.ListaStructPrijatelja.Count);
			if(FacebookManager.ListaStructPrijatelja[scoresAndIndexes[i].index].PrijateljID==FacebookManager.User)
				pozicijaKorisnika = redosled;

			if(i<5)
			{
				if(scoresAndIndexes[i].score > 0 || FacebookManager.ListaStructPrijatelja[scoresAndIndexes[i].index].PrijateljID==FacebookManager.User)
				{
					trenutniPrijatelj = _GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN/Friends Level Win " + redosled + " HOLDER");
					trenutniPrijatelj.Find("FB").gameObject.SetActive(false);
					if(!trenutniPrijatelj.Find("Friends Level Win " + redosled).gameObject.activeSelf)
					{
						trenutniPrijatelj.Find("Friends Level Win " + redosled).gameObject.SetActive(true);
					}
					trenutniPrijatelj.Find("Friends Level Win " + redosled+"/Friends Level Win Picture " + redosled).GetComponent<Renderer>().material.mainTexture = FacebookManager.ListaStructPrijatelja[scoresAndIndexes[i].index].profilePicture;
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
						_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN/Friends Level Win " + redosled + " HOLDER/FB").gameObject.SetActive(true);
						_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN/Friends Level Win " + redosled + " HOLDER/Friends Level Win " + redosled).gameObject.SetActive(false);
					}
				}
			}
			redosled++;
		}
		if(scoresAndIndexes.Count < 5)
		{
			for(int i=redosled;i<=5;i++)
			{
				_GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN/Friends Level Win " + i + " HOLDER/Friends Level Win " + i).gameObject.SetActive(false);
			}
		}
		if(!korisnik)
		{
			trenutniPrijatelj = _GUI.Find("MISSION HOLDER/AnimationHolderGlavni/AnimationHolder/Friends FB level WIN/Friends Level Win 5 HOLDER");
			trenutniPrijatelj.Find("Friends Level Win 5/Friends Level Win Picture 5").GetComponent<Renderer>().material.mainTexture = FacebookManager.ListaStructPrijatelja[scoresAndIndexes[pozicijaKorisnika-1].index].profilePicture;
			trenutniPrijatelj.Find("Friends Level Win 5/Friends Level Win Picture 5/Points Number level win fb").GetComponent<TextMesh>().text = scoresAndIndexes[pozicijaKorisnika-1].score.ToString();
			trenutniPrijatelj.Find("Friends Level Win 5/Friends Level Win Picture 5/Position Number").GetComponent<TextMesh>().text = pozicijaKorisnika.ToString();
			trenutniPrijatelj.Find("Friends Level Win 5").GetComponent<SpriteRenderer>().sprite = trenutniPrijatelj.parent.Find("ReferencaYOU").GetComponent<SpriteRenderer>().sprite;
		}
		scoresAndIndexes.Clear();
	}

	void prebaciStrelicuNaItem()
	{
		//TutorialShop.transform.position = new Vector3(-39,-95,-75);
		TutorialShop.transform.position = new Vector3(-20,-105.5f,-75);
		//TutorialShop.transform.GetChild(0).Find("SpotLightMalaKocka1").localPosition = new Vector3(2.5f,-1.6f,0);
		TutorialShop.transform.GetChild(0).Find("SpotLightMalaKocka2").localPosition = new Vector3(-1.6f,-2.39f,0);
		//TutorialShop.transform.GetChild(0).Find("SpotLightMalaKocka3").localPosition = new Vector3();
		//TutorialShop.transform.GetChild(0).Find("SpotLightMalaKocka4").localPosition = new Vector3();
		TutorialShop.transform.GetChild(0).Find("RedArrowHolder").localPosition = new Vector3(0.5f,0.4f,-0.8f);
		TutorialShop.transform.GetChild(0).Find("RedArrowHolder").rotation = Quaternion.Euler(0,0,-43);
		TutorialShop.SetActive(true);
		TutorialShop.transform.GetChild(0).GetComponent<Animation>().Play();
		TutorialShop.transform.GetChild(0).Find("RedArrowHolder/RedArrow").GetComponent<Animation>().Play();
	}
	void spustiPopup()
	{
		popupZaSpustanje.localPosition += new Vector3(0,-35,0);
		popupZaSpustanje = null; //izbacio je ovde null reference exception, treba na nekom mestu da se proveri da li je popup razlicit od null
	}

	IEnumerator checkConnectionForLoginButton()
	{
		StartCoroutine(CheckInternetConnection.Instance.checkInternetConnection());
		while(!CheckInternetConnection.Instance.checkDone)
		{
			yield return null;
		}
		if(CheckInternetConnection.Instance.internetOK)
		{
			FacebookManager.KorisnikoviPodaciSpremni=false;
			ShopManagerFull.ShopInicijalizovan=false;
			if(PlaySounds.soundOn)
				PlaySounds.Play_Button_OpenLevel();
			if(!FB.IsLoggedIn)
			{
				FacebookManager.MestoPozivanjaLogina = 3;
				FacebookManager.FacebookObject.FacebookLogin();
			}
		}
		else
		{
			CheckInternetConnection.Instance.openPopup();
		}
	}

	IEnumerator checkConnectionForPageLike(string url, string key)
	{
		StartCoroutine(CheckInternetConnection.Instance.checkInternetConnection());
		while(!CheckInternetConnection.Instance.checkDone)
		{
			yield return null;
		}
		if(CheckInternetConnection.Instance.internetOK)
		{
			//StartCoroutine(FacebookManager.FacebookObject.otvaranjeStranice(url, key));
		}
		else
		{
			CheckInternetConnection.Instance.openPopup();
		}
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
//				//AdsScript.sceneID = 1;
//				AdsScript.reward = StagesParser.watchVideoReward;
//				AdColony.ShowVideoAd();
//			}
//			else
//			{
//				CheckInternetConnection.Instance.NoVideosAvailable_OpenPopup();
//			}
			//@@@@@@@@@@@@@
			StagesParser.sceneID = 1;
			//WebelinxCMS.Instance.IsActionLoaded("watch_video");
			if(Advertisement.IsReady()) 
			{
				Advertisement.Show(null, new ShowOptions {
					resultCallback = result => {
						Debug.Log(result.ToString());
						if(result.ToString().Equals("Finished"))
						{
							if(StagesParser.sceneID == 0)
							{
								Debug.Log("ovde li sam");
								StagesParser.currentMoney += StagesParser.watchVideoReward;
								PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
								PlayerPrefs.Save();
								StartCoroutine(StagesParser.Instance.moneyCounter(StagesParser.watchVideoReward, ShopManagerFull.ShopObject.transform.Find("Shop Interface/Coins/Coins Number").GetComponent<TextMesh>(), true));
							}
							else if(StagesParser.sceneID == 1)
							{
								Camera.main.SendMessage("WatchVideoCallback",1,SendMessageOptions.DontRequireReceiver);
								
							}
							else if(StagesParser.sceneID == 2)
							{
								GameObject.Find("_GameManager").SendMessage("WatchVideoCallback",SendMessageOptions.DontRequireReceiver);
							}
							StagesParser.ServerUpdate = 1;
						}
					}
				});
			}
			else
			{
				CheckInternetConnection.Instance.NoVideosAvailable_OpenPopup();
			}
		}
		else
		{
			CheckInternetConnection.Instance.openPopup();
		}
	}
	IEnumerator checkConnectionForInviteFriend()
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

	IEnumerator checkConnectionForTelevizor()
	{
		StartCoroutine(CheckInternetConnection.Instance.checkInternetConnection());
		while(!CheckInternetConnection.Instance.checkDone)
		{
			yield return null;
		}
		if(CheckInternetConnection.Instance.internetOK)
		{
			popupZaSpustanje = _GUI.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni");
			Invoke("spustiPopup",0.5f);
			_GUI.Find("WATCH VIDEO HOLDER/AnimationHolderGlavni").GetComponent<Animator>().Play("ClosePopup");
			makniPopup = 0;
			bool match = false;
			string prefsValue = PlayerPrefs.GetString("WatchVideoWorld"+(StagesParser.currSetIndex+1));
			//monkeyCurrentLevelIndex = monkeyDestinationLevelIndex;
			if(PlayerPrefs.HasKey("WatchVideoWorld"+(StagesParser.currSetIndex+1)))
			{
				string[] values2 = prefsValue.Split('#');
				for(int i=0;i<values2.Length;i++)
				{
					if(int.Parse(values2[i]) == trenutniTelevizor)
					{
						match = true;
					}
				}
				if(!match)
				{
					prefsValue += "#" + trenutniTelevizor;
					PlayerPrefs.SetString("WatchVideoWorld"+(StagesParser.currSetIndex+1),prefsValue);
					PlayerPrefs.Save();
				}
				Televizori[trenutniTelevizor-1].gameObject.SetActive(false);
			}
			else
			{
				Televizori[trenutniTelevizor-1].gameObject.SetActive(false);
				PlayerPrefs.SetString("WatchVideoWorld"+(StagesParser.currSetIndex+1),trenutniTelevizor.ToString());
				PlayerPrefs.Save();
			}
			if(!televizorIzabrao)
			{
				animator.Play("Running");
				StartCoroutine("KretanjeMajmunceta");
			}
			else
			{
				televizorIzabrao = false;
			}
//			AdsScript.Instance.ShowAd();

			//@@@@@@@@@@@@@ SKLANJA SE ADCOLONY
//			if(AdColony.IsVideoAvailable(AdsScript.zoneID1))
//			{
//				//AdsScript.sceneID = 1;				
//				AdsScript.reward = televizorCenePoSvetovima[StagesParser.currSetIndex];
//				AdColony.ShowVideoAd();
//			}
//			else
//			{
//				CheckInternetConnection.Instance.NoVideosAvailable_OpenPopup();
//			}
			//@@@@@@@@@@@@@
			StagesParser.sceneID = 1;
			//WebelinxCMS.Instance.IsActionLoaded("watch_video");

//			StagesParser.currentMoney += televizorCenePoSvetovima[StagesParser.currSetIndex];
//			PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
//			PlayerPrefs.Save();
//			StartCoroutine(StagesParser.Instance.moneyCounter(televizorCenePoSvetovima[StagesParser.currSetIndex],_GUI.Find("INTERFACE HOLDER/_TopLeft/Coins/Coins Number").GetComponent<TextMesh>(),true));
		}
		else
		{
			CheckInternetConnection.Instance.openPopup();
		}
	}

	public void WatchVideoCallback(int value)
	{
		if(value == 1) //finished
		{
			if(makniPopup == 0) //watch video popup
			{
				StagesParser.currentMoney += televizorCenePoSvetovima[StagesParser.currSetIndex];
				PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
				PlayerPrefs.Save();
				StartCoroutine(StagesParser.Instance.moneyCounter(televizorCenePoSvetovima[StagesParser.currSetIndex],_GUI.Find("INTERFACE HOLDER/_TopLeft/Coins/Coins Number").GetComponent<TextMesh>(),true));
				StagesParser.ServerUpdate = 1;
			}
			else if(makniPopup == 8) //free coins
			{
				StagesParser.currentMoney += StagesParser.watchVideoReward;
				PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
				PlayerPrefs.Save();
				StartCoroutine(StagesParser.Instance.moneyCounter(StagesParser.watchVideoReward, ShopManagerFull.ShopObject.transform.Find("Shop Interface/Coins/Coins Number").GetComponent<TextMesh>(), true));
			}
		}
		else if(value == 2) //skipped
		{

		}
		else if(value == 3) //failed
		{
			CheckInternetConnection.Instance.NoVideosAvailable_OpenPopup();
		}
	}
}
