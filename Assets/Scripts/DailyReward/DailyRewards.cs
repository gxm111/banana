using UnityEngine;
using System.Collections;
using System;

public class DailyRewards : MonoBehaviour {

	public static int [] DailyRewardAmount = new int[]{100, 200, 300, 400, 500, 600};
	int OneDayTime=60*60*24;
	public static int LevelReward;
	System.Globalization.DateTimeFormatInfo format;
	private  DateTime  VremePokretanjaDateTime,VremeIzlaska;
	string VremePokretanja,lastPlayDate,VremeQuitString,Vreme;
	string danUlaska, mesecUlaska, godinaUlaska, danIzlaska, mesecIzlaska, godinaIzlaska;

	// Use this for initialization
	void Start () {
//		PlayerPrefs.DeleteAll();
		DateTime currentTime = DateTime.Now;
//		danUlaska="1";
//		mesecUlaska="1";
//		godinaUlaska="2015";
		danUlaska = currentTime.Day.ToString();
		mesecUlaska = currentTime.Month.ToString();
		godinaUlaska = currentTime.Year.ToString();
		Debug.Log("Trenutno vreme je: "+currentTime+" danUlaska: "+danUlaska+" mesecUlaska: "+mesecUlaska);

		if(PlayerPrefs.HasKey("LevelReward"))
		{
			LevelReward=PlayerPrefs.GetInt("LevelReward");
			Debug.Log("Ppokretanje. Lewel je: "+LevelReward);
		}
		else
		{
			LevelReward=0;
			Debug.Log("Prvo pokretanje. Lewel je: "+LevelReward);
		}

		if(PlayerPrefs.HasKey("VremeQuit"))
		{
			lastPlayDate=PlayerPrefs.GetString("VremeQuit");
			VremeIzlaska=DateTime.Parse(lastPlayDate);
//			danIzlaska="31";
//			mesecIzlaska="12";
//			godinaIzlaska="2014";
			danIzlaska=VremeIzlaska.Day.ToString();
			mesecIzlaska = VremeIzlaska.Month.ToString();
			godinaIzlaska = VremeIzlaska.Year.ToString();
			Debug.Log("Vreme izlaska je: "+lastPlayDate+" danIzlaska: "+danIzlaska+" mesecIzlaska: "+mesecIzlaska);
			Debug.Log ("Razlika je "+(int.Parse(danUlaska)-int.Parse(danIzlaska)));
			if((int.Parse(godinaUlaska)-int.Parse(godinaIzlaska))<1)
			{
				Debug.Log("Ista godina");

				if((int.Parse(mesecUlaska)-int.Parse(mesecIzlaska))==0)
				{
					Debug.Log("Isti mesec: "+(int.Parse(mesecUlaska)-int.Parse(mesecIzlaska)));

					if((int.Parse(danUlaska)-int.Parse(danIzlaska)) > 1)
					{
						Debug.Log("Resetuj rewards, isti mesec je samo sto je proslo vise od 2 dana");
						LevelReward=1;
						GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyDolazak");
						PlayerPrefs.SetInt("LevelReward",LevelReward);
						PlayerPrefs.Save();
						PrikaziDailyReward(LevelReward);
					}
					else if ((int.Parse(danUlaska)-int.Parse(danIzlaska)) > 0)
					{


						if(LevelReward<6)
						{
							Debug.Log("Stari LevelReward je :"+LevelReward);
							GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyDolazak");
							LevelReward++;

							Debug.Log ("Dobio si nagradu! Novi dan je. Dan: "+LevelReward);
							PlayerPrefs.SetInt("LevelReward",LevelReward);
							PlayerPrefs.Save();
							Debug.Log("Novi LevelReward je :"+LevelReward);
							PrikaziDailyReward(LevelReward);
						}
						else
						{
						
							LevelReward=1;
							Debug.Log ("Proslo 6 dana. Redovan bio ali resetuj :"+LevelReward);
							GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyDolazak");
							PlayerPrefs.SetInt("LevelReward",LevelReward);
							PlayerPrefs.Save();
							PrikaziDailyReward(LevelReward);
						}

						//Ponisti notifikaciju za DailyReward i posalji novu i prikazi DailyRewards sa nivoom LevelReward na 24h
					}
					else
					{
						Debug.Log("Nije novi dan jos uvek");
					}
				}
				else
				{
					Debug.Log("Nije isti mesec");

					if(int.Parse(danUlaska)==1)
					{
						if(int.Parse(mesecIzlaska)==1 || int.Parse(mesecIzlaska)==3 || int.Parse(mesecIzlaska)==5 || int.Parse(mesecIzlaska)==7 || int.Parse(mesecIzlaska)==8 || int.Parse(mesecIzlaska)==10 || int.Parse(mesecIzlaska)==12)
						{
							Debug.Log("Prethodni Mesec ima 31 dan");
							if(int.Parse(danIzlaska)==31)
							{

								if(LevelReward<6)
								{
									GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyDolazak");
									LevelReward++;
									Debug.Log ("Dobio si nagradu! Prelazak iz meseca u mesec:"+LevelReward);

									PlayerPrefs.SetInt("LevelReward",LevelReward);
									PlayerPrefs.Save();
									PrikaziDailyReward(LevelReward);
								}
								else
								{
									LevelReward=1;
									Debug.Log ("Dobio si nagradu! Prelazak iz meseca u mesec i proslo 6 dana:"+LevelReward);
									GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyDolazak");

									PlayerPrefs.SetInt("LevelReward",LevelReward);
									PlayerPrefs.Save();
									PrikaziDailyReward(LevelReward);
								}

								//Ponisti notifikaciju za DailyReward i posalji novu i prikazi DailyRewards sa nivoom LevelReward na 24h
							}
							else
							{
								Debug.Log ("Prelazak iz meseca u mesec. Resetuj Rewards, proslo vise od 2 dana");
								LevelReward=1;
								GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyDolazak");
								PlayerPrefs.SetInt("LevelReward",LevelReward);
								PlayerPrefs.Save();
								PrikaziDailyReward(LevelReward);

								//Ponisti notifikaciju za DailyReward i posalji novu i prikazi DailyRewards sa nivoom LevelReward na 24h
							}
						}
						else if(int.Parse(mesecIzlaska)==4 || int.Parse(mesecIzlaska)==6 || int.Parse(mesecIzlaska)==9 || int.Parse(mesecIzlaska)==11)
						{
							Debug.Log("Prethodni Mesec ima 30 dan");
							if(int.Parse(danIzlaska)==30)
							{
								Debug.Log ("Dobio si nagradu! Prelazak iz meseca u mesec.");

								if(LevelReward<6)
								{
									GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyDolazak");
									LevelReward++;

									PlayerPrefs.SetInt("LevelReward",LevelReward);
									PlayerPrefs.Save();
									PrikaziDailyReward(LevelReward);
								}
								else
								{
									LevelReward=1;
									GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyDolazak");

									PlayerPrefs.SetInt("LevelReward",LevelReward);
									PlayerPrefs.Save();
									PrikaziDailyReward(LevelReward);
								}

								//Ponisti notifikaciju za DailyReward i posalji novu i prikazi DailyRewards sa nivoom LevelReward na 24h
							}
							else
							{
								Debug.Log ("Prelazak iz meseca u mesec. Resetuj Rewards, proslo vise od 2 dana");
								LevelReward=1;
								GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyDolazak");
								PlayerPrefs.SetInt("LevelReward",LevelReward);
								PlayerPrefs.Save();
								PrikaziDailyReward(LevelReward);
								//Ponisti notifikaciju za DailyReward i posalji novu i prikazi DailyRewards sa nivoom LevelReward na 24h
							}
						}
						else
						{
							Debug.Log("Mesec je Februar");
							if(int.Parse(danIzlaska)>27)
							{
								Debug.Log ("Dobio si nagradu! Prelazak iz Februara u Mart.");

								if(LevelReward<6)
								{
									GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyDolazak");
									LevelReward++;
								
									PlayerPrefs.SetInt("LevelReward",LevelReward);
									PlayerPrefs.Save();
									PrikaziDailyReward(LevelReward);
								}
								else
								{
									LevelReward=1;
									GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyDolazak");
								
									PlayerPrefs.SetInt("LevelReward",LevelReward);
									PlayerPrefs.Save();
									PrikaziDailyReward(LevelReward);
								}

								//Ponisti notifikaciju za DailyReward i posalji novu i prikazi DailyRewards sa nivoom LevelReward na 24h
							}
							else
							{
								Debug.Log ("Prelazak iz meseca u mesec. Resetuj Rewards, proslo vise od 2 dana");
								LevelReward=1;
								GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyDolazak");
								PlayerPrefs.SetInt("LevelReward",LevelReward);
								PlayerPrefs.Save();
								PrikaziDailyReward(LevelReward);
								//Ponisti notifikaciju za DailyReward i posalji novu i prikazi DailyRewards sa nivoom LevelReward na 24h
							}
						}
					}
					else
					{
						Debug.Log("Resetuj Rewards, posto je novi mesec i razlika je vise od 2 dana");
						LevelReward=1;
						GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyDolazak");
						PlayerPrefs.SetInt("LevelReward",LevelReward);
						PlayerPrefs.Save();
						PrikaziDailyReward(LevelReward);
						//Ponisti notifikaciju za DailyReward i posalji novu i prikazi DailyRewards sa nivoom LevelReward na 24h
					}

				}


			}
			else
			{
				Debug.Log("Nije ista godina");
				if(int.Parse(danIzlaska)==31 && int.Parse(danUlaska)==1)
				{
					Debug.Log("Prvi dan u Novoj Godini, i izasao iz app 31. decembra, dobija Daily Rewards");

					if(LevelReward<6)
					{
						GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyDolazak");
						LevelReward++;

						PlayerPrefs.SetInt("LevelReward",LevelReward);
						PlayerPrefs.Save();
						PrikaziDailyReward(LevelReward);
					}
					else
					{
						LevelReward=1;
						GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyDolazak");
					
						PlayerPrefs.SetInt("LevelReward",LevelReward);
						PlayerPrefs.Save();
						PrikaziDailyReward(LevelReward);
					}
					
					//Ponisti notifikaciju za DailyReward i posalji novu i prikazi DailyRewards sa nivoom LevelReward na 24h
				}
				else
				{
					Debug.Log("Resetuj Daily Rewards, nije bio redovan zbog Nove godine");

					LevelReward=1;
					GameObject.Find("DailyReward").GetComponent<Animation>().Play("DailyDolazak");
					PlayerPrefs.SetInt("LevelReward",LevelReward);
					PlayerPrefs.Save();
					PrikaziDailyReward(LevelReward);
					//Ponisti notifikaciju za DailyReward i posalji novu i prikazi DailyRewards sa nivoom LevelReward na 24h
				}
			}


		}
		else
		{
			Debug.Log("PrvoPokretanje");
			LevelReward=0;
			PlayerPrefs.SetInt("LevelReward",LevelReward);
			PlayerPrefs.Save();

			//Pokreni Notifikaciju za DailyReward na 24h
		}


	}



	void OnApplicationPause(bool pauseStatus) { //vraca false kad je aktivna app
		if(pauseStatus)
		{
			//izasao iz aplikacuje
			VremeQuitString = DateTime.Now.ToString();
			PlayerPrefs.SetString("VremeQuit", VremeQuitString);
			PlayerPrefs.Save();
			
		}
		else
		{
			//usao u aplikacuju
			Debug.Log("Usao nazad u aplikaciju iz Pause");
			
		}
		
		
	}

	void PrikaziDailyReward(int TrenutniDan)
	{
		Debug.Log ("Trenutni dan nagrade je: " + TrenutniDan);
		GameObject.Find("SpotLight").GetComponent<Animation>().Play("DailyReward"+TrenutniDan);
		GameObject currentDay;
		if(TrenutniDan < 6)
			currentDay = GameObject.Find("Day " + TrenutniDan.ToString());
		else
			currentDay = GameObject.Find("Day 6 - Magic Box");
		currentDay.transform.GetChild(0).GetComponent<Animator>().Play("CollectDailyRewardTab");
		currentDay.transform.GetChild(0).Find("DailyRewardParticlesIdle").GetComponent<ParticleSystem>().Play();
//		switch(TrenutniDan)
//		{
//		case 1:
//			GameObject.Find("SpotLight").GetComponent<Animation>().Play("DailyReward1");
//			break;
//
//		case 2:
//			GameObject.Find("SpotLight").GetComponent<Animation>().Play("DailyReward2");
//			break;
//
//		case 3:
//			GameObject.Find("SpotLight").GetComponent<Animation>().Play("DailyReward3");
//			break;
//
//		case 4:
//			GameObject.Find("SpotLight").GetComponent<Animation>().Play("DailyReward4");
//			break;
//
//		case 5:
//			GameObject.Find("SpotLight").GetComponent<Animation>().Play("DailyReward5");
//			break;
//
//		case 6:
//			GameObject.Find("SpotLight").GetComponent<Animation>().Play("DailyReward6");
//			break;
//		}
	}

	void OnApplicationQuit() {
		VremeQuitString = DateTime.Now.ToString();
		PlayerPrefs.SetString("VremeQuit", VremeQuitString);
		PlayerPrefs.Save();

		//Pokreni Notifikaciju za DailyReward na 24h
	}
}
