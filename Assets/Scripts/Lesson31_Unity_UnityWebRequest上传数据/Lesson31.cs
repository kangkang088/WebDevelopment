using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Lesson31 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<IMultipartFormSection> dataList = new List<IMultipartFormSection>();

        dataList.Add(new MultipartFormDataSection(Encoding.UTF8.GetBytes("12121212")));
        dataList.Add(new MultipartFormDataSection("Name", "MrTang", Encoding.UTF8, "application/x-www-form-urlencoded"));
        dataList.Add(new MultipartFormDataSection("Msg", new byte[1024], "application/octet-strean"));

        dataList.Add(new MultipartFormFileSection(File.ReadAllBytes(Application.streamingAssetsPath + "/Test.bmp")));
        dataList.Add(new MultipartFormFileSection("上传的文件.jpg", File.ReadAllBytes(Application.streamingAssetsPath + "/Test.bmp")));
        dataList.Add(new MultipartFormFileSection("123123123", "text.txt"));
        dataList.Add(new MultipartFormFileSection("123123123", Encoding.UTF8, "text.txt"));
        dataList.Add(new MultipartFormFileSection("file", new byte[1024], "test.txt", ""));
        dataList.Add(new MultipartFormFileSection("file", "1213131", Encoding.UTF8, "test.txt"));

        //StartCoroutine(UpLoad());
        //StartCoroutine(UpLoadPut());
    }
    IEnumerator UpLoadPut()
    {
        UnityWebRequest req = UnityWebRequest.Put("http://192.168.1.9:8080/HTTPServer/", File.ReadAllBytes(Application.streamingAssetsPath + "/Test.bmp"));
        yield return req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.Success)
        {
            print("put成功");
        }
        else
        {
            print("上传失败：" + req.result + req.error + req.responseCode);
        }
    }
    IEnumerator UpLoad()
    {
        List<IMultipartFormSection> data = new List<IMultipartFormSection>();
        data.Add(new MultipartFormDataSection("Name", "MrTang"));
        //PlayerMsg playerMsg = new PlayerMsg();
        //data.Add(new MultipartFormDataSection("Msg", playerMsg.Writing()));
        data.Add(new MultipartFormFileSection("TestTest123.bmp", File.ReadAllBytes(Application.streamingAssetsPath + "/Test.bmp")));
        data.Add(new MultipartFormFileSection("123132", "Test123.txt"));
        UnityWebRequest req = UnityWebRequest.Post("http://192.168.1.9:8080/HTTPServer/", data);
        req.SendWebRequest();
        while (!req.isDone)
        {
            print(req.uploadedBytes);
            print(req.uploadProgress);
            yield return null;
        }
        print(req.uploadedBytes);
        print(req.uploadProgress);
        if (req.result == UnityWebRequest.Result.Success)
        {
            print("上传成功");
        }
        else
        {
            print("上传失败：" + req.responseCode + req.error + req.result);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
