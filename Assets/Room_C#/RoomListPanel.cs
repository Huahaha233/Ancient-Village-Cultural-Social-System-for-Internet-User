using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;

public class RoomListPanel:MonoBehaviour
{
    private Text idText;
    private Text winText;
    private Text lostText;
    private Transform content;
    private GameObject roomPrefab;
    private Button closeBtn;
    private Button newBtn;
    private Button reflashBtn;

    //#region 生命周期
    ///// <summary> 初始化 </summary>
    //public override void Init(params object[] args)
    //{
    //    base.Init(args);
    //    skinPath = "RoomListPanel";
    //    layer = PanelLayer.Panel;
    //}

    //public override void OnShowing()
    //{
    //    base.OnShowing();
    //    //获取Transform
    //    Transform skinTrans = skin.transform;
    //    Transform listTrans = skinTrans.FindChild("ListImage");
    //    Transform winTrans = skinTrans.FindChild("WinImage");
    //    //获取成绩栏部件
    //    idText = winTrans.FindChild("IDText").GetComponent<Text>();
    //    winText = winTrans.FindChild("WinText").GetComponent<Text>();
    //    lostText = winTrans.FindChild("LostText").GetComponent<Text>();
    //    //获取列表栏部件
    //    Transform scroolRect = listTrans.FindChild("ScrollRect");
    //    content = scroolRect.FindChild("Content");
    //    roomPrefab = content.FindChild("RoomPrefab").gameObject;
    //    roomPrefab.SetActive(false);

    //    closeBtn = listTrans.FindChild("CloseBtn").GetComponent<Button>();
    //    newBtn = listTrans.FindChild("NewBtn").GetComponent<Button>();
    //    reflashBtn = listTrans.FindChild("ReflashBtn").GetComponent<Button>();
    //    //按钮事件
    //    reflashBtn.onClick.AddListener(OnReflashClick);
    //    newBtn.onClick.AddListener(OnNewClick);
    //    closeBtn.onClick.AddListener(OnCloseClick);
    //    //监听
    //    NetMgr.srvConn.msgDist.AddListener("GetAchieve", RecvGetAchieve);
    //    NetMgr.srvConn.msgDist.AddListener("GetRoomList", RecvGetRoomList);

    //    //发送查询
    //    ProtocolBytes protocol = new ProtocolBytes();
    //    protocol.AddString("GetRoomList");
    //    NetMgr.srvConn.Send(protocol);

    //    protocol = new ProtocolBytes();
    //    protocol.AddString("GetAchieve");
    //    NetMgr.srvConn.Send(protocol);
    //}

    //public override void OnClosing()
    //{
    //    NetMgr.srvConn.msgDist.DelListener("GetAchieve", RecvGetAchieve);
    //    NetMgr.srvConn.msgDist.DelListener("GetRoomList", RecvGetRoomList);
    //}

    //#endregion


    //收到GetAchieve协议
    public void RecvGetAchieve(ProtocolBase protocol)
    {
        //解析协议
        ProtocolBytes proto = (ProtocolBytes)protocol;
        int start = 0;
        string protoName = proto.GetString(start, ref start);
        int win = proto.GetInt(start, ref start);
        int lost = proto.GetInt(start, ref start);
        //处理
        idText.text = "指挥官：" + GameMgr.instance.id;
        winText.text = win.ToString();
        lostText.text = lost.ToString();
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
        int count = proto.GetInt(start, ref start);
        for (int i = 0; i < count; i++)
        {
            int num = proto.GetInt(start, ref start);
            int status = proto.GetInt(start, ref start);
            GenerateRoomUnit(i, num, status);
        }
    }

    public void ClearRoomUnit()
    {
        for (int i = 0; i < content.childCount; i++)
            if (content.GetChild(i).name.Contains("Clone"))
                Destroy(content.GetChild(i).gameObject);
    }


    //创建一个房间单元
    //参数 i，房间序号（从0开始）
    //参数num，房间里的玩家数
    //参数status，房间状态，1-准备中 2-战斗中
    public void GenerateRoomUnit(int i, int num, int status)
    {
        //添加房间
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (i + 1) * 110);
        GameObject o = Instantiate(roomPrefab);
        o.transform.SetParent(content);
        o.SetActive(true);
        //房间信息
        Transform trans = o.transform;
        Text nameText = trans.FindChild("nameText").GetComponent<Text>();
        Text countText = trans.FindChild("CountText").GetComponent<Text>();
        Text statusText = trans.FindChild("StatusText").GetComponent<Text>();
        nameText.text = "序号：" + (i + 1).ToString();
        countText.text = "人数：" + num.ToString();
        if (status == 1)
        {
            statusText.color = Color.black;
            statusText.text = "状态：准备中";
        }
        else
        {
            statusText.color = Color.red;
            statusText.text = "状态：开战中";
        }
        //按钮事件
        Button btn = trans.FindChild("JoinButton").GetComponent<Button>();
        btn.name = i.ToString();   //改变按钮的名字，以便给OnJoinBtnClick传参
        btn.onClick.AddListener(delegate()
        {
            OnJoinBtnClick(btn.name);
        }
        );
    }


    //刷新按钮
    public void OnReflashClick()
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("GetRoomList");
        NetMgr.srvConn.Send(protocol);
    }

    //加入按钮
    public void OnJoinBtnClick(string name)
    {
        ProtocolBytes protocol = new ProtocolBytes();
        protocol.AddString("EnterRoom");

        protocol.AddInt(int.Parse(name));
        NetMgr.srvConn.Send(protocol, OnJoinBtnBack);
        Debug.Log("请求进入房间 " + name);
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

        }
        else
        {
            
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

        }
        else
        {
            
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
        NetMgr.srvConn.Close();
    }
}