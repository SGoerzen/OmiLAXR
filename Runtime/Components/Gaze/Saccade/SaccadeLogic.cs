using UnityEngine;

namespace OmiLAXR.Components.Gaze.Saccade
{
    public abstract class SaccadeLogic : ScriptableObject
    {
        public abstract void ResetLogic();
        
        public static SaccadeLogic GetDefault() => CreateInstance<SaccadeLogicVelocityThreshold>();

        /// <summary>
        /// Aktualisiert die Sakkaden-Statemachine.
        /// Gibt true zurück, wenn ein Zustandswechsel (Start/Ende) erkannt wurde.
        /// isStart==true: Sakkade startet (liefert Start-Daten),
        /// isStart==false: Sakkade endet (liefert vollständige SaccadeData).
        /// </summary>
        public abstract bool TryUpdateSaccade(
            GazeHit previousHit,
            GazeHit currentHit,
            Vector3 previousDirection,
            Vector3 currentDirection,
            float deltaTime,
            float? pupilDiameterMillimeters,
            out bool isStart,
            out SaccadeData data);
    }
}