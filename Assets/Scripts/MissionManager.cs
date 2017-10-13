using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

public class MissionManager : MonoBehaviour {

	private string xmlName 				= "Missions.xml";
	public static int totalMissions 	= 0;
	public static MissionTemplate[] missions;

	public static bool activeBaboonsMission 			= false;
	public static bool activeFly_BaboonsMission 		= false;
	public static bool activeBoomerang_BaboonsMission 	= false;
	public static bool activeGorillaMission 			= false;
	public static bool activeFly_GorillaMission 		= false;
	public static bool activeKoplje_GorillaMission 		= false;
	public static bool activeDiamondsMission 			= false;
	public static bool activeCoinsMission 				= false;
	public static bool activeDistanceMission 			= false;
	public static bool activeBarrelsMission 			= false;
	public static bool activeRedDiamondsMission 		= false;
	public static bool activeBlueDiamondsMission 		= false;
	public static bool activeGreenDiamondsMission 		= false;

	public static int NumberOfQuests = 0;
	TextMesh missionDescription;

	static MissionManager instance;
	static int currentLevel;

	static TextMesh baboonsText;
	static TextMesh fly_baboonsText;
	static TextMesh boomerang_baboonsText;
	static TextMesh gorillaText;
	static TextMesh fly_gorillaText;
	static TextMesh koplje_gorillaText;
	static TextMesh diamondsText;
	static TextMesh coinsText;
	static TextMesh distanceText;
	static TextMesh barrelsText;
	static TextMesh redDiamondsText;
	static TextMesh blueDiamondsText;
	static TextMesh greenDiamondsText;

	static TextMeshEffects baboonsTextEffects;
	static TextMeshEffects fly_baboonsTextEffects;
	static TextMeshEffects boomerang_baboonsTextEffects;
	static TextMeshEffects gorillaTextEffects;
	static TextMeshEffects fly_gorillaTextEffects;
	static TextMeshEffects koplje_gorillaTextEffects;
	static TextMeshEffects diamondsTextEffects;
	static TextMeshEffects coinsTextEffects;
	static TextMeshEffects distanceTextEffects;
	static TextMeshEffects barrelsTextEffects;
	static TextMeshEffects redDiamondsTextEffects;
	static TextMeshEffects blueDiamondsTextEffects;
	static TextMeshEffects greenDiamondsTextEffects;

	public static bool missionsComplete;
	static bool postavioFinish = false;

	static Renderer aktivnaIkonicaMisija1 = null;
	static Renderer aktivnaIkonicaMisija2 = null;
	static Renderer aktivnaIkonicaMisija3 = null;

	static float previousDistance = 0;

	public static int points3Stars = 0;

	public static MissionManager Instance
	{
		get
		{
			if(instance == null)
				instance = FindObjectOfType(typeof(MissionManager)) as MissionManager;

			return instance;
		}
	}

	void Awake ()
	{
		transform.name = "MissionManager";
		instance = this;
		DontDestroyOnLoad(gameObject);

		StartCoroutine(LoadMissions());
	}

	IEnumerator LoadMissions()
	{
		string filePath = System.String.Empty;
		string result = System.String.Empty;
		//Debug.Log("1111 " + Application.streamingAssetsPath);

		//if(System.IO.File.Exists(System.IO.Path.Combine(Application.streamingAssetsPath,xmlName)))
		{

			filePath = System.IO.Path.Combine(Application.streamingAssetsPath,xmlName);
			Debug.Log("path: " + filePath);
		}
		//else
		{
		//	Debug.LogError("Error, Missions.xml doesn't exist!");
		}

		if (filePath.Contains("://")) //streaming assets
		{
			//#if UNITY_ANDROID
			WWW www = new WWW(filePath);
			yield return www;
			if(string.IsNullOrEmpty(www.error))
			{
				result = www.text;
			}
			else
			{
				Debug.LogError("Error reading file from path! " + www.error);
			}
			//#endif
		}
		else
			result = System.IO.File.ReadAllText(filePath);
		//Debug.Log("result: " + result);

		XElement xmlContent = XElement.Parse(result);
		IEnumerable<XElement> xmlElements = xmlContent.Elements();
		totalMissions = xmlElements.Count();
		missions = new MissionTemplate[totalMissions];
		IEnumerable<XElement> xmlHelpElements;
		for(int i=0; i<totalMissions; i++)
		{
			xmlHelpElements = xmlElements.ElementAt(i).Elements();
			missions[i] = new MissionTemplate();
			missions[i].level 					= xmlElements.ElementAt(i).Attribute("level").Value;
			missions[i].baboons 				= int.Parse(xmlHelpElements.ElementAt(0).Value);
			missions[i].fly_baboons 			= int.Parse(xmlHelpElements.ElementAt(1).Value);
			missions[i].boomerang_baboons 		= int.Parse(xmlHelpElements.ElementAt(2).Value);
			missions[i].gorilla 				= int.Parse(xmlHelpElements.ElementAt(3).Value);
			missions[i].fly_gorilla 			= int.Parse(xmlHelpElements.ElementAt(4).Value);
			missions[i].koplje_gorilla 			= int.Parse(xmlHelpElements.ElementAt(5).Value);
			missions[i].diamonds 				= int.Parse(xmlHelpElements.ElementAt(6).Value);
			missions[i].coins 					= int.Parse(xmlHelpElements.ElementAt(7).Value);
			missions[i].distance 				= int.Parse(xmlHelpElements.ElementAt(8).Value);
			missions[i].barrels 				= int.Parse(xmlHelpElements.ElementAt(9).Value);
			missions[i].red_diamonds 			= int.Parse(xmlHelpElements.ElementAt(10).Value);
			missions[i].blue_diamonds 			= int.Parse(xmlHelpElements.ElementAt(11).Value);
			missions[i].green_diamonds		 	= int.Parse(xmlHelpElements.ElementAt(12).Value);
			missions[i].points		 			= int.Parse(xmlHelpElements.ElementAt(13).Value);
			missions[i].description_en 			= xmlHelpElements.ElementAt(14).Value;
			missions[i].description_us 			= xmlHelpElements.ElementAt(15).Value;
			missions[i].description_es 			= xmlHelpElements.ElementAt(16).Value;
			missions[i].description_ru 			= xmlHelpElements.ElementAt(17).Value;
			missions[i].description_pt 			= xmlHelpElements.ElementAt(18).Value;
			missions[i].description_pt_br 		= xmlHelpElements.ElementAt(19).Value;
			missions[i].description_fr 			= xmlHelpElements.ElementAt(20).Value;
			missions[i].description_tha 		= xmlHelpElements.ElementAt(21).Value;
			missions[i].description_zh 			= xmlHelpElements.ElementAt(22).Value;
			missions[i].description_tzh 		= xmlHelpElements.ElementAt(23).Value;
			missions[i].description_ger 		= xmlHelpElements.ElementAt(24).Value;
			missions[i].description_it 			= xmlHelpElements.ElementAt(25).Value;
			missions[i].description_srb 		= xmlHelpElements.ElementAt(26).Value;
			missions[i].description_tur 		= xmlHelpElements.ElementAt(27).Value;
			missions[i].description_kor 		= xmlHelpElements.ElementAt(28).Value;
		}

		StagesParser.stagesLoaded = true;

//		currentLevel = LevelFactory.level-1;
//		missionDescription = GameObject.Find("MissionDescription").GetComponent<TextMesh>();
//		missionDescription.text = MissionManager.missions[currentLevel].description;
//		OdrediMisiju(currentLevel);

	}

	public static void OdrediJezik()
	{

	}

	public static void OdrediMisiju(int level, bool mapa)
	{
		postavioFinish = false;
		NumberOfQuests = 0;
		currentLevel = level;

		if(missions[level].baboons > 0)
		{
			activeBaboonsMission = true;
			NumberOfQuests++;
		}
		else
		{
			activeBaboonsMission = false;
		}

		if(missions[level].fly_baboons > 0)
		{
			activeFly_BaboonsMission = true;
			NumberOfQuests++;
		}
		else
		{
			activeFly_BaboonsMission = false;
		}

		if(missions[level].boomerang_baboons > 0)
		{
			activeBoomerang_BaboonsMission = true;
			NumberOfQuests++;
		}
		else
		{
			activeBoomerang_BaboonsMission = false;
		}

		if(missions[level].gorilla > 0)
		{
			activeGorillaMission = true;
			NumberOfQuests++;
		}
		else
		{
			activeGorillaMission = false;
		}

		if(missions[level].fly_gorilla > 0)
		{
			activeFly_GorillaMission = true;
			NumberOfQuests++;
		}
		else
		{
			activeFly_GorillaMission = false;
		}

		if(missions[level].koplje_gorilla > 0)
		{
			activeKoplje_GorillaMission = true;
			NumberOfQuests++;
		}
		else
		{
			activeKoplje_GorillaMission = false;
		}

		if(missions[level].diamonds > 0)
		{
			activeDiamondsMission = true;
			NumberOfQuests++;
		}
		else
		{
			activeDiamondsMission = false;
		}

		if(missions[level].coins > 0)
		{
			activeCoinsMission = true;
			NumberOfQuests++;
		}
		else
		{
			activeCoinsMission = false;
		}

		if(missions[level].distance > 0)
		{
			activeDistanceMission = true;
			NumberOfQuests++;
		}
		else
		{
			activeDistanceMission = false;
		}

		if(missions[level].barrels > 0)
		{
			activeBarrelsMission = true;
			NumberOfQuests++;
		}
		else
		{
			activeBarrelsMission = false;
		}

		if(missions[level].red_diamonds > 0)
		{
			activeRedDiamondsMission = true;
			NumberOfQuests++;
		}
		else
		{
			activeRedDiamondsMission = false;
		}

		if(missions[level].blue_diamonds > 0)
		{
			activeBlueDiamondsMission = true;
			NumberOfQuests++;
		}
		else
		{
			activeBlueDiamondsMission = false;
		}

		if(missions[level].green_diamonds > 0)
		{
			activeGreenDiamondsMission = true;
			NumberOfQuests++;
		}
		else
		{
			activeGreenDiamondsMission = false;
		}

		missionsComplete = false;
		previousDistance = 0;

		points3Stars = missions[level].points;

		if(missions[level].IspisiDescriptionNaIspravnomJeziku().Contains("BOSS STAGE"))
			StagesParser.bossStage = true;

		if(mapa)
			OdrediIkoniceNaMapi(level);
		else
			OdrediIkonice(level);
	}

	public static void OdrediIkonice(int level)
	{
		Transform mission = GameObject.Find("_GameManager/Gameplay Scena Interface/_TopMissions").transform;
		Transform child;
		float offset = 0;
		int NumberOfQuestsLeft = 1;
		Transform textMission = null;

		if(activeBaboonsMission)
		{
			mission.Find("MissionIcons/Babun").gameObject.SetActive(true);

			if(NumberOfQuestsLeft == 1)
			{
				textMission = mission.Find("TextField Mission1");
				textMission.parent = mission.Find("MissionIcons/Babun").parent = mission.Find("Mission1");
			}
			else if(NumberOfQuestsLeft == 2)
			{
				textMission = mission.Find("TextField Mission2");
				textMission.parent = mission.Find("MissionIcons/Babun").parent = mission.Find("Mission2");
			}
			else if(NumberOfQuestsLeft == 3)
			{
				textMission = mission.Find("TextField Mission3");
				textMission.parent = mission.Find("MissionIcons/Babun").parent = mission.Find("Mission3");
			}
			textMission.localPosition = new Vector3(0,0,-0.1f);
			textMission.gameObject.SetActive(true);
			baboonsText = textMission.GetChild(0).GetChild(0).GetComponent<TextMesh>();
			baboonsText.text = "0/" + missions[level].baboons.ToString();
			baboonsTextEffects = baboonsText.GetComponent<TextMeshEffects>();
			baboonsTextEffects.RefreshTextOutline(false,true);
			NumberOfQuestsLeft++;
		}
		if(activeFly_BaboonsMission)
		{
			mission.Find("MissionIcons/Babun Leteci").gameObject.SetActive(true);
			
			if(NumberOfQuestsLeft == 1)
			{
				textMission = mission.Find("TextField Mission1");
				textMission.parent = mission.Find("MissionIcons/Babun Leteci").parent = mission.Find("Mission1");
			}
			else if(NumberOfQuestsLeft == 2)
			{
				textMission = mission.Find("TextField Mission2");
				textMission.parent = mission.Find("MissionIcons/Babun Leteci").parent = mission.Find("Mission2");
			}
			else if(NumberOfQuestsLeft == 3)
			{
				textMission = mission.Find("TextField Mission3");
				textMission.parent = mission.Find("MissionIcons/Babun Leteci").parent = mission.Find("Mission3");
			}
			textMission.localPosition = new Vector3(0,0,-0.1f);
			textMission.gameObject.SetActive(true);
			fly_baboonsText = textMission.GetChild(0).GetChild(0).GetComponent<TextMesh>();
			fly_baboonsText.text = "0/" + missions[level].fly_baboons.ToString();
			fly_baboonsTextEffects = fly_baboonsText.GetComponent<TextMeshEffects>();
			fly_baboonsTextEffects.RefreshTextOutline(false,true);
			NumberOfQuestsLeft++;

			if(missions[level].fly_baboons > 0)
			{
				LevelFactory.instance.leteciBabuni_Kvota = LevelFactory.instance.leteciBabuni_Kvota_locked = 7f/missions[level].fly_baboons;
			}
		}
		if(activeBoomerang_BaboonsMission)
		{
			mission.Find("MissionIcons/Babun Boomerang").gameObject.SetActive(true);
			
			if(NumberOfQuestsLeft == 1)
			{
				textMission = mission.Find("TextField Mission1");
				textMission.parent = mission.Find("MissionIcons/Babun Boomerang").parent = mission.Find("Mission1");
			}
			else if(NumberOfQuestsLeft == 2)
			{
				textMission = mission.Find("TextField Mission2");
				textMission.parent = mission.Find("MissionIcons/Babun Boomerang").parent = mission.Find("Mission2");
			}
			else if(NumberOfQuestsLeft == 3)
			{
				textMission = mission.Find("TextField Mission3");
				textMission.parent = mission.Find("MissionIcons/Babun Boomerang").parent = mission.Find("Mission3");
			}
			textMission.localPosition = new Vector3(0,0,-0.1f);
			textMission.gameObject.SetActive(true);
			boomerang_baboonsText = textMission.GetChild(0).GetChild(0).GetComponent<TextMesh>();
			boomerang_baboonsText.text = "0/" + missions[level].boomerang_baboons.ToString();
			boomerang_baboonsTextEffects = boomerang_baboonsText.GetComponent<TextMeshEffects>();
			boomerang_baboonsTextEffects.RefreshTextOutline(false,true);
			NumberOfQuestsLeft++;

			if(missions[level].boomerang_baboons > 0)
			{
				LevelFactory.instance.boomerangBabuni_Kvota = LevelFactory.instance.boomerangBabuni_Kvota_locked = 7f/missions[level].boomerang_baboons;
			}
		}
		if(activeGorillaMission)
		{
			mission.Find("MissionIcons/Gorila").gameObject.SetActive(true);
			
			if(NumberOfQuestsLeft == 1)
			{
				textMission = mission.Find("TextField Mission1");
				textMission.parent = mission.Find("MissionIcons/Gorila").parent = mission.Find("Mission1");
			}
			else if(NumberOfQuestsLeft == 2)
			{
				textMission = mission.Find("TextField Mission2");
				textMission.parent = mission.Find("MissionIcons/Gorila").parent = mission.Find("Mission2");
			}
			else if(NumberOfQuestsLeft == 3)
			{
				textMission = mission.Find("TextField Mission3");
				textMission.parent = mission.Find("MissionIcons/Gorila").parent = mission.Find("Mission3");
			}
			textMission.localPosition = new Vector3(0,0,-0.1f);
			textMission.gameObject.SetActive(true);
			gorillaText = textMission.GetChild(0).GetChild(0).GetComponent<TextMesh>();
			gorillaText.text = "0/" + missions[level].gorilla.ToString();
			gorillaTextEffects = gorillaText.GetComponent<TextMeshEffects>();
			gorillaTextEffects.RefreshTextOutline(false,true);
			NumberOfQuestsLeft++;
		}
		if(activeFly_GorillaMission)
		{
			mission.Find("MissionIcons/Gorila Leteca").gameObject.SetActive(true);
			
			if(NumberOfQuestsLeft == 1)
			{
				textMission = mission.Find("TextField Mission1");
				textMission.parent = mission.Find("MissionIcons/Gorila Leteca").parent = mission.Find("Mission1");
			}
			else if(NumberOfQuestsLeft == 2)
			{
				textMission = mission.Find("TextField Mission2");
				textMission.parent = mission.Find("MissionIcons/Gorila Leteca").parent = mission.Find("Mission2");
			}
			else if(NumberOfQuestsLeft == 3)
			{
				textMission = mission.Find("TextField Mission3");
				textMission.parent = mission.Find("MissionIcons/Gorila Leteca").parent = mission.Find("Mission3");
			}
			textMission.localPosition = new Vector3(0,0,-0.1f);
			textMission.gameObject.SetActive(true);
			fly_gorillaText = textMission.GetChild(0).GetChild(0).GetComponent<TextMesh>();
			fly_gorillaText.text = "0/" + missions[level].fly_gorilla.ToString();
			fly_gorillaTextEffects = fly_gorillaText.GetComponent<TextMeshEffects>();
			fly_gorillaTextEffects.RefreshTextOutline(false,true);
			NumberOfQuestsLeft++;

			if(missions[level].fly_gorilla > 0)
			{
				LevelFactory.instance.leteceGorile_Kvota = LevelFactory.instance.leteceGorile_Kvota_locked = 7f/missions[level].fly_gorilla;
			}
		}
		if(activeKoplje_GorillaMission)
		{
			mission.Find("MissionIcons/Gorila Sa Kopljem").gameObject.SetActive(true);
			
			if(NumberOfQuestsLeft == 1)
			{
				textMission = mission.Find("TextField Mission1");
				textMission.parent = mission.Find("MissionIcons/Gorila Sa Kopljem").parent = mission.Find("Mission1");
			}
			else if(NumberOfQuestsLeft == 2)
			{
				textMission = mission.Find("TextField Mission2");
				textMission.parent = mission.Find("MissionIcons/Gorila Sa Kopljem").parent = mission.Find("Mission2");
			}
			else if(NumberOfQuestsLeft == 3)
			{
				textMission = mission.Find("TextField Mission3");
				textMission.parent = mission.Find("MissionIcons/Gorila Sa Kopljem").parent = mission.Find("Mission3");
			}
			textMission.localPosition = new Vector3(0,0,-0.1f);
			textMission.gameObject.SetActive(true);
			koplje_gorillaText = textMission.GetChild(0).GetChild(0).GetComponent<TextMesh>();
			koplje_gorillaText.text = "0/" + missions[level].koplje_gorilla.ToString();
			koplje_gorillaTextEffects = koplje_gorillaText.GetComponent<TextMeshEffects>();
			koplje_gorillaTextEffects.RefreshTextOutline(false,true);
			NumberOfQuestsLeft++;

			if(missions[level].koplje_gorilla > 0)
			{
				LevelFactory.instance.kopljeGorile_Kvota = LevelFactory.instance.kopljeGorile_Kvota_locked = 7f/missions[level].koplje_gorilla;
			}
		}

		if(activeDiamondsMission)
		{
			mission.Find("MissionIcons/Svi Dijamanti").gameObject.SetActive(true);
			
			if(NumberOfQuestsLeft == 1)
			{
				textMission = mission.Find("TextField Mission1");
				textMission.parent = mission.Find("MissionIcons/Svi Dijamanti").parent = mission.Find("Mission1");
			}
			else if(NumberOfQuestsLeft == 2)
			{
				textMission = mission.Find("TextField Mission2");
				textMission.parent = mission.Find("MissionIcons/Svi Dijamanti").parent = mission.Find("Mission2");
			}
			else if(NumberOfQuestsLeft == 3)
			{
				textMission = mission.Find("TextField Mission3");
				textMission.parent = mission.Find("MissionIcons/Svi Dijamanti").parent = mission.Find("Mission3");
			}
			textMission.localPosition = new Vector3(0,0,-0.1f);
			textMission.gameObject.SetActive(true);
			diamondsText = textMission.GetChild(0).GetChild(0).GetComponent<TextMesh>();
			diamondsText.text = "0/" + missions[level].diamonds.ToString();
			diamondsTextEffects = diamondsText.GetComponent<TextMeshEffects>();
			diamondsTextEffects.RefreshTextOutline(false,true);
			NumberOfQuestsLeft++;

		}

		if(activeCoinsMission)
		{
			mission.Find("MissionIcons/Coin").gameObject.SetActive(true);
			
			if(NumberOfQuestsLeft == 1)
			{
				textMission = mission.Find("TextFieldSplit Mission1");
				textMission.parent = mission.Find("MissionIcons/Coin").parent = mission.Find("Mission1");
			}
			else if(NumberOfQuestsLeft == 2)
			{
				textMission = mission.Find("TextFieldSplit Mission2");
				textMission.parent = mission.Find("MissionIcons/Coin").parent = mission.Find("Mission2");
			}
			else if(NumberOfQuestsLeft == 3)
			{
				textMission = mission.Find("TextFieldSplit Mission3");
				textMission.parent = mission.Find("MissionIcons/Coin").parent = mission.Find("Mission3");
			}
			textMission.localPosition = new Vector3(0,0,-0.1f);
			textMission.gameObject.SetActive(true);
			textMission.GetChild(0).Find("Target Number").GetComponent<TextMesh>().text = missions[level].coins.ToString();
			textMission.GetChild(0).Find("Target Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			coinsText = textMission.GetChild(0).Find("Current Number").GetComponent<TextMesh>();
			coinsText.text = "0";
			coinsTextEffects = coinsText.GetComponent<TextMeshEffects>();
			coinsTextEffects.RefreshTextOutline(false,true);
			NumberOfQuestsLeft++;
		}

		if(activeDistanceMission)
		{
			mission.Find("MissionIcons/Distance").gameObject.SetActive(true);
			
			if(NumberOfQuestsLeft == 1)
			{
				textMission = mission.Find("TextFieldSplit Mission1");
				textMission.parent = mission.Find("MissionIcons/Distance").parent = mission.Find("Mission1");
			}
			else if(NumberOfQuestsLeft == 2)
			{
				textMission = mission.Find("TextFieldSplit Mission2");
				textMission.parent = mission.Find("MissionIcons/Distance").parent = mission.Find("Mission2");
			}
			else if(NumberOfQuestsLeft == 3)
			{
				textMission = mission.Find("TextFieldSplit Mission3");
				textMission.parent = mission.Find("MissionIcons/Distance").parent = mission.Find("Mission3");
			}
			textMission.localPosition = new Vector3(0,0,-0.1f);
			textMission.gameObject.SetActive(true);
			textMission.GetChild(0).Find("Target Number").GetComponent<TextMesh>().text = missions[level].distance.ToString();
			textMission.GetChild(0).Find("Target Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			distanceText = textMission.GetChild(0).Find("Current Number").GetComponent<TextMesh>();
			distanceText.text = "0";
			distanceTextEffects = distanceText.GetComponent<TextMeshEffects>();
			distanceTextEffects.RefreshTextOutline(false,true);
			NumberOfQuestsLeft++;
		}

		if(activeBarrelsMission)
		{
			mission.Find("MissionIcons/Bure").gameObject.SetActive(true);
			
			if(NumberOfQuestsLeft == 1)
			{
				textMission = mission.Find("TextField Mission1");
				textMission.parent = mission.Find("MissionIcons/Bure").parent = mission.Find("Mission1");
			}
			else if(NumberOfQuestsLeft == 2)
			{
				textMission = mission.Find("TextField Mission2");
				textMission.parent = mission.Find("MissionIcons/Bure").parent = mission.Find("Mission2");
			}
			else if(NumberOfQuestsLeft == 3)
			{
				textMission = mission.Find("TextField Mission3");
				textMission.parent = mission.Find("MissionIcons/Bure").parent = mission.Find("Mission3");
			}
			textMission.localPosition = new Vector3(0,0,-0.1f);
			textMission.gameObject.SetActive(true);
			barrelsText = textMission.GetChild(0).GetChild(0).GetComponent<TextMesh>();
			barrelsText.text = "0/" + missions[level].barrels.ToString();
			barrelsTextEffects = barrelsText.GetComponent<TextMeshEffects>();
			barrelsTextEffects.RefreshTextOutline(false,true);
			NumberOfQuestsLeft++;
		}

		if(activeRedDiamondsMission)
		{
			mission.Find("MissionIcons/Crveni Dijamant").gameObject.SetActive(true);
			
			if(NumberOfQuestsLeft == 1)
			{
				textMission = mission.Find("TextField Mission1");
				textMission.parent = mission.Find("MissionIcons/Crveni Dijamant").parent = mission.Find("Mission1");
			}
			else if(NumberOfQuestsLeft == 2)
			{
				textMission = mission.Find("TextField Mission2");
				textMission.parent = mission.Find("MissionIcons/Crveni Dijamant").parent = mission.Find("Mission2");
			}
			else if(NumberOfQuestsLeft == 3)
			{
				textMission = mission.Find("TextField Mission3");
				textMission.parent = mission.Find("MissionIcons/Crveni Dijamant").parent = mission.Find("Mission3");
			}
			textMission.localPosition = new Vector3(0,0,-0.1f);
			textMission.gameObject.SetActive(true);
			redDiamondsText = textMission.GetChild(0).GetChild(0).GetComponent<TextMesh>();
			redDiamondsText.text = "0/" + missions[level].red_diamonds.ToString();
			redDiamondsTextEffects = redDiamondsText.GetComponent<TextMeshEffects>();
			redDiamondsTextEffects.RefreshTextOutline(false,true);
			NumberOfQuestsLeft++;

			if(missions[level].red_diamonds > 0)
			{
				LevelFactory.instance.crveniDijamant_Kvota = LevelFactory.instance.crveniDijamant_Kvota_locked = 7f/missions[level].red_diamonds;
			}
		}

		if(activeBlueDiamondsMission)
		{
			mission.Find("MissionIcons/Plavi Dijamant").gameObject.SetActive(true);
			
			if(NumberOfQuestsLeft == 1)
			{
				textMission = mission.Find("TextField Mission1");
				textMission.parent = mission.Find("MissionIcons/Plavi Dijamant").parent = mission.Find("Mission1");
			}
			else if(NumberOfQuestsLeft == 2)
			{
				textMission = mission.Find("TextField Mission2");
				textMission.parent = mission.Find("MissionIcons/Plavi Dijamant").parent = mission.Find("Mission2");
			}
			else if(NumberOfQuestsLeft == 3)
			{
				textMission = mission.Find("TextField Mission3");
				textMission.parent = mission.Find("MissionIcons/Plavi Dijamant").parent = mission.Find("Mission3");
			}
			textMission.localPosition = new Vector3(0,0,-0.1f);
			textMission.gameObject.SetActive(true);
			blueDiamondsText = textMission.GetChild(0).GetChild(0).GetComponent<TextMesh>();
			blueDiamondsText.text = "0/" + missions[level].blue_diamonds.ToString();
			blueDiamondsTextEffects = blueDiamondsText.GetComponent<TextMeshEffects>();
			blueDiamondsTextEffects.RefreshTextOutline(false,true);
			NumberOfQuestsLeft++;

			if(missions[level].blue_diamonds > 0)
			{
				LevelFactory.instance.plaviDijamant_Kvota = LevelFactory.instance.plaviDijamant_Kvota_locked = 7f/missions[level].blue_diamonds;
			}
		}

		if(activeGreenDiamondsMission)
		{
			mission.Find("MissionIcons/Zeleni Dijamant").gameObject.SetActive(true);
			
			if(NumberOfQuestsLeft == 1)
			{
				textMission = mission.Find("TextField Mission1");
				textMission.parent = mission.Find("MissionIcons/Zeleni Dijamant").parent = mission.Find("Mission1");
			}
			else if(NumberOfQuestsLeft == 2)
			{
				textMission = mission.Find("TextField Mission2");
				textMission.parent = mission.Find("MissionIcons/Zeleni Dijamant").parent = mission.Find("Mission2");
			}
			else if(NumberOfQuestsLeft == 3)
			{
				textMission = mission.Find("TextField Mission3");
				textMission.parent = mission.Find("MissionIcons/Zeleni Dijamant").parent = mission.Find("Mission3");
			}
			textMission.localPosition = new Vector3(0,0,-0.1f);
			textMission.gameObject.SetActive(true);
			greenDiamondsText = textMission.GetChild(0).GetChild(0).GetComponent<TextMesh>();
			greenDiamondsText.text = "0/" + missions[level].green_diamonds.ToString();
			greenDiamondsTextEffects = greenDiamondsText.GetComponent<TextMeshEffects>();
			greenDiamondsTextEffects.RefreshTextOutline(false,true);
			NumberOfQuestsLeft++;

			if(missions[level].green_diamonds > 0)
			{
				LevelFactory.instance.zeleniDijamant_Kvota = LevelFactory.instance.zeleniDijamant_Kvota_locked = 7f/missions[level].green_diamonds;
			}
		}

		if(NumberOfQuests == 1)
		{
			mission.Find("Mission1").localPosition = new Vector3(7,0,0);
			mission.Find("Mission2").localPosition = new Vector3(0,7,0);
			mission.Find("Mission3").localPosition = new Vector3(0,7,0);
		}
		else if(NumberOfQuests == 2)
		{
			mission.Find("Mission1").localPosition = new Vector3(4.5f,0,0);
			mission.Find("Mission2").localPosition = new Vector3(10.5f,0,0);
			mission.Find("Mission3").localPosition = new Vector3(0,7,0);
		}
		else if(NumberOfQuests == 3)
		{
			mission.Find("Mission1").localPosition = new Vector3(1,0,0);
			mission.Find("Mission2").localPosition = new Vector3(7,0,0);
			mission.Find("Mission3").localPosition = new Vector3(13,0,0);
		}

	}

	public static void OdrediIkoniceNaMapi(int level)
	{
		Transform missionPopup = GameObject.FindGameObjectWithTag("Mission").transform;
		int tempNumberOfQuestsLeft = 1;
		int levell = level;
		levell++;
		levell = levell % 20;
		if(levell == 0) levell = 20;
		missionPopup.Find("Text/Level No").GetComponent<TextMesh>().text = LanguageManager.Level + " " + levell;
		missionPopup.Find("Text/Level No").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		string[] values;

		if(activeBaboonsMission)
		{
			if(tempNumberOfQuestsLeft == 1)
			{
				if(aktivnaIkonicaMisija1 != null)
					aktivnaIkonicaMisija1.enabled = false;
				aktivnaIkonicaMisija1 = missionPopup.Find("Mission Icons/Mission 1/Babun").GetComponent<Renderer>();
				aktivnaIkonicaMisija1.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 1");
				currentMissionText.GetComponent<TextMesh>().text = values[0];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 2)
			{
				if(aktivnaIkonicaMisija2 != null)
					aktivnaIkonicaMisija2.enabled = false;
				aktivnaIkonicaMisija2 = missionPopup.Find("Mission Icons/Mission 2/Babun").GetComponent<Renderer>();
				aktivnaIkonicaMisija2.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 2");
				currentMissionText.GetComponent<TextMesh>().text = values[1];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 3)
			{
				if(aktivnaIkonicaMisija3 != null)
					aktivnaIkonicaMisija3.enabled = false;
				aktivnaIkonicaMisija3 = missionPopup.Find("Mission Icons/Mission 3/Babun").GetComponent<Renderer>();
				aktivnaIkonicaMisija3.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 3");
				currentMissionText.GetComponent<TextMesh>().text = values[2];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			tempNumberOfQuestsLeft++;
		}
		if(activeFly_BaboonsMission)
		{
			if(tempNumberOfQuestsLeft == 1)
			{
				if(aktivnaIkonicaMisija1 != null)
					aktivnaIkonicaMisija1.enabled = false;
				aktivnaIkonicaMisija1 = missionPopup.Find("Mission Icons/Mission 1/Babun Leteci").GetComponent<Renderer>();
				aktivnaIkonicaMisija1.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 1");
				currentMissionText.GetComponent<TextMesh>().text = values[0];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 2)
			{
				if(aktivnaIkonicaMisija2 != null)
					aktivnaIkonicaMisija2.enabled = false;
				aktivnaIkonicaMisija2 = missionPopup.Find("Mission Icons/Mission 2/Babun Leteci").GetComponent<Renderer>();
				aktivnaIkonicaMisija2.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 2");
				currentMissionText.GetComponent<TextMesh>().text = values[1];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 3)
			{
				if(aktivnaIkonicaMisija3 != null)
					aktivnaIkonicaMisija3.enabled = false;
				aktivnaIkonicaMisija3 = missionPopup.Find("Mission Icons/Mission 3/Babun Leteci").GetComponent<Renderer>();
				aktivnaIkonicaMisija3.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 3");
				currentMissionText.GetComponent<TextMesh>().text = values[2];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			tempNumberOfQuestsLeft++;
		}
		if(activeBoomerang_BaboonsMission)
		{
			if(tempNumberOfQuestsLeft == 1)
			{
				if(aktivnaIkonicaMisija1 != null)
					aktivnaIkonicaMisija1.enabled = false;
				aktivnaIkonicaMisija1 = missionPopup.Find("Mission Icons/Mission 1/Babun Bumerang").GetComponent<Renderer>();
				aktivnaIkonicaMisija1.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 1");
				currentMissionText.GetComponent<TextMesh>().text = values[0];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 2)
			{
				if(aktivnaIkonicaMisija2 != null)
					aktivnaIkonicaMisija2.enabled = false;
				aktivnaIkonicaMisija2 = missionPopup.Find("Mission Icons/Mission 2/Babun Bumerang").GetComponent<Renderer>();
				aktivnaIkonicaMisija2.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 2");
				currentMissionText.GetComponent<TextMesh>().text = values[1];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 3)
			{
				if(aktivnaIkonicaMisija3 != null)
					aktivnaIkonicaMisija3.enabled = false;
				aktivnaIkonicaMisija3 = missionPopup.Find("Mission Icons/Mission 3/Babun Bumerang").GetComponent<Renderer>();
				aktivnaIkonicaMisija3.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 3");
				currentMissionText.GetComponent<TextMesh>().text = values[2];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			tempNumberOfQuestsLeft++;
		}
		if(activeGorillaMission)
		{
			if(tempNumberOfQuestsLeft == 1)
			{
				if(aktivnaIkonicaMisija1 != null)
					aktivnaIkonicaMisija1.enabled = false;
				aktivnaIkonicaMisija1 = missionPopup.Find("Mission Icons/Mission 1/Gorila").GetComponent<Renderer>();
				aktivnaIkonicaMisija1.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 1");
				currentMissionText.GetComponent<TextMesh>().text = values[0];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 2)
			{
				if(aktivnaIkonicaMisija2 != null)
					aktivnaIkonicaMisija2.enabled = false;
				aktivnaIkonicaMisija2 = missionPopup.Find("Mission Icons/Mission 2/Gorila").GetComponent<Renderer>();
				aktivnaIkonicaMisija2.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 2");
				currentMissionText.GetComponent<TextMesh>().text = values[1];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 3)
			{
				if(aktivnaIkonicaMisija3 != null)
					aktivnaIkonicaMisija3.enabled = false;
				aktivnaIkonicaMisija3 = missionPopup.Find("Mission Icons/Mission 3/Gorila").GetComponent<Renderer>();
				aktivnaIkonicaMisija3.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 3");
				currentMissionText.GetComponent<TextMesh>().text = values[2];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			tempNumberOfQuestsLeft++;
		}
		if(activeFly_GorillaMission)
		{
			if(tempNumberOfQuestsLeft == 1)
			{
				if(aktivnaIkonicaMisija1 != null)
					aktivnaIkonicaMisija1.enabled = false;
				aktivnaIkonicaMisija1 = missionPopup.Find("Mission Icons/Mission 1/Gorila Leteca").GetComponent<Renderer>();
				aktivnaIkonicaMisija1.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 1");
				currentMissionText.GetComponent<TextMesh>().text = values[0];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 2)
			{
				if(aktivnaIkonicaMisija2 != null)
					aktivnaIkonicaMisija2.enabled = false;
				aktivnaIkonicaMisija2 = missionPopup.Find("Mission Icons/Mission 2/Gorila Leteca").GetComponent<Renderer>();
				aktivnaIkonicaMisija2.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 2");
				currentMissionText.GetComponent<TextMesh>().text = values[1];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 3)
			{
				if(aktivnaIkonicaMisija3 != null)
					aktivnaIkonicaMisija3.enabled = false;
				aktivnaIkonicaMisija3 = missionPopup.Find("Mission Icons/Mission 3/Gorila Leteca").GetComponent<Renderer>();
				aktivnaIkonicaMisija3.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 3");
				currentMissionText.GetComponent<TextMesh>().text = values[2];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			tempNumberOfQuestsLeft++;
		}
		if(activeKoplje_GorillaMission)
		{
			if(tempNumberOfQuestsLeft == 1)
			{
				if(aktivnaIkonicaMisija1 != null)
					aktivnaIkonicaMisija1.enabled = false;
				aktivnaIkonicaMisija1 = missionPopup.Find("Mission Icons/Mission 1/Gorila Sa Kopljem").GetComponent<Renderer>();
				aktivnaIkonicaMisija1.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 1");
				currentMissionText.GetComponent<TextMesh>().text = values[0];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 2)
			{
				if(aktivnaIkonicaMisija2 != null)
					aktivnaIkonicaMisija2.enabled = false;
				aktivnaIkonicaMisija2 = missionPopup.Find("Mission Icons/Mission 2/Gorila Sa Kopljem").GetComponent<Renderer>();
				aktivnaIkonicaMisija2.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 2");
				currentMissionText.GetComponent<TextMesh>().text = values[1];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 3)
			{
				if(aktivnaIkonicaMisija3 != null)
					aktivnaIkonicaMisija3.enabled = false;
				aktivnaIkonicaMisija3 = missionPopup.Find("Mission Icons/Mission 3/Gorila Sa Kopljem").GetComponent<Renderer>();
				aktivnaIkonicaMisija3.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 3");
				currentMissionText.GetComponent<TextMesh>().text = values[2];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			tempNumberOfQuestsLeft++;
		}
		if(activeDiamondsMission)
		{
			if(tempNumberOfQuestsLeft == 1)
			{
				if(aktivnaIkonicaMisija1 != null)
					aktivnaIkonicaMisija1.enabled = false;
				aktivnaIkonicaMisija1 = missionPopup.Find("Mission Icons/Mission 1/3 Dijamanta").GetComponent<Renderer>();
				aktivnaIkonicaMisija1.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 1");
				currentMissionText.GetComponent<TextMesh>().text = values[0];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 2)
			{
				if(aktivnaIkonicaMisija2 != null)
					aktivnaIkonicaMisija2.enabled = false;
				aktivnaIkonicaMisija2 = missionPopup.Find("Mission Icons/Mission 2/3 Dijamanta").GetComponent<Renderer>();
				aktivnaIkonicaMisija2.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 2");
				currentMissionText.GetComponent<TextMesh>().text = values[1];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 3)
			{
				if(aktivnaIkonicaMisija3 != null)
					aktivnaIkonicaMisija3.enabled = false;
				aktivnaIkonicaMisija3 = missionPopup.Find("Mission Icons/Mission 3/3 Dijamanta").GetComponent<Renderer>();
				aktivnaIkonicaMisija3.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 3");
				currentMissionText.GetComponent<TextMesh>().text = values[2];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			tempNumberOfQuestsLeft++;
		}
		if(activeCoinsMission)
		{
			if(tempNumberOfQuestsLeft == 1)
			{
				if(aktivnaIkonicaMisija1 != null)
					aktivnaIkonicaMisija1.enabled = false;
				aktivnaIkonicaMisija1 = missionPopup.Find("Mission Icons/Mission 1/Coin").GetComponent<Renderer>();
				aktivnaIkonicaMisija1.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 1");
				currentMissionText.GetComponent<TextMesh>().text = values[0];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 2)
			{
				if(aktivnaIkonicaMisija2 != null)
					aktivnaIkonicaMisija2.enabled = false;
				aktivnaIkonicaMisija2 = missionPopup.Find("Mission Icons/Mission 2/Coin").GetComponent<Renderer>();
				aktivnaIkonicaMisija2.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 2");
				currentMissionText.GetComponent<TextMesh>().text = values[1];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 3)
			{
				if(aktivnaIkonicaMisija3 != null)
					aktivnaIkonicaMisija3.enabled = false;
				aktivnaIkonicaMisija3 = missionPopup.Find("Mission Icons/Mission 3/Coin").GetComponent<Renderer>();
				aktivnaIkonicaMisija3.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 3");
				currentMissionText.GetComponent<TextMesh>().text = values[2];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			tempNumberOfQuestsLeft++;
		}
		if(activeDistanceMission)
		{
			if(tempNumberOfQuestsLeft == 1)
			{
				if(aktivnaIkonicaMisija1 != null)
					aktivnaIkonicaMisija1.enabled = false;
				aktivnaIkonicaMisija1 = missionPopup.Find("Mission Icons/Mission 1/Distance").GetComponent<Renderer>();
				aktivnaIkonicaMisija1.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 1");
				currentMissionText.GetComponent<TextMesh>().text = values[0];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 2)
			{
				if(aktivnaIkonicaMisija2 != null)
					aktivnaIkonicaMisija2.enabled = false;
				aktivnaIkonicaMisija2 = missionPopup.Find("Mission Icons/Mission 2/Distance").GetComponent<Renderer>();
				aktivnaIkonicaMisija2.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 2");
				currentMissionText.GetComponent<TextMesh>().text = values[1];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 3)
			{
				if(aktivnaIkonicaMisija3 != null)
					aktivnaIkonicaMisija3.enabled = false;
				aktivnaIkonicaMisija3 = missionPopup.Find("Mission Icons/Mission 3/Distance").GetComponent<Renderer>();
				aktivnaIkonicaMisija3.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 3");
				currentMissionText.GetComponent<TextMesh>().text = values[2];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			tempNumberOfQuestsLeft++;
		}
		if(activeBarrelsMission)
		{
			if(tempNumberOfQuestsLeft == 1)
			{
				if(aktivnaIkonicaMisija1 != null)
					aktivnaIkonicaMisija1.enabled = false;
				aktivnaIkonicaMisija1 = missionPopup.Find("Mission Icons/Mission 1/Bure").GetComponent<Renderer>();
				aktivnaIkonicaMisija1.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 1");
				currentMissionText.GetComponent<TextMesh>().text = values[0];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 2)
			{
				if(aktivnaIkonicaMisija2 != null)
					aktivnaIkonicaMisija2.enabled = false;
				aktivnaIkonicaMisija2 = missionPopup.Find("Mission Icons/Mission 2/Bure").GetComponent<Renderer>();
				aktivnaIkonicaMisija2.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 2");
				currentMissionText.GetComponent<TextMesh>().text = values[1];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 3)
			{
				if(aktivnaIkonicaMisija3 != null)
					aktivnaIkonicaMisija3.enabled = false;
				aktivnaIkonicaMisija3 = missionPopup.Find("Mission Icons/Mission 3/Bure").GetComponent<Renderer>();
				aktivnaIkonicaMisija3.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 3");
				currentMissionText.GetComponent<TextMesh>().text = values[2];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			tempNumberOfQuestsLeft++;
		}
		if(activeRedDiamondsMission)
		{
			if(tempNumberOfQuestsLeft == 1)
			{
				if(aktivnaIkonicaMisija1 != null)
					aktivnaIkonicaMisija1.enabled = false;
				aktivnaIkonicaMisija1 = missionPopup.Find("Mission Icons/Mission 1/Crveni Dijamant").GetComponent<Renderer>();
				aktivnaIkonicaMisija1.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 1");
				currentMissionText.GetComponent<TextMesh>().text = values[0];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 2)
			{
				if(aktivnaIkonicaMisija2 != null)
					aktivnaIkonicaMisija2.enabled = false;
				aktivnaIkonicaMisija2 = missionPopup.Find("Mission Icons/Mission 2/Crveni Dijamant").GetComponent<Renderer>();
				aktivnaIkonicaMisija2.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 2");
				currentMissionText.GetComponent<TextMesh>().text = values[1];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 3)
			{
				if(aktivnaIkonicaMisija3 != null)
					aktivnaIkonicaMisija3.enabled = false;
				aktivnaIkonicaMisija3 = missionPopup.Find("Mission Icons/Mission 3/Crveni Dijamant").GetComponent<Renderer>();
				aktivnaIkonicaMisija3.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 3");
				currentMissionText.GetComponent<TextMesh>().text = values[2];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			tempNumberOfQuestsLeft++;
		}
		if(activeBlueDiamondsMission)
		{
			if(tempNumberOfQuestsLeft == 1)
			{
				if(aktivnaIkonicaMisija1 != null)
					aktivnaIkonicaMisija1.enabled = false;
				aktivnaIkonicaMisija1 = missionPopup.Find("Mission Icons/Mission 1/Plavi Dijamant").GetComponent<Renderer>();
				aktivnaIkonicaMisija1.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 1");
				currentMissionText.GetComponent<TextMesh>().text = values[0];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 2)
			{
				if(aktivnaIkonicaMisija2 != null)
					aktivnaIkonicaMisija2.enabled = false;
				aktivnaIkonicaMisija2 = missionPopup.Find("Mission Icons/Mission 2/Plavi Dijamant").GetComponent<Renderer>();
				aktivnaIkonicaMisija2.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 2");
				currentMissionText.GetComponent<TextMesh>().text = values[1];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 3)
			{
				if(aktivnaIkonicaMisija3 != null)
					aktivnaIkonicaMisija3.enabled = false;
				aktivnaIkonicaMisija3 = missionPopup.Find("Mission Icons/Mission 3/Plavi Dijamant").GetComponent<Renderer>();
				aktivnaIkonicaMisija3.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 3");
				currentMissionText.GetComponent<TextMesh>().text = values[2];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			tempNumberOfQuestsLeft++;
		}
		if(activeGreenDiamondsMission)
		{
			if(tempNumberOfQuestsLeft == 1)
			{
				if(aktivnaIkonicaMisija1 != null)
					aktivnaIkonicaMisija1.enabled = false;
				aktivnaIkonicaMisija1 = missionPopup.Find("Mission Icons/Mission 1/Zeleni Dijamant").GetComponent<Renderer>();
				aktivnaIkonicaMisija1.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 1");
				currentMissionText.GetComponent<TextMesh>().text = values[0];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 2)
			{
				if(aktivnaIkonicaMisija2 != null)
					aktivnaIkonicaMisija2.enabled = false;
				aktivnaIkonicaMisija2 = missionPopup.Find("Mission Icons/Mission 2/Zeleni Dijamant").GetComponent<Renderer>();
				aktivnaIkonicaMisija2.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 2");
				currentMissionText.GetComponent<TextMesh>().text = values[1];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			else if(tempNumberOfQuestsLeft == 3)
			{
				if(aktivnaIkonicaMisija3 != null)
					aktivnaIkonicaMisija3.enabled = false;
				aktivnaIkonicaMisija3 = missionPopup.Find("Mission Icons/Mission 3/Zeleni Dijamant").GetComponent<Renderer>();
				aktivnaIkonicaMisija3.enabled = true;

				values = missions[level].IspisiDescriptionNaIspravnomJeziku().Split(new string[] {"\n"}, System.StringSplitOptions.None);
				Transform currentMissionText = missionPopup.Find("Text/Mission 3");
				currentMissionText.GetComponent<TextMesh>().text = values[2];
				currentMissionText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			}
			tempNumberOfQuestsLeft++;
		}
	}

	public void BaboonEvent(int currentBaboons)
	{
		if(missions[currentLevel].baboons > 0)
		{
			baboonsText.text = currentBaboons + "/" + missions[currentLevel].baboons;
			baboonsTextEffects.RefreshTextOutline(false,true);
			if(currentBaboons >= missions[currentLevel].baboons && baboonsText.color != Color.green)
			{
				NumberOfQuests--;
				baboonsText.color = Color.green;
			}
			
			if(NumberOfQuests <= 0)
			{
				MissionComplete();
			}
		}
	}

	public void Fly_BaboonEvent(int currentFly_Baboons)
	{
		if(missions[currentLevel].fly_baboons > 0)
		{
			fly_baboonsText.text = currentFly_Baboons + "/" + missions[currentLevel].fly_baboons;
			fly_baboonsTextEffects.RefreshTextOutline(false,true);
			if(currentFly_Baboons >= missions[currentLevel].fly_baboons && fly_baboonsText.color != Color.green)
			{
				NumberOfQuests--;
				fly_baboonsText.color = Color.green;
				//LevelFactory.instance.leteciBabuni = 0;
			}
			
			if(NumberOfQuests <= 0)
			{
				MissionComplete();
			}
		}
	}

	public void Boomerang_BaboonEvent(int currentBoomerang_Baboons)
	{
		if(missions[currentLevel].boomerang_baboons > 0)
		{
			boomerang_baboonsText.text = currentBoomerang_Baboons + "/" + missions[currentLevel].boomerang_baboons;
			boomerang_baboonsTextEffects.RefreshTextOutline(false,true);
			if(currentBoomerang_Baboons >= missions[currentLevel].boomerang_baboons && boomerang_baboonsText.color != Color.green)
			{
				NumberOfQuests--;
				boomerang_baboonsText.color = Color.green;
				//LevelFactory.instance.boomerangBabuni = 0;
			}
			
			if(NumberOfQuests <= 0)
			{
				MissionComplete();
			}
		}
	}

	public void GorillaEvent(int currentGorillas)
	{
		if(missions[currentLevel].gorilla > 0)
		{
			gorillaText.text = currentGorillas + "/" + missions[currentLevel].gorilla;
			gorillaTextEffects.RefreshTextOutline(false,true);
			if(currentGorillas >= missions[currentLevel].gorilla && gorillaText.color != Color.green)
			{
				NumberOfQuests--;
				gorillaText.color = Color.green;
			}
			
			if(NumberOfQuests <= 0)
			{
				MissionComplete();
			}
		}
	}

	public void Fly_GorillaEvent(int currentFly_Gorillas)
	{
		if(missions[currentLevel].fly_gorilla > 0)
		{
			fly_gorillaText.text = currentFly_Gorillas + "/" + missions[currentLevel].fly_gorilla;
			fly_gorillaTextEffects.RefreshTextOutline(false,true);
			if(currentFly_Gorillas >= missions[currentLevel].fly_gorilla && fly_gorillaText.color != Color.green)
			{
				NumberOfQuests--;
				fly_gorillaText.color = Color.green;
				LevelFactory.instance.leteceGorile = 0;
			}
			
			if(NumberOfQuests <= 0)
			{
				MissionComplete();
			}
		}
	}

	public void Koplje_GorillaEvent(int currentKoplje_Gorillas)
	{
		if(missions[currentLevel].koplje_gorilla > 0)
		{
			koplje_gorillaText.text = currentKoplje_Gorillas + "/" + missions[currentLevel].koplje_gorilla;
			koplje_gorillaTextEffects.RefreshTextOutline(false,true);
			if(currentKoplje_Gorillas >= missions[currentLevel].koplje_gorilla && koplje_gorillaText.color != Color.green)
			{
				NumberOfQuests--;
				koplje_gorillaText.color = Color.green;
				LevelFactory.instance.kopljeGorile = 0;
			}
			
			if(NumberOfQuests <= 0)
			{
				MissionComplete();
			}
		}
	}

	public void DiamondEvent(int currentDiamonds)
	{
		if(missions[currentLevel].diamonds > 0)
		{
			diamondsText.text = currentDiamonds + "/" + missions[currentLevel].diamonds;
			diamondsTextEffects.RefreshTextOutline(false,true);
			if(currentDiamonds >= missions[currentLevel].diamonds && diamondsText.color != Color.green)
			{
				NumberOfQuests--;
				diamondsText.color = Color.green;
			}
			
			if(NumberOfQuests <= 0)
			{
				MissionComplete();
			}
		}
	}
	public void CoinEvent(int currentCoins)
	{
		if(missions[currentLevel].coins > 0)
		{
			coinsText.text = currentCoins.ToString();// + "/" + missions[currentLevel].coins;
			coinsTextEffects.RefreshTextOutline(false,true);
			if(currentCoins >= missions[currentLevel].coins && coinsText.color != Color.green)
			{
				NumberOfQuests--;
				coinsText.color = Color.green;
				coinsText.transform.parent.Find("Target Number").GetComponent<TextMesh>().color = Color.green;
			}
			
			if(NumberOfQuests <= 0)
			{
				MissionComplete();
			}
		}
	}

	public void DistanceEvent(float currentDistance)
	{
		distanceText.text = currentDistance.ToString();// + "/" + missions[currentLevel].distance + "m";
		distanceTextEffects.RefreshTextOutline(false,true);
		if(currentDistance % 10 == 0 && currentDistance != previousDistance)
		{
			Manage.points+=10;
			Manage.pointsText.text = Manage.points.ToString();
			Manage.pointsEffects.RefreshTextOutline(false,true);
			previousDistance = currentDistance;
		}

		if(currentDistance >= missions[currentLevel].distance && distanceText.color != Color.green)
		{
			NumberOfQuests--;
			distanceText.color = Color.green;
			distanceText.transform.parent.Find("Target Number").GetComponent<TextMesh>().color = Color.green;
		}

		if(NumberOfQuests <= 0)
		{
			MissionComplete();
		}
	}

	public void BarrelEvent(int currentBarrels)
	{
		if(missions[currentLevel].barrels > 0)
		{
			barrelsText.text = currentBarrels + "/" + missions[currentLevel].barrels;
			barrelsTextEffects.RefreshTextOutline(false,true);
			if(currentBarrels >= missions[currentLevel].barrels && barrelsText.color != Color.green)
			{
				NumberOfQuests--;
				barrelsText.color = Color.green;
			}
			
			if(NumberOfQuests <= 0)
			{
				MissionComplete();
			}
		}
	}

	public void RedDiamondEvent(int currentRedDiamonds)
	{
		if(missions[currentLevel].red_diamonds > 0)
		{
			redDiamondsText.text = currentRedDiamonds + "/" + missions[currentLevel].red_diamonds;
			redDiamondsTextEffects.RefreshTextOutline(false,true);
			if(currentRedDiamonds >= missions[currentLevel].red_diamonds && redDiamondsText.color != Color.green)
			{
				NumberOfQuests--;
				redDiamondsText.color = Color.green;
				LevelFactory.instance.crveniDijamant = 0;
			}
			
			if(NumberOfQuests <= 0)
			{
				MissionComplete();
			}
		}
	}

	public void BlueDiamondEvent(int currentBlueDiamonds)
	{
		if(missions[currentLevel].blue_diamonds > 0)
		{
			blueDiamondsText.text = currentBlueDiamonds + "/" + missions[currentLevel].blue_diamonds;
			blueDiamondsTextEffects.RefreshTextOutline(false,true);
			if(currentBlueDiamonds >= missions[currentLevel].blue_diamonds && blueDiamondsText.color != Color.green)
			{
				NumberOfQuests--;
				blueDiamondsText.color = Color.green;
				LevelFactory.instance.plaviDijamant = 0;
			}
			
			if(NumberOfQuests <= 0)
			{
				MissionComplete();
			}
		}
	}

	public void GreenDiamondEvent(int currentGreenDiamonds)
	{
		if(missions[currentLevel].green_diamonds > 0)
		{
			greenDiamondsText.text = currentGreenDiamonds + "/" + missions[currentLevel].green_diamonds;
			greenDiamondsTextEffects.RefreshTextOutline(false,true);
			if(currentGreenDiamonds >= missions[currentLevel].green_diamonds && greenDiamondsText.color != Color.green)
			{
				NumberOfQuests--;
				greenDiamondsText.color = Color.green;
				LevelFactory.instance.zeleniDijamant = 0;
			}
			
			if(NumberOfQuests <= 0)
			{
				MissionComplete();
			}
		}
	}

	void MissionComplete()
	{
		if(!postavioFinish)
		{
			MonkeyController2D.Instance.invincible = true;
			Manage.pauseEnabled = false;
			Manage.Instance.ApplyPowerUp(-1);
			Manage.Instance.ApplyPowerUp(-2);
			Manage.Instance.ApplyPowerUp(-3);
			postavioFinish = true;
			//Debug.Log("POSTAVI FINISH"); 
			missionsComplete = true;
			//LevelFactory.instance.PostaviFinish();
			//if(!MonkeyController2D.Instance.inAir)
			{
			//	MonkeyController2D.Instance.maxSpeedX = 0;
			//	MonkeyController2D.Instance.animator.Play("Dancing");
			//	Invoke("RestoreMaxSpeed",5.3f);
				//Invoke("NotifyFinish",6.7f); //0.5f
				MonkeyController2D.Instance.Finish();
			}
				
		}
	}

	void NotifyFinish()
	{
		MonkeyController2D.Instance.Finish();
	}


}
