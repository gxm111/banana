using UnityEngine;
using System.Collections;

public class LoadingManager : MonoBehaviour {
	/*
	 * Scene:NewLoadingScene
	 * Object:Main Camera
	 * Sluzi za loadovanje scena, loaduje se scena sa imenom 'nextLevel'
	 * prethodni nivo koji je zatrazio ucitavanje sledece scene se nalazi 
	 * u previousLevel, u odnosu na previous level se cuvaju ili pripremaju odgovarajuci
	 * podaci (pamti se info o predjenim nivoima i otkljucavaju se novi).
	 * 
	 * Note: Napraviti da se otvara odmah iz splash-a tj posle prepareManagers i pamtiti
	 * koji je status igre i koja se scena treba otvoriti(za slucaj da je igrica ugasena zbog manjka memorije)
	 * */
	public static string previousLevel="";
	public static string nextLevel="";
	// Use this for initialization
	void Awake () {
		
		//GameObject.Find("text").transform.position=Camera.main.ScreenToWorldPoint(new Vector3(Screen.width*0.01f,Screen.height*0.99f,1));
	}
	void Start()
	{
		if(previousLevel=="PrepareManagersScene")
		{
			Application.LoadLevelAsync(nextLevel);
		}
		if(previousLevel=="LevelSelectScene")
		{
			Application.LoadLevelAsync(nextLevel);
		}
		if(previousLevel=="PlayScene")
		{
			GameObject.Find("StagesParserManager").SendMessage("CallSave");
			
		}
	}
	
	
	IEnumerator saveData()
	{
		yield return null;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(previousLevel=="PlayScene"&& StagesParser.saving)
		{
			StagesParser.saving=false;
			Application.LoadLevelAsync(nextLevel);
		}
		if(Input.GetMouseButtonDown(0))
		{
			GameObject.Find("text").GetComponent<Animation>().Stop();
			int c= Random.Range(0,16);
			if(c<2)
			{
				GameObject.Find("text").GetComponent<TextMesh>().text="Stop Clicking!!\nIm Loading!!!";
				GameObject.Find("text").GetComponent<Animation>().Play();
			}
			else if(c<4)
			{
				GameObject.Find("text").GetComponent<TextMesh>().text="Leave me Alone!!!\nIm Loading!!! ";
				GameObject.Find("text").GetComponent<Animation>().Play();
			}
			else if(c<6)
			{
				GameObject.Find("text").GetComponent<TextMesh>().text="I wont load any faster\nno matter how much you click!";
				GameObject.Find("text").GetComponent<Animation>().Play();
			}
			else if(c<8)
			{
				GameObject.Find("text").GetComponent<TextMesh>().text="I don't think we're\nin kansas anymore!";
				GameObject.Find("text").GetComponent<Animation>().Play();
			}
			else if(c<10)
			{
				GameObject.Find("text").GetComponent<TextMesh>().text="These aren't the\ndroids you're looking for!";
				GameObject.Find("text").GetComponent<Animation>().Play();
			}
			else 
			{
				GameObject.Find("text").GetComponent<TextMesh>().GetComponent<Renderer>().enabled=false;
			}
		}
	}
}
