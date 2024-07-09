using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource bgmSource; // BGM을 재생
    public AudioSource sfxSource; // 효과음을 재생

    public AudioClip startSceneBGM;
    public AudioClip villageSceneBGM;
    public AudioClip golemSceneBGM;
    public AudioClip dryadSceneBGM;
    public AudioClip field1SceneBGM1;
    public AudioClip field1SceneBGM2;
    
    public AudioClip footstepSound;
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip dashSound;
    public AudioClip potionSound;
    
    public AudioClip[][] skillSounds;

    // 직업별 스킬 사운드
    public AudioClip warriorAttack1; 
    public AudioClip warriorAttack2; 
    public AudioClip warriorSkill1; 
    public AudioClip warriorSkill2;
    public AudioClip warriorSkill3;
    public AudioClip warriorSkill4;


    public AudioClip magicianAttack1; 
    public AudioClip magicianAttack2; 
    public AudioClip magicianSkill1; 
    public AudioClip magicianSkill2;
    public AudioClip magicianSkill3;
    public AudioClip magicianSkill4;


    public AudioClip archerAttack1; 
    public AudioClip archerAttack2; 
    public AudioClip archerSkill1; 
    public AudioClip archerSkill2;
    public AudioClip archerSkill3;
    public AudioClip archerSkill4;


    public AudioClip fighterAttack1; 
    public AudioClip fighterAttack2; 
    public AudioClip fighterSkill1; 
    public AudioClip fighterSkill2;
    public AudioClip fighterSkill3;
    public AudioClip fighterSkill4;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject); // 중복 인스턴스 제거
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); // 싱글턴 객체 유지
        }

        ResetSkillSounds();
    }

    private void Start()
    {
        PlayBGM(startSceneBGM); // 초기 Start Scene BGM
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;

        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySkillSFX(int jobIndex, int skillIndex)
    {
        if (jobIndex >= 0 && jobIndex < skillSounds.Length)
        {
            if (skillIndex >= 0 && skillIndex < skillSounds[jobIndex].Length)
            {
                PlaySFX(skillSounds[jobIndex][skillIndex]);
            }
        }
    }

    void ResetSkillSounds()
    {
        skillSounds = new AudioClip[4][];

        // 각 직업군에 대한 스킬 사운드 배열 초기화
        skillSounds[0] = new AudioClip[6]; // 전사의 스킬 사운드
        skillSounds[0][0] = warriorAttack1;
        skillSounds[0][1] = warriorAttack2;
        skillSounds[0][2] = warriorSkill1;
        skillSounds[0][3] = warriorSkill2;
        skillSounds[0][4] = warriorSkill3;
        skillSounds[0][5] = warriorSkill4;

        skillSounds[1] = new AudioClip[6]; // 마법사의 스킬 사운드
        skillSounds[1][0] = magicianAttack1;
        skillSounds[1][1] = magicianAttack2;
        skillSounds[1][2] = magicianSkill1;
        skillSounds[1][3] = magicianSkill2;
        skillSounds[1][4] = magicianSkill3;
        skillSounds[1][5] = magicianSkill4;

        skillSounds[2] = new AudioClip[6]; // 궁수의 스킬 사운드
        skillSounds[2][0] = archerAttack1;
        skillSounds[2][1] = archerAttack2;
        skillSounds[2][2] = archerSkill1;
        skillSounds[2][3] = archerSkill2;
        skillSounds[2][4] = archerSkill3;
        skillSounds[2][5] = archerSkill4;

        skillSounds[3] = new AudioClip[6]; // 권사의 스킬 사운드
        skillSounds[3][0] = fighterAttack1;
        skillSounds[3][1] = fighterAttack2;
        skillSounds[3][2] = fighterSkill1;
        skillSounds[3][3] = fighterSkill2;
        skillSounds[3][4] = fighterSkill3;
        skillSounds[3][5] = fighterSkill4;
    }
}
