using UnityEngine;

namespace OmiLAXR.Pipelines
{
    public abstract class PipelineTrigger : MonoBehaviour
    {
        public abstract bool TriggerCondition();
    }
}