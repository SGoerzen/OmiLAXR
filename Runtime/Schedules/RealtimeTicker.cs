using System;
using System.Collections;
using UnityEngine;

namespace OmiLAXR.Schedules
{
    /// <summary>
    /// Scheduler that triggers on every Unity frame update.
    /// </summary>
    [CreateAssetMenu(
        fileName = "RealtimeTicker",
        menuName = "OmiLAXR/Scheduler/RealtimeTicker",
        order = 1)]
    public class RealtimeTicker : Scheduler
    {
       /// <summary>
        /// Run loop: triggers every frame.
        /// </summary>
        protected override IEnumerator Run()
        {
            while (isActive)
            {
                // Wait one frame
                yield return null;
                TriggerOnTick();
            }
        }

        /// <summary>
        /// Factory for programmatic creation.
        /// </summary>
        public static RealtimeTicker Create(
            MonoBehaviour owner,
            Action onTick = null,
            Action onTickStart = null,
            Action onTickEnd = null, bool runImmediate = false)
        {
            var scheduler = CreateInstance<RealtimeTicker>();
            scheduler.Init(owner, onTick, onTickStart, onTickEnd, runImmediate);
            return scheduler;
        }
    }
}