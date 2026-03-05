using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GradientFill : MonoBehaviour
{
    private Image image;

    [SerializeField] private Gradient _gradient;
    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        image = GetComponent<Image>();
    }

    [HideInInspector]
    public void Fill(float amount)
    {
        image.fillAmount = amount;
        image.color = _gradient.Evaluate(amount);
    }
}
