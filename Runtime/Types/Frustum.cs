using UnityEngine;

namespace OmiLAXR.Types
{
    public struct Frustum
    {
        public readonly float ZNear;
        public readonly float ZFar;
        public readonly float FovX;
        public readonly float FovY;

        public Frustum(float zNear, float zFar, float fovX, float fovY)
        {
            ZNear = zNear;
            ZFar = zFar;
            FovX = fovX; 
            FovY = fovY;
        }
        
        /// <summary>
        /// Liefert das symmetrische Frustum der (Mono-)Kamera.
        /// FovX/FovY sind in Grad. Gilt für Perspective-Kameras.
        /// </summary>
        public static Frustum FromCamera(Camera cam)
        {
            if (cam == null) throw new System.ArgumentNullException(nameof(cam));
            if (cam.orthographic)
                throw new System.NotSupportedException("Orthographic-Kameras haben kein sinnvolles FOV. Nutze Breite/Höhe statt FOV.");

            var zNear = cam.nearClipPlane;
            var zFar  = cam.farClipPlane;

            var fovY = cam.fieldOfView; // Vertikales FOV in Grad
            var fovX = 2f * Mathf.Atan(Mathf.Tan(fovY * Mathf.Deg2Rad * 0.5f) * cam.aspect) * Mathf.Rad2Deg;

            return new Frustum(zNear, zFar, fovX, fovY);
        }

        /// <summary>
        /// Liefert das (symmetrisch angenäherte) Frustum für ein Stereo-Auge.
        /// Nutzt die per-Auge-Projektionsmatrix, falls Stereo aktiv ist.
        /// </summary>
        public static Frustum FromCameraStereo(Camera cam, Camera.StereoscopicEye eye)
        {
            if (cam == null) throw new System.ArgumentNullException(nameof(cam));
            if (!cam.stereoEnabled)
                return FromCamera(cam); // Fallback

            if (cam.orthographic)
                throw new System.NotSupportedException("Orthographic-Kameras haben kein sinnvolles FOV. Nutze Breite/Höhe statt FOV.");

            var P = cam.GetStereoProjectionMatrix(eye);
            var zNear = cam.nearClipPlane;
            var zFar  = cam.farClipPlane;

            // Für ein (nahezu) symmetrisches FOV genügen m00/m11:
            var fovY = 2f * Mathf.Atan(1f / P[1,1]) * Mathf.Rad2Deg;
            var fovX = 2f * Mathf.Atan(1f / P[0,0]) * Mathf.Rad2Deg;

            return new Frustum(zNear, zFar, fovX, fovY);
        }
    }
}