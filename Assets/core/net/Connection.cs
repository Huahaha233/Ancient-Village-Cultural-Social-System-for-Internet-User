using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

//网络链接
public class Connection
{
    //常量
    const int BUFFER_SIZE = 1024;//缓冲区大小
    //Socket
    private Socket socket;
    //Buff
    private byte[] readBuff = new byte[BUFFER_SIZE];//读缓冲区
    private int buffCount = 0;//当前读缓冲区的长度
    //沾包分包
    private int msgLength = 0;//消息长度
    private byte[] lenBytes = new byte[sizeof(int)];//转换成byte类型后的消息长度
    //协议
    public ProtocolBase proto;
    //心跳时间
    public float lastTickTime = 0;//上一次心跳的时间
    public float heartBeatTime = 30;//时间间隔
    //消息分发
    public MsgDistribution msgDist = new MsgDistribution();
    //连接状态，有None、Connected两种
    public enum Status
    {
        None,
        Connected,
    };
    public Status status = Status.None;
    
    //连接服务端
    public bool Connect(string host, int port)
    {
        try
        {
            //socket
            socket = new Socket(AddressFamily.InterNetwork,
                      SocketType.Stream, ProtocolType.Tcp);
            //Connect
            socket.Connect(host, port);
            //BeginReceive
            socket.BeginReceive(readBuff, buffCount,
                      BUFFER_SIZE - buffCount, SocketFlags.None,
                      ReceiveCb, readBuff);
            Debug.Log("连接成功");
            //状态
            status = Status.Connected;
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("连接失败:" + e.Message);
            return false;
        }
    }

    //关闭连接
    public bool Close()
    {
        try
        {
            socket.Close();
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("关闭失败:" + e.Message);
            return false;
        }
    }

    //接收回调
    private void ReceiveCb(IAsyncResult ar)
    {
        try
        {
            int count = socket.EndReceive(ar);//处理缓冲区
            buffCount = buffCount + count;
            ProcessData();//处理数据
            socket.BeginReceive(readBuff, buffCount,
                     BUFFER_SIZE - buffCount, SocketFlags.None,
                     ReceiveCb, readBuff);
        }
        catch (Exception e)
        {
            Debug.Log("ReceiveCb失败:" + e.Message);
            status = Status.None;
        }
    }

    //消息处理
    private void ProcessData()
    {
        //小于长度字节
        if (buffCount < sizeof(int))
            return;
        //消息长度
        Array.Copy(readBuff, lenBytes, sizeof(int));
        msgLength = BitConverter.ToInt32(lenBytes, 0);
        if (buffCount < msgLength + sizeof(int))
            return;
        //处理消息
        ProtocolBase protocol = proto.Decode(readBuff, sizeof(int), msgLength);//协议解码
        Debug.Log("收到消息 " + protocol.GetDesc());
        lock (msgDist.msgList)
        {
            msgDist.msgList.Add(protocol);
        }
        //清除已处理的消息
        int count = buffCount - msgLength - sizeof(int);
        Array.Copy(readBuff,sizeof(int) + msgLength, readBuff, 0, count);
        buffCount = count;
        if (buffCount > 0)
        {
            ProcessData();
        }
    }
    
    public bool Send(ProtocolBase protocol)
    {
        if (status != Status.Connected)
        {
            Debug.LogError("[Connection]未连接到服务器！！！");
            return true;
        }

        byte[] b = protocol.Encode();
        byte[] length = BitConverter.GetBytes(b.Length);

        byte[] sendbuff = length.Concat(b).ToArray();
        socket.Send(sendbuff);
        Debug.Log("发送消息 " + protocol.GetDesc());
        return true;
    }

    public bool Send(ProtocolBase protocol, string cbName, MsgDistribution.Delegate cb)
    {
        if (status != Status.Connected)
            return false;
        msgDist.AddOnceListener(cbName, cb);
        return Send(protocol);
    }

    public bool Send(ProtocolBase protocol, MsgDistribution.Delegate cb)
    {
        string cbName = protocol.GetName();
        return Send(protocol, cbName, cb);
    }
    
    public void Update()
    {
        //消息
        msgDist.Update();
        //心跳
        if (status == Status.Connected)
        {
            if (Time.time - lastTickTime > heartBeatTime)
            {
                ProtocolBase protocol = NetMgr.GetHeatBeatProtocol();
                Send(protocol);
                lastTickTime = Time.time;
            }
        }
    }
}