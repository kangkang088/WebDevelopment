using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerInfo
{
    public int lev;
    public string name;
    public short atk;
    public bool sex;
    public byte[] GetBytes()
    {
        int indexNumber = sizeof(int) + //lev  
                          sizeof(int) + //name的长度
                          Encoding.UTF8.GetBytes(name).Length + //name本身的内容
                          sizeof(short) + //atk
                          sizeof(bool); //sex
                                        //声明一个装载信息的字节数组容器
        byte[] playerBytes = new byte[indexNumber];
        //将对象中的信息转换为字节数组，存到容器中
        int index = 0;
        //存等级
        BitConverter.GetBytes(lev).CopyTo(playerBytes, index);
        index += sizeof(int);
        //存姓名
        byte[] strBytes = Encoding.UTF8.GetBytes(name);
        int num = strBytes.Length;
        //先存姓名字符串的长度
        BitConverter.GetBytes(num).CopyTo(playerBytes, index);
        index += sizeof(int);
        //再存姓名字符串内容
        strBytes.CopyTo(playerBytes, index);
        index += num;
        BitConverter.GetBytes(atk).CopyTo(playerBytes, index);
        index += sizeof(short);
        BitConverter.GetBytes(sex).CopyTo(playerBytes, index);
        index += sizeof(bool);
        return playerBytes;
    }
}
public class Lesson3 : MonoBehaviour
{
    void Start()
    {
        // byte[] bytes = BitConverter.GetBytes(1);
        // byte[] bytes1 = Encoding.UTF8.GetBytes("123");

        PlayerInfo info = new PlayerInfo();
        info.lev = 10;
        info.name = "kangkang";
        info.atk = 88;
        info.sex = false;

        //确定要存储的对象的字节数组的长度
        int indexNumber = sizeof(int) + //lev  
                          sizeof(int) + //name的长度
                          Encoding.UTF8.GetBytes(info.name).Length + //name本身的内容
                          sizeof(short) + //atk
                          sizeof(bool); //sex
        //声明一个装载信息的字节数组容器
        byte[] playerBytes = new byte[indexNumber];
        //将对象中的信息转换为字节数组，存到容器中
        int index = 0;
        //存等级
        BitConverter.GetBytes(info.lev).CopyTo(playerBytes, index);
        index += sizeof(int);
        //存姓名
        byte[] strBytes = Encoding.UTF8.GetBytes(info.name);
        int num = strBytes.Length;
        //先存姓名字符串的长度
        BitConverter.GetBytes(num).CopyTo(playerBytes, index);
        index += sizeof(int);
        //再存姓名字符串内容
        strBytes.CopyTo(playerBytes, index);
        index += num;
        BitConverter.GetBytes(info.atk).CopyTo(playerBytes, index);
        index += sizeof(short);
        BitConverter.GetBytes(info.sex).CopyTo(playerBytes, index);
        index += sizeof(bool);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
