using UnityEngine;
using System.Collections;

public class ObjLanguages : MonoBehaviour {

	public static bool Languages;
	public SwipeControlLanguages swipeCtrl;
	public Transform[] obj = new Transform[0];

	//public GameObject[] arrayOfObj;
	
	
	public float minXPos = 0f; //min x position of the camera
	public float maxXPos = 115f; //max x position of the camera
	private float xDist; //distance between camMinXPos and camMaxXPos
	private float xDistFactor; // = 1/camXDist
	private float swipeSmoothFactor = 1.0f; // 1/swipeCtrl.maxValue

	private float rememberYPos;
	void Awake()
	{
		Languages = false;
	}
	// Use this for initialization
	void Start () {
	

	xDist = maxXPos - minXPos;
	xDistFactor = 1.0f / xDist;

		if(!swipeCtrl) swipeCtrl = gameObject.AddComponent<SwipeControlLanguages>();

	swipeCtrl.skipAutoSetup = true; //skip auto-setup, we'll call Setup() manually once we're done changing stuff
	swipeCtrl.clickEdgeToSwitch = false; //only swiping will be possible
	swipeCtrl.SetMouseRect(new Rect(0, 0, Screen.width, Screen.height)); //entire screen
	swipeCtrl.maxValue = obj.Length - 1; //max value


	//Default
	//swipeCtrl.currentValue = swipeCtrl.maxValue; //current value set to max, so it starts from the end
	//swipeCtrl.startValue = Mathf.RoundToInt(swipeCtrl.maxValue * 0.5f); //when Setup() is called it will animate from the end to the middle
	
	swipeCtrl.currentValue = obj.Length - 2; //current value set to max, so it starts from the end					bilo je 1
	swipeCtrl.startValue = obj.Length - 2; //when Setup() is called it will animate from the end to the middle		bilo je 1

		
		
	//swipeCtrl.partWidth = Screen.width  / swipeCtrl.maxValue; //how many pixels do you have to swipe to change the value by one? in this case we make it dependent on the screen-width and the maxValue, so swiping from one edge of the screen to the other will scroll through all values.

	swipeCtrl.partWidth = Screen.width  / swipeCtrl.maxValue;
		
	swipeCtrl.Setup();

	swipeSmoothFactor = 1.0f/swipeCtrl.maxValue; //divisions are expensive, so we'll only do this once in start
	
	rememberYPos = obj[0].position.y;


	}
	
	// Update is called once per frame
	void Update () {
		if(Languages)
			{
				for(int i = 0; i < obj.Length; i++) {
					//			obj[i].position = new Vector3(obj[i].position.x,minXPos + i * (xDist * swipeSmoothFactor) - swipeCtrl.smoothValue*swipeSmoothFactor*xDist , obj[i].position.z);
					
				//obj[i].position = new Vector3(obj[i].position.x,minXPos - i * (xDist * swipeSmoothFactor) + swipeCtrl.smoothValue*swipeSmoothFactor*xDist , obj[i].position.z);
				obj[i].position = new Vector3(obj[i].position.x,minXPos - i * (xDist * swipeSmoothFactor) - swipeCtrl.smoothValue*swipeSmoothFactor*xDist , obj[i].position.z);
					
					
				}
			}
			
		

			

	
		}

//	public void PostaviImenaItemaShop
	
}
	
