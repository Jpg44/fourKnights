using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttack : MonoBehaviour
{
    public CharacterManager characterManager;
  

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

    }

    private IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(0.1f);

        gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other) {
        // 몬스터한테 데미지 넣는 함수
        Debug.Log("평타 온트리거 됨");
        //Debug.Log(playerController.attackStyle);
        BossStatus bossStatus = other.GetComponentInChildren<BossStatus>();
        BossPart bossPart = other.GetComponentInChildren<BossPart>();
        DryadStatus bossStatus2 = other.GetComponent<DryadStatus>();
        FlowerStatus bossStatus3 = other.GetComponent<FlowerStatus>();
        RainbowFlowerStatus bossStatus4 = other.GetComponent<RainbowFlowerStatus>();
        if(other.CompareTag("Boss"))
        {
            Debug.Log("보스에게 공격 닿았음");
           
            if(bossStatus2 != null)
            {
                Debug.Log("보스스테이터스있음");
                // 플레이어의 방어율 로직도 적용
                int reduction = (int)(50 * (1-(float)bossStatus2.defense/(bossStatus2.defense+100)));
                int ranNum = Random.Range(-10, 11);
                int totalDmg = reduction + (int)(reduction*ranNum/100); // 데미지 바운더리 10%
                bossStatus2.TakeDamage(totalDmg);
                Debug.Log("최종적용데미지 : "+totalDmg);
                Debug.Log(bossStatus2.defense);
                //bossStatus.TakeDamage(dmg);
            }
            else if(bossPart != null)
            {
                bossPart.TakeDamage(0);
            }

            if(bossStatus3 != null)
            {
                Debug.Log("보스스테이터스있음");
                // // 플레이어의 방어율 로직도 적용
                // int reduction = (int)(dmg * (1-(float)bossStatus.defense/(bossStatus.defense+100)));
                // int ranNum = Random.Range(-10, 11);
                // int totalDmg = reduction + (int)(reduction*ranNum/100); // 데미지 바운더리 10%
                bossStatus3.TakeDamage(20);
            
                //bossStatus.TakeDamage(dmg);
            }

            if(bossStatus4 != null)
            {
                Debug.Log("보스스테이터스있음");
                // // 플레이어의 방어율 로직도 적용
                int reduction = (int)(8 * (1-(float)bossStatus.defense/(bossStatus.defense+100)));
                int ranNum = Random.Range(-10, 11);
                int totalDmg = reduction + (int)(reduction*ranNum/100); // 데미지 바운더리 10%
                bossStatus4.TakeDamage(65);
             
                //bossStatus.TakeDamage(dmg);
            }
        }
    
    }
}
