using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FighterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private Image _iconImage;

    [Header("Health")]
    [SerializeField] private GradientFill _hpFill;
    [SerializeField] private TMP_Text _hpText;

    [Header("Energy")]
    [SerializeField] private GradientFill _energyFill;
    [SerializeField] private TMP_Text _energyText;

    private BattleManager bm;
    private ActiveFighter actFighter;

    #region GS
    public ActiveFighter ActFighter { get => actFighter; set => actFighter = value; }
    #endregion

    public void Initialize(BattleManager bm, ActiveFighter actFighter)
    {
        this.bm = bm;
        this.actFighter = actFighter;

        _levelText.text = "Lv. " + actFighter.Data.Level;
        //_icon.sprite = actFighter.Data.Parts.Head.//Set icon to fighter head icon.

        _hpFill.Initialize();
        UpdateHPVisuals();
    }

    public void UpdateHPVisuals()
    {
        _hpFill.Fill(actFighter.Data.GetNormalizedHP());
        _hpText.text = actFighter.Data.CurHP + " / " + actFighter.Data.MaxHP;
    }
    public void UpdateEnergyVisuals()
    {
        _energyFill.Fill(actFighter.Data.GetNormalizedEnergy());
        _energyText.text = actFighter.Data.CurEnergy + " / " + actFighter.Data.MaxEnergy;
    }
}
