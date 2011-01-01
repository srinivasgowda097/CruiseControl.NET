﻿namespace CruiseControl.Core.Tests
{
    using CruiseControl.Core.Structure;
    using CruiseControl.Core.Tests.Stubs;
    using NUnit.Framework;

    [TestFixture]
    public class ServerTests
    {
        #region Tests
        [Test]
        public void AddingAProjectSetsTheServer()
        {
            var project = new Project();
            var server = new Server();
            server.Children.Add(project);
            Assert.AreSame(server, project.Server);
        }

        [Test]
        public void StartingAServerWithChildrenSetsTheServer()
        {
            var project = new Project();
            var server = new Server(project);
            Assert.AreSame(server, project.Server);
        }

        [Test]
        public void RemovingAProjectClearsTheServer()
        {
            var project = new Project();
            var server = new Server(project);
            server.Children.Remove(project);
            Assert.IsNull(project.Server);
        }

        [Test]
        public void UniversalNameGetsTheUrnName()
        {
            var server = new Server("Test");
            var actual = server.UniversalName;
            Assert.AreEqual("urn:ccnet:Test", actual);
        }

        [Test]
        public void UniversalNameHandlesMissingName()
        {
            var server = new Server();
            var actual = server.UniversalName;
            Assert.AreEqual("urn:ccnet:", actual);
        }

        [Test]
        public void ValidateValidatesName()
        {
            var validated = false;
            var server = new Server();
            var validationStub = new ValidationLogStub
                                     {
                                         OnAddErrorMessage = (m, a) =>
                                                                 {
                                                                     Assert.AreEqual("The Server has no name specified.", m);
                                                                     Assert.AreEqual(0, a.Length);
                                                                     validated = true;
                                                                 }
                                     };
            server.Validate(validationStub);
            Assert.IsTrue(validated);
        }

        [Test]
        public void ValidateValidatesChildren()
        {
            var validated = false;
            var project = new ProjectStub
                              {
                                  Name = "Project",
                                  OnValidate = vl => validated = true
                              };
            var server = new Server("Server", project);
            var validationStub = new ValidationLogStub();
            server.Validate(validationStub);
            Assert.IsTrue(validated);
        }

        [Test]
        public void ValidateDetectsDuplicateChildItems()
        {
            var errorAdded = false;
            var queue1 = new Queue("Queue");
            var queue2 = new Queue("Queue");
            var queue3 = new Queue("OtherQueue");
            var server = new Server("Server", queue1, queue2, queue3);
            var validationStub = new ValidationLogStub
                                     {
                                         OnAddErrorMessage = (m, a) =>
                                                                 {
                                                                     Assert.AreEqual(
                                                                         "Duplicate {1} name detected: '{0}'", m);
                                                                     CollectionAssert.AreEqual(
                                                                         new[] { "Queue", "child" },
                                                                         a);
                                                                     errorAdded = true;
                                                                 }
                                     };
            server.Validate(validationStub);
            Assert.IsTrue(errorAdded);
        }

        [Test]
        public void ValidateDetectsDuplicateProjects()
        {
            var errorAdded = false;
            var project1 = new Project("Project");
            var project2 = new Project("Project");
            var childQueue = new Queue("Test", project2);
            var project3 = new Project("OtherProject");
            var server = new Server("Server", project1, childQueue, project3);
            var validationStub = new ValidationLogStub
                                     {
                                         OnAddErrorMessage = (m, a) =>
                                                                 {
                                                                     Assert.AreEqual(
                                                                         "Duplicate {1} name detected: '{0}'", m);
                                                                     CollectionAssert.AreEqual(
                                                                         new[] { "Project", "project" },
                                                                         a);
                                                                     errorAdded = true;
                                                                 }
                                     };
            server.Validate(validationStub);
            Assert.IsTrue(errorAdded);
        }
        #endregion
    }
}
