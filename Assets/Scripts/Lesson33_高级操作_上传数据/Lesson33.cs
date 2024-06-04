using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Lesson33 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpLoad());
    }
    IEnumerator UpLoad()
    {
        UnityWebRequest req = new UnityWebRequest("http://192.168.1.9:8080/HTTPServer/", UnityWebRequest.kHttpVerbPOST);
        byte[] bytes = Encoding.UTF8.GetBytes("12133");
        //1.UploadHandlerRaw
        req.uploadHandler = new UploadHandlerRaw(bytes);

        //2.UploadHandlerFile
        req.uploadHandler = new UploadHandlerFile(Application.streamingAssetsPath + "/Test.bmp");
        //req.uploadHandler.contentType = "细分类型";
        yield return req.SendWebRequest();
        print(req.result);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
