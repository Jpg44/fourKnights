using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCoolTime : MonoBehaviour
{
    public Image dashCooldownImg;
    public Image skill1CooldownImg;
    public Image skill2CooldownImg;
    public Image skill3CooldownImg;
    public Image skill4CooldownImg;
    public Image potionCooldownImg;

    public TextMeshProUGUI dashCooldownText;
    public TextMeshProUGUI skill1CooldownText;
    public TextMeshProUGUI skill2CooldownText;
    public TextMeshProUGUI skill3CooldownText;
    public TextMeshProUGUI skill4CooldownText;
    public TextMeshProUGUI potionCooldownText;

    public CharacterManager characterManager;

    private float[] cooldownTimes = { 3f, 3f, 5f, 15f, 10f, 3f };
    private bool[] isCooldowns = new bool[6];

    private void Start()
    {
        // 초기 상태 설정
        SetCooldownState(dashCooldownImg, dashCooldownText, cooldownTimes[0], false);
        SetCooldownState(skill1CooldownImg, skill1CooldownText, cooldownTimes[1], false);
        SetCooldownState(skill2CooldownImg, skill2CooldownText, cooldownTimes[2], false);
        SetCooldownState(skill3CooldownImg, skill3CooldownText, cooldownTimes[3], false);
        SetCooldownState(skill4CooldownImg, skill4CooldownText, cooldownTimes[4], false);
        SetCooldownState(potionCooldownImg, potionCooldownText, cooldownTimes[5], false);

        characterManager = FindObjectOfType<CharacterManager>();
    }


    public void StartCooldown(int skillIndex, float cooldownTime)
    {
        cooldownTime -= cooldownTime * characterManager.reduceCoolTime / 100;
        switch (skillIndex)
        {
            case 0:
                StartCoroutine(StartCooldownCoroutine(dashCooldownImg, dashCooldownText, 0, cooldownTime));
                break;
            case 1:
                StartCoroutine(StartCooldownCoroutine(skill1CooldownImg, skill1CooldownText, 1, cooldownTime));
                break;
            case 2:
                StartCoroutine(StartCooldownCoroutine(skill2CooldownImg, skill2CooldownText, 2, cooldownTime));
                break;
            case 3:
                StartCoroutine(StartCooldownCoroutine(skill3CooldownImg, skill3CooldownText, 3, cooldownTime));
                break;
            case 4:
                StartCoroutine(StartCooldownCoroutine(skill4CooldownImg, skill4CooldownText, 4, cooldownTime));
                break;
            case 5:
                StartCoroutine(StartCooldownCoroutine(potionCooldownImg, potionCooldownText, 5, cooldownTime));
                break;
        }
    }

    private IEnumerator StartCooldownCoroutine(Image cooldownImage, TextMeshProUGUI cooldownText, int skillIndex, float cooldownTime)
    {
        isCooldowns[skillIndex] = true;
        SetCooldownState(cooldownImage, cooldownText, cooldownTime, true);
        float remainingCooldown = cooldownTime;

        while (remainingCooldown > 0)
        {
            cooldownImage.fillAmount = remainingCooldown / cooldownTime;
            cooldownText.text = $"{remainingCooldown:F1}";
            remainingCooldown -= Time.deltaTime;
            yield return null;
        }

        SetCooldownState(cooldownImage, cooldownText, cooldownTime, false);
        isCooldowns[skillIndex] = false;
    }

    private void SetCooldownState(Image cooldownImage, TextMeshProUGUI cooldownText, float cooldownTime, bool isActive)
    {
        cooldownImage.gameObject.SetActive(isActive);
        cooldownText.gameObject.SetActive(isActive);
        if (!isActive)
        {
            cooldownImage.fillAmount = 0;
            cooldownText.text = $"{cooldownTime:F1}";
        }
    }
}
