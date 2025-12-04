using System;
using UnityEngine;

namespace OmiLAXR.Context
{
    public abstract class SdkProvider : MonoBehaviour
    {
        public abstract string GetName();
        public abstract Version GetVersion();

        private static SdkProvider _instance;
        public static SdkProvider Instance {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<SdkProvider>();
                }

                return _instance;
            }
        }
    }
}