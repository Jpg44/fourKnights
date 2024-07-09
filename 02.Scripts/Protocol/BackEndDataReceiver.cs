using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;

public class BackEndDataReceiver : MonoBehaviourPunCallbacks
{
    // 싱글톤
    private static BackEndDataReceiver instance;

    public static BackEndDataReceiver Instance
    {
        get
        {
            // 인스턴스가 없는 경우 새로 생성
            if (instance == null)
            {
                Debug.Log("백리시버 새 오브젝트 생성해서 붙인당");
                // 새 게임 오브젝트를 생성하여 게임 매니저를 붙임
                GameObject singleton = new GameObject("BackEndDataReceiver");
                instance = singleton.AddComponent<BackEndDataReceiver>();
            }
            return instance;
        }
    }
    //public static BackEndDataReceiver instance = null;
    CharacterManager characterManager;
    UserManager userManager;

    private void Awake()
    {
        if (instance == null)
        {
            Debug.Log("백리시버 강철모두");
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.Log("백리시버 삭제~");
            Destroy(this.gameObject);
        }
        // if (instance == null)
        // {
        //     instance = this;
        // }
        // else if (instance != this)
        // {
        //     Destroy(gameObject);
        // }
        // DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        characterManager = FindObjectOfType<CharacterManager>();
        userManager = FindObjectOfType<UserManager>();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            Debug.Log("백리시버 삭제~");
            Destroy(gameObject);
        }

        if (SceneManager.GetActiveScene().name != "CharacterScene")
        {
            if (ItemManager.userItemList.Count > 0)
            {
                gold = ItemManager.userItemList[0].quantity;
                for (int i = 0; i < ItemManager.userItemList.Count; i++)
                {
                    // if (ItemManager.userItemList[i].itemName == "enforceStone1")
                    // {
                    //     enforceStone1 = ItemManager.userItemList[i].quantity;
                    // }
                    switch (ItemManager.userItemList[i].itemName)
                    {
                        case "Potion": potion = ItemManager.userItemList[i].quantity; break;
                        case "enforceStone1": enforceStone1 = ItemManager.userItemList[i].quantity; break;
                        case "enforceStone2": enforceStone2 = ItemManager.userItemList[i].quantity; break;
                        case "UpgradeMaterial1": advanceStone1 = ItemManager.userItemList[i].quantity; break;
                        case "UpgradeMaterial2": advanceStone2 = ItemManager.userItemList[i].quantity; break;
                        case "RatoTale": RatoTale = ItemManager.userItemList[i].quantity; break;
                        case "AntoChin": AntoChin = ItemManager.userItemList[i].quantity; break;
                        case "WormoSoil": Wormosoil = ItemManager.userItemList[i].quantity; break;
                        case "ScopiontoPoison": ScopiontoPoison = ItemManager.userItemList[i].quantity; break;
                    }
                }
            }
        }

    }

    public int weaponLevel;
    public int helmetLevel;
    public int armorLevel;
    public int gloveLevel;
    public int shoesLevel;
    public int cloakLevel;
    public int myWeaponPk;
    public int myHelmetPk;
    public int myArmorPk;
    public int myGlovePk;
    public int myShoesPk;
    public int myCloakPk;

    public int gold;
    public int potion;
    public int enforceStone1;
    public int enforceStone2;
    public int advanceStone1;
    public int advanceStone2;
    public int RatoTale;
    public int AntoChin;
    public int Wormosoil;
    public int ScopiontoPoison;

    public List<int> itemPk;
    public List<int> inventoryNum;
    public List<int> quantity;

    public TMP_InputField nicknameInputField;
    public TextMeshProUGUI nicknameCheckText;

    public int jobCode;

    //public int characterInfoPk;

    // //////////////////// test용 로그인 코드

    ////////////////////////////////


    // 캐릭터 선택창에서 인벤토리, 스킬세팅, 장비 정보를 가져옴 (GET)
    public void characterPick(int num)
    {
        if (userManager.characterSlots[num] != null)
        {
            StartCoroutine(pick(userManager.characterSlots[num].characterInfoPk));
        }

    }

    public IEnumerator pick(int characterInfoPk)
    {
        string userId = characterManager.userID;

        Debug.Log("백엔드리시버의 유저 아이디 : " + userId);
        Debug.Log("백엔드리시버의 캐릭터 인포pk : " + characterInfoPk);
        string url = $"http://k10b208.p.ssafy.io:8081/character/choose?userId={userId}&characterInfoPk={characterInfoPk}";

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

                InventoryData inventoryData = JsonUtility.FromJson<InventoryData>(request.downloadHandler.text);

                weaponLevel = inventoryData.gearInventories[0].gearLevel;
                helmetLevel = inventoryData.gearInventories[1].gearLevel;
                armorLevel = inventoryData.gearInventories[2].gearLevel;
                gloveLevel = inventoryData.gearInventories[3].gearLevel;
                shoesLevel = inventoryData.gearInventories[4].gearLevel;
                cloakLevel = inventoryData.gearInventories[5].gearLevel;

                myWeaponPk = inventoryData.gearInventories[0].gearInventoryPk;
                myHelmetPk = inventoryData.gearInventories[1].gearInventoryPk;
                myArmorPk = inventoryData.gearInventories[2].gearInventoryPk;
                myGlovePk = inventoryData.gearInventories[3].gearInventoryPk;
                myShoesPk = inventoryData.gearInventories[4].gearInventoryPk;
                myCloakPk = inventoryData.gearInventories[5].gearInventoryPk;

                // 캐릭터 바뀔 때, 아이템들 초기화
                gold = 0;
                potion = 0;
                enforceStone1 = 0;
                enforceStone2 = 0;
                advanceStone1 = 0;
                advanceStone2 = 0;
                RatoTale = 0;
                AntoChin = 0;
                Wormosoil = 0;
                ScopiontoPoison = 0;

                itemPk = new List<int>();
                quantity = new List<int>();
                inventoryNum = new List<int>();

                foreach (var inven in inventoryData.inventory)
                {
                    switch (inven.itemPk)
                    {
                        case 1: gold = inven.quantity; break;
                        case 2: potion = inven.quantity; break;
                        case 3: enforceStone1 = inven.quantity; break;
                        case 4: enforceStone2 = inven.quantity; break;
                        case 5: advanceStone1 = inven.quantity; break;
                        case 6: advanceStone2 = inven.quantity; break;
                        case 7: RatoTale = inven.quantity; break;
                        case 8: AntoChin = inven.quantity; break;
                        case 9: Wormosoil = inven.quantity; break;
                        case 10: ScopiontoPoison = inven.quantity; break;
                        default: break;
                    }

                    itemPk.Add(inven.itemPk);
                    quantity.Add(inven.quantity);
                    inventoryNum.Add(inven.location);
                }
            }
        }
    }

    // 캐릭터 생성
    public class DataToSend
    {
        public string userId;
        public int jobPk;
        public string characterNickname;
    }

    public void createCharacter()
    {
        if (jobCode == 0 || nicknameInputField.text == "")
        {
            return;
        }
        // 데이터 객체 생성
        DataToSend data = new DataToSend
        {
            userId = characterManager.userID,
            jobPk = jobCode,
            characterNickname = nicknameInputField.text
        };

        // JSON으로 변환
        string jsonData = JsonUtility.ToJson(data);

        // abc 주소로 POST 요청 생성
        string url = "http://k10b208.p.ssafy.io:8081/character/choose";
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        // HTTP 헤더 설정
        request.SetRequestHeader("Content-Type", "application/json");

        // 요청 전송
        request.SendWebRequest().completed += (asyncOperation) =>
        {
            // 요청 완료 후 처리
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("Response: " + request.downloadHandler.text);

                CreateData createData = JsonUtility.FromJson<CreateData>(request.downloadHandler.text);

                Debug.Log(createData.characterInfo.characterInfoPk);
                Debug.Log(createData.characterInfo.jobPk);
                Debug.Log(createData.characterInfo.level);

                userManager.GetCharacter();


                characterManager.selectedCharacter(userManager.charCount);
                characterPick(userManager.charCount);


            }
        };
    }

    // 닉네임 중복 확인

    public void NicknameCheck()
    {
        if (nicknameInputField.text == "")
        {
            return;
        }
        StartCoroutine(NickCheck());
    }

    public IEnumerator NickCheck()
    {
        string nickname = nicknameInputField.text;

        string url = $"http://k10b208.p.ssafy.io:8081/character/nickname?characterNickname={nickname}";

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
                nicknameInputField.readOnly = true;
                nicknameCheckText.text = "사용 가능";
            }
        }
    }

    public void WarriorBtn()
    {
        jobCode = 1;
    }

    public void StellaBtn()
    {
        jobCode = 3;
    }

    // 캐릭터 삭제


    // 보스 잡았을 때
    // public void KillTheBoss()
    // {

    // }

    // 강화 시도했을 때
    public class EnforceData
    {
        public int characterInfoPk; // 캐릭터의 고유 번호
        public int gearInventoryPk; // 장비 고유 번호
        public int gearLevel;
        public int gold;
        public int itemPk;
        public int reinforcementMaterial;
        public bool isSuccess;
        public string statType;
        public int statValue;
    }

    public void enforceTry(int gearInventoryPk, int gearLevel, int gold, int itemPk, int reinforcementMaterial, bool isSuccess, string statType, int statValue)
    {
        Debug.Log("gearInventoryPk: " + gearInventoryPk);
        Debug.Log("gearLevel: " + gearLevel);
        Debug.Log("gold: " + gold);
        Debug.Log("reinforcementMaterial: " + reinforcementMaterial);
        Debug.Log("isSuccess: " + isSuccess);
        Debug.Log("statType: " + statType);
        Debug.Log("statValue: " + statValue);

        // 데이터 객체 생성
        EnforceData data = new EnforceData
        {
            characterInfoPk = characterManager.characterInfoPk,
            gearInventoryPk = gearInventoryPk,
            gearLevel = gearLevel,
            gold = gold,
            itemPk = itemPk,
            reinforcementMaterial = reinforcementMaterial,
            isSuccess = isSuccess,
            statType = statType,
            statValue = statValue
        };

        // JSON으로 변환
        string jsonData = JsonUtility.ToJson(data);

        // abc 주소로 POST 요청 생성
        string url = "http://k10b208.p.ssafy.io:8081/town/reinforcement";
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        // HTTP 헤더 설정
        request.SetRequestHeader("Content-Type", "application/json");

        // 요청 전송
        request.SendWebRequest().completed += (asyncOperation) =>
        {
            // 요청 완료 후 처리
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("Response: " + request.downloadHandler.text);
            }
        };
    }

    // 계승 성공했을 때
    public class AdvancementData
    {
        public int characterInfoPk; // 캐릭터의 고유 번호
        public int gearInventoryPk; // 장비 고유 번호
        public int gearLevel;
        public int gearAdvanceLevel; // 장비 승급레벨 추가되어야함
        public int gold;
        public int itemPk;
        public int advancementMaterial;
        public bool isSuccess;
        public string statType;
        public int statValue;
    }
    public void advancementTry(int gearInventoryPk, int gearLevel, int gold, int itemPk, int advancementMaterial, bool isSuccess, string statType, int statValue)
    {
        Debug.Log("gearInventoryPk: " + gearInventoryPk);
        int tmp = gearLevel % 100;
        Debug.Log("gearLevel: " + gearLevel);
        int gearAdvanceLevel = (int)(gearLevel / 100);
        Debug.Log("gearAdvanceLevel:" + gearAdvanceLevel);
        Debug.Log("gold: " + gold);
        Debug.Log("advancementMaterial: " + advancementMaterial);
        Debug.Log("isSuccess: " + isSuccess);
        Debug.Log("statType: " + statType);
        Debug.Log("statValue: " + statValue);

        // 데이터 객체 생성
        AdvancementData data = new AdvancementData
        {
            characterInfoPk = characterManager.characterInfoPk,
            gearInventoryPk = gearInventoryPk,
            gearLevel = tmp,
            gearAdvanceLevel = gearAdvanceLevel,
            gold = gold,
            itemPk = itemPk,
            advancementMaterial = advancementMaterial,
            isSuccess = isSuccess,
            statType = statType,
            statValue = statValue
        };

        // JSON으로 변환
        string jsonData = JsonUtility.ToJson(data);
        Debug.Log("JSON Data: " + jsonData);
        // abc 주소로 POST 요청 생성
        string url = "http://k10b208.p.ssafy.io:8081/town/advancement";
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        // HTTP 헤더 설정
        request.SetRequestHeader("Content-Type", "application/json");

        // // 요청 전송
        // request.SendWebRequest().completed += (asyncOperation) =>
        // {
        //     // 요청 완료 후 처리
        //     if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        //     {
        //         Debug.LogError(request.error);
        //     }
        //     else
        //     {
        //         Debug.Log("Response: " + request.downloadHandler.text);
        //     }
        // };
        try
        {
            request.SendWebRequest().completed += (asyncOperation) =>
            {
                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + request.error);
                    Debug.LogError("Response: " + request.downloadHandler.text);
                }
                else
                {
                    Debug.Log("Response: " + request.downloadHandler.text);
                }
            };
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception: " + ex.Message);
        }
    }




    // 씬 전환될 때
    [System.Serializable]
    public class SaveData
    {
        public int characterInfoPk;
        public int currentHP;
        public Inventory[] inventory;
    }

    public void SaveOfChangeScene(string logInfo)
    {
        Inventory[] inventoryArr = new Inventory[ItemManager.userItemList.Count];
        for (int i = 0; i < ItemManager.userItemList.Count; i++)
        {
            Debug.Log("데이터 저장 체크사항");
            Debug.Log("아이템pk : " + ItemManager.userItemList[i].itemNum);
            Debug.Log("아이템 수 : " + ItemManager.userItemList[i].quantity);
            Debug.Log("아이템 위치 : " + ItemManager.userItemList[i].inventoryNum);
            inventoryArr[i] = new Inventory
            {
                itemPk = ItemManager.userItemList[i].itemNum,
                quantity = ItemManager.userItemList[i].quantity,
                location = ItemManager.userItemList[i].inventoryNum
            };
        }

        // 데이터 객체 생성
        SaveData data = new SaveData
        {
            characterInfoPk = characterManager.characterInfoPk,
            currentHP = characterManager.currentHP,
            inventory = inventoryArr
        };

        // JSON으로 변환
        string jsonData = JsonUtility.ToJson(data);
        string url;
        // abc 주소로 POST 요청 생성
        switch (logInfo)
        {
            case "fieldIn": url = "http://k10b208.p.ssafy.io:8081/field/entrance"; break;
            case "fieldOut": url = "http://k10b208.p.ssafy.io:8081/field/exit"; break;
            case "RaidIn": url = "http://k10b208.p.ssafy.io:8081/raid/entrance"; break;
            case "RaidOut": url = "http://k10b208.p.ssafy.io:8081/raid/exit"; break;
            default: url = "null"; break;
        }
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        // HTTP 헤더 설정
        request.SetRequestHeader("Content-Type", "application/json");

        // 요청 전송
        request.SendWebRequest().completed += (asyncOperation) =>
        {
            // 요청 완료 후 처리
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("데이터 저장 성공, Response: " + request.downloadHandler.text);
            }
        };
    }





    // 일정 시간마다
    // 강제종료의 경우는 wpf에 먼저 보내기에 그쪽 코드로

    [System.Serializable]
    public class Inventory
    {
        public int itemPk;
        public int quantity;
        public int location;
    }

    [System.Serializable]
    public class GearInventory
    {
        public string gearType;
        public int gearInventoryPk;
        public int gearLevel;
    }

    [System.Serializable]
    public class SkillSetting
    {
        public int skillSetting1;
        public int skillSetting2;
        public int skillSetting3;
        public int skillSetting4;
    }

    // 전체 JSON 구조에 해당하는 클래스
    [System.Serializable]
    public class InventoryData
    {
        public string message;
        public int statusCode;
        public Inventory[] inventory;
        public GearInventory[] gearInventories; // gearInventories 부분을 담을 배열
        public SkillSetting[] skillSetting;
    }


    [System.Serializable]
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
    }

    [System.Serializable]
    public class CreateData
    {
        public CharacterInfo characterInfo;
    }

}
