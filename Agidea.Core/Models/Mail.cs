using System.Collections.Generic;

namespace Agidea.Core.Models
{
    public class Mail
    {
        public List<string> To { get; set; }

        public List<string> CC { get; set; }

        public List<string> BCC { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}