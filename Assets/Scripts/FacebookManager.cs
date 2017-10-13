using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Parse;
using System.Threading.Tasks;
using System.Collections.ObjectModel;


public class FacebookManager : MonoBehaviour {

	public static string UserSveKupovineHats, UserSveKupovineShirts, UserSveKupovineBackPacks;

	public static int UserCoins;
	public static string bonusLevels;
	public static int UserScore;
	public static string UserLanguage;
	public static int UserBanana;
	public static int UserPowerMagnet;
	public static int UserPowerShield;
	public static int UserPowerDoubleCoins;
	public static int GlavaItem;
	public static int TeloItem;
	public static int LedjaItem;
	public static bool Usi;
	public static bool Kosa;
	public static int indexUListaStructPrijatelja;
	public static int MestoPozivanjaLogina = 1;   // 1 - HomeScena , 2 - Mapa, 3 - EndLevel
	public static bool KorisnikoviPodaciSpremni=false;
	bool isInit = false;
	public static FacebookManager FacebookObject;
	private Texture2D lastResponseTexture;
	string ApiQuery = "";
	public static bool Ulogovan;
	public static string stranica = System.String.Empty;
	static bool otisaoDaLajkuje = false;
	public static string IDstranice = System.String.Empty;
	static bool lajkovao = false;
	static int nagrada = 0;
	System.DateTime timeToShowNextElement;
	bool leftApp = false;
	public static string lokacijaProvere = "Shop";
	public static string User;
	string UserRodjendan;
	List<string> Prijatelji;

	public struct IDiSlika
	{
		public string PrijateljID;
		public Texture profilePicture;
	}
	public static List<IDiSlika> ProfileSlikePrijatelja = new List<IDiSlika>();

	//public static List<Texture> ProfileSlikePrijatelja=new List<Texture>();
	public List<string> Korisnici;
	public List<string> Scorovi;
	public List<string> Imena;
	public static int NumberOfFriends;
	public bool odobrioPublishActions;
	public int scoreToSet;
	public bool nePostojiKorisnik = true;
	public bool zavrsioUcitavanje = false;
	public struct StrukturaPrijatelja
	{
		public string PrijateljID;
		public IList<int> scores;
		public IList<int> stars;
		public int MaxLevel;
		public Texture profilePicture;

	}
	public static string[] permisija;
	public static string[] statusPermisije;

//	ScorePrijatelja[] SkoroviPrijateljaNiz =	new ScorePrijatelja[2];
	public static List<StrukturaPrijatelja> ListaStructPrijatelja=new List<StrukturaPrijatelja>();
	int TrenutniNivoIgraca;
	int[] scorePoNivouPrijatelja=new int[120];
	int[] ScorePoNivoimaNiz = new int[120];
	int[] BrojZvezdaPoNivouNiz = new int[120];
	int[] testNiz=new int[120];
	bool WaitForFacebook=false,WaitForFacebookFriend=false;
	public static string UserName;
	public static Texture ProfilePicture,FriendPic1,FriendPic2,FriendPic3,FriendPic4,FriendPic5;
	string permissions="user_friends,publish_actions";//,user_birthday - za rodjendan,user_games_activity"; //permisije za Facebook user_friends-za pristup listi prijatelja, user_likes-svi like-ovi korisnika, public_profile-id,name,first name, last name, link, gender, locale, age range; publish_actions - Allows your app to publish to the Open Graph using Built-in Actions, Achievements, Scores, or Custom Actions. Your app can also publish other activity which is detailed in the Publishing Permissions doc.


	string Code;
	string TipNagrade;
	int IznosNagrade;

	//SERVERSKI DEO PARSE - START
	ParseObject Korisnik= new ParseObject("User");
	ParseObject LevelScore = new ParseObject("LevelScore");

	string JezikServer;
	int NivoServer;
	//SERVERSKI DEO PARSE - END

	bool updatedSuccessfullyScoreNaNivoima = false;
	bool updatedSuccessfullyPodaciKorisnika = false;
	[HideInInspector] public bool OKzaLogout = false;
	[HideInInspector] public int resetovanScoreNaNulu = 0;

	void Awake ()
	{
		FacebookManager.FacebookObject = this;
		DontDestroyOnLoad(gameObject);
//		PlayerPrefs.DeleteAll ();
	}

	void Start()
	{
		for(int i=0;i<100;i++)
		{
			testNiz[i]=i+100;
		}
		FB.Init(OnInitComplete,OnHideUnity);
		if(PlayerPrefs.HasKey("UserSveKupovineHats"))
		{
			UserSveKupovineHats=PlayerPrefs.GetString("UserSveKupovineHats");
		}
		else
		{
			UserSveKupovineHats = "0#0#";//0#0#0#0#0#0#0#0#0#0#0#0#0#";
		}
		if(PlayerPrefs.HasKey("UserSveKupovineShirts"))
		{
			UserSveKupovineShirts=PlayerPrefs.GetString("UserSveKupovineShirts");
		}
		else
		{
			UserSveKupovineShirts = "0#0#0#0#0#0#0#0#";
		}
		if(PlayerPrefs.HasKey("UserSveKupovineBackPacks"))
		{
			UserSveKupovineBackPacks=PlayerPrefs.GetString("UserSveKupovineBackPacks");
		}
		else
		{
			UserSveKupovineBackPacks = "0#0#0#0#0#0#";
		}
		StagesParser.svekupovineGlava = UserSveKupovineHats;
		StagesParser.svekupovineMajica = UserSveKupovineShirts;
		StagesParser.svekupovineLedja = UserSveKupovineBackPacks;

		if(FB.IsLoggedIn)
		{
			Ulogovan = true;
		}
		else
		{
			Ulogovan = false;
		}
	}

	public static void FacebookLogout()
	{
		if(FB.IsLoggedIn)
		{
			FB.Logout();
			Ulogovan = false;
			//if(FacebookManager.MestoPozivanjaLogina==1)
			{
//				MainScene.FacebookLogIn.SetActive(true);
//				MainScene.LeaderBoardInvite.SetActive(false);
			}
			StagesParser.Instance.ObrisiProgresNaLogOut();
		}
	}

	private void OnInitComplete()
	{
		isInit = true;
	}
	
	private void OnHideUnity(bool isGameShown)
	{
		Debug.Log("Is game showing? " + isGameShown);
	}

	public void FacebookLogin()
	{
		FB.Login(permissions,LoginCallback);
	}

	public void LoginCallback(FBResult result)
	{
		if(result.Error != null)
		{
			StagesParser.Instance.UgasiLoading();
		}
		else if(!FB.IsLoggedIn)
		{
			StagesParser.Instance.UgasiLoading();
		}
		else
		{
			Ulogovan=true;
		
			User=FB.UserId;

			PlayerPrefs.SetInt("Logovan",0);

	//		if(StagesParser.lastLoggedUser.Equals(System.String.Empty))
	//		{
	//			StagesParser.lastLoggedUser = User;
	//			PlayerPrefs.SetString("LastLoggedUser",StagesParser.lastLoggedUser);
	//		}
	//		else
	//		{
	//			StagesParser.lastLoggedUser = PlayerPrefs.GetString("LastLoggedUser");
	//		}
			PlayerPrefs.Save();


			GetFacebookName();

			if(MestoPozivanjaLogina==1)
			{
				Transform loading = GameObject.Find("Loading Buffer HOLDER").transform;
				Transform camera = Camera.main.transform;
				loading.position = new Vector3(camera.position.x,camera.position.y,loading.position.z);
				loading.GetChild(0).gameObject.SetActive(true);
				loading.GetChild(0).GetComponent<Animator>().Play("LoadingBufferUlazAnimation");

				ProveriKorisnika();

			}

			else if(MestoPozivanjaLogina == 2)
			{
				Transform loading = GameObject.Find("Loading Buffer HOLDER").transform;
				Transform guiCamera = GameObject.Find("GUICamera").transform;
				loading.position = new Vector3(guiCamera.position.x,guiCamera.position.y,loading.position.z);
				loading.GetChild(0).gameObject.SetActive(true);
				loading.GetChild(0).GetComponent<Animator>().Play("LoadingBufferUlazAnimation");

				ProveriKorisnika();
			}
			else if(MestoPozivanjaLogina == 3)
			{
				Transform loading = GameObject.Find("Loading Buffer HOLDER").transform;
				Transform guiCamera = GameObject.Find("GUICamera").transform;
				loading.position = new Vector3(guiCamera.position.x,guiCamera.position.y,loading.position.z);
				loading.GetChild(0).gameObject.SetActive(true);
				loading.GetChild(0).GetComponent<Animator>().Play("LoadingBufferUlazAnimation");

				ProveriKorisnika();
			}
		}


	}

	public void RefreshujScenu1PosleLogina()
	{
		MainScene.FacebookLogIn.SetActive(false); //Ovde izbacio MissingReferenceException kad sam se vratio na main scenu i odmah isao na play
		MainScene.LeaderBoardInvite.SetActive(true);
	}
	public void RefreshujScenu2PosleLogina()
	{
		GameObject.Find("FB Login").SetActive(false);
		AllMapsManageFull.makniPopup = 0;
	}
	public void RefreshujScenu3PosleLogina()
	{
		GameObject.Find("FB Login").SetActive(false);
		KameraMovement.makniPopup = 0;
	}

	void OpenPage()
	{
		if(stranica == "BananaIsland")
		{
			IDstranice = "636650059721490";
			nagrada = 1000;
//			Application.OpenURL("fb://page/"+IDstranice);
//				float startTime;
//				startTime = Time.timeSinceLevelLoad;
//				if (Time.timeSinceLevelLoad - startTime <= 1f)
//				{
//					
//					//fail. Open safari.
//					
//					Application.OpenURL(facebookAddress);
//					
//				}
//			Application.OpenURL("https://www.facebook.com/pages/Banana-Island/636650059721490");
			otisaoDaLajkuje = true;
			PlayerPrefs.SetInt("otisaoDaLajkuje",nagrada);
			PlayerPrefs.SetString("IDstranice",IDstranice);
			PlayerPrefs.SetString("stranica",stranica);
			PlayerPrefs.Save();
		}
		else if(stranica == "Webelinx")
		{
			IDstranice = "437444296353647";
			nagrada = 1000;
			//Application.OpenURL("fb://page/"+IDstranice);
			//Application.OpenURL("https://www.facebook.com/WebelinxGamesApps");
			otisaoDaLajkuje = true;
			PlayerPrefs.SetInt("otisaoDaLajkuje",nagrada);
			PlayerPrefs.SetString("IDstranice",IDstranice);
			PlayerPrefs.SetString("stranica",stranica);
			PlayerPrefs.Save();
		}
	}

	void OnApplicationPause(bool pauseStatus)
	{
		if(!pauseStatus) //odpauzirana
		{

			if(otisaoDaLajkuje)
			{
				//FB.API("/"+FB.UserId+"/likes",Facebook.HttpMethod.GET, UserLikesCallback);
				otisaoDaLajkuje = false;
				StagesParser.Instance.UgasiLoading();
				if(stranica == "BananaIsland")
				{
					ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCBILikePage").GetComponent<Collider>().enabled = false;
					ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCBILikePage").GetComponent<Renderer>().material.color = new Color(0.58f,0.58f,0.58f);
					ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCBILikePage/Like BananaIsland FC").GetComponent<Renderer>().material.color = new Color(0.58f,0.58f,0.58f);
					StagesParser.currentMoney += StagesParser.likePageReward;
					PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
					PlayerPrefs.Save();
					StartCoroutine(StagesParser.Instance.moneyCounter(StagesParser.likePageReward,ShopManagerFull.ShopObject.transform.Find("Shop Interface/Coins/Coins Number").GetComponent<TextMesh>(),true));
				}
				if(stranica == "Webelinx")
				{
					ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCWLLikePage").GetComponent<Collider>().enabled = false;
					ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCWLLikePage").GetComponent<Renderer>().material.color = new Color(0.58f,0.58f,0.58f);
					ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCWLLikePage/Like Webelinx FC").GetComponent<Renderer>().material.color = new Color(0.58f,0.58f,0.58f);
					StagesParser.currentMoney += StagesParser.likePageReward;
					PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
					PlayerPrefs.Save();
					StartCoroutine(StagesParser.Instance.moneyCounter(StagesParser.likePageReward,ShopManagerFull.ShopObject.transform.Find("Shop Interface/Coins/Coins Number").GetComponent<TextMesh>(),true));
				}
				StagesParser.ServerUpdate = 1;
			}
			if(PlayerPrefs.HasKey("Logovan"))
			{
				if(PlayerPrefs.GetInt("Logovan") == 0)
				{
					if(FB.IsLoggedIn)
					{
						FB.Logout();
						Ulogovan = false;
						zavrsioUcitavanje = false;
						BrojPrijatelja=0;
						//StagesParser.Instance.UgasiLoading();
						if(MestoPozivanjaLogina == 1)
						{
							Transform leaderboardTabs = MainScene.LeaderBoardInvite.transform.parent.Find("Friends Tabs");
							Transform[] FriendsObjects = new Transform[leaderboardTabs.childCount];
							for(int i=0;i<leaderboardTabs.childCount;i++)
							{
								leaderboardTabs.GetChild(i).Find("Friend/LeaderboardYou").GetComponent<SpriteRenderer>().enabled = false;
								FriendsObjects[i] = leaderboardTabs.GetChild(i);
							}

							Transform leaderboardLogin = MainScene.LeaderBoardInvite.transform.parent.Find("Friends Tabs/Friend No 2");
							leaderboardLogin.localPosition = new Vector3(leaderboardLogin.localPosition.x, -1.85f, leaderboardLogin.localPosition.z);
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
						FacebookManager.FacebookObject.BrojPrijatelja = 0;
						FacebookManager.FacebookObject.Korisnici.Clear();
						FacebookManager.FacebookObject.Scorovi.Clear();
						FacebookManager.FacebookObject.Imena.Clear();
						FacebookManager.ProfileSlikePrijatelja.Clear();
						FacebookManager.ListaStructPrijatelja.Clear();
					}
					else
					{
					}
				}
				else
				{
				}
			}
		}
		else //pauzirana
		{
			leftApp = true;
			if(StagesParser.ServerUpdate == 1 && FB.IsLoggedIn)
			{
				FacebookManager.FacebookObject.scoreToSet = StagesParser.currentPoints;
				FacebookManager.FacebookObject.proveraPublish_ActionPermisije();
				FacebookManager.FacebookObject.SacuvajScoreNaNivoima(StagesParser.PointsPoNivoima,StagesParser.StarsPoNivoima, StagesParser.maxLevel,StagesParser.bonusLevels);
				FacebookManager.FacebookObject.UpdateujPodatkeKorisnika(StagesParser.currentMoney,StagesParser.currentPoints,LanguageManager.chosenLanguage,StagesParser.currentBananas,StagesParser.powerup_magnets,StagesParser.powerup_shields,StagesParser.powerup_doublecoins,StagesParser.svekupovineGlava,StagesParser.svekupovineMajica,StagesParser.svekupovineLedja,StagesParser.ledja,StagesParser.glava,StagesParser.majica,StagesParser.imaUsi,StagesParser.imaKosu,FacebookManager.NumberOfFriends);
			}
		}
	}

	//DEO ZA UZIMANJE PROFILE PICTURE KORISNIKA - START

	public void GetProfilePicture()
	{
		FB.API("/"+User+"/picture?redirect=true&height=64&type=normal&width=64", Facebook.HttpMethod.GET, MyPictureCallback); 
	}

	public void MyPictureCallback(FBResult result)                                                                                        
	{                                                                                                                              
		
		if (result.Error != null)                                                                                                  
		{                                                                                                                          
			// Let's just try again                                                                                                
			//FB.API("/"+User+"/picture?redirect=true&height=64&type=normal&width=64", Facebook.HttpMethod.GET, MyPictureCallback);
			StagesParser.Instance.UgasiLoading();
			return;                                                                                                                
		}  
		else
		{
			ProfilePicture=result.Texture;
			GameObject.Find("FaceButton").GetComponent<Renderer>().material.mainTexture=ProfilePicture; 
		}
		                                                                     
	}

	public void GetFacebookName()
	{
		FB.API("me?fields=id,name", Facebook.HttpMethod.GET, GetFacebookNameCallback); 
	}

	public void GetFacebookNameCallback(FBResult result)   
	{                                                                                                                              
		
		if (result.Error != null)                                                                                                  
		{                                                                                                                          
			// Let's just try again                                                                                                
			//FB.API("/"+User+"/picture?redirect=true&height=64&type=normal&width=64", Facebook.HttpMethod.GET, MyPictureCallback);
			StagesParser.Instance.UgasiLoading();
			return;                                                                                                                
		}  
		else
		{
			Dictionary<string, object> rez= Facebook.MiniJSON.Json.Deserialize(result.Text) as Dictionary<string,object>;
			UserName=rez["name"].ToString();
		}
		                                                                   
	}


	//DEO ZA FRIEND INVITE - POCETAK
	//FACE INVITE
	string FriendSelectorTitle = "Banana Island - Bobo's Epic Tale";
	string FriendSelectorMessage = LanguageManager.Play+" \"Banana Island - Bobo's Epic Tale\"";
	string FriendSelectorFilters = "[\"all\",\"app_users\",\"app_non_users\"]";
	string FriendSelectorData = "{}";
	string FriendSelectorExcludeIds = "";
	string FriendSelectorMax = "";
	string lastResponse = "";
	//FACE INVITE
	public void FaceInvite()
	{
		if(FB.IsLoggedIn)
		{
			int? maxRecipients = null;
			if (FriendSelectorMax != "")
			{
				try
				{
					maxRecipients = Int32.Parse(FriendSelectorMax);
				}
				catch
				{

				}
			}
			
			// include the exclude ids
			string[] excludeIds = (FriendSelectorExcludeIds == "") ? null : FriendSelectorExcludeIds.Split(',');
			
			FB.AppRequest(
				
//				FriendSelectorMessage,
//				null,
//				FriendSelectorFilters,
//				excludeIds,
//				maxRecipients,
//				FriendSelectorData,
//				FriendSelectorTitle,
//				InviteCallback
//				);

		message: FriendSelectorMessage,
		filters: FriendSelectorFilters,
		excludeIds: excludeIds,
		maxRecipients: maxRecipients,
		data: FriendSelectorData,
		title: FriendSelectorTitle,
		callback: InviteCallback);

//			FB.AppRequest(
//				FriendSelectorMessage, 
//				null,         
//				FriendSelectorFilters, 
//				excludeIds,          
//				maxRecipients,       
//				FriendSelectorData,       
//				FriendSelectorTitle,         
//				Callback 
//			              );
		}
		else
		{
			FacebookManager.FacebookObject.FacebookLogin();
		}

	}

	void InviteCallback(FBResult result)
	{
		Dictionary<string, object> rez= Facebook.MiniJSON.Json.Deserialize(result.Text) as Dictionary<string,object>;

		List<object> prijatelji= (List<object>) rez["to"];

		StagesParser.currentMoney += prijatelji.Count * StagesParser.InviteReward;
		PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
		PlayerPrefs.Save();

		if(Application.loadedLevelName.Contains("Mapa"))
		{
			GameObject.Find("INTERFACE HOLDER/_TopLeft/Coins/Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
			GameObject.Find("INTERFACE HOLDER/_TopLeft/Coins/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		}

	}

	//DEO ZA FRIEND INVITE - KRAJ

	//FEED - POCETAK
	
//	 string FeedIdNaCijiseZidKaci = "";
//	 string FeedLink = "http://apps.facebook.com/";
//	 string FeedLinkName = "Tekst linka-a";
	 string FeedLinkKratakOpis = " ";
//	 string FeedLinkDugacakOpis = "Trenutni ostrvo je ";
	string LinkSlike = "https://trello-attachments.s3.amazonaws.com/52fb9e010aaea80557f83f1f/1024x500/d64a4cc319932dde0630258aee32d7d4/Feature-Graphic.jpg";  //preporucuje se slika 600x600, minimum je 200x200
	 string LinkVideailiZvuka = "";
	 string FeedActionName = "";
	 string FeedActionLink = "";
	 string FeedReference = "";
	 bool IncludeFeedProperties = false;
	 Dictionary<string, string[]> FeedProperties = new Dictionary<string, string[]>();
	
	public void CallFBFeed(string ImeOstrva, int BrojNivoa)  //bug za iOS u xcode projektu ispravka http://stackoverflow.com/questions/21022557/unity-fb-sdk-on-ios-problems-with-fb-feed-function
	{
		Dictionary<string, string[]> feedProperties = null;
		if (IncludeFeedProperties)
		{
			feedProperties = FeedProperties;
		}
		//string FeedLink = "http://apps.facebook.com/" + FB.AppId;
		string FeedLink = "https://www.facebook.com/pages/Banana-Island/636650059721490";
		FB.Feed(
			"",
			FeedLink,
			LanguageManager.LevelCompleted,
			FeedLinkKratakOpis,
			ImeOstrva+" - "+LanguageManager.Level+" "+BrojNivoa,
			LinkSlike,
			LinkVideailiZvuka,
			FeedActionName,
			FeedActionLink,
			FeedReference,
			feedProperties,
			FeedCallback
			);
	}

	void FeedCallback(FBResult result)
	{

	}
	
	//FEED - KRAJ

	//FACEBOOK SHARE - POCETAK
	//FACEBOOK SHARE - KRAJ

	public void ProveriPermisije()
	{
		FB.API("/me/permissions", Facebook.HttpMethod.GET, MyPermissionsCallback); 
	}

	public void MyPermissionsCallback(FBResult result)
	{

		
		if (result.Error != null)                                                                                                  
		{                                                                                                                          
			// Let's just try again                                                                                                
			//FB.API("/me/permissions", Facebook.HttpMethod.GET, MyPermissionsCallback);
			StagesParser.Instance.UgasiLoading();
			return;                                                                                                                
		}
		else
		{
			Dictionary<string, object> rez= Facebook.MiniJSON.Json.Deserialize(result.Text) as Dictionary<string,object>;



			object data = rez["data"] as object;
			object PodData=rez["data"];
			List<object> podaci = new List<object>();
			podaci=(List<object>)PodData;
			object Permisija;
			int BrojPermisija = podaci.Count;
			permisija=new string[BrojPermisija];
			statusPermisije=new string[BrojPermisija];
			for(int i=0;i<BrojPermisija;i++)
			{
				Dictionary<string, object> Permisije = podaci[i] as Dictionary<string, object>;
			
					permisija[i]=(string)Permisije["permission"];
					statusPermisije[i]=(string)Permisije["status"];

			}

			bool publishActions = false;

			for(int j=0;j<BrojPermisija;j++)
			{
				if(permisija[j]=="publish_actions")
				{
					publishActions = true;
					if(statusPermisije[j]=="granted")
					{
						string imeOstrva = "";
						switch(StagesParser.currSetIndex)
						{
						case 0: imeOstrva = LanguageManager.BananaIsland; break;
						case 1: imeOstrva = LanguageManager.SavannaIsland; break;
						case 2: imeOstrva = LanguageManager.JungleIsland; break;
						case 3: imeOstrva = LanguageManager.TempleIsland; break;
						case 4: imeOstrva = LanguageManager.VolcanoIsland; break;
						case 5: imeOstrva = LanguageManager.FrozenIsland; break;
						}
						CallFBFeed(imeOstrva, StagesParser.currStageIndex + 1);

					}
					else
					{
						MestoPozivanjaLogina = 3;
						FB.Login(permissions,LoginCallback);
					}
				}
			}
			if(!publishActions)
			{
				MestoPozivanjaLogina = 3;
				FB.Login(permissions,LoginCallback);
			}


		}
	}

	public void proveraPublish_ActionPermisije()
	{
		FB.API("/me/permissions", Facebook.HttpMethod.GET, Publish_ActionsCallback); 
	}

	public void Publish_ActionsCallback(FBResult result)
	{
		
		if (result.Error != null)                                                                                                  
		{                                                                                                                          
			// Let's just try again                                                                                                
			//FB.API("/me/permissions", Facebook.HttpMethod.GET, Publish_ActionsCallback);
			StagesParser.Instance.UgasiLoading();
			return; 
		}
		else
		{
			Dictionary<string, object> rez= Facebook.MiniJSON.Json.Deserialize(result.Text) as Dictionary<string,object>;
			
			
			
			object data = rez["data"] as object;
			object PodData=rez["data"];
			List<object> podaci = new List<object>();
			podaci=(List<object>)PodData;
			object Permisija;
			int BrojPermisija = podaci.Count;
			permisija=new string[BrojPermisija];
			statusPermisije=new string[BrojPermisija];
			for(int i=0;i<BrojPermisija;i++)
			{
				Dictionary<string, object> Permisije = podaci[i] as Dictionary<string, object>;
				
				permisija[i]=(string)Permisije["permission"];
				statusPermisije[i]=(string)Permisije["status"];				
			}

			odobrioPublishActions = false;
			
			for(int j=0;j<BrojPermisija;j++)
			{
				if(permisija[j]=="publish_actions")
				{
					if(statusPermisije[j]=="granted")
					{
						odobrioPublishActions = true;
						SetFacebookHighScore(scoreToSet);
					}

				}
			}

		}
	}

	// Uzimanje rodjendana korisnika - POCETAK  trazi dodatnu permisiju user_birthday
	public void GetRodjendan()
	{
		FB.API("/me?fields=birthday", Facebook.HttpMethod.GET, MyBirthdayCallback); 
	}

	public void MyBirthdayCallback(FBResult result)                                                                                        
	{                                                                                                                              
		
		if (result.Error != null)                                                                                                  
		{                                                                                                                          
			// Let's just try again                                                                                                
			//FB.API("/me?fields=birthday", Facebook.HttpMethod.GET, MyBirthdayCallback);
			StagesParser.Instance.UgasiLoading();
			return;                                                                                                                
		}
		else
		{
			Dictionary<string, object> rez= Facebook.MiniJSON.Json.Deserialize(result.Text) as Dictionary<string,object>;
			UserRodjendan=rez["birthday"].ToString();
		}
		                                                                   
	}
	// Uzimanje rodjendana korisnika - KRAJ

	//SCORE KORISNIKA - POCETAK
	public void SetFacebookHighScore(int trenutniScore)
	{
		if(FB.IsLoggedIn)
		{
//			Dictionary<string, string> probaScore=new Dictionary<string, string>(){{"Score", trenutniScore.ToString()}};
			Dictionary<string, string> wwwForm = new Dictionary<string, string>();
			wwwForm["score"] = trenutniScore.ToString();

			FB.API("/me/scores?", Facebook.HttpMethod.POST, SetFacebookScoreCallback, wwwForm);
		} 
	}

	public void SetFacebookScoreCallback(FBResult result)
	{
		if(result.Error != null)
		{
		}
		else
		{
			if(resetovanScoreNaNulu == 2)
				resetovanScoreNaNulu = 1;
		}

	}

	public void GetFacebookHighScore()
	{
		FB.API("me?fields=scores",Facebook.HttpMethod.GET,GetFacebookHighScoreCallback);   //vraca samo user HighScore
//		FB.API("1474610946102580?fields=scores",Facebook.HttpMethod.GET,GetScoreCallback);  // vraca sve HighScorove
	}

	public void GetFacebookHighScoreCallback(FBResult result)
	{
		if(result.Error != null)
		{
		}
		else
		{

			Dictionary<string, object> rez= Facebook.MiniJSON.Json.Deserialize(result.Text) as Dictionary<string,object>;
			object scores = rez["scores"] as object;
			object HighScore;
			List<object> data = new List<object>();
			if(rez.TryGetValue("scores", out HighScore))
			{

				data = (List<object>)(((Dictionary<string,object>)HighScore) ["data"]);
				Dictionary<string, object> DataDic=((Dictionary<string,object>)(data[0]));
				object proba = DataDic["score"];


			}

		}
		
	}
	//SCORE KORISNIKA - KRAJ

	//SCORE SVIH PRIJATELJA - POCETAK

	public void GetFacebookFriendScores()
	{
		//1474610946102580?fields=scores ovaj upit vraca sa scores na pocetku, pazi zbog Dictionary sta koristis
		FB.API("1609658469261083/scores",Facebook.HttpMethod.GET,GetFacebookFriendScoresCallback);  // vraca sve HighScorove Prijatelja 
	}

	public void GetFacebookFriendScoresCallback(FBResult result)
	{
		if(result.Error != null)
		{
			StagesParser.Instance.UgasiLoading();
		}
		else
		{
			
			Dictionary<string, object> rez= Facebook.MiniJSON.Json.Deserialize(result.Text) as Dictionary<string,object>;
			object data = rez["data"] as object;
			object PodData=rez["data"];
			List<object> podaci = new List<object>();
			podaci=(List<object>)PodData;
			int BrojKorisnika = podaci.Count;
			Korisnici=new List<string>();
			Scorovi=new List<string>();
			Imena=new List<string>();
			object User;
			string UserPodaci;
			string IdPodaci;
			NumberOfFriends=podaci.Count;
			for(int i=0;i<podaci.Count; i++)
			{
				Dictionary<string, object> CelinaUser = podaci[i] as Dictionary<string, object>;

				if(CelinaUser.TryGetValue("user", out User))
				{
					UserPodaci = (string)(((Dictionary<string,object>)User) ["name"]);
					IdPodaci = (string)(((Dictionary<string,object>)User) ["id"]);
					Korisnici.Add(IdPodaci);
					Scorovi.Add(CelinaUser["score"].ToString());
					Imena.Add(UserPodaci);
				}

			}
			if(MestoPozivanjaLogina == 1)
			{
				for(int i=0;i<10;i++)
				{
					Camera.main.GetComponent<MainScene>().FriendsObjects[i].gameObject.SetActive(true);
					Camera.main.GetComponent<MainScene>().FriendsObjects[i].FindChild("FB Invite").gameObject.SetActive(false);
					Camera.main.GetComponent<MainScene>().FriendsObjects[i].FindChild("Friend").gameObject.SetActive(true);
				}



			
				for(int j=0;j<Scorovi.Count;j++)
				{
					int broj=1+j;
					if(j<10)
					{
						Camera.main.GetComponent<MainScene>().FriendsObjects[j].gameObject.SetActive(true);
		//				MainScene.FriendsObjects[j].gameObject.SetActive(false);

						GameObject.Find("Pts Number "+broj.ToString()).GetComponent<TextMesh>().text=Scorovi[j];
						GameObject.Find("Pts Number "+broj.ToString()).GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

						GameObject.Find("Name "+broj.ToString()).GetComponent<TextMesh>().text=Imena[j];
						GameObject.Find("Name "+broj.ToString()).GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

						if(Korisnici[j]==FB.UserId)
						{
							Camera.main.GetComponent<MainScene>().FriendsObjects[j].Find("Friend/LeaderboardYou").GetComponent<SpriteRenderer>().enabled=true;
						}
					}
				}

				for(int j=Imena.Count;j<10;j++)
				{
					Camera.main.GetComponent<MainScene>().FriendsObjects[j].FindChild("FB Invite").gameObject.SetActive(true);
					Camera.main.GetComponent<MainScene>().FriendsObjects[j].FindChild("FB Invite/Coin Number").GetComponent<TextMesh>().text = "+" + StagesParser.InviteReward;
					Camera.main.GetComponent<MainScene>().FriendsObjects[j].FindChild("FB Invite/Coin Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,false);
					Camera.main.GetComponent<MainScene>().FriendsObjects[j].FindChild("Friend").gameObject.SetActive(false);
				}
				ObjLeaderboard.Leaderboard=true;  
				SwipeControlLeaderboard.controlEnabled = true;
			}
			StartCoroutine("GetFriendPictures");
			StartCoroutine("TrenutniNivoSvihPrijatelja");
		}
		
	}

	IEnumerator GetFriendPictures()
	{
		int j=0;
		if(Ulogovan)
		{
			while(j<Korisnici.Count)
			{
				WaitForFacebook=false;
				GetFacebookFriendPicture(Korisnici[j]);
				while(!WaitForFacebook && Ulogovan)
				{
					yield return null;
				}
				j++;
			}
		
			BrojPrijatelja = 0;
			yield return null;
			if(zavrsioUcitavanje)
			{
				StagesParser.Instance.UgasiLoading();
				PlayerPrefs.SetInt("Logovan",1);
				if(MestoPozivanjaLogina == 1)
					RefreshujScenu1PosleLogina();
				else if(MestoPozivanjaLogina == 2)
					RefreshujScenu2PosleLogina();
				else if(MestoPozivanjaLogina == 3)
					RefreshujScenu3PosleLogina();
				zavrsioUcitavanje = false;
			}
			else
			{
				zavrsioUcitavanje = true;
			}
		}
		yield return null;
	}
	//SCORE SVIH PRIJATELJA - KRAJ

	//LISTA SVIH PRIJATELJA KOJI IGRAJU IGRU - START
	int nesto=0;
	public void SpisakSvihFacebookPrijatelja()
	{
//		FB.API("me/friends/?fields=installed,name,picture",Facebook.HttpMethod.GET,SviPrijateljiFacebookCallback);
		FB.API("/fql?q=SELECT+uid,name,pic_square+FROM+user+WHERE+is_app_user=1+AND+uid+IN+(SELECT+uid2+FROM+friend+WHERE+uid1=me())", Facebook.HttpMethod.GET, SviPrijateljiFacebookCallback) ;
	}

	public void SviPrijateljiFacebookCallback(FBResult result)
	{
		if(result.Error != null)
		{
			StagesParser.Instance.UgasiLoading();
		}
		else
		{

			Dictionary<string, object> rez= Facebook.MiniJSON.Json.Deserialize(result.Text) as Dictionary<string,object>;
			object PodData=rez["data"];
			List<object> podaci = new List<object>();
			podaci=(List<object>)PodData;
			int BrojKorisnika = podaci.Count;
			Prijatelji = new List<string>();
			object User;
			string UserPodaci;
			for(int i=0;i<podaci.Count; i++)
			{
				Dictionary<string, object> CelinaUser = podaci[i] as Dictionary<string, object>;
				if(CelinaUser.TryGetValue("name", out User))
				{
					int broj=1+i;
					nesto++;
					string op=CelinaUser["uid"].ToString();
					Prijatelji.Add(op);

				}
	
			}

		}
	}

	public int BrojPrijatelja=0;
	public void GetFacebookFriendPicture(string PrijateljevID)
	{
		if(Ulogovan)
		{
			prijateljevIDzaSliku = PrijateljevID;
			FB.API(PrijateljevID+"/picture?redirect=true&height=64&type=normal&width=64",Facebook.HttpMethod.GET, FacebookFriendPictureCallback);
		}
	}
	string URL;
	string prijateljevIDzaSliku;
	public void FacebookFriendPictureCallback(FBResult result)
	{

		if(result.Error != null)
		{
			StagesParser.Instance.UgasiLoading();
		}
		else
		{
			BrojPrijatelja++;
			if(BrojPrijatelja>=Korisnici.Count)//bilo je count-1
			{
				WaitForFacebook=true;
			}
			else
			{
			}

			IDiSlika tempPrijatelj = new IDiSlika();
			tempPrijatelj.PrijateljID = prijateljevIDzaSliku;
			tempPrijatelj.profilePicture = result.Texture;
			ProfileSlikePrijatelja.Add(tempPrijatelj);
			//ProfileSlikePrijatelja.Add(result.Texture);
			if(MestoPozivanjaLogina == 1)
			{
				switch(BrojPrijatelja)
				{
				case 1:
	//				FriendPic1=result.Texture;
					GameObject.Find("Friends Level Win Picture 1").GetComponent<Renderer>().material.mainTexture=ProfileSlikePrijatelja[0].profilePicture;
					WaitForFacebook=true;
				break;

				case 2:
	//				FriendPic2=result.Texture;
					GameObject.Find("Friends Level Win Picture 2").GetComponent<Renderer>().material.mainTexture=ProfileSlikePrijatelja[1].profilePicture;
					WaitForFacebook=true;
				break;

				case 3:
	//				FriendPic3=result.Texture;
					GameObject.Find("Friends Level Win Picture 3").GetComponent<Renderer>().material.mainTexture=ProfileSlikePrijatelja[2].profilePicture;
					WaitForFacebook=true;
				break;
					
				case 4:
	//				FriendPic4=result.Texture;
					GameObject.Find("Friends Level Win Picture 4").GetComponent<Renderer>().material.mainTexture=ProfileSlikePrijatelja[3].profilePicture;
					WaitForFacebook=true;
				break;
				
				case 5:
	//				FriendPic5=result.Texture;
					GameObject.Find("Friends Level Win Picture 5").GetComponent<Renderer>().material.mainTexture=ProfileSlikePrijatelja[4].profilePicture;
					WaitForFacebook=true;
				break;

				case 6:
					//				FriendPic5=result.Texture;
					GameObject.Find("Friends Level Win Picture 6").GetComponent<Renderer>().material.mainTexture=ProfileSlikePrijatelja[5].profilePicture;
					WaitForFacebook=true;
					break;

				case 7:
					//				FriendPic5=result.Texture;
					GameObject.Find("Friends Level Win Picture 7").GetComponent<Renderer>().material.mainTexture=ProfileSlikePrijatelja[6].profilePicture;
					WaitForFacebook=true;
					break;

				case 8:
					//				FriendPic5=result.Texture;
					GameObject.Find("Friends Level Win Picture 8").GetComponent<Renderer>().material.mainTexture=ProfileSlikePrijatelja[7].profilePicture;
					WaitForFacebook=true;
					break;

				case 9:
					//				FriendPic5=result.Texture;
					GameObject.Find("Friends Level Win Picture 9").GetComponent<Renderer>().material.mainTexture=ProfileSlikePrijatelja[8].profilePicture;
					WaitForFacebook=true;
					break;

				case 10:
					//				FriendPic5=result.Texture;
					GameObject.Find("Friends Level Win Picture 10").GetComponent<Renderer>().material.mainTexture=ProfileSlikePrijatelja[9].profilePicture;
					WaitForFacebook=true;
					break;

				}
				if(BrojPrijatelja > 10)
					WaitForFacebook=true;
			}
			else
				WaitForFacebook=true;

		}
	}

	//LISTA SVIH PRIJATELJA KOJI IGRAJU IGRU - END 

	//UZIMANJE SVIH ACHIEVMENTA IGRE - POCETAK
	public void GetFacebookGameAchievements()
	{
		FB.API(FB.AppId+"/achievements",Facebook.HttpMethod.GET,GetFacebookGameAchievementsCallback);  // vraca sve achivmente iz igre
	}

	public void GetFacebookGameAchievementsCallback(FBResult result)
	{
		if(result.Error != null)
		{
		}
		else
		{

			Dictionary<string, object> rez= Facebook.MiniJSON.Json.Deserialize(result.Text) as Dictionary<string,object>;

			object PodData=rez["data"];
			List<object> podaci = new List<object>();
			podaci=(List<object>)PodData;
			int BrojAchivmenta = podaci.Count;

			object Achivment;
			string AchivmentPodaci;
			for(int i=0;i<podaci.Count; i++)
			{
				Dictionary<string, object> CelinaAchivment = podaci[i] as Dictionary<string, object>;
				//				string id = (string) CelinaUser["id"];
				//				GetFriendPicture(id);
				if(CelinaAchivment.TryGetValue("title", out Achivment))
				{
					int broj=1+i;
					
				}
				
				
			}
		}
	}
	//UZIMANJE SVIH ACHIEVMENTA IGRE _ KRAJ

	//POSTOVANJE ACHIEVMENTA - POCETAK
	public void DodajFacebookAchievement(string URLAchivmenta)
	{
		Dictionary<string, string> dict = new Dictionary<string, string>();
		dict["achievement"]= URLAchivmenta;
		FB.API("me/achievements", Facebook.HttpMethod.POST, DodajFacebookAchievementCallback,dict);
	}
	
	public void DodajFacebookAchievementCallback(FBResult result)
	{
		if(result.Error != null)
		{
		}
		else
		{
		}
	}
	//POSTOVANJE ACHIEVMENTA - KRAJ

	//BRISANJE ACHIEVMENTA-POCETAK
	public void ObrisiFacebookAchievement(string UrlACH)
	{

		FB.API("me/achievements?achievement="+UrlACH, Facebook.HttpMethod.DELETE, ObrisiFacebookAchievementCallback);
	}
	
	public void ObrisiFacebookAchievementCallback(FBResult result)
	{
		if(result.Error != null)
		{
		}
		else
		{
		}
	}
	//BRISANJE ACHIEVMENTA-KRAJ

	//PROVERA DA LI USER IMA ACHIVMENT - POCETAK
	public void ProveraFacebookAchievmenta()
	{
		FB.API("me/achievements", Facebook.HttpMethod.GET, ProveraAchievmentaCallback);
	}

	public void ProveraAchievmentaCallback(FBResult result)
	{
		if(result.Error != null)
		{
		}
		else
		{


		}
	}

	//PROVERA DA LI USER IMA ACHIVMENT - KRAJ











	//SERVERSKI DEO - POCETAK


	public void InicijalizujKorisnika(string KorisnikovID, int numCoins, int Score, string Jezik, int Banana, int Shield, int Magnet, int DoubleCoins, string UserSveKupovineHats, string UserSveKupovineShirts, string UserSveKupovineBackPacks)    // poziva se samo jednom, prilikom prvog pokretanja aplikacije
	{
		if(FB.IsLoggedIn)
		{

			Korisnik["UserID"] = KorisnikovID;
			Korisnik["Name"]=UserName;
			Korisnik["Coins"] = numCoins;
			Korisnik["Score"] = Score;
			Korisnik["Language"] = Jezik;
			Korisnik["Banana"] = Banana;
			Korisnik["PowerShield"] = Shield;
			Korisnik["PowerMagnet"] = Magnet;
			Korisnik["PowerDoubleCoins"] = DoubleCoins;
			Korisnik["UserSveKupovineHats"] = UserSveKupovineHats;
			Korisnik["UserSveKupovineShirts"] = UserSveKupovineShirts;
			Korisnik["UserSveKupovineBackPacks"] = UserSveKupovineBackPacks;
			Korisnik["GlavaItems"]=0;
			Korisnik["TeloItems"]=0;
			Korisnik["LedjaItems"]=0;
			Korisnik["Usi"]=true;
			Korisnik["Kosa"]=true;
			Korisnik["NumberOfFriends"] = NumberOfFriends;
			Korisnik["OdgledaoShopTutorial"] = StagesParser.otvaraoShopNekad;
			Korisnik["JezikPromenjen"] = StagesParser.jezikPromenjen;
			Task saveTask = Korisnik.SaveAsync();

			//PREFS DEO
		}
		else
		{
			//PREFS DEO
		}
	}

	public void ProcitajPodatkeKorisnika()
	{
		StagesParser.languageBefore = LanguageManager.chosenLanguage;
		if(FB.IsLoggedIn)
		{
			var query1 = ParseObject.GetQuery("User").WhereEqualTo("UserID",User);
			query1.FirstAsync().ContinueWith(t =>
			                                {

				if(t.IsCompleted)
				{

					ParseObject obj = t.Result;

					UserCoins = obj.Get<int>("Coins");
					UserScore = obj.Get<int>("Score");
					UserLanguage = obj.Get<string>("Language");
					UserBanana = obj.Get<int>("Banana");
					UserPowerMagnet = obj.Get<int>("PowerMagnet");
					UserPowerShield = obj.Get<int>("PowerShield");
					UserPowerDoubleCoins = obj.Get<int>("PowerDoubleCoins");
					UserSveKupovineHats = obj.Get<string>("UserSveKupovineHats");
					UserSveKupovineShirts = obj.Get<string>("UserSveKupovineShirts");
					UserSveKupovineBackPacks = obj.Get<string>("UserSveKupovineBackPacks");
					StagesParser.otvaraoShopNekad = obj.Get<int>("OdgledaoShopTutorial");
					StagesParser.jezikPromenjen = obj.Get<int>("JezikPromenjen");

					GlavaItem = obj.Get<int>("GlavaItems");
					TeloItem = obj.Get<int>("TeloItems");
					LedjaItem = obj.Get<int>("LedjaItems");
					Usi = obj.Get<bool>("Usi");
					Kosa = obj.Get<bool>("Kosa");

					KorisnikoviPodaciSpremni=true;

				}
				else if(t.IsFaulted || t.IsCanceled)
				{
					//PREFS DEO
					UserCoins = StagesParser.currentMoney;
					UserScore = StagesParser.currentPoints;
					UserLanguage = LanguageManager.chosenLanguage;
					UserBanana = StagesParser.currentBananas;
					UserPowerMagnet = StagesParser.powerup_magnets;
					UserPowerShield = StagesParser.powerup_shields;
					UserPowerDoubleCoins = StagesParser.powerup_doublecoins;
					UserSveKupovineHats = StagesParser.svekupovineGlava;
					UserSveKupovineShirts = StagesParser.svekupovineMajica;
					UserSveKupovineBackPacks = StagesParser.svekupovineLedja;
					
					GlavaItem = StagesParser.glava;
					TeloItem = StagesParser.majica;
					LedjaItem = StagesParser.ledja;
					Usi = StagesParser.imaUsi;
					Kosa = StagesParser.imaKosu;

					KorisnikoviPodaciSpremni=true;
				}




				
			});
		}
		else
		{
			//PREFS DEO
			UserCoins = StagesParser.currentMoney;
			UserScore = StagesParser.currentPoints;
			UserLanguage = LanguageManager.chosenLanguage;
			UserBanana = StagesParser.currentBananas;
			UserPowerMagnet = StagesParser.powerup_magnets;
			UserPowerShield = StagesParser.powerup_shields;
			UserPowerDoubleCoins = StagesParser.powerup_doublecoins;
			UserSveKupovineHats = StagesParser.svekupovineGlava;
			UserSveKupovineShirts = StagesParser.svekupovineMajica;
			UserSveKupovineBackPacks = StagesParser.svekupovineLedja;
			
			GlavaItem = StagesParser.glava;
			TeloItem = StagesParser.majica;
			LedjaItem = StagesParser.ledja;
			Usi = StagesParser.imaUsi;
			Kosa = StagesParser.imaKosu;
			
			KorisnikoviPodaciSpremni=true;
		}
	}
	public void ProveriKorisnika()
	{
		ListaStructPrijatelja.Clear();
		StartCoroutine("DaLiPostojiKorisnik");
	}
	IEnumerator DaLiPostojiKorisnik()
	{
		if(FB.IsLoggedIn)
		{
			var query = ParseObject.GetQuery("User").WhereEqualTo("UserID",User).Limit(1);
			var queryTask= query.FindAsync();
			//			queryTask.FirstAsync();
			while(!queryTask.IsCompleted)yield return null;
			
			ReadOnlyCollection<Parse.ParseObject> results =(ReadOnlyCollection<Parse.ParseObject>) queryTask.Result;
			
			if(results.Count>0)
			{
				ProcitajPodatkeKorisnika();
				GetFacebookFriendScores();
				nePostojiKorisnik = false;
				//PlayerPrefs.SetInt("Logout",0); //ovako ne moze zato sto ako se korisnik izloguje, onda igra kao guest i predje vise, kad se opet uloguje isti user nema uslova da udje u compare scores i ne povlaci progres
				//"OVAKO, DA SE PROVERI DA LI CE LEPO DA ODRADI SVE ZA JOS PAR NOVIH KORISNIKA, TJ. DA LI MOZE BEZ OVOG REDA IZNAD!!!!!"
				//"DRUGO, DA SE PROVERI DA LI SE SNIMA LASTLOGGEDUSER, DA LI SE CITA I GDE SE BRISE (LINIJA 192 U STAGES PARSER DA SE UBACI LOG, I JOS GDE TREBA DA SE PROVERAVA ISTO)!!!!!"
			}
			else
			{
				nePostojiKorisnik = true;

				StagesParser.currentMoney += StagesParser.LoginReward;
				PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
				PlayerPrefs.Save();
				if(MestoPozivanjaLogina == 1)
				{
					GameObject temp = GameObject.Find("CoinsReward");
					//temp.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
					//temp.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
					temp.GetComponent<Animation>().Play("CoinsRewardDolazak");
					StartCoroutine(StagesParser.Instance.moneyCounter(2000,temp.transform.Find("Coins Number").GetComponent<TextMesh>(),true));
					Invoke("SkloniCoinsReward",1.2f);
				}
				else
				{
					StartCoroutine(StagesParser.Instance.moneyCounter(StagesParser.LoginReward,GameObject.Find("_GUI").transform.Find("INTERFACE HOLDER/_TopLeft/Coins/Coins Number").GetComponent<TextMesh>(),true));
				}
				StagesParser.ServerUpdate = 1;
				scoreToSet = StagesParser.currentPoints;
				proveraPublish_ActionPermisije();
				//SetFacebookHighScore(StagesParser.currentPoints);

				InicijalizujKorisnika(User,StagesParser.currentMoney,StagesParser.currentPoints,LanguageManager.chosenLanguage,StagesParser.currentBananas,StagesParser.powerup_shields,StagesParser.powerup_magnets,StagesParser.powerup_doublecoins,StagesParser.svekupovineGlava,StagesParser.svekupovineMajica,StagesParser.svekupovineLedja);
				InicijalizujScoreNaNivoima(StagesParser.StarsPoNivoima,StagesParser.PointsPoNivoima,StagesParser.maxLevel, StagesParser.bonusLevels);
				GetFacebookFriendScores();
			}

			
		}
		else
		{
			//PREFS DEO
			ProcitajPodatkeKorisnika();
		}
	}

	public void UpdateujPodatkeKorisnika(int BrojCoina, int Score, string Jezik, int Banana, int PowerMagnet, int PowerShield, int PowerDoubleCoins, string UserSveKupovineHats, string UserSveKupovineShirts,string UserSveKupovineBackPacks,int Ledja, int Glava, int Telo, bool Usi, bool Kosa, int NumberOfFriends)
	{
		if(FB.IsLoggedIn)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("User").WhereEqualTo("UserID",User);
			query.FirstAsync().ContinueWith(t =>
			                                {
				
				if(t.IsCompleted)
				{
					ParseObject obj = t.Result;
					obj["Coins"] = BrojCoina;
					obj["Score"] = Score;
					obj["Language"] = Jezik;
					obj["Banana"] = Banana;
					obj["PowerMagnet"] = PowerMagnet;
					obj["PowerShield"] = PowerShield;
					obj["PowerDoubleCoins"] = PowerDoubleCoins;
					obj["UserSveKupovineHats"] = UserSveKupovineHats;
					obj["UserSveKupovineShirts"] = UserSveKupovineShirts;
					obj["UserSveKupovineBackPacks"] = UserSveKupovineBackPacks;
					obj["GlavaItems"] = Glava;
					obj["TeloItems"] = Telo;
					obj["LedjaItems"] = Ledja;
					obj["Usi"] = Usi;
					obj["Kosa"] = Kosa;
					obj["NumberOfFriends"] = NumberOfFriends;
					obj["OdgledaoShopTutorial"] = StagesParser.otvaraoShopNekad;
					obj["JezikPromenjen"] = StagesParser.jezikPromenjen;
					obj.SaveAsync();
					updatedSuccessfullyPodaciKorisnika = true;
					if(StagesParser.ServerUpdate == 1 && updatedSuccessfullyScoreNaNivoima)
					{
						StagesParser.ServerUpdate = 2;
						PlayerPrefs.SetInt("ServerUpdate",StagesParser.ServerUpdate);
						PlayerPrefs.Save();
					}
//					else if(StagesParser.ServerUpdate == 3)
//					{
//						StagesParser.ServerUpdate = 2;
//						FacebookManager.FacebookLogout();
//					}
				}
				else if(t.IsFaulted || t.IsCanceled)
				{
					//PREFS DEO
				}
				
			});
		}
		else
		{
			//PREFS DEO
		}
	}

	void SkloniCoinsReward()
	{
		GameObject.Find("CoinsReward").GetComponent<Animation>().Play("CoinsRewardOdlazak");
	}

	public void SacuvajBrojNovcica(int numCoins)    // poziva se prilikom update-a broja coina, na kraju svakog nivo-a, ili izlaska iz igre
	{
		if(FB.IsLoggedIn)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("User");
			query.WhereEqualTo("UserID",User);
			query.FirstAsync().ContinueWith(t =>
			                                {
				if(t.IsCompleted)
				{
					ParseObject obj = t.Result;
					obj["Coins"] = numCoins;
					obj.SaveAsync();
				}
				else if(t.IsFaulted || t.IsCanceled)
				{
					//PREFS DEO
				}

			});	
		}
		else
		{
			//PREFS DEO
		}


	}
	public void ProcitajBrojNovcica()  // poziva se prilikom citanja broja coina sa servera
	{
		if(FB.IsLoggedIn)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("User");
			query.WhereEqualTo("UserID",User);
			query.FirstAsync().ContinueWith(t =>
			                                {

				if(t.IsCompleted)
				{
					ParseObject obj = t.Result;
					int Coins = obj.Get<int>("Coins");
				}
				else if(t.IsFaulted || t.IsCanceled)
				{
					//PREFS DEO
				}

			});
		}
		else
		{
			//PREFS DEO
		}

	}



	public void SacuvajBrojBanana(int BrojBanana)
	{
		if(FB.IsLoggedIn)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("User");
			query.WhereEqualTo("UserID",User);
			query.FirstAsync().ContinueWith(t =>
			                                {
				if(t.IsCompleted)
				{
					ParseObject obj = t.Result;
					obj["Banana"] = BrojBanana;
					obj.SaveAsync();
				}
				else if(t.IsFaulted || t.IsCanceled)
				{
					//PREFS DEO
				}
				
			});	
		}
		else
		{
			//PREFS DEO
		}
	}

	public void ProcitajBrojBanana()
	{
		if(FB.IsLoggedIn)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("User");
			query.WhereEqualTo("UserID",User);
			query.FirstAsync().ContinueWith(t =>
			                                {
				
				if(t.IsCompleted)
				{
					ParseObject obj = t.Result;
					int Banana = obj.Get<int>("Banana");
				}
				else if(t.IsFaulted || t.IsCanceled)
				{
					//PREFS DEO
				}
				
			});
		}
		else
		{
			//PREFS DEO
		}
	}


	public void SacuvajScore(int GlobalScore)
	{
		if(FB.IsLoggedIn)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("User");
			query.WhereEqualTo("UserID",User);
			query.FirstAsync().ContinueWith(t =>
			                                {
				if(t.IsCompleted)
				{
					ParseObject obj = t.Result;
					obj["Score"] = GlobalScore;
					obj.SaveAsync();
				}
				else if(t.IsFaulted || t.IsCanceled)
				{
					//PREFS DEO
				}
				
			});	
		}
		else
		{
			//PREFS DEO
		}
	}
	
	public void ProcitajScore()
	{
		if(FB.IsLoggedIn)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("User");
			query.WhereEqualTo("UserID",User);
			query.FirstAsync().ContinueWith(t =>
			                                {
				
				if(t.IsCompleted)
				{
					ParseObject obj = t.Result;
					int GlobalScore = obj.Get<int>("Score");
				}
				else if(t.IsFaulted || t.IsCanceled)
				{
					//PREFS DEO
				}
				
			});
		}
		else
		{
			//PREFS DEO
		}
	}



	public void SacuvajLanguage(string NoviJezik)
	{
		if(FB.IsLoggedIn)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("User");
			query.WhereEqualTo("UserID",User);
			query.FirstAsync().ContinueWith(t =>
			                                {
				if(t.IsCompleted)
				{
					ParseObject obj = t.Result;
					obj["Language"] = NoviJezik;
					obj.SaveAsync();
				}
				else if(t.IsFaulted || t.IsCanceled)
				{
					//PREFS DEO
				}
				
			});	
		}
		else
		{
			//PREFS DEO
		}
	}
	
	public void ProcitajLanguage()
	{
		if(FB.IsLoggedIn)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("User");
			query.WhereEqualTo("UserID",User);
			query.FirstAsync().ContinueWith(t =>
			                                {
				
				if(t.IsCompleted)
				{
					ParseObject obj = t.Result;
					string OdabraniJezik = obj.Get<string>("Language");
				}
				else if(t.IsFaulted || t.IsCanceled)
				{
					//PREFS DEO
				}
				
			});
		}
		else
		{
			//PREFS DEO
		}
	}




	public void SacuvajPowerShield(int BrojPowerShield)
	{
		if(FB.IsLoggedIn)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("User");
			query.WhereEqualTo("UserID",User);
			query.FirstAsync().ContinueWith(t =>
			                                {
				if(t.IsCompleted)
				{
					ParseObject obj = t.Result;
					obj["PowerShield"] = BrojPowerShield;
					obj.SaveAsync();
				}
				else if(t.IsFaulted || t.IsCanceled)
				{
					//PREFS DEO
				}
				
			});	
		}
		else
		{
			//PREFS DEO
		}
	}
	
	public void ProcitajPowerShield()
	{
		if(FB.IsLoggedIn)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("User");
			query.WhereEqualTo("UserID",User);
			query.FirstAsync().ContinueWith(t =>
			                                {
				
				if(t.IsCompleted)
				{
					ParseObject obj = t.Result;
					int PowerShieldBroj = obj.Get<int>("PowerShield");
				}
				else if(t.IsFaulted || t.IsCanceled)
				{
					//PREFS DEO
				}
				
			});
		}
		else
		{
			//PREFS DEO
		}
	}



	public void SacuvajPowerMagnet(int BrojPowerMagnet)
	{
		if(FB.IsLoggedIn)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("User");
			query.WhereEqualTo("UserID",User);
			query.FirstAsync().ContinueWith(t =>
			                                {
				if(t.IsCompleted)
				{
					ParseObject obj = t.Result;
					obj["PowerMagnet"] = BrojPowerMagnet;
					obj.SaveAsync();
				}
				else if(t.IsFaulted || t.IsCanceled)
				{
					//PREFS DEO
				}
				
			});	
		}
		else
		{
			//PREFS DEO
		}
	}
	
	public void ProcitajPowerMagnet()
	{
		if(FB.IsLoggedIn)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("User");
			query.WhereEqualTo("UserID",User);
			query.FirstAsync().ContinueWith(t =>
			                                {
				
				if(t.IsCompleted)
				{
					ParseObject obj = t.Result;
					int PowerMagnetBroj = obj.Get<int>("PowerMagnet");
				}
				else if(t.IsFaulted || t.IsCanceled)
				{
					//PREFS DEO
				}
				
			});
		}
		else
		{
			//PREFS DEO
		}
	}



	public void SacuvajPowerDoubleCoins(int BrojPowerDoubleCoins)
	{
		if(FB.IsLoggedIn)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("User");
			query.WhereEqualTo("UserID",User);
			query.FirstAsync().ContinueWith(t =>
			                                {
				if(t.IsCompleted)
				{
					ParseObject obj = t.Result;
					obj["PowerDoubleCoins"] = BrojPowerDoubleCoins;
					obj.SaveAsync();
				}
				else if(t.IsFaulted || t.IsCanceled)
				{
					//PREFS DEO
				}
				
			});	
		}
		else
		{
			//PREFS DEO
		}
	}
	
	public void ProcitajPowerDoubleCoins()
	{
		if(FB.IsLoggedIn)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("User");
			query.WhereEqualTo("UserID",User);
			query.FirstAsync().ContinueWith(t =>
			                                {
				
				if(t.IsCompleted)
				{
					ParseObject obj = t.Result;
					int PowerDoubleCoinsBroj = obj.Get<int>("PowerDoubleCoins");
				}
				else if(t.IsFaulted || t.IsCanceled)
				{
					//PREFS DEO
				}
				
			});
		}
		else
		{
			//PREFS DEO
		}
	}




	public void SacuvajSveMoci(int BrojPowerShield, int BrojPowerMagnet, int BrojPowerDoubleCoins)
	{
		if(FB.IsLoggedIn)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("User");
			query.WhereEqualTo("UserID",User);
			query.FirstAsync().ContinueWith(t =>
			                                {
				if(t.IsCompleted)
				{
					ParseObject obj = t.Result;
					obj["PowerShield"] = BrojPowerShield;
					obj["PowerMagnet"] = BrojPowerMagnet;
					obj["PowerDoubleCoins"] = BrojPowerDoubleCoins;
					obj.SaveAsync();
				}
				else if(t.IsFaulted || t.IsCanceled)
				{
					//PREFS DEO
				}
				
			});	
		}
		else
		{
			//PREFS DEO
		}
	}
	
	public void ProcitajSveMoci()
	{
		if(FB.IsLoggedIn)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("User");
			query.WhereEqualTo("UserID",User);
			query.FirstAsync().ContinueWith(t =>
			                                {
				
				if(t.IsCompleted)
				{
					ParseObject obj = t.Result;
					int PowerShieldBroj = obj.Get<int>("PowerShield");
					int PowerMagnetBroj = obj.Get<int>("PowerMagnet");
					int PowerDoubleCoinsBroj = obj.Get<int>("PowerDoubleCoins");

				}
				else if(t.IsFaulted || t.IsCanceled)
				{
					//PREFS DEO
				}
				
			});
		}
		else
		{
			//PREFS DEO
		}
	}



	public void SacuvajKupljeneStvari(string UserSveKupovineHats, string UserSveKupovineShirts, string UserSveKupovineBackPacks)
	{
		if(FB.IsLoggedIn)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("User");
			query.WhereEqualTo("UserID",User);
			query.FirstAsync().ContinueWith(t =>
			                                {
				if(t.IsCompleted)
				{
					ParseObject obj = t.Result;
					obj["UserSveKupovineHats"] = UserSveKupovineHats;
					obj["UserSveKupovineShirts"] = UserSveKupovineShirts;
					obj["UserSveKupovineBackPacks"] = UserSveKupovineBackPacks;
					obj.SaveAsync();
				}
				else if(t.IsFaulted || t.IsCanceled)
				{
					//PREFS DEO
				}
				
			});	
		}
		else
		{
			//PREFS DEO
		}
	}
	
	public void ProcitajKupljeneStvari()
	{
		if(FB.IsLoggedIn)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("User");
			query.WhereEqualTo("UserID",User);
			query.FirstAsync().ContinueWith(t =>
			                                {
				
				if(t.IsCompleted)
				{
					ParseObject obj = t.Result;
					UserSveKupovineHats = obj.Get<string>("UserSveKupovineHats");
					UserSveKupovineShirts = obj.Get<string>("UserSveKupovineShirts");
					UserSveKupovineHats = obj.Get<string>("UserSveKupovineHats");
				}
				else if(t.IsFaulted || t.IsCanceled)
				{
					//PREFS DEO
				}
				
			});
		}
		else
		{
			//PREFS DEO
		}
	}


	//PROGRES PO NIVOIMA - END
//	void UbaciNiz()
//	{
//		int[] nekiniz=new int[100];
//		for(int i=0;i<100;i++)
//		{
//			nekiniz[i]=i;
//		}
//		
//		if(FB.IsLoggedIn)
//		{
//			ParseQuery<ParseObject> query = ParseObject.GetQuery("LevelScore").WhereEqualTo("level", 3);
//			query.WhereEqualTo("UserID",User);
//			query.FirstAsync().ContinueWith(t =>
//			                                {
//				if(t.IsCompleted)
//				{
//					ParseObject obj = t.Result;
//					obj["Niz"] = nekiniz;
//					obj.SaveAsync();
//				}
//				else if(t.IsFaulted || t.IsCanceled)
//				{
//					//PREFS DEO
//				}
//				
//			});	
//		}
//		else
//		{
//			//PREFS DEO
//		}
//	}



	public void InicijalizujScoreNaNivoima(int[] NizBrojZvezdaPoNivou, int[] NizScorovaPoNivoima,int MaxNivo, string BonusLevels)
	{

			LevelScore["NumOfStars"]=NizBrojZvezdaPoNivou;
			LevelScore["LevelScore"]=NizScorovaPoNivoima;
			LevelScore["MaxLevel"]=MaxNivo;
			LevelScore ["BonusLevels"] = BonusLevels;
			LevelScore["UserID"] = User;
			Task saveTask = LevelScore.SaveAsync();
		
	}

	public void SacuvajScoreNaNivoima(int[] ScorePoNivoima, int[] BrojZvezdaPoNivoima, int TrenutniNivoIgraca, string BonusLevels)
	{
		if(FB.IsLoggedIn)
		{

			ParseQuery<ParseObject> query = ParseObject.GetQuery("LevelScore").WhereEqualTo("UserID",User);
			query.FirstAsync().ContinueWith(t =>
			                                {
				if(t.IsCompleted)
				{

					ParseObject obj = t.Result;

					obj["LevelScore"] = ScorePoNivoima;
					obj["NumOfStars"] = BrojZvezdaPoNivoima;
					obj["MaxLevel"] = TrenutniNivoIgraca;
					obj["BonusLevels"] = BonusLevels;
					obj.SaveAsync();
					updatedSuccessfullyScoreNaNivoima = true;

					if(StagesParser.ServerUpdate == 3)
					{
						StagesParser.ServerUpdate = 2;
						OKzaLogout = true;
					}
				}
				else if(t.IsFaulted || t.IsCanceled)
				{
					//PREFS DEO
				}


			});	
			if(StagesParser.ServerUpdate == 1 && updatedSuccessfullyPodaciKorisnika)
			{
				StagesParser.ServerUpdate = 2;
				PlayerPrefs.SetInt("ServerUpdate",StagesParser.ServerUpdate);
				PlayerPrefs.Save();
			}
		}
		else
		{
			//PREFS DEO
		}
	}

	public void ProcitajScoreNaNivoima()
	{

		if(FB.IsLoggedIn)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("LevelScore").WhereEqualTo("UserID",User);
			query.FirstAsync().ContinueWith(t =>
			                                {
				if(t.IsCompleted)
				{
					ParseObject obj = t.Result;
					IList<int> ScorePoNivoimaNiz = obj.Get<IList<int>>("LevelScore");
					IList<int> BrojZvezdaPoNivouNiz = obj.Get<IList<int>>("NumOfStars");
					TrenutniNivoIgraca=obj.Get<int>("MaxLevel");

				}
				else if(t.IsFaulted || t.IsCanceled)
				{
				}


			});	
		}
		else
		{
			//PREFS DEO

		}

	}



	public void ProcitajScorovePrijatelja(string FriendID)
	{
		int rBrUsera = 0;
		if(FB.IsLoggedIn)
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery("LevelScore").WhereEqualTo("UserID",FriendID);
			query.FirstAsync().ContinueWith(t =>
			                                {
				if(t.IsCompleted)
				{

					ParseObject obj = t.Result;
					IList<int> scorePoNivouPrijatelja = obj.Get<IList<int>>("LevelScore");
					IList<int> starsPoNivouPrijatelja = obj.Get<IList<int>>("NumOfStars");
					int MaxNivoPrijatelja = obj.Get<int>("MaxLevel");
					bonusLevels = obj.Get<string>("BonusLevels");
					StrukturaPrijatelja Prijatelj=new StrukturaPrijatelja
					{
						PrijateljID=FriendID
							
					};
					Prijatelj.MaxLevel=MaxNivoPrijatelja;
					Prijatelj.scores=scorePoNivouPrijatelja;
					Prijatelj.stars=starsPoNivouPrijatelja;
					ListaStructPrijatelja.Add (Prijatelj);
					WaitForFacebookFriend=true;

				}

				else if(t.IsFaulted || t.IsCanceled)
				{
					StagesParser.Instance.UgasiLoading();
				}


			});

		}
		else
		{
			//PREFS DEO

		}
	}


	//TRENUTNI NIVO svih prijatelja

	IEnumerator TrenutniNivoSvihPrijatelja()
	{
		int j=0;
		float timer = 0;
		while(j<Korisnici.Count)
		{
			WaitForFacebookFriend=false;
			ProcitajScorovePrijatelja(Korisnici[j]);
			while(!WaitForFacebookFriend && Ulogovan)
			{
				if(timer > 20)
					WaitForFacebookFriend = true;
				timer+=Time.deltaTime;
				yield return null;
			}
			j++;
		}
		if(resetovanScoreNaNulu == 1)
		{
			for(int i=0;i<ListaStructPrijatelja.Count;i++)
			{
				if(ListaStructPrijatelja[i].PrijateljID == FB.UserId)
				{
					for(int jj=0; jj<scorePoNivouPrijatelja.Length;jj++)
					{
						ListaStructPrijatelja[i].scores[jj] = 0;
					}
				}
			}
			resetovanScoreNaNulu = 0;
			StagesParser.Instance.UgasiLoading();
		}

		for(int s=0;s<ListaStructPrijatelja.Count;s++)
		{
			StrukturaPrijatelja Igrac=ListaStructPrijatelja[s];
			if(Igrac.PrijateljID==User)
			{
				indexUListaStructPrijatelja=s;
//				GameObject.Find("Test").renderer.material.mainTexture= ProfileSlikePrijatelja[s];
			}
//			for(int i=0;i<100;i++)
//			{
//			}
		}



		//if(StagesParser.lastLoggedUser != System.String.Empty || PlayerPrefs.HasKey("Logout")) //ako se vec nekad logovao i ako se loguje kao postojeci user
		if(Ulogovan)
		{
			if(!nePostojiKorisnik)
				StagesParser.Instance.CompareScores();
			else
			{
				if(zavrsioUcitavanje)
				{
					StagesParser.Instance.UgasiLoading(); //ako se loguje kao novi user, ne treba da ulazi u CompareScores(), treba samo da ugasi loading
					PlayerPrefs.SetInt("Logovan",1);
					if(MestoPozivanjaLogina == 1)
						RefreshujScenu1PosleLogina();
					else if(MestoPozivanjaLogina == 2)
						RefreshujScenu2PosleLogina();
					else if(MestoPozivanjaLogina == 3)
						RefreshujScenu3PosleLogina();
					zavrsioUcitavanje = false;
				}
				else
				{
					zavrsioUcitavanje = true;
				}
			}
		}
		else
		{
			StagesParser.Instance.UgasiLoading();
		}

	
	}

	//SERVERSKI DEO - KRAJ


}
