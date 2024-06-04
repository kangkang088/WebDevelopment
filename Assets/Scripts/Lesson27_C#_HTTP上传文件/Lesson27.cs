using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class Lesson27 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //1.创建request对象
        //ftp上传时，直接在字符串里指明上传完成后的文件名，但是HTTP不用，只要指明上传到的目录就行，名字在后面设置
        HttpWebRequest req = HttpWebRequest.Create(new Uri("http://192.168.1.9:8080/HTTPServer/")) as HttpWebRequest;
        req.Method = WebRequestMethods.Http.Post;
        req.Credentials = new NetworkCredential("Kang", "088757");
        req.PreAuthenticate = true;//先验证身份再上传
        //2.设置上传的文件类型
        req.ContentType = "multipart/form-data;boundary=MrTang";
        req.Timeout = 500000;

        //3.设置头部字符串和尾部结束字符串
        //头部信息
        string head = "--MrTang\r\n" + //空行
                      "Content-Disposition:form-data;name=\"file\";filename=\"http上传的文件.jpg\"\r\n" + //空行
                      "Content-Type:application/octet-stream\r\n\r\n"; //空两行
        byte[] headBytes = Encoding.UTF8.GetBytes(head);

        //结束的边界信息
        byte[] endBytes = Encoding.UTF8.GetBytes("\r\n--MrTang--\r\n");

        //4.传入内容
        using (FileStream localFileStream = File.OpenRead(Application.streamingAssetsPath + "/Test.bmp"))
        {
            //设置总长度  头+内容+尾部
            req.ContentLength = headBytes.Length + endBytes.Length + localFileStream.Length;
            Stream upLoadStream = req.GetRequestStream();
            //5.数据写入上传流
            //先写头
            upLoadStream.Write(headBytes, 0, headBytes.Length);
            //再写内容
            byte[] bytes = new byte[2048];
            int contentLength = localFileStream.Read(bytes, 0, bytes.Length);
            while (contentLength != 0)
            {
                upLoadStream.Write(bytes, 0, contentLength);
                contentLength = localFileStream.Read(bytes, 0, bytes.Length);
            }
            //再写尾部
            upLoadStream.Write(endBytes, 0, endBytes.Length);
            upLoadStream.Close();
            localFileStream.Close();
        }
        //5.上传数据
        HttpWebResponse res = req.GetResponse() as HttpWebResponse;
        if (res.StatusCode == HttpStatusCode.OK)
        {
            print("上传通信成功");
        }
        else
        {
            print("上传失败:" + res.StatusCode);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
