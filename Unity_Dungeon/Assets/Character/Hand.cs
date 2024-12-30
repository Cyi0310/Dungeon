using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private float delayDuration = 0.25f;
    private CancellationTokenSource handMoveToken;

    public IEquippable equippable;
    public void Init()
    {
        handMoveToken = new CancellationTokenSource();
    }

    public void SetEquippable(IEquippable equippable)
    {
        this.equippable = equippable;
    }

    public async void Move(Vector2 value)
    {
        handMoveToken.Cancel();
        handMoveToken = new CancellationTokenSource();

        await UniTask.WhenAll
        (
            transform.DOMove(equippable.GetLerpPosition(value), delayDuration)
                        .ToUniTask(cancellationToken: handMoveToken.Token),

            transform.DORotate(equippable.GetLerpRotation(value), delayDuration)
                        .ToUniTask(cancellationToken: handMoveToken.Token)
        );
    }

    public async void CancellMove()
    {
        handMoveToken.Cancel();
        handMoveToken = new CancellationTokenSource();

        float elapsed = 0f;
        Vector3 startPosition = transform.position;
        Vector3 startRotation = transform.rotation.eulerAngles;

        while (elapsed < delayDuration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, equippable.GetBasicPosition(), elapsed / delayDuration);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(startRotation, equippable.GetBasicRotation(), elapsed / delayDuration));
            await UniTask.Yield();
        }
    }


}
