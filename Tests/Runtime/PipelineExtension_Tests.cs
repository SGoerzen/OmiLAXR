
using NUnit.Framework;
using UnityEngine;
using OmiLAXR.Extensions;
using OmiLAXR.Filters;
using OmiLAXR.Listeners;
using OmiLAXR.TrackingBehaviours;

namespace OmiLAXR.Tests
{
    public class PipelineExtension_Tests
    {
        
        public class MockFilter : Filter
        {
            public override Object[] Pass(Object[] gos)
            {
                return gos;
            }
        }
        public class MockListener : Listener
        {
            public override void StartListening()
            {
            
            }
        }
        public class MockTrackingBehaviour : TrackingBehaviour
        {
            protected override void AfterFilteredObjects(Object[] objects)
            {
            
            }
        }
        
        // Mock classes to simulate the Pipeline and other components
        public class MockPipeline : Pipeline
        {
            public void TestAwake() => Awake();
        }

        public class MockPipelineExtension : PipelineExtension<MockPipeline>
        {
            protected override PipelineComponent[] OnExtend()
            {
                return new PipelineComponent[]
                {
                    gameObject.AddComponent<MockFilter>(),
                    gameObject.AddComponent<MockListener>()
                };
            }
        }
        
        private GameObject _pipelineObject;
        private GameObject _pipelineExtObject;
        private MockPipeline _pipeline;
        private MockPipelineExtension _pipelineExtension;

        [SetUp]
        public void SetUp()
        {
            _pipelineObject = new GameObject("Pipeline");
            _pipelineExtObject = new GameObject("Pipeline Extension");
            _pipeline = _pipelineObject.AddComponent<MockPipeline>();
            _pipelineObject.AddComponent<Actor>();
            _pipelineExtension = _pipelineExtObject.AddComponent<MockPipelineExtension>();

            // Trigger the Awake method manually for testing
            _pipeline.TestAwake();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_pipelineExtension.gameObject);
            Object.DestroyImmediate(_pipelineObject);
        }

        [Test]
        public void PipelineExtension_Tests_Awake_ShouldFindPipelineAndRegisterExtensions()
        {
            // Assert
            Assert.IsNotNull(_pipelineExtension.GetPipeline());
            Assert.AreEqual(_pipeline, _pipelineExtension.GetPipeline());
            
            var extensions = _pipeline.GetComponents<Extension>();
            var mockFilter = _pipelineExtension.GetComponent(typeof(MockFilter));
            var mockListener = _pipelineExtension.GetComponent(typeof(MockListener));
            
            Assert.AreEqual(2, extensions.Length);
            Assert.IsInstanceOf<MockFilter>(mockFilter);
            Assert.IsInstanceOf<MockListener>(mockListener);
        }
    }

    

    

}