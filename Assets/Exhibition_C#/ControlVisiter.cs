using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ControlVisiter : MonoBehaviour {
    //用户控制游客运动时的动作
    public GameObject Player;//用户
    public GameObject LeaveRoomPlane;//退出房间按钮
    //运动状态，判断第一人称脚本是都激活
    public enum Motion
    {
        Active,
        Stop,
    }
    public static Motion motion = Motion.Active;//初始化
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetInput();
        LeaveRoom();
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (motion == Motion.Active) motion = Motion.Stop;
                else motion = Motion.Active;
        }

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
        Player.transform.GetChild(1).GetComponent<Animator>().SetBool("idle", idlebool);
        Player.transform.GetChild(1).GetComponent<Animator>().SetBool("walk", walkbool);
        Player.transform.GetChild(1).GetComponent<Animator>().SetBool("run", runbool);
    }
    //离开当前房间
    private void LeaveRoom()
    {
        if (motion == Motion.Active)
        {
            Player.GetComponent<FirstPersonController>().enabled = true;
            LeaveRoomPlane.GetComponent<TweenScale>().PlayReverse();
        }
        else
        {
            Player.GetComponent<FirstPersonController>().enabled = false;
            LeaveRoomPlane.GetComponent<TweenScale>().PlayForward();
        }
    }
}
