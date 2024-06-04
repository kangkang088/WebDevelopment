using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Lesson20 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            FtpWebRequest req = FtpWebRequest.Create(new Uri("ftp://127.0.0.1/pic.bmp")) as FtpWebRequest;
            //避免http代理的影响
            req.Proxy = null;
            NetworkCredential n = new NetworkCredential("kangkang", "08875799");
            req.Credentials = n;
            req.KeepAlive = false;
            req.Method = WebRequestMethods.Ftp.UploadFile;
            req.UseBinary = true;
            Stream upLoadStream = req.GetRequestStream();

            using (FileStream file = File.OpenRead(Application.streamingAssetsPath + "/Test.bmp"))
            {
                //一点点的读出来，再传入上传流中
                byte[] bytes = new byte[1024];
                //每一次真正读的字节数
                int contentLength = file.Read(bytes, 0, bytes.Length);
                while (contentLength != 0)
                {
                    //写入上传流
                    upLoadStream.Write(bytes, 0, contentLength);
                    contentLength = file.Read(bytes, 0, bytes.Length);
                }
                //写完了
                file.Close();
                upLoadStream.Close();
            }
        }
        catch (Exception e)
        {
            print("上传失败：" + e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
