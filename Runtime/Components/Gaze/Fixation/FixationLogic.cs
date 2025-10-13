using UnityEngine;

namespace OmiLAXR.Components.Gaze.Fixation
{
    public abstract class FixationLogic : ScriptableObject
    {
        public abstract void ResetLogic();
        public abstract bool TryUpdateFixation(RaycastHit hit, Transform hmdTransform, out bool isNewFixation);
        public static FixationLogic GetDefault() => CreateInstance<FixationLogicAngularStability>();
        public FixationLogic Clone() => Instantiate(this);
    }
}