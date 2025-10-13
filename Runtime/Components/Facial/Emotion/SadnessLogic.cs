using OmiLAXR.Types;
using UnityEngine;

namespace OmiLAXR.Components.Facial.Emotion
{
    [CreateAssetMenu(menuName = "OmiLAXR / Facial / Emotions/Sadness")]
    public class SadnessLogic : EmotionLogic
    {
        public override string EmotionType => OmiLAXR.Types.Emotion.Sadness.ToString();

        protected override float ComputeActivation(FaceData p)
        {
            var brow = p.GetAU(FaceActionUnit.AU1_InnerBrowRaiser);
            var lip = p.GetAU(FaceActionUnit.AU15_LipCornerDepressor);
            var chin = p.GetAU(FaceActionUnit.AU17_ChinRaiser);
            return (brow + lip + chin) / 3f;
        }
    }
}