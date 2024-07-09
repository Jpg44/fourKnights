using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UserItem : MonoBehaviour
{

    // private void OnDestroy()
    // {
    //     UserItem.userItemList.Clear();
    // }
    public Obj obj = new Obj();

    public Transform previousParentI;
    public CharacterManager characterManager;
    public RectTransform inventoryPanel;
    public TextMeshProUGUI tmp;
    public Image[] images;
    public Image[] slots;
    public Image cash;
    public Image[] items;
    public Image copyItem;
    public UserItem userItem;
    public static bool isDrag;
    int key = 0;


    // Start is called before the first frame update
    void Awake()
    {
        // GetUserItemList();
        // inventoryPanel = GetComponent<RectTransform>();
        // items = inventoryPanel.GetComponentsInChildren<Image>();
        // previousParentI = GetComponent<RectTransform>();
        // //characterManager = GetComponent<CharacterManager>();
        // userItem = GetComponent<UserItem>();
        // ItemListApply();
    }
    void Start()
    {
        GetUserItemList();
        inventoryPanel = GetComponent<RectTransform>();
        images = inventoryPanel.GetComponentsInChildren<Image>();
        slots = images.Where(child => child.CompareTag("Slot")).ToArray();
        cash = System.Array.Find(images, child => child.CompareTag("Cash"));
        previousParentI = GetComponent<RectTransform>();
        //characterManager = GetComponent<CharacterManager>();
        userItem = GetComponent<UserItem>();
        //SetItem();

    }
    // Update is called once per frame
    void Update()
    {
        // foreach (Obj obj in temList)
        // {
        //     print("이름:" + obj.name + "수량" + obj.quantity);
        // }
        SetItem();
    }
    public void GetUserItemList()
    {
        // for (int i = 2; i < 5; i++)
        // {
        //     foreach (ItemListTable item in ItemManager.itemListTables)
        //     {
        //         if (item.itemNum == i)
        //         {
        //         }

        //     }
        // }
    }
    public void SetItem()
    {
        if (!isDrag)
        {
            foreach (Obj obj in ItemManager.userItemList)
            {
                if (obj.itemName == "Gold")
                {
                    cash.GetComponentInChildren<TextMeshProUGUI>().text = obj.quantity.ToString();
                }
                if (obj.inventoryNum - 1 >= 0)
                {
                    if (obj.itemName != "Gold" && slots[obj.inventoryNum - 1].transform.childCount == 0)
                    {
                        GameObject realItem = Resources.Load<GameObject>("Items/" + obj.itemName);
                        GameObject copyItem = Instantiate(realItem);
                        copyItem.transform.parent = slots[obj.inventoryNum - 1].transform;
                        copyItem.transform.localScale = new Vector3(1, 1, 1);
                        copyItem.GetComponent<RectTransform>().position = slots[obj.inventoryNum - 1].GetComponent<RectTransform>().position;
                        copyItem.GetComponentInChildren<TextMeshProUGUI>().text = obj.quantity.ToString();
                    }
                    else if (obj.itemName != "Gold" && slots[obj.inventoryNum - 1].transform.childCount != 0)
                    {
                        slots[obj.inventoryNum - 1].transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = obj.quantity.ToString();
                    }
                }
            }
        }
    }
    public void UpdateItem()
    {

    }

}

