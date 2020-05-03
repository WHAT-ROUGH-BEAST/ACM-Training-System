using CTrainingSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using WindowsFormsControlLibrary2;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCreateXML()
        {
            XmlGenerator.GenerateXML<ExerciseProblem>("C:\\Users\\18069\\Desktop\\test.xml",
                new ExerciseProblem("name", "des", null, null));
        }

        [TestMethod]
        public void TestReadXML()
        {
            ExerciseProblem t = 
                XmlGenerator.ReadXML<ExerciseProblem>("C:\\Users\\18069\\Desktop\\test.xml");

            Assert.IsTrue(t.Description.Equals("des"));
            Assert.IsTrue(t.Name.Equals("name"));
        }
    }
}