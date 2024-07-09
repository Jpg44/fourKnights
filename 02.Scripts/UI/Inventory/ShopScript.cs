using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ShopScript : MonoBehaviour
{
    public Image[] images;
    public Image[] shopSlots;
    public Image[] slots;
    public Image cash;
    public int check;

    // Start is called before the first frame update
    void Start()
    {

        SetSlots(ItemManager.shopList.Count);

        images = transform.GetComponentsInChildren<Image>();
        shopSlots = images.Where(child => child.CompareTag("Slot")).ToArray();
        SetShopItem();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void SetSlots(int cnt)
    {
        for (int i = 0; i < cnt; i++)
        {
            GameObject realItem = Resources.Load<GameObject>("Items/ShopItem");
            GameObject copyItem = Instantiate(realItem);
            copyItem.transform.SetParent(transform);
            copyItem.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    public void SetShopItem()
    {
        for (int i = 0; i < ItemManager.shopList.Count; i++)
        {
            ItemListTable shopItem = ItemManager.shopList[i];
            Image realItem = Resources.Load<Image>("Items/" + shopItem.itemName);
            Image copyItem = shopSlots[i].transform.Find("Image").GetComponent<Image>();
            copyItem.sprite = realItem.sprite;
            shopSlots[i].transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = shopItem.itemTitle;
            shopSlots[i].transform.Find("PriceText").GetComponent<TextMeshProUGUI>().text = "가격: " + shopItem.itemPrice.ToString();
        }
    }


}
