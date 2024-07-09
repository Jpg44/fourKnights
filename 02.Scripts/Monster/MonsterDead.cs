using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDead : MonoBehaviour
{
    public MonsterState monsterState;
    public MonsterAttack monsterAttack;
    public ItemManager itemManager;
    public Obj noInvenItem;
    public ItemListTable dropItem;
    // Start is called before the first frame update
    void Start()
    {
        itemManager = GameObject.Find("ItemManger").transform.GetComponent<ItemManager>();
        monsterState = GetComponent<MonsterState>();
        monsterAttack = GetComponent<MonsterAttack>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //몬스터 죽으면 변화 상태 (죽음,배틀,애니메이션,애니메이션 진행중,몬스터 컨트롤러)
    public void MonsterDie()
    {
        if (monsterState.monsterName != null && monsterState.monsterHp <= 0 && !monsterState.monsterIsDead )
        {
            monsterState.monsterIsDead = true;
            monsterState.monsterBattle = false;
            monsterState.monsterAnimation = "Die";
            monsterState.monsterIsAnimation = true;
            monsterState.monsterController.enabled = false;
            MonsterDrop();
        }
    }

    //몬스터 드랍
    public void MonsterDrop()
    {
        //몬스터가 가진 드랍테이블 확인
        for (int i = 0; i < monsterState.monsterDropTable.Count; i++)
        {   //드랍테이블에 있는 확률에 따라 드랍
            if (monsterState.monsterDropTable[i].percentage > Random.Range(0, 100))
            {   //확률에 따라 드랍이 되면 드랍테이블에 있는 수량대로 득
                int dropQ = monsterState.monsterDropTable[i].quantity;
                //만약 골드면 골드는 현재 수량의 +-10퍼로 변동되어 득
                if (monsterState.monsterDropTable[i].itemNum == 1)
                {
                    int tenP = monsterState.monsterDropTable[i].quantity / 10;
                    dropQ = Random.Range(dropQ - tenP, dropQ + tenP);
                }
                for (int k = 0; k < ItemManager.itemListTables.Count; k++)
                {
                    ItemListTable item = ItemManager.itemListTables[k];
                    if (item.itemNum == monsterState.monsterDropTable[i].itemNum)
                    {
                        dropItem = item;
                        break;
                    }
                }
                itemManager.userListAddItem(dropItem, dropQ);
            }
        }

    }
}
