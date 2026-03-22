using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GradientFill : MonoBehaviour
{
    [SerializeField] 
        protected Gradient _gradient;

    [Header("Slow Fill")]
    [SerializeField][MinValue(0.001f)]
        protected float _slowFillSpeed;
    private UnityEvent onSlowFillFinish;
    private bool isFilling;
    private float targetAmount;

    protected float fillAmount = 1;

    #region GS
    public float FillAmount { get => fillAmount; set => fillAmount = value; }
    public UnityEvent OnSlowFillFinish { get => onSlowFillFinish; set => onSlowFillFinish = value; }
    #endregion

    private void Awake()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
        onSlowFillFinish = new UnityEvent();
    }

    public virtual void Fill(float amount)
    {
        amount = Mathf.Clamp(amount, 0, 1);
    }
    public void StartSlowFill(float amount)
    {
        targetAmount = amount;

        if (isFilling) return;

        StartCoroutine(SlowFill());
    }
    private IEnumerator SlowFill()
    {
        isFilling = true;
        while (fillAmount != targetAmount)
        {
            yield return null;
            Fill(Mathf.SmoothStep(fillAmount, targetAmount, _slowFillSpeed * Time.deltaTime));   
            //Fill(Mathf.Lerp(fillAmount, targetAmount, _slowFillSpeed * Time.deltaTime));
        }

        onSlowFillFinish.Invoke();
        onSlowFillFinish.RemoveAllListeners();

        isFilling = false;
    }
}
