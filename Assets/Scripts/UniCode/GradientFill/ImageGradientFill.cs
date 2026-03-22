using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageGradientFill : GradientFill
{
    private Image image;

    public override void Initialize()
    {
        base.Initialize();
        image = GetComponent<Image>();
    }

    public override void Fill(float amount)
    {
        base.Fill(amount);
        fillAmount = amount;

        image.fillAmount = amount;
        image.color = _gradient.Evaluate(amount);
    }
}
