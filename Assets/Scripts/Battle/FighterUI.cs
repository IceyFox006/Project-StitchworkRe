using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class FighterUI : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private Image _iconImage;

    [SerializeField] private float _fillUpdateInterval;

    [Header("Health")]
    [SerializeField] private SliderGradientFill _hpFill;
    [SerializeField] private TMP_Text _hpText;

    [Header("Energy")]
    [SerializeField] private SliderGradientFill _energyFill;
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

        animator = GetComponent<Animator>();

        _levelText.text = "Lv. " + actFighter.Data.Level;
        //_icon.sprite = actFighter.Data.Parts.Head.//Set icon to fighter head icon.

        _hpFill.Initialize();
        UpdateHPVisuals();

        _energyFill.Initialize();
        UpdateEnergyVisuals();
    }
    public void OnKill()
    {
        animator.Play("DIE");
    }
    public void OnWin()
    {
        animator.Play("WIN");
    }

    public void UpdateHPVisuals(bool doNextAction = false)
    {
        if (doNextAction)
            bm.StartNextActionWait(_hpFill.Duration);

        if (actFighter.Data.CurHP == 0)
            _hpFill.OnSlowFillFinish.AddListener(() => {actFighter.Die();});

        _hpFill.StartSlowFill(actFighter.Data.GetNormalizedHP());
        _hpText.text = actFighter.Data.CurHP + " / " + actFighter.Data.MaxHP;
    }
    public void UpdateEnergyVisuals()
    {
        _energyFill.StartSlowFill(actFighter.Data.GetNormalizedEnergy());
        _energyText.text = actFighter.Data.CurEnergy + " / " + actFighter.Data.MaxEnergy;
    }
}
