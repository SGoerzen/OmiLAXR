using System.Collections.Generic;
using OmiLAXR.Extensions;
using OmiLAXR.Types;

namespace OmiLAXR.Components
{
    public abstract class FaceData
    {
        protected readonly Dictionary<FaceActionUnit, float> AuValues;
        protected readonly Dictionary<FaceActionUnit, float> AuConfidences;

        public int Count => Weights.Length; // assuming same length

        // Fast, allocation-free reads
        public float this[int i] => Weights[i];
        public float GetConfidence(int i) => Confidences[i];

        // If you want a safe enumerable:
        public float[] Weights { get; }

        public float[] Confidences { get; }

        public FaceData(float[] weights, float[] confidences)
        {
            Weights = weights;
            Confidences    = confidences;
            AuValues = new Dictionary<FaceActionUnit, float>();
            AuConfidences = new Dictionary<FaceActionUnit, float>();
        }
        
        public float GetAU(FaceActionUnit au)
        {
            return AuValues.TryGetValue(au, out var value) ? value : 0f;
        }

        public float GetConfidence(FaceActionUnit au)
        {
            return AuConfidences.TryGetValue(au, out var value) ? value : 1f;
        }

        public override bool Equals(object obj)
        {
            if (typeof(object) != typeof(FaceData))
                return false;
            var fd = (FaceData)obj;
            return fd.Weights.AreValuesEqual(Weights) && fd.Confidences.AreValuesEqual(Confidences);
        }
    }
}