﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NGUI;

public class UIMove : MonoBehaviour
{
    //在开始界面，当用户点击“登录”“游客登录”按钮时，控制NGUI按钮组件移动，并激活登录UI

    ////下方为将需要移动的UI的名称存入数组，方便取用
    //string[] Str_Start = new string[] { "Start_Title", "Start_Login", "Start_NoLogin", "Start_Out" };
    //string[] Str_Login = new string[] { "Login_Title", "Login_Login","Login_Back", "Login_UserID", "Login_UserPSW", "Login_Forget", "Login_Register" ,"Login_Code","Login_Tips"};
    //string[] Str_Register = new string[] { "Register_Title", "Register_Register", "Register_Back", "Register_UserID", "Register_UserPSW", "Register_ReUserPSW", "Register_Sex", "Register_Adress", "Register_Question", "Register_Answer", "Register_Phone","Register_Code" ,"Register_Tips"};
    //string[] Str_Forget = new string[] { };
    //string[] Str_Reset = new string[] { };

    private void Start()
    {
        PlayContent("Start", 0);//开始界面时的NGUI的移动
    }

    //用户在开始界面点击登录按钮
    public void Start_Login()
    {
        PlayContent("Start", 1);//在开始界面内，点击按钮后，开始界面的按钮和标题就必须回到显示方框之外
        PlayContent("Login", 0);//登录界面按钮动画播放
    }

    //用户在开始界面选择游客登陆模式
    public void Start_NoLogin()
    {
        PlayContent("Start", 1);//在开始界面内，点击按钮后，开始界面的按钮和标题就必须回到显示方框之外
    }

    //用户在开始界面选择退出按钮
    public void Start_Out()
    {
        Application.Quit();
    }

    //用户在登录界面选择登录按钮
    public void Login_Login()
    {
        PlayContent("Login", 1);
    }

    //用户在登录界面选择返回按钮
    public void Login_Back()
    {
        PlayContent("Login", 1);
        PlayContent("Start", 0);
    }

    //用户在登录界面选择注册按钮
    public void Login_Register()
    {
        PlayContent("Login", 1);
        PlayContent("Register", 0);
    }

    //用户在登录界面选择忘记密码按钮
    public void Login_Forget()
    {
        PlayContent("Login", 1);
        PlayContent("Forget", 0);
    }

    //用户在注册界面选择注册按钮
    public void Register_Register()
    {
        PlayContent("Register", 1);
    }

    //用户在注册界面选择返回按钮
    public void Register_Back()
    {
        PlayContent("Register", 1);
        PlayContent("Login", 0);
    }

    //用户在忘记密码界面选择下一步按钮
    public void Forget_Next()
    {
        PlayContent("Forget", 1);
        PlayContent("Reset", 0);
    }

    //用户在忘记密码界面选择返回按钮
    public void Forget_Back()
    {
        PlayContent("Forget", 1);
        PlayContent("Login", 0);
    }

    //用户在重置密码界面选择完成按钮
    public void Reset_OK()
    {
        PlayContent("Reset", 1);
    }

    //用户在重置密码界面选择返回按钮
    public void Reset_Back()
    {
        PlayContent("Reset", 1);
        PlayContent("Forget", 0);
    }

    //调用接口
    private void PlayContent(string str, int p)//传入需要移动的UI动画的tag，常量p表示UI的移动顺序，0表示正向播放，1表示反向播放
    {
        INGUIMove ngui = new NGUIMove();
        ngui.Name(str, p);
    }
}




