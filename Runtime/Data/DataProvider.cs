using UnityEngine;

namespace OmiLAXR.Data
{
    public abstract class DataProvider : MonoBehaviour
    {
        public StatementComposer[] composers;
        private void Awake()
        {
            composers = FindObjectsOfType<StatementComposer>();
            Debug.Log("Found " + composers.Length + " composers.");
        }
      
    }
}