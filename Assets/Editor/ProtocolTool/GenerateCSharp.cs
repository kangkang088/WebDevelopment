using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using NUnit.Framework.Constraints;
using PlasticPipe.Client;
using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.XR.WSA;

namespace GamePlayer
{
    public enum E_PLAYER_TYPE
    {
        MAIN = 1, OTHER,
    }
}
namespace GamePlayer
{
    public class PlayerData : BaseData
    {
        int id;
        float atk;
        bool sex;
        long lev;
        List<int> list;
        Dictionary<int, string> dic;
        int[] arrays;
        public override int GetBytesNum()
        {
            throw new System.NotImplementedException();
        }

        public override int Reading(byte[] bytes, int beginIndex = 0)
        {
            throw new System.NotImplementedException();
        }

        public override byte[] Writing()
        {
            throw new System.NotImplementedException();
        }
    }
}
public class GenerateCSharp
{
    private string SAVE_PATH = Application.dataPath + "/Scripts/Protocol/";
    //生成枚举
    public void GenerateEnum(XmlNodeList nodes)
    {
        string namespaceStr = "";
        string enumNameStr = "";
        string fieldStr = "";
        foreach (XmlNode enumNode in nodes)
        {
            namespaceStr = enumNode.Attributes["namespace"].Value;
            enumNameStr = enumNode.Attributes["name"].Value;
            XmlNodeList enumFields = enumNode.SelectNodes("field");
            fieldStr = "";
            foreach (XmlNode field in enumFields)
            {
                fieldStr += "\t\t" + field.Attributes["name"].Value;
                if (field.InnerText != "")
                    fieldStr += " = " + field.InnerText;
                fieldStr += ",\r\n";
            }
            string enumStr = $"namespace {namespaceStr}\r\n" +
                             "{\r\n" +
                             $"\tpublic enum {enumNameStr}\r\n" +
                             "\t{\r\n" +
                             $"{fieldStr}" +
                             "\t}\r\n" +
                             "}";
            //保存文件
            string path = SAVE_PATH + namespaceStr + "/Enum/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.WriteAllText(path + enumNameStr + ".cs", enumStr);
        }
        Debug.Log("枚举生成结束");
    }
    //生成数据结构类
    public void GenerateData(XmlNodeList nodes)
    {
        string namespaceStr = "";
        string classNameStr = "";
        string fieldStr = "";
        string getBytesNumStr = "";
        string writingStr = "";
        string readingStr = "";

        foreach (XmlNode dataNode in nodes)
        {
            namespaceStr = dataNode.Attributes["namespace"].Value;
            classNameStr = dataNode.Attributes["name"].Value;
            XmlNodeList fields = dataNode.SelectNodes("field");

            fieldStr = GetFieldStr(fields);
            getBytesNumStr = GetGetBytesNum(fields);
            writingStr = GetWriting(fields);
            readingStr = GetReadingStr(fields);

            string dataStr = "using System.Collections.Generic;\r\n" + "using System.Text;\r\n" + "using System;\r\n" +
                             $"namespace {namespaceStr}\r\n" +
                             "{\r\n" +
                             $"\tpublic class {classNameStr} : BaseData\r\n" +
                             "\t{\r\n" +
                             $"{fieldStr}" +

                             "\t\tpublic override int GetBytesNum()\r\n" +
                             "\t\t{\r\n" +
                             "\t\t\tint num = 0;\r\n" +
                             $"{getBytesNumStr}" +
                             "\t\t\treturn num;\r\n" +
                             "\t\t}\r\n" +

                             "\t\tpublic override byte[] Writing()\r\n" +
                             "\t\t{\r\n" +
                             "\t\t\tint index = 0;\r\n" +
                             "\t\t\tbyte[] bytes = new byte[GetBytesNum()];\r\n" +
                             $"{writingStr}" +
                             "\t\t\treturn bytes;\r\n" +
                             "\t\t}\r\n" +

                             "\t\tpublic override int Reading(byte[] bytes,int beginIndex = 0)\r\n" +
                             "\t\t{\r\n" +
                             "\t\t\tint index = beginIndex;\r\n" +
                             $"{readingStr}" +
                             "\t\t\treturn index - beginIndex;\r\n" +
                             "\t\t}\r\n" +

                             "\t}\r\n" +
                             "}";
            //保存文件
            string path = SAVE_PATH + namespaceStr + "/Data/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.WriteAllText(path + classNameStr + ".cs", dataStr);
        }
    }
    private string GetGetBytesNum(XmlNodeList fields)
    {
        string bytesNumStr = "";
        string type = "";
        string name = "";
        foreach (XmlNode field in fields)
        {
            type = field.Attributes["type"].Value;
            name = field.Attributes["name"].Value;
            if (type == "list")
            {
                string T = field.Attributes["T"].Value;
                bytesNumStr += "\t\t\tnum += 2;\r\n" +
                               "\t\t\tfor(int i = 0;i < " + name + ".Count;++i)\r\n" +
                               "\t\t\t\tnum += " +
                               GetValueBytesNum(T, name + "[i]") +
                               ";\r\n";

            }
            else if (type == "array")
            {
                string data = field.Attributes["data"].Value;
                bytesNumStr += "\t\t\tnum += 2;\r\n" +
                               "\t\t\tfor(int i = 0;i < " + name + ".Length;++i)\r\n" +
                               "\t\t\t\tnum += " +
                               GetValueBytesNum(data, name + "[i]") +
                               ";\r\n";
            }
            else if (type == "dic")
            {
                string TKey = field.Attributes["TKey"].Value;
                string TValue = field.Attributes["TValue"].Value;
                bytesNumStr += "\t\t\tnum += 2;\r\n" +
                               "\t\t\tforeach(" + TKey + " key in " + name + ".Keys)\r\n" +
                               "\t\t\t{\r\n" +
                               "\t\t\t\tnum += " + GetValueBytesNum(TKey, "key") + ";\r\n" +
                               "\t\t\t\tnum += " + GetValueBytesNum(TValue, name + "[key]") + ";\r\n" +
                               "\t\t\t}\r\n";
            }
            else
            {
                bytesNumStr += "\t\t\tnum += " + GetValueBytesNum(type, name) + ";\r\n";
            }
        }
        return bytesNumStr;
    }
    private string GetWriting(XmlNodeList fields)
    {
        string writingStr = "";
        string type = "";
        string name = "";
        foreach (XmlNode field in fields)
        {
            type = field.Attributes["type"].Value;
            name = field.Attributes["name"].Value;
            if (type == "list")
            {
                string T = field.Attributes["T"].Value;
                writingStr += "\t\t\tWriteShort(bytes,(short)" + name + ".Count,ref index);\r\n";
                writingStr += "\t\t\tfor(int i = 0;i < " + name + ".Count; ++i)\r\n";
                writingStr += "\t\t\t\t" + GetFieldWritingStr(T, name + "[i]") + "\r\n";
            }
            else if (type == "array")
            {
                string data = field.Attributes["data"].Value;
                writingStr += "\t\t\tWriteShort(bytes,(short)" + name + ".Length,ref index);\r\n";
                writingStr += "\t\t\tfor(int i = 0;i < " + name + ".Length; ++i)\r\n";
                writingStr += "\t\t\t\t" + GetFieldWritingStr(data, name + "[i]") + "\r\n";
            }
            else if (type == "dic")
            {
                string TKey = field.Attributes["TKey"].Value;
                string TValue = field.Attributes["TValue"].Value;
                writingStr += "\t\t\tWriteShort(bytes,(short)" + name + ".Count,ref index);\r\n";
                writingStr += "\t\t\tforeach(" + TKey + " key in " + name + ".Keys)\r\n";
                writingStr += "\t\t\t{\r\n";
                writingStr += "\t\t\t\t" + GetFieldWritingStr(TKey, "key") + "\r\n";
                writingStr += "\t\t\t\t" + GetFieldWritingStr(TValue, name + "[key]") + "\r\n";
                writingStr += "\t\t\t}\r\n";

            }
            else
            {
                writingStr += "\t\t\t" + GetFieldWritingStr(type, name) + "\r\n";
            }
        }
        return writingStr;
    }
    private string GetReadingStr(XmlNodeList fields)
    {
        string readingStr = "";
        string type = "";
        string name = "";
        foreach (XmlNode field in fields)
        {
            type = field.Attributes["type"].Value;
            name = field.Attributes["name"].Value;
            if (type == "list")
            {
                string T = field.Attributes["T"].Value;
                readingStr += "\t\t\t" + name + " = new List<" + T + ">();\r\n";
                readingStr += "\t\t\tshort " + name + "Count = ReadShort(bytes,ref index);\r\n";
                readingStr += "\t\t\tfor(int i = 0;i < " + name + "Count;++i)\r\n";
                readingStr += "\t\t\t\t" + name + ".Add(" + GetFieldReadingStr(T) + ");\r\n";
            }
            else if (type == "array")
            {
                string data = field.Attributes["data"].Value;
                readingStr += "\t\t\tshort " + name + "Length = ReadShort(bytes,ref index);\r\n";
                readingStr += "\t\t\t" + name + " = new " + data + "[" + name + "Length];\r\n";
                readingStr += "\t\t\tfor(int i = 0;i < " + name + "Length;++i)\r\n";
                readingStr += "\t\t\t\t" + name + "[i] = " + GetFieldReadingStr(data) + ";\r\n";
            }
            else if (type == "dic")
            {
                string TKey = field.Attributes["TKey"].Value;
                string TValue = field.Attributes["TValue"].Value;
                readingStr += "\t\t\t" + name + " = new Dictionary<" + TKey + "," + TValue + ">();\r\n";
                readingStr += "\t\t\tshort " + name + "Count = ReadShort(bytes,ref index);\r\n";
                readingStr += "\t\t\tfor(int i = 0;i < " + name + "Count;++i)\r\n";
                readingStr += "\t\t\t\t" + name + ".Add(" + GetFieldReadingStr(TKey) + "," +
                                                            GetFieldReadingStr(TValue) + ");\r\n";

            }
            else if (type == "enum")
            {
                string data = field.Attributes["data"].Value;
                readingStr += "\t\t\t" + name + " = (" + data + ")ReadInt(bytes,ref index);\r\n";
            }
            else
            {
                readingStr += "\t\t\t" + name + " = " + GetFieldReadingStr(type) + ";\r\n";
            }
        }
        return readingStr;
    }
    private string GetFieldReadingStr(string type)
    {
        switch (type)
        {
            case "byte":
                return "ReadByte(bytes,ref index)";
            case "int":
                return "ReadInt(bytes,ref index)";
            case "short":
                return "ReadShort(bytes,ref index)";
            case "bool":
                return "ReadBool(bytes,ref index)";
            case "long":
                return "ReadLong(bytes,ref index)";
            case "float":
                return "ReadFloat(bytes,ref index)";
            case "string":
                return "ReadString(bytes,ref index)";
            default:
                return "ReadData<" + type + ">(bytes,ref index)";
        }
    }
    private string GetFieldWritingStr(string type, string name)
    {
        switch (type)
        {
            case "byte":
                return "WriteByte(bytes, " + name + ",ref index);";
            case "int":
                return "WriteInt(bytes, " + name + ",ref index);";
            case "short":
                return "WriteShort(bytes, " + name + ",ref index);";
            case "long":
                return "WriteLong(bytes, " + name + ",ref index);";
            case "float":
                return "WriteFloat(bytes, " + name + ",ref index);";
            case "bool":
                return "WriteBool(bytes, " + name + ",ref index);";
            case "string":
                return "WriteString(bytes, " + name + ",ref index);";
            case "enum":
                return "WriteInt(bytes, Convert.ToInt32(" + name + "),ref index);";
            default:
                return "WriteData(bytes, " + name + ",ref index);";
        }
    }
    private string GetValueBytesNum(string type, string name)
    {
        switch (type)
        {
            case "int":
            case "float":
            case "enum":
                return "4";
            case "long":
                return "8";
            case "byte":
            case "bool":
                return "1";
            case "short":
                return "2";
            case "string":
                return "4 + Encoding.UTF8.GetByteCount(" + name + ")";
            default:
                return name + ".GetBytesNum()";
        }
    }
    private string GetFieldStr(XmlNodeList fields)
    {
        string fieldStr = "";
        foreach (XmlNode field in fields)
        {
            string type = field.Attributes["type"].Value;
            string fieldName = field.Attributes["name"].Value;
            if (type == "list")
            {
                string T = field.Attributes["T"].Value;
                fieldStr += "\t\tpublic " + "List<" + T + "> ";
            }
            else if (type == "array")
            {
                string data = field.Attributes["data"].Value;
                fieldStr += "\t\tpublic " + data + "[] ";
            }
            else if (type == "dic")
            {
                string TKey = field.Attributes["TKey"].Value;
                string TValue = field.Attributes["TValue"].Value;
                fieldStr += "\t\tpublic Dictionary<" + TKey + ", " + TValue + "> ";
            }
            else if (type == "enum")
            {
                string data = field.Attributes["data"].Value;
                fieldStr += "\t\tpublic " + data + " ";
            }
            else
            {
                fieldStr += "\t\tpublic " + type + " ";
            }
            fieldStr += fieldName + ";\r\n";
        }
        return fieldStr;
    }
    //生成消息类
    public void GenerateMsg(XmlNodeList nodes)
    {
        string idStr = "";
        string namespaceStr = "";
        string classNameStr = "";
        string fieldStr = "";
        string getBytesNumStr = "";
        string writingStr = "";
        string readingStr = "";

        foreach (XmlNode dataNode in nodes)
        {
            idStr = dataNode.Attributes["id"].Value;
            namespaceStr = dataNode.Attributes["namespace"].Value;
            classNameStr = dataNode.Attributes["name"].Value;
            XmlNodeList fields = dataNode.SelectNodes("field");

            fieldStr = GetFieldStr(fields);
            getBytesNumStr = GetGetBytesNum(fields);
            writingStr = GetWriting(fields);
            readingStr = GetReadingStr(fields);

            string dataStr = "using System.Collections.Generic;\r\n" + "using System.Text;\r\n" + "using System;\r\n" +
                             $"namespace {namespaceStr}\r\n" +
                             "{\r\n" +
                             $"\tpublic class {classNameStr} : BaseMsg\r\n" +
                             "\t{\r\n" +
                             $"{fieldStr}" +

                             "\t\tpublic override int GetBytesNum()\r\n" +
                             "\t\t{\r\n" +
                             "\t\t\tint num = 8;\r\n" +
                             $"{getBytesNumStr}" +
                             "\t\t\treturn num;\r\n" +
                             "\t\t}\r\n" +

                             "\t\tpublic override byte[] Writing()\r\n" +
                             "\t\t{\r\n" +
                             "\t\t\tint index = 0;\r\n" +
                             "\t\t\tbyte[] bytes = new byte[GetBytesNum()];\r\n" +
                             "\t\t\tWriteInt(bytes,GetID(),ref index);\r\n" +
                             "\t\t\tWriteInt(bytes,bytes.Length - 8,ref index);\r\n" +
                             $"{writingStr}" +
                             "\t\t\treturn bytes;\r\n" +
                             "\t\t}\r\n" +

                             "\t\tpublic override int Reading(byte[] bytes,int beginIndex = 0)\r\n" +
                             "\t\t{\r\n" +
                             "\t\t\tint index = beginIndex;\r\n" +
                             $"{readingStr}" +
                             "\t\t\treturn index - beginIndex;\r\n" +
                             "\t\t}\r\n" +

                             "\t\tpublic override int GetID()\r\n" +
                             "\t\t{\r\n" +
                             "\t\t\treturn " + idStr + ";\r\n" +
                             "\t\t}\r\n" +

                             "\t}\r\n" +
                             "}";
            //保存文件
            string path = SAVE_PATH + namespaceStr + "/Msg/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.WriteAllText(path + classNameStr + ".cs", dataStr);
        }

    }
}
