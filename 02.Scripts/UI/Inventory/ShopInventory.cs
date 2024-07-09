using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ShopInventory : MonoBehaviour
{

    public Image[] images;
    public Image[] shopSlots;
    public Image[] slots;
    public Image cash;
    public int check;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < ItemManager.userItemList.Count; i++)
        {
            SetSlots();
        }
        for (int i = 1; i < ItemManager.userItemList.Count; i++)
        {
            SetInventoryItem(i,1);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void SetSlots()
    {
        GameObject realItem = Resources.Load<GameObject>("Items/ShopInvenItem");
        GameObject copyItem = Instantiate(realItem);
        copyItem.transform.SetParent(transform);
        copyItem.transform.localScale = new Vector3(1, 1,1);

    }
    public void SetInventoryItem(int i, int check)
    {
        
        images = transform.GetComponentsInChildren<Image>();
        shopSlots = images.Where(child => child.CompareTag("Slot")).ToArray();
        Obj useritem = ItemManager.userItemList[i];
        if (check == 1)
        {
            Image realItem = Resources.Load<Image>("Items/" + useritem.itemName);
            Image copyItem = shopSlots[i - 1].transform.Find("Image").GetComponent<Image>();
            copyItem.sprite = realItem.sprite;
            shopSlots[i - 1].transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = useritem.itemTitle;
            ItemListTable itemCheck = ItemManager.itemListTables[useritem.itemNum-1];
            shopSlots[i - 1].transform.Find("PriceText").GetComponent<TextMeshProUGUI>().text = "가격: " + itemCheck.itemPrice.ToString();
            shopSlots[i - 1].transform.Find("QuantityText").GetComponent<TextMeshProUGUI>().text = "수량: " + useritem.quantity.ToString();
        }
        else if (check == 2)
        {
            shopSlots[i - 1].transform.Find("QuantityText").GetComponent<TextMeshProUGUI>().text = "수량: " + useritem.quantity.ToString();
        }
    }
}
