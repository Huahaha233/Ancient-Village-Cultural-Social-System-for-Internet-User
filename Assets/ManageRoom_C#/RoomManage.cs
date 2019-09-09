using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManage : MonoBehaviour {
    public GameObject content;//挂载资源列表的Grid
    public GameObject RosourePrefab;//资源单元UI的预制体
    public GameObject RoomName;//显示当前房间名称的UI
    // Use this for initialization
    void Start () {
        OnGetResoureList();
    }
    #region 获取房间资源列表；也可用于刷新列表
    private void OnGetResoureList()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("GetResoureList");
        NetMgr.srvConn.Send(protocol, OnResoureListBack);
    }

    //房间列表信息返回
    private void OnResoureListBack(ProtocolBase protocol)
    {
        GameMgr.instance.resourelist.Clear();//清空字典
        //解析参数
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int RoomCount = proto.GetInt(start, ref start);
        for (int i = 0; i < RoomCount; i++)
        {
            string RoomName = proto.GetString(start, ref start);
            int ResoureCount = proto.GetInt(start, ref start);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int j = 0; j < ResoureCount; j++)
            {
                string ResoureName = proto.GetString(start, ref start);
                string ResoureSort = proto.GetString(start, ref start);
                dic.Add(ResoureName, ResoureSort);
            }
            GameMgr.instance.RoomNameList.Add(RoomName);
            GameMgr.instance.resourelist.Add(RoomName, dic);
        }
        WriteRoomName(GameMgr.instance.RoomNameList);
        GenerateRoomUnit(GameMgr.instance.RoomNameList[0]);
    }


    //将所有房间名称写入房间选择下拉列表中
    private void WriteRoomName(List<string> list)
    {
        RoomName.transform.GetComponent<UIPopupList>().items = list;
    }
    #endregion

    //改变用户房间的按钮
    public void ChangeRoom()
    {
        GenerateRoomUnit(RoomName.transform.GetChild(0).GetComponent<UILabel>().text);
    }

    #region 创建房间单元
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
    private void OnDeleteRoomBack(ProtocolBase protocol)
    {
        //解析参数
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int Ret = proto.GetInt(start, ref start);
        if (Ret == 0)
        {
            GameMgr.instance.RoomNameList.Remove(RoomName.transform.GetChild(0).GetComponent<UILabel>().text);
            GameMgr.instance.resourelist.Remove(RoomName.transform.GetChild(0).GetComponent<UILabel>().text);
            OnGetResoureList();
            Debug.Log("删除房间成功!");
        } 
        else Debug.Log("删除房间失败!");
    }
    #endregion

    #region 删除房间资源
    public void OnDeleteResoureClick()
    {
        GameObject buttonself = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;//当前点击的按钮的属性
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("DeleteResoure");
        //传入房间名、资源、类型的名称
        protocol.AddString(RoomName.transform.GetChild(0).GetComponent<UILabel>().text);
        protocol.AddString(buttonself.transform.parent.GetComponent<UILabel>().text);//资源名称
        protocol.AddString(buttonself.transform.parent.GetChild(1).GetComponent<UILabel>().text);
        NetMgr.srvConn.Send(protocol, OnDeleteResoureBack);
    }

    //删除房间资源返回
    private void OnDeleteResoureBack(ProtocolBase protocol)
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

    #region 上传资源
    public void OnAddResoure()
    {

    }
    public void OnAddResoureBack()
    {

    }
    #endregion

    // 返回房间列表界面
    public void Back()
    {
        SceneManager.LoadScene("RoomList");
    }
}
