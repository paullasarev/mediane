using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mediane.Models;
using XmlUnit.Xunit;

//Install-Package XmlUnit.Xunit

namespace Mediane.Tests.Models
{
    /// <summary>
    /// Summary description for ContentModelTest
    /// </summary>
    [TestClass]
    public class ContentModelTest
    {
        ContentModel model = new ContentModel("main");

        public ContentModelTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ShouldHaveIdProperty()
        {
            Assert.AreEqual("main", model.Id);
        }

        [TestMethod]
        public void ShouldTrimIdSpaces()
        {
            ContentModel model = new ContentModel(" main ");
            Assert.AreEqual("main", model.Id);
        }

        [TestMethod]
        public void ShouldGetSetContent()
        {
            string content = "The wiki markup";

            model.Content = content;
            Assert.AreEqual(content, model.Content);
        }

        [TestMethod]
        public void NullContentAssignDoesNotAffectState()
        {
            string content = "The wiki markup";
            model.Content = content;
            model.Content = null;
            Assert.AreEqual(content, model.Content);
        }

        void AssertXmlIsEqual(string expect, string actual)
        {
            XmlAssertion.AssertXmlEquals(
                new XmlDiff(
                    new XmlInput(expect),
                    new XmlInput(actual),
                    new DiffConfiguration("XmlUnit", System.Xml.WhitespaceHandling.None)
                    ));
        }

        [TestMethod]
        public void RenderedShouldProduceParagraphTag()
        {
            model.Content = "one paragraph";
            AssertXmlIsEqual(" <p>one paragraph</p>\n ", model.Rendered);
        }

    }
}
