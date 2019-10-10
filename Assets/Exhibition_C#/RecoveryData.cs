using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryData{
    //在场景中还原下载到本地文件夹的数据
    public void RecoveryPicture(GameObject AllPicture,Resoure resoure,int index)
    {
        WWW w= new WWW(Application.persistentDataPath+resoure.adress);
        AllPicture.transform.GetChild(index).GetChild(0).GetComponent<Material>().mainTexture=w.texture;
        AllPicture.transform.GetChild(index).GetChild(1).GetComponent<UILabel>().text=resoure.name+"\n"+resoure.ins;
    }
    public void RecoveryVideo(GameMgr AllVideo, Resoure resoure, int index)
    {

    }
    public void RecoveryModel()
    {

    }
}
