using OmiLAXR.Types;
using UnityEngine;

namespace OmiLAXR.Components.Facial.Emotion
{
    [CreateAssetMenu(menuName = "OmiLAXR / Facial / Emotions / Fear")]
    public class FearLogic : EmotionLogic
    {
        public override string EmotionType => OmiLAXR.Types.Emotion.Fear.ToString();

        protected override float ComputeActivation(FaceData p)
        {
            var brow = Mathf.Max(p.GetAU(FaceActionUnit.AU1_InnerBrowRaiser),
                p.GetAU(FaceActionUnit.AU2_OuterBrowRaiser));
            var lid  = p.GetAU(FaceActionUnit.AU5_UpperLidRaiser);
            var lips = p.GetAU(FaceActionUnit.AU20_LipStretcher);
            return (brow + lid + lips) / 3f;
        }
    }
}