using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HandlePicture{
    //上传处理图片
    public static HandlePicture instance;
    public HandlePicture()
    {
        instance = this;
    }
    //打开文件夹，选择发送的文件或图片
    public string OpenFlie()
    {
        string extion = "png,jpg,jpeg,mp3,mp4,obj";
        string path = "";
        path = UnityEditor.EditorUtility.OpenFilePanel("Load Images of Directory", Application.dataPath, extion);
        if (path != null)
        {
            Debug.Log("获取文件路径成功：" + path);
        }
        return path;
    }
    public byte[] ChangeByte(string path)
    {
        FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
        byte[] data = new byte[fs.Length];
        BinaryReader strread = new BinaryReader(fs);
        strread.Read(data, 0, data.Length);
        fs.Close();
        return data;
    }
    //判断文件类型
    public string JudgeSort(string path)
    {
        string end = path.Split('.')[1];
        switch (end)
        {
            case "png":
                return "picture";
            case "jpg":
                return "picture";
            case "jepg":
                return "picture";
            case "mp3":
                return "video";
            case "mp4":
                return "video";
            case "obj":
                return "model";
        }
        return null;
    }
    //还原图片
    public void RecoveryImage(GameObject picture,byte[] imagedata)
    {
        Texture2D tex = new Texture2D(80, 80);
        tex.LoadImage(imagedata);
        picture.GetComponent<MeshRenderer>().material.mainTexture = tex;
    }
}
