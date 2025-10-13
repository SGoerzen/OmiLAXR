using System;
using OmiLAXR.Components.Gaze;
using OmiLAXR.Types;

public sealed class PursuitData
{
    public readonly Duration Duration;
    public readonly float AvgVelocityDegPerSec;
    public readonly float TrackingErrorDeg;

    public readonly float? TargetVelocityDegPerSec;
    public readonly float? Gain;
    public readonly int? PursuitLatencyMs;
    public readonly float? MeanConfidence;
    public readonly int? DropoutCount;
    public readonly int SampleCount;

    public readonly DateTime? StartTime;
    public readonly DateTime? EndTime;
    public readonly GazeHit Hit;

    public PursuitData(
        GazeHit hit,
        float avgVelocityDegPerSec,
        float trackingErrorDeg,
        DateTime? startTime,
        DateTime? endTime,
        float? targetVelocityDegPerSec = null,
        float? meanConfidence = null,
        int? dropoutCount = null,
        int? pursuitLatencyMs = null,
        int sampleCount = 0)
    {
        Hit = hit;
        AvgVelocityDegPerSec = avgVelocityDegPerSec;
        TrackingErrorDeg = trackingErrorDeg;
        StartTime = startTime;
        EndTime = endTime;

        TargetVelocityDegPerSec = targetVelocityDegPerSec;
        MeanConfidence = meanConfidence;
        DropoutCount = dropoutCount;
        PursuitLatencyMs = pursuitLatencyMs;
        SampleCount = sampleCount;

        Gain = (targetVelocityDegPerSec.HasValue && targetVelocityDegPerSec.Value > 1e-4f)
            ? avgVelocityDegPerSec / targetVelocityDegPerSec.Value
            : (float?)null;

        if (startTime.HasValue && endTime.HasValue)
        {
            var ms = (int)(endTime.Value - startTime.Value).TotalMilliseconds;
            Duration = Duration.FromMilliseconds(ms);
        }
    }
}
