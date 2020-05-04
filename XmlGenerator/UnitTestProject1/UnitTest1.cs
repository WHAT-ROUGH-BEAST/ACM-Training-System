using CTrainingSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using WindowsFormsControlLibrary2;

namespace UnitTestProject1
{/*
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCreateXML()
        {
            XmlGenerator.GenerateXML<MyTest<What1, What2>>("C:\\Users\\18069\\Desktop\\test.xml",
                new MyTest<What1, What2>(new What1("name"), new What2("test")));
        }

        [TestMethod]
        public void TestReadXML()
        {
            MyTest<What1, What2> t = 
                XmlGenerator.ReadXML<MyTest<What1, What2>>("C:\\Users\\18069\\Desktop\\test.xml");

            Assert.AreEqual(t.a, 100);

            Assert.IsTrue(t.GetInputs()[0][0].GetAttr().Equals("name"));
            Assert.IsTrue(t.GetOutputs()[0][0].GetAttr().Equals("test"));

            Assert.IsTrue(t.list[0].Equals("here"));
        }
    }
    */

    [TestClass]
    public class Unitest2
    {
        [TestMethod]
        public void TestExePro()
        {
            List<object> list = new List<object>();
            list.Add("input");
            List<List<object>> Inputs = new List<List<object>>();
            Inputs.Add(list);

            List<object> list1 = new List<object>();
            list1.Add("output");
            List<List<object>> Outputs = new List<List<object>>();
            Outputs.Add(list1);

            XmlGenerator.GenerateXML<ExerciseProblem> ("C:\\Users\\18069\\Desktop\\test.xml",
                new ExerciseProblem("name", "des", Inputs, Outputs));

            ExerciseProblem t = XmlGenerator.ReadXML<ExerciseProblem>("C:\\Users\\18069\\Desktop\\test.xml");

            Assert.IsTrue(t.GetName().Equals("name"));
            Assert.IsTrue(t.GetDes().Equals("des"));
            Assert.IsTrue(t.GetIn()[0][0].Equals("input"));
            Assert.IsTrue(t.GetOut()[0][0].Equals("output"));
        }
    }
}