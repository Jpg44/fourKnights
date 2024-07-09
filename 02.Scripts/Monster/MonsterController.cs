using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public MonsterState monsterState;
    public MonsterMove monsterMove;
    public MonsterAttack monsterAttack;
    public MonsterAnimations monsterAnimations;
    public MonsterDead monsterDead;

    // Start is called before the first frame update
    void Start()
    {
        monsterState = GetComponent<MonsterState>();
        monsterMove = GetComponent<MonsterMove>();
        monsterAttack = GetComponent<MonsterAttack>();
        monsterAnimations = GetComponent<MonsterAnimations>();
        monsterDead = GetComponent<MonsterDead>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!monsterState.monsterIsDead)
        {
            //몬스터 중력
            monsterMove.MonsterGravity();
            //몬스터 주변 탐색 후 움직임
            monsterMove.MonsterSearch();
            //몬스터 움직이다 상태 변화(복귀,기본)
            monsterMove.MonsterStateChange();
            //몬스터 공격
            monsterAttack.MonsterStartAttack();
        }
        //몬스터 제자리 복귀
        monsterMove.MonsterReturnPosition();

        //몬스터 죽음
        monsterDead.MonsterDie();
    }
}
