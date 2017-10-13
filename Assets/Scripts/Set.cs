using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Klasa za cuvanje podataka o jednom svetu tj Set-u.
/// </summary>
public class Set  {
	/// <summary>
	/// The total stars in stage i.e. level(max that can be achived in this world).
	/// </summary>
	private int totalStarsInStage=0;
	public int TotalStarsInStage
	{
		get{return totalStarsInStage;}
		set{totalStarsInStage=value;}
	}
	/// <summary>
	/// The current stars(stars that have been collected) in stage.
	/// </summary>
	private int currentStarsInStage=0;
	public int CurrentStarsInStage
	{
		get{return currentStarsInStage;}
		set{currentStarsInStage=value;}
	}

	private int currentStarsInStageNEW=0;
	public int CurrentStarsInStageNEW
	{
		get{return currentStarsInStageNEW;}
		set{currentStarsInStageNEW=value;}
	}

	/// <summary>
	/// Required number of stars to unlock this world.
	/// </summary>
	private int starRequirement=0;
	public int StarRequirement
	{
		get{return starRequirement;}
		set{starRequirement=value;}
	}
	
	/// <summary>
	/// Name of the world.
	/// </summary>
	private string setID="NoTNamed";
	public string SetID
	{
		get{return setID;}
		set{setID=value;}
	}
	
	/// <summary>
	/// Numbere of stages on set.
	/// </summary>
	private int stagesOnSet=0;
	public int StagesOnSet
	{
		get{return stagesOnSet;}
		set{stagesOnSet=value;}
	}
	
	//-1 locked, 0 unlocked but not passed, rest(1,2,3) passed
	/// <summary>
	/// Array of stars per stage. Number determains state of stage -1 locked, 0 unlocked but not passed, rest(1,2,3) passed. -42 not initialized
	/// </summary>
	public int[] starsPerStage;
	public int GetStarOnStage(int lvl)
	{
		if(lvl<stagesOnSet)
		{
			if(starsPerStage!=null)
			{
				return starsPerStage[lvl];
			}
		}
		return -42;
	}
	/// <summary>
	/// Determines whether specified level is unlocked.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this level is unlocked for the specified level; otherwise, <c>false</c>.
	/// </returns>
	/// <param name='lvl'>
	/// what level should be checked 
	/// </param>
	/// <exception cref='Exception'>
	/// Throws exception with text "ERROR!"
	/// </exception>
	public bool IsLvlUnlocked(int lvl)
	{
		if(lvl<stagesOnSet)
		{
			if(starsPerStage!=null)
			{
				if( starsPerStage[lvl]>-1)
					return true;
				else 
					return false;
			}
			throw new Exception("ERROR!");
		}
		throw new Exception("ERROR!");
	}
	/// <summary>
	/// Sets the stars on stage. Checks if its bigger than previous and increments needed values
	/// </summary>
	/// <param name='lvl'>
	/// Level for which stars should be set.
	/// </param>
	/// <param name='starN'>
	/// Number of stars.
	/// </param>
	public void SetStarOnStage(int lvl,int starN)
	{
		if(lvl<stagesOnSet)
		{
			if(starsPerStage!=null)
			{
				if( starsPerStage[lvl]<starN)
				{
					CurrentStarsInStage-=(( starsPerStage[lvl]>0)? starsPerStage[lvl]:0);
					StagesParser.currentStars-=(( starsPerStage[lvl]>0)? starsPerStage[lvl]:0);
					starsPerStage[lvl]=starN;
					CurrentStarsInStage+=(( starN>0)? starN:0);
					StagesParser.currentStars+=(( starN>0)? starN:0);
				}
			}
			else
			{
				starsPerStage = new int[stagesOnSet];
				for(int i=0;i<stagesOnSet;i++)
					starsPerStage[i]=-42;
				starsPerStage[lvl]=starN;

			}
		}
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="Set"/> class.
	/// </summary>
	public Set()
	{
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="Set"/> class.
	/// </summary>
	/// <param name='numberOfStages'>
	/// Number of stages.
	/// </param>
	public Set(int numberOfStages)
	{
		starsPerStage= new int[numberOfStages];
		stagesOnSet=numberOfStages;
		for(int i=0;i<stagesOnSet;i++)
					starsPerStage[i]=-42;
	}
}
