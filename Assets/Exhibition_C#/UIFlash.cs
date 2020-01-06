using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NGUI;

public class UIFlash : MonoBehaviour {
    //展示分类UI刷新
    // Use this for initialization
    public static string sort;//类别
    //家具类
    public void Fumiture()
    {
        sort = "fumiture";
        Close();
        Change("Content1","pfumiture1");
        Change("Content2", "pfumiture2");
        Change("Content3", "pfumiture3");
    }
    //建筑类
    public void Build()
    {
        sort = "build";
        Close();
        Change("Content1", "pbuild1");
    }
    //视频类
    public void Video()
    {
        sort = "video";
        Close();
        Change("Content1", "video1");
    }
    //图片类
    public void Picture()
    {
        sort = "picture";
        Close();
        Change("Content1", "picture1");
        Change("Content2", "picture2");
        Change("Content3", "picture3");
        Change("Content4", "picture4");
    }

    //封装
    public void Change(string str1,string str2)
    {
        transform.Find("ThingMenu/" + str1).GetComponent<UITexture>().mainTexture = Resources.Load<Texture>(str2);
        transform.Find("ThingMenu/" + str1).GetComponent<TweenScale>().PlayForward();
    }
    //关闭所有UI
    public void Close()
    {
        for (int i = 1; i < 5; i++)
        {
            transform.Find("ThingMenu/Content" + i).GetComponent<TweenScale>().PlayReverse();
        }

        //关闭之前的所有UI，防止挡住
        transform.Find("视频播放暂停").GetComponent<TweenScale>().PlayReverse();
        transform.Find("Video").gameObject.SetActive(false);
        transform.Find("Picture").GetComponent<TweenAlpha>().PlayReverse();

    }
}
