using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//[ExecuteInEditMode]
public class AvatarEditor : MonoBehaviour 
{
	public bool AvatarInit=false;
	public GameObject renderPlate;
	RenderTexture avatarRTD;
	public Material avatarMat;
	public Camera avatarCam;
	public TextureSwapper[] swappers;
	public List<SkinnedMeshRenderer> hairModels;
	public int currentHairModel;
	public int oldHairModel;
	public SkinColors skincolors;
	
	// Use this for initialization
	public void Start () 
	{
		if(AvatarInit == false)
		{
			Transform[] tempTMs = gameObject.GetComponentsInChildren<Transform>();
			for(int index=0;index<tempTMs.Length;index++)
			{
				if(tempTMs[index].name == "RenderPlates")
				{
					renderPlate= tempTMs[index].gameObject;
				}
			}

			avatarRTD = new RenderTexture(2048,1024,24,RenderTextureFormat.ARGB32);
			avatarMat = new Material(avatarMat);
			avatarCam.targetTexture = avatarRTD;
			avatarMat.SetTexture("_MainTex",avatarRTD);
			SkinnedMeshRenderer[] skins = transform.parent.parent.GetComponentsInChildren<SkinnedMeshRenderer>();
			print (transform.parent.parent.name);
			for(int index=0;index<skins.Length;index++)
			{
				if(avatarMat.name.Contains(skins[index].sharedMaterial.name)) skins[index].sharedMaterial = avatarMat;
			}

			//get Texture Swappers
			swappers = renderPlate.GetComponentsInChildren<TextureSwapper>();

			//initialize Texture Swappers
			for(int index=0;index<swappers.Length;index++)
			{
				swappers[index].Start();
			}

			skincolors = renderPlate.GetComponentInChildren<SkinColors>();
			skincolors.Start();

			//setup mesh hair swapper
			SkinnedMeshRenderer[] tempskins = transform.parent.transform.parent.GetComponentsInChildren<SkinnedMeshRenderer>();
			hairModels = new List<SkinnedMeshRenderer>();
			for(int index=0;index<tempskins.Length;index++)
			{
				if(tempskins[index].name.Contains("hair"))
				{
					hairModels.Add(tempskins[index]);
				}
			}
		}
		AvatarInit = true;
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
}
