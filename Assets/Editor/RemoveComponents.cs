using UnityEngine;
using UnityEditor; 
using System.Collections;

public class RemoveComponents : ScriptableWizard	
{			
	[MenuItem("Custom/Remove Components")]		
	
	static void CreateWizard ()		
	{						
		ScriptableWizard .DisplayWizard <RemoveComponents> ("Remove Components", "RemoveComponents");
	}
	
	void OnWizardCreate ()
	{
		Animator[] components = GameObject.FindSceneObjectsOfType (typeof(Animator)) as Animator[];
		foreach (var component in components) {
			DestroyImmediate ( component );
		}   
	}
	
}