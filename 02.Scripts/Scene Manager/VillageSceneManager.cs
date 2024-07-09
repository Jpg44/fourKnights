using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class VillageSceneManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        Debug.Log("VillageSceneManager : VillageScene");

        NetworkManager.Instance.SetIsScene("Village");

        if (!PhotonNetwork.InLobby)
            NetworkManager.Instance.JoinLobby();
        else
            NetworkManager.Instance.JoinOrCreateRoom();
    }
}