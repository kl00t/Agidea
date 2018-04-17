using System.Collections.Generic;

namespace Agidea.Core.Models
{
    public class Message
    {
        public string Id { get; set; }

        public Dictionary<string, string> Attributes { get; set; }

        public string Body { get; set; }

        public string ReceiptHandle { get; set; }
    }
}