using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UDPMain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (UdpNetMgr.Instance == null)
        {
            GameObject obj = new GameObject("UDPNet");
            obj.AddComponent<UdpNetMgr>();
        }
        UdpNetMgr.Instance.StartClient("127.0.0.1", 8080);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
