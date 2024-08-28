using NUnit.Framework;
using OmiLAXR.Filters;
using TMPro;
using UnityEngine;

namespace OmiLAXR.Tests.Filters
{
    public class UnityComponentsFilter_Tests
    {
        private UnityComponentsFilter _filter;

        [SetUp]
        public void SetUp()
        {
            var gameObject = new GameObject();
            _filter = gameObject.AddComponent<UnityComponentsFilter>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_filter.gameObject);
        }

        [Test]
        public void Pass_Allows_GameObject_Without_Forbidden_Components()
        {
            // Arrange
            var go = new GameObject("TestObject");
            go.AddComponent<Rigidbody>();

            // Act
            var result = _filter.Pass(new Object[] { go });

            // Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(result[0], Is.EqualTo(go));
        }

        [Test]
        public void Pass_Filters_Out_GameObject_With_Forbidden_Components()
        {
            // Arrange
            var go = new GameObject("TestObject");
            go.AddComponent<TextMeshPro>();

            // Act
            var result = _filter.Pass(new Object[] { go });

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Pass_Allows_Non_GameObject_Allowed_Type()
        {
            // Arrange
            var allowedObject = new Material(Shader.Find("Standard"));

            // Act
            var result = _filter.Pass(new Object[] { allowedObject });

            // Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(result[0], Is.EqualTo(allowedObject));
        }

        [Test]
        public void Pass_Filters_Out_Non_GameObject_Forbidden_Type()
        {
            // Arrange
            var forbiddenObject = new TextMeshPro();

            // Act
            var result = _filter.Pass(new Object[] { forbiddenObject });

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void IsAllowedType_Identifies_Forbidden_Types()
        {
            // Arrange
            var forbiddenType = typeof(TextMeshPro);

            // Act
            var isAllowed = UnityComponentsFilter.IsAllowedType(forbiddenType);

            // Assert
            Assert.IsFalse(isAllowed);
        }

        [Test]
        public void IsAllowedType_Identifies_Allowed_Types()
        {
            // Arrange
            var allowedType = typeof(Rigidbody);

            // Act
            var isAllowed = UnityComponentsFilter.IsAllowedType(allowedType);

            // Assert
            Assert.IsTrue(isAllowed);
        }

        [Test]
        public void IsAllowedObject_Identifies_Forbidden_GameObject()
        {
            // Arrange
            var go = new GameObject("TestObject");
            go.AddComponent<TextMeshPro>();

            // Act
            var isAllowed = UnityComponentsFilter.IsAllowedObject(go);

            // Assert
            Assert.IsFalse(isAllowed);
        }

        [Test]
        public void IsAllowedObject_Identifies_Allowed_GameObject()
        {
            // Arrange
            var go = new GameObject("TestObject");
            go.AddComponent<Rigidbody>();

            // Act
            var isAllowed = UnityComponentsFilter.IsAllowedObject(go);

            // Assert
            Assert.IsTrue(isAllowed);
        }
    }
}