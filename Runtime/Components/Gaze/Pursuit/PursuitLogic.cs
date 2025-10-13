using UnityEngine;

namespace OmiLAXR.Components.Gaze.Pursuit
{
    public abstract class PursuitLogic : ScriptableObject
    {
        public abstract void ResetLogic();

        /// <summary>
        /// Aktualisiert die Verfolgungs-Statemachine.
        /// Gibt true zurück, wenn ein Zustandswechsel (Start/Ende) erkannt wurde.
        /// isStart==true: Pursuit startet (liefert Start-Schnappschuss),
        /// isStart==false: Pursuit endet (liefert vollständige SmoothPursuitData).
        /// </summary>
        public abstract bool TryUpdatePursuit(
            GazeHit currentHit,
            Vector3 prevEyeDir,    Vector3 currEyeDir,
            Vector3 prevTargetDir, Vector3 currTargetDir,
            float deltaTime,
            out bool isStart,
            out PursuitData data);

        public static PursuitLogic GetDefault() => CreateInstance<PursuitLogicBasic>();

    }
}