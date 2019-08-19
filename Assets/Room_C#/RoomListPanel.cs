using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;
using NGUI;
using UnityEngine.SceneManagement;

public class RoomListPanel:MonoBehaviour
{
    private Text idText;//客户端用户ID
    private GameObject content;//挂载房间列表的Grid
    private GameObject roomPrefab;//房间单元UI的预制体
    void Start()
    {
        OnShowing();
    }
    #region 生命周期
    public void OnShowing()
    {
        //监听
        NetMgr.srvConn.msgDist.AddListener("GetAchieve", RecvGetAchieve);
        NetMgr.srvConn.msgDist.AddListener("GetRoomList", RecvGetRoomList);

        //发送查询
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("GetRoomList");
        NetMgr.srvConn.Send(protocol);

        protocol = new ProtocolBytes();
        protocol.AddString("GetAchieve");
        NetMgr.srvConn.Send(protocol);
    }

    public void OnClosing()
    {
        NetMgr.srvConn.msgDist.DelListener("GetAchieve", RecvGetAchieve);
        NetMgr.srvConn.msgDist.DelListener("GetRoomList", RecvGetRoomList);
    }

    #endregion


    //收到GetAchieve协议
    public void RecvGetAchieve(ProtocolBase protocol)
    {
        //解析协议
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int picturecount = proto.GetInt(start, ref start);
        int videocount= proto.GetInt(start, ref start);
        int modelcount= proto.GetInt(start, ref start);
        //处理
        idText.text = "用户ID：" + GameMgr.instance.id;
    }


    //收到GetRoomList协议
    public void RecvGetRoomList(ProtocolBase protocol)
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
            GenerateRoomUnit(i,name, num,author);
        }
    }

    //清理房间列表
    public void ClearRoomUnit()
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
    public void GenerateRoomUnit(int i,string name,int num,string author)
    {
        //添加房间单元
        GameObject instance = NGUITools.AddChild(content, roomPrefab);
        //房间信息
        Transform trans = instance.transform;
        trans.GetComponent<UILabel>().text=(i + 1).ToString();//房间ID
        trans.GetChild(0).GetComponent<UILabel>().text=name;//房间名称
        trans.GetChild(1).GetComponent<UILabel>().text=num.ToString()+"/10";//房间人数
        trans.GetChild(3).GetComponent<UILabel>().text=author;//房间作者
    }


    //刷新按钮
    public void OnReflashClick()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("GetRoomList");
        NetMgr.srvConn.Send(protocol);
    }

    //加入按钮
    public void OnJoinBtnClick()
    {
        GameObject buttonself = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;//当前点击的按钮的属性
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("EnterRoom");
        protocol.AddInt(int.Parse(buttonself.transform.parent.GetComponent<UILabel>().text));
        NetMgr.srvConn.Send(protocol, OnJoinBtnBack);
        Debug.Log("请求进入房间 " + buttonself.transform.parent.GetComponent<UILabel>().text);
    }

    //加入按钮返回
    public void OnJoinBtnBack(ProtocolBase protocol)
    {
        //解析参数
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int ret= proto.GetInt(start, ref start);
        //处理
        if (ret == 0)
        {
            SceneManager.LoadScene("");//进入到展厅
        }
        else
        {
            Debug.Log("加入房间失败！");
        }
    }

    //新建按钮
    public void OnNewClick()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("CreateRoom");
        NetMgr.srvConn.Send(protocol, OnNewBack);
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
            
        }
        else
        {
            Debug.Log("创建房间失败！");
        }
    }

    //登出按钮
    public void OnCloseClick()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("Logout");
        NetMgr.srvConn.Send(protocol, OnCloseBack);
    }

    //登出返回
    public void OnCloseBack(ProtocolBase protocol)
    {
        OnClosing();//关闭监听
        NetMgr.srvConn.Close();
    }
}