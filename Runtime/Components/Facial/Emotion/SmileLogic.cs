using OmiLAXR.Types;
using UnityEngine;

namespace OmiLAXR.Components.Facial.Emotion
{
    [CreateAssetMenu(menuName = "OmiLAXR / Facial / Emotions / Smile")]
    public class SmileLogic : EmotionLogic
    {
        public override string EmotionType => OmiLAXR.Types.Emotion.Smile.ToString();

        protected override float ComputeActivation(FaceData p)
        {
            var au12 = p.GetAU(FaceActionUnit.AU12_LipCornerPuller);
            return au12; // pure smile without cheek involvement
        }
    }
}