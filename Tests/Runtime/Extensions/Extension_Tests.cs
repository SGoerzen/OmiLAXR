using NUnit.Framework;
using OmiLAXR.Extensions;
using UnityEngine;

namespace OmiLAXR.Tests.Extensions
{
     public class Extension_Tests
    {
        private GameObject _gameObject;
        private Extension _extension;
        private MockPipelineComponent _mockExtensionComponent;
        private MockPipelineComponent _mockPipelineExtension;

        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject("TestObject");
            _extension = _gameObject.AddComponent<Extension>();

            _mockExtensionComponent = _gameObject.AddComponent<MockPipelineComponent>();
            _mockPipelineExtension = _gameObject.AddComponent<MockPipelineComponent>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_extension.gameObject);
        }

        [Test]
        public void Extension_Tests_CanAssignExtensionComponent()
        {
            // Act
            _extension.extensionComponent = _mockExtensionComponent;

            // Assert
            Assert.AreEqual(_mockExtensionComponent, _extension.extensionComponent);
        }

        [Test]
        public void Extension_Tests_CanAssignPipelineExtension()
        {
            // Act
            _extension.pipelineExtension = _mockPipelineExtension;

            // Assert
            Assert.AreEqual(_mockPipelineExtension, _extension.pipelineExtension);
        }

        [Test]
        public void Extension_Tests_CanAssignBothComponents()
        {
            // Act
            _extension.extensionComponent = _mockExtensionComponent;
            _extension.pipelineExtension = _mockPipelineExtension;

            // Assert
            Assert.AreEqual(_mockExtensionComponent, _extension.extensionComponent);
            Assert.AreEqual(_mockPipelineExtension, _extension.pipelineExtension);
        }

        [Test]
        public void Extension_Tests_InitialValues_AreNull()
        {
            // Assert
            Assert.IsNull(_extension.extensionComponent);
            Assert.IsNull(_extension.pipelineExtension);
        }
    }

    // Mock class to simulate the PipelineComponent
    public class MockPipelineComponent : PipelineComponent
    {
        // You can add custom behavior here if needed for more complex tests
    }
}

