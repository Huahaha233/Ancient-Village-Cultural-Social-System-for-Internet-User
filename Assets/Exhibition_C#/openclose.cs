using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NGUI;

public class openclose : MonoBehaviour {
    //关闭或打开古村落物品分类展示UI
    // Use this for initialization
    public GameObject UI;//分类UI
    public GameObject Open_button;//开启按钮
    //打开和关闭UI
    public void OpenClose()
    {
        if(Open_button.transform.GetChild(0).GetComponent<UILabel>().text == "开启")
        {
            UI.GetComponent<TweenPosition>().PlayForward();
            Open_button.transform.GetChild(0).GetComponent<UILabel>().text = "关闭";
        }
        else
        {
            UI.GetComponent<TweenPosition>().PlayReverse();
            Open_button.transform.GetChild(0).GetComponent<UILabel>().text = "开启";
        }
    }
    
}
