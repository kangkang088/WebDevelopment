using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Lesson1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        byte[] ipAddress = new byte[] { 118, 108, 111, 1 };
        IPAddress ip1 = new IPAddress(ipAddress);

        IPAddress ip2 = new IPAddress(0x79666F0B);

        IPAddress ip3 = IPAddress.Parse("118.108.111.1");

        IPEndPoint iPEndPoint = new IPEndPoint(0x79666F0B, 8080);
        IPEndPoint iPEndPoint2 = new IPEndPoint(IPAddress.Parse("118.108.111.1"), 8080);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
