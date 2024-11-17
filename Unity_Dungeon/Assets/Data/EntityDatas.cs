using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityDatas", menuName = "ScriptableObjects/Entity/EntityDatas")]
public class EntityDatas : ScriptableObject
{
    [field: SerializeField] public EntityData Empty{ get; private set; }
    [field: SerializeField] public EntityData Player { get; private set; }
    [field: SerializeField] public EntityData Monster{ get; private set; }

}

[System.Serializable]
public class EntityData
{
    [field: SerializeField] public EntityType EntityType { get; private set; }

    /*View 之後改繼承的VIEW*/
    [field: SerializeField] public BaseEntityView BaseEntityView { get; private set; }
}

