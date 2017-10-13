using UnityEngine;
using System.Collections;

public class AllMapsManageFull : MonoBehaviour {


	//public float speed = 0.1F;
	float TrenutniX, TrenutniY;
	int[] Klik = {0,1,2,3,4,5,6,7,8,9};
	int KliknutoNa;
	float startX;
	float endX;
	float vremeKlika;
	string clickedItem;
	string releasedItem;
	bool moved;
	bool released;
	bool bounce;
	float pomerajX;
	GameObject bonusIsland;
	float levaGranica = 87.9f;
	float desnaGranica = 95.44f;
	Transform lifeManager;
	public Transform levo;
	public Transform desno;
	Transform _GUI;
	Camera guiCamera;

	float razlikaX = 0;
	float razlikaY = 0;

	GameObject temp;
	Vector3 originalScale;
	public static int makniPopup = 0;

	void Awake()
	{
		if(PlayerPrefs.HasKey("PrvoPokretanjeIgre"))
		{
			if(PlayerPrefs.GetInt("PrvoPokretanjeIgre") == 1)
				StagesParser.openedButNotPlayed[0] = false;
		}
		else
		{
			StagesParser.openedButNotPlayed[0] = true;
		}
		_GUI = GameObject.Find("_GUI/INTERFACE HOLDER").transform;
		guiCamera = GameObject.Find("GUICamera").GetComponent<Camera>();
		//lifeManager = GameObject.Find("LifeManager").transform;

		//bonusIsland = GameObject.Find("BonusIsland");
		//bonusIsland.SetActive(false);
		//GameObject.Find("HolderBackToMenu").GetComponent<Transform>().position=new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x, Camera.main.ViewportToWorldPoint(Vector3.one).y, -2f);
		//GameObject.Find("HolderHeaderMaps").GetComponent<Transform>().position=new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one/2).x, Camera.main.ViewportToWorldPoint(Vector3.one).y, -2f);
		//GameObject.Find("HolderLife").GetComponent<Transform>().position=new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one).x - 2.5f, Camera.main.ViewportToWorldPoint(Vector3.one).y - 0.75f, -2f);

		KliknutoNa=0;

		levaGranica = levo.position.x+Camera.main.orthographicSize*Camera.main.aspect;
		desnaGranica = desno.position.x-Camera.main.orthographicSize*Camera.main.aspect;

		InitWorlds();

		Camera.main.transform.position = new Vector3( Mathf.Clamp(GameObject.Find(StagesParser.worldToFocus.ToString()).transform.position.x, levaGranica,desnaGranica), Camera.main.transform.position.y, Camera.main.transform.position.z);
	}

	void RefreshScene()
	{
		InitWorlds();
		_GUI.Find("_TopLeft/Coins/Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
		_GUI.Find("_TopLeft/Coins/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		_GUI.Find("_TopLeft/PTS/PTS Number").GetComponent<TextMesh>().text = StagesParser.currentPoints.ToString();
		_GUI.Find("_TopLeft/PTS/PTS Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		_GUI.Find("_TopLeft/Bananas/Banana Number").GetComponent<TextMesh>().text = StagesParser.currentBananas.ToString();
		_GUI.Find("_TopLeft/Bananas/Banana Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		_GUI.Find("FB Login/Text/Number").GetComponent<TextMesh>().text = "+"+StagesParser.LoginReward.ToString();
		_GUI.Find("FB Login/Text/Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		bool prvoZakljucano = true;
		for(int i = 0; i < StagesParser.totalSets; i++)
		{
			GameObject.Find("AllWorlds_holder/"+i.ToString()+"/TotalStars/Stars Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
//			GameObject.Find("AllWorlds_holder").transform.Find(i.ToString()+"/LevelsHolder/LevelText").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);//DODATO!!!
//			GameObject.Find("AllWorlds_holder").transform.Find(i.ToString()+"/LevelsHolder/LevelValue").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);//DODATO!!!
			if(i>0 && GameObject.Find("AllWorlds_holder").transform.Find("HolderClouds"+(i).ToString()).gameObject.activeSelf)
			{
				GameObject.Find("AllWorlds_holder").transform.Find("HolderClouds"+(i).ToString()+"/CloudsMove/AllMapsKatanac/Stars Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
				GameObject.Find("AllWorlds_holder").transform.Find("HolderClouds"+(i).ToString()+"/CloudsMove/AllMapsKatanac/Level Text").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
				GameObject.Find("AllWorlds_holder").transform.Find(i.ToString()+"/LevelsHolder").gameObject.SetActive(false);
				
				if(StagesParser.unlockedWorlds[i] == false)
				{
					if(!prvoZakljucano)
					{
						UgasiOstrvce(i);
					}
					else
					{
						prvoZakljucano = false;
						UpaliOstrvce(i);
					}
				}
			}
			else
				IspisiBrojLevela(i);
		}
		changeLanguage();
		CheckInternetConnection.Instance.refreshText();
		StagesParser.LoadingPoruke.Clear();
		StagesParser.RedniBrojSlike.Clear();
		StagesParser.Instance.UcitajLoadingPoruke();
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
				FacebookManager.MestoPozivanjaLogina = 2;
				FacebookManager.FacebookObject.FacebookLogin();
			}
		}
		else
		{
			CheckInternetConnection.Instance.openPopup();
		}
	}

	void Start()
	{
		makniPopup = 0;
		changeLanguage();

		if(Loading.Instance != null)
			StartCoroutine(Loading.Instance.UcitanaScena(guiCamera,3,0));
		if(StagesParser.vratioSeNaSvaOstrva)
		{
			_GUI.parent.Find("LOADING HOLDER NEW/Loading Animation Vrata").GetComponent<Animator>().Play("Loading Zidovi Odlazak");
			StagesParser.vratioSeNaSvaOstrva = false;
		}

		_GUI.Find("_TopLeft/Coins/Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
		_GUI.Find("_TopLeft/Coins/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		_GUI.Find("_TopLeft/PTS/PTS Number").GetComponent<TextMesh>().text = StagesParser.currentPoints.ToString();
		_GUI.Find("_TopLeft/PTS/PTS Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		_GUI.Find("_TopLeft/Bananas/Banana Number").GetComponent<TextMesh>().text = StagesParser.currentBananas.ToString();
		_GUI.Find("_TopLeft/Bananas/Banana Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		_GUI.Find("FB Login/Text/Number").GetComponent<TextMesh>().text = "+"+StagesParser.LoginReward.ToString();
		_GUI.Find("FB Login/Text/Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);

		if(PlaySounds.musicOn && !PlaySounds.BackgroundMusic_Menu.isPlaying)
			PlaySounds.Play_BackgroundMusic_Menu();

		_GUI.Find("_TopLeft").position = new Vector3(guiCamera.ViewportToWorldPoint(Vector3.zero).x,_GUI.Find("_TopLeft").position.y,_GUI.Find("_TopLeft").position.z);
		_GUI.Find("FB Login").position = new Vector3(guiCamera.ViewportToWorldPoint(new Vector3(0.91f,0,0)).x,_GUI.Find("FB Login").position.y,_GUI.Find("FB Login").position.z);
		_GUI.Find("TotalStars").position = new Vector3(guiCamera.ViewportToWorldPoint(new Vector3(0.89f,0,0)).x,_GUI.Find("TotalStars").position.y,_GUI.Find("TotalStars").position.z);

		bool prvoZakljucano = true;

		for(int i = 0; i < StagesParser.totalSets; i++)
		{
			GameObject.Find("AllWorlds_holder/"+i.ToString()+"/TotalStars/Stars Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			GameObject.Find("AllWorlds_holder/"+i.ToString()+"/LevelsHolder/LevelText").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			GameObject.Find("AllWorlds_holder/"+i.ToString()+"/LevelsHolder/LevelValue").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			if(i>0 && GameObject.Find("AllWorlds_holder").transform.Find("HolderClouds"+(i).ToString()).gameObject.activeSelf)
			{
				GameObject.Find("AllWorlds_holder").transform.Find("HolderClouds"+(i).ToString()+"/CloudsMove/AllMapsKatanac/Stars Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
				GameObject.Find("AllWorlds_holder").transform.Find("HolderClouds"+(i).ToString()+"/CloudsMove/AllMapsKatanac/Level Text").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
				GameObject.Find("AllWorlds_holder/"+i.ToString()+"/LevelsHolder").SetActive(false);

				if(StagesParser.unlockedWorlds[i] == false)
				{
					if(!prvoZakljucano)
					{
						UgasiOstrvce(i);
					}
					else
					{
						prvoZakljucano = false;
					}
				}
			}
		}

		if(FB.IsLoggedIn)
		{
			GameObject.Find("FB Login").SetActive(false);
		}
	}

	void UgasiOstrvce(int index)
	{
		Transform temp = GameObject.Find("AllWorlds_holder").transform.Find("HolderClouds"+(index).ToString()+"/CloudsMove/AllMapsKatanac").transform;
		temp.Find("Level Number").gameObject.SetActive(false);
		temp.Find("Level Text").gameObject.SetActive(false);
		temp.Find("Fields/BlueFields/BlueField2").gameObject.SetActive(false);
		temp.Find("Fields/BlueFields/BlueField4").gameObject.SetActive(false);
		temp.Find("Fields/BlueFields/WhiteFields/WhiteField2").gameObject.SetActive(false);
		temp.Find("Fields/BlueFields/WhiteFields/WhiteField3").gameObject.SetActive(false);
		temp.Find("Fields/TextField2").gameObject.SetActive(false);
		temp.Find("Fields/Senka").gameObject.SetActive(false);
		temp.Find("Fields/LastIslandIconHOLDER").gameObject.SetActive(false);
	}

	void UpaliOstrvce(int index)
	{
		Transform temp = GameObject.Find("AllWorlds_holder").transform.Find("HolderClouds"+(index).ToString()+"/CloudsMove/AllMapsKatanac").transform;
		if(!temp.Find("Fields/BlueFields/BlueField2").gameObject.activeSelf)
		{
			temp.Find("Level Number").gameObject.SetActive(true);
			temp.Find("Level Text").gameObject.SetActive(true);
			temp.Find("Fields/BlueFields/BlueField2").gameObject.SetActive(true);
			temp.Find("Fields/BlueFields/BlueField4").gameObject.SetActive(true);
			temp.Find("Fields/BlueFields/WhiteFields/WhiteField2").gameObject.SetActive(true);
			temp.Find("Fields/BlueFields/WhiteFields/WhiteField3").gameObject.SetActive(true);
			temp.Find("Fields/TextField2").gameObject.SetActive(true);
			temp.Find("Fields/Senka").gameObject.SetActive(true);
			temp.Find("Fields/LastIslandIconHOLDER").gameObject.SetActive(true);
		}
	}

	void IspisiBrojLevela(int index)
	{
		GameObject levelsHolder = GameObject.Find("AllWorlds_holder").transform.Find(index.ToString()+"/LevelsHolder").gameObject;
		//if(!levelsHolder.activeSelf)
		{
			levelsHolder.SetActive(true);
			levelsHolder.transform.Find("LevelText").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);//DODATO!!!
			levelsHolder.transform.Find("LevelValue").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);//DODATO!!!
		}
	}

	void InitWorlds() //ovde je negde izbacio null reference exception, nakon poziva RefreshScene() posle logovanja
	{
		//StagesParser.SetsInGame[StagesParser.currSetIndex].CurrentStarsInStageNEW = 0;
		for (int i = 0; i < StagesParser.totalSets; i++)
		{

			//if(i > 0)
			{
				//Debug.Log("current stars: " + StagesParser.currentStarsNEW);
				if(StagesParser.currentStarsNEW >= StagesParser.SetsInGame[i].StarRequirement)
				{ //unlocked
					//Debug.Log(" world " + (i+1) + " unlocked");
					if(i>0)
					{
						string[] values = StagesParser.allLevels[(i-1)*20+19].Split('#');
						if(int.Parse(values[1]) > 0)
						{
							//@@@@@@ zaobilaznica - desava se da mu se otkljuca sledece ostrvo, tj. da ima zvezdice i da je presao 20. nivo, ali se iz nekog razloga ne otkljucava prvi nivo na
							//sledecem ostrvu i ne moze dalje da se igra, izgleda da se desava kada logovan korisnik ide na reset progress i krene ispocetka da igra, ali ima jos neki uslov jer 
							//se ne javlja stalno...bio je problem da se u ResetProgress() ne resetuju otkljucani svetovi i lastUnlockedWorldIndex, ali to je sada ispravljeno, tako da ne mozemo
							//lepo da ustanovimo uzrok. Ideja je da se napravi zaobilaznica da cak i kada se desi bag opet, da mu se otkljuca taj prvi nivo, da moze da nastavi da igra
							if(StagesParser.allLevels[i*20].Split('#')[1].Equals("-1"))
							{
								StagesParser.allLevels[i*20] = (i*20+1)+"#0#0";
								StagesParser.StarsPoNivoima[i*20] = 0;
							}

							StagesParser.unlockedWorlds[i] = true;
							StagesParser.lastUnlockedWorldIndex = i;
							if(StagesParser.openedButNotPlayed[i] == true)
							{
								//Debug.Log("nije otvaran dosad");
								StartCoroutine("AnimacijaOblakaOstrvo",i);
								StagesParser.openedButNotPlayed[i] = false;
								//PlayerPrefs.SetInt("PrvoPokretanjeIgre",1);
								//PlayerPrefs.Save();
							}
							else
							{
								//Debug.Log("bio je vec otvaran");
								GameObject.Find("HolderClouds" + i).SetActive(false);

								//@@@@@@ DODATAK ZA NOVA OSTRVA
								if(StagesParser.lastUnlockedWorldIndex == 5 && FB.IsLoggedIn)
								{
									for(int ii=0;ii<FacebookManager.ListaStructPrijatelja.Count;ii++)
									{
										if(FacebookManager.ListaStructPrijatelja[ii].PrijateljID.Equals(FacebookManager.User))
										{
											if(FacebookManager.ListaStructPrijatelja[ii].scores.Count < StagesParser.allLevels.Length)
											{
												for(int j=FacebookManager.ListaStructPrijatelja[ii].scores.Count;j<StagesParser.allLevels.Length;j++)
													FacebookManager.ListaStructPrijatelja[ii].scores.Add(0);
											}
										}
									}
								}
								//@@@@@@
							}
						}
						else
						{ //locked
							//Debug.Log(" world " + (i+1) + " Locked");
							StagesParser.unlockedWorlds[i] = false;
						}


						//@@@@@@@@@@@@@@@@@ VISAK ZASAD
//						if(PlayerPrefs.HasKey("Level"+((i-1)*20+20)))
//						{
//							string[] values = PlayerPrefs.GetString("Level"+((i-1)*20+20)).Split('#');
//							if(int.Parse(values[1]) > 0)
//							{
//								StagesParser.unlockedWorlds[i] = true;
//								StagesParser.lastUnlockedWorldIndex = i;
//								if(StagesParser.openedButNotPlayed[i] == true)
//								{
//									//Debug.Log("nije otvaran dosad");
//									StartCoroutine("AnimacijaOblakaOstrvo",i);
//									StagesParser.openedButNotPlayed[i] = false;
//									//PlayerPrefs.SetInt("PrvoPokretanjeIgre",1);
//									//PlayerPrefs.Save();
//								}
//								else
//								{
//									//Debug.Log("bio je vec otvaran");
//									GameObject.Find("HolderClouds" + i).SetActive(false);
//								}
//							}
//							else
//							{ //locked
//								//Debug.Log(" world " + (i+1) + " Locked");
//								StagesParser.unlockedWorlds[i] = false;
//							}
//						}
						//@@@@@@@@@@@@@@@@@ END OF VISAK
					}
				}
			}
//			if(i==0)
//			{
//				StagesParser.unlockedWorlds[i] = true;
//				if(StagesParser.openedButNotPlayed[i] == true)
//				{
//					StartCoroutine("AnimacijaOblakaOstrvo",i);
//					StagesParser.openedButNotPlayed[i] = false;
//					PlayerPrefs.SetInt("PrvoPokretanjeIgre",1);
//					PlayerPrefs.Save();
//				}
//				else
//				{
//					GameObject.Find("HolderClouds" + i).SetActive(false);
//				}
//			}

//			if(y > 0)
//			{
//				Debug.Log("world: " + y);
//				if(StagesParser.SetsInGame[y-1].StarRequirement<= StagesParser.SetsInGame[y-1].CurrentStarsInStage)
//				{//unlocked
//					Debug.Log("otkljucan je");
//					StagesParser.unlockedWorlds[y] = true;
//					if(StagesParser.openedButNotPlayed[y] == true)
//					{
//						Debug.Log("nije otvaran dosad");
//						StartCoroutine("AnimacijaOblakaOstrvo",y);
//						StagesParser.openedButNotPlayed[y] = false;
//						//PlayerPrefs.SetInt("PrvoPokretanjeIgre",1);
//						//PlayerPrefs.Save();
//					}
//					else
//					{
//						Debug.Log("bio je vec otvaran");
//						GameObject.Find("HolderClouds" + y).SetActive(false);
//					}
//				}
//				else
//				{//locked
//					Debug.Log("zakljucan je");
//					StagesParser.unlockedWorlds[y] = false;
//				}
//			}
//			else
//			{
//				StagesParser.unlockedWorlds[y] = true;
//				if(StagesParser.openedButNotPlayed[y] == true)
//				{
//					StartCoroutine("AnimacijaOblakaOstrvo",y);
//					StagesParser.openedButNotPlayed[y] = false;
//					PlayerPrefs.SetInt("PrvoPokretanjeIgre",1);
//					PlayerPrefs.Save();
//				}
//				else
//				{
//					GameObject.Find("HolderClouds" + y).SetActive(false);
//				}
//			}

			//for(int j=0; j<StagesParser.SetsInGame[i].StagesOnSet; j++)
			{
				//values2 = StagesParser.allLevels[i*20+j].Split('#');
				//if(int.Parse(values2[1]) > -1)
				//	StagesParser.SetsInGame[i].CurrentStarsInStageNEW += int.Parse(values2[1]);

				//@@@@@@@@@@@@@@@ VISAK ZASAD
//				if(PlayerPrefs.HasKey("Level"+(StagesParser.currSetIndex*20+j+1)))
//				{
//					string level = PlayerPrefs.GetString("Level"+(StagesParser.currSetIndex*20+j+1));
//					values2 = level.Split('#');
//					StagesParser.SetsInGame[StagesParser.currSetIndex].SetStarOnStage(j,int.Parse(values2[1]));
//					StagesParser.SetsInGame[StagesParser.currSetIndex].CurrentStarsInStageNEW += int.Parse(values2[1]);
//				}
//				else
//				{
//					if(j==0)
//						StagesParser.SetsInGame[StagesParser.currSetIndex].SetStarOnStage(j,0);
//					else
//						StagesParser.SetsInGame[StagesParser.currSetIndex].SetStarOnStage(j,-1);
//				}
				//@@@@@@@@@@@@@@@ END OF VISAK
			}
			//_GUI.Find("INTERFACE HOLDER/TotalStars/Stars Number").GetComponent<TextMesh>().text = StagesParser.SetsInGame[StagesParser.currSetIndex].CurrentStarsInStageNEW + "/" + StagesParser.SetsInGame[StagesParser.currSetIndex].StagesOnSet*3;

			GameObject.Find("AllWorlds_holder/"+i.ToString()+"/TotalStars/Stars Number").GetComponent<TextMesh>().text = StagesParser.SetsInGame[i].CurrentStarsInStageNEW + "/" + StagesParser.SetsInGame[i].StagesOnSet*3;
			GameObject.Find("AllWorlds_holder").transform.Find(i.ToString()+"/LevelsHolder/LevelText").GetComponent<TextMesh>().text = LanguageManager.Level;
			GameObject.Find("AllWorlds_holder").transform.Find(i.ToString()+"/LevelsHolder/LevelValue").GetComponent<TextMesh>().text = StagesParser.maxLevelNaOstrvu[i] + "/20";
			if(i>0)
			{
				GameObject.Find("AllWorlds_holder").transform.Find("HolderClouds"+(i).ToString()+"/CloudsMove/AllMapsKatanac/Stars Number").GetComponent<TextMesh>().text = StagesParser.SetsInGame[i].StarRequirement.ToString();
				GameObject.Find("AllWorlds_holder").transform.Find("HolderClouds"+(i).ToString()+"/CloudsMove/AllMapsKatanac/Level Text").GetComponent<TextMesh>().text = LanguageManager.Level;
			}
		}


		_GUI.Find("TotalStars/Stars Number").GetComponent<TextMesh>().text = StagesParser.currentStarsNEW + "/" + StagesParser.totalStars;
		_GUI.Find("TotalStars/Stars Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
	}

	void Update ()
	{


		if(Input.GetKeyUp(KeyCode.Escape))
		{
			if(makniPopup == 1)
			{
				makniPopup = 0;
				StartCoroutine(CheckInternetConnection.Instance.ClosePopup());
			}
			else if(makniPopup == 0)
			{
				//GameObject.Find("HolderLife").transform.parent = lifeManager.GetChild(0);
				if(StagesParser.ServerUpdate == 1 && FB.IsLoggedIn)
				{
					FacebookManager.FacebookObject.scoreToSet = StagesParser.currentPoints;
					FacebookManager.FacebookObject.proveraPublish_ActionPermisije();
					FacebookManager.FacebookObject.SacuvajScoreNaNivoima(StagesParser.PointsPoNivoima,StagesParser.StarsPoNivoima, StagesParser.maxLevel,StagesParser.bonusLevels);
					FacebookManager.FacebookObject.UpdateujPodatkeKorisnika(StagesParser.currentMoney,StagesParser.currentPoints,LanguageManager.chosenLanguage,StagesParser.currentBananas,StagesParser.powerup_magnets,StagesParser.powerup_shields,StagesParser.powerup_doublecoins,StagesParser.svekupovineGlava,StagesParser.svekupovineMajica,StagesParser.svekupovineLedja,StagesParser.ledja,StagesParser.glava,StagesParser.majica,StagesParser.imaUsi,StagesParser.imaKosu,FacebookManager.NumberOfFriends);
				}
				//Debug.Log("ButtonBack");
				//GameObject.Find("HolderLife").transform.parent = lifeManager.GetChild(0);
				if(PlaySounds.soundOn)
					PlaySounds.Play_Button_GoBack();
				Application.LoadLevel(1);
			}

		}


//		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
//		{
//			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
//			transform.position=new Vector3( Mathf.Clamp((transform.position.x -touchDeltaPosition.x * speed * Time.deltaTime), 87.9f , 95.44f), transform.position.y , -10);
//		}

		if(Input.GetMouseButtonDown(0))
		{
			if(released)
				released = false;

			clickedItem = RaycastFunction(Input.mousePosition);

			startX = Input.mousePosition.x;
			vremeKlika = Time.time;

			razlikaX = Input.mousePosition.x;
			razlikaY = Input.mousePosition.y;

			if(clickedItem.Equals("Button_CheckOK")) 
			{
				temp = GameObject.Find(clickedItem);
				originalScale = temp.transform.localScale;
				temp.transform.localScale = originalScale * 1.2f;
			}
			else if(clickedItem != System.String.Empty)
			{
				temp = GameObject.Find(clickedItem);
				originalScale = temp.transform.localScale;
			}

			if(RaycastFunction(Input.mousePosition) == "0")
			{					
				KliknutoNa=1;
			}
			else if(RaycastFunction(Input.mousePosition) == "1")
			{
				KliknutoNa=2;
			}
			else if(RaycastFunction(Input.mousePosition) == "2")
			{
				KliknutoNa=3;
			}
			else if(RaycastFunction(Input.mousePosition) == "3")
			{
				KliknutoNa=4;
			}
			else if(RaycastFunction(Input.mousePosition) == "4")
			{
				KliknutoNa=5;
			}
			else if(RaycastFunction(Input.mousePosition) == "HouseShop")
			{
				KliknutoNa=6;
			}
			else if(RaycastFunction(Input.mousePosition) == "HolderBonus")
			{
				KliknutoNa=7;
			}
			else if(RaycastFunction(Input.mousePosition) == "HolderShipFreeCoins")
			{
				KliknutoNa=8;
			}
			else if(RaycastFunction(Input.mousePosition) == "ButtonBackToMenu")
			{
				KliknutoNa=9;
			}
			
		}

		if(Input.GetMouseButton(0) && makniPopup == 0)
		{

			endX = Input.mousePosition.x;
			pomerajX = (endX - startX)/45;
			if(pomerajX != 0)
				moved = true;
			//Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x - pomerajX, levaGranica, desnaGranica), Camera.main.transform.position.y, Camera.main.transform.position.z);
			Camera.main.transform.position = new Vector3(Mathf.Clamp(Mathf.Lerp(Camera.main.transform.position.x,Camera.main.transform.position.x - pomerajX,0.75f),levaGranica,desnaGranica), Camera.main.transform.position.y, Camera.main.transform.position.z);
			startX = endX;
		}

		if(released && Mathf.Abs(pomerajX)>0.0001f)
		{
			if(Camera.main.transform.position.x <= levaGranica + 0.25f)
			{
				if(bounce)
				{	
					pomerajX = -0.04f;//0.075f;
					bounce = false;
				}
				//Debug.Log(pomerajX);
			}
			else if(Camera.main.transform.position.x >= desnaGranica - 0.25f)
			{
				if(bounce)
				{	
					pomerajX = 0.04f;//0.075f;
					bounce = false;
				}
			}
//
//			//if(Camera.main.transform.position.x > 87.9f + 0.6f && Camera.main.transform.position.x < 95.44f - 0.6f)
//			{
				Camera.main.transform.Translate(-pomerajX,0,0);
				pomerajX *= 0.92f;
				Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x,levaGranica,desnaGranica),Camera.main.transform.position.y,Camera.main.transform.position.z);
//				Debug.Log(pomerajX);
//			}
		}
		if(Camera.main.transform.position.x > desnaGranica)
		{

		}

		if(Input.GetMouseButtonUp(0))
		{
			releasedItem = RaycastFunction(Input.mousePosition);
			if(moved)
			{
				moved = false;
				released = true;
				bounce = true;
			}

			startX = endX = 0;

			razlikaX = Input.mousePosition.x - razlikaX;
			razlikaY = Input.mousePosition.y - razlikaY;

			if(temp != null)
				temp.transform.localScale = originalScale;

			//Debug.Log("Time: " + (Time.time - vremeKlika));
			if(clickedItem == releasedItem && (Time.time - vremeKlika < 0.35f) && Mathf.Abs(razlikaX) < 40 && Mathf.Abs(razlikaY) < 40)
			{

				if(RaycastFunction(Input.mousePosition) == "0")
				{		
					//Debug.Log("Sta mu je sad");
					//if(StagesParser.unlockedWorlds[0] == true)
					{
						//Debug.Log("Banana");
						StagesParser.currSetIndex = 0;
						StagesParser.currentWorld = 1;
						//if(StagesParser.openedButNotPlayed[0] == true)
						{
							StagesParser.zadnjiOtkljucanNivo = 0;
						}
						StartCoroutine(UcitajOstrvo("_Mapa 1 Banana"));
					}
						
				}
				else if(RaycastFunction(Input.mousePosition) == "1")
				{
					if(StagesParser.unlockedWorlds[1] == true)
					{
						//Debug.Log("Savana");
						StagesParser.currSetIndex = 1;
						StagesParser.currentWorld = 2;
						//if(StagesParser.openedButNotPlayed[1] == true)
						{
							StagesParser.zadnjiOtkljucanNivo = 0;
						}
						StartCoroutine(UcitajOstrvo("_Mapa 2 Savanna"));
					}

				}
				else if(RaycastFunction(Input.mousePosition) == "2")
				{
					if(StagesParser.unlockedWorlds[2] == true)
					{
						//Debug.Log("Jungle");
						StagesParser.currSetIndex = 2;
						StagesParser.currentWorld = 3;
						//if(StagesParser.openedButNotPlayed[2] == true)
						{
							StagesParser.zadnjiOtkljucanNivo = 0;
						}
						StartCoroutine(UcitajOstrvo("_Mapa 3 Jungle"));
					}
				}
				else if(RaycastFunction(Input.mousePosition) == "3")
				{
					if(StagesParser.unlockedWorlds[3] == true)
					{
						//Debug.Log("Temple");
						StagesParser.currSetIndex = 3;
						StagesParser.currentWorld = 4;
						//if(StagesParser.openedButNotPlayed[3] == true)
						{
							StagesParser.zadnjiOtkljucanNivo = 0;
						}
						StartCoroutine(UcitajOstrvo("_Mapa 4 Temple"));
					}

				}
				else if(RaycastFunction(Input.mousePosition) == "4")
				{
					if(StagesParser.unlockedWorlds[4] == true)
					{
						//Debug.Log("Volcano");
						StagesParser.currSetIndex = 4;
						StagesParser.currentWorld = 5;
						//if(StagesParser.openedButNotPlayed[4] == true)
						{
							StagesParser.zadnjiOtkljucanNivo = 0;
						}
						StartCoroutine(UcitajOstrvo("_Mapa 5 Volcano"));
					}
						
				}
				else if(RaycastFunction(Input.mousePosition) == "5") //@@@@@@ CHANGE
				{
					if(StagesParser.unlockedWorlds[5] == true)
					{
						//Debug.Log("Volcano");
						StagesParser.currSetIndex = 5;
						StagesParser.currentWorld = 6;
						//if(StagesParser.openedButNotPlayed[4] == true)
						{
							StagesParser.zadnjiOtkljucanNivo = 0;
						}
						StartCoroutine(UcitajOstrvo("_Mapa 6 Ice"));
					}
					
				}

				else if(releasedItem.Equals("ButtonBackToMenu"))
				{
					if(KliknutoNa==Klik[9])
					{
						if(StagesParser.ServerUpdate == 1 && FB.IsLoggedIn)
						{
							FacebookManager.FacebookObject.scoreToSet = StagesParser.currentPoints;
							FacebookManager.FacebookObject.proveraPublish_ActionPermisije();
							FacebookManager.FacebookObject.SacuvajScoreNaNivoima(StagesParser.PointsPoNivoima,StagesParser.StarsPoNivoima, StagesParser.maxLevel,StagesParser.bonusLevels);
							FacebookManager.FacebookObject.UpdateujPodatkeKorisnika(StagesParser.currentMoney,StagesParser.currentPoints,LanguageManager.chosenLanguage,StagesParser.currentBananas,StagesParser.powerup_magnets,StagesParser.powerup_shields,StagesParser.powerup_doublecoins,StagesParser.svekupovineGlava,StagesParser.svekupovineMajica,StagesParser.svekupovineLedja,StagesParser.ledja,StagesParser.glava,StagesParser.majica,StagesParser.imaUsi,StagesParser.imaKosu,FacebookManager.NumberOfFriends);
						}

						//Debug.Log("ButtonBack");
						//GameObject.Find("HolderLife").transform.parent = lifeManager.GetChild(0);
						if(PlaySounds.soundOn)
							PlaySounds.Play_Button_GoBack();
						Application.LoadLevel(1);
					}
						
				}
				else if(releasedItem.Equals("FB Login"))
				{
					makniPopup = 1;
					StartCoroutine(checkConnectionForLoginButton());
				}
				else if(releasedItem.Equals("Button_CheckOK"))
				{
					makniPopup = 0;
					StartCoroutine(CheckInternetConnection.Instance.ClosePopup());
				}

			}
		}



	}

	IEnumerator AnimacijaOblakaOstrvo(int index)
	{
		yield  return new WaitForSeconds(1f);
		GameObject.Find("HolderClouds" + index + "/CloudsMove").GetComponent<Animation>().Play("CloudsOpenMap");
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

	IEnumerator UcitajOstrvo(string ime)
	{
		if(PlaySounds.soundOn)
			PlaySounds.Play_Button_OpenWorld();
		//GameObject.Find("HolderLife").transform.parent = lifeManager.GetChild(0);
		//GameObject.Find("OblaciPomeranje").animation.Play("OblaciPostavljanje");
		_GUI.parent.Find("LOADING HOLDER NEW/Loading Animation Vrata").GetComponent<Animator>().Play("Loading Zidovi Dolazak");
		yield return new WaitForSeconds(1.1f);
		Application.LoadLevel(ime);
	}

	void changeLanguage()
	{
		if(!FB.IsLoggedIn)
		{
			GameObject.Find("Log In").GetComponent<TextMesh>().text = LanguageManager.LogIn;
			GameObject.Find("Log In").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		}
		GameObject.Find("Coming Soon").GetComponent<TextMesh>().text = LanguageManager.ComingSoon;
		GameObject.Find("Coming Soon").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
	}

}
	