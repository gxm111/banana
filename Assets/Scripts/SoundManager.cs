using UnityEngine;
using System.Collections;

public class SoundManager:MonoBehaviour{
	/*
	 * Scene:All
	 * Object:SoundManager
	 * Klasa koja definise 2 promenljive soundOn i musicOn,
	 * koji se koriste da bi se ispitalo da li treba da se pustio audio zvuk 
	 * Postavlja se u PrepareManagers i nalazi se u svim ostalim scenama
	 * Na startu ucitava prethodna podesavanja za zvuk, a pri gasenju ili
	 * pauziranju aplikacije ih cuva. Takodje osigurati da u Awake funkciji
	 * postoji DontDestroyOnLoad(gameObject)
	 * 
	 * */
	// Use this for initialization
	public static bool soundOn=false;
	public static bool musicOn=false;
	 
	void Awake()
	{
		name="SoundManager";
		DontDestroyOnLoad(gameObject);
		if(PlayerPrefs.HasKey("soundOn"))
		{
			soundOn= ((PlayerPrefs.GetInt("soundOn")==1)?true:false);
			musicOn= ((PlayerPrefs.GetInt("musicOn")==1)?true:false);
		}
		else
		{
			soundOn=musicOn=true;
			PlayerPrefs.SetInt("soundOn",1);
			PlayerPrefs.SetInt("musicOn",1);
			PlayerPrefs.Save();
		}
	}
	void OnApplicationQuit()
	{
		PlayerPrefs.SetInt("soundOn",((soundOn)?1:0));
		PlayerPrefs.SetInt("musicOn",((musicOn)?1:0));
		PlayerPrefs.Save();
	}
	void OnApplicationPause(bool pauseStatus)
	{
		if(pauseStatus)
		{
			PlayerPrefs.SetInt("soundOn",((soundOn)?1:0));
			PlayerPrefs.SetInt("musicOn",((musicOn)?1:0));
			PlayerPrefs.Save();
		}
	}
	
}
