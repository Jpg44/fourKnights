using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navTest : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform target;

    void Awake()
    {

        navMeshAgent = GetComponent<NavMeshAgent>();
      
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.SetDestination(target.position);
        navMeshAgent.speed = 0; // 기본 속도
        navMeshAgent.angularSpeed = 0; // 기본 각속도
        Debug.Log(navMeshAgent.speed);
        Debug.Log(navMeshAgent.angularSpeed);
        navMeshAgent.speed = 10; // 기본 속도
        navMeshAgent.angularSpeed = 120; // 기본 각속도
    }
}
