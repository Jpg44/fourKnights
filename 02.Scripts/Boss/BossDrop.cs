using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDrop : MonoBehaviour
{
    public int cnt;
    public Image[] bossDropItems;
    [SerializeField]
    public int goldDropQuantity;//드랍 골드 수량
    
    public int dropItem1;//드랍 아이템1
    public int dropItem1Quantity;//드랍 아이템1 수량
    public int dropItem2;//드랍 아이템2
    public int dropItem2Quantity;//드랍 아이템2 수량

    // Start is called before the first frame update
    void Start()
    {
        BossDropAdd(0, goldDropQuantity);

        BossDropAdd(dropItem1, dropItem1Quantity);

        BossDropAdd(dropItem2, dropItem2Quantity);
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void BossDropAdd(int itemNum, int quantity)
    {
        cnt += 1;
        ItemListTable item = ItemManager.itemListTables[itemNum];
        GameObject.Find("ItemManger").GetComponent<ItemManager>().userListAddItem(item, quantity);
        BossDropImage(itemNum, cnt);
    }
    public void BossDropImage(int itemNum, int listNum)
    {
        ItemListTable item = ItemManager.itemListTables[itemNum];
        print(item.itemName+" "+listNum);
        Image reload = Resources.Load<Image>("Items/" + item.itemName);
        print("dlrj"+reload);
        bossDropItems = transform.Find("BossDropItems").GetComponentsInChildren<Image>();
        bossDropItems[listNum].sprite = reload.sprite;
    }
}
