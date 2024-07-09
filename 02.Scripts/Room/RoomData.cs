// https://ojui.tistory.com/41
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RoomData : MonoBehaviour
{
    private TMP_Text RoomInfoText;
    private RoomInfo _roomInfo;
    // public TMP_InputField userIdText;
    public RoomInfo RoomInfo
    {
        get
        {
            return _roomInfo;
        }
        set
        {
            _roomInfo = value;
            // EX: room 03 (1/2)
            RoomInfoText.text = $"{_roomInfo.Name}  ({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})"; // 버튼의 클릭 이벤트에 함수를 연결
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnEnterRoom(_roomInfo.Name));
        }
    }

    void Awake()
    {
        RoomInfoText = GetComponentInChildren<TMP_Text>();
        // userIdText = GameObject.Find("InputField (TMP) - Nickname").GetComponent<TMP_InputField>();
    }

    void OnEnterRoom(string roomName)
    {   
        Debug.Log("RoomData.OnEnterRoom()");
        NetworkManager.Instance.JoinRoom(roomName);
    }
}