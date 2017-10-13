using UnityEngine;
using System.Collections;

public class EntityProperties : MonoBehaviour {

	public int Type;
	[HideInInspector] public Vector3 originalPosition;
	public int minimumLevel = 1;
	public int Verovatnoca = 100;
	[HideInInspector] public bool currentlyUsable;
	public bool DozvoljenoSkaliranje = false;
	[HideInInspector] public Vector3 originalScale;
	public int maxBrojPojavljivanja;
	[HideInInspector] public int brojPojavljivanja = 0;
	public bool slobodanEntitet = true;
	public bool trenutnoJeAktivan = false;
	[HideInInspector] public bool smanjivanjeVerovatnoce = false;
	[HideInInspector] public bool instanciran = false;

	void Awake ()
	{
		originalPosition = transform.localPosition;
		originalScale = transform.localScale;
	}
}
