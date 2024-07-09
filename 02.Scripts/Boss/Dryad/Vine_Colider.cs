using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class Vine_Colider : MonoBehaviourPunCallbacks
{
   
    private float timeToReturn = 1.45f; 
    public CharacterManager characterManager;

    void OnEnable()
    {
        GameObject characterManagerObject = GameObject.FindWithTag("CharacterManager");
        if (characterManagerObject != null) {
            characterManager = characterManagerObject.GetComponent<CharacterManager>();
        }

        Invoke("ReturnToPool", timeToReturn);
    }
    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            PhotonView photonView = other.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                characterManager.SetHP(600);
            }
        }
    }

    void ReturnToPool()
    {
        // 모든 Invoke를 취소하여 중복 반환 방지
        CancelInvoke();

        // 미사일을 비활성화하고 오브젝트 풀로 반환
        ObjectPoolManager.Instance.ReturnVineCube(gameObject);
    }
}
