using NUnit.Framework;
using UnityEngine;

namespace OmiLAXR.Tests.Actors
{
    public class ActorGroup_Tests
    {
        private GameObject _gameObject;
        private ActorGroup _actorGroup;

        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject("TestActorGroup");
            _actorGroup = _gameObject.AddComponent<ActorGroup>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_actorGroup.gameObject);
        }

        [Test]
        public void ActorGroup_Tests_Has_Default_Values()
        {
            // Assert
            Assert.AreEqual("Anonymous", _actorGroup.actorName);
            Assert.AreEqual("anonymous@omilaxr.dev", _actorGroup.actorEmail);
            Assert.IsNull(_actorGroup.members);  // By default, the members array should be null
        }

        [Test]
        public void ActorGroup_Tests_Can_Set_Members()
        {
            // Arrange
            var member1 = new GameObject("Member1").AddComponent<Actor>();
            var member2 = new GameObject("Member2").AddComponent<Actor>();
            _actorGroup.members = new Actor[] { member1, member2 };

            // Act & Assert
            Assert.AreEqual(2, _actorGroup.members.Length);
            Assert.AreEqual(member1, _actorGroup.members[0]);
            Assert.AreEqual(member2, _actorGroup.members[1]);
        }

        [Test]
        public void ActorGroup_Tests_Can_Set_ActorName_And_Email()
        {
            // Act
            _actorGroup.actorName = "Test Group Name";
            _actorGroup.actorEmail = "testgroup@example.com";

            // Assert
            Assert.AreEqual("Test Group Name", _actorGroup.actorName);
            Assert.AreEqual("testgroup@example.com", _actorGroup.actorEmail);
        }

        [Test]
        public void ActorGroup_Tests_Members_Array_Can_Be_Empty()
        {
            // Act
            _actorGroup.members = new Actor[] { };

            // Assert
            Assert.IsNotNull(_actorGroup.members);
            Assert.IsEmpty(_actorGroup.members);
        }

        [Test]
        public void ActorGroup_Tests_ActorGroup_Inherits_Actor_Properties()
        {
            // Act
            _actorGroup.actorName = "Group Name";
            _actorGroup.actorEmail = "group@example.com";

            // Assert
            Assert.AreEqual("Group Name", _actorGroup.actorName);
            Assert.AreEqual("group@example.com", _actorGroup.actorEmail);
        }
    }
}