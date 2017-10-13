using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class MainScene : MonoBehaviour {

	public Transform[] FriendsObjects = new Transform[0];
	public Transform[] SettingsObjects = new Transform[0];
	public Transform[] LanguagesObjects = new Transform[0];
	public static GameObject LeaderBoardInvite, FacebookLogIn;
	bool SettingsOtvoren=false, LeaderboardOtvoren=false;
	int SettingState = 1; // 1- glavni settings, 2 - languages , 3-languages bez back-a
	string releasedItem;
	string clickedItem;
	Vector3 originalScale;
	GameObject temp;
	System.Globalization.DateTimeFormatInfo format;
	int selectedLanguage = 0;
	int dailyReward = 0;
	string jezikPreUlaskaUPromenuJezika;
	bool logoutKliknut = false; //@@@@@@ DODATAK U 1.3
	// Use this for initialization
	void Start () {
		//PlayerPrefs.DeleteKey("Logovan");
	
		if(Advertisement.isSupported)
		{
			Advertisement.Initialize(StagesParser.Instance.UnityAdsVideoGameID);
		}
		else
		{
			Debug.Log("UNITYADS Platform not supported");
		}
		StartCoroutine(checkConnectionForAutologin());

	
		if(Loading.Instance != null)
			StartCoroutine(Loading.Instance.UcitanaScena(Camera.main,4,0.25f));

		LeaderBoardInvite = GameObject.Find ("Leaderboard Scena/FB Invite");
		LeaderBoardInvite.transform.Find("Text/Number").GetComponent<TextMesh>().text = "+" + StagesParser.InviteReward;
		LeaderBoardInvite.transform.Find("Text/Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,false);

		//ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCBILikePage/Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text = StagesParser.likePageReward.ToString();
		ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCWatchVideo/Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text = StagesParser.watchVideoReward.ToString();
		//ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCWLLikePage/Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text = StagesParser.likePageReward.ToString();
		//ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCBILikePage/Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,false);
		ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCWatchVideo/Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,false);
		//ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCWLLikePage/Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,false);

		FacebookLogIn = GameObject.Find ("FB HOLDER LogIn");
		GameObject.Find("Gore Levo HOLDER Buttons").transform.position=new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x, Camera.main.ViewportToWorldPoint(Vector3.one).y, 0 );
		GameObject.Find("Dole Desno HOLDER Buttons").transform.position=new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one).x, Camera.main.ViewportToWorldPoint(Vector3.zero).y, 0 );
		GameObject.Find("Dole Levo HOLDER Buttons").transform.position=new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x, Camera.main.ViewportToWorldPoint(Vector3.zero).y, 0 );
		if(PlayerPrefs.HasKey("JezikPromenjen"))
		{
			switch(LanguageManager.chosenLanguage)
			{
			case "_en":
				PromeniZastavu(1);
				break;
			case "_us":
				PromeniZastavu(2);
				break;
			case "_es":
				PromeniZastavu(3);
				break;
			case "_ru":
				PromeniZastavu(4);
				break;
			case "_pt":
				PromeniZastavu(5);
				break;
			case "_br":
				PromeniZastavu(6);
				break;
			case "_fr":
				PromeniZastavu(7);
				break;
			case "_th":
				PromeniZastavu(8);
				break;
			case "_ch":
				PromeniZastavu(9);
				break;
			case "_tch":
				PromeniZastavu(10);
				break;
			case "_de":
				PromeniZastavu(11);
				break;
			case "_it":
				PromeniZastavu(12);
				break;
			case "_srb":
				PromeniZastavu(13);
				break;
			case "_tr":
				PromeniZastavu(14);
				break;
			case "_ko":
				PromeniZastavu(15);
				break;
			default:
				PromeniZastavu(0);
				break;
			}
		}

		if(PlaySounds.soundOn)
		{
			SettingsObjects[2].FindChild("Shop Tab Element Selected").GetComponent<SpriteRenderer>().enabled=false;
		}
		else
		{
			SettingsObjects[2].FindChild("Shop Tab Element Selected").GetComponent<SpriteRenderer>().enabled=true;
		}

		if(PlaySounds.musicOn)
		{
			SettingsObjects[1].FindChild("Shop Tab Element Selected").GetComponent<SpriteRenderer>().enabled=false;
			PlaySounds.Play_BackgroundMusic_Menu();
		}
		else
		{
			SettingsObjects[1].FindChild("Shop Tab Element Selected").GetComponent<SpriteRenderer>().enabled=true;
		}

		ShopManagerFull.AktivanTab=0;
		if(FB.IsLoggedIn)
		{
			GameObject.Find("FB HOLDER LogIn").SetActive(false);
			LeaderBoardInvite.SetActive(true);
			//FacebookManager.FacebookObject.ProveriKorisnika();
		}
		else
		{
			LeaderBoardInvite.SetActive(false);
			GameObject.Find("FB HOLDER LogIn").SetActive(true);
			for(int i=0;i<10;i++)
			{
				if(i==1)
				{
					FriendsObjects[i].FindChild("FB Invite").gameObject.SetActive(true);
					FriendsObjects[i].FindChild("Friend").gameObject.SetActive(false);
					FriendsObjects[i].FindChild("FB Invite/Coin Shop").gameObject.SetActive(false);
					FriendsObjects[i].FindChild("FB Invite/Invite").GetComponent<TextMesh>().text=LanguageManager.LogIn;
					FriendsObjects[i].FindChild("FB Invite/Invite").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
					FriendsObjects[i].FindChild("FB Invite/Coin Number").GetComponent<TextMesh>().text = "+" + StagesParser.LoginReward;
					FriendsObjects[i].FindChild("FB Invite/Coin Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,false);
				}
				else
				{
					FriendsObjects[i].gameObject.SetActive(false);
				}
			}
		}


		if(!PlayerPrefs.HasKey("VecPokrenuto"))
		{
			GameObject.Find("FB HOLDER LogIn").SetActive(false);
			GameObject.Find("Dole Desno HOLDER Buttons").SetActive(false);
			GameObject.Find("Dole Levo HOLDER Buttons").SetActive(false);
			GameObject.Find("Zastava").GetComponent<Collider>().enabled = false;
		}

		if(StagesParser.obucenSeLogovaoNaDrugojSceni)
		{
			StagesParser.obucenSeLogovaoNaDrugojSceni = false;
			StagesParser.Instance.ShopDeoIzCompareScores();
		}
	}

	IEnumerator checkConnectionForAutologin()
	{
		StartCoroutine(CheckInternetConnection.Instance.checkInternetConnection());
		while(!CheckInternetConnection.Instance.checkDone)
		{
			yield return null;
		}
		if(CheckInternetConnection.Instance.internetOK)
		{
			if(PlayerPrefs.HasKey("Logovan")) //neko se logovao ranije
			{
				if(PlayerPrefs.GetInt("Logovan") == 1)
				{
					if(!FacebookManager.Ulogovan)
					{
						//PlayerPrefs.SetInt("Logout",1);
						FacebookManager.FacebookObject.FacebookLogin();
					}
					else
					{
						FacebookManager.MestoPozivanjaLogina = 1;
						FacebookManager.FacebookObject.BrojPrijatelja = 0;
						FacebookManager.FacebookObject.Korisnici.Clear();
						FacebookManager.FacebookObject.Scorovi.Clear();
						FacebookManager.FacebookObject.Imena.Clear();
						FacebookManager.ProfileSlikePrijatelja.Clear();
						FacebookManager.ListaStructPrijatelja.Clear();
						
						FacebookManager.FacebookObject.GetFacebookFriendScores();
					}
				}
				else
				{
					if(FB.IsLoggedIn) // situacija kada iz nekog razloga nije kompletirao login (pukla aplikacija npr.) i nije postavio prefs "Logovan", ali prijavljuje da jeste logovan
					{
						if(!FacebookManager.Ulogovan)
						{
							//PlayerPrefs.SetInt("Logout",1);
							FacebookManager.FacebookObject.FacebookLogin();
						}
						else
						{
							FacebookManager.MestoPozivanjaLogina = 1;
							FacebookManager.FacebookObject.BrojPrijatelja = 0;
							FacebookManager.FacebookObject.Korisnici.Clear();
							FacebookManager.FacebookObject.Scorovi.Clear();
							FacebookManager.FacebookObject.Imena.Clear();
							FacebookManager.ProfileSlikePrijatelja.Clear();
							FacebookManager.ListaStructPrijatelja.Clear();
							
							FacebookManager.FacebookObject.GetFacebookFriendScores();
						}
					}
				}
			}
		}
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
			if(!FB.IsLoggedIn)
			{
				FacebookManager.MestoPozivanjaLogina=1;
				FacebookManager.FacebookObject.FacebookLogin();
			}
		}
		else
		{
			CheckInternetConnection.Instance.openPopup();
		}
	}

	IEnumerator checkConnectionForLeaderboardLogin()
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
			//   if(AdColony.IsVideoAvailable(AdsScript.zoneID1))
			//   {
			//    AdsScript.sceneID = 0;
			//    AdsScript.reward = StagesParser.watchVideoReward;
			//    AdColony.ShowVideoAd();
			//   }
			//   else
			//   {
			//    CheckInternetConnection.Instance.NoVideosAvailable_OpenPopup();
			//   }
			//@@@@@@@@@@@@@
			StagesParser.sceneID = 0;
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
	IEnumerator checkConnectionForLogout()
	{
		StartCoroutine(CheckInternetConnection.Instance.checkInternetConnection());
		while(!CheckInternetConnection.Instance.checkDone)
		{
			yield return null;
		}
		if(CheckInternetConnection.Instance.internetOK)
		{
			logoutKliknut = true; //@@@@@@ DODATAK U 1.3
			SettingsObjects[5].GetComponent<Collider>().enabled = false;
			SettingsObjects[5].Find("Shop Tab Element Selected").GetComponent<SpriteRenderer>().enabled = true;
			
			if(FB.IsLoggedIn)
			{
				FacebookManager.FacebookObject.scoreToSet = StagesParser.currentPoints;
				FacebookManager.FacebookObject.proveraPublish_ActionPermisije();
				StagesParser.ServerUpdate = 3;
				FacebookManager.FacebookObject.SacuvajScoreNaNivoima(StagesParser.PointsPoNivoima,StagesParser.StarsPoNivoima, StagesParser.maxLevel,StagesParser.bonusLevels);
				FacebookManager.FacebookObject.UpdateujPodatkeKorisnika(StagesParser.currentMoney,StagesParser.currentPoints,LanguageManager.chosenLanguage,StagesParser.currentBananas,StagesParser.powerup_magnets,StagesParser.powerup_shields,StagesParser.powerup_doublecoins,StagesParser.svekupovineGlava,StagesParser.svekupovineMajica,StagesParser.svekupovineLedja,StagesParser.ledja,StagesParser.glava,StagesParser.majica,StagesParser.imaUsi,StagesParser.imaKosu,FacebookManager.NumberOfFriends);
			}
			
			Transform loading = GameObject.Find("Loading Buffer HOLDER").transform;
			Transform camera = Camera.main.transform;
			loading.position = new Vector3(camera.position.x,camera.position.y,loading.position.z);
			loading.GetChild(0).gameObject.SetActive(true);
			loading.GetChild(0).GetComponent<Animator>().Play("LoadingBufferUlazAnimation");
			
			OcistiLeaderboard();
			DeaktivirajLeaderboard();
			Transform leaderboardLogin = LeaderBoardInvite.transform.parent.Find("Friends Tabs/Friend No 2");
			leaderboardLogin.localPosition = new Vector3(leaderboardLogin.localPosition.x, -1.85f, leaderboardLogin.localPosition.z);
			StartCoroutine(DoLogout());
			MainScene.FacebookLogIn.SetActive(true);
			MainScene.LeaderBoardInvite.SetActive(false);
			for(int i=0;i<10;i++)
			{
				if(i==1)
				{
					FriendsObjects[i].FindChild("FB Invite").gameObject.SetActive(true);
					FriendsObjects[i].FindChild("Friend").gameObject.SetActive(false);
					FriendsObjects[i].FindChild("FB Invite/Coin Shop").gameObject.SetActive(false);
					FriendsObjects[i].FindChild("FB Invite/Invite").GetComponent<TextMesh>().text=LanguageManager.LogIn;
					FriendsObjects[i].FindChild("FB Invite/Invite").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
				}
				else
				{
					FriendsObjects[i].gameObject.SetActive(false);
				}
			}
		}
		else
		{
			CheckInternetConnection.Instance.openPopup();
		}
	}

	IEnumerator checkConnectionForResetProgress()
	{
		StartCoroutine(CheckInternetConnection.Instance.checkInternetConnection());
		while(!CheckInternetConnection.Instance.checkDone)
		{
			yield return null;
		}
		if(CheckInternetConnection.Instance.internetOK)
		{
			StagesParser.ServerUpdate = 1;
			FacebookManager.FacebookObject.resetovanScoreNaNulu = 2;

			FacebookManager.FacebookObject.scoreToSet = 0;
			FacebookManager.FacebookObject.proveraPublish_ActionPermisije();
			StartCoroutine(SacekajDaSePostaviScoreNaNulu());
		}
		else
		{
			CheckInternetConnection.Instance.openPopup();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			#if UNITY_ANDROID
			if(!ShopManagerFull.otvorenShop && !SettingsOtvoren && !LeaderboardOtvoren)
			{
				if(StagesParser.ServerUpdate == 1 && FB.IsLoggedIn)
				{
					FacebookManager.FacebookObject.scoreToSet = StagesParser.currentPoints;
					FacebookManager.FacebookObject.proveraPublish_ActionPermisije();
					FacebookManager.FacebookObject.SacuvajScoreNaNivoima(StagesParser.PointsPoNivoima,StagesParser.StarsPoNivoima, StagesParser.maxLevel,StagesParser.bonusLevels);
					FacebookManager.FacebookObject.UpdateujPodatkeKorisnika(StagesParser.currentMoney,StagesParser.currentPoints,LanguageManager.chosenLanguage,StagesParser.currentBananas,StagesParser.powerup_magnets,StagesParser.powerup_shields,StagesParser.powerup_doublecoins,StagesParser.svekupovineGlava,StagesParser.svekupovineMajica,StagesParser.svekupovineLedja,StagesParser.ledja,StagesParser.glava,StagesParser.majica,StagesParser.imaUsi,StagesParser.imaKosu,FacebookManager.NumberOfFriends);
				}

				//@@@@@@@@
				Application.Quit();

			}
			else if(SettingsOtvoren)
			{
				if(SettingState==1)
				{
					if(!GameObject.Find("Loading Buffer HOLDER").transform.GetChild(0).gameObject.activeSelf) //@@@@@@ DODATAK U 1.3
					{
						GameObject.Find("Settings i Language Scena").GetComponent<Animation>().Play("SettingsOdlazak");
						SettingsOtvoren=false;
						Invoke ("DeaktivirajSettings",1f);
						ProveraZaLogoutZbogDugmica(); //@@@@@@ DODATAK U 1.3
					}
				}
				else if(SettingState==2)
				{
					SettingState=1; 
					GameObject.Find ("Settings i Language Scena/Settings Tabs").GetComponent<Animation> ().Play ("TabSettingsDolazak");
					GameObject.Find ("Settings i Language Scena/Language Tabs").GetComponent<Animation> ().Play ("TabSettingsOdlazak");
					AktivirajSettings();
					if(LanguageManager.chosenLanguage != jezikPreUlaskaUPromenuJezika)
					{
						jezikPreUlaskaUPromenuJezika = LanguageManager.chosenLanguage;
						ShopManagerFull.ShopObject.RefresujImenaItema();
					}
				}
				else if(SettingState==3)
				{
					SettingState=1; 
					SettingsOtvoren=false;
					GameObject.Find("Settings i Language Scena").GetComponent<Animation>().Play("SettingsOdlazak");
					GameObject.Find ("Settings i Language Scena/Language Tabs").GetComponent<Animation> ().Play ("TabSettingsOdlazak");
					if(LanguageManager.chosenLanguage != jezikPreUlaskaUPromenuJezika)
					{
						jezikPreUlaskaUPromenuJezika = LanguageManager.chosenLanguage;
						ShopManagerFull.ShopObject.RefresujImenaItema();
					}
				}
			}
			else if(ShopManagerFull.otvorenShop)
			{
				ShopManagerFull.ShopObject.SkloniShop();
			}
			else if(LeaderboardOtvoren)
			{
				LeaderboardOtvoren=false;
				GameObject.Find("Leaderboard Scena").GetComponent<Animation>().Play("MeniOdlazak");
				Invoke ("DeaktivirajLeaderboard",1f);
			}

			#endif
		}
		
		if(Input.GetMouseButtonDown(0))
		{
			clickedItem = RaycastFunction(Input.mousePosition);
			
			if(clickedItem.Equals("NekoDugme") || clickedItem.Equals("ButtonSettings") || clickedItem.Equals("ButtonLeaderboard") || clickedItem.Equals("Custumization") || clickedItem.Equals("FreeCoins")
			   || clickedItem.Equals("4 Reset Progres") || clickedItem.Equals("5 Reset Tutorials") || clickedItem.Equals("6 Log Out") || clickedItem.Equals("Button_CheckOK")) 
			{
				temp = GameObject.Find(clickedItem);
				originalScale = temp.transform.localScale;
				temp.transform.localScale = originalScale * 1.2f;
			}
			else if(clickedItem.Equals("ClearAll"))
			{
				ShopManagerFull.ShopObject.transform.Find("3 Customize/Costumization BG/ClearAll/ClearAll_Selected").GetComponent<SpriteRenderer>().enabled = true;
			}
			else if(clickedItem != System.String.Empty)
			{
				temp = GameObject.Find(clickedItem);
				originalScale = temp.transform.localScale;
			}
		}

		if(Input.GetMouseButtonUp(0))
		{
				releasedItem = RaycastFunction(Input.mousePosition);
				if(!clickedItem.Equals(System.String.Empty))
				{
					if(temp != null)
						temp.transform.localScale = originalScale;

				if(ShopManagerFull.ShopObject.transform.Find("3 Customize/Costumization BG/ClearAll/ClearAll_Selected").GetComponent<SpriteRenderer>().enabled)
					ShopManagerFull.ShopObject.transform.Find("3 Customize/Costumization BG/ClearAll/ClearAll_Selected").GetComponent<SpriteRenderer>().enabled = false;

				if(clickedItem==releasedItem && releasedItem == "NekoDugme")
				{
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				if(clickedItem==releasedItem && releasedItem == "Play")
				{
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
					format =System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat;
					string VremeQuitString = System.DateTime.Now.ToString(format.FullDateTimePattern);
					PlayerPrefs.SetString("VremeQuit", VremeQuitString);
					PlayerPrefs.SetFloat("VremeBrojaca", TimeReward.VremeBrojaca);
					PlayerPrefs.Save();

					GameObject.Find("Loading Animation Vrata").GetComponent<Animator>().Play("Loading Zidovi Dolazak");

					StartCoroutine(otvoriSledeciNivo());
				}

				if(clickedItem==releasedItem && releasedItem== "ButtonSettings")
				{
					GameObject.Find("Settings i Language Scena").GetComponent<Animation>().Play("SettingsDolazak");

					PrikaziSettings();
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "ButtonFacebook")
				{
					FacebookManager.KorisnikoviPodaciSpremni=false;
					ShopManagerFull.ShopInicijalizovan=false;
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();

					StartCoroutine(checkConnectionForLoginButton());

				}

				else if(clickedItem==releasedItem && releasedItem == "ButtonMusic")
				{
					if(!PlaySounds.musicOn)
					{
						PlaySounds.musicOn = true;
						if(PlaySounds.soundOn)
							PlaySounds.Play_Button_MusicOn();
	//					GameObject.Find("ButtonMusic").GetComponent<SpriteRenderer>().sprite = GameObject.Find("dugmeMuzikaSprite").GetComponent<SpriteRenderer>().sprite;
						PlaySounds.Play_BackgroundMusic_Menu();
						PlayerPrefs.SetInt("musicOn",1);
						PlayerPrefs.Save();
					}
					else
					{
						PlaySounds.musicOn = false;
	//					GameObject.Find("ButtonMusic").GetComponent<SpriteRenderer>().sprite = GameObject.Find("dugmeMuzikaOffSprite").GetComponent<SpriteRenderer>().sprite;
						PlaySounds.Stop_BackgroundMusic_Menu();
						PlayerPrefs.SetInt("musicOn",0);
						PlayerPrefs.Save();
					}
				}

				else if(clickedItem==releasedItem && releasedItem == "ButtonSound")
				{
					if(!PlaySounds.soundOn)
					{
						PlaySounds.soundOn = true;
						PlaySounds.Play_Button_SoundOn();
	//					GameObject.Find("ButtonSound").GetComponent<SpriteRenderer>().sprite = GameObject.Find("dugmeSoundOffSprite").GetComponent<SpriteRenderer>().sprite;
						PlaySounds.Play_Button_SoundOn();
						PlayerPrefs.SetInt("soundOn",1);
						PlayerPrefs.Save();
					}
					else
					{
						PlaySounds.soundOn = false;
	//					GameObject.Find("ButtonSound").GetComponent<SpriteRenderer>().sprite = GameObject.Find("dugmeSoundOffSprite").GetComponent<SpriteRenderer>().sprite;
						PlayerPrefs.SetInt("soundOn",0);
						PlayerPrefs.Save();
					}
				}

				else if(clickedItem==releasedItem && releasedItem == "Zastava")
				{
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
					GameObject.Find("Settings i Language Scena").GetComponent<Animation>().Play("SettingsDolazak");
					PrikaziJezike();
					SettingsOtvoren=true;
					SettingState=3;
				}

				else if(clickedItem==releasedItem && releasedItem == "ButtonLeaderboard")
				{
					GameObject.Find("Leaderboard Scena").GetComponent<Animation>().Play("MeniDolazak");
					LeaderboardOtvoren=true;
					if(FB.IsLoggedIn)
					{
						Invoke ("AktivirajLeaderboard",1f);
					}

					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();

				}

				else if(clickedItem==releasedItem && releasedItem == "FreeCoins")
				{
					GameObject.Find("Shop").GetComponent<Animation>().Play("MeniDolazak");
					
					if(ShopManagerFull.AktivanRanac == 0)
						GameObject.Find("MonkeyHolder").transform.Find("PrinceGorilla/ROOT/Hip/Spine/"+ShopManagerFull.AktivanRanac).transform.GetChild(0).GetComponent<MeshFilter>().mesh = GameObject.Find("RefZaShop_AndjeoskaKrila").GetComponent<MeshFilter>().mesh;
					else if(ShopManagerFull.AktivanRanac == 5)
						GameObject.Find("MonkeyHolder").transform.Find("PrinceGorilla/ROOT/Hip/Spine/"+ShopManagerFull.AktivanRanac).transform.GetChild(0).GetComponent<MeshFilter>().mesh = GameObject.Find("RefZaShop_SlepiMisKrila").GetComponent<MeshFilter>().mesh;

					ShopManagerFull.ShopObject.PozoviTab(1);

					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "Custumization")
				{
					GameObject.Find("Shop").GetComponent<Animation>().Play("MeniDolazak");
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

					if(ShopManagerFull.AktivanRanac == 0)
						GameObject.Find("MonkeyHolder").transform.Find("PrinceGorilla/ROOT/Hip/Spine/"+ShopManagerFull.AktivanRanac).transform.GetChild(0).GetComponent<MeshFilter>().mesh = GameObject.Find("RefZaShop_AndjeoskaKrila").GetComponent<MeshFilter>().mesh;
					else if(ShopManagerFull.AktivanRanac == 5)
						GameObject.Find("MonkeyHolder").transform.Find("PrinceGorilla/ROOT/Hip/Spine/"+ShopManagerFull.AktivanRanac).transform.GetChild(0).GetComponent<MeshFilter>().mesh = GameObject.Find("RefZaShop_SlepiMisKrila").GetComponent<MeshFilter>().mesh;

					ShopManagerFull.ShopObject.PozoviTab(3);
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}
				
				else if(clickedItem==releasedItem && releasedItem == "ButtonFreeCoins")
				{
					ShopManagerFull.ShopObject.PozoviTab(1);
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}
				
				else if(clickedItem==releasedItem && releasedItem == "ButtonShop")
				{

					ShopManagerFull.ShopObject.PozoviTab(2);
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "ButtonCustomize")
				{

					ShopManagerFull.ShopObject.PozoviTab(3);

					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "ButtonPowerUps")
				{
					ShopManagerFull.ShopObject.PozoviTab(4);
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "ButtonBackShop")
				{
					ShopManagerFull.ShopObject.SkloniShop();
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "BackButtonLeaderboard")
				{
					LeaderboardOtvoren=false;
					GameObject.Find("Leaderboard Scena").GetComponent<Animation>().Play("MeniOdlazak");
					Invoke ("DeaktivirajLeaderboard",1f);
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "BackButtonSettings")
				{
					if(SettingState==1)
					{
						GameObject.Find("Settings i Language Scena").GetComponent<Animation>().Play("SettingsOdlazak");
						SettingsOtvoren=false;
						Invoke ("DeaktivirajSettings",1f);
						ProveraZaLogoutZbogDugmica(); //@@@@@@ DODATAK U 1.3
					}
					else if(SettingState==2)
					{
						SettingState=1; 
						GameObject.Find ("Settings i Language Scena/Settings Tabs").GetComponent<Animation> ().Play ("TabSettingsDolazak");
						GameObject.Find ("Settings i Language Scena/Language Tabs").GetComponent<Animation> ().Play ("TabSettingsOdlazak");
						AktivirajSettings();
						if(LanguageManager.chosenLanguage != jezikPreUlaskaUPromenuJezika)
						{
							jezikPreUlaskaUPromenuJezika = LanguageManager.chosenLanguage;
							ShopManagerFull.ShopObject.RefresujImenaItema();
						}

					}
					else if(SettingState==3)
					{
						SettingState=1; 
						SettingsOtvoren=false;
						GameObject.Find("Settings i Language Scena").GetComponent<Animation>().Play("SettingsOdlazak");
						GameObject.Find ("Settings i Language Scena/Language Tabs").GetComponent<Animation> ().Play ("TabSettingsOdlazak");
						if(LanguageManager.chosenLanguage != jezikPreUlaskaUPromenuJezika)
						{
							jezikPreUlaskaUPromenuJezika = LanguageManager.chosenLanguage;
							ShopManagerFull.ShopObject.RefresujImenaItema();
						}
					}

					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "ButtonCollect")
				{
					PlayerPrefs.SetInt("ProveriVreme",1);
					PlayerPrefs.Save();
					//GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyOdlazak");
					switch(DailyRewards.LevelReward)
					{
					case 1:
						GameObject temp0 = GameObject.Find("CoinsReward");
						temp0.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
						temp0.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						StagesParser.currentMoney+=DailyRewards.DailyRewardAmount[0];
						PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
						PlayerPrefs.Save();
						dailyReward = DailyRewards.DailyRewardAmount[0];
						GameObject.Find("Day 1").transform.GetChild(0).Find("CollectCoinsDailyRewardParticles").GetComponent<ParticleSystem>().Play();
						GameObject.Find("Day 1").GetComponent<Collider>().enabled = false;
						GameObject.Find("ButtonCollect").GetComponent<Collider>().enabled = false;
						//GameObject temp0 = GameObject.Find("CoinsReward");
						//temp0.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
						//temp0.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						temp0.GetComponent<Animation>().Play("CoinsRewardDolazak");
						Invoke("DelayZaOdbrojavanje",1.15f);
						break;
					case 2:
						GameObject temp1 = GameObject.Find("CoinsReward");
						temp1.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
						temp1.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						StagesParser.currentMoney+=DailyRewards.DailyRewardAmount[1];
						PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
						PlayerPrefs.Save();
						dailyReward = DailyRewards.DailyRewardAmount[1];
						GameObject.Find("Day 2").transform.GetChild(0).Find("CollectCoinsDailyRewardParticles").GetComponent<ParticleSystem>().Play();
						GameObject.Find("Day 2").GetComponent<Collider>().enabled = false;
						GameObject.Find("ButtonCollect").GetComponent<Collider>().enabled = false;
						//GameObject temp1 = GameObject.Find("CoinsReward");
						//temp1.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
						//temp1.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						temp1.GetComponent<Animation>().Play("CoinsRewardDolazak");
						Invoke("DelayZaOdbrojavanje",1.15f);
						break;
					case 3:
						GameObject temp2 = GameObject.Find("CoinsReward");
						temp2.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
						temp2.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						StagesParser.currentMoney+=DailyRewards.DailyRewardAmount[2];
						PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
						PlayerPrefs.Save();
						dailyReward = DailyRewards.DailyRewardAmount[2];
						GameObject.Find("Day 3").transform.GetChild(0).Find("CollectCoinsDailyRewardParticles").GetComponent<ParticleSystem>().Play();
						GameObject.Find("Day 3").GetComponent<Collider>().enabled = false;
						GameObject.Find("ButtonCollect").GetComponent<Collider>().enabled = false;
						//GameObject temp2 = GameObject.Find("CoinsReward");
						//temp2.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
						//temp2.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						temp2.GetComponent<Animation>().Play("CoinsRewardDolazak");
						Invoke("DelayZaOdbrojavanje",1.15f);
						break;
					case 4:
						GameObject temp3 = GameObject.Find("CoinsReward");
						temp3.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
						temp3.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						StagesParser.currentMoney+=DailyRewards.DailyRewardAmount[3];
						PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
						PlayerPrefs.Save();
						dailyReward = DailyRewards.DailyRewardAmount[3];
						GameObject.Find("Day 4").transform.GetChild(0).Find("CollectCoinsDailyRewardParticles").GetComponent<ParticleSystem>().Play();
						GameObject.Find("Day 4").GetComponent<Collider>().enabled = false;
						GameObject.Find("ButtonCollect").GetComponent<Collider>().enabled = false;
						//GameObject temp3 = GameObject.Find("CoinsReward");
						//temp3.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
						//temp3.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						temp3.GetComponent<Animation>().Play("CoinsRewardDolazak");
						Invoke("DelayZaOdbrojavanje",1.15f);
						break;
					case 5:
						GameObject temp4 = GameObject.Find("CoinsReward");
						temp4.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
						temp4.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						StagesParser.currentMoney+=DailyRewards.DailyRewardAmount[4];
						PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
						PlayerPrefs.Save();
						dailyReward = DailyRewards.DailyRewardAmount[4];
						GameObject.Find("Day 5").transform.GetChild(0).Find("CollectCoinsDailyRewardParticles").GetComponent<ParticleSystem>().Play();
						GameObject.Find("Day 5").GetComponent<Collider>().enabled = false;
						GameObject.Find("ButtonCollect").GetComponent<Collider>().enabled = false;
						//GameObject temp4 = GameObject.Find("CoinsReward");
						//temp4.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
						//temp4.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						temp4.GetComponent<Animation>().Play("CoinsRewardDolazak");
						Invoke("DelayZaOdbrojavanje",1.15f);
						break;
					case 6:
						//StagesParser.currentMoney+=DailyRewards.DailyRewardAmount[5];
						//dailyReward = DailyRewards.DailyRewardAmount[5];
						//Invoke("DelayZaOdbrojavanje",1.15f);
						GameObject.Find("Day 6 - Magic Box").GetComponent<Collider>().enabled = false;
						GameObject.Find("ButtonCollect").GetComponent<Collider>().enabled = false;
						MysteryBox();
						break;
					}
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();

					StagesParser.ServerUpdate = 1;
				}

				else if(clickedItem==releasedItem && releasedItem == "Day 1")
				{
					if(DailyRewards.LevelReward==1)
					{
						GameObject temp0 = GameObject.Find("CoinsReward");
						temp0.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
						temp0.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						//GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyOdlazak");
						StagesParser.currentMoney+=DailyRewards.DailyRewardAmount[0];
						PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
						PlayerPrefs.Save();
						dailyReward = DailyRewards.DailyRewardAmount[0];
						GameObject.Find("Day 1").transform.GetChild(0).Find("CollectCoinsDailyRewardParticles").GetComponent<ParticleSystem>().Play();
						GameObject.Find("Day 1").GetComponent<Collider>().enabled = false;
						GameObject.Find("ButtonCollect").GetComponent<Collider>().enabled = false;

						temp0.GetComponent<Animation>().Play("CoinsRewardDolazak");
						Invoke("DelayZaOdbrojavanje",1.15f);
					}
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "Day 2")
				{
					if(DailyRewards.LevelReward==2)
					{
						GameObject temp1 = GameObject.Find("CoinsReward");
						temp1.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
						temp1.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						//GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyOdlazak");
						StagesParser.currentMoney+=DailyRewards.DailyRewardAmount[1];
						PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
						PlayerPrefs.Save();
						dailyReward = DailyRewards.DailyRewardAmount[1];
						GameObject.Find("Day 2").transform.GetChild(0).Find("CollectCoinsDailyRewardParticles").GetComponent<ParticleSystem>().Play();
						GameObject.Find("Day 2").GetComponent<Collider>().enabled = false;
						GameObject.Find("ButtonCollect").GetComponent<Collider>().enabled = false;

						temp1.GetComponent<Animation>().Play("CoinsRewardDolazak");
						Invoke("DelayZaOdbrojavanje",1.15f);
					}
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "Day 3")
				{
					if(DailyRewards.LevelReward==3)
					{
						GameObject temp2 = GameObject.Find("CoinsReward");
						temp2.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
						temp2.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						//GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyOdlazak");
						StagesParser.currentMoney+=DailyRewards.DailyRewardAmount[2];
						PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
						PlayerPrefs.Save();
						dailyReward = DailyRewards.DailyRewardAmount[2];
						GameObject.Find("Day 3").transform.GetChild(0).Find("CollectCoinsDailyRewardParticles").GetComponent<ParticleSystem>().Play();
						GameObject.Find("Day 3").GetComponent<Collider>().enabled = false;
						GameObject.Find("ButtonCollect").GetComponent<Collider>().enabled = false;

						temp2.GetComponent<Animation>().Play("CoinsRewardDolazak");
						Invoke("DelayZaOdbrojavanje",1.15f);
					}
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "Day 4")
				{
					if(DailyRewards.LevelReward==4)
					{
						GameObject temp3 = GameObject.Find("CoinsReward");
						temp3.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
						temp3.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						//GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyOdlazak");
						StagesParser.currentMoney+=DailyRewards.DailyRewardAmount[3];
						PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
						PlayerPrefs.Save();
						dailyReward = DailyRewards.DailyRewardAmount[3];
						GameObject.Find("Day 4").transform.GetChild(0).Find("CollectCoinsDailyRewardParticles").GetComponent<ParticleSystem>().Play();
						GameObject.Find("Day 4").GetComponent<Collider>().enabled = false;
						GameObject.Find("ButtonCollect").GetComponent<Collider>().enabled = false;

						temp3.GetComponent<Animation>().Play("CoinsRewardDolazak");
						Invoke("DelayZaOdbrojavanje",1.15f);
					}
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "Day 5")
				{
					if(DailyRewards.LevelReward==5)
					{
						GameObject temp4 = GameObject.Find("CoinsReward");
						temp4.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
						temp4.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						//GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyOdlazak");
						StagesParser.currentMoney+=DailyRewards.DailyRewardAmount[4];
						PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
						PlayerPrefs.Save();
						dailyReward = DailyRewards.DailyRewardAmount[4];
						GameObject.Find("Day 5").transform.GetChild(0).Find("CollectCoinsDailyRewardParticles").GetComponent<ParticleSystem>().Play();
						GameObject.Find("Day 5").GetComponent<Collider>().enabled = false;
						GameObject.Find("ButtonCollect").GetComponent<Collider>().enabled = false;

						temp4.GetComponent<Animation>().Play("CoinsRewardDolazak");
						Invoke("DelayZaOdbrojavanje",1.15f);
					}
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "Day 6 - Magic Box")
				{
					if(DailyRewards.LevelReward==6)
					{
						//GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyOdlazak");
						//StagesParser.currentMoney+=DailyRewards.DailyRewardAmount[5];
						GameObject.Find("Day 6 - Magic Box").GetComponent<Collider>().enabled = false;
						GameObject.Find("ButtonCollect").GetComponent<Collider>().enabled = false;
						MysteryBox();
					}
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "1HatsShopTab")
				{
					ShopManagerFull.ShopObject.DeaktivirajCustomization();
					ShopManagerFull.AktivanItemSesir=ShopManagerFull.AktivanItemSesir+1;
					ShopManagerFull.ShopObject.PozoviCustomizationTab(1);

					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "2TShirtsShopTab")
				{
					ShopManagerFull.ShopObject.DeaktivirajCustomization();
					ShopManagerFull.AktivanItemMajica=ShopManagerFull.AktivanItemMajica+1;
					ShopManagerFull.ShopObject.PozoviCustomizationTab(2);

					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "3BackPackShopTab")
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

				else if(clickedItem==releasedItem && releasedItem == "ClearAll")
				{
					ShopManagerFull.ShopObject.OcistiMajmuna();
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "Kovceg")
				{
					GameObject temp1 = GameObject.Find("Kovceg");
					temp1.GetComponent<Collider>().enabled = false;

					GameObject temp2 = GameObject.Find("CoinsReward");
					temp2.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
					temp2.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);

					temp1.GetComponent<TimeReward>().PokupiNagradu();
					temp1.GetComponent<Animator>().Play("Kovceg Collect Animation Click");
					temp1.transform.Find("PARTIKLI za Kada Se Klikne Collect/CFXM3 Spikes").GetComponent<ParticleSystem>().Play();
					temp1.transform.Find("PARTIKLI za Kada Se Klikne Collect/CollectCoinsParticles").GetComponent<ParticleSystem>().Play();

					temp2.GetComponent<Animation>().Play("CoinsRewardDolazak");
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "Shop Banana")
				{
					ShopManagerFull.ShopObject.KupiBananu();
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}
				else if(clickedItem==releasedItem && releasedItem.StartsWith("ShopInApp"))
				{
					string clickedName = releasedItem;
					switch(releasedItem)
					{
					case "ShopInAppPackSmall":
						break;
					case "ShopInAppPackMedium":
						break;
					case "ShopInAppPackLarge":
						break;
					case "ShopInAppPackGiant":
						break;
					case "ShopInAppPackMonster":
						break;
					case "ShopInAppBananaSmall":
						break;
					case "ShopInAppBananaMedium":
						break;
					case "ShopInAppBananaLarge":
						break;
					case "ShopInAppStarter":
						break;
					case "ShopInAppBobosChoice":
						break;
					case "ShopInAppRemoveAds":
						break;
					case "ShopInAppRestore":
						break;
					
					}
				}
				else if(clickedItem==releasedItem && releasedItem == "Shop POWERUP Double Coins")
				{
					ShopManagerFull.ShopObject.KupiDoubleCoins();
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "Shop POWERUP Magnet")
				{
					ShopManagerFull.ShopObject.KupiMagnet();
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "Shop POWERUP Shield")
				{
					ShopManagerFull.ShopObject.KupiShield();
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "FB Invite")
				{
					StartCoroutine(checkConnectionForLeaderboardLogin());
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "Preview Button")
				{
					if(ShopManagerFull.PreviewState)
					{
						ShopManagerFull.ShopObject.PreviewItem();
					}
					else
					{
						;
					}
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}


				else if(clickedItem==releasedItem && releasedItem == "Buy Button")
				{
					ShopManagerFull.ShopObject.KupiItem();
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "1 Language")
				{
					GameObject.Find ("Settings i Language Scena/Settings Tabs").GetComponent<Animation> ().Play ("TabSettingsOdlazak");
					PrikaziJezike();
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "2 Music")
				{
					if(PlaySounds.musicOn)
					{
						SettingsObjects[1].FindChild("Shop Tab Element Selected").GetComponent<SpriteRenderer>().enabled=true;
						PlaySounds.musicOn=false;
						PlayerPrefs.SetInt("musicOn",0);
						PlayerPrefs.Save();

						PlaySounds.Stop_BackgroundMusic_Menu();
					}
					else
					{
						SettingsObjects[1].FindChild("Shop Tab Element Selected").GetComponent<SpriteRenderer>().enabled=false;
						PlaySounds.musicOn=true;
						PlayerPrefs.SetInt("musicOn",1);
						PlayerPrefs.Save();

						PlaySounds.Play_BackgroundMusic_Menu();
					}
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}

				else if(clickedItem==releasedItem && releasedItem == "3 Sound")
				{
					if(PlaySounds.soundOn)
					{
						SettingsObjects[2].FindChild("Shop Tab Element Selected").GetComponent<SpriteRenderer>().enabled=true;
						PlaySounds.soundOn=false;
						PlayerPrefs.SetInt("soundOn",0);
						PlayerPrefs.Save();
					}
					else
					{
						SettingsObjects[2].FindChild("Shop Tab Element Selected").GetComponent<SpriteRenderer>().enabled=false;
						PlaySounds.soundOn=true;
						PlayerPrefs.SetInt("soundOn",1);
						PlayerPrefs.Save();

						if(PlaySounds.soundOn)
							PlaySounds.Play_Button_OpenLevel();
					}

				}

				else if(clickedItem==releasedItem && releasedItem == "4 Reset Progres")
				{
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();

					SettingsObjects[3].GetComponent<Collider>().enabled = false;
					SettingsObjects[3].Find("Shop Tab Element Selected").GetComponent<SpriteRenderer>().enabled = true;

					ResetProgress();
				}

				else if(clickedItem==releasedItem && releasedItem == "5 Reset Tutorials")
				{
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();

					SettingsObjects[4].GetComponent<Collider>().enabled = false;
					SettingsObjects[4].Find("Shop Tab Element Selected").GetComponent<SpriteRenderer>().enabled = true;

					ResetTutorials();
				}

				else if(clickedItem==releasedItem && releasedItem == "6 Log Out")
				{
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();

					StartCoroutine(checkConnectionForLogout());
				}

				else if(clickedItem==releasedItem && releasedItem == "1")
				{
					if(!PlayerPrefs.HasKey("JezikPromenjen"))
					{
						PlayerPrefs.SetInt("JezikPromenjen",1);
						PlayerPrefs.Save();
					}
					PromeniZastavu(1);
					StagesParser.ServerUpdate = 1;
				}

				else if(clickedItem==releasedItem && releasedItem == "2")
				{
					if(!PlayerPrefs.HasKey("JezikPromenjen"))
					{
						PlayerPrefs.SetInt("JezikPromenjen",1);
						PlayerPrefs.Save();
					}
					PromeniZastavu(2);
					StagesParser.ServerUpdate = 1;
				}

				else if(clickedItem==releasedItem && releasedItem == "3")
				{
					if(!PlayerPrefs.HasKey("JezikPromenjen"))
					{
						PlayerPrefs.SetInt("JezikPromenjen",1);
						PlayerPrefs.Save();
					}
					PromeniZastavu(3);
					StagesParser.ServerUpdate = 1;
				}

				else if(clickedItem==releasedItem && releasedItem == "4")
				{
					if(!PlayerPrefs.HasKey("JezikPromenjen"))
					{
						PlayerPrefs.SetInt("JezikPromenjen",1);
						PlayerPrefs.Save();
					}
					PromeniZastavu(4);
					StagesParser.ServerUpdate = 1;
				}

				else if(clickedItem==releasedItem && releasedItem == "5")
				{
					if(!PlayerPrefs.HasKey("JezikPromenjen"))
					{
						PlayerPrefs.SetInt("JezikPromenjen",1);
						PlayerPrefs.Save();
					}
					PromeniZastavu(5);
					StagesParser.ServerUpdate = 1;
				}

				else if(clickedItem==releasedItem && releasedItem == "6")
				{
					if(!PlayerPrefs.HasKey("JezikPromenjen"))
					{
						PlayerPrefs.SetInt("JezikPromenjen",1);
						PlayerPrefs.Save();
					}
					PromeniZastavu(6);
					StagesParser.ServerUpdate = 1;
				}

				else if(clickedItem==releasedItem && releasedItem == "7")
				{
					if(!PlayerPrefs.HasKey("JezikPromenjen"))
					{
						PlayerPrefs.SetInt("JezikPromenjen",1);
						PlayerPrefs.Save();
					}
					PromeniZastavu(7);
					StagesParser.ServerUpdate = 1;
				}

				else if(clickedItem==releasedItem && releasedItem == "8")
				{
					if(!PlayerPrefs.HasKey("JezikPromenjen"))
					{
						PlayerPrefs.SetInt("JezikPromenjen",1);
						PlayerPrefs.Save();
					}
					PromeniZastavu(8);
					StagesParser.ServerUpdate = 1;
				}

				else if(clickedItem==releasedItem && releasedItem == "9")
				{
					if(!PlayerPrefs.HasKey("JezikPromenjen"))
					{
						PlayerPrefs.SetInt("JezikPromenjen",1);
						PlayerPrefs.Save();
					}
					PromeniZastavu(9);
					StagesParser.ServerUpdate = 1;
				}

				else if(clickedItem==releasedItem && releasedItem == "10")
				{
					if(!PlayerPrefs.HasKey("JezikPromenjen"))
					{
						PlayerPrefs.SetInt("JezikPromenjen",1);
						PlayerPrefs.Save();
					}
					PromeniZastavu(10);
					StagesParser.ServerUpdate = 1;
				}

				else if(clickedItem==releasedItem && releasedItem == "11")
				{
					if(!PlayerPrefs.HasKey("JezikPromenjen"))
					{
						PlayerPrefs.SetInt("JezikPromenjen",1);
						PlayerPrefs.Save();
					}
					PromeniZastavu(11);
					StagesParser.ServerUpdate = 1;
				}

				else if(clickedItem==releasedItem && releasedItem == "12")
				{
					if(!PlayerPrefs.HasKey("JezikPromenjen"))
					{
						PlayerPrefs.SetInt("JezikPromenjen",1);
						PlayerPrefs.Save();
					}
					PromeniZastavu(12);
					StagesParser.ServerUpdate = 1;
				}

				else if(clickedItem==releasedItem && releasedItem == "13")
				{
					if(!PlayerPrefs.HasKey("JezikPromenjen"))
					{
						PlayerPrefs.SetInt("JezikPromenjen",1);
						PlayerPrefs.Save();
					}
					PromeniZastavu(13);
					StagesParser.ServerUpdate = 1;
				}

				else if(clickedItem==releasedItem && releasedItem == "14")
				{
					if(!PlayerPrefs.HasKey("JezikPromenjen"))
					{
						PlayerPrefs.SetInt("JezikPromenjen",1);
						PlayerPrefs.Save();
					}
					PromeniZastavu(14);
					StagesParser.ServerUpdate = 1;
				}

				else if(clickedItem==releasedItem && releasedItem == "15")
				{
					if(!PlayerPrefs.HasKey("JezikPromenjen"))
					{
						PlayerPrefs.SetInt("JezikPromenjen",1);
						PlayerPrefs.Save();
					}
					PromeniZastavu(15);
					StagesParser.ServerUpdate = 1;
				}

				else if(clickedItem==releasedItem && releasedItem == "Exit Button")
				{
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();

					if(StagesParser.ServerUpdate == 1 && FB.IsLoggedIn)
					{
						FacebookManager.FacebookObject.scoreToSet = StagesParser.currentPoints;
						FacebookManager.FacebookObject.proveraPublish_ActionPermisije();
						FacebookManager.FacebookObject.SacuvajScoreNaNivoima(StagesParser.PointsPoNivoima,StagesParser.StarsPoNivoima, StagesParser.maxLevel,StagesParser.bonusLevels);
						FacebookManager.FacebookObject.UpdateujPodatkeKorisnika(StagesParser.currentMoney,StagesParser.currentPoints,LanguageManager.chosenLanguage,StagesParser.currentBananas,StagesParser.powerup_magnets,StagesParser.powerup_shields,StagesParser.powerup_doublecoins,StagesParser.svekupovineGlava,StagesParser.svekupovineMajica,StagesParser.svekupovineLedja,StagesParser.ledja,StagesParser.glava,StagesParser.majica,StagesParser.imaUsi,StagesParser.imaKosu,FacebookManager.NumberOfFriends);
					}

					//@@@@@@@@@
					Application.Quit();

				}
				else if(clickedItem==releasedItem && releasedItem == "Button_CheckOK")
				{
					StartCoroutine(CheckInternetConnection.Instance.ClosePopup());
				}
				else if(clickedItem==releasedItem && releasedItem == "ShopFCBILikePage")
				{
					StartCoroutine(checkConnectionForPageLike("https://www.facebook.com/pages/Banana-Island/636650059721490","BananaIsland"));
					//StartCoroutine(FacebookManager.FacebookObject.otvaranjeStranice("https://www.facebook.com/pages/Banana-Island/636650059721490","BananaIsland"));
				}
				else if(clickedItem==releasedItem && releasedItem == "ShopFCWatchVideo")
				{
					StartCoroutine(checkConnectionForWatchVideo());
				}
				else if(clickedItem==releasedItem && releasedItem == "ShopFCWLLikePage")
				{
					StartCoroutine(checkConnectionForPageLike("https://www.facebook.com/WebelinxGamesApps","Webelinx"));
					//StartCoroutine(FacebookManager.FacebookObject.otvaranjeStranice("https://www.facebook.com/WebelinxGamesApps","Webelinx"));
				}
			}
		}
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

//	IEnumerator otvoriSledeciNivo()
//	{
//		yield return new WaitForSeconds(0.25f);
//		Application.LoadLevel("Worlds");
//	}

	public void AktivirajLeaderboard()
	{

		ObjLeaderboard.Leaderboard=true;
		SwipeControlLeaderboard.controlEnabled = true;
	}
	
	public void DeaktivirajLeaderboard()
	{
		ObjLeaderboard.Leaderboard=false;
		SwipeControlLeaderboard.controlEnabled = false;
	}

	public void OcistiLeaderboard()
	{
		Transform leaderboardTabs = LeaderBoardInvite.transform.parent.Find("Friends Tabs");
		for(int i=0;i<leaderboardTabs.childCount;i++)
		{
			leaderboardTabs.GetChild(i).Find("Friend/LeaderboardYou").GetComponent<SpriteRenderer>().enabled = false;
		}
		FacebookManager.FacebookObject.BrojPrijatelja = 0;
		FacebookManager.FacebookObject.Korisnici.Clear();
		FacebookManager.FacebookObject.Scorovi.Clear();
		FacebookManager.FacebookObject.Imena.Clear();
		FacebookManager.ProfileSlikePrijatelja.Clear();
		FacebookManager.ListaStructPrijatelja.Clear();
	}

	public void AktivirajSettings()
	{
		
		ObjSettingsTabs.SettingsTabs=true;
		SwipeControlSettingsTabs.controlEnabled = true;
	}
	
	public void DeaktivirajSettings()
	{
		ObjSettingsTabs.SettingsTabs=false;
		SwipeControlSettingsTabs.controlEnabled = false;
	}

	public void AktivirajLanguages()
	{
		
		ObjLanguages.Languages=true;
		SwipeControlLanguages.controlEnabled = true;
	}
	
	public void DeaktivirajLanguages()
	{
		ObjLanguages.Languages=false;
		SwipeControlLanguages.controlEnabled = false;
	}

	public void PrikaziJezike()
	{
		SettingState = 2;
		DeaktivirajSettings();
		Invoke("AktivirajLanguages",1f);
		jezikPreUlaskaUPromenuJezika = LanguageManager.chosenLanguage;
		GameObject.Find ("Settings i Language Scena/Language Tabs").GetComponent<Animation> ().Play ("TabSettingsDolazak");
	}

	public void PrikaziSettings()
	{
		if(FB.IsLoggedIn)
		{
			SettingsObjects[5].GetComponent<Collider>().enabled = true;
			SettingsObjects[5].Find("Shop Tab Element Selected").GetComponent<SpriteRenderer>().enabled = false;
		}
		else
		{
			SettingsObjects[5].GetComponent<Collider>().enabled = false;
			SettingsObjects[5].Find("Shop Tab Element Selected").GetComponent<SpriteRenderer>().enabled = true;
		}
		SettingState = 1;
		SettingsOtvoren=true;
		DeaktivirajLanguages();
		Invoke("AktivirajSettings",1f);
		GameObject.Find ("Settings i Language Scena/Settings Tabs").GetComponent<Animation> ().Play ("TabSettingsDolazak");

	}

	public void PromeniZastavuNaOsnovuImena()
	{
		if(!StagesParser.languageBefore.Equals(LanguageManager.chosenLanguage))
		{
			StagesParser.jezikPromenjen = 1;
			PlayerPrefs.SetInt("JezikPromenjen",1);
			PlayerPrefs.Save();
		}
		int broj=0;

		if(StagesParser.jezikPromenjen != 0 || (GameObject.FindGameObjectWithTag("Zastava").GetComponent<Renderer>().material.mainTexture.name.Equals("0") && !LanguageManager.chosenLanguage.Equals("_en")))
		{
			switch(LanguageManager.chosenLanguage)
			{
			case "_en":
				broj = 1;
				break;
			case "_us":
				broj = 2;
				break;
			case "_es":
				broj = 3;
				break;
			case "_ru":
				broj = 4;
				break;
			case "_pt":
				broj = 5;
				break;
			case "_br":
				broj = 6;
				break;
			case "_fr":
				broj = 7;
				break;
			case "_th":
				broj = 8;
				break;
			case "_ch":
				broj = 9;
				break;
			case "_tch":
				broj = 10;
				break;
			case "_de":
				broj = 11;
				break;
			case "_it":
				broj = 12;
				break;
			case "_srb":
				broj = 13;
				break;
			case "_tr":
				broj = 14;
				break;
			case "_ko":
				broj = 15;
				break;
			}
			Texture Zastava = Resources.Load("Zastave/"+broj) as Texture;
			GameObject.FindGameObjectWithTag("Zastava").GetComponent<Renderer>().material.SetTexture("_MainTex", Zastava);
		}

		LanguageManager.RefreshTexts ();
		PrevediTekstove ();
		CheckInternetConnection.Instance.refreshText();
		StagesParser.LoadingPoruke.Clear();
		StagesParser.RedniBrojSlike.Clear();
		StagesParser.Instance.UcitajLoadingPoruke();
	}

	public void PromeniZastavu(int BrojZastave)
	{
		Texture Zastava = Resources.Load("Zastave/"+BrojZastave) as Texture;
		GameObject.FindGameObjectWithTag("Zastava").GetComponent<Renderer>().material.SetTexture("_MainTex", Zastava);
		if(PlayerPrefs.HasKey("JezikPromenjen"))
			GameObject.Find("Settings i Language Scena").transform.Find("Language Tabs/"+BrojZastave.ToString()+"/Shop Tab Element Selected").GetComponent<Renderer>().enabled = true;
		if(selectedLanguage != 0 && selectedLanguage != BrojZastave)
		{
			GameObject.Find("Settings i Language Scena").transform.Find("Language Tabs/"+selectedLanguage.ToString()+"/Shop Tab Element Selected").GetComponent<Renderer>().enabled = false;
		}
		selectedLanguage = BrojZastave;
		switch(BrojZastave)
		{
		case 1:
			LanguageManager.chosenLanguage="_en";
			break;
		case 2:
			LanguageManager.chosenLanguage="_us";
			break;
		case 3:
			LanguageManager.chosenLanguage="_es";
			break;
		case 4:
			LanguageManager.chosenLanguage="_ru";
			break;
		case 5:
			LanguageManager.chosenLanguage="_pt";
			break;
		case 6:
			LanguageManager.chosenLanguage="_br";
			break;
		case 7:
			LanguageManager.chosenLanguage="_fr";
			break;
		case 8:
			LanguageManager.chosenLanguage="_th";
			break;
		case 9:
			LanguageManager.chosenLanguage="_ch";
			break;
		case 10:
			LanguageManager.chosenLanguage="_tch";
			break;
		case 11:
			LanguageManager.chosenLanguage="_de";
			break;
		case 12:
			LanguageManager.chosenLanguage="_it";
			break;
		case 13:
			LanguageManager.chosenLanguage="_srb";
			break;
		case 14:
			LanguageManager.chosenLanguage="_tr";
			break;
		case 15:
			LanguageManager.chosenLanguage="_ko";
			break;
		
		}
		LanguageManager.RefreshTexts ();

		PlayerPrefs.SetString ("choosenLanguage", LanguageManager.chosenLanguage);
		PlayerPrefs.Save ();
		PrevediTekstove ();
		CheckInternetConnection.Instance.refreshText();
		StagesParser.LoadingPoruke.Clear();
		StagesParser.RedniBrojSlike.Clear();
		StagesParser.Instance.UcitajLoadingPoruke();
		//ShopManagerFull.ShopObject.RefresujImenaItema();
	}

	void PrevediTekstove()
	{
		#if UNITY_ANDROID
		GameObject.Find ("FreeCoins/CoinsText").GetComponent<TextMesh> ().text = LanguageManager.Coins;
		GameObject.Find ("FreeCoins/CoinsText").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		GameObject.Find ("FreeCoins/Free").GetComponent<TextMesh> ().text = LanguageManager.Free;
		GameObject.Find ("FreeCoins/Free").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		#elif UNITY_IPHONE
//		GameObject.Find ("RemoveAdsText").GetComponent<TextMesh> ().text = LanguageManager.RemoveAds;
//		GameObject.Find ("RemoveAdsText").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
//		
//		GameObject.Find ("RestoreText").GetComponent<TextMesh> ().text = LanguageManager.Restore;
//		GameObject.Find ("RestoreText").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		#endif
		GameObject.Find("Kovceg/Text/Collect").GetComponent<TextMesh> ().text = LanguageManager.Collect;
		//GameObject.Find ("Kovceg/Text/Collect").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		GameObject.Find("ButtonCollect/Text").GetComponent<TextMesh> ().text = LanguageManager.Collect;
		GameObject.Find ("ButtonCollect/Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		GameObject.Find("Home Scena Interface").transform.Find("FB HOLDER LogIn/ButtonFacebook/Log in").GetComponent<TextMesh> ().text = LanguageManager.LogIn;
		GameObject.Find("Home Scena Interface").transform.Find("FB HOLDER LogIn/ButtonFacebook/Log in").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		GameObject.Find("Zid Header Shop/Text").GetComponent<TextMesh> ().text = LanguageManager.DailyReward;
		GameObject.Find ("Zid Header Shop/Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

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

		GameObject.Find("ShopFCBILikePage/Text/FollowUsOnFacebook").GetComponent<TextMesh> ().text = LanguageManager.FollowUsOnFacebook;
		GameObject.Find ("ShopFCBILikePage/Text/FollowUsOnFacebook").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		GameObject.Find("ShopFCWLLikePage/Text/FollowUsOnFacebook").GetComponent<TextMesh> ().text = LanguageManager.FollowUsOnFacebook;
		GameObject.Find ("ShopFCWLLikePage/Text/FollowUsOnFacebook").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

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

		GameObject.Find("TextLanguage").GetComponent<TextMesh> ().text = LanguageManager.Language;
		GameObject.Find ("TextLanguage").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		GameObject.Find("TextSettings").GetComponent<TextMesh> ().text = LanguageManager.Settings;
		GameObject.Find ("TextSettings").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		GameObject.Find("TextLeaderboard").GetComponent<TextMesh> ().text = LanguageManager.Leaderboard;
		GameObject.Find ("TextLeaderboard").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		GameObject.Find("Leaderboard Scena").transform.Find("FB Invite/Text/Invite And Earn").GetComponent<TextMesh> ().text = LanguageManager.InviteAndEarn;
		GameObject.Find("Leaderboard Scena").transform.Find("FB Invite/Text/Invite And Earn").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

//		GameObject.Find("Friends Tabs").transform.Find("Friend No 2/FB Invite/Invite").GetComponent<TextMesh> ().text = LanguageManager.Invite;
//		GameObject.Find("Friends Tabs").transform.Find("Friend No 2/FB Invite/Invite").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		GameObject.Find ("Day 1/DailyRewordDayAnimationHolder/Text/Day").GetComponent<TextMesh> ().text = LanguageManager.Day+" 1";
		GameObject.Find ("Day 1/DailyRewordDayAnimationHolder/Text/Day").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		GameObject.Find ("Day 2/DailyRewordDayAnimationHolder/Text/Day").GetComponent<TextMesh> ().text = LanguageManager.Day+" 2";
		GameObject.Find ("Day 2/DailyRewordDayAnimationHolder/Text/Day").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		GameObject.Find ("Day 3/DailyRewordDayAnimationHolder/Text/Day").GetComponent<TextMesh> ().text = LanguageManager.Day+" 3";
		GameObject.Find ("Day 3/DailyRewordDayAnimationHolder/Text/Day").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		GameObject.Find ("Day 4/DailyRewordDayAnimationHolder/Text/Day").GetComponent<TextMesh> ().text = LanguageManager.Day+" 4";
		GameObject.Find ("Day 4/DailyRewordDayAnimationHolder/Text/Day").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		GameObject.Find ("Day 5/DailyRewordDayAnimationHolder/Text/Day").GetComponent<TextMesh> ().text = LanguageManager.Day+" 5";
		GameObject.Find ("Day 5/DailyRewordDayAnimationHolder/Text/Day").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		GameObject.Find ("Day 6 - Magic Box/DailyRewordDayAnimationHolder/Text/Day").GetComponent<TextMesh> ().text = LanguageManager.Day+" 6";
		GameObject.Find ("Day 6 - Magic Box/DailyRewordDayAnimationHolder/Text/Day").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		Transform temp = GameObject.Find("Settings Tabs").transform;
		temp.Find("1 Language/Text/Text").GetComponent<TextMesh>().text = LanguageManager.Language;
		temp.Find("1 Language/Text/Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		temp.Find("2 Music/Text/Text").GetComponent<TextMesh>().text = LanguageManager.Music;
		temp.Find("2 Music/Text/Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		temp.Find("3 Sound/Text/Text").GetComponent<TextMesh>().text = LanguageManager.Sound;
		temp.Find("3 Sound/Text/Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		temp.Find("4 Reset Progres/Text/Text").GetComponent<TextMesh>().text = LanguageManager.ResetProgress;
		temp.Find("4 Reset Progres/Text/Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		temp.Find("5 Reset Tutorials/Text/Text").GetComponent<TextMesh>().text = LanguageManager.ResetTutorials;
		temp.Find("5 Reset Tutorials/Text/Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		temp.Find("6 Log Out/Text/Text").GetComponent<TextMesh>().text = LanguageManager.LogOut;
		temp.Find("6 Log Out/Text/Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		temp = LeaderBoardInvite.transform.parent.Find("Friends Tabs");
		for(int i=0;i<10;i++)
		{
			if(temp.GetChild(i).Find("FB Invite").gameObject.activeSelf)
			{
				temp.GetChild(i).Find("FB Invite/Invite").GetComponent<TextMesh>().text = LanguageManager.InviteFriendsAndEarn;
				temp.GetChild(i).Find("FB Invite/Invite").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else
			{
				temp.GetChild(i).Find("FB Invite").gameObject.SetActive(true);
				temp.GetChild(i).Find("FB Invite/Invite").GetComponent<TextMesh>().text = LanguageManager.InviteFriendsAndEarn;
				temp.GetChild(i).Find("FB Invite/Invite").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
				temp.GetChild(i).Find("FB Invite").gameObject.SetActive(false);
			}
		}
	}

	IEnumerator otvoriSledeciNivo()
	{
		//		yield return new WaitForSeconds(2f);
		//yield return null;
		yield return new WaitForSeconds(1.1f);
		//@@@@@@@@@@@@@@@ PRIVREMENO BEZ TUTORIALA
		if(StagesParser.odgledaoTutorial == 0)
		{
			StagesParser.loadingTip = 1;
			Application.LoadLevel("LoadingScene");
		}
		else 
		{
			StagesParser.vratioSeNaSvaOstrva = true;
			Application.LoadLevel("All Maps");
		}
	}

	void SkloniCoinsReward()
	{
		GameObject.Find("CoinsReward").GetComponent<Animation>().Play("CoinsRewardOdlazak");
		GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyOdlazak");
	}
	
	void DelayZaOdbrojavanje()
	{
		StartCoroutine(StagesParser.Instance.moneyCounter(dailyReward,GameObject.Find("CoinsReward/Coins Number").GetComponent<TextMesh>(),true));
		Invoke("SkloniCoinsReward",1.2f);
	}

	void MysteryBox()
	{
		GameObject temp1 = GameObject.Find("Day 6 - Magic Box");
		temp1.transform.GetChild(0).GetComponent<Animator>().Play("CollectDailyRewardMagicBox");
		Sprite powerUpRewardSprite = null;
		if(StagesParser.powerup_magnets <= StagesParser.powerup_shields)
		{
			if(StagesParser.powerup_magnets <= StagesParser.powerup_doublecoins)
			{
				powerUpRewardSprite = ShopManagerFull.ShopObject.transform.Find("4 Power-Ups/Power-Ups Tabovi/Shop POWERUP Magnet/Plavi Bedz/Magnet Icon").GetComponent<SpriteRenderer>().sprite;
				if(StagesParser.powerup_magnets <= 10)
				{
					StagesParser.powerup_magnets += 3;
					temp1.transform.GetChild(0).Find("Magic Box Reward HOLDER/Magic Box Reward/Kolicina").GetComponent<TextMesh>().text = "x 3";
					temp1.transform.GetChild(0).Find("Magic Box Reward HOLDER/Magic Box Reward/Kolicina").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
				}
				else
				{
					StagesParser.powerup_magnets += 2;
					temp1.transform.GetChild(0).Find("Magic Box Reward HOLDER/Magic Box Reward/Kolicina").GetComponent<TextMesh>().text = "x 2";
					temp1.transform.GetChild(0).Find("Magic Box Reward HOLDER/Magic Box Reward/Kolicina").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
				}
			}
			else
			{
				powerUpRewardSprite = ShopManagerFull.ShopObject.transform.Find("4 Power-Ups/Power-Ups Tabovi/Shop POWERUP Double Coins/Plavi Bedz/Double Coins Icon").GetComponent<SpriteRenderer>().sprite;
				if(StagesParser.powerup_doublecoins <= 10)
				{
					StagesParser.powerup_doublecoins += 3;
					temp1.transform.GetChild(0).Find("Magic Box Reward HOLDER/Magic Box Reward/Kolicina").GetComponent<TextMesh>().text = "x 3";
					temp1.transform.GetChild(0).Find("Magic Box Reward HOLDER/Magic Box Reward/Kolicina").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
				}
				else
				{
					StagesParser.powerup_doublecoins += 2;
					temp1.transform.GetChild(0).Find("Magic Box Reward HOLDER/Magic Box Reward/Kolicina").GetComponent<TextMesh>().text = "x 2";
					temp1.transform.GetChild(0).Find("Magic Box Reward HOLDER/Magic Box Reward/Kolicina").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
				}
			}
		}
		else if(StagesParser.powerup_shields <= StagesParser.powerup_doublecoins)
		{
			powerUpRewardSprite = ShopManagerFull.ShopObject.transform.Find("4 Power-Ups/Power-Ups Tabovi/Shop POWERUP Shield/Plavi Bedz/Shield Icon").GetComponent<SpriteRenderer>().sprite;
			if(StagesParser.powerup_shields <= 10)
			{
				StagesParser.powerup_shields += 3;
				temp1.transform.GetChild(0).Find("Magic Box Reward HOLDER/Magic Box Reward/Kolicina").GetComponent<TextMesh>().text = "x 3";
				temp1.transform.GetChild(0).Find("Magic Box Reward HOLDER/Magic Box Reward/Kolicina").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			}
			else
			{
				StagesParser.powerup_shields += 2;
				temp1.transform.GetChild(0).Find("Magic Box Reward HOLDER/Magic Box Reward/Kolicina").GetComponent<TextMesh>().text = "x 2";
				temp1.transform.GetChild(0).Find("Magic Box Reward HOLDER/Magic Box Reward/Kolicina").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			}
		}
		else
		{
			powerUpRewardSprite = ShopManagerFull.ShopObject.transform.Find("4 Power-Ups/Power-Ups Tabovi/Shop POWERUP Double Coins/Plavi Bedz/Double Coins Icon").GetComponent<SpriteRenderer>().sprite;
			if(StagesParser.powerup_doublecoins <= 10)
			{
				StagesParser.powerup_doublecoins += 3;
				temp1.transform.GetChild(0).Find("Magic Box Reward HOLDER/Magic Box Reward/Kolicina").GetComponent<TextMesh>().text = "x 3";
				temp1.transform.GetChild(0).Find("Magic Box Reward HOLDER/Magic Box Reward/Kolicina").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			}
			else
			{
				StagesParser.powerup_doublecoins += 2;
				temp1.transform.GetChild(0).Find("Magic Box Reward HOLDER/Magic Box Reward/Kolicina").GetComponent<TextMesh>().text = "x 2";
				temp1.transform.GetChild(0).Find("Magic Box Reward HOLDER/Magic Box Reward/Kolicina").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			}
		}

		StagesParser.currentBananas++;
		PlayerPrefs.SetInt("TotalBananas",StagesParser.currentBananas);
		
		PlayerPrefs.SetString("PowerUps",(StagesParser.powerup_doublecoins+"#"+StagesParser.powerup_magnets+"#"+StagesParser.powerup_shields));
		PlayerPrefs.Save();

		GameObject.Find ("Double Coins Number/Number").GetComponent<TextMesh> ().text = StagesParser.powerup_doublecoins.ToString ();
		GameObject.Find ("Double Coins Number/Number").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		
		GameObject.Find ("Magnet Number/Number").GetComponent<TextMesh> ().text = StagesParser.powerup_magnets.ToString ();
		GameObject.Find ("Magnet Number/Number").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		
		GameObject.Find ("Shield Number/Number").GetComponent<TextMesh> ().text = StagesParser.powerup_shields.ToString ();
		GameObject.Find ("Shield Number/Number").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		temp1.transform.GetChild(0).Find("Magic Box Reward HOLDER/Magic Box Reward").GetComponent<SpriteRenderer>().sprite = powerUpRewardSprite;
		Invoke("SkloniDailyRewardsPosleMysteryBox",4.5f);
	}

	void SkloniDailyRewardsPosleMysteryBox()
	{
		GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyOdlazak");
		Invoke("UgasiMysteryBox",2f);
	}

	void UgasiMysteryBox()
	{
		GameObject.Find("Day 6 - Magic Box").SetActive(false);
	}

	void ResetProgress()
	{
		Transform loading = GameObject.Find("Loading Buffer HOLDER").transform;
		Transform camera = Camera.main.transform;
		loading.position = new Vector3(camera.position.x,camera.position.y,loading.position.z);
		loading.GetChild(0).gameObject.SetActive(true);
		loading.GetChild(0).GetComponent<Animator>().Play("LoadingBufferUlazAnimation");

		string pom = "1#0#0";
		for(int i=1;i<StagesParser.allLevels.Length;i++)
		{
			pom+="_"+(i+1).ToString()+"#-1#0";
		}
		StagesParser.allLevels = pom.Split('_');
		PlayerPrefs.SetString("AllLevels",pom);

		StagesParser.currentPoints = 0;
		PlayerPrefs.SetInt("TotalPoints",StagesParser.currentPoints);

		StagesParser.lastUnlockedWorldIndex = 0;
		for(int i=0;i<StagesParser.totalSets;i++)
		{
			StagesParser.unlockedWorlds[i] = false;
		}
		StagesParser.unlockedWorlds[0] = true;

		if(FB.IsLoggedIn)
		{
			StartCoroutine(checkConnectionForResetProgress());
		}
		for(int i=0;i<StagesParser.totalSets;i++)
		{
			StagesParser.trenutniNivoNaOstrvu[i] = 1;
			PlayerPrefs.SetInt("TrenutniNivoNaOstrvu"+i.ToString(),StagesParser.trenutniNivoNaOstrvu[i]);
		}
		PlayerPrefs.Save();

//		string pom = "1#3#3750_2#3#3750_3#3#4200_4#3#3850_5#3#1120_6#3#2120_7#3#2300_8#3#2340_9#3#3750_10#2#1010_11#2#2120_12#3#2320_13#1#3220_14#3#2260_15#2#2610_16#3#1620_17#3#1740_18#2#2140_19#3#2130_20#3#0_21#2#0_22#1#0_23#1#0_24#2#0_25#1#0_26#1#0_27#1#0_28#1#0_29#1#0_30#1#0_31#2#0_32#1#0_33#2#0_34#1#0_35#1#0_36#1#0_37#1#0_38#3#0_39#3#0_40#3#0_41#3#0_42#3#0_43#3#0_44#3#0_45#3#0_46#3#0_47#3#0_48#3#0_49#3#0_50#3#0_51#3#0_52#3#0_53#3#0_54#3#0_55#3#0_56#3#0_57#3#0_58#3#0_59#3#0_60#1#0_61#3#0_62#1#0_63#1#0_64#1#0_65#1#0_66#1#0_67#1#0_68#1#0_69#1#0_70#1#0_71#1#0_72#1#0_73#1#0_74#1#0_75#1#0_76#1#0_77#1#0_78#1#0_79#1#0_80#1#0_81#3#2500_82#3#2120_83#0#0_84#-1#0_85#-1#0_86#-1#0_87#-1#0_88#-1#0_89#-1#0_90#-1#0_91#-1#0_92#-1#0_93#-1#0_94#-1#0_95#-1#0_96#-1#0_97#-1#0_98#-1#0_99#-1#0_100#-1#0";
//		StagesParser.allLevels = pom.Split('_');
//		PlayerPrefs.SetString("AllLevels",pom);
//		PlayerPrefs.Save();
		StagesParser.RecountTotalStars();
		if(!FB.IsLoggedIn)
			StagesParser.Instance.UgasiLoading();

	}

	IEnumerator SacekajDaSePostaviScoreNaNulu()
	{
		while(FacebookManager.FacebookObject.resetovanScoreNaNulu == 2)
		{
			yield return null;
		}

		FacebookManager.MestoPozivanjaLogina = 1;
		OcistiLeaderboard();
		
		FacebookManager.FacebookObject.GetFacebookFriendScores();
	}

	void ResetTutorials()
	{
		StagesParser.odgledaoTutorial = 0;
		StagesParser.currStageIndex = 0;
		StagesParser.currSetIndex = 0;
		PlayerPrefs.SetString("OdgledaoTutorial",StagesParser.odgledaoTutorial.ToString()+"#"+StagesParser.otvaraoShopNekad.ToString());
		PlayerPrefs.Save();
	}

	IEnumerator DoLogout()
	{
		while(!FacebookManager.FacebookObject.OKzaLogout)
		{
			yield return null;
		}
		FacebookManager.FacebookLogout();
		FacebookManager.FacebookObject.OKzaLogout = false;
	}

	void ProveraZaLogoutZbogDugmica()
	{
		if(logoutKliknut)
		{
			logoutKliknut = false;
			if(!FB.IsLoggedIn)
			{
				if(PlayerPrefs.GetInt("Logovan") == 1)
					PlayerPrefs.SetInt("Logovan",0);

				MainScene.FacebookLogIn.SetActive(true);
				MainScene.LeaderBoardInvite.SetActive(false);
				for(int i=0;i<10;i++)
				{
					if(i==1)
					{
						FriendsObjects[i].FindChild("FB Invite").gameObject.SetActive(true);
						FriendsObjects[i].FindChild("Friend").gameObject.SetActive(false);
						FriendsObjects[i].FindChild("FB Invite/Coin Shop").gameObject.SetActive(false);
						FriendsObjects[i].FindChild("FB Invite/Invite").GetComponent<TextMesh>().text=LanguageManager.LogIn;
						FriendsObjects[i].FindChild("FB Invite/Invite").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
					}
					else
					{
						FriendsObjects[i].gameObject.SetActive(false);
					}
				}

				PlayerPrefs.DeleteKey("JezikPromenjen");
				PlayerPrefs.Save();
			}
		}
	}

}
