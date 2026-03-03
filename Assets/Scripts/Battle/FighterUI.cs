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

    [HideInInspector]
    public void Initialize()
    {

    }

    [HideInInspector]
    public void UpdateHPVisuals(float amount)
    {
        _hpFill.Fill(amount);
        _hpText.text = fighter.CurrentHP + " / " + fighter.MaxHP;
    }
}
