using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FighterGO : MonoBehaviour
{
    private Animator animator;
    private ButtonObject button;
    private PartAssemble partAssemble;
    private VFXGO vfx;

    private BattleManager bm;
    private ActiveFighter actFighter;

    #region GS
    public ButtonObject Button { get => button; set => button = value; }
    public Animator Animator { get => animator; set => animator = value; }
    public VFXGO Vfx { get => vfx; set => vfx = value; }
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

        vfx = GetComponentInChildren<VFXGO>();
        vfx.Initialize(bm);
    }

    public void SelectAsTarget() //@UsedLocal_ObjectButton
    {
        bm.CurAction.AddTarget(actFighter);
    }
    public void PlayVFXAnimation() //@UsedLocal_Animation
    {
        Animator tempAnimator;
        foreach (ActiveFighter target in bm.CurAction.Targets)
        {
            tempAnimator = target.Go.Vfx.Animator;
            tempAnimator.runtimeAnimatorController = bm.CurAction.Action.VfxAc;
            tempAnimator.Play("PLAY");
        }
        Debug.Log("H");
        //vfx.Animator.Play("PLAY");
    }
}
