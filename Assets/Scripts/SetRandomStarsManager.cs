using UnityEngine;
using System.Collections;

public class SetRandomStarsManager : MonoBehaviour {
	
	
	int currSet = 0;
	int currStage = 0;

	int prevoiousSetIndex;
	//ManageFull gameManager; // // ZA PRVU VERZIJU
	Manage gameManager; // ZA FINALNU VERZIJU

	bool uslovNivo = false;
	bool uslovZvezdice = false;

	// Use this for initialization
	void Start () {
	
		StagesParser.NemaRequiredStars_VratiULevele = false;

		currSet = StagesParser.currSetIndex;
		currStage = StagesParser.currStageIndex;

		//GameObject.Find("StagesParserManager").GetComponent<StagesParser>().CallSave();

		gameManager = GetComponent<Manage>(); // ZA PRVU VERZIJU
		//gameManager = GetComponent<ManageFull>(); // ZA FINALNU VERZIJU

		//Invoke("GoBack",1);
		
		
	}

	
	public void GoBack()
	{
		//Debug.Log("Trenutni nivo: " + StagesParser.currStageIndex);
		prevoiousSetIndex = StagesParser.currSetIndex;

		int starsGained=gameManager.starsGained;

		if(StagesParser.bonusLevel)
		{
			string[] BonusValues = PlayerPrefs.GetString("BonusLevel").Split('_');
			string kovcezi = BonusValues[StagesParser.currSetIndex];
			string[] kovceziValues = kovcezi.Split('#');
			kovceziValues[StagesParser.bonusID-1] = "1";
			
			string pom = System.String.Empty;
			kovcezi = System.String.Empty;
			for(int i=0;i<kovceziValues.Length;i++)
			{
				kovcezi+=kovceziValues[i] + "#";
			}
			kovcezi = kovcezi.Remove(kovcezi.Length-1);
			BonusValues[StagesParser.currSetIndex] = kovcezi;
			
			for(int i=0;i<StagesParser.totalSets;i++)
			{
				pom += BonusValues[i] + "_"; 
			}
			pom = pom.Remove(pom.Length-1);
			PlayerPrefs.SetString("BonusLevel",pom);
			PlayerPrefs.Save();
			StagesParser.bonusLevels = pom;
			StagesParser.ServerUpdate = 1;

			//PlayerPrefs.SetInt("BonusLevel#"+StagesParser.currentWorld+"#"+StagesParser.bonusID,1);
			//PlayerPrefs.Save();
		}
		else
		{
			string[] levelValues = StagesParser.allLevels[currSet*20+currStage].Split('#');
			int previousPoints = int.Parse(levelValues[2]);
		
			//if(StagesParser.SetsInGame[currSet].GetStarOnStage(currStage) < starsGained)
			if(Manage.points > previousPoints)
			{
				string pom = System.String.Empty;
				StagesParser.allLevels[currSet*20+currStage] = (currSet*20+currStage+1).ToString()+"#"+starsGained+"#"+Manage.points;
				for(int i=0;i<StagesParser.allLevels.Length;i++)
				{
					pom+=StagesParser.allLevels[i];
					pom+="_";
				}
				pom = pom.Remove(pom.Length-1);
				PlayerPrefs.SetString("AllLevels",pom);
				PlayerPrefs.Save();


				if(StagesParser.currSetIndex != 5 || StagesParser.currStageIndex != 19) //bilo je StagesParser.currSetIndex != 4
				{
					string[] values = StagesParser.allLevels[currSet*20+currStage+1].Split('#');

					if(currStage<19 && int.Parse(values[1]) == -1)
					{
						pom = System.String.Empty;
						StagesParser.allLevels[currSet*20+currStage+1] = (currSet*20+currStage+2).ToString()+"#0#0";
						for(int i=0;i<StagesParser.allLevels.Length;i++)
						{
							pom+=StagesParser.allLevels[i];
							pom+="_";
						}
						pom = pom.Remove(pom.Length-1);
						PlayerPrefs.SetString("AllLevels",pom);
						PlayerPrefs.Save();
						StagesParser.zadnjiOtkljucanNivo = currStage+2;
					}

					if(StagesParser.maxLevel < currSet*20+currStage+1)
						StagesParser.maxLevel = currSet*20+currStage+1;
				}
			}

			StagesParser.trenutniNivoNaOstrvu[StagesParser.currSetIndex] = StagesParser.currStageIndex + 1;
			PlayerPrefs.SetInt("TrenutniNivoNaOstrvu"+(StagesParser.currSetIndex).ToString(),StagesParser.trenutniNivoNaOstrvu[StagesParser.currSetIndex]);
			PlayerPrefs.Save();

			string[] valuess = StagesParser.allLevels[StagesParser.lastUnlockedWorldIndex*20+19].Split('#');
			Debug.Log("ISPRED USLOV ZA NIVO: " + StagesParser.allLevels[StagesParser.lastUnlockedWorldIndex*20+19]);
			if(int.Parse(valuess[1]) > 0)
			{
				uslovNivo = true;
			}

			StagesParser.RecountTotalStars();
			float potrebneZvezdice = 0;
			int limit = StagesParser.totalSets;
			for(int i=0;i<=StagesParser.lastUnlockedWorldIndex+1;i++)
			{
				if(i<limit)
					potrebneZvezdice = StagesParser.SetsInGame[i].StarRequirement;
			}

			PlayerPrefs.SetInt("CurrentStars",StagesParser.currentStarsNEW);
			PlayerPrefs.Save();
			Debug.Log("potrebne zvezdice: " + potrebneZvezdice + ", a ima gi: " + StagesParser.currentStarsNEW + ", last unlocked world index: " + StagesParser.lastUnlockedWorldIndex);
			Debug.Log("Unlocked worlds: " + StagesParser.unlockedWorlds[0] + ", " +  StagesParser.unlockedWorlds[1] + ", " +  StagesParser.unlockedWorlds[2] + ", " +  StagesParser.unlockedWorlds[3] + ", " +  StagesParser.unlockedWorlds[4] + ", " + StagesParser.unlockedWorlds[5]);
			if(potrebneZvezdice <= StagesParser.currentStarsNEW)
			{
				if(StagesParser.lastUnlockedWorldIndex + 1 < StagesParser.totalSets)
				{
					if(!StagesParser.unlockedWorlds[StagesParser.lastUnlockedWorldIndex + 1])
					{
						uslovZvezdice = true;
					}
				}
			}
			Debug.Log("uslov nivo: " + uslovNivo + ", uslov zvezdice: " + uslovZvezdice);
			if(uslovNivo && uslovZvezdice)
			{
				Debug.Log("ULETEO OVDE: IMA USLOVE ZA NIVO I ZVEZDICE");
				StagesParser.unlockedWorlds[StagesParser.lastUnlockedWorldIndex + 1] = true;
				StagesParser.openedButNotPlayed[StagesParser.lastUnlockedWorldIndex + 1] = true;
				StagesParser.lastUnlockedWorldIndex+=1;
				StagesParser.isJustOpened = true;

				StagesParser.allLevels[StagesParser.lastUnlockedWorldIndex*20] = (StagesParser.lastUnlockedWorldIndex*20+1)+"#0#0";
				StagesParser.StarsPoNivoima[StagesParser.lastUnlockedWorldIndex*20] = 0;

				if(StagesParser.lastUnlockedWorldIndex == 5 && FB.IsLoggedIn) //@@@@@@ DODATAK ZA NOVA OSTRVA
				{
					for(int i=0;i<FacebookManager.ListaStructPrijatelja.Count;i++)
					{
						if(FacebookManager.ListaStructPrijatelja[i].PrijateljID.Equals(FacebookManager.User))
						{
							if(FacebookManager.ListaStructPrijatelja[i].scores.Count < StagesParser.allLevels.Length)
							{
								for(int j=FacebookManager.ListaStructPrijatelja[i].scores.Count;j<StagesParser.allLevels.Length;j++)
									FacebookManager.ListaStructPrijatelja[i].scores.Add(0);
							}
						}
					}
				} //@@@@@@

				string pom = System.String.Empty;
				for(int i=0;i<StagesParser.allLevels.Length;i++)
				{
					pom+=StagesParser.allLevels[i];
					pom+="_";
				}
				pom = pom.Remove(pom.Length-1);

				PlayerPrefs.SetString("AllLevels",pom);
				PlayerPrefs.Save();
				Debug.Log("StagesParser.allLevels[StagesParser.lastUnlockedWorldIndex*20] = " + StagesParser.allLevels[StagesParser.lastUnlockedWorldIndex*20] + ", treba upisat': " +((StagesParser.lastUnlockedWorldIndex*20+1)+"#0#0"));
				Debug.Log("svi nivoji: " + pom);
			}
			else
			{
				StagesParser.NemaRequiredStars_VratiULevele = true;
			}
		}
		StagesParser.saving = true;

	}
}
