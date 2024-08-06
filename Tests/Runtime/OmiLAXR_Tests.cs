using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using UnityEditor;

namespace OmiLAXR.Tests
{
    public class OmiLAXR_Tests
    {
        private GameObject _mainGo;
        
        [SetUp]
        public void Setup()
        {
            // Pipeline Setup
            _mainGo = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Packages/com.rwth.unity.omilaxr/Prefabs/OmiLAXR.prefab"));
        }
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator DataPassedFullPipeline()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }// A Test behaves as an ordinary method
        [Test]
        public void TestPrefab()
        {
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}