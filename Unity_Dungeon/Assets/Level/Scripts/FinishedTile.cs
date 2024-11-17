using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedTile : Tile
{
    protected override void OnExecute()
    {
        Debug.Log("Finished Game");
    }
}
