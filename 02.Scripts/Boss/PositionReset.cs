using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionReset : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController controller = other.GetComponent<CharacterController>();
            
            if (controller != null)
            {
                // 현재 위치에서 목표 y축 위치까지의 변위 계산
                float deltaY = 110 - other.transform.position.y;
                Vector3 moveVector = new Vector3(0, deltaY, 0);

                // Move 메소드를 사용하여 이동
                controller.Move(moveVector);

                Debug.Log("이동 후 위치: " + other.transform.position);
            }
        }
    }
}
