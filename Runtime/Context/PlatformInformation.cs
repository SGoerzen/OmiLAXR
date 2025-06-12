using System;
using System.ComponentModel;
using UnityEngine;

namespace OmiLAXR.Context
{
    [AddComponentMenu("OmiLAXR / Scenario Context / Platform Information")]
    [DisallowMultipleComponent]
    [Description("Provides platform information in following format [prefixes:]OmiLAXR.{MODULE}:{COMPOSER}:{VERSION}:{OS}[:suffixes].")]
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
        
        // This will store the value in the inspector
        [SerializeField] 
        public PlatformInformationAppendix appendix = new PlatformInformationAppendix();

        public string GetPlatformString(string module, string version, string composer)
        {
            var platformStr = $"OmiLAXR.{module}:{composer}:v{version}:{Application.platform}";

            if (appendix.prefixes.Length > 0)
            {
                platformStr = $"{string.Join(":", appendix.prefixes)}:{platformStr}";
            }
            
            if (appendix.suffixes.Length > 0)
            {
                platformStr = $"{platformStr}:{string.Join(":", appendix.suffixes)}";
            }

            return platformStr;
        }
    }
}