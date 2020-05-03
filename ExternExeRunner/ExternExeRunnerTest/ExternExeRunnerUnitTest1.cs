using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExeRunner;

namespace ExternExeRunnerTest
{
    [TestClass]
    public class ExternExeRunnerUnitTest1
    {
        [TestMethod]
        public void Test1()
        {
            // arrange
            // REPLACE THIS ADDRESS TO THE ACTUAL ADDRESS ON YOUR PC!
            string adder = @"C:\Users\HASEE\source\repos\ExeternExeRunner\ExeternExeRunner\Adder.exe";
            string[] args = 
            { 
                "2",        // 2 groups of test data
                "1 2",      
                "3 4"
            };
            string[] RefResults =
            {
                "3",
                "7"
            };

            // act
            string[] results = ExternExeRunner.Run(adder, args);

            // assert
            for (int i = 0; i < results.Length; i++)
            {
                Assert.AreEqual(RefResults[i], results[i]);
            }

        }
    }
}
