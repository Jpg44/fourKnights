using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerName : MonoBehaviourPunCallbacks
{
    private PhotonView pv;
    private CharacterManager characterManager;

    public string playerName;
    public TextMeshPro playerNameLabel;

    private void Awake()
    {
        // photonView = GetComponentInParent<PhotonView>();
        pv = GetComponent<PhotonView>();

        // if (!pv.IsMine)
        // {
        //     return;
        // }

        GameObject characterManagerObj = GameObject.Find("CharacterManager");
        characterManager = characterManagerObj.GetComponent<CharacterManager>();
        playerName = characterManager.characterNickname;
    }
        
    public void Start()
    {
        if (!pv.IsMine)
        {
            return;
        }

        // SetNickname(playerName);
        photonView.RPC("SetNickname", RpcTarget.AllBuffered, playerName);
    }

    [PunRPC]
    public void SetNickname(string name)
    {
        Debug.Log("PlayerName.SetNickname() : RPC + " + name);
        playerNameLabel.text = name;
    }

}
