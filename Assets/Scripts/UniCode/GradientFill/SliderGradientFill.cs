using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderGradientFill : GradientFill
{
    private Slider slider;
    private Image fillImage;

    public override void Initialize()
    {
        base.Initialize();
        slider = GetComponent<Slider>();
        fillImage = slider.fillRect.GetComponent<Image>();
    }
    public override void Fill(float amount)
    {
        base.Fill(amount);
        fillAmount = amount;

        slider.value = amount;
        fillImage.color = _gradient.Evaluate(amount);
    }
}
