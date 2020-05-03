using System;
using GccCompiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string Add = GccCompiler.GccCompiler.Compile("C:\\Users\\18069\\Desktop\\test\\test.c");

            Assert.IsTrue(Add.Equals("C:\\Users\\18069\\Desktop\\test\\test.exe"));
        }
    }
}
