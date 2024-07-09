using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// 캐릭터 정보 클래스
public class CharacterInfo
{
    public string characterNickname;
    public int characterInfoPk;
    public int jobPk;
    public int level;
    public int maxHP;
    public int currentHP;
    public int ATK;
    public int DEF;
    public int speed;
    public int reduceCoolTime;
    public int avoidanceRate;
};


// 유저에 대한 정보를 전체적으로 가지고 있는 매니저
// 유저ID, 닉네임, 캐릭터들의 정보
public class UserManager : MonoBehaviour
{
    public static UserManager instance = null;
    private WpfDataReceiver wpfDataReceiver;
    private BackEndDataReceiver backEndDataReceiver;

    public int pick; // 이 값으로 선택 유저 고름

    // 유저 정보
    public string userID;
    private string nickname;

    // 각 직업의 캐릭터 정보
    public CharacterInfo[] characterSlots = new CharacterInfo[4];

    public int charCount = 0; // 현재 캐릭터 수

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        // 씬 전환 시 파괴되지 않도록 설정
        DontDestroyOnLoad(gameObject);

        wpfDataReceiver = GetComponent<WpfDataReceiver>();
        backEndDataReceiver = GetComponent<BackEndDataReceiver>();
    }

    private void Start()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        if (args.Length > 1)
        {
            userID = args[1]; // 첫 번째 인수 (인덱스 1)를 가져옵니다.
            Debug.Log("Received userId: " + userID);
        }

        //userID = "jhtest1";
        userID = "testcase1";

        if (userID == "-projectpath")
        {
            //////////////////// 현재 WPF에 영향받지 않도록 더미 데이터로 넣어둠 /////////////////////
            /////////////////// 추후 이 캐릭터슬롯에 백엔드 데이터가 매핑되도록 처리 ///////////////
            //userID = "userId2";
            nickname = "테스트계정";
            characterSlots[0] = new CharacterInfo
            {
                characterNickname = userID,
                characterInfoPk = 1,
                jobPk = 1,
                level = 6,
                maxHP = 120,
                currentHP = 120,
                ATK = 9,
                DEF = 5,
                speed = 7,
                reduceCoolTime = 5,
                avoidanceRate = 10
            };

            for (int i = 1; i <= 3; i++)
            {
                characterSlots[i] = new CharacterInfo
                {
                    characterNickname = i + "번째미완성",
                    characterInfoPk = 1,
                    jobPk = i + 1,
                    level = 6,
                    maxHP = 120 * 5,
                    currentHP = 120 * 5,
                    ATK = 9,
                    DEF = 5,
                    speed = 7,
                    reduceCoolTime = 5,
                    avoidanceRate = 10
                };
            }
        }
        else
        {
            GetCharacter();
        }
    }

    public void createCharacter(int i)
    {
        // characterSlots[i] = new CharacterInfo
        // {
        //     characterPk = wpfDataReceiver.getCharacterInfoPk(i),
        //     characterClass = wpfDataReceiver.getJobPk(i) 
        //     switch
        //     {
        //         1 => "전사",
        //         2 => "궁수",
        //         3 => "마법사",
        //         4 => "권사",
        //         _ => "만들지 않은 직업"
        //     },
        //     maxHP = wpfDataReceiver.getMaxHP(i) * 5,
        //     currentHP = wpfDataReceiver.getCurrentHP(i) * 5,
        //     ATK = wpfDataReceiver.getATK(i),
        //     DEF = wpfDataReceiver.getDEF(i),
        //     speed = wpfDataReceiver.getSpeed(i),
        //     reduceCoolTime = wpfDataReceiver.getReduceCoolTime(i),
        //     avoidanceRate = wpfDataReceiver.getAvoidanceRate(i)
        // };
    }

    // CharacterManager 에 userID 를 전달하기 위한 함수
    public string GetUserID()
    {
        if (userID == null)
        {
            return "null";
        }
        return userID;
    }

    public string GetNickname()
    {
        if (userID == null)
        {
            return "null";
        }
        return nickname;
    }

    public string GetCharacterNickname()
    {
        if (characterSlots[pick] == null)
        {
            return "null";
        }
        return characterSlots[pick].characterNickname;
    }

    public int GetCharacterInfoPk()
    {
        if (characterSlots[pick] == null)
        {
            return -1;
        }
        return characterSlots[pick].characterInfoPk;
    }

    public string GetCharacterClass()
    {
        if (characterSlots[pick] == null)
        {
            return "만들지 않은 직업";
        }
        switch (characterSlots[pick].jobPk)
        {
            case 1: return "전사";
            case 2: return "궁수";
            case 3: return "마법사";
            case 4: return "권사";
            default: return "만들지 않은 직업";
        }
    }

    public int GetLevel()
    {
        if (characterSlots[pick] == null)
        {
            return -1;
        }
        return characterSlots[pick].level;
    }

    public int GetMaxHP()
    {
        if (characterSlots[pick] == null)
        {
            return 100;
        }
        return characterSlots[pick].maxHP;
    }

    public int GetCurrentHP()
    {
        if (characterSlots[pick] == null)
        {
            return 100;
        }
        return characterSlots[pick].currentHP;
    }

    public int GetATK()
    {
        if (characterSlots[pick] == null)
        {
            return 9;
        }
        return characterSlots[pick].ATK;
    }

    public int GetDEF()
    {
        if (characterSlots[pick] == null)
        {
            return 5;
        }
        return characterSlots[pick].DEF;
    }

    public int Getspeed()
    {
        if (characterSlots[pick] == null)
        {
            return 10;
        }
        return characterSlots[pick].speed;
    }

    public int GetReduceCoolTime()
    {
        if (characterSlots[pick] == null)
        {
            return 10;
        }
        return characterSlots[pick].reduceCoolTime;
    }

    public int GetAvoidanceRate()
    {
        if (characterSlots[pick] == null)
        {
            return 10;
        }
        return characterSlots[pick].avoidanceRate;
    }

    // 캐릭터 정보 받아오기
    public void GetCharacter()
    {
        StartCoroutine(GetChar());
    }

    public IEnumerator GetChar()
    {
        string url = $"http://k10b208.p.ssafy.io:8081/user/characters?userId={userID}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                // 성공적으로 데이터를 받았을 때 처리
                Debug.Log(request.downloadHandler.text);

                CharacterData characterData = JsonUtility.FromJson<CharacterData>(request.downloadHandler.text);

                nickname = characterData.nickname;

                for (int i = 0; i < characterData.charactersInfo.Length; i++)
                {
                    characterSlots[i] = new CharacterInfo
                    {
                        characterNickname = characterData.charactersInfo[i].characterNickname,
                        characterInfoPk = characterData.charactersInfo[i].characterInfoPk,
                        jobPk = characterData.charactersInfo[i].jobPk,
                        level = characterData.charactersInfo[i].level,
                        maxHP = characterData.charactersInfo[i].maxHP,
                        currentHP = characterData.charactersInfo[i].currentHP,
                        ATK = characterData.charactersInfo[i].atk,
                        DEF = characterData.charactersInfo[i].def,
                        speed = characterData.charactersInfo[i].speed,
                        reduceCoolTime = characterData.charactersInfo[i].reduceCoolTime,
                        avoidanceRate = characterData.charactersInfo[i].avoidanceRate
                    };
                }

                charCount = characterData.charactersInfo.Length;
            }
        }
    }

    [System.Serializable]
    public class CharacterData
    {
        public string message;
        public int statusCode;
        public string nickname;
        public CharacterInfoConvert[] charactersInfo;
    }

    [System.Serializable]
    public class CharacterInfoConvert
    {
        public int characterInfoPk;
        public string characterNickname;
        public int level;
        public int jobPk;
        public int maxHP;
        public int currentHP;
        public int speed;
        public int reduceCoolTime;
        public int avoidanceRate;
        public int atk;
        public int def;
    };

}

// 작성 참고
//   characterSlot1 = new CharacterInfo
// {
//     characterPk = 1,
//     characterClass = CharacterClass.Knight,
//     maxHP = 100,
//     currentHP = 100,
//     ATK = 20,
//     DEF = 15,
//     speed = 10,
//     reduceCoolTime = 5,
//     avoidanceRate = 10
// };
