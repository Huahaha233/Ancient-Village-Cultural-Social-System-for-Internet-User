using System;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HandleData{
    private int downcount=-1;//下载资源数量
    public int DownCount
    {
        get {return downcount; }
        set { downcount = value;}
    }
    private bool isupload=false;//是否上传完成
    #region 下载
    //sort为类型、resourename为资源名称、filename为资源下载路径
    public void DownLoad()
    {
        downcount = GameMgr.instance.resoures.Count;
        foreach (Resoure resoure in GameMgr.instance.resoures.Values)
        {
            if (resoure.sort != "video")
            {
                //定义_webClient对象
                WebClient _webClient = new WebClient();
                //使用默认的凭据——读取的时候，只需默认凭据就可以
                _webClient.Credentials = CredentialCache.DefaultCredentials;
                //下载的链接地址（文件服务器）
                Uri _uri = new Uri(@"http://121.199.29.232:7789" + resoure.adress);
                _webClient.DownloadFileCompleted += _webClient_DownloadFileCompleted;
                //异步下载到D盘
                _webClient.DownloadFileAsync(_uri, Application.persistentDataPath + resoure.adress);
                //_webClient.Dispose();
            }
        }
    }
    //下载完成事件处理程序
    private void _webClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {
        downcount--;//有一资源下载结束
    }
    #endregion
    #region 上传
    //resourename为资源服务器地址、filename为本地资源上传地址
    public void Upload(string filename,string resourename)
    {
        WebClient myWebClient = new WebClient();
        myWebClient.Credentials = new NetworkCredential("AncientVillageUser", "Avu123456");
        FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);
        Byte[] postArray = br.ReadBytes(Convert.ToInt32(fs.Length));
        Stream postStream = myWebClient.OpenWrite("http://121.199.29.232:7789" + resourename, "PUT");
        if (postStream.CanWrite)
        {
            postStream.Write(postArray, 0, postArray.Length);
        }
        postStream.Close();
        fs.Close();
        myWebClient.Dispose();
    }
    #endregion
    //返回文件保存地址
    public string UploadName(string filename, string resourename)
    {
        string sort = JudgeSort(filename);
        string suffix = JudgeSuffix(filename);//后缀
        return "/data/" + sort + "/" + resourename + suffix;//返回存储位置与名称
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
            case "jpeg":
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
    //判断并返回文件后缀
    private string JudgeSuffix(string path)
    {
        return "."+ path.Split('.')[1];
    }
}
