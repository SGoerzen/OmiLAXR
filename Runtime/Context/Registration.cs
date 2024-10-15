using System;
using OmiLAXR.Context;
using UnityEngine;

namespace OmiLAXR
{
    [DefaultExecutionOrder(-10000)]
    [DisallowMultipleComponent]
    public class Registration : LearningContext
    {
        private static Registration _instance;
        public static Registration Instance
            => _instance ??= FindObjectOfType<Registration>();
        
        [Header("Must be an UUID according to RFC4122.")]
        public string uuid;

        public bool autoGenerateUuid = false;

        private void Awake()
        {
            if (autoGenerateUuid)
                uuid = GenerateUuid().ToString();
        }

        private void OnEnable()
        {
            
        }

        public Guid GenerateUuid()
        {
            return System.Guid.NewGuid();
        }
    }
}