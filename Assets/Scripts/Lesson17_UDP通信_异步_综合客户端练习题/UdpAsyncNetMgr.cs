using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class UdpAsyncNetMgr : MonoBehaviour
{
    private static UdpAsyncNetMgr instance;
    public static UdpAsyncNetMgr Instance => instance;

    private EndPoint serverIpPoint;

    private Socket socket;

    //�ͻ���socket�Ƿ�ر�
    private bool isClose = true;

    //������Ϣ�Ķ��� �ڶ��߳�������Բ���
    private Queue<BaseMsg> receiveQueue = new Queue<BaseMsg>();

    private byte[] cacheBytes = new byte[512];

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
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

    /// <summary>
    /// �����ͻ���socket��صķ���
    /// </summary>
    /// <param name="ip">Զ�˷�������IP</param>
    /// <param name="port">Զ�˷�������port</param>
    public void StartClient(string ip, int port)
    {
        //�����ǰ�ǿ���״̬ �Ͳ����ٿ���
        if (!isClose)
            return;

        //�ȼ�¼��������ַ��һ�ᷢ��Ϣʱ��ʹ�� 
        serverIpPoint = new IPEndPoint(IPAddress.Parse(ip), port);

        IPEndPoint clientIpPort = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081);
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(clientIpPort);
            isClose = false;
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.SetBuffer(cacheBytes, 0, cacheBytes.Length);
            args.RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            args.Completed += ReceiveMsg;
            socket.ReceiveFromAsync(args);
            print("�ͻ�����������");
        }
        catch (System.Exception e)
        {
            print("����Socket������" + e.Message);
        }
    }

    private void ReceiveMsg(object obj, SocketAsyncEventArgs args)
    {
        int nowIndex;
        int msgID;
        int msgLength;
        if(args.SocketError == SocketError.Success)
        {
            try
            {
                //Ҫ�Ƿ��������ĲŴ���
                if (args.RemoteEndPoint.Equals(serverIpPoint))
                {
                    //�����������������Ϣ
                    nowIndex = 0;
                    //����ID
                    msgID = BitConverter.ToInt32(args.Buffer, nowIndex);
                    nowIndex += 4;
                    //��������
                    msgLength = BitConverter.ToInt32(args.Buffer, nowIndex);
                    nowIndex += 4;
                    //������Ϣ��
                    BaseMsg msg = null;
                    switch (msgID)
                    {
                        case 1001:
                            msg = new PlayerMsg();
                            //�����л���Ϣ��
                            msg.Reading(args.Buffer, nowIndex);
                            break;
                    }
                    if (msg != null)
                        receiveQueue.Enqueue(msg);
                }
                //�ٴν�����Ϣ
                if (socket != null && !isClose)
                {
                    args.SetBuffer(0, cacheBytes.Length);
                    socket.ReceiveFromAsync(args);
                }
            }
            catch (SocketException s)
            {
                print("������Ϣ����" + s.SocketErrorCode + s.Message);
                Close();
            }
            catch (Exception e)
            {
                print("������Ϣ����(�����Ƿ����л�����)" + e.Message);
                Close();
            }
        }
        else
        {
            print("������Ϣʧ��" + args.SocketError);
        }
    }

    //������Ϣ
    public void Send(BaseMsg msg)
    {
        try
        {
            if(socket != null && !isClose)
            {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                byte[] bytes = msg.Writing();
                args.SetBuffer(bytes, 0, bytes.Length);
                args.Completed += SendToCallBack;
                //����Զ��Ŀ��
                args.RemoteEndPoint = serverIpPoint;
                socket.SendToAsync(args);
            }
        }
        catch (SocketException s)
        {
            print("������Ϣ����" + s.SocketErrorCode + s.Message);
        }
        catch (Exception e)
        {
            print("������Ϣ����(���������л�����)" + e.Message);
        }
    }

    private void SendToCallBack(object o, SocketAsyncEventArgs args)
    {
        if (args.SocketError != SocketError.Success)
            print("������Ϣʧ��" + args.SocketError);
    }

    //�ر�socket
    public void Close()
    {
        if (socket != null)
        {
            isClose = true;
            QuitMsg msg = new QuitMsg();
            //����һ���˳���Ϣ�������� �����Ƴ���¼
            socket.SendTo(msg.Writing(), serverIpPoint);
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            socket = null;
        }

    }

    private void OnDestroy()
    {
        Close();
    }
}
