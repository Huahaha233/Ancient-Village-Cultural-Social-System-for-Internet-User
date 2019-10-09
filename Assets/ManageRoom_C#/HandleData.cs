using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;
using UnityEngine;

public class HandleData{

    #region 下载
    //sort为类型、resourename为资源名称、filename为资源下载路径
    public void DownLoad(string filename, string resoureadress)
    {
        //定义_webClient对象
        WebClient _webClient = new WebClient();
        //使用默认的凭据——读取的时候，只需默认凭据就可以
        _webClient.Credentials = CredentialCache.DefaultCredentials;
        //下载的链接地址（文件服务器）
        Uri _uri = new Uri(@"http://121.199.29.232:7789/data/"+resoureadress);
        //注册下载进度事件通知
        _webClient.DownloadProgressChanged += _webClient_DownloadProgressChanged;
        //注册下载完成事件通知             _webClient.DownloadFileCompleted += _webClient_DownloadFileCompleted;
        //异步下载到D盘
        _webClient.DownloadFile(_uri, filename+ "/data/" + resoureadress);
        //_webClient.Dispose();
    }
 
    //下载完成事件处理程序
    private void _webClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {
        Debug.Log("Download Completed...");
    }
    //下载进度事件处理程序
    private void _webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        Debug.Log(e.BytesReceived/e.TotalBytesToReceive);
    }
    #endregion
    #region 上传
    //sort为类型、resourename为资源名称、filename为资源下载路径
    public string Upload(string filename,string resourename)
    {
        string sort = JudgeSort(filename);
        string suffix = JudgeSuffix(filename);//后缀
        WebClient myWebClient = new WebClient();
        myWebClient.Credentials = new NetworkCredential("AncientVillageUser", "Avu123456");
        FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);
        Byte[] postArray = br.ReadBytes(Convert.ToInt32(fs.Length));
        Stream postStream = myWebClient.OpenWrite(@"http://121.199.29.232:7789/data/" + sort + "/" + resourename+suffix, "PUT");
        if (postStream.CanWrite)
        {
            postStream.Write(postArray, 0, postArray.Length);
        }
        postStream.Close();
        fs.Close();
        myWebClient.Dispose();
        return sort + "/" + resourename+suffix;//返回存储位置与名称
    }
    #endregion
    //打开文件夹，选择发送的文件
    public string OpenFlie()
    {
        string path = "";
        OpenFileDialog dlg = new OpenFileDialog();
        dlg.Filter = "文件(*.png;*.jpg;*.bmp;*.jpeg;*.mp3;*.mp4;*.obj)|*.png;*.jpg;*.bmp;*.jpeg;*.mp3;*.mp4;*.obj";
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            path = dlg.FileName;
            Debug.Log("获取文件路径成功：" + path);
        }
        return path;
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
