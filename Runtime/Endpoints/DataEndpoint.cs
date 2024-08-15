using UnityEngine;

namespace OmiLAXR.Endpoints
{
    public abstract class DataEndpoint : MonoBehaviour
    {
        public BasicAuthCredentials credentials = new BasicAuthCredentials("https://lrs.elearn.rwth-aachen.de/data/xAPI", "", "");

    }
}