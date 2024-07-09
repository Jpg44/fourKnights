using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuantityModal : MonoBehaviour
{
    public ItemManager itemManager;
    // Start is called before the first frame update
    public Transform itemTranform;
    public string checkExplanation;
    void Start()
    {
        itemManager = GameObject.Find("ItemManger").transform.GetComponent<ItemManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            EnterPass();
        }
    }
    public void EnterPass()
    {
        string quantity = transform.GetComponentInChildren<TMP_InputField>().text;
        int parsedQuantity;
        if (int.TryParse(quantity, out parsedQuantity))
        {

            AfterQuantity(itemTranform, parsedQuantity);
        }
        transform.GetComponentInChildren<TMP_InputField>().text = null;
        gameObject.SetActive(false);
        List<string> uiNumber = GameObject.Find("PlayerUI").transform.GetComponent<InventoryKey>().uiNum;
        for (int i = 0; i < uiNumber.Count; i++)
        {
            if (uiNumber[i].Equals("QuantityInput"))
            {
                uiNumber.RemoveAt(i);
            }
        }
    }
    public void GetTransfom(Transform getTransform)
    {
        itemTranform = getTransform;
        Transform explanation = transform.Find("Explanation");
        print(explanation.GetComponent<TextMeshProUGUI>().text);
        if (getTransform.name == "ShopItem(Clone)")
        {
            explanation.GetComponent<TextMeshProUGUI>().text = "얼만큼 사시겠습니까?";

        }
        else if (getTransform.name == "ShopInvenItem(Clone)")
        {
            explanation.GetComponent<TextMeshProUGUI>().text = "얼만큼 파시겠습니까?";

        }
    }
    public void AfterQuantity(Transform getTransform, int quantity)
    {
        Image img = getTransform.Find("Image").GetComponent<Image>();
        print(getTransform.name);
        if (getTransform.name == "ShopItem(Clone)")
        {
            BuyItem(img.sprite.name, quantity);
        }
        else if (getTransform.name == "ShopInvenItem(Clone)")
        {
            SellItem(img.sprite.name, quantity, getTransform);
        }
    }

    public ItemListTable checkItem(string name)
    {
        ItemListTable buyItem = new ItemListTable();
        for (int i = 1; i < ItemManager.itemListTables.Count; i++)
        {
            ItemListTable item = ItemManager.itemListTables[i];
            if (item.itemName == name)
            {
                buyItem = item;
                break;
            }
        }
        return buyItem;
    }
    public void BuyItem(string name, int quantity)
    {
        ItemListTable item = checkItem(name);
        if (ItemManager.userItemList[0].quantity >= item.itemPrice * quantity)
        {
            Obj obj = ItemManager.userItemList[0];
            int totalMinus = item.itemPrice * quantity;
            obj.quantity -= totalMinus;
            ItemManager.userItemList[0] = obj;
            itemManager.userListAddItem(item, quantity);
        }
        //GetComponentInParent<ShopInventory>().SetInventoryItem();
    }
    public void SellItem(string name, int quantity, Transform getTransform)
    {
        ItemListTable item = checkItem(name);
        itemManager.userListRemoveItem(item, quantity, getTransform);

    }
}
