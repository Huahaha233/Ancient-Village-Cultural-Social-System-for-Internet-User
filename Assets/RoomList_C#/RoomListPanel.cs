﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;
using NGUI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.IO;

public class RoomListPanel : MonoBehaviour
{
    //该脚本控制房间列表协议的收发
    public GameObject Achieve;//客户端用户ID
    public GameObject content;//挂载房间列表的Grid
    public GameObject roomPrefab;//房间单元UI的预制体
    public GameObject Ins;//简介框
    public GameObject WriteInsPlane;//点击新建房间按钮后，弹出填写房间基本信息UI
    private string RoomName;//点击选择的房间名称
    HandleData handledata = new HandleData();
    void Start()
    {
        GetAchieve();
        GetRoomList();
    }
    
    #region 收到用户的个人信息
    //发送GetAchieve协议
    public void GetAchieve()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("GetAchieve");
        NetMgr.srvConn.Send(protocol, GetAchieveBack);
    }
    //收到GetAchieve协议
    public void GetAchieveBack(ProtocolBase protocol)
    {
        //解析协议
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int picturecount = proto.GetInt(start, ref start);
        int videocount = proto.GetInt(start, ref start);
        int modelcount = proto.GetInt(start, ref start);
        //处理
        Achieve.transform.GetComponent<UILabel>().text += GameMgr.instance.id;
        Achieve.transform.GetChild(0).GetComponent<UILabel>().text += picturecount;
        Achieve.transform.GetChild(1).GetComponent<UILabel>().text += videocount;
        Achieve.transform.GetChild(2).GetComponent<UILabel>().text += modelcount;
    }
    #endregion

    #region 接收房间列表与生成房间列表
    //发送GetRoomList协议
    public void GetRoomList()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("GetRoomList");
        NetMgr.srvConn.Send(protocol, GetRoomListBack);
    }
    //房间列表及信息
    //收到GetRoomList协议
    public void GetRoomListBack(ProtocolBase protocol)
    {
        //清理
        ClearRoomUnit();
        //解析协议
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int count = proto.GetInt(start, ref start);//房间数
        for (int i = 0; i < count; i++)
        {
            string name = proto.GetString(start, ref start);
            int num = proto.GetInt(start, ref start);//房间中的人数
            string author = proto.GetString(start, ref start);
            GenerateRoomUnit(i, name, num, author);
        }
    }

    //清理房间列表
    private void ClearRoomUnit()
    {
        for (int i = 0; i < content.transform.childCount; i++)
        {
            if (content.transform.GetChild(i).name.Contains("RoomList"))
                Destroy(content.transform.GetChild(i).gameObject);
        }
    }


    //创建一个房间单元
    //参数 i，房间序号（从0开始）
    //参数num，房间里的玩家数
    //参数status，房间状态，1-准备中 2-战斗中
    private void GenerateRoomUnit(int i, string name, int num, string author)
    {
        //添加房间单元
        GameObject instance = NGUITools.AddChild(content, roomPrefab);
        //房间信息
        Transform trans = instance.transform;
        trans.GetComponent<UILabel>().text = (i + 1).ToString();//房间ID
        trans.GetChild(0).GetComponent<UILabel>().text = name;//房间名称
        trans.GetChild(1).GetComponent<UILabel>().text = num.ToString() + "/10";//房间人数
        trans.GetChild(2).GetComponent<UILabel>().text = author;//房间作者
        //在房间列表单元中添加脚本
        trans.GetChild(3).GetComponent<UIButton>().onClick.Add(new EventDelegate(OnGetRoomInfoClick));
    }
    #endregion

    #region 点击获取当前选择的房间信息
    public void OnGetRoomInfoClick()
    {
        GameObject buttonself = UICamera.currentTouch.current;//当前点击的按钮的属性//当前点击的按钮的属性
        RoomName = buttonself.transform.parent.GetChild(0).GetComponent<UILabel>().text;
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("GetRoomInfo");
        protocol.AddString(RoomName);//当前房间的名称
        NetMgr.srvConn.Send(protocol, OnGetRoomInfoBack);
    }
    public void OnGetRoomInfoBack(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        string ins = proto.GetString(start, ref start);
        //将房间简介Ins放入简介框中
        Ins.transform.GetChild(2).GetComponent<UILabel>().text = ins;
        Ins.transform.GetComponent<TweenScale>().PlayForward();
    }
    //关闭简介界面
    public void CloseIns()
    {
        Ins.transform.GetComponent<TweenScale>().PlayReverse();
    }
    #endregion

    //刷新按钮
    public void OnReflashClick()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("GetRoomList");
        NetMgr.srvConn.Send(protocol,GetRoomListBack);
    }

    #region 加入按钮
    public void OnJoinBtnClick()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("EnterRoom");
        protocol.AddString(RoomName);
        NetMgr.srvConn.Send(protocol, OnJoinBtnBack);
        Debug.Log("请求进入房间 " + RoomName);
    }

    //加入按钮返回
    public void OnJoinBtnBack(ProtocolBase protocol)
    {
        //解析参数
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int ret = proto.GetInt(start, ref start);
        //处理
        if (ret == 0)
        {
            GetResoureClick();//下载资源
        }
        else
        {
            Debug.Log("加入房间失败！");
        }
    }
    #endregion

    #region 新建房间
    //新建房间前的房间信息填写
    public void OpenWriteInsPlane()
    {
        WriteInsPlane.transform.GetComponent<TweenPosition>().PlayForward();
    }
    //新建按钮
    public void OnNewClick()
    {
        WriteInsPlane.transform.GetChild(4).GetComponent<UILabel>().text = "";
        if (WriteInsPlane.transform.GetChild(1).transform.GetChild(1).GetComponent<UILabel>().text != null
            || WriteInsPlane.transform.GetChild(2).transform.GetChild(1).GetComponent<UILabel>().text != null)
        {
            ProtocolBytes protocol = new ProtocolBytes();
            protocol.AddString("CreateRoom");
            protocol.AddString(WriteInsPlane.transform.GetChild(1).transform.GetChild(1).GetComponent<UILabel>().text);
            protocol.AddString(WriteInsPlane.transform.GetChild(2).transform.GetChild(1).GetComponent<UILabel>().text);
            NetMgr.srvConn.Send(protocol, OnNewBack);
        }
        else WriteInsPlane.transform.GetChild(4).GetComponent<UILabel>().text = "填写信息不能为空！！！";
    }

    //新建按钮返回
    public void OnNewBack(ProtocolBase protocol)
    {
        //解析参数
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int ret = proto.GetInt(start, ref start);
        //处理
        if (ret == 0)
        {
            //创建成功,进入上传资源界面
            WriteInsPlane.transform.GetComponent<TweenPosition>().PlayReverse();
            SceneManager.LoadScene("ManageRoom");
        }
        else
        {
            Debug.Log("创建房间失败！");
        }
    }
    //关闭新建房间前的房间信息填写
    public void CloseWriteInsPlane()
    {
        WriteInsPlane.transform.GetComponent<TweenPosition>().PlayReverse();
    }
    #endregion

    #region 判断当前用户是否拥有房间按钮
    public void OnHaveRoomClick()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("HaveRoom");
        NetMgr.srvConn.Send(protocol, OnHaveRoomBack);
    }

    //判断当前用户是否拥有房间按钮返回
    public void OnHaveRoomBack(ProtocolBase protocol)
    {
        //解析参数
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int ret = proto.GetInt(start, ref start);
        //处理
        if (ret == 0)
        {
            //创建成功,进入上传资源界面
            SceneManager.LoadScene("ManageRoom");
        }
        else
        {
            Debug.Log("当前用户未拥有展厅，请先创建！");
        }
    }
    #endregion

    #region 登出按钮
    public void OnCloseClick()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("Logout");
        NetMgr.srvConn.Send(protocol, OnCloseBack);
    }
    //在房间列表界面点击返回到登录界面
    //登出返回
    public void OnCloseBack(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start,ref start);
        int ret = proto.GetInt(start,ref start);
        if (ret == 0)
        {
            NetMgr.srvConn.Close();//断开连接
            SceneManager.LoadScene("Login");//返回到登录界面
        }
    }
    #endregion

    #region 下载房间中的资源
    public void GetResoureClick()
    {
        DelectAll();//删除临时文件
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("GetResoure");
        NetMgr.srvConn.Send(protocol, GetResoureBack);
    }
    public void GetResoureBack(ProtocolBase protocol)
    {
        GameMgr.instance.resoures.Clear();//清空
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int resourecount = proto.GetInt(start, ref start);
        for (int i = 0; i < resourecount; i++)
        {
            Resoure resoure = new Resoure();
            resoure.name= proto.GetString(start, ref start);
            resoure.ins = proto.GetString(start, ref start);
            resoure.sort = proto.GetString(start, ref start);
            resoure.adress = proto.GetString(start, ref start);
            GameMgr.instance.resoures.Add(resoure);
        }
        handledata.DownLoad();
    }
    #endregion

    #region 删除某文件夹下的所有文件
public void DelectAll()
    {
        DelectFile(Application.persistentDataPath + "/data/picture");
        DelectFile(Application.persistentDataPath + "/data/video");
        DelectFile(Application.persistentDataPath + "/data/model");
    }
    private void DelectFile(string srcPath)
    {
        try
        {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
            foreach (FileSystemInfo i in fileinfo)
            {
                if (i is DirectoryInfo)//判断是否文件夹
                {
                    DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                    subdir.Delete(true);//删除子目录和文件
                }
                else
                {
                    File.Delete(i.FullName);//删除指定文件
                }
            }
        }
        catch { }
    }
    #endregion
}