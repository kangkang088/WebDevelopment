using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using UnityEngine;

public class TestInfo : BaseData
{
    public short lev;
    public Player player;
    public int hp;
    public string name;
    public bool sex;
    public override int GetBytesNum()
    {
        return 2 + player.GetBytesNum() + 4 + 4 + Encoding.UTF8.GetBytes(name).Length + 1;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        lev = ReadShort(bytes, ref index);
        player = ReadData<Player>(bytes, ref index);
        hp = ReadInt(bytes, ref index);
        name = ReadString(bytes, ref index);
        sex = ReadBool(bytes, ref index);
        return index - beginIndex;
    }

    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        WriteShort(bytes, lev, ref index);
        WriteData(bytes, player, ref index);
        WriteInt(bytes, hp, ref index);
        WriteString(bytes, name, ref index);
        WriteBool(bytes, sex, ref index);
        return bytes;
    }
}
public class Player : BaseData
{
    public int atk;
    public override int GetBytesNum()
    {
        return sizeof(int);
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        atk = ReadInt(bytes, ref index);
        return index - beginIndex;
    }

    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        WriteInt(bytes, atk, ref index);
        return bytes;
    }
}
public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TestInfo info = new TestInfo();
        info.lev = 87;
        info.player = new Player();
        info.player.atk = 77;
        info.hp = 100;
        info.name = "kangkang";
        info.sex = true;
        byte[] bytes = info.Writing();

        TestInfo info2 = new TestInfo();
        info2.Reading(bytes);
        print(info2.lev);
        print(info2.player.atk);
        print(info2.sex);
        print(info2.name);
        print(info2.hp);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
