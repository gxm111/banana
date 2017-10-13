using UnityEngine;
using System.Collections;

public class CheckGroundUp : MonoBehaviour {

	MonkeyController2D player;
	public bool gornji;
	public bool donji;

	//bool neTrebaDaProdje = false;

	void Awake()
	{
		player = transform.parent.GetComponent<MonkeyController2D>();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if((player.state == MonkeyController2D.State.jumped || player.state == MonkeyController2D.State.climbUp || player.state == MonkeyController2D.State.wasted) && col.tag != "Finish")
		{
			float triggerPosition;
			if(col.transform.childCount > 0)
				triggerPosition = col.transform.Find("TriggerPositionDown").position.y;
			else
				triggerPosition = col.transform.position.y;

			//Debug.Log("playerY: " + player.transform.position.y + ", terenY: " + triggerPosition + ", playerTrigger: " + player.collider2D.isTrigger);
			//if(player.rigidbody2D.velocity.y >= -90) //TEST USLOV: ukoliko je brzina korisnika manja od -90 znaci da je slajdovao sa velike visine i tada ne treba da proverava uslove da prodje kroz platformu
			if(!player.isSliding)
			{
				if(player.transform.position.y < triggerPosition)// || (player.transform.position.y >= col.transform.position.y && !player.triggerCheckDownTrigger && !player.triggerCheckDownBehind))//if(!player.neTrebaDaProdje)
				{
					//Physics2D.IgnoreLayerCollision(13,18,true);
					//Debug.Log("-1");
					transform.parent.GetComponent<Collider2D>().isTrigger = true;
				}
				else if(player.transform.position.y >= triggerPosition)
				{
					//Debug.Log("0");
					if(!player.triggerCheckDownTrigger)
					{
						if(!player.triggerCheckDownBehind)
						{
							transform.parent.GetComponent<Collider2D>().isTrigger = true;
							//Debug.Log("1");
						}
						else
						{
							;//Debug.Log("2");
						}
					}
					else if(!player.triggerCheckDownBehind)
					{
						;//Debug.Log("3");
					}
					else 
					{
						;//Debug.Log("4");
					}
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		//Debug.Log("izadj, " + player.collider2D.isTrigger);
		if(player.GetComponent<Collider2D>().isTrigger && !player.triggerCheckDownTrigger)
		{
			//Debug.Log("izasao");
			player.GetComponent<Collider2D>().isTrigger = false;
		}
	}

//	void OnCollisionEnter2D(Collision2D col)
//	{
//		//if(transform.position.y + 0.5f < col.contacts[0].point.y)
//		{
//			//Physics2D.IgnoreLayerCollision(13,18,true);
//			transform.parent.collider2D.isTrigger = true;
//			Debug.Log("Trigeerovao ga");
//		}
//	}

}
