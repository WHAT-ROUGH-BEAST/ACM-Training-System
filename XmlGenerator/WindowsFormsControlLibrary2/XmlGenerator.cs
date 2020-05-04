using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Xml.Serialization;

namespace WindowsFormsControlLibrary2
{
    /// <summary>
    /// 将XmlGenerator定义为泛型后，即typeof(T)
    /// 
    /// 需要进行序列化的类，如果有private属性，则需要对类声明[DataContract()],，对【所有】属性声明[DataMember()]
    /// 同时对于涉及的属性的自定义类，进行以上声明
    ///         
    /// 自定义类型可以使用泛型
    /// </summary>
    public class XmlGenerator
    {
        private static DataContractSerializer Serializer;

        public static void GenerateXML<T>(string FileName, T Item)
        {
            // 说明需要序列化的类型
            Serializer = new DataContractSerializer(typeof(T));

            try
            {
                // 创建文件流
                Stream writer = new FileStream(FileName, FileMode.Create);

                Serializer.WriteObject(writer, Item);

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
                Serializer = new DataContractSerializer(typeof(T));

                Stream reader = new FileStream(FileName, FileMode.Open);

                // 从xml文件里提取出存的对象
                t = (T)Serializer.ReadObject(reader);

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
}