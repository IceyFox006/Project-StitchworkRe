using System.Collections;
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

    //Waits for the VFX clip to finish playing then updates HP UI.
    public IEnumerator UpdateHpUiWait(float time)
    {
        yield return new WaitForSeconds(time);
        actFighter.Ui.UpdateHPVisuals(true);
    }

    public void PlayHurtAnimation() //@UsedLocal_Animation
    {
        if (actFighter.WasHurt)
            actFighter.Go.Animator.SetTrigger("HURT");
    }
}
