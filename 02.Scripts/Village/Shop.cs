using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public RectTransform uiGroup;
    public Animator anim;
    CharacterController enterPlayer;
    PhotonPlayerController playerController;
    PhotonCameraController cameraController;

    public void Enter(CharacterController player)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        // enterPlayer = player;

        // playerController = enterPlayer.GetComponent<PhotonPlayerController>();
        // cameraController = enterPlayer.GetComponentInChildren<PhotonCameraController>();
        // if (cameraController != null)
        // {
        //     Debug.Log(cameraController);
        //     cameraController.enabled = false;
        // }

        // UI 활성화
        // print(uiGroup.name);
        GameObject villageUI = GameObject.Find("VillageUI");
        villageUI.transform.Find(uiGroup.name).gameObject.SetActive(true);
        GameObject.Find("PlayerUI").transform.GetComponent<InventoryKey>().uiNum.Add(uiGroup.name);
        GameObject.Find("CharacterManager").transform.GetComponent<CharacterManager>().IsUI(true);
    }

    public void Exit()
    {
        // NPC 애니메이션 실행
        anim.SetTrigger("doBye");

        // UI 비활성화
        GameObject villageUI = GameObject.Find("VillageUI");
        villageUI.transform.Find(uiGroup.name).gameObject.SetActive(false);
        GameObject.Find("CharacterManager").transform.GetComponent<CharacterManager>().IsUI(false);
        // 커서 설정
        Cursor.visible = false;                     // 마우스 커서를 보이지 않게 설정
        Cursor.lockState = CursorLockMode.Locked;   // 마우스 커서 위치 고정

        // if (playerController != null)
        // {
        //     playerController.isInteract = false;
        // }

        // if (cameraController != null)
        // {
        //     cameraController.enabled = true;
        // }
        List<string> uiNumber = GameObject.Find("PlayerUI").transform.GetComponent<InventoryKey>().uiNum;
        for (int i = 0; i < uiNumber.Count; i++)
        {
            if (uiNumber[i].Equals(uiGroup.name))
            {
                uiNumber.RemoveAt(i);
            }
        }
    }
}
