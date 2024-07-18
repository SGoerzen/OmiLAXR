using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using OmiLAXR.Pipeline.Stages.ObjectDetection;
using UnityEditor;

namespace OmiLAXR.Tests
{
    public class OmiLAXR_Pipeline_Test
    {
        private readonly GameObject _prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/OmiLAXR Pipeline.prefab");
        private GameObject _goPipeline;
        
        [SetUp]
        public void Setup()
        {
            // Pipeline Setup
            _goPipeline = Object.Instantiate(_prefab);
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
        public void NewTestScriptSimplePasses()
        {
            // Use the Assert class to test conditions
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