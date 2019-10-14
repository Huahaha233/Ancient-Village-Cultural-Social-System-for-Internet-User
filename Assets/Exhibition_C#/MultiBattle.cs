using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MultiBattle : MonoBehaviour
{
    //单例
    public static MultiBattle instance;
    //浏览者预设
    public GameObject Prefabs;
    //姓名条预设体
    public GameObject NamePrefab;
    //图片
    public GameObject AllPicture;
    //模型
    public GameObject AllModel;
    //战场中的所有用户
    public Dictionary<string, Visiter> list = new Dictionary<string, Visiter>();
    RecoveryData recoverydata = new RecoveryData();
    void Start()
    {
        recoverydata.RecoveryModel(AllModel, null, 0);
        //单例模式
        instance = this;
        StartVisit();
        //开启监听
        NetMgr.srvConn.msgDist.AddListener("AddPlayer", RecvAddPlayer);//场景增加人员
        NetMgr.srvConn.msgDist.AddListener("DelPlayer", RecvDelPlayer);//场景删除人员 
        Recovery();
    }
   
    #region 在开始时向服务器发送请求获取房间内其他用户的位置信息
    public void StartVisit()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("StartVisit");
        NetMgr.srvConn.Send(protocol, StartVisitBack);
    }
    //开始浏览
    public void StartVisitBack(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        //解析协议
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        //拜访者总数
        int count = proto.GetInt(start, ref start);
        //每一位拜访者
        for (int i = 0; i < count; i++)
        {
            string id = proto.GetString(start, ref start);
            float posx = proto.GetFloat(start, ref start);
            float posy = proto.GetFloat(start, ref start);
            float posz = proto.GetFloat(start, ref start);
            float rotx = proto.GetFloat(start, ref start);
            float roty = proto.GetFloat(start, ref start);
            float rotz = proto.GetFloat(start, ref start);
            if(id!=GameMgr.instance.id) GenerateVisit(id,new Vector3(posx,posy,posz),new Vector3(rotx,roty,rotz));
            }
        NetMgr.srvConn.msgDist.AddListener ("UpdateUnitInfo", RecvUpdateUnitInfo);
    }
    #endregion

    #region 接受到新加入房间的人的协议
    public void RecvAddPlayer(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start,ref start);
        string id = proto.GetString(start, ref start);
        float posx = proto.GetFloat(start,ref start);
        float posy = proto.GetFloat(start,ref start);
        float posz = proto.GetFloat(start,ref start);
        float rotx = proto.GetFloat(start,ref start);
        float roty = proto.GetFloat(start,ref start);
        float rotz = proto.GetFloat(start,ref start);
        if (id != GameMgr.instance.id) GenerateVisit(id, new Vector3(posx, posy, posz), new Vector3(rotx, roty, rotz));
    }

    public void RecvDelPlayer(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        string id = proto.GetString(start,ref start);
        //删除场景中的人物预制体
        GameObject gameObject = GameObject.Find(id);
        Destroy(gameObject);
    }
    #endregion

    #region 还原房间中的数据
    //还原
    private void Recovery()
    {
        foreach(Resoure resoure in GameMgr.instance.resoures.Values)
        {
            int index = 0;
            switch (resoure.sort)
            {
                case "picture":
                    recoverydata.RecoveryPicture(AllPicture,resoure,index);
                    break;
                case "video":
                    break;
                case "model":
                    recoverydata.RecoveryModel(AllModel, resoure, index);
                    break;
            }
            index++;
        }
        
    }
    #endregion

    #region 退出当前房间
    public void OnLeaveRoomClick()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("LeaveRoom");
        NetMgr.srvConn.Send(protocol, OnLeaveRoomBack);
    }
    public void OnLeaveRoomBack(ProtocolBase protocol)
    {
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int ret = proto.GetInt(start, ref start);
        if (ret == 0)
        {
            //删除存储在本地的所有临时文件
            SceneManager.LoadScene("RoomList");
        }
    }
    #endregion

    //产生浏览者
    public void GenerateVisit(string id,Vector3 pos,Vector3 rot)
    {
        //产生浏览者
        GameObject visitObj = (GameObject)Instantiate(Prefabs);
        visitObj.name = id;
        visitObj.transform.position = pos;
        visitObj.transform.rotation = Quaternion.Euler(rot);
        CreatName(visitObj);
        //列表处理
        Visiter visiter = new Visiter();
        visiter = visitObj.GetComponent<Visiter>();
        list.Add(id, visiter);
        //玩家处理
        if (id == GameMgr.instance.id)
        {
            visiter.ctrlType = Visiter.CtrlType.player;
        }
        else
        {
            visiter.ctrlType = Visiter.CtrlType.net;
            visiter.InitNetCtrl ();  //初始化网络同步
        }
    }
    //生成角色头顶的姓名条
    private void CreatName(GameObject VisitObj)
    {
        GameObject nameobj = (GameObject)Instantiate(NamePrefab);
        nameobj.transform.GetComponent<UILabel>().text = VisitObj.name;
        nameobj.GetComponent<UIFollowTarget>().target = VisitObj.transform.GetChild(0);
        nameobj.GetComponent<UIFollowTarget>().gameCamera = GameObject.Find("Player").transform.GetChild(0).GetComponent<Camera>();
        nameobj.GetComponent<UIFollowTarget>().uiCamera = GameObject.Find("UI Root").transform.GetChild(0).GetComponent<Camera>();
        nameobj.GetComponent<UIFollowTarget>().disableIfInvisible = false;
    }
    //更新其他网络用户的位置
    public void RecvUpdateUnitInfo(ProtocolBase protocol)
    {
        //解析协议
        int start = 0;
        ProtocolBytes proto = (ProtocolBytes)protocol;
        string protoName = proto.GetString(start, ref start);
        string id = proto.GetString(start, ref start);
        Vector3 nPos;
        Vector3 nRot;
        nPos.x = proto.GetFloat(start, ref start);
        nPos.y = proto.GetFloat(start, ref start);
        nPos.z = proto.GetFloat(start, ref start);
        nRot.x = proto.GetFloat(start, ref start);
        nRot.y = proto.GetFloat(start, ref start);
        nRot.z = proto.GetFloat(start, ref start);
        //处理
        Debug.Log("RecvUpdateUnitInfo " + id);
        if (!list.ContainsKey(id))
        {
            Debug.Log("RecvUpdateUnitInfo bt == null ");
            return;
        }
        Visiter visiter = list[id];
        if (id != GameMgr.instance.id) visiter.NetForecastInfo(nPos, nRot);
    }
}



