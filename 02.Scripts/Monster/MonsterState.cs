using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct MonsterStateObject
{
    public string monsterName; //몬스터 이름
    public float monsterHp; //몬스터 현재 체력
    public float monsterMaxHp; //몬스터 최대 체력
    public float monsterSpeed; //몬스터 스피드
    public int monsterAttackForce; //몬스터 공격력
    public float monsterAttackRange; // 몬스터 공격 사거리
    public float monsterAttackInterval; //몬스터 공격 간격 (초)
    public float monsterSetRange; //몬스터 생성 범위
    public float monsterSearchRange; //몬스터 인식 범위
    public float monsterResetRange; //몬스터 리셋 범위
    public float monsterResetSpeed; //몬스터 리셋 스피드
}
public struct MonsterDropTable
{
    public string monsterName;
    public int itemNum;
    public int quantity;
    public float percentage;
}
public class MonsterState : MonoBehaviour
{
    public bool monsterBattle; //몬스터 전투 중 인지
    public bool monsterReset; //몬스터 리셋
    public bool monsterIsGravity; //몬스터 중력
    public bool monsterIsDead; //몬스터 죽음 확인
    public bool monsterIsMove; // 몬스터 이동 중 인지
    public bool monsterIsAnimation; //몬스터 애니메이션 중
    public bool monsterOneAnimation; //몬스터 한 번 애니메이션
    public bool monsterIsHit;// 몬스터가 지금 피격 중 인지
    public string monsterAnimation; //몬스터 애니메이션 이름
    public string monsterName; //몬스터 이름
    public float monsterHp; //몬스터 현재 체력
    public float monsterMaxHp; //몬스터 최대 체력
    public float monsterSpeed; //몬스터 스피드
    public int monsterAttackForce; //몬스터 공격력
    public float monsterAttackRange; // 몬스터 공격 사거리
    public float monsterAttackInterval; //몬스터 공격 간격 (초)
    public float monsterSetRange; //몬스터 생성 범위
    public float monsterSearchRange; //몬스터 인식 범위
    public float monsterResetRange; //몬스터 리셋 범위
    public float monsterResetSpeed; //몬스터 리셋 스피드
    public int monsterHitDamage;//몬스터가 맞은 데미지
    public Transform monsterBody;
    public CharacterController monsterController;
    public ExcelLoadScript excelLoadScriptState;
    public ExcelLoadScript excelLoadScriptDrop;
    public List<MonsterStateObject> monsterAllStateObject;
    public MonsterStateObject monster;
    //몬스터 각자 드랍 테이블 정리
    public List<MonsterDropTable> monsterDropTable;
    public AudioSource hitAudioSource;
     public AudioSource deadAudioSource;
   
    void Start()
    {
        monsterBody = transform.Find("MonsterBody");
        monsterController = GetComponent<CharacterController>();
        //csv별 로드 스크립트 가져오기
        excelLoadScriptState = GameObject.Find("MonsterAllState").GetComponent<ExcelLoadScript>();
        excelLoadScriptDrop = GameObject.Find("MonsterDropTable").GetComponent<ExcelLoadScript>();
        monsterDropTable = new List<MonsterDropTable>();
        for (int i = 0; i < excelLoadScriptState.monsterStateObjects.Count; i++)
        {
            monster = excelLoadScriptState.monsterStateObjects[i];
            MonsterSetState(monster);
        }
        
    }
    void Update()
    {
    }
    public void MonsterSetState(MonsterStateObject monster)
    {
        monsterName = transform.Find("").ToString().Split(' ')[0];
        if (monsterName.Equals(monster.monsterName))
        {
            if(monsterName.Equals("Spider")){
                print(monster.monsterHp);
            }
            monsterAnimation = "Idle";
            monsterHp = monster.monsterHp;
            monsterMaxHp = monster.monsterMaxHp;
            monsterSpeed = monster.monsterSpeed;
            monsterAttackForce = monster.monsterAttackForce;
            monsterAttackRange = monster.monsterAttackRange;
            monsterAttackInterval = monster.monsterAttackInterval;
            monsterSetRange = monster.monsterSetRange;
            monsterSearchRange = monster.monsterSearchRange;
            monsterResetRange = monster.monsterResetRange;
            monsterResetSpeed = monster.monsterResetSpeed;
            deadAudioSource=GameObject.Find(monsterName+"DeadAudio").GetComponent<AudioSource>();
            hitAudioSource=GameObject.Find("PlayerAttackAudio").GetComponent<AudioSource>();
        }
        for (int i = 0; i < excelLoadScriptDrop.monsterDropTables.Count; i++)
        {
            if (monsterName.Equals(excelLoadScriptDrop.monsterDropTables[i].monsterName))
            {
                monsterDropTable.Add(excelLoadScriptDrop.monsterDropTables[i]);
            }
        }

    }

}

