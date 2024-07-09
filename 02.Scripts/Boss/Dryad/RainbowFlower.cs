using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RainbowFlower : MonoBehaviour
{
    public static RainbowFlower Instance { get; private set; }
    public GameObject redFlower;
    public GameObject greenFlower;
    public GameObject blueFlower;
    public GameObject purpleFlower;
    public GameObject alarm;
    public GameObject cutScean;

    public NavMeshAgent navMeshAgent;

    public ParticleSystem beam;

    public CharacterManager characterManager; // 캐릭터 매니저의 참조

    private Queue<GameObject> destroyOrder = new Queue<GameObject>(); // 파괴해야 할 꽃의 순서를 저장하는 큐
    private bool sequenceCorrect = true; // 순서가 정확한지 추적하는 플래그
    public float timeLimit = 20f; // 제한 시간 설정 

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
    }
    
    void OnEnable()
    {
        beam.Stop();
        alarm.SetActive(true);
        BossAttackController.Instance.isActionInProgress = true;
        GameObject characterManagerObject = GameObject.FindWithTag("CharacterManager");
        if (characterManagerObject != null) {
            characterManager = characterManagerObject.GetComponent<CharacterManager>();
        }

        // 각 꽃을 지정된 위치에 활성화합니다.
        redFlower.SetActive(true);
        greenFlower.SetActive(true);
        blueFlower.SetActive(true);
        purpleFlower.SetActive(true);

        navMeshAgent.speed = 0f;
        navMeshAgent.angularSpeed = 0f;

        // 파괴 순서 큐를 초기화합니다.
        destroyOrder.Clear(); // 큐를 비우고 새로 시작합니다.
        destroyOrder.Enqueue(redFlower);
        destroyOrder.Enqueue(greenFlower);
        destroyOrder.Enqueue(blueFlower);
        destroyOrder.Enqueue(purpleFlower);
        StartCoroutine(DisableAlarm());

        sequenceCorrect = true; // 순서 추적 플래그를 초기화
        StartCoroutine(CheckSequenceAfterTime(timeLimit)); // 시간 제한 후 순서 검사 코루틴 시작
    }

    public void FlowerDestroyed(GameObject flower)
    {
        // 꽃이 올바른 순서로 파괴되었는지 확인합니다.
        if (destroyOrder.Count > 0 && destroyOrder.Peek() == flower)
        {
            destroyOrder.Dequeue();
        }
        else
        {
            // 잘못된 순서로 파괴됐을 경우
            sequenceCorrect = false;
            cutScean.SetActive(true);
            beam.Play();
            StartCoroutine(Beam());
            characterManager.SetHP(200000);
            cutScean.SetActive(false);
            StopCoroutine(CheckSequenceAfterTime(timeLimit)); // 타이머 중지
        }
    }

    private IEnumerator CheckSequenceAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay); // 설정한 시간만큼 대기
        if (destroyOrder.Count > 0 || !sequenceCorrect) // 시간 내에 모든 꽃이 파괴되지 않았거나 순서가 틀렸을 경우
        {
            beam.Play();
            cutScean.SetActive(true);
            yield return new WaitForSeconds(4.5f);
            characterManager.SetHP(200000); // 캐릭터 HP를 설정
            cutScean.SetActive(false);
        }
        else
        {
            // 모든 꽃이 올바른 순서로 파괴되었으므로 비활성화
            redFlower.SetActive(false);
            greenFlower.SetActive(false);
            blueFlower.SetActive(false);
            purpleFlower.SetActive(false);
            navMeshAgent.speed = 2f;
            navMeshAgent.angularSpeed = 120f;
            BossAttackController.Instance.isActionInProgress = false;
            // Debug.Log("레인보우");
            yield return new WaitForSeconds(2);
            gameObject.SetActive(false);
        }
    }

    IEnumerator DisableAlarm()
    {
        yield return new WaitForSeconds(5f);
        alarm.SetActive(false);
    }

    IEnumerator Beam()
    {
        yield return new WaitForSeconds(4.5f);
        characterManager.SetHP(200000);
    }
}
