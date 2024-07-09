using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PotionSlot : MonoBehaviour
{
    public string potionQuantity;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Find("PotionQuantity").GetComponentInChildren<TextMeshProUGUI>().text = potionQuantity;
        SearchItem(2);
    }
    public void SearchItem(int index)
    {
        int searchCheck = 0;
        for (int i = 0; i < ItemManager.userItemList.Count; i++)
        {
            Debug.Log("현재 아이템 숫자 : " + ItemManager.userItemList[i].itemNum);
            if (ItemManager.userItemList[i].itemNum == index)
            {
                Obj obj = ItemManager.userItemList[i];
                potionQuantity = obj.quantity.ToString();
                searchCheck = 1;
            }
        }
        if (searchCheck == 0)
        {
            potionQuantity = "0";
        }
    }
}
