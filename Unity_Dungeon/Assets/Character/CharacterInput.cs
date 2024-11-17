using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInput : MonoBehaviour
{
    [SerializeField] private Button leftButtons;
    [SerializeField] private Button frontButtons;
    [SerializeField] private Button rightButtons;

    private IReadOnlyDictionary<ActiveType, Button> buttonMap;

    private Coroutine clickRoutine;

    public void Init(IReadOnlyDictionary<ActiveType, Action> handlerMap)
    {
        buttonMap = new Dictionary<ActiveType, Button>()
        {
            {ActiveType.Left, leftButtons },
            {ActiveType.Front, frontButtons },
            {ActiveType.Right, rightButtons},
        };

        foreach (var item in handlerMap)
        {
            if (buttonMap.TryGetValue(item.Key, out var button))
            {
                button.onClick.AddListener(() => item.Value());
            }
        }
        ResetToDefault();
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
        //TODO: §â©Ò¦³button active = true
    }


}
