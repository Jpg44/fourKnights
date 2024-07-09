using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BossAttackController : MonoBehaviourPunCallbacks
{
    public static BossAttackController Instance { get; private set; }
    private Animator animator;
    // public ParticleSystem attackparticle;

    public GameObject solarbeam;
    public GameObject shield;
    public GameObject vine;
    public GameObject thorn_needle;
    public GameObject greenhole;
    public GameObject entangle;
    public GameObject rainbowflower;
    public GameObject teleport;
    public GameObject slash;
    public ChargeController chargeController;
    private float attackTimer;
    private float attackCooldown;
    public bool isActionInProgress = false;

    

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 중복 인스턴스 제거
        }

        animator = GetComponent<Animator>();
        ResetAttackTimer();
    
    }

    void Update()
    {
        
        if (PhotonNetwork.IsMasterClient)
        {
            if (!isActionInProgress) // 새 동작을 시작하기 전에 현재 동작이 끝났는지 확인
            {
            attackTimer -= Time.deltaTime;
            }
            // attackTimer -= Time.deltaTime;
            if (attackTimer <= 0 && !isActionInProgress) // 새 동작을 시작하기 전에 현재 동작이 끝났는지 확인
            {
                PerformRandomAttack();
                ResetAttackTimer();
            }
        }
    }

    private void PerformRandomAttack()
    {
        if (DryadStatus.Instance == null)
        {
            Debug.LogWarning("DryadStatus.Instance가 null입니다. PerformRandomAttack 호출 전에 초기화되었는지 확인하세요.");
            return;
        }
        Debug.Log("인스턴스 있음");
        
        int currentHealth = DryadStatus.Instance.currentHealth;
        int currentMana = DryadStatus.Instance.currentMana;

        List<int> possibleAttacks = new List<int>();  // 가능한 공격 유형을 저장하는 리스트

        // 기본 공격 유형 추가
        for (int i = 0; i <= 6; i++) 
        {
            possibleAttacks.Add(i);
        }

        // currentMana가 5 이상일 경우, solarbeam(7번) 추가
        if (currentMana >= 5)
        {
            possibleAttacks.Add(7);  // solarbeam 추가
        }

        // currentHealth가 1700 이하일 경우, teleport(8번) 추가
        if (currentHealth <= 1700)
        {
            possibleAttacks.Add(8);  // teleport 추가
        }

        // 가능한 공격 유형 중 하나를 랜덤 선택
        int attackType = possibleAttacks[UnityEngine.Random.Range(0, possibleAttacks.Count)];
        // int attackType = 5; 


        photonView.RPC("ExecuteAttackRPC", RpcTarget.Others, attackType);


        switch (attackType)
        {
            case 0:
                // isActionInProgress = true;
                shield.SetActive(true);
                animator.SetTrigger("Skill1");
                break;
            case 1:
                chargeController.StartEffectWithCallback(() =>
                {
                    animator.SetTrigger("Skill2");
                    vine.SetActive(true);
                });
                break;
            case 2:
                animator.SetTrigger("Skill2");
                thorn_needle.SetActive(true);
                break;
            case 3:
                chargeController.StartEffectWithCallback(() =>
                {
                    isActionInProgress = true;
                    animator.SetTrigger("Attack");
                    greenhole.SetActive(true);
                });
                break;
            case 4:
                isActionInProgress = true;
                animator.SetTrigger("Skill2");
                entangle.SetActive(true);
                break;
            case 5:
                isActionInProgress = true;
                animator.SetTrigger("Skill1");
                rainbowflower.SetActive(true);
                break;
            case 6:
                isActionInProgress = true;
                chargeController.StartEffectWithCallback(() =>
                {
                    animator.SetTrigger("Slash");
                    slash.SetActive(true);
                });
                break;
            case 7:
                isActionInProgress = true;
                solarbeam.SetActive(true);
                break;
            case 8:
                teleport.SetActive(true);
                isActionInProgress = true;
                break;
        }
    }


    [PunRPC]
    private void ExecuteAttackRPC(int attackType)
    {
        switch (attackType)
        {
            case 0:
                // isActionInProgress = true;
                shield.SetActive(true);
                animator.SetTrigger("Skill1");
                break;
            case 1:
                chargeController.StartEffectWithCallback(() =>
                {
                    animator.SetTrigger("Skill2");
                    vine.SetActive(true);
                });
                break;
            case 2:
                animator.SetTrigger("Skill2");
                thorn_needle.SetActive(true);
                break;
            case 3:
                chargeController.StartEffectWithCallback(() =>
                {
                    isActionInProgress = true;
                    animator.SetTrigger("Attack");
                    greenhole.SetActive(true);
                });
                break;
            case 4:
                isActionInProgress = true;
                animator.SetTrigger("Skill2");
                entangle.SetActive(true);
                break;
            case 5:
                animator.SetTrigger("Skill1");
                rainbowflower.SetActive(true);
                break;
            case 6:
                isActionInProgress = true;
                chargeController.StartEffectWithCallback(() =>
                {
                    animator.SetTrigger("Slash");
                    slash.SetActive(true);
                });
                break;
            case 7:
                isActionInProgress = true;
                solarbeam.SetActive(true);
                break;
            case 8:
                teleport.SetActive(true);
                isActionInProgress = true;
                break;
        }
    }


    

    private void ResetAttackTimer()
    {
        attackCooldown = UnityEngine.Random.Range(3.0f, 4.5f);
        attackTimer = attackCooldown;
    }

    IEnumerator effect()
    {
        yield return new WaitForSeconds(1.5f);
    }
}
