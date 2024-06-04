using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System.Net;
using System.Text;

public class Lesson14 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //1.创建UDP套接字
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //2.绑定本机地址
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
        socket.Bind(ipPoint);
        //3.发送到指定目标
        IPEndPoint remoteIPPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081);
        socket.SendTo(Encoding.UTF8.GetBytes("123132131"), remoteIPPoint);
        //4.接收消息
        byte[] bytes = new byte[512];
        EndPoint remoteIPPoint2 = new IPEndPoint(IPAddress.Any, 0);
        //把给我发消息的那个服务器的IP和端口存入到声明的这个EndPoint里面
        int length = socket.ReceiveFrom(bytes, ref remoteIPPoint2);
        print((remoteIPPoint2 as IPEndPoint).Address.ToString() + "发来了：" + Encoding.UTF8.GetString(bytes, 0, length));
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
