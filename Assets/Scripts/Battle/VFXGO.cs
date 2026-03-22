using UnityEngine;

[RequireComponent(typeof(Animator))]
public class VFXGO : MonoBehaviour
{
    private Animator animator;

    private BattleManager bm;
    private ActiveFighter actFighter;

    #region GS
    public Animator Animator { get => animator; set => animator = value; }
    #endregion

    public void Initialize(BattleManager bm, ActiveFighter actFighter)
    {
        this.bm = bm;
        this.actFighter = actFighter;

        animator = GetComponent<Animator>();
    }

    public void UpdateUI() //@UsedLocal_Animation
    {
        actFighter.Ui.UpdateHPVisuals(true);
    }
    public void PlayHurtAnimation() //@UsedLocal_Animation
    {
        if (actFighter.WasHurt)
            actFighter.Go.Animator.SetTrigger("HURT");
    }

    public void NextAction()//@UsedLocal_Animation
    {
        bm.Actions.NextAction();
    }
}
