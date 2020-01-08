using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPre : MonoBehaviour {
    //用鼠标控制预制体的旋转，缩放等操作
    private Vector2 MouseMoveDirection;//用于表示移动方向
    private float distance = 0;//鼠标滚轮滚动的距离
    public Camera camera;//摄像机
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        
    }
    private void Control()
    {
        if (Input.GetMouseButton(0))
        {
          MouseMoveDirection = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
          Debug.Log("MouseMoveDirection:" + MouseMoveDirection);
            try
            {
                Rotatate();
            }
            catch { }
        }
        
    }
    //旋转
    private void Rotatate()
    {
        GameObject prefabs = GameObject.Find("prefabs");
        //if(MouseMoveDirection.x>0)
        prefabs.transform.Rotate(MouseMoveDirection.y*Time.deltaTime*200, -MouseMoveDirection.x * Time.deltaTime*200, 0, Space.World);//旋转
    }
    //控制摄像机的移动
    private void ControlCamera()
    {
        distance = Input.GetAxis("Mouse ScrollWheel") * 5;
        camera.transform.position += transform.TransformDirection(new Vector3(0, 0, distance));//向摄像机面向的方向前进，等于transform.forward
       
    }
}
