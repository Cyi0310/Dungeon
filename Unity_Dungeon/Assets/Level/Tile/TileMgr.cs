using System;
using System.Collections.Generic;
using UnityEngine;

public class TileMgr : MonoBehaviour
{
    [SerializeField] private Transform root;
    //[SerializeField] private int totalTilesCount = 10;
    [SerializeField] private Transform basic;
    [SerializeField] private float distance = 15f;
    [SerializeField] private Tile tilePrefab;
    [field: SerializeField] public EntityType[] TileOfEntityTypes { get; private set; }
    private IReadOnlyList<Tile> tiles;

    public delegate bool TryGetTileDelegate(int index, out Tile tile);
    public TryGetTileDelegate TryGetTileHandler { get; private set; }

    public event Action CharacterCanMoveHandler;
    private IEntity player;

    public void Init(IReadOnlyDictionary<EntityType, SpawnEntityComponent> spawnEntityComponentMap)
    {
        tiles = SpawnTiles();

        TryGetTileHandler = TryGetTile;
        SpawnEntity(spawnEntityComponentMap, out var entitys);
        for (int i = 0; i < entitys.Count; i++)
        {
            tiles[i].SetNowEntity(entitys[i]);

            if (entitys[i] != null)
            {
                //避免Closure改值
                var index = i;
                entitys[i].OnDieHandler += () => EntityDie(index);
            }
        }
    }

    private IReadOnlyList<Tile> SpawnTiles()
    {
        var tiles = new Tile[TileOfEntityTypes.Length];
        for (int i = 0; i < TileOfEntityTypes.Length; i++)
        {
            var offset = Vector3.forward * distance * i;
            var tile = Instantiate(tilePrefab, basic.position + offset, basic.rotation, root);
            tiles[i] = tile;
        }
        return tiles;
    }

    private void SpawnEntity(IReadOnlyDictionary<EntityType, SpawnEntityComponent> spawnEntityComponentMap, out IList<IEntity> tileEntitys)
    {
        tileEntitys = new List<IEntity>();
        for (int i = 0; i < tiles.Count; i++)
        {
            var entityType = TileOfEntityTypes[i];
            IEntity nowEntity = null;
            if (spawnEntityComponentMap.TryGetValue(entityType, out var entity))
            {
                if (entity.EntityViewPrefab != null)
                {
                    if (entityType == EntityType.Player)
                    {
                        nowEntity = entity.TileEntity;
                        player = nowEntity;
                    }
                    else if (entityType == EntityType.Monster)
                    {
                        var prefab = Instantiate(entity.EntityViewPrefab, tiles[i].transform.position, Quaternion.identity);
                        var view = prefab.GetComponent<MonsterView>();
                        var monster = new Monster();
                        nowEntity = monster;
                        view.SetEntity(monster);
                    }
                    else
                    {
                        nowEntity = entity.TileEntity;
                    }
                }
            }
            tileEntitys.Add(nowEntity);
        }
    }

    private void EntityDie(int index)
    {
        if (player.NowPosition + 1 == index)
        {
            CharacterCanMoveHandler.Invoke();
        }
        tiles[index].SetNowEntity(null);
    }

    private bool TryGetTile(int index, out Tile tile)
    {
        if (index >= tiles.Count)
        {
            tile = null;
            return false;
        }
        tile = tiles[index];
        return true;
    }
}
