using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossSelectionController : MonoBehaviour
{
    public Image mainImage; // 메인 이미지를 표시할 Image 컴포넌트
    public TextMeshProUGUI descriptionText; // 설명을 표시할 Text 컴포넌트
    public GameObject bossListButton; // 보스 버튼 프리팹
    public Transform bossList; // 보스 리스트를 담을 부모 컨테이너
    public Button matchingBtn; // 보스 매칭 버튼
    public Button bossRoomListBtn; // 보스 파티 찾기버튼

    public RectTransform bossRoomPanel; // 보스 방 리스트를 담을 UI 패널

    public Image findPartyImage; // Find Party UI 내의 보스 이미지
    public TextMeshProUGUI findPartyDescription; // Find Party UI 내의 보스 설명
    public GameObject roomListButtonPrefab; // 방 리스트 버튼 프리팹
    public Transform roomList; // 방 리스트를 담을 부모 컨테이너
    private int selectedBossIndex = 0; // 현재 선택된 보스의 인덱스

    public List<Image> rewardImages; // 보상 이미지를 표시할 Image 컴포넌트 리스트

    public Button easyButton;  // Easy 난이도 버튼
    public Button normalButton; // Normal 난이도 버튼
    public Button hardButton;   // Hard 난이도 버튼
    void Start()
    {
        SetupBossList();
        easyButton.onClick.AddListener(() => UpdateRewardImage(Difficulty.Easy));
        normalButton.onClick.AddListener(() => UpdateRewardImage(Difficulty.Normal));
        hardButton.onClick.AddListener(() => UpdateRewardImage(Difficulty.Hard));
    }

    void SetupBossList()
    {
        if (BossManager.Instance != null)
        {

            for (int i = 0; i < BossManager.Instance.bosses.Count; i++)
            {
                GameObject buttonObj = Instantiate(bossListButton, bossList);
                buttonObj.name = BossManager.Instance.bosses[i].name;
                buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = BossManager.Instance.bosses[i].name;
                buttonObj.GetComponentInChildren<Image>().sprite = BossManager.Instance.bosses[i].bossListImage;
                int index = i;
                buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectBoss(index));
            }
        }
    }

    public void SelectBoss(int index)
    {
        RoomManager.Instance.selectedBoss(index);
        selectedBossIndex = index;

        if (index < 0 || index >= BossManager.Instance.bosses.Count) return;

        var selectedBoss = BossManager.Instance.bosses[index];
        mainImage.sprite = selectedBoss.mainImage;
        descriptionText.text = selectedBoss.description;

        // Find Party UI 정보 업데이트
        findPartyImage.sprite = selectedBoss.mainImage; // 보스 이미지 갱신
        findPartyDescription.text = selectedBoss.description; // 보스 설명 갱신
    }

    public void ChangeToBossScene()
    {
        NetworkManager.Instance.SetIsScene(BossManager.Instance.bosses[selectedBossIndex].name);
        NetworkManager.Instance.LeaveRoom();
        //LoadingSceneController.LoadScene(BossManager.Instance.bosses[selectedBossIndex].name);
        Cursor.visible = false;                     // 마우스 커서를 보이지 않게 설정
        Cursor.lockState = CursorLockMode.Locked;   // 마우스 커서 위치 고정
    }

    void UpdateRewardImage(Difficulty difficulty)
    {
        if (selectedBossIndex < 0 || selectedBossIndex >= BossManager.Instance.bosses.Count) return;

        var selectedBoss = BossManager.Instance.bosses[selectedBossIndex];
        var difficultyRewards = selectedBoss.difficultyRewards.Find(d => d.difficulty == difficulty);
        if (difficultyRewards != null)
        {
            for (int i = 0; i < rewardImages.Count; i++)
            {
                if (i < difficultyRewards.rewards.Count)
                    rewardImages[i].sprite = difficultyRewards.rewards[i].image;
                else
                    rewardImages[i].enabled = false; // 이미지가 없는 경우 이미지 컴포넌트를 비활성화
            }
        }
    }
}
