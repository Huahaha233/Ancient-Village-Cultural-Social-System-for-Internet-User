using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomListFold : MonoBehaviour {
    //控制右侧的展厅列表的缩放与展开
    public GameObject rl;//展厅列表
    public GameObject fold;//展开图标
    private bool isfold = false;//是否为已展开
    public void OnClick()
    {
        if (isfold == false)
        {
            Fold();
            isfold = true;
        } 
        else
        {
            UnFold();
            isfold = false;
        }
    }
    //展开
    private void Fold()
    {
        rl.transform.GetComponent<TweenPosition>().PlayForward();
        fold.transform.GetComponent<UIButton>().normalSprite = "收起";
    }
    //收起
    private void UnFold()
    {
        rl.transform.GetComponent<TweenPosition>().PlayReverse();
        fold.transform.GetComponent<UIButton>().normalSprite = "展开";
    }
}
