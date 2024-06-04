using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class FtpMgr
{
    private static FtpMgr instance = new FtpMgr();
    public static FtpMgr Instance => instance;
    private string FTP_PATH = "ftp://127.0.0.1/";
    private string USER_NAME = "kangkang";
    private string PASSWORD = "08875799";
    private FtpMgr()
    { }
    public async void UpLoadFile(string fileName, string localPath, UnityAction action = null)
    {
        await Task.Run(() =>
        {
            try
            {
                FtpWebRequest req = FtpWebRequest.Create(new Uri(FTP_PATH + fileName)) as FtpWebRequest;
                req.Proxy = null;
                NetworkCredential n = new NetworkCredential(USER_NAME, PASSWORD);
                req.Credentials = n;
                req.KeepAlive = false;
                req.UseBinary = true;
                req.Method = WebRequestMethods.Ftp.UploadFile;
                //上传流
                Stream upLoadStream = req.GetRequestStream();
                using (FileStream file = File.OpenRead(localPath))
                {
                    byte[] bytes = new byte[1024];
                    int contentNum = file.Read(bytes, 0, bytes.Length);
                    while (contentNum != 0)
                    {
                        upLoadStream.Write(bytes, 0, contentNum);
                        contentNum = file.Read(bytes, 0, bytes.Length);
                    }
                    //上传结束
                    file.Close();
                    upLoadStream.Close();
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("上传文件出错:" + e.Message);
            }
        });
        action?.Invoke();
        Debug.Log("上传成功");
    }
    public async void DownLoadFile(string fileName, string localPath, UnityAction action = null)
    {
        await Task.Run(() =>
        {
            try
            {
                FtpWebRequest req = FtpWebRequest.Create(new Uri(FTP_PATH + fileName)) as FtpWebRequest;
                req.Credentials = new NetworkCredential(USER_NAME, PASSWORD);
                req.Proxy = null;
                req.KeepAlive = false;
                req.UseBinary = true;
                req.Method = WebRequestMethods.Ftp.DownloadFile;
                FtpWebResponse res = req.GetResponse() as FtpWebResponse;
                Stream downLoadStream = res.GetResponseStream();
                using (FileStream file = File.Create(localPath))
                {
                    byte[] bytes = new byte[1024];
                    int contentLength = downLoadStream.Read(bytes, 0, bytes.Length);
                    while (contentLength != 0)
                    {
                        file.Write(bytes, 0, contentLength);
                        contentLength = downLoadStream.Read(bytes, 0, bytes.Length);
                    }
                    //下载完成
                    file.Close();
                    downLoadStream.Close();
                    res.Close();
                }
                Debug.Log("下载成功");
            }
            catch (System.Exception e)
            {
                Debug.Log("下载失败：" + e.Message);
            }
        });
        action?.Invoke();
    }
    public async void DeleteFile(string fileName, UnityAction<bool> action = null)
    {
        await Task.Run(() =>
        {
            try
            {
                FtpWebRequest req = FtpWebRequest.Create(new Uri(FTP_PATH + fileName)) as FtpWebRequest;
                req.Credentials = new NetworkCredential(USER_NAME, PASSWORD);
                req.Proxy = null;
                req.KeepAlive = false;
                req.UseBinary = true;
                req.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse res = req.GetResponse() as FtpWebResponse;
                res.Close();
                action?.Invoke(true);
            }
            catch (System.Exception e)
            {
                Debug.Log("删除失败：" + e.Message);
                action?.Invoke(false);
            }

        });
    }
    public async void GetFileSize(string fileName, UnityAction<long> action)
    {
        await Task.Run(() =>
        {
            try
            {
                FtpWebRequest req = FtpWebRequest.Create(new Uri(FTP_PATH + fileName)) as FtpWebRequest;
                req.Credentials = new NetworkCredential(USER_NAME, PASSWORD);
                req.Proxy = null;
                req.KeepAlive = false;
                req.UseBinary = true;
                req.Method = WebRequestMethods.Ftp.GetFileSize;
                FtpWebResponse res = req.GetResponse() as FtpWebResponse;
                action?.Invoke(res.ContentLength);
                res.Close();
            }
            catch (System.Exception e)
            {
                Debug.Log("获取大小失败：" + e.Message);
                action?.Invoke(0);
            }

        });
    }
    public async void CreateDirectory(string directoryName, UnityAction<bool> action = null)
    {
        await Task.Run(() =>
        {
            try
            {
                FtpWebRequest req = FtpWebRequest.Create(new Uri(FTP_PATH + directoryName)) as FtpWebRequest;
                req.Credentials = new NetworkCredential(USER_NAME, PASSWORD);
                req.Proxy = null;
                req.KeepAlive = false;
                req.UseBinary = true;
                req.Method = WebRequestMethods.Ftp.MakeDirectory;
                FtpWebResponse res = req.GetResponse() as FtpWebResponse;
                res.Close();
                action?.Invoke(true);
            }
            catch (System.Exception e)
            {
                Debug.Log("创建文件夹失败：" + e.Message);
                action?.Invoke(false);
            }

        });
    }
    public async void GetFileList(string directoryName, UnityAction<List<string>> action = null)
    {
        await Task.Run(() =>
        {
            try
            {
                FtpWebRequest req = FtpWebRequest.Create(new Uri(FTP_PATH + directoryName)) as FtpWebRequest;
                req.Credentials = new NetworkCredential(USER_NAME, PASSWORD);
                req.Proxy = null;
                req.KeepAlive = false;
                req.UseBinary = true;
                req.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse res = req.GetResponse() as FtpWebResponse;
                //通过流来获取
                StreamReader streamReader = new StreamReader(res.GetResponseStream());
                List<string> nameStrs = new List<string>();
                string line = streamReader.ReadLine();
                while (line != null)
                {
                    nameStrs.Add(line);
                    line = streamReader.ReadLine();
                }
                streamReader.Close();
                res.Close();
                action?.Invoke(nameStrs);

            }
            catch (System.Exception e)
            {
                Debug.Log("获取文件夹文件列表失败：" + e.Message);
                action?.Invoke(null);
            }
        });
    }
}
