using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    [SerializeField] private Level level;
    [SerializeField] private EntityDatas entityDatas;
    void Start()
    {
        //var enums = typeof(EntityType).GetEnumNames();

        level.Init(entityDatas);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            level.ResetToDefault();
        }


    }
}
