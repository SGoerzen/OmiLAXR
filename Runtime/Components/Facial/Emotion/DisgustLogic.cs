using OmiLAXR.Types;
using UnityEngine;

namespace OmiLAXR.Components.Facial.Emotion
{
    [CreateAssetMenu(menuName = "OmiLAXR / Facial / Emotions / Disgust")]
    public class DisgustLogic : EmotionLogic
    {
        public override string EmotionType => OmiLAXR.Types.Emotion.Disgust.ToString();

        protected override float ComputeActivation(FaceData p)
        {
            var nose = p.GetAU(FaceActionUnit.AU9_NoseWrinkler);
            var lip  = p.GetAU(FaceActionUnit.AU10_UpperLipRaiser);
            return (nose + lip) * 0.5f;
        }
    }
}