using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManage : MonoBehaviour {
    public GameObject content;//挂载资源列表的Grid
    public GameObject RosourePrefab;//资源单元UI的预制体
    // Use this for initialization
    void Start () {
        OnGetResoureList();
    }

    #region 获取房间资源列表
    private void OnGetResoureList()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("GetResoureList");
        protocol.AddString(GameMgr.instance.id);//传入用户ID
        NetMgr.srvConn.Send(protocol, OnResoureListBack);
    }

    //管理房间返回
    private void OnResoureListBack(ProtocolBase protocol)
    {
        //解析参数
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int RoomCount = proto.GetInt(start, ref start);
        for (int i = 0; i < RoomCount; i++)
        {
            string RoomName = proto.GetString(start, ref start);
            string RoomSort = proto.GetString(start, ref start);
            GenerateRoomUnit(RoomName,RoomSort);
        }

    }
    //清理房间列表
    private void ClearRoomUnit()
    {
        for (int i = 0; i < content.transform.childCount; i++)
        {
            if (content.transform.GetChild(i).name.Contains("Clone"))
                Destroy(content.transform.GetChild(i).gameObject);
        }
    }
    
    //创建一个房间单元
    //参数 i，房间序号（从0开始）
    //参数num，房间里的玩家数
    //参数status，房间状态，1-准备中 2-战斗中
    private void GenerateRoomUnit(string name, string sort)
    {
        //添加房间单元
        GameObject instance = NGUITools.AddChild(content, RosourePrefab);
        //房间信息
        Transform trans = instance.transform;
        trans.GetChild(0).GetComponent<UILabel>().text = name;//房间名称
        trans.GetChild(1).GetComponent<UILabel>().text = sort;//房间类别
    }

    #endregion

    #region 删除房间
    private void OnDeletRoomClick()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("DeletResoure");
        protocol.AddString(GameMgr.instance.id);//传入用户ID
        //传入房间名称
        NetMgr.srvConn.Send(protocol, OnDeletRoomBack);
    }

    //删除房间返回
    private void OnDeletRoomBack(ProtocolBase protocol)
    {
        //解析参数
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int Ret = proto.GetInt(start, ref start);
        if (Ret == 0) Debug.Log("删除房间成功!");
        else Debug.Log("删除房间失败!");
    }
    #endregion

    #region 删除房间资源
    private void OnDeletResoureClick()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("DeletResoure");
        protocol.AddString(GameMgr.instance.id);//传入用户ID
        //传入资源的名称
        NetMgr.srvConn.Send(protocol, OnDeletResoureBack);
    }

    //删除房间资源返回
    private void OnDeletResoureBack(ProtocolBase protocol)
    {
        //解析参数
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int Ret = proto.GetInt(start, ref start);
        if (Ret == 0) Debug.Log("删除成功!");
        else Debug.Log("删除失败!");
    }
    #endregion
}
