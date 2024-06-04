using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Lesson43 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print("是否是小端模式：" + BitConverter.IsLittleEndian);

        int i = 99;
        byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(i));
        int receiveInt = BitConverter.ToInt32(bytes, 0);
        receiveInt = IPAddress.NetworkToHostOrder(receiveInt);

        if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
