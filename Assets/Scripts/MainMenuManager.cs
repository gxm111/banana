using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

	
	const int CAMPAIGN_MODE = 1;
	const int TIME_MODE = 2;
	const int MORE_APPS = 3;
	
	
	GameObject clickedOnObj;
	
	public Mesh[] arrayOfButtonMeshes;
	
	
	bool soundOn = true;
	bool musicOn = true;
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		
		if(Input.GetMouseButtonDown(0))
		{
			clickedOnObj = GetClickedObject();
			
			if(clickedOnObj != null)
			{
				if(clickedOnObj.name.Equals("Main Campaign"))
				{
					//GameObject.Find("Campaign text").GetComponent<TextMesh>().characterSize = 0.0195f;
					//clickedOnObj.GetComponent<MeshFilter>().mesh = arrayOfButtonMeshes[1];
					clickedOnObj.GetComponent<Animation>().Play();
					StartCoroutine(WaitForAnimation(clickedOnObj.GetComponent<Animation>(), true, 3));
					
				}
				else if(clickedOnObj.name.Equals("Main Endless"))
				{
					clickedOnObj.GetComponent<Animation>().Play();
					StartCoroutine(WaitForAnimation(clickedOnObj.GetComponent<Animation>(), true, 3));
					
				}
				else if(clickedOnObj.name.Equals("Main More Apps"))
				{
					clickedOnObj.GetComponent<Animation>().Play();
					StartCoroutine(WaitForAnimation(clickedOnObj.GetComponent<Animation>(), true, 3));
					
				}
				else if(clickedOnObj.name.Equals("ButtonSound"))
				{
					if(soundOn)
					{//iskljuci
						clickedOnObj.GetComponent<MeshFilter>().mesh = arrayOfButtonMeshes[1];
					}
					else
					{
						clickedOnObj.GetComponent<Animation>().Play();
						clickedOnObj.GetComponent<MeshFilter>().mesh = arrayOfButtonMeshes[0];
					}
					
					soundOn = !soundOn;
				}
				else if(clickedOnObj.name.Equals("ButtonMusic"))
				{
					if(musicOn)
					{//iskljuci
						clickedOnObj.GetComponent<MeshFilter>().mesh = arrayOfButtonMeshes[3];
					}
					else
					{
						clickedOnObj.GetComponent<Animation>().Play();
						clickedOnObj.GetComponent<MeshFilter>().mesh = arrayOfButtonMeshes[2];
					}
					
					musicOn = !musicOn;
					
				}
				
				
				
				
				/* stari deo koji je koristio arrayOfMeshes
				
				else if(clickedOnObj.name.Equals("EndlessBtn"))
				{
					GameObject.Find("EndlessTxt").GetComponent<TextMesh>().characterSize = 0.0295f;	
					clickedOnObj.GetComponent<MeshFilter>().mesh = arrayOfButtonMeshes[3];
				}
				else if(clickedOnObj.name.Equals("MoreAppsBtn"))
				{	
					GameObject.Find("MoreAppsTxt").GetComponent<TextMesh>().characterSize = 0.0295f;
					clickedOnObj.GetComponent<MeshFilter>().mesh = arrayOfButtonMeshes[5];
				}
				*/
				
			}
		}
		
		
		
		/*
		
		else if (Input.GetMouseButtonUp (0)) 
		{
		
			if(clickedOnObj.name.Equals("Main Campaign"))
			{
				clickedOnObj.GetComponent<MeshFilter>().mesh = arrayOfButtonMeshes[0];
				//StartCoroutine(WaitForAnimation(clickedOnObj.animation, true, CAMPAIGN_MODE));
			}
			else if(clickedOnObj.name.Equals("EndlessBtn"))
			{
				GameObject.Find("EndlessTxt").GetComponent<TextMesh>().characterSize = 0.03f;
				clickedOnObj.GetComponent<MeshFilter>().mesh = arrayOfButtonMeshes[2];
				//definisi ekran
			}
			else if(clickedOnObj.name.Equals("MoreAppsBtn"))
			{
				clickedOnObj.GetComponent<MeshFilter>().mesh = arrayOfButtonMeshes[4];
				//definisi ekran
			}
			
			
			else if(clickedOnObj.name.Equals("SoundBtn"))
			{
				if(soundOn)
				{//iskljuci
					clickedOnObj.GetComponent<MeshFilter>().mesh = arrayOfButtonMeshes[7];
				}
				else
				{
					clickedOnObj.GetComponent<MeshFilter>().mesh = arrayOfButtonMeshes[6];
				}
				
				
				
				soundOn = !soundOn;
				
			}
			else if(clickedOnObj.name.Equals("MusicBtn"))
			{
				if(soundOn)
				{//iskljuci
					clickedOnObj.GetComponent<MeshFilter>().mesh = arrayOfButtonMeshes[9];
				}
				else
				{
					clickedOnObj.GetComponent<MeshFilter>().mesh = arrayOfButtonMeshes[8];
				}
				
				soundOn = !soundOn;
				
			}
		}
		
		*/
		
		
	}

	
	private IEnumerator WaitForAnimation ( Animation animation, bool loadAnotherScene, int indexOfSceneToLoad )
	{
		
		//animation.Play();
		
	    do
	    {
	      yield return null;
	    } 
		while ( animation.isPlaying );
		
		if(loadAnotherScene)
			Application.LoadLevel(indexOfSceneToLoad);
			
	}
	
	
	GameObject GetClickedObject ()
    {

        GameObject target = null;
		RaycastHit hit;
		Ray ray;

        ray = Camera.main.ScreenPointToRay (Input.mousePosition);

        if (Physics.Raycast (ray.origin, ray.direction * 10, out hit)) {
			
			//if(hit.collider.gameObject.transform.position.z == 0)
            	target = hit.collider.gameObject;

        }

        return target;

    }
	
}
