using UnityEngine;
using System.Collections;

public class MonkeyParticles : MonoBehaviour
{
	public ParticleSystem doubleJump;
	public ParticleSystem doubleJumpEffect;
	public ParticleSystem blast;
	public ParticleSystem death;
	public ParticleSystem deathDrag;
	public ParticleSystem hitBlast;
	public ParticleSystem hitSmoke;
	public ParticleSystem grabDust;
	// ParticleSystem coinSparkle;
	//public ParticleSystem coinWave;
	

	void particleDoubleJump()
	{
		doubleJump.Emit(25);
	}
	void particleDoubleJumpEffect()
	{
		doubleJumpEffect.Emit(1);
	}
	void particleBlast()
	{
		blast.Emit(1);
	}
	void particleDeath()
	{
		death.Emit(100);
	}
	void particleDeathDrag()
	{
		deathDrag.Play();
	}
	void particleHitBlast()
	{
		hitBlast.Emit(100);
	}
	void particleHitSmoke()
	{
		hitSmoke.Play();
	}
	void particleGrabDust()
	{
		grabDust.Play();
	}
//	void particleCoinSparkle()
//	{
//		coinSparkle.Emit(25);
//	}
//	void particleCoinWave()
//	{
//		coinWave.Emit(1);
//	}
	void StartClimbing()
	{
		GameObject.FindGameObjectWithTag("Monkey").SendMessage("climb");
	}

}
