using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ShopItemClick : MonoBehaviour, IPointerClickHandler
{
    public ItemManager itemManager;
    // Start is called before the first frame update
    void Start()
    {
        itemManager = GameObject.Find("ItemManger").transform.GetComponent<ItemManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private const float doubleClickTime = 0.3f; // 더블클릭 간격 시간 설정 (초)
    private float lastClickTime;

    public void OnPointerClick(PointerEventData eventData)
    {
        // 클릭 사이의 간격 시간 계산
        if (Time.time - lastClickTime < doubleClickTime)
        {
            OnDoubleClick();
        }
        lastClickTime = Time.time;
    }

    private void OnDoubleClick()
    {
        // 더블클릭 이벤트 처리 로직

        GameObject.Find("Item Shop Group").transform.Find("QuantityInput").gameObject.SetActive(true);
        GameObject.Find("PlayerUI").transform.GetComponent<InventoryKey>().uiNum.Add("QuantityInput");
        GameObject.Find("QuantityInput").GetComponent<QuantityModal>().GetTransfom(transform);
        // 여기서 원하는 동작을 구현하면 됩니다.5e
    }
    
   

}
