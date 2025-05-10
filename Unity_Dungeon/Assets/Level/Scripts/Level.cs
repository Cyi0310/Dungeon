using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Level : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cvCamera;
    [SerializeField] private TileSettings tileSettings;
    [SerializeField] private CharacterInput input;

    [SerializeField] private TileMgr tileMgr;
    public void Init(EntityDatas entityDatas)
    {
        tileMgr.Init(entityDatas.EntityPrefabMap, tileSettings);
        SetCharacterDatas(tileSettings.TileOfEntityTypes);
    }

    private void SetCharacterDatas(EntityType[] tileOfEntityTypes)
    {
        for (int i = 0; i < tileOfEntityTypes.Length; i++)
        {
            if (tileMgr.TryGetTileHandler(i, out Tile tile))
            {
                var entityView = tile.NowEntityView;

                switch (tileOfEntityTypes[i])
                {
                    case EntityType.Empty:
                        break;
                    case EntityType.Player:
                        SetPlayerData(entityView);
                        break;
                    case EntityType.Monster:
                        SetMonsterData(entityView);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void SetPlayerData(IBaseEntityView view)
    { 
        if (view is not CharacterView characterView)
        {
            return;
        }

        characterView.Init(input);
        tileMgr.CharacterCanMoveHandler += characterView.OnCanMove;
        characterView.TryGetTileHandler = tileMgr.TryGetTileHandler;
        cvCamera.Follow = characterView.Head;
    }

    private void SetMonsterData(IBaseEntityView view)
    {
        if (view is not MonsterView monsterView)
        {
            return;
        }

    }

    public void ResetToDefault()
    {
        //For each call all view reset to default
        //characterView.ResetToDefault();
    }

    public void Dispose()
    {
        tileMgr.Dispose();
    }
}
