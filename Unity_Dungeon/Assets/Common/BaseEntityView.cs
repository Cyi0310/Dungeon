using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEntityView<TEntity> : MonoBehaviour, IBaseEntityView where TEntity : IEntity
{
    protected TEntity Entity { get; set; }
    public abstract void SetEntity(TEntity entity);
    public IEntity GetEntity() => Entity;
}
