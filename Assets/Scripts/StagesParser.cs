using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
public class StagesParser : MonoBehaviour {
	/*
	 * Scene:All
	 * Object:StagesParserManager
	 * Parsuje XML sa stazama i nivoima. Takodje sadrzi informacije o ukupnom broju
	 * setova, ukupnom broju sakupljenih i broju zvezdica u celoj igri, i niz Set-ova
	 * u Awakeu ucitava iz xml-a, a cuva se pozivom corutine SaveStages ili pozivom funkcije CallSave.
	 * Za ponovno ucitavanje koristi se korutina LoadStages ili funkcija CallLoad. Xml je ocekivan da pre prvog
	 * save-a se nalazi u streaming assets, a nakon toga u persistant data pathu. Ime xmla 
	 * je defaultno "StarsAndStages.xml" a defenise se preko xmlName. Za parsiranje xml-a se koristi
	 * XElement struktura definisana u Xml.Linq
	 * Takodje osigurati da u Awake funkciji
	 * postoji DontDestroyOnLoad(gameObject)
	 * 
	 * NOTE: Ako se pojavljuje greska prilikom build-a iz monodevelopa 
	 * 		 ici na Reference=(rightClick)=>Edit Reference i dodati system.xml.linq
	 * */
	private string xmlName="StarsAndStages.xml";
	public static int totalSets=0;
	public static Set[] SetsInGame;
	public static bool stagesLoaded=false;
	public static bool saving=false;
	public static int totalStars=0;
	public static int currentStars=0;
	
	public static int currSetIndex = 0;
	public static int currStageIndex = 0;
	public static int[] currWorldGridIndex = {0, 0, 0, 0, 0}; 
	public static bool[] unlockedWorlds = {false, false, false, false, false, false};//, false, false, false, false, false}; //@@@@@@ CHANGE
	public static bool[] openedButNotPlayed = {false, false, false, false, false, false};//, false, false, false, false, false}; //@@@@@@ CHANGE
	public static bool isJustOpened = false;
	public static bool NemaRequiredStars_VratiULevele =false;
	public static bool nemojDaAnimirasZvezdice = false;

	public static int starsGained = 0;
	public static int animate = 0;

	public static int currentMoney = 0;
	public static int currentBananas = 0;
	public static int bananaCost = 2000;
	public static int currentPoints = 0;

	public static int powerup_magnets = 0;
	public static int powerup_doublecoins = 0;
	public static int powerup_shields = 0;
	public static int cost_magnet = 100;
	public static int cost_doublecoins = 250;
	public static int cost_shield = 500;

	public static int numberGotKilled = 0;

	public static int lastOpenedNotPlayedYet = 1;

	public static int lastUnlockedWorldIndex = 0;

	bool prefs = false;

	public static int worldToFocus = 0;
	public static int levelToFocus = 0;

	public static int currentWorld = 1;
	public static int currentLevel = 1;
	public static int totalWorlds = 6; //@@@@@@ CHANGE
	public static int currentStarsNEW = 0;
	int tour;
	public static int maxLevel = 1;

	public static bool maska = false;
	public static int[] trenutniNivoNaOstrvu;
	public static int nivoZaUcitavanje;
	public static int zadnjiOtkljucanNivo = 0;

	public static Vector3 pozicijaMajmuncetaNaMapi = Vector3.zero;
	public static bool bonusLevel = false;
	public static string bonusName;
	public static int bonusID;
	public static bool dodatnaProveraIzasaoIzBonusa = false;
	//public static string chosenLanguage = "_en";
	public static bool bossStage = false;

	public static bool vratioSeNaSvaOstrva = false;

	public static List<string> LoadingPoruke=new List<string>();
	public static int odgledaoTutorial = 0; // 0 - prvi put pokrenuo igru, 1 - presao prvi nivo, 2 - obukao prvi put majmunce, 3 - presao drugi nivo
	public static int loadingTip = -1; //1-Do not miss tips and tricks, 2-Want to keep playing? Spend banana... , 3-Jump on enemies' heads...
	public static int odgledanihTipova = 0;
	public static int otvaraoShopNekad = 0;

	public static bool imaUsi = true;
	public static bool imaKosu = true;
	public static int majica = -1;
	public static int glava = -1;
	public static int ledja = -1;
	public static Color bojaMajice = Color.white;
	public static string svekupovineGlava = System.String.Empty;
	public static string svekupovineMajica = System.String.Empty;
	public static string svekupovineLedja = System.String.Empty;

	public static int[] PointsPoNivoima;
	public static int[] StarsPoNivoima;
	public static int[] maxLevelNaOstrvu;

	public static List<int> RedniBrojSlike=new List<int>();
	public static int ServerUpdate = 0;	//0 - default, 1 - treba da se updateuje ali nije updateovan, 2 - uspesno updateovan

	public static string[] allLevels;
	public static string bonusLevels;
	public static int LoginReward = 1000;
	public static int InviteReward = 100;
	public static int ShareReward = 100;
	public static int likePageReward = 1000;
	public static int watchVideoReward = 1000;

	public static bool internetOn = false;
	public static string lastLoggedUser = System.String.Empty;
	public static int brojIgranja = 0;
	public static bool obucenSeLogovaoNaDrugojSceni = false;
	public static bool ucitaoMainScenuPrviPut = false;
	public static int jezikPromenjen = 0;
	public static int sceneID = 0;	// 0 - mainScene, 1 - Mapa, 2 - Gameplay
	public static string languageBefore = "";
	[Header("Rate Link Set Up:")]
	public string rateLink;

	[Header("Advertisement Set Up:")]
	public string AdMobInterstitialID;
	public string UnityAdsVideoGameID;
	static StagesParser instance;

	public static StagesParser Instance
	{
		get
		{
			if(instance == null)
				instance = GameObject.FindObjectOfType(typeof(StagesParser)) as StagesParser;

			return instance;
		}
	}

	// Use this for initialization
	void Awake () {
		//PlayerPrefs.DeleteAll();

		#if UNITY_IPHONE
		bananaCost = 2500;
		watchVideoReward = 500;
		cost_magnet = 200;
		cost_doublecoins = 400;
		cost_shield = 1000;
		#endif


		instance = this;
		transform.name="StagesParserManager";
		DontDestroyOnLoad(gameObject);
		if(!PlayerPrefs.HasKey("TotalMoney"))
		{
			PlayerPrefs.SetInt("TotalMoney",currentMoney);
			PlayerPrefs.Save();
		}
		else
		{
			currentMoney = PlayerPrefs.GetInt("TotalMoney");
		}

		if(!PlayerPrefs.HasKey("TotalBananas"))
		{
			PlayerPrefs.SetInt("TotalBananas",currentBananas);
			PlayerPrefs.Save();
		}
		else
		{
			currentBananas = PlayerPrefs.GetInt("TotalBananas");
		}
		if(!PlayerPrefs.HasKey("TotalPoints"))
		{
			PlayerPrefs.SetInt("TotalPoints",currentPoints);
			PlayerPrefs.Save();
		}
		else
		{
			currentPoints = PlayerPrefs.GetInt("TotalPoints");
		}
		if(PlayerPrefs.HasKey("PowerUps"))
		{
			string[] values = PlayerPrefs.GetString("PowerUps").Split('#');
			powerup_doublecoins = int.Parse(values[0]);
			powerup_magnets = int.Parse(values[1]);
			powerup_shields = int.Parse(values[2]);
		}
		else
		{
			PlayerPrefs.SetString("PowerUps",(powerup_doublecoins+"#"+powerup_magnets+"#"+powerup_shields));
			PlayerPrefs.Save();
		}

		if(PlayerPrefs.HasKey("OdgledaoTutorial"))
		{
			string[] values = PlayerPrefs.GetString("OdgledaoTutorial").Split('#');
			odgledaoTutorial = int.Parse(values[0]); //3
			otvaraoShopNekad = int.Parse(values[1]); //1
		}

		if(PlayerPrefs.HasKey("LastLoggedUser"))
			lastLoggedUser = PlayerPrefs.GetString("LastLoggedUser");

		if(PlayerPrefs.HasKey("JezikPromenjen"))
		{
			jezikPromenjen = PlayerPrefs.GetInt("JezikPromenjen");
		}

		//@@@@@@@@@ ZA BRISANJE POSLE
//		odgledaoTutorial = 0;
//		otvaraoShopNekad = 0;

		//@@@@@@ CHANGE
		PointsPoNivoima = new int[120];
		StarsPoNivoima = new int[120];

		for(int i=0;i<PointsPoNivoima.Length;i++)
		{
			PointsPoNivoima[i] = 0;
			StarsPoNivoima[i] = -1;
		}

		//@@@@@@@@@ ZA BRISANJE POSLE
//		PlayerPrefs.DeleteKey("UserSveKupovineHats");
//		PlayerPrefs.DeleteKey("AktivniItemi");

//		PlayerPrefs.DeleteKey("UserSveKupovineHats");
//		PlayerPrefs.DeleteKey("UserSveKupovineShirts");
//		PlayerPrefs.DeleteKey("UserSveKupovineBackPacks");
//		PlayerPrefs.DeleteKey("OtkljucaniItemi");

		if(PlayerPrefs.HasKey("UserSveKupovineHats"))
		{
			svekupovineGlava = PlayerPrefs.GetString("UserSveKupovineHats");
		}
		if(PlayerPrefs.HasKey("UserSveKupovineShirts"))
		{
			svekupovineMajica = PlayerPrefs.GetString("UserSveKupovineShirts");
		}
		if(PlayerPrefs.HasKey("UserSveKupovineBackPacks"))
		{
			svekupovineLedja = PlayerPrefs.GetString("UserSveKupovineBackPacks");
		}

		////////////////////////////////////////////////////////////////////////////////
		// **************** INICIJALIZACIJA SVIH NIVOA POCETNIM VREDNOSTIMA ************
		////////////////////////////////////////////////////////////////////////////////

		allLevels = new string[120]; //@@@@@@ CHANGE
		if(!PlayerPrefs.HasKey("AllLevels")) //ukoliko prvi put pokrece igru
		{
			string pom = "1#0#0";
			for(int i=1;i<allLevels.Length;i++)
			{
				pom+="_"+(i+1).ToString()+"#-1#0";
			}
			allLevels = pom.Split('_');
			PlayerPrefs.SetString("AllLevels",pom);
			PlayerPrefs.Save();
		}
		else // ukoliko je vec pokretao igru
		{
			//@@@@@@@ CITANJE NIVOA IZ PLAYER PREFS-A, OVAKO TREBA DA OSTANE NA KRAJU
			//allLevels = PlayerPrefs.GetString("AllLevels").Split('_');
			//@@@@@@@ ZA TEST, RUCNO ODREDJIVANJE NIVOA 
			//otkljucano zadnje ostrvo
			//string qqqq = "1#3#1380_2#3#0_3#3#0_4#2#1200_5#3#1120_6#3#2120_7#3#2300_8#3#2340_9#1#310_10#2#1010_11#2#0_12#3#0_13#1#0_14#3#0_15#2#0_16#3#0_17#3#0_18#2#0_19#3#0_20#3#0_21#2#0_22#1#0_23#1#0_24#2#0_25#1#0_26#1#0_27#1#0_28#1#0_29#1#0_30#1#0_31#2#0_32#1#0_33#2#0_34#1#0_35#1#0_36#1#0_37#1#0_38#3#0_39#3#0_40#3#0_41#3#0_42#3#0_43#3#0_44#3#0_45#3#0_46#3#0_47#3#0_48#3#0_49#3#0_50#3#0_51#3#0_52#3#0_53#3#0_54#3#0_55#3#0_56#3#0_57#3#0_58#3#0_59#3#0_60#1#0_61#3#0_62#1#0_63#1#0_64#1#0_65#1#0_66#1#0_67#1#0_68#1#0_69#1#0_70#1#0_71#1#0_72#3#0_73#3#0_74#3#0_75#3#0_76#3#0_77#3#0_78#3#0_79#3#0_80#3#0_81#3#0_82#3#0_83#3#0_84#3#0_85#3#0_86#3#0_87#3#0_88#3#0_89#3#0_90#3#0_91#3#0_92#3#0_93#3#0_94#3#0_95#3#0_96#3#0_97#3#0_98#3#0_99#3#0_100#0#0_101#-1#0_102#-1#0_103#-1#0_104#-1#0_105#-1#0_106#-1#0_107#-1#0_108#-1#0_109#-1#0_110#-1#0_111#-1#0_112#-1#0_113#-1#0_114#-1#0_115#-1#0_116#-1#0_117#-1#0_118#-1#0_119#-1#0_120#-1#0";
			//predjeno 99 nivoa, niz ima 100 elemenata, 230 zvezdica
			//string qqqq = "1#3#1380_2#3#0_3#3#0_4#2#1200_5#3#1120_6#3#2120_7#3#2300_8#3#2340_9#1#310_10#2#1010_11#2#0_12#3#0_13#1#0_14#3#0_15#2#0_16#3#0_17#3#0_18#2#0_19#3#0_20#3#0_21#2#0_22#1#0_23#1#0_24#2#0_25#1#0_26#1#0_27#1#0_28#1#0_29#1#0_30#1#0_31#2#0_32#1#0_33#2#0_34#1#0_35#1#0_36#1#0_37#1#0_38#3#0_39#3#0_40#3#0_41#3#0_42#3#0_43#3#0_44#3#0_45#3#0_46#3#0_47#3#0_48#3#0_49#3#0_50#3#0_51#3#0_52#3#0_53#3#0_54#3#0_55#3#0_56#3#0_57#3#0_58#3#0_59#3#0_60#1#0_61#3#0_62#1#0_63#1#0_64#1#0_65#1#0_66#1#0_67#1#0_68#1#0_69#1#0_70#1#0_71#1#0_72#3#0_73#3#0_74#3#0_75#3#0_76#3#0_77#3#0_78#3#0_79#3#0_80#3#0_81#3#0_82#3#0_83#3#0_84#3#0_85#3#0_86#3#0_87#3#0_88#3#0_89#3#0_90#3#0_91#3#0_92#3#0_93#3#0_94#3#0_95#3#0_96#3#0_97#1#0_98#1#0_99#1#0_100#0#0";
			//predjeno 100 nivoa, niz ima 100 elemenata, ima dovoljno zvezdica za 6. ostrvo
			//string qqqq = "1#3#1380_2#3#0_3#3#0_4#2#1200_5#3#1120_6#3#2120_7#3#2300_8#3#2340_9#1#310_10#2#1010_11#2#0_12#3#0_13#1#0_14#3#0_15#2#0_16#3#0_17#3#0_18#2#0_19#3#0_20#3#0_21#2#0_22#1#0_23#1#0_24#2#0_25#1#0_26#1#0_27#1#0_28#1#0_29#1#0_30#1#0_31#2#0_32#1#0_33#2#0_34#1#0_35#1#0_36#1#0_37#1#0_38#3#0_39#3#0_40#3#0_41#3#0_42#3#0_43#3#0_44#3#0_45#3#0_46#3#0_47#3#0_48#3#0_49#3#0_50#3#0_51#3#0_52#3#0_53#3#0_54#3#0_55#3#0_56#3#0_57#3#0_58#3#0_59#3#0_60#1#0_61#3#0_62#1#0_63#1#0_64#1#0_65#1#0_66#1#0_67#1#0_68#1#0_69#1#0_70#1#0_71#1#0_72#3#0_73#3#0_74#3#0_75#3#0_76#3#0_77#3#0_78#3#0_79#3#0_80#3#0_81#3#0_82#3#0_83#3#0_84#3#0_85#3#0_86#3#0_87#3#0_88#3#0_89#3#0_90#3#0_91#3#0_92#3#0_93#3#0_94#3#0_95#3#0_96#3#0_97#1#0_98#3#0_99#3#0_100#3#0";
			//neposredno pre otkljucavanja drugog ostrva
			//string qqqq = "1#3#1380_2#3#0_3#3#0_4#2#1200_5#3#1120_6#3#2120_7#3#2300_8#3#2340_9#1#310_10#2#1010_11#2#0_12#3#0_13#1#0_14#3#0_15#2#0_16#3#0_17#3#0_18#2#0_19#3#0_20#0#0_21#-1#0_22#-1#0_23#-1#0_24#-1#0_25#-1#0_26#-1#0_27#-1#0_28#-1#0_29#-1#0_30#-1#0_31#-1#0_32#-1#0_33#-1#0_34#-1#0_35#-1#0_36#-1#0_37#-1#0_38#-1#0_39#-1#0_40#-1#0_41#-1#0_42#-1#0_43#-1#0_44#-1#0_45#-1#0_46#-1#0_47#-1#0_48#-1#0_49#-1#0_50#-1#0_51#-1#0_52#-1#0_53#-1#0_54#-1#0_55#-1#0_56#-1#0_57#-1#0_58#-1#0_59#-1#0_60#-1#0_61#-1#0_62#-1#0_63#-1#0_64#-1#0_65#-1#0_66#-1#0_67#-1#0_68#-1#0_69#-1#0_70#-1#0_71#-1#0_72#-1#0_73#-1#0_74#-1#0_75#-1#0_76#-1#0_77#-1#0_78#-1#0_79#-1#0_80#-1#0_81#-1#2500_82#-1#2120_83#-1#0_84#-1#0_85#-1#0_86#-1#0_87#-1#0_88#-1#0_89#-1#0_90#-1#0_91#-1#0_92#-1#0_93#-1#0_94#-1#0_95#-1#0_96#-1#0_97#-1#0_98#-1#0_99#-1#0_100#-1#0";
			//neposredno pre otkljucavanja treceg ostrva
			//string qqqq = "1#3#2000_2#3#2000_3#3#2000_4#3#2000_5#3#2000_6#3#2000_7#3#2000_8#3#2000_9#3#2000_10#3#2000_11#3#2000_12#3#2000_13#3#2000_14#3#2000_15#3#2000_16#3#2000_17#3#2000_18#3#2000_19#3#2000_20#3#2000_21#3#2000_22#3#2000_23#3#2000_24#3#2000_25#3#2000_26#3#2000_27#3#2000_28#3#2000_29#3#2000_30#3#2000_31#3#2000_32#3#2000_33#3#2000_34#3#2000_35#3#2000_36#3#2000_37#3#2000_38#3#2000_39#3#2000_40#0#0_41#-1#0_42#-1#0_43#-1#0_44#-1#0_45#-1#0_46#-1#0_47#-1#0_48#-1#0_49#-1#0_50#-1#0_51#-1#0_52#-1#0_53#-1#0_54#-1#0_55#-1#0_56#-1#0_57#-1#0_58#-1#0_59#-1#0_60#-1#0_61#-1#0_62#-1#0_63#-1#0_64#-1#0_65#-1#0_66#-1#0_67#-1#0_68#-1#0_69#-1#0_70#-1#0_71#-1#0_72#-1#0_73#-1#0_74#-1#0_75#-1#0_76#-1#0_77#-1#0_78#-1#0_79#-1#0_80#-1#0_81#-1#0_82#-1#0_83#-1#0_84#-1#0_85#-1#0_86#-1#0_87#-1#0_88#-1#0_89#-1#0_90#-1#0_91#-1#0_92#-1#0_93#-1#0_94#-1#0_95#-1#0_96#-1#0_97#-1#0_98#-1#0_99#-1#0_100#-1#0";
			//zadnji predjen 81. nivo
			//string qqqq = "1#3#1420_2#3#1890_3#3#2040_4#3#1770_5#3#1800_6#3#2770_7#3#2300_8#3#1680_9#3#2580_10#3#2200_11#3#2050_12#3#2160_13#3#3270_14#3#2330_15#3#3020_16#3#2240_17#3#2270_18#3#2480_19#3#2240_20#3#2520_21#3#2190_22#3#2370_23#3#2800_24#3#2620_25#3#2220_26#3#2590_27#3#3320_28#3#2210_29#2#1830_30#3#2560_31#3#2680_32#3#2220_33#3#2380_34#3#3990_35#3#2990_36#3#2250_37#3#2960_38#3#2260_39#3#2310_40#3#2210_41#3#2080_42#3#3430_43#3#2690_44#3#2120_45#3#2190_46#3#2710_47#3#3220_48#3#2100_49#3#2870_50#3#2420_51#3#2650_52#3#2240_53#3#2460_54#3#2470_55#3#3420_56#3#2750_57#3#2540_58#3#2220_59#3#2050_60#3#2120_61#3#2440_62#3#2470_63#3#2490_64#3#2130_65#3#2060_66#3#2780_67#3#2460_68#3#3120_69#3#2820_70#3#2070_71#3#2620_72#2#1500_73#3#2090_74#3#2260_75#3#3000_76#0#0_77#-1#0_78#-1#0_79#-1#0_80#-1#0_81#-1#0_82#-1#0_83#-1#0_84#-1#0_85#-1#0_86#-1#0_87#-1#0_88#-1#0_89#-1#0_90#-1#0_91#-1#0_92#-1#0_93#-1#0_94#-1#0_95#-1#0_96#-1#0_97#-1#0_98#-1#0_99#-1#0_100#-1#0";
			//allLevels = qqqq.Split('_');
			//PlayerPrefs.SetString("AllLevels",qqqq);

			//@@@@@@@ ZA TEST, RESETOVANJE NIVOA
//			string pom = "1#0#0";
//			for(int i=1;i<allLevels.Length;i++)
//			{
//				pom+="_"+(i+1).ToString()+"#-1#0";
//			}
//			allLevels = pom.Split('_');

			//@@@@@@ DODATAK ZA NOVA OSTRVA

			string[] pomNiz = new string[allLevels.Length];
			//@@@@@@ OVO POSLE TREBA DA SE VRATI
			pomNiz = PlayerPrefs.GetString("AllLevels").Split('_');
			
			//@@@@@@ TRENUTNI NIVOI KOJI CE DA SE ISKOPIRAJU U ALL LEVELS
			//pomNiz = qqqq.Split('_');


			if(pomNiz.Length != allLevels.Length)
			{
				for(int i=0;i<pomNiz.Length;i++)
					allLevels[i] = pomNiz[i];

				for(int i=pomNiz.Length; i<allLevels.Length;i++)
				{
					allLevels[i] = (i+1).ToString()+"#-1#0";
				}

				allLevels[pomNiz.Length] = (pomNiz.Length+1).ToString() + "#0#0";

				string pom = System.String.Empty;
				for(int i=0;i<StagesParser.allLevels.Length;i++)
				{
					pom+=StagesParser.allLevels[i];
					pom+="_";
				}
				pom = pom.Remove(pom.Length-1);
				PlayerPrefs.SetString("AllLevels",pom);
				PlayerPrefs.Save();
			}
			else
			{
				allLevels = PlayerPrefs.GetString("AllLevels").Split('_');
			}
			//@@@@@@

		}

		////////////////////////////////////////////////////////
		// **************** INICIJALIZACIJA END ****************
		////////////////////////////////////////////////////////


		////////////////////////////////////////////////////////
		////////////////////////////////////////////////////////
		//////////////////////////////////////////////////////// 

		totalSets = 6;
		maxLevelNaOstrvu = new int[totalSets];
		SetsInGame= new Set[totalSets];
		trenutniNivoNaOstrvu = new int[totalSets];
		for(int i=0;i<totalSets;i++)
		{
			int stagesInSet=20;
			SetsInGame[i]= new Set(stagesInSet);
			SetsInGame[i].StagesOnSet=stagesInSet;
			//SetsInGame[i].StarRequirement=45;
			SetsInGame[i].SetID= (i+1).ToString();
			SetsInGame[i].TotalStarsInStage+=3*stagesInSet;
			if(PlayerPrefs.HasKey("TrenutniNivoNaOstrvu"+i.ToString()))
				trenutniNivoNaOstrvu[i] = PlayerPrefs.GetInt("TrenutniNivoNaOstrvu"+i.ToString());
			else
				trenutniNivoNaOstrvu[i] = 1;

//			for(int j=0;j<stagesInSet;j++)
//			{
//				if(j!=0)
//				SetsInGame[i].SetStarOnStage(j, -1);
//				else
//					SetsInGame[i].SetStarOnStage(j, 0);
//			}
		}

		// **************** INICIJALIZACIJA BONUS NIVOA ********
		if(!PlayerPrefs.HasKey("BonusLevel"))
		{
			string pom = System.String.Empty;
			for(int i=0;i<StagesParser.totalSets;i++)
				pom += "-1#-1#-1#-1_";
			pom = pom.Remove(pom.Length-1);
			PlayerPrefs.SetString("BonusLevel",pom);
			PlayerPrefs.Save();
			bonusLevels = pom;

		}
		else
		{
			bonusLevels = PlayerPrefs.GetString("BonusLevel");

			//@@@@@@ DODATAK ZA NOVA OSTRVA

			string[] BonusValues = bonusLevels.Split('_');
			if(BonusValues.Length < totalSets)
			{
				for(int i=BonusValues.Length;i<totalSets;i++)
				{
					bonusLevels += "_-1#-1#-1#-1";
				}
				PlayerPrefs.SetString("BonusLevel",bonusLevels);
				PlayerPrefs.Save();
			}

			//@@@@@@

			// rucni reset
//			string pom = System.String.Empty;
//			for(int i=0;i<StagesParser.totalSets;i++)
//				pom += "-1#-1#-1#-1_";
//			pom = pom.Remove(pom.Length-1);
//			PlayerPrefs.SetString("BonusLevel",pom);
//			PlayerPrefs.Save();
//			bonusLevels = pom;
		}

		// **************** INICIJALIZACIJA BONUS END   ********

		totalStars=0;
		currentStars=0;
		for(int i=0;i<totalSets;i++)
		{
			totalStars+=3*SetsInGame[i].StagesOnSet;
			for(int j=0;j<SetsInGame[i].StagesOnSet;j++)
			{
				currentStars+= ((SetsInGame[i].GetStarOnStage(j)<0)?0:SetsInGame[i].GetStarOnStage(j));
			}
		}
		if(PlayerPrefs.HasKey("CurrentStars"))
			currentStarsNEW = PlayerPrefs.GetInt("CurrentStars");
		stagesLoaded=true;
		prefs = false;

		if(!PlayerPrefs.HasKey("Tour"))
			tour = 1;
		else
			tour = PlayerPrefs.GetInt("Tour");

		switch(tour)
		{
		case 1: SetsInGame[0].StarRequirement = 0; 
				SetsInGame[1].StarRequirement = 40; 
				SetsInGame[2].StarRequirement = 85; 
				SetsInGame[3].StarRequirement = 135;
				SetsInGame[4].StarRequirement = 185;
				SetsInGame[5].StarRequirement = 235; break;

		case 2: SetsInGame[0].StarRequirement = 0; 
				SetsInGame[1].StarRequirement = 50; 
				SetsInGame[2].StarRequirement = 100; 
				SetsInGame[3].StarRequirement = 150;
				SetsInGame[4].StarRequirement = 200; 
				SetsInGame[5].StarRequirement = 260; break;

		case 3: SetsInGame[0].StarRequirement = 0; 
				SetsInGame[1].StarRequirement = 55; 
				SetsInGame[2].StarRequirement = 110; 
				SetsInGame[3].StarRequirement = 165;
				SetsInGame[4].StarRequirement = 220;
				SetsInGame[5].StarRequirement = 280; break;
		}

		RecountTotalStars();
//		for (int i = 0; i < StagesParser.totalSets; i++)
//		{
//			if(StagesParser.currentStarsNEW >= StagesParser.SetsInGame[i].StarRequirement)
//				StagesParser.unlockedWorlds[i] = true;
//		}
		for (int i = 0; i < StagesParser.totalSets; i++)
		{
			if(StagesParser.currentStarsNEW >= StagesParser.SetsInGame[i].StarRequirement && i > 0 && int.Parse(allLevels[(i-1)*20+19].Split('#')[1])>0)
			{
				StagesParser.unlockedWorlds[i] = true;
			}
			else
			{
				;
			}
		}
		StagesParser.unlockedWorlds[0] = true;

		for(int i=0;i<StagesParser.totalSets;i++)
		{
			if(PlayerPrefs.HasKey("WatchVideoWorld"+(i+1)))
			{
				PlayerPrefs.DeleteKey("WatchVideoWorld"+(i+1));
			}
		}
		StartCoroutine(checkInternetConnection());
		//StartCoroutine("LoadStages");
	}

//	public void ProveraLoginPrviPut()
//	{
//		if(PlayerPrefs.HasKey("Logovan")) //vec se logovao ranije
//		{
//			if(PlayerPrefs.GetInt("Logovan") < 1)
//			{
//				PlayerPrefs.SetInt("Logovan",1);
//				PlayerPrefs.Save();
//			}
//		}
//		else //prvi put se loguje
//		{
//			string pomLevels = PlayerPrefs.GetString("AllLevels");
//			PlayerPrefs.SetString("AllLevelsFB",pomLevels);
//			PlayerPrefs.Save();
//
//			PlayerPrefs.SetInt("TotalMoneyFB",currentMoney);
//			PlayerPrefs.SetInt("TotalBananasFB",currentBananas);
//			PlayerPrefs.SetInt("CurrentStarsFB",currentStarsNEW);
//			PlayerPrefs.SetInt("PoslednjiOdigranNivoFB",PlayerPrefs.GetInt("PoslednjiOdigranNivo"));
//			PlayerPrefs.SetString("PowerUpsFB",PlayerPrefs.GetString("PowerUps"));
//			PlayerPrefs.SetInt("TotalPointsFB",currentPoints);
//			PlayerPrefs.SetString("choosenLanguageFB",PlayerPrefs.GetString("choosenLanguage"));
//			PlayerPrefs.SetInt("Logovan",0);
//			PlayerPrefs.Save();
//		}
//	}
	public void ObrisiProgresNaLogOut()
	{
		int levelReward = 0;
		int proveriVreme = 0;
		float vremeBrojaca = 0;
		string vremeQuit = System.String.Empty;
		string odgledaoTutorialiShop = System.String.Empty;
		ShopManagerFull.ShopObject.OcistiMajmuna();

		if(PlayerPrefs.HasKey("LevelReward"))
			levelReward = PlayerPrefs.GetInt("LevelReward");
		if(PlayerPrefs.HasKey("ProveriVreme"))
			proveriVreme = PlayerPrefs.GetInt("ProveriVreme");
		if(PlayerPrefs.HasKey("VremeBrojaca"))
			vremeBrojaca = PlayerPrefs.GetFloat("VremeBrojaca");
		if(PlayerPrefs.HasKey("VremeQuit"))
			vremeQuit = PlayerPrefs.GetString("VremeQuit");
		if(PlayerPrefs.HasKey("OdgledaoTutorial"))
			odgledaoTutorialiShop = PlayerPrefs.GetString("OdgledaoTutorial");

		PlayerPrefs.DeleteAll();

		PlayerPrefs.SetInt("Logovan",0);
		PlayerPrefs.SetInt("Logout",1);
		PlayerPrefs.SetInt("LevelReward",levelReward);
		PlayerPrefs.SetInt("ProveriVreme",proveriVreme);
		PlayerPrefs.SetFloat("VremeBrojaca",vremeBrojaca);
		PlayerPrefs.SetString("VremeQuit",vremeQuit);
		PlayerPrefs.SetString("OdgledaoTutorial",odgledaoTutorialiShop);
		PlayerPrefs.SetInt("VecPokrenuto",1);



		//PlayerPrefs.Save();

		//ponovna inicijalizacija nivoa
		string pom = "1#0#0";
		for(int i=1;i<allLevels.Length;i++)
		{
			pom+="_"+(i+1).ToString()+"#-1#0";
		}
		allLevels = pom.Split('_');
		PlayerPrefs.SetString("AllLevels",pom);
		//PlayerPrefs.Save();

		//ponovna inicijalizacija bonus nivoa
		pom = System.String.Empty;
		for(int i=0;i<StagesParser.totalSets;i++)
			pom += "-1#-1#-1#-1_";
		pom = pom.Remove(pom.Length-1);
		PlayerPrefs.SetString("BonusLevel",pom);
		//PlayerPrefs.Save();
		bonusLevels = pom;

		for(int i=0;i<totalSets;i++)
		{
			trenutniNivoNaOstrvu[i] = 1;
			PlayerPrefs.SetInt("TrenutniNivoNaOstrvu"+i.ToString(),trenutniNivoNaOstrvu[i]);
		}

		//provera za zvezdice i otkljucane svetove
		for (int i = 0; i < StagesParser.totalSets; i++)
		{
			StagesParser.unlockedWorlds[i] = false;
		}
		RecountTotalStars();
//		for (int i = 0; i < StagesParser.totalSets; i++)
//		{
//			if(StagesParser.currentStarsNEW >= StagesParser.SetsInGame[i].StarRequirement)
//				StagesParser.unlockedWorlds[i] = true;
//		}
		for (int i = 0; i < StagesParser.totalSets; i++)
		{
			if(StagesParser.currentStarsNEW >= StagesParser.SetsInGame[i].StarRequirement && i > 0 && int.Parse(allLevels[(i-1)*20+19].Split('#')[1])>0)
			{
				StagesParser.unlockedWorlds[i] = true;
			}
			else
			{
				;
			}
		}
		StagesParser.unlockedWorlds[0] = true;
		lastUnlockedWorldIndex = 0;
		imaUsi = true;
		imaKosu = true;
		majica = -1;
		glava = -1;
		ledja = -1;
		ShopManagerFull.AktivanSesir = -1;
		ShopManagerFull.AktivnaMajica = -1;
		ShopManagerFull.AktivanRanac = -1;
		bojaMajice = Color.white;
		svekupovineGlava = System.String.Empty;
		svekupovineMajica = System.String.Empty;
		svekupovineLedja = System.String.Empty;
		currentMoney = 0;
		currentBananas = 0;
		bananaCost = 2000;
		currentPoints = 0;
		powerup_magnets = 0;
		powerup_doublecoins = 0;
		powerup_shields = 0;
		cost_magnet = 150;
		cost_doublecoins = 300;
		cost_shield = 600;

		ShopManagerFull.ShopObject.transform.Find("3 Customize/Customize Tabovi/1Hats").gameObject.SetActive(true);
		ShopManagerFull.ShopObject.transform.Find("3 Customize/Customize Tabovi/2Shirts").gameObject.SetActive(true);
		ShopManagerFull.ShopObject.transform.Find("3 Customize/Customize Tabovi/3BackPack").gameObject.SetActive(true);

		LanguageManager.chosenLanguage = "_en";
		Camera.main.SendMessage("PromeniZastavuNaOsnovuImena",SendMessageOptions.DontRequireReceiver);
		Texture Zastava = Resources.Load("Zastave/0") as Texture;
		GameObject.FindGameObjectWithTag("Zastava").GetComponent<Renderer>().material.SetTexture("_MainTex", Zastava);
		ShopManagerFull.ShopObject.SviItemiInvetory();
		ShopManagerFull.ShopObject.PobrisiSveOtkljucanoIzShopa();
		ShopManagerFull.ShopObject.RefresujImenaItema();

		FacebookManager.Ulogovan = false;
		PlayerPrefs.Save();
		UgasiLoading();

	}

	public static void RecountTotalStars()
	{
		string[] values;
		currentStarsNEW = 0;
		for(int k = 0; k<totalSets; k++)
		{
			maxLevelNaOstrvu[k] = 0;
			SetsInGame[k].CurrentStarsInStageNEW = 0;
			for(int i=0; i<SetsInGame[k].StagesOnSet; i++)
			{
				values = allLevels[k*20+i].Split('#');
				SetsInGame[k].SetStarOnStage(i, int.Parse(values[1]));

				if(int.Parse(values[1])>-1)
				{
					SetsInGame[k].CurrentStarsInStageNEW += int.Parse(values[1]);
					maxLevel = k*20+i+1;
				}
				if(int.Parse(values[1])>0)
				{
					maxLevelNaOstrvu[k] = i+1;
				}
				PointsPoNivoima[k*20+i] = int.Parse(values[2]);
				StarsPoNivoima[k*20+i] = int.Parse(values[1]);

				//@@@@@@@@@@@@@@@@ VISAK ZASAD
//				//StagesParser.currentLevel = StagesParser.SetsInGame[StagesParser.currSetIndex]*10 + ;
//				if(PlayerPrefs.HasKey("Level"+(k*20+i+1)))
//				{
//					string level = PlayerPrefs.GetString("Level"+(k*20+i+1));
//					//Debug.Log("Level " + level);
//					values = level.Split('#');
//					SetsInGame[k].SetStarOnStage(i,int.Parse(values[1]));
//					SetsInGame[k].CurrentStarsInStageNEW += int.Parse(values[1]);
//					//Debug.Log("trenutno zvezdica: " + int.Parse(values[1]) + ", ukupan broj: " + SetsInGame[k].CurrentStarsInStageNEW);
//					maxLevel = k*20+i+1;
//					PointsPoNivoima[k*20+i] = int.Parse(values[2]);
//					StarsPoNivoima[k*20+i] = int.Parse(values[1]);
//					//Debug.Log("AOJEOOEOJEOJEO >>>  na " + (k*20+i) + ". nivo ima " + StarsPoNivoima[k*20+i] + " zvezdica i " + PointsPoNivoima[k*20+i] + " poena");
//				}
//				else
//				{
//					SetsInGame[k].SetStarOnStage(i,-1);
//				}
				//@@@@@@@@@@@@@@@@ END OF VISAK
			}
			currentStarsNEW += SetsInGame[k].CurrentStarsInStageNEW;
			//Debug.Log("Set " + k + ", stars: " + SetsInGame[k].CurrentStarsInStageNEW);
		}
	}

	public IEnumerator checkInternetConnection()
	{
		WWW www = new WWW("https://www.google.com");
		yield return www;
		if(!string.IsNullOrEmpty(www.error))
		{
			internetOn = false;
		}
		else
		{
			internetOn = true;
		}
	}

	public void CallLoad()
	{
		StartCoroutine("LoadStages");
	}
	public void CallSave()
	{
		StartCoroutine("SaveStages");
	}
	public IEnumerator LoadStages()
	{
		if(PlayerPrefs.HasKey("firstSave18819"))
		{
			Debug.Log("Ima key firstSave18819");
		}
		else
		{
			Debug.Log("NEMA key firstSave18819");
		}

		stagesLoaded=false;
		string	filePath=System.String.Empty;
		string result=System.String.Empty;
		
		Debug.Log("Streaming assets: " + System.IO.Path.Combine(Application.streamingAssetsPath, xmlName));
		Debug.Log("Persistent data: " + System.IO.Path.Combine(Application.persistentDataPath, xmlName));
		
		if(PlayerPrefs.HasKey("firstSave18819") && System.IO.File.Exists(System.IO.Path.Combine(Application.persistentDataPath, xmlName)))
		{
			filePath = System.IO.Path.Combine(Application.persistentDataPath, xmlName);
			Debug.Log("usao i pokusao: " + filePath);
		}
		else if(PlayerPrefs.HasKey("starsandstages"))
		{
			prefs = true;
			result = PlayerPrefs.GetString("starsandstages");
			Debug.Log("result je (iz load stages): " + result);
		}
		else 
		{
			filePath = System.IO.Path.Combine(Application.streamingAssetsPath, xmlName);
		}
		
		
		//filePath = System.IO.Path.Combine(Application.streamingAssetsPath, xmlName);
		
		
		if (filePath.Contains("://")) //streaming assets
		{
			Debug.Log("koliko puta ovde");
			WWW www = new WWW(filePath);
			yield return www;
			if(!string.IsNullOrEmpty(www.error))
			{
				Debug.Log("Error se desio: " + www.error + ", " + filePath);
				//if(PlayerPrefs.HasKey("firstSave11"))
				//	PlayerPrefs.DeleteKey("firstSave11");
				//yield break;
			}
			else
			{
				result = www.text;
				Debug.Log("result: " + result);
			}
			
		}
		else if(!prefs)//persistent data
		{
			Debug.Log("Usao u persistent data");
			if(filePath == System.String.Empty)
			{
				Debug.Log("Uleteo ovde, prazna staza, brisi key");
				PlayerPrefs.DeleteKey("firstSave18819");
				StartCoroutine("LoadStages");
				yield break;
			}
			else 
			{
				result = System.IO.File.ReadAllText(filePath);
			}
		}
		XElement xmlNov= XElement.Parse(result);
		IEnumerable<XElement> xmls = xmlNov.Elements();	
		totalSets=xmls.Count();
		SetsInGame= new Set[totalSets];
		IEnumerable<XElement> helpXmls;
		for(int i=0;i<totalSets;i++)
		{
			helpXmls=xmls.ElementAt(i).Elements();	
			int stagesInSet=helpXmls.Count();
			SetsInGame[i]= new Set(stagesInSet);
			SetsInGame[i].StagesOnSet=stagesInSet;
			SetsInGame[i].StarRequirement=int.Parse( xmls.ElementAt(i).Attribute("req").Value);
			SetsInGame[i].SetID= xmls.ElementAt(i).Attribute("id").Value;
			SetsInGame[i].TotalStarsInStage+=3*stagesInSet;
			for(int j=0;j<stagesInSet;j++)
			{
				
				SetsInGame[i].SetStarOnStage(j, int.Parse(helpXmls.ElementAt(j).Value));
				
			}
		}
		totalStars=0;
		currentStars=0;
		for(int i=0;i<totalSets;i++)
		{
			totalStars+=3*SetsInGame[i].StagesOnSet;
			for(int j=0;j<SetsInGame[i].StagesOnSet;j++)
			{
				currentStars+= ((SetsInGame[i].GetStarOnStage(j)<0)?0:SetsInGame[i].GetStarOnStage(j));
			}
		}
		stagesLoaded=true;
		prefs = false;
	}
	
	
	public IEnumerator SaveStages()
	{
		string filePath=System.String.Empty;
		string result=System.String.Empty;
		if(PlayerPrefs.HasKey("firstSave18819") && System.IO.File.Exists(System.IO.Path.Combine(Application.persistentDataPath, xmlName)))
		{
			filePath = System.IO.Path.Combine(Application.persistentDataPath, xmlName);
		}
		else if(PlayerPrefs.HasKey("starsandstages"))
		{
			prefs = true;
			result = PlayerPrefs.GetString("starsandstages");
		}
		else 
		{
			filePath = System.IO.Path.Combine(Application.streamingAssetsPath, xmlName);
		}
		
		
		if (filePath.Contains("://")) 
		{
			WWW www = new WWW(filePath);
			yield return www;
			result = www.text;
		} 
		else if(!prefs)
		{
			result = System.IO.File.ReadAllText(filePath);
		}
		
		XElement xmlNov= XElement.Parse(result);
		IEnumerable<XElement> xmls = xmlNov.Elements();	
		IEnumerable<XElement> helpXml;
		for(int i=0;i<SetsInGame.Length;i++)
		{
			helpXml=xmls.ElementAt(i).Elements();
			for(int j=0;j<SetsInGame[i].StagesOnSet;j++)
				helpXml.ElementAt(j).Value=SetsInGame[i].GetStarOnStage(j).ToString();
			
			
		}
		//filePath = System.IO.Path.Combine(Application.persistentDataPath, xmlName);
		//xmlNov.Save(filePath);
		if(PlayerPrefs.HasKey("firstSave18819"))
		{
			Debug.Log("brisem iz persistent data xml");
			PlayerPrefs.DeleteKey("firstSave18819");
		}
		string starsAndStages = xmlNov.ToString();
		PlayerPrefs.SetString("starsandstages",starsAndStages);
		PlayerPrefs.Save();
		//Debug.Log("xml u prefs: " + PlayerPrefs.GetString("starsandstages"));
		//PlayerPrefs.SetInt("firstSave11",1);
		//PlayerPrefs.Save();
		saving=true;
		prefs = false;
	}

	public IEnumerator moneyCounter(int kolicina, TextMesh moneyText, bool hasOutline)
	{
		if(kolicina>0)
		{
			if(PlaySounds.soundOn)
				PlaySounds.Play_CoinsSpent();
		}

		int current = int.Parse(moneyText.text);
		int suma = current + kolicina;
		int korak = (suma - current)/10;
		while(current != suma)
		{
			current += korak;
			moneyText.text = current.ToString();
			if(hasOutline)
				moneyText.GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			yield return new WaitForSeconds(0.07f);
		}
		moneyText.text = StagesParser.currentMoney.ToString();
		moneyText.GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
	}

	public void UcitajLoadingPoruke()
	{
		TextAsset aset =(TextAsset)Resources.Load("LoadingBackground/Loading"+LanguageManager.chosenLanguage);
		XElement xmlNov= XElement.Parse(aset.ToString());
		IEnumerable<XElement> xmls = xmlNov.Elements(); 

		int broj = xmls.Count();
		for(int i=0;i<broj;i++)
		{
			RedniBrojSlike.Add(int.Parse(xmls.ElementAt(i).Attribute("redniBroj").Value));
		}
		
		LoadingPoruke.Add(xmls.ElementAt(0).Value);
		LoadingPoruke.Add(xmls.ElementAt(1).Value);
		LoadingPoruke.Add(xmls.ElementAt(2).Value);
		LoadingPoruke.Add(xmls.ElementAt(3).Value);
		LoadingPoruke.Add(xmls.ElementAt(4).Value);
		LoadingPoruke.Add(xmls.ElementAt(5).Value);
		LoadingPoruke.Add(xmls.ElementAt(6).Value);
		LoadingPoruke.Add(xmls.ElementAt(7).Value);
		LoadingPoruke.Add(xmls.ElementAt(8).Value);
		LoadingPoruke.Add(xmls.ElementAt(9).Value);
		LoadingPoruke.Add(xmls.ElementAt(10).Value);
		LoadingPoruke.Add(xmls.ElementAt(11).Value);
		LoadingPoruke.Add(xmls.ElementAt(12).Value);
		LoadingPoruke.Add(xmls.ElementAt(13).Value);
		LoadingPoruke.Add(xmls.ElementAt(14).Value);
		LoadingPoruke.Add(xmls.ElementAt(15).Value);
		LoadingPoruke.Add(xmls.ElementAt(16).Value);
		LoadingPoruke.Add(xmls.ElementAt(17).Value);
		LoadingPoruke.Add(xmls.ElementAt(18).Value);
		LoadingPoruke.Add(xmls.ElementAt(19).Value);
		LoadingPoruke.Add(xmls.ElementAt(20).Value);
		LoadingPoruke.Add(xmls.ElementAt(21).Value);
		//LoadingPoruke.Add(xmls.ElementAt(22).Value);
		//LoadingPoruke.Add(xmls.ElementAt(23).Value);
		//LoadingPoruke.Add(xmls.ElementAt(24).Value);
		//LoadingPoruke.Add(xmls.ElementAt(25).Value);
		//LoadingPoruke.Add(xmls.ElementAt(26).Value);
		//LoadingPoruke.Add(xmls.ElementAt(27).Value);
		//LoadingPoruke.Add(xmls.ElementAt(28).Value);
	}

	public void CompareScores()
	{
		Debug.Log("COMPARE SCORES - FacebookManager.User: " + FacebookManager.User + ", LastLoggedUser: " + lastLoggedUser + ", LogoutPrefs: " + PlayerPrefs.GetInt("Logout"));
		//if(FacebookManager.UserScore > currentPoints)
		if(!FacebookManager.User.Equals(lastLoggedUser) || lastLoggedUser.Equals(System.String.Empty) || PlayerPrefs.GetInt("Logout") == 1)
		{
			Debug.Log("USAO U PRVI USLOV");
			currentMoney = FacebookManager.UserCoins;
			currentPoints = FacebookManager.UserScore;
			LanguageManager.chosenLanguage = FacebookManager.UserLanguage;
			currentBananas = FacebookManager.UserBanana;
			powerup_magnets = FacebookManager.UserPowerMagnet;
			powerup_shields = FacebookManager.UserPowerShield;
			powerup_doublecoins = FacebookManager.UserPowerDoubleCoins;
			glava = FacebookManager.GlavaItem;
			majica = FacebookManager.TeloItem;
			if(majica >= 0)
				bojaMajice = ShopManagerFull.ShopObject.TShirtColors[majica];
			ledja = FacebookManager.LedjaItem;
			imaUsi = FacebookManager.Usi;
			imaKosu = FacebookManager.Kosa;
			svekupovineGlava = FacebookManager.UserSveKupovineHats;
			svekupovineMajica = FacebookManager.UserSveKupovineShirts;
			svekupovineLedja = FacebookManager.UserSveKupovineBackPacks;
			bonusLevels = FacebookManager.bonusLevels;

			LanguageManager.RefreshTexts();

			for(int i=0;i<totalSets;i++)
			{
				trenutniNivoNaOstrvu[i] = 1;
				PlayerPrefs.SetInt("TrenutniNivoNaOstrvu"+i.ToString(),trenutniNivoNaOstrvu[i]);
			}
			bool proveriJednom = false;

			for(int s=0;s<FacebookManager.ListaStructPrijatelja.Count;s++)
			{
				FacebookManager.StrukturaPrijatelja Igrac=FacebookManager.ListaStructPrijatelja[s];
				if(Igrac.PrijateljID==FacebookManager.User && !proveriJednom)
				{
					proveriJednom = true;
					FacebookManager.indexUListaStructPrijatelja=s;
					for(int i=0;i<Igrac.scores.Count;i++) //@@@@@@ BILO JE i<100
					{
						PointsPoNivoima[i] = Igrac.scores[i];
						StarsPoNivoima[i] = Igrac.stars[i];
					}
					maxLevel = Igrac.MaxLevel;
				}
			}

			string pom = System.String.Empty;
			for(int i=0;i<PointsPoNivoima.Length;i++) //@@@@@@ BILO JE i<100
			{
				pom+=(i+1).ToString()+"#"+StarsPoNivoima[i].ToString()+"#"+PointsPoNivoima[i].ToString()+"_";
			}
			pom=pom.Remove(pom.Length-1);

			//@@@@@@ DODATAK ZA NOVA OSTRVA
			string[] pomNiz = pom.Split('_');
			if(allLevels.Length > pomNiz.Length)
			{
				for(int i=0;i<pomNiz.Length;i++)
				{
					allLevels[i] = pomNiz[i];
				}
				for(int i=pomNiz.Length;i<allLevels.Length;i++)
				{
					allLevels[i] =  (i+1).ToString()+"#-1#0";
				}
				allLevels[pomNiz.Length] = pomNiz.Length.ToString()+"#0#0";

				pom = System.String.Empty;
				for(int i=0;i<allLevels.Length;i++)
				{
					pom+=allLevels[i];
					pom+="_";
				}
				pom = pom.Remove(pom.Length-1);
			}
			else
			{
				allLevels = pom.Split('_');
			}

			//@@@@@@ DODATAK ZA NOVA OSTRVA - BONUS NIVOI

			string[] BonusValues = bonusLevels.Split('_');
			if(BonusValues.Length < totalSets)
			{
				for(int i=BonusValues.Length;i<totalSets;i++)
				{
					bonusLevels += "_-1#-1#-1#-1";
				}
				PlayerPrefs.SetString("BonusLevel",bonusLevels);
				PlayerPrefs.Save();
			}

			//@@@@@@

			PlayerPrefs.SetString("AllLevels",pom);
			RecountTotalStars();
			for (int i = 0; i < StagesParser.totalSets; i++)
			{
				if(StagesParser.currentStarsNEW >= StagesParser.SetsInGame[i].StarRequirement && i > 0 && int.Parse(allLevels[(i-1)*20+19].Split('#')[1])>0)
				{
					StagesParser.unlockedWorlds[i] = true;
					lastUnlockedWorldIndex = i;
				}
				else
				{
					;
				}
			}
			StagesParser.unlockedWorlds[0] = true;



			Debug.Log("stigao do pred Shop");


			if(FacebookManager.MestoPozivanjaLogina != 2)
			{
				Debug.Log("login pozvan u 1. ili 3. sceni");
				ShopManagerFull.ShopObject.OcistiMajmuna();
				
				ShopManagerFull.ShopObject.transform.Find("3 Customize/Customize Tabovi/1Hats").gameObject.SetActive(true);
				ShopManagerFull.ShopObject.transform.Find("3 Customize/Customize Tabovi/2Shirts").gameObject.SetActive(true);
				ShopManagerFull.ShopObject.transform.Find("3 Customize/Customize Tabovi/3BackPack").gameObject.SetActive(true);
				
				svekupovineGlava = FacebookManager.UserSveKupovineHats;
				svekupovineMajica = FacebookManager.UserSveKupovineShirts;
				svekupovineLedja = FacebookManager.UserSveKupovineBackPacks;
				glava = FacebookManager.GlavaItem;
				majica = FacebookManager.TeloItem;
				if(majica >= 0)
					bojaMajice = ShopManagerFull.ShopObject.TShirtColors[majica];
				ledja = FacebookManager.LedjaItem;
				imaUsi = FacebookManager.Usi;
				imaKosu = FacebookManager.Kosa;
				ShopManagerFull.AktivanSesir=glava;
				ShopManagerFull.AktivnaMajica=majica;
				ShopManagerFull.AktivanRanac=ledja;
				string niz=ShopManagerFull.AktivanSesir.ToString()+"#"+ShopManagerFull.AktivnaMajica.ToString()+"#"+ShopManagerFull.AktivanRanac.ToString();
				PlayerPrefs.SetString("AktivniItemi", niz);
				PlayerPrefs.Save();
				ShopManagerFull.ShopObject.SviItemiInvetory();
				ShopManagerFull.ShopObject.PobrisiSveOtkljucanoIzShopa();
				ShopManagerFull.ShopObject.RefresujImenaItema();
				if(FacebookManager.MestoPozivanjaLogina == 1)
					ShopManagerFull.ShopObject.ObuciMajmunaNaStartu();
			}
			else 
			{
				obucenSeLogovaoNaDrugojSceni = true;
				Debug.Log("logovao se na 2. sceni");
			}

			//OVO JE BILO DOLE VAN IF-A
			PlayerPrefs.SetInt("TotalMoney",currentMoney);
			PlayerPrefs.SetInt("TotalPoints",currentPoints);
			PlayerPrefs.SetString ("choosenLanguage", LanguageManager.chosenLanguage);
			PlayerPrefs.SetInt("TotalBananas",currentBananas);
			PlayerPrefs.SetString("PowerUps",(powerup_doublecoins+"#"+powerup_magnets+"#"+powerup_shields));
			PlayerPrefs.SetString("UserSveKupovineHats",svekupovineGlava);
			PlayerPrefs.SetString("UserSveKupovineShirts",svekupovineMajica);
			PlayerPrefs.SetString("UserSveKupovineBackPacks",svekupovineLedja);
			PlayerPrefs.SetString("BonusLevel",bonusLevels);
			
			PlayerPrefs.SetInt("Logout",0);
			lastLoggedUser = FacebookManager.User;
			PlayerPrefs.SetString("LastLoggedUser",lastLoggedUser);
			PlayerPrefs.Save();
			
			ServerUpdate = 0;



			Debug.Log("ZAVRSIO USLOV");

		}
		else
		{
			//Debug.Log("ELSE USAO JE ON");
			Debug.Log("NIJE UPAO U USLOV");
			//mozda i ovde da se ishendluje
		}

		if(FacebookManager.MestoPozivanjaLogina == 2)
		{
			Camera.main.SendMessage("RefreshScene",SendMessageOptions.DontRequireReceiver);
			//Camera.main.SendMessage("changeLanguage",SendMessageOptions.DontRequireReceiver);
			if(FacebookManager.FacebookObject.zavrsioUcitavanje)
			{
				Debug.Log("^^^^^^ GOTOVO ZA FACEBOOK, SKLONI DUGME!!!!! LOKACIJA CS2");
				Invoke("UgasiLoading",0.5f);
				PlayerPrefs.SetInt("Logovan",1);
				if(FacebookManager.MestoPozivanjaLogina == 1)
					FacebookManager.FacebookObject.RefreshujScenu1PosleLogina();
				else if(FacebookManager.MestoPozivanjaLogina == 2)
					FacebookManager.FacebookObject.RefreshujScenu2PosleLogina();
				else if(FacebookManager.MestoPozivanjaLogina == 3)
					FacebookManager.FacebookObject.RefreshujScenu3PosleLogina();
				FacebookManager.FacebookObject.zavrsioUcitavanje = false;
			}
			else
			{
				FacebookManager.FacebookObject.zavrsioUcitavanje = true;
				Debug.Log("##### ZAVRSIO UCITAVANJE, LOKACIJA CS2");
			}
		}
		else if(FacebookManager.MestoPozivanjaLogina == 3)
		{
			Camera.main.SendMessage("RefreshScene",SendMessageOptions.DontRequireReceiver);
			//Camera.main.SendMessage("changeLanguage",SendMessageOptions.DontRequireReceiver);
			if(FacebookManager.FacebookObject.zavrsioUcitavanje)
			{
				Debug.Log("^^^^^^ GOTOVO ZA FACEBOOK, SKLONI DUGME!!!!! LOKACIJA CS3");
				Invoke("UgasiLoading",0.5f);
				PlayerPrefs.SetInt("Logovan",1);
				if(FacebookManager.MestoPozivanjaLogina == 1)
					FacebookManager.FacebookObject.RefreshujScenu1PosleLogina();
				else if(FacebookManager.MestoPozivanjaLogina == 2)
					FacebookManager.FacebookObject.RefreshujScenu2PosleLogina();
				else if(FacebookManager.MestoPozivanjaLogina == 3)
					FacebookManager.FacebookObject.RefreshujScenu3PosleLogina();
				FacebookManager.FacebookObject.zavrsioUcitavanje = false;
			}
			else
			{
				FacebookManager.FacebookObject.zavrsioUcitavanje = true;
				Debug.Log("##### ZAVRSIO UCITAVANJE, LOKACIJA CS3");
			}
		}
		else
		{
			if(FacebookManager.FacebookObject.zavrsioUcitavanje)
			{
				Debug.Log("^^^^^^ GOTOVO ZA FACEBOOK, SKLONI DUGME!!!!! LOKACIJA CS1");
				Transform loading = GameObject.Find("Loading Buffer HOLDER").transform;
				loading.position = new Vector3(0,-70,loading.position.z);
				loading.GetChild(0).GetComponent<Animator>().StopPlayback();
				PlayerPrefs.SetInt("Logovan",1);
				if(FacebookManager.MestoPozivanjaLogina == 1)
					FacebookManager.FacebookObject.RefreshujScenu1PosleLogina();
				else if(FacebookManager.MestoPozivanjaLogina == 2)
					FacebookManager.FacebookObject.RefreshujScenu2PosleLogina();
				else if(FacebookManager.MestoPozivanjaLogina == 3)
					FacebookManager.FacebookObject.RefreshujScenu3PosleLogina();
				FacebookManager.FacebookObject.zavrsioUcitavanje = false;
			}
			else
			{
				FacebookManager.FacebookObject.zavrsioUcitavanje = true;
				Debug.Log("##### ZAVRSIO UCITAVANJE, LOKACIJA CS1");
			}
			Camera.main.SendMessage("PromeniZastavuNaOsnovuImena",SendMessageOptions.DontRequireReceiver);
		}


	}

	public void UgasiLoading()
	{
		GameObject.Find("Loading Buffer HOLDER").transform.GetChild(0).gameObject.SetActive(false);
	}

	public void ShopDeoIzCompareScores()
	{
		ShopManagerFull.ShopObject.OcistiMajmuna();
		
		ShopManagerFull.ShopObject.transform.Find("3 Customize/Customize Tabovi/1Hats").gameObject.SetActive(true);
		ShopManagerFull.ShopObject.transform.Find("3 Customize/Customize Tabovi/2Shirts").gameObject.SetActive(true);
		ShopManagerFull.ShopObject.transform.Find("3 Customize/Customize Tabovi/3BackPack").gameObject.SetActive(true);
		
		svekupovineGlava = FacebookManager.UserSveKupovineHats;
		svekupovineMajica = FacebookManager.UserSveKupovineShirts;
		svekupovineLedja = FacebookManager.UserSveKupovineBackPacks;
		glava = FacebookManager.GlavaItem;
		majica = FacebookManager.TeloItem;
		if(majica >= 0)
			bojaMajice = ShopManagerFull.ShopObject.TShirtColors[majica];
		ledja = FacebookManager.LedjaItem;
		imaUsi = FacebookManager.Usi;
		imaKosu = FacebookManager.Kosa;
		ShopManagerFull.AktivanSesir=glava;
		ShopManagerFull.AktivnaMajica=majica;
		ShopManagerFull.AktivanRanac=ledja;
		string niz=ShopManagerFull.AktivanSesir.ToString()+"#"+ShopManagerFull.AktivnaMajica.ToString()+"#"+ShopManagerFull.AktivanRanac.ToString();
		PlayerPrefs.SetString("AktivniItemi", niz);
		PlayerPrefs.Save();
		ShopManagerFull.ShopObject.SviItemiInvetory();
		ShopManagerFull.ShopObject.PobrisiSveOtkljucanoIzShopa();
		ShopManagerFull.ShopObject.RefresujImenaItema();
		if(Application.loadedLevel == 1)
			ShopManagerFull.ShopObject.ObuciMajmunaNaStartu();

	}

}
