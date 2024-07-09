using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//아이템 리스트 구조체
public struct ItemListTable
{
    //아이템 pk값
    public int itemNum;
    //아이템 영어 이름
    public string itemName;
    //아이템 한글 이름
    public string itemTitle;
    //아이템 설명 설명
    public string itemExplanation;
    //아이템 가격
    public int itemPrice;
}
//유저가 가지고 있는 데이터 구조체
public struct Obj
{
    //아이템 pk값
    public int itemNum;
    //아이템 영어 이름
    public string itemName;
    //아이템 한글 이름
    public string itemTitle;

    //아이템 설명 글귀
    public string itemExplanation;
    //아이템 수량
    public int quantity;
    //아이템 인벤토리 위치 값
    public int inventoryNum;
}
public class ItemManager : MonoBehaviour
{
    public static ItemManager instance = null;
    //아이템 테이블 구조체 리스트

    public static List<ItemListTable> itemListTables = new List<ItemListTable>();
    //유저가 가진 아이템 리스트
    public static List<Obj> userItemList = new List<Obj>();
    //상점 리스트
    public static List<ItemListTable> shopList = new List<ItemListTable>();
    public BackEndDataReceiver backEndDataReceiver;

    // 아이템 및 재화 정보
    // 골드
    // 
    // private void OnDestroy()
    // {
    //     ItemManager.itemListTables.Clear();
    // }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        backEndDataReceiver = GameObject.Find("BackEndDataReceiver").GetComponent<BackEndDataReceiver>();
        if (itemListTables.Count == 0)
        {
            ItemAllTableSet();
        }
    }
    void Start()
    {
        for (int i = 0; i < backEndDataReceiver.inventoryNum.Count; i++)
        {
            Obj obj = new Obj();
            obj.itemName = itemListTables[backEndDataReceiver.itemPk[i] - 1].itemName;
            obj.itemTitle = itemListTables[backEndDataReceiver.itemPk[i] - 1].itemTitle;
            obj.itemNum = backEndDataReceiver.itemPk[i]; // item의 pk값
            obj.itemExplanation = itemListTables[backEndDataReceiver.itemPk[i] - 1].itemExplanation;
            obj.inventoryNum = backEndDataReceiver.inventoryNum[i]; // 여기에 해당되는 인벤 위치
            obj.quantity = backEndDataReceiver.quantity[i]; // 여기에 해당되는 수량
            userItemList.Add(obj);
        }
        if (userItemList.Count == 0)
        {
            Obj obj = new Obj();
            obj.itemName = itemListTables[0].itemName;
            obj.itemTitle = itemListTables[0].itemTitle;
            obj.itemNum = itemListTables[0].itemNum;
            obj.itemExplanation = itemListTables[0].itemExplanation;
            obj.inventoryNum = 0;
            obj.quantity = 0;
            userItemList.Add(obj);
        }
    }
    void Update()
    {
        CheckScene();
    }
    public void CheckScene()
    {
        // 현재 씬 가져오기
        Scene currentScene = SceneManager.GetActiveScene();

        // 현재 씬의 이름 출력
        string sceneName = currentScene.name;
        if (sceneName == "CharacterScene")
        {
            Destroy(gameObject);
            Destroy(GameObject.Find("BossManager").gameObject);
        }
    }
    public void ItemAllTableSet()
    {

        //아이템 테이블 리스트 엑셀 가져오기
        TextAsset textAsset = Resources.Load<TextAsset>("ItemAllTable");
        //아이템 엑셀 text로 만들기
        string sr = textAsset.text;
        //text 한 줄씩 나누기
        string[] line = sr.Split('\n');

        for (int i = 0; i < line.Length - 1; i++)
        {
            //쉼표별로 배열에 저장
            if (i != 0)
            {

                string[] data_values = line[i].Split(',');
                ItemListTable itemListTable = new ItemListTable();
                itemListTable.itemNum = int.Parse(data_values[0]);
                itemListTable.itemName = data_values[1];
                itemListTable.itemTitle = data_values[2];
                itemListTable.itemExplanation = data_values[3];
                itemListTable.itemPrice = int.Parse(data_values[4]);
                itemListTables.Add(itemListTable);
                if (i == 2)
                {
                    //상점에 현재 포션만 있음
                    shopList.Add(itemListTable);
                }
            }
        }
        //혹시 모를 기본 골드 0값 넣어주기
        // if (userItemList.Count == 0)
        // {
        //     Obj obj = new Obj();
        //     obj.itemName = itemListTables[0].itemName;
        //     obj.itemTitle = itemListTables[0].itemTitle;
        //     obj.itemNum = itemListTables[0].itemNum;
        //     obj.itemExplanation = itemListTables[0].itemExplanation;
        //     obj.inventoryNum = 0;
        //     obj.quantity = 0;
        //     userItemList.Add(obj);
        // }
    }
    public void userListAddItem(ItemListTable addItem, int quantity)
    {
        int noInven = 0;
        int[] setInven = new int[userItemList.Count + 1];
        int minInven = 0;
        //인벤토리 가까운 빈 곳 찾기
        for (int i = 0; i < userItemList.Count; i++)
        {
            Obj invenItemNum = userItemList[i];
            if (invenItemNum.inventoryNum < userItemList.Count)
            {
                setInven[invenItemNum.inventoryNum] = 1;
            }
        }
        for (int i = 0; i < userItemList.Count + 1; i++)
        {
            if (setInven[i] != 1)
            {
                minInven = i;
                break;
            }
        }
        //있으면 수량만 늘려주기
        for (int j = 0; j < userItemList.Count; j++)
        {
            if (userItemList[j].itemNum == addItem.itemNum)
            {
                Obj invenItem = new Obj();
                noInven = 1;
                invenItem = userItemList[j];
                invenItem.quantity += quantity;
                userItemList[j] = invenItem;
                if (GameObject.Find("Inventory") != null)
                {
                    GameObject.Find("Inventory").GetComponent<ShopInventory>().SetInventoryItem(userItemList.Count - 1, 2);
                }
            }
        }
        //인벤토리에 없으면 유저아이템 리스트에 넣어줌
        if (noInven == 0)
        {
            Obj noInvenItem = new Obj();
            noInvenItem.itemNum = addItem.itemNum;
            noInvenItem.itemName = addItem.itemName;
            noInvenItem.itemTitle = addItem.itemTitle;
            noInvenItem.itemExplanation = addItem.itemExplanation;

            noInvenItem.quantity = quantity;
            noInvenItem.inventoryNum = minInven;

            userItemList.Add(noInvenItem);
            if (GameObject.Find("Inventory") != null)
            {
                GameObject.Find("Inventory").GetComponent<ShopInventory>().SetSlots();
                GameObject.Find("Inventory").GetComponent<ShopInventory>().SetInventoryItem(userItemList.Count - 1, 1);
            }
        }
    }
    public void userListRemoveItem(ItemListTable addItem, int quantity, Transform getTransform)
    {
        for (int i = 1; i < userItemList.Count; i++)
        {
            Obj item = userItemList[i];
            if (addItem.itemNum == item.itemNum)
            {
                int totalQuantity = item.quantity - quantity;
                print(totalQuantity);
                if (totalQuantity > 0)
                {
                    item.quantity = totalQuantity;
                    userItemList[i] = item;
                    if (GameObject.Find("Inventory") != null)
                    {
                        GameObject.Find("Inventory").GetComponent<ShopInventory>().SetInventoryItem(i, 2);
                    }
                    int totalPlus = addItem.itemPrice * quantity;
                    Obj obj = ItemManager.userItemList[0];
                    obj.quantity += totalPlus;
                    ItemManager.userItemList[0] = obj;
                    break;
                }
                else if (totalQuantity == 0)
                {
                    //인벤 슬롯창 아이템 

                    GameObject.Find("PlayerUI").transform.Find("InventoryPanel").gameObject.SetActive(true);
                    Transform[] invenItem = GameObject.Find(item.inventoryNum.ToString()).GetComponentsInChildren<Transform>();
                    RectTransform[] shopInvenItem = getTransform.transform.GetComponentsInChildren<RectTransform>();
                    //슬롯창까지 가져오기에 슬롯창을 뺀 삭제
                    for (int y = 1; y < invenItem.Length; y++)
                    {
                        Destroy(invenItem[y].gameObject);

                    }
                    //슬롯창까지 삭제
                    for (int y = 0; y < shopInvenItem.Length; y++)
                    {
                        Destroy(shopInvenItem[y].gameObject);
                    }
                    GameObject.Find("PlayerUI").transform.Find("InventoryPanel").gameObject.SetActive(false);
                    int totalPlus = addItem.itemPrice * quantity;
                    Obj obj = ItemManager.userItemList[0];
                    obj.quantity += totalPlus;
                    ItemManager.userItemList[0] = obj;

                    userItemList.RemoveAt(i);
                }

                else if (totalQuantity < 0)
                {
                    print("팔수 없음");
                }
            }
        }
    }
}
