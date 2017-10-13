using UnityEngine;
using System.Collections;

public class LianaAnimationEvent : MonoBehaviour {

	MonkeyController2D player;
	public Transform lijanaTarget;
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Monkey").GetComponent<MonkeyController2D>();
	}
	
	public void OtkaciMajmuna()
	{
		//if(player.lijana)
		{
			StartCoroutine(SacekajIOtkaciMajmuna());
		}
	}
	
	IEnumerator SacekajIOtkaciMajmuna()
	{
		//yield return new WaitForSeconds(0.33f);
		yield return new WaitForSeconds(0.6f);
		player.OtkaciMajmuna();
	}

}
