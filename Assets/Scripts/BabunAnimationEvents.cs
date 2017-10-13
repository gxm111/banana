using UnityEngine;
using System.Collections;

public class BabunAnimationEvents : MonoBehaviour 
{
	Animator anim;
	Transform babun;

	void Awake()
	{
		babun = transform.GetChild(0);
		anim = babun.GetComponent<Animator>();
	}

	void startPatrolRight()
	{
		anim.SetBool("changeSide",true);
	}
	
	void startPatrolLeft()
	{
		anim.SetBool("changeSide",false);
	}

	void landBaboon()
	{
		anim.SetBool("Land",true);
	}

	void startJumpBaboon()
	{
		anim.Play("Jump");
		anim.SetBool("Land",false);
	}

}
