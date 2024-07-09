using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Photon.Pun;

public class BossMovementController : MonoBehaviourPunCallbacks
{
    private NavMeshAgent navMeshAgent;
    public Transform target;

    private float targetRefreshRate = 20.0f; // 타겟을 재설정하는 시간 간격
    private float timeSinceLastTargetUpdate = 30; // 마지막 타겟 업데이트 이후 경과 시간

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        // UpdateTarget();
    }

    void Update()
    {
        if (target)
        {
            SetTarget(target.position);
        }
        
        timeSinceLastTargetUpdate += Time.deltaTime;
        if (PhotonNetwork.IsMasterClient)
        {
            if (timeSinceLastTargetUpdate >= targetRefreshRate)
            {
            UpdateTarget();
            timeSinceLastTargetUpdate = 0;
            }
        
        
        }
    }

    void UpdateTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0)
        {
            Debug.Log("No players found");
            return; // 플레이어가 없으면 함수 종료
        }
        
        // 랜덤하게 플레이어 선택
        int randomIndex = UnityEngine.Random.Range(0, players.Length);
        target = players[randomIndex].transform; // 랜덤 플레이어를 타겟으로 설정
        Debug.Log(players[randomIndex].GetComponent<PhotonView>().ViewID);
        SetTarget(target.position);
        photonView.RPC("SetTargetRPC", RpcTarget.Others, target.position);
        

    }

    void SetTarget(Vector3 targetPosition)
    {
        navMeshAgent.SetDestination(targetPosition);
    }

    [PunRPC]
    void SetTargetRPC(Vector3 targetPosition)
    {
        navMeshAgent.SetDestination(targetPosition);
    }


}
