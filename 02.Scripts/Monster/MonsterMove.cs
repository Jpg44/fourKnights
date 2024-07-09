using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterMove : MonoBehaviour
{

    //유저 포지션
    public Vector3 playerPosition;
    // Start is called before the first frame update
    public CharacterController monsterController;
    //몬스터 중력
    public Vector3 monsterGravity = new Vector3(0f, -9.81f, 0f);
    //몬스터 초기 위치
    public Vector3 monsterSetPosition;
    public Vector3 direction;
    //몬스터 상태
    public MonsterState monsterState;
    //타켓 위치 회전값
    public Quaternion targetRotation;
    //유저와 캐릭터 거리차
    public float distance;
    void Start()
    {
        monsterController = GetComponent<CharacterController>();
        monsterSetPosition = transform.position;
        monsterState = GetComponent<MonsterState>();
        monsterState.monsterIsGravity = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    //몬스터 중력 함수
    public void MonsterGravity()
    {
        //몬스터가 중력중 이면 -y방향으로 이동
        if (monsterState.monsterIsGravity)
        {
            monsterController.Move(monsterGravity * Time.deltaTime);
        }
    }
    //몬스터 이동 함수
    public void IsMonsterMove(Collider collider)
    {
        //유저와 몬스터 거리차 계산
        distance=Vector3.Distance(transform.position, playerPosition);
        //몬스터랑 유저랑 거리 차이가 2 이상이면서 몬스터가 리셋 상태가 아닐때 플레이어 방향으로 회전
        if (distance >= 0.01f && !monsterState.monsterReset&&!monsterState.monsterBattle)
        {
            // 플레이어를 향하는 방향 벡터
            direction = (playerPosition - transform.position).normalized;
            direction.y = 0f;
             targetRotation = Quaternion.LookRotation(direction);
        }
        //몬스터랑 유저랑 거리 차이가 1.5 이상이면서 몬스터가 리셋 상태가 아닐때 이동
        if (distance >= monsterState.monsterAttackRange && !monsterState.monsterReset&&!monsterState.monsterBattle)
        {
            monsterState.monsterAnimation = "Move";
            monsterState.monsterIsAnimation = true;
            monsterState.monsterIsMove = true;
            
            transform.rotation = targetRotation;
            // 정면 방향으로 이동할 거리 벡터 계산
            Vector3 moveVector = direction * monsterState.monsterSpeed * Time.deltaTime;
            monsterController.Move(moveVector);

        }
        
    }
    //몬스터 리턴 포지션
    public void MonsterReturnPosition()
    {
        //몬스터가 리셋 상태일때
        if (monsterState.monsterReset)
        {
            monsterState.monsterReset = true;
            // 처음 위치로 향하는 방향 벡터
            direction = (monsterSetPosition - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
            // 정면 방향으로 이동할 거리 벡터 계산
            // Vector3 moveVector = direction * moveSpeed * Time.deltaTime;
            // monsterController.Move(moveVector);

            //트랜스 폼 포지션으로 모든 물체들 관통하고 지나감
            transform.position = Vector3.Lerp(transform.position, monsterSetPosition, monsterState.monsterResetSpeed * Time.deltaTime);
        }

    }
    //몬스터 위치 리셋 할 건지 거리차 구하기 
    public void MonsterStateChange()
    {
        float rDistance =Vector3.Distance(transform.position, monsterSetPosition) ;
        //기존 위치에서 멀어지면 리셋 시작
        if (rDistance> monsterState.monsterResetRange)
        {
            //애니메이션 Move로 바꾸고, 리셋 true,캐릭터 컨트롤러 false, 중력 false, 배틀 false
            monsterState.monsterAnimation = "Move";
            monsterState.monsterReset = true;
            monsterController.enabled = false;
            monsterState.monsterIsGravity = false;
            monsterState.monsterBattle = false;

        }
        //기존위치랑 거진 비슷하면 
        else if (rDistance <= 0.1f)
        {
            //애니메이션 Idle, 리셋 false, 컨트롤러 true, 중력true
            monsterState.monsterAnimation = "Idle";
            monsterState.monsterReset = false;
            monsterController.enabled = true;
            monsterState.monsterIsGravity = true;
        }
    }
    //몬스터가 주변 유저 찾기
    public void MonsterSearch()
    {
        //범위내의 모든 콜라이더 수집
        Collider[] colliders = Physics.OverlapSphere(transform.position, monsterState.monsterSetRange);
        Transform body = transform.Find("MonsterBody");
        foreach (Collider collider in colliders)
        {
            //콜라이더중 플레이어 태그 들고있는애 있으면
            if (collider.CompareTag("Player"))
            {
                monsterState.monsterBody.gameObject.SetActive(true);
                playerPosition = collider.transform.position;
                if (Vector3.Distance(transform.position, playerPosition) < monsterState.monsterSearchRange)
                {
                    IsMonsterMove(collider);
                }
            }

        }
    }

}
