using System;
using NUnit.Framework;

namespace OmiLAXR.Tests.Attributes
{
    public class ActionAttribute_Tests
    {
        [Test]
        public void ActionAttribute_Tests_Can_Create_With_Name()
        {
            // Arrange
            var expectedName = "TestAction";

            // Act
            var attribute = new ActionAttribute(expectedName);

            // Assert
            Assert.AreEqual(expectedName, attribute.Name);
        }

        [Test]
        public void ActionAttribute_Tests_Allows_Multiple_Attributes()
        {
            // Arrange
            var fieldInfo = typeof(MockClass).GetField("FieldWithMultipleAttributes");

            // Act
            var attributes = (ActionAttribute[])fieldInfo.GetCustomAttributes(typeof(ActionAttribute), false);

            // Assert
            Assert.AreEqual(2, attributes.Length);
            Assert.AreEqual("Action1", attributes[0].Name);
            Assert.AreEqual("Action2", attributes[1].Name);
        }

        [Test]
        public void ActionAttribute_Tests_Can_Be_Applied_To_Fields_And_Events()
        {
            // Arrange
            var fieldInfo = typeof(MockClass).GetField("TestField");

            // Act
            var fieldAttribute = (ActionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(ActionAttribute));

            // Assert
            Assert.IsNotNull(fieldAttribute);
            Assert.AreEqual("FieldAction", fieldAttribute.Name);
        }

        // Mock class to demonstrate the usage of ActionAttribute
        private class MockClass
        {
            [Action("FieldAction")]
            public string TestField;

            [Action("Action1")]
            [Action("Action2")]
            public string FieldWithMultipleAttributes;
        }
    }
}