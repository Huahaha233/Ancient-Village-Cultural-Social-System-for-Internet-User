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
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            //走
            SetBool("walk");
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //跑
                SetBool("run");
            }
        }
        else SetBool("idle");
    }
    private void SetBool(string str)
    {
        bool idlebool,walkbool ,runbool;
        idlebool = false;
        walkbool = false;
        runbool = false;
        switch (str)
        {
            case "idle":
                idlebool = true;
                break;
            case "walk":
                walkbool = true;
                break;
            case "run":
                runbool = true;
                break;
        }
        People.transform.GetComponent<Animator>().SetBool("idle", idlebool);
        People.transform.GetComponent<Animator>().SetBool("walk", walkbool);
        People.transform.GetComponent<Animator>().SetBool("run", runbool);
    }
}
