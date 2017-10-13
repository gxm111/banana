using UnityEngine;
using System.Collections;

public class PlaySounds : MonoBehaviour {

	// Use this for initialization
	static AudioSource Button_MusicOn;
	static AudioSource Button_SoundOn;
	static AudioSource Button_Play;
	static AudioSource Button_GoBack;
	static AudioSource Button_OpenWorld;
	static AudioSource Button_OpenLevel;
	static AudioSource Button_LockedLevel_Click;
	static AudioSource Button_Pause;
	static AudioSource Button_NextLevel;
	static AudioSource Button_RestartLevel;
	[HideInInspector]
	public static AudioSource Run;
	static AudioSource Jump1;
	static AudioSource Jump2;
	static AudioSource Jump3;
	static AudioSource VoiceJump1;
	static AudioSource VoiceJump2;
	static AudioSource VoiceJump3;
	static AudioSource VoiceJump4;
	static AudioSource VoiceJump5;
	static AudioSource VoiceJump6;
	static AudioSource VoiceJump7;
	static AudioSource VoiceJump8;
	static AudioSource Landing1;
	static AudioSource Landing2;
	static AudioSource Landing3;
	static AudioSource Landing_Strong;
	static AudioSource SmashBaboon;
	[HideInInspector]
	public static AudioSource Level_Failed_Popup;
	static AudioSource Level_Completed_Popup;
	static AudioSource CollectCoin;
	static AudioSource CollectCoin_2nd;
	static AudioSource CollectCoin_3rd;
	static AudioSource GetStar;
	static AudioSource GetStar2;
	static AudioSource GetStar3;
	static AudioSource CoinsSpent;
	static AudioSource NoMoreCoins;
	static AudioSource Biljka_Ugriz_NEW;
	static AudioSource Collect_Banana_NEW;
	static AudioSource Collect_Diamond_NEW;
	static AudioSource Collect_PowerUp_NEW;
	[HideInInspector]
	public static AudioSource Glide_NEW;
	static AudioSource OtkljucavanjeNivoa_NEW;
	static AudioSource SmashGorilla_NEW;
	static AudioSource Otvaranje_Kovcega_NEW;
	static AudioSource Bure_Eksplozija_NEW;
	static AudioSource MushroomBounce;
	static AudioSource Siljci;
	static AudioSource TNTBure_Eksplozija;
	static AudioSource LooseShield;
	static AudioSource MajmunUtepan;
	[HideInInspector]
	public static AudioSource BackgroundMusic_Gameplay;
	[HideInInspector]
	public static AudioSource BackgroundMusic_Menu;

	[HideInInspector]
	public static bool soundOn;
	[HideInInspector]
	public static bool musicOn;

	static int zvukZaBabuna = 0;

	void Awake () 
	{
		//PlayerPrefs.DeleteAll();
		DontDestroyOnLoad(this.gameObject);
		Button_MusicOn 				= transform.Find("Button_MusicOn").GetComponent<AudioSource>();
		Button_SoundOn 				= transform.Find("Button_SoundOn").GetComponent<AudioSource>();
		Button_Play 				= transform.Find("Button_Play").GetComponent<AudioSource>();
		Button_GoBack 				= transform.Find("Button_GoBack").GetComponent<AudioSource>();
		Button_OpenWorld 			= transform.Find("Button_OpenWorld").GetComponent<AudioSource>();
		Button_OpenLevel 			= transform.Find("Button_OpenLevel").GetComponent<AudioSource>();
		Button_LockedLevel_Click 	= transform.Find("Button_LockedLevel_Click").GetComponent<AudioSource>();
		Button_Pause 				= transform.Find("Button_Pause").GetComponent<AudioSource>();
		Button_NextLevel 			= transform.Find("Button_NextLevel").GetComponent<AudioSource>();
		Button_RestartLevel 		= transform.Find("Button_RestartLevel").GetComponent<AudioSource>();
		Run 						= transform.Find("Run").GetComponent<AudioSource>();
		Jump1 						= transform.Find("Jump1").GetComponent<AudioSource>();
		Jump2 						= transform.Find("Jump2").GetComponent<AudioSource>();
		Jump3 						= transform.Find("Jump3").GetComponent<AudioSource>();
		VoiceJump1 					= transform.Find("VoiceJump1").GetComponent<AudioSource>();
		VoiceJump2 					= transform.Find("VoiceJump2").GetComponent<AudioSource>();
		VoiceJump3 					= transform.Find("VoiceJump3").GetComponent<AudioSource>();
		VoiceJump4 					= transform.Find("VoiceJump4").GetComponent<AudioSource>();
		VoiceJump5 					= transform.Find("VoiceJump5").GetComponent<AudioSource>();
		VoiceJump6 					= transform.Find("VoiceJump6").GetComponent<AudioSource>();
		VoiceJump7 					= transform.Find("VoiceJump7").GetComponent<AudioSource>();
		VoiceJump8 					= transform.Find("VoiceJump8").GetComponent<AudioSource>();
		Landing1 					= transform.Find("Landing1").GetComponent<AudioSource>();
		Landing2 					= transform.Find("Landing2").GetComponent<AudioSource>();
		Landing3 					= transform.Find("Landing3").GetComponent<AudioSource>();
		Landing_Strong 				= transform.Find("Landing_Strong").GetComponent<AudioSource>();
		SmashBaboon 				= transform.Find("SmashBaboon").GetComponent<AudioSource>();
		Level_Failed_Popup 			= transform.Find("Level_Failed_Popup").GetComponent<AudioSource>();
		Level_Completed_Popup 		= transform.Find("Level_Completed_Popup").GetComponent<AudioSource>();
		CollectCoin 				= transform.Find("CollectCoin").GetComponent<AudioSource>();
		CollectCoin_2nd 			= transform.Find("CollectCoin_2nd").GetComponent<AudioSource>();
		CollectCoin_3rd 			= transform.Find("CollectCoin_3rd").GetComponent<AudioSource>();
		GetStar 					= transform.Find("GetStar").GetComponent<AudioSource>();
		GetStar2 					= transform.Find("GetStar2").GetComponent<AudioSource>();
		GetStar3					= transform.Find("GetStar3").GetComponent<AudioSource>();
		BackgroundMusic_Gameplay	= transform.Find("BackgroundMusic_Gameplay").GetComponent<AudioSource>();
		BackgroundMusic_Menu		= transform.Find("BackgroundMusic_Menu").GetComponent<AudioSource>();
		CoinsSpent					= transform.Find("CoinsSpent").GetComponent<AudioSource>();
		NoMoreCoins					= transform.Find("NoMoreCoins").GetComponent<AudioSource>();
		Biljka_Ugriz_NEW			= transform.Find("PiranhaPlantBite").GetComponent<AudioSource>();
		Collect_Banana_NEW			= transform.Find("Collect_Banana").GetComponent<AudioSource>();
		Collect_Diamond_NEW			= transform.Find("Collect_Diamond").GetComponent<AudioSource>();
		Collect_PowerUp_NEW			= transform.Find("Collect_PowerUp").GetComponent<AudioSource>();
		Glide_NEW					= transform.Find("Glide").GetComponent<AudioSource>();
		OtkljucavanjeNivoa_NEW		= transform.Find("LevelUnlock").GetComponent<AudioSource>();
		SmashGorilla_NEW			= transform.Find("SmashGorilla").GetComponent<AudioSource>();
		Otvaranje_Kovcega_NEW		= transform.Find("UnlockChest").GetComponent<AudioSource>();
		Bure_Eksplozija_NEW			= transform.Find("BarrelExplode").GetComponent<AudioSource>();
		MushroomBounce				= transform.Find("MushroomBounce").GetComponent<AudioSource>();
		Siljci						= transform.Find("SpikesHit").GetComponent<AudioSource>();
		TNTBure_Eksplozija			= transform.Find("TNTBarrelExplode").GetComponent<AudioSource>();
		LooseShield					= transform.Find("LooseShield").GetComponent<AudioSource>();
		MajmunUtepan				= transform.Find("MonkeyKilled").GetComponent<AudioSource>();


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

	public static void Play_Button_MusicOn()
	{
		if(Button_MusicOn.clip != null)
			Button_MusicOn.Play();
	}

	public static void Play_Button_SoundOn()
	{
		if(Button_SoundOn.clip != null)
			Button_SoundOn.Play();
	}

	public static void Play_Button_Play()
	{
		if(Button_Play.clip != null)
			Button_Play.Play();
	}

	public static void Play_Button_GoBack()
	{
		if(Button_GoBack.clip != null)
			Button_GoBack.Play();
	}

	public static void Play_Button_OpenWorld()
	{
		if(Button_OpenWorld.clip != null)
			Button_OpenWorld.Play();
	}

	public static void Play_Button_OpenLevel()
	{
		if(Button_OpenLevel.clip != null)
			Button_OpenLevel.Play();
	}

	public static void Play_Button_Pause()
	{
		if(Button_OpenLevel.clip != null)
			Button_OpenLevel.Play();
	}

	public static void Play_Button_NextLevel()
	{
		if(Button_NextLevel.clip != null)
			Button_NextLevel.Play();
	}

	public static void Play_Button_RestartLevel()
	{
		if(Button_RestartLevel.clip != null)
			Button_RestartLevel.Play();
	}

	public static void Play_Button_LockedLevel_Click()
	{
		if(Button_LockedLevel_Click.clip != null)
			Button_LockedLevel_Click.Play();
	}

	public static void Play_Run()
	{
		if(Run.clip != null)
		{
			Run.pitch = Random.Range(0.9f,1.8f);
			Run.Play();
		}
	}

	public static void Stop_Run()
	{
		if(Run.clip != null)
			Run.Stop();
	}

	public static void Play_Jump()
	{
		int zvuk = Random.Range(1,4);
		switch(zvuk)
		{
			case 1: if(Jump1.clip != null) Jump1.Play(); break;
			case 2: if(Jump2.clip != null) Jump2.Play(); break;
			case 3: if(Jump3.clip != null) Jump3.Play(); break;
		}
	}

	public static void Play_VoiceJump()
	{
		int zvuk = Random.Range(1,56);
		switch(zvuk)
		{
			case 1: if(VoiceJump1.clip != null) VoiceJump1.Play(); break;
			case 2: if(VoiceJump2.clip != null) VoiceJump2.Play(); break;
			case 3: if(VoiceJump3.clip != null) VoiceJump3.Play(); break;
			case 4: if(VoiceJump4.clip != null) VoiceJump4.Play(); break;
			case 5: if(VoiceJump5.clip != null) VoiceJump5.Play(); break;
			case 6: if(VoiceJump6.clip != null) VoiceJump6.Play(); break;
			case 7: if(VoiceJump7.clip != null) VoiceJump7.Play(); break;
			case 8: if(VoiceJump8.clip != null) VoiceJump8.Play(); break;
		}
	}

	public static void Play_Landing()
	{
		int zvuk = Random.Range(1,4);
		switch(zvuk)
		{
			case 1: if(Landing1.clip != null) Landing1.Play(); break;
			case 2: if(Landing2.clip != null) Landing2.Play(); break;
			case 3: if(Landing3.clip != null) Landing3.Play(); break;
		}
	}

	public static void Play_Landing_Strong()
	{
		if(Landing_Strong.clip != null)
			Landing_Strong.Play();
	}

	public static void Play_SmashBaboon()
	{
		zvukZaBabuna++;
		if(zvukZaBabuna % 2 == 0)
		{
			if(SmashGorilla_NEW.clip != null)
				SmashGorilla_NEW.Play();
		}
		else
		{
			if(SmashBaboon.clip != null)
				SmashBaboon.Play();
		}
	}

	public static void Play_Level_Failed_Popup()
	{
		if(Level_Failed_Popup.clip != null)
			Level_Failed_Popup.Play();
	}

	public static void Play_Level_Completed_Popup()
	{
		if(Level_Completed_Popup.clip != null)
			Level_Completed_Popup.Play();
	}

	public static void Play_CollectCoin()
	{
		if(CollectCoin.clip != null)
			CollectCoin.Play();
	}

	public static void Play_CollectCoin_2nd()
	{
		if(CollectCoin_2nd.clip != null)
			CollectCoin_2nd.Play();
	}

	public static void Play_CollectCoin_3rd()
	{
		if(CollectCoin_3rd.clip != null)
			CollectCoin_3rd.Play();
	}

	public static void Play_GetStar()
	{
		if(GetStar.clip != null)
			GetStar.Play();
	}

	public static void Play_GetStar2()
	{
		if(GetStar2.clip != null)
			GetStar2.Play();
	}

	public static void Play_GetStar3()
	{
		if(GetStar3.clip != null)
			GetStar3.Play();
	}
	
	public static void Play_CoinsSpent()
	{
		if(CoinsSpent.clip != null)
			CoinsSpent.Play();
	}

	public static void Play_NoMoreCoins()
	{
		if(NoMoreCoins.clip != null)
			NoMoreCoins.Play();
	}

	public static void Play_BiljkaUgriz()
	{
		if(Biljka_Ugriz_NEW.clip != null)
			Biljka_Ugriz_NEW.Play();
	}

	public static void Play_CollectBanana()
	{
		if(Collect_Banana_NEW.clip != null)
			Collect_Banana_NEW.Play();
	}

	public static void Play_CollectDiamond()
	{
		if(Collect_Diamond_NEW.clip != null)
			Collect_Diamond_NEW.Play();
	}

	public static void Play_CollectPowerUp()
	{
		if(Collect_PowerUp_NEW.clip != null)
			Collect_PowerUp_NEW.Play();
	}

	public static void Play_Glide()
	{
		if(Glide_NEW.clip != null)
			Glide_NEW.Play();
	}

	public static void Stop_Glide()
	{
		if(Glide_NEW.clip != null)
			Glide_NEW.Stop();
	}

	public static void Play_OtkljucavanjeNivoa()
	{
		if(OtkljucavanjeNivoa_NEW.clip != null)
			OtkljucavanjeNivoa_NEW.Play();
	}

	public static void Play_SmashGorilla()
	{
		if(SmashGorilla_NEW.clip != null)
			SmashGorilla_NEW.Play();
	}

	public static void Play_Otvaranje_Kovcega()
	{
		if(Otvaranje_Kovcega_NEW.clip != null)
			Otvaranje_Kovcega_NEW.Play();
	}

	public static void Play_Bure_Eksplozija()
	{
		if(Bure_Eksplozija_NEW.clip != null)
			Bure_Eksplozija_NEW.Play();
	}

	public static void Play_MushroomBounce()
	{
		if(MushroomBounce.clip != null)
			MushroomBounce.Play();
	}

	public static void Play_BackgroundMusic_Gameplay()
	{
		if(BackgroundMusic_Gameplay.clip != null)
			BackgroundMusic_Gameplay.Play();
	}

	public static void Play_BackgroundMusic_Menu()
	{
		if(BackgroundMusic_Menu.clip != null)
			BackgroundMusic_Menu.Play();
	}

	public static void Stop_BackgroundMusic_Gameplay()
	{
		if(BackgroundMusic_Gameplay.clip != null)
		{
			BackgroundMusic_Gameplay.Stop();
		}

	}

	public static void Stop_BackgroundMusic_Menu()
	{
		if(BackgroundMusic_Menu.clip != null)
			BackgroundMusic_Menu.Stop();
	}

	public static void Stop_Level_Failed_Popup()
	{
		if(Level_Failed_Popup.clip != null)
			Level_Failed_Popup.Stop();
	}

	public static void Play_Siljci()
	{
		if(Siljci.clip != null)
			Siljci.Play();
	}

	public static void Play_TNTBure_Eksplozija()
	{
		if(TNTBure_Eksplozija.clip != null)
			TNTBure_Eksplozija.Play();
	}

	public static void Play_LooseShield()
	{
		if(LooseShield.clip != null)
			LooseShield.Play();
	}

	public static void Play_MajmunUtepan()
	{
		if(MajmunUtepan.clip != null)
			MajmunUtepan.Play();
	}

}
