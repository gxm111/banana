using UnityEngine;
using System.Collections;

public class CollectCoin : MonoBehaviour {

	public ParticleSystem coinSparkle;
	public ParticleSystem coinSparkle1;
	public ParticleSystem coinWave;
	GameObject gameManager;
	TextMesh coinsCollectedText;
	Manage manage; // ZA PRVU VERZIJU
	//ManageFull manage; // ZA FINALNU VERZIJU
	Animation coinRotate;
	bool magnetDrag = false;
	Transform Monkey;
	MonkeyController2D controller;
	Renderer novcicMeshRenderer;
	Vector3 orgPos;
	TextMeshEffects effects;
	bool magnetAnimacija = false;

	void Awake () 
	{
		gameManager = GameObject.Find("_GameManager");
		//coinsCollectedText = GameObject.Find("CoinsGamePlayText").GetComponent<TextMesh>(); // ZA PRVU VERZIJU
		//coinsCollectedText = GameObject.Find("TextCoins").GetComponent<TextMesh>(); // ZA FINALNU VERZIJU
		manage = gameManager.GetComponent<Manage>();	// ZA PRVU VERZIJU
		//manage = gameManager.GetComponent<ManageFull>(); // ZA FINALNU VERZIJU
		Monkey = GameObject.FindGameObjectWithTag("Monkey").transform;
		controller = Monkey.GetComponent<MonkeyController2D>();
		//coinRotate = transform.Find("NovcicNovi").animation;
		orgPos = transform.localPosition;
		novcicMeshRenderer = transform.Find("NovcicNovi").GetChild(1).GetComponent<Renderer>();
	}

	void Start ()
	{
		coinsCollectedText = manage.coinsCollectedText;
		effects = coinsCollectedText.GetComponent<TextMeshEffects>();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag == "Monkey" && controller.state != MonkeyController2D.State.wasted)
		{
			if(Manage.coinsCollected %3 == 0)
			{
				if(PlaySounds.soundOn)
				PlaySounds.Play_CollectCoin_3rd();
			}
			else if(Manage.coinsCollected %2 == 0)
			{
				if(PlaySounds.soundOn)
				PlaySounds.Play_CollectCoin_2nd();
			}
			else
			{
				if(PlaySounds.soundOn)
				PlaySounds.Play_CollectCoin();
			}
			coinSparkle.Play();
			coinWave.Play();
			coinSparkle1.Play();
			if(!magnetDrag)
			{
				GetComponent<Animation>().Play();
				Invoke("DisableRenderer",1f);
				Invoke("WaitAndTurnOff",5f);
			}
			else
			{
				Invoke("WaitAndTurnOff",0.5f);
			}
			GetComponent<Collider2D>().enabled = false;

			if(manage.PowerUp_doubleCoins) // ZA FINALNU VERZIJU
			{
				Manage.coinsCollected += 2;
				MissionManager.Instance.CoinEvent(Manage.coinsCollected);
//				manage.AddPoints(12);
			}
			else
			{
				Manage.coinsCollected++;
				//StagesParser.currentMoney++;
				MissionManager.Instance.CoinEvent(Manage.coinsCollected);
//				manage.AddPoints(6);
			}
			coinsCollectedText.text = Manage.coinsCollected.ToString();
			effects.RefreshTextOutline(false,true);
			Manage.points += 10;
			Manage.pointsText.text = Manage.points.ToString();
			Manage.pointsEffects.RefreshTextOutline(false,true);
			magnetDrag = false;

		}
		else if(col.tag == "Magnet")
		{
			//Debug.Log("MAGNEEEET");
			magnetDrag = true;
			StartCoroutine(magnetWorking());
			GetComponent<Collider2D>().enabled = false;
		}
	}

	void DisableRenderer()
	{
		novcicMeshRenderer.enabled = false;
		transform.localScale = Vector3.one;
		magnetAnimacija = false;
	}

	IEnumerator magnetWorking()
	{
		orgPos = transform.localPosition;
		float t = 0;
		while(t < 0.25f)
		{
			if(((transform.position.x < Monkey.position.x + 3f && transform.position.x >= Monkey.position.x-2f) /*&& (transform.position.y < Monkey.position.y + 1.25f && transform.position.y > Monkey.position.y - 1.25f)*/) && !magnetAnimacija)
			{
				magnetAnimacija = true;
				GetComponent<Animation>().Play("CoinCollectedWithMagnet");
			}
			transform.position = Vector3.Lerp(transform.position, Monkey.position + new Vector3(1,1,0)/* + Vector3.right*/, t);
			t += Time.deltaTime/3;
			yield return null;
		}
		if(Manage.coinsCollected %3 == 0)
		{
			if(PlaySounds.soundOn)
				PlaySounds.Play_CollectCoin_3rd();
		}
		else if(Manage.coinsCollected %2 == 0)
		{
			if(PlaySounds.soundOn)
				PlaySounds.Play_CollectCoin_2nd();
		}
		else
		{
			if(PlaySounds.soundOn)
				PlaySounds.Play_CollectCoin();
		}
		//novcicMeshRenderer.enabled = false;
		if(manage.PowerUp_doubleCoins) // ZA FINALNU VERZIJU
		{
			Manage.coinsCollected += 2;
			MissionManager.Instance.CoinEvent(Manage.coinsCollected);
			//				manage.AddPoints(12);
		}
		else
		{
			Manage.coinsCollected++;
			//StagesParser.currentMoney++;
			MissionManager.Instance.CoinEvent(Manage.coinsCollected);
			//				manage.AddPoints(6);
		}
		coinsCollectedText.text = Manage.coinsCollected.ToString();
		effects.RefreshTextOutline(false,true);
		Manage.points += 10;
		Manage.pointsText.text = Manage.points.ToString();
		Manage.pointsEffects.RefreshTextOutline(false,true);
		magnetDrag = false;


		Invoke("DisableRenderer",1f);
		Invoke("WaitAndTurnOff",5f);
	}

	void WaitAndTurnOff()
	{
		//gameObject.SetActive(false);
		//Debug.Log("rezni");
		if(MonkeyController2D.canRespawnThings)
		{
			//transform.localPosition = Vector3.zero;
			//transform.localScale = Vector3.one;
			transform.localPosition = orgPos;
			novcicMeshRenderer.enabled = true;
			GetComponent<Collider2D>().enabled = true;
			//Debug.Log("scale: " + transform.localScale);
		}
	}
	
}
