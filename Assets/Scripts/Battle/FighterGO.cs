using UnityEngine;

public class FighterGO : MonoBehaviour
{
    private ButtonObject button;
    private PartAssemble partAssemble;

    private BattleManager bm;
    private ActiveFighter actFighter;

    #region GS
    public ButtonObject Button { get => button; set => button = value; }
    #endregion

    public void Initialize(BattleManager bm, ActiveFighter actFighter)
    {
        this.bm = bm;
        this.actFighter = actFighter;

        partAssemble = GetComponentInChildren<PartAssemble>();
        partAssemble.Initialize(actFighter.Data.Parts, actFighter.Data.Palettes);

        button = GetComponentInChildren<ButtonObject>();
        button.Initialize();
    }

    public void SelectAsTarget() //@UsedLocal
    {
        bm.CurAction.AddTarget(actFighter);
    }

    public void UseAction() //@UsedLocal
    {
        if (bm.CurAction == null) return;

        bm.CurAction.UseAction();
        bm.CurAction = null;
    }
}
