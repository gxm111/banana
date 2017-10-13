using UnityEngine;
using System.Collections;
using System;

public class RacunanjeVremena : MonoBehaviour {
	private  DateTime VremeQuitDateTime, VremeResumeDateTime;
	string VremeQuitString, VremeResumeString;
	public static string Vreme;
	int ProveriVreme;
	string sati, minuti, sekunde, UkupnoSek;	
	int satiInt, minutiInt, sekundeInt;
	public static int UkupnoSekundi;
	System.Globalization.DateTimeFormatInfo format;
	
	
	// Use this for initialization
	void Awake () {
//				PlayerPrefs.DeleteAll();
		format =System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat;

		if(PlayerPrefs.HasKey("VremeQuit"))
		{
			ProveriVreme = PlayerPrefs.GetInt("ProveriVreme");
			
			VremeQuitString=PlayerPrefs.GetString("VremeQuit");
			VremeQuitDateTime = DateTime.Parse(VremeQuitString);
			
			VremeResumeString=DateTime.Now.ToString(format.FullDateTimePattern);
			VremeResumeDateTime=DateTime.Parse(VremeResumeString);
			
			TimeSpan duration = VremeResumeDateTime.Subtract(VremeQuitDateTime);
			Vreme=duration.ToString(); 
//			GameObject.Find("Text").GetComponent<TextMesh>().text=Vreme;
			string[] brojevi = Vreme.Split(':');
			string[] pom=brojevi[0].Split('.');
			int duzina=pom.Length;
			Debug.Log("duzina:"+duzina);
			if(duzina==2)
			{
				int DanUSate=int.Parse(pom[0])*24+int.Parse(pom[duzina-1]);
				
				if(DanUSate<0)
				{
					DanUSate=Mathf.Abs(DanUSate);
				}
				sati=DanUSate.ToString();
				Debug.Log("UkupnoSek sati posle konverzije iz dana:"+sati);
			}
			else
			{
				sati=pom[duzina-1];
			}

			minuti=brojevi[1];
			sekunde=brojevi[2];
			
			satiInt = Int32.Parse(sati);
			minutiInt = Int32.Parse(minuti);
			sekundeInt = Int32.Parse(sekunde);
			
			UkupnoSekundi = sekundeInt+minutiInt*60+satiInt*3600;
			UkupnoSek=UkupnoSekundi.ToString();
			Debug.Log("Proslo je ukupno: "+UkupnoSek);
			
		}
		else
		{
			string dateTime = DateTime.Now.ToString(format.FullDateTimePattern);
			PlayerPrefs.SetString("VremeQuit", dateTime);
			PlayerPrefs.SetInt("ProveriVreme", 0);
			PlayerPrefs.Save();
			UkupnoSekundi=0;
			

		}
		
	}
	

	
	
	void OnApplicationPause(bool pauseStatus) { //vraca false kad je aktivna app
		if(pauseStatus)
		{
			//izasao iz aplikacuje
			VremeQuitString = DateTime.Now.ToString(format.FullDateTimePattern); //izbacio Null Reference Exception na pauzu
			PlayerPrefs.SetString("VremeQuit", VremeQuitString);
			PlayerPrefs.SetInt("ProveriVreme",1);
			PlayerPrefs.Save();
			
		}
		else
		{
			//usao u aplikacuju
			ProveriVreme = PlayerPrefs.GetInt("ProveriVreme");
			if(ProveriVreme==1)
			{

				format =System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat;
				VremeResumeString=DateTime.Now.ToString(format.FullDateTimePattern);
				VremeResumeDateTime=DateTime.Parse(VremeResumeString);
				VremeQuitString=PlayerPrefs.GetString("VremeQuit");
				VremeQuitDateTime = DateTime.Parse(VremeQuitString);
				
				TimeSpan duration = VremeResumeDateTime.Subtract(VremeQuitDateTime);
				Debug.Log("Duration: "+duration);
				Vreme=duration.ToString(); 

				
//				GameObject.Find("Text").GetComponent<TextMesh>().text=Vreme;
				string[] brojevi = Vreme.Split(':');
				string[] pom=brojevi[0].Split('.');
				int duzina=pom.Length;
				Debug.Log("duzina:"+duzina);

				if(duzina==2)
				{
					int DanUSate=int.Parse(pom[0])*24+int.Parse(pom[duzina-1]);

					if(DanUSate<0)
					{
						DanUSate=Mathf.Abs(DanUSate);
					}
					sati=DanUSate.ToString();
					Debug.Log("UkupnoSek sati posle konverzije iz dana:"+sati);
				}
				else
				{
					sati=pom[duzina-1];
				}
				minuti=brojevi[1];
				sekunde=brojevi[2];
				
				satiInt = Int32.Parse(sati);
				minutiInt = Int32.Parse(minuti);
				sekundeInt = Int32.Parse(sekunde);
				
				UkupnoSekundi = sekundeInt+minutiInt*60+satiInt*3600;
				UkupnoSek=UkupnoSekundi.ToString();
				
				
				Debug.Log("Sati :"+sati+" Minuti: " +minuti+" Sekunde: "+sekunde);
//				GameObject.Find("TextSati").GetComponent<TextMesh>().text=sati;
//				GameObject.Find("TextMinuti").GetComponent<TextMesh>().text=minuti;
//				GameObject.Find("TextSekunde").GetComponent<TextMesh>().text=sekunde;
//				GameObject.Find("TextUkupnoSek").GetComponent<TextMesh>().text=UkupnoSek;
			
			}
			else
			{
				Debug.Log("Proveri vreme je 0");
			}
			
		}
		
		
	}
	
	void OnApplicationQuit() {
		VremeQuitString = DateTime.Now.ToString(format.FullDateTimePattern);
		PlayerPrefs.SetString("VremeQuit", VremeQuitString);
		PlayerPrefs.SetInt("ProveriVreme",1);
		PlayerPrefs.Save();
	}
	
}
