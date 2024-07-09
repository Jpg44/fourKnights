using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI 네임스페이스 추가

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }
    public GameObject settingsPanelPrefab;  // 환경설정 UI 프리팹
    private GameObject settingsPanelInstance;  // 환경설정 UI 인스턴스
    private Button exitButton;

    public float playTime;
    public TextMeshProUGUI nickNameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI playTimeText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 씬이 변경되어도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject);
        }

        settingsPanelInstance = Instantiate(settingsPanelPrefab);
        settingsPanelInstance.SetActive(false);
        DontDestroyOnLoad(settingsPanelInstance);

        // 씬이 로드될 때마다 호출될 메서드 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        playTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameObject.Find("PlayerUI") != null && GameObject.Find("PlayerUI").transform.GetComponent<InventoryKey>().uiNum.Count == 0)
            {
                ToggleSettings();
            }
        }
    }

    void LateUpdate()
    {
        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600) / 60);
        int sec = (int)(playTime % 60);

        if (playTimeText != null)
        {
            playTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", hour, min, sec);
        }
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Canvas canvas = null;
        if (GameObject.Find("Canvas") != null)
        {
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        }
        else if (canvas == null)
        {
            canvas = GameObject.Find("Canvas R").GetComponent<Canvas>();
        }
        if (canvas != null && settingsPanelPrefab != null)
        {
            if (settingsPanelInstance == null || settingsPanelInstance.Equals(null))
            {
                settingsPanelInstance = Instantiate(settingsPanelPrefab, canvas.transform, false);
                settingsPanelInstance.SetActive(false);
                DontDestroyOnLoad(settingsPanelInstance);

                playTimeText = settingsPanelInstance.GetComponentInChildren<TextMeshProUGUI>();
            }
            else
            {
                settingsPanelInstance.transform.SetParent(canvas.transform, false);
            }
        }
    }

    public void ToggleSettings()
    {
        if (settingsPanelInstance != null)
        {
            settingsPanelInstance.SetActive(!settingsPanelInstance.activeSelf);
            ManageCursorVisibility();
        }
    }

    private void ManageCursorVisibility()
    {
        if (SceneManager.GetActiveScene().name == "StartScene" ||
            SceneManager.GetActiveScene().name == "CharacterSelectScene")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = settingsPanelInstance.activeSelf;
            Cursor.lockState = settingsPanelInstance.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    public void SetVolume(float newVolume)
    {
        // 볼륨 설정 코드
    }

    public void SetResolution(int width, int height)
    {
        // 해상도 설정 코드
    }
}
