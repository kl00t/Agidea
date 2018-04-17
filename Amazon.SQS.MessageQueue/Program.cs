using System;
using System.Configuration;
using Ninject;

namespace Amazon.SQS.MessageQueue
{
    public class Program
    {
        public static IMessageQueue _messageQueue;
        private static string QueueOwnerAccountId = ConfigurationManager.AppSettings["QueueOwnerAccountId"];
        private static string _queueName;
        private static string _queueUrl;

        public static void Main(string[] args)
        {
            Setup();
            Start();
        }

        private static void Setup()
        {
            IKernel kernel = new StandardKernel();
            _messageQueue = kernel.Get<IMessageQueue>();
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
                        CreateQueue();
                        break;
                    case "2":
                        GetQueueUrl();
                        break;
                    case "3":
                        SendMessage();
                        break;
                    case "4":
                        ReceiveMessages();
                        break;
                    case "5":
                        DeleteMessages();
                        break;
                    case "6":
                        DeleteQueue();
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

        private static void CreateQueue()
        {
            Console.WriteLine("Enter the queue name");
            _queueName = Console.ReadLine();
            _queueUrl = _messageQueue.CreateQueue(_queueName);
            Console.WriteLine("Queue URL: " + _queueUrl);
        }

        private static void GetQueueUrl()
        {
            _queueUrl = _messageQueue.GetQueueUrl(_queueName, QueueOwnerAccountId);
            Console.WriteLine("Queue URL: " + _queueUrl);
        }

        private static void SendMessage()
        {
            Console.WriteLine("Enter the message body");
            var messageBody = Console.ReadLine();
            var messageId = _messageQueue.SendMessage(_queueUrl, messageBody);
            Console.WriteLine("Message Id: " + messageId);
        }

        private static void ReceiveMessages()
        {
            var messages = _messageQueue.ReceiveMessages(_queueUrl);
            foreach (var message in messages)
            {
                Console.WriteLine("Message Id: " + message.Key);
            }
        }

        private static void DeleteMessages()
        {
            if (_messageQueue.DeleteMessages(_queueUrl))
            {
                Console.WriteLine("Messages Deleted");
            }
        }

        private static void DeleteQueue()
        {
            if (_messageQueue.DeleteQueue(_queueUrl))
            {
                Console.WriteLine("Message Queue Deleted");
            }
        }
    }
}
