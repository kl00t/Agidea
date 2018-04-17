using System;
using Ninject;

namespace Amazon.SQS.MessageQueue
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Setup();
            Start();
        }

        private static void Setup()
        {
            IKernel kernel = new StandardKernel();
        }

        private static void Start()
        {
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
