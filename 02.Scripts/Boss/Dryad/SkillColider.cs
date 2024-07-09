using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SkillColider : MonoBehaviourPunCallbacks
{
    
    public CharacterManager characterManager;

    private void Start()
    {
        GameObject characterManagerObject = GameObject.FindWithTag("CharacterManager");
        if (characterManagerObject != null) {
            characterManager = characterManagerObject.GetComponent<CharacterManager>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView photonView = other.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                characterManager.SetHP(800);
            }
        }
    }
}
