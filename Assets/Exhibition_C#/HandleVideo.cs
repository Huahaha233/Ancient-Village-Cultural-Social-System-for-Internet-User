using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NGUI;
using UnityEngine.Video;

public class HandleVideo : MonoBehaviour {
    //用于控制视频播放功能
    public GameObject VideoShow;//播放视频的物体
    public GameObject videoprefab;//视频资料预制体
    public GameObject Menu;//菜单
    private GameObject content;//Grid，用于挂载预制体
    private int index = 0;//正在播放视频的序号
    List<string> videos = new List<string>();//存放视频的名称
    bool ismenu = false;
    bool isplay = false;
	// Use this for initialization
	void Start () {
        content = Menu.transform.GetChild(2).gameObject;
        CreatList();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(ismenu==false)
            {
                OpenMenu();
                ismenu = true;
            }
            else
            {
                CloseMenu();
                ismenu = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isplay == false)
            {
                PlayVideo();
                isplay = true;
            }
            else
            {
                PauseVideo();
                isplay = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            PlayChooseVideo();

        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            index--;
            ChooseVideo();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            index++;
            ChooseVideo();
        }
    }

    //打开菜单
    private void OpenMenu()
    {
        Menu.GetComponent<TweenPosition>().PlayForward();
    }
    //关闭菜单
    private void CloseMenu()
    {
        Menu.GetComponent<TweenPosition>().PlayReverse();
    }
    //播放视频
    private void PlayVideo()
    {
        VideoShow.transform.GetComponent<VideoPlayer>().Play();
    }
    //暂停视频
    private void PauseVideo()
    {
        VideoShow.transform.GetComponent<VideoPlayer>().Pause();
    }

    #region 创建视频列表
    //创建视频列表
    private void CreatList()
    {
        ClearRoomUnit();
        videos.Clear();
        foreach (Resoure resoure in GameMgr.instance.resoures.Values)
        {
            if (resoure.sort == "video")
            {
                videos.Add(resoure.name);
                GenerateRoomUnit(resoure.name);
            }
        }
        content.transform.GetChild(0).GetComponent<UISprite>().enabled = true;//将视频列表第一个视频设为默认状态
        Menu.transform.GetChild(0).GetComponent<UILabel>().text = videos[0];
        VideoShow.transform.GetComponent<VideoPlayer>().url = GameMgr.instance.resoures[videos[index]].adress;
    }
    //清理视频列表
    private void ClearRoomUnit()
    {
        for (int i = 0; i < content.transform.childCount; i++)
        {
            if (content.transform.GetChild(i).name.Contains("VideoList"))
                Destroy(content.transform.GetChild(i).gameObject);
        }
    }
    //创建一个房间单元
    //参数 i，房间序号（从0开始）
    //参数num，房间里的玩家数
    //参数status，房间状态，1-准备中 2-战斗中
    private void GenerateRoomUnit(string name)
    {
        //添加房间单元
        GameObject instance = NGUITools.AddChild(content, videoprefab);
        //房间信息
        Transform trans = instance.transform;
        trans.GetChild(0).GetComponent<UILabel>().text = name;//房间名称
    }
    #endregion

    //选择视频
    private void ChooseVideo()
    {
        content.transform.GetChild(index-1).GetComponent<UISprite>().enabled = false;
        content.transform.GetChild(index).GetComponent<UISprite>().enabled = true;
    }

    //选择播放选中的视频
    private void PlayChooseVideo()
    {
        Menu.transform.GetChild(0).GetComponent<UILabel>().text = videos[index];
        VideoShow.transform.GetComponent<VideoPlayer>().url = GameMgr.instance.resoures[videos[index]].adress;
    }
}
