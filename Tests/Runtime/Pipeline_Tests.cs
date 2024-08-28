using System;
using System.Collections.Generic;
using NUnit.Framework;
using OmiLAXR.Filters;
using OmiLAXR.Listeners;
using OmiLAXR.TrackingBehaviours;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace OmiLAXR.Tests
{
    public class Pipeline_Tests
    {
        // Mock classes to simulate the Pipeline and other components
        public class MockPipeline : Pipeline
        {
            public void TestAwake() => Awake();
            public void TestCollectGesturesAndActions() => CollectGesturesAndActions();
        }
        // Mock classes to simulate the various components
        public class MockListener : Listener
        {
            public bool HasStartedListening { get; private set; }

            public override void StartListening()
            {
                HasStartedListening = true;
                
                Found(new Object[] { new GameObject("TestObject") });
            }
        }

        public class MockFilter : Filter
        {
            public override Object[] Pass(Object[] gos)
            {
                return new Object[] { new GameObject("TestObject") };
            }
        }

        public class MockTrackingBehaviour : TrackingBehaviour
        {
            [Action("MockAction")]
            public TrackingBehaviourEvent MockAction;

            [Gesture("MockGesture")]
            public TrackingBehaviourEvent MockGesture;

            protected override void AfterFilteredObjects(Object[] objects)
            {
                
            }
        }
        
        private GameObject _pipelineObject;
        private MockPipeline _pipeline;

        [SetUp]
        public void SetUp()
        {
            _pipelineObject = new GameObject("TestPipeline");
            _pipeline = _pipelineObject.AddComponent<MockPipeline>();
            _pipelineObject.AddComponent<Actor>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_pipeline.gameObject);
        }

        [Test]
        public void Pipeline_Tests_Awake_Initializes_Actor_If_Null()
        {
            // Arrange
            Assert.IsNull(_pipeline.actor);

            // Act
            _pipeline.TestAwake();

            // Assert
            Assert.IsNotNull(_pipeline.actor);
            Assert.IsInstanceOf<Actor>(_pipeline.actor);
        }

        [Test]
        public void Pipeline_Tests_Add()
        {
            // Arrange
            var listener = _pipelineObject.AddComponent<MockListener>();
            var filter = _pipelineObject.AddComponent<MockFilter>();
            var trackingBehaviour = _pipelineObject.AddComponent<MockTrackingBehaviour>();

            // Act
            _pipeline.Add(listener);
            _pipeline.Add(filter);
            _pipeline.Add(trackingBehaviour);

            // Assert
            Assert.AreEqual(1, _pipeline.Listeners.Count);
            Assert.AreEqual(1, _pipeline.Filters.Count);
            Assert.AreEqual(1, _pipeline.TrackingBehaviours.Count);
            Assert.Contains(listener, _pipeline.Listeners);
            Assert.Contains(filter, _pipeline.Filters);
            Assert.Contains(trackingBehaviour, _pipeline.TrackingBehaviours);
            
            // Additional
            _pipeline.Add((PipelineComponent)filter);
            _pipeline.Add((PipelineComponent)trackingBehaviour);
            _pipeline.Add((PipelineComponent)listener);
            Assert.AreEqual(2, _pipeline.Listeners.Count);
            Assert.AreEqual(2, _pipeline.Filters.Count);
            Assert.AreEqual(2, _pipeline.TrackingBehaviours.Count);
            
            // Cleanup
            _pipeline.Listeners.RemoveAt(1);
            _pipeline.TrackingBehaviours.RemoveAt(1);
            _pipeline.Filters.RemoveAt(1);
        }

        [Test]
        public void Pipeline_Tests_Collects_Gestures_And_Actions()
        {
            // Arrange
            var trackingBehaviour = _pipelineObject.AddComponent<MockTrackingBehaviour>();
            _pipeline.Add(trackingBehaviour);

            // Act
            _pipeline.TestCollectGesturesAndActions();

            // Assert
            Assert.IsTrue(_pipeline.Actions.ContainsKey("MockAction"));
            Assert.IsTrue(_pipeline.Gestures.ContainsKey("MockGesture"));
        }

        [Test]
        public void Pipeline_Tests_OnEnable_Starts_Listening()
        {
            _pipeline.StopPipeline();
            
            // Arrange
            var listener = _pipelineObject.AddComponent<MockListener>();
            _pipeline.Add(listener);

            // Act
            _pipeline.StartPipeline();

            // Assert
            Assert.IsTrue(listener.HasStartedListening);
        }

        [Test]
        public void Pipeline_Tests_OnDisable_Stops_Pipeline()
        {
            _pipeline.StartPipeline();
            
            // Arrange
            var beforeStoppedCalled = false;
            _pipeline.BeforeStoppedPipeline += (p) => beforeStoppedCalled = true;

            // Act
            _pipeline.StopPipeline();

            // Assert
            Assert.IsTrue(beforeStoppedCalled);
        }

        [Test]
        public void Pipeline_Tests_FoundObjects_Applies_Filters()
        {
            
            var pipelineObject = new GameObject("TestPipeline");
            var pipeline = pipelineObject.AddComponent<MockPipeline>();
            pipelineObject.AddComponent<Actor>();
            
            pipeline.TestAwake();
            pipeline.StopPipeline();
            
            // Arrange
            var listener = pipelineObject.AddComponent<MockListener>();
            pipeline.Add(listener);

            var trackingObjects = new List<Object>();
            pipeline.AfterStarted += (p) =>
            {
                trackingObjects = p.trackingObjects;
            };
            
            pipeline.StartPipeline();

            // Assert
            Assert.AreEqual(1, trackingObjects.Count);
            Assert.AreEqual("TestObject", trackingObjects[0].name);
        }

        [Test]
        public void Pipeline_Tests_StartPipeline_Activates_GameObject()
        {
            // Arrange
            _pipelineObject.SetActive(false);

            // Act
            _pipeline.StartPipeline();

            // Assert
            Assert.IsTrue(_pipelineObject.activeSelf);
        }

        [Test]
        public void Pipeline_Tests_StopPipeline_Deactivates_GameObject()
        {
            // Arrange
            _pipelineObject.SetActive(true);

            // Act
            _pipeline.StopPipeline();

            // Assert
            Assert.IsFalse(_pipelineObject.activeSelf);
        }
    }

    
}