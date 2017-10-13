using UnityEngine;
using System.Collections;

public class CoinMagnet : MonoBehaviour {


	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag == "Novcic")
		col.gameObject.SetActive(false);
	}

}
