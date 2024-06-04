using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Lesson24 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        HttpWebRequest req = HttpWebRequest.Create(new Uri("http://192.168.1.9:8080/HTTPServer/")) as HttpWebRequest;
        //req.Abort();
        System.IO.Stream stream = req.GetRequestStream();
        HttpWebResponse res = req.GetResponse() as HttpWebResponse;
        res.Close();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
