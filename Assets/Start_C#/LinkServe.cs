using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;

namespace LinkServe
{
    interface ILinkServe
    {
        void Connetion();//连接服务器
        void Send(string str);//发送字符串到服务端

    }
    public class Linkserve : ILinkServe
    {

        //连接服务器
        //以下是异步程序代码
        public string recvStr;//接收到的信息
        Socket socket;
        const int BUFFER_SIZE = 1024;
        public byte[] readBuff = new byte[BUFFER_SIZE];
        public void Connetion()
        {
            //transform.Find("Link/Text").GetComponent<Text>().text = "断开";
            //清空聊天框
            //recvText.text = "";
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string host = "127.0.0.1";
            int port = int.Parse("1234");
            socket.Connect(host, port);
            //clientText.text = "客户端地址：" + socket.LocalEndPoint.ToString();
            socket.BeginReceive(readBuff, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCb, null);
        }

        private void ReceiveCb(IAsyncResult ar)
        {
            try
            {
                int count = socket.EndReceive(ar);
                string str = Encoding.UTF8.GetString(readBuff, 0, count);//数据处理
               
                recvStr += str + "\n";
                //继续接收
                socket.BeginReceive(readBuff, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCb, null);
            }
            catch (Exception e)
            {
                //recvText.text += "连接已断开";
                socket.Close();
            }
        }
        public void Send(string str)//发送按钮
        {
            //string str = Login.user_name + ":" + textInput.text;
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            try
            {
                socket.Send(bytes);
            }
            catch
            {

            }
        }
    }
}
