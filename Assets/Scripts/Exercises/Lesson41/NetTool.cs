using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Google.Protobuf;
using UnityEngine;

public static class NetTool
{
    public static byte[] GetProtoBytes(IMessage message)
    {
        // byte[] bytes = null;
        // using (MemoryStream ms = new MemoryStream())
        // {
        //     message.WriteTo(ms);
        //     bytes = ms.ToArray();
        // }
        // return bytes;

        return message.ToByteArray();
    }
    public static T GetProtoMsg<T>(byte[] bytes) where T : class, IMessage
    {
        Type type = typeof(T);
        PropertyInfo propertyInfo = type.GetProperty("Parser");
        object parserObj = propertyInfo.GetValue(null, null);
        Type parserType = parserObj.GetType();
        MethodInfo methodInfo = parserType.GetMethod("ParseFrom", new Type[] { typeof(byte[]) });
        object message = methodInfo?.Invoke(parserObj, new object[] { bytes });
        return message as T;
    }
}
