using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DropKill : MonoBehaviourPunCallbacks
{
    public CharacterManager characterManager;
    public GameObject golem;
    private GameObject player;

    private void Start()
    {
        GameObject characterManagerObject = GameObject.FindWithTag("CharacterManager");
        if (characterManagerObject != null) 
        {
            characterManager = characterManagerObject.GetComponent<CharacterManager>();
        }

        
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // 플레이어와 골렘 사이의 거리를 계산
        float distance = Vector3.Distance(player.transform.position, golem.transform.position);
        // 거리가 200미터 이상이면 캐릭터 사망 처리
        if (distance > 700f)
        {
            if (characterManager.currentHP > 0)
            {
                characterManager.Die();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player")) // 플레이어와 충돌했는지 확인
        {
            // 플레이어 사망
            PhotonView photonView = other.GetComponent<PhotonView>();
            Debug.Log("낙사");
            if (characterManager.currentHP > 0)
            {
                if (photonView != null && photonView.IsMine)
                {
                    characterManager.Die();
                }
            }
        }
    }
}
