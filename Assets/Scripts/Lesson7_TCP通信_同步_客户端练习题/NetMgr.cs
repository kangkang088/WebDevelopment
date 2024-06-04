using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Text;
using System.Threading;
using System;

public class NetMgr : MonoBehaviour
{
    private static NetMgr instance;
    public static NetMgr Instance => instance;
    private Socket socket;
    private Queue<BaseMsg> sendMessageQueue = new Queue<BaseMsg>();
    //private Thread sendThread;
    // private byte[] reveiveBytes = new byte[1024 * 1024];
    // private int receiveNumber = 0;
    public Queue<BaseMsg> reveiveMessageQueue = new Queue<BaseMsg>();
    //private Thread receiveThread;
    private bool isConnected = false;
    private byte[] cacheBytes = new byte[1024 * 1024];
    private int cacheNum = 0;
    private int SEND_HEART_MSG = 2;
    private HeartMsg heartMsg = new HeartMsg();
    public void SendTest(byte[] bytes)
    {
        socket.Send(bytes);
    }
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        InvokeRepeating("SendHeartMsg", 0, SEND_HEART_MSG);
    }
    private void SendHeartMsg()
    {
        if (isConnected)
            Send(heartMsg);
    }
    void Start()
    {

    }
    void Update()
    {
        if (reveiveMessageQueue.Count > 0)
        {
            BaseMsg baseMsg = reveiveMessageQueue.Dequeue();
            if (baseMsg is PlayerMsg)
            {
                PlayerMsg playerMsg = baseMsg as PlayerMsg;
                print(playerMsg.playerID);
                print(playerMsg.playerData.atk);
                print(playerMsg.playerData.lev);
            }
        }
    }
    void OnDestroy()
    {
        Close();
    }
    //连接服务器
    public void Connect(string ip, int port)
    {
        if (isConnected)
            return;
        if (socket == null)
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            socket.Connect(ipPoint);
            isConnected = true;
            ThreadPool.QueueUserWorkItem(SendMessage);
            //sendThread = new Thread(SendMessage);
            //sendThread.Start();
            ThreadPool.QueueUserWorkItem(ReceiveMessage);
            //receiveThread = new Thread(ReceiveMessage);
            //receiveThread.Start();
        }
        catch (SocketException e)
        {
            if (e.ErrorCode == 10061)
                print("服务器拒绝连接");
            else
                print("连接失败：" + e.ErrorCode + "." + e.Message);
        }

    }
    //收发消息
    public void Send(BaseMsg info)
    {
        sendMessageQueue.Enqueue(info);
    }
    private void SendMessage(object obj)
    {
        while (isConnected)
        {
            if (sendMessageQueue.Count > 0)
            {
                socket.Send(sendMessageQueue.Dequeue().Writing());
            }
        }
    }
    private void ReceiveMessage(object obj)
    {
        while (isConnected)
        {
            if (socket.Available > 0)
            {
                byte[] reveiveBytes = new byte[1024 * 1024];
                int receiveNumber = socket.Receive(reveiveBytes);
                HandleReceiveMessage(reveiveBytes, receiveNumber);
                // int mesID = BitConverter.ToInt32(reveiveBytes, 0);
                // BaseMsg baseMsg = null;
                // switch (mesID)
                // {
                //     case 1001:
                //         PlayerMsg playerMsg = new PlayerMsg();
                //         playerMsg.Reading(reveiveBytes, 4);
                //         baseMsg = playerMsg;
                //         break;
                // }
                // if (baseMsg == null)
                //     continue;
                // reveiveMessageQueue.Enqueue(baseMsg);
            }
        }
    }
    private void HandleReceiveMessage(byte[] receiveBytes, int receiveNumber)
    {
        int msgID = 0;
        int msgLength = 0;
        int nowIndex = 0;
        //收到消息时，看看之前有没有缓存的，有的话，直接拼接
        receiveBytes.CopyTo(cacheBytes, cacheNum);
        cacheNum += receiveNumber;
        while (true)
        {
            msgLength = -1;
            if (cacheNum - nowIndex >= 8)
            {
                msgID = BitConverter.ToInt32(cacheBytes, nowIndex);
                nowIndex += 4;
                msgLength = BitConverter.ToInt32(cacheBytes, nowIndex);
                nowIndex += 4;
            }
            if (cacheNum - nowIndex >= msgLength && msgLength != -1)
            {
                BaseMsg baseMsg = null;
                switch (msgID)
                {
                    case 1001:
                        PlayerMsg playerMsg = new PlayerMsg();
                        playerMsg.Reading(cacheBytes, nowIndex);
                        baseMsg = playerMsg;
                        break;
                }
                if (baseMsg != null)
                    reveiveMessageQueue.Enqueue(baseMsg);
                nowIndex += msgLength;
                if (nowIndex == cacheNum)
                {
                    cacheNum = 0;
                    break;
                }

            }
            else
            {
                //不满足，有分包，把当前收到的内容记录下来，下次接收到消息的时候再处理
                // receiveBytes.CopyTo(cacheBytes, 0);
                // cacheNum = receiveNumber;
                if (msgLength != -1)
                    nowIndex -= 8;
                Array.Copy(cacheBytes, nowIndex, cacheBytes, 0, cacheNum - nowIndex);
                cacheNum = cacheNum - nowIndex;
                break;
            }
        }

    }
    public void Close()
    {
        if (socket != null)
        {
            print("客户端断开连接");
            QuitMsg quitMsg = new QuitMsg();
            socket.Send(quitMsg.Writing());
            socket.Shutdown(SocketShutdown.Both);
            socket.Disconnect(false);
            socket.Close();
            socket = null;
            isConnected = false;
            // sendThread = null;
            // receiveThread = null;
        }
    }
}
