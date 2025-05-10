using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInput : MonoBehaviour
{
    [SerializeField] private Button frontButtons;
    [SerializeField] private InputPanel leftInputPanel;
    [SerializeField] private InputPanel rightInputPanel;

    private Hand rightHand;
    private Hand leftHand;

    private bool CanRightMove
    {
        get { return !isRightWaving; }
    }

    private bool CanRightWaving
    {
        get { return !isRightWaving; }
    }

    private bool isRightWaving;

    private IReadOnlyDictionary<ActiveType, Button> buttonMap;

    public void Init(IReadOnlyDictionary<ActiveType, Action<Button>> handlerMap, Hand rightHand, Hand leftHand)
    {
        this.rightHand = rightHand;
        this.leftHand = leftHand;

        rightInputPanel.OnMoveHandler += (v2) => RightMove(v2);
        rightInputPanel.OnPointerExitHandler += () => RightCancellMove();
        rightInputPanel.OnPointerDownHandler += () => RightWaving().Forget();

        leftInputPanel.OnDragHandler += (v2) => LeftMove(v2);
        leftInputPanel.OnPointerExitHandler += () => LeftCancellMove();

        buttonMap = new Dictionary<ActiveType, Button>()
        {
            {ActiveType.Front, frontButtons },
        };

        foreach (var item in handlerMap)
        {
            if (buttonMap.TryGetValue(item.Key, out var button))
            {
                button.onClick.AddListener(() => item.Value(button));
            }
        }
        ResetToDefault();
    }

    private void RightMove(Vector2 v2)
    {
        if(!CanRightMove)
        {
            return;
        }
        rightHand.Move(v2).Forget();
    }

    private void RightCancellMove()
    {
        isRightWaving = false;
        rightHand.CancellMove().Forget();
    }

    private async UniTaskVoid RightWaving()
    {
        if (!CanRightWaving)
        {
            return;
        }

        isRightWaving = true;
        await rightHand.Waving();
        isRightWaving = false;
    }

    private void LeftMove(Vector2 v2)
    {
        leftHand.Move(v2).Forget();
    }

    private void LeftCancellMove()
    {
        leftHand.CancellMove().Forget();
    }

    public void SwitchInteractable(bool isCanDoAction, ActiveType activeType)
    {
        if (buttonMap.TryGetValue(activeType, out var button))
        {
            button.interactable = isCanDoAction;
        }
    }

    public void SwitchInteractable(bool isCanDoAction)
    {
        foreach (var item in buttonMap)
        {
            var button = item.Value;
            button.interactable = isCanDoAction;
        }
    }

    public void ResetToDefault()
    {
        //TODO: 把所有button active = true
    }
}
