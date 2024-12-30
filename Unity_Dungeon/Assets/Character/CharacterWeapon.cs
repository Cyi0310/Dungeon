using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CharacterWeapon : MonoBehaviour, IEquippable
{
    [SerializeField] private Transform[] transforms;
    public event Action<IHealth> OnTriggerEnterHandler;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<MonsterView>(out var entityView))
        {
            if (entityView is IHealth enemy)
            {
                OnTriggerEnterHandler?.Invoke(enemy);
            }
        }
    }

    public Vector3 GetBasicPosition()
    {
        return transforms[4].transform.position;
    }

    public Vector3 GetBasicRotation()
    {
        return transforms[4].rotation.eulerAngles;
    }

    public Vector3 GetLerpPosition(Vector2 vector2)
    {
        var bottomLeft = transforms[0];
        var bottomRight = transforms[1];
        var topLeft = transforms[2];
        var topRight = transforms[3];

        float x = vector2.x;
        float y = vector2.y;

        Vector2 left = Vector2.Lerp(bottomLeft.position, topLeft.position, y);
        Vector2 right = Vector2.Lerp(bottomRight.position, topRight.position, y);
        Vector2 lerpPosition = Vector2.Lerp(left, right, x);
        return new Vector3(lerpPosition.x, lerpPosition.y, transforms[4].position.z);
    }

    public Vector3 GetLerpRotation(Vector2 vector2)
    {
        var bottomLeft = transforms[0];
        var bottomRight = transforms[1];
        var topLeft = transforms[2];
        var topRight = transforms[3];

        float x = vector2.x;
        float y = vector2.y;

        var left = Quaternion.Lerp(bottomLeft.rotation, topLeft.rotation, y);
        var right = Quaternion.Lerp(bottomRight.rotation, topRight.rotation, y);
        Quaternion lerpRotation = Quaternion.Lerp(left, right, x);
        return lerpRotation.eulerAngles;
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
}
