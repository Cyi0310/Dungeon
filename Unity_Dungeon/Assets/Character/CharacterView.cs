using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Button = UnityEngine.UI.Button;
using static TileMgr;
using UnityEngine.TextCore.Text;

public class CharacterView : BaseEntityView<Character>, IHealth
{
    private CharacterInput input;
    [field: SerializeField] public Transform Head { private set; get; } //TODO 然後再 ASSIGN WEAPON OR SHILED

    /*TODO: 之後改成用其他類別*/
    [SerializeField] private Hand rightHand; //TODO 然後再 ASSIGN WEAPON OR SHILED
    [SerializeField] private CharacterWeapon weapen;
    private CancellationTokenSource handMoveToken;

    [SerializeField] private ColliderController colliderController;

    /*TODO: 之後改成用其他類別*/
    //[SerializeField] private Hand leftHand; //TODO 然後再 ASSIGN WEAPON OR SHILED
    [SerializeField] private Hand leftHand;
    [SerializeField] private CharacterShield shield;

    private readonly Observable<bool> isCanDoMove = new Observable<bool>();
    private readonly Observable<bool> isCanDoAction = new Observable<bool>();
    private Coroutine moveCoroutine;


    public TryGetTileDelegate TryGetTileHandler { get; set; }

    //private Sequence sequence;


    //TODO Send Character data to here
    public void Init(CharacterInput input)
    {
        handMoveToken = new CancellationTokenSource();

        rightHand.Init();
        rightHand.SetEquippable(weapen);

        leftHand.Init();
        leftHand.SetEquippable(shield);
        this.input = input;

        //TODO: 之後可以考慮把行為拆成類別
        var map = new Dictionary<ActiveType, Action<Button>>()
        {
            //{ActiveType.Left, (button) => Shield(button) },
            {ActiveType.Front, (button) => Move(button).Forget() },
            //{ActiveType.Right, (button) => Attack(button).Forget() },
        };

        input.Init(map, rightHand, leftHand);
        isCanDoMove.ValueChanged += (b) => input.SwitchInteractable(b, ActiveType.Front);
        isCanDoMove.Value = true;

        isCanDoAction.ValueChanged += input.SwitchInteractable;
        isCanDoAction.Value = true;

        //sequence = DOTween.Sequence();

        weapen.OnTriggerEnterHandler += WeaponTriggerEntityView;

        colliderController.Init(this);
    }

    public override void SetEntity(Character entity)
    {
        Entity = entity;
    }

    public void Shield(Button button)
    {
        var targetIndex = Entity.NowPosition + 1;
        if (!TryGetTileHandler.Invoke(targetIndex, out var target))
        {
            return;
        }

        var entity = target.NowEntityView;
        if (entity is Monster monster)
        {
            /*todo shield*/
        }

        Entity.Shield();
        Debug.Log("View Shield");
    }

    public async UniTask Move(Button button)
    {
        Debug.Log($"View Move");
        handMoveToken.Cancel();
        handMoveToken = new CancellationTokenSource();
        isCanDoAction.Value = false;
        int targetIndex = 0;
        for (int i = 1; i <= Entity.MainAability.WalkCount; i++)
        {
            targetIndex = Entity.NowPosition + i;
            if (!TryGetTileHandler.Invoke(targetIndex, out var target) || target.NowEntityView != null)
            {
                isCanDoMove.Value = false;
                break;
            }

            await transform.DOMove(target.transform.position, 0.5f);

            //await transform.DOMove(,)
            transform.position = target.transform.position;

            target.Execute();
            Entity.Move();
        }

        /*Judge is front has item will inactive*/
        isCanDoAction.Value = true;

        targetIndex = Entity.NowPosition + 1;
        if (TryGetTileHandler.Invoke(targetIndex, out var tile) && tile.NowEntityView != null)
        {
            isCanDoMove.Value = false;
        }
    }

    public void OnCanMove()
    {
        isCanDoMove.Value = true;
        Debug.Log("OnCanMove");
    }

    private void WeaponTriggerEntityView(IHealth entityView)
    {
        entityView.OnHit(100);
        //if (entityView is IHealth enemy)
        //{
        //}
    }

    public bool OnHeal(int value)
    {
        return Entity.OnHeal(value);
    }

    public bool OnHit(int value)
    {
        return Entity.OnHit(value);
    }

    public void ResetToDefault()
    {
        isCanDoAction.Value = true;
        Entity.ResetToDefault();
        input.ResetToDefault();

        TryGetTileHandler.Invoke(Entity.NowPosition, out var target);
        transform.position = target.transform.position;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }

    public void Dispose()
    {
        isCanDoAction.ValueChanged -= input.SwitchInteractable;
        isCanDoAction.Dispose();
    }

}
