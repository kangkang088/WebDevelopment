using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Lesson25 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            //1.创建HTTP连接通信对象
            HttpWebRequest req = HttpWebRequest.Create(new Uri("http://192.168.1.9:8080/HTTPServer/BigImage.jpg")) as HttpWebRequest;
            //2.设置请求类型和其他参数
            req.Method = WebRequestMethods.Http.Head;
            //超时设置 2s后服务器没反应，客户端认为这次请求失败了
            req.Timeout = 2000;
            //3.发送请求
            HttpWebResponse res = req.GetResponse() as HttpWebResponse;

            if (res.StatusCode == HttpStatusCode.OK)
            {
                print("文件存在且可用");
                print("文件大小：" + res.ContentLength);
                print("文件类型：" + res.ContentType);
            }
            else
            {
                print("文件不可用：" + res.StatusCode);
            }
        }
        catch (WebException e)
        {
            print("获取出错：" + e.Message + e.Status);
        }

        try
        {
            //1.创建HTTP连接通信对象
            HttpWebRequest req = HttpWebRequest.Create(new Uri("http://192.168.1.9:8080/HTTPServer/BigImage.jpg")) as HttpWebRequest;
            //2.设置请求类型和其他参数
            req.Method = WebRequestMethods.Http.Get;
            //超时设置 3s后服务器没反应，客户端认为这次下载请求失败了
            req.Timeout = 3000;
            //3.发送请求
            HttpWebResponse res = req.GetResponse() as HttpWebResponse;
            //4.获取下载流，写入本地路径
            if (res.StatusCode == HttpStatusCode.OK)
            {
                print(Application.persistentDataPath);
                using (FileStream file = File.Create(Application.persistentDataPath + "/HttpDownLoad.jpg"))
                {
                    Stream downLoadStream = res.GetResponseStream();
                    byte[] bytes = new byte[2048];
                    int contentLength = downLoadStream.Read(bytes, 0, bytes.Length);
                    while (contentLength != 0)
                    {
                        file.Write(bytes, 0, contentLength);
                        contentLength = downLoadStream.Read(bytes, 0, bytes.Length);
                    }
                    file.Close();
                    downLoadStream.Close();
                    res.Close();
                }
                print("下载成功");
            }
            else
                print("下载失败：" + res.StatusCode);
        }
        catch (WebException w)
        {
            print("下载出错:" + w.Message + w.Status);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
