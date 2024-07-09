using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Unity.VisualScripting;

// 현재 선택된 직업에 대한 정보를 가지고 있는 매니저 (싱글톤)
// 유저 정보 (userID, 닉네임) + 선택된 캐릭터의 정보 (캐릭터pk, 직업, HP, ATK, DEF, 스피드, 쿨감, 회피율, 인벤토리, 스킬 세팅, 방어구)
public class CharacterManager : MonoBehaviourPunCallbacks
{
    private UserManager userManager;
    private PhotonPlayerController playerController;
    private BackEndDataReceiver backEndDataReceiver;

    // 유저 정보
    public string userID;
    public string nickname;

    // 선택된 캐릭터 정보
    public string characterNickname;
    public int characterInfoPk;
    public string characterClass;
    public int level;
    public int maxHP;
    public int currentHP;
    public int ATK;
    public int DEF;
    public int speed;
    public int reduceCoolTime;
    public int avoidanceRate;
    public bool isAlive = true;
    public int shield;
    public bool isShield = false;

    // 인벤토리, 스킬 세팅, 방어구
    public Dictionary<string, int> inventory = new Dictionary<string, int>();
    public List<int> skillSetting = new List<int>();
    public List<Dictionary<string, object>> gearList = new List<Dictionary<string, object>>();

    // 싱글톤
    private static CharacterManager instance;

    public static CharacterManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우 새로 생성
            if (instance == null)
            {
                Debug.Log("캐릭터매니저 새 오브젝트 생성해서 붙인당");
                // 새 게임 오브젝트를 생성하여 게임 매니저를 붙임
                GameObject singleton = new GameObject("CharacterManager");
                instance = singleton.AddComponent<CharacterManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            Debug.Log("캐릭매니저 강철모두");
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.Log("캐릭매니저 삭제~");
            Destroy(this.gameObject);
        }

        // UserManager를 찾아서 참조 설정
        userManager = FindObjectOfType<UserManager>();
        backEndDataReceiver = FindObjectOfType<BackEndDataReceiver>();
    }

    private void Start()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        if (args.Length > 1)
        {
            userID = args[1]; // 첫 번째 인수 (인덱스 1)를 가져옵니다.
            Debug.Log("Received userId: " + userID);
        }

        // userID = "jhtest1";
        userID = "testcase1";
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            Debug.Log("캐릭매니저 삭제~");
            Destroy(gameObject);
        }
    }

    public void selectedCharacter(int i)
    {
        userManager.pick = i;

        nickname = userManager.GetNickname();
        characterNickname = userManager.GetCharacterNickname();
        characterInfoPk = userManager.GetCharacterInfoPk();
        characterClass = userManager.GetCharacterClass();
        level = userManager.GetLevel();
        maxHP = userManager.GetMaxHP();
        currentHP = userManager.GetCurrentHP();
        ATK = userManager.GetATK();
        DEF = userManager.GetDEF();
        speed = userManager.Getspeed();
        reduceCoolTime = userManager.GetReduceCoolTime();
        avoidanceRate = userManager.GetAvoidanceRate();
    }

    public void SetHP(int damage)
    {
        int avoidRanNum = Random.Range(1, 100);

        if (avoidRanNum <= avoidanceRate)
        {
            Debug.Log("회피!");
            return;
        }

        int reduction = (int)(damage * (1 - (float)DEF / (DEF + 100)));
        int ranNum = Random.Range(-10, 11);
        int totalDmg = reduction + (int)(reduction * ranNum / 100); // 데미지 바운더리 10%

        if (isAlive)
        {
            totalDmg = SetShield(totalDmg);
            currentHP -= totalDmg;
        }

        if (currentHP <= 0 && isAlive)
        {
            isAlive = false;
            Die();
        }
    }
    public void Die()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            PhotonView pv = player.GetComponent<PhotonView>();
            if (pv != null && pv.IsMine)
            {
                // 모든 클라이언트에게 이 오브젝트를 비활성화하도록 RPC 호출
                photonView.RPC("RPC_DisableObject", RpcTarget.Others, pv.ViewID);
                // 로컬에서도 비활성화
                player.SetActive(false);
            }
        }



        isAlive = false;
        currentHP = 0;
        if (ItemManager.userItemList != null)
        {
            Obj goldDelete = ItemManager.userItemList[0];
            goldDelete.quantity = goldDelete.quantity * 8 / 10;
            print(goldDelete.quantity);
            ItemManager.userItemList[0] = goldDelete;
        }
        if (playerController != null)
        {
            PhotonPlayerMovement playerMovement = playerController.GetComponent<PhotonPlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.Die();
            }
        }
    }

    [PunRPC]
    private void RPC_DisableObject(int viewID)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView != null)
        {
            targetView.gameObject.SetActive(false);
        }
    }


    public int SetShield(int totalDmg)
    {
        if (shield > totalDmg)
        {
            shield -= totalDmg;
            return 0;
        }
        else if (shield == totalDmg)
        {
            shield = 0;
            return 0;
        }
        else
        {
            int res = totalDmg - shield;
            shield = 0;
            return res;
        }
    }

    public void UsePotion()
    {
        if (backEndDataReceiver == null)
        {
            backEndDataReceiver = FindObjectOfType<BackEndDataReceiver>();
        }
        else
        {

            for (int i = 0; i < ItemManager.userItemList.Count; i++)
            {
                if (ItemManager.userItemList[i].itemName == "Potion")
                {
                    Obj Potion = ItemManager.userItemList[i];
                    Potion.quantity -= 1;
                    ItemManager.userItemList[i] = Potion;
                }
            }

            if (currentHP + maxHP / 2 > maxHP)
            {
                currentHP = maxHP;
            }
            else
            {
                currentHP += maxHP / 2;
            }
        }
    }
    public void IsUI(bool check)
    {
        GameObject player = GameObject.Find("Player(Clone)");
        if (check)
        {
            player.transform.GetComponent<PhotonPlayerController>().enabled = false;
            player.transform.GetComponent<PhotonPlayerMovement>().enabled = false;
            player.transform.Find("Camera").GetComponent<PhotonCameraController>().enabled = false;
            player.transform.GetComponentInChildren<Animator>().enabled = false;
        }
        else if (!check)
        {
            player.transform.GetComponent<PhotonPlayerController>().enabled = true;
            player.transform.GetComponent<PhotonPlayerMovement>().enabled = true;
            player.transform.Find("Camera").GetComponent<PhotonCameraController>().enabled = true;
            player.transform.GetComponentInChildren<Animator>().enabled = true;
        }
    }

    public void SendToVilageScene()
    {
        // 마을로 보내기
        // SceneManager.LoadScene("VillageScene");

        // 데이터 처리 (체력 증가, 돈 감소)
        currentHP = maxHP;
        // 돈 감소 로직
    }
}
