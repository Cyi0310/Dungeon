using System;
using System.Collections.Generic;
using UnityEngine;

public class TileMgr : MonoBehaviour
{
    private TileSettings tileSettings;

    [SerializeField] private Transform root;
    //[SerializeField] private int totalTilesCount = 10;
    [SerializeField] private Transform basic;
    [SerializeField] private float distance = 15f;
    [SerializeField] private Tile finalTilePrefab;
    [SerializeField] private Tile tilePrefab;

    private IReadOnlyList<Tile> tiles;

    public delegate bool TryGetTileDelegate(int index, out Tile tile);
    public TryGetTileDelegate TryGetTileHandler { get; private set; }

    public event Action CharacterCanMoveHandler;

    public void Init(IReadOnlyDictionary<EntityType, GameObject> entityPrefabMap, TileSettings tileSettings)
    {
        this.tileSettings = tileSettings;
        tiles = SpawnTiles();

        TryGetTileHandler = TryGetTile;
        SpawnEntity(entityPrefabMap, out var entityViews);

        IEntity player = null;
        for (int i = 0; i < entityViews.Count; i++)
        {
            player = tileSettings.TileOfEntityTypes[i] == EntityType.Player ? entityViews[i].GetEntity() : null;
            if (player != null)
            {
                break;
            }
        }

        for (int i = 0; i < entityViews.Count; i++)
        {
            if (entityViews[i] != null)
            {
                var entity = entityViews[i].GetEntity();

                //避免Closure改值
                var index = i;
                entity.SetNowPosition(i);
                entity.OnDieHandler += () => EntityDie(player, index);
                tiles[i].SetNowEntityView(entityViews[i]);
            }
        }
    }

    private IReadOnlyList<Tile> SpawnTiles()
    {
        var tiles = new Tile[tileSettings.TileOfEntityTypes.Length];
        for (int i = 0; i < tileSettings.TileOfEntityTypes.Length; i++)
        {
            var offset = Vector3.forward * distance * i;
            var tile = i == tileSettings.TileOfEntityTypes.Length - 1 ? finalTilePrefab : tilePrefab;
            var tilePrefabObject = Instantiate(tile, basic.position + offset, basic.rotation, root);
            tiles[i] = tilePrefabObject;
        }
        return tiles;
    }

    private void SpawnEntity(IReadOnlyDictionary<EntityType, GameObject> entityPrefabMap, out IList<IBaseEntityView> tileEntityViews)
    {
        tileEntityViews = new List<IBaseEntityView>();
        for (int i = 0; i < tiles.Count; i++)
        {
            var entityType = tileSettings.TileOfEntityTypes[i];
            IBaseEntityView nowEntityView = null;

            if (entityPrefabMap.TryGetValue(entityType, out var entity))
            {
                nowEntityView = CreateEntity(entityType, entityPrefabMap[entityType], tiles[i].transform.position);
            }
            tileEntityViews.Add(nowEntityView);
        }
    }

    private IBaseEntityView CreateEntity(EntityType entityType, GameObject prefab, Vector3 position)
    {
        return entityType switch
        {
            EntityType.Player => CreateViewWithEntity<Character, CharacterView>(prefab, position),
            EntityType.Monster => CreateViewWithEntity<Monster, MonsterView>(prefab, position),
            _ => null
        };
    }

    private IBaseEntityView CreateViewWithEntity<TEntity, TView>(GameObject prefab, Vector3 position)
        where TEntity : IEntity, new()
        where TView : BaseEntityView<TEntity>
    {
        var prefabObject = Instantiate(prefab, position, Quaternion.identity);
        var view = prefabObject.GetComponent<TView>();
        var entity = new TEntity();
        view.SetEntity(entity);
        return view;
    }

    private void EntityDie(IEntity entity, int index)
    {
        if ((entity?.NowPosition ?? int.MinValue) + 1 == index)
        {
            CharacterCanMoveHandler.Invoke();
        }
        tiles[index].SetNowEntityView(null);
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
