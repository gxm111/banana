using UnityEngine;
using System.Collections;

public class RotateLogo : MonoBehaviour {

	public static bool animationDone = false;

	void Start()
	{
		Invoke ("PlaySound", 0.15f);
	}
	void OnTriggerEnter(Collider col)
	{
		GetComponent<Collider>().enabled = false;
		GetComponent<Animator>().Play("RotateLogo_v2");
	}

	void AnimationDone()
	{
		animationDone = true;
	}

	void PlaySound()
	{
		transform.GetChild(0).GetComponent<AudioSource>().Play();
	}
}
