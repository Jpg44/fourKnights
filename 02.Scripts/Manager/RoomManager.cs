using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [Header("Boss")]
    public string boss = "Golem";  
    
    [Header("UI")]
    public GameObject makeRoomObj;      // 방 만들기 UI
    public Button makeButton;           // 방 만들기 - 버튼
    public TMP_InputField newRoomName;  // 방 만들기 - 방 이름
    public Button startButton;          // 방 입장하기 - 버튼
    public Button leaveButton;          // 방 나가기 - 버튼
    public Button exitButton;           // 파티창 닫기 - 버튼
    public Button refreshButton;        // 방 새로고침 - 버튼
    
    [Header("Room List")]
    public GameObject roomPrefab;           // 방 프리팹
    public Transform roomScrollContent;     // 방 리스트 위치를 담음
    private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();


    [Header("My Room")]
    public TextMeshProUGUI myRoomName;      // 내 파티 
    public GameObject myRoomMemberObj;      // 내 파티 오브젝트
    public GameObject member1;
    public GameObject member2;
    public GameObject member3;
    public GameObject member4;
    public Image member1Job;
    public Image member2Job;
    public Image member3Job;
    public Image member4Job;
    public TextMeshProUGUI member1Name;
    public TextMeshProUGUI member2Name;
    public TextMeshProUGUI member3Name;
    public TextMeshProUGUI member4Name;

    [Header("Warning Message")]
    public TextMeshProUGUI warning1;
    public TextMeshProUGUI warning2;

    public Camera camera;
    private Dictionary<string, object> myRoom; 

    // 싱글톤
    private static RoomManager instance;

    public static RoomManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우 새로 생성
            if (instance == null)
            {
                Debug.Log("RoomManager 가 사용되면 안될 상황에서 호출 중임");
                // 새 게임 오브젝트를 생성하여 게임 매니저를 붙임
                // GameObject singleton = new GameObject("RoomManager");
                // instance = singleton.AddComponent<RoomManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(this.gameObject);
        }
        // else
        // {
        //     Destroy(this.gameObject);
        // }
        
        makeButton.onClick.AddListener(() => MakeParty());
        startButton.onClick.AddListener(() => StartParty());
        leaveButton.onClick.AddListener(() => LeaveParty());
        exitButton.onClick.AddListener(() => EndParty());
    }

    public void selectedBoss(int index)
    {
        switch (index)
        {
            case 0:
                boss = "Golem";
                break;
            case 1:
                boss = "Dryad";
                break;
            case 2:
                boss = "Boss3";
                break;
            case 3:
                boss = "Boss4";
                break;
            default:
                break;
        }

        Debug.Log("RoomManager.selectedBoss() " + boss);
    }

    ////////////////////////////////// 보스 UI 열기 //////////////////////////////////
    
    public void ReadyForParty()
    {
        Debug.Log("RoomManager.ReadyForParty() : 보스 매칭창 열기");

        PhotonNetwork.AutomaticallySyncScene = true;
        NetworkManager.Instance.roomRequest = true;

        myRoomName.text  = "파티 없음";
        member1.SetActive(false);
        member2.SetActive(false);
        member3.SetActive(false);
        member4.SetActive(false);

        makeRoomObj.SetActive(true);
        makeButton.gameObject.SetActive(true);

        startButton.interactable = true;
        leaveButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        
        warning1.gameObject.SetActive(false);
        warning2.gameObject.SetActive(false);

        // 보스UI 임을 Network Manager 에 알림
        if (!NetworkManager.Instance.isBossUI)
            NetworkManager.Instance.SetIsScene("BossUI");

        // 방 상태인 경우에 방에서 나오고 보스UI 상태로 바꿈 (로비o 방x)
        if (PhotonNetwork.InRoom)
        {
            NetworkManager.Instance.LeaveRoom();

            camera.gameObject.SetActive(true);
        }

        Cursor.visible = true;                     // 마우스 커서를 보이게
        Cursor.lockState = CursorLockMode.None;   // 마우스 커서 위치 고정 해제
    }

    ////////////////////////////////// 선택된 보스의 파티 만들기 //////////////////////////////////
    
    public void MakeParty()
    {
        makeButton.interactable = false;

        if (newRoomName.text != "" && newRoomName.text != "파티 없음")
        {
            Debug.Log("RoomManager.MakeParty() : 파티 생성 요청 - 방 이름 유효");
            NetworkManager.Instance.roomName = boss + ": " + newRoomName.text;
            NetworkManager.Instance.CreateRoom();
        }
        else
        {
            Debug.Log("RoomManager.MakeParty() : 파티 생성 요청 - 방 이름 무효");
            MakePartyFailed();
        }
    }
    
    public void MakePartySucceeded()
    {
        // 파티 만드는 데 성공했으면 파티 만들기 UI 숨기기
        Debug.Log("RoomManager.MakePartySucceeded() : 파티 생성 성공");

        makeButton.interactable = true;
        makeRoomObj.SetActive(false);

        leaveButton.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);

        warning1.gameObject.SetActive(true);
        warning2.gameObject.SetActive(false);

        UpdateCurrentPartyList();
        ResetAllPartyList();
    }

    public void MakePartyFailed()
    {
        Debug.Log("RoomManager.MakePartyFailed() : 파티 생성 실패");
        makeButton.interactable = true;
        warning1.gameObject.SetActive(false);
        warning2.gameObject.SetActive(true);
    }

    ////////////////////////////////// 파티에 참여하기 //////////////////////////////////
 
    public void JoinPartySucceeded()
    {
        Debug.Log("RoomManager.JoinPartySucceeded() : 파티 참여 성공");

        makeButton.interactable = true;
        makeRoomObj.SetActive(false);

        leaveButton.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);

        warning1.gameObject.SetActive(true);
        warning2.gameObject.SetActive(false);

        UpdateCurrentPartyList();
        ResetAllPartyList();
    }
    /////////////////////////////////// 선택된 보스 입장하기 //////////////////////////////////
   
    public void StartParty()
    {
        Debug.Log("RoomManager.StartParty() : 파티 입장 - " + boss);
        Cursor.visible = false;                     // 마우스 커서를 보이게
        Cursor.lockState = CursorLockMode.Locked;   // 마우스 커서 위치 고정 해제
        
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        // SceneManager.LoadScene(boss);
        PhotonNetwork.LoadLevel(boss);
    }

    ////////////////////////////////// 현재 보스 파티 나가기 //////////////////////////////////

    public void LeaveParty()
    {
        Debug.Log("RoomManager.LeaveParty() : 파티 탈퇴");
        
        // 방을 나가고 로비 상태로 변환
        NetworkManager.Instance.LeaveRoom();
 
        myRoomName.text  = "파티 없음";
        member1.SetActive(false);
        member2.SetActive(false);
        member3.SetActive(false);
        member4.SetActive(false);
        
        makeRoomObj.SetActive(true);
        // makeButton.gameObject.SetActive(true);

        leaveButton.gameObject.SetActive(false);
        startButton.interactable = true;
        startButton.gameObject.SetActive(false);

        warning1.gameObject.SetActive(false);
        warning2.gameObject.SetActive(false);

        Cursor.visible = true;                     // 마우스 커서를 보이게
        Cursor.lockState = CursorLockMode.None;   // 마우스 커서 위치 고정 해제
    }

    ////////////////////////////////// 보스 UI 창 닫기 //////////////////////////////////
    
    public void EndParty()
    {
        Debug.Log("RoomManager.EndParty() : 보스 매칭창 닫기");
        PhotonNetwork.AutomaticallySyncScene = false;

        NetworkManager.Instance.roomRequest = false;
        camera.gameObject.SetActive(false);
        makeButton.interactable = true;
        warning1.gameObject.SetActive(false);
        warning2.gameObject.SetActive(false);
        
        NetworkManager.Instance.SetIsScene("Village");
        
        if (PhotonNetwork.InRoom)
            NetworkManager.Instance.LeaveRoom();
        else
            NetworkManager.Instance.JoinOrCreateRoom();
    }
    
    ////////////////////////////////// 파티 리스트 갱신 //////////////////////////////////

    public void UpdateAllPartyList(List<RoomInfo> roomList)
    {
        GameObject tempRoom = null;
        foreach (RoomInfo room in roomList)
        {
            if (room.Name.StartsWith(boss + ": "))
            {
                // 방이 삭제된 경우
                if (room.RemovedFromList)
                {
                    Debug.Log("RoomManager.OnRoomListUpdate() : 로비 내에 파티 삭제됨");
                    roomDict.TryGetValue(room.Name, out tempRoom);
                    Destroy(tempRoom);
                    roomDict.Remove(room.Name);
                }
                // 방 정보가 갱신 (변경) 된 경우
                else
                {
                    // 방이 처음 생성된 경우
                    if (!roomDict.ContainsKey(room.Name))
                    {
                        Debug.Log("RoomManager.OnRoomListUpdate() : 로비 내에 파티 생성됨");
                        GameObject _room = Instantiate(roomPrefab, roomScrollContent);
                        _room.GetComponent<RoomData>().RoomInfo = room;
                        roomDict.Add(room.Name, _room);
                    }
                    else
                    {
                        Debug.Log("RoomManager.OnRoomListUpdate() : 로비 내에 파티 갱신됨");
                        roomDict.TryGetValue(room.Name, out tempRoom);
                        tempRoom.GetComponent<RoomData>().RoomInfo = room;
                    }
                }
            }
        }
    }

    public void ResetAllPartyList()
    {
        Debug.Log("RoomManager.ResetAllPartyList() : 모든 파티 리스트 초기화");

        roomDict.Clear();

        for (int i = roomScrollContent.childCount - 1; i >= 0; i--)
        {
            Destroy(roomScrollContent.GetChild(i).gameObject);
        }
    }

    public void UpdateCurrentPartyList()
    {
        Debug.Log("RoomManager.UpdateCurrentPartyList() : 현재 파티 리스트 업데이트");
        member1.gameObject.SetActive(false);
        member2.gameObject.SetActive(false);
        member3.gameObject.SetActive(false);
        member4.gameObject.SetActive(false);

        myRoomName.text = PhotonNetwork.CurrentRoom.Name;
        int n = 0;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            switch(n)
            {
                case 0:
                    member1.gameObject.SetActive(true);
                    member1Name.text = player.NickName;
                    break;
                case 1:
                    member2.gameObject.SetActive(true);
                    member2Name.text = player.NickName;
                    break;
                case 2:
                    member3.gameObject.SetActive(true);
                    member3Name.text = player.NickName;
                    break;
                case 3:
                    member4.gameObject.SetActive(true);
                    member4Name.text = player.NickName;
                    break;
            }
            n++;
        }

        // 방장만 startButton을 활성화할 수 있게 설정
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }
}
