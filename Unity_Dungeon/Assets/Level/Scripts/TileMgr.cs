using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileMgr : MonoBehaviour
{
    //[SerializeField] private int totalTilesCount = 10;
    [field: SerializeField] public EntityType[] TileOfEntityTypes { get; private set; }
    [SerializeField] private Tile[] tiles;

    private List<Tile> nowTiles;

    public delegate bool TryGetTileDelegate(int index, out Tile tile);
    public TryGetTileDelegate TryGetTileHandler { get; private set; }

    public void Init(IReadOnlyDictionary<EntityType, SpawnEntityComponent> spawnEntityComponentMap)
    {
        if (tiles.Length != TileOfEntityTypes.Length)
        {
            throw new Exception($"tiles.Length: {tiles.Length} != tileOfEntityTypes.Length: {TileOfEntityTypes.Length}");
        }

        TryGetTileHandler = TryGetTile;
        SpawnEntity(spawnEntityComponentMap, out var entitys);
        for (int i = 0; i < entitys.Count; i++)
        {
            tiles[i].SetNowEntity(entitys[i]);

            if (entitys[i] != null)
            {
                //Á×§KClosure§ï­È
                var index = i;
                entitys[i].OnDieHandler += () => EntityDie(index);
            }
        }
    }

    private void SpawnEntity(IReadOnlyDictionary<EntityType, SpawnEntityComponent> spawnEntityComponentMap, out IList<IEntity> tileEntitys)
    {
        tileEntitys = new List<IEntity>();
        for (int i = 0; i < tiles.Length; i++)
        {
            var entityType = TileOfEntityTypes[i];
            IEntity nowEntity = null;
            if (spawnEntityComponentMap.TryGetValue(entityType, out var entity))
            {
                if (entity.EntityData.BaseEntityView != null)
                {
                    if (entityType == EntityType.Player)
                    {
                        nowEntity = entity.tileEntity;
                    }
                    else if (entityType == EntityType.Monster)
                    {
                        var view = Instantiate(entity.EntityData.BaseEntityView, tiles[i].transform.position, Quaternion.identity);
                        view.Init();
                        nowEntity = new Monster();
                        nowEntity.SetView(view);
                    }
                    else
                    {
                        nowEntity = entity.tileEntity;
                    }
                }
            }
            tileEntitys.Add(nowEntity);
        }
    }

    private void EntityDie(int index)
    {
        tiles[index].SetNowEntity( null);
    }

    private bool TryGetTile(int index, out Tile tile)
    {
        if (index >= tiles.Length)
        {
            tile = null;
            return false;
        }
        tile = tiles[index];
        return true;
    }
}
