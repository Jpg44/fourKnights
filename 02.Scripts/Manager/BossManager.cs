using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    // 난이도를 나타내는 Enum
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }
// 보스 선택 UI를 관리하는 컨트롤러
public class BossManager : MonoBehaviour
{
    // Singleton 패턴을 사용해 BossManager에 쉽게 접근
    public static BossManager Instance { get; private set; }
    

    [System.Serializable]
    public class BossData
    {
        public string name;
        public string description;
        public Sprite bossListImage;
        public Sprite mainImage;
        public List<DifficultyRewardStats> difficultyRewards; // 난이도별 보상 및 스탯
    }

    [System.Serializable]
    public class DifficultyRewardStats
    {
        public Difficulty difficulty;
        public BossStats stats; // 스탯
        public List<RewardData> rewards; // 보상 리스트
    }

    [System.Serializable]
    public class BossStats
    {
        public int maxHealth; // 보스 체력
        public int attack;    // 보스 공격력
        public int defense;   // 보스 방어력
    }

    [System.Serializable]
    public class RewardData
    {
        public string type; // 보상 종류 (골드, 포션, 장비 등)
        public Sprite image; // 보상 이미지
        public int minQuantity; // 최소 보상 수량
        public int maxQuantity; // 최대 보상 수량
        public float dropRate; // 드랍 확률 (0-100%)
    }



    public List<BossData> bosses = new List<BossData>(); // 보스 데이터 리스트

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
    }
}
