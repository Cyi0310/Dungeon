using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 類似二維的Slider功能，可用於平面的2維空間
/// </summary>
public class InputPanel : BaseEventTrigger
{
    public event Action<Vector2> OnDragHandler;
    public event Action OnPointerExitHandler;

    [SerializeField] private RectTransform eventTriggerRect;
    private Vector2 currentNormalizedPosition;
    public void Start()
    {
        RegisterEvent(EventTriggerType.Drag, Drag);
        RegisterEvent(EventTriggerType.PointerDown, PointerDown);
        RegisterEvent(EventTriggerType.PointerUp, Exit);
        RegisterEvent(EventTriggerType.PointerExit, Exit);
    }

    private void Drag(BaseEventData eventData)
    {
        if (eventData is PointerEventData pointerEventData)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(eventTriggerRect, pointerEventData.position))
            {
                UpdateNormalizedPosition(pointerEventData);
                OnDragHandler?.Invoke(currentNormalizedPosition);
            }
            else
            {
                OnPointerExitHandler?.Invoke();
                Debug.Log($"OnPointerExitHandler");
            }
        }
    }

    private void PointerDown(BaseEventData eventData)
    {
        if (eventData is PointerEventData pointerEventData)
        {
            UpdateNormalizedPosition(pointerEventData);
            OnDragHandler?.Invoke(currentNormalizedPosition);
        }
    }

    private void Exit(BaseEventData eventData)
    {
        OnPointerExitHandler?.Invoke();
        Debug.Log($"Exit");
    }

    private void UpdateNormalizedPosition(PointerEventData eventData)
    {
        Vector2 localPoint = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            eventTriggerRect,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint))
        {
            // 將本地座標正規化為 (0,0) 到 (1,1)
            Vector2 rectSize = eventTriggerRect.rect.size;
            currentNormalizedPosition = new Vector2(
                (localPoint.x / rectSize.x) + 0.5f,
                (localPoint.y / rectSize.y) + 0.5f
            );

            Debug.Log($"Normalized Position: {currentNormalizedPosition}");
        }
    }
}
