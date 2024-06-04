using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.IO;
using System.Text;

public class HttpMgr
{
    private static HttpMgr instance = new HttpMgr();
    public static HttpMgr Instance => instance;
    private string HTTP_PATH = "http://192.168.1.9:8080/HTTPServer/";
    private string USER_NAME = "Kang";
    private string PASSWORD = "088757";

    private HttpMgr() { }

    public async void DownLoadFile(string fileName, string localFilePath, UnityAction<HttpStatusCode> action)
    {
        HttpStatusCode result = HttpStatusCode.OK;
        await Task.Run(() =>
        {
            try
            {
                HttpWebRequest req = HttpWebRequest.Create(new Uri(HTTP_PATH + fileName)) as HttpWebRequest;
                req.Timeout = 3000;
                req.Method = WebRequestMethods.Http.Head;
                HttpWebResponse res = req.GetResponse() as HttpWebResponse;
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    res.Close();
                    Debug.Log("文件存在");
                    req = HttpWebRequest.Create(new Uri(HTTP_PATH + fileName)) as HttpWebRequest;
                    req.Timeout = 3000;
                    req.Method = WebRequestMethods.Http.Get;
                    res = req.GetResponse() as HttpWebResponse;
                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        using (FileStream file = File.Create(localFilePath))
                        {
                            Stream stream = res.GetResponseStream();
                            byte[] bytes = new byte[4096];
                            int contentLength = stream.Read(bytes, 0, bytes.Length);
                            while (contentLength != 0)
                            {
                                file.Write(bytes, 0, contentLength);
                                contentLength = stream.Read(bytes, 0, bytes.Length);
                            }
                            stream.Close();
                            file.Close();
                        }
                    }
                    else
                    {
                        result = res.StatusCode;
                    }
                }
                else
                {
                    result = res.StatusCode;
                }
                res.Close();
            }
            catch (WebException w)
            {
                result = HttpStatusCode.InternalServerError;
                Debug.Log("下载出错：" + w.Message + w.Status);
            }
        });
        action.Invoke(result);
    }
    public async void UpLoadFile(string fileName, string localFilePath, UnityAction<HttpStatusCode> action)
    {
        HttpStatusCode result = HttpStatusCode.BadRequest;
        await Task.Run(() =>
        {
            try
            {
                HttpWebRequest req = HttpWebRequest.Create(new Uri("http://192.168.1.9:8080/HTTPServer/")) as HttpWebRequest;
                req.Method = WebRequestMethods.Http.Post;
                req.Timeout = 50000;
                req.ContentType = "multipart/form-data;boundary=MrTang";
                req.Credentials = new NetworkCredential(USER_NAME, PASSWORD);
                req.PreAuthenticate = true;

                string head = "--MrTang\r\n" +
                              "Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\n" +
                              "Content-Type:application/octet-stream\r\n\r\n";
                head = string.Format(head, fileName);
                byte[] headBytes = Encoding.UTF8.GetBytes(head);
                byte[] endBytes = Encoding.UTF8.GetBytes("\r\n--MrTang--\r\n");
                using (FileStream localFileStream = File.OpenRead(localFilePath))
                {
                    req.ContentLength = headBytes.Length + localFileStream.Length + endBytes.Length;
                    Stream upLoadStream = req.GetRequestStream();
                    upLoadStream.Write(headBytes, 0, headBytes.Length);
                    byte[] bytes = new byte[4096];
                    int contentLength = localFileStream.Read(bytes, 0, bytes.Length);
                    while (contentLength != 0)
                    {
                        upLoadStream.Write(bytes, 0, contentLength);
                        contentLength = localFileStream.Read(bytes, 0, bytes.Length);
                    }
                    upLoadStream.Write(endBytes, 0, endBytes.Length);
                    upLoadStream.Close();
                    localFileStream.Close();
                }
                HttpWebResponse res = req.GetResponse() as HttpWebResponse;
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    result = res.StatusCode;
                }
                else
                {
                    result = res.StatusCode;
                }
                res.Close();
            }
            catch (WebException w)
            {
                result = HttpStatusCode.InternalServerError;
                Debug.Log("上传失败:" + w.Message + w.Status);
            }
        });
        action?.Invoke(result);
    }
}
