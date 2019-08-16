using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomList : MonoBehaviour {
    public GameObject prefab;
    public GameObject Grid;
	// Use this for initialization
	void Start () {
        for(int i = 0; i < 6; i++)
        {
            GameObject instance = NGUITools.AddChild(Grid,prefab);
        }
        
	}
	
}
