//serialization
namespace CTrainingSystem
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    // contain methods to properly read exercise problems from text files
    public class ReadTextProblem
    {             
        // find the first string in Texts that equals to Identifier
        // return its index SKIPPING the idnetifier; return -1 if fail
        public static int GetIdentifierIndex(string[] Texts, string Identifier)
        {           
            int Index = -1;
            for (int i = 0; i != Texts.Length; i++)
            {
                string Lower = Texts[i].ToLower();
                if (Lower.Contains(Identifier))
                    //(Lower.Equals(Identifier))
                {
                    Index = i;
                    break;
                }
            }
            return Index + 1;
        }

        // Concatenates strings of the range [IndexBeg, IndexEnd) of Texts
        // ignores empty strings and compensates changing line with a space
        static string StringConcat(string[] Texts, int Beg, int End)
        {
            System.Text.StringBuilder strbuild = new System.Text.StringBuilder();
            while (Beg < Texts.Length && Beg < End)
            {
                if (Texts[Beg].Length > 0)
                {
                    strbuild.Append(Texts[Beg]);
                    // compensate the loss of \n
                    strbuild.Append(' ');                    
                }
                Beg++;
            }
            return strbuild.ToString();
        }

        // read name or description from a string
        // ignores the identifier ("name:", "description:", etc)
        public static string ReadText(string[] Texts, int Beg, int End)
        {                       
            if (Texts.Length <= 0)
            {
                return null;
            }              
            if (Beg == -1 || End == -1)
            {
                return null;
            }

            // assign
            string str = StringConcat(Texts, Beg, End);           
            return str;
        }

        // convert a string, which seperates a sequence of numerics by space, 
        // to a list of numerics whose types could be int or double.
        // return null if fail
        static List<object> StringToNumerics(string str, bool IsInteger = true)
        {
            if (str.Length <= 0)
            {
                return null;
            }
            string[] Numbers = str.Split(' ');
            List<object> Results = new List<object>();
            foreach (string s in Numbers)
            {                
                try
                {
                    object temp;
                    if (IsInteger)
                    {
                        temp = System.Convert.ToInt32(s);
                    }
                    else
                    { 
                        temp = System.Convert.ToDouble(s); 
                    }
                    Results.Add(temp);
                }
                catch (System.OverflowException)
                {
                    if (IsInteger)
                    { 
                        System.Console.WriteLine("{0} is outside the range of the Int32 type.", str); 
                    }
                    else
                    {
                        System.Console.WriteLine("{0} is outside the range of the Double type.", str);
                    }
                    return null;
                }
                catch (System.FormatException)
                {
                    System.Console.WriteLine("The {0} value '{1}' is not in a recognizable format.",
                                      str.GetType().Name, str);
                    return null;
                }
            }
            return Results;
        }

        // read the counts from designated position: Texts[Index]    
        public static int ReadCounts(string[] Texts, int Index)
        {
            int Counts = -1;
            List<object> List = StringToNumerics(Texts[Index], true);
            if (List.Count > 0)
            {
                Counts = System.Convert.ToInt32(List[0]);
            }
            return Counts;
        }

        // read test data from a string array in [Beg, End)
        // in which each string contains a sequence of numerics
        // return List<List<object>> if succeed; null if fail
        public static List<List<object>> ReadTestData(string[] Texts, int Beg, int End)
        {            
            if (Texts.Length <= 0)
            {
                return null;
            }

            // determine that if datas are integers or float points
            bool IsInteger = true;
            for (int i = Beg; i < Texts.Length && i < End; i++)
            {
                if (Texts[i].Contains("."))
                {
                    IsInteger = false;
                    break;
                }
            }

            List<List<object>> Lists = new List<List<object>>();
            while (Beg < Texts.Length && Beg < End)
            {
                List<object> TempList = StringToNumerics(Texts[Beg], IsInteger);
                if (TempList != null)
                {
                    Lists.Add(TempList);                  
                }
                else
                {
                    System.Console.WriteLine("Failed to convert a string to numerics!");                   
                }
                Beg++;
            }// end of while

            return Lists;
        }

    }

    [DataContract]
    public class ExerciseProblem
    {
        static readonly string NameIdentifier = "name:";
        static readonly string DescriptionIdentifier = "description:";
        static readonly string CountsIdentifier = "counts:";
        static readonly string InputIdentifier = "input:";
        static readonly string OutputIdentifier = "output:";

        // factory construct method
        public static ExerciseProblem GetExerciseProblem(string FilePath)
        {
            // input from file
            string[] Texts = System.IO.File.ReadAllLines(FilePath);
            if (Texts.Length <= 0)
            {
                return null;
            }

            // locate data
            int NameIndex = ReadTextProblem.GetIdentifierIndex(Texts, NameIdentifier);
            int DescriptionIndex = ReadTextProblem.GetIdentifierIndex(Texts, DescriptionIdentifier);
            int CountsIndex = ReadTextProblem.GetIdentifierIndex(Texts, CountsIdentifier);
            int InputIndex = ReadTextProblem.GetIdentifierIndex(Texts, InputIdentifier);
            int OutputIndex = ReadTextProblem.GetIdentifierIndex(Texts, OutputIdentifier);
            if (NameIndex == -1 
                || DescriptionIndex == -1
                || CountsIndex == -1
                || InputIndex == -1
                || OutputIndex == -1)
            {
                return null;
            }

            // read input from postitions
            string Name = ReadTextProblem.ReadText(Texts, NameIndex, DescriptionIndex - 1);
            string Description = ReadTextProblem.ReadText(Texts, DescriptionIndex, CountsIndex - 1);
            int Counts = ReadTextProblem.ReadCounts(Texts, CountsIndex);
            List<List<object>> TestInputs = ReadTextProblem.ReadTestData(Texts, InputIndex, InputIndex + Counts);
            List<List<object>> TestOutputs = ReadTextProblem.ReadTestData(Texts, OutputIndex, OutputIndex + Counts);
            if (TestInputs == null 
                || TestOutputs == null 
                || Counts < 1)
            {
                return null;
            }

            // build an ExerciseProblem
            ExerciseProblem problem = new ExerciseProblem(Name, Description, TestInputs, TestOutputs);

            return problem;
        }

        // ctor
        // 必须拥有无参数的ctor才能序列化
        private ExerciseProblem() { }
        public ExerciseProblem(string pName, string pDescription, 
            List<List<object>> pTestInputs, List<List<object>> pTestOutputs)
        {
            try 
            { 
                Name = pName;
                Description = pDescription;    
            
                // note: cannot deep copy source objects if they are of reference types
                TestInputs.AddRange(pTestInputs);//const char* str = "..."; -> string str = "...";
                TestOutputs.AddRange(pTestOutputs);
            }
            catch (System.Exception expt)
            {
                System.Console.WriteLine(expt.Message);
            }
        }           

        // fields
        [DataMember]
        private string Name;
        [DataMember]
        private string Description;
        // ok
        [DataMember]
        public List<List<object>> TestInputs = new List<List<object>>();
        [DataMember]
        public List<List<object>> TestOutputs = new List<List<object>>();

        public string GetName()
        {
            return this.Name;
        }

        public string GetDes()
        {
            return this.Description;
        }

        public List<List<object>> GetIn()
        {
            return TestInputs;
        }

        public List<List<object>> GetOut()
        {
            return TestOutputs;
        }
    }
    
    public class TestClass
    {
        static void PrintListOfList(List<List<object>> Lists)
        {
            foreach (List<object> List in Lists)
            {
                foreach (object obj in List)
                {
                    System.Console.Write("{0} ", obj);
                }
                System.Console.WriteLine();
            }
        }

        static void PrintExerciseProblem(ExerciseProblem problem)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Name:\n{0}\n", problem.GetName());
            System.Console.WriteLine("Description:\n{0}\n", problem.GetDes());
            System.Console.WriteLine("Counts: {0}\n", problem.TestInputs.Count);
            System.Console.WriteLine("TestInput:");
            PrintListOfList(problem.TestInputs);
            System.Console.WriteLine("\nTestOutput:");
            PrintListOfList(problem.TestOutputs);
        }

        public static void Main(string[] args)
        {
            // arrange
            string FilePath = @"C:\Users\HASEE\source\repos\CsTest\CsTest\quicksort.txt";//测试前把这个地址改为实际的问题文档的地址
            ExerciseProblem problem = ExerciseProblem.GetExerciseProblem(FilePath);
            PrintExerciseProblem(problem);
            System.Console.ReadKey();
        }
    }
}