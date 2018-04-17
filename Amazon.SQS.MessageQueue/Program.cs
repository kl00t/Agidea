using System;
using Amazon.SQS.MessageQueue;
using Ninject;

namespace Amazon.ConsoleApp
{
    public class Program
    {
        public static IMessageQueue _messageQueue;
       

        public static void Main(string[] args)
        {
            Setup();
            Start();
        }

        private static void Setup()
        {
            IKernel kernel = new StandardKernel(new MessageQueueModule());
            _messageQueue = kernel.Get<IMessageQueue>();
        }

        private static void Start()
        {
            while (true)
            {
                Console.WriteLine("Enter command:");
                Console.WriteLine(Environment.NewLine +
                                  "(1) Create Queue " + Environment.NewLine +
                                  "(2) Send Message " + Environment.NewLine +
                                  "(3) Receive Messages " + Environment.NewLine +
                                  "(4) Delete Messages " + Environment.NewLine +
                                  "(5) Delete Queue " + Environment.NewLine +
                                  "(6) List Queues " + Environment.NewLine + 
                                  "(7) Get Queue " + Environment.NewLine);
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
                        SendMessage();
                        break;
                    case "3":
                        ReceiveMessages();
                        break;
                    case "4":
                        DeleteMessages();
                        break;
                    case "5":
                        DeleteQueue();
                        break;
                    case "6":
                        ListQueues();
                        break;
                    case "7":
                        GetQueue();
                        break;
                    default:
                        Console.WriteLine("Invalid menu selection.");
                        continue;
                }

                //Console.ReadLine();
                //break;
            }
        }

        private static void CreateQueue()
        {
            Console.WriteLine("Enter the queue name");
            var queueName = Console.ReadLine();
            var queueUrl = _messageQueue.CreateQueue(queueName);
            Console.WriteLine("Queue URL: " + queueUrl);
        }

        private static string GetQueueUrl()
        {
            Console.WriteLine("Enter the queue name");
            var queueName = Console.ReadLine();
            return _messageQueue.GetQueueUrl(queueName);
        }

        private static void GetQueue()
        {
            var queueUrl = GetQueueUrl();
            Console.WriteLine("Queue Url: " + queueUrl);
        }

        private static void SendMessage()
        {
            var queueUrl = GetQueueUrl();
            Console.WriteLine("Enter the message body");
            var messageBody = Console.ReadLine();
            var messageId = _messageQueue.SendMessage(queueUrl, messageBody);
            Console.WriteLine("Message Id: " + messageId);
        }

        private static void ReceiveMessages()
        {
            var queueUrl = GetQueueUrl();
            var messages = _messageQueue.ReceiveMessages(queueUrl);
            foreach (var message in messages)
            {
                Console.WriteLine("Message Id: " + message.Key);
            }
        }

        private static void DeleteMessages()
        {
            var queueUrl = GetQueueUrl();
            if (_messageQueue.DeleteMessages(queueUrl))
            {
                Console.WriteLine("Messages Deleted");
            }
        }

        private static void DeleteQueue()
        {
            var queueUrl = GetQueueUrl();
            if (_messageQueue.DeleteQueue(queueUrl))
            {
                Console.WriteLine("Message Queue Deleted");
            }
        }

        private static void ListQueues()
        {
            var queues = _messageQueue.ListQueues();
            foreach (var queue in queues)
            {
                Console.WriteLine("Queue: " + queue);
            }
        }
    }
}
