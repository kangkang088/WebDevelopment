using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Lesson7 : MonoBehaviour
{
    public Button button;
    public Button button1;
    public Button button2;
    public Button button3;
    public InputField input;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(() =>
        {
            PlayerMsg playerMsg = new PlayerMsg();
            playerMsg.playerID = 1100;
            playerMsg.playerData = new PlayerData();
            playerMsg.playerData.name = "wudi";
            playerMsg.playerData.atk = 500;
            playerMsg.playerData.lev = 100;
            NetMgr.Instance.Send(playerMsg);
        });
        //黏包
        button1.onClick.AddListener(() =>
        {
            PlayerMsg playerMsg = new PlayerMsg();
            playerMsg.playerID = 1001;
            playerMsg.playerData = new PlayerData();
            playerMsg.playerData.name = "wudi1";
            playerMsg.playerData.atk = 1;
            playerMsg.playerData.lev = 1;

            PlayerMsg playerMsg2 = new PlayerMsg();
            playerMsg2.playerID = 1002;
            playerMsg2.playerData = new PlayerData();
            playerMsg2.playerData.name = "wudi2";
            playerMsg2.playerData.atk = 2;
            playerMsg2.playerData.lev = 2;

            byte[] bytes = new byte[playerMsg.GetBytesNum() + playerMsg2.GetBytesNum()];
            playerMsg.Writing().CopyTo(bytes, 0);
            playerMsg2.Writing().CopyTo(bytes, playerMsg.GetBytesNum());

            NetMgr.Instance.SendTest(bytes);
        });
        //分包
        button2.onClick.AddListener(async () =>
        {
            PlayerMsg playerMsg3 = new PlayerMsg();
            playerMsg3.playerID = 1003;
            playerMsg3.playerData = new PlayerData();
            playerMsg3.playerData.name = "wudi3";
            playerMsg3.playerData.atk = 3;
            playerMsg3.playerData.lev = 3;
            byte[] bytes = playerMsg3.Writing();
            byte[] bytes1 = new byte[10];
            byte[] bytes2 = new byte[bytes.Length - 10];
            Array.Copy(bytes, 0, bytes1, 0, 10);
            Array.Copy(bytes, 10, bytes2, 0, bytes.Length - 10);

            NetMgr.Instance.SendTest(bytes1);
            await Task.Delay(500);
            NetMgr.Instance.SendTest(bytes2);
        });
        //黏包分包
        button3.onClick.AddListener(async () =>
        {
            PlayerMsg playerMsg = new PlayerMsg();
            playerMsg.playerID = 1001;
            playerMsg.playerData = new PlayerData();
            playerMsg.playerData.name = "wudi1";
            playerMsg.playerData.atk = 1;
            playerMsg.playerData.lev = 1;

            PlayerMsg playerMsg2 = new PlayerMsg();
            playerMsg2.playerID = 1002;
            playerMsg2.playerData = new PlayerData();
            playerMsg2.playerData.name = "wudi2";
            playerMsg2.playerData.atk = 2;
            playerMsg2.playerData.lev = 2;

            byte[] bytes1 = playerMsg.Writing();
            byte[] bytes2 = playerMsg2.Writing();

            byte[] bytes2_1 = new byte[10];
            byte[] bytes2_2 = new byte[bytes2.Length - 10];
            Array.Copy(bytes2, 0, bytes2_1, 0, 10);
            Array.Copy(bytes2, 10, bytes2_2, 0, bytes2.Length - 10);

            byte[] bytes = new byte[bytes1.Length + bytes2_1.Length];
            bytes1.CopyTo(bytes, 0);
            bytes2_1.CopyTo(bytes, bytes1.Length);
            NetMgr.Instance.SendTest(bytes);
            await Task.Delay(500);
            NetMgr.Instance.SendTest(bytes2_2);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
