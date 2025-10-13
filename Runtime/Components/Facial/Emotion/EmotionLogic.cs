
using UnityEngine;

namespace OmiLAXR.Components.Facial.Emotion
{
public abstract class EmotionLogic : ScriptableObject
{
    [Header("Detection Settings")]
    [Range(0f, 1f)] public float onThreshold = 0.6f;
    [Range(0f, 1f)] public float offThreshold = 0.4f;
    public float minOnsetMs = 180f;
    public float minOffsetMs = 250f;
    public float emaAlpha = 0.25f;

    public bool IsActive { get; protected set; }
    protected double OnsetCandidate = double.NaN;
    protected double OffsetCandidate = double.NaN;
    protected float EmaValue;
    protected bool HasEma;

    public abstract string EmotionType { get; }
    
    public float CurrentIntensity { get; private set; }

    /// <summary>
    /// Implementiert in den Subklassen: berechnet die Rohaktivierung einer Emotion.
    /// </summary>
    protected abstract float ComputeActivation(FaceData provider);

    public virtual void Evaluate(FaceData data, double timestamp)
    {
        var raw = Mathf.Clamp01(ComputeActivation(data));

        // Exponentielles Glätten
        if (!HasEma) { EmaValue = raw; HasEma = true; }
        else EmaValue = emaAlpha * raw + (1f - emaAlpha) * EmaValue;

        CurrentIntensity = EmaValue;

        // Hysterese-Logik (On/Off mit Mindestdauer)
        if (!IsActive)
        {
            if (CurrentIntensity >= onThreshold)
            {
                if (double.IsNaN(OnsetCandidate)) OnsetCandidate = timestamp;
                if ((timestamp - OnsetCandidate) * 1000.0 >= minOnsetMs)
                {
                    IsActive = true;
                    OnsetCandidate = double.NaN;
                    OffsetCandidate = double.NaN;
                    OnActivated(timestamp, CurrentIntensity);
                }
            }
            else OnsetCandidate = double.NaN;
        }
        else
        {
            if (CurrentIntensity < offThreshold)
            {
                if (double.IsNaN(OffsetCandidate)) OffsetCandidate = timestamp;
                if ((timestamp - OffsetCandidate) * 1000.0 >= minOffsetMs)
                {
                    IsActive = false;
                    OffsetCandidate = double.NaN;
                    OnsetCandidate = double.NaN;
                    OnDeactivated(timestamp, CurrentIntensity);
                }
            }
            else OffsetCandidate = double.NaN;
        }
    }

    protected virtual void OnActivated(double ts, float intensity) 
        => Debug.Log($"{name} ON @ {ts:F3}s I={intensity:F2}");

    protected virtual void OnDeactivated(double ts, float intensity) 
        => Debug.Log($"{name} OFF @ {ts:F3}s I={intensity:F2}");
}

}
