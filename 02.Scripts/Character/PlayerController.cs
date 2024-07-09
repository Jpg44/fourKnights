using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 해닻 스크립트에서는 Input과 관련된 내용을 다룸
    // Input에 따라 애니메이션과 함수를 실행
    [SerializeField]
    private Transform cameraTransform;
    private PlayerMovement playerMovement;
    private PlayerAnimator playerAnimator;
    private CharacterController controller;

    public CharacterManager characterManager;

    public float hAxis { get; private set; }
    public float vAxis { get; private set; }
    public bool rDown { get; private set; }
    public bool m0Down { get; private set; }
    public bool m1Down { get; private set; }
    public bool jDown { get; private set; }
    public bool dDown { get; private set; }
    public bool eDown { get; private set; }
    public bool sDown1 { get; private set; }
    public bool sDown2 { get; private set; }
    public bool sDown3 { get; private set; }
    public bool sDown4 { get; private set; }
    public bool qDown { get; private set; }
    public bool escDown { get; private set; }



    public bool isInteract;             // UI 창이 열려있는지 확인
    public bool isSettingsOpen = false;  // 환경설정 창이 열려있는지 확인

    private bool isAttackReady = true;
    private bool isDashReady = true;
    private bool isSkill1Ready = true;
    private bool isSkill2Ready = true;
    private bool isSkill3Ready = true;
    private bool isSkill4Ready = true;
    private bool isPotionReady = true;

    private float dashCooldown = 3f;
    private float skill1Cooldown = 3f;
    private float skill2Cooldown = 5f;
    private float skill3Cooldown = 15f;
    private float skill4Cooldown = 10f;
    private float potionCooldown = 10f;

    public string attackStyle;

    private void Awake()
    {
        // 초기 Scene과 상점 UI등에서는 마우스를 켜야하기 때문에 커서 비활성 조건 필요
        // Cursor.visible = false;                     // 마우스 커서를 보이지 않게 설정
        // Cursor.lockState = CursorLockMode.Locked;   // 마우스 커서 위치 고정

        playerMovement = GetComponent<PlayerMovement>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        controller = GetComponent<CharacterController>();

        characterManager = FindObjectOfType<CharacterManager>();

    }

    private void Update()
    {
        GetInput();

        // 애니메이션 파라미터 설정 (horizontal, vertical)
        playerAnimator.OnMovement(hAxis, vAxis);
        // 이동속도 앞으로 이동할때만 5
        // playerMovement.moveSpeed = vAxis > 0 ? 10.0f : 8.0f;

        // 이동 방향 저장 (카메라 방향 기준으로 이동)
        playerMovement.MoveTo(cameraTransform.rotation * new Vector3(hAxis, 0, vAxis));

        // 캐릭터 회전 설정(카메라 기준으로 앞만 보게)
        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);

        if (jDown && isPotionReady && isAttackReady && controller.isGrounded)
        {
            playerAnimator.Jump();
            playerMovement.Jump();
        }

        if (dDown && controller.isGrounded && !playerMovement.isDash && isDashReady && isAttackReady && vAxis > 0)
        {
            playerAnimator.Dash();
            playerMovement.Dash();
            StartCoroutine(SetSkillCooldown(0, dashCooldown));
        }

        if (eDown)
        {
            playerMovement.Interaction();
        }

        // 마우스 왼쪽 공격
        if (m0Down && isAttackReady && isPotionReady && !isInteract)
        {
            attackStyle = "m0Down";
            playerAnimator.Attack1();
            isAttackReady = false;
            StartCoroutine(EnableAttack());
        }
        // 마우스 오른쪽 공격
        if (m1Down && isAttackReady && isPotionReady && !isInteract)
        {
            attackStyle = "m1Down";
            playerAnimator.Attack2();
            isAttackReady = false;
            StartCoroutine(EnableAttack());
        }

        if (sDown1 && isAttackReady && isPotionReady && isSkill1Ready)
        {
            attackStyle = "sDown1";
            playerAnimator.Skill1();
            isAttackReady = false;
            StartCoroutine(SetSkillCooldown(1, skill1Cooldown));
            StartCoroutine(EnableAttack());
        }
        if (sDown2 && isAttackReady && isPotionReady && isSkill2Ready)
        {
            attackStyle = "sDown2";
            playerAnimator.Skill2();
            isAttackReady = false;
            StartCoroutine(SetSkillCooldown(2, skill2Cooldown));
            StartCoroutine(EnableAttack());
        }
        if (sDown3 && isAttackReady && isPotionReady && isSkill3Ready)
        {
            attackStyle = "sDown3";
            playerAnimator.Skill3();
            isAttackReady = false;
            StartCoroutine(SetSkillCooldown(3, skill3Cooldown));
            StartCoroutine(EnableAttack());
        }
        if (sDown4 && isAttackReady && isPotionReady && isSkill4Ready)
        {
            attackStyle = "sDown4";
            playerAnimator.Skill4();
            isAttackReady = false;
            StartCoroutine(SetSkillCooldown(4, skill4Cooldown));
            StartCoroutine(EnableAttack());
        }

        if (qDown && isPotionReady)
        {
            // 포션 로직
            characterManager.UsePotion();
            StartCoroutine(SetPotionCooldown(potionCooldown));
        }
    }

    void GetInput()
    {
        // 움직임 입력 구현
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        rDown = Input.GetButton("Run");
        jDown = Input.GetButtonDown("Jump");
        dDown = Input.GetButtonDown("Dash");
        m0Down = Input.GetButtonDown("Attack1");
        m1Down = Input.GetButtonDown("Attack2");
        eDown = Input.GetButtonDown("Interaction");
        sDown1 = Input.GetButtonDown("Skill1");
        sDown2 = Input.GetButtonDown("Skill2");
        sDown3 = Input.GetButtonDown("Skill3");
        sDown4 = Input.GetButtonDown("Skill4");
        qDown = Input.GetButtonDown("Potion");
    }
    IEnumerator EnableAttack()
    {
        // 애니메이션 길이 만큼 대기
        yield return new WaitForSeconds(0.3f); // 예를 들어 애니메이션 길이가 0.5초라고 가정
        isAttackReady = true;
    }
    IEnumerator SetSkillCooldown(int skillNumber, float cooldown)
    {
        switch (skillNumber)
        {
            case 0:
                isDashReady = false;
                break;
            case 1:
                isSkill1Ready = false;
                break;
            case 2:
                isSkill2Ready = false;
                break;
            case 3:
                isSkill3Ready = false;
                break;
            case 4:
                isSkill4Ready = false;
                break;
        }

        yield return new WaitForSeconds(cooldown);

        switch (skillNumber)
        {
            case 0:
                isDashReady = true;
                break;
            case 1:
                isSkill1Ready = true;
                break;
            case 2:
                isSkill2Ready = true;
                break;
            case 3:
                isSkill3Ready = true;
                break;
            case 4:
                isSkill4Ready = true;
                break;
        }
    }

    IEnumerator SetPotionCooldown(float cooldown)
    {
        isPotionReady = false;
        yield return new WaitForSeconds(cooldown);
        isPotionReady = true;
    }

}
