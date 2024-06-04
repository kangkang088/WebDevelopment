using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Lesson28_Test : MonoBehaviour
{
    public RawImage rawImage;
    // Start is called before the first frame update
    void Start()
    {
        if (NetWWWMgr.Instance == null)
        {
            GameObject obj = new GameObject("WWW");
            obj.AddComponent<NetWWWMgr>();
        }
        // NetWWWMgr.Instance.LoadRes<Texture>("http://192.168.1.9:8080/HTTPServer/BigImage.jpg", (texture) =>
        // {
        //     rawImage.texture = texture;
        // });
        NetWWWMgr.Instance.UpLoadFile("TestTest.bmp", Application.streamingAssetsPath + "/Test.bmp", (result) =>
        {
            if (result == UnityWebRequest.Result.Success)
            {
                print("上传成功");
            }
            else
            {
                print(result);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
