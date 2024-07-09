using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckCash : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetComponentInChildren<TextMeshProUGUI>().text = ItemManager.userItemList[0].quantity.ToString();
    }
}
