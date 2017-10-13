using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

public class LoadingScreen : MonoBehaviour {




		
	
	// Use this for initialization
	void Start () {




//		if(PlaySounds.BackgroundMusic_Menu.isPlaying)
//			PlaySounds.Stop_BackgroundMusic_Menu();
//		if(PlaySounds.BackgroundMusic_Gameplay.isPlaying)
//			PlaySounds.Stop_BackgroundMusic_Gameplay();
		StartCoroutine(LoadNextLevel());
	}
	
	IEnumerator LoadNextLevel()
	{
		System.GC.Collect();
		Resources.UnloadUnusedAssets();
		yield return new WaitForSeconds(3);
		//Application.LoadLevelAsync((StagesParser.currSetIndex*10)+StagesParser.currStageIndex+5);
		Application.LoadLevelAsync(1); //BICE 9+StagesParser.currSetIndex KAD SE DODAJU NIVOI ZA OSTALA OSTRVA
	}

	 
		
}
