using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Lesson5 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Socket tcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        Socket udp = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        //套接字连接状态
        if (tcp.Connected)
        {

        }
        //套接字类型
        print(tcp.SocketType);
        //协议类型
        print(tcp.ProtocolType);
        //寻址方案
        print(tcp.AddressFamily);
        //网络中接受到的数据字节数
        print(tcp.Available);
        //本机EndPoint
        print(tcp.LocalEndPoint);
        //远程EndPoint
        print(tcp.RemoteEndPoint);

        IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
        //绑定IP和端口
        tcp.Bind(iPEndPoint);
        //设置连接的客户端最大数量
        tcp.Listen(9999);
        //等待客户端连入
        tcp.Accept();

        //连接远端服务器（根据ip和端口）
        tcp.Connect(IPAddress.Parse("118.12.123.11"), 8080);

        //发送数据
        tcp.Send(new byte[] { 111 });
        //接受数据

        //释放连接并关闭Socket，先于Close调用
        tcp.Shutdown(SocketShutdown.Both);
        tcp.Close();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
