using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameTool
{
    public static IEnumerator WaitRunAction(IEnumerable<WaitDoAction> enumerable)
    {
        var enumerator = enumerable.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;
            var waitDuration = current.WaitDuration;
            var action = current.Action;
            yield return new WaitForSeconds(waitDuration);
            action.Invoke();
        }
    }

    public static IEnumerator WaitRunActionPerFrame(IEnumerable<WaitDoAction> enumerable)
    {
        var enumerator = enumerable.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;
            var waitDuration = current.WaitDuration;
            var action = current.Action;

            var t = 0f;
            while (t <= waitDuration)
            {
                action.Invoke();
                yield return null;
                t += Time.deltaTime;
            }
        }
    }
}
