using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NickNameController : MonoBehaviour
{
    public TextMeshProUGUI nickNameText;
    public TextMeshProUGUI lvText;
    public TextMeshProUGUI ATKText;
    public TextMeshProUGUI DEFText;
    public CharacterManager characterManager;

    void Start()
    {
        GameObject characterManagerObject = GameObject.FindWithTag("CharacterManager");
        if (characterManagerObject != null)
        {
            characterManager = characterManagerObject.GetComponent<CharacterManager>();
            nickNameText.text = characterManager.characterNickname;
            lvText.text = "Lv. " + characterManager.level.ToString();
            ATKText.text = "ATK : " + characterManager.ATK.ToString();
            DEFText.text = "DEF : " + characterManager.DEF.ToString();
        }

    }

}
