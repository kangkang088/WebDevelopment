using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Lesson35 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //1.读取xml文件信息
        XmlDocument xml = new XmlDocument();
        xml.Load(Application.dataPath + "/Scripts/Lesson35_协议(消息)配置/Lesson35.xml");
        //2.读取各个节点的元素
        //根节点
        XmlNode root = xml.SelectSingleNode("messages");
        //枚举节点
        XmlNodeList enumList = root.SelectNodes("enum");
        foreach (XmlNode enumNode in enumList)
        {
            print("*************");
            print("*****枚举*****");
            print("枚举名字：" + enumNode.Attributes["name"].Value);
            print("枚举命名空间：" + enumNode.Attributes["namespace"].Value);
            print("*****枚举成员*****");
            XmlNodeList fieldList = enumNode.SelectNodes("field");
            foreach (XmlNode field in fieldList)
            {
                string str = field.Attributes["name"].Value;
                if (field.InnerText != "")
                    str += " = " + field.InnerText;
                str += ",";
                print(str);
            }
        }
        //数据结构类节点
        XmlNodeList dataList = root.SelectNodes("data");
        foreach (XmlNode data in dataList)
        {
            print("*************");
            print("*****数据结构类*****");
            print("类名：" + data.Attributes["name"].Value);
            print("类所在命名空间：" + data.Attributes["namespace"].Value);
            print("*****数据结构类成员*****");
            XmlNodeList fieldList = data.SelectNodes("field");
            foreach (XmlNode field in fieldList)
            {
                print(field.Attributes["type"].Value + " " + field.Attributes["name"].Value);
            }
        }
        //消息类节点
        XmlNodeList msgList = root.SelectNodes("message");
        foreach (XmlNode data in msgList)
        {
            print("*************");
            print("*****数据结构类*****");
            print("消息类ID:" + data.Attributes["id"].Value);
            print("消息类名：" + data.Attributes["name"].Value);
            print("消息类所在命名空间：" + data.Attributes["namespace"].Value);
            print("*****消息类成员*****");
            XmlNodeList fieldList = data.SelectNodes("field");
            foreach (XmlNode field in fieldList)
            {
                print(field.Attributes["type"].Value + " " + field.Attributes["name"].Value);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
