using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimations : MonoBehaviour
{
    public Animator monsterAnimator;
    public MonsterState monsterState;
    public Transform monsterBody;
    public bool playing;
    public int Key;
    // Start is called before the first frame update
    void Awake()
    {
        monsterBody = transform.Find("MonsterBody");
        monsterAnimator = monsterBody.GetComponentInChildren<Animator>();
        monsterState = GetComponent<MonsterState>();

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(MonsterAnimationPlay(monsterState.monsterAnimation));

    }

    public IEnumerator MonsterAnimationPlay(string animatorName)
    {
        //몬스터가 애니메이터가 있고 애니메이션 중일 때
        if (monsterAnimator != null && monsterState.monsterIsAnimation)
        {
            monsterAnimator.speed = 1.0f;
            monsterState.monsterIsAnimation = false;
            //애니메이션 이름에 맞게 스위치 실행
            switch (animatorName)
            {
                case "Idle":
                    monsterAnimator.Play("Idle");
                    break;
                case "Attack":
                    monsterState.monsterOneAnimation = true;
                    monsterAnimator.Play("Attack");
                    yield return new WaitForSeconds(0.5f);
                    monsterAnimator.Play("Idle");
                    yield return new WaitForSeconds(1);
                    monsterState.monsterOneAnimation = false;
                    monsterState.monsterBattle = false;
                    break;
                case "Move":
                    monsterAnimator.Play("Move");
                    monsterState.monsterOneAnimation = false;
                    break;
                case "Die":
                    monsterState.monsterOneAnimation = true;
                    monsterState.deadAudioSource.Play();
                    monsterAnimator.Play("Die");
                    yield return new WaitForSeconds(0.5f);
                    monsterState.monsterBody.gameObject.SetActive(false);
                    monsterState.monsterHp = monsterState.monsterMaxHp;
                    monsterState.monsterReset = true;
                    monsterState.monsterIsGravity = false;
                    monsterState.monsterBattle = false;
                    yield return new WaitForSeconds(5);
                    monsterState.monsterBody.gameObject.SetActive(true);
                    monsterState.monsterIsDead = false;
                    monsterAnimator.Play("Idle");
                    monsterState.monsterController.enabled = true;
                    monsterState.monsterOneAnimation = false;
                    break;
            }

        }
    }


}
