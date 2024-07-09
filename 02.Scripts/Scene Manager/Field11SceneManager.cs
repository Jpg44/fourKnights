using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class Field11SceneManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        Debug.Log("Field11SceneManager : Field1-1");

        if (!NetworkManager.Instance.isField1)
            NetworkManager.Instance.SetIsScene("Field1-1");
        // NetworkManager.Instance.JoinOrCreateRoom_toField();
    }

    private void Update()
    {

    }

    // private IEnumerator RejoinLobby()
    // {
    //     // 방을 나간 후 잠시 기다립니다.
    //     yield return new WaitForSeconds(1.0f);

    //     // 연결 상태 확인 후 로비에 입장
    //     if (PhotonNetwork.IsConnectedAndReady)
    //     {
    //         NetworkManager.Instance.JoinLobby();
    //     }
    // }
}