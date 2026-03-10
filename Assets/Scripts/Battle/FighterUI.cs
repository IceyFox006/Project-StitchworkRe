using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FighterUI : MonoBehaviour
{
    [SerializeField] private Image _icon;

    [SerializeField] private GradientFill _hpFill;
    [SerializeField] private TMP_Text _hpText;

    private BattleManager bm;
    private ActiveFighter actFighter;

    #region GS
    public ActiveFighter ActFighter { get => actFighter; set => actFighter = value; }
    #endregion

    public void Initialize(BattleManager bm, ActiveFighter actFighter)
    {
        this.bm = bm;
        this.actFighter = actFighter;

        //_icon.sprite = actFighter.Data.Parts.Head.//Set icon to fighter head icon.

        _hpFill.Initialize();
        UpdateHPVisuals();
    }

    public void UpdateHPVisuals()
    {
        _hpFill.Fill(actFighter.Data.GetNormalizedHP());
        _hpText.text = actFighter.Data.CurrentHP + " / " + actFighter.Data.MaxHP;
    }
}
