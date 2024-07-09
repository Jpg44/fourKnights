using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFootstepSound()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.footstepSound);
    }

    public void PlayDashSound()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.dashSound);
    }
    public void PlayJumpSound()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.jumpSound);
    }
    public void PlayPotionSound()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.potionSound);
    }
    public void PlayLandSound()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.landSound);
    }

    public void PlaySkillSound(int skillIndex)
    {
        // int jobIndex = CharacterManager.Instance.characterInfoPk; // 캐릭터의 직업 인덱스 가져오기
        int jobIndex = 0;
        SoundManager.Instance.PlaySkillSFX(jobIndex, skillIndex);
    }
}
