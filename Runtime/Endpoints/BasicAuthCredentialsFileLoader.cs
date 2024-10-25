using System.ComponentModel;
using System.IO;
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