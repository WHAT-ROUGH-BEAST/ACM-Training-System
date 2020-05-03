using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Configuration;

namespace GccCompiler
{
    public class GccCompiler
    {
        // 保持一次操作的一致
        private static Dictionary<string, string> Settings = null;

        static GccCompiler()
        {
            GetConfig();
        }

        public void UpdateSettings()
        {
            GetConfig();
        }

        public static void Main(string[] args)
        {
            string Add = GccCompiler.Compile("C:\\Users\\18069\\Desktop\\test\\test.c");
            Console.WriteLine(Add);
        }

        // 返回
        public static string Compile(string Addr)
        {
            if (null == Addr)
                throw new Exception();

            // 验证地址合法性 : 读一遍
            try
            {
                Stream Reader = new FileStream(Addr, FileMode.Open);
                Reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }

            // 解析地址
            string[] Addrs = Addr.Split('\\');
            string exeName = "test.exe";
            StringBuilder StrBuilder = new StringBuilder();
            for (int i = 0; i < Addrs.Length - 1; i ++)
            {
                StrBuilder.Append(Addrs[i]);
                StrBuilder.Append("\\");
            }
            StrBuilder.Append(exeName); // 生成的名字统一命名为test.exe

            // 执行
            try
            {
                ExeCmd(" -o " + StrBuilder.ToString() + " " + Addr);
                return StrBuilder.ToString();
            }
            catch (CompileErrException e)
            {
                // 编译失败，结束程序，直接向上报错，停止后续执行
                Console.WriteLine(e.Message);
                throw e;
            }
            catch (Exception e)
            {
                // 方便定位错误
                throw e;
            }
        }

        private static void ExeCmd(string Cmd) // throws CompileErrException
        {
            string CompileInfo = null;
            Process GccProcess = null;

            // StreamReader NormOutput = null;
            StreamReader ErrOutput = null;

            try
            {
                GccProcess = new Process();
                GccProcess.StartInfo.FileName = Settings["Compiler"]; // Windows控制台，大家的位置应该都一样
                GccProcess.StartInfo.Arguments = Cmd; // 加"/c "，在执行Cmd后立即返回
                GccProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                GccProcess.StartInfo.RedirectStandardOutput = true;
                GccProcess.StartInfo.RedirectStandardError = true;
                GccProcess.StartInfo.UseShellExecute = false;
                GccProcess.StartInfo.CreateNoWindow = true;
                GccProcess.Start();

                // NormOutput = GccProcess.StandardOutput;
                ErrOutput = GccProcess.StandardError;

                GccProcess.WaitForExit();

                // 若出错，把gcc的报错信息作为Exception向上传
                CompileInfo = ErrOutput.ReadToEnd();
                if ("" != CompileInfo && null != CompileInfo)
                {
                    // 没装编译器
                    if (CompileInfo.StartsWith("'" + Settings["Compiler"] + "'"))
                        throw new CompileErrException("你没有安装" + Settings["Compiler"] + "或者环境变量有误");
                    else
                    {
                        // gcc 内部错误
                        throw new CompileErrException(
                            "Compilation error : \n" + CompileInfo);
                    }
                }

                // 正常输出
                // CompileInfo.Append(NormOutput.ReadToEnd());

                // 关闭进程
                GccProcess.Close();
            }
            catch (CompileErrException e)
            {
                // 向上传出错信息
                throw e;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        private static void GetConfig()
        {
            string CompilerPath = ConfigurationManager.AppSettings["Compiler"];
            string ShellPath = ConfigurationManager.AppSettings["Shell"];

            Settings = new Dictionary<string, string>();
            Settings.Add("Compiler", CompilerPath);
        }
    }

    class CompileErrException : Exception
    {
        public CompileErrException()
        { }
        public CompileErrException(string message) : base(message)
        { }
    }
}