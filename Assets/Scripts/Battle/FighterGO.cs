using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FighterGO : MonoBehaviour
{
    private Animator animator;
    private ButtonObject button;
    private PartAssemble partAssemble;

    private BattleManager bm;
    private ActiveFighter actFighter;

    #region GS
    public ButtonObject Button { get => button; set => button = value; }
    public Animator Animator { get => animator; set => animator = value; }
    #endregion

    public void Initialize(BattleManager bm, ActiveFighter actFighter)
    {
        this.bm = bm;
        this.actFighter = actFighter;

        animator = GetComponent<Animator>();
        partAssemble = GetComponentInChildren<PartAssemble>();
        partAssemble.Initialize(actFighter.Data.Parts, actFighter.Data.Palettes);

        button = GetComponentInChildren<ButtonObject>();
        button.Initialize();
    }

    public void SelectAsTarget() //@UsedLocal_ObjectButton
    {
        bm.CurAction.AddTarget(actFighter);
    }

    public void UseAction() //@UsedLocal_Animation
    {
        bm.CurAction.UseAction();
    }
}
