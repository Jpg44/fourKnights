using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FlowerMissile : MonoBehaviourPunCallbacks
{
    private float timeToReturn = 15.0f; // 15초 후에 반환
    public CharacterManager characterManager;

    void OnEnable()
    {
        GameObject characterManagerObject = GameObject.FindWithTag("CharacterManager");
        if (characterManagerObject != null) {
            characterManager = characterManagerObject.GetComponent<CharacterManager>();
        }
        // 활성화 될 때마다 15초 후에 ReturnToPool 메서드를 호출
        Invoke("ReturnToPool", timeToReturn);
    }

    void OnDisable()
    {
        // 오브젝트가 비활성화될 때 모든 Invoke를 취소 (중복 호출 방지)
        CancelInvoke();
    }

    void OnTriggerEnter(Collider other)
    {
        // Player 태그를 지닌 오브젝트와 충돌 시
        if (other.CompareTag("Player"))
        {
            PhotonView photonView = other.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                characterManager.SetHP(500);
            }
            ReturnToPool(); // 즉시 풀로 반환
        }
    }

    void ReturnToPool()
    {
        // 모든 Invoke를 취소하여 중복 반환 방지
        CancelInvoke();

        // 미사일을 비활성화하고 오브젝트 풀로 반환
        ObjectPoolManager.Instance.ReturnMissile(gameObject);
    }

}
