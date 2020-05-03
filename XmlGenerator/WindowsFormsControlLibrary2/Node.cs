using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WindowsFormsControlLibrary2
{
    public class Test : ICloneable
    {
        // 同一文件夹下，题目名称唯一
        private string Name;
        private string Description;
        private List<List<object>> TestInputs;
        private List<List<object>> TestOutputs;

        public Test(string Name, string Description,
            List<List<object>> TestInputs, List<List<object>> TestOutputs)
        {
            this.Name = Name;
            this.Description = Description;
            this.TestInputs = TestInputs;
            this.TestOutputs = TestOutputs;
        }

        public string GetName()
        {
            return Name;
        }

        public string GetDes()
        {
            return Description;
        }

        public List<List<object>> GetInputs()
        {
            // 要进一步修改为深拷贝
            return TestInputs;
        }

        public List<List<object>> GetOutputs()
        {
            // 要进一步修改为深拷贝
            return TestOutputs;
        }

        public void SetName(string Name)
        {
            if (null == Name)
                throw new ArgumentNullException("null Name");
            if (Name.Equals(""))
                throw new Exception("invalid Test Name");

            this.Name = Name;
        }

        public void SetDes(string Des)
        {
            if (null == Des)
                throw new ArgumentNullException("null Description");
            if (Des.Equals(""))
                throw new Exception("invalid Test Name");

            this.Description = Des;
        }

        public void SetInputs(List<List<object>> Inputs)
        {
            if (null == Inputs)
                throw new ArgumentNullException("null Inputs");

            this.TestInputs = Inputs;
        }

        public void SetOutputs(List<List<object>> Outputs)
        {
            if (null == Outputs)
                throw new ArgumentNullException("null Outputs");

            this.TestOutputs = Outputs;
        }

        public object Clone()
        {
            //TODO : TestInputs, TestOutputs要进一步修改为深拷贝
            return new Test(Name, Description, TestInputs, TestOutputs);
        }
    }

    abstract class Node
    {
        public abstract Boolean IsFile();

        public abstract string GetName();

        public abstract void AddChild(Node Child);

        public abstract List<Node> GetChild();

        public abstract void DeleteChild(Node Child);
    }

    class FileNode : Node
    {
        private const Boolean isFile = true;

        private string FileName = null;
        private List<Node> Childs;

        public FileNode(String FileName, List<Node> Childs)
        {
            if (null == FileName)
                throw new ArgumentNullException("null Inputs");
            if (FileName.Equals(""))
                throw new Exception("invalid FileName");
            if (null == Childs)
                throw new ArgumentNullException("null Childs");

            this.FileName = FileName;
            this.Childs = Childs;
        }

        public FileNode(String FileName)
        {
            if (null == FileName)
                throw new ArgumentNullException("null Inputs");
            if (FileName.Equals(""))
                throw new Exception("invalid FileName");

            this.FileName = FileName;
            Childs = new List<Node>();
        }

        public override Boolean IsFile()
        {
            return isFile;
        }

        public override string GetName()
        {
            return FileName;
        }

        public override void AddChild(Node Child)
        {
            Childs.Add(Child);
        }

        public override List<Node> GetChild()
        {
            return Childs;
        }

        public override void DeleteChild(Node Child)
        {
            if (!Childs.Contains(Child))
                throw new Exception("no such child");

            Childs.Remove(Child);
        }
    }

    class TestNode : Node
    {
        private const Boolean isFile = false;

        private Test Test = null;

        public TestNode(Test Test)
        {
            if (null == Test)
                throw new ArgumentNullException("null Test");

            this.Test = Test;
        }

        public override Boolean IsFile()
        {
            return isFile;
        }

        public override string GetName()
        {
            return Test.GetName();
        }

        public override void AddChild(Node Child)
        {
            throw new NotTestNodeMethod("can't add Child to a TestNode");
        }

        public override List<Node> GetChild()
        {
            throw new NotTestNodeMethod("can't find Childs at a TestNode");
        }

        public override void DeleteChild(Node Child)
        {
            throw new NotTestNodeMethod("can't find Childs at a TestNode");
        }

        // 修改test的方法，不直接提供Test内容
        // 都为非Node接口方法，下转型之后使用
        public Test GetTest()
        {
            return (Test)Test.Clone();
        }
    }

    public class NotTestNodeMethod : Exception
    {
        public NotTestNodeMethod() { }
        public NotTestNodeMethod(string message) : base(message)
        { }
    }
}