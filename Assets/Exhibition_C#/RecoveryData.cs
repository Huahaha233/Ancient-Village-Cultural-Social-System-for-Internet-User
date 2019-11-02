using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriLib.Samples;

public class RecoveryData{
    //在场景中还原下载到本地文件夹的数据
    public void RecoveryPicture(GameObject AllPicture,Resoure resoure,int index)
    {
        WWW w= new WWW(Application.persistentDataPath+resoure.adress);
        AllPicture.transform.GetChild(index).GetChild(0).GetComponent<Renderer>().material.mainTexture = w.texture;
        AllPicture.transform.GetChild(index).GetChild(1).GetComponent<TextMesh>().text=resoure.name+"\n"+resoure.ins;
    }
    public void RecoveryModel(GameObject AllModel, Resoure resoure, int index)
    {
        IAssetLoaderWindow assetLoaderWindow = new AssetLoaderWindow();
        assetLoaderWindow.RootGameObjectParent = AllModel.transform.GetChild(index).gameObject;
        assetLoaderWindow.LoadInternal(Application.persistentDataPath+resoure.adress, null);
        AllModel.transform.GetChild(index).GetChild(0).GetComponent<TextMesh>().text = resoure.name + "\n" + resoure.ins;
    }
}
