using System;
using Newtonsoft.Json;

namespace OmiLAXR.TrackingBehaviours.Learner.EyeTracking
{
    // public struct PupilDilationData
    // {
    //     
    //     public readonly double PupilDiameterMillimeters;
    //     public readonly DateTime? Timestamp; // Optional
    //     
    //     public PupilDilationData(double pupilDiameterMillimeters, DateTime? timestamp)
    //     {
    //         PupilDiameterMillimeters = pupilDiameterMillimeters;
    //         Timestamp = timestamp;
    //     }
    // }
    
    /// <summary>
    /// Represents pupil dilation data, including start and end diameters, dilation change, and duration.
    /// </summary>
    public struct PupilDilationData
    {
        /// <summary>
        /// Initial pupil diameter at the start of the event, in millimeters.
        /// </summary>
        [JsonProperty("pupilDiameterStart")]
        public readonly double PupilDiameterStart;

        /// <summary>
        /// Final pupil diameter at the end of the event, in millimeters.
        /// </summary>
        [JsonProperty("pupilDiameterEnd")]
        public readonly double PupilDiameterEnd;

        /// <summary>
        /// The change in pupil diameter from start to end, in millimeters.
        /// </summary>
        [JsonProperty("dilationChange")]
        public readonly double DilationChange;

        /// <summary>
        /// The time in milliseconds over which the pupil dilation change occurred.
        /// </summary>
        [JsonProperty("durationInMilliseconds")]
        public readonly int DurationInMilliseconds;

        /// <summary>
        /// Initializes a new instance of the <see cref="PupilDilationData"/> class.
        /// </summary>
        /// <param name="pupilDiameterStart">Initial pupil diameter in mm.</param>
        /// <param name="pupilDiameterEnd">Final pupil diameter in mm.</param>
        /// <param name="durationInMilliseconds">Duration of the dilation event in ms.</param>
        public PupilDilationData(double pupilDiameterStart, double pupilDiameterEnd, int durationInMilliseconds)
        {
            PupilDiameterStart = pupilDiameterStart;
            PupilDiameterEnd = pupilDiameterEnd;
            DilationChange = pupilDiameterEnd - pupilDiameterStart;
            DurationInMilliseconds = durationInMilliseconds;
        }

        /// <summary>
        /// Overrides the ToString() method to provide a string representation of the pupil dilation data.
        /// </summary>
        public override string ToString()
        {
            return $"Pupil Dilation Data: Start = {PupilDiameterStart} mm, End = {PupilDiameterEnd} mm, " +
                   $"Change = {DilationChange} mm, Duration = {DurationInMilliseconds} ms";
        }
    }
}