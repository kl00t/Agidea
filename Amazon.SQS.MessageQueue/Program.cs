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
            while (true)
            {
                Console.WriteLine("Enter command:");
                Console.WriteLine(Environment.NewLine +
                                  "(1) Create Queue " + Environment.NewLine +
                                  "(2) Get Queue Url " + Environment.NewLine +
                                  "(3) Send Message " + Environment.NewLine +
                                  "(4) Receive Messages " + Environment.NewLine +
                                  "(5) Delete Messages " + Environment.NewLine +
                                  "(6) Delete Queue " + Environment.NewLine +
                                  "(0) Exit " + Environment.NewLine);
                var input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    throw new ArgumentNullException();
                }

                switch (input.ToUpper())
                {
                    case "1":
                        //CreateQueue();
                        break;
                    case "2":
                        //GetQueueUrl();
                        break;
                    case "3":
                        //SendMessage();
                        break;
                    case "4":
                        //ReceiveMessages();
                        break;
                    case "5":
                        //DeleteMessages();
                        break;
                    case "6":
                        //DeleteQueue();
                        break;
                    case "0":
                        break;
                    default:
                        Console.WriteLine("Invalid menu selection.");
                        continue;
                }

                break;
            }
        }
    }
}
