using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickRay : MonoBehaviour {
    //射线，鼠标点击地图图标，返回该地点的名称
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) Ray();
	}
    private void Ray()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast (ray,out hit))
        {
            Debug.Log(hit.transform.parent.GetChild(0).GetComponent<TextMesh>().text);
        }
        else
        {
            Debug.Log("未找到！");
        }
        
    }
    
}
