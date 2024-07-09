using System.Diagnostics; // Process 클래스 사용을 위해 필요
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Networking;
using System.Collections;

public class WpfDataReceiver : MonoBehaviour
{
    public ChannelReceiver channelReceiver;
    public CharacterManager characterManager;

    private string versionCheckUrl = "http://k10b208.p.ssafy.io:8081/info/version";
    public string serverVersion;


    void Awake()
    {
        StartCoroutine(GetServerVersion());
    }

    IEnumerator GetServerVersion()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(versionCheckUrl))
        {
            // 요청을 보내고 응답을 기다립니다.
            yield return webRequest.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (webRequest.result != UnityWebRequest.Result.Success)
#else
            if (webRequest.isNetworkError || webRequest.isHttpError)
#endif
            {
                // 실패
            }
            else
            {
                // 서버로부터 받은 버전 정보
                string jsonResponse = webRequest.downloadHandler.text;
                VersionInfo versionInfo = JsonUtility.FromJson<VersionInfo>(jsonResponse);

                serverVersion = versionInfo.version;
            }
        }
    }
    void Start()
    {
        // WPF 애플리케이션의 프로세스 이름 확인
        string wpfProcessName = "FourKnights"; // 실제 WPF 애플리케이션의 프로세스 이름으로 교체해야 합니다.

        // WPF 애플리케이션이 실행 중인지 확인
        bool wpfRunning = Process.GetProcessesByName(wpfProcessName).Length > 0;

        // WPF 애플리케이션이 실행 중이지 않다면 유니티 애플리케이션 종료
        if (!wpfRunning)
        {
            UnityEngine.Debug.Log("WPF 애플리케이션 미실행으로 종료");
            Application.Quit();
        }

        // 유니티의 버전과 백엔드의 버전이 다르다면 게임을 종료
        if (Application.version != serverVersion)
        {
            UnityEngine.Debug.Log("유니티 버전과 백엔드 버전이 다릅니다." + "Unity : " + Application.version + " / Backend : " + serverVersion);
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

        // 채널이 1~6인 경우에만 로컬에 저장
        if (channelData.channel >= 1 && channelData.channel <= 6)
        {
            File.WriteAllText(path, channelJson);
        }

        // 강제종료 시 게임 관련 데이터 처리
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

        // 캐릭터의 pk값이 있는 경우에만 로컬에 저장
        if (characterManager.characterInfoPk >= 1 && ItemManager.userItemList.Count >= 1)
        {
            File.WriteAllText(path2, saveJson);
        }

        // 게임이 종료될 때, 현재 게임에서 가지고 있는 게임 버전을 로컬에 저장
        string unityVersion = Application.version;
        string path3 = Application.persistentDataPath + "/version.txt";

        File.WriteAllText(path3, unityVersion);
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

    [System.Serializable]
    public class VersionInfo
    {
        public string message;
        public int statusCode;
        public string version;
    }

}
