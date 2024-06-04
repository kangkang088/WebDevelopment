using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson22 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FtpMgr.Instance.DeleteFile("pic.bmp", (result) =>
        {
            if (result == true)
            {
                print("删除成功");
            }
            else
            {
                print("删除失败");
            }
        });

        FtpMgr.Instance.GetFileSize("BigImage.jpg", (size) =>
        {
            print("文件大小为：" + size);
        });
        FtpMgr.Instance.CreateDirectory("KangTest", (b) =>
        {
            if (b == true)
                print("创建成功");
            else
                print("创建失败");
        });
        FtpMgr.Instance.GetFileList(null, (list) =>
        {
            if (list == null)
            {
                print("获取文件列表失败");
            }
            foreach (string name in list)
            {
                print(name);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
