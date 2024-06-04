using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class Lesson21 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            FtpWebRequest req = FtpWebRequest.Create(new Uri("ftp://127.0.0.1/BigImage.jpg")) as FtpWebRequest;
            req.Credentials = new NetworkCredential("kangkang", "08875799");
            req.Proxy = null;
            req.KeepAlive = false;
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            req.UseBinary = true;
            //获取用于下载的对象
            FtpWebResponse res = req.GetResponse() as FtpWebResponse;
            Stream downloadStream = res.GetResponseStream();

            print(Application.persistentDataPath);
            using (FileStream file = File.Create(Application.persistentDataPath + "/ReceiveBigImage.jpg"))
            {
                byte[] bytes = new byte[1024];
                //读取下载流数据
                int contentLength = downloadStream.Read(bytes, 0, bytes.Length);
                while (contentLength != 0)
                {
                    file.Write(bytes, 0, contentLength);
                    contentLength = downloadStream.Read(bytes, 0, bytes.Length);
                }
                //下载完成
                file.Close();
                downloadStream.Close();
            }
        }
        catch (System.Exception e)
        {
            print("下载出错:" + e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
