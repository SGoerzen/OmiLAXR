using NUnit.Framework;
using UnityEngine;

namespace OmiLAXR.Tests.Actors
{
    public class Actor_Tests
    {
        private GameObject _gameObject;
        private Actor _actor;

        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject("TestActor");
            _actor = _gameObject.AddComponent<Actor>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_actor.gameObject);
        }

        [Test]
        public void Actor_Tests_Has_Default_Name_And_Email()
        {
            // Assert
            Assert.AreEqual("Anonymous", _actor.actorName);
            Assert.AreEqual("anonymous@omilaxr.dev", _actor.actorEmail);
        }

        [Test]
        public void Actor_Tests_Can_Set_ActorName()
        {
            // Act
            _actor.actorName = "Test Name";

            // Assert
            Assert.AreEqual("Test Name", _actor.actorName);
        }

        [Test]
        public void Actor_Tests_Can_Set_ActorEmail()
        {
            // Act
            _actor.actorEmail = "test@example.com";

            // Assert
            Assert.AreEqual("test@example.com", _actor.actorEmail);
        }

        [Test]
        public void Actor_Tests_Can_Set_ActorName_And_Email()
        {
            // Act
            _actor.actorName = "Test Name";
            _actor.actorEmail = "test@example.com";

            // Assert
            Assert.AreEqual("Test Name", _actor.actorName);
            Assert.AreEqual("test@example.com", _actor.actorEmail);
        }

        [Test]
        public void Actor_Tests_Can_Reset_To_Default_Values()
        {
            // Arrange
            _actor.actorName = "Test Name";
            _actor.actorEmail = "test@example.com";

            // Act
            _actor.actorName = "Anonymous";
            _actor.actorEmail = "anonymous@omilaxr.dev";

            // Assert
            Assert.AreEqual("Anonymous", _actor.actorName);
            Assert.AreEqual("anonymous@omilaxr.dev", _actor.actorEmail);
        }
    }
}