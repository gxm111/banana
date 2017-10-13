using UnityEngine;
using System.Collections;

public class IslandHolderManager : MonoBehaviour {
	GameObject Kamera;
	int[] Klik = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23};
	int KliknutoNa=0;
	float TrenutniX, TrenutniY;
	float forceX, forceY, startX, endX, startY, endY;

	void Awake()
	{
		GameObject.Find("HolderBack").GetComponent<Transform>().position=new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x, Camera.main.ViewportToWorldPoint(Vector3.one).y, -2f);
		GameObject.Find("HolderHeaderLev").GetComponent<Transform>().position=new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one/2).x, Camera.main.ViewportToWorldPoint(Vector3.one).y, -2f);
		GameObject.Find("HolderLife").GetComponent<Transform>().position=new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one).x, Camera.main.ViewportToWorldPoint(Vector3.one).y, -2f);
		
		Kamera = GameObject.Find("Main Camera");
		
		GameObject.Find("HolderBack").transform.parent=Camera.main.transform;
		GameObject.Find("HolderHeaderLev").transform.parent=Camera.main.transform;
		GameObject.Find("HolderLife").transform.parent=Camera.main.transform;
	}

	// Use this for initialization
	void Start () {
	
	}

	void Update () {

		if(Input.GetKeyUp(KeyCode.Escape))
			Application.LoadLevel("All Maps");

		endX=Input.mousePosition.x;
		endY=Input.mousePosition.y;

		
		if(Input.GetMouseButtonDown(0))
		{
			startX=Input.mousePosition.x;
			startY=Input.mousePosition.y;
			//					Debug.Log(startX);
			
			if(RaycastFunction(Input.mousePosition) == "Level1")
			{
				KliknutoNa=1;
				
			}
			else if(RaycastFunction(Input.mousePosition) == "Level2")
			{
				KliknutoNa=2;
				
			}
			else if(RaycastFunction(Input.mousePosition) == "Level3")
			{
				KliknutoNa=3;
				
			}
			else if(RaycastFunction(Input.mousePosition) == "Level4")
			{
				KliknutoNa=4;
				
			}
			else if(RaycastFunction(Input.mousePosition) == "Level5")
			{
				KliknutoNa=5;
			}
			else if(RaycastFunction(Input.mousePosition) == "Level6")
			{
				KliknutoNa=6;
				
			}
			else if(RaycastFunction(Input.mousePosition) == "Level7")
			{
				KliknutoNa=7;
				
			}
			else if(RaycastFunction(Input.mousePosition) == "Level8")
			{
				KliknutoNa=8;
				
			}
			else if(RaycastFunction(Input.mousePosition) == "Level9")
			{
				KliknutoNa=9;
			}
			else if(RaycastFunction(Input.mousePosition) == "Level10")
			{
				KliknutoNa=10;
				
			}
			else if(RaycastFunction(Input.mousePosition) == "Level11")
			{
				KliknutoNa=11;
				
			}
			else if(RaycastFunction(Input.mousePosition) == "Level12")
			{
				KliknutoNa=12;
				
			}
			else if(RaycastFunction(Input.mousePosition) == "Level13")
			{
				KliknutoNa=13;
			}
			else if(RaycastFunction(Input.mousePosition) == "Level14")
			{
				KliknutoNa=14;
				
			}
			else if(RaycastFunction(Input.mousePosition) == "Level15")
			{
				KliknutoNa=15;
				
			}
			else if(RaycastFunction(Input.mousePosition) == "Level16")
			{
				KliknutoNa=16;
				
			}
			else if(RaycastFunction(Input.mousePosition) == "Level17")
			{
				KliknutoNa=17;
			}
			else if(RaycastFunction(Input.mousePosition) == "Level18")
			{
				KliknutoNa=18;
				
			}
			else if(RaycastFunction(Input.mousePosition) == "Level19")
			{
				KliknutoNa=19;
				
			}
			else if(RaycastFunction(Input.mousePosition) == "Level20")
			{
				KliknutoNa=20;
				
			}
			else if(RaycastFunction(Input.mousePosition) == "HouseShop")
			{
				KliknutoNa=21;
			}
			else if(RaycastFunction(Input.mousePosition) == "HolderShipFreeCoins")
			{
				KliknutoNa=22;
			}
			else if(RaycastFunction(Input.mousePosition) == "ButtonBack")
			{
				KliknutoNa=23;

			}
			else
			{
				KliknutoNa=0;
			}
		}
		
		if(Input.GetMouseButtonUp(0))
		{
			endX=Input.mousePosition.x;
			endY=Input.mousePosition.y;
			
			forceX=-(endX-startX)/(90*GetComponent<Camera>().aspect)*5f;
			forceY=-(endY-startY)/(90*GetComponent<Camera>().aspect);
			
			//					Debug.Log(forceX);
			
			if(RaycastFunction(Input.mousePosition) == "Level1")
			{
				if(KliknutoNa==Klik[1])
				{
					Debug.Log("Level 1");

				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
				
			}
			else if(RaycastFunction(Input.mousePosition) == "Level2")
			{
				
				if(KliknutoNa==Klik[2])
				{
					Debug.Log("Level 2");
				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
			}
			else if(RaycastFunction(Input.mousePosition) == "Level3")
			{
				
				if(KliknutoNa==Klik[3])
				{
					Debug.Log("Level 3");
				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
			}
			else if(RaycastFunction(Input.mousePosition) == "Level4")
			{
				
				if(KliknutoNa==Klik[4])
				{
					Debug.Log("Level 4");
				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
			}
			else if(RaycastFunction(Input.mousePosition) == "Level5")
			{
				
				if(KliknutoNa==Klik[5])
				{
					Debug.Log("Level 5");
				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
			}
			else if(RaycastFunction(Input.mousePosition) == "Level6")
			{
				
				if(KliknutoNa==Klik[6])
				{
					Debug.Log("Level 6");
				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
			}
			else if(RaycastFunction(Input.mousePosition) == "Level7")
			{
				
				if(KliknutoNa==Klik[7])
				{
					Debug.Log("Level 7");
				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
			}
			else if(RaycastFunction(Input.mousePosition) == "Level8")
			{
				
				if(KliknutoNa==Klik[8])
				{
					Debug.Log("Level 8");
				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
			}
			else if(RaycastFunction(Input.mousePosition) == "Level9")
			{
				
				if(KliknutoNa==Klik[9])
				{
					Debug.Log("Level 9");
				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
			}
			else if(RaycastFunction(Input.mousePosition) == "Level10")
			{
				
				if(KliknutoNa==Klik[10])
				{
					Debug.Log("Level 10");
				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
			}
			else if(RaycastFunction(Input.mousePosition) == "Level11")
			{
				
				if(KliknutoNa==Klik[11])
				{
					Debug.Log("Level 11");
				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
			}
			else if(RaycastFunction(Input.mousePosition) == "Level12")
			{
				
				if(KliknutoNa==Klik[12])
				{
					Debug.Log("Level 12");
				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
			}
			else if(RaycastFunction(Input.mousePosition) == "Level13")
			{
				
				if(KliknutoNa==Klik[13])
				{
					Debug.Log("Level 13");
				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
			}
			else if(RaycastFunction(Input.mousePosition) == "Level14")
			{
				
				if(KliknutoNa==Klik[14])
				{
					Debug.Log("Level 14");
				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
			}
			else if(RaycastFunction(Input.mousePosition) == "Level15")
			{
				
				if(KliknutoNa==Klik[15])
				{
					Debug.Log("Level 15");
				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
			}
			else if(RaycastFunction(Input.mousePosition) == "Level16")
			{
				
				if(KliknutoNa==Klik[16])
				{
					Debug.Log("Level 16");
				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
			}
			else if(RaycastFunction(Input.mousePosition) == "Level17")
			{
				
				if(KliknutoNa==Klik[17])
				{
					Debug.Log("Level 17");
				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
			}
			else if(RaycastFunction(Input.mousePosition) == "Level18")
			{
				
				if(KliknutoNa==Klik[18])
				{
					Debug.Log("Level 18");
				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
			}
			else if(RaycastFunction(Input.mousePosition) == "Level19")
			{
				
				if(KliknutoNa==Klik[19])
				{
					Debug.Log("Level 19");
				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
			}
			else if(RaycastFunction(Input.mousePosition) == "Level20")
			{
				
				if(KliknutoNa==Klik[20])
				{
					Debug.Log("Level 20");
				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
			}



			else if(RaycastFunction(Input.mousePosition) == "HouseShop")
			{
				
				if(KliknutoNa==Klik[21])
				{
					Debug.Log("HouseShop");
					
				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
			}
			else if(RaycastFunction(Input.mousePosition) == "HolderShipFreeCoins")
			{
				
				if(KliknutoNa==Klik[22])
				{
					Debug.Log("HolderShipFreeCoins");
				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
			}
			else if(RaycastFunction(Input.mousePosition) == "ButtonBack")
			{
				
				if(KliknutoNa==Klik[23])
				{
					Debug.Log("ButtonBack");
					Debug.Log("Objasni mi ovo");
					StartCoroutine(UcitajOstrvo("All Maps"));
				}
				else
				{
					//							Debug.Log("Skrol kamere");
					PomeriKameru();
				}
			}
			else
			{
				//						Debug.Log("Skrol kamere");
				PomeriKameru();
			}
		}
		
		
		
	}


	void PomeriKameru()
	{
		TrenutniX=Kamera.transform.position.x;
		TrenutniY=Kamera.transform.position.y;
		//		Debug.Log("TrenutniX: "+TrenutniX+" Trenutni force: "+forceX);
		
		
		if(TrenutniX<=-11.87f && forceX<0)
		{
			Debug.Log("Leva granica");
		}
		else if(TrenutniX>=11f && forceX>0)
		{
			Debug.Log("Desna  granica");
		}
		else if(TrenutniX+forceX>=-11.87f && TrenutniX+forceX<=11f)
		{
			if(TrenutniX+forceX>=-11.87f)
			{
				Debug.Log("Desno");
				Kamera.transform.Translate(forceX,forceY, 0);
			}
			else if(TrenutniX+forceX<=11f)
			{
				Debug.Log("Levo");
				Kamera.transform.Translate(forceX,forceY, 0);
			}
			else
			{
				Debug.Log("Novo");
			}
			
			//			Kamera.transform.Translate(forceX,forceY, 0);
		}
		else if(TrenutniX+forceX<-11.87f)
		{
			
			Kamera.GetComponent<Transform>().position=new Vector3(-11.87f,Kamera.transform.position.y, -10);
		}
		else if(TrenutniX+forceX>11f)
		{
			
			Kamera.GetComponent<Transform>().position=new Vector3(11f,Kamera.transform.position.y, -10);
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

	IEnumerator UcitajOstrvo(string ime)
	{
		GameObject.Find("OblaciPomeranje").GetComponent<Animation>().Play("OblaciPostavljanje");
		yield return new WaitForSeconds(0.85f);
		Application.LoadLevel(ime);
	}
}
