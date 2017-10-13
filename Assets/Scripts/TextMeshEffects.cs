using UnityEngine;
using System.Collections;

[RequireComponent (typeof (TextMesh))]
public class TextMeshEffects : MonoBehaviour {

	TextMesh thisComponent;
	Transform myTransform;
	public static string chosenLanguage="_en";
	public bool neMenjajFont = false;

	void Awake ()
	{
		myTransform = this.transform;
		thisComponent = this.GetComponent<TextMesh>();
	}

	public void RefreshTextOutline(bool adjustTextSize, bool hasWhiteSpaces, bool increaseFont = true)
	{
		if(!neMenjajFont)
		{
			if(LanguageManager.chosenLanguage != "_en" && LanguageManager.chosenLanguage != "_us")
			{
				Font ArialFont;
				if(LanguageManager.chosenLanguage == "_th")
					ArialFont = (Font)Resources.Load("Angsana New Bold");//(Font)Resources.GetBuiltinResource (typeof(Font), "Arial.ttf");
				else
					ArialFont = (Font)Resources.Load("ARIBLK");//(Font)Resources.GetBuiltinResource (typeof(Font), "Arial.ttf");
				thisComponent.font = ArialFont;
				thisComponent.GetComponent<Renderer>().sharedMaterial = ArialFont.material;
			}
			else
			{
				Font EnglishFont = (Font)Resources.Load("SOUPOFJUSTICE");
				thisComponent.font = EnglishFont;
				thisComponent.GetComponent<Renderer>().sharedMaterial = EnglishFont.material;
			}
		}

		if(adjustTextSize)
		{
			thisComponent.AdjustFontSize(true,hasWhiteSpaces,increaseFont);
		}

		if(myTransform.childCount == 8)
		{
			for(int i=0;i<8;i++)
			{
				if(!neMenjajFont)
				{
					if(LanguageManager.chosenLanguage != "_en" && LanguageManager.chosenLanguage != "_us")
					{
						Font ArialFont;
						if(LanguageManager.chosenLanguage == "_th")
							ArialFont = (Font)Resources.Load("Angsana New Bold");//(Font)Resources.GetBuiltinResource (typeof(Font), "Arial.ttf");
						else
							ArialFont = (Font)Resources.Load("ARIBLK");//(Font)Resources.GetBuiltinResource (typeof(Font), "Arial.ttf");
						myTransform.GetChild(i).GetComponent<TextMesh>().font = ArialFont;
						myTransform.GetChild(i).GetComponent<TextMesh>().GetComponent<Renderer>().sharedMaterial = ArialFont.material;
					}
					else
					{
						Font EnglishFont = (Font)Resources.Load("SOUPOFJUSTICE");
						myTransform.GetChild(i).GetComponent<TextMesh>().font = EnglishFont;
						myTransform.GetChild(i).GetComponent<TextMesh>().GetComponent<Renderer>().sharedMaterial = EnglishFont.material;
					}
				}

				myTransform.GetChild(i).GetComponent<TextMesh>().text = thisComponent.text;
				if(adjustTextSize)
				{
					//myTransform.GetChild(i).GetComponent<TextMesh>().AdjustFontSize(true,hasWhiteSpaces,increaseFont);
					myTransform.GetChild(i).GetComponent<TextMesh>().characterSize = thisComponent.characterSize;
				}
			}


		}
	}


}
