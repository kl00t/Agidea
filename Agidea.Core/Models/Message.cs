using System;
using System.Collections.Generic;

namespace Agidea.Core.Models
{
    public class Message
    {
        public Guid Id { get; set; }

        public Dictionary<string, string> Attributes { get; set; }

        public string Body { get; set; }

        public string ReceiptHandle { get; set; }

        public MessageType MessageType { get; set; }
    }
}