using System.IO;
using UnityEditor;
using UnityEngine;

namespace OmiLAXR.Endpoints
{
    [RequireComponent(typeof(Endpoint))]
    [AddComponentMenu("OmiLAXR / 6) Endpoints / Basic Auth Credentials File Loader")]
    [DefaultExecutionOrder(-1000)]
    public class BasicAuthCredentialsFileLoader : MonoBehaviour
    {
        [Header("Name of file that is located in StreamingAssets path.")]
        public string filename = "credentials.json";

        public BasicAuthEndpoint targetEndpoint;
        
#if UNITY_EDITOR
        [MenuItem("OmiLAXR / Create credentials.json file ")]
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
            var credentialsPath = Path.GetFullPath("Packages/com.rwth.unity.omilaxr/Examples/example.credentials.json");
            File.Copy(credentialsPath, destFilePath, false);
        }        
#endif
        
        private void Start()
        {
            
        }

        private void Awake()
        {
            if (!targetEndpoint)
            {
                targetEndpoint = GetComponent<BasicAuthEndpoint>();
            }
            LoadConfig();
        }

        private void LoadConfig()
        {
            var filePath = Path.Combine(Application.streamingAssetsPath, filename);

            if (File.Exists(filePath))
            {
                var jsonContent = File.ReadAllText(filePath);
                var credentials = JsonUtility.FromJson<BasicAuthCredentials>(jsonContent);

                targetEndpoint.credentials = credentials;
                targetEndpoint.StartSending();

                Debug.Log($"Loaded '{filename}' successfully.", this);
            }
            else
            {
                Debug.LogError($"Cannot find '{filename}' in path '{filePath}'.", this);
            }
        }
    }
}