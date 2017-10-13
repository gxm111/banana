using UnityEngine;
using System.Collections;
using System;

public class TimeReward : MonoBehaviour {

	public static float VremeBrojaca;
	float VremeZaOduzimanje;
	int Minuti, Sekunde, Sati;
	GameObject Kovceg;
	System.Globalization.DateTimeFormatInfo format;
	private  DateTime  VremePokretanjaDateTime,VremeIzlaska;
	string VremePokretanja,lastPlayDate,VremeQuitString,Vreme;
	string danUlaska, mesecUlaska, godinaUlaska, danIzlaska, mesecIzlaska, godinaIzlaska;
	public static bool PokupiTimeNagradu=false;


	// Use this for initialization
	void Start () {
//		PlayerPrefs.DeleteAll();


		Kovceg = GameObject.Find("Kovceg");
		if(PlayerPrefs.HasKey("VremeBrojaca"))
		{
			VremeBrojaca=PlayerPrefs.GetFloat("VremeBrojaca");
			VremeZaOduzimanje=(float)RacunanjeVremena.UkupnoSekundi;
			//Debug.Log("VremeZaOduzimanje: "+VremeZaOduzimanje+" VremeBrojaca"+VremeBrojaca);

			if(VremeZaOduzimanje>10798)
			{

				VremeBrojaca=0;
				//Debug.Log("VremeZaOduzimanje: "+0);

			}
			else
			{
				VremeBrojaca=VremeBrojaca-VremeZaOduzimanje;

			}
			
			if(VremeBrojaca<1)
			{
				PokupiTimeNagradu=true;
				Kovceg.GetComponent<Collider>().enabled = true;
				Kovceg.transform.Find("Text/Collect").GetComponent<TextMesh>().text= LanguageManager.Collect;
				//Kovceg.transform.Find("Text/Collect").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
				Kovceg.transform.Find("Novcici").gameObject.SetActive(true);
				Kovceg.GetComponent<Animator>().Play("Kovceg Collect Animation");
			}
			else
			{
				Kovceg.transform.Find("Text/Coin Number").gameObject.SetActive(false);
				Kovceg.transform.Find("Text/Coin").gameObject.SetActive(false);
				Kovceg.transform.Find("Novcici").gameObject.SetActive(false);
				Kovceg.GetComponent<Collider>().enabled = false;
			}
		}
		else
		{
			//Debug.Log("PrvoPokretanje");
			VremeBrojaca=10800;
			Kovceg.transform.Find("Text/Coin Number").gameObject.SetActive(false);
			Kovceg.transform.Find("Text/Coin").gameObject.SetActive(false);
			Kovceg.transform.Find("Novcici").gameObject.SetActive(false);
			Kovceg.GetComponent<Collider>().enabled = false;
			//Pokreni Notifikaciju za DailyReward na 24h
		}


	}
	
	// Update is called once per frame
	void Update () {
		if(!PokupiTimeNagradu)
		{
			Odbrojavanje();
		}


	}

	void Odbrojavanje()
	{
		VremeBrojaca=(VremeBrojaca-Time.deltaTime);

		Minuti=(int)VremeBrojaca/60;
		Sekunde=(int)VremeBrojaca%60;
		Sati=Minuti/60;
		Minuti=Minuti-60*Sati;

//				Kovceg.GetComponent<TextMesh>().text= VremeBrojaca.ToString("F0");
		if(Sekunde<1 && Minuti<1 && Sati<1)
		{
			//Debug.Log("IStekloVreme");
			Kovceg.GetComponent<Animator>().Play("Kovceg Collect Animation");
			Kovceg.GetComponent<Collider>().enabled = true;
			Kovceg.transform.Find("Text/Collect").GetComponent<TextMesh>().text= LanguageManager.Collect;
			//Kovceg.transform.Find("Text/Collect").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
			Kovceg.transform.Find("Text/Coin Number").gameObject.SetActive(true);
			Kovceg.transform.Find("Text/Coin").gameObject.SetActive(true);
			Kovceg.transform.Find("Novcici").gameObject.SetActive(true);
			PokupiTimeNagradu=true;
		}
		else if(Sati>0)
		{
			if(Sekunde>=0 && Sekunde<=9)
			{
				
				if(Minuti<10)
				{
					Kovceg.transform.Find("Text/Collect").GetComponent<TextMesh>().text= "0"+Sati.ToString()+":0"+Minuti.ToString()+":"+"0"+Sekunde.ToString();
				}
				else
				{
					Kovceg.transform.Find("Text/Collect").GetComponent<TextMesh>().text= "0"+Sati.ToString()+":"+Minuti.ToString()+":"+"0"+Sekunde.ToString();
				}
			}
			else
			{
				if(Minuti<10)
				{
					Kovceg.transform.Find("Text/Collect").GetComponent<TextMesh>().text="0"+Sati.ToString()+":0"+Minuti.ToString()+":"+Sekunde.ToString();
				}
				else
				{
					Kovceg.transform.Find("Text/Collect").GetComponent<TextMesh>().text="0"+Sati.ToString()+":"+Minuti.ToString()+":"+Sekunde.ToString();
				}
			}
		}
		else
		{
			if(Sekunde>=0 && Sekunde<=9)
			{
				
				if(Minuti<10)
				{
					Kovceg.transform.Find("Text/Collect").GetComponent<TextMesh>().text= "00:0"+Minuti.ToString()+":"+"0"+Sekunde.ToString();
				}
				else
				{
					Kovceg.transform.Find("Text/Collect").GetComponent<TextMesh>().text= "00:"+Minuti.ToString()+":"+"0"+Sekunde.ToString();
				}
			}
			else
			{
				if(Minuti<10)
				{
					Kovceg.transform.Find("Text/Collect").GetComponent<TextMesh>().text="00:0"+Minuti.ToString()+":"+Sekunde.ToString();
				}
				else
				{
					Kovceg.transform.Find("Text/Collect").GetComponent<TextMesh>().text="00:"+Minuti.ToString()+":"+Sekunde.ToString();
				}
			}
		}

		//Kovceg.transform.Find("Text/Collect").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
	}

	void OnApplicationPause(bool pauseStatus) { //vraca false kad je aktivna app
		if(pauseStatus)
		{
			//izasao iz aplikacuje

			PlayerPrefs.SetFloat("VremeBrojaca", VremeBrojaca);
			PlayerPrefs.Save();
			
		}
		else
		{
			//usao u aplikacuju
			//Debug.Log("Usao nazad u aplikaciju iz Pause");
			VremeBrojaca=PlayerPrefs.GetFloat("VremeBrojaca");
			VremeZaOduzimanje=(float)RacunanjeVremena.UkupnoSekundi;
			//VremeZaOduzimanje=10888;  //- za testiranje deo
			if(VremeZaOduzimanje>10798)
			{
				
				VremeBrojaca=0;
				//Debug.Log("VremeZaOduzimanje: "+0);
				PokupiTimeNagradu=true;
				Kovceg.GetComponent<Collider>().enabled = true;
				Kovceg.transform.Find("Text/Coin Number").gameObject.SetActive(true);
				Kovceg.transform.Find("Text/Collect").GetComponent<TextMesh>().text= LanguageManager.Collect;
				//Kovceg.transform.Find("Text/Collect").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
				Kovceg.transform.Find("Novcici").gameObject.SetActive(true);
				Kovceg.GetComponent<Animator>().Play("Kovceg Collect Animation");
			}
			else
			{
//				Debug.Log("Vreme za oduzimanje: " + VremeZaOduzimanje);
				VremeBrojaca=VremeBrojaca-VremeZaOduzimanje;
//				if(VremeBrojaca <= 0)
//					Kovceg.transform.Find("Novcici").gameObject.SetActive(false);
			}
		}
		
		
	}

	public void PokupiNagradu()
	{
		if(TimeReward.PokupiTimeNagradu)
		{
			TimeReward.VremeBrojaca=10800;
			TimeReward.PokupiTimeNagradu=false;
			StagesParser.currentMoney+=100;
			PlayerPrefs.SetInt("TotalMoney",StagesParser.currentMoney);
			PlayerPrefs.Save();
			Invoke("DelayZaOdbrojavanje",1.5f);
			Kovceg.transform.Find("Text/Coin Number").gameObject.SetActive(false);
			Kovceg.transform.Find("Text/Coin").gameObject.SetActive(false);
			//Kovceg.transform.Find("Novcici").gameObject.SetActive(false);
			StagesParser.ServerUpdate = 1;
		}
	}
	void SkloniCoinsReward()
	{
		GameObject.Find("CoinsReward").GetComponent<Animation>().Play("CoinsRewardOdlazak");
	}

	void DelayZaOdbrojavanje()
	{
		StartCoroutine(StagesParser.Instance.moneyCounter(100,GameObject.Find("CoinsReward/Coins Number").GetComponent<TextMesh>(),true));
		Invoke("SkloniCoinsReward",1.2f);
	}
	
	
	void OnApplicationQuit() {

		PlayerPrefs.SetFloat("VremeBrojaca", VremeBrojaca);
		PlayerPrefs.Save();
		
		//Pokreni Notifikaciju za TimeReward na 3h
	}
}
