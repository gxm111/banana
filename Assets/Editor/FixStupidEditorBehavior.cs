using UnityEngine;

using UnityEditor;



public class FixStupidEditorBehavior : MonoBehaviour {
	
	[MenuItem("GameObject/Create Empty Child #&n")]
	
	static void BringMeBackFromTheEdgeOfMadness() {
		
		GameObject go = new GameObject("GameObject");
		
		if(Selection.activeTransform != null)
			
			go.transform.parent = Selection.activeTransform;
		
	}
	
}