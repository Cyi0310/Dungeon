using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MonsterView : BaseEntityView<Monster>, IHealth
{
    [SerializeField] private Transform hand;
    [SerializeField] private Weapon weapon;

    public override void SetEntity(Monster character)
    {
        weapon.Init(gameObject, 10);

        /*TODO: 可以在Base class 用 generic，但序列化關聯沒辦法使用，暫時解法轉型*/
        Entity = character;
        Entity.OnDieHandler += Die;

        SwagWeapon().Forget();
    }

    private async UniTask SwagWeapon()
    {
        var delayStartDuration = Random.Range(0.1f, 1.15f);
        await UniTask.WaitForSeconds(delayStartDuration);
        while (true)
        {
            await hand.transform.DORotate(Vector3.left * 180f, 1);
            await hand.transform.DORotate(Vector3.zero, 1);
        }
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
