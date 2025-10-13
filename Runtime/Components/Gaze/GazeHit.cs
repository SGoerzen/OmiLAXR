using UnityEngine;

namespace OmiLAXR.Components.Gaze
{
    public sealed class GazeHit 
    {
        public RaycastHit RayHit;
        public GazeDetector GazeDetector;
        public Vector3 GazeDirectionInWorld;
        public Vector3 GazeOriginInWorld; 
        public bool IsValid => RayHit.collider != null;
        public GameObject Target => RayHit.collider.gameObject;
        public GameObject Source => GazeDetector.gameObject;

        /// <summary>0° = directly aimed at AOI center.</summary>
        public float ViewingAngleDeg;

        /// <summary>0° = straight-on to the hit surface (optional metric).</summary>
        public float IncidenceAngleDeg;
        
        public GazeHit(GazeDetector gazeDetector, RaycastHit rayHit,
            Vector3 gazeDirectionInWorld, Vector3 gazeOriginInWorld)
        {
            RayHit = rayHit;
            GazeDetector = gazeDetector;
            GazeDirectionInWorld = gazeDirectionInWorld;
            GazeOriginInWorld = gazeOriginInWorld;
            
            if (rayHit.collider != null)
            {
                // viewingAngle: ray vs. vector to AOI center
                var toCenter = rayHit.collider.bounds.center - gazeOriginInWorld;
                ViewingAngleDeg = (toCenter.sqrMagnitude > 1e-8f)
                    ? Vector3.Angle(gazeDirectionInWorld, toCenter)
                    : 0f;

                // incidenceAngle: ray vs. surface normal (0° = straight-on)
                IncidenceAngleDeg = Vector3.Angle(-gazeDirectionInWorld, rayHit.normal);
            }
            else
            {
                ViewingAngleDeg = float.NaN;
                IncidenceAngleDeg = float.NaN;
            }
        }
    }
}