using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryKey : MonoBehaviour
{
    //인벤토리 켜져있는지 확인하는 값
    public bool setInventory;
    public List<string> uiNum = new List<string>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            setInventory = !setInventory;
            transform.Find("InventoryPanel").gameObject.SetActive(setInventory);
            if (setInventory)
            {
                GameObject.Find("CharacterManager").GetComponent<CharacterManager>().IsUI(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                UserItem.isDrag = false;
                uiNum.Add("InventoryPanel");
            }
            if (!setInventory)
            {

                GameObject.Find("CharacterManager").GetComponent<CharacterManager>().IsUI(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                for (int i = 0; i < uiNum.Count; i++)
                {
                    if (uiNum[i].Equals("InventoryPanel"))
                    {
                        //print("여기"+uiNum[i]);
                        uiNum.RemoveAt(i);
                        //print("삭제");
                    }
                }
            }
            for (int i = 0; i < ItemManager.userItemList.Count; i++)
            {
                Obj item = ItemManager.userItemList[i];
                print(i + " " + item.itemName + " " + item.quantity);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (uiNum.Count - 1 >= 0)
            {
                if (!uiNum[uiNum.Count - 1].Equals("Raid Group"))
                {
                    print(uiNum[uiNum.Count - 1]);

                    GameObject.Find(uiNum[uiNum.Count - 1]).gameObject.SetActive(false);
                    GameObject.Find("CharacterManager").GetComponent<CharacterManager>().IsUI(false);
                    uiNum.RemoveAt(uiNum.Count - 1);
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    setInventory = false;
                }
                else if (uiNum[uiNum.Count - 1].Equals("Raid Group"))
                {
                    if (GameObject.Find("Find Party Group") != null)
                    {
                        GameObject.Find("Raid Group").transform.Find("Find Party Group").transform.Find("ExitBtn").GetComponent<Button>().onClick.Invoke();
                        GameObject.Find("CharacterManager").GetComponent<CharacterManager>().IsUI(false);
                        Cursor.visible = false;
                        Cursor.lockState = CursorLockMode.Locked;
                    }
                    else if (GameObject.Find("Find Party Group") == null)
                    {
                        GameObject.Find("Raid Group").transform.Find("ExitBtn").GetComponent<Button>().onClick.Invoke();
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            if (Cursor.visible == false)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (Cursor.visible == true)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            GameObject pl = GameObject.Find("Player(Clone)");
            if (pl != null)
            {
                pl.GetComponent<CharacterController>().enabled = false;
                pl.transform.position = new Vector3(0f, 0f, 0f);
                pl.GetComponent<CharacterController>().enabled = true;
            }
        }

    }
}
