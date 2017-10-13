using UnityEngine;
using System.Collections;

public class ButtonsScript : MonoBehaviour {

	// Use this for initialization
	string clickedOn="";
	Material save;
	Vector3 scaleSave;
	//Funckcija koja menja stanje dugmici, btn za konkretan objekat. drugi parametar je 1 za kliknuto 2 za pusteno 
	void AnimateButton(GameObject btn,int Onoff)
	{
		if(btn.name!="PlayerName")
		{
			if(Onoff== 1)
			{
				scaleSave=btn.transform.localScale;
				btn.transform.localScale= scaleSave*0.8f;
				save= new Material(btn.GetComponent<Renderer>().sharedMaterial);
				Material nov =new Material(btn.GetComponent<Renderer>().sharedMaterial);
				nov.color=new Color(save.color.r-0.2f,save.color.g-0.2f,save.color.b-0.2f,save.color.a);
				btn.GetComponent<Renderer>().sharedMaterial=nov;
			}
			else
			{	
				btn.transform.localScale= scaleSave;
				btn.GetComponent<Renderer>().sharedMaterial=save;
			}
		}
	}
	//Funkcija koja baca raycast od vector3 pozicije (uglavnom input.mosueposition ili touchposition)
	// i vraca ime objekta na koji je kliknuto ili prazna string
	//ako postoje vise objekta sa istim imenima ili je potrebna referenca na njega vracati GameObject
	string RaycastFunct(Vector3 v)
	{
		Ray rej=GameObject.Find("Main Camera").GetComponent<Camera>().ScreenPointToRay(v+Vector3.forward*10);
		RaycastHit hit;
		if(Physics.Raycast(rej,out hit,500))
		{
			return hit.transform.name;
			
		}
		return "";
		
	}
	public GameObject test;
	void Start () 
	{
	//postavlja gameobject na odredjenu poziciju u donosu na ekran
	//	test.transform.position=myTransform.camera.ScreenToWorldPoint(new Vector3(Screen.width*0.95f,Screen.height,40));
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			clickedOn=RaycastFunct(Input.mousePosition);
			if(clickedOn!="")
				AnimateButton(GameObject.Find(clickedOn),1);
		}
		if(Input.GetMouseButtonUp(0))
		{
			if(clickedOn!="")
				AnimateButton(GameObject.Find(clickedOn),2);
			string rez=RaycastFunct(Input.mousePosition);
			if(rez==clickedOn)
			{
				
				if(rez!="")
				{
					Debug.Log("Boza je kralj!!!!");
				}
			}
		}
	}
}

