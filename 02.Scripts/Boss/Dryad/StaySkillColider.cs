using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StaySkillColider : MonoBehaviourPunCallbacks
{
    public int count;
    public CharacterManager characterManager;

    private void Start()
    {
        GameObject characterManagerObject = GameObject.FindWithTag("CharacterManager");
        if (characterManagerObject != null) {
            characterManager = characterManagerObject.GetComponent<CharacterManager>();
        }
    }

    void OnTriggerStay(Collider other)
    {
        count ++;
        if (other.CompareTag("Player"))
        {
            PhotonView photonView = other.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                characterManager.SetHP(8);
            }
        }
    }
}
