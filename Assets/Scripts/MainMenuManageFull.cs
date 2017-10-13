using UnityEngine;
using System.Collections;

public class MainMenuManageFull : MonoBehaviour {

	FacebookManager Face;
	Sprite dugmeMuzikaSprite;
	Sprite dugmeSoundSprite;
	Sprite dugmeMuzikaOffSprite;
	Sprite dugmeSoundOffSprite;
	GameObject dugmeMuzika;
	GameObject dugmeSound;
	GameObject dugmePlay;
	GameObject holderLogo;
	GameObject majmunLogo;
	GameObject LeaderBoard;
	GameObject Languages;
	GameObject TrenutnaZastava;
	bool muzikaOff = false;
	bool soundOff = false;
	NivoManager nivoManager;
	AudioSource MusicOn_Button;
	AudioSource SoundOn_Button;
	AudioSource Play_Button;
	bool LeaderBoardAktivan=false;
	int BrojZastave;
	static public bool LanguagesAktivan;
	float x,y,z;
	GameObject Zastave;
	GameObject TextJezik;
	string[] JezikTekst = {"RU","GR","CH","ENG","ESP","SRB","SVD","IT","FR","POR","AR","CRO","KO","JPN"};
	void Awake()
	{
		GameObject.Find("HolderGornjiDesniUgaoDugmici").GetComponent<Transform>().position=new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one).x, Camera.main.ViewportToWorldPoint(Vector3.one).y, -0.05f);
		GameObject.Find("HolderGornjiLeviUgaoDugmici").GetComponent<Transform>().position=new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x, Camera.main.ViewportToWorldPoint(Vector3.one).y, -0.05f);
		GameObject.Find("HolderDonjiDesniUgaoDugmici").GetComponent<Transform>().position=new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one).x, Camera.main.ViewportToWorldPoint(Vector3.zero).y, -0.05f);
		GameObject.Find("HolderDonjiLeviUgaoDugmici").GetComponent<Transform>().position=new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x, Camera.main.ViewportToWorldPoint(Vector3.zero).y, -0.05f);
		Face= GameObject.Find("FacebookManager").GetComponent<FacebookManager>();
		//dugmeMuzika = GameObject.Find(       HolderLanguageFlagsMove
//		holderLogo = GameObject.Find("HolderLogoGlavni");
//		majmunLogo = GameObject.Find("HolderMajmun");
		LeaderBoard =GameObject.Find("HolderLeaderboardMove");
		dugmeMuzika = GameObject.Find("ButtonMusic");
		dugmeSound = GameObject.Find("ButtonSound");
		dugmeMuzikaSprite = GameObject.Find("ButtonMusic").GetComponent<SpriteRenderer>().sprite;
		dugmeSoundSprite = GameObject.Find("ButtonSound").GetComponent<SpriteRenderer>().sprite;
		dugmeMuzikaOffSprite = GameObject.Find("ButtonMusicOff").GetComponent<SpriteRenderer>().sprite;
		dugmeSoundOffSprite = GameObject.Find("ButtonSoundOff").GetComponent<SpriteRenderer>().sprite;
		Languages = GameObject.Find("HolderLanguageFlagsMove");
		Languages.GetComponent<Animation>().Play("MainLanguageFlagsPosition");
		Zastave=GameObject.Find("Zastave");
		Zastave.SetActive(false);
		LanguagesAktivan=false;
		TrenutnaZastava = GameObject.Find("TrenutniJezik");
		TextJezik = GameObject.Find("Text2letters");
//		holderLogo.transform.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x,Camera.main.ViewportToWorldPoint(Vector3.one).y,holderLogo.transform.position.z);
//		majmunLogo.transform.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one).x,Camera.main.ViewportToWorldPoint(Vector3.one).y,majmunLogo.transform.position.z);
//		nivoManager = GameObject.Find("NivoManager").GetComponent<NivoManager>();


	}

	void Start()
	{
		GameObject.Find("PrinceGorilla").GetComponent<Animator>().Play("Idle Main Screen");
		if(PlaySounds.musicOn)
		{
			if(!PlaySounds.BackgroundMusic_Menu.isPlaying)
				PlaySounds.Play_BackgroundMusic_Menu();
			dugmeMuzika.GetComponent<SpriteRenderer>().sprite = dugmeMuzikaSprite;
		}
		else 
		{
			dugmeMuzika.GetComponent<SpriteRenderer>().sprite = dugmeMuzikaOffSprite;
		}
		if(PlaySounds.soundOn)
		{
			dugmeSound.GetComponent<SpriteRenderer>().sprite = dugmeSoundSprite;
		}
		else 
		{
			dugmeSound.GetComponent<SpriteRenderer>().sprite = dugmeSoundOffSprite;
		}
		BrojZastave=PlayerPrefs.GetInt("LanguageFlag");
		Debug.Log("BrojZastave"+BrojZastave);
		switch(BrojZastave)
		{
		case 1:
			TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag1Referenca").GetComponent<SpriteRenderer>().sprite;
			TextJezik.GetComponent<TextMesh>().text=JezikTekst[0];
			break;
		case 2:
			TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag2Referenca").GetComponent<SpriteRenderer>().sprite;
			TextJezik.GetComponent<TextMesh>().text=JezikTekst[1];
			break;
		case 3:
			TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag3Referenca").GetComponent<SpriteRenderer>().sprite;
			TextJezik.GetComponent<TextMesh>().text=JezikTekst[2];
			break;
		case 4:
			TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag4Referenca").GetComponent<SpriteRenderer>().sprite;
			TextJezik.GetComponent<TextMesh>().text=JezikTekst[3];
			break;
		case 5:
			TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag5Referenca").GetComponent<SpriteRenderer>().sprite;
			TextJezik.GetComponent<TextMesh>().text=JezikTekst[4];
			break;
		case 6:
			TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag6Referenca").GetComponent<SpriteRenderer>().sprite;
			TextJezik.GetComponent<TextMesh>().text=JezikTekst[5];
			break;
		case 7:
			TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag7Referenca").GetComponent<SpriteRenderer>().sprite;
			TextJezik.GetComponent<TextMesh>().text=JezikTekst[6];
			break;
		case 8:
			TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag8Referenca").GetComponent<SpriteRenderer>().sprite;
			TextJezik.GetComponent<TextMesh>().text=JezikTekst[7];
			break;
		case 9:
			TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag9Referenca").GetComponent<SpriteRenderer>().sprite;
			TextJezik.GetComponent<TextMesh>().text=JezikTekst[8];
			break;
		case 10:
			TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag10Referenca").GetComponent<SpriteRenderer>().sprite;
			TextJezik.GetComponent<TextMesh>().text=JezikTekst[9];
			break;
		case 11:
			TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag11Referenca").GetComponent<SpriteRenderer>().sprite;
			TextJezik.GetComponent<TextMesh>().text=JezikTekst[10];
			break;
		case 12:
			TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag12Referenca").GetComponent<SpriteRenderer>().sprite;
			TextJezik.GetComponent<TextMesh>().text=JezikTekst[11];
			break;
		case 13:
			TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag13Referenca").GetComponent<SpriteRenderer>().sprite;
			TextJezik.GetComponent<TextMesh>().text=JezikTekst[12];
			break;
		case 14:
			TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag14Referenca").GetComponent<SpriteRenderer>().sprite;
			TextJezik.GetComponent<TextMesh>().text=JezikTekst[13];
			break;
		}
	}

	void Update () 
	{
		if(Input.GetKeyUp(KeyCode.Escape))
			Application.Quit();
		//Debug.Log("savedMusic: " + PlayerPrefs.GetInt("musicOn") + ", savedSound: " + PlayerPrefs.GetInt("soundOn") + ", musicOn: " + PlaySounds.musicOn + ", soundOn: " + PlaySounds.soundOn);
		if(Input.GetMouseButtonUp(0))
		{
			if(RaycastFunction(Input.mousePosition) == "ButtonMusic")
			{
				Debug.Log("Music Button");
				if(!PlaySounds.musicOn)
				{
					PlaySounds.musicOn = true;
					muzikaOff = false;
					dugmeMuzika.GetComponent<SpriteRenderer>().sprite = dugmeMuzikaSprite;
					if(PlayerPrefs.HasKey("soundOn"))
						if(PlayerPrefs.GetInt("soundOn") == 1)
					PlaySounds.Play_Button_MusicOn();
					PlaySounds.Play_BackgroundMusic_Menu();
					PlayerPrefs.SetInt("musicOn",1);
					PlayerPrefs.Save();
				}
				else
				{
					PlaySounds.musicOn = false;
					muzikaOff = true;
					dugmeMuzika.GetComponent<SpriteRenderer>().sprite = dugmeMuzikaOffSprite;
					PlaySounds.Stop_BackgroundMusic_Menu();
					PlayerPrefs.SetInt("musicOn",0);
					PlayerPrefs.Save();
				}
				Debug.Log("Music Promena :"+PlayerPrefs.GetInt("musicOn"));
				Debug.Log("MusicON: "+PlaySounds.musicOn);
			}
			else if(RaycastFunction(Input.mousePosition) == "ButtonSound")
			{
				Debug.Log("Sound Button");
				if(!PlaySounds.soundOn)
				{
					PlaySounds.soundOn = true;
					soundOff = false;
					dugmeSound.GetComponent<SpriteRenderer>().sprite = dugmeSoundSprite;
					PlaySounds.Play_Button_SoundOn();
					PlayerPrefs.SetInt("soundOn",1);
					PlayerPrefs.Save();
				}
				else
				{
					PlaySounds.soundOn = false;
					soundOff = true;
					dugmeSound.GetComponent<SpriteRenderer>().sprite = dugmeSoundOffSprite;
					PlayerPrefs.SetInt("soundOn",0);
					PlayerPrefs.Save();
				}
				Debug.Log("Sound Promena :"+PlayerPrefs.GetInt("soundOn"));
				Debug.Log("SoundON: "+PlaySounds.soundOn);
			}
			else if(RaycastFunction(Input.mousePosition) == "MainButtonLeaderboard")
			{
				Debug.Log("Leaderboard Button");
				if(PlaySounds.soundOn)
				{
					PlaySounds.Play_Button_SoundOn();
				}

			}
			else if(RaycastFunction(Input.mousePosition) == "MainButtonResetProgress")
			{
				Debug.Log("ResetProgress Button");
				if(PlaySounds.soundOn)
				{
					PlaySounds.Play_Button_SoundOn();
				}

			}
			else if(RaycastFunction(Input.mousePosition) == "MainButtonResetTutorial")
			{
				Debug.Log("ResetTutorial Button");
				if(PlaySounds.soundOn)
				{
					PlaySounds.Play_Button_SoundOn();
				}
			
			}
			else if(RaycastFunction(Input.mousePosition) == "MainLeaderboardArrow")
			{
				Debug.Log("LeaderboardArrow Button");
				if(PlaySounds.soundOn)
				{
					PlaySounds.Play_Button_SoundOn();
				}
				if(LeaderBoardAktivan)
				{
					LeaderBoard.GetComponent<Animation>().Play("MainLeaderboardGo");
				}
				else
				{
					LeaderBoard.GetComponent<Animation>().Play("MainLeaderboardShow");
				}
				LeaderBoardAktivan=!LeaderBoardAktivan;

			}
			else if(RaycastFunction(Input.mousePosition) == "ButtonLanguage")
			{
				Debug.Log("ButtonLanguage Button");
				if(PlaySounds.soundOn)
				{
					PlaySounds.Play_Button_SoundOn();
				}
				if(LanguagesAktivan)
				{
					Zastave.SetActive(false);
					Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].normalizedTime=1;
					Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].speed=-1.5f;
					Languages.GetComponent<Animation>().Play("MainLanguageFlagsShow");

				}
				else
				{
					Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].normalizedTime=0;
					Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].speed=1;
					Languages.GetComponent<Animation>().Play("MainLanguageFlagsShow");
					StartCoroutine("PrikaziZastave");
				}
				LanguagesAktivan=!LanguagesAktivan;

			}
			else if(RaycastFunction(Input.mousePosition) == "ButtonFreeCoins")
			{
				Debug.Log("ButtonFreeCoins Button");
				if(PlaySounds.soundOn)
				{
					PlaySounds.Play_Button_SoundOn();
				}
				StartCoroutine(ShopManager.OpenFreeCoinsCard());

			}
			else if(RaycastFunction(Input.mousePosition) == "ButtonShop")
			{
				Debug.Log("ButtonShop Button");
				if(PlaySounds.soundOn)
				{
					PlaySounds.Play_Button_SoundOn();
				}
				StartCoroutine(ShopManager.OpenShopCard());
				
			}
			else if(RaycastFunction(Input.mousePosition) == "ButtonNews")
			{
				Debug.Log("ButtonNews Button");
				if(PlaySounds.soundOn)
				{
					PlaySounds.Play_Button_SoundOn();
				}
				
			}
			else if(RaycastFunction(Input.mousePosition) == "FaceButton")
			{

				if(!FacebookManager.Ulogovan)
				{
					Debug.Log("LogOut Button");
					if(PlaySounds.soundOn)
					{
						PlaySounds.Play_Button_SoundOn();
					}
					Face.FacebookLogin();
				}


				
			}


			else if(RaycastFunction(Input.mousePosition) == "MainPlayButton")
			{
				Debug.Log("Play Button");
				if(PlayerPrefs.HasKey("soundOn"))
					if(PlayerPrefs.GetInt("soundOn") == 1)
				PlaySounds.Play_Button_Play();
				StartCoroutine(otvoriSledeciNivo());
			}

			else if(RaycastFunction(Input.mousePosition) == "MainLanguageSlideLevo")
			{
				Debug.Log("SlideLevo Button");
				if(Zastave.transform.position.x<=(GameObject.Find("MainLanguageSlideLevo").transform.position.x-9f))
				{}
				else
				{
				Zastave.transform.Translate(Vector3.left*0.5f, Space.World);
				}

			}
			else if(RaycastFunction(Input.mousePosition) == "MainLanguageSlideDesno")
			{
				Debug.Log("SlideDesno Button");
				if(Zastave.transform.position.x>=(GameObject.Find("MainLanguageSlideDesno").transform.position.x-3.5f))
				{}
				else
				{
				Zastave.transform.Translate(Vector3.right*0.5f, Space.World);
				
				}
			}

			else if(RaycastFunction(Input.mousePosition) == "Flag1")
			{
				Debug.Log("Flag1");
				TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag1").GetComponent<SpriteRenderer>().sprite;
				TextJezik.GetComponent<TextMesh>().text=JezikTekst[0];
				Zastave.SetActive(false);
				LanguagesAktivan=false;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].normalizedTime=1;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].speed=-1.5f;
				Languages.GetComponent<Animation>().Play("MainLanguageFlagsShow");
				PlayerPrefs.SetInt("LanguageFlag",1);
				PlayerPrefs.Save();
				
			}
			else if(RaycastFunction(Input.mousePosition) == "Flag2")
			{
				Debug.Log("Flag2");
				TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag2").GetComponent<SpriteRenderer>().sprite;
				TextJezik.GetComponent<TextMesh>().text=JezikTekst[1];
				Zastave.SetActive(false);
				LanguagesAktivan=false;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].normalizedTime=1;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].speed=-1.5f;
				Languages.GetComponent<Animation>().Play("MainLanguageFlagsShow");
				PlayerPrefs.SetInt("LanguageFlag",2);
				PlayerPrefs.Save();
			}
			else if(RaycastFunction(Input.mousePosition) == "Flag3")
			{
				Debug.Log("Flag3");
				TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag3").GetComponent<SpriteRenderer>().sprite;
				TextJezik.GetComponent<TextMesh>().text=JezikTekst[2];
				Zastave.SetActive(false);
				LanguagesAktivan=false;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].normalizedTime=1;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].speed=-1.5f;
				Languages.GetComponent<Animation>().Play("MainLanguageFlagsShow");
				PlayerPrefs.SetInt("LanguageFlag",3);
				PlayerPrefs.Save();
			}
			else if(RaycastFunction(Input.mousePosition) == "Flag4")
			{
				Debug.Log("Flag4");
				TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag4").GetComponent<SpriteRenderer>().sprite;
				TextJezik.GetComponent<TextMesh>().text=JezikTekst[3];
				Zastave.SetActive(false);
				LanguagesAktivan=false;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].normalizedTime=1;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].speed=-1.5f;
				Languages.GetComponent<Animation>().Play("MainLanguageFlagsShow");
				PlayerPrefs.SetInt("LanguageFlag",4);
				PlayerPrefs.Save();
			}
			else if(RaycastFunction(Input.mousePosition) == "Flag5")
			{
				Debug.Log("Flag5");
				TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag5").GetComponent<SpriteRenderer>().sprite;
				TextJezik.GetComponent<TextMesh>().text=JezikTekst[4];
				Zastave.SetActive(false);
				LanguagesAktivan=false;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].normalizedTime=1;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].speed=-1.5f;
				Languages.GetComponent<Animation>().Play("MainLanguageFlagsShow");
				PlayerPrefs.SetInt("LanguageFlag",5);
				PlayerPrefs.Save();
			}
			else if(RaycastFunction(Input.mousePosition) == "Flag6")
			{
				Debug.Log("Flag6");
				TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag6").GetComponent<SpriteRenderer>().sprite;
				TextJezik.GetComponent<TextMesh>().text=JezikTekst[5];
				Zastave.SetActive(false);
				LanguagesAktivan=false;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].normalizedTime=1;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].speed=-1.5f;
				Languages.GetComponent<Animation>().Play("MainLanguageFlagsShow");
				PlayerPrefs.SetInt("LanguageFlag",6);
				PlayerPrefs.Save();
			}
			else if(RaycastFunction(Input.mousePosition) == "Flag7")
			{
				Debug.Log("Flag7");
				TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag7").GetComponent<SpriteRenderer>().sprite;
				TextJezik.GetComponent<TextMesh>().text=JezikTekst[6];
				Zastave.SetActive(false);
				LanguagesAktivan=false;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].normalizedTime=1;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].speed=-1.5f;
				Languages.GetComponent<Animation>().Play("MainLanguageFlagsShow");
				PlayerPrefs.SetInt("LanguageFlag",7);
				PlayerPrefs.Save();
			}
			else if(RaycastFunction(Input.mousePosition) == "Flag8")
			{
				Debug.Log("Flag8");
				TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag8").GetComponent<SpriteRenderer>().sprite;
				TextJezik.GetComponent<TextMesh>().text=JezikTekst[7];
				Zastave.SetActive(false);
				LanguagesAktivan=false;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].normalizedTime=1;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].speed=-1.5f;
				Languages.GetComponent<Animation>().Play("MainLanguageFlagsShow");
				PlayerPrefs.SetInt("LanguageFlag",8);
				PlayerPrefs.Save();
			}
			else if(RaycastFunction(Input.mousePosition) == "Flag9")
			{
				Debug.Log("Flag9");
				TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag9").GetComponent<SpriteRenderer>().sprite;
				TextJezik.GetComponent<TextMesh>().text=JezikTekst[8];
				Zastave.SetActive(false);
				LanguagesAktivan=false;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].normalizedTime=1;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].speed=-1.5f;
				Languages.GetComponent<Animation>().Play("MainLanguageFlagsShow");
				PlayerPrefs.SetInt("LanguageFlag",9);
				PlayerPrefs.Save();
			}
			else if(RaycastFunction(Input.mousePosition) == "Flag10")
			{
				Debug.Log("Flag10");
				TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag10").GetComponent<SpriteRenderer>().sprite;
				TextJezik.GetComponent<TextMesh>().text=JezikTekst[9];
				Zastave.SetActive(false);
				LanguagesAktivan=false;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].normalizedTime=1;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].speed=-1.5f;
				Languages.GetComponent<Animation>().Play("MainLanguageFlagsShow");
				PlayerPrefs.SetInt("LanguageFlag",10);
				PlayerPrefs.Save();
			}
			else if(RaycastFunction(Input.mousePosition) == "Flag11")
			{
				Debug.Log("Flag11");
				TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag11").GetComponent<SpriteRenderer>().sprite;
				TextJezik.GetComponent<TextMesh>().text=JezikTekst[10];
				Zastave.SetActive(false);
				LanguagesAktivan=false;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].normalizedTime=1;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].speed=-1.5f;
				Languages.GetComponent<Animation>().Play("MainLanguageFlagsShow");
				PlayerPrefs.SetInt("LanguageFlag",11);
				PlayerPrefs.Save();
			}
			else if(RaycastFunction(Input.mousePosition) == "Flag12")
			{
				Debug.Log("Flag12");
				TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag12").GetComponent<SpriteRenderer>().sprite;
				TextJezik.GetComponent<TextMesh>().text=JezikTekst[11];
				Zastave.SetActive(false);
				LanguagesAktivan=false;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].normalizedTime=1;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].speed=-1.5f;
				Languages.GetComponent<Animation>().Play("MainLanguageFlagsShow");
				PlayerPrefs.SetInt("LanguageFlag",12);
				PlayerPrefs.Save();
			}
			else if(RaycastFunction(Input.mousePosition) == "Flag13")
			{
				Debug.Log("Flag13");
				TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag13").GetComponent<SpriteRenderer>().sprite;
				TextJezik.GetComponent<TextMesh>().text=JezikTekst[12];
				Zastave.SetActive(false);
				LanguagesAktivan=false;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].normalizedTime=1;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].speed=-1.5f;
				Languages.GetComponent<Animation>().Play("MainLanguageFlagsShow");
				PlayerPrefs.SetInt("LanguageFlag",13);
				PlayerPrefs.Save();
			}
			else if(RaycastFunction(Input.mousePosition) == "Flag14")
			{
				Debug.Log("Flag14");
				TrenutnaZastava.GetComponent<SpriteRenderer>().sprite=GameObject.Find ("Flag14").GetComponent<SpriteRenderer>().sprite;
				TextJezik.GetComponent<TextMesh>().text=JezikTekst[13];
				Zastave.SetActive(false);
				LanguagesAktivan=false;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].normalizedTime=1;
				Languages.GetComponent<Animation>()["MainLanguageFlagsShow"].speed=-1.5f;
				Languages.GetComponent<Animation>().Play("MainLanguageFlagsShow");
				PlayerPrefs.SetInt("LanguageFlag",14);
				PlayerPrefs.Save();
			}


		}
	}

	IEnumerator otvoriSledeciNivo()
	{
//		yield return new WaitForSeconds(2f);
		yield return null;
		if(StagesParser.odgledaoTutorial == 0)
		{
			StagesParser.loadingTip = 1;
			Application.LoadLevel("LoadingScene");
		}
		else Application.LoadLevel("All Maps");
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

	IEnumerator PrikaziZastave()
	{
		yield return new WaitForSeconds(0.5f);
		Zastave.SetActive(true);
	}
		

}
