using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquippable
{
    Vector3 GetBasicPosition();
    Vector3 GetBasicRotation();
    Vector3 GetLerpPosition(Vector2 vector2);
    Vector3 GetLerpRotation(Vector2 vector2);

    void Equip(Hand hand);  // 動態更換裝備
    void Unequip();         // 卸下

}
