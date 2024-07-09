using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

// public class NetworkManager : MonoBehaviour 
public class NetworkManager : MonoBehaviourPunCallbacks
{
    private readonly string version = "1.0f"; // 게임 버전
    private AppSettings settings;
    private bool isNickname = false;

    [Header("Player Prefab")]
    public GameObject playerPrefab;
    public GameObject spawnedPlayer;

    [Header("Channel")]
    private int selectedChannelNumber;
    public Button channel1;
    public Button channel2;
    public Button channel3;
    public Button channel4;
    public Button channel5;
    public Button channel6;

    [Header("Scene Info")]
    public bool isStart = false;
    public bool isCharacter = false;
    public bool isVillage = false;
    public bool isField1 = false;
    public bool isGolem = false;
    public bool isDryad = false;

    [Header("Status Flag")]
    public bool isDied = false;
    public bool isPotal = false;
    public bool isBossUI = false;


    [Header("Room Info")]
    public bool roomRequest = false;
    public string roomName = "";

    // private static RoomList;

    // 채널
    public ChannelReceiver channelReceiver;

    public BackEndDataReceiver backEndDataReceiver;

    private static NetworkManager instance;

    public static NetworkManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우 새로 생성
            if (instance == null)
            {
                // 새 게임 오브젝트를 생성하여 게임 매니저를 붙임
                GameObject singleton = new GameObject("NetworkManager");
                instance = singleton.AddComponent<NetworkManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        PhotonNetwork.GameVersion = version;
        PhotonNetwork.AutomaticallySyncScene = false;
        // PhotonNetwork.LogLevel = PunLogLevel.Full;

        InitializeChannelButtons();

        channelReceiver = GetComponent<ChannelReceiver>();
    }

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    void Update()
    {
        if (backEndDataReceiver == null)
        {
            backEndDataReceiver = FindObjectOfType<BackEndDataReceiver>();
        }
    }

    private void InitializeChannelButtons()
    {
        Debug.Log("NetworkManager.InitializeChannelButtons() : 채널 선택 버튼 할당");
        // 버튼 객체 찾기
        channel1 = GameObject.Find("Channel 1").GetComponent<Button>();
        channel2 = GameObject.Find("Channel 2").GetComponent<Button>();
        channel3 = GameObject.Find("Channel 3").GetComponent<Button>();
        channel4 = GameObject.Find("Channel 4").GetComponent<Button>();
        channel5 = GameObject.Find("Channel 5").GetComponent<Button>();
        channel6 = GameObject.Find("Channel 6").GetComponent<Button>();

        // 버튼 이벤트 리스너 추가
        channel1.onClick.AddListener(() => ConnectToChannel(1));
        channel2.onClick.AddListener(() => ConnectToChannel(2));
        channel3.onClick.AddListener(() => ConnectToChannel(3));
        channel4.onClick.AddListener(() => ConnectToChannel(4));
        channel5.onClick.AddListener(() => ConnectToChannel(5));
        channel6.onClick.AddListener(() => ConnectToChannel(6));
    }
    ////////////////////////////////// 씬 설정 //////////////////////////////////

    // OnLeftRoom 실행되고, 씬을 이동시켜주기 위한 씬 설정
    public void SetIsScene(string scene)
    {
        isStart = false;
        isCharacter = false;
        isVillage = false;
        isField1 = false;
        isGolem = false;
        isDryad = false;
        isBossUI = false;

        if (scene == "Start")
            isStart = true;
        else if (scene == "Character")
            isCharacter = true;
        else if (scene == "Village")
            isVillage = true;
        else if (scene == "Field1-1")
            isField1 = true;
        else if (scene == "BossUI")
            isBossUI = true;
        else if (scene == "Golem")
            isGolem = true;
        else if (scene == "Dryad")
            isDryad = true;
        Debug.Log("NetworkManager.SetIsScene() : " + isVillage + " " + isField1 + " " + isBossUI);
        Debug.Log("NetworkManager.SetIsScene() : " + isGolem + " " + isDryad);
    }

    ////////////////////////////////// 스폰 //////////////////////////////////

    public void Spawn()
    {
        Debug.Log("NetworkManager.Spawn() : 캐릭터 스폰 ");
        GameObject spawnedPlayer = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 0.5f, 0f), Quaternion.identity, 0);
        CharacterManager.Instance.isAlive = true;
        // if (roomRequest)
        // {
        //     DontDestroyOnLoad(spawnedPlayer);
        // }
    }

    ////////////////////////////////// 채널 선택 //////////////////////////////////

    public void ConnectToChannel(int channelNumber)
    {
        Debug.Log("NetworkManager.ConnectToChannel()");
        string punId = GetAppIdByChannel(channelNumber);
        string chatId = GetChatAppIdByChannel(channelNumber);
        selectedChannelNumber = channelNumber;

        settings = new AppSettings()
        {
            AppIdRealtime = punId,
            // AppId = punId,
            AppIdChat = chatId,

        };

        // ServerDataManager.Instance.settings = settings;
        // Debug.Log("punID : ", AppSettings.AppIdRealtime);
        // Debug.Log("chatId : ", AppSettings.AppIdChat);
        Connect();
    }

    ////////////////////////////////// 포톤 서버 접속 //////////////////////////////////

    // Photon Online Server에 접속하기
    public void Connect()
    {
        Debug.Log("NetworkManager.Connect() : 서버 연결 요청");
        PhotonNetwork.ConnectUsingSettings(settings);
    }
    public override void OnConnectedToMaster()
    {
        channelReceiver.ChannelConnect(); // 채널리시버의 접속 확인 백엔드 보냄
        Debug.Log("NetworkManager.OnConnectedToMaster() : 서버 연결 성공");

        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            SceneManager.LoadScene("CharacterScene");
        }
        // PlayerCounter();

        if (!PhotonNetwork.InLobby)
            JoinLobby();
    }

    ////////////////////////////////// 로비 //////////////////////////////////

    // 로비에 접속하기
    public void JoinLobby()
    {
        Debug.Log("NetworkManager.JoinLobby()");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("NetworkManager.OnJoinedLobby() : 로비 접속 완료");


        if (isVillage) JoinOrCreateRoom();
        else if (isField1) JoinOrCreateRoom();
        // else if (isBossUI) JoinOrCreateRoom();
        else if (isGolem) CreateRoom();
        else if (isDryad) CreateRoom();
    }

    ////////////////////////////////// 방 생성/참가 //////////////////////////////////

    // 방 참가하는데, 방이 없으면 생성하고 참가. (마을과 사냥터용)
    public void JoinOrCreateRoom()
    {
        if (isVillage)
        {
            if (!isNickname)
            {
                PhotonNetwork.NickName = CharacterManager.Instance.characterNickname;
                isNickname = true;
            }
            string roomName = "Channel" + selectedChannelNumber + "_Village";
            PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions { MaxPlayers = 20 }, null);
            Debug.Log("NetworkManager.JoinOrCreateRoom() : 방 생성 및 참가 ->" + roomName);
        }
        else if (isField1)
        {
            string roomName = "Field1_ " + CharacterManager.Instance.userID + DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions { MaxPlayers = 1 }, null);
            Debug.Log("NetworkManager.JoinOrCreateRoom() : 방 생성 및 참가 ->" + roomName);
        }
    }

    ////////////////////////////////// 방 생성 //////////////////////////////////

    // 방 생성하고, 참가 (방을 참가하려면, Connect 되어있거나 Lobby에 참가해있어야 함)
    // 보스매칭에서 방을 생성할 때 사용
    public void CreateRoom()
    {
        Debug.Log("NetworkManager.CreateRoom() : 방 만들기 요청 - " + roomName);

        // 방 이름, 최대 플레이어 수, 비공개 등을 지정 가능.
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 4 });
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("NetworkManager.OnCreatedRoom() : 방 만들기 완료");
        if (roomRequest)
        {
            RoomManager.Instance.MakePartySucceeded();
        }
        roomName = "";
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("NetworkManager.OnCreateRoomFailed() : 방 만들기 실패");
        if (roomRequest)
        {
            RoomManager.Instance.MakePartyFailed();
        }
        roomName = "";
    }

    ////////////////////////////////// 방 참가 //////////////////////////////////

    // 방 참가하기 (방 이름으로 입장 가능)
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("NetworkManager.OnJoinedRoom() : 방 참가 완료");
        if (roomRequest)
        {
            // 확인
            RoomManager.Instance.JoinPartySucceeded();
        }
        Spawn();
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("NetworkManager.OnJoinRoomFailed() : 방 참가 실패");
    }

    ////////////////////////////////// 랜덤 참가 //////////////////////////////////

    // 방 랜덤으로 참가하기
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("NetworkManager.OnJoinRandomFailed() : 방 랜덤 참가 실패");
    }

    ////////////////////////////////// 방 나가기 //////////////////////////////////

    // 방 떠나기
    public void LeaveRoom()
    {
        Debug.Log("NetworkManager.LeaveRoom()");

        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("NetworkManager.OnLeftRoom()");

        if (isDied)
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            isDied = false;
            SceneManager.LoadScene("VillageScene");
        }
        else if (isPotal && isVillage)
        {
            //LoadingSceneController.LoadScene("Field1-1");
            SceneManager.LoadScene("Field1-1");
            backEndDataReceiver.SaveOfChangeScene("fieldIn"); // 마을 -> 필드 1로 포탈 통신
            isPotal = false;
        }
        else if (isPotal && isField1 && Portal.fieldNum == 0)
        {
            SceneManager.LoadScene("VillageScene");
            backEndDataReceiver.SaveOfChangeScene("fieldOut"); // 필드 1 -> 마을로 포탈 통신
            //LoadingSceneController.LoadScene("VillageScene");
            isPotal = false;
        }
        else if (isPotal && isField1 && Portal.fieldNum == 2)
        {
            SceneManager.LoadScene("Field2-1");
            backEndDataReceiver.SaveOfChangeScene("fieldIn"); // 필드 1 -> 필드 2로 포탈 통신
            //LoadingSceneController.LoadScene("VillageScene");
            isPotal = false;
        }
        else if (isPotal && Portal.fieldNum == 1)
        {
            //LoadingSceneController.LoadScene("Field1-1");
            SceneManager.LoadScene("Field1-1");
            backEndDataReceiver.SaveOfChangeScene("fieldOut"); // 필드 2 -> 필드 1로 포탈 통신
            isPotal = false;
        }
        else if (isStart)
        {
            // SceneManager.LoadScene("StartScene");
            Disconnect();
        }
        else if (isVillage)
        {
            // SceneManager.LoadScene("VillageScene");
        }
        else if (isCharacter)
        {
            SceneManager.LoadScene("CharacterScene");
        }
        else if (isGolem)
        {
            SceneManager.LoadScene("Golem");
        }
        else if (isDryad)
        {
            SceneManager.LoadScene("Dryad");
        }
    }

    ////////////////////////////////// 방 설정 //////////////////////////////////

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("NetworkManager.OnRoomListUpdate() : 파티 리스트 업데이트");
        if (roomRequest)
        {
            RoomManager.Instance.UpdateAllPartyList(roomList);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("NetworkManager.OnPlayerEnteredRoom() : " + newPlayer.NickName);
        if (roomRequest)
        {
            RoomManager.Instance.UpdateCurrentPartyList();
        }

        // 채팅 관련
        if (isVillage)
        {
            ChatManager.Instance.ChatPlayerEntered(newPlayer);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("NetworkManager.OnPlayerLeftRoom() : " + otherPlayer.NickName);
        if (roomRequest)
        {
            RoomManager.Instance.UpdateCurrentPartyList();
        }

        // 채팅 관련
        if (isVillage)
        {
            ChatManager.Instance.ChatPlayerLeft(otherPlayer);
        }
    }

    ////////////////////////////////// 연결 완전 끊기 //////////////////////////////////

    // 연결 끊기
    public void Disconnect()
    {
        Debug.Log("NetworkManager.Disconnect()");
        PhotonNetwork.Disconnect();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        channelReceiver.ChannelDisconnect(); // 채널 끊을 때 백엔드로 보내기
        Debug.Log("NetworkManager.OnDisconnected()");
        if (isStart)
        {
            InitializeChannelButtons();
        }
    }

    ////////////////////////////////// 확인용 //////////////////////////////////

    public void PlayerCounter()
    {
        int playerCount = PhotonNetwork.CountOfPlayersOnMaster;
        Debug.Log("NetworkManager.PlayerCounter() : 현재 서버의 입장 인원 " + playerCount);
    }

    ////////////////////////////////// 환경 설정 //////////////////////////////////

    public void SetStart()
    {
        Debug.Log("NetworkManager.SetStart() : 첫 화면으로 돌아가기");
        SetIsScene("Start");
        SceneManager.LoadScene("StartScene");
        if (PhotonNetwork.InRoom)
            LeaveRoom();
        else
            Disconnect();
    }

    public void SetCharacter()
    {
        Debug.Log("NetworkManager.SetCharacter() : 캐릭터 선택으로 돌아가기");
        SetIsScene("Character");
        SceneManager.LoadScene("CharacterScene");
        LeaveRoom();
    }

    public void SetViliage()
    {
        Debug.Log("NetworkManager.SetViliage() : 마을로 돌아가기");
        if (!isVillage)
        {
            SetIsScene("Village");
            LeaveRoom();
        }
        // SceneManager.LoadScene("VillageScene");
    }

    ////////////////////////////////// 채널 선택 설정 //////////////////////////////////

    private string GetAppIdByChannel(int channelNumber)
    {
        switch (channelNumber)
        {
            case 1: return "3bd1cc2f-b6e1-4980-ab43-706dd8a681ce";  // sh
            case 2: return "50ad9263-3b57-43f7-88a0-e47f156716f3";  // dh
            case 3: return "e9719074-db54-48d7-8577-37228e26b85c";  // wj
            case 4: return "8da7510a-4df2-406d-a32c-17603a0e21e0";  // yj
            case 5: return "f6dd77e6-a928-4f50-a0b5-870b2d8acaf1";  // hw
            case 6: return "1bed5af5-423f-4e36-872f-365e84daed36";  // jh

            default: return "null";
        }
    }

    private string GetChatAppIdByChannel(int channelNumber)
    {
        switch (channelNumber)
        {
            case 1: return "b3ee1fa4-df20-4557-b328-70b0ac7e46c3";  // sh
            case 2: return "0df69a4f-3ea3-4f30-8a5a-ac7a489bb7ce";  // dh
            case 3: return "87a7155e-92a8-4c12-abbb-a00f85621920";  // wj
            case 4: return "e777a21f-3362-47e7-a7bd-96d09e2408c2";  // yj
            case 5: return "7bcafdb9-32b9-4445-9ee6-5ee49e773a03";  // hw
            case 6: return "0d2182a0-fd5e-4ea4-a679-fba05aa7809d";  // jh

            default: return "null";
        }
    }
}