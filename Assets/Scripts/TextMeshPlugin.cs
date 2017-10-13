using UnityEngine;
using System.Collections;

[RequireComponent (typeof (TextMesh))]
public class TextMeshPlugin: MonoBehaviour {
	public enum TextEffect 
	{
		None,Shadow,Outline,SingleOutline
	};
	public TextEffect textEffect = TextEffect.Outline;
	public float outlineOffset = 0.05f;
	/// <summary>
	/// The single outline offset. 1- ...Procenti za koliko je veci font iza.
	/// </summary>
	public float singleOutlineOffset=1.1f;
	public Vector2 shadowPosition;
	public Color effectColor=Color.black;
	TextMesh thisComponent;
	
	// Use this for initialization
	void Start () 
	{
		thisComponent=this.GetComponent<TextMesh>();
		if(textEffect==TextEffect.Outline)
		{
			thisComponent.AddOutline(effectColor,outlineOffset);
		}
		else if(textEffect==TextEffect.Shadow)
		{
			thisComponent.AddShadow(effectColor,shadowPosition);
		}
		else if(textEffect==TextEffect.SingleOutline)
			thisComponent.AddSingleOutline(effectColor,singleOutlineOffset);
	}
}
