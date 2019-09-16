//ASCL AvatarSystem @Ranjeet "Rungy" Singhal July 2016 (SexySideKicks.com)
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AvatarLODSystem : MonoBehaviour 
{
	public List<SkinnedMeshRenderer> bodies;
	public List<float> ratios;
	public float screenRatio;
	public int currentBodyIndex;

	// Use this for initialization
	void Start () 
	{
		currentBodyIndex=0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//we need to get the screenratio for this model to use LOD's
		Vector3 top = Camera.main.WorldToScreenPoint( bodies[0].bounds.center+ bodies[0].bounds.max);
		Vector3 bottom = Camera.main.WorldToScreenPoint(bodies[0].transform.position);
		screenRatio =  (Mathf.Abs(top.y-bottom.y))/Screen.height;
		//from the line above, any value larger than 1 means the character is larger than the screen
		//at .01 (1% of the screen) we should probably turn the model off completely
		for(int lod = 0;lod<bodies.Count;lod++)
		{
			if(ratios[lod]<screenRatio)
			{
				bodies[currentBodyIndex].enabled = false;
				bodies[lod].enabled=true;
				currentBodyIndex = lod;
				break;
			}
		}
	}
}
