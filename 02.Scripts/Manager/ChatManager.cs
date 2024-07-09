// https://hyokim.tistory.com/4
// https://velog.io/@kyj/Unity-Photon-Network%EB%A1%9C-%EC%B1%84%ED%8C%85%EB%B0%A9-%EB%A7%8C%EB%93%A4%EA%B8%B0
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using UnityEngine.EventSystems;

public class ChatManager : MonoBehaviourPunCallbacks
{
    public bool isChatEnabled = false;
    public GameObject chatPanel;
    public RectTransform chatPanelRectTransform;
    public TextMeshProUGUI chatLog; //채팅 내역
    public TMP_InputField inputField; //채팅입력 인풋필드
    public Button sendBtn; //채팅 입력버튼
    ScrollRect scroll_rect = null; //채팅이 많이 쌓일 경우 스크롤바의 위치를 아래로 고정하기 위함
    // public Text playerList; //참가자 목록
    // string players; //참가자들


    private static ChatManager instance;

    public static ChatManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우 새로 생성
            if (instance == null)
            {
                Debug.Log("ChatManager 가 사용되면 안될 상황에서 호출 중임");
                // 새 게임 오브젝트를 생성하여 게임 매니저를 붙임
                // GameObject singleton = new GameObject("ChatManager");
                // instance = singleton.AddComponent<ChatManager>();
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

        sendBtn.onClick.AddListener(() => SendButtonOnClicked());
    }


    void Start()
    {
        chatPanelRectTransform = chatPanel.GetComponent<RectTransform>();

        PhotonNetwork.IsMessageQueueRunning = true;
        scroll_rect = GameObject.FindObjectOfType<ScrollRect>();
    }

    /// <summary>
    /// chatterUpdate(); 메소드로 주기적으로 플레이어 리스트를 업데이트하며
    /// input에 포커스가 맞춰져있고 엔터키가 눌려졌을 경우에도 SendButtonOnClicked(); 메소드를 실행.
    /// </summary>
    void Update()
    {
        // ChatterUpdate();
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            Debug.Log("업데이트 체크");
            // && !inputField.isFocused
            SendButtonOnClicked();
        }
        // 채팅이 꺼져있음에도 inputField 가 활성화되는 버그가 있으므로, 이때 강제로 비활성화시킴
        else if (!isChatEnabled && inputField.isFocused)
        {
            inputField.text = "";
            inputField.DeactivateInputField();
        }

        

        if(isChatEnabled && chatPanelRectTransform.sizeDelta.y != 400)
        {
            chatPanelRectTransform.sizeDelta = new Vector2(chatPanelRectTransform.sizeDelta.x, 400);
            StartCoroutine(ScrollUpdate());
        }
        else if (!isChatEnabled && chatPanelRectTransform.sizeDelta.y != 110)
        {
            chatPanelRectTransform.sizeDelta = new Vector2(chatPanelRectTransform.sizeDelta.x, 110);
            StartCoroutine(ScrollUpdate());
        }
    }

    /// <summary>
    /// 전송 버튼이 눌리면 실행될 메소드. 메세지 전송을 담당함.
    /// input이 비어있으면 아무것도 전송하지 않고, 비어있지 않다면
    /// "[ID] 메세지"의 형식으로 메세지를 전송함.
    /// 메세지 전송은 photonView.RPC 메소드를 이용해 각 유저들에게 ReceiveMsg 메소드를 실행하게 함.
    /// 자기 자신에게도 메세지를 띄워야 하므로 ReceiveMsg(msg);를 실행함.
    /// input.ActivateInputField();는 메세지 전송 후 바로 메세지를 입력할 수 있게 포커스를 Input Field로 옮김 (편의 기능)
    /// 그 후 input.text를 빈 칸으로 만듦
    /// </summary>
    public void SendButtonOnClicked()
    {
        // 입력 값이 없을 때
        if (inputField.text.Equals(""))
        {
            // 채팅 중이 아니라면 활성화
            if (!isChatEnabled)
            {
                inputField.ActivateInputField();
                isChatEnabled = true;
            }
            // 채팅 중이라면 비활성화
            else
            {
                isChatEnabled = false;
            }
        }
        // 입력 값이 있을 때
        else
        {
            string msg = string.Format("[{0}] {1}", PhotonNetwork.LocalPlayer.NickName, inputField.text);
            photonView.RPC("ReceiveMsg", RpcTarget.OthersBuffered, msg);
            ReceiveMsg(msg);
            inputField.text = "";

            inputField.ActivateInputField(); // 메세지 전송 후 바로 메세지를 입력할 수 있게 포커스를 Input Field로 옮기는 편의 기능
            isChatEnabled = true;
        }
    }

    /// <summary>
    /// 채팅 참가자 목록을 업데이트 하는 함수.
    /// '참가자 목록' 텍스트 아래에 플레이어들의 ID를 더해주는 식으로 작동하며,
    /// 실시간으로 출입하는 유저들의 ID를 반영함.
    /// </summary>
    // void ChatterUpdate()
    // {
    //     players = "참가자 목록\n";
    //     foreach (Player p in PhotonNetwork.PlayerList)
    //     {
    //         players += p.NickName + "\n";
    //     }
    //     playerList.text = players;
    // }

    // public void GameStart()
    // {
    //     if (PhotonNetwork.IsMasterClient)
    //     {
    //         photonView.RPC("OnGameRoom", RpcTarget.AllBuffered);
    //     }
    //     else
    //     {
    //         // Debug.Log("마스터 클라이언트가 아님");
    //     }
    // }

    public void ChatPlayerEntered(Player newPlayer)
    {
        string msg = string.Format("<color=#00bb00>[{0}] 님이 입장하셨습니다.</color>", newPlayer.NickName);
        ReceiveMsg(msg);
    }

    public void ChatPlayerLeft(Player otherPlayer)
    {
        string msg = string.Format("<color=#ff0000>[{0}] 님이 퇴장하셨습니다.</color>", otherPlayer.NickName);
        ReceiveMsg(msg);
    }

    [PunRPC]
    public void ReceiveMsg(string msg)
    {
        // 기존 코드
        // chatLog.text += "\n" + msg;
        // StartCoroutine(ScrollUpdate());

        // 새 메시지를 로그에 추가
        chatLog.text += "\n" + msg;
        // 채팅 로그를 줄 단위로 분할
        string[] lines = chatLog.text.Split('\n');
        // 로그 개수가 80개를 초과할 경우, 가장 오래된 로그를 제거
        if (lines.Length > 100)
        {
            chatLog.text = string.Join("\n", lines.Skip(1));
        }
        StartCoroutine(ScrollUpdate());
    }

    // [PunRPC]
    // public void OnGameRoom()
    // {
    //     PhotonNetwork.LoadLevel(2);
    // }

    IEnumerator ScrollUpdate()
    {
        yield return null;
        scroll_rect.verticalNormalizedPosition = 0.0f;
    }
}