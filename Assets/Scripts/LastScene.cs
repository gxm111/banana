using UnityEngine;
using System.Collections;

public class LastScene : MonoBehaviour {

	GameObject majmunLogo;
	Vector3 originalScale;
	string clickedItem;
	string releasedItem;
	TouchScreenKeyboard keyboard;
	string mail;
	GameObject invalidMail;
	TextMesh boardText;
	TextMesh boardText_black;
	GameObject partikli;
	GameObject temp;

	void Start ()
	{
		StartCoroutine(Loading.Instance.UcitanaScena(Camera.main,5,0f));
		partikli = GameObject.Find("Partikli Finalna scena");
		partikli.SetActive(false);
		boardText = GameObject.Find("BoardText_Congratulations").GetComponent<TextMesh>();
		boardText_black = GameObject.Find("BoardText_NewLevelsComingSoon").GetComponent<TextMesh>();
		boardText.text = LanguageManager.Congratulations;
		boardText_black.text = LanguageManager.NewLevelsComingSoon;
		boardText.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		boardText_black.GetComponent<TextMeshEffects>().RefreshTextOutline(true,true);
		Invoke("UkljuciPartikle",0.75f);

		if(PlaySounds.musicOn)
		{
			PlaySounds.Play_BackgroundMusic_Gameplay();
			if(PlaySounds.Level_Failed_Popup.isPlaying)
				PlaySounds.Stop_Level_Failed_Popup();
		}
	}

	void UkljuciPartikle()
	{
		partikli.SetActive(true);
	}
	
	void Update () 
	{
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			if(PlaySounds.soundOn)
				PlaySounds.Play_Button_OpenLevel();
			Application.LoadLevel(1);
		}
		else if(Input.GetMouseButtonDown(0))
		{
			clickedItem = RaycastFunction(Input.mousePosition);
			if(clickedItem.Equals("Button_Back") || clickedItem.Equals("Button_Subscribe")) 
			{
				temp = GameObject.Find(clickedItem);
				originalScale = temp.transform.localScale;
				temp.transform.localScale = originalScale * 0.8f;
			}
		}

		else if(Input.GetMouseButtonUp(0))
		{
			releasedItem = RaycastFunction(Input.mousePosition);
			if(!clickedItem.Equals(System.String.Empty))
			{
				if(temp != null) 
					temp.transform.localScale = originalScale;

				if(releasedItem.Equals("Button_Back"))
				{
					if(PlaySounds.BackgroundMusic_Gameplay.isPlaying)
						PlaySounds.Stop_BackgroundMusic_Gameplay();
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
					Application.LoadLevel(1);
				}
//				else if(clickedItem.Equals("Button_Subscribe"))
//				{
//					keyboard = TouchScreenKeyboard.Open(mail,TouchScreenKeyboardType.ASCIICapable,false,true,false,false, "Enter Term");
//				}
				else if(clickedItem.Equals("Button_Subscribe"))
				{
					if(PlaySounds.soundOn)
						PlaySounds.Play_Button_OpenLevel();
					//keyboard = TouchScreenKeyboard.Open(mail,TouchScreenKeyboardType.ASCIICapable,false,true,false,false, "Enter Your Email Here");
					PlayerPrefs.SetInt("MailSent",1);
					PlayerPrefs.Save();
					GameObject.Find("Button_Subscribe").SetActive(false);
				}
			}
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
