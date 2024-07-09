using System.Diagnostics; // Process 클래스 사용을 위해 필요
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Networking;
using System.Collections;

public class TestDown : MonoBehaviour
{
    public ChannelReceiver channelReceiver;
    public CharacterManager characterManager;
    void Start()
    {
        // WPF 애플리케이션의 프로세스 이름 확인
        string wpfProcessName = "FourKnights"; // 실제 WPF 애플리케이션의 프로세스 이름으로 교체해야 합니다.

        // WPF 애플리케이션이 실행 중인지 확인
        bool wpfRunning = Process.GetProcessesByName(wpfProcessName).Length > 0;

        // WPF 애플리케이션이 실행 중이지 않다면 유니티 애플리케이션 종료
        if (!wpfRunning)
        {
            Application.Quit();
        }

        channelReceiver = GetComponent<ChannelReceiver>();
    }

    void Update()
    {
        if (characterManager == null)
        {
            characterManager = FindObjectOfType<CharacterManager>();
        }
    }
    private void OnApplicationQuit() // 종료될 때,
    {
        // 강제종료 시 채널 데이터 로컬에 저장
        ChannelData channelData = new ChannelData();
        channelData.userId = channelReceiver.myId;
        channelData.channel = channelReceiver.channelNum;

        string channelJson = JsonUtility.ToJson(channelData);
        string path = Application.persistentDataPath + "/channelData.json";
        File.WriteAllText(path, channelJson);

        // 강제종료 시 게임 관련 데이터 로컬에 저장
        Inventory[] inventoryArr = new Inventory[ItemManager.userItemList.Count];
        for (int i = 0; i < ItemManager.userItemList.Count; i++)
        {
            inventoryArr[i] = new Inventory
            {
                itemPk = ItemManager.userItemList[i].itemNum,
                quantity = ItemManager.userItemList[i].quantity,
                location = ItemManager.userItemList[i].inventoryNum
            };
        }

        // 데이터 객체 생성
        SaveData saveData = new SaveData
        {
            characterInfoPk = characterManager.characterInfoPk,
            currentHP = characterManager.currentHP,
            inventory = inventoryArr
        };

        string saveJson = JsonUtility.ToJson(saveData);
        string path2 = Application.persistentDataPath + "/saveData.json";
        File.WriteAllText(path2, saveJson);

    }

    [System.Serializable]
    public class ChannelData
    {
        public string userId;
        public int channel;
    }

    [System.Serializable]
    public class SaveData
    {
        public int characterInfoPk;
        public int currentHP;
        public Inventory[] inventory;
    }

    [System.Serializable]
    public class Inventory
    {
        public int itemPk;
        public int quantity;
        public int location;
    }
}
