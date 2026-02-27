using TMPro;
using UnityEngine;

public class FighterUI : MonoBehaviour
{
    [SerializeField] private GradientFill _hpFill;
    [SerializeField] private TMP_Text _hpText;

    [HideInInspector]
    public void UpdateHPVisuals(float amount)
    {
        _hpFill.Fill(amount);
        //hp text;
    }
}
