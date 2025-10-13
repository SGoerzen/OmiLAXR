using OmiLAXR.Types;
using UnityEngine;

namespace OmiLAXR.Components.Facial.Emotion
{
    [CreateAssetMenu(menuName = "OmiLAXR / Facial / Emotions / Happiness")]
    public class HappinessLogic : EmotionLogic
    {
        public override string EmotionType => OmiLAXR.Types.Emotion.Happiness.ToString();

        [Range(0f, 1f)] public float cheekWeight = 0.35f;

        protected override float ComputeActivation(FaceData p)
        {
            var au12 = p.GetAU(FaceActionUnit.AU12_LipCornerPuller);
            var au6  = p.GetAU(FaceActionUnit.AU6_CheekRaiser);
            return (1f - cheekWeight) * au12 + cheekWeight * au6;
        }
    }
}