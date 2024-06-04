using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GamePlayerTest;
using Google.Protobuf;
using UnityEngine;

public class Lesson41 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TestMsg msg = new TestMsg();
        msg.ListInt.Add(1);
        msg.TestBool = false;
        msg.TestD = 5.5;
        msg.TestInt32 = 99;
        msg.TestMap.Add(1, "MrTang");
        msg.TestMsg2 = new TestMsg2();
        msg.TestMsg2.TestInt32 = 88;
        msg.TestMsg3 = new TestMsg.Types.TestMsg3();
        msg.TestMsg3.TestInt32 = 66;

        msg.TestHeart = new GameSystemTest.HeartMsg();
        msg.TestHeart.Time = 7777;

        print(Application.persistentDataPath);
        using (FileStream fs = File.Create(Application.persistentDataPath + "/TestMsg.tang"))
        {
            msg.WriteTo(fs);
        }

        TestMsg msg2 = new TestMsg();
        using (FileStream fs = File.OpenRead(Application.persistentDataPath + "/TestMsg.tang"))
        {
            msg2 = TestMsg.Parser.ParseFrom(fs);
        }
        print(msg2.TestMap[1]);
        print(msg2.ListInt[0]);
        print(msg2.TestD);

        byte[] bytes = null;
        using (MemoryStream ms = new MemoryStream())
        {
            msg.WriteTo(ms);
            bytes = ms.ToArray();
            print("字节数组长度：" + bytes.Length);
        }

        using (MemoryStream ms = new MemoryStream(bytes))
        {
            TestMsg msg3 = null;
            msg3 = TestMsg.Parser.ParseFrom(ms);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
