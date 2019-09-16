using UnityEngine;
using System.Collections;
using System.IO;

public class TextureSwapper : MonoBehaviour 
{	public string dirPath = "/AllStarCharacterLibrary/Resources/ASCL_AtlasData/Female/Materials/Eyes/";
	public MeshRenderer mesh;
	public Material mat;
	public Material oldMat;
	public Texture2D currentTexture;
	public int currentIndex;
	public int oldIndex;
	public string[] names;

	// Use this for initialization
	public void Start () 
	{
		string[] ffiles = Directory.GetFiles( Application.dataPath + dirPath,"*.tga");
		names = new string[ffiles.Length];
		for (int i = 0;i<ffiles.Length;i++)
		{
			string[] tempStringArray = ffiles[i].Split ('/');
			names[i]=tempStringArray[tempStringArray.Length-1];
		}
		if(oldMat!=null)
		{
			oldMat = mesh.sharedMaterial;
		}
		mat = new Material(mesh.sharedMaterial);
		mesh.sharedMaterial = mat;
	}

	public void Update()
	{
		if (currentIndex!=oldIndex)
		{
			if(currentIndex<0 || currentIndex>names.Length-1)
			{
				currentIndex = oldIndex;
			}
			else
			{
				UpdateTexture();
			}
			//r.renderer.material.color = new Color(1f,.3f,0.3f,1f);
		}
	}

	// Update is called once per frame
	void UpdateTexture () 
	{

		currentTexture = (Texture2D) UnityEditor.AssetDatabase.LoadAssetAtPath(("Assets"+ dirPath + names[currentIndex]),typeof(Texture2D));

		mat.SetTexture("_MainTex",currentTexture);
		oldIndex=currentIndex;
	}
}
