using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TileMgr;


public class CharacterView : BaseEntityView
{
    [SerializeField] private CharacterInput input;

    private readonly Observable<bool> isCanDoAction = new Observable<bool>();
    private Coroutine moveCoroutine;

    public Character Main { get; private set; }
 
    private TryGetTileDelegate TryGetTileHandler { get; set; }

    public override void Init()
    {

    }

    public void Init(/*Character data*/TryGetTileDelegate tryGetTileHandler)
    {
        TryGetTileHandler = tryGetTileHandler;

        Main = new Character();
        Main.SetData(0);

        var map = new Dictionary<ActiveType, Action>()
        {
            {ActiveType.Left, () => Shield() },
            {ActiveType.Front, () => moveCoroutine = StartCoroutine(Move()) },
            {ActiveType.Right, () => Attack() },
        };

        input.Init(map);
        isCanDoAction.ValueChanged += input.SwitchInteractable;
        isCanDoAction.Value = true;

    }

    public void Shield()
    {
        var targetIndex = Main.NowPosition + 1;
        if (!TryGetTileHandler(targetIndex, out var target))
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

    public IEnumerator Move()
    {
        Debug.Log($"View Move");

        isCanDoAction.Value = false;
        for (int i = 1; i <= Main.MainAability.WalkCount; i++)
        {
            var targetIndex = Main.NowPosition + i;
            if (!TryGetTileHandler(targetIndex, out var target))
            {
                break;
            }

            /**/
            yield return new WaitForSeconds(1f);
            transform.position = target.transform.position;
            /**/

            target.Execute();
            Main.Move();
        }
        isCanDoAction.Value = true;
    }

    public void Attack()
    {
        var targetIndex = Main.NowPosition + 1;
        if (!TryGetTileHandler(targetIndex, out var target))
        {
            return;
        }
        
        /*Triiger hit call this*/
        var entity = target.NowEntity;
        if (entity != null && entity is IHealth obj)
        {
            obj.OnHit(100);
        }

        Main.Attack();
        Debug.Log($"Attack");
    }

    public void ResetToDefault()
    {
        isCanDoAction.Value = true;
        Main.ResetToDefault();
        input.ResetToDefault();

        TryGetTileHandler(Main.NowPosition, out var target);
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
