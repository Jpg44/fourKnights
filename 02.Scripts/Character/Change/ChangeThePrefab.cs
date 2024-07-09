using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ChangeThePrefab : MonoBehaviour
{
    public CharacterManager characterManager;
    public GameObject Warrior01;
    public GameObject Warrior02;
    public GameObject Warrior03;
    public GameObject Archer01;
    public GameObject Archer02;
    public GameObject Archer03;
    public GameObject Wizard01;
    public GameObject Wizard02;
    public GameObject Wizard03;
    public GameObject Kun01;
    public GameObject Kun02;
    public GameObject Kun03;

    private PhotonView photonView;

    public GameObject selectedCharacter;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        characterManager = FindObjectOfType<CharacterManager>();

        SelecedChar(characterManager.characterClass);
    }

    // void Update()
    // {
    //     UpdateChar();
    // }

    // public void SelecedChar(string characterClass)
    // {
    //     Warrior01.SetActive(false);
    //     Warrior02.SetActive(false);
    //     Warrior03.SetActive(false);
    //     Archer01.SetActive(false);
    //     Archer02.SetActive(false);
    //     Archer03.SetActive(false);
    //     Wizard01.SetActive(false);
    //     Wizard02.SetActive(false);
    //     Wizard03.SetActive(false);
    //     Kun01.SetActive(false);
    //     Kun02.SetActive(false);
    //     Kun03.SetActive(false);

    //     switch (characterClass)
    //     {
    //         case "전사": Warrior01.SetActive(true); break;
    //         case "궁수": Archer01.SetActive(true); break;
    //         case "마법사": Wizard01.SetActive(true); break;
    //         case "권사": Kun01.SetActive(true); break;
    //         default: break;
    //     }
    // }

    public void SelecedChar(string characterClass)
    {
        // 모든 캐릭터를 비활성화
        GameObject[] characters = { Warrior01, Warrior02, Warrior03, Archer01, Archer02, Archer03, Wizard01, Wizard02, Wizard03, Kun01, Kun02, Kun03 };
        foreach (var character in characters)
        {
            character.SetActive(false);
        }

        selectedCharacter = null;

        switch (characterClass)
        {
            case "전사": selectedCharacter = Warrior01; break;
            case "궁수": selectedCharacter = Archer01; break;
            case "마법사": selectedCharacter = Wizard01; break;
            case "권사": selectedCharacter = Kun01; break;
            default: break;
        }

        selectedCharacter.SetActive(true);
    }

    // public void UpdateChar()
    // {
    //     if (selectedCharacter != null)
    //     {
    //         selectedCharacter.SetActive(true);

    //         Animator animator = selectedCharacter.GetComponentInChildren<Animator>();
    //         PhotonAnimatorView animatorView = selectedCharacter.GetComponent<PhotonAnimatorView>();
    //         if (animator != null && animatorView != null)
    //         {
    //             animatorView.SetLayerSynchronized(0, PhotonAnimatorView.SynchronizeType.Discrete);
    //             photonView.ObservedComponents.Clear();
    //             photonView.ObservedComponents.Add(animatorView);
    //         }
    //         else
    //         {
    //             Debug.LogWarning("Animator or PhotonAnimatorView component is missing in " + selectedCharacter.name);
    //         }
    //     }
    // }
}
