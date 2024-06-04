using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using System.Threading;
using System;

public class UdpNetMgr : MonoBehaviour
{
    private static UdpNetMgr instance;
    public static UdpNetMgr Instance => instance;
    private Socket socket;
    private bool isClosed = true;
    private EndPoint serverIpPoint;
    private Queue<BaseMsg> sendQueue = new Queue<BaseMsg>();
    private Queue<BaseMsg> receiveQueue = new Queue<BaseMsg>();
    private byte[] cacheBytes = new byte[512];
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {
        if (receiveQueue.Count > 0)
        {
            BaseMsg baseMsg = receiveQueue.Dequeue();
            switch (baseMsg)
            {
                case PlayerMsg msg:
                    print(msg.playerID);
                    print(msg.playerData.name);
                    print(msg.playerData.atk);
                    print(msg.playerData.lev);
                    break;
            }
        }
    }
    public void StartClient(string ip, int port)
    {
        if (!isClosed)
            return;
        serverIpPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        IPEndPoint clientIpPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081);
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(clientIpPoint);
            print("客户端启动");
            isClosed = false;
            ThreadPool.QueueUserWorkItem(ReceiveMessage);
            ThreadPool.QueueUserWorkItem(SendMessage);
        }
        catch (System.Exception e)
        {
            print("启动socket出错" + e.Message);
            throw;
        }
    }
    private void ReceiveMessage(object obj)
    {
        EndPoint tempIpPoint = new IPEndPoint(IPAddress.Any, 0);
        int nowIndex = 0;
        int msgID;
        int msgLength;
        while (!isClosed)
        {
            if (socket != null && socket.Available > 0)
            {
                try
                {
                    socket.ReceiveFrom(cacheBytes, ref tempIpPoint);
                    //为了避免非服务器发来的骚扰消息
                    if (!tempIpPoint.Equals(serverIpPoint))
                    {
                        continue;
                    }
                    //处理服务器发来的消息
                    nowIndex = 0;
                    msgID = BitConverter.ToInt32(cacheBytes, nowIndex);
                    nowIndex += 4;
                    msgLength = BitConverter.ToInt32(cacheBytes, nowIndex);
                    nowIndex += 4;
                    BaseMsg msg = null;
                    switch (msgID)
                    {
                        case 1001:
                            msg = new PlayerMsg();
                            msg.Reading(cacheBytes, nowIndex);
                            break;
                    }
                    if (msg != null)
                    {
                        receiveQueue.Enqueue(msg);
                    }
                }
                catch (SocketException s)
                {
                    print("接收消息出问题：" + s.SocketErrorCode + s.Message);
                }
                catch (Exception e)
                {
                    print("其他问题：" + e.Message);
                }
            }
        }
    }
    private void SendMessage(object obj)
    {
        while (!isClosed)
        {
            if (socket != null && sendQueue.Count > 0)
            {
                try
                {
                    socket.SendTo(sendQueue.Dequeue().Writing(), serverIpPoint);
                }
                catch (SocketException e)
                {
                    print("发送消息出错:" + e.SocketErrorCode + e.Message);
                    throw;
                }
            }
        }
    }
    public void Send(BaseMsg msg)
    {
        sendQueue.Enqueue(msg);
    }
    public void Close()
    {
        if (socket != null)
        {
            isClosed = true;
            QuitMsg quitMsg = new QuitMsg();
            socket.SendTo(quitMsg.Writing(), serverIpPoint);
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            socket = null;
        }

    }
    void OnDestroy()
    {
        Close();
    }
}
