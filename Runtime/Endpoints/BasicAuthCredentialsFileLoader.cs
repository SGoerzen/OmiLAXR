using System.IO;
using UnityEngine;

namespace OmiLAXR.Endpoints
{
    [RequireComponent(typeof(Endpoint))]
    [AddComponentMenu("OmiLAXR / 6) Endpoints / Basic Auth Credentials File Loader")]
    [DefaultExecutionOrder(0)]
    public class BasicAuthCredentialsFileLoader : MonoBehaviour
    {
        [Header("Name of file that is located in Assets folder (data path).")]
        public string filename = "credentials.json";

        public BasicAuthEndpoint targetEndpoint;
        
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
            string filePath;
#if UNITY_EDITOR
            filePath = Path.Combine(Application.dataPath, filename);
#else
            filePath = Path.Combine(Application.dataPath, "../" + filename);
#endif

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