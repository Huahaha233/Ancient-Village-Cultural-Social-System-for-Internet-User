using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiBattle : MonoBehaviour
{
    //单例
    public static MultiBattle instance;
    //坦克预设
    public GameObject[] tankPrefabs;
    //战场中的所有坦克
    public Dictionary<string, BattleTank> list = new Dictionary<string, BattleTank>();

    // Use this for initialization
    void Start()
    {
        //单例模式
        instance = this;
    }

    //获取阵营 0表示错误
    public int GetCamp(GameObject tankObj)
    {
        foreach (BattleTank mt in list.Values)
        {
            if (mt.tank.gameObject == tankObj)
                return mt.camp;
        }
        return 0;
    }

    //是否同一阵营
    public bool IsSameCamp(GameObject tank1, GameObject tank2)
    {
        return GetCamp(tank1) == GetCamp(tank2);
    }

    //清理场景
    public void ClearBattle()
    {
        list.Clear();
        GameObject[] tanks = GameObject.FindGameObjectsWithTag("Tank");
        for (int i = 0; i < tanks.Length; i++)
            Destroy(tanks[i]);
    }

    //开始战斗
    public void StartBattle(ProtocolBytes proto)
    {
        //解析协议
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        if (protoName != "Fight")
            return;
        //坦克总数
        int count = proto.GetInt(start, ref start);
        //清理场景
        ClearBattle();
        //每一辆坦克
        for (int i = 0; i < count; i++)
        {
            string id = proto.GetString(start, ref start);
            int team = proto.GetInt(start, ref start);
            int swopID = proto.GetInt(start, ref start);
            GenerateTank(id, team, swopID);
        }
        NetMgr.srvConn.msgDist.AddListener ("UpdateUnitInfo", RecvUpdateUnitInfo);
        //NetMgr.srvConn.msgDist.AddListener ("Shooting", RecvShooting);
        //NetMgr.srvConn.msgDist.AddListener ("Hit", RecvHit);
        //NetMgr.srvConn.msgDist.AddListener ("Result", RecvResult);
    }


    //产生坦克
    public void GenerateTank(string id, int team, int swopID)
    {
        //获取出生点
        Transform sp = GameObject.Find("SwopPoints").transform;
        Transform swopTrans;
        if (team == 1)
        {
            Transform teamSwop = sp.GetChild(0);
            swopTrans = teamSwop.GetChild(swopID - 1);
        }
        else
        {
            Transform teamSwop = sp.GetChild(1);
            swopTrans = teamSwop.GetChild(swopID - 1);
        }
        if (swopTrans == null)
        {
            Debug.LogError("GenerateTank出生点错误！");
            return;
        }
        //预设
        if (tankPrefabs.Length < 2)
        {
            Debug.LogError("坦克预设数量不够");
            return;
        }
        //产生坦克
        GameObject tankObj = (GameObject)Instantiate(tankPrefabs[team - 1]);
        tankObj.name = id;
        tankObj.transform.position = swopTrans.position;
        tankObj.transform.rotation = swopTrans.rotation;
        //列表处理
        BattleTank bt = new BattleTank();
        bt.tank = tankObj.GetComponent<Tank>();
        bt.camp = team;
        list.Add(id, bt);
        //玩家处理
        if (id == GameMgr.instance.id)
        {
            bt.tank.ctrlType = Tank.CtrlType.player;
            //CameraFollow cf = Camera.main.gameObject.GetComponent<CameraFollow>();
            //GameObject target = bt.tank.gameObject;
            //cf.SetTarget(target);
        }
        else
        {
            bt.tank.ctrlType = Tank.CtrlType.net;
            bt.tank.InitNetCtrl ();  //初始化网络同步
        }
    }


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
        float turretY = proto.GetFloat(start, ref start);
        float gunX = proto.GetFloat(start, ref start);
        //处理
        Debug.Log("RecvUpdateUnitInfo " + id);
        if (!list.ContainsKey(id))
        {
            Debug.Log("RecvUpdateUnitInfo bt == null ");
            return;
        }
        BattleTank bt = list[id];
        if (id == GameMgr.instance.id)
            return;

        bt.tank.NetForecastInfo(nPos, nRot);
        bt.tank.NetTurretTarget(turretY, gunX); //稍后实现
    }
}



