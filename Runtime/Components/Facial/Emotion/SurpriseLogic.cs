using OmiLAXR.Types;
using UnityEngine;

namespace OmiLAXR.Components.Facial.Emotion
{
    [CreateAssetMenu(menuName = "OmiLAXR / Facial / Emotions / Surprise")]
    public class SurpriseLogic : EmotionLogic
    {
        public override string EmotionType => OmiLAXR.Types.Emotion.Surprise.ToString();

        protected override float ComputeActivation(FaceData p)
        {
            var brow = Mathf.Max(p.GetAU(FaceActionUnit.AU1_InnerBrowRaiser),
                p.GetAU(FaceActionUnit.AU2_OuterBrowRaiser));
            var lid  = p.GetAU(FaceActionUnit.AU5_UpperLidRaiser);
            var jaw  = p.GetAU(FaceActionUnit.AU26_JawDrop);
            return (brow + lid + jaw) / 3f;
        }
    }

}