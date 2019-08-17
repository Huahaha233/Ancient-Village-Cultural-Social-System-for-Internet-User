using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NGUI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GetUIButton : MonoBehaviour {
    #region 获取UI的值
    // Use this for initialization
    public GameObject Login_UserID;
    public GameObject Login_UserPSW;
    public GameObject Login_Code;
    public GameObject Tips;
    public GameObject Register_UserID;
    public GameObject Register_UserPSW;
    public GameObject Register_ReUserPSW;
    public GameObject Register_Sex;
    public GameObject Register_Adress;
    public GameObject Register_Question;
    public GameObject Register_Answer;
    public GameObject Register_Phone;
    public GameObject Register_Code;
    public GameObject SendForget_UserID;
    public GameObject SendForget_Code;
    public GameObject Forget_UserID;
    public GameObject Forget_Question;
    public GameObject Forget_Answer;
    public GameObject Forget_Code;
    public GameObject Reset_UserPSW;
    public GameObject Reset_ReUserPSW;
    public GameObject Reset_Code;
    #endregion
    private string Code_Str;//验证码字符串
    private string Answer = null;//密保问题的答案
    private void Start()
    {
        //在开始界面，当用户点击“登录”“游客登录”按钮时，控制NGUI按钮组件移动，并激活登录UI
        PlayContent("Start", 0);//开始界面时的NGUI的移动
        SetCode();
    }
    #region 验证码
    public void SetCode()
    {
        VerificationCode vCode = new VerificationCode(300, 100, 4);
        Texture2D text2D = VerificationCode.Image2Texture(vCode.Image);
        GameObject[] texts=GameObject.FindGameObjectsWithTag("Code");
        foreach(GameObject texture in texts)
        {
            texture.GetComponent<UITexture>().mainTexture=text2D;
        }
        Code_Str = vCode.Text;
    }
    #endregion
    #region 登录
    //登录按钮
    public void OnLoginClick()
    {
        //用户名密码为空
        if (Login_UserID.transform.GetChild(0).GetComponent<UIInput>().value == "" || Login_UserPSW.transform.GetChild(0).GetComponent<UIInput>().value == "")
        {
            Tips.GetComponent<UILabel>().text = "用户名密码不能为空!";
            Debug.Log("用户名密码不能为空!");
            return;
        }
        //判断验证码输入是否正确
        if (Code_Str != Login_Code.transform.GetChild(0).GetComponent<UIInput>().value)
        {
            Tips.GetComponent<UILabel>().text = "验证码错误!";
            SetCode();
            return;
        }
        if (NetMgr.srvConn.status != Connection.Status.Connected)
        {
            string host = "127.0.0.1";
            int port = 1234;
            NetMgr.srvConn.proto = new ProtocolBytes();
            NetMgr.srvConn.Connect(host, port);
        }
        GameMgr.instance.id = Login_UserID.transform.GetChild(0).GetComponent<UIInput>().value;//将用户ID保存至GameMgr
        //发送
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("Login");
        protocol.AddString(Login_UserID.transform.GetChild(0).GetComponent<UIInput>().value);
        protocol.AddString(Login_UserPSW.transform.GetChild(0).GetComponent<UIInput>().value);
        Debug.Log("发送 " + protocol.GetDesc());
        NetMgr.srvConn.Send(protocol, OnLoginBack);
    }
    //处理发送登录信息后服务器返回的信息
    private void OnLoginBack(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int ret = proto.GetInt(start, ref start);
        if (ret == 0)
        {
            Tips.GetComponent<UILabel>().text = "登录成功!";
            Debug.Log("登录成功!");
            Login_Login();
            SceneManager.LoadScene("Play");
        }
        else
        {
            Tips.GetComponent<UILabel>().text = "登录失败!";
            Debug.Log("登录失败!");
            SetCode();
        }
    }
    #endregion

    #region 注册
    //注册按钮
    public void OnRegClick()
    {
        //用户名密码为空
        if (Register_UserID.transform.GetChild(0).GetComponent<UIInput>().value == "" || Register_UserPSW.transform.GetChild(0).GetComponent<UIInput>().value == ""
            || Register_ReUserPSW.transform.GetChild(0).GetComponent<UIInput>().value == "" || Register_Adress.transform.GetChild(0).GetComponent<UILabel>().text == ""
            || Register_Question.transform.GetChild(0).GetComponent<UILabel>().text == ""|| Register_Answer.transform.GetChild(0).GetComponent<UIInput>().value == "" 
            || Register_Phone.transform.GetChild(0).GetComponent<UIInput>().value == "")
        {
            Tips.GetComponent<UILabel>().text = "注册信息未填完！";
            Debug.Log("用户名密码不能为空!");
            return;
        }
        //两次输入的密码不一致
        if (Register_UserPSW.transform.GetChild(0).GetComponent<UIInput>().value != Register_ReUserPSW.transform.GetChild(0).GetComponent<UIInput>().value)
        {
            Tips.GetComponent<UILabel>().text = "密码不一致！";
            Debug.Log("密码不一致!");
            return;
        }
        //判断验证码输入是否正确
        if (Code_Str != Register_Code.transform.GetChild(0).GetComponent<UIInput>().value)
        {
            Tips.GetComponent<UILabel>().text = "验证码错误!";
            SetCode();
        }
        if (NetMgr.srvConn.status != Connection.Status.Connected)
        {
            string host = "127.0.0.1";
            int port = 1234;
            NetMgr.srvConn.proto = new ProtocolBytes();
            NetMgr.srvConn.Connect(host, port);
        }
        //发送
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("Register");
        protocol.AddString(Register_UserID.transform.GetChild(0).GetComponent<UIInput>().value);
        protocol.AddString(Register_UserPSW.transform.GetChild(0).GetComponent<UIInput>().value);
        if (Register_Sex.transform.GetChild(0).GetComponent<UIToggle>().value)
        {
            protocol.AddString("男");
        }else protocol.AddString("女");
        protocol.AddString(Register_Adress.transform.GetChild(0).GetComponent<UILabel>().text);
        protocol.AddString(Register_Question.transform.GetChild(0).GetComponent<UILabel>().text);
        protocol.AddString(Register_Answer.transform.GetChild(0).GetComponent<UIInput>().value);
        protocol.AddString(Register_Phone.transform.GetChild(0).GetComponent<UIInput>().value);

        Debug.Log("发送 " + protocol.GetDesc());
        NetMgr.srvConn.Send(protocol, OnRegBack);
    }

    //处理发送注册信息后服务器返回的信息
    private void OnRegBack(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int ret = proto.GetInt(start, ref start);
        if (ret == 0)
        {
            Tips.GetComponent<UILabel>().text = "注册成功！";
            Debug.Log("注册成功!");
            //注册成功后回到登录界面
            Register_Register();
        }
        else
        {
            Tips.GetComponent<UILabel>().text = "注册失败！";
            Debug.Log("注册失败!");
            SetCode();
        }
    }
    #endregion

    #region 在点击忘记密码按钮时，率先向服务端发送UserID，以获取该用户的密保问题及答案
    //登录按钮
    public void OnSendForgetClick()
    {
        //用户名密码为空
        if (SendForget_UserID.transform.GetChild(0).GetComponent<UIInput>().value == "")
        {
            Tips.GetComponent<UILabel>().text = "用户名不能为空!";
            Debug.Log("用户名不能为空!");
            return;
        }
        //判断验证码输入是否正确
        if (Code_Str != SendForget_Code.transform.GetChild(0).GetComponent<UIInput>().value)
        {
            Tips.GetComponent<UILabel>().text = "验证码错误!";
            SetCode();
            return;
        }

        if (NetMgr.srvConn.status != Connection.Status.Connected)
        {
            string host = "127.0.0.1";
            int port = 1234;
            NetMgr.srvConn.proto = new ProtocolBytes();
            NetMgr.srvConn.Connect(host, port);
        }
        //发送
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("SendForget");
        protocol.AddString(SendForget_UserID.transform.GetChild(0).GetComponent<UILabel>().text);
        Debug.Log("发送 " + protocol.GetDesc());
        NetMgr.srvConn.Send(protocol, OnSendForgetBack);
    }
    //处理发送登录信息后服务器返回的信息
    private void OnSendForgetBack(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        string back = proto.GetString(start, ref start);
        string[] str = back.Split(';');//前半部分为问题，后半部分为答案1
        if (back != "false")
        {
            Forget_Question.transform.GetChild(0).GetComponent<UILabel>().text = str[0];//在UI上显示密保问题
            Tips.GetComponent<UILabel>().text = "获取成功!";
            Debug.Log("成功!");
            SendForget_Next();
            Answer = str[1];
        }
        else
        {
            Tips.GetComponent<UILabel>().text = "获取失败!";
            Debug.Log("失败!");
            SetCode();
        }
    }

    #endregion

    #region 重置密码
    //重置密码按钮
    public void OnResetClick()
    {
        //用户名密码为空
        if (Reset_UserPSW.transform.GetChild(0).GetComponent<UIInput>().value == "" || Reset_ReUserPSW.transform.GetChild(0).GetComponent<UIInput>().value == "")
        {
            Tips.GetComponent <UILabel>().text = "密码不能为空!";
            Debug.Log("密码不能为空!");
            return;
        }
        //两次输入的密码不一致
        if (Reset_UserPSW.transform.GetChild(0).GetComponent<UIInput>().value != Reset_ReUserPSW.transform.GetChild(0).GetComponent<UIInput>().value)
        {
            Tips.GetComponent<UILabel>().text = "密码不一致!";
            Debug.Log("密码不一致!");
            return;
        }
        //判断验证码输入是否正确
        if (Code_Str != Reset_Code.transform.GetChild(0).GetComponent<UIInput>().value)
        {
            Tips.GetComponent<UILabel>().text = "验证码错误!";
            SetCode();
            return;
        }
        if (NetMgr.srvConn.status != Connection.Status.Connected)
        {
            string host = "127.0.0.1";
            int port = 1234;
            NetMgr.srvConn.proto = new ProtocolBytes();
            NetMgr.srvConn.Connect(host, port);
        }
        //发送
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("Reset");
        protocol.AddString(Reset_UserPSW.transform.GetChild(0).GetComponent<UIInput>().value);
        Debug.Log("发送 " + protocol.GetDesc());
        NetMgr.srvConn.Send(protocol, OnResetBack);
    }
    //处理发送登录信息后服务器返回的信息
    private void OnResetBack(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int ret = proto.GetInt(start, ref start);
        if (ret == 0)
        {
            Tips.GetComponent<UILabel>().text = "密码重置成功!";
            Debug.Log("密码重置成功!");
            //重置成功后回到登录界面
            Reset_OK();
        }
        else
        {
            Tips.GetComponent<UILabel>().text = "密码重置失败!";
            Debug.Log("密码重置失败!");
            SetCode();
        }
    }
    #endregion

    #region UI的移动
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
        SceneManager.LoadScene("");
    }

    //用户在开始界面选择退出按钮
    public void Start_Out()
    {
        Application.Quit();
    }

    //用户在登录界面选择登录按钮
    private void Login_Login()
    {
        PlayContent("Login", 1);
        Tips.GetComponent<UILabel>().text = "";
    }

    //用户在登录界面选择返回按钮
    public void Login_Back()
    {
        PlayContent("Login", 1);
        PlayContent("Start", 0);
        SetCode();
    }

    //用户在登录界面选择注册按钮
    public void Login_Register()
    {
        PlayContent("Login", 1);
        PlayContent("Register", 0);
        SetCode();
    }

    //用户在登录界面选择忘记密码按钮
    public void Login_Forget()
    {
        PlayContent("Login", 1);
        PlayContent("SendForget", 0);
        SetCode();
    }

    //用户在注册界面选择注册按钮
    private void Register_Register()
    {
        PlayContent("Register", 1);
        PlayContent("Start",0);
        Tips.GetComponent<UILabel>().text = "";
        SetCode();
    }

    //用户在注册界面选择返回按钮
    public void Register_Back()
    {
        PlayContent("Register", 1);
        PlayContent("Login", 0);
        SetCode();
    }

    //用户在忘记密码输入ID界面输入ID后选择下一步按钮
    private void SendForget_Next()
    {
        PlayContent("SendForget", 1);
        PlayContent("Forget", 0);
        Tips.GetComponent<UILabel>().text = "";
        SetCode();
    }

    //用户在忘记密码输入ID界面选择返回按钮
    public void SendForget_Back()
    {
        PlayContent("SendForget", 1);
        PlayContent("Login", 0);
        SetCode();
    }

    //用户在忘记密码界面选择下一步按钮
    public void Forget_Next()
    {
        if(Answer== Forget_Answer.transform.GetChild(0).GetComponent<UILabel>().text)
        {
            Tips.GetComponent<UILabel>().text = "答案正确!";
            PlayContent("Forget", 1);
            PlayContent("Reset", 0);
        }
        else Tips.GetComponent<UILabel>().text = "答案错误!";
    }

    //用户在忘记密码界面选择返回按钮
    public void Forget_Back()
    {
        PlayContent("Forget", 1);
        PlayContent("SendForget", 0);
    }

    //用户在重置密码界面选择完成按钮
    private void Reset_OK()
    {
        PlayContent("Reset", 1);
        PlayContent("Login", 0);
        Tips.GetComponent<UILabel>().text = "";
        SetCode();
    }

    //用户在重置密码界面选择返回按钮
    public void Reset_Back()
    {
        PlayContent("Reset", 1);
        PlayContent("Forget", 0);
        SetCode();
    }
    #endregion
    //调用接口
    private void PlayContent(string str, int p)//传入需要移动的UI动画的tag，常量p表示UI的移动顺序，0表示正向播放，1表示反向播放
    {
        INGUIMove ngui = new NGUIMove();
        ngui.Name(str, p);
    }
}
