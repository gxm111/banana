using UnityEngine;
using System.Collections;

public class TutorialEvents : MonoBehaviour {

	public static bool postavljenCollider = false;
	bool helpBool;
	System.DateTime timeToShowNextElement;

	void Start ()
	{
		if(gameObject.name.Contains("1"))
		{
			transform.GetChild(0).GetChild(0).Find("AnimationHolder/Tutorial1 Popup/Tutorial Text").GetComponent<TextMesh>().text = LanguageManager.TutorialTapJump;
			transform.GetChild(0).GetChild(0).Find("AnimationHolder/Tutorial1 Popup/Tutorial Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true,false);
		}
		else if(gameObject.name.Contains("2"))
		{
			transform.GetChild(0).GetChild(0).Find("AnimationHolder/Tutorial2 Popup/Tutorial Text").GetComponent<TextMesh>().text = LanguageManager.TutorialGlide;
			transform.GetChild(0).GetChild(0).Find("AnimationHolder/Tutorial2 Popup/Tutorial Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true,false);
		}
		else if(gameObject.name.Contains("3"))
		{
			transform.GetChild(0).GetChild(0).Find("AnimationHolder/Tutorial3 Popup/Tutorial Text").GetComponent<TextMesh>().text = LanguageManager.TutorialSwipe;
			transform.GetChild(0).GetChild(0).Find("AnimationHolder/Tutorial3 Popup/Tutorial Text").GetComponent<TextMeshEffects>().RefreshTextOutline(true,true,false);
		}
		this.transform.GetChild(0).gameObject.SetActive(false);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag == "Monkey")// && !PlayerPrefs.HasKey("OdgledaoTutorial"))
		{
			postavljenCollider = false;
			Manage.pauseEnabled = false;
			int koji=0;
			if(gameObject.name.Contains("1"))
			{
				GameObject.FindGameObjectWithTag("Monkey").GetComponent<MonkeyController2D>().KontrolisaniSkok = true;
				koji=1;

			}
			else if(gameObject.name.Contains("2"))
			{
				GameObject.FindGameObjectWithTag("Monkey").GetComponent<MonkeyController2D>().Glide = true;
				koji=2;
			}
			else if(gameObject.name.Contains("3"))
			{
				GameObject.FindGameObjectWithTag("Monkey").GetComponent<MonkeyController2D>().SlideNaDole = true;
				koji=3;
			}
			Time.timeScale = 0;
			GetComponent<Collider2D>().enabled = false;
			transform.position = Camera.main.transform.position + Vector3.forward*10;
			transform.GetChild(0).gameObject.SetActive(true);
			//StartCoroutine(TutorialPlay(transform.GetChild(0).GetChild(0),"TutorialUlaz_A",koji));
			transform.GetChild(0).GetChild(0).GetComponent<Animator>().Play("OpenPopup");
		}
	}

//	IEnumerator TutorialPlay(Transform obj, string ime, int next)
//	{
//		StartCoroutine( obj.animation.Play(ime, false, what => {helpBool=true;}) );
//		//animation.Play(
//		while(!helpBool)
//		{
//			yield return null;
//		}
//		if(ime.Contains("_A"))
//		{
//			StartCoroutine(ActivateCollider());
//		}
//		helpBool=false;
//
//
//		if(next == 1)
//			StartCoroutine(TutorialPlay(obj,"TutorialIdle1_A",-1));
//		else if(next == 2)
//			StartCoroutine(TutorialPlay(obj,"TutorialIdle3_A",-1));
//		else if(next == 3)
//			StartCoroutine(TutorialPlay(obj,"TutorialIdle4_A",-1));
//		
//	}

//	IEnumerator ActivateCollider()
//	{
//		float sec=0;
//		if(name.Contains("1"))
//		{
//			sec = 3.75f;
//		}
//		if(name.Contains("2"))
//		{
//			sec = 4.25f;
//		}
//		else if(name.Contains("3"))
//		{
//			sec = 3.3f;
//			PlayerPrefs.SetInt("OdgledaoTutorial",1);
//			PlayerPrefs.Save();
//		}
//		timeToShowNextElement = System.DateTime.Now.AddSeconds(sec);
//		
//		while (System.DateTime.Now < timeToShowNextElement)
//		{
//			yield return null;
//		}
//		//transform.GetChild(0).collider.enabled = true;
//		postavljenCollider = true;
//	}
}
