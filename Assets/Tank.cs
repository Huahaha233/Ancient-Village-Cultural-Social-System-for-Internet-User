using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Visiter : MonoBehaviour
{
    //脚步声音源
    public AudioSource footAudioSource;
    //脚步音效
    public AudioClip footClip;
    
    //网络同步
    private float lastSendInfoTime = float.MinValue;

    //操控类型
    public enum CtrlType
    {
        none,
        player,
        net,
    }
    public CtrlType ctrlType = CtrlType.player;

    //最大生命值
    private float maxHp = 100;
    //当前生命值
    public float hp = 100;
    
    //生命指示条素材
    public Texture2D hpBarBg;
    public Texture2D hpBar;

    //last 上次的位置信息
    Vector3 lPos;
    Vector3 lRot;
    //forecast 预测的位置信息
    Vector3 fPos;
    Vector3 fRot;
    //时间间隔
    float delta = 1;
    //上次接收的时间
    float lastRecvInfoTime = float.MinValue;

    //位置预测
    public void NetForecastInfo(Vector3 nPos, Vector3 nRot)
    {
        //预测的位置
        fPos = lPos + (nPos - lPos) * 2;
        fRot = lRot + (nRot - lRot) * 2;
        if (Time.time - lastRecvInfoTime > 0.3f)
        {
            fPos = nPos;
            fRot = nRot;
        }
        //时间
        delta = Time.time - lastRecvInfoTime;
        //更新
        lPos = nPos;
        lRot = nRot;
        lastRecvInfoTime = Time.time;
    }

    //初始化位置预测数据
    public void InitNetCtrl()
    {
        lPos = transform.position;
        lRot = transform.eulerAngles;
        fPos = transform.position;
        fRot = transform.eulerAngles;
        Rigidbody r = GetComponent<Rigidbody>();
        r.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void NetUpdate()
    {
        //当前位置
        Vector3 pos = transform.position;
        Vector3 rot = transform.eulerAngles;
        //更新位置
        if (delta > 0)
        {
            transform.position = Vector3.Lerp(pos, fPos, delta);
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(rot),
                                              Quaternion.Euler(fRot), delta);
        }
       
        //脚步音效
        NetWheelsRotation();
    }

    public void NetWheelsRotation()
    {
        float z = transform.InverseTransformPoint(fPos).z;
        //判断浏览者是否在移动
        if (Mathf.Abs(z) < 0.1f || delta <= 0.05f)
        {
            footAudioSource.Pause();
            return;
        }
       
        //声音
        if (!footAudioSource.isPlaying)
        {
            footAudioSource.loop = true;
            footAudioSource.clip = footClip;
            footAudioSource.Play();
        }
    }

    //玩家控制
    public void PlayerCtrl()
    {
        //只有玩家操控的塔克才会生效
        if (ctrlType != CtrlType.player)
            return;
 
        //网络同步
        if (Time.time - lastSendInfoTime > 0.2f)
        {
            SendUnitInfo();
            lastSendInfoTime = Time.time;
        }
    }
 
    //开始时执行
    void Start()
    {
        //马达音源
        footAudioSource = gameObject.AddComponent<AudioSource>();
        footAudioSource.spatialBlend = 1;
    }

    //每帧执行一次
    void Update()
    {
        //网络同步
        if (ctrlType == CtrlType.net)
        {
            NetUpdate();
            return;
        }
        //操控
        PlayerCtrl();
        //马达音效
        MotorSound();
    }
    

    //马达音效
    void MotorSound()
    {
        if (!footAudioSource.isPlaying)
        {
            footAudioSource.loop = true;
            footAudioSource.clip = footClip;
            footAudioSource.Play();
        }
        else
        {
            footAudioSource.Pause();
        }
    }
    
    //绘制ID条
    public void DrawHp()
    {
        //底框
        Rect bgRect = new Rect(30, Screen.height - hpBarBg.height - 15,
                                 hpBarBg.width, hpBarBg.height);
        GUI.DrawTexture(bgRect, hpBarBg);
        //指示条
        float width = hp * 102 / maxHp;
        Rect hpRect = new Rect(bgRect.x + 29, bgRect.y + 9, width, hpBar.height);
        GUI.DrawTexture(hpRect, hpBar);
        //文字
        string text = Mathf.Ceil(hp).ToString() + "/" + Mathf.Ceil(maxHp).ToString();
        Rect textRect = new Rect(bgRect.x + 80, bgRect.y -10, 50, 50);
        GUI.Label(textRect, text);
    }
    
    //绘图
    void OnGUI()
    {
        if (ctrlType != CtrlType.player)
            return;
        DrawHp();
    }
    
    public void SendUnitInfo()
    {
        ProtocolBytes proto = new ProtocolBytes();
        proto.AddString("UpdateUnitInfo");
        //位置旋转
        Vector3 pos = transform.position;
        Vector3 rot = transform.eulerAngles;
        proto.AddFloat(pos.x);
        proto.AddFloat(pos.y);
        proto.AddFloat(pos.z);
        proto.AddFloat(rot.x);
        proto.AddFloat(rot.y);
        proto.AddFloat(rot.z);
        NetMgr.srvConn.Send(proto);
    }
}