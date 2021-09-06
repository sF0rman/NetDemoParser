using System;
using System.Linq;

namespace DemoParserNET
{
    class Program
    {
        static bool bShowGameEvents = true;
        static bool bIncludeWarmup = false;
        private static string filePath;

        static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                Console.WriteLine("DemoParserNET\n");
                Console.WriteLine("Required Arguments:\n" +
                                  "<path/filename>.dem\n");

                Console.WriteLine("Optional Arguments:\n" +
                                  "-gameevents        Dump out game events\n" +
                                  "-includewarmup          Skip deaths during warmup");

                return;
            }

            if (args.Length >= 1)
            {
                foreach (Object arg in args)
                {
                    if (arg.Equals("-gameevents"))
                    {
                        Console.WriteLine("Showing Game Events");
                        bShowGameEvents = true;
                    }
                    else if (arg.Equals("-includewarmup"))
                    {
                        Console.WriteLine("Including Warmup");
                        bIncludeWarmup = true;
                    }
                    else if (arg.ToString().EndsWith(".dem"))
                    {
                        Console.WriteLine("File: " + arg);
                        filePath = arg.ToString();
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Invalid argument: " + arg);
                        return;
                    }
                }
            }

            DemoReader.parseDemo(filePath);
        }
    }
}