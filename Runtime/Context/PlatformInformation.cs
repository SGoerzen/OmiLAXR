using System;
using UnityEngine;

namespace OmiLAXR.Context
{
    [AddComponentMenu("OmiLAXR / 0) Scenario Context / Platform Information")]
    [DisallowMultipleComponent]
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
        public static PlatformInformation Instance
            => _instance ??= FindObjectOfType<PlatformInformation>();
        private const string OmiLAXR_Version = "2.0.7";

        // This will store the value in the inspector
        [SerializeField] 
        public PlatformInformationAppendix appendix = new PlatformInformationAppendix();

        public string GetPlatformString()
        {
            var platformStr = $"OmiLAXR:{OmiLAXR_Version}:{Application.platform}";

            if (appendix.prefixes.Length > 0)
            {
                platformStr = $"{string.Join(':', appendix.prefixes)}:{platformStr}";
            }
            
            if (appendix.suffixes.Length > 0)
            {
                platformStr = $"{platformStr}:{string.Join(':', appendix.suffixes)}";
            }

            return platformStr;
        }
    }
}