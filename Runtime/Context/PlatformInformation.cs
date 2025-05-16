using System;
using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.Context
{
    [AddComponentMenu("OmiLAXR / 0) Scenario Context / Platform Information")]
    [DisallowMultipleComponent]
    [Description("Provides platform information in following format [prefixes:]OmiLAXR:v2.0.14:{OS}[:suffixes].")]
    public class PlatformInformation : LearningContext
    {
        [Serializable]
        public struct PlatformInformationAppendix
        {
            [SerializeField] 
            public string[] suffixes;
            [SerializeField] 
            public string[] prefixes;
        }
        
        private static PlatformInformation _instance;
        public static PlatformInformation Instance => GetInstance(ref _instance);
        
        private const string OmiLAXR_Version = "v2.0.14";

        // This will store the value in the inspector
        [SerializeField] 
        public PlatformInformationAppendix appendix = new PlatformInformationAppendix();

        public string GetPlatformString()
        {
            var platformStr = $"OmiLAXR:{OmiLAXR_Version}:{Application.platform}";

            if (appendix.prefixes.Length > 0)
            {
#if UNITY_2019 || UNITY_2020
                platformStr = $"{string.Join(":", appendix.prefixes)}:{platformStr}";
#else
                platformStr = $"{string.Join(':', appendix.prefixes)}:{platformStr}";
#endif
            }
            
            if (appendix.suffixes.Length > 0)
            {
#if UNITY_2019 || UNITY_2020
                platformStr = $"{platformStr}:{string.Join(":", appendix.suffixes)}";
#else
                platformStr = $"{platformStr}:{string.Join(':', appendix.suffixes)}";
#endif
            }

            return platformStr;
        }
    }
}