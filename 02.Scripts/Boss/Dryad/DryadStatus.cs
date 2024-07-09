using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.AI;

public class DryadStatus : MonoBehaviourPunCallbacks
{
    public static DryadStatus Instance { get; private set; } // 싱글톤 인스턴스

    public int maxHealth = 4000;
    public int currentHealth;
    public int attackPower = 100;
    public int defense = 30;
    public int currentMana = 0;
    private bool isArmorActive = false;

    private UIHooks ui;
    public GameObject alarm;

    public GameObject stageclear;
    public GameObject dryad;

    public Animator animator;
    private bool isDead = false;
    public CharacterManager characterManager;


    // Emission 관련 변수 추가
    public SkinnedMeshRenderer bossRenderer;
    private Material bossMaterial;
    private Color originalColor;
    public Color hitEmissionColor = new Color(118f / 255f, 20f / 255f, 20f / 255f); // R=118, G=20, B=20
    public float hitEffectDuration = 0.8f;
    public TextMeshProUGUI damageTextMesh;

    public BossAttackController bossAttackController;
    public NavMeshAgent navMeshAgent;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // 게임 내내 유지되도록 설정
        }
        else
        {
            Destroy(gameObject); // 중복 인스턴스 제거
        }

        GameObject characterManagerObject = GameObject.FindWithTag("CharacterManager");
        if (characterManagerObject != null) {
            characterManager = characterManagerObject.GetComponent<CharacterManager>();
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        ui = GetComponent<UIHooks>();

        // Emission 관련 초기화
        if (bossRenderer != null)
        {
            bossMaterial = bossRenderer.material;
            originalColor = bossMaterial.color;
            Debug.Log("Original Color: " + originalColor);
        }
        else
        {
            Debug.LogError("Boss Renderer not assigned.");
        }
    }

    void Update()
    {
        if (currentHealth <= 0 && !isDead)
        {
            Die();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        Debug.Log(currentMana);
    }

    void Die()
    {
        animator.SetTrigger("Die");
        isDead = true;
        // animator.Play("Die");
        StartCoroutine(DisableAfterSeconds(6));
    }

    IEnumerator DisableAfterSeconds(float seconds)
    {
        bossAttackController.enabled = false;
        navMeshAgent.speed = 0f;
        yield return new WaitForSeconds(seconds);
        stageclear.SetActive(true);
        dryad.SetActive(false);
        yield return new WaitForSeconds(seconds);
        
    }

    public void TakeDamage(int damage)
    {

        Debug.Log("아파");
        photonView.RPC("SendDamageToMaster", RpcTarget.MasterClient, damage);
        StartCoroutine(ShowHitEffect());
        StartCoroutine(UpdateDamageText(damage));
    }

    // 마스터 클라이언트가 피해량을 받아서 처리
    [PunRPC]
    public void SendDamageToMaster(int damage)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("마스터 클라이언트에서 피해 수신: " + damage);
            currentHealth -= damage;
            if (currentHealth < 0) currentHealth = 0;

            // 체력 및 UI 갱신
            UpdateHealthUI();

            // 다른 클라이언트들에게 현재 체력 동기화
            photonView.RPC("UpdateHealthRPC", RpcTarget.Others, currentHealth);

            if (currentHealth <= 0 && !isDead)
            {
                Die();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;   // 마우스 커서 위치 고정 해제
            }
        }
    }

    // 체력 및 UI 갱신 메서드
    private void UpdateHealthUI()
    {
        if (ui != null)
        {
            ui.SetHealth(currentHealth, maxHealth);
        }
        else
        {
            Debug.LogError("UIHooks is not initialized.");
        }
    }

    // 다른 클라이언트에서 호출될 RPC 메서드
    [PunRPC]
    public void UpdateHealthRPC(int newHealth)
    {
        currentHealth = newHealth;
        UpdateHealthUI();
    }

    private IEnumerator ShowHitEffect()
    {
        
        if (bossMaterial != null)
        {
            Debug.Log("드라이어드피격");
            bossMaterial.color = hitEmissionColor;
            yield return new WaitForSeconds(hitEffectDuration);
            bossMaterial.color = originalColor;
        }
        else
        {
            Debug.Log("dzzzzzzzzzzzzzzzd");
        }
    }

    private IEnumerator UpdateDamageText(int damage)
    {
        damageTextMesh.gameObject.SetActive(true);


        if (damageTextMesh != null)
        {
            damageTextMesh.text = damage.ToString();
        }
        else
        {
            Debug.LogError("TextMesh 컴포넌트를 찾을 수 없습니다.");
        }
        yield return new WaitForSeconds(0.5f);

        damageTextMesh.gameObject.SetActive(false);
    }

}

