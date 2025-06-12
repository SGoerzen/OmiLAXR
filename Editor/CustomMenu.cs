#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace OmiLAXR.Editor
{
    internal static class CustomMenu
    {
        [MenuItem("OmiLAXR / Create credentials.json file")]
        private static void CreateCredentialsFile()
        {
            var destFilePath = Path.Combine(Application.streamingAssetsPath, "credentials.json");
            // interupt, if credentials file exists
            if (File.Exists(destFilePath))
            {
                Debug.LogError($"There is already a file '{destFilePath}'.");
                return;
            }
            
            // ensure StreamingAssets folder
            if (!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }
            
            // copy credentials file
            var credentialsPath = Path.GetFullPath("Packages/com.rwth.unity.omilaxr/Resources/StreamingAssets/example.credentials.json");
            File.Copy(credentialsPath, destFilePath, false);
        }        
    }
}
#endif