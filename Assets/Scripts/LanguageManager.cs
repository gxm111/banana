using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

public class LanguageManager : MonoBehaviour {
	/*
	 * Scene:All
	 * Object:LanguageManager
	 * Sadrzi tekstove za menije za izabrani jezik, kao i
	 * informaciju koji je jezik izabran ( u formatu _SkracenicaZaJezik e.g. _en )
	 * u statikom stringu chosenLanguage.  U okviru awake funkcije je neophodno da stoji
	 * DontDestroyOnLoad(gameObject) da bi manager u svim scenema
	 * Funkcije 
	 * void Awake()
	 * public static void RefreshTexts()
	 * public static string SplitTextIntoRows(string, int) 
	 * 
	 * Promena tekstova se vrsi promenom izabranog
	 * jezika i pozivanje funkcije RefreshTexts()
	 * RefreshTexts cita xmlove sa lokacije Resources/xmls/inGameText/SpaceDefence+chosenLanguage
	 * 
	 * SplitTextIntoRows se koristi za prilagodjavanje duzine tekstova redu.
	 * */
	
	public static string chosenLanguage="_en";


	//////////////////////////////////////////////////////
	//
	// kolekcija stringova koji ce se koristiti u toku igre
	//
	//////////////////////////////////////////////////////
	
	public static string Collect="Collect";
	public static string LogIn="Log In";
	public static string Free="Free";
	public static string Coins="Coins";
	public static string Music="Music";
	public static string Sound="Sound";
	public static string Language="Language";
	public static string Settings="Settings";
	public static string ResetProgress="Reset Progress";
	public static string ResetTutorials="Reset Tutorials";
	public static string LogOut="Log Out";
	public static string Leaderboard="Leaderboard";
	public static string InviteAndEarn="Invite and earn";
	public static string InviteFriendsAndEarn="Invite friends and earn";
	public static string FreeCoins="Free Coins";
	public static string Shop="Shop";
	public static string Customize="Customize";
	public static string PowerUps="Power-Ups";
	public static string WatchVideo="Watch Video";
	public static string Banana="Banana";
	public static string DoubleCoins="Double Coins";
	public static string CoinsMagnet="Coins Magnet";
	public static string Shield="Shield";
	public static string New="New";
	public static string Preview="Preview";
	public static string Buy="Buy";
	public static string Equip="Equip";
	public static string Unequip="Unequip";
	public static string ShareAndEarn="Share and earn";
	public static string PostAndEarn="Post and earn";
	public static string TweetAndEarn="Tweet and earn";
	public static string LogInAndEarn="Log In and earn";	
	public static string Level="Level";
	public static string Mission="Mission";
	public static string Invite="Invite";
	public static string BonusLevel="Bonus Level";
	public static string Unlock="Unlock";
	public static string No="No";
	public static string Yes="Yes";
	public static string NoVideo="No videos available, please try again later";
	public static string NotEnoughBananas="Not enough bananas";
	public static string Loading="Loading";
	public static string Tip="Tip";
	public static string Tips="Tips";
	public static string ComingSoon="Coming Soon";
	public static string TapScreenToStart="Tap Screen To Start!";
	public static string Pause="Pause";
	public static string KeepPlaying="Keep Playing";
	public static string LevelFailed="Level Failed";
	public static string LevelCompleted="Level Completed";
	public static string RateThisGame="Rate this game";
	public static string DoYouLikeOurGame="Do you like our game?";
	public static string Cancel="Cancel";
	public static string Rate="Rate";
	public static string Congratulations="Congratulations!";
	public static string NewLevelsComingSoon="New levels coming soon!";
	public static string TutorialTapJump="1-TAP anywhere to JUMP\n2-TAP and HOLD to JUMP HIGHER";
	//public static string TutorialTapJumpHigher="2-TAP and HOLD to JUMP HIGHER";
	public static string TutorialGlide="When in the AIR you can TAP and HOLD to GLIDE slowly to the ground";
	public static string TutorialSwipe="SWIPE DOWN at great heights to perform a deadly super drop!";
	public static string NoInternet="No internet";
	public static string CheckInternet="Check your internet connection";
	public static string HowWouldYouRate="How would you rate our game?";
	public static string Downloading="Downloading...";
	public static string Maintenance="Maintenance";
	public static string BeBackSoon="We'll be back soon";
	public static string Ok="OK";
	public static string Reward="Reward";
	public static string DailyReward="Daily Reward";
	public static string Day = "Day";
	public static string Play = "Play";
	public static string BananaIsland = "Banana Island";
	public static string SavannaIsland = "Savanna Island";
	public static string JungleIsland = "Jungle Island";
	public static string TempleIsland = "Temple Island";
	public static string VolcanoIsland = "Volcano Island";
	public static string Completed = "Completed";
	public static string Share = "Share";
	public static string FollowUsOnFacebook = "Follow us on Facebook";
	public static string LoadingTip1 = "Don't miss Tips and Tricks in the Loading Screen, they can be useful.";
	public static string LoadingTip2 = "Want to keep playing? Spend one of your bananas to continue running.";
	public static string LoadingTip3 = "Jump on enemies' heads to knock'em out.";
	public static string FrozenIsland = "Frozen Island";





	//koristi se da naznaci da li izabrani jezik poseduje blanko znake
	// za japanski i kineski jezik (moguce jos neki) treba da ima 
	//vrednost false
	public static bool hasWhiteSpaces=true;
	
	/////////////////////////////////////////////////////
	//
	// kraj kolekcije stringova 
	//
	//////////////////////////////////////////////////

	//Poziva se jednom kad se startuje igra
	void Awake()
	{
		transform.name="LanguageManager";
		DontDestroyOnLoad(gameObject);
		RefreshTexts();
		if(PlayerPrefs.HasKey("choosenLanguage"))
			chosenLanguage = PlayerPrefs.GetString("choosenLanguage");

		Debug.Log("Chosen Languafe iz Language Mangaer: " + chosenLanguage);
	}
	
	//Potrebno je pozvati se kad se promeni izabrani jezik.
	//Menja kolekciju stringova na vrednosti za odgovarajuci jezik
	public static void RefreshTexts()
	{
		TextAsset aset =(TextAsset)Resources.Load("xmls/inGameText/Language"+chosenLanguage);
		XElement xmlNov= XElement.Parse(aset.ToString());
		IEnumerable<XElement> xmls = xmlNov.Elements();	
		int number=xmls.Count();
		Debug.Log ("Ukupno ima "+number+" xml elemenata");
		Collect=xmls.ElementAt(0).Value;
		LogIn=xmls.ElementAt(1).Value;
		Free=xmls.ElementAt(2).Value;
		Coins=xmls.ElementAt(3).Value;
		Music=xmls.ElementAt(4).Value;
		Sound=xmls.ElementAt(5).Value;
		Language=xmls.ElementAt(6).Value;
		Settings=xmls.ElementAt(7).Value;
		ResetProgress=xmls.ElementAt(8).Value;
		ResetTutorials=xmls.ElementAt(9).Value;
		LogOut=xmls.ElementAt(10).Value;
		Leaderboard=xmls.ElementAt(11).Value;
		InviteAndEarn=xmls.ElementAt(12).Value;
		InviteFriendsAndEarn=xmls.ElementAt(13).Value;
		FreeCoins=xmls.ElementAt(14).Value;
		Shop=xmls.ElementAt(15).Value;
		Customize=xmls.ElementAt(16).Value;
		PowerUps=xmls.ElementAt(17).Value;
		WatchVideo=xmls.ElementAt(18).Value;
		Banana=xmls.ElementAt(19).Value;
		DoubleCoins=xmls.ElementAt(20).Value;
		CoinsMagnet=xmls.ElementAt(21).Value;
		Shield=xmls.ElementAt(22).Value;
		New=xmls.ElementAt(23).Value;
		Preview=xmls.ElementAt(24).Value;
		Buy=xmls.ElementAt(25).Value;
		Equip=xmls.ElementAt(26).Value;
		Unequip=xmls.ElementAt(27).Value;
		ShareAndEarn=xmls.ElementAt(28).Value;
		PostAndEarn=xmls.ElementAt(29).Value;
		TweetAndEarn=xmls.ElementAt(30).Value;
		LogInAndEarn=xmls.ElementAt(31).Value;
		Level=xmls.ElementAt(32).Value;
		Mission=xmls.ElementAt(33).Value;
		Invite=xmls.ElementAt(34).Value;
		BonusLevel=xmls.ElementAt(35).Value;
		Unlock=xmls.ElementAt(36).Value;
		No=xmls.ElementAt(37).Value;
		Yes=xmls.ElementAt(38).Value;
		NoVideo=xmls.ElementAt(39).Value;
		NotEnoughBananas=xmls.ElementAt(40).Value;
		Loading=xmls.ElementAt(41).Value;
		Tip=xmls.ElementAt(42).Value;
		Tips=xmls.ElementAt(43).Value;
		ComingSoon=xmls.ElementAt(44).Value;
		TapScreenToStart=xmls.ElementAt(45).Value;
		Pause=xmls.ElementAt(46).Value;
		KeepPlaying=xmls.ElementAt(47).Value;
		LevelFailed=xmls.ElementAt(48).Value;
		LevelCompleted=xmls.ElementAt(49).Value;
		RateThisGame=xmls.ElementAt(50).Value;
		DoYouLikeOurGame=xmls.ElementAt(51).Value;
		Cancel=xmls.ElementAt(52).Value;
		Rate=xmls.ElementAt(53).Value;
		Congratulations=xmls.ElementAt(54).Value;
		NewLevelsComingSoon=xmls.ElementAt(55).Value;
		TutorialTapJump=xmls.ElementAt(56).Value;
		//TutorialTapJumpHigher=xmls.ElementAt(57).Value;
		TutorialGlide=xmls.ElementAt(57).Value;
		TutorialSwipe=xmls.ElementAt(58).Value;
		NoInternet=xmls.ElementAt(59).Value;
		CheckInternet=xmls.ElementAt(60).Value;
		HowWouldYouRate=xmls.ElementAt(61).Value;
		Downloading=xmls.ElementAt(62).Value;
		Maintenance=xmls.ElementAt(63).Value;
		BeBackSoon=xmls.ElementAt(64).Value;
		Ok=xmls.ElementAt(65).Value;
		Reward=xmls.ElementAt(66).Value;
		DailyReward=xmls.ElementAt(67).Value;
		Day=xmls.ElementAt(68).Value;
		Play=xmls.ElementAt(69).Value;
		BananaIsland=xmls.ElementAt(70).Value;
		SavannaIsland=xmls.ElementAt(71).Value;
		JungleIsland=xmls.ElementAt(72).Value;
		TempleIsland=xmls.ElementAt(73).Value;
		VolcanoIsland =xmls.ElementAt(74).Value;
		Completed =xmls.ElementAt(75).Value;
		Share =xmls.ElementAt(76).Value;
		FollowUsOnFacebook =xmls.ElementAt(77).Value;
		LoadingTip1 = xmls.ElementAt(78).Value;
		LoadingTip2 = xmls.ElementAt(79).Value;
		LoadingTip3 = xmls.ElementAt(80).Value;
		//RemoveAds = xmls.ElementAt(81).Value;
		//Restore = xmls.ElementAt(82).Value;
		FrozenIsland = xmls.ElementAt(83).Value;
	}
	
	//Poziva se kad je potrebno da tekst ne zauzima vise od "rowLimit" karaktera
	//u redu
	public string SplitTextIntoRows(string originalText,int rowLimit)
	{
		string text,finalText;
		text=finalText="";
		originalText=originalText.Replace("\n"," ");
		int length=originalText.Length;
		if(length<rowLimit) return originalText;
		bool firstRow=true;
		string[] split= originalText.Split(' ');
		if(chosenLanguage=="_jp"||chosenLanguage=="_ch")
		{
			split=new string[originalText.Length];
			for(int i=0;i<originalText.Length;i++)
			{
				split[i]=originalText[i].ToString();
			}
		}
		for(int i=0;i<split.Length;)
		{
			if(split.Length>i)
			{
				if(split[i].Length>=rowLimit)
				{
					text+=split[i++]+((chosenLanguage!="_jp"&&chosenLanguage!="_ch")?" ":"");
				}
				else 
				{
					while(((text.Length+split[i].Length)<rowLimit)&&i<split.Length)
					{
						text+=split[i++]+((chosenLanguage!="_jp"&&chosenLanguage!="_ch")?" ":"");
						if(i>=split.Length) break;
					}
				}	
				if(firstRow)
				{
					finalText=text;
					firstRow=false;
				}
				else
				{
					finalText+="\n"+text;
				}
				text="";
			}
		}
		return finalText;
	}
	
	string SplitInHalf(string toSplit)
	{
	//	string text;
		return toSplit;
	}

	public IEnumerator helpfunk(TextMesh toAdjust)
	{
		//Debug.Log(toAdjust.text);
		//	Bounds newBounds=toAdjust.renderer.bounds;
	//	float oldCharSize=toAdjust.characterSize;
		string oldText=toAdjust.text;
		while(((toAdjust.GetComponent<Renderer>().bounds.size.x *1.1f)<toAdjust.GetComponent<Collider>().bounds.size.x)&&((toAdjust.GetComponent<Renderer>().bounds.size.y *1.1f)<toAdjust.GetComponent<Collider>().bounds.size.y))
		{
			toAdjust.characterSize*=1.1f;
		}
		while(((toAdjust.GetComponent<Renderer>().bounds.size.x *0.9f)>toAdjust.GetComponent<Collider>().bounds.size.x)&&((toAdjust.GetComponent<Renderer>().bounds.size.y *0.9f)>toAdjust.GetComponent<Collider>().bounds.size.y))
		{
			toAdjust.characterSize*=0.9f;
		}
		string[] wholeText= oldText.Split(' ');
		if(chosenLanguage=="_jp"||chosenLanguage=="_ch")
		{
			char[] help= oldText.ToCharArray();
			wholeText= new string[help.Length];
			for(int i =0;i<wholeText.Length;i++)
			{
				wholeText[i]= help[i].ToString();
			}
		}
		bool again=false;
		float charSize=toAdjust.characterSize;
		do
		{again=false;
			toAdjust.characterSize=charSize;
			int counter=1;
			toAdjust.text=wholeText[0];
			string previous=toAdjust.text;
			string helpText="";
			//Debug.Log(wholeText.Length+"##"+oldText);
			while((counter<wholeText.Length))
			{
				int helpCounter=0;
				while(((toAdjust.GetComponent<Renderer>().bounds.size.x<toAdjust.GetComponent<Collider>().bounds.size.x)||toAdjust.text==wholeText[((counter<wholeText.Length)?counter:wholeText.Length-1)])&&counter<wholeText.Length)
				{
					previous=toAdjust.text;
					toAdjust.text+=((chosenLanguage=="_jp"||chosenLanguage=="_ch")?"":" ")+wholeText[counter];
					counter++;
					helpCounter++;
					yield return null;
					yield return new WaitForSeconds(0.1f);
				}
				if(helpCounter==0){Debug.Log("again");again=true;break;}
				
				if((counter<wholeText.Length)){toAdjust.text=wholeText[counter-1];Debug.Log(counter+"<"+wholeText.Length);
				helpText+=previous+"\n";}
				else {
					helpText+=toAdjust.text;Debug.Log(counter+">="+wholeText.Length);}
				
				
				Debug.Log(wholeText.Length+"##"+counter);
				yield return null;
				yield return new WaitForSeconds(0.1f);
				
			}
			toAdjust.text=helpText;
			charSize*=0.9f;
			Debug.Log("enddd");
		}
		while(toAdjust.GetComponent<Renderer>().bounds.size.y>toAdjust.GetComponent<Collider>().bounds.size.y ||again);
	}
	
	public  void AdjustFontSize(TextMesh toAdjust,bool rowSplit)
	{
		
		
	//	float oldCharSize=toAdjust.characterSize;
		string oldText=toAdjust.text;
		
		//larger
		while(((toAdjust.GetComponent<Renderer>().bounds.size.x *1.1f)<toAdjust.GetComponent<Collider>().bounds.size.x)&&((toAdjust.GetComponent<Renderer>().bounds.size.y *1.1f)<toAdjust.GetComponent<Collider>().bounds.size.y))
		{
			toAdjust.characterSize*=1.1f;
		}
		if(!rowSplit)
		{
			while(((toAdjust.GetComponent<Renderer>().bounds.size.x )>toAdjust.GetComponent<Collider>().bounds.size.x)&&((toAdjust.GetComponent<Renderer>().bounds.size.y )>toAdjust.GetComponent<Collider>().bounds.size.y))
			{
				toAdjust.characterSize*=0.9f;
			}
		}
		else
		{
			//oldCharSize=toAdjust.characterSize=1;
			string[] wholeText= oldText.Split(' ');
			if(chosenLanguage=="_jp"||chosenLanguage=="_ch")
			{
				char[] help= oldText.ToCharArray();
				wholeText= new string[help.Length];
				for(int i =0;i<wholeText.Length;i++)
				{
					wholeText[i]= help[i].ToString();
				}
			}
			bool again=false;
			float charSize=toAdjust.characterSize;
			do
			{
				again=false;
				toAdjust.characterSize=charSize;
				int counter=1;
				toAdjust.text=wholeText[0];
				string previous=toAdjust.text;
				string helpText="";
				//Debug.Log(wholeText.Length+"##"+oldText);
				while((counter<wholeText.Length))
				{
					int helpCounter=0;
					while(((toAdjust.GetComponent<Renderer>().bounds.size.x<toAdjust.GetComponent<Collider>().bounds.size.x)||toAdjust.text==wholeText[((counter<wholeText.Length)?counter:wholeText.Length-1)])&&counter<wholeText.Length)
					{
						previous=toAdjust.text;
						toAdjust.text+=((chosenLanguage=="_jp"||chosenLanguage=="_ch")?"":" ")+wholeText[counter];
						counter++;
						helpCounter++;
						//yield return null;
						//yield return new WaitForSeconds(0.1f);
					}
					if(helpCounter==0){Debug.Log("again");again=true;break;}
					
					if((counter<wholeText.Length)){toAdjust.text=wholeText[counter-1];Debug.Log(counter+"<"+wholeText.Length);
					helpText+=previous+"\n";}
					else {
						helpText+=toAdjust.text;Debug.Log(counter+">="+wholeText.Length);}
					
					
					Debug.Log(wholeText.Length+"##"+counter);
					//yield return null;
					//yield return new WaitForSeconds(0.1f);
					
				}
				toAdjust.text=helpText;
				charSize*=0.9f;
				Debug.Log("enddd");
			}
			while(((toAdjust.GetComponent<Renderer>().bounds.size.y>toAdjust.GetComponent<Collider>().bounds.size.y)||(toAdjust.GetComponent<Renderer>().bounds.size.x>toAdjust.GetComponent<Collider>().bounds.size.x)) ||again);
			Debug.Log((toAdjust.GetComponent<Renderer>().bounds.size.y>toAdjust.GetComponent<Collider>().bounds.size.y)+"###"+(toAdjust.GetComponent<Renderer>().bounds.size.x>toAdjust.GetComponent<Collider>().bounds.size.x)+"###"+again);
		}
		//toAdjust.text=previous;
	/*	while((toAdjust.renderer.bounds.size.x>toAdjust.collider.bounds.size.x) && (toAdjust.renderer.bounds.size.y<toAdjust.collider.bounds.size.y ))
		{
			
		}
		
		if(toAdjust.renderer.bounds.size.x>toAdjust.collider.bounds.size.x )
		{
			toAdjust.characterSize*=0.9f;
			AdjustFontSize(toAdjust);
		}
		else
		if(toAdjust.renderer.bounds.size.y>toAdjust.collider.bounds.size.y )
		{
			toAdjust.characterSize*=0.9f;
			AdjustFontSize(toAdjust);
		}
		else  if(((toAdjust.renderer.bounds.size.x *1.1f)<toAdjust.collider.bounds.size.x)&&((toAdjust.renderer.bounds.size.y *1.1f)<toAdjust.collider.bounds.size.y))
		{
			toAdjust.characterSize*=1.1f;
			AdjustFontSize(toAdjust);
		}*/
	}
	
	
	
	
}


