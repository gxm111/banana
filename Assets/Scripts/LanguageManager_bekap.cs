//using UnityEngine;
//using System.Collections;
//using System.Xml;
//using System.IO;
//using System.Collections.Generic;
//using System.Xml.Linq;
//using System.Linq;
//
////Klasa za dodavanje funkcije TextMesh-u
//internal static class TextMeshExtensions  {
//	
//	/// <summary>
//	/// Podesavanje velicine teksta u odnosu na collider koji je vezan za taj objekat. NAPOMENA: Proverava se samo velicina collidera ne i njegova pozicija
//	/// </summary>
//	/// <param name='rowSplit'>
//	/// Da li funkcija treba da podeli tekst u redove.
//	/// </param>
//	/// <param name='hasWhiteSpaces'>
//	/// Da li se blanko znak koristi za odvajanja reci ili ne.
//	/// </param>
//	public static void AdjustFontSize(this TextMesh toAdjust,bool rowSplit, bool hasWhiteSpaces)
//	{
//		//mora da postoji collider na osnovu koga proverava velicinu
//		//zastita ako nije postavljen
//		if(toAdjust.collider!=null)
//		{
//			string oldText=toAdjust.text;
//			//provera ako treba da se poveca tekst
//			while(((toAdjust.renderer.bounds.size.x *1.1f)<toAdjust.collider.bounds.size.x)&&((toAdjust.renderer.bounds.size.y *1.1f)<toAdjust.collider.bounds.size.y))
//			{
//				toAdjust.characterSize*=1.1f;
//			}
//			//ako se ne deli u nove redovima
//			if(!rowSplit)
//			{
//				while(((toAdjust.renderer.bounds.size.x )>toAdjust.collider.bounds.size.x)&&((toAdjust.renderer.bounds.size.y )>toAdjust.collider.bounds.size.y))
//				{
//					toAdjust.characterSize*=0.9f;
//				}
//			}
//			// ako se deli tekst u nove redove
//			else
//			{
//				string[] wholeText= oldText.Split(' ');
//				//kako se odvajaju reci
//				if(!hasWhiteSpaces)
//				{
//					char[] help= oldText.ToCharArray();
//					wholeText= new string[help.Length];
//					for(int i =0;i<wholeText.Length;i++)
//					{
//						wholeText[i]= help[i].ToString();
//					}
//				}
//				//za slucaj da je samo jedna rec
//				if(wholeText.Length==1)
//				{
//					while(((toAdjust.renderer.bounds.size.x )>toAdjust.collider.bounds.size.x)&&((toAdjust.renderer.bounds.size.y )>toAdjust.collider.bounds.size.y))
//					{
//						toAdjust.characterSize*=0.9f;
//					}
//				}
//				else
//				{//logika za vise reci u vise redova
//					bool again=false;
//					float charSize=toAdjust.characterSize;
//					do
//					{
//						again=false;
//						toAdjust.characterSize=charSize;
//						int counter=1;
//						toAdjust.text=wholeText[0];
//						string previous=toAdjust.text;
//						string helpText="";
//						while((counter<wholeText.Length))
//						{
//							int helpCounter=0;
//							while(((toAdjust.renderer.bounds.size.x<toAdjust.collider.bounds.size.x)||toAdjust.text==wholeText[((counter<wholeText.Length)?counter:wholeText.Length-1)])&&counter<wholeText.Length)
//							{
//								previous=toAdjust.text;
//								toAdjust.text+=((!hasWhiteSpaces)?"":" ")+wholeText[counter];
//								counter++;
//								helpCounter++;
//							}
//							if(helpCounter==0){Debug.Log("again");again=true;break;}
//							
//							if((counter<wholeText.Length)){toAdjust.text=wholeText[counter-1];Debug.Log(counter+"<"+wholeText.Length);
//							helpText+=previous+"\n";}
//							else {
//								helpText+=toAdjust.text;Debug.Log(counter+">="+wholeText.Length);}
//						}
//						toAdjust.text=helpText;
//						charSize*=0.9f;
//					}
//					while(((toAdjust.renderer.bounds.size.y>toAdjust.collider.bounds.size.y)||(toAdjust.renderer.bounds.size.x>toAdjust.collider.bounds.size.x)) ||again);
//				}
//			}
//		}
//		else
//		{
//			Debug.Log("No collider attached to "+ toAdjust.name + " object in" + Application.loadedLevelName +" Scene");
//		}
//	}
//}
//
//public class LanguageManager_bekap : MonoBehaviour {
//	/*
//	 * Scene:All
//	 * Object:LanguageManager
//	 * Sadrzi tekstove za menije za izabrani jezik, kao i
//	 * informaciju koji je jezik izabran ( u formatu _SkracenicaZaJezik e.g. _en )
//	 * u statikom stringu chosenLanguage.  U okviru awake funkcije je neophodno da stoji
//	 * DontDestroyOnLoad(gameObject) da bi manager u svim scenema
//	 * Funkcije 
//	 * void Awake()
//	 * public static void RefreshTexts()
//	 * public static string SplitTextIntoRows(string, int) 
//	 * 
//	 * Promena tekstova se vrsi promenom izabranog
//	 * jezika i pozivanje funkcije RefreshTexts()
//	 * RefreshTexts cita xmlove sa lokacije Resources/xmls/inGameText/SpaceDefence+chosenLanguage
//	 * 
//	 * SplitTextIntoRows se koristi za prilagodjavanje duzine tekstova redu.
//	 * */
//	
//	public static string chosenLanguage="_en";
//	
//	//////////////////////////////////////////////////////
//	//
//	// kolekcija stringova koji ce se koristiti u toku igre
//	//
//	//////////////////////////////////////////////////////
//	
//	public static string playText="Play";
//	public static string startText="Start";
//	public static string quitText="Quit";
//	public static string backText="Back";
//	public static string stageText="Stage";
//	public static string worldText="World";
//	//koristi se da naznaci da li izabrani jezik poseduje blanko znake
//	// za japanski i kineski jezik (moguce jos neki) treba da ima 
//	//vrednost false
//	public static bool hasWhiteSpaces=true;
//	
//	/////////////////////////////////////////////////////
//	//
//	// kraj kolekcije stringova 
//	//
//	//////////////////////////////////////////////////
//
//	//Poziva se jednom kad se startuje igra
//	void Awake()
//	{
//		transform.name="LanguageManager";
//		DontDestroyOnLoad(gameObject);
//		RefreshTexts();
//	}
//	
//	//Potrebno je pozvati se kad se promeni izabrani jezik.
//	//Menja kolekciju stringova na vrednosti za odgovarajuci jezik
//	public static void RefreshTexts()
//	{
//		TextAsset aset =(TextAsset)Resources.Load("xmls/inGameText/LevelSelect"+chosenLanguage);
//		XElement xmlNov= XElement.Parse(aset.ToString());
//		IEnumerable<XElement> xmls = xmlNov.Elements();	
//	//	int number=xmls.Count();
//		playText=xmls.ElementAt(0).Value;
//		startText=xmls.ElementAt(1).Value;
//		quitText=xmls.ElementAt(2).Value;
//		backText=xmls.ElementAt(3).Value;
//		stageText=xmls.ElementAt(4).Value;
//		worldText=xmls.ElementAt(5).Value;
//	}
//	
//	//Poziva se kad je potrebno da tekst ne zauzima vise od "rowLimit" karaktera
//	//u redu
//	public string SplitTextIntoRows(string originalText,int rowLimit)
//	{
//		string text,finalText;
//		text=finalText="";
//		originalText=originalText.Replace("\n"," ");
//		int length=originalText.Length;
//		if(length<rowLimit) return originalText;
//		bool firstRow=true;
//		string[] split= originalText.Split(' ');
//		if(chosenLanguage=="_jp"||chosenLanguage=="_zh")
//		{
//			split=new string[originalText.Length];
//			for(int i=0;i<originalText.Length;i++)
//			{
//				split[i]=originalText[i].ToString();
//			}
//		}
//		for(int i=0;i<split.Length;)
//		{
//			if(split.Length>i)
//			{
//				if(split[i].Length>=rowLimit)
//				{
//					text+=split[i++]+((chosenLanguage!="_jp"&&chosenLanguage!="_zh")?" ":"");
//				}
//				else 
//				{
//					while(((text.Length+split[i].Length)<rowLimit)&&i<split.Length)
//					{
//						text+=split[i++]+((chosenLanguage!="_jp"&&chosenLanguage!="_zh")?" ":"");
//						if(i>=split.Length) break;
//					}
//				}	
//				if(firstRow)
//				{
//					finalText=text;
//					firstRow=false;
//				}
//				else
//				{
//					finalText+="\n"+text;
//				}
//				text="";
//			}
//		}
//		return finalText;
//	}
//	
//	string SplitInHalf(string toSplit)
//	{
//	//	string text;
//		return toSplit;
//	}
//	
//	public IEnumerator helpfunk(TextMesh toAdjust)
//	{
//		//Debug.Log(toAdjust.text);
//		//	Bounds newBounds=toAdjust.renderer.bounds;
//	//	float oldCharSize=toAdjust.characterSize;
//		string oldText=toAdjust.text;
//		while(((toAdjust.renderer.bounds.size.x *1.1f)<toAdjust.collider.bounds.size.x)&&((toAdjust.renderer.bounds.size.y *1.1f)<toAdjust.collider.bounds.size.y))
//		{
//			toAdjust.characterSize*=1.1f;
//		}
//		while(((toAdjust.renderer.bounds.size.x *0.9f)>toAdjust.collider.bounds.size.x)&&((toAdjust.renderer.bounds.size.y *0.9f)>toAdjust.collider.bounds.size.y))
//		{
//			toAdjust.characterSize*=0.9f;
//		}
//		string[] wholeText= oldText.Split(' ');
//		if(chosenLanguage=="_jp"||chosenLanguage=="_zh")
//		{
//			char[] help= oldText.ToCharArray();
//			wholeText= new string[help.Length];
//			for(int i =0;i<wholeText.Length;i++)
//			{
//				wholeText[i]= help[i].ToString();
//			}
//		}
//		bool again=false;
//		float charSize=toAdjust.characterSize;
//		do
//		{again=false;
//			toAdjust.characterSize=charSize;
//			int counter=1;
//			toAdjust.text=wholeText[0];
//			string previous=toAdjust.text;
//			string helpText="";
//			//Debug.Log(wholeText.Length+"##"+oldText);
//			while((counter<wholeText.Length))
//			{
//				int helpCounter=0;
//				while(((toAdjust.renderer.bounds.size.x<toAdjust.collider.bounds.size.x)||toAdjust.text==wholeText[((counter<wholeText.Length)?counter:wholeText.Length-1)])&&counter<wholeText.Length)
//				{
//					previous=toAdjust.text;
//					toAdjust.text+=((chosenLanguage=="_jp"||chosenLanguage=="_zh")?"":" ")+wholeText[counter];
//					counter++;
//					helpCounter++;
//					yield return null;
//					yield return new WaitForSeconds(0.1f);
//				}
//				if(helpCounter==0){Debug.Log("again");again=true;break;}
//				
//				if((counter<wholeText.Length)){toAdjust.text=wholeText[counter-1];Debug.Log(counter+"<"+wholeText.Length);
//				helpText+=previous+"\n";}
//				else {
//					helpText+=toAdjust.text;Debug.Log(counter+">="+wholeText.Length);}
//				
//				
//				Debug.Log(wholeText.Length+"##"+counter);
//				yield return null;
//				yield return new WaitForSeconds(0.1f);
//				
//			}
//			toAdjust.text=helpText;
//			charSize*=0.9f;
//			Debug.Log("enddd");
//		}
//		while(toAdjust.renderer.bounds.size.y>toAdjust.collider.bounds.size.y ||again);
//	}
//	
//	public  void AdjustFontSize(TextMesh toAdjust,bool rowSplit)
//	{
//		
//		
//	//	float oldCharSize=toAdjust.characterSize;
//		string oldText=toAdjust.text;
//		
//		//larger
//		while(((toAdjust.renderer.bounds.size.x *1.1f)<toAdjust.collider.bounds.size.x)&&((toAdjust.renderer.bounds.size.y *1.1f)<toAdjust.collider.bounds.size.y))
//		{
//			toAdjust.characterSize*=1.1f;
//		}
//		if(!rowSplit)
//		{
//			while(((toAdjust.renderer.bounds.size.x )>toAdjust.collider.bounds.size.x)&&((toAdjust.renderer.bounds.size.y )>toAdjust.collider.bounds.size.y))
//			{
//				toAdjust.characterSize*=0.9f;
//			}
//		}
//		else
//		{
//			//oldCharSize=toAdjust.characterSize=1;
//			string[] wholeText= oldText.Split(' ');
//			if(chosenLanguage=="_jp"||chosenLanguage=="_zh")
//			{
//				char[] help= oldText.ToCharArray();
//				wholeText= new string[help.Length];
//				for(int i =0;i<wholeText.Length;i++)
//				{
//					wholeText[i]= help[i].ToString();
//				}
//			}
//			bool again=false;
//			float charSize=toAdjust.characterSize;
//			do
//			{
//				again=false;
//				toAdjust.characterSize=charSize;
//				int counter=1;
//				toAdjust.text=wholeText[0];
//				string previous=toAdjust.text;
//				string helpText="";
//				//Debug.Log(wholeText.Length+"##"+oldText);
//				while((counter<wholeText.Length))
//				{
//					int helpCounter=0;
//					while(((toAdjust.renderer.bounds.size.x<toAdjust.collider.bounds.size.x)||toAdjust.text==wholeText[((counter<wholeText.Length)?counter:wholeText.Length-1)])&&counter<wholeText.Length)
//					{
//						previous=toAdjust.text;
//						toAdjust.text+=((chosenLanguage=="_jp"||chosenLanguage=="_zh")?"":" ")+wholeText[counter];
//						counter++;
//						helpCounter++;
//						//yield return null;
//						//yield return new WaitForSeconds(0.1f);
//					}
//					if(helpCounter==0){Debug.Log("again");again=true;break;}
//					
//					if((counter<wholeText.Length)){toAdjust.text=wholeText[counter-1];Debug.Log(counter+"<"+wholeText.Length);
//					helpText+=previous+"\n";}
//					else {
//						helpText+=toAdjust.text;Debug.Log(counter+">="+wholeText.Length);}
//					
//					
//					Debug.Log(wholeText.Length+"##"+counter);
//					//yield return null;
//					//yield return new WaitForSeconds(0.1f);
//					
//				}
//				toAdjust.text=helpText;
//				charSize*=0.9f;
//				Debug.Log("enddd");
//			}
//			while(((toAdjust.renderer.bounds.size.y>toAdjust.collider.bounds.size.y)||(toAdjust.renderer.bounds.size.x>toAdjust.collider.bounds.size.x)) ||again);
//			Debug.Log((toAdjust.renderer.bounds.size.y>toAdjust.collider.bounds.size.y)+"###"+(toAdjust.renderer.bounds.size.x>toAdjust.collider.bounds.size.x)+"###"+again);
//		}
//		//toAdjust.text=previous;
//	/*	while((toAdjust.renderer.bounds.size.x>toAdjust.collider.bounds.size.x) && (toAdjust.renderer.bounds.size.y<toAdjust.collider.bounds.size.y ))
//		{
//			
//		}
//		
//		if(toAdjust.renderer.bounds.size.x>toAdjust.collider.bounds.size.x )
//		{
//			toAdjust.characterSize*=0.9f;
//			AdjustFontSize(toAdjust);
//		}
//		else
//		if(toAdjust.renderer.bounds.size.y>toAdjust.collider.bounds.size.y )
//		{
//			toAdjust.characterSize*=0.9f;
//			AdjustFontSize(toAdjust);
//		}
//		else  if(((toAdjust.renderer.bounds.size.x *1.1f)<toAdjust.collider.bounds.size.x)&&((toAdjust.renderer.bounds.size.y *1.1f)<toAdjust.collider.bounds.size.y))
//		{
//			toAdjust.characterSize*=1.1f;
//			AdjustFontSize(toAdjust);
//		}*/
//	}
//	
//	
//	
//	
//}
//
//
