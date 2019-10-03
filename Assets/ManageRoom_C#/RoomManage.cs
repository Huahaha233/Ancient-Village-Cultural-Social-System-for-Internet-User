using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManage : MonoBehaviour {
    public GameObject content;//挂载资源列表的Grid
    public GameObject RosourePrefab;//资源单元UI的预制体
    public GameObject RoomName;//显示当前房间名称的UI
    public GameObject UpdateBackground;//添加资源面板
    private string path="";//文件地址
    //初始化
    HandlePicture HandlePicture = new HandlePicture();
    // Use this for initialization
    void Start () {
        OnGetRoomNameList();
    }
    #region 获取房间名称列表；也可用于刷新列表
    public void OnGetRoomNameList()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("GetRoomNameList");
        NetMgr.srvConn.Send(protocol, OnGetRoomNameListBack);
    }

    //房间列表信息返回
    public void OnGetRoomNameListBack(ProtocolBase protocol)
    {
        GameMgr.instance.RoomNameList.Clear();//清空
        //解析参数
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int RoomCount = proto.GetInt(start, ref start);
        for (int i = 0; i < RoomCount; i++)
        {
            string RoomName = proto.GetString(start, ref start);
            GameMgr.instance.RoomNameList.Add(RoomName);
        }
        WriteRoomName(GameMgr.instance.RoomNameList);
    }
    #endregion

    #region 获取房间资源列表；也可用于刷新列表
    public void OnGetResoureList()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("GetResoureList");
        protocol.AddString(RoomName.transform.GetChild(0).GetComponent<UILabel>().text);
        NetMgr.srvConn.Send(protocol, OnGetResoureListBack);
    }

    //房间列表信息返回
    public void OnGetResoureListBack(ProtocolBase protocol)
    {
        GameMgr.instance.resourelist.Clear();//清空字典
        //解析参数
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        string RoomName = proto.GetString(start, ref start);
        int ResoureCount = proto.GetInt(start, ref start);
        if (ResoureCount == -1) return;
        Dictionary<string, string> dic = new Dictionary<string, string>();
        for (int i = 0; i < ResoureCount; i++)
        {
            string ResoureName = proto.GetString(start, ref start);
            string ResoureSort = proto.GetString(start, ref start);
            dic.Add(ResoureName, ResoureSort);
        }
        GameMgr.instance.resourelist.Add(RoomName, dic);
        GenerateRoomUnit(RoomName);
    }
    
    //将所有房间名称写入房间选择下拉列表中
    private void WriteRoomName(List<string> list)
    {
        RoomName.transform.GetComponent<UIPopupList>().items = list;
    }
    #endregion
    
    #region 创建房间资源单元
    //清理房间列表
    private void ClearRoomUnit()
    {
        for (int i = 0; i < content.transform.childCount; i++)
        {
            if (content.transform.GetChild(i).name.Contains("ResoureManage"))
                Destroy(content.transform.GetChild(i).gameObject);
        }
    }

    //创建一个房间单元
    private void GenerateRoomUnit(string name)
    {
        ClearRoomUnit();//清空所有房间单元
        foreach (string key in GameMgr.instance.resourelist[name].Keys)
        {
            //添加房间单元
            GameObject instance = NGUITools.AddChild(content, RosourePrefab);
            //房间信息
            Transform trans = instance.transform;
            trans.GetChild(0).GetComponent<UILabel>().text = key;//名称
            trans.GetChild(1).GetComponent<UILabel>().text = GameMgr.instance.resourelist[name][key];//房间类别
            //在管理列表房间单元中添加删除脚本
            trans.GetChild(2).GetComponent<UIButton>().onClick.Add(new EventDelegate(OnDeleteResoureClick));
        }
    }
    #endregion

    #region 删除房间
    public void OnDeleteRoomClick()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("DeleteRoom");
        protocol.AddString(RoomName.transform.GetChild(0).GetComponent<UILabel>().text);//传入房间名称
        NetMgr.srvConn.Send(protocol, OnDeleteRoomBack);
    }

    //删除房间返回
    public void OnDeleteRoomBack(ProtocolBase protocol)
    {
        string roomname = RoomName.transform.GetChild(0).GetComponent<UILabel>().text;
        //解析参数
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int Ret = proto.GetInt(start, ref start);
        if (Ret == 0)
        {
            GameMgr.instance.RoomNameList.Remove(roomname);
            GameMgr.instance.resourelist.Remove(roomname);
            OnGetRoomNameList();
            Debug.Log("删除房间成功!");
        } 
        else Debug.Log("删除房间失败!");
    }
    #endregion

    #region 删除房间资源
    public void OnDeleteResoureClick()
    {
        GameObject buttonself = UICamera.currentTouch.current;//当前点击的按钮的属性
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("DeleteResoure");
        //传入房间名、资源、类型的名称
        protocol.AddString(RoomName.transform.GetChild(0).GetComponent<UILabel>().text);
        protocol.AddString(buttonself.transform.parent.GetComponent<UILabel>().text);//资源名称
        protocol.AddString(buttonself.transform.parent.GetChild(1).GetComponent<UILabel>().text);//属性
        NetMgr.srvConn.Send(protocol, OnDeleteResoureBack);
    }

    //删除房间资源返回
    public void OnDeleteResoureBack(ProtocolBase protocol)
    {
        //解析参数
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int Ret = proto.GetInt(start, ref start);
        string resourename= proto.GetString(start, ref start);
        if (Ret == 0)
        {
            GameMgr.instance.resourelist.Remove(GameMgr.instance.resourelist[RoomName.transform.GetChild(0).GetComponent<UILabel>().text][resourename]);
            OnGetResoureList();
            Debug.Log("删除成功!");
        } 
        else Debug.Log("删除失败!");
    }
    #endregion

    //选择文件按钮
    public void ChooseFileClick()
    {
       path= HandlePicture.instance.OpenFlie();
    }

    #region 上传资源
    public void OnAddResoure()
    {   // 类型有：图片、视频、3D模型
        if (path == null) return;
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("AddResoure");
        if (RoomName.transform.GetChild(0).GetComponent<UILabel>().text == null) return;
        protocol.AddString(RoomName.transform.GetChild(0).GetComponent<UILabel>().text);//房间名称
        protocol.AddString(UpdateBackground.transform.GetChild(2).GetChild(1).GetComponent<UILabel>().text);//资源名称
        protocol.AddString(UpdateBackground.transform.GetChild(3).GetChild(1).GetComponent<UILabel>().text);//资源介绍
        protocol.AddString(HandlePicture.instance.JudgeSort(path));//类别
        protocol.AddByte(HandlePicture.instance.ChangeByte(path));//数据
        NetMgr.srvConn.Send(protocol, OnAddResoureBack);
    }
    public void OnAddResoureBack(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int ret = proto.GetInt(start, ref start);
        if (ret == 0)
        {
            //添加成功
            OnGetResoureList();//刷新列表
            UpdatePlaneClose();
        }
    }
    #endregion
    //点击上传按钮，弹出上传信息框
    public void UpdatePlaneClick()
    {
        UpdateBackground.transform.GetComponent<TweenScale>().PlayForward();
    }
    //点击上传按钮，弹出上传信息框
    public void UpdatePlaneClose()
    {
        UpdateBackground.transform.GetComponent<TweenScale>().PlayReverse();
    }
    // 返回房间列表界面
    public void Back()
    {
        SceneManager.LoadScene("RoomList");
    }
}
