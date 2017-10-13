using UnityEngine;
using System.Collections;

public class ScenaReklama : MonoBehaviour {

	Vector3 originalScale;
	string clickedItem;
	string releasedItem;
	TouchScreenKeyboard keyboard;
	string mail;
	GameObject invalidMail;

	void Start ()
	{
		invalidMail = GameObject.Find("IncorrectMail");
		invalidMail.SetActive(false);
		//majmunLogo = GameObject.Find("HolderMajmun");
		//majmunLogo.transform.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one).x,Camera.main.ViewportToWorldPoint(Vector3.one).y,majmunLogo.transform.position.z);
	}
	
	void Update () 
	{
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			if(PlaySounds.soundOn)
				PlaySounds.Play_Button_OpenLevel();
			GameObject.Find(releasedItem).GetComponent<Collider>().enabled = false;
			System.GC.Collect();
			Resources.UnloadUnusedAssets();
			Application.LoadLevelAsync((StagesParser.currSetIndex*10)+StagesParser.currStageIndex+5);
		}
		else if(Input.GetMouseButtonDown(0))
		{
			clickedItem = RaycastFunction(Input.mousePosition);
			if(clickedItem.Equals("Button_Continue") || clickedItem.Equals("Button_Subscribe")) 
			{
				GameObject temp = GameObject.Find(clickedItem);
				originalScale = temp.transform.localScale;
				temp.transform.localScale = originalScale * 0.8f;
			}
		}

		else if(Input.GetMouseButtonUp(0))
		{
			releasedItem = RaycastFunction(Input.mousePosition);
//			if(!clickedItem.Equals(System.String.Empty))
//			{
//				GameObject temp = GameObject.Find(clickedItem);
//				temp.transform.localScale = originalScale;
//				if(releasedItem.Equals("Button_Continue"))
//				{
//					if(PlaySounds.soundOn)
//						PlaySounds.Play_Button_OpenLevel();
//					Debug.Log("continue dalje");
//					GameObject.Find(releasedItem).collider.enabled = false;
//					System.GC.Collect();
//					Resources.UnloadUnusedAssets();
//					Application.LoadLevelAsync((StagesParser.currSetIndex*10)+StagesParser.currStageIndex+5);
//				}
//				else if(clickedItem.Equals("Button_Subscribe"))
//				{
//					if(PlaySounds.soundOn)
//						PlaySounds.Play_Button_OpenLevel();
//					//keyboard = TouchScreenKeyboard.Open(mail,TouchScreenKeyboardType.ASCIICapable,false,true,false,false, "Enter Your Email Here");
//					#if UNITY_ANDROID
//					using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) 
//					{
//						using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) 
//						{
//							obj_Activity.Call("OtvoriWebView");
//						}
//					}
//					#elif UNITY_IPHONE
//					//WebelinxBinding.sendMessage("OpenWebView");
//					#endif
//					PlayerPrefs.SetInt("MailSent",1);
//					PlayerPrefs.Save();
//					GameObject.Find("Button_Subscribe").SetActive(false);
//				}
//			}
		}

		if(keyboard != null)
		{
			if(keyboard.done)
			{
				mail = keyboard.text;
				keyboard = null;
				if(!mail.Equals(System.String.Empty) && mail.Contains("@"))
				{
					Debug.Log("poruka: " + mail);
					if(invalidMail.activeSelf)
						invalidMail.SetActive(false);
					GameObject.Find("Button_Subscribe").SetActive(false);
					//StartCoroutine("postToServer");

				}
				else
				{
					Debug.Log("treba da je prazno: " + mail);
					invalidMail.SetActive(true);
				}
			}
		}
	}

	byte[] GetBytes(string str)
	{
		byte[] bytes = new byte[str.Length * sizeof(char)];
		System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
		return bytes;
	}

	IEnumerator postToServer()
	{
		yield return new WaitForEndOfFrame();
		WWWForm form = new WWWForm();
		//mail = "PRPRPR";
		byte[] bb = GetBytes(mail);
		System.IO.File.WriteAllBytes(Application.persistentDataPath + "/mailList.txt", bb);

//		using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Public\TestFolder\WriteLines2.txt", true))
//		{
//			file.WriteLine("Fourth line");
//
//		}
		form.AddBinaryData("NekiNaziv",bb,"index.php");
		WWW www = new WWW("http://www.tapsong.net/content/MonkeyRush/",form);
		yield return www;
		if(string.IsNullOrEmpty(www.error))
		{
			GameObject.Find("MAIL").GetComponent<TextMesh>().text = "SUCCESSFULLY UPLOAD:\n" + www.text;
		}
		else
		{
			GameObject.Find("MAIL").GetComponent<TextMesh>().text = "ERROR UPLOADING:\n" + www.error;
		}
	}

	string RaycastFunction(Vector3 vector)
	{
		Ray ray = Camera.main.ScreenPointToRay(vector);
		RaycastHit hit;
		if(Physics.Raycast(ray,out hit))
		{
			return hit.collider.name;
		}
		return "";
	}
}
