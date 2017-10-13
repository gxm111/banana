using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour {


	//NivoManager nivoManager;

//	void Awake()
//	{
//		nivoManager = GameObject.Find("NivoManager").GetComponent<NivoManager>();
//	}
	GameObject Tips;
	GameObject Vrata;
//	Texture Background;
//	GameObject Pozadina;
	TextMesh PozadinaText;
	int RandomBroj;
	string LoadingText;

	static Loading instance;

	public static Loading Instance
	{
		get
		{
			if(instance == null)
				instance = GameObject.FindObjectOfType(typeof(Loading)) as Loading;

			return instance;
		}
	}

	void Awake ()
	{
		instance = this;
	}

	void Start () 
	{
		gameObject.name = "LOADING ZA BRISANJE";
		DontDestroyOnLoad(this.gameObject);
		if(StagesParser.LoadingPoruke.Count==0)
		{
			StagesParser.LoadingPoruke.Clear();
			StagesParser.RedniBrojSlike.Clear();
			StagesParser.Instance.UcitajLoadingPoruke();
		}

		transform.Find("Loading Animation Tip-s/Loading sa Tip-om/Loading Text").GetComponent<TextMesh>().text = LanguageManager.Loading;
		transform.Find("Loading Animation Tip-s/Loading sa Tip-om/Loading Text").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);

		RandomBroj=Random.Range(1,StagesParser.LoadingPoruke.Count);

		PozadinaText=GameObject.Find("Tip Text").GetComponent<TextMesh>();

		//1-Do not miss tips and tricks, 2-Want to keep playing? Spend banana... , 3-Jump on enemies' heads...
		if(StagesParser.loadingTip == 1)
		{
			PozadinaText.text = LanguageManager.LoadingTip1;
			PozadinaText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true,false);
			GameObject Background = Instantiate (Resources.Load("LoadingBackground/BgP8")) as GameObject;
			Background.transform.parent = GameObject.Find ("Loading Tip BG").transform;
			RandomBroj = 8;
		}
		else if(StagesParser.loadingTip == 2)
		{
			PozadinaText.text = LanguageManager.LoadingTip2;
			PozadinaText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true,false);
			GameObject Background = Instantiate (Resources.Load("LoadingBackground/BgP5")) as GameObject;
			Background.transform.parent = GameObject.Find ("Loading Tip BG").transform;
			RandomBroj = 5;
		}
		else if(StagesParser.loadingTip == 3)
		{
			PozadinaText.text = LanguageManager.LoadingTip3;
			PozadinaText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true,false);
			GameObject Background = Instantiate (Resources.Load("LoadingBackground/BgP3")) as GameObject;
			Background.transform.parent = GameObject.Find ("Loading Tip BG").transform;
			RandomBroj = 3;
		}

		else
		{
			PozadinaText.text=StagesParser.LoadingPoruke[RandomBroj-1];
			PozadinaText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true,false);
			StagesParser.LoadingPoruke.RemoveAt (RandomBroj - 1);

			GameObject Background = Instantiate (Resources.Load("LoadingBackground/BgP"+StagesParser.RedniBrojSlike[RandomBroj-1].ToString())) as GameObject;
			StagesParser.RedniBrojSlike.RemoveAt (RandomBroj - 1);
			Background.transform.parent = GameObject.Find ("Loading Tip BG").transform;
		}

		//TextMeshExtensions.AdjustFontSize(PozadinaText,true,true);
		if(PlaySounds.BackgroundMusic_Menu.isPlaying)
			PlaySounds.Stop_BackgroundMusic_Menu();
		if(PlaySounds.BackgroundMusic_Gameplay.isPlaying)
			PlaySounds.Stop_BackgroundMusic_Gameplay();
		StartCoroutine(LoadNextLevel());
	}

	IEnumerator LoadNextLevel()
	{
		System.GC.Collect();
		Resources.UnloadUnusedAssets();
		yield return new WaitForSeconds(3);
		//Application.LoadLevelAsync((StagesParser.currSetIndex*10)+StagesParser.currStageIndex+5);
		if(StagesParser.odgledaoTutorial == 0)
		{
			if(StagesParser.nivoZaUcitavanje == 1)
			{
				Application.LoadLevel(StagesParser.nivoZaUcitavanje);
				StagesParser.nivoZaUcitavanje = 0;
			}
			else
				Application.LoadLevel("_Tutorial Level");
		}
		else if(StagesParser.bonusLevel)
			Application.LoadLevelAsync("_Bonus Level Clouds");
		//else if(StagesParser.currStageIndex == 19)
		//	Application.LoadLevelAsync("BOSS_LEVEL");
		else
			Application.LoadLevelAsync(StagesParser.nivoZaUcitavanje); //BICE 9+StagesParser.currSetIndex KAD SE DODAJU NIVOI ZA OSTALA OSTRVA

	}



	public IEnumerator UcitanaScena(Camera camera, int skaliraj, float delay) // 1 - ucitan level, 2 - ucitana mapa, 3 - ucitana ostrva, 4 - ucitan main screen, 5 - Ucitana poslednja scena
	{
		StagesParser.loadingTip = -1;
		float time = 0.45f;
		if(skaliraj == 2)
		{
			transform.localScale = new Vector3(0.334f,0.334f,0.334f);
			transform.position = new Vector3(9,-44.061f,-25.05859f);
			time = 0.65f;
		}
		else if(skaliraj == 3)
		{
			transform.localScale = new Vector3(0.334f,0.334f,0.334f);
			transform.position = new Vector3(82.20029f,-40.65633f,-25.05859f);
			time = 0.65f;
		}
		else if(skaliraj == 5)
		{
			//transform.localScale = new Vector3(0.334f,0.334f,0.334f);
			transform.position = new Vector3(0f,0f,-5f);
			transform.Find("Loading Animation Vrata").GetComponent<Animator>().speed = 2;
			time = 0;
		}
		else
		{
			transform.position = new Vector3(camera.transform.position.x,camera.transform.position.y,camera.transform.position.z+5);
		}
		yield return new WaitForSeconds(delay);
		transform.Find("Loading Animation Tip-s").GetComponent<Animator>().Play("Loading Tip Odlazak");
		StartCoroutine(unistiObjekat(time,skaliraj));
	}

	IEnumerator unistiObjekat(float time, int kojaScena)
	{
		yield return new WaitForSeconds(time);
		transform.Find("Loading Animation Vrata").GetComponent<Animator>().Play("Loading Zidovi Odlazak");
		yield return new WaitForSeconds(1f);
		Destroy(this.gameObject);
		if(kojaScena == 1)
		{
			GameObject.Find("GO screen").GetComponent<Collider>().enabled = true;
		}
	}

}
