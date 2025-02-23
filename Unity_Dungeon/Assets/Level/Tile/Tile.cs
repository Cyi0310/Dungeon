using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, ITile
{
    public IBaseEntityView NowEntityView { get; protected set; }
    public virtual void SetNowEntityView(IBaseEntityView nowEntityView)
    {
        this.NowEntityView = nowEntityView;
    }

    public void Execute()
    {
        OnExecute();
    }

    protected virtual void OnExecute()
    { }
}
