using UnityEngine;
using System.Collections;

public class MainMenuManage : MonoBehaviour {

	Sprite dugmeMuzikaSprite;
	Sprite dugmeSoundSprite;
	Sprite dugmeMuzikaOffSprite;
	Sprite dugmeSoundOffSprite;
	GameObject dugmeMuzika;
	GameObject dugmeSound;
	GameObject dugmePlay;
	GameObject holderLogo;
	GameObject majmunLogo;
	ParticleSystem bananaRasipuje;
	bool muzikaOff = false;
	bool soundOff = false;
	AudioSource MusicOn_Button;
	AudioSource SoundOn_Button;
	AudioSource Play_Button;
	string clickedItem;
	string releasedItem;
	Vector3 originalScale;
	Transform dock_goreDesno;
	Transform dock_doleDesno;
	Transform dock_goreLevo;
	Transform dock_doleLevo;

	void Awake()
	{
		//dugmeMuzika = GameObject.Find(
		holderLogo = GameObject.Find("HolderLogoGlavni");
		majmunLogo = GameObject.Find("HolderMajmun");
		dugmeMuzika = GameObject.Find("MusicMain");
		dugmeSound = GameObject.Find("SoundMain");
		dugmeMuzikaSprite = dugmeMuzika.GetComponent<SpriteRenderer>().sprite;
		dugmeSoundSprite = dugmeSound.GetComponent<SpriteRenderer>().sprite;
		dugmeMuzikaOffSprite = GameObject.Find("MusicOffMain").GetComponent<SpriteRenderer>().sprite;
		dugmeSoundOffSprite = GameObject.Find("SoundOff").GetComponent<SpriteRenderer>().sprite;
		//bananaRasipuje = GameObject.Find("BananaClickPartikli").GetComponent<ParticleSystem>();
		//holderLogo.transform.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x,Camera.main.ViewportToWorldPoint(Vector3.one).y,holderLogo.transform.position.z);
		//majmunLogo.transform.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one).x,Camera.main.ViewportToWorldPoint(Vector3.one).y,majmunLogo.transform.position.z);
		GameObject.Find("LifeManager").transform.position = new Vector3(50,50,0);
		//GameObject.Find("HolderLife").transform.localPosition = new Vector3(0,0,-5);
		dock_goreDesno = GameObject.Find("Dock_GoreDesno").transform;
		dock_doleDesno = GameObject.Find("Dock_DoleDesno").transform;
		dock_goreLevo = GameObject.Find("Dock_GoreLevo").transform;
		dock_doleLevo = GameObject.Find("Dock_DoleLevo").transform;

		dock_doleDesno.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one).x,Camera.main.ViewportToWorldPoint(Vector3.zero).y,holderLogo.transform.position.z);
		dock_doleLevo.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x,Camera.main.ViewportToWorldPoint(Vector3.zero).y,holderLogo.transform.position.z);
		dock_goreDesno.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one).x,Camera.main.ViewportToWorldPoint(Vector3.one).y,holderLogo.transform.position.z);
		dock_goreLevo.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x,Camera.main.ViewportToWorldPoint(Vector3.one).y,holderLogo.transform.position.z);

		#if UNITY_IPHONE
		dock_goreLevo.GetChild(0).gameObject.SetActive(false);
		#endif
	}

	void Start()
	{
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
		if(!ShopManager.shopExists)
		{
			GameObject.Find("ButtonFreeCoinsDock").transform.position = GameObject.Find("ButtonShop").transform.position;
			GameObject.Find("ButtonShop").SetActive(false);
		}
		if(!ShopManager.freeCoinsExists)
			GameObject.Find("ButtonFreeCoins").SetActive(false);
		ShopManager.RescaleShop();
	}

	void Update () 
	{
		//Debug.Log("savedMusic: " + PlayerPrefs.GetInt("musicOn") + ", savedSound: " + PlayerPrefs.GetInt("soundOn") + ", musicOn: " + PlaySounds.musicOn + ", soundOn: " + PlaySounds.soundOn);
		if(Input.GetKeyUp(KeyCode.Escape) && !ShopManager.otvorenShop)
		{
	
		}

		if(Input.GetMouseButtonDown(0))
		{
			clickedItem = RaycastFunction(Input.mousePosition);

			if(clickedItem.Equals("PlayMain") || clickedItem.Equals("PlayMainFly")|| clickedItem.Equals("ButtonFreeCoins") || clickedItem.Equals("ButtonShop") || clickedItem.Equals("ButtonExit")) 
			{
				GameObject temp = GameObject.Find(clickedItem);
				originalScale = temp.transform.localScale;
				temp.transform.localScale = originalScale * 0.8f;
			}
			else if(clickedItem != System.String.Empty)
			{
				GameObject temp = GameObject.Find(clickedItem);
				originalScale = temp.transform.localScale;
			}
		}

		if(Input.GetMouseButtonUp(0))
		{
			releasedItem = RaycastFunction(Input.mousePosition);
			if(!clickedItem.Equals(System.String.Empty))
			{
				GameObject temp = GameObject.Find(clickedItem);
				temp.transform.localScale = originalScale;
				if(releasedItem == "MusicMain")
				{
					if(!PlaySounds.musicOn)
					{
						PlaySounds.musicOn = true;
						muzikaOff = false;
						//dugmeMuzika.GetComponent<SpriteRenderer>().sprite = dugmeMuzikaSprite;
						dugmeMuzika.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
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
						//dugmeMuzika.GetComponent<SpriteRenderer>().sprite = dugmeMuzikaOffSprite;
						dugmeMuzika.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
						PlaySounds.Stop_BackgroundMusic_Menu();
						PlayerPrefs.SetInt("musicOn",0);
						PlayerPrefs.Save();
					}
				}
				else if(releasedItem == "SoundMain")
				{
					if(!PlaySounds.soundOn)
					{
						PlaySounds.soundOn = true;
						soundOff = false;
						//dugmeSound.GetComponent<SpriteRenderer>().sprite = dugmeSoundSprite;
						dugmeSound.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
						PlaySounds.Play_Button_SoundOn();
						PlayerPrefs.SetInt("soundOn",1);
						PlayerPrefs.Save();
					}
					else
					{
						PlaySounds.soundOn = false;
						soundOff = true;
						//dugmeSound.GetComponent<SpriteRenderer>().sprite = dugmeSoundOffSprite;
						dugmeSound.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
						PlayerPrefs.SetInt("soundOn",0);
						PlayerPrefs.Save();
					}
				}
				else if(releasedItem == "PlayMainFly")
				{
					GameObject.Find(releasedItem).GetComponent<Collider>().enabled = false;
					if(PlayerPrefs.HasKey("soundOn"))
						if(PlayerPrefs.GetInt("soundOn") == 1)
					PlaySounds.Play_Button_Play();
					//bananaRasipuje.Play();
//					if(PlayerPrefs.HasKey("VecPokrenuto") || PlayerPrefs.HasKey("starsandstages"))
//						StartCoroutine(otvoriSledeciNivo());
//					else
//					{
//						StagesParser.currSetIndex = 0;
//						StagesParser.currStageIndex = 0;
//						Application.LoadLevel(2);
//					}
					if(!PlayerPrefs.HasKey("OdgledaoTutorial"))
						Application.LoadLevel("LoadingScene");
					else Application.LoadLevel(3);
				}
				else if(releasedItem == "ButtonFreeCoins")
				{
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
					StartCoroutine(ShopManager.OpenFreeCoinsCard());
				}
				else if(releasedItem == "ButtonShop")
				{
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
					StartCoroutine(ShopManager.OpenShopCard());
				}
				else if(releasedItem == "ButtonExit")
				{
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
					Application.Quit();
				}
			}
		}
	}

	IEnumerator otvoriSledeciNivo()
	{
		yield return new WaitForSeconds(0.25f);
		Application.LoadLevel("Worlds");
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
}
