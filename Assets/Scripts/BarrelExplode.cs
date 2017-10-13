using UnityEngine;
using System.Collections;

public class BarrelExplode : MonoBehaviour {

	Animator anim;
	public ParticleSystem eksplozija1;
	public ParticleSystem eksplozija2;
	public ParticleSystem eksplozija3;
	public ParticleSystem eksplozija4;
	bool razbijenoBure = false;
	public Animator coinsReward;

	void Start ()
	{
		anim = transform.GetChild(0).GetComponent<Animator>();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag.Equals("Monkey"))
		{
			anim.Play("BarrelBoom");
			eksplozija1.Play();
			eksplozija2.Play();
			eksplozija3.Play();
			eksplozija4.Play();
			razbijenoBure = true;
			if(!name.Contains("TNT"))
			{
				if(PlaySounds.soundOn)
					PlaySounds.Play_Bure_Eksplozija();

				//@@@@@@ DODATO
				GetComponent<Collider2D>().enabled = false;

				Manage.barrelsSmashed++;
				MissionManager.Instance.BarrelEvent(Manage.barrelsSmashed);
				Manage.points += 20;
				Manage.pointsText.text = Manage.points.ToString();
				Manage.pointsEffects.RefreshTextOutline(false,true);
				int value = Random.Range(5,11);
				coinsReward.transform.Find("+3Coins").GetComponent<TextMesh>().text = coinsReward.transform.Find("+3Coins/+3CoinsShadow").GetComponent<TextMesh>().text = "+"+value;
				coinsReward.Play("FadeOutCoins");
				Manage.coinsCollected+=value;
				MissionManager.Instance.CoinEvent(Manage.coinsCollected);
				Manage.Instance.coinsCollectedText.text = Manage.coinsCollected.ToString();
				Manage.Instance.coinsCollectedText.GetComponent<TextMeshEffects>().RefreshTextOutline(false,true);
			}
			else
				if(PlaySounds.soundOn)
					PlaySounds.Play_TNTBure_Eksplozija();
		}
	}

	public void ObnoviBure()
	{
		if(razbijenoBure)
		{
			//@@@@@@ DODATO
			GetComponent<Collider2D>().enabled = true;

			anim.SetTrigger("ObnoviBure");
			razbijenoBure = false;
			transform.GetChild(0).Find("Barrel").gameObject.SetActive(true);
			transform.GetChild(0).Find("BarrelBrokenBottom").gameObject.SetActive(false);
		}
	}
}
