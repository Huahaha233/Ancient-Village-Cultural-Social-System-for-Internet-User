using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//网络管理
public class NetMgr
{
    public static Connection srvConn = new Connection();
    public static void Update()
    {
        srvConn.Update();
    }

    //心跳
    public static ProtocolBase GetHeatBeatProtocol()
    {
        //具体的发送内容根据服务端设定改动,主要处理非正常退出行为
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("HeatBeat");
        return protocol;
    }
}