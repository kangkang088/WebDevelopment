using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ProtoBufTool
{
    //协议配置文件路径
    private static string PROTO_PATH = "D:\\ProtocolBuffers\\proto";
    //协议生成可执行文件的路径
    private static string PROTOC_PATH = "D:\\ProtocolBuffers\\protoc.exe";
    //csharp文件生成路径
    private static string CSHARP_PATH = "D:\\ProtocolBuffers\\csharp";
    //c++文件生成路径
    private static string CPP_PATH = "D:\\ProtocolBuffers\\cpp";
    //java
    private static string JAVA_PATH = "D:\\ProtocolBuffers\\java";
    [MenuItem("ProtoBufTool/GenerateCSharp")]
    private static void GenerateCSharp()
    {
        Generate("csharp_out", CSHARP_PATH);
    }
    [MenuItem("ProtoBufTool/GenerateCPP")]
    private static void GenerateCPP()
    {
        Generate("cpp_out", CPP_PATH);
    }
    [MenuItem("ProtoBufTool/GenerateJAVA")]
    private static void GenerateJAVA()
    {
        Generate("java_out", JAVA_PATH);
    }
    private static void Generate(string outCmd, string outPath)
    {
        DirectoryInfo directoryInfo = Directory.CreateDirectory(PROTO_PATH);
        FileInfo[] files = directoryInfo.GetFiles();
        //遍历所有后缀为proto的文件，为其生成协议脚本
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Extension == ".proto")
            {
                //cmd类 
                Process cmd = new Process();
                //protoc.exe的路径
                cmd.StartInfo.FileName = PROTOC_PATH;
                //命令
                cmd.StartInfo.Arguments = $"-I{PROTO_PATH} --{outCmd}={outPath} {files[i]}";
                //执行
                cmd.Start();

                UnityEngine.Debug.Log(files[i] + ":生成结束");
            }
        }
        UnityEngine.Debug.Log("Generate Code Over");
    }
}
