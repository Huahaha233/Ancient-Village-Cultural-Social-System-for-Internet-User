using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CreateTextureAndMaterial : MonoBehaviour 
{
	public GameObject 				source;
	public GameObject					nSource;
	public Camera							cam;
	public Texture2D					texture;
	public Material							newMat; 
	public bool								reInit;
	//public ProceduralMaterial					substance;

	// Use this for initialization
	void Awake () 
	{

		DestroyImmediate(GetComponent<Renderer>().sharedMaterial);
		newMat = new Material(source.GetComponent<Renderer>().sharedMaterial);
		
		if(reInit==true)
		{
			//store active render texture
			RenderTexture currentRT = RenderTexture.active;
			//change renderTexture to cam's renderTexture
			RenderTexture.active = cam.targetTexture;
			
			cam.Render();
			
			//make a new texture
			texture = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
			//read pixels copies from the active render texture
			texture.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
			texture.Apply();
			
			//restore last active renderTexture
			RenderTexture.active = currentRT;

			newMat.mainTexture = texture;
		}
		else
		{
			texture= new Texture2D(2048,2048);
			//texture = (nSource.renderer.sharedMaterial.GetTexture("_MainTex") as Texture2D);
			//texture = (substance.mainTexture as Texture2D);
			//texture = (substance.GetTexture("_BumpMap") as Texture2D);
			newMat.SetTexture("_BumpMap", texture);
		}
		
			GetComponent<Renderer>().sharedMaterial  = newMat;

	}
}
