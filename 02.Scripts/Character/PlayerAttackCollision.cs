using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollision : MonoBehaviour
{
    public CharacterManager characterManager;
    public PhotonPlayerController PhotonPlayerController;
    private void OnEnable()
    {
        StartCoroutine(AutoDisable());
    }

    private void Start()
    {
        GameObject characterManagerObject = GameObject.FindWithTag("CharacterManager");
        if (characterManagerObject != null) {
            characterManager = characterManagerObject.GetComponent<CharacterManager>();
        }

        PhotonPlayerController = GetComponentInParent<PhotonPlayerController>();
    }

    private IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(0.1f);

        gameObject.SetActive(false);
    }

    // void Update()
    // {
    //     Debug.Log("!!!!!!! 어택은 잘한다" + PhotonPlayerController.attackStyle);
    // }

    private double skillDmg()
    {
        double dmg = (double)characterManager.ATK;
        switch(PhotonPlayerController.attackStyle)
        {
            // m0Down은 x1의 데미지
            case "m1Down":
                dmg = dmg * 1.2;
                break;
            case "sDown1":
                dmg = dmg * 1.5;
                break;
            case "sDown2":
            // 현재 3번 임시 3번 스킬은 딜스킬이 아님
            //case "sDown3":
            case "sDown4":
                dmg = dmg * 3;
                break;
            default:
                break;
        }

        return dmg;
    }

    private void OnTriggerEnter(Collider other) {
        // 몬스터한테 데미지 넣는 함수
        Debug.Log("평타 온트리거 됨");
        //Debug.Log(playerController.attackStyle);
        int dmg = (int)skillDmg();
        BossStatus bossStatus = other.GetComponentInChildren<BossStatus>();
        BossPart bossPart = other.GetComponentInChildren<BossPart>();
        DryadStatus bossStatus2 = other.GetComponent<DryadStatus>();
        FlowerStatus bossStatus3 = other.GetComponent<FlowerStatus>();
        RainbowFlowerStatus bossStatus4 = other.GetComponent<RainbowFlowerStatus>();
        if(other.CompareTag("Boss"))
        {
            Debug.Log("보스에게 공격 닿았음");
            if(bossStatus != null)
            {
                Debug.Log("보스스테이터스있음");
                // 플레이어의 방어율 로직도 적용
                int reduction = (int)(dmg * (1-(float)bossStatus.defense/(bossStatus.defense+100)));
                int ranNum = Random.Range(-10, 11);
                int totalDmg = reduction + (int)(reduction*ranNum/100); // 데미지 바운더리 10%
                bossStatus.TakeDamage(totalDmg);
                Debug.Log("최종적용데미지 : "+totalDmg);
                //bossStatus.TakeDamage(dmg);
            }

            else if(bossPart != null)
            {
                int reduction = (int)(dmg * (1-(float)BossStatus.Instance.defense/(BossStatus.Instance.defense+100)));
                int ranNum = Random.Range(-10, 11);
                int totalDmg = reduction + (int)(reduction*ranNum/100); // 데미지 바운더리 10%
                bossPart.TakeDamage(totalDmg);
                Debug.Log("부위최종적용데미지 : "+totalDmg);
            }
        }

        

        if(other.CompareTag("Boss"))
        {
            Debug.Log("보스에게 공격 닿았음");
            if(bossStatus2 != null)
            {
                Debug.Log("보스스테이터스있음");
                // 플레이어의 방어율 로직도 적용
                int reduction = (int)(dmg * (1-(float)bossStatus2.defense/(bossStatus2.defense+100)));
                int ranNum = Random.Range(-10, 11);
                int totalDmg = reduction + (int)(reduction*ranNum/100); // 데미지 바운더리 10%
                bossStatus2.TakeDamage(totalDmg);
                Debug.Log("최종적용데미지 : "+totalDmg);
                //bossStatus.TakeDamage(dmg);
            }
        }

        if(other.CompareTag("Boss"))
        {
            Debug.Log("보스에게 공격 닿았음");
            if(bossStatus3 != null)
            {
                Debug.Log("보스스테이터스있음");
                // 플레이어의 방어율 로직도 적용
                int reduction = (int)(dmg * (1-(float)bossStatus3.defense/(bossStatus3.defense+100)));
                int ranNum = Random.Range(-10, 11);
                int totalDmg = reduction + (int)(reduction*ranNum/100); // 데미지 바운더리 10%
                bossStatus3.TakeDamage(totalDmg);
                Debug.Log("최종적용데미지 : "+totalDmg);
                //bossStatus.TakeDamage(dmg);
            }
        }

        if(other.CompareTag("Boss"))
        {
            Debug.Log("보스에게 공격 닿았음");
            if(bossStatus4 != null)
            {
                Debug.Log("레인보우");
                // 플레이어의 방어율 로직도 적용
                int reduction = (int)(dmg * (1-(float)bossStatus4.defense/(bossStatus4.defense+100)));
                int ranNum = Random.Range(-10, 11);
                int totalDmg = reduction + (int)(reduction*ranNum/100); // 데미지 바운더리 10%
                bossStatus4.TakeDamage(totalDmg);
                Debug.Log("최종적용데미지 : "+totalDmg);
                //bossStatus.TakeDamage(dmg);
            }
        }

        
        if(other.CompareTag("Monster"))
        {
            MonsterState monsterState = other.GetComponent<MonsterState>();
            Debug.Log("몬스터에게 공격 닿았음");
            if(monsterState != null)
            {
                Debug.Log("몬스터 스텟 있음");
                // 플레이어의 방어율 로직도 적용
                int reduction = (int)(dmg );//* (1-(float)bossStatus.defense/(bossStatus.defense+100)));
                int ranNum = Random.Range(-10, 11);
                int totalDmg = reduction + (int)(reduction*ranNum/100); // 데미지 바운더리 10%
                monsterState.monsterHp-=totalDmg;
                Debug.Log("최종적용데미지 : "+totalDmg);
                monsterState.monsterIsHit=true;
                monsterState.monsterHitDamage=totalDmg;
                //bossStatus.TakeDamage(dmg);
            }
        }
    }
}
