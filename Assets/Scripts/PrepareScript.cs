using UnityEngine;
using System;
using System.Collections;
using System.IO;

public class PrepareScript : MonoBehaviour {
	/* Scene: PrepareManagers
	 * Object: MainCamera
	 * Izgleda kao splash, i scena sluzi 
	 * da se pripreme svi manageri za igru.
	 * 
	 * */
	
	// Use this for initialization
	void Awake () 
	{
		if(SystemInfo.systemMemorySize <=1024 && SystemInfo.processorCount == 1)
		{
			QualitySettings.masterTextureLimit = 1;
		}

		//Application.targetFrameRate=35;
		Application.targetFrameRate=60;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Delay call with some condition if needed
		if(StagesParser.stagesLoaded)
		{
			Application.LoadLevel(1);
		}
	}
}
