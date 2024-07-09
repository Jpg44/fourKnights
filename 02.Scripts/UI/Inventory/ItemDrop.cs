using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDrop : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    public Image img;

    public RectTransform rect;
    public RectTransform changeRect;
    public Image[] childImgs;
    public Image changeImg;

    public UserItem userItem;
    // Start is called before the first frame update
    void Awake()
    {
        //itemList = GetComponentInParent<ItemList>();
        img = GetComponent<Image>();
        changeRect = GetComponent<RectTransform>();
        rect = GetComponent<RectTransform>();
    }
    //마우스 포인터가 현재 아이템 슬롯 영역 내부로 들어갈 때 1회 호출
    public void OnPointerEnter(PointerEventData eventData)
    {
        //아이템 색상 변경
        img.color = Color.yellow;
        if (!UserItem.isDrag)
        {
            childImgs = GetComponentsInChildren<Image>();
            foreach (var chaild in childImgs)
            {
                if (chaild.CompareTag("Item"))
                {
                    chaild.transform.Find("ExplanationPanel").gameObject.SetActive(true);
                }
            }
        }
    }
    //마우스 포인터가 현재 아이템 슬롯 영역을 빠져나갈 때 1회 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        //아이템 슬롯의 색상을 하얀색으로 변경
        img.color = Color.white;
         if (!UserItem.isDrag)
        {
            childImgs = GetComponentsInChildren<Image>();
            foreach (var chaild in childImgs)
            {
                if (chaild.CompareTag("Item"))
                {
                    chaild.transform.Find("ExplanationPanel").gameObject.SetActive(false);
                }
            }
        }
    }
    //현재 아이템 슬록 영역 내부에서 드롭 했을 때 1회 호출
    public void OnDrop(PointerEventData eventData)
    {
        //포인터 드래그는 현재 드래그 하고있는 대상 (아이템)
        if (eventData.pointerDrag != null)
        {
            //드래그하고 있는 대상의 부모를 현재 오브젝트로 설정하고,위치를 현재 오브젝트 위치와 동일하게 설정
            childImgs = GetComponentsInChildren<Image>();
            foreach (var child in childImgs)
            {
                if (child.CompareTag("Item"))
                {
                    //pTransform = itemList.previousParentI;
                    changeImg = child;
                    changeImg.transform.SetParent(InventoryDrag.previousParent);
                    changeImg.rectTransform.position = InventoryDrag.previousParent.GetComponent<RectTransform>().position;
                }
            }
            if (eventData.pointerDrag != null)
            {
                eventData.pointerDrag.transform.SetParent(transform);
                eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;
            }
            int changeNum = 0;
            for (int i = 0; i < ItemManager.userItemList.Count; i++)
            {
                //print("부모:"+int.Parse(InventoryDrag.previousParent.name)+" 데이터:"+ UserItem.userItemList[i].inventoryNum);
                if (changeNum == 0 && int.Parse(InventoryDrag.previousParent.name) == ItemManager.userItemList[i].inventoryNum)
                {
                    changeNum = ItemManager.userItemList[i].itemNum;
                    Obj obj = new Obj();
                    obj = ItemManager.userItemList[i];
                    obj.inventoryNum = int.Parse(transform.name);
                    ItemManager.userItemList[i] = obj;
                    //print("이건이름:" + UserItem.userItemList[i].itemName + " 인벤토리:" + UserItem.userItemList[i].inventoryNum + " 개수:" + UserItem.userItemList[i].quantity);
                }
            }
            for (int i = 0; i < ItemManager.userItemList.Count; i++)
            {
                if (changeNum != ItemManager.userItemList[i].itemNum && int.Parse(transform.name) == ItemManager.userItemList[i].inventoryNum)
                {

                    Obj obj = new Obj();
                    obj = ItemManager.userItemList[i];
                    obj.inventoryNum = int.Parse(InventoryDrag.previousParent.name);
                    ItemManager.userItemList[i] = obj;
                }

            }
        }
        UserItem.isDrag = false;
    }
    // Update is called once per frame
    void Update()
    {
    }
}
