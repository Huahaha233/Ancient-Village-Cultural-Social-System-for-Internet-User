using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlVisiter : MonoBehaviour {
    //用户控制游客运动时的动作
    public GameObject People;//用户
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetInput();
	}
    //获取按键
    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            //走
            People.transform.GetComponent<Animator>().SetBool("walk",true);
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                //跑
                People.transform.GetComponent<Animator>().SetBool("run", true);
            }
            else People.transform.GetComponent<Animator>().SetBool("run", false);
        }
        else People.transform.GetComponent<Animator>().SetBool("idle", true);
    }
}
