using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Collections.Generic;

using System.Xml.Linq;

using System.Linq;
#if !UNITY_METRO
using ArabicSupport;
#endif
using System.Text.RegularExpressions;

public class ShopManagerFull : MonoBehaviour {

	public bool EarsAndHairCustomization = false;
	public Transform[] HatsObjects = new Transform[0];
	public Transform[] ShirtsObjects = new Transform[0];
	public Transform[] BackPacksObjects = new Transform[0];
	public Transform[] PowerUpsObjects = new Transform[0];
	public static int BuyButtonState;  // 0-buy moze, 1 - buy ne moze, 2-equip, 3-unequip, 4-zakljucano
	public static bool PreviewState=false;
	GameObject ZidFooter, Custumization;
	bool ImaNovihMajica, ImaNovihKapa, ImaNovihRanceva;
	GameObject ButtonShop, ButtonShopSprite, PreviewShopButton, ShopBanana;
	public static int AktivanSesir, AktivnaMajica, AktivanRanac;
	int PreviewSesir, PreviewMajica, PreviewRanac;
	public static bool otvorenShop = false;
	public static int AktivanTab;
	public static int AktivanCustomizationTab;
	public static int AktivanItemSesir, AktivanItemMajica, AktivanItemRanac;
	int TrenutniSelektovanSesir=999, TrenutnoSelektovanaMajica=999, TrenutnoSelektovanRanac=999;
	string[] Hats;  // ovo dobijamo sa servera, FacebookManager.KupljeneZezalice je string koji prosledjujemo
	string[] Shirts;  // ovo dobijamo sa servera, FacebookManager.KupljeneZezalice je string koji prosledjujemo
	string[] BackPacks;  // ovo dobijamo sa servera, FacebookManager.KupljeneZezalice je string koji prosledjujemo
	string[] AktivniItemi;
	string AktivniItemString;
	GameObject MajmunBobo;
	Vector3 MainScenaPozicija, ShopCustomizationPozicija;
	public static bool ImaUsi, ImaKosu;
	public static ShopManagerFull ShopObject;
//	public SwipeControlCustomization swipeCtrl;
	string releasedItem;
	string clickedItem;
	Vector3 originalScale;
	static Color KakiBoja=new Color(0.97255f, 0.79216f, 0.40784f);   
	static Color PopustBoja=new Color(0.11373f, 0.82353f, 0.38039f);        
	static float gornjaGranica;
	static float donjaGranica;
	TextAsset aset2;
	string aset;
	public static bool ShopInicijalizovan=false;
//	string[] KupljeneStvariParse; // ovo dobijamo sa servera, FacebookManager.KupljeneZezalice je string koji prosledjujemo
//	public static string chosenLanguage="_en";
	int BrojItemaShopHats, BrojItemaShopShirts, BrojItemaShopBackPack, BrojItemaShop;
//	public static List<string> ImenaItema=new List<string>();
//	public static List<int> SveStvariZaOblacenje=new List<int>();

	public static List<int> SveStvariZaOblacenjeHats=new List<int>();
	public static List<int> SveStvariZaOblacenjeShirts=new List<int>();
	public static List<int> SveStvariZaOblacenjeBackPack=new List<int>();

	List<string> ImenaHats;
	List<string> ImenaShirts;
	List<string> ImenaBackPacks;
	List<string> ImenaPowerUps;
	string ImeBanana;


	public List<string> CoinsHats;
	List<string> CoinsShirts;
	List<string> CoinsBackPacks;
	List<string> CoinsPowerUps;
	string cenaBanana;

	List<string> BananaHats=new List<string>();
	List<string> BananaShirts=new List<string>();
	List<string> BananaBackPacks=new List<string>();

	List<string> PopustHats=new List<string>();
	List<string> PopustShirts=new List<string>();
	List<string> PopustBackPacks=new List<string>();
	List<string> PopustPowerUps=new List<string>();
	string PopustBanana;

	List<string> UsiHats=new List<string>();
	List<string> KosaHats=new List<string>(); 

	float ProcenatOtkljucan;
	string StariBrojOtkljucanihItema;
	string[] StariBrojOtkljucanihItemaNiz; 
	public static int BrojOtkljucanihMajici, BrojOtkljucanihRanceva, BrojOtkljucanihKapa, StariBrojOtkljucanihMajici, StariBrojOtkljucanihRanceva, StariBrojOtkljucanihKapa;
	List<int> ZakljucaniHats=new List<int>();
	List<int> ZakljucaniShirts=new List<int>();
	List<int> ZakljucaniBackPacks=new List<int>();

	GameObject CustomizationHats, CustomizationShirts, CustomizationBackPack;
	GameObject CoinsNumber;
	GameObject temp;
	bool mozeDaOtvoriSledeciTab = true;
	bool kliknuoJednomNaTab = true;

	// Use this for initialization
	public Color[] TShirtColors; //19,19,19,0 ; 201,201,201,0 ; 184,163,0 ; 161,0,0 ; 0,78,156 ; 35,107,0 ;

	void Awake()
	{
		ShopManagerFull.ShopObject = this;
		if(EarsAndHairCustomization)
		{
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			UsiHats.Add ("0");
			UsiHats.Add ("1");
			UsiHats.Add ("0");
			UsiHats.Add ("1");
			UsiHats.Add ("0");
			
			KosaHats.Add ("0");
			KosaHats.Add ("0");
			KosaHats.Add ("0");
			KosaHats.Add ("1");
			KosaHats.Add ("0");
			KosaHats.Add ("0");
			KosaHats.Add ("0");
			KosaHats.Add ("0");
			KosaHats.Add ("0");
			KosaHats.Add ("0");
			KosaHats.Add ("0");
			KosaHats.Add ("0");
			KosaHats.Add ("0");
			KosaHats.Add ("0");
			KosaHats.Add ("0");
		}
		else
		{
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			UsiHats.Add ("1");
			
			KosaHats.Add ("1");
			KosaHats.Add ("1");
			KosaHats.Add ("1");
			KosaHats.Add ("1");
			KosaHats.Add ("1");
			KosaHats.Add ("1");
			KosaHats.Add ("1");
			KosaHats.Add ("1");
			KosaHats.Add ("1");
			KosaHats.Add ("1");
			KosaHats.Add ("1");
			KosaHats.Add ("1");
			KosaHats.Add ("1");
			KosaHats.Add ("1");
			KosaHats.Add ("1");
		}

		ButtonShop = GameObject.Find ("ButtonBuy");
		ButtonShopSprite = GameObject.Find ("Buy Button");
		PreviewShopButton = GameObject.Find ("Preview Button");
//		DontDestroyOnLoad(gameObject);
	}
	void Start () {
//		PlayerPrefs.DeleteAll ();
//		//Debug.Log ("Aspect: "+Camera.main.aspect);

		if(Camera.main.aspect<1.51)
		{
			GameObject.Find("ButtonBackShop").transform.localPosition = new Vector3(-1.58f,-0.8f,0f);
		}
		ShopBanana = GameObject.Find("Shop Banana");
		AktivanCustomizationTab=1;
		ZidFooter = GameObject.Find("ZidFooterShop");
		Custumization = GameObject.Find("Custumization");
		Custumization.transform.Find("Znak Uzvika telo").gameObject.SetActive(false);
		CoinsNumber = GameObject.Find("Shop/Shop Interface/Coins");
		CoinsNumber.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
		CoinsNumber.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		GameObject.Find ("Shop/2 Shop - BANANA/Zid Shop/Zid Header i Footer/Zid Footer Shop/Banana Number/Number").GetComponent<TextMesh> ().text = StagesParser.currentBananas.ToString ();
		GameObject.Find ("Shop/2 Shop - BANANA/Zid Shop/Zid Header i Footer/Zid Footer Shop/Banana Number/Number").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		GameObject.Find ("Double Coins Number/Number").GetComponent<TextMesh> ().text = StagesParser.powerup_doublecoins.ToString ();
		GameObject.Find ("Double Coins Number/Number").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		GameObject.Find ("Magnet Number/Number").GetComponent<TextMesh> ().text = StagesParser.powerup_magnets.ToString ();
		GameObject.Find ("Magnet Number/Number").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

		GameObject.Find ("Shield Number/Number").GetComponent<TextMesh> ().text = StagesParser.powerup_shields.ToString ();
		GameObject.Find ("Shield Number/Number").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		ImaNovihMajica = false;
		ImaNovihKapa = false;
		ImaNovihRanceva = false;
		//MainScenaPozicija = new Vector3 (-11.36262f, -10.04164f, 2.56f);
		//ShopCustomizationPozicija = new Vector3 (8.233864f, -5.05306f, -31.75042f);
		if(Application.loadedLevel == 1)
			ShopCustomizationPozicija = new Vector3 (6.586132f, -5.05306f, -31.75042f);
		else
			ShopCustomizationPozicija = new Vector3 (-16.58703f, -98.95457f, -50f);


		if(PlayerPrefs.HasKey("AktivniItemi"))
		{
			AktivniItemString = PlayerPrefs.GetString("AktivniItemi");
			AktivniItemi=AktivniItemString.Split('#');
			AktivanSesir=int.Parse(AktivniItemi[0]);
			AktivnaMajica=int.Parse(AktivniItemi[1]);
			AktivanRanac=int.Parse(AktivniItemi[2]);
			//Debug.Log("aktivni itemi: " + PlayerPrefs.GetString("AktivniItemi"));

		}
		else
		{
			AktivanSesir = -1;
			AktivnaMajica = -1;
			AktivanRanac = -1;
		}



		PreviewSesir = -1;
		PreviewMajica = -1;
		PreviewRanac = -1;
		MajmunBobo=GameObject.Find("MonkeyHolder");
		CustomizationHats = GameObject.Find("1Hats");
		CustomizationShirts = GameObject.Find("2Shirts");
		CustomizationBackPack = GameObject.Find("3BackPack");
		MainScenaPozicija = MajmunBobo.transform.position;

		BrojItemaShopHats=CountItemsInShop(GameObject.Find("Shop/3 Customize/Customize Tabovi/1Hats").GetComponent<Transform>());
		BrojItemaShopShirts=CountItemsInShop(GameObject.Find("Shop/3 Customize/Customize Tabovi/2Shirts").GetComponent<Transform>());
		BrojItemaShopBackPack=CountItemsInShop(GameObject.Find("Shop/3 Customize/Customize Tabovi/3BackPack").GetComponent<Transform>());
		BrojItemaShop = BrojItemaShopHats+BrojItemaShopShirts+BrojItemaShopBackPack;
		//Debug.Log ("BrojItemaShopHats: " + BrojItemaShopHats + " BrojItemaShopShirts: " + BrojItemaShopShirts + " BrojItemaShopBackPack: " + BrojItemaShopBackPack);
//		if(PlayerPrefs.HasKey("choosenLanguage"))
//		{
//			chosenLanguage=PlayerPrefs.GetString("choosenLanguage");
//			
//		}
//		else
//		chosenLanguage="_en";

		ObuciMajmunaNaStartu();

		transform.name="Shop";

		SviItemiInvetory();

		StartCoroutine(PokreniInicijalizacijuShopa());

	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetMouseButtonDown(0))
		{
			clickedItem = RaycastFunction(Input.mousePosition);
			
			if(clickedItem.Equals("NekiNaziv") || clickedItem.Equals("NekiNaziv1")) 
			{
				temp = GameObject.Find(clickedItem);
				originalScale = temp.transform.localScale;
				temp.transform.localScale = originalScale * 0.8f;
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
				
				
				if(releasedItem == "NekoDugme")
				{
					//Debug.Log("NekoDugme");
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
				}
			}
		}
		if(ObjCustomizationHats.CustomizationHats || ObjCustomizationShirts.CustomizationShirts || ObjCustomizationBackPacks.CustomizationBackPacks)
		{
			if(AktivanCustomizationTab==1)
			{
				ProveraTrenutnogItema(AktivanItemSesir);
			}
			else if(AktivanCustomizationTab==2)
			{
				ProveraTrenutnogItema(AktivanItemMajica);
			}
			else if(AktivanCustomizationTab==3)
			{
				ProveraTrenutnogItema(AktivanItemRanac);
			}
//			ProveraTrenutnogItema(AktivanItem);
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



	int CountItemsInShop(Transform Shop)
	{
		int ChildCount=0;
		foreach(Transform Item in Shop)
		{
//			//Debug.Log("Dete: "+Item);
			ChildCount++;
		}
//		//Debug.Log("Ukupno ima: " +ChildCount);
		return ChildCount;
	}

//	IEnumerator RefresujImenaItema()
//	{
//		aset2 = (TextAsset)Resources.Load("xmls/Shop/Shop"+LanguageManager.chosenLanguage);
//		aset = aset2.text;
//	}
	public void RefresujImenaItema()
	{
		StartCoroutine(ParsirajImenaItemaIzRadnje());
	}

	public void PobrisiSveOtkljucanoIzShopa()
	{
		ZakljucaniHats.Clear();
		ZakljucaniShirts.Clear();
		ZakljucaniBackPacks.Clear();
	}

	IEnumerator ParsirajImenaItemaIzRadnje()
	{
		yield return null;

		aset2 = (TextAsset)Resources.Load("xmls/Shop/Shop"+LanguageManager.chosenLanguage);

		aset = aset2.text;

		CoinsHats=new List<string>();
		CoinsShirts=new List<string>();
		CoinsBackPacks=new List<string>();
		CoinsPowerUps=new List<string>();
		ImenaHats=new List<string>();
		ImenaShirts=new List<string>();
		ImenaBackPacks=new List<string>();
		ImenaPowerUps=new List<string>();

			XElement xmlNov= XElement.Parse(aset.ToString());
			IEnumerable<XElement> xmls = xmlNov.Elements();	
			int number=xmls.Count();

			if(StagesParser.unlockedWorlds[4])
			{
				ProcenatOtkljucan=1;

//				//Debug.Log("Dobijeni broj je: "+ProcenatOtkljucan);
			}
			else if(StagesParser.unlockedWorlds[3])
			{
				ProcenatOtkljucan=0.9f;
//				//Debug.Log("Dobijeni broj je: "+ProcenatOtkljucan);
			}
			else if(StagesParser.unlockedWorlds[2])
			{
				ProcenatOtkljucan=0.8f;
//				//Debug.Log("Dobijeni broj je: "+ProcenatOtkljucan);
			}
			else if(StagesParser.unlockedWorlds[1])
			{
				ProcenatOtkljucan=0.7f;
//				//Debug.Log("Dobijeni broj je: "+ProcenatOtkljucan);
			}
			else if(StagesParser.unlockedWorlds[0])
			{
				ProcenatOtkljucan=0.6f;
//				//Debug.Log("Dobijeni broj je: "+ProcenatOtkljucan);
			}

			if(PlayerPrefs.HasKey("OtkljucaniItemi"))
			{
				StariBrojOtkljucanihItema=PlayerPrefs.GetString("OtkljucaniItemi");
			}
			else
			{
				StariBrojOtkljucanihItema="0#0#0";
			}
//			//Debug.Log(StariBrojOtkljucanihItema);

			StariBrojOtkljucanihItemaNiz=StariBrojOtkljucanihItema.Split('#');
			StariBrojOtkljucanihKapa=int.Parse(StariBrojOtkljucanihItemaNiz[0]);
			StariBrojOtkljucanihMajici=int.Parse(StariBrojOtkljucanihItemaNiz[1]);
			StariBrojOtkljucanihRanceva=int.Parse(StariBrojOtkljucanihItemaNiz[2]);
			BrojOtkljucanihKapa = Mathf.FloorToInt((BrojItemaShopHats)*ProcenatOtkljucan)-1;
			BrojOtkljucanihMajici = Mathf.FloorToInt((BrojItemaShopShirts)*ProcenatOtkljucan)-1;
			BrojOtkljucanihRanceva = Mathf.FloorToInt((BrojItemaShopBackPack)*ProcenatOtkljucan)-1;
			StariBrojOtkljucanihItema=BrojOtkljucanihKapa+"#"+BrojOtkljucanihMajici+"#"+BrojOtkljucanihRanceva;
			PlayerPrefs.SetString("OtkljucaniItemi",StariBrojOtkljucanihItema);
			PlayerPrefs.Save();

//			//Debug.Log("StariBrojOtkljucanihKapa "+StariBrojOtkljucanihKapa+" BrojOtkljucanihKapa "+BrojOtkljucanihKapa);

			for(int j=0;j<BrojItemaShopHats;j++)
			{
				if(BrojOtkljucanihKapa>=j)
				{
					ZakljucaniHats.Add(1);
				}
				else
				{
					ZakljucaniHats.Add(0);
				}
				
//				//Debug.Log("Zakljucani niz hats broj "+j+" je "+ZakljucaniHats[j]);
			}
			
			for(int j=0;j<BrojItemaShopShirts;j++)
			{
				if(BrojOtkljucanihMajici>=j)
				{
					ZakljucaniShirts.Add(1);
				}
				else
				{
					ZakljucaniShirts.Add(0);
				}
				
//				//Debug.Log("Zakljucani niz Shirts broj "+j+" je "+ZakljucaniShirts[j]);
			}
			
			for(int j=0;j<BrojItemaShopBackPack;j++)
			{
				if(BrojOtkljucanihRanceva>=j)
				{
					ZakljucaniBackPacks.Add(1);
				}
				else
				{
					ZakljucaniBackPacks.Add(0);
				}
				
//				//Debug.Log("Zakljucani niz BackPacks broj "+j+" je "+ZakljucaniBackPacks[j]);
			}

			if(xmls.Count()==BrojItemaShop+4)
			{
			for(int i=0;i<BrojItemaShopHats;i++)
				{

					if(ZakljucaniHats[i]==1)
					{
						HatsObjects[i].Find("Zakkljucano").gameObject.SetActive(false);
						HatsObjects[i].Find("Polje za unos COINA U shopu - Shop").gameObject.SetActive(true);
					}
					else
					{
						HatsObjects[i].Find("Polje za unos COINA U shopu - Shop").gameObject.SetActive(false);
						HatsObjects[i].Find("Bedz - Popust").gameObject.SetActive(false);
						HatsObjects[i].Find("Zakkljucano").gameObject.SetActive(true);
						HatsObjects[i].Find("Polje za unos COINA U shopu - Shop_NoDiscount").gameObject.SetActive(false);
					}

					if(xmls.ElementAt(i).Attribute("kategorija").Value=="Hats")
					{
						ImenaHats.Add((string)xmls.ElementAt(i).Value);
						CoinsHats.Add((string)xmls.ElementAt(i).Attribute("coins").Value);
						BananaHats.Add((string)xmls.ElementAt(i).Attribute("banana").Value);
						PopustHats.Add((string)xmls.ElementAt(i).Attribute("popust").Value);
//						UsiHats.Add((string)xmls.ElementAt(i).Attribute("usi").Value);
//						KosaHats.Add((string)xmls.ElementAt(i).Attribute("kosa").Value);
						if(SveStvariZaOblacenjeHats[i]==1) // Kupljen je
						{
							HatsObjects[i].Find("Polje za unos COINA U shopu - Shop").gameObject.SetActive(false);							
							HatsObjects[i].Find("Bedz - Popust").gameObject.SetActive(false);
							HatsObjects[i].Find("Polje za unos COINA U shopu - Shop_NoDiscount").gameObject.SetActive(false);
						}
						else
						{
						

							if(PopustHats[i]=="0")
							{		
								HatsObjects[i].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text=CoinsHats[i];
								HatsObjects[i].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
								if(i>StariBrojOtkljucanihKapa & i<=BrojOtkljucanihKapa)
								{
//									//Debug.Log("Kapa sa indeksom "+i+" je novi element posto je "+i+">"+StariBrojOtkljucanihKapa+" i "+i+"<"+BrojOtkljucanihKapa);
									ImaNovihKapa=true;
									HatsObjects[i].Find("Bedz - Popust/Text/Number").GetComponent<TextMesh>().text=LanguageManager.New;
									HatsObjects[i].Find("Bedz - Popust/Text/Number/Number Shadow").GetComponent<TextMesh>().text=LanguageManager.New;
								}
								else
								{
//									//Debug.Log("Kapa sa indeksom "+i+" nije novi element");
									HatsObjects[i].Find("Bedz - Popust").gameObject.SetActive(false);
								}
							}
							else
							{
								if(i>StariBrojOtkljucanihKapa & i<=BrojOtkljucanihKapa)
								{
//									//Debug.Log("Kapa sa indeksom "+i+" je novi element posto je "+i+">"+StariBrojOtkljucanihKapa+" i "+i+"<"+BrojOtkljucanihKapa );
									ImaNovihKapa=true;
									HatsObjects[i].Find("Bedz - Popust/Text/Number").GetComponent<TextMesh>().text=LanguageManager.New;
									HatsObjects[i].Find("Bedz - Popust/Text/Number/Number Shadow").GetComponent<TextMesh>().text=LanguageManager.New;

									if(ZakljucaniHats[i]==1)
									{
										HatsObjects[i].Find("Polje za unos COINA U shopu - Shop_NoDiscount").gameObject.SetActive(true);
										HatsObjects[i].Find("Polje za unos COINA U shopu - Shop_NoDiscount/Coins Number").GetComponent<TextMesh>().text=CoinsHats[i];
										if(HatsObjects[i].Find("Polje za unos COINA U shopu - Shop_NoDiscount/Coins Number").gameObject.activeSelf)
											HatsObjects[i].Find("Polje za unos COINA U shopu - Shop_NoDiscount/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
										
										string popust="0."+PopustHats[i];
										float cena=float.Parse(CoinsHats[i])-float.Parse(CoinsHats[i])*float.Parse(popust);
										CoinsHats[i]=cena.ToString();
										HatsObjects[i].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text=cena.ToString();
										HatsObjects[i].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
										HatsObjects[i].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().color=PopustBoja;
									}
								}
								else
								{
//									//Debug.Log("Kapa sa indeksom "+i+" nije novi element");
									if(ZakljucaniHats[i]==1)
									{
										HatsObjects[i].Find("Polje za unos COINA U shopu - Shop_NoDiscount").gameObject.SetActive(true);
										HatsObjects[i].Find("Bedz - Popust/Text/Number").GetComponent<TextMesh>().text="-"+PopustHats[i]+"%";
										HatsObjects[i].Find("Bedz - Popust/Text/Number/Number Shadow").GetComponent<TextMesh>().text=PopustHats[i]+"%";

										HatsObjects[i].Find("Polje za unos COINA U shopu - Shop_NoDiscount/Coins Number").GetComponent<TextMesh>().text=CoinsHats[i];
										if(HatsObjects[i].Find("Polje za unos COINA U shopu - Shop_NoDiscount/Coins Number").gameObject.activeSelf)
											HatsObjects[i].Find("Polje za unos COINA U shopu - Shop_NoDiscount/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);

										string popust="0."+PopustHats[i];
										float cena=float.Parse(CoinsHats[i])-float.Parse(CoinsHats[i])*float.Parse(popust);
										CoinsHats[i]=cena.ToString();
										HatsObjects[i].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text=cena.ToString();
										HatsObjects[i].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
										HatsObjects[i].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().color=PopustBoja;
									}
									
								}

								//OVDE SE NALAZIO DEO ZA POPUSTE, PREBACEN JE U ELSE IZNAD!!!!!
							}
						}

						HatsObjects[i].Find("Text/ime").GetComponent<TextMesh>().text=ImenaHats[i];
						HatsObjects[i].Find("Text/ime").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);


//						//Debug.Log("Ime itema broj "+i+" je :"+ImenaHats[i]+" coini: "+CoinsHats[i]+" banana: "+BananaHats[i]);
					}
				}



				for(int j=0;j<BrojItemaShopShirts;j++)
				{

					if(ZakljucaniShirts[j]==1)
					{
						ShirtsObjects[j].Find("Zakkljucano").gameObject.SetActive(false);
					ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop").gameObject.SetActive(true);
					}
					else
					{
						ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop").gameObject.SetActive(false);
						ShirtsObjects[j].Find("Bedz - Popust").gameObject.SetActive(false);
						ShirtsObjects[j].Find("Zakkljucano").gameObject.SetActive(true);
						ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop_NoDiscount").gameObject.SetActive(false);
					}

					if(xmls.ElementAt(BrojItemaShopHats+j).Attribute("kategorija").Value=="Shirts")
					{
						ImenaShirts.Add((string)xmls.ElementAt(BrojItemaShopHats+j).Value);
						CoinsShirts.Add((string)xmls.ElementAt(BrojItemaShopHats+j).Attribute("coins").Value);
						BananaShirts.Add((string)xmls.ElementAt(BrojItemaShopHats+j).Attribute("banana").Value);
						PopustShirts.Add((string)xmls.ElementAt(BrojItemaShopHats+j).Attribute("popust").Value);
						if(SveStvariZaOblacenjeShirts[j]==1)
						{
							ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop").gameObject.SetActive(false);
							ShirtsObjects[j].Find("Bedz - Popust").gameObject.SetActive(false);
							ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop_NoDiscount").gameObject.SetActive(false);

						}
						else
						{
							if(PopustShirts[j]=="0")
							{
								ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text=CoinsShirts[j];
								ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
								if(j>StariBrojOtkljucanihMajici & j<=BrojOtkljucanihMajici)
								{
//									//Debug.Log("Majica sa indeksom "+j+" je novi element posto je "+j+">"+StariBrojOtkljucanihMajici+" i "+j+"<"+BrojOtkljucanihMajici);
									ImaNovihMajica=true;
									ShirtsObjects[j].Find("Bedz - Popust/Text/Number").GetComponent<TextMesh>().text=LanguageManager.New;
									ShirtsObjects[j].Find("Bedz - Popust/Text/Number/Number Shadow").GetComponent<TextMesh>().text=LanguageManager.New;
								}
								else
								{
//									//Debug.Log("Majica sa indeksom "+j+" nije novi element");
									ShirtsObjects[j].Find("Bedz - Popust").gameObject.SetActive(false);
								}
							}
							else
							{
								if(j>StariBrojOtkljucanihMajici & j<=BrojOtkljucanihMajici)
								{
//									//Debug.Log("Majica sa indeksom "+j+" je novi element posto je "+j+">"+StariBrojOtkljucanihMajici+" i "+j+"<"+BrojOtkljucanihMajici );
									ImaNovihMajica=true;
									ShirtsObjects[j].Find("Bedz - Popust/Text/Number").GetComponent<TextMesh>().text=LanguageManager.New;
									ShirtsObjects[j].Find("Bedz - Popust/Text/Number/Number Shadow").GetComponent<TextMesh>().text=LanguageManager.New;

									if(ZakljucaniShirts[j]==1)
									{
										ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop_NoDiscount").gameObject.SetActive(true);
										
										ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop_NoDiscount/Coins Number").GetComponent<TextMesh>().text=CoinsShirts[j].ToString();
										if(ShirtsObjects[j].parent.gameObject.activeSelf)
											ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop_NoDiscount/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
										
										string popust="0."+PopustShirts[j];
										float cena=float.Parse(CoinsShirts[j])-float.Parse(CoinsShirts[j])*float.Parse(popust);
										CoinsShirts[j]=cena.ToString();
										ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text=cena.ToString();
										ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
										ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().color=PopustBoja;
									}
									
								}
								else
								{
//									//Debug.Log("Kapa sa indeksom "+j+" nije novi element");
									if(ZakljucaniShirts[j]==1)
									{
										ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop_NoDiscount").gameObject.SetActive(true);

										ShirtsObjects[j].Find("Bedz - Popust/Text/Number").GetComponent<TextMesh>().text="-"+PopustShirts[j]+"%";
										ShirtsObjects[j].Find("Bedz - Popust/Text/Number/Number Shadow").GetComponent<TextMesh>().text=PopustShirts[j]+"%";

										ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop_NoDiscount/Coins Number").GetComponent<TextMesh>().text=CoinsShirts[j].ToString();
										if(ShirtsObjects[j].parent.gameObject.activeSelf)
											ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop_NoDiscount/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
										
										string popust="0."+PopustShirts[j];
										float cena=float.Parse(CoinsShirts[j])-float.Parse(CoinsShirts[j])*float.Parse(popust);
										CoinsShirts[j]=cena.ToString();
										ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text=cena.ToString();
										ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
										ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().color=PopustBoja;
									}

								}

								
							}
						}
						ShirtsObjects[j].Find("Text/ime").GetComponent<TextMesh>().text=ImenaShirts[j];
						ShirtsObjects[j].Find("Text/ime").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

//						//Debug.Log("Ime itema broj "+j+" je :"+ImenaShirts[j]);
					}
				}

				for(int k=0;k<BrojItemaShopBackPack;k++)
				{

					if(ZakljucaniBackPacks[k]==1)
					{
						BackPacksObjects[k].Find("Zakkljucano").gameObject.SetActive(false);
					BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop").gameObject.SetActive(true);
					}
					else
					{
						BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop").gameObject.SetActive(false);
						BackPacksObjects[k].Find("Bedz - Popust").gameObject.SetActive(false);
						BackPacksObjects[k].Find("Zakkljucano").gameObject.SetActive(true);
						BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop_NoDiscount").gameObject.SetActive(false);
					}

					if(xmls.ElementAt(BrojItemaShopHats+BrojItemaShopShirts+k).Attribute("kategorija").Value=="BackPack")
					{
						ImenaBackPacks.Add((string)xmls.ElementAt(BrojItemaShopHats+BrojItemaShopShirts+k).Value);
						CoinsBackPacks.Add((string)xmls.ElementAt(BrojItemaShopHats+BrojItemaShopShirts+k).Attribute("coins").Value);
						BananaBackPacks.Add((string)xmls.ElementAt(BrojItemaShopHats+BrojItemaShopShirts+k).Attribute("banana").Value);
						PopustBackPacks.Add((string)xmls.ElementAt(BrojItemaShopHats+BrojItemaShopShirts+k).Attribute("popust").Value);
						if(SveStvariZaOblacenjeBackPack[k]==1)
						{
							BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop").gameObject.SetActive(false);
							BackPacksObjects[k].Find("Bedz - Popust").gameObject.SetActive(false);
							BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop_NoDiscount").gameObject.SetActive(false);
						}
						else
						{

							if(PopustBackPacks[k]=="0")
							{
								BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text=CoinsBackPacks[k];
								BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
								if(k>StariBrojOtkljucanihRanceva & k<=BrojOtkljucanihRanceva)
								{
//									//Debug.Log("Ranac sa indeksom "+k+" je novi element posto je "+k+">"+StariBrojOtkljucanihRanceva+" i "+k+"<"+BrojOtkljucanihRanceva);
									ImaNovihRanceva=true;
									BackPacksObjects[k].Find("Bedz - Popust/Text/Number").GetComponent<TextMesh>().text=LanguageManager.New;
									BackPacksObjects[k].Find("Bedz - Popust/Text/Number/Number Shadow").GetComponent<TextMesh>().text=LanguageManager.New;
								}
								else
								{
//									//Debug.Log("Ranac sa indeksom "+k+" nije novi element");
									BackPacksObjects[k].Find("Bedz - Popust").gameObject.SetActive(false);
								}
							}
							else
							{
								BackPacksObjects[k].Find("Bedz - Popust/Text/Number").GetComponent<TextMesh>().text="-"+PopustBackPacks[k]+"%";
								BackPacksObjects[k].Find("Bedz - Popust/Text/Number/Number Shadow").GetComponent<TextMesh>().text=PopustBackPacks[k]+"%";

								if(k>StariBrojOtkljucanihRanceva & k<=BrojOtkljucanihRanceva)
								{
//									//Debug.Log("Ranac sa indeksom "+k+" je novi element posto je "+k+">"+StariBrojOtkljucanihRanceva+" i "+k+"<"+BrojOtkljucanihRanceva );
									ImaNovihRanceva=true;
									BackPacksObjects[k].Find("Bedz - Popust/Text/Number").GetComponent<TextMesh>().text=LanguageManager.New;
									BackPacksObjects[k].Find("Bedz - Popust/Text/Number/Number Shadow").GetComponent<TextMesh>().text=LanguageManager.New;

									if(ZakljucaniBackPacks[k]==1)
									{
										BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop_NoDiscount").gameObject.SetActive(true);
										
										BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop_NoDiscount/Coins Number").GetComponent<TextMesh>().text=CoinsBackPacks[k].ToString();
										if(BackPacksObjects[k].parent.gameObject.activeSelf)
											BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop_NoDiscount/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
										
										string popust="0."+PopustBackPacks[k];
										float cena=float.Parse(CoinsBackPacks[k])-float.Parse(CoinsBackPacks[k])*float.Parse(popust);
										CoinsBackPacks[k]=cena.ToString();
										BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text=cena.ToString();
										BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
										BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().color=PopustBoja;
									}
									
								}
								else
								{
//									//Debug.Log("Ranac sa indeksom "+k+" nije novi element");
									if(ZakljucaniBackPacks[k]==1)
									{
										BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop_NoDiscount").gameObject.SetActive(true);

										BackPacksObjects[k].Find("Bedz - Popust/Text/Number").GetComponent<TextMesh>().text="-"+PopustBackPacks[k]+"%";
										BackPacksObjects[k].Find("Bedz - Popust/Text/Number/Number Shadow").GetComponent<TextMesh>().text=PopustBackPacks[k]+"%";

										BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop_NoDiscount/Coins Number").GetComponent<TextMesh>().text=CoinsBackPacks[k].ToString();
										if(BackPacksObjects[k].parent.gameObject.activeSelf)
											BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop_NoDiscount/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
										
										string popust="0."+PopustBackPacks[k];
										float cena=float.Parse(CoinsBackPacks[k])-float.Parse(CoinsBackPacks[k])*float.Parse(popust);
										CoinsBackPacks[k]=cena.ToString();
										BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text=cena.ToString();
										BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
										BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().color=PopustBoja;
									}

								}

								
							}
						}

						BackPacksObjects[k].Find("Text/ime").GetComponent<TextMesh>().text=ImenaBackPacks[k];
						BackPacksObjects[k].Find("Text/ime").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

//						//Debug.Log("Ime itema broj "+k+" je :"+ImenaBackPacks[k]);
					}
					
				}


				for(int i=0;i<3;i++)
				{

					if(xmls.ElementAt(BrojItemaShopHats+BrojItemaShopShirts+BrojItemaShopBackPack+i).Attribute("kategorija").Value=="PowerUps")
					{
//						//Debug.Log("Usao u PowerUPs");
						ImenaPowerUps.Add((string)xmls.ElementAt(BrojItemaShopHats+BrojItemaShopShirts+BrojItemaShopBackPack+i).Value);
						CoinsPowerUps.Add((string)xmls.ElementAt(BrojItemaShopHats+BrojItemaShopShirts+BrojItemaShopBackPack+i).Attribute("coins").Value);
						PopustPowerUps.Add((string)xmls.ElementAt(BrojItemaShopHats+BrojItemaShopShirts+BrojItemaShopBackPack+i).Attribute("popust").Value);

							
							
							
						if(PopustPowerUps[i]=="0")
							{
								
								PowerUpsObjects[i].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text=CoinsPowerUps[i];
								PowerUpsObjects[i].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
								PowerUpsObjects[i].Find("Popust").gameObject.SetActive(false);

							}
							else
							{

								PowerUpsObjects[i].Find("Popust/Text/Number").GetComponent<TextMesh>().text="-"+PopustPowerUps[i]+"%";
								PowerUpsObjects[i].Find("Popust/Text/Number/Number Shadow").GetComponent<TextMesh>().text=PopustPowerUps[i]+"%";
									
									
								string popust="0."+PopustPowerUps[i];
								float cena=float.Parse(CoinsPowerUps[i])-float.Parse(CoinsPowerUps[i])*float.Parse(popust);
								CoinsPowerUps[i]=cena.ToString();
								PowerUpsObjects[i].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text=cena.ToString();
								PowerUpsObjects[i].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
								PowerUpsObjects[i].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().color=PopustBoja;
							}
						
						
						
						PowerUpsObjects[i].Find("Text/ime").GetComponent<TextMesh>().text=ImenaPowerUps[i];
						PowerUpsObjects[i].Find("Text/ime").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
						
						
//						//Debug.Log("Ime itema broj "+i+" je :"+ImenaHats[i]+" coini: "+CoinsHats[i]+" banana: "+BananaHats[i]);
					}
				}
				StagesParser.cost_doublecoins = int.Parse(CoinsPowerUps[0]);
				StagesParser.cost_magnet = int.Parse(CoinsPowerUps[1]);
				StagesParser.cost_shield = int.Parse(CoinsPowerUps[2]);

				if(xmls.ElementAt(BrojItemaShopHats+BrojItemaShopShirts+BrojItemaShopBackPack+3).Attribute("kategorija").Value=="Banana")
				{
					ImeBanana=(string)xmls.ElementAt(BrojItemaShopHats+BrojItemaShopShirts+BrojItemaShopBackPack+3).Value;
					cenaBanana=(string)xmls.ElementAt(BrojItemaShopHats+BrojItemaShopShirts+BrojItemaShopBackPack+3).Attribute("coins").Value;
					PopustBanana=(string)xmls.ElementAt(BrojItemaShopHats+BrojItemaShopShirts+BrojItemaShopBackPack+3).Attribute("popust").Value;
					string popust="0."+PopustBanana;
					float cena=float.Parse(cenaBanana)-float.Parse(cenaBanana)*float.Parse(popust);
					cenaBanana=cena.ToString();
					StagesParser.bananaCost = (int)cena;
					if(int.Parse(PopustBanana)>0)
					{
						ShopBanana.transform.FindChild("Popust").gameObject.SetActive(true);
						ShopBanana.transform.FindChild("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().color=PopustBoja;
						ShopBanana.transform.FindChild("Popust/Text/Number").GetComponent<TextMesh>().text=PopustBanana+"%";
						ShopBanana.transform.FindChild("Popust/Text/Number/Number Shadow").GetComponent<TextMesh>().text=PopustBanana+"%";
					}
					else
					{
						ShopBanana.transform.FindChild("Popust").gameObject.SetActive(false);
					}
					ShopBanana.transform.FindChild("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text=cenaBanana;
					ShopBanana.transform.FindChild("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

					ShopBanana.transform.FindChild("Text/Banana").GetComponent<TextMesh>().text=ImeBanana;
					ShopBanana.transform.FindChild("Text/Banana").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
//					//Debug.Log("Ime itema je :"+ImeBanana+" coini: "+cenaBanana+" popust: "+PopustBanana);
				}
//				GameObject.Find("FacebookManager").GetComponent<FacebookManager>().ProveriKorisnika();
//				while(!FacebookManager.KorisnikoviPodaciSpremni)
//				{
//					yield return null;
//				}
			
			if(ImaNovihKapa)
				{
					ZidFooter.transform.Find("1HatsShopTab/Znak Uzvika telo").gameObject.SetActive(true);
				}
				else
				{
					ZidFooter.transform.Find("1HatsShopTab/Znak Uzvika telo").gameObject.SetActive(false);
				}
			
			if(ImaNovihMajica)
				{
					ZidFooter.transform.Find("2TShirtsShopTab/Znak Uzvika telo").gameObject.SetActive(true);
				}
				else
				{
					ZidFooter.transform.Find("2TShirtsShopTab/Znak Uzvika telo").gameObject.SetActive(false);
				}

			if(ImaNovihRanceva)
				{
					ZidFooter.transform.Find("3BackPackShopTab/Znak Uzvika telo").gameObject.SetActive(true);
				}
				else
				{
					ZidFooter.transform.Find("3BackPackShopTab/Znak Uzvika telo").gameObject.SetActive(false);
				}

				if(ImaNovihKapa | ImaNovihMajica | ImaNovihRanceva)
				{
					Custumization.transform.Find("Znak Uzvika telo").gameObject.SetActive(true);
					Custumization.GetComponent<Animation>().PlayQueued("Button Customization Idle",QueueMode.CompleteOthers);
				}
				else
				{
					Custumization.transform.Find("Znak Uzvika telo").gameObject.SetActive(false);
				}

			}
			else
			{
				//Debug.Log ("Broj Itema u prodavnici i XML-u Shop nije isti za jezik: "+LanguageManager.chosenLanguage);
			}


		
		



	}

	public void SviItemiInvetory()
	{
		SveStvariZaOblacenjeHats.Clear();
		SveStvariZaOblacenjeShirts.Clear();
		SveStvariZaOblacenjeBackPack.Clear();
		Hats = StagesParser.svekupovineGlava.Split('#');
		Shirts = StagesParser.svekupovineMajica.Split('#');
		BackPacks = StagesParser.svekupovineLedja.Split('#');
		for(int j=0;j<BrojItemaShopHats;j++)
		{
			if(Hats.Length-1>j)
			{
//				//Debug.Log("Sesir broj "+j+" je "+Hats[j]);
				SveStvariZaOblacenjeHats.Add(int.Parse(Hats[j]));
			}
			else
			{
//				//Debug.Log("Puni bzv "+j+" "+Hats[j]);
				SveStvariZaOblacenjeHats.Add(0);
			}

			if(SveStvariZaOblacenjeHats[j]==0)
			{
				HatsObjects[j].Find("Stikla").gameObject.SetActive(false);
			}
			else
			{
				HatsObjects[j].Find("Stikla").gameObject.SetActive(true);
			}
//			//Debug.Log("SveStvariZaOblacenjeHats "+j+" je : "+SveStvariZaOblacenjeHats[j]);
		}

		for(int j=0;j<BrojItemaShopShirts;j++)
		{

			if(Shirts.Length-1>j)
			{
				SveStvariZaOblacenjeShirts.Add(int.Parse(Shirts[j]));
			}
			else
			{
				SveStvariZaOblacenjeShirts.Add(0);
			}

			if(SveStvariZaOblacenjeShirts[j]==0)
			{
				ShirtsObjects[j].Find("Stikla").gameObject.SetActive(false);
			}
			else
			{
				ShirtsObjects[j].Find("Stikla").gameObject.SetActive(true);
			}
//			//Debug.Log("SveStvariZaOblacenjeShirts "+j+" je : "+SveStvariZaOblacenjeShirts[j]);
		}

		for(int j=0;j<BrojItemaShopBackPack;j++)
		{

			if(BackPacks.Length-1>j)
			{
				SveStvariZaOblacenjeBackPack.Add(int.Parse(BackPacks[j]));
			}
			else
			{
				SveStvariZaOblacenjeBackPack.Add(0);
			}

			if(SveStvariZaOblacenjeBackPack[j]==0)
			{
				BackPacksObjects[j].Find("Stikla").gameObject.SetActive(false);
			}
			else
			{
				BackPacksObjects[j].Find("Stikla").gameObject.SetActive(true);
			}
//			//Debug.Log("SveStvariZaOblacenjeBackPack "+j+" je : "+SveStvariZaOblacenjeBackPack[j]);
		}
		ShopInicijalizovan=true;
		PokreniShop();
		Hats = null;
		Shirts = null;
		BackPacks = null;

	}

	IEnumerator PokreniInicijalizacijuShopa()
	{
		if(FacebookManager.KorisnikoviPodaciSpremni)
		{
			//Debug.Log("PokreniInicijalizacijuShopa Podaci Spremni");
			StartCoroutine(ParsirajImenaItemaIzRadnje());
		}
		else
		{
			//Debug.Log("PokreniInicijalizacijuShopa Podaci NISU Spremni");
			//FacebookManager.FacebookObject.ProveriKorisnika();
			FacebookManager.UserCoins = StagesParser.currentMoney;
			FacebookManager.UserScore = StagesParser.currentPoints;
			FacebookManager.UserLanguage = LanguageManager.chosenLanguage;
			FacebookManager.UserBanana = StagesParser.currentBananas;
			FacebookManager.UserPowerMagnet = StagesParser.powerup_magnets;
			FacebookManager.UserPowerShield = StagesParser.powerup_shields;
			FacebookManager.UserPowerDoubleCoins = StagesParser.powerup_doublecoins;
			FacebookManager.UserSveKupovineHats = StagesParser.svekupovineGlava;
			FacebookManager.UserSveKupovineShirts = StagesParser.svekupovineMajica;
			FacebookManager.UserSveKupovineBackPacks = StagesParser.svekupovineLedja;
			
			FacebookManager.GlavaItem = StagesParser.glava;
			FacebookManager.TeloItem = StagesParser.majica;
			FacebookManager.LedjaItem = StagesParser.ledja;
			FacebookManager.Usi = StagesParser.imaUsi;
			FacebookManager.Kosa = StagesParser.imaKosu;
			
			FacebookManager.KorisnikoviPodaciSpremni=true;

			while(!FacebookManager.KorisnikoviPodaciSpremni)
			{
				yield return null;
			}
			StartCoroutine(PokreniInicijalizacijuShopa());
		}
	}

	public void PokreniShop()
	{
		if(!ShopInicijalizovan)
		{
			StartCoroutine(PokreniInicijalizacijuShopa());
		}
		else
		{
			//Pokreni Shop
			//Debug.Log("Shop je pokrenut!");
		}

	}

	public void SkloniShop()
	{
		if(Application.loadedLevel == 1)
		{
			MajmunBobo.transform.Find("PrinceGorilla").GetComponent<Animator>().Play("Idle Main Screen");
			MajmunBobo.transform.Find("ButterflyHolder").gameObject.SetActive(true);
			if(AktivanRanac == 0)
				MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/"+AktivanRanac).transform.GetChild(0).GetComponent<MeshFilter>().mesh = GameObject.Find("RefZaSedenje_AndjeoskaKrila").GetComponent<MeshFilter>().mesh;
			else if(AktivanRanac == 5)
				MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/"+AktivanRanac).transform.GetChild(0).GetComponent<MeshFilter>().mesh = GameObject.Find("RefZaSedenje_SlepiMisKrila").GetComponent<MeshFilter>().mesh;
		}
		otvorenShop = false;
		OcistiPreview();
		DeaktivirajCustomization();
		MajmunBobo.transform.position = MainScenaPozicija;
		MajmunBobo.transform.Find("PrinceGorilla").rotation = Quaternion.Euler(new Vector3(0,104,0));
		DeaktivirajFreeCoins ();
		DeaktivirajPowerUps ();
		DeaktivirajShopTab();
//		AktivanCustomizationTab = 0;
		if(PlayerPrefs.HasKey("OtkljucaniItemi"))
		{
			StariBrojOtkljucanihItema=PlayerPrefs.GetString("OtkljucaniItemi");
		}
		else
		{
			StariBrojOtkljucanihItema="0#0#0";
		}

		StariBrojOtkljucanihItemaNiz=StariBrojOtkljucanihItema.Split('#');
		StariBrojOtkljucanihKapa=int.Parse(StariBrojOtkljucanihItemaNiz[0]);
		StariBrojOtkljucanihMajici=int.Parse(StariBrojOtkljucanihItemaNiz[1]);
		StariBrojOtkljucanihRanceva=int.Parse(StariBrojOtkljucanihItemaNiz[2]);

		if(ImaNovihKapa)
		{
			BrojOtkljucanihKapa = Mathf.FloorToInt((BrojItemaShopHats)*ProcenatOtkljucan)-1;
		}
		else
		{
			BrojOtkljucanihKapa=StariBrojOtkljucanihKapa;
		}

		if(ImaNovihMajica)
		{
			BrojOtkljucanihMajici = Mathf.FloorToInt((BrojItemaShopShirts)*ProcenatOtkljucan)-1;
		}
		else
		{
			BrojOtkljucanihMajici=StariBrojOtkljucanihMajici;
		}

		if(ImaNovihRanceva)
		{
			BrojOtkljucanihRanceva = Mathf.FloorToInt((BrojItemaShopBackPack)*ProcenatOtkljucan)-1;
		}
		else
		{
			BrojOtkljucanihRanceva=StariBrojOtkljucanihRanceva;
		}

		if(!ImaNovihKapa && !ImaNovihMajica && !ImaNovihRanceva)
		{
				//Debug.Log("Sacuvaj!!!!!!!!!!");
					StariBrojOtkljucanihItema=BrojOtkljucanihKapa+"#"+BrojOtkljucanihMajici+"#"+BrojOtkljucanihRanceva;
					PlayerPrefs.SetString("OtkljucaniItemi",StariBrojOtkljucanihItema);
					PlayerPrefs.Save();
		}




		ProveriStanjeCelogShopa ();
		GameObject.Find("Shop").GetComponent<Animation>().Play("MeniOdlazak");
		if(AktivanTab==1)
		{
			GameObject.Find("ButtonFreeCoins").GetComponent<SpriteRenderer>().sprite=GameObject.Find("ShopTab").GetComponent<SpriteRenderer>().sprite;

		}
		else if(AktivanTab==2)
		{
			GameObject.Find("ButtonShop").GetComponent<SpriteRenderer>().sprite=GameObject.Find("ShopTab").GetComponent<SpriteRenderer>().sprite;
		}
		else if(AktivanTab==3)
		{
			GameObject.Find("ButtonCustomize").GetComponent<SpriteRenderer>().sprite=GameObject.Find("ShopTab").GetComponent<SpriteRenderer>().sprite;
		}
		else if(AktivanTab==4)
		{
			GameObject.Find("ButtonPowerUps").GetComponent<SpriteRenderer>().sprite=GameObject.Find("ShopTab").GetComponent<SpriteRenderer>().sprite;
		}
		AktivanTab=0;

		AktivanItemSesir = 998;
		AktivanItemMajica = 998;
		AktivanItemRanac = 998;
	}

	public void PozoviTab(int RedniBrojTaba)
	{
		if(mozeDaOtvoriSledeciTab && kliknuoJednomNaTab)
		{
			mozeDaOtvoriSledeciTab = false;
			kliknuoJednomNaTab = false;
			if(RedniBrojTaba == 3)
				Invoke("MozeDaKliknePonovoNaTab",1.5f);
			else
				Invoke("MozeDaKliknePonovoNaTab",0.75f);

		if(StagesParser.otvaraoShopNekad == 0)
		{
			//Debug.Log("Upadoh elegantno");
			StagesParser.otvaraoShopNekad = 1;
			PlayerPrefs.SetString("OdgledaoTutorial",StagesParser.odgledaoTutorial.ToString()+"#"+StagesParser.otvaraoShopNekad.ToString());
			PlayerPrefs.Save();
		}

		otvorenShop = true;
		CustomizationShirts.SetActive(false);
		CustomizationBackPack.SetActive(false);

		CoinsNumber.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
		CoinsNumber.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		//		AktivanCustomizationTab=1;
		if (AktivanTab != RedniBrojTaba) {
			//Debug.Log("Bem te jedan, rednibroj: "+RedniBrojTaba+" AktivanTab "+AktivanTab);
			if (AktivanTab == 1) 
			{
				DeaktivirajFreeCoins();
				GameObject.Find ("ButtonFreeCoins").GetComponent<SpriteRenderer> ().sprite = GameObject.Find ("ShopTab").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Shop/1 Free Coins").GetComponent<Animation> ().Play ("TabOdlazak");
			} else if (AktivanTab == 2) 
			{
				DeaktivirajShopTab();
				GameObject.Find ("ButtonShop").GetComponent<SpriteRenderer> ().sprite = GameObject.Find ("ShopTab").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Shop/2 Shop - BANANA").GetComponent<Animation> ().Play ("TabOdlazak");

			} else if (AktivanTab == 3) 
			{
				DeaktivirajCustomization ();
//				AktivanCustomizationTab=1;
				GameObject.Find ("ButtonCustomize").GetComponent<SpriteRenderer> ().sprite = GameObject.Find ("ShopTab").GetComponent<SpriteRenderer> ().sprite;

				GameObject.Find ("Shop/3 Customize").GetComponent<Animation> ().Play ("TabOdlazak");

				MajmunBobo.transform.position = MainScenaPozicija;
			} 
			else if (AktivanTab == 4) 
			{
				DeaktivirajPowerUps();
				GameObject.Find ("ButtonPowerUps").GetComponent<SpriteRenderer> ().sprite = GameObject.Find ("ShopTab").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Shop/4 Power-Ups").GetComponent<Animation> ().Play ("TabOdlazak");
			}
			AktivanTab = RedniBrojTaba;

			if (AktivanTab == 1) 
			{
				if(PlayerPrefs.HasKey("LikeBananaIsland"))
				{
					ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCBILikePage").GetComponent<Collider>().enabled = false;
					ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCBILikePage").GetComponent<Renderer>().material.color = new Color(0.58f,0.58f,0.58f);
					ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCBILikePage/Like BananaIsland FC").GetComponent<Renderer>().material.color = new Color(0.58f,0.58f,0.58f);
				}
				if(PlayerPrefs.HasKey("LikeWebelinx"))
				{
					ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCWLLikePage").GetComponent<Collider>().enabled = false;
					ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCWLLikePage").GetComponent<Renderer>().material.color = new Color(0.58f,0.58f,0.58f);
					ShopManagerFull.ShopObject.transform.Find("1 Free Coins/Free Coins Tabovi/ShopFCWLLikePage/Like Webelinx FC").GetComponent<Renderer>().material.color = new Color(0.58f,0.58f,0.58f);
				}
				GameObject.Find ("ButtonFreeCoins").GetComponent<SpriteRenderer> ().sprite = GameObject.Find ("ShopTabSelected").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Shop/1 Free Coins").GetComponent<Animation> ().Play ("TabDolazak");
				AktivirajFreeCoins();

			} 
			else if (AktivanTab == 2) 
			{
				AktivirajShopTab();
				GameObject.Find ("ButtonShop").GetComponent<SpriteRenderer> ().sprite = GameObject.Find ("ShopTabSelected").GetComponent<SpriteRenderer> ().sprite;
				GameObject.Find ("Shop/2 Shop - BANANA").GetComponent<Animation> ().Play ("TabDolazak");


			} else if (AktivanTab == 3) 
			{
				if (AktivanCustomizationTab == 1) 
				{
					GameObject.Find ("1HatsShopTab").GetComponent<SpriteRenderer> ().color = Color.green;
					GameObject.Find ("2TShirtsShopTab").GetComponent<SpriteRenderer> ().color = KakiBoja;
					GameObject.Find ("3BackPackShopTab").GetComponent<SpriteRenderer> ().color = KakiBoja;
					ImaNovihKapa=false;
					CustomizationHats.SetActive (true);
					CustomizationShirts.SetActive (false);
					CustomizationBackPack.SetActive (false);
				} 
				else if (AktivanCustomizationTab == 2) 
				{
					GameObject.Find ("1HatsShopTab").GetComponent<SpriteRenderer> ().color = KakiBoja;
					GameObject.Find ("2TShirtsShopTab").GetComponent<SpriteRenderer> ().color = Color.green;
					GameObject.Find ("3BackPackShopTab").GetComponent<SpriteRenderer> ().color = KakiBoja;
					ImaNovihMajica=false;
					CustomizationHats.SetActive (false);
					CustomizationShirts.SetActive (true);
					CustomizationBackPack.SetActive (false);
				} 
				else if (AktivanCustomizationTab == 3) 
				{
					GameObject.Find ("1HatsShopTab").GetComponent<SpriteRenderer> ().color = KakiBoja;
					GameObject.Find ("2TShirtsShopTab").GetComponent<SpriteRenderer> ().color = KakiBoja;
					GameObject.Find ("3BackPackShopTab").GetComponent<SpriteRenderer> ().color = Color.green;
					ImaNovihRanceva=false;
					CustomizationHats.SetActive (false);
					CustomizationShirts.SetActive (false);
					CustomizationBackPack.SetActive (true);
				}
					GameObject.Find ("ButtonCustomize").GetComponent<SpriteRenderer> ().sprite = GameObject.Find ("ShopTabSelected").GetComponent<SpriteRenderer> ().sprite;
					
					GameObject.Find ("Shop/3 Customize").GetComponent<Animation> ().Play ("TabDolazak");
			
				Invoke("AktivirajCustomization",0.4f);
				MajmunBobo.transform.Find("PrinceGorilla").GetComponent<Animator>().Play("Povlacenje");//Idle Shop
				MajmunBobo.transform.Find("ButterflyHolder").gameObject.SetActive(false);
				} 
			else if (AktivanTab == 4) 
			{
					AktivirajPowerUps();
					GameObject.Find ("ButtonPowerUps").GetComponent<SpriteRenderer> ().sprite = GameObject.Find ("ShopTabSelected").GetComponent<SpriteRenderer> ().sprite;
					GameObject.Find ("Shop/4 Power-Ups").GetComponent<Animation> ().Play ("TabDolazak");
			}
				
		} else if (RedniBrojTaba == 3) 
				{
					//Debug.Log("Tu si bem te");
				}
		}
		else if(!mozeDaOtvoriSledeciTab && kliknuoJednomNaTab)
		{
			kliknuoJednomNaTab = false;
		}
	}

	void MozeDaKliknePonovoNaTab()
	{
		mozeDaOtvoriSledeciTab = true;
		kliknuoJednomNaTab = true;
	}

	public void PozoviCustomizationTab(int RedniBrojCustomizationTaba)
	{
		StopCoroutine("CustomizationTab");
		StartCoroutine("CustomizationTab",RedniBrojCustomizationTaba); //OVDE SAM STAO!!!
	}
	public IEnumerator CustomizationTab(int RedniBrojCustomizationTaba1)
	{
		//Debug.Log ("F-ja CustomizationTab, AktivanCustomizationTab je: "+AktivanCustomizationTab+", a PozoviCustomizationTab: "+RedniBrojCustomizationTaba1);
		if(AktivanCustomizationTab!=RedniBrojCustomizationTaba1)
		{

			if(AktivanCustomizationTab==1)
			{
				GameObject.Find("1HatsShopTab").GetComponent<SpriteRenderer>().color = KakiBoja;
				CustomizationHats.SetActive(false);
				ImaNovihKapa=false;



			}
			else if(AktivanCustomizationTab==2)
			{
				GameObject.Find("2TShirtsShopTab").GetComponent<SpriteRenderer>().color = KakiBoja;
				CustomizationShirts.SetActive(false);
				ImaNovihMajica=false;



			}
			else if(AktivanCustomizationTab==3)
			{
				GameObject.Find("3BackPackShopTab").GetComponent<SpriteRenderer>().color = KakiBoja;
				CustomizationBackPack.SetActive(false);
				ImaNovihRanceva=false;


				//8.399837
			}

			yield return new WaitForSeconds(0.15f);

			AktivanCustomizationTab=RedniBrojCustomizationTaba1;

			if(AktivanCustomizationTab==1)
			{
				TrenutniSelektovanSesir=999;
				GameObject.Find("1HatsShopTab").GetComponent<SpriteRenderer>().color = Color.green;
				CustomizationHats.SetActive(true);
				ImaNovihKapa=false;

				Quaternion a = Quaternion.Euler(new Vector3(0,90,0));
				float t=0;
				while(t<0.3f)
				{
					MajmunBobo.transform.Find("PrinceGorilla").rotation = Quaternion.Lerp(MajmunBobo.transform.Find("PrinceGorilla").rotation,a,t);
					t+=Time.deltaTime/2;
					yield return null;
				}
			}
			else if(AktivanCustomizationTab==2)
			{
				TrenutnoSelektovanaMajica=999;
				GameObject.Find("2TShirtsShopTab").GetComponent<SpriteRenderer>().color = Color.green;
				CustomizationShirts.SetActive(true);
				ImaNovihMajica=false;

				Quaternion a = Quaternion.Euler(new Vector3(0,150,0));
				float t=0;
				while(t<0.3f)
				{
					MajmunBobo.transform.Find("PrinceGorilla").rotation = Quaternion.Lerp(MajmunBobo.transform.Find("PrinceGorilla").rotation,a,t);
					t+=Time.deltaTime/2;
					yield return null;
				}
			}
			else if(AktivanCustomizationTab==3)
			{
				TrenutnoSelektovanRanac=999;
				GameObject.Find("3BackPackShopTab").GetComponent<SpriteRenderer>().color = Color.green;
				CustomizationBackPack.SetActive(true);
				ImaNovihRanceva=false;

				Quaternion a = Quaternion.Euler(new Vector3(0,35,0));
				float t=0;
				while(t<0.3f)
				{
					MajmunBobo.transform.Find("PrinceGorilla").rotation = Quaternion.Lerp(MajmunBobo.transform.Find("PrinceGorilla").rotation,a,t);
					t+=Time.deltaTime/2;
					yield return null;
				}
			}

		

		}
		else
		{
			if(AktivanCustomizationTab==1)
			{
				TrenutniSelektovanSesir=999;
				GameObject.Find("1HatsShopTab").GetComponent<SpriteRenderer>().color = Color.green;
				CustomizationHats.SetActive(true);
			}
			else if(AktivanCustomizationTab==2)
			{
				TrenutnoSelektovanaMajica=999;
				GameObject.Find("2TShirtsShopTab").GetComponent<SpriteRenderer>().color = Color.green;
				CustomizationShirts.SetActive(true);
			}
			else if(AktivanCustomizationTab==3)
			{
				TrenutnoSelektovanRanac=999;
				GameObject.Find("3BackPackShopTab").GetComponent<SpriteRenderer>().color = Color.green;
				CustomizationBackPack.SetActive(true);
			}
		}

		AktivirajCustomization ();
	}

	public void AktivirajCustomization()
	{
		if(AktivanCustomizationTab==1)
		{
			ObjCustomizationShirts.CustomizationShirts=false;
			SwipeControlCustomizationShirts.controlEnabled = false;

			ObjCustomizationBackPacks.CustomizationBackPacks=false;
			SwipeControlCustomizationBackPacks.controlEnabled = false;

			ObjCustomizationHats.CustomizationHats=true;
			SwipeControlCustomizationHats.controlEnabled = true;
		}
		else if(AktivanCustomizationTab==2)
		{
			ObjCustomizationHats.CustomizationHats=false;
			SwipeControlCustomizationHats.controlEnabled = false;

			ObjCustomizationShirts.CustomizationShirts=true;
			SwipeControlCustomizationShirts.controlEnabled = true;

			ObjCustomizationBackPacks.CustomizationBackPacks=false;
			SwipeControlCustomizationBackPacks.controlEnabled = false;
		}
		else if(AktivanCustomizationTab==3)
		{
			ObjCustomizationHats.CustomizationHats=false;
			SwipeControlCustomizationHats.controlEnabled = false;

			ObjCustomizationShirts.CustomizationShirts=false;
			SwipeControlCustomizationShirts.controlEnabled = false;

			ObjCustomizationBackPacks.CustomizationBackPacks=true;
			SwipeControlCustomizationBackPacks.controlEnabled = true;
		}

		MajmunBobo.transform.position = ShopCustomizationPozicija;
	}

	public void DeaktivirajCustomization()
	{
		if(AktivanCustomizationTab==1)
		{
			ObjCustomizationHats.CustomizationHats=false;
			SwipeControlCustomizationHats.controlEnabled = false;
		}
		else if(AktivanCustomizationTab==2)
		{
			ObjCustomizationShirts.CustomizationShirts=false;
			SwipeControlCustomizationShirts.controlEnabled = false;
		}
		else if(AktivanCustomizationTab==3)
		{
			ObjCustomizationBackPacks.CustomizationBackPacks=false;
			SwipeControlCustomizationBackPacks.controlEnabled = false;
		}

	}

	public void AktivirajFreeCoins()
	{
		ObjFreeCoins.FreeCoins=true;
		SwipeControlFreeCoins.controlEnabled = true;
	}
	
	public void DeaktivirajFreeCoins()
	{
		ObjFreeCoins.FreeCoins=false;
		SwipeControlFreeCoins.controlEnabled = false;
	}

	public void AktivirajPowerUps()
	{
		ObjPowerUps.PowerUps=true;
		SwipeControlPowerUps.controlEnabled = true;
	}
	
	public void DeaktivirajPowerUps()
	{
		ObjPowerUps.PowerUps=false;
		SwipeControlPowerUps.controlEnabled = false;
	}

	public void AktivirajShopTab()
	{
		Debug.Log("Aktiviraj Shop pozvan");
		ObjShop.Shop=true;
		SwipeControlShop.controlEnabled = true;
	}
	
	public void DeaktivirajShopTab()
	{
		Debug.Log("Deaktiviraj Shop pozvan");
		ObjShop.Shop=false;
		SwipeControlShop.controlEnabled = false;
	}


	public void ProveraTrenutnogItema(int TrenutniItem)
	{
		if(AktivanCustomizationTab==1)
		{
//			//Debug.Log("Trenutni item je u tabu: " + AktivanCustomizationTab + " je "+ TrenutniItem);

			
			ProveriStanjeSesira(TrenutniItem);
		}
		else if(AktivanCustomizationTab==2)
		{
//			//Debug.Log("Trenutni item je u tabu: " + AktivanCustomizationTab + " je "+ TrenutniItem);
			ProveriStanjeMajica(TrenutniItem);
		}
		else if(AktivanCustomizationTab==3)
		{
//			//Debug.Log("Trenutni item je u tabu: " + AktivanCustomizationTab + " je "+ TrenutniItem);
			ProveriStanjeRanca(TrenutniItem);
		}


	}

	public void ProveriStanjeSesira(int TrenutniItem)
	{

		if(TrenutniItem!=TrenutniSelektovanSesir)
		{

			TrenutniSelektovanSesir=TrenutniItem;
			if(TrenutniItem < SveStvariZaOblacenjeHats.Count)
			{
				if(SveStvariZaOblacenjeHats[TrenutniSelektovanSesir]==1)
				{
					ButtonShopSprite.GetComponent<SpriteRenderer>().color=Color.white;
	//				PreviewShopButton.GetComponent<SpriteRenderer>().color=new Color(0.41176f, 0.41176f, 0.41176f);


					PreviewState=false;
					if(AktivanSesir==TrenutniItem)
					{
						//Debug.Log(TrenutniItem+" sesir je kupljen i equipovan");
						ButtonShop.GetComponent<TextMesh>().text=LanguageManager.Unequip;

						BuyButtonState=3;
					}
					else
					{
						//Debug.Log(TrenutniItem+" sesir je kupljen");
						ButtonShop.GetComponent<TextMesh>().text=LanguageManager.Equip;
						BuyButtonState=2;
					}
					ButtonShop.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
				}
				else
				{
					//Debug.Log(TrenutniItem+" sesir nije kupljen");

					PreviewState=true;
					ButtonShop.GetComponent<TextMesh>().text=LanguageManager.Buy;
					ButtonShop.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
	//				//Debug.Log("index je: "+CoinsHats.Count);
					int cena=int.Parse(CoinsHats[TrenutniItem]);
					if(ZakljucaniHats[TrenutniSelektovanSesir]==1)
					{
	//					PreviewShopButton.GetComponent<SpriteRenderer>().color=Color.white;
						if(StagesParser.currentMoney<cena)
						{
							ButtonShopSprite.GetComponent<SpriteRenderer>().color=new Color(0.41176f, 0.41176f, 0.41176f);
							BuyButtonState=1;
						}
						else
						{
							ButtonShopSprite.GetComponent<SpriteRenderer>().color=Color.white;
							BuyButtonState=0;
						}
					}
					else
					{
						BuyButtonState=4;
	//					PreviewShopButton.GetComponent<SpriteRenderer>().color=new Color(0.41176f, 0.41176f, 0.41176f);
						ButtonShopSprite.GetComponent<SpriteRenderer>().color=new Color(0.41176f, 0.41176f, 0.41176f);

					}

				}
			}
		}

	}

	public void ProveriStanjeMajica(int TrenutniItem)
	{

		if(TrenutniItem!=TrenutnoSelektovanaMajica)
		{
			TrenutnoSelektovanaMajica=TrenutniItem;
			//Debug.Log("Ukupno ima "+SveStvariZaOblacenjeShirts.Count+" a index je "+TrenutniItem);
			if(TrenutniItem < SveStvariZaOblacenjeShirts.Count)
			{
				if(SveStvariZaOblacenjeShirts[TrenutniItem]==1)
				{
					ButtonShopSprite.GetComponent<SpriteRenderer>().color=Color.white;
	//				PreviewShopButton.GetComponent<SpriteRenderer>().color=new Color(0.41176f, 0.41176f, 0.41176f);
					PreviewState=false;
					if(AktivnaMajica==TrenutniItem)
					{
						//Debug.Log(TrenutniItem+" majica je kupljena i equipovan");
						ButtonShop.GetComponent<TextMesh>().text=LanguageManager.Unequip;
						BuyButtonState=3;
					}
					else
					{
						//Debug.Log(TrenutniItem+" majica je kupljena");
						ButtonShop.GetComponent<TextMesh>().text=LanguageManager.Equip;
						BuyButtonState=2;
					}
					ButtonShop.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
				}
				else
				{
					//Debug.Log(TrenutniItem+" majica nije kupljena");
					PreviewState=true;
					ButtonShop.GetComponent<TextMesh>().text=LanguageManager.Buy;
					ButtonShop.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
	//				PreviewShopButton.GetComponent<SpriteRenderer>().color=Color.white;
					int cena=int.Parse(CoinsShirts[TrenutniItem]);
					//Debug.Log("Ukupno coina: "+StagesParser.currentMoney+" cena je: "+cena);
					if(ZakljucaniShirts[TrenutnoSelektovanaMajica]==1)
					{
	//					PreviewShopButton.GetComponent<SpriteRenderer>().color=Color.white;
						if(StagesParser.currentMoney<cena)
						{
							ButtonShopSprite.GetComponent<SpriteRenderer>().color=new Color(0.41176f, 0.41176f, 0.41176f);
							BuyButtonState=1;
						}
						else
						{
							ButtonShopSprite.GetComponent<SpriteRenderer>().color=Color.white;
							BuyButtonState=0;
						}
					}
					else
					{
						BuyButtonState=4;
	//					PreviewShopButton.GetComponent<SpriteRenderer>().color=new Color(0.41176f, 0.41176f, 0.41176f);
						ButtonShopSprite.GetComponent<SpriteRenderer>().color=new Color(0.41176f, 0.41176f, 0.41176f);
					}

				}
			}
		}

	}

	public void ProveriStanjeRanca(int TrenutniItem)
	{

		if(TrenutniItem!=TrenutnoSelektovanRanac)
		{
			TrenutnoSelektovanRanac=TrenutniItem;
			//Debug.Log("Ukupno ima "+SveStvariZaOblacenjeBackPack.Count+" a index je "+TrenutniItem);
			if(TrenutniItem < SveStvariZaOblacenjeBackPack.Count)
			{
				if(SveStvariZaOblacenjeBackPack[TrenutniItem]==1)
				{
					ButtonShopSprite.GetComponent<SpriteRenderer>().color=Color.white;
	//				PreviewShopButton.GetComponent<SpriteRenderer>().color=new Color(0.41176f, 0.41176f, 0.41176f);
					PreviewState=false;
					if(AktivanRanac==TrenutniItem)
					{
						//Debug.Log(TrenutniItem+" ranac je kupljen i equipovan");
						ButtonShop.GetComponent<TextMesh>().text=LanguageManager.Unequip;
						BuyButtonState=3;
					}
					else
					{
						//Debug.Log(TrenutniItem+" ranac je kupljen");
						ButtonShop.GetComponent<TextMesh>().text=LanguageManager.Equip;
						BuyButtonState=2;
					}
					ButtonShop.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
				}
				else
				{
					//Debug.Log(TrenutniItem+" ranac nije kupljen");
					PreviewState=true;
					ButtonShop.GetComponent<TextMesh>().text=LanguageManager.Buy;
					ButtonShop.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
	//				PreviewShopButton.GetComponent<SpriteRenderer>().color=Color.white;
					int cena=int.Parse(CoinsBackPacks[TrenutniItem]);
					if(ZakljucaniBackPacks[TrenutnoSelektovanRanac]==1)
					{
	//					PreviewShopButton.GetComponent<SpriteRenderer>().color=Color.white;
						if(StagesParser.currentMoney<cena)
						{
							ButtonShopSprite.GetComponent<SpriteRenderer>().color=new Color(0.41176f, 0.41176f, 0.41176f);
							BuyButtonState=1;
						}
						else
						{
							ButtonShopSprite.GetComponent<SpriteRenderer>().color=Color.white;
							BuyButtonState=0;
						}
					}
					else
					{
						BuyButtonState=4;
	//					PreviewShopButton.GetComponent<SpriteRenderer>().color=new Color(0.41176f, 0.41176f, 0.41176f);
						ButtonShopSprite.GetComponent<SpriteRenderer>().color=new Color(0.41176f, 0.41176f, 0.41176f);
					}

				}
			}
		}


	}

	public void ProveriStanjeCelogShopa()
	{
		for(int i=0;i<BrojItemaShopHats;i++)
		{
			
			if(ZakljucaniHats[i]==1)
			{
				HatsObjects[i].Find("Zakkljucano").gameObject.SetActive(false);
				
			}
			else
			{
				HatsObjects[i].Find("Polje za unos COINA U shopu - Shop").gameObject.SetActive(false);
				HatsObjects[i].Find("Bedz - Popust").gameObject.SetActive(false);
			}
			

			if(SveStvariZaOblacenjeHats[i]==1) // Kupljen je
			{
					HatsObjects[i].Find("Polje za unos COINA U shopu - Shop").gameObject.SetActive(false);
					HatsObjects[i].Find("Bedz - Popust").gameObject.SetActive(false);
					
			}
			else
			{
					
					
					
					if(PopustHats[i]=="0")
					{
						
						HatsObjects[i].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text=CoinsHats[i];
					HatsObjects[i].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						if(i>StariBrojOtkljucanihKapa & i<=BrojOtkljucanihKapa)
						{
							//Debug.Log("Kapa sa indeksom "+i+" je novi element posto je "+i+">"+StariBrojOtkljucanihKapa+" i "+i+"<"+BrojOtkljucanihKapa);
							ImaNovihKapa = true;
							
						HatsObjects[i].Find("Bedz - Popust/Text/Number").GetComponent<TextMesh>().text=LanguageManager.New;
						HatsObjects[i].Find("Bedz - Popust/Text/Number/Number Shadow").GetComponent<TextMesh>().text=LanguageManager.New;
						}
						else
						{
							//Debug.Log("Kapa sa indeksom "+i+" nije novi element");
							HatsObjects[i].Find("Bedz - Popust").gameObject.SetActive(false);
						}
					}
					else
					{
						if(i>StariBrojOtkljucanihKapa & i<=BrojOtkljucanihKapa)
						{
							//Debug.Log("Kapa sa indeksom "+i+" je novi element posto je "+i+">"+StariBrojOtkljucanihKapa+" i "+i+"<"+BrojOtkljucanihKapa );
							ImaNovihKapa = true;
							
						HatsObjects[i].Find("Bedz - Popust/Text/Number").GetComponent<TextMesh>().text=LanguageManager.New;
						HatsObjects[i].Find("Bedz - Popust/Text/Number/Number Shadow").GetComponent<TextMesh>().text=LanguageManager.New;
							
						}
						else
						{
							//Debug.Log("Kapa sa indeksom "+i+" nije novi element");
							HatsObjects[i].Find("Bedz - Popust/Text/Number").GetComponent<TextMesh>().text="-"+PopustHats[i]+"%";
							HatsObjects[i].Find("Bedz - Popust/Text/Number/Number Shadow").GetComponent<TextMesh>().text=PopustHats[i]+"%";
						}
						
						string popust="0."+PopustHats[i];


					if(ZakljucaniHats[i] == 1)
					{
						float staraCena = float.Parse(CoinsHats[i])/(1-float.Parse(popust));
						HatsObjects[i].Find("Polje za unos COINA U shopu - Shop_NoDiscount").gameObject.SetActive(true);
						HatsObjects[i].Find("Polje za unos COINA U shopu - Shop_NoDiscount/Coins Number").GetComponent<TextMesh>().text=staraCena.ToString();
						if(HatsObjects[i].parent.gameObject.activeSelf)
							HatsObjects[i].Find("Polje za unos COINA U shopu - Shop_NoDiscount/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						HatsObjects[i].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text=CoinsHats[i];
						HatsObjects[i].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						HatsObjects[i].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().color=PopustBoja;
					}
					
					//						float cena=float.Parse(CoinsHats[i])-float.Parse(CoinsHats[i])*float.Parse(popust);
//						CoinsHats[i]=cena.ToString();
						
					}
			}
				
				
				HatsObjects[i].Find("Text/ime").GetComponent<TextMesh>().text=ImenaHats[i];
				HatsObjects[i].Find("Text/ime").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
				
				
				//Debug.Log("Ime itema broj "+i+" je :"+ImenaHats[i]+" coini: "+CoinsHats[i]+" banana: "+BananaHats[i]);
			
		}
		
		for(int j=0;j<BrojItemaShopShirts;j++)
		{
			
			if(ZakljucaniShirts[j]==1)
			{
				ShirtsObjects[j].Find("Zakkljucano").gameObject.SetActive(false);
			}
			else
			{
				ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop").gameObject.SetActive(false);
				ShirtsObjects[j].Find("Bedz - Popust").gameObject.SetActive(false);
			}
			

				if(SveStvariZaOblacenjeShirts[j]==1)
				{
					ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop").gameObject.SetActive(false);
					ShirtsObjects[j].Find("Bedz - Popust").gameObject.SetActive(false);
					
				}
				else
				{
					if(PopustShirts[j]=="0")
					{
						ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text=CoinsShirts[j];
						ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						if(j>StariBrojOtkljucanihMajici & j<=BrojOtkljucanihMajici)
						{
							//Debug.Log("Majica sa indeksom "+j+" je novi element posto je "+j+">"+StariBrojOtkljucanihMajici+" i "+j+"<"+BrojOtkljucanihMajici);
							ImaNovihMajica = true;

						ShirtsObjects[j].Find("Bedz - Popust/Text/Number").GetComponent<TextMesh>().text=LanguageManager.New;
						ShirtsObjects[j].Find("Bedz - Popust/Text/Number/Number Shadow").GetComponent<TextMesh>().text=LanguageManager.New;
						}
						else
						{
							//Debug.Log("Majica sa indeksom "+j+" nije novi element");
							ShirtsObjects[j].Find("Bedz - Popust").gameObject.SetActive(false);
						}
					}
					else
					{
						if(j>StariBrojOtkljucanihMajici & j<=BrojOtkljucanihMajici)
						{
							//Debug.Log("Majica sa indeksom "+j+" je novi element posto je "+j+">"+StariBrojOtkljucanihMajici+" i "+j+"<"+BrojOtkljucanihMajici );
							ImaNovihMajica = true;

						ShirtsObjects[j].Find("Bedz - Popust/Text/Number").GetComponent<TextMesh>().text=LanguageManager.New;
						ShirtsObjects[j].Find("Bedz - Popust/Text/Number/Number Shadow").GetComponent<TextMesh>().text=LanguageManager.New;
							
						}
						else
						{
							//Debug.Log("Kapa sa indeksom "+j+" nije novi element");
							ShirtsObjects[j].Find("Bedz - Popust/Text/Number").GetComponent<TextMesh>().text="-"+PopustShirts[j]+"%";
							ShirtsObjects[j].Find("Bedz - Popust/Text/Number/Number Shadow").GetComponent<TextMesh>().text=PopustShirts[j]+"%";
						}
						
						string popust="0."+PopustShirts[j];

					if(ZakljucaniShirts[j] == 1)
					{
						float staraCena = float.Parse(CoinsShirts[j])/(1-float.Parse(popust));
						ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop_NoDiscount").gameObject.SetActive(true);
						ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop_NoDiscount/Coins Number").GetComponent<TextMesh>().text=staraCena.ToString();
						if(ShirtsObjects[j].parent.gameObject.activeSelf)
							ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop_NoDiscount/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text=CoinsShirts[j];
						ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						ShirtsObjects[j].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().color=PopustBoja;
					}

//						float cena=float.Parse(CoinsShirts[j])-float.Parse(CoinsShirts[j])*float.Parse(popust);
//						CoinsShirts[j]=cena.ToString();
						
					}
				}
				ShirtsObjects[j].Find("Text/ime").GetComponent<TextMesh>().text=ImenaShirts[j];
				ShirtsObjects[j].Find("Text/ime").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
				//Debug.Log("Ime itema broj "+j+" je :"+ImenaShirts[j]);
			
		}
		
		for(int k=0;k<BrojItemaShopBackPack;k++)
		{
			
			if(ZakljucaniBackPacks[k]==1)
			{
				BackPacksObjects[k].Find("Zakkljucano").gameObject.SetActive(false);
			}
			else
			{
				BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop").gameObject.SetActive(false);
				BackPacksObjects[k].Find("Bedz - Popust").gameObject.SetActive(false);
			}
			

				if(SveStvariZaOblacenjeBackPack[k]==1)
				{
					BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop").gameObject.SetActive(false);
					BackPacksObjects[k].Find("Bedz - Popust").gameObject.SetActive(false);
					
				}
				else
				{
					
					if(PopustBackPacks[k]=="0")
					{
						BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text=CoinsBackPacks[k];
						BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						if(k>StariBrojOtkljucanihRanceva & k<=BrojOtkljucanihRanceva)
						{
							//Debug.Log("Ranac sa indeksom "+k+" je novi element posto je "+k+">"+StariBrojOtkljucanihRanceva+" i "+k+"<"+BrojOtkljucanihRanceva);
							ImaNovihRanceva = true;
						BackPacksObjects[k].Find("Bedz - Popust/Text/Number").GetComponent<TextMesh>().text=LanguageManager.New;
						BackPacksObjects[k].Find("Bedz - Popust/Text/Number/Number Shadow").GetComponent<TextMesh>().text=LanguageManager.New;
						}
						else
						{
							//Debug.Log("Ranac sa indeksom "+k+" nije novi element");
							BackPacksObjects[k].Find("Bedz - Popust").gameObject.SetActive(false);
						}
					}
					else
					{
						BackPacksObjects[k].Find("Bedz - Popust/Text/Number").GetComponent<TextMesh>().text="-"+PopustBackPacks[k]+"%";
						BackPacksObjects[k].Find("Bedz - Popust/Text/Number/Number Shadow").GetComponent<TextMesh>().text=PopustBackPacks[k]+"%";
						
						if(k>StariBrojOtkljucanihRanceva & k<=BrojOtkljucanihRanceva)
						{
							//Debug.Log("Ranac sa indeksom "+k+" je novi element posto je "+k+">"+StariBrojOtkljucanihRanceva+" i "+k+"<"+BrojOtkljucanihRanceva );
							ImaNovihRanceva = true;
						BackPacksObjects[k].Find("Bedz - Popust/Text/Number").GetComponent<TextMesh>().text=LanguageManager.New;
						BackPacksObjects[k].Find("Bedz - Popust/Text/Number/Number Shadow").GetComponent<TextMesh>().text=LanguageManager.New;
							
						}
						else
						{
							//Debug.Log("Ranac sa indeksom "+k+" nije novi element");
							BackPacksObjects[k].Find("Bedz - Popust/Text/Number").GetComponent<TextMesh>().text="-"+PopustBackPacks[k]+"%";
							BackPacksObjects[k].Find("Bedz - Popust/Text/Number/Number Shadow").GetComponent<TextMesh>().text=PopustBackPacks[k]+"%";
						}
						
						string popust="0."+PopustBackPacks[k];



					if(ZakljucaniBackPacks[k] == 1)
					{
						float staraCena = float.Parse(CoinsBackPacks[k])/(1-float.Parse(popust));
						BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop_NoDiscount").gameObject.SetActive(true);
						BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop_NoDiscount/Coins Number").GetComponent<TextMesh>().text=staraCena.ToString();
						if(BackPacksObjects[k].parent.gameObject.activeSelf)
							BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop_NoDiscount/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().text=CoinsBackPacks[k];
						BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
						BackPacksObjects[k].Find("Polje za unos COINA U shopu - Shop/Coins Number").GetComponent<TextMesh>().color=PopustBoja;
					}
//						float cena=float.Parse(CoinsBackPacks[k])-float.Parse(CoinsBackPacks[k])*float.Parse(popust);
//						CoinsBackPacks[k]=cena.ToString();
						
					}
				}
				
				BackPacksObjects[k].Find("Text/ime").GetComponent<TextMesh>().text=ImenaBackPacks[k];
				BackPacksObjects[k].Find("Text/ime").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
				
				//Debug.Log("Ime itema broj "+k+" je :"+ImenaBackPacks[k]);
			
			
		}

		if(ImaNovihKapa)
		{
			ZidFooter.transform.Find("1HatsShopTab/Znak Uzvika telo").gameObject.SetActive(true);
		}
		else
		{
			ZidFooter.transform.Find("1HatsShopTab/Znak Uzvika telo").gameObject.SetActive(false);
		}
		
		if(ImaNovihMajica)
		{
			ZidFooter.transform.Find("2TShirtsShopTab/Znak Uzvika telo").gameObject.SetActive(true);
		}
		else
		{
			ZidFooter.transform.Find("2TShirtsShopTab/Znak Uzvika telo").gameObject.SetActive(false);
		}
		
		if(ImaNovihRanceva)
		{
			ZidFooter.transform.Find("3BackPackShopTab/Znak Uzvika telo").gameObject.SetActive(true);
		}
		else
		{
			ZidFooter.transform.Find("3BackPackShopTab/Znak Uzvika telo").gameObject.SetActive(false);
		}

		if(ImaNovihKapa | ImaNovihMajica | ImaNovihRanceva)
		{
			Custumization.transform.Find("Znak Uzvika telo").gameObject.SetActive(true);
		}
		else
		{
			Custumization.transform.Find("Znak Uzvika telo").gameObject.SetActive(false);
		}
	}

	public void KupiItem()
	{
		if(BuyButtonState==0)
		{
			//Debug.Log("Moze da kupi");
			if(AktivanCustomizationTab==1)
			{
				StagesParser.currentMoney = StagesParser.currentMoney - int.Parse(CoinsHats[AktivanItemSesir]);
				SveStvariZaOblacenjeHats[AktivanItemSesir]=1;
				HatsObjects[AktivanItemSesir].Find("Stikla").gameObject.SetActive(true);
				HatsObjects[AktivanItemSesir].Find("Polje za unos COINA U shopu - Shop").gameObject.SetActive(false);
				HatsObjects[AktivanItemSesir].Find("Polje za unos COINA U shopu - Shop_NoDiscount").gameObject.SetActive(false);
				HatsObjects[AktivanItemSesir].Find("Bedz - Popust").gameObject.SetActive(false);
				TrenutniSelektovanSesir=-1;
				ProveraTrenutnogItema(AktivanItemSesir);
				FacebookManager.UserSveKupovineHats="";
				for(int i=0;i<SveStvariZaOblacenjeHats.Count;i++)
				{
					//Debug.Log(SveStvariZaOblacenjeHats[i]);
					FacebookManager.UserSveKupovineHats = string.Concat(FacebookManager.UserSveKupovineHats, SveStvariZaOblacenjeHats[i]+"#");
				}
				StagesParser.svekupovineGlava = FacebookManager.UserSveKupovineHats;
				PlayerPrefs.SetString("UserSveKupovineHats", FacebookManager.UserSveKupovineHats);
				PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
				PlayerPrefs.Save();
			}
			else if(AktivanCustomizationTab==2)
			{
				StagesParser.currentMoney = StagesParser.currentMoney - int.Parse(CoinsShirts[AktivanItemMajica]);
				SveStvariZaOblacenjeShirts[AktivanItemMajica]=1;
				ShirtsObjects[AktivanItemMajica].Find("Stikla").gameObject.SetActive(true);
				ShirtsObjects[AktivanItemMajica].Find("Polje za unos COINA U shopu - Shop").gameObject.SetActive(false);
				ShirtsObjects[AktivanItemMajica].Find("Polje za unos COINA U shopu - Shop_NoDiscount").gameObject.SetActive(false);
				ShirtsObjects[AktivanItemMajica].Find("Bedz - Popust").gameObject.SetActive(false);
				TrenutnoSelektovanaMajica=-1;
				ProveraTrenutnogItema(AktivanItemMajica);

				FacebookManager.UserSveKupovineShirts="";
				for(int i=0;i<SveStvariZaOblacenjeShirts.Count;i++)
				{
					//Debug.Log(SveStvariZaOblacenjeShirts[i]);
					FacebookManager.UserSveKupovineShirts = string.Concat(FacebookManager.UserSveKupovineShirts, SveStvariZaOblacenjeShirts[i]+"#");
					
				}
				StagesParser.svekupovineMajica = FacebookManager.UserSveKupovineShirts;
				PlayerPrefs.SetString("UserSveKupovineShirts", FacebookManager.UserSveKupovineShirts);
				PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
				PlayerPrefs.Save();
			}
			if(AktivanCustomizationTab==3)
			{
				StagesParser.currentMoney = StagesParser.currentMoney - int.Parse(CoinsBackPacks[AktivanItemRanac]);
				SveStvariZaOblacenjeBackPack[AktivanItemRanac]=1;
				BackPacksObjects[AktivanItemRanac].Find("Stikla").gameObject.SetActive(true);
				BackPacksObjects[AktivanItemRanac].Find("Polje za unos COINA U shopu - Shop").gameObject.SetActive(false);
				BackPacksObjects[AktivanItemRanac].Find("Polje za unos COINA U shopu - Shop_NoDiscount").gameObject.SetActive(false);
				BackPacksObjects[AktivanItemRanac].Find("Bedz - Popust").gameObject.SetActive(false);
				TrenutnoSelektovanRanac=-1;
				ProveraTrenutnogItema(AktivanItemRanac);

				FacebookManager.UserSveKupovineBackPacks="";
				for(int i=0;i<SveStvariZaOblacenjeBackPack.Count;i++)
				{
					//Debug.Log(SveStvariZaOblacenjeShirts[i]);
					FacebookManager.UserSveKupovineBackPacks = string.Concat(FacebookManager.UserSveKupovineBackPacks, SveStvariZaOblacenjeBackPack[i]+"#");
					
				}
				StagesParser.svekupovineLedja = FacebookManager.UserSveKupovineBackPacks;
				PlayerPrefs.SetString("UserSveKupovineBackPacks", FacebookManager.UserSveKupovineBackPacks);
				PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
				PlayerPrefs.Save();
			}
			StagesParser.ServerUpdate = 1;
			CoinsNumber.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
			CoinsNumber.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
		}
		else if(BuyButtonState==1)
		{
			//Debug.Log("Ne moze da kupi poziva se animacija");
			CoinsNumber.GetComponent<Animation>().Play("Not Enough Coins");
		}
		else if(BuyButtonState==2)
		{
			//Debug.Log("Equip");
			if(AktivanCustomizationTab==1)
			{
				if(PreviewSesir!=-1)
				{
					MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/Chest/Neck/Head/"+PreviewSesir).transform.GetChild(0).gameObject.SetActive(false);
					PreviewSesir = -1;
				}
				if(int.Parse(UsiHats[AktivanItemSesir])==1)
				{
					if(!ImaUsi)
					{
						ImaUsi=true;
						MajmunBobo.transform.Find("PrinceGorilla/Usi").gameObject.SetActive(true);
					}
				}
				else
				{
					if(ImaUsi)
					{
						ImaUsi=false;
						MajmunBobo.transform.Find("PrinceGorilla/Usi").gameObject.SetActive(false);
					}

				}

				if(int.Parse(KosaHats[AktivanItemSesir])==1)
				{


						ImaKosu=true;
						MajmunBobo.transform.Find("PrinceGorilla/Kosa").gameObject.SetActive(true);
					
				}
				else
				{

						ImaKosu=false;
						MajmunBobo.transform.Find("PrinceGorilla/Kosa").gameObject.SetActive(false);

				}
				if(AktivanSesir!=-1)
				{
					MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/Chest/Neck/Head/"+AktivanSesir).transform.GetChild(0).gameObject.SetActive(false);
				}

				MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/Chest/Neck/Head/"+AktivanItemSesir).transform.GetChild(0).gameObject.SetActive(true);
				AktivanSesir=AktivanItemSesir;
				TrenutniSelektovanSesir=-1;
				ProveraTrenutnogItema(AktivanItemSesir);
				//Debug.Log ("Upali ga beeeeee sesir "+AktivanItemSesir+" ime mu je "+MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/Chest/Neck/Head/"+AktivanItemSesir).transform.GetChild(0).name);
				//Debug.Log("ULEDJIVAC KUPIO HELMET!!!");
				StagesParser.glava = AktivanSesir;
				StagesParser.imaKosu = ImaKosu;
				StagesParser.imaUsi = ImaUsi;
			}
			else if(AktivanCustomizationTab==2)
			{
				MajmunBobo.transform.Find("PrinceGorilla/custom_Majica").gameObject.SetActive(true);
				Texture MajicaTekstura = Resources.Load("Majice/Bg"+AktivanItemMajica) as Texture;
				MajmunBobo.transform.Find("PrinceGorilla/custom_Majica").GetComponent<Renderer>().material.SetTexture("_MainTex", MajicaTekstura);
				MajmunBobo.transform.Find("PrinceGorilla/custom_Majica").GetComponent<Renderer>().material.color = TShirtColors[AktivanItemMajica];
				AktivnaMajica=AktivanItemMajica;
				TrenutnoSelektovanaMajica=-1;
				ProveraTrenutnogItema(AktivanItemMajica);
				//Debug.Log("ULEDJIVAC KUPIO MAJICU!!!");
				StagesParser.majica = AktivnaMajica;
				StagesParser.bojaMajice = TShirtColors[AktivnaMajica];
			}
			if(AktivanCustomizationTab==3)
			{
//				if(PreviewMajica!=-1)
//				{
//					MajmunBobo.transform.Find("PrinceGorilla/custom_Majica").gameObject.SetActive(false);
//					PreviewMajica = -1;
//				}
				if(PreviewRanac!=-1)
				{
					MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/"+PreviewRanac).transform.GetChild(0).gameObject.SetActive(false);
					PreviewRanac = -1;
				}
				if(AktivanRanac!=-1)
				{
					MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/"+AktivanRanac).transform.GetChild(0).gameObject.SetActive(false);
				}
				MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/"+AktivanItemRanac).transform.GetChild(0).gameObject.SetActive(true);
				AktivanRanac=AktivanItemRanac;
				TrenutnoSelektovanRanac=-1;
				ProveraTrenutnogItema(AktivanItemRanac);
				//Debug.Log("ULEDJIVAC KUPIO RANAC!!!");
				StagesParser.ledja = AktivanRanac;
			}
			string niz=AktivanSesir.ToString()+"#"+AktivnaMajica.ToString()+"#"+AktivanRanac.ToString();
			PlayerPrefs.SetString("AktivniItemi", niz);
			PlayerPrefs.Save();





		}
		else if(BuyButtonState==3)
		{
			//Debug.Log("UnEquip");
			if(AktivanCustomizationTab==1)
			{

				ImaUsi=true;
				MajmunBobo.transform.Find("PrinceGorilla/Usi").gameObject.SetActive(true);

				ImaKosu=true;
				MajmunBobo.transform.Find("PrinceGorilla/Kosa").gameObject.SetActive(true);
				
				
				MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/Chest/Neck/Head/"+AktivanItemSesir).transform.GetChild(0).gameObject.SetActive(false);
				AktivanSesir=-1;
				TrenutniSelektovanSesir=-1;
				ProveraTrenutnogItema(AktivanItemSesir);
				//Debug.Log ("Ugasi ga beeeeee");

				StagesParser.imaKosu = true;
				StagesParser.imaUsi = true;
				StagesParser.glava = -1;
			}
			else if(AktivanCustomizationTab==2)
			{
				MajmunBobo.transform.Find("PrinceGorilla/custom_Majica").gameObject.SetActive(false);
				AktivnaMajica=-1;
				TrenutnoSelektovanaMajica=-1;
				ProveraTrenutnogItema(AktivanItemMajica);

				StagesParser.majica = -1;
			}
			if(AktivanCustomizationTab==3)
			{
				MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/"+AktivanItemRanac).transform.GetChild(0).gameObject.SetActive(false);
				AktivanRanac=-1;
				TrenutnoSelektovanRanac=-1;
				ProveraTrenutnogItema(AktivanItemRanac);

				StagesParser.ledja = -1;
			}
			string niz=AktivanSesir.ToString()+"#"+AktivnaMajica.ToString()+"#"+AktivanRanac.ToString();
			PlayerPrefs.SetString("AktivniItemi", niz);
			PlayerPrefs.Save();
		}
		if(BuyButtonState==4)
		{
			//Debug.Log("Item je zakljucan");
		}

	}

	public void PreviewItem()
	{
		if(AktivanCustomizationTab==1)
		{
			if(ZakljucaniHats[AktivanItemSesir]==1)
			{
				if(int.Parse(UsiHats[AktivanItemSesir])==1)
				{
					if(!ImaUsi)
					{
						ImaUsi=true;
						MajmunBobo.transform.Find("PrinceGorilla/Usi").gameObject.SetActive(true);
					}
				}
				else
				{
					if(ImaUsi)
					{
						ImaUsi=false;
						MajmunBobo.transform.Find("PrinceGorilla/Usi").gameObject.SetActive(false);
					}
					
				}
				
				if(int.Parse(KosaHats[AktivanItemSesir])==1)
				{
					
					
					ImaKosu=true;
					MajmunBobo.transform.Find("PrinceGorilla/Kosa").gameObject.SetActive(true);
					
				}
				else
				{
					
					ImaKosu=false;
					MajmunBobo.transform.Find("PrinceGorilla/Kosa").gameObject.SetActive(false);
					
				}

				if(PreviewSesir!=-1)
				{
					MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/Chest/Neck/Head/"+PreviewSesir).transform.GetChild(0).gameObject.SetActive(false);
				}

				if(AktivanSesir!=-1)
				{
					MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/Chest/Neck/Head/"+AktivanSesir).transform.GetChild(0).gameObject.SetActive(false);
				}
				
				MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/Chest/Neck/Head/"+AktivanItemSesir).transform.GetChild(0).gameObject.SetActive(true);
				PreviewSesir=AktivanItemSesir;
				TrenutniSelektovanSesir=-1;
				ProveraTrenutnogItema(PreviewSesir);
			}
		}
		else if(AktivanCustomizationTab==2)
		{
			if(ZakljucaniShirts[AktivanItemMajica]==1)
			{
				MajmunBobo.transform.Find("PrinceGorilla/custom_Majica").gameObject.SetActive(true);
				Texture MajicaTekstura = Resources.Load("Majice/Bg"+AktivanItemMajica) as Texture;
				MajmunBobo.transform.Find("PrinceGorilla/custom_Majica").GetComponent<Renderer>().material.SetTexture("_MainTex", MajicaTekstura);
				MajmunBobo.transform.Find("PrinceGorilla/custom_Majica").GetComponent<Renderer>().material.color = TShirtColors[AktivanItemMajica];
				PreviewMajica=AktivanItemMajica;
				TrenutnoSelektovanaMajica=-1;
				ProveraTrenutnogItema(PreviewMajica);
			}
		}
		if(AktivanCustomizationTab==3)
		{
			if(ZakljucaniBackPacks[AktivanItemRanac]==1)
			{
				if(PreviewRanac!=-1)
				{
					MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/"+PreviewRanac).transform.GetChild(0).gameObject.SetActive(false);
				}

				if(AktivanRanac!=-1)
				{
					MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/"+AktivanRanac).transform.GetChild(0).gameObject.SetActive(false);
				}
				MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/"+AktivanItemRanac).transform.GetChild(0).gameObject.SetActive(true);
				PreviewRanac=AktivanItemRanac;
				TrenutnoSelektovanRanac=-1;
				ProveraTrenutnogItema(PreviewRanac);
			}
		}
	}

	public void KupiDoubleCoins()
	{
		if(StagesParser.currentMoney<int.Parse(CoinsPowerUps[0]))
		{
			CoinsNumber.GetComponent<Animation>().Play("Not Enough Coins");
		}
		else
		{
			StagesParser.currentMoney = StagesParser.currentMoney - int.Parse(CoinsPowerUps[0]);
			StagesParser.powerup_doublecoins++;
			GameObject.Find("Double Coins Number").GetComponent<Animation>().Play ("BoughtPowerUp");
			CoinsNumber.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
			CoinsNumber.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			GameObject.Find ("Double Coins Number/Number").GetComponent<TextMesh> ().text = StagesParser.powerup_doublecoins.ToString ();
			GameObject.Find ("Double Coins Number/Number").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);

			PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
			PlayerPrefs.SetString("PowerUps",(StagesParser.powerup_doublecoins+"#"+StagesParser.powerup_magnets+"#"+StagesParser.powerup_shields));
			PlayerPrefs.Save();
			StagesParser.ServerUpdate = 1;
		}   
	}

	public void KupiMagnet()
	{
		if(StagesParser.currentMoney<int.Parse(CoinsPowerUps[1]))
		{
			CoinsNumber.GetComponent<Animation>().Play("Not Enough Coins");
		}
		else
		{
			StagesParser.currentMoney = StagesParser.currentMoney - int.Parse(CoinsPowerUps[1]);
			StagesParser.powerup_magnets++;
			GameObject.Find("Magnet Number").GetComponent<Animation>().Play ("BoughtPowerUp");
			CoinsNumber.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
			CoinsNumber.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			GameObject.Find ("Magnet Number/Number").GetComponent<TextMesh> ().text = StagesParser.powerup_magnets.ToString ();
			GameObject.Find ("Magnet Number/Number").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);		

			PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
			PlayerPrefs.SetString("PowerUps",(StagesParser.powerup_doublecoins+"#"+StagesParser.powerup_magnets+"#"+StagesParser.powerup_shields));
			PlayerPrefs.Save();
			StagesParser.ServerUpdate = 1;
		}
	}

	public void KupiShield()
	{
		if(StagesParser.currentMoney<int.Parse(CoinsPowerUps[2]))
		{
			CoinsNumber.GetComponent<Animation>().Play("Not Enough Coins");
		}
		else
		{
			StagesParser.currentMoney = StagesParser.currentMoney - int.Parse(CoinsPowerUps[2]);
			StagesParser.powerup_shields++;
			GameObject.Find("Shield Number").GetComponent<Animation>().Play ("BoughtPowerUp");
			CoinsNumber.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
			CoinsNumber.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			GameObject.Find ("Shield Number/Number").GetComponent<TextMesh> ().text = StagesParser.powerup_shields.ToString ();
			GameObject.Find ("Shield Number/Number").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);		

			PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
			PlayerPrefs.SetString("PowerUps",(StagesParser.powerup_doublecoins+"#"+StagesParser.powerup_magnets+"#"+StagesParser.powerup_shields));
			PlayerPrefs.Save();
			StagesParser.ServerUpdate = 1;
		}
	}

	public void KupiBananu()
	{
		if(StagesParser.currentMoney<StagesParser.bananaCost)
		{
			CoinsNumber.GetComponent<Animation>().Play("Not Enough Coins");
		}
		else
		{
			StagesParser.currentMoney = StagesParser.currentMoney - StagesParser.bananaCost;
			StagesParser.currentBananas++;
			GameObject.Find("Shop/2 Shop - BANANA/Zid Shop/Zid Header i Footer/Zid Footer Shop/Banana Number").GetComponent<Animation>().Play ("BoughtPowerUp");
			CoinsNumber.transform.Find("Coins Number").GetComponent<TextMesh>().text = StagesParser.currentMoney.ToString();
			CoinsNumber.transform.Find("Coins Number").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			GameObject.Find ("Shop/2 Shop - BANANA/Zid Shop/Zid Header i Footer/Zid Footer Shop/Banana Number/Number").GetComponent<TextMesh> ().text = StagesParser.currentBananas.ToString ();
			GameObject.Find ("Shop/2 Shop - BANANA/Zid Shop/Zid Header i Footer/Zid Footer Shop/Banana Number/Number").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);		
			PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
			PlayerPrefs.SetInt("TotalBananas",StagesParser.currentBananas);
			PlayerPrefs.Save();
			StagesParser.ServerUpdate = 1;
		}
	}
	public void OcistiPreview()
	{
		//Debug.Log ("AktivanSesir "+AktivanSesir+" PreviewSesir "+PreviewSesir+" AktivnaMajica "+AktivnaMajica+" PreviewMajica "+PreviewMajica+" AktivanRanac "+AktivanRanac+" PreviewRanac "+PreviewRanac);

		if(PreviewSesir!=-1)
		{
			ImaUsi=true;
			MajmunBobo.transform.Find("PrinceGorilla/Usi").gameObject.SetActive(true);
			
			ImaKosu=true;
			MajmunBobo.transform.Find("PrinceGorilla/Kosa").gameObject.SetActive(true);

			MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/Chest/Neck/Head/"+PreviewSesir).transform.GetChild(0).gameObject.SetActive(false);
			PreviewSesir=-1;
		}

		if(PreviewMajica!=-1)
		{
			MajmunBobo.transform.Find("PrinceGorilla/custom_Majica").gameObject.SetActive(false);
			PreviewMajica=-1;
		}

		if(PreviewRanac!=-1)
		{
			MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/"+PreviewRanac).transform.GetChild(0).gameObject.SetActive(false);
			PreviewRanac=-1;
		}

		
		if(AktivanCustomizationTab==1)
		{
			TrenutniSelektovanSesir=-1;
			ProveraTrenutnogItema(AktivanItemSesir);
		}
		else if(AktivanCustomizationTab==2)
		{
			TrenutnoSelektovanaMajica=-1;
			ProveraTrenutnogItema(AktivanItemMajica);
		}
		else if(AktivanCustomizationTab==3)
		{
			TrenutnoSelektovanRanac=-1;
			ProveraTrenutnogItema(AktivanItemRanac);
		}

		if(AktivanSesir!=-1)
		{
			if(int.Parse(UsiHats[AktivanSesir])==1)
			{
				if(!ImaUsi)
				{
					ImaUsi=true;
					MajmunBobo.transform.Find("PrinceGorilla/Usi").gameObject.SetActive(true);
				}
			}
			else
			{
				if(ImaUsi)
				{
					ImaUsi=false;
					MajmunBobo.transform.Find("PrinceGorilla/Usi").gameObject.SetActive(false);
				}
				
			}
			
			if(int.Parse(KosaHats[AktivanSesir])==1)
			{
				
				
				ImaKosu=true;
				MajmunBobo.transform.Find("PrinceGorilla/Kosa").gameObject.SetActive(true);
				
			}
			else
			{
				
				ImaKosu=false;
				MajmunBobo.transform.Find("PrinceGorilla/Kosa").gameObject.SetActive(false);
				
			}

			MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/Chest/Neck/Head/"+AktivanSesir).transform.GetChild(0).gameObject.SetActive(true);
//			AktivanSesir=-1;
			TrenutniSelektovanSesir=-1;
			ProveraTrenutnogItema(AktivanItemSesir);
		}

		if(AktivnaMajica!=-1)
		{
			MajmunBobo.transform.Find("PrinceGorilla/custom_Majica").gameObject.SetActive(true);
			Texture MajicaTekstura = Resources.Load("Majice/Bg"+AktivnaMajica) as Texture;
			MajmunBobo.transform.Find("PrinceGorilla/custom_Majica").GetComponent<Renderer>().material.SetTexture("_MainTex", MajicaTekstura);
			MajmunBobo.transform.Find("PrinceGorilla/custom_Majica").GetComponent<Renderer>().material.color = TShirtColors[AktivnaMajica];
//			AktivnaMajica=-1;
			TrenutnoSelektovanaMajica=-1;
			ProveraTrenutnogItema(AktivanItemMajica);
		}

		if(AktivanRanac!=-1)
		{
			MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/"+AktivanRanac).transform.GetChild(0).gameObject.SetActive(true);
//			AktivanRanac=-1;
			TrenutnoSelektovanRanac=-1;
			ProveraTrenutnogItema(AktivanItemRanac);
		}
	}

	public void OcistiMajmuna()
	{
		ImaUsi=true;
		MajmunBobo.transform.Find("PrinceGorilla/Usi").gameObject.SetActive(true);
		
		ImaKosu=true;
		MajmunBobo.transform.Find("PrinceGorilla/Kosa").gameObject.SetActive(true);

		StagesParser.glava = -1;
		StagesParser.majica = -1;
		StagesParser.ledja = -1;
		StagesParser.imaKosu = true;
		StagesParser.imaUsi = true;

		AktivniItemString = "-1#-1#-1";
		PlayerPrefs.SetString("AktivniItemi",AktivniItemString);
		PlayerPrefs.Save();

		if(AktivanSesir!=-1)
		{
			MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/Chest/Neck/Head/"+AktivanSesir).transform.GetChild(0).gameObject.SetActive(false);
			AktivanSesir=-1;
		}

		if(PreviewSesir!=-1)
		{
		MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/Chest/Neck/Head/"+PreviewSesir).transform.GetChild(0).gameObject.SetActive(false);
		PreviewSesir = -1;
		}

		if(AktivnaMajica!=-1)
		{
			MajmunBobo.transform.Find("PrinceGorilla/custom_Majica").gameObject.SetActive(false);
			AktivnaMajica=-1;
		}

		if(PreviewMajica!=-1)
		{
			MajmunBobo.transform.Find("PrinceGorilla/custom_Majica").gameObject.SetActive(false);
			PreviewMajica = -1;
		}

		if(AktivanRanac!=-1)
		{
			MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/"+AktivanRanac).transform.GetChild(0).gameObject.SetActive(false);
			AktivanRanac=-1;
		}

		if(PreviewRanac!=-1)
		{
			MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/"+PreviewRanac).transform.GetChild(0).gameObject.SetActive(false);
			PreviewRanac = -1;
		}



		if(AktivanCustomizationTab==1)
		{
			TrenutniSelektovanSesir=-1;
			ProveraTrenutnogItema(AktivanItemSesir);
		}
		else if(AktivanCustomizationTab==2)
		{
			TrenutnoSelektovanaMajica=-1;
			ProveraTrenutnogItema(AktivanItemMajica);
		}
		else if(AktivanCustomizationTab==3)
		{
			TrenutnoSelektovanRanac=-1;
			ProveraTrenutnogItema(AktivanItemRanac);
		}
	}


	public void ObuciMajmunaNaStartu()
	{
		if(PlayerPrefs.HasKey("AktivniItemi"))
		{
			AktivniItemString = PlayerPrefs.GetString("AktivniItemi");
			AktivniItemi=AktivniItemString.Split('#');
			AktivanSesir=int.Parse(AktivniItemi[0]);
			AktivnaMajica=int.Parse(AktivniItemi[1]);
			AktivanRanac=int.Parse(AktivniItemi[2]);
			
		}
		else
		{
			AktivanSesir = -1;
			AktivnaMajica = -1;
			AktivanRanac = -1;
			StagesParser.glava = -1;
			StagesParser.imaKosu = true;
			StagesParser.imaUsi = true;
			StagesParser.majica = -1;
			StagesParser.ledja = -1;
		}
		if(AktivanSesir!=-1)
		{
			//Debug.Log("AtivanSesir JE: "+AktivanSesir);
			if(int.Parse(UsiHats[AktivanSesir])==1)
			{
				if(!ImaUsi)
				{
					ImaUsi=true;
					MajmunBobo.transform.Find("PrinceGorilla/Usi").gameObject.SetActive(true);
					StagesParser.imaUsi = true;
				}
			}
			else
			{
				if(ImaUsi)
				{
					ImaUsi=false;
					MajmunBobo.transform.Find("PrinceGorilla/Usi").gameObject.SetActive(false);
					StagesParser.imaUsi = false;
				}
				
			}
			
			if(int.Parse(KosaHats[AktivanSesir])==1)
			{
				
				
				ImaKosu=true;
				MajmunBobo.transform.Find("PrinceGorilla/Kosa").gameObject.SetActive(true);
				StagesParser.imaKosu = true;
				
			}
			else
			{
				
				ImaKosu=false;
				MajmunBobo.transform.Find("PrinceGorilla/Kosa").gameObject.SetActive(false);
				StagesParser.imaKosu = false;
			}
			
			MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/Chest/Neck/Head/"+AktivanSesir).transform.GetChild(0).gameObject.SetActive(true);
			//StagesParser.glava = MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/Chest/Neck/Head/"+AktivanSesir).transform.GetChild(0).gameObject.name;
			StagesParser.glava = AktivanSesir;
		}
		else
		{
			StagesParser.glava = -1;
			StagesParser.imaKosu = true;
			StagesParser.imaUsi = true;
		}
		
		if(AktivnaMajica!=-1)
		{
			MajmunBobo.transform.Find("PrinceGorilla/custom_Majica").gameObject.SetActive(true);
			Texture MajicaTekstura = Resources.Load("Majice/Bg"+AktivnaMajica) as Texture;
			MajmunBobo.transform.Find("PrinceGorilla/custom_Majica").GetComponent<Renderer>().material.SetTexture("_MainTex", MajicaTekstura);
			MajmunBobo.transform.Find("PrinceGorilla/custom_Majica").GetComponent<Renderer>().material.color = TShirtColors[AktivnaMajica];
			////Debug.Log("teksturamname: " + MajicaTekstura.name);
			//StagesParser.majica = MajicaTekstura.name;
			StagesParser.majica = AktivnaMajica;
			StagesParser.bojaMajice = TShirtColors[AktivnaMajica];
		}
		else
		{
			StagesParser.majica = -1;
			StagesParser.bojaMajice = Color.white;
		}
		
		if(AktivanRanac!=-1)
		{
			if(Application.loadedLevel == 1)
			{
				if(AktivanRanac == 0)
					MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/"+AktivanRanac).transform.GetChild(0).GetComponent<MeshFilter>().mesh = GameObject.Find("RefZaSedenje_AndjeoskaKrila").GetComponent<MeshFilter>().mesh;
				else if(AktivanRanac == 5)
					MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/"+AktivanRanac).transform.GetChild(0).GetComponent<MeshFilter>().mesh = GameObject.Find("RefZaSedenje_SlepiMisKrila").GetComponent<MeshFilter>().mesh;
			}
			MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/"+AktivanRanac).transform.GetChild(0).gameObject.SetActive(true);
			//StagesParser.ledja = MajmunBobo.transform.Find("PrinceGorilla/ROOT/Hip/Spine/"+AktivanRanac).transform.GetChild(0).gameObject.name;
			StagesParser.ledja = AktivanRanac;
		}
		else
		{
			StagesParser.ledja = -1;
		}
	}

	void OnApplicationQuit() {
		StariBrojOtkljucanihItema=BrojOtkljucanihKapa+"#"+BrojOtkljucanihMajici+"#"+BrojOtkljucanihRanceva;
		PlayerPrefs.SetString("OtkljucaniItemi",StariBrojOtkljucanihItema);
		PlayerPrefs.Save();
	}

}
