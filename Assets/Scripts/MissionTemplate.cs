using UnityEngine;
using System.Collections;

public class MissionTemplate {

	public int baboons 					= 0;
	public int fly_baboons 				= 0;
	public int boomerang_baboons 		= 0;
	public int gorilla 					= 0;
	public int fly_gorilla 				= 0;
	public int koplje_gorilla 			= 0;
	public int diamonds 				= 0;
	public int coins 					= 0;
	public int distance 				= 0;
	public int barrels 					= 0;
	public int red_diamonds 			= 0;
	public int blue_diamonds 			= 0;
	public int green_diamonds 			= 0;
	public int points		 			= 0;
	public string level					= System.String.Empty;
	public string description_en 		= System.String.Empty;
	public string description_us 		= System.String.Empty;
	public string description_es 		= System.String.Empty;
	public string description_ru 		= System.String.Empty;
	public string description_pt 		= System.String.Empty;
	public string description_pt_br 	= System.String.Empty;
	public string description_fr	 	= System.String.Empty;
	public string description_tha	 	= System.String.Empty;
	public string description_zh	 	= System.String.Empty;
	public string description_tzh	 	= System.String.Empty;
	public string description_ger	 	= System.String.Empty;
	public string description_it	 	= System.String.Empty;
	public string description_srb	 	= System.String.Empty;
	public string description_tur	 	= System.String.Empty;
	public string description_kor	 	= System.String.Empty;

	public MissionTemplate()
	{
		baboons 					= 0;
		fly_baboons 				= 0;
		boomerang_baboons 		= 0;
		gorilla 					= 0;
		fly_gorilla 				= 0;
		koplje_gorilla 			= 0;
		diamonds 					= 0;
		coins 					= 0;
		distance 					= 0;
		barrels 					= 0;
		red_diamonds 				= 0;
		blue_diamonds 			= 0;
		green_diamonds 			= 0;
		points		 			= 0;
		level					= System.String.Empty;
		description_en 		= System.String.Empty;
		description_us 		= System.String.Empty;
		description_es 		= System.String.Empty;
		description_ru 		= System.String.Empty;
		description_pt 		= System.String.Empty;
		description_pt_br 	= System.String.Empty;
		description_fr	 	= System.String.Empty;
		description_tha	 	= System.String.Empty;
		description_zh	 	= System.String.Empty;
		description_tzh	 	= System.String.Empty;
		description_ger	 	= System.String.Empty;
		description_it	 	= System.String.Empty;
		description_srb	 	= System.String.Empty;
		description_tur	 	= System.String.Empty;
		description_kor	 	= System.String.Empty;
	}

	public string IspisiDescriptionNaIspravnomJeziku()
	{
		if(LanguageManager.chosenLanguage.Equals("_en"))
			return description_en;
		if(LanguageManager.chosenLanguage.Equals("_us"))
			return description_us;
		if(LanguageManager.chosenLanguage.Equals("_es"))
			return description_es;
		if(LanguageManager.chosenLanguage.Equals("_ru"))
			return description_ru;
		if(LanguageManager.chosenLanguage.Equals("_pt"))
			return description_pt;
		if(LanguageManager.chosenLanguage.Equals("_br"))
			return description_pt_br;
		if(LanguageManager.chosenLanguage.Equals("_fr"))
			return description_fr;
		if(LanguageManager.chosenLanguage.Equals("_th"))
			return description_tha;
		if(LanguageManager.chosenLanguage.Equals("_ch"))
			return description_zh;
		if(LanguageManager.chosenLanguage.Equals("_tch"))
			return description_tzh;
		if(LanguageManager.chosenLanguage.Equals("_de"))
			return description_ger;
		if(LanguageManager.chosenLanguage.Equals("_it"))
			return description_it;
		if(LanguageManager.chosenLanguage.Equals("_srb"))
			return description_srb;
		if(LanguageManager.chosenLanguage.Equals("_tr"))
			return description_tur;
		if(LanguageManager.chosenLanguage.Equals("_ko"))
			return description_kor;
		return System.String.Empty;
	}
}
