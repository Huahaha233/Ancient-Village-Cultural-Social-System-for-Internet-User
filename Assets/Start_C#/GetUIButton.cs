using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NGUI;
using UnityEngine.UI;

public class GetUIButton : MonoBehaviour {
    #region 获取UI的值
    // Use this for initialization
    public GameObject Login_UserID;
    public GameObject Login_UserPSW;
    public GameObject Login_Code;
    public GameObject Login_Tips;
    public GameObject Register_UserID;
    public GameObject Register_UserPSW;
    public GameObject Register_ReUserPSW;
    public GameObject Register_Sex;
    public GameObject Register_Adress;
    public GameObject Register_Question;
    public GameObject Register_Answer;
    public GameObject Register_Phone;
    public GameObject Register_Code;
    public GameObject Register_Tips;
    public GameObject SendForget_UserID;
    public GameObject SendForget_Tips;
    public GameObject Forget_UserID;
    public GameObject Forget_Question;
    public GameObject Forget_Answer;
    public GameObject Forget_Code;
    public GameObject Forget_Tips;
    public GameObject Reset_UserPSW;
    public GameObject Reset_ReUserPSW;
    public GameObject Reset_Tips;
    public GameObject Reset_Code;
    #endregion

    #region 登录
    //登录按钮
    public void OnLoginClick()
    {
        //用户名密码为空
        if (Login_UserID.GetComponent<Text>().text == "" || Login_UserPSW.GetComponent<Text>().text == "")
        {
            Login_Tips.GetComponent<Text>().text = "用户名密码不能为空!";
            Debug.Log("用户名密码不能为空!");
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
        protocol.AddString("Login");
        protocol.AddString(Login_UserID.GetComponent<Text>().text);
        protocol.AddString(Login_UserPSW.GetComponent<Text>().text);
        Debug.Log("发送 " + protocol.GetDesc());
        NetMgr.srvConn.Send(protocol, OnLoginBack);
    }
    //处理发送登录信息后服务器返回的信息
    public void OnLoginBack(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int ret = proto.GetInt(start, ref start);
        if (ret == 0)
        {
            Login_Tips.GetComponent<Text>().text = "登录成功!";
            Debug.Log("登录成功!");
            //开始游戏
        }
        else
        {
            Login_Tips.GetComponent<Text>().text = "登录失败!";
            Debug.Log("登录失败!");
        }
    }
    #endregion

    #region 注册
    //注册按钮
    public void OnRegClick()
    {
        //用户名密码为空
        if (Register_UserID.GetComponent<Text>().text == "" || Register_UserPSW.GetComponent<Text>().text == ""
            || Register_ReUserPSW.GetComponent<Text>().text == "" || Register_Sex.GetComponent<Text>().text == ""
            || Register_Adress.GetComponent<Text>().text == "" || Register_Question.GetComponent<Text>().text == ""
            || Register_Answer.GetComponent<Text>().text == "" || Register_Phone.GetComponent<Text>().text == "")
        {
            Register_Tips.GetComponent<Text>().text = "注册信息未填完！";
            Debug.Log("用户名密码不能为空!");
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
        protocol.AddString("Register");
        protocol.AddString(Register_UserID.GetComponent<Text>().text);
        protocol.AddString(Register_UserPSW.GetComponent<Text>().text);
        protocol.AddString(Register_Sex.GetComponent<Text>().text);
        protocol.AddString(Register_Adress.GetComponent<Text>().text);
        protocol.AddString(Register_Question.GetComponent<Text>().text);
        protocol.AddString(Register_Answer.GetComponent<Text>().text);
        protocol.AddString(Register_Phone.GetComponent<Text>().text);
        Debug.Log("发送 " + protocol.GetDesc());
        NetMgr.srvConn.Send(protocol, OnRegBack);
    }

    //处理发送注册信息后服务器返回的信息
    public void OnRegBack(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int ret = proto.GetInt(start, ref start);
        if (ret == 0)
        {
            Register_Tips.GetComponent<Text>().text = "注册成功！";
            Debug.Log("注册成功!");
        }
        else
        {
            Register_Tips.GetComponent<Text>().text = "注册失败！";
            Debug.Log("注册失败!");
        }
    }
    #endregion

    #region 在点击忘记密码按钮时，率先向服务端发送UserID，以获取该用户的密保问题及答案
    //登录按钮
    public void OnSendForgetClick()
    {
        //用户名密码为空
        if (SendForget_UserID.GetComponent<Text>().text == "")
        {
            SendForget_Tips.GetComponent<Text>().text = "用户名不能为空!";
            Debug.Log("用户名不能为空!");
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
        protocol.AddString(SendForget_UserID.GetComponent<Text>().text);
        Debug.Log("发送 " + protocol.GetDesc());
        NetMgr.srvConn.Send(protocol, OnSendForgetBack);
    }
    //处理发送登录信息后服务器返回的信息
    public void OnSendForgetBack(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        string que = proto.GetString(start, ref start);
        string ans = proto.GetString(start, ref start);
        if (que == "false")
        {
            Forget_Question.GetComponent<Text>().text=que;//在UI上显示密保问题
            SendForget_Tips.GetComponent<Text>().text = "成功!";
            Debug.Log("成功!");
            //开始游戏
        }
        else
        {
            SendForget_Tips.GetComponent<Text>().text = "失败!";
            Debug.Log("失败!");
        }
    }
    #endregion

    #region 忘记密码
    //登录按钮
    public void OnForgetClick()
    {
        //用户名密码为空
        if (Forget_Answer.GetComponent<Text>().text == "")
        {
            Forget_Tips.GetComponent<Text>().text = "信息不能为空!";
            Debug.Log("信息不能为空!");
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
        protocol.AddString("Login");
        protocol.AddString(Forget_UserID.GetComponent<Text>().text);
        Debug.Log("发送 " + protocol.GetDesc());
        NetMgr.srvConn.Send(protocol, OnForgetBack);
    }
    //处理发送登录信息后服务器返回的信息
    public void OnForgetBack(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int ret = proto.GetInt(start, ref start);
        if (ret == 0)
        {
            Login_Tips.GetComponent<Text>().text = "登录成功!";
            Debug.Log("登录成功!");
            //开始游戏
        }
        else
        {
            Login_Tips.GetComponent<Text>().text = "登录失败!";
            Debug.Log("登录失败!");
        }
    }
    #endregion

    #region 重置密码
    //登录按钮
    public void OnResetClick()
    {
        //用户名密码为空
        if (Reset_UserPSW.GetComponent<Text>().text == "" || Reset_ReUserPSW.GetComponent<Text>().text == "")
        {
            Reset_Tips.GetComponent<Text>().text = "用户密码不能为空!";
            Debug.Log("用户密码不能为空!");
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
        protocol.AddString(Reset_UserPSW.GetComponent<Text>().text);
        protocol.AddString(Reset_ReUserPSW.GetComponent<Text>().text);
        Debug.Log("发送 " + protocol.GetDesc());
        NetMgr.srvConn.Send(protocol, OnResetBack);
    }
    //处理发送登录信息后服务器返回的信息
    public void OnResetBack(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int ret = proto.GetInt(start, ref start);
        if (ret == 0)
        {
            Login_Tips.GetComponent<Text>().text = "密码重置成功!";
            Debug.Log("密码重置成功!");
            //开始游戏
        }
        else
        {
            Login_Tips.GetComponent<Text>().text = "密码重置失败!";
            Debug.Log("密码重置失败!");
        }
    }
    #endregion
}
