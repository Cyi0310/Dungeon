using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, ITile
{
    public IEntity NowEntity { get; protected set; }
    public virtual void SetNowEntity(IEntity nowEntity)
    {
        this.NowEntity = nowEntity;
    }

    public void Execute()
    {
        OnExecute();
    }

    protected virtual void OnExecute()
    { }
}
