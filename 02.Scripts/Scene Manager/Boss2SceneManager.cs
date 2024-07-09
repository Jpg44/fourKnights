using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class Boss2SceneManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        Debug.Log("Boss2SceneManager : Dryad");

        NetworkManager.Instance.SetIsScene("Dryad");
        Cursor.visible = false;                     // 마우스 커서를 보이게
        Cursor.lockState = CursorLockMode.Locked;   // 마우스 커서 위치 고정 해제

        NetworkManager.Instance.roomRequest = false;
        NetworkManager.Instance.Spawn();
        //NetworkManager.Instance.SpawnBoss("Dryad");
    }
}