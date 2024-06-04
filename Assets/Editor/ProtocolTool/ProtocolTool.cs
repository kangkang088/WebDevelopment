using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class ProtocolTool
{
    private static string PROTOCOL_INFO_PATH = Application.dataPath + "/Editor/ProtocolTool/ProtocolInfo.xml";
    private static GenerateCSharp generateCSharp = new GenerateCSharp();
    [MenuItem("ProtocolTool/GenerateCSharpCode")]
    private static void GenerateCSharp()
    {
        //1.读取xml相关信息
        XmlNodeList list = GetNodes("enum");
        //根据信息，拼接字符串
        generateCSharp.GenerateEnum(list);
        generateCSharp.GenerateData(GetNodes("data"));
        generateCSharp.GenerateMsg(GetNodes("message"));
        //刷新
        AssetDatabase.Refresh();
    }
    [MenuItem("ProtocolTool/GenerateCCode")]
    private static void GenerateC()
    {
        Debug.Log("生成C代码");
    }
    [MenuItem("ProtocolTool/GenerateCPPCode")]
    private static void GenerateCPP()
    {
        Debug.Log("生成C++代码");
    }
    private static XmlNodeList GetNodes(string nodeName)
    {
        XmlDocument xml = new XmlDocument();
        xml.Load(PROTOCOL_INFO_PATH);
        XmlNode root = xml.SelectSingleNode("messages");
        return root.SelectNodes(nodeName);
    }
}
