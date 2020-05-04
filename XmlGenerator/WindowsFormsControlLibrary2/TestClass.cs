using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsControlLibrary2
{
    /// <summary>
    /// List<泛型> 成员
    /// List<基本类型> 成员
    /// List<List<泛型>> 成员
    /// </summary>
    
    [DataContract()]
    public class MyTest<T, B>
    {
        [DataMember()]
        private List<List<T>> Inputs = new List<List<T>>();
        [DataMember()]
        private List<List<B>> Outputs = new List<List<B>>();
        [DataMember()]
        public int a = 100;
        [DataMember()]
        public List<string> list = new List<string>();

        public MyTest(T arg1, B arg2)
        {
            this.list.Add("here");

            List<T> list = new List<T>();
            list.Add(arg1);
            Inputs.Add(list);

            List<B> list2 = new List<B>();
            list2.Add(arg2);
            Outputs.Add(list2);
        }

        public MyTest()
        {
            this.list.Add("here");
        }

        public List<List<T>> GetInputs()
        {
            return Inputs;
        }

        public List<List<B>> GetOutputs()
        {
            return Outputs;
        }
    }
    [DataContract()]
    public class What1
    {
        [DataMember()]
        private string attr;
        public What1(string name)
        {
            attr = name;
        }
        private What1() { }

        public string GetAttr()
        {
            return attr;
        }
    }
    [DataContract()]
    public class What2
    {
        [DataMember()]
        private string attr;
        public What2(string name)
        {
            attr = name;
        }
        private What2() { }

        public string GetAttr()
        {
            return attr;
        }
    }
}
