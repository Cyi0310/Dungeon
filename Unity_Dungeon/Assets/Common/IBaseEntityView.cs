using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseEntityView
{
    IEntity GetEntity();
    void Dispose();
}
