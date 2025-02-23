using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "TileSettings", menuName = "ScriptableObjects/TileSettings") ]
public class TileSettings : ScriptableObject
{
    [field: SerializeField] public EntityType[] TileOfEntityTypes { get; private set; }

}
