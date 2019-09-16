using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

[Serializable]
public class PhysicsCharacterOutfitter : MonoBehaviour 
{
	PhysicsCharacterController cpc;
	int oldWeaponIndex;
	[SerializeField]
	public List<WeaponSlot> weapons;
	
	// Use this for initialization
	void Start () 
	{
		cpc = GetComponentInChildren<PhysicsCharacterController>();
		for(int i = 0;i<weapons.Count;i++)
		{
			for(int model=0;model<weapons[i].models.Count;model++)
			{
				weapons[i].models[model].enabled = false;
			}
		}
		for(int model=0;model<weapons[cpc.WeaponState].models.Count;model++)
		{
			weapons[cpc.WeaponState].models[model].enabled = true;
		}
		oldWeaponIndex=cpc.WeaponState;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(cpc.WeaponState!=oldWeaponIndex)
		{
			for(int model=0;model<weapons[oldWeaponIndex].models.Count;model++)
			{
				weapons[oldWeaponIndex].models[model].enabled = false;
			}
			for(int model=0;model<weapons[cpc.WeaponState].models.Count;model++)
			{
				weapons[cpc.WeaponState].models[model].enabled = true;
			}
			oldWeaponIndex=cpc.WeaponState;
		}
	}
}

