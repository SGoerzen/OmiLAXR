using System.Collections.Generic;
using UnityEngine;

namespace OmiLAXR.Pipeline
{
    public class PipelineJobs<TInput, TOutput> : List<PipelineJob<TInput, TOutput>> {}
    public abstract class PipelineJob<TInput, TOutput> : MonoBehaviour
    {
        public readonly PipelineJobs<TInput, TOutput> Jobs = new PipelineJobs<TInput, TOutput>();

        public abstract PipelineData<TOutput> Pass(PipelineData<TInput> data);
    }
}