using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Lesson29 : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(UpLoadData());
    }
    IEnumerator UpLoadData()
    {
        WWWForm data = new WWWForm();
        data.AddField("Name", "MrTang", Encoding.UTF8);
        data.AddField("Age", 100);
        data.AddBinaryData("file", File.ReadAllBytes(Application.streamingAssetsPath + "/Test.bmp"));
        WWW www = new WWW("http://192.168.1.9:8080/HTTPServer/", data);
        yield return www;
        if (www.error == null)
        {
            print("上传成功");
        }
        else
        {
            print("上传出错：" + www.error);
        }
    }
    void Update()
    {

    }
}
