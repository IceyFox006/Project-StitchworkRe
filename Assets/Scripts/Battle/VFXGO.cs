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
        actFighter.Ui.UpdateHPVisuals();
    }

    public void NextAction()//@UsedLocal_Animation
    {
        if (bm.Actions.List.Count > 0)
            bm.Actions.UseFirstAction();
        else
            bm.Ui.BattleMenu.Enable();
    }
}
