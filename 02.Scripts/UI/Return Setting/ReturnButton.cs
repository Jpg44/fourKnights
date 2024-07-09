using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ReturnButton : MonoBehaviour
{
    public Button returnStartBtn;
    public Button returnCharacterBtn;
    public Button returnVillageBtn;

    private void Awake()
    {
        returnStartBtn.onClick.AddListener(() => NetworkManager.Instance.SetStart());
        returnCharacterBtn.onClick.AddListener(() => NetworkManager.Instance.SetCharacter());
        returnVillageBtn.onClick.AddListener(() => NetworkManager.Instance.SetViliage());
    }
}