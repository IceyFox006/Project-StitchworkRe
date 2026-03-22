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
    [SerializeField][MinValue(0)]
        private float _duration;
    private UnityEvent onSlowFillFinish;
    private bool isFilling;
    private float targetAmount;
    private float time;

    protected float fillAmount = 1;

    #region GS
    public float FillAmount { get => fillAmount; set => fillAmount = value; }
    public UnityEvent OnSlowFillFinish { get => onSlowFillFinish; set => onSlowFillFinish = value; }
    public float Duration { get => _duration; set => _duration = value; }
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
        time = 0;
        targetAmount = amount;

        if (isFilling) return;

        StartCoroutine(SlowFill());
    }
    private IEnumerator SlowFill()
    {
        isFilling = true;

        while (time <= _duration)//(fillAmount != targetAmount)
        {
            Fill(Mathf.Lerp(fillAmount, targetAmount, Lerp.EaseOut(time / _duration)));
            yield return null;
            time += Time.deltaTime;
        }
        onSlowFillFinish.Invoke();
        onSlowFillFinish.RemoveAllListeners();

        isFilling = false;
    }
}
