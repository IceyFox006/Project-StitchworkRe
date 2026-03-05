using TMPro;
using UnityEngine;

public class FighterUI : MonoBehaviour
{
    private Fighter fighter;

    [SerializeField] private GradientFill _hpFill;
    [SerializeField] private TMP_Text _hpText;

    #region GS
    public Fighter Fighter { get => fighter; set => fighter = value; }
    #endregion

    public void Initialize(Fighter fighter)
    {
        this.fighter = fighter;

        _hpFill.Initialize();
        UpdateHPVisuals();
    }

    public void UpdateHPVisuals()
    {
        _hpFill.Fill(fighter.GetNormalizedHP());
        _hpText.text = fighter.CurrentHP + " / " + fighter.MaxHP;
    }
}
