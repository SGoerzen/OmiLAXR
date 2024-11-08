using OmiLAXR.Enums;
using UnityEngine;

namespace OmiLAXR.Context
{
    [AddComponentMenu("OmiLAXR / 0) Scenario Context / Scenario Language")]
    [DisallowMultipleComponent]
    public class ScenarioLanguage : LearningContext
    {
        private static ScenarioLanguage _instance;
        public static ScenarioLanguage Instance => GetInstance(ref _instance);

        public Languages language = Languages.en;
    }
}