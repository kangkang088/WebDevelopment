using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson20_Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FtpMgr.Instance.UpLoadFile("Test2.bmp", Application.streamingAssetsPath + "/Test.bmp", () =>
        {
            print("上传结束");
        });
        print("我比上面先执行的！");
        FtpMgr.Instance.DownLoadFile("BigImage.jpg", Application.persistentDataPath + "/DownLoad.jpg", () =>
        {
            print("下载结束");
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
