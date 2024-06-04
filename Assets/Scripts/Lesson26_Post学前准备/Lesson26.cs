using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;

public class Lesson26 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        HttpWebRequest req = HttpWebRequest.Create(new Uri("http://192.168.1.9:8080/HTTPServer/")) as HttpWebRequest;
        req.Method = WebRequestMethods.Http.Post;
        req.Timeout = 2000;
        //设置上传的内容的类型
        req.ContentType = "application/x-www-form-urlencoded";
        //设置要上传的数据
        string str = "Name=MrWei&ID=2";
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        //上传前一定要先设置内容的长度
        req.ContentLength = bytes.Length;
        //上传数据
        Stream stream = req.GetRequestStream();
        stream.Write(bytes, 0, bytes.Length);
        stream.Close();
        //发送数据 调用GetResponse后才会上传
        HttpWebResponse res = req.GetResponse() as HttpWebResponse;
        print(res.StatusCode);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
