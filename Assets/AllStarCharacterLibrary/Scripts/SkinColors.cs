using UnityEngine;
using System.Collections;

public class SkinColors : MonoBehaviour 
{
	public MeshRenderer skinPlate;
	public MeshRenderer facePlate;
	Material skinMat;
	Material faceMat;
	public Material[] skinColors;
	[SerializeField]
	public int skinColorCurrentIndex;
	public int skinColorOldIndex;
	public void Start()
	{
		skinMat = new Material(skinPlate.sharedMaterial);
		skinPlate.sharedMaterial = skinMat;
		faceMat = facePlate.sharedMaterial;

		//Handle the Main character material and Render Texture
	}

	public void Update()
	{
		skinMat = skinPlate.sharedMaterial;
		faceMat = facePlate.sharedMaterial;
		skinMat.shader = skinColors[skinColorCurrentIndex].shader;
		skinMat.color = skinColors[skinColorCurrentIndex].color;
		faceMat.shader = skinColors[skinColorCurrentIndex].shader;
		faceMat.color = skinColors[skinColorCurrentIndex].color;
	}
}
