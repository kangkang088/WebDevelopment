using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.Text;
using System.Net;
using System;

public class Lesson16 : MonoBehaviour
{
    private byte[] receiveBytes = new byte[512];
    // Start is called before the first frame update
    void Start()
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        byte[] bytes = Encoding.UTF8.GetBytes("1213154");
        EndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
        socket.BeginSendTo(bytes, 0, bytes.Length, SocketFlags.None, ipPoint, SendToOver, socket);

        socket.BeginReceiveFrom(receiveBytes, 0, receiveBytes.Length, SocketFlags.None, ref ipPoint, ReceiveFromOver, (socket, ipPoint));

        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
        args.SetBuffer(bytes, 0, bytes.Length);
        args.Completed += SendToAsyc;
        socket.SendToAsync(args);

        SocketAsyncEventArgs args1 = new SocketAsyncEventArgs();
        args1.SetBuffer(receiveBytes, 0, receiveBytes.Length);
        args1.Completed += ReceiveFromAsync;
        socket.ReceiveFromAsync(args1);
    }
    private void ReceiveFromAsync(object s, SocketAsyncEventArgs args)
    {
        if (args.SocketError == SocketError.Success)
        {
            print("接收成功");
            //具体接收到的字节数
            //args.BytesTransferred
            //得到接收数据的字节数组
            //args.Buffer;receiveBytes

            //继续接收
            Socket socket = s as Socket;
            //重新设置位置，没必要再传容器了，上面已经设置了
            args.SetBuffer(0, receiveBytes.Length);
            socket.ReceiveFromAsync(args);
        }
        else
        {
            print("接收失败");
        }
    }
    //s:代表调用SendToAsync的调用者  args：代表SendToAsync的参数
    private void SendToAsyc(object s, SocketAsyncEventArgs args)
    {
        if (args.SocketError == SocketError.Success)
        {
            print("发送成功");
        }
        else
        {
            print("发送失败");
        }
    }
    private void ReceiveFromOver(IAsyncResult result)
    {
        try
        {
            (Socket s, EndPoint ipPoint) info = ((Socket, EndPoint))result.AsyncState;
            int num = info.s.EndReceiveFrom(result, ref info.ipPoint);
            info.s.BeginReceiveFrom(receiveBytes, 0, receiveBytes.Length, SocketFlags.None, ref info.ipPoint, ReceiveFromOver, info);
        }
        catch (SocketException e)
        {
            print("接收消息出错误:" + e.SocketErrorCode + e.Message);
        }
    }
    private void SendToOver(IAsyncResult result)
    {
        try
        {
            Socket s = (result.AsyncState as Socket);
            s.EndSendTo(result);
            print("发送成功");
        }
        catch (SocketException e)
        {
            print("发送失败：" + e.SocketErrorCode + e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
