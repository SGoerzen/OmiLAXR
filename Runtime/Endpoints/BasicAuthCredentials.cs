
using UnityEngine;

namespace OmiLAXR.Endpoints
{
    [System.Serializable]
    public struct BasicAuthCredentials
    {
        public string endpoint;
        [Tooltip("Alternatively called key.")]
        public string username;
        [Tooltip("Alternatively called secret.")]
        public string password;

        public BasicAuthCredentials(string endpoint, string username, string password)
        {
            this.endpoint = endpoint;
            this.username = username;
            this.password = password;
        }
        
        public bool IsValid => !string.IsNullOrEmpty(endpoint) 
                               && !string.IsNullOrEmpty(username) 
                               && !string.IsNullOrEmpty(password);

        public override string ToString()
        {
            return $"[BasicAuthCredentials endpoint={endpoint}, username={username}, password={password}]";
        }
    }
}