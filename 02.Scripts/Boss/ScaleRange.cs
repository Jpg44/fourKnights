using Unity.VisualScripting;
using UnityEngine;

public class ScaleRange : MonoBehaviour
{
    public float maxScale = 0.3f; // 최대 Scale 값
    public float scaleSpeed = 0.01f; // 증가 속도

    private Vector3 originalScale; // 원래 Scale 저장
    private Vector3 startScale;

    // Start is called before the first frame update

    void Start()
    {
        startScale = transform.localScale;
    }
    void OnEnable()
    {
        // originalScale = new Vector3(0.1f, 0.1f, 1f); // X축을 제외하고 원래 Scale 저장
        transform.localScale = startScale; // 원래 Scale로 설정
        
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 Scale
        Vector3 scale = transform.localScale;

        // X 축의 Scale만 증가시키기
        scale.x += scaleSpeed * Time.deltaTime;
        scale.y += scaleSpeed * Time.deltaTime;

        // Scale이 최대치를 넘지 않도록 제한하기
        if (scale.x < maxScale)
        {
            transform.localScale = scale;
        }
        else // 최대 Scale에 도달했을 때 비활성화
        {
            transform.localScale = new Vector3(maxScale, scale.y, scale.z); // Scale을 최대치로 설정
            gameObject.SetActive(false); // 게임 오브젝트 비활성화
        }

        // Y 축 위치 고정
        Vector3 position = transform.position;
        position.y = -5f; // Y 축 값을 0.007로 고정
        transform.position = position;
    }
}