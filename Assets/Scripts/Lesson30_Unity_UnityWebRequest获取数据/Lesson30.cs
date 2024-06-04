using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Lesson30 : MonoBehaviour
{
    public RawImage rawImage;
    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(LoadText());
        //StartCoroutine(LoadTexture());
        //StartCoroutine(LoadAB());
    }
    IEnumerator LoadAB()
    {
        UnityWebRequest req = UnityWebRequestAssetBundle.GetAssetBundle("http://192.168.1.9:8080/HTTPServer/lua.assetbundle");
        yield return req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.Success)
        {
            //AssetBundle ab = DownloadHandlerAssetBundle.GetContent(req);
            AssetBundle ab = (req.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
            print(ab.name);
        }
        else
        {
            print(req.result + req.error + req.responseCode);
        }
    }
    IEnumerator LoadTexture()
    {
        UnityWebRequest req = UnityWebRequestTexture.GetTexture("http://192.168.1.9:8080/HTTPServer/BigImage.jpg");
        yield return req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.Success)
        {
            //rawImage.texture = (req.downloadHandler as DownloadHandlerTexture).texture;
            rawImage.texture = DownloadHandlerTexture.GetContent(req);
        }
        else
        {
            print("获取失败：" + req.result + req.responseCode + req.error);
        }
    }
    IEnumerator LoadText()
    {
        UnityWebRequest req = UnityWebRequest.Get("http://192.168.1.9:8080/HTTPServer/Test.txt");
        yield return req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.Success)
        {
            //直接得到文本
            //可以通过handler，我们可以自定义处理 获取的消息
            print(req.downloadHandler.text);
            //直接得到字节数据
            byte[] bytes = req.downloadHandler.data;
        }
        else
        {
            print("获取失败：" + req.result + req.error + req.responseCode);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
