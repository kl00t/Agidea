using System;
using System.Collections.Generic;
using Agidea.Core.Interfaces;
using Agidea.Core.Models;
using Ninject;

namespace Agidea.ConsoleApp
{
    public class Program
    {
        public static IMessageQueue _messageQueue;
        public static IEmailRepository _emailRepository;
        public static IFileStorageProvider _bucket;
        public static void Main(string[] args)
        {
            Setup();
            Start();
        }

        private static void Setup()
        {
            IKernel kernel = new StandardKernel(new BindingModule());
            _messageQueue = kernel.Get<IMessageQueue>();
            _emailRepository = kernel.Get<IEmailRepository>();
            _bucket = kernel.Get<IFileStorageProvider>();
        }

        private static void Start()
        {
            while (true)
            {
                Console.WriteLine("Enter command:");
                Console.WriteLine(Environment.NewLine +
                                  "(1) Send Test Messages To Queue" + Environment.NewLine +
                                  "(2) Receive Messages From Queue" + Environment.NewLine +
                                  "(3) Delete Messages From Queue" + Environment.NewLine +
                                  "(4) List Files In Bucket" + Environment.NewLine +
                                  "(5) Get File From Bucket" + Environment.NewLine +
                                  "(6) Get File Url" + Environment.NewLine);

                var input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    throw new ArgumentNullException();
                }

                switch (input.ToUpper())
                {
                    case "1":
                        SendMessages();
                        break;
                    case "2":
                        ReceiveMessages();
                        break;
                    case "3":
                        DeleteMessages();
                        break;
                    case "4":
                        ListFiles();
                        break;
                    case "5":
                        GetFile();
                        break;
                    case "6":
                        GetFileUrl();
                        break;
                    default:
                        Console.WriteLine("Invalid menu selection.");
                        continue;
                }

            }
        }

        private static void GetFile()
        {
            _bucket.GetFile("Monthly Footfall Report-Airport Retail Park  Coventry-September 2013.pdf");
        }

        private static void GetFileUrl()
        {
            var fileUrl = _bucket.GetFileUrl("Monthly Footfall Report-Airport Retail Park  Coventry-September 2013.pdf");
            Console.WriteLine("FileUrl: {0}", fileUrl);
        }

        private static void ListFiles()
        {
            _bucket.ListFiles();
        }

        private static void SendMessages()
        {
            var mails = _emailRepository.GetEmails();

            var messages = new List<Message>();

            foreach (var mail in mails)
            {
                messages.Add(new Message
                {
                    Id = Guid.NewGuid(),
                    Body = Core.Helper.Converter.ConvertToJson(mail),
                    MessageType = MessageType.Email,
                    Attributes = new Dictionary<string, string>
                    {
                        {
                            typeof(MessageType).ToString() , 
                            MessageType.Email.ToString()
                        }
                    }
                });
            }

            if (_messageQueue.SendMessages(messages))
            {
                Console.WriteLine("Messages sent to queue");
            }
        }

        private static void ReceiveMessages()
        {
            var messages = _messageQueue.ReceiveMessages();
            foreach (var message in messages)
            {
                Console.WriteLine("Message Id: " + message.Id);
            }
        }

        private static void DeleteMessages()
        {
            var messages = _messageQueue.ReceiveMessages();
            if (_messageQueue.DeleteMessages(messages))
            {
                Console.WriteLine("Messages Deleted from queue");
            }
        }
    }
}