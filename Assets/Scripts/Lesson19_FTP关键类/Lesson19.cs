using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Lesson19 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetworkCredential n = new NetworkCredential("kangkang", "08875799");

        //1.Create  创建FTP对象
        FtpWebRequest req = FtpWebRequest.Create(new Uri("ftp://127.0.0.1/Test.txt")) as FtpWebRequest;
        //2.Abort  停止传输
        req.Abort();
        //3.获取用于上传的文件流   用于上传的流
        Stream stream = req.GetRequestStream();
        //4.返回FTP服务器的响应
        //FtpWebResponse webResponse = req.GetResponse() as FtpWebResponse;

        //1.设置通信凭证  让我（FTP通信对象）知道服务器的账号密码
        req.Credentials = n;
        //2.请求(上传或下载)完成时，是否保持控制连接（默认true，保持，不关闭）
        req.KeepAlive = false;
        //3.操作的命令的设置
        req.Method = WebRequestMethods.Ftp.DownloadFile;
        //4.是否使用二进制传输
        req.UseBinary = true;
        //5.重命名
        req.RenameTo = "MyTest.txt";

        //1.
        FtpWebResponse res = req.GetResponse() as FtpWebResponse;

        //1.处理完成后释放资源
        res.Close();
        //2.返回一个从服务器下载数据的流 用于下载服务器文件的流
        Stream stream1 = res.GetResponseStream();

        //1.收到的数据的长度
        print(res.ContentLength);
        //2.接收的数据的类型
        print(res.ContentType);
        //3.获取FTP服务器下发的最新状态码（当前服务器的状态）
        print(res.StatusCode);
        //4.FTP服务器下发的状态码的文本信息
        print(res.StatusDescription);
        //5.登陆前建立连接时服务器发送的消息
        print(res.BannerMessage);
        //6.FTP会话结束时，服务器发送的消息
        print(res.ExitMessage);
        //7.服务器上次文件修改的日期和时间
        print(res.LastModified);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
