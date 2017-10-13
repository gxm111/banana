using UnityEngine;
using System.Collections;

public class CheckInternetConnection : MonoBehaviour {

	static CheckInternetConnection instance;
	TextMesh noInternet;
	TextMesh checkConnection;
	TextMesh checkOK;
	Animator loading;
	Transform loadingHolder;
	Transform pomCollider;
	bool otvorenPopup = false;
	string url = "https://www.google.com";
	[HideInInspector] public bool internetOK = true;
	[HideInInspector] public bool checkDone = false;

	public static CheckInternetConnection Instance
	{
		get
		{
			if(instance == null)
				instance = GameObject.FindObjectOfType(typeof(CheckInternetConnection)) as CheckInternetConnection;

			return instance;
		}
	}

	void Awake ()
	{
		instance = this;
		noInternet = transform.Find("AnimationHolderGlavni/AnimationHolder/NoInternetPopup/Text/NoInternet").GetComponent<TextMesh>();
		checkConnection = transform.Find("AnimationHolderGlavni/AnimationHolder/NoInternetPopup/Text/CheckConnection").GetComponent<TextMesh>();
		checkOK = transform.Find("AnimationHolderGlavni/AnimationHolder/NoInternetPopup/Button_CheckOK/Text").GetComponent<TextMesh>();
		loading = transform.Find("AnimationHolderGlavni/Loading Buffer HOLDER/Loading").GetComponent<Animator>();
		pomCollider = transform.Find("AnimationHolderGlavni/PomocniColliderKodProveravanjaKonekcije");
		loadingHolder = transform.Find("AnimationHolderGlavni/Loading Buffer HOLDER");
	}
	void Start () 
	{
		//StartCoroutine(checkInternetConnectionAndOpenPopup());
		refreshText();

		//"DA SE UBACI DA SE BANANA NA TUTORIJALU POJAVLJUJE SAMO PRVI PUT (MOZE DA SE ISKORISTI FLAG VecPokrenuto IZ PREFS-A!!!!!"
		//"DA SE ISPRAVI SCROLL U SHOP-U!!!!!"
		//"DA SE PROVERE STANJA KADA SE NE PRIHVATI NEKA PERMISIJA!!!!!"
		//"DA SE UBACI DA SE SVUDA POZIVA CHECK CONNECTION POPUP ZA SVE STO ZAHTEVA NET, A DA MU NIJE UKLJUCEN!!!!!"
		//"DA SE PREPRAVI DA SE BOOMERANG BABUNI NE POJAVLJUJU NA PADINAMA!!!!!"
		//"DA SE RAZMISLI KAKO NAJLAKSE DA SE STAVI DA SE NA PREFABU STVORI ENTITET ZA MISIJU I GORE I DOLE AKO JE TAKAV PREFAB!!!!!"
	}

	public IEnumerator checkInternetConnectionAndOpenPopup()
	{
		//yield return new WaitForSeconds(10);
		WWW www = new WWW(url);
		yield return www;
		if(!string.IsNullOrEmpty(www.error))
		{
			Debug.Log("internet error: " + www.error);
			internetOK = false;
			checkDone = true;
			loadingHolder.gameObject.SetActive(false);
			if(!otvorenPopup)
			{
				transform.GetChild(0).GetComponent<Animator>().Play("OpenPopup");
				otvorenPopup = true;
			}
			yield return new WaitForSeconds(0.25f);
			pomCollider.gameObject.SetActive(false);
		}
		else
		{
			Debug.Log("Sve je OK");
			internetOK = true;
			checkDone = true;
			loadingHolder.gameObject.SetActive(false);
			//transform.GetChild(0).GetComponent<Animator>().Play("OpenPopup");
			if(otvorenPopup)
			{
				transform.GetChild(0).GetComponent<Animator>().Play("ClosePopup");
				otvorenPopup = false;
			}
			yield return new WaitForSeconds(0.25f);
			pomCollider.gameObject.SetActive(false);
		}
	}

	public IEnumerator checkInternetConnection()
	{
		WWW www = new WWW(url);
		yield return www;
		if(!string.IsNullOrEmpty(www.error))
		{
			internetOK = false;
			checkDone = true;
		}
		else
		{
			internetOK = true;
			checkDone = true;
		}
	}

	public void openPopup()
	{
		if(!otvorenPopup)
		{
			refreshText();
			transform.GetChild(0).GetComponent<Animator>().Play("OpenPopup");
			otvorenPopup = true;
		}
	}

	public void NoVideosAvailable_OpenPopup()
	{
		if(!otvorenPopup)
		{
			checkConnection.text = LanguageManager.NoVideo;
			noInternet.text = System.String.Empty;
			checkConnection.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			noInternet.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			transform.GetChild(0).GetComponent<Animator>().Play("OpenPopup");
			otvorenPopup = true;
		}
	}

	void OnApplicationPause(bool pauseStatus)
	{
		if(!pauseStatus) //odpauzirana
		{
			//StopCoroutine(checkInternetConnection());
			//StartCoroutine(checkInternetConnection());
		}
	}

	public void refreshText()
	{
//		if(LanguageManager.chosenLanguage != "_en" && LanguageManager.chosenLanguage != "_us")
//		{
//			Font ArialFont = (Font)Resources.GetBuiltinResource (typeof(Font), "Arial.ttf");
//			noInternet.font = ArialFont;
//			noInternet.renderer.material = ArialFont.material;
//
//		}
		noInternet.text = LanguageManager.NoInternet;
		checkConnection.text = LanguageManager.CheckInternet;
		checkOK.text = LanguageManager.Ok;
		noInternet.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		//if(LanguageManager.chosenLanguage == "_fr")
		//	checkConnection.GetComponent<TextMeshEffects>().RefreshTextOutline(true,false);
		//else
		checkConnection.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		checkOK.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
	}

	public void closePopupAndCheck()
	{
		//loadingHolder.gameObject.SetActive(true);
		//pomCollider.gameObject.SetActive(true);
		//loading.Play("LoadingBufferAnimation");
		//transform.GetChild(0).GetComponent<Animator>().Play("ClosePopup");
		StartCoroutine(checkInternetConnection());
	}

	public IEnumerator ClosePopup()
	{
		loadingHolder.gameObject.SetActive(false);
		if(otvorenPopup)
		{
			transform.GetChild(0).GetComponent<Animator>().Play("ClosePopup");
			otvorenPopup = false;
		}
		yield return new WaitForSeconds(0.25f);
		pomCollider.gameObject.SetActive(false);
	}
}
