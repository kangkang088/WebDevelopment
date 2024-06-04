using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUdpAsync : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (UdpAsyncNetMgr.Instance == null)
        {
            GameObject obj = new GameObject("UdpNet");
            obj.AddComponent<UdpAsyncNetMgr>();
        }

        UdpAsyncNetMgr.Instance.StartClient("127.0.0.1", 8080);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
