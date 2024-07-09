using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;


public class MonsterAttack : MonoBehaviour
{
    public MonsterState monsterState;
    public Animator monsterAnimator;
    public MonsterDead monsterDead;
    public GameObject characterManagerObject;
    public CharacterManager characterManager;
    public float attackTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        monsterState = GetComponent<MonsterState>();
        monsterDead = GetComponent<MonsterDead>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }
    //몬스터 공격 시작
    public void MonsterStartAttack()
    {
        monsterAnimator = GetComponentInChildren<Animator>();
        //현재 배틀 중인지 애니메이션
        if (monsterState.monsterBattle && monsterAnimator != null && !monsterState.monsterOneAnimation)
        {
            monsterState.monsterIsAnimation = true;
            monsterState.monsterAnimation = "Attack";
            StartCoroutine(MonsterDamage());
        }
    }
    public IEnumerator MonsterDamage()
    {
        characterManagerObject = GameObject.FindWithTag("CharacterManager");
        characterManager = characterManagerObject.GetComponent<CharacterManager>();
        if (characterManager.isAlive)
        {
            characterManager.SetHP(monsterState.monsterAttackForce);
            GameObject.Find("PlayerUI").transform.Find("DamageImage").gameObject.SetActive(true);
            GameObject.Find("PlayerUI").transform.Find("DamageImage").gameObject.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            Debug.Log("공격!");
            yield return new WaitForSeconds(0.5f);
            GameObject.Find("PlayerUI").transform.Find("DamageImage").gameObject.SetActive(false);
        }
    }

    public void Attack()
    {
        RaycastHit hit;
        float radius = monsterState.monsterAttackRange;
        if (Physics.SphereCast(transform.position, radius, transform.forward, out hit, monsterState.monsterAttackRange))
        {
            // 레이를 그립니다.
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
            if (hit.collider.CompareTag("Player"))
            {
                monsterState.monsterIsMove = false;
                monsterState.monsterBattle = true;
            }
        }
    }
}
