using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEntityView<TEntity> : MonoBehaviour where TEntity : IEntity
{
    public abstract void SetEntity(TEntity entity);
}
