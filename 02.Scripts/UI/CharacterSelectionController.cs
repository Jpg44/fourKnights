using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectionController : MonoBehaviour
{
    public TextMeshProUGUI NicknameText;
    public TextMeshProUGUI LvText;
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI ATKText;
    public TextMeshProUGUI DEFText;
    public CharacterManager characterManager;

    void Start() 
    {
        GameObject characterManagerObject = GameObject.FindWithTag("CharacterManager");
        if (characterManagerObject != null) {
            characterManager = characterManagerObject.GetComponent<CharacterManager>();
        }

        // NicknameText.text = "닉네임 : " + characterManager.nickname;
        // LvText.text = "Item LV : " + "구현필요";
        // HPText.text = "HP : " + characterManager.currentHP + " / " + characterManager.maxHP;
        // ATKText.text = "공격력 : " + characterManager.ATK;
        // DEFText.text = "방어력 : " + characterManager.DEF;
    }

    void Update()
    {
        if (characterManager.characterInfoPk == -1)
        {
            NicknameText.text = "닉네임 : ";
            LvText.text = "Item LV : ";
            HPText.text = "HP : ";
            ATKText.text = "공격력 : ";
            DEFText.text = "방어력 : ";    
        }
        else
        {
            NicknameText.text = "닉네임 : " + characterManager.characterNickname;
            LvText.text = "Item LV : " + characterManager.level;
            HPText.text = "HP : " + characterManager.currentHP + " / " + characterManager.maxHP;
            ATKText.text = "공격력 : " + characterManager.ATK;
            DEFText.text = "방어력 : " + characterManager.DEF;    
        }

    }
}
