using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToCharacter : MonoBehaviour
{
    public CharacterManager characterManager;

    public GameObject WarriorKc01;
    public GameObject ArcherRi01;
    public GameObject Stella01;
    public GameObject WarriorKun01;
    public GameObject NewCharacter;
    public GameObject InfoModal;

    // Start is called before the first frame update
    // void Start()
    // {
    //     GameObject characterManagerObject = GameObject.FindWithTag("CharacterManager");
    //     if (characterManagerObject != null) {
    //         characterManager = characterManagerObject.GetComponent<CharacterManager>();
    //     }
    // }

    // 캐릭터 선택창에서의 캐릭터를 변경하고, 캐릭터가 없다면 생성창을 띄워줌
    public void Change()
    {
        WarriorKc01.SetActive(false);
        ArcherRi01.SetActive(false);
        Stella01.SetActive(false);
        WarriorKun01.SetActive(false);
        NewCharacter.SetActive(false);
        InfoModal.SetActive(false);

        switch (characterManager.characterClass)
        {
            case "전사": WarriorKc01.SetActive(true); InfoModal.SetActive(true); break;
            case "궁수": ArcherRi01.SetActive(true); InfoModal.SetActive(true); break;
            case "마법사": Stella01.SetActive(true); InfoModal.SetActive(true); break;
            case "권사": WarriorKun01.SetActive(true); InfoModal.SetActive(true); break;
            default: NewCharacter.SetActive(true); break;
        }
    }

    public void Create()
    {
        NewCharacter.SetActive(false);
    }

    // 캐릭터 생성창에서의 오른쪽에 나오는 캐릭터를 변경
    public void CreateChar(string jobName)
    {
        WarriorKc01.SetActive(false);
        Stella01.SetActive(false);

        switch (jobName)
        {
            case "전사": WarriorKc01.SetActive(true); break;
            case "마법사": Stella01.SetActive(true); break;
            default: break;
        }
    }
}
