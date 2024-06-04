using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DownLoadHandlerMsg : DownloadHandlerScript
{
    BaseMsg msg; byte[] cacheBytes; int index = 0;
    public DownLoadHandlerMsg() : base() { }
    public T GetMsg<T>() where T : BaseMsg
    {
        return msg as T;
    }
    protected override byte[] GetData()
    {
        return cacheBytes;
    }
    protected override bool ReceiveData(byte[] data, int dataLength)
    {
        data.CopyTo(cacheBytes, index);
        index += dataLength;
        return true;
    }
    protected override void ReceiveContentLengthHeader(ulong contentLength)
    {
        cacheBytes = new byte[contentLength];
    }
    protected override void CompleteContent()
    {
        index = 0;
        int msgID = BitConverter.ToInt32(cacheBytes, index);
        index += 4;
        int msgLength = BitConverter.ToInt32(cacheBytes, index);
        index += 4;
        switch (msgID)
        {
            case 1001:
                msg = new PlayerMsg();
                msg.Reading(cacheBytes, index);
                break;
        }
        if (msg == null)
        {
            Debug.Log("对应ID:" + msgID + "没有处理");
        }
        else
        {
            Debug.Log("解析成功");
        }
    }
}
