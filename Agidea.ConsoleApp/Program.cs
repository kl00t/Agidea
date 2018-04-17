using System;
using Agidea.Core.Interfaces;
using Ninject;

namespace Agidea.ConsoleApp
{
    public class Program
    {
        public static IMessageQueue _messageQueue;
        public static IMailer _mailer;
        public static IEmailRepository _emailRepository;
        
        public static void Main(string[] args)
        {
            Setup();
            Start();
        }

        private static void Setup()
        {
            IKernel kernel = new StandardKernel(new BindingModule());
            _messageQueue = kernel.Get<IMessageQueue>();
        }

        private static void Start()
        {
            while (true)
            {
                Console.WriteLine("Enter command:");
                Console.WriteLine(Environment.NewLine +
                                  "(1) Create Queue " + Environment.NewLine +
                                  "(2) Send Messages " + Environment.NewLine +
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
                        SendMessages();
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

        private static void SendMessages()
        {
            var queueUrl = GetQueueUrl();
            var mails = _emailRepository.GetEmails();
            if (_messageQueue.SendMessages(queueUrl, mails))
            {
                Console.WriteLine("Messages sent to queue url: " + queueUrl);
            }
        }

        private static void ReceiveMessages()
        {
            var queueUrl = GetQueueUrl();
            var messages = _messageQueue.ReceiveMessages(queueUrl);
            foreach (var message in messages)
            {
                Console.WriteLine("Message Id: " + message.Id);
            }
        }

        private static void DeleteMessages()
        {
            var queueUrl = GetQueueUrl();
            var messages = _messageQueue.ReceiveMessages(queueUrl);
            if (_messageQueue.DeleteMessages(queueUrl, messages))
            {
                Console.WriteLine("Messages Deleted from queue url: " + queueUrl);
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
