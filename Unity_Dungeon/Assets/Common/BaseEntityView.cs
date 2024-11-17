using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEntityView : MonoBehaviour
{
    public abstract void Init();

    public void Die()
    {
        Destroy(gameObject);
    }

}
