using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Lesson4 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        byte[] bytes = BitConverter.GetBytes(99);
        int v = BitConverter.ToInt32(bytes, 0);
        print(v);

        byte[] bytes1 = Encoding.UTF8.GetBytes("12312313");
        string str = Encoding.UTF8.GetString(bytes1, 0, bytes1.Length);
        print(str);

        PlayerInfo info = new PlayerInfo();
        info.lev = 10;
        info.name = "kangkang";
        info.atk = 88;
        info.sex = false;
        byte[] playerBytes = info.GetBytes();

        PlayerInfo info2 = new PlayerInfo();
        int index = 0;
        info2.lev = BitConverter.ToInt32(playerBytes, index);
        print(info2.lev);
        index += 4;

        int length = BitConverter.ToInt32(playerBytes, index);
        index += 4;
        info2.name = Encoding.UTF8.GetString(playerBytes, index, length);
        print(info2.name);
        index += length;

        info2.atk = BitConverter.ToInt16(playerBytes, index);
        print(info2.atk);
        index += 2;

        info2.sex = BitConverter.ToBoolean(playerBytes, index);
        print(info2.sex);
        index += 1;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
