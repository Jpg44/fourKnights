using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomScrollRect : ScrollRect
{
    // 마우스 휠 입력만 처리하도록 OnScroll 메서드를 오버라이드
    public override void OnScroll(PointerEventData data)
    {
        if (data.scrollDelta.y != 0)
        {
            base.OnScroll(data);
        }
    }

    // 키보드 입력에 의한 스크롤을 무시하도록 OnInitializePotentialDrag를 오버라이드
    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
        // 이 메서드를 비워두면 키보드 입력에 의한 스크롤 준비가 무시됩니다.
    }

    // 드래그 시작 이벤트를 무시
    public override void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그를 시작할 때 마우스 왼쪽 버튼이 아니면 무시
    }

    // 드래그 동안 이벤트를 무시
    public override void OnDrag(PointerEventData eventData)
    {
        // 드래그 중 마우스 왼쪽 버튼이 아니면 무시
    }

    // 드래그 끝 이벤트를 무시
    public override void OnEndDrag(PointerEventData eventData)
    {
        // 드래그가 끝났을 때 아무런 동작도 하지 않음
    }
}
