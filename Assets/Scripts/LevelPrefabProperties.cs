using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelPrefabProperties : MonoBehaviour {

	//public bool slobodanTeren = true;
	public int slobodanTeren = 2; // 2 - slobodan teren, 1 - tek pretrcan, 0 - zauzet
	public int enemies_Slots_Count = 0;
	public int environment_Slots_Count = 0;
	public int coins_Slots_Count = 0;
	public int special_Slots_Count = 0;
	public List<Transform> environmentsSlots;
	public List<Transform> enemiesSlots;
	public List<Transform> coinsSlots;
	public List<Transform> specialSlots;
	public int minimumLevel;
	public int maximumLevel;
	public int tipTerena;
	public int[] moguDaSeNakace;
	[HideInInspector] public Vector3 originalPosition;
	Transform tipSlota;
	public int brojUNizu;

	void Awake()
	{
		originalPosition = transform.position;

//		foreach( Transform child in transform)
//		{
//			if(child.name.StartsWith("Enemy"))
//			{
//				enemies_Slots_Count++;
//				enemiesSlots.Add(child);
//			}
//			else if(child.name.StartsWith("Environment"))
//			{
//				environment_Slots_Count++;
//				environmentsSlots.Add(child);
//			}
//			else if(child.name.StartsWith("CoinsStart"))
//			{
//				coins_Slots_Count++;
//				coinsSlots.Add(child);
//			}
//		}
		tipSlota = transform.Find("Enemies_Slots");
		if(tipSlota.childCount > 0)
		for(int i=0;i<tipSlota.childCount; i++)
		{
			enemies_Slots_Count++;
			enemiesSlots.Add(tipSlota.GetChild(i));
		}
		tipSlota = transform.Find("Environment_Slots");
		if(tipSlota.childCount > 0)
		for(int i=0;i<tipSlota.childCount; i++)
		{
			environment_Slots_Count++;
			environmentsSlots.Add(tipSlota.GetChild(i));
		}
		tipSlota = transform.Find("CoinsStart_Slots");
		if(tipSlota.childCount > 0)
		for(int i=0;i<tipSlota.childCount; i++)
		{
			coins_Slots_Count++;
			coinsSlots.Add(tipSlota.GetChild(i));
		}
		tipSlota = transform.Find("Special_Slots");
		if(tipSlota.childCount > 0)
		for(int i=0;i<tipSlota.childCount; i++)
		{
			special_Slots_Count++;
			specialSlots.Add(tipSlota.GetChild(i));
		}
		slobodanTeren = 2;
	}

//	public void ResetUsability_CoinsSlots()
//	{
//		for(int i=0; i<coins_Slots_Count; i++)
//		{
//			coinsSlots[i].GetComponent<EntityProperties>().currentlyUsable = false;
//		}
//	}
//
//	public void ResetUsability_EnemiesSlots()
//	{
//		for(int i=0; i<enemies_Slots_Count; i++)
//		{
//			enemiesSlots[i].GetComponent<EntityProperties>().currentlyUsable = false;
//		}
//	}
//
//	public void ResetUsability_EnvironmentSlots()
//	{
//		for(int i=0; i<environment_Slots_Count; i++)
//		{
//			environmentsSlots[i].GetComponent<EntityProperties>().currentlyUsable = false;
//		}
//	}
}
