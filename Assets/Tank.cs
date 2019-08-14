using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tank : MonoBehaviour
{
    //炮塔炮管轮子履带
    public Transform turret;
    public Transform gun;
    private Transform wheels;
    private Transform tracks;

    //炮塔炮管目标角度
    private float turretRotTarget = 0;
    private float turretRollTarget = 0;
    
    //轮轴
    //public List<AxleInfo> axleInfos;
    //马力/最大马力
    private float motor = 0;
    public float maxMotorTorque;
    //制动/最大制动
    private float brakeTorque = 0;
    public float maxBrakeTorque = 100;
    //转向角/最大转向角
    private float steering = 0;
    public float maxSteeringAngle;
    
    //马达音源
    public AudioSource motorAudioSource;
    //马达音效
    public AudioClip motorClip;
    
    //网络同步
    private float lastSendInfoTime = float.MinValue;

    //操控类型
    public enum CtrlType
    {
        none,
        player,
        computer,
        net,
    }
    public CtrlType ctrlType = CtrlType.player;

    //最大生命值
    private float maxHp = 100;
    //当前生命值
    public float hp = 100;

    //焚烧特效
    public GameObject destoryEffect;

    //中心准心
    public Texture2D centerSight;
    //坦克准心
    public Texture2D tankSight;

    //生命指示条素材
    public Texture2D hpBarBg;
    public Texture2D hpBar;

    //击杀提示图标
    public Texture2D killUI;
    //击杀图标开始显示的时间
    private float killUIStartTime = float.MinValue;

    //发射炮弹音源
    public AudioSource shootAudioSource;
    //发射音效
    public AudioClip shootClip;

    //人工智能
    //private AI ai;

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
       
        //轮子履带马达音效
        NetWheelsRotation();
    }

    public void NetTurretTarget(float y, float x)
    {
        turretRotTarget = y;
        turretRollTarget = x;
    }

    public void NetWheelsRotation()
    {
        float z = transform.InverseTransformPoint(fPos).z;
        //判断坦克是否在移动
        if (Mathf.Abs(z) < 0.1f || delta <= 0.05f)
        {
            motorAudioSource.Pause();
            return;
        }
        //轮子
        foreach (Transform wheel in wheels)
        {
            wheel.localEulerAngles += new Vector3(360 * z / delta, 0, 0);
        }
        //履带
        float offset = -wheels.GetChild(0).localEulerAngles.x / 90f;
        foreach (Transform track in tracks)
        {
            MeshRenderer mr = track.gameObject.GetComponent<MeshRenderer>();
            if (mr == null) continue;
            Material mtl = mr.material;
            mtl.mainTextureOffset = new Vector2(0, offset);
        }
        //声音
        if (!motorAudioSource.isPlaying)
        {
            motorAudioSource.loop = true;
            motorAudioSource.clip = motorClip;
            motorAudioSource.Play();
        }
    }

    //显示击杀图标
    public void StartDrawKill()
    {
        killUIStartTime = Time.time;
    }

    //玩家控制
    public void PlayerCtrl()
    {
        //只有玩家操控的塔克才会生效
        if (ctrlType != CtrlType.player)
            return;
        //马力和转向角
        motor = maxMotorTorque * Input.GetAxis("Vertical");
        steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        //制动
        //brakeTorque = 0;
        //foreach (AxleInfo axleInfo in axleInfos)
        //{
        //    if (axleInfo.leftWheel.rpm > 5 && motor < 0)  //前进时，按下“下”键
        //        brakeTorque = maxBrakeTorque;
        //    else if (axleInfo.leftWheel.rpm < -5 && motor > 0)  //后退时，按下“上”键
        //        brakeTorque = maxBrakeTorque;
        //    continue;
        //}
        ////网络同步
        //if (Time.time - lastSendInfoTime > 0.2f)
        //{
        //    SendUnitInfo();
        //    lastSendInfoTime = Time.time;
        //}
    }
    
    //无人控制
    public void NoneCtrl()
    {
        if (ctrlType != CtrlType.none)
            return;
        motor = 0;
        steering = 0;
        brakeTorque = maxBrakeTorque / 2;
    }

    //开始时执行
    void Start()
    {
        //获取炮塔
        turret = transform.FindChild("turret");
        //获取炮管
        gun = turret.FindChild("gun");
        //获取轮子
        wheels = transform.FindChild("wheels");
        //获取履带
        tracks = transform.FindChild("tracks");
        //马达音源
        motorAudioSource = gameObject.AddComponent<AudioSource>();
        motorAudioSource.spatialBlend = 1;
        //发射音源
        shootAudioSource = gameObject.AddComponent<AudioSource>();
        shootAudioSource.spatialBlend = 1;

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
        NoneCtrl();
        //遍历车轴
        //foreach (AxleInfo axleInfo in axleInfos)
        //{
        //    //转向
        //    if (axleInfo.steering)
        //    {
        //        axleInfo.leftWheel.steerAngle = steering;
        //        axleInfo.rightWheel.steerAngle = steering;
        //    }
        //    //马力
        //    if (axleInfo.motor)
        //    {
        //        axleInfo.leftWheel.motorTorque = motor;
        //        axleInfo.rightWheel.motorTorque = motor;
        //    }
        //    //制动
        //    if (true)
        //    {
        //        axleInfo.leftWheel.brakeTorque = brakeTorque;
        //        axleInfo.rightWheel.brakeTorque = brakeTorque;
        //    }
        //}

        //马达音效
        MotorSound();
    }
    

    //马达音效
    void MotorSound()
    {
        if (motor != 0 && !motorAudioSource.isPlaying)
        {
            motorAudioSource.loop = true;
            motorAudioSource.clip = motorClip;
            motorAudioSource.Play();
        }
        else if (motor == 0)
        {
            motorAudioSource.Pause();
        }
    }
    
    //绘制生命条
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
        //炮塔
        float angleY = turretRotTarget;
        proto.AddFloat(angleY);
        //炮管
        float angleX = turretRollTarget;
        proto.AddFloat(angleX);
        NetMgr.srvConn.Send(proto);
    }
}