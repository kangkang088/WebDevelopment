using System;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using System.Net;
using System.Text;

public class Lesson12 : MonoBehaviour
{
    void Start()
    {
        // CountDownAsync(5, () =>
        // {
        //     print("倒计时结束");
        // });
        CountDownAsync(5);
        print("主线程逻辑");

        Socket socketTcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socketTcp.BeginAccept(AcceptCallback, socketTcp);

        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
        socketTcp.BeginConnect(ipPoint, (result) =>
        {
            try
            {
                //(result.AsyncState as Socket) 得到第二个参数，就是相当于与服务端通信的socket
                //注意服务端是先得到第二个参数，用第二个参数得到与客户端通信的socket
                //客户端直接得到的第二个参数就是用于通信的 EndConnect没有返回值
                (result.AsyncState as Socket).EndConnect(result);
            }
            catch (SocketException e)
            {
                print("连接出错:" + e.SocketErrorCode + e.Message);
            }

        }, socketTcp);

        socketTcp.BeginReceive(resultBytes, 0, resultBytes.Length, SocketFlags.None, ReceiveCallback, socketTcp);

        byte[] bytes = Encoding.UTF8.GetBytes("1231321321321321");
        socketTcp.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, (reslut) =>
        {
            try
            {
                //返回成功发送的字节数
                socketTcp.EndSend(reslut);
                print("发送成功");
            }
            catch (SocketException e)
            {
                print("发送错误:" + e.SocketErrorCode + e.Message);
            }
        }, socketTcp);

        SocketAsyncEventArgs e = new SocketAsyncEventArgs();
        e.Completed += (socket, args) =>
        {
            //socket参数代表当前启用AcceptAsync方法的socket socketTcp。
            //args代表我们调用AcceptAsync方法传入的SocketAsyncEventArgs e

            if (args.SocketError == SocketError.Success)
            {
                //获取连入客户端的socket,进行处理
                Socket clientSocket = args.AcceptSocket;

                //继续监听连入的客户端
                (socket as Socket).AcceptAsync(args);
            }
            else
            {
                print("连入客户端失败：" + args.SocketError);
            }
        };
        socketTcp.AcceptAsync(e);

        SocketAsyncEventArgs e2 = new SocketAsyncEventArgs();
        e2.Completed += (socket, args) =>
        {
            if (args.SocketError == SocketError.Success)
            {
                //连入成功

            }
            else
            {
                print("连接失败：" + args.SocketError);
            }
        };
        socketTcp.ConnectAsync(e2);

        SocketAsyncEventArgs e3 = new SocketAsyncEventArgs();
        byte[] bytes2 = Encoding.UTF8.GetBytes("1215165151310");
        //要发送的字节数组  偏移位置 发送数量
        e3.SetBuffer(bytes2, 0, bytes2.Length);
        e3.Completed += (socket, args) =>
        {
            if (args.SocketError == SocketError.Success)
            {
                print("发送成功");
            }
            else
            {
                print("发送失败：" + args.SocketError);
            }
        };
        socketTcp.SendAsync(e3);

        SocketAsyncEventArgs e4 = new SocketAsyncEventArgs();
        //设置接收数据的容器，偏移位置，容量
        e4.SetBuffer(new byte[1024 * 1024], 0, 1024 * 1024);
        e4.Completed += (socket, args) =>
        {
            if (args.SocketError == SocketError.Success)
            {
                print("接收成功");
                //解析数据,偏移位置  具体接收到的字节数
                Encoding.UTF8.GetString(args.Buffer, 0, args.BytesTransferred);

                //解析完了，还要接收，复用容器，设置下一次开始接收的位置
                //没解析完，下一次从解析结束位置开始接收
                args.SetBuffer(0, 1024 * 1024);
                (socket as Socket).ReceiveAsync(e4);
            }
            else
            {
                print("接收失败：" + args.SocketError);
            }
        };
        socketTcp.ReceiveAsync(e4);
    }
    private byte[] resultBytes = new byte[1024];
    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            Socket s = result.AsyncState as Socket;
            //实际接收到的消息字节数
            int num = s.EndReceive(result);
            //消息处理
            Encoding.UTF8.GetString(resultBytes, 0, num);
            s.BeginReceive(resultBytes, 0, resultBytes.Length, SocketFlags.None, ReceiveCallback, s);
        }
        catch (SocketException e)
        {
            print("接收的消息有问题：" + e.SocketErrorCode + e.Message);
            throw;
        }
    }
    private void AcceptCallback(IAsyncResult result)
    {
        try
        {
            //1.AsyncState 获取传入的参数(就是这个方法的第二个参数)
            Socket s = result.AsyncState as Socket;
            //2.EndAccept 返回与第二个参数代表的服务端socket相连的，用于与客户端通信的socket
            Socket socketClient = s.EndAccept(result);
            s.BeginAccept(AcceptCallback, s);
        }
        catch (SocketException e)
        {
            print(e.SocketErrorCode);
        }
    }
    void Update()
    {

    }
    public void CountDownAsync(int second, UnityAction callback)
    {
        print("开始计时");
        Thread thread = new Thread(() =>
        {
            while (true)
            {
                print(second);
                Thread.Sleep(1000);
                --second;
                if (second == 0)
                    break;
            }
            callback?.Invoke();
        });
        thread.Start();

    }
    public async void CountDownAsync(int second)
    {
        print("倒计时开始");
        await Task.Run(() =>
        {
            while (true)
            {
                print(second);
                Thread.Sleep(1000);
                --second;
                if (second == 0)
                    break;
            }
        });
        print("倒计时结束,执行函数");
    }
}
