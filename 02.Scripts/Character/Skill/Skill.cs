using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public int damage;
    public float skillRemainTime;
    private CharacterManager characterManager;
    private PhotonPlayerController playerController;

    private void Start()
    {
        // CharacterManager와 PlayerController를 찾음
        characterManager = CharacterManager.Instance;
        playerController = FindObjectOfType<PhotonPlayerController>();

        // 일정 시간이 지나면 오브젝트 파괴
        StartCoroutine(DestroyAfterTime(skillRemainTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        // 몬스터나 보스와 충돌했을 때 데미지 적용 (스킬이 파괴되지 않음)
        if (other.CompareTag("Boss") || other.CompareTag("Monster"))
        {
            ApplyDamage(other);
        }
    }

    private int CalculateDamage()
    {
        double dmg = (double)characterManager.ATK;
        switch(playerController.attackStyle)
        {
            case "sDown2": // 스킬2에 대한 데미지 배율 적용
                dmg = dmg * 2.0;
                break;
            default:
                break;
        }
        return (int)dmg;
    }

    private void ApplyDamage(Collider other)
    {
        int dmg = CalculateDamage();
        
        BossStatus bossStatus = other.GetComponentInChildren<BossStatus>();
        BossPart bossPart = other.GetComponentInChildren<BossPart>();
        DryadStatus bossStatus2 = other.GetComponent<DryadStatus>();
        FlowerStatus bossStatus3 = other.GetComponent<FlowerStatus>();
        RainbowFlowerStatus bossStatus4 = other.GetComponent<RainbowFlowerStatus>();
        MonsterState monsterState = other.GetComponent<MonsterState>();

        if (bossStatus != null)
        {
            int reduction = (int)(dmg * (1 - (float)bossStatus.defense / (bossStatus.defense + 100)));
            int ranNum = Random.Range(-10, 11);
            int totalDmg = reduction + (int)(reduction * ranNum / 100); // 데미지 바운더리 10%
            bossStatus.TakeDamage(totalDmg);
        }
        else if (bossPart != null)
        {
            int reduction = (int)(dmg * (1 - (float)BossStatus.Instance.defense / (BossStatus.Instance.defense + 100)));
            int ranNum = Random.Range(-10, 11);
            int totalDmg = reduction + (int)(reduction * ranNum / 100); // 데미지 바운더리 10%
            bossPart.TakeDamage(totalDmg);
        }
        else if (bossStatus2 != null)
        {
            int reduction = (int)(dmg * (1 - (float)bossStatus2.defense / (bossStatus2.defense + 100)));
            int ranNum = Random.Range(-10, 11);
            int totalDmg = reduction + (int)(reduction * ranNum / 100); // 데미지 바운더리 10%
            bossStatus2.TakeDamage(totalDmg);
        }
        else if (bossStatus3 != null)
        {
            int reduction = (int)(dmg * (1 - (float)bossStatus3.defense / (bossStatus3.defense + 100)));
            int ranNum = Random.Range(-10, 11);
            int totalDmg = reduction + (int)(reduction * ranNum / 100); // 데미지 바운더리 10%
            bossStatus3.TakeDamage(totalDmg);
        }
        else if (bossStatus4 != null)
        {
            int reduction = (int)(dmg * (1 - (float)bossStatus4.defense / (bossStatus4.defense + 100)));
            int ranNum = Random.Range(-10, 11);
            int totalDmg = reduction + (int)(reduction * ranNum / 100); // 데미지 바운더리 10%
            bossStatus4.TakeDamage(totalDmg);
        }
        else if (monsterState != null)
        {
            int reduction = (int)(dmg * 1); // 여기에 몬스터 방어율 적용 로직 추가 가능
            int ranNum = Random.Range(-10, 11);
            int totalDmg = reduction + (int)(reduction * ranNum / 100); // 데미지 바운더리 10%
            monsterState.monsterHp -= totalDmg;
            monsterState.monsterIsHit = true;
            monsterState.monsterHitDamage = totalDmg;
        }
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
