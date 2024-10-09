using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

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
            Assert.AreEqual("Group", _actorGroup.groupName);
            Assert.AreEqual("group@omilaxr.dev", _actorGroup.groupEmail);
            Assert.IsEmpty(_actorGroup.GetMembers());  // By default, the members array should be null
        }

        [Test]
        public void ActorGroup_Tests_Can_Set_Members()
        {
            // Arrange
            var member1 = new GameObject("Member1").AddComponent<Actor>();
            var member2 = new GameObject("Member2").AddComponent<Actor>();

            _actorGroup.AddMembers(member1, member2);

            var members = _actorGroup.GetMembers();
            Array.Sort(members, (actor, actor1) => string.CompareOrdinal(actor.actorName, actor1.actorName));

            // Act & Assert
            Assert.AreEqual(2, members.Length);
            Assert.Contains(member1, members);
            Assert.Contains(member2, members);
        }

        [Test]
        public void ActorGroup_Tests_Can_Set_ActorName_And_Email()
        {
            // Act
            _actorGroup.groupName = "Test Group Name";
            _actorGroup.groupEmail = "testgroup@example.com";

            // Assert
            Assert.AreEqual("Test Group Name", _actorGroup.groupName);
            Assert.AreEqual("testgroup@example.com", _actorGroup.groupEmail);
        }

        [Test]
        public void ActorGroup_Tests_Members_Array_Can_Be_Empty()
        {
            // Act
            var members = _actorGroup.GetMembers();

            // Assert
            Assert.IsNotNull(members);
            Assert.IsEmpty(members);
        }

        [Test]
        public void ActorGroup_Tests_ActorGroup_Inherits_Actor_Properties()
        {
            // Act
            _actorGroup.groupName = "Group Name";
            _actorGroup.groupEmail = "group@example.com";

            // Assert
            Assert.AreEqual("Group Name", _actorGroup.groupName);
            Assert.AreEqual("group@example.com", _actorGroup.groupEmail);
        }
    }
}