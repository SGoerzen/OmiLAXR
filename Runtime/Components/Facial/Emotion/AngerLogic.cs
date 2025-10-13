using OmiLAXR.Types;
using UnityEngine;

namespace OmiLAXR.Components.Facial.Emotion
{
    [CreateAssetMenu(menuName = "OmiLAXR / Facial / Emotions / Anger")]
    public class AngerLogic : EmotionLogic
    {
        public override string EmotionType => OmiLAXR.Types.Emotion.Anger.ToString();

        protected override float ComputeActivation(FaceData p)
        {
            var brow = p.GetAU(FaceActionUnit.AU4_BrowLowerer);
            var lid  = p.GetAU(FaceActionUnit.AU7_LidTightener);
            var lips = Mathf.Max(p.GetAU(FaceActionUnit.AU23_LipTightener),
                p.GetAU(FaceActionUnit.AU24_LipPressor));
            return (brow + lid + lips) / 3f;
        }
    }

}