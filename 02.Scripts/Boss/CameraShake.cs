using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) // K 키를 누를 때
        {
            Camera playerCamera = GetComponentInChildren<Camera>(); // 카메라 컴포넌트 찾기
            if (playerCamera != null)
            {
                StartCoroutine(playerCamera.GetComponent<CameraShake>().Shake(0.5f, 0.5f)); // 카메라 흔들기 코루틴 실행, 지속 시간과 강도는 조절 가능
            }
        }
    }
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            Debug.Log("흔들기");
            transform.position = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
    }
}
