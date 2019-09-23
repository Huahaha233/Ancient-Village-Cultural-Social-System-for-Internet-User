using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiBattle : MonoBehaviour
{
    //单例
    public static MultiBattle instance;
    //浏览者预设
    public GameObject Prefabs;
    //战场中的所有用户
    public Dictionary<string, Visiter> list = new Dictionary<string, Visiter>();

    // Use this for initialization
    void Start()
    {
        //单例模式
        instance = this;
        StartVisit();
    }

    //在开始时向服务器发送请求获取房间内其他用户的位置信息
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
            GenerateTank(id,new Vector3(posx,posy,posz),new Vector3(rotx,roty,rotz));
        }
        NetMgr.srvConn.msgDist.AddListener ("UpdateUnitInfo", RecvUpdateUnitInfo);
    }


    //产生浏览者
    public void GenerateTank(string id,Vector3 pos,Vector3 rot)
    {
        //产生浏览者
        GameObject visitObj = (GameObject)Instantiate(Prefabs);
        visitObj.name = id;
        visitObj.transform.position = pos;
        visitObj.transform.rotation = Quaternion.Euler(rot);
        //列表处理
        Visiter visiter = new Visiter();
        visiter = visitObj.GetComponent<Visiter>();
        list.Add(id, visiter);
        //玩家处理
        if (id == GameMgr.instance.id)
        {
            visiter.ctrlType = Visiter.CtrlType.player;
            //CameraFollow cf = Camera.main.gameObject.GetComponent<CameraFollow>();
            //GameObject target = bt.tank.gameObject;
            //cf.SetTarget(target);
        }
        else
        {
            visiter.ctrlType = Visiter.CtrlType.net;
            visiter.InitNetCtrl ();  //初始化网络同步
        }
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
        //float turretY = proto.GetFloat(start, ref start);
        //float gunX = proto.GetFloat(start, ref start);
        //处理
        Debug.Log("RecvUpdateUnitInfo " + id);
        if (!list.ContainsKey(id))
        {
            Debug.Log("RecvUpdateUnitInfo bt == null ");
            return;
        }
        Visiter visiter = list[id];
        if (id == GameMgr.instance.id)
            return;

        visiter.NetForecastInfo(nPos, nRot);
        //bt.NetTurretTarget(turretY, gunX); //稍后实现
    }
}



