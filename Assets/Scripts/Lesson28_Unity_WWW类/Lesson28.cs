using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lesson28 : MonoBehaviour
{
    public RawImage rawImage;
    // Start is called before the first frame update
    void Start()
    {
        //WWW www = new WWW("http://192.168.1.9:8080/HTTPServer/BigImage.jpg");

        //AudioClip audioClip = www.GetAudioClip();

        // Texture2D tex = new Texture2D(100, 100);
        // www.LoadImageIntoTexture(tex);

        //WWW.LoadFromCacheOrDownload("http://192.168.1.9:8080/HTTPServer/test.assetbundle", 1);

        //1.HTTP
        //StartCoroutine(DownLoadHttp());
        //2.FTP
        //StartCoroutine(DownLoadFtp());
        //3.local
        //StartCoroutine(DownLoadLocal());
    }
    IEnumerator DownLoadLocal()
    {
        //unity不支持bmp
        WWW www = new WWW("file://" + Application.streamingAssetsPath + "/Test.bmp");
        while (!www.isDone)
        {
            print(www.bytesDownloaded);
            print(www.progress);
            yield return null;
        }
        print(www.bytesDownloaded);
        print(www.progress);
        if (www.error == null)
        {
            rawImage.texture = www.texture;
        }
        else
        {
            print(www.error);
        }
    }
    IEnumerator DownLoadHttp()
    {
        WWW www = new WWW("https://c-ssl.dtstatic.com/uploads/blog/202012/15/20201215151813_58a20.thumb.1000_0.jpeg");
        while (!www.isDone)
        {
            print(www.bytesDownloaded);
            print(www.progress);
            yield return null;
        }
        print(www.bytesDownloaded);
        print(www.progress);
        if (www.error == null)
        {
            rawImage.texture = www.texture;
        }
        else
        {
            print(www.error);
        }
    }
    IEnumerator DownLoadFtp()
    {
        WWW www = new WWW("ftp://127.0.0.1/BigImage.jpg");
        while (!www.isDone)
        {
            print(www.bytesDownloaded);
            print(www.progress);
            yield return null;
        }
        print(www.bytesDownloaded);
        print(www.progress);
        if (www.error == null)
        {
            rawImage.texture = www.texture;
        }
        else
        {
            print(www.error);
        }
    }
    void Update()
    {

    }
}
