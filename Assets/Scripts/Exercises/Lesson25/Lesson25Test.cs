using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Lesson25Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // HttpMgr.Instance.DownLoadFile("BigImage.jpg", Application.persistentDataPath + "/HttpMgr_DownLoad.jpg", (statusCode) =>
        // {
        //     if (statusCode == System.Net.HttpStatusCode.OK)
        //     {
        //         print("下载成功");
        //     }
        //     else
        //     {
        //         print("下载失败:" + statusCode);
        //     }
        // });
        HttpMgr.Instance.UpLoadFile("HttpMgrUpLoadTest.bmp", Application.streamingAssetsPath + "/Test.bmp", (statusCode) =>
        {
            if (statusCode == System.Net.HttpStatusCode.OK)
            {
                print("上传成功");
            }
            else
            {
                print("上传失败:" + statusCode);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
