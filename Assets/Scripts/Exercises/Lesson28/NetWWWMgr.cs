using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetWWWMgr : MonoBehaviour
{
    private static NetWWWMgr instance;
    public static NetWWWMgr Instance => instance;
    private string HTTP_SERVER_PATH = "http://192.168.1.9:8080/HTTPServer/";
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    public void LoadRes<T>(string path, UnityAction<T> action) where T : class
    {
        StartCoroutine(LoadResAsync<T>(path, action));
    }
    private IEnumerator LoadResAsync<T>(string path, UnityAction<T> action) where T : class
    {
        WWW www = new WWW(path);
        yield return www;
        if (www.error == null)
        {
            //根据T的类型，决定使用哪种类型的资源传递给外部
            if (typeof(T) == typeof(AssetBundle))
            {
                action?.Invoke(www.assetBundle as T);
            }
            else if (typeof(T) == typeof(Texture))
            {
                action?.Invoke(www.texture as T);
            }
            else if (typeof(T) == typeof(AudioClip))
            {
                action?.Invoke(www.GetAudioClip() as T);
            }
            else if (typeof(T) == typeof(string))
            {
                action?.Invoke(www.text as T);
            }
            else if (typeof(T) == typeof(byte[]))
            {
                action?.Invoke(www.bytes as T);
            }
        }
        else
        {
            Debug.LogError("加载资源出错：" + www.error);
        }
    }
    public void SendMessage<T>(BaseMsg msg, UnityAction<T> action) where T : BaseMsg
    {
        StartCoroutine(SendMeaasgeAsync(msg, action));
    }
    private IEnumerator SendMeaasgeAsync<T>(BaseMsg msg, UnityAction<T> action) where T : BaseMsg
    {
        WWWForm data = new WWWForm();
        data.AddBinaryData("Msg", msg.Writing());
        WWW www = new WWW(HTTP_SERVER_PATH, data);
        yield return www;
        //发送完成，收到响应，且认为服务端回复的消息也是一个BaseMsg类型的二进制数据
        if (www.error == null)
        {
            int index = 0;
            int msgID = BitConverter.ToInt32(www.bytes, 0);
            index += 4;
            int msgLength = BitConverter.ToInt32(www.bytes, index);
            index += 4;
            BaseMsg baseMsg = null;
            switch (msgID)
            {
                case 1001:
                    baseMsg = new PlayerMsg();
                    baseMsg.Reading(www.bytes, index);
                    break;
            }
            if (baseMsg != null)
            {
                //服务器回复的消息传给委托函数，处理服务器回复的消息
                action?.Invoke(baseMsg as T);
            }
        }
        else
            Debug.LogError("消息发送出问题：" + www.error);
    }
    public void UpLoadFile(string fileName, string localPath, UnityAction<UnityWebRequest.Result> action)
    {
        StartCoroutine(UpLoadFileAsync(fileName, localPath, action));
    }
    private IEnumerator UpLoadFileAsync(string fileName, string localPath, UnityAction<UnityWebRequest.Result> action)
    {
        List<IMultipartFormSection> data = new List<IMultipartFormSection>();
        data.Add(new MultipartFormFileSection(fileName, File.ReadAllBytes(localPath)));
        UnityWebRequest req = UnityWebRequest.Post(HTTP_SERVER_PATH, data);
        yield return req.SendWebRequest();
        action?.Invoke(req.result);
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("上传有问题：" + req.error + req.responseCode);
        }
    }
    public void UnityWebRequestLoad<T>(string path, UnityAction<T> action, string localPath = "", AudioType type = AudioType.MPEG) where T : class
    {
        StartCoroutine(UnityWebRequestLoadAsync(path, action, localPath, type));
    }
    private IEnumerator UnityWebRequestLoadAsync<T>(string path, UnityAction<T> action, string localPath = "", AudioType type = AudioType.MPEG) where T : class
    {
        UnityWebRequest req = new UnityWebRequest(path, UnityWebRequest.kHttpVerbGET);
        if (typeof(T) == typeof(byte[]))
        {
            req.downloadHandler = new DownloadHandlerBuffer();
        }
        else if (typeof(T) == typeof(Texture))
        {
            req.downloadHandler = new DownloadHandlerTexture();
        }
        else if (typeof(T) == typeof(AssetBundle))
        {
            req.downloadHandler = new DownloadHandlerAssetBundle(req.url, 0);
        }
        else if (typeof(T) == typeof(object))
        {
            req.downloadHandler = new DownloadHandlerFile(localPath);
        }
        else if (typeof(T) == typeof(AudioClip))
        {
            req = UnityWebRequestMultimedia.GetAudioClip(path, type);
        }
        else
        {
            Debug.LogError("未知类型：" + typeof(T));
            yield break;
        }
        yield return req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.Success)
        {
            if (typeof(T) == typeof(byte[]))
            {
                action?.Invoke(req.downloadHandler.data as T);
            }
            else if (typeof(T) == typeof(Texture))
            {
                action?.Invoke((req.downloadHandler as DownloadHandlerTexture).texture as T);
            }
            else if (typeof(T) == typeof(AssetBundle))
            {
                action?.Invoke(DownloadHandlerAssetBundle.GetContent(req) as T);
            }
            else if (typeof(T) == typeof(object))
            {
                action?.Invoke(null);
            }
            else if (typeof(T) == typeof(AudioClip))
            {
                action?.Invoke(DownloadHandlerAudioClip.GetContent(req) as T);
            }
        }
        else
        {
            Debug.LogError("获取结果失败：" + req.result + req.error + req.responseCode);
        }
    }
}
