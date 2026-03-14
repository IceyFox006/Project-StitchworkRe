using UnityEngine;

[RequireComponent(typeof(Animator))]
public class VFXGO : MonoBehaviour
{
    private Animator animator;

    private BattleManager bm;

    #region GS
    public Animator Animator { get => animator; set => animator = value; }
    #endregion

    public void Initialize(BattleManager bm)
    {
        this.bm = bm;

        animator = GetComponent<Animator>();
    }

    public void UseAction() //@UsedLocal_Animation
    {
        if (bm.CurAction == null) return;

        bm.CurAction.UseAction();
        bm.CurAction = null;
    }

    public void ShowBattleMenu()//@UsedLocal_Animation
    {
        bm.Ui.BattleMenu.Enable();
    }
}
