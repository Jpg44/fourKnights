using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;


public class BossMonster : MonoBehaviourPunCallbacks
{
    public Transform target;
    public Animator animator;
    private NavMeshAgent navMeshAgent;

    public PhotonView photonViewA;

    private float targetRefreshRate = 20.0f; // 타겟을 재설정하는 시간 간격
    private float timeSinceLastTargetUpdate = 30; // 마지막 타겟 업데이트 이후 경과 시간


    [Header("Audio Clips")]
    [SerializeField]

    private AudioClip audioClipPunch;
    public AudioClip crash;
    public AudioClip roar;

    private AudioSource audioSource;

    public ParticleSystem roll_particle;
    public ParticleSystem jump_particle;

    public GameObject l_hand;
    public GameObject rock;
    public GameObject r_hand;

    public GameObject wall_1;
    public GameObject wall_2;
    public GameObject wall_3;
    public GameObject wall_4;

    public GameObject roll;

    public GameObject arrowalarm;

    public GameObject rangealarm;
    public GameObject map;
    public GameObject cutScean;
    public Material red;
    public Material origin;

    public GameObject objectToSpawn; // 스폰할 스킬의 프리팹
    public Vector3 spawnAreaCenter; // 스폰 범위의 중심
    public float spawnRadius; // 스폰할 원의 반경
    public int numberOfObjects; // 스폰할 오브젝트 수

    public float knockbackForce = 500.0f;
    public float knockbackDuration = 0.5f;
    public Transform targetPosition;

    
    private float attackTimer;
    private float attackCooldown;

    private bool isHolding = false;

    private bool isActionInProgress = false;

    void Awake()
    {
        roll_particle.Stop();
        jump_particle.Stop();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) { // AudioSource 컴포넌트가 없다면 추가
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        ResetAttackTimer();
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
        


        if (PhotonNetwork.IsMasterClient)
        {

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0 && !isActionInProgress)
        {
            if (distanceToTarget <= 40.0f)  // 타겟과의 거리가 20미터 이하일 경우
            {
                navMeshAgent.speed = 1f;
                navMeshAgent.angularSpeed = 70;
                PerformCloseRangeAttack();
            }
            else  // 타겟과의 거리가 20미터 초과일 경우
            {
                navMeshAgent.speed = 3f;  // 기본 속도
                navMeshAgent.angularSpeed = 100;  // 기본 회전 속도
                PerformLongRangeAttack();
            }
            ResetAttackTimer();
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
        Debug.Log(targetPosition);
    }



    void PerformCloseRangeAttack()
    {
        int attackType = UnityEngine.Random.Range(0, 7);  // 0부터 5까지의 공격 유형 선택
        int currentHealth = BossStatus.Instance.currentHealth;
      
        if (currentHealth >= 300 && currentHealth <= 600)
        {
            attackType = UnityEngine.Random.Range(0, 7);  // 'case 4'를 포함한 모든 공격 유형
        }
        else
        {
            do
            {
                attackType = UnityEngine.Random.Range(0, 7);  // 'case 4'를 제외한 공격 유형 선택
            }
            while (attackType == 5);
        }
        // attackType = 2;
        ExecuteAttack(attackType);
        photonView.RPC("ExecuteAttackRPC", RpcTarget.Others, attackType);

        
    }


    void PerformLongRangeAttack()
    {
        int attackType = UnityEngine.Random.Range(3, 8);  // 6부터 7까지의 공격 유형 선택
        int currentHealth = BossStatus.Instance.currentHealth;

        if (currentHealth >= 300 && currentHealth <= 600)
        {
            attackType = UnityEngine.Random.Range(3, 8);  // 'case 4'를 포함한 모든 공격 유형
        }
        else
        {
            do
            {
                attackType = UnityEngine.Random.Range(3, 8);  // 'case 4'를 제외한 공격 유형 선택
            }
            while (attackType == 5);
        }
       
        ExecuteAttack(attackType);
        photonView.RPC("ExecuteAttackRPC", RpcTarget.Others, attackType);
    }

    private void ExecuteAttack(int attackType)
    {

        // attackType = 2;
    
        switch (attackType)
        {
            case 0:
                animator.SetTrigger("Combo");
                // Hand 콜라이더 활성화
                ToggleHandCollider(true);
                Invoke("DeactivateHandCollider", 2.0f);
                break;
            case 1:
                animator.SetTrigger("BackAttack");
                ToggleHandCollider(true);
                Invoke("DeactivateHandCollider", 2.0f);
                break;
            case 2:
                animator.SetTrigger("HorizonalAttack");
                ToggleHandCollider(true);
                Invoke("DeactivateHandCollider", 2.0f);
                break;
            case 3:
                StartCoroutine(TripleJump());
                break;
            case 4:
                SpawnObjectsInCircle();
                break;
            case 5:
                animator.SetTrigger("Jump");
                StartCoroutine(ActivateSkill());
                break;
            case 6:
                StartCoroutine(ToggleRoll(roll, true));
                break;
            case 7:
                // Invoke("ThrowRock", 0.5f); // Add a delay to match the animation
                StartCoroutine(ThrowRock());
                break;
           
            
        }
    }


    [PunRPC]
    private void ExecuteAttackRPC(int attackType)
    {

        // attackType = 2;
        switch (attackType)
        {
            case 0:
                animator.SetTrigger("Combo");
                // Hand 콜라이더 활성화
                ToggleHandCollider(true);
                Invoke("DeactivateHandCollider", 2.0f);
                break;
            case 1:
                animator.SetTrigger("BackAttack");
                ToggleHandCollider(true);
                Invoke("DeactivateHandCollider", 2.0f);
                break;
            case 2:
                animator.SetTrigger("HorizonalAttack");
                // ToggleHandCollider(true);
                // Invoke("DeactivateHandCollider", 2.0f);
                break;
            case 3:
                StartCoroutine(TripleJump());
                break;
            case 4:
                SpawnObjectsInCircle();
                break;
            case 5:
                animator.SetTrigger("Jump");
                StartCoroutine(ActivateSkill());
                break;
            case 6:
                StartCoroutine(ToggleRoll(roll, true));
                break;
            case 7:
                // Invoke("ThrowRock", 0.5f); // Add a delay to match the animation
                StartCoroutine(ThrowRock());
                break;
           
            
        }
    }


    IEnumerator ThrowRock()
    {
        arrowalarm.SetActive(true);
        yield return new WaitForSeconds(2.0f);  

        Vector3 throwDirection = (target.position - r_hand.transform.position).normalized;
        throwDirection += new Vector3(0, 0.2f, 0); 
        // if (Vector3.Distance(target.position, r_hand.transform.position) <= 50f)
        // {
        //     // 50미터 이내라면 1.5초 추가 대기
        //     yield return new WaitForSeconds(0.5f);
        // }
            
        isHolding = true;
        GameObject instantRock = Instantiate(rock, r_hand.transform.position, Quaternion.identity);
        instantRock.transform.SetParent(r_hand.transform); 
        instantRock.transform.localPosition = Vector3.zero; 

        StartCoroutine(ThrowObject(instantRock, throwDirection));
    }

    IEnumerator ThrowObject(GameObject objectToThrow,Vector3 throwDirection )
    {
        
        isActionInProgress = true;
        animator.SetTrigger("Throw");

        yield return new WaitForSeconds(0.5f);

        objectToThrow.transform.SetParent(null);  // 부모 관계 해제

        Rigidbody rb = objectToThrow.GetComponent<Rigidbody>();
        if (rb == null) {
            rb = objectToThrow.AddComponent<Rigidbody>();
        }
        rb.isKinematic = false;

        // Calculate and apply force
        rb.AddForce(throwDirection * 100f, ForceMode.VelocityChange);  // 힘의 방향과 강도 조정

        // 회전을 목표 방향에 맞추어 조정
        objectToThrow.transform.rotation = Quaternion.LookRotation(throwDirection);
        isActionInProgress = false;
    }



    public IEnumerator ActivateSkill()
    {
        // 보스 이동
        isActionInProgress = true;
        cutScean.SetActive(true);
        yield return new WaitForSeconds(1); 
        Debug.Log("스킬 발동");
        navMeshAgent.enabled = false;
        // transform.position = targetPosition.position;
        Vector3 jumpPosition = transform.position + Vector3.up * 100f; // 10미터 위로 점프
        float jumpDuration = 3.0f; // 점프에 걸리는 시간
        float startTime = Time.time;
        
        while (Time.time < startTime + jumpDuration)
        {
            transform.position = Vector3.Lerp(transform.position, jumpPosition, (Time.time - startTime) / jumpDuration);
            if (Time.time > 4f)
            {
                cutScean.SetActive(false);
            }
            yield return null;
        }

        // 목표 위치까지 부드럽게 내려오기
        float dropDuration = 0.5f; // 내려오는데 걸리는 시간
        startTime = Time.time;
        while (Time.time < startTime + dropDuration)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition.position, (Time.time - startTime) / dropDuration);
            yield return null;
        }
        

        animator.SetTrigger("Scream");
        audioSource.clip = roar;
        audioSource.Play();

        
        wall_1.SetActive(true);
        wall_2.SetActive(true);
        wall_3.SetActive(true);
        wall_4.SetActive(true);
        map.GetComponent<Renderer>().material = red;

        rangealarm.SetActive(true);


        yield return new WaitForSeconds(5); // 3초 딜레이

        // float timeToIncrease = 5.0f; // 증가 시간 설정
        // float timePassed = 0f; // 경과 시간을 추적하는 변수

        // while (timePassed < timeToIncrease)
        // {
        //     rangealarm.transform.localScale += new Vector3(0.04f, 0.04f, 0); // x, y 좌표 증가
        //     yield return new WaitForSeconds(0.1f); // 0.1초 마다 위치 업데이트
        //     timePassed += 0.1f; // 경과 시간 업데이트
        // }


        List<Coroutine> knockbacks = new List<Coroutine>();

        // 모든 플레이어 감지
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            
            
            if (IsPlayerVisible(player.transform))
            {
                Debug.Log("플레이어감지");
                StartCoroutine(KnockbackPlayer(player.GetComponent<CharacterController>(), knockbackDuration, 500.0f));
            }
        }

        foreach (Coroutine cor in knockbacks)
        {
            yield return cor;  // 모든 KnockbackPlayer 코루틴이 완료될 때까지 기다림
        }

        navMeshAgent.enabled = true;
        navMeshAgent.Warp(transform.position);

        isActionInProgress = false;

        wall_1.SetActive(false);
        wall_2.SetActive(false);
        wall_3.SetActive(false);
        wall_4.SetActive(false);
        map.GetComponent<Renderer>().material = origin;


        rangealarm.SetActive(false);

    
        Debug.Log("NavMeshAgent 재활성화");
    }


    private bool IsPlayerVisible(Transform playerTransform)
    {
        Debug.Log("구체 광선 쏘기");
        float sphereRadius = 1.0f; // 구체의 반경
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.Normalize(); // 방향 벡터를 정규화

        // 구체 광선(SphereCastAll)을 사용하여 경로 상의 모든 객체 감지
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, sphereRadius, directionToPlayer, Mathf.Infinity);
        // 거리에 따라 충돌 결과를 정렬
        Array.Sort(hits, (hit1, hit2) => hit1.distance.CompareTo(hit2.distance));

        // 충돌체 분석
        foreach (var hit in hits)
        {
            if (hit.transform.CompareTag("Wall"))
            {
                Debug.Log("벽에 맞음: " + hit.transform.name);
                return false; // 벽에 가려진 경우
            }
            if (hit.transform == playerTransform)
            {
                Debug.Log("플레이어 감지: " + playerTransform.name);
                return true; // 플레이어가 벽보다 먼저 감지된 경우
            }
        }
        return false; // 플레이어 또는 벽을 감지하지 못한 경우
    }

    private IEnumerator KnockbackPlayer(CharacterController playerController, float duration, float knockbackForce )
    {
        Debug.Log("플레이어 밀치기");
        float timer = 0;
        Vector3 forceDirection = playerController.transform.position - transform.position;
        forceDirection.y = 10; // 수평 방향으로만 힘을 가함
        forceDirection.Normalize();

        while (timer < duration)
        {
            playerController.Move(forceDirection * knockbackForce * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }


    }


    private void ResetAttackTimer()
    {
        attackCooldown = UnityEngine.Random.Range(3.5f, 5.0f);
        attackTimer = attackCooldown;
    }

    private void ToggleHandCollider(bool state)
    {
        ToggleCollider(l_hand, state);
        ToggleCollider(r_hand, state);
    }

    private void ToggleCollider(GameObject hand, bool state)
    {

        
        isActionInProgress = true;

        Debug.Log(isActionInProgress);
        // audioSource.clip = audioClipPunch;
        // audioSource.Play();

        BoxCollider boxCollider = hand.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.isTrigger = state;
        }
        else
        {
            Debug.LogError("Hand object is not assigned in the inspector!");
        }

  
        isActionInProgress = false;
        // Debug.Log(isActionInProgress);
        // audioSource.Stop();
    }

    private IEnumerator ToggleRoll(GameObject body, bool state)
    {
        
        roll_particle.Play();

        yield return new WaitForSeconds(2.5f);
        roll_particle.Stop();
        isActionInProgress = true;
        navMeshAgent.enabled = false;
        roll.SetActive(true);
        animator.SetTrigger("Roll");

        yield return new WaitForSeconds(2); 


        navMeshAgent.enabled = true;
        roll.SetActive(false);
        navMeshAgent.Warp(transform.position);
        isActionInProgress = false; 
    }


    /// 3 연속 내려찍기 패턴 (내려 찍을 때 해당 유저가 점프를 안할 때 즉 땅에 닿아 있으면 피격 및 밀치기)
    IEnumerator TripleJump()
    {
        // rangealarm.SetActive(true);
        map.GetComponent<Renderer>().material = red;
        isActionInProgress = true;
        roll_particle.Play();
        yield return new WaitForSeconds(2.5f);
        roll_particle.Stop();
        Debug.Log("스킬 발동");
        navMeshAgent.enabled = false;
        animator.SetTrigger("Jump");
        yield return new WaitForSeconds(2.0f);
        CheckAndKnockbackPlayers();
        audioSource.clip = crash;
        audioSource.Play();
        jump_particle.Play();
        animator.SetTrigger("Jump");
        yield return new WaitForSeconds(3.1f);
        CheckAndKnockbackPlayers();
        audioSource.Play();
        jump_particle.Play();
        animator.SetTrigger("Jump");
        yield return new WaitForSeconds(3.2f);
        CheckAndKnockbackPlayers();
        audioSource.Play();
        jump_particle.Play();
        navMeshAgent.enabled = true;
        map.GetComponent<Renderer>().material = origin;
        navMeshAgent.Warp(transform.position);
        isActionInProgress = false; 
        // audioSource.Stop();
    
    }

    void CheckAndKnockbackPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller.isGrounded)
            {
                StartCoroutine(KnockbackPlayer(controller, knockbackDuration, 500.0f));
                Debug.Log("점점점");
            }
        }
    }


    /// 맵 랜덤 위치에 장판 오브젝트 생성
    void SpawnObjectsInCircle()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 spawnPosition = GetRandomPositionInCircle(spawnAreaCenter, spawnRadius);
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        }
    }

    // 주어진 중심과 반경 내에서 랜덤 위치를 반환하는 메서드
    Vector3 GetRandomPositionInCircle(Vector3 center, float radius)
    {
        Debug.Log("오브젝트 소환");
        float angle = UnityEngine.Random.Range(0, Mathf.PI * 2);
        float distance = UnityEngine.Random.Range(0, radius);
        return center + new Vector3(Mathf.Cos(angle) * distance, 0, Mathf.Sin(angle) * distance);
    }


    /// 골렘 주먹 콜라이더 해제
    public void DeactivateHandCollider()
    {
        ToggleHandCollider(false);
    }


    // 거리에 따라 목표 플레이어 계속 갱신하는 로직 (30 초 마다 거리가 가장 가까운 플레이어를 목표로 지정)
    // void UpdateTarget()
    // {
    //     GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    //     if (players.Length == 0) {
    //         Debug.Log("No players found");
    //         return;  // 플레이어가 없으면 함수 종료
    //     }
    //     Transform closest = null;
    //     float closestDistance = Mathf.Infinity;
    //     Vector3 position = transform.position;

    //     foreach (GameObject player in players)
    //     {
    //         float distance = Vector3.Distance(player.transform.position, position);
    //         if (distance < 300f && distance < closestDistance)
    //         {
    //             closest = player.transform;
    //             closestDistance = distance;
    //         }
    //     }

    //     target = closest;  // 가장 가까운 플레이어를 타겟으로 설정
    // }

    

}
