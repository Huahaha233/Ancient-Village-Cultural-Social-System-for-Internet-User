using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NGUI;

public class StartScript : MonoBehaviour {

	 //在开始界面，当用户点击“登录”“游客登录”按钮时，控制NGUI按钮组件移动，并激活登录UI

    private void Start()
    {
        PreStartButton();
    }

    //用户在开始界面点击登录按钮
    public void Login()
    {
        BackStartButton();
        PreLoginButton();
    }

    //用户选择游客登陆模式
    public void NoLogin()
    {
        BackStartButton();
    }


    //开始界面时的NGUI的移动
    private void PreStartButton()
    {
        //PlayContent("Start_Title",0,200,0,110);
        //PlayContent("Start_Login",-855,-40,-120,-40);
        //PlayContent("Start_NoLogin",855,-40,140,-40);
        PlayForward("Start_Title");
        PlayForward("Start_Login");
        PlayForward("Start_NoLogin");
        PlayForward("Start_Out");
    }
    //在开始界面内，点击按钮后，开始界面的按钮和标题就必须回到显示方框之外
    private void BackStartButton()
    {
         //PlayContent("Start_Title", 0,110 , 0, 200);
        //PlayContent("Start_Login", -120, -40,-855 , -40);
        //PlayContent("Start_NoLogin", 140, -40,855 , -40);
        PlayReverse("Start_Title");
        PlayReverse("Start_Login");
        PlayReverse("Start_NoLogin");
        PlayReverse("Start_Out");

    }

    //登录界面按钮动画播放
    private void PreLoginButton()
    {
        PlayForward("Login_Title");
        PlayForward("Login_Login");
        PlayForward("Login_UserID");
        PlayForward("Login_UserPSW");
        PlayForward("Login_Forget");
        PlayForward("Login_Register");
    }
    //登录回放
    private void BackLoginButton()
    {
        PlayReverse("Login_Title");
        PlayReverse("Login_Login");
        PlayReverse("Login_UserID");
        PlayReverse("Login_UserPSW");
        PlayReverse("Login_Forget");
        PlayReverse("Login_Register");
    }

    //private void PreRegisterButton()
    //{
    //    PlayForward();

    //}
    //private void BackRegisterButton()
    //{
    //    PlayReverse();
    //}

    // NGUI动画正向播放
    private void PlayForward(string str)
    {
        transform.Find(str).GetComponent<TweenPosition>().PlayForward();
    }
    //NGUI动画回放
    private void PlayReverse(string str)
    {
        transform.Find(str).GetComponent<TweenPosition>().PlayReverse();
    }

    //调用接口
    //private void PlayContent(string str,float Fx,float Fy, float Tx, float Ty)
    //{
    //    INGUIMove ngui = new NGUIMove();
    //    ngui.Start(transform.Find(str).gameObject);
    //    ngui.From(Fx,Fy);
    //    ngui.To(Tx, Ty);
    //    ngui.Speed(3);
    //}
}

   


