using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(AvatarEditor))]
public class AvatarEditorGUI : Editor
{
	[SerializeField]
	public AvatarEditor ae;
	
	void OnEnable()
	{
		ae = (AvatarEditor) target;
		ae.Start();
	}

	public override void OnInspectorGUI()
	{
		//RUNGY
		//GUI for SkinColor
		/*
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(("face:"),GUILayout.Width(80));
		ae.face.currentIndex =EditorGUILayout.IntSlider (ae.face.currentIndex,0,ae.face.names.Length,GUILayout.Width(170));
		EditorGUILayout.EndHorizontal();
		if(ae.face.currentIndex != ae.face.oldIndex) ae.face.Update();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(("eyes:"),GUILayout.Width(80));
		ae.eyes.currentIndex =EditorGUILayout.IntSlider (ae.eyes.currentIndex,0,ae.eyes.names.Length,GUILayout.Width(170));
		EditorGUILayout.EndHorizontal();
		if(ae.eyes.currentIndex != ae.eyes.oldIndex) ae.eyes.Update();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(("hair color:"),GUILayout.Width(80));
		ae.hair.currentIndex =EditorGUILayout.IntSlider (ae.hair.currentIndex,0,ae.hair.names.Length,GUILayout.Width(170));
		EditorGUILayout.EndHorizontal();
		if(ae.hair.currentIndex != ae.hair.oldIndex) ae.hair.Update();
		*/

		if(GUILayout.Button("Reinitialize",GUILayout.Width(255)))
		{
			ae.AvatarInit = false;
			ae.Start();
		}
		ae.avatarMat = (Material) EditorGUILayout.ObjectField(ae.avatarMat , typeof(Material) ,GUILayout.Width(180));
		ae.avatarCam = (Camera) EditorGUILayout.ObjectField(ae.avatarCam , typeof(Camera) ,GUILayout.Width(180));
		for(int index=0;index<ae.swappers.Length;index++)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField((ae.swappers[index].mesh.name+":"),GUILayout.Width(80));
			ae.swappers[index].currentIndex =EditorGUILayout.IntSlider (ae.swappers[index].currentIndex,0,ae.swappers[index].names.Length,GUILayout.Width(170));
			EditorGUILayout.EndHorizontal();
			if(ae.swappers[index].currentIndex != ae.swappers[index].oldIndex) ae.swappers[index].Update();
		}

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(("hair model:"),GUILayout.Width(80));
		ae.currentHairModel =EditorGUILayout.IntSlider (ae.currentHairModel,0,ae.hairModels.Count-1,GUILayout.Width(170));
		EditorGUILayout.EndHorizontal();
		if(ae.currentHairModel != ae.oldHairModel)
		{
			ae.hairModels[ae.oldHairModel].enabled = false;
			ae.hairModels[ae.currentHairModel].enabled = true;
			ae.oldHairModel = ae.currentHairModel;
		}

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(("skincolor"),GUILayout.Width(80));
		ae.skincolors.skinColorCurrentIndex =EditorGUILayout.IntSlider (ae.skincolors.skinColorCurrentIndex,0,ae.skincolors.skinColors.Length-1,GUILayout.Width(170));
		EditorGUILayout.EndHorizontal();
		if(ae.skincolors.skinColorCurrentIndex != ae.skincolors.skinColorOldIndex)
		{
			ae.skincolors.skinColorOldIndex = ae.skincolors.skinColorCurrentIndex;
			ae.skincolors.Update();
		}
	}
}
