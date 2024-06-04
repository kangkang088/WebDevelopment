using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Text;
using System;

public class Lesson6 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //1.创建TCP套接字
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //2.Connect方法与服务端连接
        //确定服务端的IP和端口，因为服务端这里模拟的是本机，所以填自己
        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
        try
        {
            socket.Connect(iPEndPoint);
        }
        catch (SocketException e)
        {
            if (e.ErrorCode == 10061)
                print("服务器拒绝连接");
            else
                print("连接服务器失败：" + e.ErrorCode);
            return;
        }
        //3.Send Receive相关方法收发数据
        //接收数据
        byte[] receiveBytes = new byte[1024];
        int number = socket.Receive(receiveBytes);
        //首先解析消息ID
        int mesID = BitConverter.ToInt32(receiveBytes, 0);
        switch (mesID)
        {
            case 1001:
                PlayerMsg playerMsg = new PlayerMsg();
                playerMsg.Reading(receiveBytes, 4);
                print(playerMsg.playerID);
                print(playerMsg.playerData.name);
                print(playerMsg.playerData.atk);
                print(playerMsg.playerData.lev);
                break;
        }
        print("收到服务端发来的消息:" + Encoding.UTF8.GetString(receiveBytes));
        //发送数据
        socket.Send(Encoding.UTF8.GetBytes("你好，我是客户端"));
        //4.释放连接
        socket.Shutdown(SocketShutdown.Both);
        //5.关闭套接字
        socket.Close();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
