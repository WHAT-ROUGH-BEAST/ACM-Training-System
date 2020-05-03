using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace WindowsFormsControlLibrary2
{
    /// <summary>
    /// 将XmlGenerator定义为泛型后，即typeof(T)
    /// 便不需要声明任何XmlInclude， Serializable等, 即使该类中有list<自定义类型>成员
    /// 
    /// 以及自定义类型可以使用泛型
    /// </summary>
    public class XmlGenerator
    {
        private static XmlSerializer Serializer;

        public static void GenerateXML<T>(string FileName, T Item)
        {
            // 说明需要序列化的类型
            Serializer = new XmlSerializer(typeof(T));

            try
            {
                // 创建文件流
                Stream writer = new FileStream(FileName, FileMode.Create);

                Serializer.Serialize(writer, Item);

                Serializer = null;
                writer.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("illegal filepath: " + e.Message);
                throw e; // 以便其他的程序收到以响应
            }
        }

        public static T ReadXML<T>(string FileName)
        {
            T t;

            try
            {
                // 必须要再new一个XmlSerializer才能其他操作，还不清楚为什么
                Serializer = new XmlSerializer(typeof(T));

                Stream reader = new FileStream(FileName, FileMode.Open);

                // 从xml文件里提取出存的对象
                t = (T)Serializer.Deserialize(reader);

                Serializer = null;
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("illegal filepath: " + e.Message);
                throw e; // 以便其他的程序收到以响应
            }

            return t;
        }
    }



    /// <summary>
    /// List<泛型> 成员
    /// List<基本类型> 成员
    /// List<List<泛型>> 成员
    /// </summary>
    public class MyTest<T, B>
    {
        public List<List<T>> Inputs = new List<List<T>>();
        public List<List<B>> Outputs = new List<List<B>>();
        public List<string> list = new List<string>();
        public List<T> list2 = new List<T>();

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

    public class What1
    {
        public string attr;
        public What1(string name)
        {
            attr = name;
        }
        public What1() { }
    }

    public class What2
    {
        public string attr;
        public What2(string name)
        {
            attr = name;
        }
        public What2() { }
    }
}