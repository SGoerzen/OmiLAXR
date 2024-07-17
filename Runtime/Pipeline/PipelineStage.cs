using System.Collections.Generic;
using UnityEngine;

namespace OmiLAXR.Pipeline
{
    public abstract class PipelineStage : MonoBehaviour
    {
        public readonly List<PipelineJob> jobs = new List<PipelineJob>();
        public abstract PipelineData Pass(PipelineData input);
    }
}