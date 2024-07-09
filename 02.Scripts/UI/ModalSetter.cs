using UnityEngine;

public class ModalSetter : MonoBehaviour
{
    public CanvasGroup modalWindowCanvasGroup;
 
    public void ShowModal()
    {
        // 모달 창을 표시
        modalWindowCanvasGroup.alpha = 0f; // 불투명, 임시로 투명하게 바꿈
        modalWindowCanvasGroup.blocksRaycasts = true; // 레이캐스트 차단 활성화
        modalWindowCanvasGroup.interactable = true; // 상호작용 가능하게 설정
    }

    public void HideModal()
    {
        // 모달 창을 숨김
        modalWindowCanvasGroup.alpha = 0f; // 투명
        modalWindowCanvasGroup.blocksRaycasts = false; // 레이캐스트 차단 비활성화
        modalWindowCanvasGroup.interactable = false; // 상호작용 불가능하게 설정
    }
}
