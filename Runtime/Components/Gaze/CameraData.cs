using OmiLAXR.Types;
using UnityEngine;

namespace OmiLAXR.Components.Gaze
{
    public sealed class CameraData : GazeData
    {
        public CameraData(GazeHit hit, Frustum frustum, Vector3 gazeOriginWorld, Vector3 gazePointWorld) 
            : base(hit, frustum, gazeOriginWorld, gazePointWorld)
        {
        }
    }
}