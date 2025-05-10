using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private float moveDuration = 0.25f;
    [SerializeField] private float swingingDuration = 0.05f;

    [SerializeField] private Vector3 swingOffset;

    private IEquippable equippable;
    private CancellationTokenSource handMoveCTS;
    public void Init()
    { }

    public void SetEquippable(IEquippable equippable)
    {
        this.equippable = equippable;
    }

    public async UniTaskVoid Move(Vector2 value)
    {
        handMoveCTS?.Cancel();
        handMoveCTS = new CancellationTokenSource();

        await UniTask.WhenAll
        (
            transform.DOMove(equippable.GetLerpPosition(value), moveDuration)
                        .ToUniTask(cancellationToken: handMoveCTS.Token),

            transform.DORotate(equippable.GetLerpRotation(value), moveDuration)
                        .ToUniTask(cancellationToken: handMoveCTS.Token)
        );
    }

    public async UniTask Waving()
    {
        handMoveCTS?.Cancel();
        handMoveCTS = new CancellationTokenSource();

        float elapsed = 0f;
        Quaternion startRotation = transform.localRotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(swingOffset);

        Vector3 startLocalPos = transform.localPosition;
        Vector3 targetLocalPos = startLocalPos + new Vector3(0, 0, 0.3f);

        while (elapsed < swingingDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / swingingDuration);
            transform.localRotation = Quaternion.Slerp(startRotation, endRotation, t);

            Vector3 lerpPos = Vector3.Lerp(startLocalPos, targetLocalPos, t);
            float swingArc = Mathf.Sin(t * Mathf.PI) * 0.1f;
            lerpPos += new Vector3(0, swingArc, 0);
            transform.localPosition = lerpPos;

            await UniTask.Yield(cancellationToken: handMoveCTS.Token);
        }
        transform.localRotation = endRotation;
        transform.localPosition = targetLocalPos;
    }

    public UniTask CancellMove()
    {
        handMoveCTS?.Cancel();
        handMoveCTS = new CancellationTokenSource();

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(equippable.GetBasicRotation());

        return UniTask.WhenAll(
            transform.DOMove(equippable.GetBasicPosition(), moveDuration)
                     .SetEase(Ease.InOutQuad)
                     .ToUniTask(cancellationToken: handMoveCTS.Token),

            transform.DORotateQuaternion(targetRotation, moveDuration)
                     .SetEase(Ease.InOutQuad)
                     .ToUniTask(cancellationToken: handMoveCTS.Token)
        );
    }

    public void Dispose()
    {
        handMoveCTS.Cancel();
        handMoveCTS.Dispose();
    }
}
