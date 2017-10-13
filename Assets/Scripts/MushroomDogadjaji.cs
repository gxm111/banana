using UnityEngine;
using System.Collections;

public class MushroomDogadjaji : MonoBehaviour {

	Animator anim;
	MonkeyController2D playerController;
	public enum Tip { Feder, Bunika };
	public Tip tip;
	int brojac = 0;

	void Awake () 
	{
		anim = GetComponent<Animator>();
		playerController = GameObject.FindGameObjectWithTag("Monkey").GetComponent<MonkeyController2D>();
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag == "Monkey")
		{
			if(playerController.state == MonkeyController2D.State.jumped)
			{
				if(tip == Tip.Feder)
				{
//					if(brojac == 0)
//					{
//						playerController.rigidbody2D.velocity = Vector2.zero;
//						collider2D.enabled = false;
//						brojac++;
//						Debug.Log("Usao i ovde");
//					}
//					else if(brojac == 1)
					{
						brojac = 0;
						//playerController.rigidbody2D.velocity = Vector2.zero;
						if(PlaySounds.soundOn)
							PlaySounds.Play_MushroomBounce();
						GetComponent<Collider2D>().enabled = false;

						//if(playerController.isSliding)
							StartCoroutine(DelayAndBounce());
//						else
//						{
//							anim.Play("Blong");
//							playerController.rigidbody2D.AddForce(new Vector2(1600,4300));
//						}
					}

				}
				else if(tip == Tip.Bunika)
				{
					if(PlaySounds.soundOn)
						PlaySounds.Play_MushroomBounce();
					GetComponent<Collider2D>().enabled = false;
					anim.Play("Blong");
					playerController.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
					playerController.GetComponent<Rigidbody2D>().AddForce(new Vector2(1000,1000));
					Camera.main.GetComponent<Animator>().Play("CameraMovePoisonMushroom");
					//GameObject.Find("Background").renderer.material.color = new Color(148,57,57,255);
					//GameObject.Find("Background").renderer.material.color = new Color(0.58f,0.22f,0.22f,1);
					GameObject.Find("Background").GetComponent<Renderer>().material.color = new Color(0.82f,0.07f,0.75f,1);
				}

			}
			else if(playerController.state == MonkeyController2D.State.running)
				GetComponent<Collider2D>().enabled = false;

			Invoke("UkljuciColliderOpet",1f);
		}
	}

	IEnumerator DelayAndBounce()
	{
		playerController.transform.position = new Vector3(transform.Find("Goal").position.x, transform.Find("Goal").position.y, playerController.transform.position.z);
		playerController.GetComponent<Rigidbody2D>().isKinematic = true;
//		Instantiate(GameObject.Find("PrinceGorilla"),playerController.transform.position,Quaternion.identity);
		//yield return new WaitForSeconds(0.035f);//0.045
		anim.Play("Blong");
		playerController.state = MonkeyController2D.State.jumped;
		Transform goal = transform.Find("BounceObj");
		goal.GetComponent<Animation>().Play("MonkeyBounceFromMushroom");
		playerController.mushroomJumped = true;
		//playerController.SlideNaDole = false;
		//playerController.Glide = false;
		//playerController.canGlide = false;
		//playerController.isSliding = false;
		playerController.GetComponent<Rigidbody2D>().drag = 0;
		playerController.animator.Play(playerController.fall_State);
		while(goal.GetComponent<Animation>().IsPlaying("MonkeyBounceFromMushroom"))
		{
			if(!playerController.mushroomJumped)
			{
				goal.GetComponent<Animation>().Stop();
			}
			yield return null;
			playerController.transform.position = new Vector3(goal.position.x,goal.position.y,playerController.transform.position.z);
		}
//		if(playerController.inAir)
//		{
//			Debug.Log("U VAZDUHU, MANJA SILA");
//			playerController.rigidbody2D.velocity = Vector2.zero;
//			//playerController.rigidbody2D.AddForce(new Vector2(1600,4300));
//			playerController.rigidbody2D.velocity = new Vector2(0,60);
//		}
//		else
//		{
//			Debug.Log("inAir je false, VECA SILA");
//			playerController.rigidbody2D.velocity = Vector2.zero;
//			playerController.rigidbody2D.velocity = new Vector2(0,30);
//			//playerController.rigidbody2D.AddForce(new Vector2(1700,4400));
//			yield return new WaitForSeconds(0.025f);
//			playerController.inAir = true;
//			playerController.animator.Play(playerController.fall_State);
//		}
//		playerController.animator.Play(playerController.fall_State);
//		playerController.state = MonkeyController2D.State.jumped;

	}

	void UkljuciColliderOpet()
	{
		GetComponent<Collider2D>().enabled = true;
	}

}
