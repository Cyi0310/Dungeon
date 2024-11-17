using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private CharacterView characterView;
    [SerializeField] private TileMgr tileMgr;

    private IReadOnlyDictionary<EntityType, SpawnEntityComponent> SpawnEntityComponentMap { get; set; }
    public void Init(EntityDatas entityDatas)
    {
        SpawnEntityComponentMap = new Dictionary<EntityType, SpawnEntityComponent>()
        {
          {EntityType.Empty, new SpawnEntityComponent(entityDatas.Empty ,  null)},

          //TODO: 之後優化成由這邊生成Character
          {EntityType.Player, new SpawnEntityComponent(entityDatas.Player , characterView.Main)},
          {EntityType.Monster, new SpawnEntityComponent(entityDatas.Monster, new Monster())},
        };
        tileMgr.Init(SpawnEntityComponentMap);
        characterView.Init(tileMgr.TryGetTileHandler);
    }

    public void ResetToDefault()
    {
        characterView.ResetToDefault();
    }

}
