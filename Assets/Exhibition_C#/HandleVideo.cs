using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NGUI;
using UnityEngine.Video;

public class HandleVideo : MonoBehaviour {
    //用于控制视频播放功能
    public GameObject Name;//显示视频的名称
    public GameObject list;//视频列表
    public GameObject PlayPause;//播放与暂停按钮
    public GameObject VideoShow;//播放视频的物体
    List<string> videos = new List<string>();//存放视频的名称
    bool isplay = false;
	// Use this for initialization
	void Start () {
        CreatList();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            list.GetComponent<UIPopupList>().Show();
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
        
    }
    
    //播放视频
    private void PlayVideo()
    {
        VideoShow.transform.GetComponent<VideoPlayer>().Play();
        PlayPause.transform.GetChild(0).GetComponent<UIButton>().normalSprite = "暂停";
    }
    //暂停视频
    private void PauseVideo()
    {
        VideoShow.transform.GetComponent<VideoPlayer>().Pause();
        PlayPause.transform.GetChild(0).GetComponent<UIButton>().normalSprite = "播放";
    }

    #region 创建视频列表
    //创建视频列表
    private void CreatList()
    {
        videos.Clear();
        list.GetComponent<UIPopupList>().Clear();
        foreach (Resoure resoure in GameMgr.instance.resoures.Values)
        {
            if (resoure.sort == "video")
            {
                videos.Add(resoure.name);
            }
        }
        list.GetComponent<UIPopupList>().items = videos;
    }
    #endregion
    
    //选择播放选中的视频
    private void PlayChooseVideo()
    {
        VideoShow.transform.GetComponent<VideoPlayer>().url = "http://121.199.29.232:7789" + GameMgr.instance.resoures[list.GetComponent<UIPopupList>().value].adress;
    }
}
