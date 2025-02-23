using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityDatas", menuName = "ScriptableObjects/Entity/EntityDatas")]
public class EntityDatas : ScriptableObject
{
    [SerializedDictionary("EntityType", "Prefab")]
    [field:SerializeField] public SerializedDictionary<EntityType, GameObject> EntityPrefabMap { private set; get; }
}
