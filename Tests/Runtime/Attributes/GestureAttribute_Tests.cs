using System;
using NUnit.Framework;

namespace OmiLAXR.Tests.Attributes
{
    public class GestureAttribute_Tests
    {
        [Test]
        public void GestureAttribute_Tests_Can_Create_With_Name()
        {
            // Arrange
            string expectedName = "TestGesture";

            // Act
            var attribute = new GestureAttribute(expectedName);

            // Assert
            Assert.AreEqual(expectedName, attribute.Name);
        }

        [Test]
        public void GestureAttribute_Tests_Allows_Multiple_Attributes()
        {
            // Arrange
            var fieldInfo = typeof(MockClass).GetField("FieldWithMultipleAttributes");

            // Act
            var attributes = (GestureAttribute[])fieldInfo.GetCustomAttributes(typeof(GestureAttribute), false);

            // Assert
            Assert.AreEqual(2, attributes.Length);
            Assert.AreEqual("Gesture1", attributes[0].Name);
            Assert.AreEqual("Gesture2", attributes[1].Name);
        }

        [Test]
        public void GestureAttribute_Tests_Can_Be_Applied_To_Fields_And_Events()
        {
            // Arrange
            var fieldInfo = typeof(MockClass).GetField("TestField");
            var eventInfo = typeof(MockClass).GetEvent("TestEvent");

            // Act
            var fieldAttribute = (GestureAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(GestureAttribute));
            var eventAttribute = (GestureAttribute)Attribute.GetCustomAttribute(eventInfo, typeof(GestureAttribute));

            // Assert
            Assert.IsNotNull(fieldAttribute);
            Assert.AreEqual("FieldGesture", fieldAttribute.Name);

            Assert.IsNotNull(eventAttribute);
            Assert.AreEqual("EventGesture", eventAttribute.Name);
        }

        // Mock class to demonstrate the usage of GestureAttribute
        private class MockClass
        {
            [Gesture("FieldGesture")]
            public string TestField;

            [Gesture("EventGesture")]
            public event Action TestEvent;

            [Gesture("Gesture1")]
            [Gesture("Gesture2")]
            public string FieldWithMultipleAttributes;
        }
    }
}