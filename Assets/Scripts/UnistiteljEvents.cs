using UnityEngine;
using System.Collections;

public class UnistiteljEvents : MonoBehaviour {

	public Transform terrainPool;
	Transform enemyPool;
	Transform environmentPool;
	Transform coinsPool;
	Transform specialPool;
	int start = 0;
	int brojac = 0;

	void Start()
	{
		enemyPool = GameObject.Find("__EnemiesPool").transform;
		environmentPool = GameObject.Find("__EnvironmentPool").transform;
		coinsPool = GameObject.Find("__CoinsPool").transform;
		specialPool = GameObject.Find("__SpecialPool").transform;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(start > 0)
		{
			if(col.name.Equals("__GranicaDesno"))
			{
				brojac++;
				StartCoroutine(vratiNazadUPool(col));
			}
		}
		else
		{
			if(col.name.Equals("__GranicaDesno"))
			{
				brojac++;
				LevelFactory.instance.Reposition();
				start++;
				col.GetComponent<Collider2D>().enabled = false;
				TrenutnoSeKoristi();
			}
		}
	}

//	IEnumerator vratiNazadUPool(Collider2D col)
//	{
//		Transform prefab = col.transform.parent;
//		LevelPrefabProperties prefabProperties = prefab.GetComponent<LevelPrefabProperties>();
//		col.collider2D.enabled = false;
//		prefabProperties.slobodanTeren = true;
//		prefab.parent = terrainPool;
//		prefab.position = prefabProperties.originalPosition;
//		EntityProperties entityProperties;
//		//smestanje neprijatelja nazad u pool
//		for(int i=0; i< prefabProperties.enemies_Slots_Count; i++)
//		{
//			if(prefabProperties.enemiesSlots[i].childCount > 0)
//			{
//				Transform enemy = prefabProperties.enemiesSlots[i].GetChild(0);
//				entityProperties = enemy.GetComponent<EntityProperties>();
//				//if(entityProperties.Type == 1)
//				//	enemy.GetChild(0).Find("BaboonReal").Find("_MajmunceNadrlja").GetComponent<BabunDogadjaji_new>().enabled = false;
//				enemy.parent = enemyPool;
//				entityProperties.slobodanEntitet = true;
//				enemy.localPosition = entityProperties.originalPosition;//new Vector3(0,0,-5);
//			}
//		}
//		yield return new WaitForSeconds(0.02f);
//		//smestanje environment nazad u pool
//		for(int i=0; i< prefabProperties.environment_Slots_Count; i++)
//		{
//			if(prefabProperties.environmentsSlots[i].childCount > 0)
//			{
//				Transform environment = prefabProperties.environmentsSlots[i].GetChild(0);
//				entityProperties = environment.GetComponent<EntityProperties>();
//				//float z = environment.localPosition.z;
//				environment.parent = environmentPool;
//				entityProperties.slobodanEntitet = true;
//				environment.localPosition = entityProperties.originalPosition;//new Vector3(0,0,z);
//				if(entityProperties.DozvoljenoSkaliranje)
//				{
//					environment.localScale = entityProperties.originalScale;
//				}
//			}
//		}
//		yield return new WaitForSeconds(0.02f);
//		//smestanje novcica nazad u pool
//		for(int i=0; i< prefabProperties.coins_Slots_Count; i++)
//		{
//			if(prefabProperties.coinsSlots[i].childCount > 0)
//			{
//				Transform coins = prefabProperties.coinsSlots[i].GetChild(0);
//				entityProperties = coins.GetComponent<EntityProperties>();
//				//float z = coins.localPosition.z;
//				coins.parent = coinsPool;
//				entityProperties.slobodanEntitet = true;
//				coins.localPosition = entityProperties.originalPosition;//new Vector3(0,0,z);
//			}
//		}
//		yield return new WaitForSeconds(0.02f);
//		//smestanje special nazad u pool
//		for(int i=0; i< prefabProperties.special_Slots_Count; i++)
//		{
//			if(prefabProperties.specialSlots[i].childCount > 0)
//			{
//				Transform special = prefabProperties.specialSlots[i].GetChild(0);
//				entityProperties = special.GetComponent<EntityProperties>();
//				//float z = special.localPosition.z;
//				if(entityProperties.Type == 2)
//				{
//					entityProperties.transform.GetChild(0).GetComponent<BarrelExplode>().ObnoviBure();
//				}
//				special.parent = specialPool;
//				entityProperties.slobodanEntitet = true;
//				special.localPosition = entityProperties.originalPosition;//new Vector3(0,0,z);
//			}
//		}
//		yield return new WaitForSeconds(0.02f);
//		if(!LevelFactory.trebaFinish)
//			LevelFactory.instance.Reposition();
//	}

	IEnumerator vratiNazadUPool(Collider2D col)
	{
		Transform prefab = col.transform.parent;
		LevelPrefabProperties prefabProperties = prefab.GetComponent<LevelPrefabProperties>();
		col.GetComponent<Collider2D>().enabled = false;
		//prefabProperties.slobodanTeren = true;
		prefabProperties.slobodanTeren = 1;
		EntityProperties entityProperties;
		
		//oslobadjanje neprijatelja
		for(int i=0;i<enemyPool.childCount;i++)
		{
			Transform enemy = enemyPool.GetChild(i);
			entityProperties = enemy.GetComponent<EntityProperties>();

			if(!entityProperties.slobodanEntitet)
			{
				if(!entityProperties.trenutnoJeAktivan)
					entityProperties.trenutnoJeAktivan = true;
				else if(entityProperties.instanciran)
					Destroy(entityProperties.gameObject);
				else
				{
					if(entityProperties.Type == 18)
					{
						entityProperties.transform.GetChild(0).GetChild(0).GetComponent<BarrelExplode>().ObnoviBure();
					}
					entityProperties.slobodanEntitet = true;
				}
			}

		}
		yield return new WaitForSeconds(0.02f);
		//oslobadjanje environment
		for(int i=0;i<environmentPool.childCount;i++)
		{
			Transform enemy = environmentPool.GetChild(i);
			entityProperties = enemy.GetComponent<EntityProperties>();
			if(!entityProperties.slobodanEntitet)
			{
				if(!entityProperties.trenutnoJeAktivan)
					entityProperties.trenutnoJeAktivan = true;
				else				
					entityProperties.slobodanEntitet = true;
			}
		}
		yield return new WaitForSeconds(0.02f);
		//oslobadjanje novcica
		for(int i=0;i<coinsPool.childCount;i++)
		{
			Transform enemy = coinsPool.GetChild(i);
			entityProperties = enemy.GetComponent<EntityProperties>();
			if(!entityProperties.slobodanEntitet)
			{
				if(!entityProperties.trenutnoJeAktivan)
					entityProperties.trenutnoJeAktivan = true;
				else
					entityProperties.slobodanEntitet = true;
			}
		}
		yield return new WaitForSeconds(0.02f);
		//oslobadjanje special
		for(int i=0;i<specialPool.childCount;i++)
		{
			Transform enemy = specialPool.GetChild(i);
			entityProperties = enemy.GetComponent<EntityProperties>();
			if(!entityProperties.slobodanEntitet)
			{

				if(!entityProperties.trenutnoJeAktivan)
					entityProperties.trenutnoJeAktivan = true;
				else
				{
					if(entityProperties.Type == 2)
					{
						entityProperties.transform.GetChild(0).GetComponent<BarrelExplode>().ObnoviBure();
					}
					entityProperties.slobodanEntitet = true;
				}
			}
		}
		yield return new WaitForSeconds(0.02f);
		//pozicioniranje novog terena
		if(!LevelFactory.trebaFinish)
			LevelFactory.instance.Reposition();
		
	}
	
	void TrenutnoSeKoristi()
	{
		EntityProperties entityProperties;
		for(int i=0;i<enemyPool.childCount;i++)
		{
			Transform enemy = enemyPool.GetChild(i);
			entityProperties = enemy.GetComponent<EntityProperties>();
			if(!entityProperties.trenutnoJeAktivan)
				entityProperties.trenutnoJeAktivan = true;
		}
		for(int i=0;i<environmentPool.childCount;i++)
		{
			Transform enemy = environmentPool.GetChild(i);
			entityProperties = enemy.GetComponent<EntityProperties>();
			if(!entityProperties.trenutnoJeAktivan)
				entityProperties.trenutnoJeAktivan = true;
		}
		for(int i=0;i<coinsPool.childCount;i++)
		{
			Transform enemy = coinsPool.GetChild(i);
			entityProperties = enemy.GetComponent<EntityProperties>();
			if(!entityProperties.trenutnoJeAktivan)
				entityProperties.trenutnoJeAktivan = true;
		}
		for(int i=0;i<specialPool.childCount;i++)
		{
			Transform enemy = specialPool.GetChild(i);
			entityProperties = enemy.GetComponent<EntityProperties>();
			if(!entityProperties.trenutnoJeAktivan)
				entityProperties.trenutnoJeAktivan = true;
		}
	}
}
