using System.Collections;
using System.Collections.Generic;
using GamePlayerTest;
using UnityEngine;

public class Lesson40 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TestMsg testMsg = new TestMsg();
        testMsg.TestBool = true;
        testMsg.ListInt.Add(1);
        print(testMsg.ListInt[0]);
        testMsg.TestMap.Add(1, "12");
        print(testMsg.TestMap[1]);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
