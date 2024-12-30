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

    /*TODO: 之後改成用其他類別*/
    [SerializeField] private Hand rightHand; //TODO 然後再 ASSIGN WEAPON OR SHILED
    [SerializeField] private CharacterWeapon weapen;
    private CancellationTokenSource handMoveToken;

    [SerializeField] private ColliderController colliderController;

    /*TODO: 之後改成用其他類別*/
    //[SerializeField] private Hand leftHand; //TODO 然後再 ASSIGN WEAPON OR SHILED
    [SerializeField] private Transform lHand;
    [SerializeField] private CharacterShield shield;

    private readonly Observable<bool> isCanDoMove = new Observable<bool>();
    private readonly Observable<bool> isCanDoAction = new Observable<bool>();
    private Coroutine moveCoroutine;


    public TryGetTileDelegate TryGetTileHandler { get; set; }

    //private Sequence sequence;

    public Character Main { get; private set; }

    //TODO Send Character data to here
    public void Init(CharacterInput input)
    {
        SetEntity(new Character());
        handMoveToken = new CancellationTokenSource();

        rightHand.Init();
        rightHand.SetEquippable(weapen);
        this.input = input;

        //TODO: 之後可以考慮把行為拆成類別
        var map = new Dictionary<ActiveType, Action<Button>>()
        {
            //{ActiveType.Left, (button) => Shield(button) },
            {ActiveType.Front, (button) => Move(button).Forget() },
            //{ActiveType.Right, (button) => Attack(button).Forget() },
        };

        input.Init(map, rightHand.Move, rightHand.CancellMove);
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
        Main = entity;
        Main.SetData(0);
    }

    public void Shield(Button button)
    {
        var targetIndex = Main.NowPosition + 1;
        if (!TryGetTileHandler.Invoke(targetIndex, out var target))
        {
            return;
        }

        var entity = target.NowEntity;
        if (entity is Monster monster)
        {
            /*todo shield*/
        }

        Main.Shield();
        Debug.Log("View Shield");
    }

    public async UniTask Move(Button button)
    {
        Debug.Log($"View Move");
        handMoveToken.Cancel();
        handMoveToken = new CancellationTokenSource();
        isCanDoAction.Value = false;
        int targetIndex = 0;
        for (int i = 1; i <= Main.MainAability.WalkCount; i++)
        {
            targetIndex = Main.NowPosition + i;
            if (!TryGetTileHandler.Invoke(targetIndex, out var target) || target.NowEntity != null)
            {
                isCanDoMove.Value = false;
                break;
            }

            await transform.DOMove(target.transform.position, 0.5f);

            //await transform.DOMove(,)
            transform.position = target.transform.position;

            target.Execute();
            Main.Move();
        }

        /*Judge is front has item will inactive*/
        isCanDoAction.Value = true;

        targetIndex = Main.NowPosition + 1;
        if (TryGetTileHandler.Invoke(targetIndex, out var tile) && tile.NowEntity != null)
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
        return Main.OnHeal(value);
    }

    public bool OnHit(int value)
    {
        return Main.OnHit(value);
    }

    public void ResetToDefault()
    {
        isCanDoAction.Value = true;
        Main.ResetToDefault();
        input.ResetToDefault();

        TryGetTileHandler.Invoke(Main.NowPosition, out var target);
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
