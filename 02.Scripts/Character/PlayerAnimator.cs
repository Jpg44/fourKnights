using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    // 해당 코드에서는 Player의 애니메이션 관련된 함수만 저장

    [SerializeField]
    public GameObject attackCollision;  // 애니메이션의 특정 프레임에 공격 범위 활성화 시켜주기 위한 변수
    private Animator anim;
    

    public ParticleSystem effectAttack2;
    public ParticleSystem trailSkill1;
    public GameObject effectSkill2;
    public ParticleSystem trailSkill2;
    public ParticleSystem effectSkill3;
    public ParticleSystem trailSkill3;
    public ParticleSystem effectSkill4;

    private void Awake() 
    {
        anim = GetComponent<Animator>();
    }
    public void OnMovement(float horizontal, float vertical)
    {
        anim.SetFloat("horizontal", horizontal);
        anim.SetFloat("vertical", vertical);
    
        // if (horizontal != 0 || vertical != 0)
        // {
        //     WalkingStep();
        // }
    }

    public void Jump()
    {
        anim.SetTrigger("doJump");
        SoundManager.Instance.PlaySFX(SoundManager.Instance.jumpSound);
    }
    public void Dash()
    {
        anim.SetTrigger("doDash");
        SoundManager.Instance.PlaySFX(SoundManager.Instance.jumpSound);
    }

    public void Attack1()
    {
        anim.SetTrigger("Attack1");
    }
    public void Attack2()
    {
        anim.SetTrigger("Attack2");
        effectAttack2.Play();
    }
    public void Skill1()
    {
        anim.SetTrigger("Skill1");
        trailSkill1.Play();
    }
    public void Skill2()
    {
        anim.SetTrigger("Skill2");
        trailSkill2.Play();
    }

    public void Skill3()
    {
        anim.SetTrigger("Skill3");
        trailSkill3.Play();
        effectSkill3.Play();
        StartCoroutine(StopEffectAfterDelay(effectSkill3, 10f)); // 10초 후 방어막 이펙트 종료
    }
    
    IEnumerator StopEffectAfterDelay(ParticleSystem effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        effect.Stop(); // 지정된 시간 후 이펙트 정지
    }

    public void Skill4()
    {
        anim.SetTrigger("Skill4");
    }

    // 공격 판정이 들어갈 것 같은 애니메이션의 프레임에서 공격범위 활성화
    public void OnAttackCollision()
    {
        attackCollision.SetActive(true);
    }

    // 발걸음 소리 활성화
    // public void WalkingStep()
    // {
    //     audioSource.PlayOneShot(walkingStep);  // audioSource를 사용하여 발걸음 소리 재생
    // }
    // public void RunningStep()
    // {
    //     audioSource.PlayOneShot(runningStep);  // audioSource를 사용하여 러닝 소리 재생
    // }
}
