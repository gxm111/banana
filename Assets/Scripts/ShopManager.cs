using UnityEngine;
using System.Collections;

public class ShopManager : MonoBehaviour {

	public static Transform shopHolder;
	public static Transform shopLevaIvica;
	public static Transform shopDesnaIvica;
	public static GameObject shopHeaderOn;
	public static GameObject shopHeaderOff;
	public static GameObject freeCoinsHeaderOn;
	public static GameObject freeCoinsHeaderOff;
	public static GameObject holderShopCard;
	public static GameObject holderFreeCoinsCard;
	public static Transform buttonShopBack;

	string clickedItem;
	string releasedItem;
	float vremeKlika;
	float startX;
	float endX;
	float pomerajX;
	static float levaGranica;
	static float desnaGranica;
	bool moved;
	bool released;
	bool bounce;
	bool started;
	Transform tempObject;
	GameObject temp;
	float clickedPos;
	public static bool shopExists = true;
	public static bool freeCoinsExists = true;
	static Vector3 originalScale;
	static float offset;
	static System.DateTime timeToShowNextElement;
	bool helpBool;
	static bool videoNotAvailable = false;
	public static bool otvorenShop = false;

	void Awake ()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	void Start () 
	{
		shopHolder = GameObject.Find("_HolderShop").transform;
		shopLevaIvica = GameObject.Find("ShopRamLevoHolder").transform;
		shopDesnaIvica = GameObject.Find("ShopRamDesnoHolder").transform;
		shopHeaderOn = GameObject.Find("ShopHeaderOn");
		shopHeaderOff = GameObject.Find("ShopHeaderOff1");
		freeCoinsHeaderOn = GameObject.Find("ShopHeaderOn1");
		freeCoinsHeaderOff = GameObject.Find("ShopHeaderOff");
		holderShopCard = GameObject.Find("HolderShopCard");
		holderFreeCoinsCard = GameObject.Find("HolderFreeCoinsCard");
		buttonShopBack = GameObject.Find("HolderBack").transform;

		originalScale = shopHolder.localScale;
		offset = 3.5f;

		shopHolder.localScale = shopHolder.localScale * Camera.main.orthographicSize/5f;
		shopHolder.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one/2).x, shopHolder.position.y, Camera.main.transform.position.z + 5);
		shopLevaIvica.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x, shopLevaIvica.position.y, shopLevaIvica.position.z);
		shopDesnaIvica.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one).x, shopLevaIvica.position.y, shopLevaIvica.position.z);
		shopHolder.gameObject.SetActive(false);
		desnaGranica = shopLevaIvica.transform.position.x + 3.5f;

		transform.Find("HolderFrame/ShopRamDesnoHolder/ShopRamDesno/FinishCoins/TextFreeCoinsUp1").GetComponent<Renderer>().sortingLayerID = 1;
		transform.Find("HolderHeader/ShopHeaderOff/TextFreeCoinsDown").GetComponent<Renderer>().sortingLayerID = 1;
		transform.Find("HolderHeader/ShopHeaderOff/TextFreeCoinsUp").GetComponent<Renderer>().sortingLayerID = 1;
		transform.Find("HolderHeader/ShopHeaderOff1/TextShopDown").GetComponent<Renderer>().sortingLayerID = 1;
		transform.Find("HolderHeader/ShopHeaderOff1/TextShopUp").GetComponent<Renderer>().sortingLayerID = 1;
		transform.Find("HolderHeader/ShopHeaderOn/TextShopDown").GetComponent<Renderer>().sortingLayerID = 1;
		transform.Find("HolderHeader/ShopHeaderOn/TextShopUp").GetComponent<Renderer>().sortingLayerID = 1;
		transform.Find("HolderHeader/ShopHeaderOn1/TextFreeCoinsDown").GetComponent<Renderer>().sortingLayerID = 1;
		transform.Find("HolderHeader/ShopHeaderOn1/TextFreeCoinsUp").GetComponent<Renderer>().sortingLayerID = 1;
		transform.Find("HolderFrame/ShopBackground").GetComponent<Renderer>().sortingLayerID = 1;
		transform.Find("HolderFrame/ShopBackground_Dif").GetComponent<Renderer>().sortingLayerID = 1;

		transform.Find("HolderFreeCoinsCard/HolderFreeCoinsCardAnimation/Card3_FC_WatchVideo/HolderCard_NotAvailable/ShopTextOnCard").GetComponent<Renderer>().sortingLayerID = 1;
		//transform.Find("HolderFreeCoinsCard/HolderFreeCoinsCardAnimation/Card3_FC_WatchVideo/HolderCard_NotAvailable/ShopTextOnCard").renderer.sortingLayerID = 1;

		Transform scrollholder = transform.Find("HolderFreeCoinsCard").GetChild(0);

		foreach (Transform child in scrollholder.transform)
		{
			child.Find("HolderCard/ShopPriceButton/ShopTextCoins1").GetComponent<Renderer>().sortingLayerID = 1;
			child.Find("HolderCard/ShopPriceButton/ShopTextCoins2").GetComponent<Renderer>().sortingLayerID = 1;
			child.Find("HolderCard/ShopTextOnCard").GetComponent<Renderer>().sortingLayerID = 1;
			child.Find("HolderCard/ShopCardShine").GetComponent<Renderer>().sortingLayerID = 1;
		}
		scrollholder = transform.Find("HolderShopCard").GetChild(0);
		
		foreach (Transform child in scrollholder.transform)
		{
			child.Find("HolderCard/ShopPriceButton/ShopTextCoins1").GetComponent<Renderer>().sortingLayerID = 1;
			child.Find("HolderCard/ShopPriceButton/ShopTextCoins2").GetComponent<Renderer>().sortingLayerID = 1;
			child.Find("HolderCard/ShopBuyCoins/ShopTextCoins1").GetComponent<Renderer>().sortingLayerID = 1;
			child.Find("HolderCard/ShopBuyCoins/ShopTextCoins2").GetComponent<Renderer>().sortingLayerID = 1;
			child.Find("HolderCard/ShopCardShine").GetComponent<Renderer>().sortingLayerID = 1;
		}


#if UNITY_ANDROID
		shopExists = false;
		shopHeaderOn.SetActive(false);
		shopHeaderOff.SetActive(false);
		freeCoinsHeaderOff.SetActive(false);
		freeCoinsHeaderOn.transform.localPosition = new Vector3(0,freeCoinsHeaderOn.transform.localPosition.y,freeCoinsHeaderOn.transform.localPosition.z);
#elif UNITY_WP8
		freeCoinsExists = false;
		freeCoinsHeaderOn.SetActive(false);
		freeCoinsHeaderOff.SetActive(false);
		shopHeaderOff.SetActive(false);
		shopHeaderOn.transform.localPosition = new Vector3(0,shopHeaderOn.transform.localPosition.y,shopHeaderOn.transform.localPosition.z);
#endif
	}
	
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			if(released)
				released = false;
			clickedItem = RaycastFunction(Input.mousePosition);
			vremeKlika = Time.time;
			clickedPos = Input.mousePosition.x;
			if(started)
			{
				started = false;
				tempObject = null;
			}
			if(clickedItem.StartsWith("Card"))
			{
				startX = Input.mousePosition.x;
				started = true;
				tempObject = GameObject.Find(clickedItem).transform;
				//levaGranica =  - (tempObject.parent.childCount-1) * tempObject.GetComponent<BoxCollider>().bounds.extents.x*2 - desnaGranica-0.5f;
				levaGranica = shopDesnaIvica.position.x-1.5f - (tempObject.parent.childCount-1) * tempObject.GetComponent<BoxCollider>().bounds.extents.x*2 - tempObject.GetComponent<BoxCollider>().bounds.extents.x;
				//Debug.Log("leva granica: " + levaGranica + ", ext: " + tempObject.GetComponent<BoxCollider>().bounds.extents.x + ", sveukupno: " + ((tempObject.parent.childCount) * tempObject.GetComponent<BoxCollider>().bounds.extents.x + 1) + ", desna granica: " + desnaGranica);
			}
		}

		if(Input.GetMouseButton(0))
		{
			if(started && ((tempObject.parent.childCount > 2 && Camera.main.aspect < 16f/9) || (tempObject.parent.childCount > 3 && Camera.main.aspect >= 16f/9)))
			{
				endX = Input.mousePosition.x;
				pomerajX = (endX - startX)*Camera.main.orthographicSize/250;
				if(pomerajX != 0)
					moved = true;
				//Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x + pomerajX, 87.9f, 95.44f), Camera.main.transform.position.y, Camera.main.transform.position.z);
				tempObject.parent.position = new Vector3(Mathf.Clamp(tempObject.parent.position.x + pomerajX,levaGranica,desnaGranica),tempObject.parent.position.y, tempObject.parent.position.z);
				startX = endX;
				Debug.Log("Uledj");
			}
		}

		if(released)
		{
			if(tempObject.parent.position.x <= levaGranica - 0.5f)
			{
				if(bounce)
				{	
					pomerajX = 0.075f;
					bounce = false;
				}
				//Debug.Log(pomerajX);
			}
			else if(tempObject.parent.position.x >= desnaGranica)
			{
				if(bounce)
				{	
					pomerajX = -0.075f;
					bounce = false;
				}
			}
			
			//if(Camera.main.transform.position.x > 87.9f + 0.6f && Camera.main.transform.position.x < 95.44f - 0.6f)
			{
				tempObject.parent.Translate(pomerajX,0,0);
				pomerajX *= 0.92f;
			}
		}

		else if(Input.GetMouseButtonUp(0))
		{
			releasedItem = RaycastFunction(Input.mousePosition);
			if(moved)
			{
				moved = false;
				released = true;
				bounce = true;
			}
			startX = endX = 0;
			if(clickedItem == releasedItem && releasedItem != System.String.Empty && (Time.time - vremeKlika < 0.35f) && Mathf.Abs(Input.mousePosition.x - clickedPos) < 50)
			{
				if(releasedItem == "HolderBack")
				{
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
					if(Time.timeScale == 0)
					{
						StartCoroutine(PausedAnim(buttonShopBack.GetChild(0),"BackButtonClick"));
						StartCoroutine(CloseShopPaused());
					}
					else
					{
						buttonShopBack.GetChild(0).GetComponent<Animation>().Play("BackButtonClick");
						StartCoroutine(CloseShop());
					}
				}
				else if(releasedItem == "ShopHeaderOff1") // TREBA DA SE AKTIVIRA SHOP TAB
				{
					holderShopCard.transform.position = new Vector3(desnaGranica,holderShopCard.transform.position.y,holderShopCard.transform.position.z);
					shopHeaderOff.SetActive(false);
					shopHeaderOn.SetActive(true);
					freeCoinsHeaderOn.SetActive(false);
					freeCoinsHeaderOff.SetActive(true);
					holderFreeCoinsCard.SetActive(false);
					holderShopCard.SetActive(true);
					if(Time.timeScale == 0)
						StartCoroutine(PausedAnim(holderShopCard.transform.GetChild(0),"DolazakShop_A"));
					else
						holderShopCard.transform.GetChild(0).GetComponent<Animation>().Play("DolazakShop_A");
				}
				else if(releasedItem == "ShopHeaderOff") // TREBA DA SE AKTIVIRA FREE COINS TAB
				{
					holderFreeCoinsCard.transform.position = new Vector3(desnaGranica,holderFreeCoinsCard.transform.position.y,holderFreeCoinsCard.transform.position.z);
					shopHeaderOn.SetActive(false);
					shopHeaderOff.SetActive(true);
					freeCoinsHeaderOff.SetActive(false);
					freeCoinsHeaderOn.SetActive(true);
					holderShopCard.SetActive(false);
					holderFreeCoinsCard.SetActive(true);
					if(Time.timeScale == 0)
						StartCoroutine(PausedAnim(holderFreeCoinsCard.transform.GetChild(0),"DolazakShop_A"));
					else
						holderFreeCoinsCard.transform.GetChild(0).GetComponent<Animation>().Play("DolazakShop_A");
				}
				else if(releasedItem.StartsWith("Card")) // KLIKNUTO NA BILO KOJU KARTICU
				{
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
					if(Time.timeScale == 0)
					{
						temp = GameObject.Find(releasedItem);
						StartCoroutine(PausedAnim(temp.transform.GetChild(0),"ShopCardClick"));
					}
					else
					{
						temp = GameObject.Find(releasedItem);
						temp.transform.GetChild(0).GetComponent<Animation>().Play("ShopCardClick");
					}
					if(releasedItem.Contains("LikeBananaIsland"))
					{
						FacebookManager.stranica = "BananaIsland";
						if(FB.IsLoggedIn)
							GameObject.Find("FacebookManager").SendMessage("OpenPage");
						else
							GameObject.Find("FacebookManager").SendMessage("FacebookLogin");
						//FacebookManager.FacebookLogin();
					}
					else if(releasedItem.Contains("LikeWebelinx"))
					{
						FacebookManager.stranica = "Webelinx";
						if(FB.IsLoggedIn)
							GameObject.Find("FacebookManager").SendMessage("OpenPage");
						else
							GameObject.Find("FacebookManager").SendMessage("FacebookLogin");
						//FacebookManager.FacebookLogin();
					}
					if(releasedItem.Contains("WatchVideo"))
					{

					}
					else if(releasedItem.Contains("Buy"))
					{
						string sta = releasedItem.Substring(releasedItem.IndexOf('y')+1);
						Debug.Log("Sta: " + sta);
					}
				}
			}
		}

		if(Input.GetKeyUp(KeyCode.Escape))
		{
			if(shopHolder.gameObject.activeSelf)
			{
				if(PlaySounds.soundOn)
					PlaySounds.Play_Button_OpenLevel();
				if(Time.timeScale == 0)
				{
					StartCoroutine(PausedAnim(buttonShopBack.GetChild(0),"BackButtonClick"));
					StartCoroutine(CloseShopPaused());
				}
				else
				{
					buttonShopBack.GetChild(0).GetComponent<Animation>().Play("BackButtonClick");
					StartCoroutine(CloseShop());
				}
			}
		}
	}

	public static IEnumerator OpenShopCard()
	{
		shopDesnaIvica.transform.Find("ShopRamDesno/FinishCoins/TextFreeCoinsUp1").GetComponent<TextMesh>().text = PlayerPrefs.GetInt("TotalMoney").ToString();
		shopHolder.transform.position = Camera.main.transform.position + Vector3.forward*5;
		shopHeaderOff.SetActive(false);
		shopHeaderOn.SetActive(true);
		if(freeCoinsExists)
		{
			freeCoinsHeaderOn.SetActive(false);
			freeCoinsHeaderOff.SetActive(true);
		}
		holderFreeCoinsCard.SetActive(false);
		holderShopCard.SetActive(true);
		holderShopCard.transform.position = new Vector3(desnaGranica,holderShopCard.transform.position.y,holderShopCard.transform.position.z);
		
		yield return new WaitForSeconds(0.25f);
		shopHolder.gameObject.SetActive(true);
		otvorenShop = true;
		holderShopCard.transform.GetChild(0).GetComponent<Animation>().Play("DolazakShop_A");

		if(PlayerPrefs.HasKey("otisaoDaLajkuje"))
		{
			//GameObject.Find("LifeManager").SendMessage("GiveReward","Shop#"+PlayerPrefs.GetInt("otisaoDaLajkuje"));
			//PlayerPrefs.DeleteKey("otisaoDaLajkuje");
			FacebookManager.lokacijaProvere = "Shop";
			FacebookManager.stranica = PlayerPrefs.GetString("stranica");
			FacebookManager.IDstranice = PlayerPrefs.GetString("IDstranice");
			GameObject.Find("FacebookManager").SendMessage("CheckLikes");
			Debug.Log("Nagradi ga iz Shop");
		}
	}
	
	public static IEnumerator OpenFreeCoinsCard()
	{

		shopDesnaIvica.transform.Find("ShopRamDesno/FinishCoins/TextFreeCoinsUp1").GetComponent<TextMesh>().text = PlayerPrefs.GetInt("TotalMoney").ToString();
		shopHolder.transform.position = Camera.main.transform.position + Vector3.forward*5;
		if(shopExists)
		{
			shopHeaderOn.SetActive(false);
			shopHeaderOff.SetActive(true);
		}
		freeCoinsHeaderOff.SetActive(false);
		freeCoinsHeaderOn.SetActive(true);
		holderShopCard.SetActive(false);
		holderFreeCoinsCard.SetActive(true);
		holderFreeCoinsCard.transform.position = new Vector3(desnaGranica,holderFreeCoinsCard.transform.position.y,holderFreeCoinsCard.transform.position.z);

		yield return new WaitForSeconds(0.25f);
		shopHolder.gameObject.SetActive(true);
		otvorenShop = true;
		if(videoNotAvailable)
			ResetVideoNotAvailable();
		holderFreeCoinsCard.transform.GetChild(0).GetComponent<Animation>().Play("DolazakShop_A");

		if(PlayerPrefs.HasKey("otisaoDaLajkuje"))
		{
			//GameObject.Find("LifeManager").SendMessage("GiveReward","Shop#"+PlayerPrefs.GetInt("otisaoDaLajkuje"));
			//PlayerPrefs.DeleteKey("otisaoDaLajkuje");
			FacebookManager.lokacijaProvere = "Shop";
			FacebookManager.stranica = PlayerPrefs.GetString("stranica");
			FacebookManager.IDstranice = PlayerPrefs.GetString("IDstranice");
			GameObject.Find("FacebookManager").SendMessage("CheckLikes");
			Debug.Log("Nagradi ga iz Shop");
		}

	}

	public void ShopCardPaused()
	{
		StartCoroutine(OpenShopCardPaused());
	}

	public IEnumerator OpenShopCardPaused()
	{
		yield return null;
		StartCoroutine(PausedAnim(holderShopCard.transform.GetChild(0),"DolazakShop_A"));
	}

	public void FreeCoinsCardPaused()
	{
		StartCoroutine(OpenFreeCoinsCardPaused());
	}

	public static void shopPreparation_Paused()
	{
		shopDesnaIvica.transform.Find("ShopRamDesno/FinishCoins/TextFreeCoinsUp1").GetComponent<TextMesh>().text = PlayerPrefs.GetInt("TotalMoney").ToString();
		shopHolder.transform.position = Camera.main.transform.position + Vector3.forward*5;
		shopHeaderOff.SetActive(false);
		shopHeaderOn.SetActive(true);
		if(freeCoinsExists)
		{
			freeCoinsHeaderOn.SetActive(false);
			freeCoinsHeaderOff.SetActive(true);
		}
		holderFreeCoinsCard.SetActive(false);
		holderShopCard.SetActive(true);
		holderShopCard.transform.position = new Vector3(desnaGranica,holderShopCard.transform.position.y,holderShopCard.transform.position.z);
		
		shopHolder.gameObject.SetActive(true);
		otvorenShop = true;
	}

	public IEnumerator OpenFreeCoinsCardPaused()
	{
		yield return null;
		StartCoroutine(PausedAnim(holderFreeCoinsCard.transform.GetChild(0),"DolazakShop_A"));
	}

	public static void freeCoinsPreparation_Paused()
	{
		shopDesnaIvica.transform.Find("ShopRamDesno/FinishCoins/TextFreeCoinsUp1").GetComponent<TextMesh>().text = PlayerPrefs.GetInt("TotalMoney").ToString();
		shopHolder.transform.position = Camera.main.transform.position + Vector3.forward*5;
		if(shopExists)
		{
			shopHeaderOn.SetActive(false);
			shopHeaderOff.SetActive(true);
		}
		freeCoinsHeaderOff.SetActive(false);
		freeCoinsHeaderOn.SetActive(true);
		holderShopCard.SetActive(false);
		holderFreeCoinsCard.SetActive(true);
		holderFreeCoinsCard.transform.position = new Vector3(desnaGranica,holderFreeCoinsCard.transform.position.y,holderFreeCoinsCard.transform.position.z);
		shopHolder.gameObject.SetActive(true);
		otvorenShop = true;
		if(videoNotAvailable)
			ResetVideoNotAvailable();
	}
	
	IEnumerator CloseShop()
	{
		yield return new WaitForSeconds(0.85f);
		shopHolder.gameObject.SetActive(false);
		otvorenShop = false;
		shopHolder.position = new Vector3(-5,-5,shopHolder.position.z);
		buttonShopBack.GetChild(0).localPosition = Vector3.zero;
	}

	IEnumerator CloseShopPaused()
	{
		timeToShowNextElement = System.DateTime.Now.AddSeconds(0.85f);
		
		while (System.DateTime.Now < timeToShowNextElement)
		{
			yield return null;
		}
		shopHolder.gameObject.SetActive(false);
		otvorenShop = false;
		shopHolder.position = new Vector3(-5,-5,shopHolder.position.z);
		buttonShopBack.GetChild(0).localPosition = Vector3.zero;
	}

	public string RaycastFunction(Vector3 vector)
	{
		Ray ray = Camera.main.ScreenPointToRay(vector);
		RaycastHit hit;
		if(Physics.Raycast(ray,out hit))
		{
			return hit.collider.name;
		}
		return "";
	}
	public static void RescaleShop()
	{
		shopHolder.localScale = originalScale * Camera.main.orthographicSize/5f;
		float locOffset = offset * Camera.main.orthographicSize/5f;
		shopHolder.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one/2).x, shopHolder.position.y, Camera.main.transform.position.z + 5);
		shopLevaIvica.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x, shopLevaIvica.position.y, shopLevaIvica.position.z);
		shopDesnaIvica.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one).x, shopLevaIvica.position.y, shopLevaIvica.position.z);
		shopHolder.gameObject.SetActive(false);
		desnaGranica = shopLevaIvica.transform.position.x + locOffset;
	}
	IEnumerator PausedAnim(Transform obj, string ime)
	{
		StartCoroutine( obj.GetComponent<Animation>().Play(ime, false, what => {helpBool=true;}) );
		//animation.Play(
		while(!helpBool)
		{
			yield return null;
		}
		helpBool=false;
	}
	void VideoNotAvailable()
	{
		GameObject.Find("Card3_FC_WatchVideo").transform.Find("HolderCard").gameObject.SetActive(false);
		GameObject.Find("Card3_FC_WatchVideo").transform.Find("HolderCard_NotAvailable").gameObject.SetActive(true);
		videoNotAvailable = true;
	}
	static void ResetVideoNotAvailable()
	{
		GameObject.Find("Card3_FC_WatchVideo").transform.Find("HolderCard").gameObject.SetActive(true);
		GameObject.Find("Card3_FC_WatchVideo").transform.Find("HolderCard_NotAvailable").gameObject.SetActive(false);
		videoNotAvailable = false;
	}
}
