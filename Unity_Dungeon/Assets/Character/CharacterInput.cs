using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterInput : MonoBehaviour
{
    [SerializeField] private Button frontButtons;
    //[SerializeField] private InputPanel leftInputPanel;
    [SerializeField] private InputPanel rightInputPanel;

    private IReadOnlyDictionary<ActiveType, Button> buttonMap;

    public void Init(IReadOnlyDictionary<ActiveType, Action<Button>> handlerMap, Action<Vector2> onValueChanged, Action onExit)
    {
        rightInputPanel.OnDragHandler += onValueChanged;
        rightInputPanel.OnPointerExitHandler += onExit;

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
