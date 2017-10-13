using UnityEngine;
using System.Collections;

public class VerticalScroll : MonoBehaviour {

	public Transform upLimit;
	public Transform downLimit;
	Transform items;
	float upLimitY;
	float downLimitY;
	bool pomeraj;
	float startY;
	float endY;
	public bool canScroll = true;
	string clickedItem;
	string releasedItem;
	float offsetY;
	bool bounce;
	bool moved;
	bool released;

	void Start () 
	{
		items = transform.Find("Items");
		upLimitY = upLimit.position.y;
		downLimitY = downLimit.position.y;
	}
	
	void Update () 
	{
		if(canScroll)
		{
			if(Input.GetMouseButtonDown(0))
			{
				clickedItem = RaycastFunction(Input.mousePosition);
				startY = endY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
				Debug.Log("start y: " + startY);
			}
			else if(Input.GetMouseButton(0))
			{
				moved = true;
				endY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
				offsetY = endY - startY;
				items.position = new Vector3(items.position.x, /*Mathf.Clamp(*/Mathf.MoveTowards(items.position.y,items.position.y+offsetY,0.5f)/*,downLimitY,upLimitY)*/,items.position.z);
				startY = endY;
			}
			else if(Input.GetMouseButtonUp(0))
			{
				if(moved)
				{
					moved = false;
					released = true;
				}
				//startY = endY = 0;
			}


			if(released)
			{
				items.Translate(0,offsetY,0);
				offsetY *= 0.92f;
			}

			if(released && startY == endY)
			{
				if(items.position.y < downLimitY)
				{
					//offsetY = -offsetY;
					items.position = new Vector3(items.position.x, Mathf.MoveTowards(items.position.y,downLimitY,1f),items.position.z);
				}
				else if(items.position.y > upLimitY)
				{
					//offsetY = -offsetY;
					items.position = new Vector3(items.position.x, Mathf.MoveTowards(items.position.y,upLimitY,1f),items.position.z);
				}
				else if(items.position.y == upLimitY || items.position.y == downLimitY)
				{
					released = false;
				}
			}
		}
	}

	string RaycastFunction(Vector3 obj)
	{
		Ray ray = Camera.main.ScreenPointToRay(obj);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit))
		{
			return hit.collider.name;
		}
		return System.String.Empty;
	}
}
