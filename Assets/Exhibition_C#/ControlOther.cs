using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlOther : MonoBehaviour
{
    //系统接受到其他用户位置信息后控制其动作
    public GameObject People;//其他角色预制体
    void Update()
    {
        switch (Visiter.sports)
        {
            case Visiter.Sports.idle:
                SetBool("idle");
                break;
            case Visiter.Sports.run:
                SetBool("run");
                break;
            case Visiter.Sports.walk:
                SetBool("walk");
                break;
        }
    }
    private void SetBool(string str)
    {
        bool idlebool, walkbool, runbool;
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
