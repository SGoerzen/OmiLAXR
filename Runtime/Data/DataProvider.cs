using UnityEngine;

namespace OmiLAXR.Data
{
    public class DataProvider : MonoBehaviour
    {
        [HideInInspector]
        public StatementComposer[] composers;
        private void Awake()
        {
            composers = GetComponentsInChildren<StatementComposer>();
            Debug.Log("Found " + composers.Length + " composers.");
        }
        
        public static DataProvider GetAll() => FindObjectOfType<DataProvider>();

      
    }
}