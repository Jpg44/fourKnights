using System.Collections;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.AI;


public class BossStatus : MonoBehaviourPunCallbacks
{
    public static BossStatus Instance { get; private set; }

    public int maxHealth = 1000;
    public int currentHealth;
    public int attackPower = 50;
    public int defense = 50;
    public int currentMana = 0;

    private UIHooks ui;
    public GameObject alarm;
    public GameObject stageclear;
    public GameObject golem; // PhotonView가 있는 오브젝트
    public Animator animator;
    private bool isDead = false;
    private bool isFogChanging = false;

    private PhotonView photonView;

    // Emission 관련 변수 추가
    public SkinnedMeshRenderer bossRenderer;
    private Material bossMaterial;
    private Color originalColor;
    public Color hitEmissionColor = new Color(118f / 255f, 20f / 255f, 20f / 255f); // R=118, G=20, B=20
    public float hitEffectDuration = 5f;

    public TextMeshProUGUI damageTextMesh;

    public BossMonster bossMonster;
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

        // 자식 오브젝트의 PhotonView 가져오기
        photonView = GetComponent<PhotonView>();
        if (photonView == null)
        {
            Debug.LogError("자식 오브젝트에서 PhotonView를 찾을 수 없습니다.");
        }
    }

    void Start()
    {
        currentHealth = maxHealth;

        // UIHooks 초기화
        ui = FindObjectOfType<UIHooks>();
        if (ui == null)
        {
            Debug.LogError("UIHooks not found in the scene.");
        }

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
                Cursor.lockState = CursorLockMode.None;   // 마우스 커서 위치 고정 해제
            }

        if (currentHealth <= 300 && !isFogChanging)
        {
            StartCoroutine(ChangeFogEndDistance(40, 5.0f));
            isFogChanging = true;
        }
    }

    void Die()
    {
        animator.SetTrigger("Die");
        isDead = true;
        StartCoroutine(DisableAfterSeconds(6));
    }

    IEnumerator DisableAfterSeconds(float seconds)
    {
        bossMonster.enabled = false;
        navMeshAgent.speed = 0f;
        yield return new WaitForSeconds(seconds);
        stageclear.SetActive(true);
        golem.SetActive(false);
        yield return new WaitForSeconds(seconds);
    }

    // 로컬에서 보스 피격 판정
    public void TakeDamage(int damage)
    {
        photonView.RPC("SendDamageToMaster", RpcTarget.MasterClient, damage);

        // Emission 효과 추가
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

    IEnumerator ChangeFogEndDistance(float targetEndDistance, float duration)
    {
        float startEndDistance = RenderSettings.fogEndDistance;
        float time = 0;

        alarm.SetActive(true);

        while (time < duration)
        {
            time += Time.deltaTime;
            float blend = Mathf.Clamp01(time / duration);
            RenderSettings.fogEndDistance = Mathf.Lerp(startEndDistance, targetEndDistance, blend);
            yield return null;
        }

        alarm.SetActive(false);
        RenderSettings.fogEndDistance = targetEndDistance; // 마지막 값으로 확실히 설정
    }

    // Emission 효과를 보여주는 코루틴
    private IEnumerator ShowHitEffect()
    {
        if (bossMaterial != null)
        {
            bossMaterial.color = hitEmissionColor;
            yield return new WaitForSeconds(hitEffectDuration);
            bossMaterial.color = originalColor;
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
