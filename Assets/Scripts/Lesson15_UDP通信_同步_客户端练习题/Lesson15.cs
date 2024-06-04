using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lesson15 : MonoBehaviour
{
    public Button btnSend;
    // Start is called before the first frame update
    void Start()
    {
        btnSend.onClick.AddListener(() =>
        {
            PlayerMsg playerMsg = new PlayerMsg();
            playerMsg.playerID = 1;
            playerMsg.playerData = new PlayerData();
            playerMsg.playerData.name = "kangkang";
            playerMsg.playerData.atk = 100;
            playerMsg.playerData.lev = 100;
            UdpNetMgr.Instance.Send(playerMsg);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
