using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Lesson32 : MonoBehaviour
{
    public RawImage rawImage;
    // Start is called before the first frame update
    void Start()
    {
        //UnityWebRequest req = UnityWebRequest.Get("");
        // UnityWebRequest req = new UnityWebRequest();
        // req.method = UnityWebRequest.kHttpVerbGET;
    }
    IEnumerator DownLoadText()
    {
        UnityWebRequest req = new UnityWebRequest("http://192.168.1.9:8080/HTTPServer/BigImage.jpg", UnityWebRequest.kHttpVerbGET);
        //1.DownloadHandlerBuffer
        req.downloadHandler = new DownloadHandlerBuffer();

        //2.DownloadHandlerFile
        req.downloadHandler = new DownloadHandlerFile(Application.persistentDataPath + "download.jpg");

        //3.DownloadHandlerTexture
        DownloadHandlerTexture downloadHandlerTexture = new DownloadHandlerTexture();
        req.downloadHandler = downloadHandlerTexture;

        //req.method = UnityWebRequest.kHttpVerbGET;
        yield return req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.Success)
        {
            //byte[] bytes = req.downloadHandler.data;
            rawImage.texture = downloadHandlerTexture.texture;
        }
        else
        {
            print("获取数据失败：" + req.result + req.responseCode + req.error);
        }
    }
    IEnumerator DownLoadAB()
    {
        UnityWebRequest req = new UnityWebRequest("http://192.168.1.9:8080/HTTPServer/lua.assetbundle", UnityWebRequest.kHttpVerbGET);
        //第二个参数 校验码 需要已知校验码 才能进行校验 AB包的完整性。不知道的话，传0，不能进行完整校验
        DownloadHandlerAssetBundle downloadHandlerAssetBundle = new DownloadHandlerAssetBundle(req.url, 0);
        req.downloadHandler = downloadHandlerAssetBundle;
        yield return req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.Success)
        {
            //byte[] bytes = req.downloadHandler.data;
            AssetBundle ab = downloadHandlerAssetBundle.assetBundle;
            print(ab.name);
        }
        else
        {
            print("获取数据失败：" + req.result + req.responseCode + req.error);
        }
    }
    IEnumerator DownLoadAudioClip()
    {
        UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip("http://192.168.1.9:8080/HTTPServer/music.mp3", AudioType.MPEG);
        yield return req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.Success)
        {
            //byte[] bytes = req.downloadHandler.data;

            AudioClip audio = DownloadHandlerAudioClip.GetContent(req);
            print(audio.name);
        }
        else
        {
            print("获取数据失败：" + req.result + req.responseCode + req.error);
        }
    }
    // Update is called once per frame
    IEnumerator DownLoadCustomHandler()
    {
        UnityWebRequest req = new UnityWebRequest("http://192.168.1.9:8080/HTTPServer/BigImage.jpg", UnityWebRequest.kHttpVerbGET);
        req.downloadHandler = new CustomDownLoadFileHandler(Application.persistentDataPath + "/CustomHandler.jpg");
        yield return req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.Success)
        {
            //byte[] bytes = req.downloadHandler.data;
        }
        else
        {
            print("获取数据失败：" + req.result + req.responseCode + req.error);
        }
    }
    void Update()
    {

    }
}
public class CustomDownLoadFileHandler : DownloadHandlerScript
{
    private string savePath;
    private byte[] cacheBytes;
    private int index = 0;
    public CustomDownLoadFileHandler() : base()
    {

    }
    public CustomDownLoadFileHandler(byte[] bytes) : base(bytes)
    {

    }
    public CustomDownLoadFileHandler(string path) : base()
    {
        savePath = path;
    }
    protected override byte[] GetData()
    {
        return cacheBytes;
    }
    //从网络中受到数据，每帧自动调用的方法
    protected override bool ReceiveData(byte[] data, int dataLength)
    {
        Debug.Log("收到数据，长度为：" + dataLength);
        data.CopyTo(cacheBytes, index);
        index += dataLength;
        return true;
    }
    //从服务器收到Content-Lenght标头时，自动调用
    protected override void ReceiveContentLengthHeader(ulong contentLength)
    {
        //根据标头，决定接收数据的缓存容器大小
        cacheBytes = new byte[contentLength];
    }
    //消息接收完毕后自动调用的方法
    protected override void CompleteContent()
    {
        Debug.Log("消息接收完成");
        //自定义处理
        File.WriteAllBytes(savePath, cacheBytes);
    }
}
