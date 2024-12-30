using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private CharacterInput input;
    [SerializeField] private CharacterView characterView;
    [SerializeField] private TileMgr tileMgr;

    private IReadOnlyDictionary<EntityType, SpawnEntityComponent> SpawnEntityComponentMap { get; set; }
    public void Init(EntityDatas entityDatas)
    {
        characterView.Init(input);
        SpawnEntityComponentMap = new Dictionary<EntityType, SpawnEntityComponent>()
        {
          {EntityType.Empty, new SpawnEntityComponent(entityDatas.Empty.EntityViewPrefab,  null)},
          {EntityType.Player, new SpawnEntityComponent(characterView.gameObject, characterView.Main)},
          {EntityType.Monster, new SpawnEntityComponent(entityDatas.Monster.EntityViewPrefab, new Monster())},
        };
        tileMgr.Init(SpawnEntityComponentMap);
        tileMgr.CharacterCanMoveHandler += characterView.OnCanMove;
        characterView.TryGetTileHandler = tileMgr.TryGetTileHandler;
    }

    public void ResetToDefault()
    {
        characterView.ResetToDefault();
    }
}
