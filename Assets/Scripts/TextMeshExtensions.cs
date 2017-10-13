using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

//Klasa za dodavanje funkcije TextMesh-u
using System.Text.RegularExpressions;


internal static class TextMeshExtensions  {
	
	/// <summary>
	/// Podesavanje velicine teksta u odnosu na collider koji je vezan za taj objekat. NAPOMENA: Proverava se samo velicina collidera ne i njegova pozicija
	/// </summary>
	/// <param name='rowSplit'>
	/// Da li funkcija treba da podeli tekst u redove.
	/// </param>
	/// <param name='hasWhiteSpaces'>
	/// Da li se blanko znak koristi za odvajanja reci ili ne.
	/// </param>
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
//								helpText+=previous+"\n";}
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

	/// <summary>
	/// Adjusts the size of the font.Version: 2.2p by KiEkNaI
	/// </summary>
	/// <param name="rowSplit">If set to <c>true</c> split rows.</param>
	/// <param name="hasWhiteSpaces">If set to <c>true</c> text is sepperated by white spaces.</param>
	/// <param name="increaseFont">If set to <c>true</c> will increase font.(Optional)</param>
	public static void AdjustFontSize(this TextMesh toAdjust,bool rowSplit, bool hasWhiteSpaces, bool increaseFont/* = true*/)
	{
		//mora da postoji collider na osnovu koga proverava velicinu
		//zastita ako nije postavljen
		if(toAdjust.GetComponent<Collider>()!=null)
		{
			if(toAdjust.GetComponent<Collider>().bounds.size.x==0||toAdjust.GetComponent<Collider>().bounds.size.y==0) return;
			string oldText=toAdjust.text;
			//provera ako treba da se poveca tekst
			if(increaseFont)
			{
				while(((toAdjust.GetComponent<Renderer>().bounds.size.x *1.1f)<toAdjust.GetComponent<Collider>().bounds.size.x)&&((toAdjust.GetComponent<Renderer>().bounds.size.y *1.1f)<toAdjust.GetComponent<Collider>().bounds.size.y))
				{
					toAdjust.characterSize*=1.1f;
				}
			}
			else
			{
				if(((toAdjust.GetComponent<Renderer>().bounds.size.x )<toAdjust.GetComponent<Collider>().bounds.size.x)&&((toAdjust.GetComponent<Renderer>().bounds.size.y )<toAdjust.GetComponent<Collider>().bounds.size.y))
				{
					return;
				}
			}
			//ako se ne deli u nove redovima
			if(!rowSplit)
			{
				while(((toAdjust.GetComponent<Renderer>().bounds.size.x )>toAdjust.GetComponent<Collider>().bounds.size.x)||((toAdjust.GetComponent<Renderer>().bounds.size.y )>toAdjust.GetComponent<Collider>().bounds.size.y))
				{
					toAdjust.characterSize*=0.9f;
				}
			}
			// ako se deli tekst u nove redove
			else
			{
				string[] wholeText= oldText.Split(' ');
				if(!hasWhiteSpaces)
				{
					char[] help= oldText.ToCharArray();
					wholeText= new string[help.Length];
					for(int i =0;i<wholeText.Length;i++)
					{
						wholeText[i]= help[i].ToString();
					}
				}
				if(wholeText.Length==1)
				{
					while(((toAdjust.GetComponent<Renderer>().bounds.size.x )>toAdjust.GetComponent<Collider>().bounds.size.x)||((toAdjust.GetComponent<Renderer>().bounds.size.y )>toAdjust.GetComponent<Collider>().bounds.size.y))
					{
						toAdjust.characterSize*=0.9f;
					}
					return;
				}
				//kako se odvajaju reci
				//za slucaj da je samo jedna rec
				if(wholeText.Length==1)
				{
					while(((toAdjust.GetComponent<Renderer>().bounds.size.x )>toAdjust.GetComponent<Collider>().bounds.size.x)&&((toAdjust.GetComponent<Renderer>().bounds.size.y )>toAdjust.GetComponent<Collider>().bounds.size.y))
					{
						toAdjust.characterSize*=0.9f;
					}
				}
				else
				{
					//logika za vise reci u vise redova
					bool again=false;
					toAdjust.text=wholeText[0];
					int novCount=1;
					while(((toAdjust.GetComponent<Renderer>().bounds.size.x *1.1f)<toAdjust.GetComponent<Collider>().bounds.size.x)&&((toAdjust.GetComponent<Renderer>().bounds.size.y *1.1f)<toAdjust.GetComponent<Collider>().bounds.size.y))
					{
						toAdjust.characterSize*=1.1f;
					}
					int nPoz=Regex.Matches(toAdjust.text, "\n").Count;
					//int startNpoz=nPoz;
					//Debug.Log("s: "+toAdjust.text);
					while(novCount<wholeText.Length)
					{ 
						nPoz=Regex.Matches(toAdjust.text, "\n").Count;;
						if(!again)
						{
							toAdjust.text+="\n"+wholeText[novCount++];//Debug.Log("s: "+toAdjust.text);
						}
						else
						{
							string back= toAdjust.text;
							toAdjust.text+=((hasWhiteSpaces)?" ":"")+wholeText[novCount++];//Debug.Log("s: "+toAdjust.text);
							if(((toAdjust.GetComponent<Renderer>().bounds.size.y>toAdjust.GetComponent<Collider>().bounds.size.y)||(toAdjust.GetComponent<Renderer>().bounds.size.x>toAdjust.GetComponent<Collider>().bounds.size.x)))
							{
								toAdjust.text=back+"\n"+wholeText[novCount-1];;//Debug.Log("s: "+toAdjust.text);
							}
						}
						while(((toAdjust.GetComponent<Renderer>().bounds.size.y>toAdjust.GetComponent<Collider>().bounds.size.y)||(toAdjust.GetComponent<Renderer>().bounds.size.x>toAdjust.GetComponent<Collider>().bounds.size.x)))
						{
							string back= toAdjust.text;
							toAdjust.text=ReplaceNextOccurence(toAdjust.text,"\n",((hasWhiteSpaces)?" ":""),nPoz++);
							//Debug.Log("s: "+toAdjust.text);
							if(((toAdjust.GetComponent<Renderer>().bounds.size.x )<toAdjust.GetComponent<Collider>().bounds.size.x)&&((toAdjust.GetComponent<Renderer>().bounds.size.y )<toAdjust.GetComponent<Collider>().bounds.size.y))
							{
								//Debug.Log("USOOO");
								again=true;
								break;
							}
							else
							{ nPoz=Regex.Matches(toAdjust.text, "\n").Count;
								toAdjust.characterSize*=0.96f;
								toAdjust.text=back;
							}
						}
					}
				}
			}
		}
		else
		{
			/*Debug.Log("No collider attached to "+ toAdjust.name + " object in" + Application.loadedLevelName +" Scene");//*/
		}
	}

	public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
	{
		int Place = Source.LastIndexOf(Find);
		string result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
		return result;
	}
	
	public static int IndexOfOccurence(this string s, string match, int occurence)
	{
		int i = 1;
		int index = 0;
		
		while (i <= occurence && (index = s.IndexOf(match, index + 1)) != -1)
		{
			if (i == occurence)
				return index;
			
			i++;
		}
		
		return -1;
	}
	public static string ReplaceNextOccurence(string Source, string Find, string Replace,int n)
	{
		
		int place=Source.IndexOfOccurence(Find,n);
		string result=Source;
		if(place!=-1)
			result = Source.Remove(place, Find.Length).Insert(place, Replace);
		return result;
	}
	

	public static bool outLine=false;
	public static bool shadow=false;
	public static bool singleOutline=false;
	
	public static void SetEffectsTexts(this TextMesh toAdjust,bool otherChildren)
	{
		if(outLine)
		{
			if(toAdjust!=null)
			{
				if(toAdjust.transform.childCount==8)
				{
					if(otherChildren)
					{
						toAdjust.transform.FindChild(toAdjust.name+"RightOutline").GetComponent<TextMesh>().text=toAdjust.text;
						toAdjust.transform.FindChild(toAdjust.name+"LeftOutline").GetComponent<TextMesh>().text=toAdjust.text;
						toAdjust.transform.FindChild(toAdjust.name+"UpOutline").GetComponent<TextMesh>().text=toAdjust.text;
						toAdjust.transform.FindChild(toAdjust.name+"DownOutline").GetComponent<TextMesh>().text=toAdjust.text;
						toAdjust.transform.FindChild(toAdjust.name+"LeftUpOutline").GetComponent<TextMesh>().text=toAdjust.text;
						toAdjust.transform.FindChild(toAdjust.name+"RightUpOutline").GetComponent<TextMesh>().text=toAdjust.text;
						toAdjust.transform.FindChild(toAdjust.name+"LeftDownOutline").GetComponent<TextMesh>().text=toAdjust.text;
						toAdjust.transform.FindChild(toAdjust.name+"RightDownOutline").GetComponent<TextMesh>().text=toAdjust.text;
					}
					else
					{
						toAdjust.transform.GetChild(0).GetComponent<TextMesh>().text=toAdjust.text;
						toAdjust.transform.GetChild(1).GetComponent<TextMesh>().text=toAdjust.text;
						toAdjust.transform.GetChild(2).GetComponent<TextMesh>().text=toAdjust.text;
						toAdjust.transform.GetChild(3).GetComponent<TextMesh>().text=toAdjust.text;
						toAdjust.transform.GetChild(4).GetComponent<TextMesh>().text=toAdjust.text;
						toAdjust.transform.GetChild(5).GetComponent<TextMesh>().text=toAdjust.text;
						toAdjust.transform.GetChild(6).GetComponent<TextMesh>().text=toAdjust.text;
						toAdjust.transform.GetChild(7).GetComponent<TextMesh>().text=toAdjust.text;
						
					}
				}
			}
		}
		else if(shadow)
		{
			
		}
		else if(singleOutline)
		{
			if(otherChildren)
			{
				toAdjust.transform.FindChild(toAdjust.name+"Outline").GetComponent<TextMesh>().text=toAdjust.text;
				
			}
			else
			{
				toAdjust.transform.GetChild(0).GetComponent<TextMesh>().text=toAdjust.text;
			}
		}
	}
	
	public static void AddSingleOutline(this TextMesh toAdjust,Color shadowColor, float offset)
	{
		if(toAdjust.transform.childCount==0)
		{
			GameObject.DestroyImmediate(toAdjust.GetComponent<TextMeshPlugin>());
			TextMesh helpTextMesh;
			GameObject help1= GameObject.Instantiate(toAdjust.gameObject) as GameObject;
			GameObject help2= GameObject.Instantiate(toAdjust.gameObject) as GameObject;
			GameObject help3= GameObject.Instantiate(toAdjust.gameObject) as GameObject;
			GameObject help4= GameObject.Instantiate(toAdjust.gameObject) as GameObject;
			help1.name= toAdjust.name+"Outline";
			helpTextMesh=	help1.GetComponent<TextMesh>();
			helpTextMesh.color=shadowColor;
			helpTextMesh.characterSize*=offset;
			helpTextMesh.offsetZ= toAdjust.offsetZ+0.1f;
			help1.GetComponent<Renderer>().enabled=true;
			help1.transform.parent= toAdjust.transform;
			help1.transform.localRotation= Quaternion.identity;
			help1.transform.localPosition= new Vector3(0,0,0);
			help1.transform.localScale= Vector3.one;
			
			help2.name= toAdjust.name+"Outline2";
			helpTextMesh=	help2.GetComponent<TextMesh>();
			helpTextMesh.color=shadowColor;
			helpTextMesh.characterSize/=offset;
			helpTextMesh.offsetZ= toAdjust.offsetZ+0.1f;
			help2.GetComponent<Renderer>().enabled=true;
			help2.transform.parent= toAdjust.transform;
			help2.transform.localRotation= Quaternion.identity;
			help2.transform.localPosition= new Vector3(0,0,0);
			help2.transform.localScale= Vector3.one;
			
			help3.name= toAdjust.name+"Outline3";
			helpTextMesh=	help3.GetComponent<TextMesh>();
			helpTextMesh.color=shadowColor;
			helpTextMesh.characterSize/=offset+0.01f;
			helpTextMesh.offsetZ= toAdjust.offsetZ+0.1f;
			help3.GetComponent<Renderer>().enabled=true;
			help3.transform.parent= toAdjust.transform;
			help3.transform.localRotation= Quaternion.identity;
			help3.transform.localPosition= new Vector3(-0.017f,0,0);
			help3.transform.localScale= Vector3.one;
			
			help4.name= toAdjust.name+"Outline4";
			helpTextMesh=	help4.GetComponent<TextMesh>();
			helpTextMesh.color=shadowColor;
			helpTextMesh.offsetZ= toAdjust.offsetZ+0.1f;
			help4.GetComponent<Renderer>().enabled=true;
			help4.transform.parent= toAdjust.transform;
			help4.transform.localRotation= Quaternion.identity;
			help4.transform.localPosition= new Vector3(0.04f,0,0);
			help4.transform.localScale= Vector3.one;
			singleOutline=true;
		}
	}
	
	public static void AddShadow(this TextMesh toAdjust,Color shadowColor, Vector2 offset)
	{
		if(toAdjust.transform.childCount==0)
		{
			GameObject.DestroyImmediate(toAdjust.GetComponent<TextMeshPlugin>());
			TextMesh helpTextMesh;
			GameObject help1= GameObject.Instantiate(toAdjust.gameObject) as GameObject;
			help1.name= toAdjust.name+"Shadow";
			helpTextMesh=	help1.GetComponent<TextMesh>();
			helpTextMesh.color=shadowColor;
			helpTextMesh.offsetZ= toAdjust.offsetZ+0.1f;
			help1.GetComponent<Renderer>().enabled=true;
			help1.transform.parent= toAdjust.transform;
			help1.transform.localRotation= Quaternion.identity;
			help1.transform.localPosition= new Vector3(offset.x,offset.y,0);
			help1.transform.localScale= Vector3.one;
			shadow=true;
		}
	}
	
	/// <summary>
	/// Adds the outline to font. WARNING: Can cause slow down in preformance.
	/// </summary>
	/// <param name='toAdjust'>
	/// To adjust.
	/// </param>
	/// <param name='outlineColor'>
	/// Outline color.
	/// </param>
	/// <param name='offset'>
	/// Offset.
	/// </param>
	public static void AddOutline(this TextMesh toAdjust,Color outlineColor, float offset)
	{
		
		if(toAdjust.transform.childCount!=8)
		{
			GameObject.DestroyImmediate(toAdjust.GetComponent<TextMeshPlugin>());
			TextMesh helpTextMesh;

//			Debug.LogError(toAdjust.gameObject.name+"##"+toAdjust.gameObject.GetComponents<MonoBehaviour>()[0]);
//			for(int i = toAdjust.gameObject.GetComponents<MonoBehaviour>().Length-1;i>=0;i--)
//			{
//				if(toAdjust.gameObject.GetComponents<MonoBehaviour>()[i]==null)
//				{
//					Debug.Log("AAAAAA");
//					Object.Destroy(toAdjust.gameObject.GetComponents<MonoBehaviour>()[i]);
//				}
//			}
//
//			Debug.LogError(toAdjust.gameObject.name+"##"+toAdjust.gameObject.GetComponents<MonoBehaviour>().Length);

			GameObject help1= GameObject.Instantiate(toAdjust.gameObject) as GameObject;
			if(help1.GetComponent<Collider>() != null)
				Object.Destroy(help1.GetComponent<Collider>());

			help1.name= toAdjust.name+"RightOutline";
			helpTextMesh=	help1.GetComponent<TextMesh>();
			helpTextMesh.color=outlineColor;
			helpTextMesh.offsetZ= toAdjust.offsetZ+0.1f;
			
			
			GameObject help2= GameObject.Instantiate(toAdjust.gameObject) as GameObject;
			if(help2.GetComponent<Collider>() != null)
				Object.Destroy(help2.GetComponent<Collider>());
			
			help2.name= toAdjust.name+"LeftOutline";
			helpTextMesh=	help2.GetComponent<TextMesh>();
			helpTextMesh.color=outlineColor;
			helpTextMesh.offsetZ= toAdjust.offsetZ+0.1f;
			
			
			
			GameObject help3= GameObject.Instantiate(toAdjust.gameObject) as GameObject;
			if(help3.GetComponent<Collider>() != null)
				Object.Destroy(help3.GetComponent<Collider>());

			help3.name= toAdjust.name+"UpOutline";
			helpTextMesh=	help3.GetComponent<TextMesh>();
			helpTextMesh.color=outlineColor;
			helpTextMesh.offsetZ= toAdjust.offsetZ+0.1f;
			
			
			
			GameObject help4= GameObject.Instantiate(toAdjust.gameObject) as GameObject;
			if(help4.GetComponent<Collider>() != null)
				Object.Destroy(help4.GetComponent<Collider>());

			help4.name= toAdjust.name+"DownOutline";
			helpTextMesh=	help4.GetComponent<TextMesh>();
			helpTextMesh.color=outlineColor;
			helpTextMesh.offsetZ= toAdjust.offsetZ+0.1f;
			
			GameObject help5= GameObject.Instantiate(toAdjust.gameObject) as GameObject;
			if(help5.GetComponent<Collider>() != null)
				Object.Destroy(help5.GetComponent<Collider>());

			help5.name= toAdjust.name+"LeftUpOutline";
			helpTextMesh=	help5.GetComponent<TextMesh>();
			helpTextMesh.color=outlineColor;
			helpTextMesh.offsetZ= toAdjust.offsetZ+0.1f;
			
			GameObject help6= GameObject.Instantiate(toAdjust.gameObject) as GameObject;
			if(help6.GetComponent<Collider>() != null)
				Object.Destroy(help6.GetComponent<Collider>());

			help6.name= toAdjust.name+"RightUpOutline";
			helpTextMesh=	help6.GetComponent<TextMesh>();
			helpTextMesh.color=outlineColor;
			helpTextMesh.offsetZ= toAdjust.offsetZ+0.1f;

			GameObject help7= GameObject.Instantiate(toAdjust.gameObject) as GameObject;
			if(help7.GetComponent<Collider>() != null)
				Object.Destroy(help7.GetComponent<Collider>());

			help7.name= toAdjust.name+"LeftDownOutline";
			helpTextMesh=	help7.GetComponent<TextMesh>();
			helpTextMesh.color=outlineColor;
			helpTextMesh.offsetZ= toAdjust.offsetZ+0.1f;
			
			GameObject help8= GameObject.Instantiate(toAdjust.gameObject) as GameObject;
			if(help8.GetComponent<Collider>() != null)
				Object.Destroy(help8.GetComponent<Collider>());

			help8.name= toAdjust.name+"RightDownOutline";
			helpTextMesh=	help8.GetComponent<TextMesh>();
			helpTextMesh.color=outlineColor;
			helpTextMesh.offsetZ= toAdjust.offsetZ+0.1f;
			
			
			//right
			help1.GetComponent<Renderer>().enabled=true;
			help1.transform.parent= toAdjust.transform;
			help1.transform.localRotation= Quaternion.identity;
			help1.transform.localPosition= new Vector3(offset,0,0);
			help1.transform.localScale= Vector3.one;
			//left
			help2.GetComponent<Renderer>().enabled=true;
			help2.transform.parent= toAdjust.transform;
			help2.transform.localRotation= Quaternion.identity;
			help2.transform.localPosition= new Vector3(-offset,0,0);
			help2.transform.localScale= Vector3.one;
			//up
			help3.GetComponent<Renderer>().enabled=true;
			help3.transform.parent= toAdjust.transform;
			help3.transform.localRotation= Quaternion.identity;
			help3.transform.localPosition= new Vector3(0,offset,0);
			help3.transform.localScale= Vector3.one;
			//down
			help4.GetComponent<Renderer>().enabled=true;
			help4.transform.parent= toAdjust.transform;
			help4.transform.localRotation= Quaternion.identity;
			help4.transform.localPosition= new Vector3(0,-offset,0);
			help4.transform.localScale= Vector3.one;
			
			//leftup
			help5.GetComponent<Renderer>().enabled=true;
			help5.transform.parent= toAdjust.transform;
			help5.transform.localRotation= Quaternion.identity;
			help5.transform.localPosition= new Vector3(-offset,3f*offset/3f,0);
			help5.transform.localScale= Vector3.one;
			
			//Rightup
			help6.GetComponent<Renderer>().enabled=true;
			help6.transform.parent= toAdjust.transform;
			help6.transform.localRotation= Quaternion.identity;
			help6.transform.localPosition= new Vector3(offset,3f*offset/3f,0);
			help6.transform.localScale= Vector3.one;
			
			//leftdown
			help7.GetComponent<Renderer>().enabled=true;
			help7.transform.parent= toAdjust.transform;
			help7.transform.localRotation= Quaternion.identity;
			help7.transform.localPosition= new Vector3(-offset,-offset,0);
			help7.transform.localScale= Vector3.one;
			
			//rightdown
			help8.GetComponent<Renderer>().enabled=true;
			help8.transform.parent= toAdjust.transform;
			help8.transform.localRotation= Quaternion.identity;
			help8.transform.localPosition= new Vector3(offset,-offset,0);
			help8.transform.localScale= Vector3.one;
			
			outLine=true;
			
		}
		
	}
	
	public static void AdjustFontSplitRows(this TextMesh toAdjust, bool hasWhiteSpaces)
	{
		string oldText= toAdjust.text;
		string[] wholeText=oldText.Split(' ');
		//kako se odvajaju reci
		if(!hasWhiteSpaces)
		{
			char[] help= oldText.ToCharArray();
			wholeText= new string[help.Length];
			for(int i =0;i<wholeText.Length;i++)
			{
				wholeText[i]= help[i].ToString();
			}
		}
		//za slucaj da je samo jedna rec
		if(wholeText.Length==1)
		{
			
			return;
		}
		else
		{//logika za vise reci u vise redova
			int counter=1;
			toAdjust.text=wholeText[0];
			string previous=toAdjust.text;
			string helpText="";
			while((counter<wholeText.Length))
			{
				
				previous=toAdjust.text;
				toAdjust.text+=((!hasWhiteSpaces)?"":" ")+wholeText[counter];
				if((toAdjust.GetComponent<Renderer>().bounds.size.x>toAdjust.GetComponent<Collider>().bounds.size.x))
					toAdjust.text=previous+"\n"+wholeText[counter];
				counter++;
			}
		}
	}

}