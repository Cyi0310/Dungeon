using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShield : MonoBehaviour, IEquippable
{
    public event Action<IDanger> OnTriggerEnterHandler;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDanger>(out var danger))
        {
            danger.Trigger();
        }
    }

    public void Equip(Hand hand)
    {
        //TODO: 動態裝備
        throw new NotImplementedException();
    }

    public void Unequip()
    {
        //TODO: 動態移除此裝備
        throw new NotImplementedException();
    }

    public Vector3 GetBasicPosition()
    {
        //TODO: 抓取盾牌原本的位置
        throw new NotImplementedException();
    }

    public Vector3 GetBasicRotation()
    {
        //TODO: 抓取盾牌原本的旋轉
        throw new NotImplementedException();
    }

    public Vector3 GetLerpPosition(Vector2 vector2)
    {
        //TODO: 抓取盾牌移動的位置
        throw new NotImplementedException();
    }

    public Vector3 GetLerpRotation(Vector2 vector2)
    {
        //TODO: 抓取盾牌旋轉的位置
        throw new NotImplementedException();
    }
}
