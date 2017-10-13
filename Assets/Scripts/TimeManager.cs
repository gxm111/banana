using UnityEngine;
using System.Collections;
using System;

public class TimeManager : MonoBehaviour {
	/*
	 * Scene: all
	 * Object: TimeManager
	 * Trenutno vreme servera je smesteno u DateTime promenljivu
	 * currentDate , Sinhronizacija se serverom se izvrsava pri 
	 * startovanju aplikacije i pri ponovnom podizanju nakon 
	 * minimizovanja.Dok je Aplikacija aktivna TimeManager
	 * funkcijom CountTime() obezbedjuje da je vreme sinhronizovano.
	 * U slucaju da je doslo da greske pri trazenju
	 * vremena polje godina currentDate-u je 42.
	 * Pri minimizovanju zaustavlja se CountTime() i cuva se vreme 
	 * kad je aplikacija minimizovana. U okviru awake funkcije je neophodno da stoji
	 * DontDestroyOnLoad(gameObject) da bi manager u svim scenema
	 * 
	 * */
	
	//trenutno vreme, ukoliko nije u skladu sa serverom sa polja su 1 ili je godina 42ga
	public static DateTime currentDate;
	
	
	void Awake() 
	{
		transform.name="TimeManager";
		DontDestroyOnLoad(gameObject);
		
		GetCurrentTime();
	}
	// Use this for initialization
	
	
	//poziva se prilikom minimizovanja i ponovnog podizanja aplikacije
	//true grana glavnog if grananja je pri minimizovanju a else prilikon
	//podizanja
	void OnApplicationPause(bool pauseStatus)
	{
		if(pauseStatus)
		{
			//PlayerPrefs.SetString("PausedOn",Application.loadedLevelName);
			//Kad se pauzira
			StopCoroutine("CountTime");
			if(currentDate.Year!=42)
			{
				PlayerPrefs.SetInt("minutes",currentDate.Minute);
				PlayerPrefs.SetInt("hours",currentDate.Hour);
				PlayerPrefs.SetInt("year",currentDate.Year);
				PlayerPrefs.SetInt("day",currentDate.Day);
				PlayerPrefs.SetInt("month",currentDate.Month);
				PlayerPrefs.Save();
			}
		}
		else
		{
			//kad je opet Aktivna
			
			GetCurrentTime();
		}
	}
	
	//Poziva se pri startu i ponovnom podizanju i trazi vreme od 
	//tapsong servera. Ukoliko ne moze da dobije vreme, rezultat u currentDate
	//godina je 42-ga
	WWW www ;
	void GetCurrentTime()
	{
		
		string url = "http://tapsong.net/content/3DGeoQuiz/index.php";
  		www   = new WWW (url);
		StartCoroutine("Wait5");
		StartCoroutine("WaitWWW");
	}
	
	IEnumerator WaitWWW()
	{
		yield return www;	
		int hours=0,minutes=0,month=0,day=0,year=0;
		if (String.IsNullOrEmpty(www.error) && www.isDone)
		{
			StopCoroutine("Wait5");
			string helpTekst=www.text;
			string[] serverDate=helpTekst.Split('/');
			day=int.Parse( serverDate[0]);
			month=int.Parse( serverDate[1]);
			year= int.Parse( serverDate[2]);
			hours= int.Parse( serverDate[3]);
			minutes= int.Parse( serverDate[4]);
			currentDate= new DateTime(year,month,day,hours,minutes,0);
			StartCoroutine("CountTime");
			CheckForDailyReward();
			Debug.Log("wwwOk");
			
		}
		else
		{
			Debug.Log("wwwError");
			StopCoroutine("Wait5");
			currentDate=new DateTime(42,1,1,1,1,1);
			
		}
	}
	IEnumerator Wait5()
	{
		yield return new WaitForSeconds(5);	
		int hours=0,minutes=0,month=0,day=0,year=0;
		if (String.IsNullOrEmpty(www.error) && www.isDone)
		{
			StopCoroutine("WaitWWW");
			string helpTekst=www.text;
			string[] serverDate=helpTekst.Split('/');
			day=int.Parse( serverDate[0]);
			month=int.Parse( serverDate[1]);
			year= int.Parse( serverDate[2]);
			hours= int.Parse( serverDate[3]);
			minutes= int.Parse( serverDate[4]);
			currentDate= new DateTime(year,month,day,hours,minutes,0);
			StartCoroutine("CountTime");
			CheckForDailyReward();
			Debug.Log("5ok");
			
		}
		else
		{Debug.Log("5Error");
			StopCoroutine("WaitWWW");
			currentDate=new DateTime(42,1,1,1,1,1);
			
		}
	}
	//Svake sekunde dok je aplikacija podignuta dodaje sekundu
	//na trenutno vreme da bi se obezbedila sinhronizacija se serverom
	IEnumerator CountTime()
	{
		while(true)
		{
			currentDate.AddSeconds(1);
			yield return new WaitForSeconds(1);
			yield return null;
		}
	}
	
	//Daily reward
	void CheckForDailyReward()
	{
		if(currentDate.Year!=42)
		{
			if(PlayerPrefs.HasKey("scheduledNotification"))
			{
				int Minutes =PlayerPrefs.GetInt("minutesNotification");
				int  Hours=PlayerPrefs.GetInt("hoursNotification");
				int Years =PlayerPrefs.GetInt("yearNotification");
				int Days =PlayerPrefs.GetInt("dayNotification");
				int Months =PlayerPrefs.GetInt("monthNotification");
				if(Years!=0)
				{
					DateTime currentDate2= new DateTime(Years,Months,Days,Hours,Minutes,0);
					if(Mathf.Abs((float)currentDate.Subtract(currentDate2).TotalHours)>24)
					{	//TODO  Implementacija nagrada ,
						//;
					}
				}
			
			}
			else
			{
				PlayerPrefs.SetInt("scheduledNotification",1);
				PlayerPrefs.SetInt("minutessNotification",currentDate.Minute);
				PlayerPrefs.SetInt("hourssNotification",currentDate.Hour);
				PlayerPrefs.SetInt("yearsNotification",currentDate.Year);
				PlayerPrefs.SetInt("daysNotification",currentDate.Day);
				PlayerPrefs.SetInt("monthsNotification",currentDate.Month);
				PlayerPrefs.Save();
			}
		}
	}
}
