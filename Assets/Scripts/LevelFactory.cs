using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelFactory : MonoBehaviour {

	public Transform currentLevelComponents;
	public Transform terrainPool;
	public int terenaUPocetku;
	public List<GameObject> teren;
	static Transform GranicaDesno;
	Transform unistitelj;
	public static LevelFactory instance;
	public static int level = 1;
	public int levelRuchno = 0;
	public int overallDifficulty;
	Transform enemyPool;
	Transform environmentPool;
	Transform coinsPool;
	Transform specialPool;
	int[] TereniKojiMoguDaDodju;
	List<int> suzenaListaTerena;
	List<int> suzenaListaObjekata;

	LevelPrefabProperties prefabProperties;
	EntityProperties entityProperties;
	SlotProperties slotProperties;

	Transform finishHolder;
	public static bool trebaFinish = false;
	//Transform defaultTeren;
	int dg;
	int gg;
	int tour;
	TextMesh missionDescription;

	public int leteciBabuni = 0;
	public int leteceGorile = 0;
	public int boomerangBabuni = 0;
	public int kopljeGorile = 0;
	public int plaviDijamant = 0;
	public int crveniDijamant = 0;
	public int zeleniDijamant = 0;

	[HideInInspector] public float leteciBabuni_Kvota = 0;
	[HideInInspector] public float leteceGorile_Kvota = 0;
	[HideInInspector] public float boomerangBabuni_Kvota = 0;
	[HideInInspector] public float kopljeGorile_Kvota = 0;
	[HideInInspector] public float plaviDijamant_Kvota = 0;
	[HideInInspector] public float crveniDijamant_Kvota = 0;
	[HideInInspector] public float zeleniDijamant_Kvota = 0;

	[HideInInspector] public float leteciBabuni_Kvota_locked = 0;
	[HideInInspector] public float leteceGorile_Kvota_locked = 0;
	[HideInInspector] public float boomerangBabuni_Kvota_locked = 0;
	[HideInInspector] public float kopljeGorile_Kvota_locked = 0;
	[HideInInspector] public float plaviDijamant_Kvota_locked = 0;
	[HideInInspector] public float crveniDijamant_Kvota_locked = 0;
	[HideInInspector] public float zeleniDijamant_Kvota_locked = 0;

	int leteciBabuni_postoji_u_poolu = 0;	// 0 - default, 1 - postoji u pool-u i slobodan je, 2 - ne postoji u pool-u ili postoji ali je zauzet, treba da se instancira
	int leteceGorile_postoji_u_poolu = 0;
	int boomerangBabuni_postoji_u_poolu = 0;
	int kopljeGorile_postoji_u_poolu = 0;
	int plaviDijamant_postoji_u_poolu = 0;
	int crveniDijamant_postoji_u_poolu = 0;
	int zeleniDijamant_postoji_u_poolu = 0;

	int postavljen_u_prefabu_leteciBabuni = 0;		// 0 - default, 1 - postavljen, 2 - nije postavljen
	int postavljen_u_prefabu_leteceGorile = 0;
	int postavljen_u_prefabu_boomerangBabuni = 0;
	int postavljen_u_prefabu_kopljeGorile = 0;

	bool prviPrefab = true;
	int brojPosebnihNeprijatelja = 0;
	int brojPosebnihDijamanata = 0;

	public GameObject[] enemiesForInstantiate;
	public GameObject[] specialsForInstantiate;

	[HideInInspector] public bool magnetCollected = false;
	[HideInInspector] public bool doubleCoinsCollected = false;
	[HideInInspector] public bool shieldCollected = false;

	List<Transform> prefaboviUIgri;
	int aktivniPrefabUIgri = 0;
	float vremeTriggeraPoslednjegPrefaba = 0;
	float vremeTriggeraNovogPrefaba = 0;


	//@@@@@@@@@@@@
	int brojacPrefaba;

	void Awake ()
	{
		instance = this;
//		if(levelRuchno != 0)
//		{
//			level = levelRuchno;
//			levelRuchno = 0;
//		}
		level = StagesParser.currentLevel;

	}

	void Start ()
	{

		if(!PlayerPrefs.HasKey("Tour"))
			tour = 1;
		else
			tour = PlayerPrefs.GetInt("Tour");

		if(StagesParser.bonusLevel)
		{
			//missionDescription.text = MissionManager.missions[level-1].description_en;
			MissionManager.OdrediMisiju(MissionManager.missions.Length-1, false);
		}
		else if(StagesParser.odgledaoTutorial > 0)
		{
			//missionDescription.text = MissionManager.missions[MissionManager.missions.Length-1].description_en;
			MissionManager.OdrediMisiju(level-1, false);
		}

		Tezina();
		KorigujVerovatnocuZbogMisije(1,0);

		trebaFinish = false;
		enemyPool = GameObject.Find("__EnemiesPool").transform;
		environmentPool = GameObject.Find("__EnvironmentPool").transform;
		coinsPool = GameObject.Find("__CoinsPool").transform;
		specialPool = GameObject.Find("__SpecialPool").transform;

		//finishHolder = GameObject.Find("FinishHolder").transform;
		//defaultTeren = GameObject.Find("DefaultTerenPrefab").transform;

		unistitelj = Camera.main.transform.Find("Unistitelj");
		unistitelj.position = new Vector3(Camera.main.transform.position.x - Camera.main.orthographicSize*Camera.main.aspect, unistitelj.position.y, unistitelj.position.z);
		GranicaDesno = GameObject.Find("Level_Start_01").transform.Find("__GranicaDesno");

		List<int> tempNiz = new List<int>();
		for(int i=0;i<teren.Count;i++)
		{
			GameObject temp = teren[i];
			prefabProperties = temp.GetComponent<LevelPrefabProperties>();
			if(prefabProperties.minimumLevel <= level)
				tempNiz.Add(i);
		}
		//"DA SE VRATI PRVI KOVCEG DA NIJE UVEK OTKLJUCAN!!!!!"
		//"DA SE NAPRAVI JOS JEDAN PREFAB KOJI CE DA PREDSTAVLJA FINISH!!!!!"
		//"DA SE PROVERI DA LI LEPO DODAJE NOVCICE NA RAZBIJANJE BURICA I AKO TREBA DA SE UBACI +3COINSSHADOW NA BURETU U SVIM SCENAMA!!!!!"
		//"DA SE PREPRAVI ANIMACIJA ZA COINSBAGCOLLECT ILI DA SE STAVI DA SE VRECA MOZE JAVITI SAMO JEDANPUT, A DA POSTOJE VISE VRECA NA SCENI!!!!!"

		suzenaListaTerena = new List<int>();
		suzenaListaObjekata = new List<int>();
		prefaboviUIgri = new List<Transform>();

		//@@@@@@@@@@@@
		//brojacPrefaba = terenaUPocetku-1;
		brojacPrefaba = 0;

		//pocetni teren
		for(int i=0; i<terenaUPocetku; i++)
		{

			int j = Random.Range(0,tempNiz.Count);
			GameObject temp = teren[tempNiz[j]];
			tempNiz.RemoveAt(j);

			temp.transform.position = GranicaDesno.position;
			//temp.transform.parent = currentLevelComponents; //bez parentovanja
			//temp.GetComponent<LevelPrefabProperties>().pooled = false;
			prefabProperties = temp.GetComponent<LevelPrefabProperties>();
			//prefabProperties.slobodanTeren = false;
			prefabProperties.slobodanTeren = 0;
			prefabProperties.brojUNizu = i;

			GranicaDesno = temp.transform.Find("__GranicaDesno");
			TereniKojiMoguDaDodju = prefabProperties.moguDaSeNakace;

			prefaboviUIgri.Add(temp.transform);

			if(i == 0)
			{
				brojacPrefaba++;
				//dodavanje neprijatelja
				StartCoroutine(DodavanjeNeprijatelja(prefabProperties));

				//dodavanje environment
				StartCoroutine(DodavanjeEnvironment(prefabProperties));

				//dodavanje novcica
				StartCoroutine(DodavanjeNovcica(prefabProperties));

				//dodavanje special
				StartCoroutine(DodavanjeSpecial(prefabProperties));

				aktivniPrefabUIgri++;
			}
		}

		if(StagesParser.bonusLevel)
		{
			Transform finish = terrainPool.Find("TerenPrefab11_Finish");
			finish.position = GranicaDesno.position;
			GranicaDesno = finish.Find("__GranicaDesno");
			GranicaDesno.GetComponent<Collider2D>().enabled = false;
			finish.parent = null;
		}
	}

	public bool mozeDaSeNakaci(int tipTerena)
	{
		for(int i=0; i<TereniKojiMoguDaDodju.Length;i++)
		{
			if(tipTerena == TereniKojiMoguDaDodju[i])
				return true;
		}
		return false;
	}

	public void Reposition()
	{
//		if(Manage.Instance.aktivnoVreme >= 40 && !povecajVerovatnocu)//if(Time.time - Manage.goTrenutak >= 40 && !povecajVerovatnocu)
//		{
//			povecajVerovatnocu = true;
//			KorigujVerovatnocuZbogMisije(2,0);
//			//Debug.Log("KRNEM IZ LEVEL FACTORY FOR REAL");
//		}
		//StartCoroutine(RepositionCoroutine());
		StartCoroutine(RepositionAndFillStuffCoroutine());
	}

	public IEnumerator RepositionAndFillStuffCoroutine()
	{
		yield return new WaitForSeconds(0.25f);
		//repozicioniranje terena
		LevelPrefabProperties prefabProperties;
		GameObject temp = null;
		
		if(suzenaListaTerena.Count > 0)
			suzenaListaTerena.Clear();
		
		for(int i=0; i<terrainPool.childCount; i++)
		{
			prefabProperties = terrainPool.GetChild(i).GetComponent<LevelPrefabProperties>();
			if(prefabProperties.slobodanTeren == 2)//if(prefabProperties.slobodanTeren)
			{
				if((System.Array.IndexOf(TereniKojiMoguDaDodju, prefabProperties.tipTerena) > -1 && prefabProperties.minimumLevel <= level && prefabProperties.maximumLevel >= level) || (TereniKojiMoguDaDodju.Length == 0 && prefabProperties.tipTerena != -10) )
				{
					suzenaListaTerena.Add(i);
				}
			}
			else if(prefabProperties.slobodanTeren == 1)
				prefabProperties.slobodanTeren = 2;
		}
		
		if(suzenaListaTerena.Count > 0)
		{
			int q = Random.Range(0,suzenaListaTerena.Count);
			temp = terrainPool.GetChild(suzenaListaTerena[q]).gameObject;
		}
		else
		{
			//temp = defaultTeren.gameObject;
		}
		yield return new WaitForSeconds(0.1f);
		
		if(!trebaFinish)
		{
			//@@@@@@@@@@@@
			brojacPrefaba++;
			
			temp.transform.position = GranicaDesno.position;
			//temp.transform.parent = currentLevelComponents; //bez parentovanja
			//temp.GetComponent<LevelPrefabProperties>().pooled = false;
			prefabProperties = temp.GetComponent<LevelPrefabProperties>();
			//prefabProperties.slobodanTeren = false;
			prefabProperties.slobodanTeren = 0;
			GranicaDesno = temp.transform.Find("__GranicaDesno");
			GranicaDesno.GetComponent<Collider2D>().enabled = true;
			TereniKojiMoguDaDodju = prefabProperties.moguDaSeNakace;
			yield return new WaitForSeconds(0.1f);

			prefaboviUIgri.Add(temp.transform);

			vremeTriggeraNovogPrefaba = Manage.Instance.aktivnoVreme;
			if(vremeTriggeraNovogPrefaba - vremeTriggeraPoslednjegPrefaba >= 15)
			{
				aktivniPrefabUIgri++;
			}
			vremeTriggeraPoslednjegPrefaba = vremeTriggeraNovogPrefaba;

			prefabProperties = prefaboviUIgri[aktivniPrefabUIgri].GetComponent<LevelPrefabProperties>();
			aktivniPrefabUIgri++;
			
			//dodavanje neprijatelja
			StartCoroutine(DodavanjeNeprijatelja(prefabProperties));
			yield return new WaitForSeconds(0.35f);
			
			//dodavanje environment
			StartCoroutine(DodavanjeEnvironment(prefabProperties));
			yield return new WaitForSeconds(0.35f);
			
			//dodavanje novcica
			StartCoroutine(DodavanjeNovcica(prefabProperties));
			yield return new WaitForSeconds(0.35f);
			
			//dodavanje special
			StartCoroutine(DodavanjeSpecial(prefabProperties));
		}
		
	}

	public IEnumerator RepositionCoroutine()
	{
		yield return new WaitForSeconds(0.25f);
		//repozicioniranje terena
		LevelPrefabProperties prefabProperties;
		GameObject temp = null;

		if(suzenaListaTerena.Count > 0)
			suzenaListaTerena.Clear();

		for(int i=0; i<terrainPool.childCount; i++)
		{
			prefabProperties = terrainPool.GetChild(i).GetComponent<LevelPrefabProperties>();
			if(prefabProperties.slobodanTeren == 2)//if(prefabProperties.slobodanTeren)
			{
				if((System.Array.IndexOf(TereniKojiMoguDaDodju, prefabProperties.tipTerena) > -1 && prefabProperties.minimumLevel <= level && prefabProperties.maximumLevel >= level) || (TereniKojiMoguDaDodju.Length == 0 && prefabProperties.tipTerena != -10) )
				{
					suzenaListaTerena.Add(i);
				}
			}
			else if(prefabProperties.slobodanTeren == 1)
				prefabProperties.slobodanTeren = 2;
		}

		if(suzenaListaTerena.Count > 0)
		{
			int q = Random.Range(0,suzenaListaTerena.Count);
			temp = terrainPool.GetChild(suzenaListaTerena[q]).gameObject;
		}
		else
		{
			//temp = defaultTeren.gameObject;
		}
		yield return new WaitForSeconds(0.1f);

		if(!trebaFinish)
		{
			//@@@@@@@@@@@@
			brojacPrefaba++;

			temp.transform.position = GranicaDesno.position;
			//temp.transform.parent = currentLevelComponents; //bez parentovanja
			//temp.GetComponent<LevelPrefabProperties>().pooled = false;
			prefabProperties = temp.GetComponent<LevelPrefabProperties>();
			//prefabProperties.slobodanTeren = false;
			prefabProperties.slobodanTeren = 0;
			GranicaDesno = temp.transform.Find("__GranicaDesno");
			GranicaDesno.GetComponent<Collider2D>().enabled = true;
			TereniKojiMoguDaDodju = prefabProperties.moguDaSeNakace;
			yield return new WaitForSeconds(0.1f);


			//dodavanje neprijatelja
			StartCoroutine(DodavanjeNeprijatelja(prefabProperties));
			yield return new WaitForSeconds(0.35f);

			//dodavanje environment
			StartCoroutine(DodavanjeEnvironment(prefabProperties));
			yield return new WaitForSeconds(0.35f);

			//dodavanje novcica
			StartCoroutine(DodavanjeNovcica(prefabProperties));
			yield return new WaitForSeconds(0.35f);

			//dodavanje special
			StartCoroutine(DodavanjeSpecial(prefabProperties));
		}

	}

	IEnumerator DodavanjeNovcica(LevelPrefabProperties prefabPropertiess)
	{
		if(prefabPropertiess.coins_Slots_Count > 0)
		{
			if(suzenaListaObjekata.Count > 0)
				suzenaListaObjekata.Clear();
			int trenutnaVerovatnoca = Random.Range(0,100);

			for(int j = 0; j < prefabPropertiess.coinsSlots.Count; j++)
			{
				slotProperties = prefabPropertiess.coinsSlots[j].GetComponent<SlotProperties>();
				if(slotProperties.Verovatnoca >= (100 - trenutnaVerovatnoca))
				{
					//slot se koristi, odredjivanje koja odgovarajuca grupa novcica iz CoinsPool-a moze da se smesti u njega
					if(coinsPool.childCount > 0)
					{
						if(suzenaListaObjekata.Count > 0)
							suzenaListaObjekata.Clear();

						for(int i = 0; i < coinsPool.childCount; i++)
						{
							entityProperties = coinsPool.GetChild(i).GetComponent<EntityProperties>();
							if(entityProperties.slobodanEntitet)
							{
								if(entityProperties.minimumLevel <= level)
								{
									if((System.Array.IndexOf(slotProperties.availableEntities, entityProperties.Type) > -1) || slotProperties.availableEntities.Length == 0)
									{
										suzenaListaObjekata.Add(i);
									}
								}
							}
						}
						if(suzenaListaObjekata.Count > 0)
						{
							int qq = Random.Range(0,suzenaListaObjekata.Count);
							Transform coins = coinsPool.GetChild(suzenaListaObjekata[qq]); //Transform child out of bounds izbacilo
							suzenaListaObjekata.RemoveAt(qq);
							if(coins != null)
							{
								entityProperties = coins.GetComponent<EntityProperties>();
								int EntityVerovatnoca = Random.Range(0,100);
								if(EntityVerovatnoca >= (100 - entityProperties.Verovatnoca))
								{
									if(entityProperties.brojPojavljivanja < entityProperties.maxBrojPojavljivanja || entityProperties.maxBrojPojavljivanja == 0)
									{
										float z = coins.localPosition.z;
										//coins.parent = prefabPropertiess.coinsSlots[j];
										entityProperties.slobodanEntitet = false;
										entityProperties.trenutnoJeAktivan = false;
										coins.position = new Vector3(prefabPropertiess.coinsSlots[j].position.x,prefabPropertiess.coinsSlots[j].position.y,z);
										//coins.localPosition = new Vector3(0,0,z);
										entityProperties.brojPojavljivanja++;
									}
								}
							}
						}
						
					}
				}
				yield return new WaitForSeconds(0.05f);
				
			}
			
			//prefabProperties.ResetUsability_CoinsSlots();
			System.GC.Collect();
		}
	}

	IEnumerator DodavanjeEnvironment(LevelPrefabProperties prefabPropertiess)
	{
		if(prefabPropertiess.environment_Slots_Count > 0)
		{
			if(suzenaListaObjekata.Count > 0)
				suzenaListaObjekata.Clear();
			int trenutnaVerovatnoca = Random.Range(0,100);
			
			for(int j = 0; j < prefabPropertiess.environmentsSlots.Count; j++)
			{
				slotProperties = prefabPropertiess.environmentsSlots[j].GetComponent<SlotProperties>();
				if(slotProperties.Verovatnoca >= (100 - trenutnaVerovatnoca))
				{
					//slot se koristi, odredjivanje koja odgovarajuca grupa environmenta iz EnvironmentPool-a moze da se smesti u njega
					if(environmentPool.childCount > 0)
					{
						if(suzenaListaObjekata.Count > 0)
							suzenaListaObjekata.Clear();
						
						for(int i = 0; i < environmentPool.childCount; i++)
						{
							entityProperties = environmentPool.GetChild(i).GetComponent<EntityProperties>();
							if(entityProperties.slobodanEntitet)
							{
								if(entityProperties.minimumLevel <= level)
								{
									if((System.Array.IndexOf(slotProperties.availableEntities, entityProperties.Type) > -1) || slotProperties.availableEntities.Length == 0)
									{
										suzenaListaObjekata.Add(i);
									}
								}
							}
						}
						if(suzenaListaObjekata.Count > 0)
						{
							int qq = Random.Range(0,suzenaListaObjekata.Count);
							Transform environment = environmentPool.GetChild(suzenaListaObjekata[qq]);
							suzenaListaObjekata.RemoveAt(qq);
							if(environment != null)
							{
								entityProperties = environment.GetComponent<EntityProperties>();
								int EntityVerovatnoca = Random.Range(0,100);
								if(EntityVerovatnoca >= (100 - entityProperties.Verovatnoca))
								{
									if(entityProperties.DozvoljenoSkaliranje)
									{
										float scalefactor = Random.Range(entityProperties.originalScale.x,2*entityProperties.originalScale.x);
										environment.localScale = new Vector3(scalefactor,scalefactor,scalefactor);
									}
									float z = environment.localPosition.z;
									//environment.parent = prefabPropertiess.environmentsSlots[j];
									entityProperties.slobodanEntitet = false;
									entityProperties.trenutnoJeAktivan = false;
									environment.position = new Vector3(prefabPropertiess.environmentsSlots[j].position.x,prefabPropertiess.environmentsSlots[j].position.y,z);
									//environment.localPosition = new Vector3(0,0,z);

								}
							}
						}
						
					}
				}
				yield return new WaitForSeconds(0.05f);
				
			}

			System.GC.Collect();
		}
	}

	IEnumerator DodavanjeNeprijatelja(LevelPrefabProperties prefabPropertiess)
	{
		//Debug.Log("=======================================");
		//Debug.Log("TERENPREFAB: " + brojacPrefaba);

		postaviInicijalnuTezinu(prefabPropertiess);
		if(prefabPropertiess.enemies_Slots_Count > 0)
		{
			if(suzenaListaObjekata.Count > 0)
				suzenaListaObjekata.Clear();
			
			for(int j = 0; j < prefabPropertiess.enemiesSlots.Count; j++)
			{
				int trenutnaVerovatnoca = Random.Range(0,100);
				//Debug.Log("-----------------------------------");
				//Debug.Log("slot " + j + ", name: " + prefabPropertiess.enemiesSlots[j].name + ", kolicina slotova: " + prefabPropertiess.enemiesSlots.Count);
				slotProperties = prefabPropertiess.enemiesSlots[j].GetComponent<SlotProperties>();
				bool slotJeSlobodan = true;
				if(slotProperties.Verovatnoca >= (100 - trenutnaVerovatnoca))
				{
					//Debug.Log("Slot se koristi!!!!");
					//slot se koristi, odredjivanje koja odgovarajuca grupa neprijatelja iz EnemyPool-a moze da se smesti u njega
					if(enemyPool.childCount > 0)
					{
						if(suzenaListaObjekata.Count > 0)
							suzenaListaObjekata.Clear();
						
						for(int i = 0; i < enemyPool.childCount; i++)
						{
							entityProperties = enemyPool.GetChild(i).GetComponent<EntityProperties>();
							if((entityProperties.Type == 3 && leteciBabuni == 1) || (entityProperties.Type == 7 && boomerangBabuni == 1) || (entityProperties.Type == 10 && leteceGorile == 1) || (entityProperties.Type == 14 && kopljeGorile == 1))
							{
								//don't summon
								//Debug.Log("JESAM LI MNOGO USAO OVDE");
							}

							else
							{
								//Debug.Log("properti da moze: " + entityProperties.Type + ", brojac: " + brojacPrefaba + ", leteciBabuniKvota: " + ((int)leteciBabuni_Kvota) + ", korigovancija: " + leteciBabuni);
								if(brojacPrefaba == (int)leteciBabuni_Kvota && brojacPrefaba <= 7 && leteciBabuni == 0) //obezbediti da se leteci babun obavezno pojavi na slotu
								{
									//Debug.Log("OPAAAAAA 1");
									if(entityProperties.Type == 3)
									{
										//Debug.Log("OPAAAAAA 2");
										if((System.Array.IndexOf(slotProperties.availableEntities, entityProperties.Type) > -1) || slotProperties.availableEntities.Length == 0)
										{
											//Debug.Log("OPAAAAAA 3");
											if(entityProperties.slobodanEntitet)
											{
												//Debug.Log("OPAAAAAA 4");
												leteciBabuni_postoji_u_poolu = 1;
												//Debug.Log("## TERENPREFAB: " + brojacPrefaba + " LETECI BABUN OBAVEZNO");
												KorigujVerovatnocuZbogMisije(2,1);
												leteciBabuni_Kvota += leteciBabuni_Kvota_locked;
												Transform enemy = enemyPool.GetChild(i);
												float z = enemy.localPosition.z;
												entityProperties.slobodanEntitet = false;
												entityProperties.trenutnoJeAktivan = false;
												if(entityProperties.Type != 15 || entityProperties.Type != 16 || entityProperties.Type != 17 || entityProperties.Type != 18)
												{
													for(int ii=0; ii<enemy.childCount;ii++)
														enemy.GetChild(ii).Find("BaboonReal").Find("_MajmunceNadrlja").GetComponent<BabunDogadjaji_new>().ResetujBabuna();
												}
												enemy.position = new Vector3(prefabPropertiess.enemiesSlots[j].position.x,prefabPropertiess.enemiesSlots[j].position.y,z);
												//enemy.localScale = enemy.localScale*10;
												entityProperties.brojPojavljivanja++;
												slotJeSlobodan = false;
												postavljen_u_prefabu_leteciBabuni = 1;
											}
											else
											{
												//Debug.Log("OPAAAAAA 5");
												leteciBabuni_postoji_u_poolu = 2;
												postavljen_u_prefabu_leteciBabuni = 2;
											}
										}
										else
										{
											postavljen_u_prefabu_leteciBabuni = 2;
										}
									}
								}
								//Debug.Log("properti da moze: " + entityProperties.Type + ", brojac: " + brojacPrefaba + ", leteceGorileKvota: " + ((int)leteceGorile_Kvota) + ", korigovancija: " + leteceGorile);
								if(brojacPrefaba == (int)leteceGorile_Kvota && brojacPrefaba <= 7 && leteceGorile == 0) //obezbediti da se leteci gorila obavezno pojavi na slotu
								{
									//Debug.Log("2OPAAAAAA 1");
									if(entityProperties.Type == 10 && slotJeSlobodan)
									{
										//Debug.Log("2OPAAAAAA 2");
										if((System.Array.IndexOf(slotProperties.availableEntities, entityProperties.Type) > -1) || slotProperties.availableEntities.Length == 0)
										{
											//Debug.Log("2OPAAAAAA 3");
											if(entityProperties.slobodanEntitet)
											{
												//Debug.Log("2OPAAAAAA 4");
												leteceGorile_postoji_u_poolu = 1;
												//Debug.Log("## TERENPREFAB: " + brojacPrefaba + " LETECI GORILA OBAVEZNO");
												KorigujVerovatnocuZbogMisije(2,2);
												leteceGorile_Kvota += leteceGorile_Kvota_locked;
												Transform enemy = enemyPool.GetChild(i);
												float z = enemy.localPosition.z;
												entityProperties.slobodanEntitet = false;
												entityProperties.trenutnoJeAktivan = false;
												if(entityProperties.Type != 15 || entityProperties.Type != 16 || entityProperties.Type != 17 || entityProperties.Type != 18)
												{
													for(int ii=0; ii<enemy.childCount;ii++)
														enemy.GetChild(ii).Find("BaboonReal").Find("_MajmunceNadrlja").GetComponent<BabunDogadjaji_new>().ResetujBabuna();
												}
												enemy.position = new Vector3(prefabPropertiess.enemiesSlots[j].position.x,prefabPropertiess.enemiesSlots[j].position.y,z);
												//enemy.localScale = enemy.localScale*10;
												entityProperties.brojPojavljivanja++;
												slotJeSlobodan = false;
												postavljen_u_prefabu_leteceGorile = 1;
											}
											else
											{
												//Debug.Log("2OPAAAAAA 5");
												leteceGorile_postoji_u_poolu = 2;
												postavljen_u_prefabu_leteceGorile = 2;
											}
										}
										else
										{
											postavljen_u_prefabu_leteceGorile = 2;
										}
									}
									else //situacija kada ne postoji u poolu uopste neprijatelj tog tipa, treba da se proveri na kojim slotovima moze da se stavi neprijatelj tog tipa i da se na poslednjem 
									{	 //takvom slotu stavi da treba da se instancira

										//zasad bez ovoga, u enemyPool-u bi trebalo da postoji makar po jedan od neprijatelja koji ucestvuju u misijama
									}
								}
								//Debug.Log("properti da moze: " + entityProperties.Type + ", brojac: " + brojacPrefaba + ", boomerangBabuniKvota: " + ((int)boomerangBabuni_Kvota) + ", korigovancija: " + boomerangBabuni);
								if(brojacPrefaba == (int)boomerangBabuni_Kvota && brojacPrefaba <= 7 && boomerangBabuni == 0) //obezbediti da se boomerang babun obavezno pojavi na slotu
								{
									//Debug.Log("3OPAAAAAA 1");
									if(entityProperties.Type == 7 && slotJeSlobodan)
									{
										//Debug.Log("3OPAAAAAA 2");
										if((System.Array.IndexOf(slotProperties.availableEntities, entityProperties.Type) > -1) || slotProperties.availableEntities.Length == 0)
										{
											//Debug.Log("3OPAAAAAA 3");
											if(entityProperties.slobodanEntitet)
											{
												//Debug.Log("3OPAAAAAA 4");
												boomerangBabuni_postoji_u_poolu = 1;
												//Debug.Log("## TERENPREFAB: " + brojacPrefaba + " BOOMERANG BABUN OBAVEZNO");
												KorigujVerovatnocuZbogMisije(2,3);
												boomerangBabuni_Kvota += boomerangBabuni_Kvota_locked;
												Transform enemy = enemyPool.GetChild(i);
												float z = enemy.localPosition.z;
												entityProperties.slobodanEntitet = false;
												entityProperties.trenutnoJeAktivan = false;
												if(entityProperties.Type != 15 || entityProperties.Type != 16 || entityProperties.Type != 17 || entityProperties.Type != 18)
												{
													for(int ii=0; ii<enemy.childCount;ii++)
														enemy.GetChild(ii).Find("BaboonReal").Find("_MajmunceNadrlja").GetComponent<BabunDogadjaji_new>().ResetujBabuna();
												}
												enemy.position = new Vector3(prefabPropertiess.enemiesSlots[j].position.x,prefabPropertiess.enemiesSlots[j].position.y,z);
												//enemy.localScale = enemy.localScale*10;
												entityProperties.brojPojavljivanja++;
												slotJeSlobodan = false;
												postavljen_u_prefabu_boomerangBabuni = 1;
											}
											else
											{
												//Debug.Log("3OPAAAAAA 5");
												boomerangBabuni_postoji_u_poolu = 2;
												postavljen_u_prefabu_boomerangBabuni = 2;
											}
										}
										else
										{
											postavljen_u_prefabu_boomerangBabuni = 2;
										}
									}
								}
								//Debug.Log("properti da moze: " + entityProperties.Type + ", brojac: " + brojacPrefaba + ", kopljeGorileKvota: " + ((int)kopljeGorile_Kvota) + ", korigovancija: " + kopljeGorile);
								if(brojacPrefaba == (int)kopljeGorile_Kvota && brojacPrefaba <= 7 && kopljeGorile == 0) //obezbediti da se koplje gorila obavezno pojavi na slotu
								{
									//Debug.Log("4OPAAAAAA 1");
									if(entityProperties.Type == 14 && slotJeSlobodan)
									{
										//Debug.Log("4OPAAAAAA 2");
										if((System.Array.IndexOf(slotProperties.availableEntities, entityProperties.Type) > -1) || slotProperties.availableEntities.Length == 0)
										{
											//Debug.Log("4OPAAAAAA 3");
											if(entityProperties.slobodanEntitet)
											{
												//Debug.Log("4OPAAAAAA 4");
												kopljeGorile_postoji_u_poolu = 1;
												//Debug.Log("## TERENPREFAB: " + brojacPrefaba + " KOPLJE GORILA OBAVEZNO");
												KorigujVerovatnocuZbogMisije(2,4);
												kopljeGorile_Kvota += kopljeGorile_Kvota_locked;
												Transform enemy = enemyPool.GetChild(i);
												float z = enemy.localPosition.z;
												entityProperties.slobodanEntitet = false;
												entityProperties.trenutnoJeAktivan = false;
												if(entityProperties.Type != 15 || entityProperties.Type != 16 || entityProperties.Type != 17 || entityProperties.Type != 18)
												{
													for(int ii=0; ii<enemy.childCount;ii++)
													enemy.GetChild(ii).Find("BaboonReal").Find("_MajmunceNadrlja").GetComponent<BabunDogadjaji_new>().ResetujBabuna();
												}
												enemy.position = new Vector3(prefabPropertiess.enemiesSlots[j].position.x,prefabPropertiess.enemiesSlots[j].position.y,z);
												//enemy.localScale = enemy.localScale*10;
												entityProperties.brojPojavljivanja++;
												slotJeSlobodan = false;
												postavljen_u_prefabu_kopljeGorile = 1;
											}
											else
											{
												//Debug.Log("4OPAAAAAA 5");
												kopljeGorile_postoji_u_poolu = 2;
												postavljen_u_prefabu_kopljeGorile = 2;
											}
										}
										else
										{
											postavljen_u_prefabu_kopljeGorile = 2;
										}
									}
								}
								if(entityProperties.slobodanEntitet && slotJeSlobodan)
								{
									if(entityProperties.minimumLevel <= level)
									{
										if((System.Array.IndexOf(slotProperties.availableEntities, entityProperties.Type) > -1) || slotProperties.availableEntities.Length == 0)
										{
											suzenaListaObjekata.Add(i);
										}
									}
								}
							}
							
						}
						if(leteciBabuni_postoji_u_poolu == 2)
						{
							//Debug.Log("INSTANCIRAM LETECEG BABUNA, KORAK 1 ^^^^");
							if(brojacPrefaba == (int)leteciBabuni_Kvota && brojacPrefaba <= 7 && leteciBabuni == 0)
							{
								//Debug.Log("INSTANCIRAM LETECEG BABUNA, KORAK 2 ^^^^");
								if(slotJeSlobodan)
								{
									//Debug.Log("## TERENPREFAB: " + brojacPrefaba + " LETECI BABUN OBAVEZNO");
									KorigujVerovatnocuZbogMisije(2,1);
									leteciBabuni_Kvota += leteciBabuni_Kvota_locked;
									GameObject a = Instantiate(enemiesForInstantiate[2],new Vector3(prefabPropertiess.enemiesSlots[j].position.x,prefabPropertiess.enemiesSlots[j].position.y,prefabPropertiess.enemiesSlots[j].position.z),Quaternion.identity) as GameObject;
									for(int ii=0; ii<a.transform.childCount;ii++)
										a.transform.GetChild(ii).Find("BaboonReal").Find("_MajmunceNadrlja").GetComponent<BabunDogadjaji_new>().ResetujBabuna();
									a.transform.parent = enemyPool;
									entityProperties = a.GetComponent<EntityProperties>();
									entityProperties.instanciran = true;
									entityProperties.slobodanEntitet = false;
									entityProperties.trenutnoJeAktivan = false;
									//a.transform.localScale = a.transform.localScale*15;
									slotJeSlobodan = false;
									leteciBabuni_postoji_u_poolu = 0;
									postavljen_u_prefabu_leteciBabuni = 1;
								}
							}
						}
						if(leteceGorile_postoji_u_poolu == 2)
						{
							//Debug.Log("INSTANCIRAM LETECEG GORILU, KORAK 1 ^^^^");
							if(brojacPrefaba == (int)leteceGorile_Kvota && brojacPrefaba <= 7 && leteceGorile == 0)
							{
								//Debug.Log("INSTANCIRAM LETECEG GORILU, KORAK 1 ^^^^");
								if(slotJeSlobodan)
								{
									//Debug.Log("## TERENPREFAB: " + brojacPrefaba + " LETECI GORILA OBAVEZNO");
									KorigujVerovatnocuZbogMisije(2,2);
									leteceGorile_Kvota += leteceGorile_Kvota_locked;
									GameObject a = Instantiate(enemiesForInstantiate[9],new Vector3(prefabPropertiess.enemiesSlots[j].position.x,prefabPropertiess.enemiesSlots[j].position.y,prefabPropertiess.enemiesSlots[j].position.z),Quaternion.identity) as GameObject;
									for(int ii=0; ii<a.transform.childCount;ii++)
										a.transform.GetChild(ii).Find("BaboonReal").Find("_MajmunceNadrlja").GetComponent<BabunDogadjaji_new>().ResetujBabuna();
									a.transform.parent = enemyPool;
									entityProperties = a.GetComponent<EntityProperties>();
									entityProperties.instanciran = true;
									entityProperties.slobodanEntitet = false;
									entityProperties.trenutnoJeAktivan = false;
									//a.transform.localScale = a.transform.localScale*15;
									slotJeSlobodan = false;
									leteceGorile_postoji_u_poolu = 0;
									postavljen_u_prefabu_leteceGorile = 1;
								}
							}
						}
						if(boomerangBabuni_postoji_u_poolu == 2)
						{
							//Debug.Log("INSTANCIRAM BOOMERANG BABUNA, KORAK 1 ^^^^");
							if(brojacPrefaba == (int)boomerangBabuni_Kvota && brojacPrefaba <= 7 && boomerangBabuni == 0)
							{
								//Debug.Log("INSTANCIRAM BOOMERANG BABUNA, KORAK 2 ^^^^");
								if(slotJeSlobodan)
								{
									//Debug.Log("## TERENPREFAB: " + brojacPrefaba + " BOOMERANG BABUN OBAVEZNO");
									KorigujVerovatnocuZbogMisije(2,3);
									boomerangBabuni_Kvota += boomerangBabuni_Kvota_locked;
									GameObject a = Instantiate(enemiesForInstantiate[6],new Vector3(prefabPropertiess.enemiesSlots[j].position.x,prefabPropertiess.enemiesSlots[j].position.y,prefabPropertiess.enemiesSlots[j].position.z),Quaternion.identity) as GameObject;
									for(int ii=0; ii<a.transform.childCount;ii++)
										a.transform.GetChild(ii).Find("BaboonReal").Find("_MajmunceNadrlja").GetComponent<BabunDogadjaji_new>().ResetujBabuna();
									a.transform.parent = enemyPool;
									entityProperties = a.GetComponent<EntityProperties>();
									entityProperties.instanciran = true;
									entityProperties.slobodanEntitet = false;
									entityProperties.trenutnoJeAktivan = false;
									//a.transform.localScale = a.transform.localScale*15;
									slotJeSlobodan = false;
									boomerangBabuni_postoji_u_poolu = 0;
									postavljen_u_prefabu_boomerangBabuni = 1;
								}
							}
						}
						if(kopljeGorile_postoji_u_poolu == 2)
						{
							//Debug.Log("INSTANCIRAM KOPLJE GORILU, KORAK 1 ^^^^");
							if(brojacPrefaba == (int)kopljeGorile_Kvota && brojacPrefaba <= 7 && kopljeGorile == 0)
							{
								//Debug.Log("INSTANCIRAM KOPLJE GORILU, KORAK 2 ^^^^");
								if(slotJeSlobodan)
								{
									//Debug.Log("## TERENPREFAB: " + brojacPrefaba + " KOPLJE GORILA OBAVEZNO");
									KorigujVerovatnocuZbogMisije(2,4);
									kopljeGorile_Kvota += kopljeGorile_Kvota_locked;
									GameObject a = Instantiate(enemiesForInstantiate[13],new Vector3(prefabPropertiess.enemiesSlots[j].position.x,prefabPropertiess.enemiesSlots[j].position.y,prefabPropertiess.enemiesSlots[j].position.z),Quaternion.identity) as GameObject;
									for(int ii=0; ii<a.transform.childCount;ii++)
										a.transform.GetChild(ii).Find("BaboonReal").Find("_MajmunceNadrlja").GetComponent<BabunDogadjaji_new>().ResetujBabuna();
									a.transform.parent = enemyPool;
									entityProperties = a.GetComponent<EntityProperties>();
									entityProperties.instanciran = true;
									entityProperties.slobodanEntitet = false;
									entityProperties.trenutnoJeAktivan = false;
									//a.transform.localScale = a.transform.localScale*15;
									slotJeSlobodan = false;
									kopljeGorile_postoji_u_poolu = 0;
									postavljen_u_prefabu_kopljeGorile = 1;
								}
							}
						}

						if(suzenaListaObjekata.Count > 0 && slotJeSlobodan)
						{
							int qq = Random.Range(0,suzenaListaObjekata.Count);
							Transform enemy = enemyPool.GetChild(suzenaListaObjekata[qq]);
							suzenaListaObjekata.RemoveAt(qq);
							if(enemy != null)
							{
								entityProperties = enemy.GetComponent<EntityProperties>();
								int EntityVerovatnoca = Random.Range(0,100);
								if(EntityVerovatnoca >= (100 - entityProperties.Verovatnoca))
								{
									//if(entityProperties.Type == 1 || entityProperties.Type == 3 || entityProperties.Type == 4 || entityProperties.Type == 5 || entityProperties.Type == 6)
									if(entityProperties.Type != 15 && entityProperties.Type != 16 && entityProperties.Type != 17 && entityProperties.Type != 18)
									{
										for(int i=0; i<enemy.childCount;i++)
										{
											//Debug.Log("ENEMICHAJLD OD I: " + enemy.GetChild(i).name + ", ENTITI PROPERTIS: " + entityProperties.Type);
											enemy.GetChild(i).Find("BaboonReal").Find("_MajmunceNadrlja").GetComponent<BabunDogadjaji_new>().ResetujBabuna();
										}
									}
									float z = enemy.localPosition.z;
									//enemy.parent = prefabPropertiess.enemiesSlots[j];
									entityProperties.slobodanEntitet = false;
									entityProperties.trenutnoJeAktivan = false;
									enemy.position = new Vector3(prefabPropertiess.enemiesSlots[j].position.x, prefabPropertiess.enemiesSlots[j].position.y, z);
									//enemy.localPosition = new Vector3(0,0,z);
								}
							}
						}
						else if(suzenaListaObjekata.Count == 0 && slotJeSlobodan) //slot se koristi, ali nema dovoljno neprijatelja u enemyPool-u, svi koji postoje se vec koriste, treba da se instancira neprijatelj
						{
							int qq = Random.Range(0,slotProperties.availableEntities.Length);
							int enemyType = slotProperties.availableEntities[qq];
							GameObject a = Instantiate(enemiesForInstantiate[enemyType-1],new Vector3(prefabPropertiess.enemiesSlots[j].position.x,prefabPropertiess.enemiesSlots[j].position.y,prefabPropertiess.enemiesSlots[j].position.z),Quaternion.identity) as GameObject;
							entityProperties = a.GetComponent<EntityProperties>();
							a.transform.parent = enemyPool;
							entityProperties.instanciran = true;
							entityProperties.slobodanEntitet = false;
							entityProperties.trenutnoJeAktivan = false;
							if(entityProperties.Type != 15 && entityProperties.Type != 16 && entityProperties.Type != 17 && entityProperties.Type != 18)
								for(int ii=0; ii<a.transform.childCount;ii++)
									a.transform.GetChild(ii).Find("BaboonReal").Find("_MajmunceNadrlja").GetComponent<BabunDogadjaji_new>().ResetujBabuna();
							//a.transform.localScale = a.transform.localScale*15;
							slotJeSlobodan = false;
						}
						
					}
				}
				//@@@@@@@
				else
				{
					//Debug.Log("Slot se NE koRIsTI");
					//slucaj kada se slot ne koristi, a dotad nije postavljen enemy koji treba za misiju, da se ipak ukljuci slot i da se ubaci enemy, jer moze da se desi da se i ostali slotovi ne koriste i da 
					//ne podrzavaju tip enemy-ja koji je potreban za misiju, pa ce taj terenPrefab ostati bez tog enemy-ja iako bi on trebalo da postoji na njemu
					if(leteciBabuni == 0)
					{
						if(enemyPool.childCount > 0)
						{
							if(suzenaListaObjekata.Count > 0)
								suzenaListaObjekata.Clear();
				
							for(int i = 0; i < enemyPool.childCount; i++)
							{
								entityProperties = enemyPool.GetChild(i).GetComponent<EntityProperties>();

								//Debug.Log("properti da moze: " + entityProperties.Type + ", brojac: " + brojacPrefaba + ", leteciBabuniKvota: " + ((int)leteciBabuni_Kvota) + ", korigovancija: " + leteciBabuni);
								if(brojacPrefaba == (int)leteciBabuni_Kvota && brojacPrefaba <= 7) //obezbediti da se leteci babun obavezno pojavi na slotu
								{
									//Debug.Log("zOPAAAAAA 1");
									if(entityProperties.Type == 3)
									{
										//Debug.Log("zOPAAAAAA 2");
										if((System.Array.IndexOf(slotProperties.availableEntities, entityProperties.Type) > -1) || slotProperties.availableEntities.Length == 0)
										{
											//Debug.Log("zOPAAAAAA 3");
											if(entityProperties.slobodanEntitet)
											{
												//Debug.Log("zOPAAAAAA 4");
												leteciBabuni_postoji_u_poolu = 1;
												//Debug.Log("## TERENPREFAB: " + brojacPrefaba + " LETECI BABUN OBAVEZNO");
												KorigujVerovatnocuZbogMisije(2,1);
												leteciBabuni_Kvota += leteciBabuni_Kvota_locked;
												Transform enemy = enemyPool.GetChild(i);
												float z = enemy.localPosition.z;
												entityProperties.slobodanEntitet = false;
												entityProperties.trenutnoJeAktivan = false;
												if(entityProperties.Type != 15 || entityProperties.Type != 16 || entityProperties.Type != 17 || entityProperties.Type != 18)
												{
													for(int ii=0; ii<enemy.childCount;ii++)
														enemy.GetChild(ii).Find("BaboonReal").Find("_MajmunceNadrlja").GetComponent<BabunDogadjaji_new>().ResetujBabuna();
												}
												enemy.position = new Vector3(prefabPropertiess.enemiesSlots[j].position.x,prefabPropertiess.enemiesSlots[j].position.y,z);
												//enemy.localScale = enemy.localScale*10;
												entityProperties.brojPojavljivanja++;
												slotJeSlobodan = false;
												postavljen_u_prefabu_leteciBabuni = 1;
											}
											else
											{
												//Debug.Log("zOPAAAAAA 5");
												leteciBabuni_postoji_u_poolu = 2;
												postavljen_u_prefabu_leteciBabuni = 2;
											}
										}
										else
										{
											postavljen_u_prefabu_leteciBabuni = 2;
										}
									}
								}
							}
							if(leteciBabuni_postoji_u_poolu == 2)
							{
								//Debug.Log("z INSTANCIRAM LETECEG BABUNA, KORAK 1 ^^^^");
								if(brojacPrefaba == (int)leteciBabuni_Kvota && brojacPrefaba <= 7)
								{
									//Debug.Log("z INSTANCIRAM LETECEG BABUNA, KORAK 2 ^^^^");
									if(slotJeSlobodan)
									{
										//Debug.Log("## TERENPREFAB: " + brojacPrefaba + " LETECI BABUN OBAVEZNO");
										KorigujVerovatnocuZbogMisije(2,1);
										leteciBabuni_Kvota += leteciBabuni_Kvota_locked;
										GameObject a = Instantiate(enemiesForInstantiate[2],new Vector3(prefabPropertiess.enemiesSlots[j].position.x,prefabPropertiess.enemiesSlots[j].position.y,prefabPropertiess.enemiesSlots[j].position.z),Quaternion.identity) as GameObject;
										for(int ii=0; ii<a.transform.childCount;ii++)
											a.transform.GetChild(ii).Find("BaboonReal").Find("_MajmunceNadrlja").GetComponent<BabunDogadjaji_new>().ResetujBabuna();
										a.transform.parent = enemyPool;
										entityProperties = a.GetComponent<EntityProperties>();
										entityProperties.instanciran = true;
										entityProperties.slobodanEntitet = false;
										entityProperties.trenutnoJeAktivan = false;
										//a.transform.localScale = a.transform.localScale*15;
										slotJeSlobodan = false;
										leteciBabuni_postoji_u_poolu = 0;
										postavljen_u_prefabu_leteciBabuni = 1;
									}
								}
							}
						}
					}
					if(leteceGorile == 0)
					{
						if(enemyPool.childCount > 0)
						{
							if(suzenaListaObjekata.Count > 0)
								suzenaListaObjekata.Clear();
							
							for(int i = 0; i < enemyPool.childCount; i++)
							{
								entityProperties = enemyPool.GetChild(i).GetComponent<EntityProperties>();

								//Debug.Log("properti da moze: " + entityProperties.Type + ", brojac: " + brojacPrefaba + ", leteceGorileKvota: " + ((int)leteceGorile_Kvota) + ", korigovancija: " + leteceGorile);
								if(brojacPrefaba == (int)leteceGorile_Kvota && brojacPrefaba <= 7) //obezbediti da se leteci gorila obavezno pojavi na slotu
								{
									//Debug.Log("zzOPAAAAAA 1");
									if(entityProperties.Type == 10 && slotJeSlobodan)
									{
										//Debug.Log("zzOPAAAAAA 2");
										if((System.Array.IndexOf(slotProperties.availableEntities, entityProperties.Type) > -1) || slotProperties.availableEntities.Length == 0)
										{
											//Debug.Log("zzOPAAAAAA 3");
											if(entityProperties.slobodanEntitet)
											{
												//Debug.Log("zzOPAAAAAA 4");
												leteceGorile_postoji_u_poolu = 1;
												//Debug.Log("## TERENPREFAB: " + brojacPrefaba + " LETECI GORILA OBAVEZNO");
												KorigujVerovatnocuZbogMisije(2,2);
												leteceGorile_Kvota += leteceGorile_Kvota_locked;
												Transform enemy = enemyPool.GetChild(i);
												float z = enemy.localPosition.z;
												entityProperties.slobodanEntitet = false;
												entityProperties.trenutnoJeAktivan = false;
												if(entityProperties.Type != 15 || entityProperties.Type != 16 || entityProperties.Type != 17 || entityProperties.Type != 18)
												{
													for(int ii=0; ii<enemy.childCount;ii++)
														enemy.GetChild(ii).Find("BaboonReal").Find("_MajmunceNadrlja").GetComponent<BabunDogadjaji_new>().ResetujBabuna();
												}
												enemy.position = new Vector3(prefabPropertiess.enemiesSlots[j].position.x,prefabPropertiess.enemiesSlots[j].position.y,z);
												//enemy.localScale = enemy.localScale*10;
												entityProperties.brojPojavljivanja++;
												slotJeSlobodan = false;
												postavljen_u_prefabu_leteceGorile = 1;
											}
											else
											{
												//Debug.Log("zzOPAAAAAA 5");
												leteceGorile_postoji_u_poolu = 2;
												postavljen_u_prefabu_leteceGorile = 2;
											}
										}
										else
										{
											postavljen_u_prefabu_leteceGorile = 2;
										}
									}
								}
							}
							if(leteceGorile_postoji_u_poolu == 2)
							{
								//Debug.Log("z INSTANCIRAM LETECEG GORILU, KORAK 1 ^^^^");
								if(brojacPrefaba == (int)leteceGorile_Kvota && brojacPrefaba <= 7)
								{
									//Debug.Log("z INSTANCIRAM LETECEG GORILU, KORAK 2 ^^^^");
									if(slotJeSlobodan)
									{
										//Debug.Log("## TERENPREFAB: " + brojacPrefaba + " LETECI GORILA OBAVEZNO");
										KorigujVerovatnocuZbogMisije(2,2);
										leteceGorile_Kvota += leteceGorile_Kvota_locked;
										GameObject a = Instantiate(enemiesForInstantiate[9],new Vector3(prefabPropertiess.enemiesSlots[j].position.x,prefabPropertiess.enemiesSlots[j].position.y,prefabPropertiess.enemiesSlots[j].position.z),Quaternion.identity) as GameObject;
										for(int ii=0; ii<a.transform.childCount;ii++)
											a.transform.GetChild(ii).Find("BaboonReal").Find("_MajmunceNadrlja").GetComponent<BabunDogadjaji_new>().ResetujBabuna();
										a.transform.parent = enemyPool;
										entityProperties = a.GetComponent<EntityProperties>();
										entityProperties.instanciran = true;
										entityProperties.slobodanEntitet = false;
										entityProperties.trenutnoJeAktivan = false;
										//a.transform.localScale = a.transform.localScale*15;
										slotJeSlobodan = false;
										leteceGorile_postoji_u_poolu = 0;
										postavljen_u_prefabu_leteceGorile = 1;
									}
								}
							}
						}
					}
					if(boomerangBabuni == 0)
					{
						if(enemyPool.childCount > 0)
						{
							if(suzenaListaObjekata.Count > 0)
								suzenaListaObjekata.Clear();
							
							for(int i = 0; i < enemyPool.childCount; i++)
							{
								entityProperties = enemyPool.GetChild(i).GetComponent<EntityProperties>();

								//Debug.Log("properti da moze: " + entityProperties.Type + ", brojac: " + brojacPrefaba + ", boomerangBabuniKvota: " + ((int)boomerangBabuni_Kvota) + ", korigovancija: " + boomerangBabuni);
								if(brojacPrefaba == (int)boomerangBabuni_Kvota && brojacPrefaba <= 7) //obezbediti da se boomerang babun obavezno pojavi na slotu
								{
									//Debug.Log("zzzOPAAAAAA 1");
									if(entityProperties.Type == 7 && slotJeSlobodan)
									{
										//Debug.Log("zzzOPAAAAAA 2");
										if((System.Array.IndexOf(slotProperties.availableEntities, entityProperties.Type) > -1) || slotProperties.availableEntities.Length == 0)
										{
											//Debug.Log("zzzOPAAAAAA 3");
											if(entityProperties.slobodanEntitet)
											{
												//Debug.Log("zzzOPAAAAAA 4");
												boomerangBabuni_postoji_u_poolu = 1;
												//Debug.Log("## TERENPREFAB: " + brojacPrefaba + " BOOMERANG BABUN OBAVEZNO");
												KorigujVerovatnocuZbogMisije(2,3);
												boomerangBabuni_Kvota += boomerangBabuni_Kvota_locked;
												Transform enemy = enemyPool.GetChild(i);
												float z = enemy.localPosition.z;
												entityProperties.slobodanEntitet = false;
												entityProperties.trenutnoJeAktivan = false;
												if(entityProperties.Type != 15 || entityProperties.Type != 16 || entityProperties.Type != 17 || entityProperties.Type != 18)
												{
													for(int ii=0; ii<enemy.childCount;ii++)
														enemy.GetChild(ii).Find("BaboonReal").Find("_MajmunceNadrlja").GetComponent<BabunDogadjaji_new>().ResetujBabuna();
												}
												enemy.position = new Vector3(prefabPropertiess.enemiesSlots[j].position.x,prefabPropertiess.enemiesSlots[j].position.y,z);
												//enemy.localScale = enemy.localScale*10;
												entityProperties.brojPojavljivanja++;
												slotJeSlobodan = false;
												postavljen_u_prefabu_boomerangBabuni = 1;
											}
											else
											{
												//Debug.Log("zzzOPAAAAAA 5");
												boomerangBabuni_postoji_u_poolu = 2;
												postavljen_u_prefabu_boomerangBabuni = 2;
											}
										}
										else
										{
											postavljen_u_prefabu_boomerangBabuni = 2;
										}
									}
								}
							}
							if(boomerangBabuni_postoji_u_poolu == 2)
							{
								//Debug.Log("z INSTANCIRAM BOOMERANG BABUNA, KORAK 1 ^^^^");
								if(brojacPrefaba == (int)boomerangBabuni_Kvota && brojacPrefaba <= 7)
								{
									//Debug.Log("z INSTANCIRAM BOOMERANG BABUNA, KORAK 2 ^^^^");
									if(slotJeSlobodan)
									{
										//Debug.Log("## TERENPREFAB: " + brojacPrefaba + " BOOMERANG BABUN OBAVEZNO");
										KorigujVerovatnocuZbogMisije(2,3);
										boomerangBabuni_Kvota += boomerangBabuni_Kvota_locked;
										GameObject a = Instantiate(enemiesForInstantiate[6],new Vector3(prefabPropertiess.enemiesSlots[j].position.x,prefabPropertiess.enemiesSlots[j].position.y,prefabPropertiess.enemiesSlots[j].position.z),Quaternion.identity) as GameObject;
										for(int ii=0; ii<a.transform.childCount;ii++)
											a.transform.GetChild(ii).Find("BaboonReal").Find("_MajmunceNadrlja").GetComponent<BabunDogadjaji_new>().ResetujBabuna();
										a.transform.parent = enemyPool;
										entityProperties = a.GetComponent<EntityProperties>();
										entityProperties.instanciran = true;
										entityProperties.slobodanEntitet = false;
										entityProperties.trenutnoJeAktivan = false;
										//a.transform.localScale = a.transform.localScale*15;
										slotJeSlobodan = false;
										boomerangBabuni_postoji_u_poolu = 0;
										postavljen_u_prefabu_boomerangBabuni = 1;
									}
								}
							}
						}
					}
					if(kopljeGorile == 0)
					{
						if(enemyPool.childCount > 0)
						{
							if(suzenaListaObjekata.Count > 0)
								suzenaListaObjekata.Clear();
							
							for(int i = 0; i < enemyPool.childCount; i++)
							{
								entityProperties = enemyPool.GetChild(i).GetComponent<EntityProperties>();

								//Debug.Log("properti da moze: " + entityProperties.Type + ", brojac: " + brojacPrefaba + ", kopljeGorileKvota: " + ((int)kopljeGorile_Kvota) + ", korigovancija: " + kopljeGorile);
								if(brojacPrefaba == (int)kopljeGorile_Kvota && brojacPrefaba <= 7) //obezbediti da se koplje gorila obavezno pojavi na slotu
								{
									//Debug.Log("zzzzOPAAAAAA 1");
									if(entityProperties.Type == 14 && slotJeSlobodan)
									{
										//Debug.Log("zzzzOPAAAAAA 2");
										if((System.Array.IndexOf(slotProperties.availableEntities, entityProperties.Type) > -1) || slotProperties.availableEntities.Length == 0)
										{
											//Debug.Log("zzzzOPAAAAAA 3");
											if(entityProperties.slobodanEntitet)
											{
												//Debug.Log("zzzzOPAAAAAA 4");
												kopljeGorile_postoji_u_poolu = 1;
												//Debug.Log("## TERENPREFAB: " + brojacPrefaba + " KOPLJE GORILA OBAVEZNO");
												KorigujVerovatnocuZbogMisije(2,4);
												kopljeGorile_Kvota += kopljeGorile_Kvota_locked;
												Transform enemy = enemyPool.GetChild(i);
												float z = enemy.localPosition.z;
												entityProperties.slobodanEntitet = false;
												entityProperties.trenutnoJeAktivan = false;
												if(entityProperties.Type != 15 || entityProperties.Type != 16 || entityProperties.Type != 17 || entityProperties.Type != 18)
												{
													for(int ii=0; ii<enemy.childCount;ii++)
													enemy.GetChild(ii).Find("BaboonReal").Find("_MajmunceNadrlja").GetComponent<BabunDogadjaji_new>().ResetujBabuna();
												}
												enemy.position = new Vector3(prefabPropertiess.enemiesSlots[j].position.x,prefabPropertiess.enemiesSlots[j].position.y,z);
												//enemy.localScale = enemy.localScale*10;
												entityProperties.brojPojavljivanja++;
												slotJeSlobodan = false;
												postavljen_u_prefabu_kopljeGorile = 1;
											}
											else
											{
												//Debug.Log("zzzzOPAAAAAA 5");
												kopljeGorile_postoji_u_poolu = 2;
												postavljen_u_prefabu_kopljeGorile = 2;
											}
										}
										else
										{
											postavljen_u_prefabu_kopljeGorile = 2;
										}
									}
								}
							}
							if(kopljeGorile_postoji_u_poolu == 2)
							{
								//Debug.Log("z INSTANCIRAM KOPLJE GORILU, KORAK 1 ^^^^");
								if(brojacPrefaba == (int)kopljeGorile_Kvota && brojacPrefaba <= 7)
								{
									//Debug.Log("z INSTANCIRAM KOPLJE GORILU, KORAK 2 ^^^^");
									if(slotJeSlobodan)
									{
										//Debug.Log("## TERENPREFAB: " + brojacPrefaba + " KOPLJE GORILA OBAVEZNO");
										KorigujVerovatnocuZbogMisije(2,4);
										kopljeGorile_Kvota += kopljeGorile_Kvota_locked;
										GameObject a = Instantiate(enemiesForInstantiate[13],new Vector3(prefabPropertiess.enemiesSlots[j].position.x,prefabPropertiess.enemiesSlots[j].position.y,prefabPropertiess.enemiesSlots[j].position.z),Quaternion.identity) as GameObject;
										for(int ii=0; ii<a.transform.childCount;ii++)
											a.transform.GetChild(ii).Find("BaboonReal").Find("_MajmunceNadrlja").GetComponent<BabunDogadjaji_new>().ResetujBabuna();
										a.transform.parent = enemyPool;
										entityProperties = a.GetComponent<EntityProperties>();
										entityProperties.instanciran = true;
										entityProperties.slobodanEntitet = false;
										entityProperties.trenutnoJeAktivan = false;
										//a.transform.localScale = a.transform.localScale*15;
										slotJeSlobodan = false;
										kopljeGorile_postoji_u_poolu = 0;
										postavljen_u_prefabu_kopljeGorile = 1;
									}
								}
							}
						}
					}
				}
				yield return new WaitForSeconds(0.05f);

				if(j == prefabPropertiess.enemiesSlots.Count-1) //prosao kroz sve prefabove i sticajem okolnosti nije postavio neprijatelja, treba da poveca brojac da bi bilo regularno za naredne prefabove
				{
					//Debug.Log("ZADNJI SLOT U PREFABU");
					if(postavljen_u_prefabu_leteciBabuni == 2 && brojacPrefaba == (int)leteciBabuni_Kvota && brojacPrefaba <= 7)
					{
						//Debug.Log("NIJE STVORIO LETECEG BABUNA, POVECAJ@!@!@!");
						leteciBabuni_Kvota += leteciBabuni_Kvota_locked;
					}
					if(postavljen_u_prefabu_leteceGorile == 2 && brojacPrefaba == (int)leteceGorile_Kvota && brojacPrefaba <= 7)
					{
						//Debug.Log("NIJE STVORIO LETECEG GORILU, POVECAJ@!@!@!");
						leteceGorile_Kvota += leteceGorile_Kvota_locked;
					}
					if(postavljen_u_prefabu_boomerangBabuni == 2 && brojacPrefaba == (int)boomerangBabuni_Kvota && brojacPrefaba <= 7)
					{
						//Debug.Log("NIJE STVORIO BOOMERANG BABUNA, POVECAJ@!@!@!");
						boomerangBabuni_Kvota += boomerangBabuni_Kvota_locked;
					}
					if(postavljen_u_prefabu_kopljeGorile == 2 && brojacPrefaba == (int)kopljeGorile_Kvota && brojacPrefaba <= 7)
					{
						//Debug.Log("NIJE STVORIO KOPLJE GORILU, POVECAJ@!@!@!");
						kopljeGorile_Kvota += kopljeGorile_Kvota_locked;
					}
				}

				if(leteciBabuni == 2)
					KorigujVerovatnocuZbogMisije(0,1);
				if(leteceGorile == 2)
					KorigujVerovatnocuZbogMisije(0,2);
				if(boomerangBabuni == 2)
					KorigujVerovatnocuZbogMisije(0,3);
				if(kopljeGorile == 2)
					KorigujVerovatnocuZbogMisije(0,4);

				//Debug.Log("KRAJ FORA: " + j);
			}
			System.GC.Collect();
		}
	}

	IEnumerator DodavanjeSpecial(LevelPrefabProperties prefabPropertiess)
	{
		//Debug.Log("=======================================");
		//Debug.Log("TERENPREFAB: " + brojacPrefaba);

		if(prefabPropertiess.special_Slots_Count > 0) //ako postoje slotovi za special objekte u prefabu
		{
			if(suzenaListaObjekata.Count > 0)
				suzenaListaObjekata.Clear();
			int trenutnaVerovatnoca = Random.Range(0,100); //verovatnoca da ce slot da se koristi
			//Debug.Log("Slotova count: " + prefabPropertiess.specialSlots.Count);
			for(int j = 0; j < prefabPropertiess.specialSlots.Count; j++) //prolaz kroz sve special slotove u prefabu
			{
				//Debug.Log("-----------------------------------");
				//Debug.Log("slot " + j + ", name: " + prefabPropertiess.specialSlots[j].name);
				slotProperties = prefabPropertiess.specialSlots[j].GetComponent<SlotProperties>();
				bool slotJeSlobodan = true;
				if(slotProperties.Verovatnoca >= (100 - trenutnaVerovatnoca))
				{
					//slot se koristi, odredjivanje koja odgovarajuca grupa speciala iz SpecialPool-a moze da se smesti u njega
					if(specialPool.childCount > 0)
					{
						if(suzenaListaObjekata.Count > 0)
							suzenaListaObjekata.Clear();
						
						for(int i = 0; i < specialPool.childCount; i++)
						{
							entityProperties = specialPool.GetChild(i).GetComponent<EntityProperties>();

							if((entityProperties.Type == 5 && crveniDijamant == 1) || (entityProperties.Type == 6 && plaviDijamant == 1) || (entityProperties.Type == 7 && zeleniDijamant == 1))
							{
								//don't summon
							}
							else 
							{
								//Debug.Log("properti da moze: " + entityProperties.Type + ", brojac: " + brojacPrefaba + ", plaviDijamant: " + ((int)plaviDijamant_Kvota) + ", korigovancija: " + plaviDijamant);
								if(brojacPrefaba == (int)plaviDijamant_Kvota && brojacPrefaba <= 7 && plaviDijamant == 0) //obezbediti da se plavi dijamant obavezno pojavi na jednom slotu u okviru trenutnog terena
								{
									if(entityProperties.Type == 6 && slotJeSlobodan) //special entitet jeste plavi dijamant ( iznad umesto !=1 bilo je ==0)
									{
										if((System.Array.IndexOf(slotProperties.availableEntities, entityProperties.Type) > -1) || slotProperties.availableEntities.Length == 0)
										{	//na slot moze da se stavi plavi dijamant
											if(entityProperties.slobodanEntitet) //plavi dijamant je slobodan
											{
												plaviDijamant_postoji_u_poolu = 1;
												//Debug.Log("## TERENPREFAB: " + brojacPrefaba + " PLAVI DIJAMANT OBAVEZNO");
												KorigujVerovatnocuZbogMisije(2,5);
												plaviDijamant_Kvota += plaviDijamant_Kvota_locked;
												Transform special = specialPool.GetChild(i);
												float z = special.localPosition.z;
												entityProperties.slobodanEntitet = false;
												entityProperties.trenutnoJeAktivan = false;
												special.position = new Vector3(prefabPropertiess.specialSlots[j].position.x,prefabPropertiess.specialSlots[j].position.y,z);
												//special.localScale = special.localScale*10;
												entityProperties.brojPojavljivanja++;
												slotJeSlobodan = false;
											}
											else
											{
												////Debug.Log("plavi dijamant NIJE SLOBODAN!!!!!  " + plaviDijamant_postoji_u_poolu);
												//if(plaviDijamant_postoji_u_poolu == 0)
													plaviDijamant_postoji_u_poolu = 2;
											}

										}

									}

								}
								//Debug.Log("properti da moze: " + entityProperties.Type + ", brojac: " + brojacPrefaba + ", crveniDijamantKvota: " + ((int)crveniDijamant_Kvota) + ", korigovancija: " + crveniDijamant);
								if(brojacPrefaba == (int)crveniDijamant_Kvota && brojacPrefaba <= 7 && crveniDijamant == 0) //obezbediti da se crveni dijamant obavezno pojavi na jednom slotu u okviru trenutnog terena
								{
									if(entityProperties.Type == 5 && slotJeSlobodan) //special entitet jeste crveni dijamant
									{
										//if((System.Array.IndexOf(slotProperties.availableEntities, entityProperties.Type) > -1) || slotProperties.availableEntities.Length == 0)
										{	//na slot moze da se stavi crveni dijamant
											if(entityProperties.slobodanEntitet) //crveni dijamant je slobodan
											{
												crveniDijamant_postoji_u_poolu = 1;
												//Debug.Log("## TERENPREFAB: " + brojacPrefaba + " CRVENI DIJAMANT OBAVEZNO");
												KorigujVerovatnocuZbogMisije(2,6);
												crveniDijamant_Kvota += crveniDijamant_Kvota_locked;
												Transform special = specialPool.GetChild(i);
												float z = special.localPosition.z;
												entityProperties.slobodanEntitet = false;
												entityProperties.trenutnoJeAktivan = false;
												special.position = new Vector3(prefabPropertiess.specialSlots[j].position.x,prefabPropertiess.specialSlots[j].position.y,z);
												//special.localScale = special.localScale*10;
												entityProperties.brojPojavljivanja++;
												slotJeSlobodan = false;
											}
											else
											{
												//if(crveniDijamant_postoji_u_poolu == 0)
													crveniDijamant_postoji_u_poolu = 2;
											}

										}

									}

								}
								////Debug.Log("properti da moze: " + entityProperties.Type + ", brojac: " + brojacPrefaba + ", zeleniDijamantKvota: " + ((int)zeleniDijamant_Kvota) + ", korigovancija: " + zeleniDijamant);
								if(brojacPrefaba == (int)zeleniDijamant_Kvota && brojacPrefaba <= 7 && zeleniDijamant == 0) //obezbediti da se zeleni dijamant obavezno pojavi na jednom slotu u okviru trenutnog terena
								{
									if(entityProperties.Type == 7 && slotJeSlobodan) //special entitet jeste zeleni dijamant
									{
										//if((System.Array.IndexOf(slotProperties.availableEntities, entityProperties.Type) > -1) || slotProperties.availableEntities.Length == 0)
										{	//na slot moze da se stavi zeleni dijamant
											if(entityProperties.slobodanEntitet) //zeleni dijamant je slobodan
											{
												zeleniDijamant_postoji_u_poolu = 1;
												////Debug.Log("## TERENPREFAB: " + brojacPrefaba + " ZELENI DIJAMANT OBAVEZNO");
												KorigujVerovatnocuZbogMisije(2,7);
												zeleniDijamant_Kvota += zeleniDijamant_Kvota_locked;
												Transform special = specialPool.GetChild(i);
												float z = special.localPosition.z;
												entityProperties.slobodanEntitet = false;
												entityProperties.trenutnoJeAktivan = false;
												special.position = new Vector3(prefabPropertiess.specialSlots[j].position.x,prefabPropertiess.specialSlots[j].position.y,z);
												//special.localScale = special.localScale*10;
												entityProperties.brojPojavljivanja++;
												slotJeSlobodan = false;
											}
											else
											{
												////Debug.Log("zeleni dijamant NIJE SLOBODAN!!!!!  " + zeleniDijamant_postoji_u_poolu);
												//if(zeleniDijamant_postoji_u_poolu == 0)
													zeleniDijamant_postoji_u_poolu = 2;
											}

										}

									}

								}

								if(entityProperties.slobodanEntitet && slotJeSlobodan)
								{
									if(entityProperties.minimumLevel <= level)
									{
										if((System.Array.IndexOf(slotProperties.availableEntities, entityProperties.Type) > -1) || slotProperties.availableEntities.Length == 0)
										{
//												if(povecajVerovatnocu)
//												{
//													if(proveriDaLiDijamantUcestvujeUMisiji(entityProperties.Type))
//														suzenaListaObjekata.Add(i);
//													else
//													{
//														if(dodajIliNe_special % 2 == 0)
//														{
//															suzenaListaObjekata.Add(i);
//														}
//														dodajIliNe_special++;
//													}
//												////Debug.Log("suzeniPool: " + i + ", " + entityProperties.transform.name);
//												}
//												else
											if((entityProperties.name.Contains("Magnet") && magnetCollected) || (entityProperties.name.Contains("Magnet") && brojacPrefaba < 2))
											{
												//magnet collected, don't add
											}
											else if((entityProperties.name.Contains("DoubleCoins") && doubleCoinsCollected) || (entityProperties.name.Contains("DoubleCoins") && brojacPrefaba < 2))
											{
												//double coins collected, don't add
											}
											else if((entityProperties.name.Contains("Shield") && shieldCollected) || (entityProperties.name.Contains("Shield") && brojacPrefaba < 2))
											{
												//shield collected, don't add
											}
											else
											{
												suzenaListaObjekata.Add(i);
											}
										}
									}
								}
							}




//							else if(entityProperties.slobodanEntitet)
//							{
//								if(entityProperties.minimumLevel <= level)
//								{
//									if((System.Array.IndexOf(slotProperties.availableEntities, entityProperties.Type) > -1) || slotProperties.availableEntities.Length == 0)
//									{
//										if(povecajVerovatnocu)
//										{
//											if(proveriDaLiDijamantUcestvujeUMisiji(entityProperties.Type))
//												suzenaListaObjekata.Add(i);
//											else
//											{
//												if(dodajIliNe_special % 2 == 0)
//												{
//													suzenaListaObjekata.Add(i);
//												}
//												dodajIliNe_special++;
//											}
//										////Debug.Log("suzeniPool: " + i + ", " + entityProperties.transform.name);
//										}
//										else
//										{
//											suzenaListaObjekata.Add(i);
//										}
//									}
//								}
//							}
							
						}

						if(plaviDijamant_postoji_u_poolu == 2)
						{
							////Debug.Log("INSTANCIRAM PLAVI, KORAK 1 ^^^^");
							if(brojacPrefaba == (int)plaviDijamant_Kvota && brojacPrefaba <= 7 && plaviDijamant == 0)
							{
								////Debug.Log("INSTANCIRAM PLAVI, KORAK 2 ^^^^");
								if(slotJeSlobodan)
								{
									////Debug.Log("## TERENPREFAB: " + brojacPrefaba + " PLAVI DIJAMANT OBAVEZNO");
									KorigujVerovatnocuZbogMisije(2,5);
									plaviDijamant_Kvota += plaviDijamant_Kvota_locked;
									GameObject a = Instantiate(specialsForInstantiate[1],new Vector3(prefabPropertiess.specialSlots[j].position.x,prefabPropertiess.specialSlots[j].position.y,prefabPropertiess.specialSlots[j].position.z),Quaternion.identity) as GameObject;
									//a.transform.localScale = a.transform.localScale*15;
									slotJeSlobodan = false;
									plaviDijamant_postoji_u_poolu = 0;
								}
							}
						}
						if(crveniDijamant_postoji_u_poolu == 2)
						{
							if(brojacPrefaba == (int)crveniDijamant_Kvota && brojacPrefaba <= 7 && crveniDijamant == 0)
							{
								if(slotJeSlobodan)
								{
									////Debug.Log("## TERENPREFAB: " + brojacPrefaba + " CRVENI DIJAMANT OBAVEZNO");
									KorigujVerovatnocuZbogMisije(2,6);
									crveniDijamant_Kvota += crveniDijamant_Kvota_locked;
									GameObject a = Instantiate(specialsForInstantiate[0],new Vector3(prefabPropertiess.specialSlots[j].position.x,prefabPropertiess.specialSlots[j].position.y,prefabPropertiess.specialSlots[j].position.z),Quaternion.identity) as GameObject;
									//a.transform.localScale = a.transform.localScale*15;
									slotJeSlobodan = false;
									crveniDijamant_postoji_u_poolu = 0;
								}
							}
						}
						if(zeleniDijamant_postoji_u_poolu == 2)
						{
							////Debug.Log("INSTANCIRAM ZELENI, KORAK 1 ^^^^");
							if(brojacPrefaba == (int)zeleniDijamant_Kvota && brojacPrefaba <= 7 && zeleniDijamant == 0)
							{
								////Debug.Log("INSTANCIRAM ZELENI, KORAK 1 ^^^^");
								if(slotJeSlobodan)
								{
									////Debug.Log("## TERENPREFAB: " + brojacPrefaba + " ZELENI DIJAMANT OBAVEZNO");
									KorigujVerovatnocuZbogMisije(2,7);
									zeleniDijamant_Kvota += zeleniDijamant_Kvota_locked;
									GameObject a = Instantiate(specialsForInstantiate[2],new Vector3(prefabPropertiess.specialSlots[j].position.x,prefabPropertiess.specialSlots[j].position.y,prefabPropertiess.specialSlots[j].position.z),Quaternion.identity) as GameObject;
									//a.transform.localScale = a.transform.localScale*15;
									slotJeSlobodan = false;
									zeleniDijamant_postoji_u_poolu = 0;
								}
							}
						}
						//plaviDijamant_postoji_u_poolu = 0;
						//crveniDijamant_postoji_u_poolu = 0;
						//zeleniDijamant_postoji_u_poolu = 0;

						if(suzenaListaObjekata.Count > 0 && slotJeSlobodan)
						{
							int qq = Random.Range(0,suzenaListaObjekata.Count);
							////Debug.Log("izvucen broj: " + qq);
							////Debug.Log("a objekat: " + specialPool.GetChild(suzenaListaObjekata[qq]).name);
							Transform special = specialPool.GetChild(suzenaListaObjekata[qq]);
							suzenaListaObjekata.RemoveAt(qq);
							if(special != null)
							{
								entityProperties = special.GetComponent<EntityProperties>();
								int EntityVerovatnoca = Random.Range(0,100);
								if(EntityVerovatnoca >= (100 - entityProperties.Verovatnoca))
								{
									if(entityProperties.brojPojavljivanja < entityProperties.maxBrojPojavljivanja || entityProperties.maxBrojPojavljivanja == 0)
									{
										if(entityProperties.DozvoljenoSkaliranje)
										{
											float scalefactor = Random.Range(entityProperties.originalScale.x,2*entityProperties.originalScale.x);
											special.transform.localScale = new Vector3(scalefactor,scalefactor,scalefactor);
										}
										float z = special.localPosition.z;
										//special.parent = prefabPropertiess.specialSlots[j];
										entityProperties.slobodanEntitet = false;
										entityProperties.trenutnoJeAktivan = false;
										special.position = new Vector3(prefabPropertiess.specialSlots[j].position.x,prefabPropertiess.specialSlots[j].position.y,z);
										//special.localPosition = new Vector3(0,0,z);
										entityProperties.brojPojavljivanja++;
									}
								}
							}
						}
						
					}
				}
				yield return new WaitForSeconds(0.05f);
				if(plaviDijamant == 2)
					KorigujVerovatnocuZbogMisije(0,5);
				if(crveniDijamant == 2)
					KorigujVerovatnocuZbogMisije(0,6);
				if(zeleniDijamant == 2)
					KorigujVerovatnocuZbogMisije(0,7);

				//Debug.Log("KRAJ SPECIAL FORA: " + j);
			}
			yield return new WaitForSeconds(0.5f);

			if(prviPrefab)
			{
				//Debug.Log("PRVI PREFAB");
				prviPrefab = false;
				KorigujVerovatnocuZbogMisije(0,0);
			}
//			if(plaviDijamant == 2)
//				KorigujVerovatnocuZbogMisije(0,5);
//			if(crveniDijamant == 2)
//				KorigujVerovatnocuZbogMisije(0,6);
//			if(zeleniDijamant == 2)
//				KorigujVerovatnocuZbogMisije(0,7);

			System.GC.Collect();
		}
	}

	public void PostaviFinish()
	{
		trebaFinish = true;
		MonkeyController2D.canRespawnThings = false;
		//finishHolder.position = GranicaDesno.position;
		//finishHolder.parent = currentLevelComponents;
	}

	void Tezina()
	{
		int world = level/20 + 1;
		int levell = level%20;

		if(levell == 0) {levell = 20;world--;}

		if(tour == 1)
		{
			switch(levell)
			{
			case 1: case 2: case 3: case 4: overallDifficulty = world; break;
			case 5: case 6: case 7: case 8: overallDifficulty = world + 1; break;
			case 9: case 10: case 11: case 12: overallDifficulty = world + 2; break;
			case 13: case 14: case 15: case 16: overallDifficulty = world + 3; break;
			case 17: case 18: case 19: case 20: overallDifficulty = world + 4; break;
			}
		}
		else if(tour == 2)
		{
			switch(levell)
			{
			case 1: case 2: case 3: case 4: overallDifficulty = world + 2; break;
			case 5: case 6: case 7: case 8: overallDifficulty = world + 3; break;
			case 9: case 10: case 11: case 12: overallDifficulty = world + 4; break;
			case 13: case 14: case 15: case 16: overallDifficulty = world + 5; break;
			case 17: case 18: case 19: case 20: overallDifficulty = world + 6; break;
			}
		}
		else if(tour == 3)
		{
			switch(levell)
			{
			case 1: case 2: case 3: case 4: overallDifficulty = world + 4; break;
			case 5: case 6: case 7: case 8: overallDifficulty = world + 5; break;
			case 9: case 10: case 11: case 12: overallDifficulty = world + 6; break;
			case 13: case 14: case 15: case 16: overallDifficulty = world + 7; break;
			case 17: case 18: case 19: case 20: overallDifficulty = world + 8; break;
			}
		}

		switch(overallDifficulty)
		{
		case 1: dg = 10; gg = 15; break;
		case 2: dg = 10; gg = 25; break;
		case 3: dg = 15; gg = 30; break;
		case 4: dg = 20; gg = 30; break;
		case 5: dg = 20; gg = 40; break;
		case 6: dg = 25; gg = 40; break;
		case 7: dg = 30; gg = 50; break;
		case 8: dg = 40; gg = 60; break;
		case 9: dg = 50; gg = 70; break;
		case 10: dg = 65; gg = 80; break;
		case 11: dg = 70; gg = 80; break;
		case 12: dg = 75; gg = 80; break;
		case 13: dg = 75; gg = 85; break;
		case 14: dg = 80; gg = 90; break;
		case 15: dg = 90; gg = 100; break;
		case 16: dg = 100; gg = 100; break;
		}

	}

	void postaviInicijalnuTezinu(LevelPrefabProperties prefabPropertiess)
	{
		switch(overallDifficulty)
		{
		case 1: dg = 10; gg = 15; break;
		case 2: dg = 10; gg = 25; break;
		case 3: dg = 15; gg = 30; break;
		case 4: dg = 20; gg = 30; break;
		case 5: dg = 20; gg = 40; break;
		case 6: dg = 25; gg = 40; break;
		case 7: dg = 30; gg = 50; break;
		case 8: dg = 40; gg = 60; break;
		case 9: dg = 50; gg = 70; break;
		case 10: dg = 65; gg = 80; break;
		case 11: dg = 70; gg = 80; break;
		case 12: dg = 75; gg = 80; break;
		case 13: dg = 75; gg = 85; break;
		case 14: dg = 80; gg = 90; break;
		case 15: dg = 90; gg = 100; break;
		case 16: dg = 100; gg = 100; break;
		}
		////Debug.Log("dg: " + dg + ", gg: " + gg);
		int diff = Random.Range(dg,gg);
		SlotProperties sp;
		for(int i=0;i<prefabPropertiess.enemiesSlots.Count;i++)
		{
			sp = prefabPropertiess.enemiesSlots[i].GetComponent<SlotProperties>();
			sp.Verovatnoca = diff;
		}
	}

	public void KorigujVerovatnocuZbogMisije(int value, int tip) //value: 0 - regularno se pojavljuje, 1 - 0% verovatnoca, 2 - 100% verovatnoca, ali da se samo jedanput postavi u okviru jednog slota 
	{															 //tip: 0 - odnosi se na sve, 1 - leteci babuni, 2 - letece gorile, 3 - boomerang babuni, 4 - koplje gorile, 5 - blue, 6 - red, 7 - green
		//Debug.Log("()()()()()() -- ULETAAAC");
		if(MissionManager.activeFly_BaboonsMission && (tip == 0 || tip == 1))
		{
			if(value == 1)
			{
				if((int)leteciBabuni_Kvota == 7)
				{
					leteciBabuni = value;
				}
			}
			else
			{
				leteciBabuni = value;
			}
		}
		if(MissionManager.activeFly_GorillaMission && (tip == 0 || tip == 2))
		{
			if(value == 1)
			{
				if((int)leteceGorile_Kvota == 7)
				{
					leteceGorile = value;
				}
			}
			else
			{
				leteceGorile = value;
			}
		}
		if(MissionManager.activeBoomerang_BaboonsMission && (tip == 0 || tip == 3))
		{
			if(value == 1)
			{
				if((int)boomerangBabuni_Kvota == 7)
				{
					boomerangBabuni = value;
				}
			}
			else
			{
				boomerangBabuni = value;
			}
		}
		if(MissionManager.activeKoplje_GorillaMission && (tip == 0 || tip == 4))
		{
			if(value == 1)
			{
				if((int)kopljeGorile_Kvota == 7)
				{
					kopljeGorile = value;
				}
			}
			else
			{
				kopljeGorile = value;
			}
		}
		if(MissionManager.activeBlueDiamondsMission && (tip == 0 || tip == 5))
		{
			if(value == 1)
			{
				if((int)plaviDijamant_Kvota == 7)
				{
					plaviDijamant = value;
				}
			}
			else
				plaviDijamant = value;
		}
		if(MissionManager.activeRedDiamondsMission && (tip == 0 || tip == 6))
		{
			if(value == 1)
			{
				if((int)crveniDijamant_Kvota == 7)
				{
					crveniDijamant = value;
				}
			}
			else
				crveniDijamant = value;
		}
		if(MissionManager.activeGreenDiamondsMission && (tip == 0 || tip == 7))
		{
			if(value == 1)
			{
				if((int)zeleniDijamant_Kvota == 7)
				{
					zeleniDijamant = value;
				}
			}
			else
				zeleniDijamant = value;
		}
	}

	public void izbrojPosebne()
	{
		if(MissionManager.activeFly_BaboonsMission)
			brojPosebnihNeprijatelja++;
		if(MissionManager.activeFly_GorillaMission)
			brojPosebnihNeprijatelja++;
		if(MissionManager.activeBoomerang_BaboonsMission)
			brojPosebnihNeprijatelja++;
		if(MissionManager.activeKoplje_GorillaMission)
			brojPosebnihNeprijatelja++;
		if(MissionManager.activeBlueDiamondsMission)
			brojPosebnihDijamanata++;
		if(MissionManager.activeRedDiamondsMission)
			brojPosebnihDijamanata++;
		if(MissionManager.activeGreenDiamondsMission)
			brojPosebnihDijamanata++;
	}

	bool proveriDaLiNeprijateljUcestvujeUMisiji(int type)
	{
		//Debug.Log("Proveravam: " + type);
		if(type == 3 && leteciBabuni == 2)
		{
			//Debug.Log("misija sa letecim babunima");
			return true;
		}
		if(type == 4 && boomerangBabuni == 2)
		{
			//Debug.Log("misija sa boomerang babunima");
			return true;
		}
		if(type == 5 && leteceGorile == 2)
		{
			//Debug.Log("misija sa letecim gorilama");
			return true;
		}
		if(type == 6 && kopljeGorile == 2)
		{
			//Debug.Log("misija sa kopljem gorilama");
			return true;
		}
		return false;
	}
	bool proveriDaLiDijamantUcestvujeUMisiji(int type)
	{
		if(type == 5 && crveniDijamant == 2)
			return true;
		if(type == 6 && plaviDijamant == 2)
			return true;
		if(type == 7 && zeleniDijamant == 2)
			return true;
		return false;
	}
	
}
