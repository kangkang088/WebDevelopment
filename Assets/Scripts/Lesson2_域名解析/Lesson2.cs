using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class Lesson2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Dns.GetHostName());

        // IPHostEntry iPHostEntry = Dns.GetHostEntry("www.baidu.com");
        // for (int i = 0; i < iPHostEntry.AddressList.Length; i++)
        // {
        //     print("IP地址：" + iPHostEntry.AddressList[i]);
        // }
        // for (int i = 0; i < iPHostEntry.Aliases.Length; i++)
        // {
        //     print("别名：" + iPHostEntry.Aliases[i]);
        // }
        // print("dns名：" + iPHostEntry.HostName);

        GetHostEntry();
    }
    public async void GetHostEntry()
    {
        Task<IPHostEntry> task = Dns.GetHostEntryAsync("www.baidu.com");
        await task;
        for (int i = 0; i < task.Result.AddressList.Length; i++)
        {
            print("IP地址：" + task.Result.AddressList[i]);
        }
        for (int i = 0; i < task.Result.Aliases.Length; i++)
        {
            print("别名：" + task.Result.Aliases[i]);
        }
        print("dns名：" + task.Result.HostName);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
