using NaughtyAttributes;
using System.Collections;
using UnityEngine;

[ExecuteAlways]
public class TimeManager : Manager
{
    [SerializeField] 
        private Light _directionalLight;

    [SerializeField][MinValue(0)]
        private float _updateInterval;
    [SerializeField][MinValue(0)]
        private float _updateSpeed;

    [SerializeField][Range(0,24)]
        private float _timeOfDay;
    [SerializeField]
        private TimeVisualSO _curVisual;

    private bool updatingTime = true;

    private void OnValidate()
    {
        _directionalLight = RenderSettings.sun;
    }

    public override void Load()
    {
        base.Load();
        
        StartCoroutine(IntervalUpdate());
    }
    private IEnumerator IntervalUpdate()
    {
        while (updatingTime)
        {
            yield return new WaitForSeconds(_updateInterval);
            if (Application.isPlaying)
            {
                _timeOfDay += _updateSpeed * _updateInterval;
                _timeOfDay %= 24;
                UpdateLighting(_timeOfDay / 24);
            }
            else
                UpdateLighting(_timeOfDay / 24);
        }
    }
    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = _curVisual.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = _curVisual.FogColor.Evaluate(timePercent);

        _directionalLight.color = _curVisual.DirectionalColor.Evaluate(timePercent);
        _directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170, 0));
    }
}
