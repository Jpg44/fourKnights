using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShop : MonoBehaviour
{
    private BackEndDataReceiver backEndDataReceiver;

    public TextMeshProUGUI needGoldText;  // 필요 골드 표시
    public TextMeshProUGUI needMaterialText; // 필요 재료 표시
    public Button enforceButton;
    // public Button advanceButton;
    public Image weaponImage;
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI levelText;  // 강화 레벨 표시 텍스트

    public TextMeshProUGUI GetMaterial; // 현재 보유중인 재료
    public TextMeshProUGUI GetGold; // 현재 보유중인 골드

    public TextMeshProUGUI title;

    public CharacterManager characterManager;


    [System.Serializable]
    public class EquipmentData
    {
        public string equipmentName; // 장비 타입
        public Sprite equipmentSprite; // 장비 이미지
        public int gearLevel;  // 현재 강화 레벨
        public int gearGrade; // 현재 승급 레벨
    }

    public List<EquipmentData> equipments; // 장비 데이터 리스트
    public Button[] equipmentButtons; // 장비 선택 버튼 배열

    private int maxLevel = 10; // 최대 강화 레벨
    private EquipmentData selectedEquipment; // 현재 선택된 장비

    private int selectedEqNum;

    // 강화시 필요한 재료 목록
    private int[] materialCosts = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 0 };
    private int[] goldCosts = { 100, 100, 200, 300, 500, 800, 1300, 2100, 3400, 5500, 0 };

    void Start()
    {
        backEndDataReceiver = FindObjectOfType<BackEndDataReceiver>();

        equipments[0].gearLevel = backEndDataReceiver.weaponLevel;
        equipments[1].gearLevel = backEndDataReceiver.helmetLevel;
        equipments[2].gearLevel = backEndDataReceiver.armorLevel;
        equipments[3].gearLevel = backEndDataReceiver.gloveLevel;
        equipments[4].gearLevel = backEndDataReceiver.shoesLevel;
        equipments[5].gearLevel = backEndDataReceiver.cloakLevel;

        selectedEquipment = equipments[0];

        UpdateUI();

        // 버튼에 이벤트 리스너 추가
        for (int i = 0; i < equipmentButtons.Length; i++)
        {
            int index = i;
            equipmentButtons[i].onClick.AddListener(() => SelectEquipment(index));
        }

        enforceButton.onClick.AddListener(EnforceEquipment);

    }

    void Update()
    {
        if (characterManager == null)
        {
            characterManager = FindObjectOfType<CharacterManager>();
        }

        if (backEndDataReceiver == null)
        {
            backEndDataReceiver = FindObjectOfType<BackEndDataReceiver>();
        }
    }

    public void SetTitle(string type)
    {
        switch (type)
        {
            case "강화": title.text = "장비 강화"; break;
            case "승급": title.text = "장비 승급"; break;
            default: break;
        }
    }

    void SelectEquipment(int index)
    {
        // 선택된 장비 업데이트
        selectedEquipment = equipments[index];
        selectedEqNum = index;
        UpdateUI();
    }

    void EnforceEquipment()
    {
        switch (title.text)
        {
            case "장비 강화":
                if (selectedEquipment.gearLevel < 100)
                {
                    Enforce(selectedEquipment.gearLevel, backEndDataReceiver.gold, backEndDataReceiver.enforceStone1);
                }
                else if (selectedEquipment.gearLevel < 200)
                {
                    Enforce(selectedEquipment.gearLevel, backEndDataReceiver.gold, backEndDataReceiver.enforceStone2);
                }
                else
                {
                    Debug.Log("3티어로의 강화은 제작되지 않았습니다.");
                }
                break;
            case "장비 승급":
                if (selectedEquipment.gearLevel < 100)
                {
                    advancement(selectedEquipment.gearLevel, backEndDataReceiver.gold, backEndDataReceiver.advanceStone1);
                }
                else if (selectedEquipment.gearLevel < 200)
                {
                    advancement(selectedEquipment.gearLevel, backEndDataReceiver.gold, backEndDataReceiver.advanceStone2);
                }
                else
                {
                    Debug.Log("3티어로의 승급은 제작되지 않았습니다.");
                }
                break;
            default: break;
        }
        UpdateUI();
        // if (selectedEquipment.gearLevel <= maxLevel)
        // {
        //     selectedEquipment.gearLevel++;
        //     UpdateUI();
        // }
    }

    void UpdateUI()
    {
        // for (int i=0; i<UserItem.userItemList.Count; i++)
        // {
        //     Debug.Log(UserItem.userItemList[i].quantity.ToString());
        //     switch(UserItem.userItemList[i].itemName)
        //     {
        //         case "Gold": GetGold.text = UserItem.userItemList[i].quantity.ToString(); break;
        //         case "enforceStone1": GetMaterial.text = UserItem.userItemList[i].quantity.ToString(); break;
        //         default: break;
        //     }
        // }
        int level = selectedEquipment.gearLevel;

        // 선택된 장비의 강화 수치에 따라 UI 변경
        needGoldText.text = $"필요 골드 \n{goldCosts[level]}";
        needMaterialText.text = $"필요 재료 \n{materialCosts[level]}";
        weaponImage.sprite = selectedEquipment.equipmentSprite;
        levelText.text = $"현재 강화 레벨: +{level}";
        GetGold.text = ItemManager.userItemList[0].quantity.ToString() + "골드";
        for (int i = 0; i < ItemManager.userItemList.Count; i++)
        {
            if (ItemManager.userItemList[i].itemName == "enforceStone1")
            {
                GetMaterial.text = ItemManager.userItemList[i].quantity.ToString() + "개";
            }
        }
        // GetGold.text = backEndDataReceiver.gold.ToString();
        // GetMaterial.text = backEndDataReceiver.enforceStone1.ToString();
    }

    public bool enforceOfSuccess(int itemGrade)
    {
        // itemGrade가 0에서 1일 떄는 100퍼, ... , 9에서 10강이 10퍼
        // 1 - (itemGrade*0.1)이 강화확률
        int successRate = 10 - itemGrade;

        int randomNum = Random.Range(0, 10); // 0~9까지
        if (randomNum < successRate)
        {
            Debug.Log("강화 성공");
            return true;
        }
        else
        {
            Debug.Log("강화 실패");
            return false;
        }
    }

    public int requirements(int itemGrade)
    {
        switch (itemGrade)
        {
            case 0: return 100;
            case 1: return 100;
            case 2: return 200;
            case 3: return 300;
            case 4: return 500;
            case 5: return 800;
            case 6: return 1300;
            case 7: return 2100;
            case 8: return 3400;
            case 9: return 5500;
            default: return 0;

        }
    }
    public void Enforce(int itemGrade, int gold, int enforceStone1)
    {
        if (itemGrade >= 10)
        {
            Debug.Log("풀강입니다!");
            return;
        }

        if (requirements(itemGrade) <= gold && itemGrade + 1 <= enforceStone1)
        {
            int receiveGearPk = -1;
            string receiveStatType = "";
            switch (selectedEqNum)
            {
                case 0:
                    receiveGearPk = backEndDataReceiver.myWeaponPk;
                    receiveStatType = "ATK"; break;
                case 1:
                    receiveGearPk = backEndDataReceiver.myHelmetPk;
                    receiveStatType = "DEF"; break;
                case 2:
                    receiveGearPk = backEndDataReceiver.myArmorPk;
                    receiveStatType = "maxHP"; break;
                case 3:
                    receiveGearPk = backEndDataReceiver.myGlovePk;
                    receiveStatType = "reduceCoolTime"; break;
                case 4:
                    receiveGearPk = backEndDataReceiver.myShoesPk;
                    receiveStatType = "speed"; break;
                case 5:
                    receiveGearPk = backEndDataReceiver.myCloakPk;
                    receiveStatType = "avoidanceRate"; break;
                default: break;
            }
            int receiveGearLv = equipments[selectedEqNum].gearLevel;
            int receiveGold = gold;
            int receiveEnforeceStone1 = enforceStone1;
            int receiveStatValue = 0;

            Debug.Log(gold + "골드를 보유하여 " + requirements(itemGrade) + "골드를 사용합니다.");
            Debug.Log(enforceStone1 + "개의 재료를 보유하고 " + (itemGrade + 1) + "개의 재료를 사용합니다");
            if (ItemManager.userItemList.Count > 0)
            {
                Obj objGold = ItemManager.userItemList[0];
                objGold.quantity -= requirements(itemGrade);
                receiveGold = objGold.quantity; // 사용 후 골드 적용
                ItemManager.userItemList[0] = objGold;

                for (int i = 0; i < ItemManager.userItemList.Count; i++)
                {
                    if (ItemManager.userItemList[i].itemName == "enforceStone1")
                    {
                        Obj objEnforceStone1 = ItemManager.userItemList[i];
                        objEnforceStone1.quantity -= (itemGrade + 1);
                        receiveEnforeceStone1 = objEnforceStone1.quantity; // 사용 후 재료 적용
                        ItemManager.userItemList[i] = objEnforceStone1;
                    }
                }
            }

            //ItemManager.userItemList[0].quantity -= requirements(itemGrade);
            //backEndDataReceiver.enforceStone1 -= (itemGrade + 1);

            if (enforceOfSuccess(itemGrade))
            {
                //selectedEquipment.gearLevel++;
                equipments[selectedEqNum].gearLevel++;

                switch (selectedEqNum)
                {
                    case 0: // 무기 -> 공격력 1증가
                        receiveGearLv = ++backEndDataReceiver.weaponLevel;
                        receiveStatValue = ++characterManager.ATK;
                        break;
                    case 1: // 헬맷 -> 방어력 1증가
                        receiveGearLv = ++backEndDataReceiver.helmetLevel;
                        receiveStatValue = ++characterManager.DEF;
                        break;
                    case 2: // 아머 -> 체력 50증가
                        receiveGearLv = ++backEndDataReceiver.armorLevel;
                        characterManager.maxHP += 50;
                        receiveStatValue = characterManager.maxHP;
                        break;
                    case 3: // 글러브 -> 쿨감 1증가
                        receiveGearLv = ++backEndDataReceiver.gloveLevel;
                        receiveStatValue = ++characterManager.reduceCoolTime;
                        break;
                    case 4: // 신발 -> 이속 1증가
                        receiveGearLv = ++backEndDataReceiver.shoesLevel;
                        receiveStatValue = ++characterManager.speed;
                        break;
                    case 5: // 망토 -> 회피율 1증가
                        receiveGearLv = ++backEndDataReceiver.cloakLevel;
                        receiveStatValue = ++characterManager.avoidanceRate;
                        break;
                    default: break;
                }

                selectedEquipment = equipments[selectedEqNum];
                backEndDataReceiver.enforceTry(receiveGearPk, receiveGearLv, receiveGold, 3, receiveEnforeceStone1, true, receiveStatType, receiveStatValue);
            }
            else
            {
                backEndDataReceiver.enforceTry(receiveGearPk, receiveGearLv, receiveGold, 3, receiveEnforeceStone1, false, receiveStatType, receiveStatValue);
            }
        }
        else
        {
            if (gold - requirements(itemGrade) < 0)
            {
                Debug.Log((requirements(itemGrade) - gold) + "골드가 모자랍니다.");
            }
            if ((enforceStone1 - (itemGrade + 1)) < 0)
            {
                Debug.Log("재료가 " + ((itemGrade + 1) - enforceStone1) + "개 모자랍니다.");
            }
        }
    }

    // 승급
    public void advancement(int itemGrade, int gold, int advanceStone1)
    {
        if (itemGrade < 5)
        {
            Debug.Log("5강 미만은 승급이 안되요잉");
            return;
        }

        if (8000 <= gold && 1 <= advanceStone1)
        {
            int receiveGearPk = -1;
            string receiveStatType = "";
            switch (selectedEqNum)
            {
                case 0:
                    receiveGearPk = backEndDataReceiver.myWeaponPk;
                    receiveStatType = "ATK"; break;
                case 1:
                    receiveGearPk = backEndDataReceiver.myHelmetPk;
                    receiveStatType = "DEF"; break;
                case 2:
                    receiveGearPk = backEndDataReceiver.myArmorPk;
                    receiveStatType = "maxHP"; break;
                case 3:
                    receiveGearPk = backEndDataReceiver.myGlovePk;
                    receiveStatType = "reduceCoolTime"; break;
                case 4:
                    receiveGearPk = backEndDataReceiver.myShoesPk;
                    receiveStatType = "speed"; break;
                case 5:
                    receiveGearPk = backEndDataReceiver.myCloakPk;
                    receiveStatType = "avoidanceRate"; break;
                default: break;
            }

            int receiveGearLv = equipments[selectedEqNum].gearLevel;
            int receiveGold = gold;
            int receiveAdvanceStone1 = advanceStone1;
            int receiveStatValue = 0;

            Debug.Log(gold + "골드를 보유하여 " + 8000 + "골드를 사용합니다.");
            Debug.Log(advanceStone1 + "개의 재료를 보유하고 " + 1 + "개의 재료를 사용합니다");
            if (ItemManager.userItemList.Count > 0)
            {
                Obj objGold = ItemManager.userItemList[0];
                objGold.quantity -= 8000;
                receiveGold = objGold.quantity; // 사용 후 골드 적용
                ItemManager.userItemList[0] = objGold;

                for (int i = 0; i < ItemManager.userItemList.Count; i++)
                {
                    if (ItemManager.userItemList[i].itemName == "UpgradeMaterial1")
                    {
                        Obj objAdvanceStone1 = ItemManager.userItemList[i];
                        objAdvanceStone1.quantity -= 1;
                        receiveAdvanceStone1 = objAdvanceStone1.quantity; // 사용 후 재료 적용
                        ItemManager.userItemList[i] = objAdvanceStone1;
                    }
                }
            }

            equipments[selectedEqNum].gearLevel += 95;

            switch (selectedEqNum)
            {
                case 0: // 무기 -> 공격력 100 + 1레벨 당 10 증가
                    backEndDataReceiver.weaponLevel += 95;
                    receiveGearLv = backEndDataReceiver.weaponLevel;
                    characterManager.ATK = 100 + (100 - receiveGearLv) * 10 - (backEndDataReceiver.weaponLevel - 95);
                    receiveStatValue = characterManager.ATK;
                    break;
                case 1: // 헬맷 -> 방어력 100 + 1레벨 당 10 증가
                    backEndDataReceiver.helmetLevel += 95;
                    receiveGearLv = backEndDataReceiver.helmetLevel;
                    characterManager.DEF = 100 + (100 - receiveGearLv) * 10 - (backEndDataReceiver.helmetLevel - 95);
                    receiveStatValue = characterManager.DEF;
                    break;
                case 2: // 아머 -> 체력 50 ,,, 처리필요
                    receiveGearLv = ++backEndDataReceiver.armorLevel + 95;
                    characterManager.maxHP += 50;
                    receiveStatValue = characterManager.maxHP;
                    break;
                case 3: // 글러브 -> 쿨감 20 + 1레벨 당 2% 증가
                    backEndDataReceiver.gloveLevel += 95;
                    receiveGearLv = backEndDataReceiver.gloveLevel;
                    characterManager.reduceCoolTime = 20 + (100 - receiveGearLv) * 2 - (backEndDataReceiver.gloveLevel - 95);
                    receiveStatValue = characterManager.reduceCoolTime;
                    break;
                case 4: // 신발 -> 이속 10 증가 + 1레벨 당 1 증가
                    backEndDataReceiver.shoesLevel += 95;
                    receiveGearLv = backEndDataReceiver.shoesLevel;
                    characterManager.speed = 10 + (100 - receiveGearLv) * 1 - (backEndDataReceiver.shoesLevel - 95);
                    receiveStatValue = characterManager.speed;
                    break;
                case 5: // 망토 -> 회피율 20 증가 + 1레벨 당 2% 증가
                    backEndDataReceiver.cloakLevel += 95;
                    receiveGearLv = backEndDataReceiver.cloakLevel;
                    characterManager.avoidanceRate = 20 + (100 - receiveGearLv) * 2 - (backEndDataReceiver.cloakLevel - 95);
                    receiveStatValue = characterManager.avoidanceRate;
                    break;
                default: break;
            }

            selectedEquipment = equipments[selectedEqNum];
            backEndDataReceiver.advancementTry(receiveGearPk, receiveGearLv, receiveGold, 3, receiveAdvanceStone1, true, receiveStatType, receiveStatValue);
        }
        else
        {
            if (8000 > gold)
            {
                Debug.Log(8000 - gold + "골드가 모자랍니다.");
            }
            if (1 > advanceStone1)
            {
                Debug.Log("승급석이 없습니다.");
            }
        }
    }
}
