using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarController : MonoBehaviour
{
    public Image healthBarImage;
    public TextMeshProUGUI healthBarText;
    public CharacterManager characterManager;

    private DieModal dieModal;

    void Start()
    {
        GameObject characterManagerObject = GameObject.FindWithTag("CharacterManager");
        if (characterManagerObject != null)
        {
            characterManager = characterManagerObject.GetComponent<CharacterManager>();
        }

        dieModal = GetComponent<DieModal>();
    }


    void Update()
    {
        if (characterManager.shield > 0)
        {
            SetShield();
        }
        else
        {
            SetHealth();
        }
    }

    public void SetHealth()
    {
        healthBarImage.color = Color.red;
        healthBarImage.fillAmount = (float)characterManager.currentHP / characterManager.maxHP;
        healthBarText.text = characterManager.currentHP + " / " + characterManager.maxHP;

        if (dieModal != null && characterManager.currentHP <= 0)
        {
            dieModal.ShowModal();
        }
    }

    public void SetShield()
    {
        healthBarImage.color = new Color(0.9f, 0.8f, 0f, 1f);
        healthBarImage.fillAmount = (float)characterManager.shield / (int)(characterManager.maxHP * 0.1);
        healthBarText.color = Color.black;
        healthBarText.text = characterManager.shield + " / " + (int)(characterManager.maxHP * 0.1);
    }
}
