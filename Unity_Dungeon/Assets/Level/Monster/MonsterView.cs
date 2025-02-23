using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

public class MonsterView : BaseEntityView<Monster>, IHealth
{
    [SerializeField] private Transform hand;
    [SerializeField] private Weapon weapon;

    private CancellationTokenSource swagWeaponToken;

    public override void SetEntity(Monster character)
    {
        weapon.Init(gameObject, 10);
        weapon.OnTriggerEvent += () => TriggerEvent().Forget();

        Entity = character;
        Entity.OnDieHandler += Die;

        swagWeaponToken = new CancellationTokenSource();
        SwagWeapon(swagWeaponToken.Token).Forget();
    }

    private async UniTask TriggerEvent()
    {
        swagWeaponToken.Cancel();
        swagWeaponToken = new CancellationTokenSource();

        weapon.SetColliderEnable(false);
        await SwagWeaponBackToBasic(swagWeaponToken.Token);
        weapon.SetColliderEnable(true);

        SwagWeapon(swagWeaponToken.Token).Forget();
    }

    private async UniTask SwagWeapon(CancellationToken token)
    {
        var delayStartDuration = Random.Range(0.1f, 1.15f);
        await UniTask.WaitForSeconds(delayStartDuration, cancellationToken: token);
        while (!token.IsCancellationRequested)
        {
            var tween1 = hand.transform.DORotate(Vector3.left * 180f, 1);
            await tween1.ToUniTask(cancellationToken: token);

            var tween2 = hand.transform.DORotate(Vector3.zero, 1);
            await tween2.ToUniTask(cancellationToken: token);
        }
    }

    private async UniTask SwagWeaponBackToBasic(CancellationToken token)
    {
        var tween = hand.transform.DORotate(Vector3.left * -40f, 1);
        await tween.ToUniTask(cancellationToken: token);
    }

    public bool OnHeal(int value)
    {
        return Entity.OnHeal(value);
    }

    public bool OnHit(int value)
    {
        return Entity.OnHit(value);
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
